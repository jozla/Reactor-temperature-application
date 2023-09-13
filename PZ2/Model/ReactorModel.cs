using PZ2.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PZ2.Model
{
    public class ReactorModel: ValidationBase
    {
        private int id;
        private double temperature;
        private string name;
        private ReactorTypeModel type = new ReactorTypeModel();
        public int minTemperature = 250;
        public int maxTemperature = 350;

        public int Id
        {
            get => this.id;
            set
            {
                if (this.id != value)
                {
                    this.id = value;
                    this.OnPropertyChanged("Id");
                }
            }
        }

        public string Name
        {
            get => this.name;
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        public ReactorTypeModel Type
        {
            get => this.type;
            set
            {
                if (!this.type.Equals(value))
                {
                    this.type = value;
                    this.OnPropertyChanged("Type");
                }
            }
        }

        public double Temperature
        {
            get => this.temperature;
            set
            {
                if (this.temperature != value)
                {
                    this.temperature = value;
                    this.OnPropertyChanged("Temperature");
                    SaveColor(value);
                }
            }
        }


        public ReactorModel()
        {
            this.Name = string.Empty;
            this.Type = new ReactorTypeModel();
            this.Temperature = minTemperature;
        }

        public ReactorModel(int id, string name, ReactorTypeModel type, double temperature)
        {
            this.Id = id;
            this.Name = name ?? string.Empty;
            this.Type = type ?? new ReactorTypeModel();
            this.Temperature = temperature;
        }

        public static ReactorModel Copy(ReactorModel reactor)
        {
            return new ReactorModel(reactor.id, reactor.name, reactor.type, reactor.temperature);
        }

        public override bool Equals(object obj)
        {
            if (obj is ReactorModel reactor)
                return this.Id == reactor.Id;

            return this.ToFullString().Equals(obj?.ToString() ?? string.Empty);
        }

        public override int GetHashCode()
        {
            return this.ToFullString().GetHashCode();
        }

        protected override void ValidateSelf()
        {
            if (string.IsNullOrWhiteSpace(this.Name))
                this.ValidationErrors["Name"] = "Name field is required.";
            ValidateId();
        }


        public string ToFullString()
        {
            return $"Id:{this.Id}, Name:{this.Name}, Temperature:{this.Temperature}, Type:{this.Type}";
        }

        public void ValidateTemperature()
        {
            if (!this.IsTemperatureInMargins())
            {
                this.ValidationErrors["Temperature"] = "WARNING: Unsafe temperature!";
            }
            this.OnPropertyChanged("ValidationErrors");
        }

        public bool IsTemperatureInMargins()
        {
            return this.Temperature >= minTemperature && this.Temperature <= maxTemperature;
        }

        public void ValidateId()
        {
            if (Database.Reactors.ContainsKey(this.Id) || this.Id < 0)
                this.ValidationErrors["Id"] = "Must be unique and positive!";
            this.OnPropertyChanged("ValidationErrors");
        }


        private SolidColorBrush color;

        public SolidColorBrush Color
        {
            get => this.color;
            set
            {
                if (this.color != value)
                {
                    this.color = value;
                    this.OnPropertyChanged("Color");
                }
            }
        }

        private void SaveColor(double temperature)
        {
          if(temperature >= minTemperature && temperature <= maxTemperature)
          {
                Color = new SolidColorBrush(Colors.Cyan);
          }
          else
          {
               Color = new SolidColorBrush(Colors.Red);
          }
        }
    }

    public class ReactorsByType
    {
        public string Type { get; set; }
        public BindingList<ReactorModel> Reactors { get; set; }

        public ReactorsByType()
        {
            Reactors = new BindingList<ReactorModel>();
        }
    }
}
