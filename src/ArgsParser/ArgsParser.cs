
using System;
using System.Collections.Generic;
using System.Linq;

namespace UtilityPack.ArgsParser
{
    /// <summary> Class to parse arguments from the terminal and consult them during execution </summary>
    public class ArgsParser
    {
        private List<ArgsCommand>    CommandsDefinition = new List<ArgsCommand>();
        private List<ArgsOption>     OptionsDefinition  = new  List<ArgsOption>();
        private Dictionary<string,   List<ArgsParameter>>  ParameterDefinition = new  Dictionary<string,   List<ArgsParameter>>();

        private Dictionary<string, string> Commands  = new Dictionary<string, string>();
        private Dictionary<string, string> Options   = new Dictionary<string, string>();
        private Dictionary<string, string> Parameter = new Dictionary<string, string>();
        private bool IsHelp = false;

        /// <summary> If set to true will not print an error if no command is used </summary>
        public bool allowNoCommand = false;
        /// <summary> If set to true will not print an error if no pareter is passed </summary>
        public bool allowNoParameter = false;
        /// <summary> List of strings that recognizes the start of an option declaration. (Default {"-", "--"} ) </summary>
        public string[] optionPrefixs = { "-", "--"};
        /// <summary> List of strings indicating the presence of the "help" option to write the help message. (Default {"-help", "--help", "-h"}) </summary>
        public string[] helpFlags = { "-help", "--help", "-h"};

        /// <summary> Add a new command definition </summary>
     public void AddCommand(ArgsCommand command)
        {
            CommandsDefinition.Add(command);
        }
     /*     /// <summary> Add a new array of commands definition </summary>
        public void AddCommand(List<ArgsCommand> command)
        {
            CommandsDefinition.AddRange(command);
        }*/

        /// <summary> Add a new option definition </summary>
        public void AddOption(ArgsOption option)
        {
            OptionsDefinition.Add(option);
        }
        /// <summary> Add a new array of options definition </summary>
        public void AddOption(ArgsOption[] option)
        {
            OptionsDefinition.AddRange(option);
        }

        /// <summary> Add a new array of commands definition </summary>
        public void AddParameterGroup(string name, ArgsParameter[] param)
        {
            var list = new List<ArgsParameter>();
            list.AddRange(param);
            ParameterDefinition.Add(name, list);
        }


        /// <summary> Retrun number of commands used </summary>
        public int CommandCount()
        {
            return Commands.Count;
        }
        /// <summary> Retrun number of options used </summary>
        public int OptionCount()
        {
            return Options.Count;
        }
        /// <summary> Retrun number of parameters used </summary>
        public int ParameterCount()
        {
            return Parameter.Count;
        }


        /// <summary> Get a command name based on the position </summary>
        public string GetCommand(int pos)
        {
            if(Commands.Count > 0)
            {
                string selected = Commands.Keys.ToArray()[pos];
                return selected;
            }
            else
            {
                return "";
            }
        }


        /// <summary> Get if an option has been used based on his name</summary>
        public bool OptionExist(string name)
        {
            return Options.ContainsKey(name);
        }
        /// <summary> Get an option value based on his name </summary>
        public string GetOption(string name)
        {
            string selected = Options[name];
            return selected;
        }
        /// <summary> Get an option value of a specific type based on his name </summary>
        public T GetOption<T>(string name)
        {
            string selected = Options[name];
            return (T)(object)selected;
        }
        /// <summary> Get an option value based on his name, but check if it exist otherwise return a default value </summary>
        public T LoadOption<T>(string name, T defaultValue)
        {
            if(OptionExist(name))
            {
                string selected = Options[name];
                
                string type = typeof(T).Name;
                
                try
                { 
                    if(type == "Boolean")
                        return (T)(object)bool.Parse(selected);
                    if(type == "Int32")
                        return (T)(object)int.Parse(selected);
                    if(type == "Single")
                        return (T)(object)float.Parse(selected);
                    if(type == "Double")
                        return (T)(object)double.Parse(selected);
                
                    return (T)(object)selected;
                }
                catch(Exception)
                {
                    return defaultValue;
                }
            }
            else
            {
                return defaultValue;
            } 
        }


