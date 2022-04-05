using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;

using UtilityPack.Print;
using UtilityPack.Database;
using UtilityPack.Connections.Ftp;
using UtilityPack.Setting;
using UtilityPack.SqlBuilder;
using UtilityPack.ArgsParser;
using System.Collections.Generic;
using System.Linq;
using UtilityPack.FileManager.Ini;

namespace Library_Test
{
    public class opt 
    {
        public bool   attivo { get; set;} = false;
        public string nome   { get; set;} = "pippo";
        public int    anni   { get; set;} = 123;
    }

   public class Altro
   {
        public Altro()
        {
            int anni = Program.Settings.anni;
        }
   }

    class Program
    {
        private static Settings<opt> settings = new();
        public static opt Settings {get => settings.data;}

        static void Main(string[] args)
        {
            test_ftp();

            //string val = RegistryManager.Read(RegistryHive.LocalMachine, @"SOFTWARE\WOW6432Node\ODBC\ODBC.INI\ETOS", "Server");
            //Print.Message(val);
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
            settings.Load();
          
            int anni = settings.data.anni;
            Console.WriteLine(anni);
            settings.data.anni = 0;

            settings.Save();
        }

        public static void test_database()
        {
            //Database DB = new("ETOS", "SERVER-TEST\\SQLEXPR17", "sa", "sa1234!");
            Database DB = new("ETOS.FDB", "localhost", "SYSDBA", "masterkey", DbSystem.FIREBIRD);
            DB.TestConnection(true);
        } 

        public static void test_sqlbuilder()
        {
            SqlString sql = SqlString.CreateUpdate("CLIFO");

            sql.SetParam("NOME",    "lucio");
            sql.SetParam("COGNOME", "asdasd");

            sql.SetWhere("ANNI",    123);
            sql.SetWhere("CITTA",   "paperopoli");

            Console.WriteLine(sql.GetCommand());
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
