using PZ2.Helpers;
using PZ2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.IO;
using System.Windows.Data;
using System.Windows.Shapes;
using MVVMLight.Messaging;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using System.Windows.Threading;

namespace PZ2.ViewModel
{
    public class NetworkDisplayViewModel : BindableBase
    {
        private ReactorModel draggedItem = null;

        public ReactorModel DraggedItem
        {
            get { return draggedItem; }
            set
            {
                if (draggedItem != value)
                {
                    draggedItem = value;
                    OnPropertyChanged("DraggedItem");
                }
            }
        }

        private bool dragging = false;
        private int selected;

        public BindingList<ReactorsByType> AllReactors { get; set; }

        public MyICommand<Canvas> ItemDroppedCommand { get; set; }
        public MyICommand<Canvas> ClearItemCommand { get; set; }
        public MyICommand MouseLeftButtonUpCommand { get; set; }
        public MyICommand<TreeView> SelectedItemChangedCommand { get; set; }
        public MyICommand<DragEventArgs> CanvasDragOverCommand { get; set; }
        public MyICommand<Canvas> DrawLineCommand { get; set; }
        public MyICommand<Canvas> DragCommand { get; set; }

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

        public NetworkDisplayViewModel()
        {
            Messenger.Default.Register<Visibility>(this, ToggleHelp);
            AllReactors = new BindingList<ReactorsByType>();
            ReactorsByType rtdReactors = new ReactorsByType() { Type = "RTD" };
            ReactorsByType thermoReactors = new ReactorsByType() { Type = "Thermopile" };
            foreach (var r in Database.Reactors.Values)
            {
                if (r.Type.Name.Equals("RTD"))
                    rtdReactors.Reactors.Add(r);
                else
                    thermoReactors.Reactors.Add(r);
            }
            AllReactors.Add(rtdReactors);
            AllReactors.Add(thermoReactors);

            ItemDroppedCommand = new MyICommand<Canvas>(ItemDropped);
            ClearItemCommand = new MyICommand<Canvas>(ClearItem);
            MouseLeftButtonUpCommand = new MyICommand(MouseLeftButtonUp);
            SelectedItemChangedCommand = new MyICommand<TreeView>(SelectedItemChanged);
            CanvasDragOverCommand = new MyICommand<DragEventArgs>(CanvasDragOver);
            DrawLineCommand = new MyICommand<Canvas>(OnRightClick);
            DragCommand = new MyICommand<Canvas>(OnDrag);
        }

        public static Point center1;
        public static List<int> linije = new List<int>();
        private Line lastLine;
        private Canvas lastCanvasWithLine;
        private void OnRightClick(Canvas canvas)
        {
            linije.Add(1);
            if (linije.Count == 1)
            {
                var lineCanvas = canvas.FindAncestor<Canvas>();
                center1 = canvas.TransformToAncestor(lineCanvas).Transform(new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2));
            }
            else if(linije.Count == 2)
            {
                var lineCanvas = canvas.FindAncestor<Canvas>();
                Point center2 = canvas.TransformToAncestor(lineCanvas).Transform(new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2));

                Random random = new Random();
                byte[] rgb = new byte[3];
                random.NextBytes(rgb);

                Color randomColor = Color.FromRgb(rgb[0], rgb[1], rgb[2]);

