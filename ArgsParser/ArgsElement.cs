﻿
using System;
using System.Collections.Generic;

namespace UtilityPack
{
    /// <summary> Argument Command definition </summary>
    public class ArgsCommand
    {
        /// <summary> Command Name </summary>
        public string Name;
        /// <summary> List of valid options for this command </summary>
        public List<string> ValidOptions = new List<string>();
        /// <summary> List of valid parameters group for this command </summary>
        public List<string> ValidParameters = new List<string>();

        /// <summary> Argument Command definition </summary>
        public ArgsCommand(string name)
        {
            Name = name;
        }
    }


    /// <summary> Argument Option definition </summary>
    public class ArgsOption
    {
        /// <summary> Option Name </summary>
        public string Name;
        /// <summary> If true no additional data after the option is required </summary>
        public bool IsFlag = false;
        /// <summary> Another name to use instead of <see cref="Name"/> </summary>
        public string Alias = null;
        /// <summary> If no value is passed this value is used instead </summary>
        public string DefaultValue = "";

        /// <summary> Argument Option definition </summary>
        public ArgsOption(string name)
        {
            Name = name;
        }
    }

    /// <summary> Argument Parameter definition </summary>
    public class ArgsParameter
    {
        /// <summary> Name of the parameter, to retrive it after command parsing </summary>
        public string Name;

        /// <summary> Argument Parameter definition </summary>
        public ArgsParameter(string name)
        {
            Name = name;
        }
    }
}


