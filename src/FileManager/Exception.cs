using System;
using System.Collections.Generic;
using System.Text;

namespace FileManager
{ 
    /// <summary> Exception thrown when a ini file dosen't contain a section or a key </summary>
    [Serializable]
    public class IniValueNotFoundException : Exception
    {
        /// <summary> Exception thrown when a ini file dosen't contain a section or a key </summary>
        public IniValueNotFoundException() {}
        /// <summary> Exception thrown when a ini file dosen't contain a section or a key </summary>
        public IniValueNotFoundException(string message) : base(message) {}
        /// <summary> Exception thrown when a ini file dosen't contain a section or a key </summary>
        public IniValueNotFoundException(string message, Exception inner) : base(message, inner) {}
    }
}