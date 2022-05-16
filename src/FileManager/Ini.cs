using FileManager;
using IniParser;
using IniParser.Model;
using System;
using System.IO;

namespace UtilityPack.FileManager.Ini
{
    /// <summary>
    /// Static class to read and write data from a ini files
    /// </summary>
    public static class IniManager
    {
        /// <summary> If true print errors, when a file is not found or a section or key are missing </summary>
        public static bool printErrors = false;
        /// <summary> If true throw errors, when a file is not found or a section or key are missing </summary>
        public static bool throwErrors = true;
        /// <summary> If true create section and key when they are not found during writing </summary>
        public static bool forceWrite = true;


        /// <summary>
        /// Write a value in a ini file
        /// </summary>
        public static bool Write(string filePath, string section, string key, object value)
        {
            //search for file
            if(File.Exists(filePath) == false)
            {
                string error = $"File ini not found '{filePath}' \n{Environment.StackTrace}\n";

                if(printErrors)
                    Console.WriteLine(error);

                throw new FileNotFoundException(error);
            }

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(filePath);

            //search for section
            if(data.Sections.ContainsSection(section) == false)
            {
                string error = $"Section '{section}' not found in file '{Path.GetFileName(filePath)}' \n{Environment.StackTrace}\n";

                if(printErrors)
                    Console.WriteLine(error);

                if(forceWrite == false)
                    if(throwErrors)
                        throw new IniValueNotFoundException(error);
                    else
                        return false;

                data.Sections.Add(new SectionData(section));
            }         


            //search for key
            if(data.Sections[section].ContainsKey(key) == false)
            {
                string error = $"Key '{key}' not found in section '{section}' \n{Environment.StackTrace}\n";

                if(printErrors)
                    Console.WriteLine(error);

                if(forceWrite == false)
                    if(throwErrors)
                        throw new IniValueNotFoundException(error);
                    else
                        return false;

                data[section].AddKey(key);
            }

            data[section][key] = (string)Convert.ChangeType(value, typeof(string));
            parser.WriteFile(filePath, data);

            return true;
        }


        /// <summary>
        /// Read a value from a file ini, if some error occours return the default value passed instead
        /// </summary>
        public static T Read<T>(string filePath, string section, string key, T _default )
        {
            //search for file
            if(File.Exists(filePath) == false)
            {
                string error = $"File ini not found '{filePath}' \n{Environment.StackTrace}\n";

                if(printErrors)
                    Console.WriteLine(error);

                throw new FileNotFoundException();
            }

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(filePath);

            //search for section
            if(data.Sections.ContainsSection(section) == false)
            {
                string error = $"Section '{section}' not found in file '{Path.GetFileName(filePath)}' \n{Environment.StackTrace}\n";

                if(printErrors)
                    Console.WriteLine(error);

                if(throwErrors)
                    throw new IniValueNotFoundException(error);

                return _default;
            }

            //search for key
            if(data.Sections[section].ContainsKey(key) == false)
            {
                string error = $"Key '{key}' not found in section '{section}' \n{Environment.StackTrace}\n";

                if(printErrors)
                    Console.WriteLine(error);

                if(throwErrors)
                    throw new IniValueNotFoundException(error);

                return _default;
            }

            string value = data[section][key];
            
            return (T)Convert.ChangeType(value, typeof(T));  
        }
    }
}
