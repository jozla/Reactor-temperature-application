using PZ2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ2.Helpers
{
    public static class Database
    {
        public static SortedDictionary<int, ReactorModel> Reactors { get; private set; }
        public static List<int> ReactorIds { get; private set; }
        public static SortedDictionary<string, ReactorTypeModel> ReactorTypes { get; private set; }

        static Database()
        {
            Database.Reactors = new SortedDictionary<int, ReactorModel>();
            Database.ReactorIds = Database.Reactors.Keys.ToList();
            Database.ReactorTypes = new SortedDictionary<string, ReactorTypeModel>()
            {
                { "RTD", new ReactorTypeModel("RTD", "../../Assets/RTD.jpeg")},
                { "Thermopile", new ReactorTypeModel("Thermopile","../../Assets/Thermopile.jpeg") }
            };
        }

        public static bool Add(ReactorModel reactor)
        {
            try
            {
                if (reactor is null || Database.Reactors.ContainsKey(reactor.Id))
                    return false;
                else
                {
                    Database.Reactors.Add(reactor.Id, reactor);
                    Database.ReactorIds.Add(reactor.Id);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool Remove(int id)
        {
            try
            {
                if (!Database.Reactors.Remove(id))
                    return false;
                else
                {
                    Database.ReactorIds.Remove(id);
                    return true;
                }

            }
            catch
            {
                return false;
            }
        }
    }
}
