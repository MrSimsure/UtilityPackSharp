using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace UtilityPack.FileManager.Csv
{
    /// <summary>
    /// Static class to read and write data from a csv files
    /// </summary>
    public static class CsvManager
    {
        /// <summary> Delimiter used in the csv files. (Deafault ;)</summary>
        public static string delimiter = ";";
        /// <summary> Culture to write csv files </summary>
        public static CultureInfo culture = CultureInfo.InvariantCulture;
         /// <summary> If true create every unexisting directory when passing a csv path. (Deafault true) </summary>
        public static bool createFolders = true;

        /// <summary>
        /// Create a list of values reading them from a csv file
        /// </summary>
        public static List<T> Read<T>(string path)
        {
            CsvConfiguration config = new CsvConfiguration(culture) { Delimiter = delimiter, Encoding = Encoding.UTF8 };
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
        public static string Write<T, M>(string path, List<T> prodotti) where M : ClassMap
        {
            string dirPath = Path.GetDirectoryName(path);
            if(!Directory.Exists(dirPath) && createFolders)
                Directory.CreateDirectory(dirPath);

            CsvConfiguration config = new CsvConfiguration(culture) { Delimiter = delimiter, Encoding = Encoding.UTF8 };
            using(var writer = new StreamWriter(path))
            {
                using(var csv = new CsvWriter(writer, config))
                {
                    csv.Context.RegisterClassMap<M>();

                    csv.WriteRecords(prodotti);

                    return path;
                }
            }
        }

        /// <summary>
        /// Write a list of instance to a csv file
        /// </summary>
        public static string Write<T>(string path, List<T> prodotti) 
        {
            string dirPath = Path.GetDirectoryName(path);
            if(!Directory.Exists(dirPath) && createFolders)
                Directory.CreateDirectory(dirPath);

            CsvConfiguration config = new CsvConfiguration(culture) { Delimiter = delimiter, Encoding = Encoding.UTF8 };
            using(var writer = new StreamWriter(path))
            {
                using(var csv = new CsvWriter(writer, config))
                {
                    csv.WriteRecords(prodotti);

                    return path;
                }
            }
        }
    }
}
