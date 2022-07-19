using System;
using System.Collections.Generic;

namespace UtilityPack.ArgsParser
{
    
    /// <summary> Argument Command definition </summary>
    public class ArgsCommand
    {
        /// <summary> Command Name </summary>
        public string Name;
        /// <summary> Command description, shown when the -help option is used </summary>
        public string Description;
        /// <summary> List of valid options for this command </summary>
        public List<string> ValidOptions = new List<string>();
        /// <summary> List of valid parameters group for this command </summary>
        public List<string> ValidParameters = new List<string>();

        /// <summary> Argument Command definition </summary>
        public ArgsCommand(string name = null)
        {
            if(name != null)
                Name = name;
        }
    }
   


    /// <summary> Argument Option definition </summary>
    public class ArgsOption
    {
        /// <summary> Option Name </summary>
        public string Name;
        /// <summary> Option description, shown when the -help option is used </summary>
        public string Description;
        /// <summary> If true no additional data after the option is required </summary>
        public bool IsFlag = false;
        /// <summary> Another name to use instead of <see cref="Name"/> </summary>
        public string Alias = null;
        /// <summary> Type of this option, if blank and <see cref="IsFlag"/> is true, it will be of type 'flag' </summary>
        public string type = "";
        /// <summary> If no value is passed this value is used instead </summary>
        public string DefaultValue = "";

        /// <summary> Argument Option definition </summary>
        public ArgsOption(string name = null)
        {
            if(name != null)
                Name = name;
        }
    }

    /// <summary> Argument Parameter definition </summary>
    public class ArgsParameter
    {
        /// <summary> Name of the parameter, to retrive it after command parsing </summary>
        public string Name;
        /// <summary> Parameter description, shown when the -help option is used </summary>
        public string Description;
        /// <summary> Type of this argument </summary>
        public string type = "";
        /// <summary> Argument Parameter definition </summary>
        public ArgsParameter(string name = null)
        {
            if(name != null)
                Name = name;
        }
    }
}


