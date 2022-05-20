using System.IO;


namespace UtilityPack.Extensions
{
    /// <summary>
    /// Extension class for extended directory operations
    /// </summary>
    public static class DirectoryExt 
    {
        /// <summary>
        /// Remove every file from a folder. <br/>
        /// If <paramref name="isRecursive"/> is true, delete also every file in sub-folder, and then remove the empty sub-folder.
        /// </summary>
        public static void ClearFolder(string path, bool isRecursive = false)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            foreach(FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }

            if(isRecursive)
            {
                foreach (DirectoryInfo di in dir.GetDirectories())
                {
                    ClearFolder(di.FullName);
                    di.Delete();
                }
            }
            
        }
    }
}
