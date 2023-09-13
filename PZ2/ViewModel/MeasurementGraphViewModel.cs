using MVVMLight.Messaging;
using PZ2.Helpers;
using PZ2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PZ2.ViewModel
{
    public class MeasurementGraphViewModel : BindableBase
    {
        private List<ReactorModel> availableReactors;
        private ReactorModel selectedReactor;
        public MyICommand<Canvas> ShowCommand { get; set; }
        public MyICommand ToggleHelpCommand { get; private set; }
        private Visibility isHelpVisible = Visibility.Visible;
        private bool isToolTipVisible = true;
        public Visibility IsHelpVisible
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
        public bool IsToolTipVisible
        {
            get { return isToolTipVisible; }
            set
            {
                if (isToolTipVisible != value)
                {
                    isToolTipVisible = value;
                    OnPropertyChanged("IsToolTipVisible");
                }
            }
        }

        public List<ReactorModel> AvailableReactors
        {
            get { return availableReactors; }
            set { this.availableReactors = value; }
        }

        public ReactorModel SelectedReactor
        {
            get { return selectedReactor; }
            set
            {
                selectedReactor = value;
                OnPropertyChanged("SelectedReactor");
            }
        }

        public MeasurementGraphViewModel()
        {
            Messenger.Default.Register<Visibility>(this, ToggleHelp);
            availableReactors = Database.Reactors.Values.ToList();
            this.ShowCommand = new MyICommand<Canvas>(this.OnShow);
        }

        private Canvas graphShowedCanvas;
        private void OnShow(Canvas graphCanvas)
        {
            if (SelectedReactor != null && Log.ReadLast5Changes(selectedReactor.Id).Count == 5)
            {
                graphCanvas.Children.Clear();
                double canvasWidth = graphCanvas.ActualWidth;
                double canvasHeight = graphCanvas.ActualHeight;

                ReactorModel reactor = SelectedReactor;

                List<double> measurements = Log.ReadLast5Changes(selectedReactor.Id);
   
                double maxDiameter = 100;

                Line verticalLine4 = new Line();
                verticalLine4.X1 = canvasWidth / 2 + 140;
                verticalLine4.Y1 = 0;
                verticalLine4.X2 = canvasWidth / 2 + 140;
                verticalLine4.Y2 = canvasHeight;
                verticalLine4.Stroke = Brushes.Gray;
                graphCanvas.Children.Add(verticalLine4);

                Line verticalLine5 = new Line();
                verticalLine5.X1 = canvasWidth / 2 + 260;
                verticalLine5.Y1 = 0;
                verticalLine5.X2 = canvasWidth / 2 + 260;
                verticalLine5.Y2 = canvasHeight;
                verticalLine5.Stroke = Brushes.Gray;
                graphCanvas.Children.Add(verticalLine5);

                Line verticalLine = new Line();
                verticalLine.X1 = canvasWidth / 2 + 20;
                verticalLine.Y1 = 0;
                verticalLine.X2 = canvasWidth / 2 + 20;
                verticalLine.Y2 = canvasHeight;
                verticalLine.Stroke = Brushes.Gray;
                graphCanvas.Children.Add(verticalLine);

                Line verticalLine2 = new Line();
                verticalLine2.X1 = 80;
                verticalLine2.Y1 = 0;
                verticalLine2.X2 = 80;
                verticalLine2.Y2 = canvasHeight;
                verticalLine2.Stroke = Brushes.Gray;
                graphCanvas.Children.Add(verticalLine2);

                Line verticalLine3 = new Line();
                verticalLine3.X1 = 200;
                verticalLine3.Y1 = 0;
                verticalLine3.X2 = 200;
                verticalLine3.Y2 = canvasHeight;
                verticalLine3.Stroke = Brushes.Gray;
                graphCanvas.Children.Add(verticalLine3);

                Line verticalLineFirst = new Line();
                verticalLineFirst.X1 = 0;
                verticalLineFirst.Y1 = 0;
                verticalLineFirst.X2 = 0;
                verticalLineFirst.Y2 = canvasHeight;
                verticalLineFirst.Stroke = Brushes.Black;
                graphCanvas.Children.Add(verticalLineFirst);

                Line horizontalLine = new Line();
                horizontalLine.X1 = 0;
                horizontalLine.Y1 = canvasHeight / 2;
                horizontalLine.X2 = canvasWidth;
                horizontalLine.Y2 = canvasHeight / 2;
                horizontalLine.Stroke = Brushes.Gray;
                graphCanvas.Children.Add(horizontalLine);

                Line horizontalLine2 = new Line();
                horizontalLine2.X1 = 0;
                horizontalLine2.Y1 = canvasHeight;
                horizontalLine2.X2 = canvasWidth;
                horizontalLine2.Y2 = canvasHeight;
                horizontalLine2.Stroke = Brushes.Black;
                graphCanvas.Children.Add(horizontalLine2);

                // Broj elipsi
                int ellipseCount = measurements.Count;

                // Razmak između elipsi
                double spacing = 20;

                // Ukupna širina svih elipsi i razmaka
                double totalWidth = (ellipseCount * maxDiameter) + ((ellipseCount - 1) * spacing);

                // Početna x-koordinata za centriranje
                double startX = (canvasWidth - totalWidth) / 2 + spacing; // Dodajemo spacing da bismo pomerili prvu elipsu od ivice

                // Y-koordinata elipse
                double y = canvasHeight / 2;

                for (int i = 0; i < ellipseCount; i++)
                {
                    double temperature = measurements[i];

                    // Računanje prečnika elipse na osnovu temperature
                    double diameter = (temperature / reactor.maxTemperature) * maxDiameter;

                    // Centriranje elipse na horizontalnoj osi
                    double x = startX + (maxDiameter / 2) + (i * (maxDiameter + spacing));

                    // Kreiranje elipse
                    Ellipse ellipse = new Ellipse();
                    ellipse.Width = diameter;
                    ellipse.Height = diameter;
                    if(temperature < 250 || temperature > 350)
                        ellipse.Fill = new SolidColorBrush(Colors.Red);
                    else
                        ellipse.Fill = new SolidColorBrush(Colors.Cyan);

                    // Pozicioniranje elipse na canvasu
                    Canvas.SetLeft(ellipse, x - (diameter / 2));
                    Canvas.SetTop(ellipse, y - (diameter / 2));

                    // Dodavanje elipse na canvas
                    graphCanvas.Children.Add(ellipse);
                    graphShowedCanvas = graphCanvas;
                }
            }
            MainWindowViewModel.ToggleUndo = new MyICommand(ShowUndo);
        }

        private void ShowUndo()
        {
            graphShowedCanvas.Children.Clear();
        }

        private void ToggleHelp(Visibility visibility)
        {
            IsHelpVisible = visibility;
            IsToolTipVisible = !IsToolTipVisible;
            OnPropertyChanged("IsHelpVisible");
            OnPropertyChanged("IsToolTipVisible");
        }
    }
}
