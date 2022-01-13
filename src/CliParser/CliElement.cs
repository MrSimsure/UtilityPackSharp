using System.Collections.Generic;

namespace UtilityPack
{
    public class options
    {
        public CliOption path;
    }


    public class command
    {
        public CliCommand export_full;
        public CliCommand export_delta;
        public CliCommand import;
    }



    /// <summary> Argument Command definition </summary>
    public class CliCommand
    {
        /// <summary> List of valid options for this command </summary>
        public List<CliOption> ValidOptions = new List<CliOption>();
        /// <summary> List of valid parameters group for this command </summary>
        public List<CliParameter> ValidParameters = new List<CliParameter>();
    }


    /// <summary> Argument Option definition </summary>
    public class CliOption
    {
        /// <summary> Option Name </summary>
        public string Name;
        /// <summary> If true no additional data after the option is required </summary>
        public bool   IsFlag = false;
        /// <summary> Another name to use instead of <see cref="Name"/> </summary>
        public string Alias = null;
        /// <summary> If no value is passed this value is used instead </summary>
        public string DefaultValue = "";
    }

    /// <summary> Argument Parameter definition </summary>
    public class CliParameter
    {
        /// <summary> Name of the parameter, to retrive it after command parsing </summary>
        public string Name;
    }
}


