using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using UtilityPack.Print;
using UtilityPack.Database;
using UtilityPack.Connections.Ftp;
using UtilityPack.Setting;
using UtilityPack.ArgsParser;
using System.Collections.Generic;
using System.Linq;
using UtilityPack.FileManager.Ini;
using System.Data;
using UtilityPack.SqlBuilder;
using UtilityPack.Logger;

namespace Library_Test
{
    // Definition class with default values
    public class ProgramData 
    {
        public bool   isActive { get; set;} = false;
        public string Name     { get; set;} = "Michael";
        public int    Uses     { get; set;} = 123;
    }

    class Program
    {
        // Create the Setting instance, passing the definition class as generic parameter
        public static Settings<ProgramData> setting = new();


        class jsonObj
        {
            public string Name;
            public int Age;
        }

        static void Main()
        {
            // Set the main log folder location
            Logger.SetLocation(LogLocation.APPDATALOCA, "ProgramLogs");

            // Save a simple text message on a report.txt file located in the log folder
            Logger.SaveText("Report message", "report");

            // Create and append a json like object to a report_data.json file located in the log folder
            jsonObj json = new()
            {
                Name = "Burt",
                Age  = 20
            };

            Logger.SaveJson(json, "report_data", true);
        }


        public static string GetRealFileName(string path)
        {
            string directory = Path.GetDirectoryName(path);
            string pattern = Path.GetFileName(path);
            string resultFileName;

            IEnumerable<string> foundFiles = Directory.EnumerateFiles(directory, pattern);

            if (foundFiles.Any())
            {
                if (foundFiles.Count() > 1)
                {
                    throw new Exception("Ambiguous File reference for " + path);
                }
                else
                {
                    resultFileName = foundFiles.First();
                }
            }
            else
            {
                throw new FileNotFoundException("File not found" + path, path);
            }

            return resultFileName;
        }

        public static void test_ftp()
        {
            FtpConnection.printDebug = true;
            string localFolder = AppDomain.CurrentDomain.BaseDirectory+"ftp";

            using(FtpConnection connection = new(FtpProtocolType.SFTP, "173.249.51.65", "simone", "enter.srv.21", "22"))
            {
               //connection.Upload(localFolder+"/Order_6.xml", "./Exported_Orders");   
               connection.Download("Exported_Orders", localFolder);    
               //connection.ClearFolder("./Exported_Orders/");
            }       
        }

        public static void test_settings()
        {
            // Choose setting configuration ad load the data
            setting.crypt = true;
            setting.SetLocation(SettLocation.PROGDATA, "ProgramName/");
            setting.Load();
          
            // Change a some values
            setting.data.Uses += 1;

            // Save the setting to file
            setting.Save();

            // Read a value from the setting data
            string name = setting.data.Name;
        }

        public static void test_database()
        {
            
            Database DB = new Database("VAULT", "localhost", "mario", "12345678", DbSystem.SQL_SERVER);

            bool isConnected = DB.TestConnection(true);

            if(isConnected)
            {
                string query = "SELECT name, age FROM users";

                DataTable result = DB.ExecuteSqlQuery(query);

                if(result.Rows.Count <= 0)
                    return;

                object value = result.Rows[0]["age"];
                int firstUserAge = Database.ParseToNumber(value, 0);
            }
        } 

        public static void test_sqlbuilder()
        {

            SqlFactory sql = SqlFactory.CreateUpdate("USERS");
            
            sql.SetParam("NOME",    "mario");
            sql.SetParam("COGNOME", "rossi");
            sql.SetParam("ANNI",    123);
            sql.SetParam("CITTA",   "paperopoli");

            sql.SetWhere("ID",      481237108);

            string command = sql.GetCommand();

        }

        public static void test_parser(string[] args)
        {
            ArgsParser parser = new();

            parser.AddCommand(new[]
            {
                new ArgsCommand("replace")
                {
                    ValidOptions    = {"help", "all"}
                }
            });
                
            parser.AddParameterGroup("replaceParam", new[]
            {
                new ArgsParameter("base_string"),
                new ArgsParameter("to_replace"),
                new ArgsParameter("new_string"),
            });

            parser.AddOption(new[]
            {
                new ArgsOption("help") { Alias = "h",  IsFlag = true},
                new ArgsOption("all")  { Alias = "a",  IsFlag = false}
            });

            try
            { 
                parser.Parse(args); 
            }
            catch (ArgsParseErrorException e) 
            { 
                Console.WriteLine(e.StackTrace); 
            }


            if(parser.GetCommand(0) == "replace")
            {
                /*
                string baseString = parser.GetParameter("base_string");
                string toReplace  = parser.GetParameter("to_replace");
                string newString  = parser.GetParameter("new_string");
                */

                double opt = parser.LoadOption("all", 23.0);
                Console.WriteLine(opt);
               // string result = baseString.Replace(toReplace, newString);

                //Console.WriteLine(result);
            }     
        }
    
    }
}
