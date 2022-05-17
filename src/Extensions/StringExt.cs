using System;

namespace UtilityPack.Extensions
{
    /// <summary>
    /// Add some extension methods to the string class
    /// </summary>
    public static class StringEXtension
    {
        /// <summary>
        /// Cut a specific ammount of text
        /// </summary>
        public static string Cut(this string txt, int end)
        {
            return txt.Substring(0, Math.Min(end, txt.Length));
        }

        /// <summary>
        /// Cut a specific ammount of text in a range
        /// </summary>
        public static string Cut(this string text, int start, int end)
        {
            if(start > text.Length)
			    return "";

            int len = end;

		    if(end-start > 0)
			    len = end-start;

            if(start+len > text.Length)
                len = text.Length-start;

            return text.Substring(start, len);
        }
    }
}
