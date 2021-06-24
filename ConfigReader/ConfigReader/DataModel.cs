using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigReader
{
    class DataModel
    {
        public int OrdersPerHour { get; set; }
        public int OrderLinesPerOrder { get; set; }
        public string InboundStrategy { get; set; }
        public string PowerSupply { get; set; }
        public TimeSpan ResultStartTime { get; set; }
        public int ResultInterval { get; set; }

        //Tried to think of a way for this to be automatic, something with Nullable might work but is hard to work with
        private string[] ErrorVariables = { int.MinValue.ToString(), TimeSpan.MinValue.ToString(), "" };

        /// <summary>
        /// Constructor with default error values
        /// </summary>
        public DataModel()
        {
            OrdersPerHour = int.MinValue;
            OrderLinesPerOrder = int.MinValue;
            InboundStrategy = "";
            PowerSupply = "";
            ResultStartTime = TimeSpan.MinValue;
            ResultInterval = int.MinValue;
        }

        /// <summary>
        /// Prints results to console to see how they changed
        /// </summary>
        public void PrintResults()
        {
            foreach(var a in this.GetType().GetProperties())
            {
                Console.Write(a.Name + ": ");
                if (!ErrorVariables.Contains(a.GetValue(this, null).ToString()))
                    Console.WriteLine(a.GetValue(this, null));
                else
                    Console.WriteLine("Error");
            }
        }

        /// <summary>
        /// Reads information from a file and changes property values accordingly
        /// </summary>
        /// <param name="fileName">File path</param>
        public void ReadFromFile(string fileName)
        {
            string[] lines = System.IO.File.ReadAllLines(@fileName);
            foreach (string line in lines)
            {
                string newLine = line.Contains("//") ? line.Remove(line.IndexOf("//")) : line; //Removes comment lines in the file
                if (newLine.Contains("===") || newLine.Contains('-'))
                    continue;
                /*
                The whole idea is for the program to be extendable easily, so only the parameters need to be added and the default values set to errors.
                The Method itself will try to search the needed lines for given class parameters and set their values accordingly.
                */
                foreach(var a in this.GetType().GetProperties())
                {
                    if(newLine.Contains(char.ToLower(a.Name[0]) + a.Name.Substring(1)))
                    {
                        string valueString = newLine.Split(':', 2)[1];
                        string valueVariable = valueString.Replace("\t", "");
                        //Parsing error catching for data types
                        try
                        {
                            //TimeSpan giving a lot of trouble with parsing
                            if (a.PropertyType == typeof(TimeSpan))
                            {
                                a.SetValue(this, TimeSpan.Parse(valueVariable), null);
                            }
                            else
                            {
                                a.SetValue(this, Convert.ChangeType(valueVariable, a.PropertyType), null);
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Invalid data format cast on variable " + a.Name);
                        }
                    }
                }
            }
        }
    }
}
