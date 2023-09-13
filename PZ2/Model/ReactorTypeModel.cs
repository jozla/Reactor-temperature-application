using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ2.Model
{
    public class ReactorTypeModel
    {
        public string Name { get; set; }
        public string Image { get; set; }

        public ReactorTypeModel()
        {
            this.Name = string.Empty;
            this.Image = string.Empty;
        }

        public ReactorTypeModel(string name, string Image)
        {
            this.Name = name ?? string.Empty;
            this.Image = Image ?? string.Empty;
        }

        public override bool Equals(object obj)
        {
            return this.ToString().Equals(obj?.ToString() ?? string.Empty);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return $"{this.Name} {this.Image}";
        }

    }
}