        /// <summary> Get an option value based on his name </summary>
        public bool ParameterExist(string name)
        {
            return Parameter.ContainsKey(name);
        }
        /// <summary> Get an option value based on his name </summary>
        public string GetParameter(string name)
        {
            string selected = Parameter[name];
            return selected;
        }
        /// <summary> Get an option value based on his name </summary>
        public T GetParameter<T>(string name)
        {
            string selected = Parameter[name];
            return (T)Convert.ChangeType((object)selected, typeof(T));
        }
        
        
        /// <summary> Get a string of all valid parameters from a group </summary>
        public string GetTextParameter(string[] names)
        {
            string str = "";

            for(int i=0; i<names.Length; i++)
            {
                 string currName = names[i];
                 List<ArgsParameter> currList = ParameterDefinition[currName];
                 for(int j=0; j<currList.Count; j++)
                 {
                    ArgsParameter param = currList[j];
                    str += param.Name+", ";
                 }
                 str = str.TrimEnd(' ').TrimEnd(',');
            }

            return str;
        }


        private void ElaborateOption(ArgData Arg, ref int indexArg)
        {
            bool optionFound = false;

            for(int j=0; j<OptionsDefinition.Count; j++)
            {         
                ArgsOption definition = OptionsDefinition[j];
                if(Arg.currName == definition.Name || Arg.currName == definition.Alias)
                {
                    optionFound = true;

                    if(definition.IsFlag == false)
                    {
                        if(Arg.next != null && Arg.nextIsOpt == false)
                        {
                            Options[definition.Name] = Arg.next;
                            indexArg++;
                        }
                        else
                        {
                            PrintError($"value for option '{definition.Name}' not valid");
                        }
                    }
                    else
                    {
                        string value = definition.DefaultValue != null ? definition.DefaultValue : "";
                        Options[definition.Name] = value;
                    }

                }
            }

            if(optionFound == false)
                PrintError($"unknown option '{Arg.curr}'");
        }

        private void ElaborateCommand(ArgData Arg, ref List<string> tempParameter)
        {
            bool commandFound = false;
            for(int j=0; j<CommandsDefinition.Count; j++)
            {
                ArgsCommand definition = CommandsDefinition[j];
                if(Arg.currName == definition.Name)
                {
                    commandFound = true;
                    Commands[definition.Name] = "";
                }
            }

            if(commandFound == false)
                tempParameter.Add(Arg.curr);
        }


