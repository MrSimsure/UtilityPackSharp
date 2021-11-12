using System;
using System.Drawing;

namespace UtilityPack
{
    /// <summary> Class with static functions for printing in console or gui with different colors based on the type of function used </summary>
    public static class Print
    {
        /// <summary> If true, the messages will not be written to the console but to gui through the function <see cref="GuiWriteFunction"/> </summary>
        public static bool IsGUI   = false;
        /// <summary> If false, no messages will be written when the printing functions are called </summary>
        public static bool IsVerbose = true;
        /// <summary> If false, no debugging messages will be written when the function <see cref="Debug(object)"/> is called</summary>
        public static bool IsDebug   = false;

        /// <summary> Function called when writing to gui instead of console </summary>
        public static Action<Color, string, bool> GuiWriteFunction;



        /// <summary> Write a red error message </summary>
        public static void Error(object text, bool line = true)
        {
            if (IsVerbose)
            {
                if (IsGUI)
                {
                    GuiWriteFunction(Color.Red, PrintParse(text), line);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    if(line)
                        Console.WriteLine(PrintParse(text));
                    else
                        Console.Write(PrintParse(text));
                    Console.ForegroundColor = ConsoleColor.White;
                }

            }
        }

        /// <summary> Write a yellow warning message </summary>
        public static void Warning(object text, bool line = true)
        {
            if (IsVerbose)
            {
                if (IsGUI)
                {
                    GuiWriteFunction(Color.Yellow, PrintParse(text),line);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if(line)
                       Console.WriteLine(PrintParse(text));
                    else
                        Console.Write(PrintParse(text));
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        /// <summary> Write a green success message </summary>
        public static void Success(object text, bool line = true)
        {
            if (IsVerbose)
            {
                if (IsGUI)
                {
                    GuiWriteFunction(Color.Green, PrintParse(text),line);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    if(line)
                       Console.WriteLine(PrintParse(text));
                    else
                        Console.Write(PrintParse(text));
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        /// <summary> Write a gray note message </summary>
        public static void Note(object text, bool line = true)
        {
            if (IsVerbose)
            {
                if (IsGUI)
                {
                    GuiWriteFunction(Color.Gray, PrintParse(text),line);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if(line)
                       Console.WriteLine(PrintParse(text));
                    else
                        Console.Write(PrintParse(text));
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        /// <summary> Write a plain white message </summary>
        public static void Message(object text, bool line = true)
        {
            if (IsVerbose)
            {
                if (IsGUI)
                {
                    GuiWriteFunction(Color.White, PrintParse(text),line);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    if(line)
                       Console.WriteLine(PrintParse(text));
                    else
                        Console.Write(PrintParse(text));
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        /// <summary> Write a blue debug message </summary>
        public static void Debug(object text, bool line = true)
        {
            if (IsDebug)
            {
                if (IsGUI)
                {
                    GuiWriteFunction(Color.Blue, PrintParse(text),line);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    if(line)
                       Console.WriteLine(PrintParse(text));
                    else
                        Console.Write(PrintParse(text));
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        /// <summary> Convert the incoming values to be compatible with the terminal </summary>
        private static string PrintParse(object text)
        {
            if (text is int)
                return text.ToString();
            if (text is double)
                return text.ToString();
            if (text is string)
                return (string)text;
            if (text is float)
                return text.ToString();
            if (text is bool)
            {
                bool val = (bool)text;

                if(val == true)
                    return "true";
                else
                    return "false";
            }

            return (string)text;
        }
    }
}
