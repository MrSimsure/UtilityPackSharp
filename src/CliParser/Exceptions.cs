using System;


namespace UtilityPack
{
    /// <summary> Eccezzione lanciata quando avviene un errore nel parsing degli argomenti </summary>
    [Serializable]
    public class ArgsParseErrorException : Exception
    {
        /// <summary> Eccezzione lanciata quando avviene un errore nel parsing degli argomenti </summary>
        public ArgsParseErrorException()
        {
        }

        /// <summary> Eccezzione lanciata quando avviene un errore nel parsing degli argomenti </summary>
        public ArgsParseErrorException(string message) : base(message)
        {
        }

        /// <summary> Eccezzione lanciata quando avviene un errore nel parsing degli argomenti </summary>
        public ArgsParseErrorException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
