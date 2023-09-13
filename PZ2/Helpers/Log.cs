using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PZ2.Helpers
{
    public class Log
    {
        public static bool Add(object outObj, string fileName = "log.txt")
        {
            if (outObj is null || fileName is null)
            {
                Trace.TraceError("Both input parameters must not be null!");
                return false;
            }
            try
            {
                File.AppendAllText(Environment.CurrentDirectory + @"\" + fileName, outObj.ToString() + Environment.NewLine);
                return true;
            }
            catch (Exception err)
            {
                Trace.TraceError(err.Message);
                return false;
            }
        }

        public static string ConvertToLogFormat(Model.ReactorModel reactor)
        {
            return ConvertToLogFormat(reactor.Id, reactor.Temperature);
        }

        public static string ConvertToLogFormat(int id, double temperature)
        {
            var currDate = DateTime.Now;
            return $"{currDate.ToString("dd/MM/yyyy',' HH:mm:ss")}, {id}, {temperature}";
        }


        public static List<double> ReadLast5Changes(int id)
        {
            List<double> values = new List<double>();
            string path = Environment.CurrentDirectory + @"\" + "log.txt";
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line;
                List<string> lines = new List<string>();

                while ((line = reader.ReadLine()) != null)
                {
                    lines.Insert(0, line);
                }

                int count = 0;
                foreach (string currentLine in lines)
                {
                    string[] parts = currentLine.Split(',');

                    int lineId = int.Parse(parts[2]);
                    if (lineId == id)
                    {
                        double value = double.Parse(parts[3]);
                        values.Add(value);
                        count++;
                        if (count == 5)
                        {
                            break;
                        }
                    }
                }
            }

            return values;
        }
    }
}