        /// <summary> Execute the parsing of the arguments </summary>
        public void Parse(string[] args)
        {
            List<string> tempParameter = new List<string>();

            // loop all the arguments
            int size = args.Length;
            for(int indexArg=0; indexArg<size; indexArg++)
            {
                ArgData Arg = new ArgData()
                {
                    curr = args[indexArg],
                    currName = args[indexArg],
                    next = null,
                    currIsOpt  = false,
                    nextIsOpt  = false
                };

                // check if current is option
                if(Arg.curr.Length > 0)
                {
                    if(IsHelp == false)
                    { 
                        IsHelp = helpFlags.Contains(Arg.curr);
                        if(IsHelp)
                            continue;
                    }
             
                    for(int n=0; n<optionPrefixs.Length; n++)
                    {
                        if(!Arg.currIsOpt)
                            Arg.currIsOpt = Arg.curr.StartsWith(optionPrefixs[n]); 

                        if(Arg.currIsOpt)
                            Arg.currName = Arg.currName.TrimStart(optionPrefixs[n].ToCharArray());
                    }
                }

                // check if next exist and if is option
                if(indexArg+1 < size)
                {
                    Arg.next = args[indexArg+1];

                    if(Arg.next.Length > 0)
                    {
                        for(int n=0; n<optionPrefixs.Length; n++)
                            Arg.nextIsOpt = Arg.next.StartsWith(optionPrefixs[n]);
                    }
                }

                if(Arg.currIsOpt)     
                    ElaborateOption(Arg, ref indexArg);
                else 
                    ElaborateCommand(Arg, ref tempParameter);
            }
   
            

            // loop all passed option and parameter and check if they are valid for the last command
            if(Commands.Count > 0)
            {
                // option check
                string lastCommand = Commands.Keys.ToArray()[Commands.Count - 1];
                ArgsCommand lastCommandDef = null;

                for(int j=0; j<CommandsDefinition.Count; j++)
                {
                    ArgsCommand definition = CommandsDefinition[j];
                    if (lastCommand == definition.Name)
                        lastCommandDef = definition;
                }

                if(IsHelp)
                    PrintHelp(lastCommandDef);

                for(int i=0; i<Options.Count; i++)
                {
                    string currOpt = Options.Keys.ToArray()[i];

                    if (!lastCommandDef.ValidOptions.Contains(currOpt))
                        PrintError($"Option '{currOpt}' not valid for command '{lastCommand}'");
                }    

                // parameter check
                bool paramExist = false;
                int paramCorrect = 0;
                List<ArgsParameter> correctList = new List<ArgsParameter>();

                for(int i=0; i<lastCommandDef.ValidParameters.Count; i++)
                {
                    string paramGroup = lastCommandDef.ValidParameters[i];
                    if(ParameterDefinition.ContainsKey(paramGroup))
                    {
                        paramExist = true;
                        List<ArgsParameter> list = ParameterDefinition[paramGroup];

                        if(allowNoParameter == false)
                        { 
                            if(tempParameter.Count == list.Count)
                            {
                                paramCorrect = 0;
                                correctList = list;
                                break;
                            }
                            if(tempParameter.Count > list.Count)
                                paramCorrect = 1;
                            if(tempParameter.Count < list.Count)
                                paramCorrect = 2;
                        }
                        else
                        {
                            paramCorrect = 0;
                            correctList = list;
                        }
                    }
                }
     
                // print errors if parameters aren't valid
                if(lastCommandDef.ValidParameters.Count > 0 )
                { 
                    if(paramExist == false)
                        PrintError("Parameter group specified not found, check the ArgsCommand definition");
                
                    if(allowNoParameter == false)
                    { 
                        if(paramCorrect == 1)
                        {
                            string validParam = GetTextParameter(lastCommandDef.ValidParameters.ToArray());
                            PrintError("Too many parameters for this command. \nValid parameter sets are:", validParam);
                        }
                    
                        if(paramCorrect == 2)
                        {
                            string validParam = GetTextParameter(lastCommandDef.ValidParameters.ToArray());
                            PrintError("Not enought parameters for this command. \nValid parameter sets are:", validParam);
                        }
                    }
                }
                    

                for(int i=0; i<correctList.Count; i++)
                {
                    if(correctList.Count > i && tempParameter.Count > i)
                    {
                        ArgsParameter param = correctList[i];
                        Parameter[param.Name] = tempParameter[i];
                    }       
                }

                
            }
            else
            {
                if(IsHelp)
                    PrintHelp();

                if(CommandsDefinition.Count > 0 && allowNoCommand == false)
                    PrintError("No valid command specified");
            }
        }


        /// <summary> Print all the commands parsed and their values </summary>
        public void PrintCommands()
        {
            foreach(KeyValuePair<string, string> cmd in Commands)
                Console.WriteLine("Command = {0}, Value = {1}", cmd.Key, cmd.Value);
        }
        /// <summary> Print all the options parsed and their values </summary>
        public void PrintOptions()
        {
            foreach(KeyValuePair<string, string> cmd in Options)
                Console.WriteLine("Option = {0}, Value = {1}", cmd.Key, cmd.Value);
        }
        /// <summary> Print all the parameters parsed and their values </summary>
        public void PrintParameters()
        {
            foreach(KeyValuePair<string, string> cmd in Parameter)
                Console.WriteLine("Parameter = {0}, Value = {1}", cmd.Key, cmd.Value);
        }

