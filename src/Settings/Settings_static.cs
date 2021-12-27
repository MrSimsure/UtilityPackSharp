using System;
using System.IO;
using System.Text;
using System.Text.Json;
/*
namespace UtilityPack
{

    public static class Settings<T>
    {
        /// <summary>
        /// Property holding all the data, Loaded after calling <see cref="Load"/> and saved on disk after have called <see cref="Save"/>
        /// </summary>
        public static T data;

        /// <summary>
        /// Path where save the setting file.
        /// Default same directory as the exe
        /// </summary>
        public static string path = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// Name of the setting file, withotu extension. <br/>
        /// Default "settings"
        /// </summary>
        public static string name = "settings";

        /// <summary>
        /// If true, automatically use <see cref="SaveCrypt"/> when calling <see cref="Save"/> and <see cref="LoadCrypt"/> when calling <see cref="Load"/>
        /// </summary>
        public static bool   crypt = false;


        
        /// <summary>
        /// Load the settings from file making
        /// </summary>
        public static void Load()
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
                File.WriteAllText(filePath, settingsJson);
            }
        }

        /// <summary>
        /// Save the settings to file
        /// </summary>
        public static void Save()
        {
            if(crypt)
            { 
                SaveCrypt();
                return;
            }

            string filePath = path + name + ".json";

            string settingsJson = JsonSerializer.Serialize(data);
            File.WriteAllText(filePath, settingsJson);
        }


        /// <summary>
        /// Load the settings from file making it unreadable to human <br/>
        /// Automaticlay done when calling <see cref="Load"/> if <see cref="crypt"/> is set to true
        /// </summary>
        public static void LoadCrypt()
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
            catch (Exception ex)
            {
                data = (T)Activator.CreateInstance(typeof(T));
                SaveCrypt();
            }
        }

        /// <summary>
        /// Save the settings to file making it unreadable to human <br/>
        /// Automaticlay done when calling <see cref="Save"/> if <see cref="crypt"/> is set to true
        /// </summary>
        public static void SaveCrypt()
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
        

    }
}
*/