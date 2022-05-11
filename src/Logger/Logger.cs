using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace UtilityPack.Logger
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
        /// <summary> ..\exe_directory\custom_dir </summary>
        EXEDIR,
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
        /// <summary> 
        /// Root directory where to save the logs <br/> 
        /// (Default same directory as the exe) 
        /// </summary>
        public static string BasePath = AppDomain.CurrentDomain.BaseDirectory+@"\";

        /// <summary> 
        /// Additional sub directories where save the logs, added at the end of BasePath <br/> 
        /// (Default "") 
        /// </summary>
        public static string SubPath = "";
        
        /// <summary> 
        /// If false, logs will not be saved <br/>
        /// (Default true)
        /// </summary>
        public static bool IsLogActive = true;
        
        /// <summary> 
        /// If true, when an error occur it will be thrown, otherwhise the functions will silently return false <br/>
        /// (Default false)
        /// </summary>
        public static bool IsCatchErrorActive = false;



        /// <summary> 
        /// Save some text to file in the log directory as a file .txt 
        /// </summary>
        public static bool SaveText(string text, string name = "", bool append = false)
        {
            if (!IsLogActive)
                return true;

            try
            {
                string fileDate = DateTime.Now.ToString("dd-MM-yyyy__HH-mm-ss");
                string fileName = $"Log__{fileDate}.txt";
                string fullPath = BasePath+SubPath;

                if(!fullPath.EndsWith("/"))
                    fullPath += "/";

                if(name != "")
                    fileName = name;

                Directory.CreateDirectory(fullPath);

                if(append)
                    File.AppendAllText(fullPath + fileName, "\n\n"+text);
                else
                    File.WriteAllText(fullPath + fileName, text);

                return true;
            }
            catch(Exception e)
            {
                if(IsCatchErrorActive)
                    throw(e);
                else
                    return false;
            }
        }

        /// <summary> 
        /// Save some json text to file in the log directory as a file .json 
        /// </summary>
        public static bool SaveJson(string text, string name = "", bool append = false)
        {
            if (!IsLogActive)
                return true;

            try
            {
                string fileDate = DateTime.Now.ToString("dd-MM-yyyy__HH-mm-ss");
                string fileName = $"Log__{fileDate}.json";
                string fullPath = BasePath+SubPath;

                if(!fullPath.EndsWith("/"))
                    fullPath += "/";

                if(name != "")
                    fileName = name;

                Directory.CreateDirectory(fullPath);

                if(append)
                   File.AppendAllText(fullPath + fileName, "\n\n"+text);
                else
                   File.WriteAllText(fullPath + fileName, text);

                return true;
            }
            catch(Exception e)
            {
                if(IsCatchErrorActive)
                    throw(e);
                else
                    return false;
            }
        }

        /// <summary> 
        /// Save a json object to file in the log directory as a file .json 
        /// </summary>
        public static bool SaveJson(object obj, string name = "", bool append = false)
        {
            return SaveJson(JsonSerializer.Serialize(obj), name, append);
        }
  
        /// <summary> 
        /// Save a list of json objects to file in the log directory as a file .json 
        /// </summary>
        public static bool SaveJsonList<T>(List<T> list, string name = "", bool append = false)
        {
            string output = "{\n";

            for (int i = 0; i < list.Count; i++)
            {
                output += JsonSerializer.Serialize(list[i]);
                output += ",\n\n";
            }
            output += "}";

            return SaveJson(output, name, append);
        }



        /// <summary> 
        /// Delete every file inside the log folder, if another folder BasePath is specified, clear it instead
        /// </summary>
        public static void ClearLogFolder(string chosenPath = null)
        {
            if(chosenPath == null)
                chosenPath = BasePath;

            DirectoryInfo dir = new DirectoryInfo(chosenPath);

            foreach(FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                ClearLogFolder(di.FullName);
                di.Delete();
            }
        }

        /// <summary> 
        /// Set the log save location 
        /// </summary>
        public static void SetLocation(LogLocation location, string customDir = "")
        {
            switch(location)
            {
                case LogLocation.ROOT:
                {
                    BasePath = Path.GetPathRoot(Environment.SystemDirectory)+@"\"+customDir;
                    break;
                }
                case LogLocation.CUSTOM:
                {
                    BasePath = customDir;
                    break;
                }
                case LogLocation.EXEPOS:
                {
                    BasePath = AppDomain.CurrentDomain.BaseDirectory+@"\"+customDir;
                    break;
                }
                case LogLocation.EXEDIR:
                {
                    BasePath = AppDomain.CurrentDomain.BaseDirectory+@"\"+customDir;
                    break;
                }
                case LogLocation.PROGDATA:
                {
                    BasePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)+@"\"+customDir;
                    break;
                }
                case LogLocation.APPDATAROAM:
                {
                    BasePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+@"\"+customDir;
                    break;
                }
                case LogLocation.APPDATALOCA:
                {
                    BasePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)+@"\"+customDir;
                    break;
                }
            }
        }
    
        /// <summary> 
        /// Set the log folder sub location 
        /// </summary>
        public static void SetSubLocation(string subLocation)
        {
            SubPath = subLocation;
        }
    
    }
}
