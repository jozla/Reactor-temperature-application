using PZ2.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MVVMLight.Messaging;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace PZ2.ViewModel
{
    public class MainWindowViewModel : BindableBase, IClosing
    {
        public MyICommand<string> NavCommand { get; private set; }
        private bool isHelpVisible = true;
        public MyICommand ToggleHelpCommand { get; private set; }
        public static MyICommand UndoCommand { get; set; }
        public static ICommand ToggleUndo { get; set; }
        private readonly NetworkEntitiesViewModel networkEntitiesViewModel = new NetworkEntitiesViewModel();
        private readonly NetworkDisplayViewModel networkDisplayViewModel = new NetworkDisplayViewModel();
        private readonly MeasurementGraphViewModel measurementGraphViewModel = new MeasurementGraphViewModel();
        private BindableBase currentViewModel;

        public bool IsHelpVisible
        {
            get { return isHelpVisible; }
            set
            {
                if (isHelpVisible != value)
                {
                    isHelpVisible = value;
                    OnPropertyChanged("IsHelpVisible");
                }
            }
        }

        public MainWindowViewModel()
        {
            this.NavCommand = new MyICommand<string>(this.OnNav);
            this.ToggleHelpCommand = new MyICommand(this.ToggleHelp);
            UndoCommand = new MyICommand(Undo);
            this.CurrentViewModel = this.networkEntitiesViewModel;
            createListener();
        }

        private void Undo()
        {
            if (ToggleUndo != null)
            {
                ToggleUndo.Execute(null);
                ToggleUndo = null;
            }
        }

        private void ToggleHelp()
        {
            IsHelpVisible = !IsHelpVisible;
            Visibility visible;
            if(IsHelpVisible)
                visible = Visibility.Visible;
            else
                visible = Visibility.Hidden;
            Messenger.Default.Send<Visibility>(visible);
            OnPropertyChanged("IsHelpVisible");
        }

        public BindableBase CurrentViewModel
        {
            get { return this.currentViewModel; }
            set
            {
                this.SetProperty(ref this.currentViewModel, value);
            }
        }

        private BindableBase previousViewModel;
        private void OnNav(string destination)
        {
            switch (destination)
            {
                case "networkEntities":
                    previousViewModel = CurrentViewModel;
                    CurrentViewModel = networkEntitiesViewModel;
                    break;

                case "networkDisplay":
                    previousViewModel = CurrentViewModel;
                    CurrentViewModel = networkDisplayViewModel;
                    break;

                case "measurementGraph":
                    previousViewModel = CurrentViewModel;
                    CurrentViewModel = measurementGraphViewModel;
                    break;
            }
            ToggleUndo = new MyICommand(NavUndo);
        }

        private void NavUndo()
        {
            CurrentViewModel = previousViewModel;
        }

        public bool OnClosing()
        {
            return true;
        }

        private void createListener()
        {
            var tcp = new TcpListener(IPAddress.Any, 25565);
            tcp.Start();

            var listeningThread = new Thread(() =>
            {
                while (true)
                {
                    int count = Database.Reactors.Count();
                    var tcpClient = tcp.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(param =>
                    {
                        //Prijem poruke
                        NetworkStream stream = tcpClient.GetStream();
                        string incomming;
                        byte[] bytes = new byte[1024];
                        int i = stream.Read(bytes, 0, bytes.Length);
                        //Primljena poruka je sacuvana u incomming stringu
                        incomming = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                        //Ukoliko je primljena poruka pitanje koliko objekata ima u sistemu -> odgovor
                        if (incomming.Equals("Need object count"))
                        {
                            //Response
                            /* Umesto sto se ovde salje count.ToString(), potrebno je poslati 
                             * duzinu liste koja sadrzi sve objekte pod monitoringom, odnosno
                             * njihov ukupan broj (NE BROJATI OD NULE, VEC POSLATI UKUPAN BROJ)
                             * */
                            Byte[] data = System.Text.Encoding.ASCII.GetBytes(count.ToString());
                            stream.Write(data, 0, data.Length);
                        }
                        else
                        {
                            //U suprotnom, server je poslao promenu stanja nekog objekta u sistemu
                            Console.WriteLine(incomming); //Na primer: "Entitet_1:272"

                            //################ IMPLEMENTACIJA ####################
                            // Obraditi poruku kako bi se dobile informacije o izmeni
                            // Azuriranje potrebnih stvari u aplikaciji
                            var match = Regex.Match(incomming, @"Entitet_(\d+):([0-9.]+)");
                            if (int.TryParse(match.Groups[1].Value, out int objectIndex))
                            {
                                if (objectIndex < 0 || objectIndex >= Database.ReactorIds.Count)
                                {
                                    Trace.TraceError($"Value \"{incomming}\" from server is out of bounds!");
                                }
                                else if (double.TryParse(match.Groups[2].Value, out double newVal))
                                {
                                    int id = Database.ReactorIds[objectIndex];
                                    Database.Reactors[id].Temperature = newVal;
                                    Database.Reactors[id].ValidateTemperature();
                                    string logStr = Log.ConvertToLogFormat(id, newVal);
                                    Log.Add(logStr);
                                }
                                else
                                {
                                    Trace.TraceError($"Could not parse \"{match.Groups[2].Value}\" to integer.");
                                }
                            }
                            else
                            {
                                Trace.TraceError($"Could not parse \"{match.Groups[1].Value}\" to integer.");
                            }
                        }
                    }, null);
                }
            });

            listeningThread.IsBackground = true;
            listeningThread.Start();
        }
    }
}
