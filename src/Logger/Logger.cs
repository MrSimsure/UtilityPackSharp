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
        /// Max size of a log file, if the file exceed this value and <see cref="IsSizeLimitActive"/> is set to true, the file will be cleared.<br/>
        /// (Default value = 5 MB = 1048576 Byte)
        /// </summary>
        public static long MaxFileSize = 1048576;

        /// <summary>
        /// If set to true, when a file exceed the <see cref="MaxFileSize"/> it will be cleared at the next log saving
        /// </summary>
        public static bool IsSizeLimitActive = false;

        /// <summary>
        /// Number of new line beetween a content append and the next one. <br/>
        /// (Default 3)
        /// </summary>
        public static int LogAppendSpace = 3;


        private static bool SaveString(string text, string name, bool append, string ext)
        { 
            if(!IsLogActive)
                    return true;

            try
            {
                string fileDate = DateTime.Now.ToString("dd-MM-yyyy__HH-mm-ss");
                string fileName = $"Log__{fileDate}."+ext;
                string fullPath = BasePath+SubPath;

                if(!fullPath.EndsWith("/"))
                    fullPath += "/";

                if(name != "")
                    fileName = name+"."+ext;

                Directory.CreateDirectory(fullPath);

                if(File.Exists(fullPath + fileName))
                {
                    long size = new FileInfo(fullPath + fileName).Length;
                    if(size > MaxFileSize)
                        File.WriteAllText(fullPath + fileName, "");
                }

                string space = "";
                for(int i=0; i<LogAppendSpace; i++)
                    space += "\n";

                if(append)
                    File.AppendAllText(fullPath + fileName, space+text);
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
        /// Save some text to file in the log directory as a file .txt 
        /// </summary>
        public static bool SaveText(string text, string name = "", bool append = false)
        {
            return SaveString(text, name, append, "txt");
        }

        /// <summary> 
        /// Save some json text to file in the log directory as a file .json 
        /// </summary>
        public static bool SaveJson(string text, string name = "", bool append = false)
        {
            return SaveString(text, name, append, "json");
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

        private static string JoinPath(string path1, string path2 = null)
        {
            string final;

            if(path2 != null)
            { 
                if(Path.IsPathRooted(path2))
		        {
			        path2 = path2.TrimStart(Path.DirectorySeparatorChar);
			        path2 = path2.TrimStart(Path.AltDirectorySeparatorChar);
		        }

                final = Path.Combine(path1, path2);
                final += final.EndsWith("/") || path1.EndsWith(@"\") ? "" : "/";
            }
            else
            {
                final = path1.EndsWith("/") || path1.EndsWith(@"\") ? path1 : path1+"/";
            }

		    return final;
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
                    BasePath = JoinPath(Path.GetPathRoot(Environment.SystemDirectory), customDir);
                    break;
                }
                case LogLocation.CUSTOM: 
                {
                    BasePath = JoinPath(customDir);
                    break;
                }
                case LogLocation.EXEDIR:
                {
                    BasePath = JoinPath(AppDomain.CurrentDomain.BaseDirectory, customDir);
                    break;
                }
                case LogLocation.PROGDATA:
                {
                    BasePath = JoinPath(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), customDir);
                    break;
                }
                case LogLocation.APPDATAROAM:
                {
                    BasePath = JoinPath(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), customDir);
                    break;
                }
                case LogLocation.APPDATALOCA:
                {
                    BasePath = JoinPath(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), customDir);
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
    
        /// <summary>
        /// Activate the size limit control and set the <see cref="MaxFileSize"/> value to as pecific number
        /// </summary>
        public static void SetMaxFileSize(long maxByteSize)
        {
            MaxFileSize = maxByteSize;
            IsSizeLimitActive = true;
        }
    }
}
