using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace UtilityPack.Setting
{
    /// <summary> Setting location types </summary>
    public enum SettLocation
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

    /// <summary>
    /// Class to manage settings
    /// </summary>
    public class Settings<T>
    {
        /// <summary>
        /// Property holding all the data, Loaded after calling <see cref="Load"/> and saved on disk after calling <see cref="Save"/>
        /// </summary>
        public T data;

        /// <summary>
        /// Path where save the setting file.
        /// (Default same directory as the exe)
        /// </summary>
        public string path = AppDomain.CurrentDomain.BaseDirectory+@"\";

        /// <summary>
        /// Name of the setting file, withotu extension. <br/>
        /// (Default "settings")
        /// </summary>
        public string name = "settings";

        /// <summary>
        /// If true, automatically use <see cref="SaveCrypt"/> when calling <see cref="Save"/> and <see cref="LoadCrypt"/> when calling <see cref="Load"/>
        /// <br/>(Default false)
        /// </summary>
        public bool   crypt = false;

        /// <summary>
        /// If true pretty print the json file.
        /// </summary>
        public bool prettyPrint = true;

        
        /// <summary>
        /// Load the settings from the file and parse them to the "data" property.<br/>
        /// If the file is not present it will be created with the default parameters.
        /// </summary>
        public void Load()
        {
            if(crypt)
            { 
                LoadCrypt();
                return;
            }

            string filePath = path + name + ".json";

            try
            {              
                if(!File.Exists(path))
                       Directory.CreateDirectory(path);

                string settingsJson = File.ReadAllText(filePath, Encoding.UTF8);
                data = JsonSerializer.Deserialize<T>(settingsJson);
            }
            catch (Exception)
            {
                data = (T)Activator.CreateInstance(typeof(T));
                string settingsJson = JsonSerializer.Serialize(data);

                if(File.Exists(filePath))
                    File.Move(filePath, path + name + "_reading_error.json");

                File.WriteAllText(filePath, settingsJson);
            }
        }

        /// <summary>
        /// Save the settings data to file.
        /// </summary>
        public void Save()
        {
            if(crypt)
            { 
                SaveCrypt();
                return;
            }

            string filePath = path + name + ".json";

            string settingsJson = JsonSerializer.Serialize(data, new JsonSerializerOptions(){WriteIndented = prettyPrint });
            File.WriteAllText(filePath, settingsJson);
        }


        /// <summary>
        /// Load the settings from the file, decrypt and parse them to the "data" property.<br/>
        /// If the file is not present it will be created with the default parameters. <br/>
        /// Automaticlay done when calling <see cref="Load"/> if <see cref="crypt"/> is set to true.
        /// </summary>
        public void LoadCrypt()
        {
            string filePath = path + name + ".json";

            try
            {              
                if(!File.Exists(path))
                       Directory.CreateDirectory(path);
          
                byte[] settingsByte = File.ReadAllBytes(filePath);
                char[] settingsJson = new char[settingsByte.Length];

                for (int i=0; i<settingsByte.Length; i++)
                {
                    byte bit = settingsByte[i];
                    bit -= 10;

                    settingsJson[i] = (char)bit;
                }

                data = JsonSerializer.Deserialize<T>(new string(settingsJson));
            }
            catch (Exception)
            {
                data = (T)Activator.CreateInstance(typeof(T));

                if(File.Exists(filePath))
                    File.Move(filePath, path + name + "_reading_error.json");

                SaveCrypt();
            }
        }

        /// <summary>
        /// Save the settings data to file, but crypting them, making it unreadable to human. <br/>
        /// Automaticlay done when calling <see cref="Save"/> if <see cref="crypt"/> is set to true.
        /// </summary>
        public void SaveCrypt()
        {
            try
            { 
                string filePath = path + name + ".json";

                string settingsJson = JsonSerializer.Serialize(data);
                byte[] settingsByte = new byte[settingsJson.Length];

                for (int i=0; i<settingsJson.Length; i++)
                {
                    byte bit = (byte)settingsJson[i];
                    bit += 10;

                    settingsByte[i] = bit;
                }
                File.WriteAllBytes(filePath, settingsByte);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        

        /// <summary> 
        /// Set the settings file save location.<br/>
        /// When location == SettLocation.CUSTOM, the "customDir" parameter work as a full path.<br/>
        /// In every other cases "customDir" works as an additional final string to the base path.
        /// </summary>
        public void SetLocation(SettLocation location, string customDir = "")
        {
            switch(location)
            {
                case SettLocation.ROOT:
                {
                    path = Path.GetPathRoot(Environment.SystemDirectory)+@"\"+customDir;
                    break;
                }
                case SettLocation.CUSTOM:
                {
                    path = customDir;
                    break;
                }
                case SettLocation.EXEDIR:
                {
                    path = AppDomain.CurrentDomain.BaseDirectory+@"\"+customDir;
                    break;
                }
                case SettLocation.PROGDATA:
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)+@"\"+customDir;
                    break;
                }
                case SettLocation.APPDATAROAM:
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+@"\"+customDir;
                    break;
                }
                case SettLocation.APPDATALOCA:
                {
                    path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)+@"\"+customDir;
                    break;
                }
            }
        }

    }
}
