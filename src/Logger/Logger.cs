using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace UtilityPack
{
    /// <summary> Log location types </summary>
    public enum LogLocation
    {
        /// <summary> C:\ </summary>
        ROOT,
        /// <summary> Custom Path </summary>
        CUSTOM,
        /// <summary> ..\exe_directory\ </summary>
        EXEPOS,
        /// <summary> C:\ProgramData\ </summary>
        PROGDATA,
        /// <summary> ..\AppData\Roaming </summary>
        APPDATAROAM,
        /// <summary> ..\AppData\Local </summary>
        APPDATALOCA
    }

    /// <summary> Static class for logging data to disk </summary>
    public static class Logger
    {
        /// <summary> Root directory where to save the logs </summary>
        private static string LogDir = @"C:\Logs\";
        /// <summary> If false, no logs will ever be saved </summary>
        public static bool IsLogActive = false;
        /// <summary> Sub directory to save the logs, by default the name of the application </summary>
        public static string LogDirSub = AppDomain.CurrentDomain.FriendlyName;



        /// <summary> Save some text to file in the log directory as a file .txt </summary>
        public static void LogText(string text, string name = "")
        {
            if (!IsLogActive)
                return;

            try
            {
                string fileDate = DateTime.Now.ToString("dd-MM-yyyy__HH-mm-ss");
                string fileName = $"Text__{name}__{fileDate}.txt";
                string filePath = LogDir + LogDirSub + "/";

                Directory.CreateDirectory(filePath);
                File.WriteAllText(filePath + fileName, text);
            }
            catch { }
        }

        /// <summary> Save a json object to file in the log directory as a file .json </summary>
        public static void LogJson(object obj, string name = "")
        {
            if (!IsLogActive)
                return;

            try
            {
                string fileDate = DateTime.Now.ToString("dd-MM-yyyy__HH-mm-ss");
                string fileName = $"JSON__{name}__{fileDate}.json";
                string filePath = LogDir + LogDirSub + "/";

                Directory.CreateDirectory(filePath);
                File.WriteAllText(filePath + fileName, JsonSerializer.Serialize(obj));
            }
            catch { }
        }

        /// <summary> Save some json text to file in the log directory as a file .json </summary>
        public static void LogJson(string text, string name = "")
        {
            if (!IsLogActive)
                return;

            try
            {
                string fileDate = DateTime.Now.ToString("dd-MM-yyyy__HH-mm-ss");
                string fileName = $"JSON__{name}__{fileDate}.json";
                string filePath = LogDir + LogDirSub + "/";

                Directory.CreateDirectory(filePath);
                File.WriteAllText(filePath + fileName, text);
            }
            catch { }
        }

        /// <summary> Save a list of json objects to file in the log directory as a file .json </summary>
        public static void LogJsonList<T>(List<T> list, string name = "")
        {
            if (!IsLogActive)
                return;

            try
            {
                string fileDate = DateTime.Now.ToString("dd-MM-yyyy__HH-mm-ss");
                string fileName = $"JSON_list__{name}__{fileDate}.json";
                string filePath = LogDir + LogDirSub + "/";

                string output = "{\n";

                for (int i = 0; i < list.Count; i++)
                {
                    output += JsonSerializer.Serialize(list[i]);
                    output += ",\n\n";
                }
                output += "}";

                Directory.CreateDirectory(filePath);
                File.WriteAllText(filePath + fileName, output);
            }
            catch { }
        }


        /// <summary> Set the log folder location </summary>
        public static void SetLogLocation(LogLocation location, string customDir = "")
        {
            switch(location)
            {
                case LogLocation.ROOT:
                {
                    LogDir = Path.GetPathRoot(Environment.SystemDirectory);

                    break;
                }
                case LogLocation.CUSTOM:
                {
                    LogDir = customDir;

                    break;
                }
                case LogLocation.EXEPOS:
                {
                    LogDir = AppDomain.CurrentDomain.BaseDirectory+@"\";

                    break;
                }
                case LogLocation.PROGDATA:
                {
                    LogDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

                    break;
                }
                case LogLocation.APPDATAROAM:
                {
                    LogDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                    break;
                }
                    case LogLocation.APPDATALOCA:
                {
                    LogDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                    break;
                }
            }
        }
    }
}
