using MVVMLight.Messaging;
using PZ2.Helpers;
using PZ2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace PZ2.ViewModel
{
    public class NetworkEntitiesViewModel : BindableBase
    {
        public BindingList<ReactorModel> ReactorList { get; private set; } = new BindingList<ReactorModel>();
        public MyICommand AddReactorCommand { get; set; }
        public MyICommand DeleteReactorCommand { get; set; }
        public MyICommand FilterCommand { get; set; }
        public MyICommand ResetFilterCommand { get; set; }

        private readonly CollectionView reactorTypes;
        public CollectionView ReactorTypes { get => reactorTypes; }

        private ReactorModel currentReactor = new ReactorModel();
        private string addType = string.Empty;
        private string filterType = "Type";
        private int filterId = 0;

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

        private bool[] filterMode = new bool[] { false, false, false };
        public bool[] FilterMode
        {
            get => filterMode;
            set
            {
                if (value != filterMode)
                {
                    filterMode = value;
                    OnPropertyChanged("FilterMode");
                }
            }
        }

        public int FilterSelectedMode
        {
            get { return Array.IndexOf(FilterMode, true); }

        }

        public string AddType
        {
            get => addType;
            set
            {
                if (addType != value && Database.ReactorTypes.ContainsKey(value))
                {
                    addType = value;
                    CurrentReactor.Type = Database.ReactorTypes[value];
                    OnPropertyChanged("AddType");
                }
            }
        }

        public string FilterType
        {
            get => filterType;
            set
            {
                if (filterType != value)
                {
                    filterType = value;
                    OnPropertyChanged("FilterType");
                }
            }
        }

        public int FilterId
        {
            get => filterId;
            set
            {
                if (value != filterId)
                {
                    filterId = value;
                    OnPropertyChanged("FilterId");
                }
            }
        }

        public ReactorModel SelectedReactor { get; set; }

        public ReactorModel CurrentReactor
        {
            get => currentReactor;
            set
            {
                currentReactor = value;
                OnPropertyChanged("CurrentReactor");
            }
        }

        public NetworkEntitiesViewModel()
        {
            Messenger.Default.Register<Visibility>(this, ToggleHelp);
            ResetFilter();
            AddReactorCommand = new MyICommand(OnAdd);
            DeleteReactorCommand = new MyICommand(OnDelete);
            FilterCommand = new MyICommand(OnFilter);
            ResetFilterCommand = new MyICommand(ResetFilter);
            reactorTypes = new CollectionView(Database.ReactorTypes.Values.ToList());
        }

        private ReactorModel lastAdded;
        private void OnAdd()
        {
            CurrentReactor.Validate();
            if (CurrentReactor.IsValid && Database.Add(ReactorModel.Copy(CurrentReactor)))
            {
                lastAdded = ReactorModel.Copy(CurrentReactor);
                ResetFilter();
                MainWindowViewModel.ToggleUndo = new MyICommand(AddUndo);
            }
        }

        private void AddUndo()
        {
            int id = lastAdded.Id;
            if (Database.Remove(id))
            {
                ResetFilter();
            }
        }

        private ReactorModel lastDeleted;
        private void OnDelete()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                int id = SelectedReactor.Id;
                lastDeleted = Database.Reactors[id];
                if (Database.Remove(id))
                    ResetFilter();
            }
            MainWindowViewModel.ToggleUndo = new MyICommand(DeleteUndo);
        }

        private void DeleteUndo()
        {
            Database.Add(lastDeleted);
            ReactorList.Add(lastDeleted);
        }

        private void OnFilter()
        {
            ReactorList.Clear();
            foreach (var pair in Database.Reactors)
            {
                int id = pair.Key;
                string typeName = pair.Value.Type.Name;

                if (filterType != "Type")
                    if (typeName != FilterType)
                        continue;

                switch (FilterSelectedMode)
                {
                    case 0:
                        if (id < FilterId)
                        {
                            ReactorList.Add(Database.Reactors[id]);
                        }
                        break;

                    case 1:
                        if (id == FilterId)
                        {
                            ReactorList.Add(Database.Reactors[id]);
                        }
                        break;

                    case 2:
                        if (id > FilterId)
                        {
                            ReactorList.Add(Database.Reactors[id]);
                        }
                        break;

                    default:
                        ReactorList.Add(Database.Reactors[id]);
                        break;
                }
            }
            MainWindowViewModel.ToggleUndo = new MyICommand(FilterUndo);
        }

        private void FilterUndo()
        {
            ResetFilter();
        }

        private void ResetFilter()
        {
            ReactorList.Clear();
            foreach (var reactor in Database.Reactors.Values)
            {
                ReactorList.Add(reactor);
            }
            for (int i = 0; i < FilterMode.Count(); i++)
            {
                FilterMode[i] = false;
            }
            filterType = "Type";
            filterId = 0;
            OnPropertyChanged("FilterType");
            OnPropertyChanged("FilterMode");
            OnPropertyChanged("FilterId");
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
