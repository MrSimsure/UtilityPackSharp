using UtilityPack;
using System;


namespace Library_Test
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("helo");

            //test_database();
            //test_sqlbuilder();
            //test_parser(args);
        }

        public class sett
        {
            public string name;
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
                    ValidParameters = {"replaceParam"},
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
                new ArgsOption("all")  { Alias = "a",  IsFlag = true}
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
                string baseString = parser.GetParameter("base_string");
                string toReplace  = parser.GetParameter("to_replace");
                string newString  = parser.GetParameter("new_string");

                string result = baseString.Replace(toReplace, newString);

                Console.WriteLine(result);
            }     
        }
    
    }
}
