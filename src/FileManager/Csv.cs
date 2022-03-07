using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace UtilityPack
{
    /// <summary>
    /// Static class to read and write data from a csv files
    /// </summary>
    public static class CsvManager
    {
        /// <summary> Delimiter used in the csv files </summary>
        public static string delimiter = ";";

        /// <summary>
        /// Create a list of values reading them from a csv file
        /// </summary>
        public static List<T> Read<T>(string path)
        {
            CsvConfiguration config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = delimiter, Encoding = Encoding.UTF8 };
            using(var reader = new StreamReader(path))
            {
                using(var csv = new CsvReader(reader, config))
                {
                    List<T> records = csv.GetRecords<T>().ToList();
            
                    return records;
                } 
            }           
        }

        /// <summary>
        /// Write a list of instance to a csv file, with a specific class map to define specific behaviour
        /// </summary>
        public static string Write<T, M>(string path, string name, List<T> prodotti) where M : ClassMap
        {
            string pathComplete = path+ $"{name}.csv"; 

            CsvConfiguration config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = delimiter, Encoding = Encoding.UTF8 };
            using(var writer = new StreamWriter(path))
            {
                using(var csv = new CsvWriter(writer, config))
                {
                    csv.Context.RegisterClassMap<M>();

                    csv.WriteRecords(prodotti);

                    return pathComplete;
                }
            }
        }

        /// <summary>
        /// Write a list of instance to a csv file
        /// </summary>
        public static string Write<T>(string path, string name, List<T> prodotti) 
        {
            string pathComplete = path+ $"{name}.csv"; 

            CsvConfiguration config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = delimiter, Encoding = Encoding.UTF8 };
            using(var writer = new StreamWriter(path))
            {
                using(var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecords(prodotti);

                    return pathComplete;
                }
            }
        }
    }
}
