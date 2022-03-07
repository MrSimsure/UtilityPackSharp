using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityPack
{
    /// <summary>
    /// Static class to read and write data from the windows registry
    /// </summary>
    public static class RegistryManager
    {
        /// <summary>
        /// Read a value from a specific section of the registry
        /// </summary>
        public static string Read(RegistryHive root, string key, string subKey)
        {
            RegistryKey baseRegistryKey = RegistryKey.OpenBaseKey(root, RegistryView.Registry32);
            RegistryKey subRegistryKey = baseRegistryKey.OpenSubKey(key, RegistryKeyPermissionCheck.ReadSubTree);
            if (subRegistryKey != null)
            {
                object value = subRegistryKey.GetValue(subKey);
                if (value != null)
                {
                    baseRegistryKey.Close();
                    subRegistryKey.Close();
                    return value.ToString();
                }
                subRegistryKey.Close();
            }            
            baseRegistryKey.Close();

            return "";
        }
    }
}