                Line line = new Line
                {
                    X1 = center1.X,
                    Y1 = center1.Y,
                    X2 = center2.X,
                    Y2 = center2.Y,
                    Stroke = new SolidColorBrush(randomColor),
                    StrokeThickness = 1
                };
                lastLine = line;
                Panel.SetZIndex(line, int.MaxValue);
                lineCanvas.Children.Add(line);
                lastCanvasWithLine = lineCanvas;
                linije.Clear();
            }   
            MainWindowViewModel.ToggleUndo = new MyICommand(OnRightClickUndo);
        }

        private void OnRightClickUndo()
        {
            List<Line> linesToRemove = lastCanvasWithLine
                                .Children
                                .OfType<Line>()
                                .Where(line =>
                                    (line.X1 == lastLine.X1 && line.Y1 == lastLine.Y1) &&
                                    (line.X2 == lastLine.X2 && line.Y2 == lastLine.Y2))
                                .ToList();
            lastCanvasWithLine.Children.Remove(linesToRemove[0]);
        }

        private void CanvasDragOver(DragEventArgs e)
        {
            Canvas kanvas = e.Source as Canvas;
            if (kanvas!= null && kanvas.Resources["taken"] != null)
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                e.Effects = DragDropEffects.Copy;
            }
            e.Handled = true;
        }

        private void MouseLeftButtonUp()
        {
            dragging = false;
            draggedItem = null;
        }
        private void SelectedItemChanged(TreeView tv)
        {
            if (!dragging && tv.SelectedItem.GetType() != typeof(ReactorsByType))
            {
                dragging = true;
                draggedItem = (ReactorModel)tv.SelectedItem;
                selected = pronadjiElement(draggedItem);
                DragDrop.DoDragDrop(tv, draggedItem, DragDropEffects.Move | DragDropEffects.Copy);
            }
        }

        public static List<Line> linesOfDragged;
        public static List<Line> linesOfDragged2;

        private void OnDrag(Canvas canvas)
        {
            string id = ((TextBlock)canvas.Children[0]).Text.Split(':')[1].Trim();
            int Id = int.Parse(((TextBlock)canvas.Children[0]).Text.Split(':')[1].Trim());
            if (!dragging)
            {
                dragging = true;
                draggedItem = Database.Reactors[Id];

                var lineCanvas = canvas.FindAncestor<Canvas>();
                Point center = canvas.TransformToAncestor(lineCanvas).Transform(new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2));
                linesOfDragged = lineCanvas
                                .Children
                                .OfType<Line>()
                                .Where(line =>
                                    (line.X1 == center.X && line.Y1 == center.Y))
                                .ToList();

                linesOfDragged2 = lineCanvas
                               .Children
                               .OfType<Line>()
                               .Where(line =>
                                   (line.X2 == center.X && line.Y2 == center.Y))
                               .ToList();

                ClearItem(canvas);
                DragDrop.DoDragDrop(canvas, draggedItem, DragDropEffects.Move | DragDropEffects.Copy);
            }
        }

        private Canvas newItemPlaceCanvas;
        private void ItemDropped(Canvas canvas)
        {
            if (draggedItem != null)
            {
                if (canvas.Resources["taken"] == null)
                {
                    newItemPlaceCanvas = canvas;
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri(("pack://application:,,," + draggedItem.Type.Image), UriKind.Absolute);
                    img.EndInit();
                    canvas.Background = new ImageBrush(img);
                    ((TextBlock)canvas.Children[0]).Text = "Id: " + draggedItem.Id.ToString();
                    ((TextBlock)canvas.Children[1]).Text = draggedItem.Name;

                    var temperatureTextBlock = (TextBlock)canvas.Children[2];
                    temperatureTextBlock.SetBinding(TextBlock.TextProperty, new Binding("Temperature") { Source = Database.Reactors[draggedItem.Id], StringFormat = "T: {0}" });
                    
                    //((TextBlock)canvas.Children[3]).Text = "ERROR";
                    //((TextBlock)canvas.Children[3]).SetBinding(TextBlock.ForegroundProperty, new Binding("Color") { Source = Database.Reactors[draggedItem.Id] });

                    canvas.Resources.Add("taken", true);
                    ukloniElement(draggedItem);

                    if (linesOfDragged != null && linesOfDragged.Count > 0)
                    {
                        var lineCanvas = canvas.FindAncestor<Canvas>();
                        Point center = canvas.TransformToAncestor(lineCanvas).Transform(new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2));
                    
                        foreach(var l in linesOfDragged)
                        {
                            l.X1 = center.X;
                            l.Y1 = center.Y;
                            lineCanvas.Children.Add(l);
                        }
                        linesOfDragged.Clear();
                    }

                    if (linesOfDragged2 != null && linesOfDragged2.Count > 0)
                    {
                        var lineCanvas = canvas.FindAncestor<Canvas>();
                        Point center = canvas.TransformToAncestor(lineCanvas).Transform(new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2));

                        foreach (var l in linesOfDragged2)
                        {
                            l.X2 = center.X;
                            l.Y2 = center.Y;
                            lineCanvas.Children.Add(l);
                        }
                        linesOfDragged2.Clear();
                    }
                }
                draggedItem = null;
                dragging = false;
            }
            MainWindowViewModel.ToggleUndo = new MyICommand(DropUndo);
        }

        private void DropUndo()
        {
            ClearItem(newItemPlaceCanvas);
        }

        private Canvas clearedCanvas;
        private string child1;
        private void ClearItem(Canvas canvas)
        {
            if (canvas.Resources["taken"] != null)
            {
                var lineCanvas = canvas.FindAncestor<Canvas>();
                Point center = canvas.TransformToAncestor(lineCanvas).Transform(new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2));
                var linesToRemove = lineCanvas
                                .Children
                                .OfType<Line>()
                                .Where(line =>
                                    (line.X1 == center.X && line.Y1 == center.Y) ||
                                    (line.X2 == center.X && line.Y2 == center.Y))
                                .ToList();

                foreach (var line in linesToRemove)
                {
                    lineCanvas.Children.Remove(line);
                }

                vratiElement(canvas);

                canvas.Background = Brushes.White;
                child1 = ((TextBlock)canvas.Children[0]).Text;
                ((TextBlock)canvas.Children[0]).Text = string.Empty;
                ((TextBlock)canvas.Children[1]).Text = string.Empty;
                ((TextBlock)canvas.Children[2]).Text = string.Empty;
                canvas.Resources.Remove("taken");
                clearedCanvas = canvas;
            }
            MainWindowViewModel.ToggleUndo = new MyICommand(ClearUndo);
        }

        private void ClearUndo()
        {
            int id = int.Parse(child1.Split(':')[1].Trim());
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(("pack://application:,,," + Database.Reactors[id].Type.Image), UriKind.Absolute);
            img.EndInit();
            clearedCanvas.Background = new ImageBrush(img);
            ((TextBlock)clearedCanvas.Children[0]).Text = child1;
            ((TextBlock)clearedCanvas.Children[1]).Text = Database.Reactors[id].Name;

            var temperatureTextBlock = (TextBlock)clearedCanvas.Children[2];
            temperatureTextBlock.SetBinding(TextBlock.TextProperty, new Binding("Temperature") { Source = Database.Reactors[id], StringFormat = "T: {0}" });
            clearedCanvas.Resources.Add("taken", true);

            if(Database.Reactors[id].Type.Name == "RTD")
            {
                int index = AllReactors[0].Reactors.IndexOf(Database.Reactors[id]);
                AllReactors[0].Reactors.RemoveAt(index);
            }
            else
            {
                int index = AllReactors[1].Reactors.IndexOf(Database.Reactors[id]);
                AllReactors[1].Reactors.RemoveAt(index);
            }
        }

        private int pronadjiElement(ReactorModel draggedItem)
        {
            int index = 0;
            if (draggedItem.Type.Name.Equals("RTD"))
            {
                index = AllReactors[0].Reactors.IndexOf(draggedItem);
            }
            else
            {
                index = AllReactors[1].Reactors.IndexOf(draggedItem);
            }
            return index;
        }

        private void ukloniElement(ReactorModel draggedItem)
        {
            if (draggedItem.Type.Name.Equals("RTD"))
            {
                AllReactors[0].Reactors.RemoveAt(selected);
            }
            else
            {
                AllReactors[1].Reactors.RemoveAt(selected);
            }
        }

        private void vratiElement(Canvas canvas)
        {
            int id = int.Parse(((TextBlock)canvas.Children[0]).Text.Split(':')[1].Trim());
            string name = ((TextBlock)canvas.Children[1]).Text;
            int temperature = int.Parse(((TextBlock)canvas.Children[2]).Text.Split(':')[1].Trim());
            ReactorTypeModel type = new ReactorTypeModel();
            foreach(var r in Database.Reactors.Values)
            {
                if (r.Id == id)
                {
                    type = r.Type;   
                }
            }
            if (type.Name.Equals("RTD"))
            {
                ImageBrush slika = (ImageBrush)canvas.Background;
                ReactorModel temp = new ReactorModel() { Id = id, Name = name, Temperature = temperature, Type = type };
                if (!AllReactors[0].Reactors.Contains(temp))
                    AllReactors[0].Reactors.Add(temp);
            }
            else
            {
                ImageBrush slika = (ImageBrush)canvas.Background;
                ReactorModel temp = new ReactorModel() { Id = id, Name = name, Temperature = temperature, Type = type };
                if (!AllReactors[1].Reactors.Contains(temp))
                    AllReactors[1].Reactors.Add(temp);
            }
        }

        private void ToggleHelp(Visibility visibility)
        {
            IsHelpVisible = visibility;
            IsToolTipVisible = !IsToolTipVisible;
            OnPropertyChanged("IsHelpVisible");
            OnPropertyChanged("IsToolTipVisible");
        }
    }

    public static class VisualTreeExtensions
    {
        public static T FindAncestor<T>(this DependencyObject dependencyObject)
            where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);

            while (parent != null && !(parent is T))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as T;
        }
    }
}