        /// <summary> Function of parsing error printing </summary>
        private void PrintError(string error, string message = "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ARGUMENTS ERROR: " + error + "\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message + "\n");
            throw new ArgsParseErrorException();
        }



        private void WriteDefinition(string name, string desc, int paddingSize, int consoleSize)
        {
            Console.ForegroundColor = ConsoleColor.White;
            string cmdName = "    "+name.PadRight(paddingSize+4);
            Console.Write(cmdName);
            Console.ResetColor();

            string cmdDesc = desc;
            List<string> cmdDescPart = new List<string>();
            while(cmdDesc.Length+cmdName.Length > consoleSize)
            {
                if(cmdDesc.Contains("\n") && cmdDesc.IndexOf("\n") <= consoleSize)
                {
                    cmdDescPart.Add(cmdDesc.Substring(0, cmdDesc.IndexOf("\n") ));
                    cmdDesc = cmdDesc.Substring(cmdDesc.IndexOf("\n")+1);
                }
                else
                {
                    cmdDescPart.Add(cmdDesc.Substring(0, consoleSize-cmdName.Length));
                    cmdDesc = cmdDesc.Substring(consoleSize-cmdName.Length);
                }          
            }
            cmdDescPart.Add(cmdDesc);

            for(int o=0; o<cmdDescPart.Count; o++)
                Console.WriteLine( o==0 ? cmdDescPart[o] : new string(' ',cmdName.Length)+cmdDescPart[o].Trim());
        }

        /// <summary>
        /// Write the help message of the last command inserted, if no command is found, print the command list
        /// </summary>
        public void PrintHelp(ArgsCommand lastCommandDef = null)
        {        
            int cmdLongest  = CommandsDefinition.OrderByDescending( s => s.Name.Length ).First().Name.Length;
            int optLongest  = OptionsDefinition.OrderByDescending(  s => s.Name.Length ).First().Name.Length;
            int paddingSize = Math.Max(cmdLongest, optLongest);
            int consoleSize = Console.WindowWidth;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Commands:");
            Console.ResetColor();

            for(int n=0; n<CommandsDefinition.Count; n++)
            {
                ArgsCommand cmdDef = CommandsDefinition[n];

                if(lastCommandDef != null && lastCommandDef.Name != cmdDef.Name)
                    continue;
               
                WriteDefinition(cmdDef.Name, cmdDef.Description, paddingSize, consoleSize);

                if(lastCommandDef != null && cmdDef.ValidOptions.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nOptions:");
                    Console.ResetColor();

                    for(int i=0; i<cmdDef.ValidOptions.Count; i++)
                    {
                        string optName = cmdDef.ValidOptions[i];
                        ArgsOption optDef = OptionsDefinition.Find((e) => e.Name == optName);
                        optName = optDef.Name;
                        /*
                        if(optDef.type == "" && optDef.IsFlag)
                            optName += ": flag"; 
                        
                        if(optDef.type != "")
                            optName += ": "+optDef.type; 
                        */
                        if(optDef != null)
                            WriteDefinition("--"+optName, optDef.Description, paddingSize, consoleSize);       
                    }
                }


                if(lastCommandDef != null && cmdDef.ValidParameters.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nParameters:");
                    Console.ResetColor();

                    for(int i=0; i<cmdDef.ValidParameters.Count; i++)
                    {
                        string parName = cmdDef.ValidParameters[i];
                        List<ArgsParameter> parDef = ParameterDefinition[parName];

                        if(parDef != null)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("    ");Console.WriteLine(""+parName);
                            Console.ResetColor();
                            Console.Write(new String(' ',paddingSize+8));Console.WriteLine(GetTextParameter(new string[] {parName})+"\n");
                        }
                    }
                }
            }

            Console.WriteLine("");
            Environment.Exit(0);  
        }
    }

    struct ArgData
    {
        public string curr;
        public string currName;
        public string next;
        public bool currIsHelp;
        public bool currIsOpt;
        public bool nextIsOpt;
    }
}


