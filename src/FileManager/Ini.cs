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
        /// <summary> If true create section and key when they are not found during writing </summary>
        public static bool forceWrite = true;

        /// <summary>
        /// Write a value in a ini file
        /// </summary>
        public static bool Write(string filePath, string section, string key, object value)
        {
            if(File.Exists(filePath) == false)
            {
                if(printErrors)
                    Console.WriteLine($"\nFile ini not found '{filePath}' \n{Environment.StackTrace}\n");
            }

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(filePath);

            if(data.Sections.ContainsSection(section) == false)
            {
                if(printErrors)
                    Console.WriteLine($"\nSection '{section}' not found in file '{Path.GetFileName(filePath)}' \n{Environment.StackTrace}\n");

                if(forceWrite == false)
                    return false;

                data.Sections.Add(new SectionData(section));
            }         

            if(data.Sections[section].ContainsKey(key) == false)
            {
                if(printErrors)
                    Console.WriteLine($"\nKey '{key}' not found in section '{section}' \n{Environment.StackTrace}\n");

                if(forceWrite == false)
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
        public static T Read<T>(string filePath, string section, string key, T defaul)
        {
            if(File.Exists(filePath) == false)
            {
                if(printErrors)
                    Console.WriteLine($"\nFile ini not found '{filePath}' \n{Environment.StackTrace}\n");

                return defaul;
            }

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(filePath);

            if(data.Sections.ContainsSection(section) == false)
            {
                if(printErrors)
                    Console.WriteLine($"\nSection '{section}' not found in file '{Path.GetFileName(filePath)}' \n{Environment.StackTrace}\n");

                return defaul;
            }

            if(data.Sections[section].ContainsKey(key) == false)
            {
                if(printErrors)
                    Console.WriteLine($"\nKey '{key}' not found in section '{section}' \n{Environment.StackTrace}\n");

                return defaul;
            }

            string value = data[section][key];
            
            return (T)Convert.ChangeType(value, typeof(T));  
        }
    }
}
