using System;
using UtilityPack;

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
            Console.WriteLine("helo");
            settings.Load();
          
            int anni = settings.data.anni;
            Console.WriteLine(anni);
            settings.data.anni = 0;

            settings.Save();

            //test_database();
            //test_sqlbuilder();
            //test_parser(args);
        }



        public static void test_database()
        {

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
