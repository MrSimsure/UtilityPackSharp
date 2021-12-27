
using System;
using System.Collections.Generic;
using System.Linq;

namespace UtilityPack
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

        /// <summary> If set to true will not print an error if no command is used </summary>
        public bool allowNoCommand = false;



        /// <summary> Add a new command definition </summary>
        public void AddCommand(ArgsCommand command)
        {
            CommandsDefinition.Add(command);
        }
        /// <summary> Add a new array of commands definition </summary>
        public void AddCommand(ArgsCommand[] command)
        {
            CommandsDefinition.AddRange(command);
        }

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
            return (T)(object)selected;
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
                 str += "\n\n";
            }

            return str;
        }


        /// <summary> Execute the parsing of the arguments </summary>
        public void Parse(string[] args)
        {
            List<string>  tempParameter = new List<string>();

            //loop all the arguments
            int size = args.Length;
            for (int i=0; i<size; i++)
            {
                //detect argument type
                string curr = args[i];
                string currName = curr.Replace("-", "");
                bool currIsOpt = false;

                if(curr.Length > 0)
                        currIsOpt = curr[0] == '-';

                string next = null;
                bool nextIsOpt = false;

                if (i+1 < size)
                {
                    next = args[i + 1];

                    if(next.Length > 0)
                        nextIsOpt = next[0] == '-';
                }

                //if is an option
                if (currIsOpt)
                {       
                    bool optionFound = false;
                    for (int j = 0; j < OptionsDefinition.Count; j++)
                    {         
                        ArgsOption definition = OptionsDefinition[j];
                        if (currName == definition.Name || currName == definition.Alias)
                        {
                            optionFound = true;

                            if (definition.IsFlag == false)
                            {
                                if (next != null && nextIsOpt == false)
                                {
                                    Options[definition.Name] = next;

                                    i++;
                                }
                                else
                                {
                                    PrintError($"value for option '{definition.Name}' not valid");
                                }
                            }
                            else
                            {
                                string value = "";

                                if (definition.DefaultValue != null)
                                    value = definition.DefaultValue;

                                Options[definition.Name] = value;
                            }

                        }
                    }

                    if (optionFound == false)
                        PrintError($"unknown option '{curr}'");
                }
                else  //if is a command or a parameter
                {    
                    bool commandFound = false;
                    for (int j = 0; j < CommandsDefinition.Count; j++)
                    {
                        ArgsCommand definition = CommandsDefinition[j];
                        if (currName == definition.Name)
                        {
                            commandFound = true;
                            Commands[definition.Name] = "";
                        }
                    }

                    if (commandFound == false)
                    {
                        tempParameter.Add(curr);
                    }
                }
            }
   
            //loop all passed option and parameter and check if they are valid for the last command
            if(Commands.Count > 0)
            {
                //option check
                string lastCommand = Commands.Keys.ToArray()[Commands.Count - 1];
                ArgsCommand lastCommandDef = null;

                for (int j = 0; j < CommandsDefinition.Count; j++)
                {
                    ArgsCommand definition = CommandsDefinition[j];
                    if (lastCommand == definition.Name)
                        lastCommandDef = definition;
                }

                for (int i = 0; i < Options.Count; i++)
                {
                    string currOpt = Options.Keys.ToArray()[i];

                    if (!lastCommandDef.ValidOptions.Contains(currOpt))
                        PrintError($"Option '{currOpt}' not valid for command '{lastCommand}'");
                }    

                //parameter check
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
                }
     
                if(lastCommandDef.ValidParameters.Count > 0 )
                { 
                    if(paramExist == false)
                        PrintError("Parameter group specified not found, check the ArgsCommand definition");
                
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
                    

                for(int i=0; i<correctList.Count; i++)
                {
                    ArgsParameter param = correctList[i];

                    Parameter[param.Name] = tempParameter[i];
                }
            }
            else
            {
                if(CommandsDefinition.Count > 0 && allowNoCommand == false)
                    PrintError("No valid command specified");
            }
        }


        /// <summary> Print all the commands parsed and their values </summary>
        public void PrintCommands()
        {
            foreach (KeyValuePair<string, string> cmd in Commands)
                Console.WriteLine("Command = {0}, Value = {1}", cmd.Key, cmd.Value);
        }
        /// <summary> Print all the options parsed and their values </summary>
        public void PrintOptions()
        {
            foreach (KeyValuePair<string, string> cmd in Options)
                Console.WriteLine("Option = {0}, Value = {1}", cmd.Key, cmd.Value);
        }
        /// <summary> Print all the parameters parsed and their values </summary>
        public void PrintParameters()
        {
            foreach (KeyValuePair<string, string> cmd in Parameter)
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
    }
}
