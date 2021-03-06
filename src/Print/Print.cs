using System;
using System.Drawing;

namespace UtilityPack.Print
{
    /// <summary> Class with static functions for printing in console or gui with different colors based on the type of function used </summary>
    public static class Print
    {
        /// <summary> If true, the messages will not be written to the console but to gui through the function <see cref="GuiWriteFunction"/> <br/>(Default false)</summary>
        public static bool IsGUI     = false;
        /// <summary> If false, no messages will be written when the printing functions are called <br/>(Default true) </summary>
        public static bool IsActive  = true;
        /// <summary> If false, no verbose message will be written when the printing functions are called <br/>(Default false) </summary>
        public static bool IsVerbose = false;
        /// <summary> If false, no debugging messages will be written when the function <see cref="Debug"/> or  <see cref="DebugVerb"/> is called <br/>(Default false)</summary>
        public static bool IsDebug   = false;

        /// <summary> Function called when writing to gui instead of console </summary>
        public static Action<Color, string, bool> GuiWriteFunction;


        /// <summary> Write a plain WHITE message. <br/>If 'line' is true go to new line.  </summary>
        public static void Message(object text, bool line = true)
        {
            if(IsActive)
            {
                if(IsGUI)
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

        /// <summary> Write a plain BLACK message. <br/>If 'line' is true go to new line.  </summary>
        public static void Text(object text, bool line = true)
        {
            if(IsActive)
            {
                if(IsGUI)
                {
                    GuiWriteFunction(Color.Black, PrintParse(text),line);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    if(line)
                       Console.WriteLine(PrintParse(text));
                    else
                        Console.Write(PrintParse(text));
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        /// <summary> Write a RED error message. <br/>If 'line' is true go to new line. </summary>
        public static void Error(object text, bool line = true)
        {
            if(IsActive)
            {
                if(IsGUI)
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

        /// <summary> Write a YELLOW warning message. <br/>If 'line' is true go to new line.  </summary>
        public static void Warning(object text, bool line = true)
        {
            if(IsActive)
            {
                if(IsGUI)
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

        /// <summary> Write a GREEN success message. <br/>If 'line' is true go to new line.  </summary>
        public static void Success(object text, bool line = true)
        {
            if(IsActive)
            {
                if(IsGUI)
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

        /// <summary> Write a GRAY note message. <br/>If 'line' is true go to new line.  </summary>
        public static void Note(object text, bool line = true)
        {
            if(IsActive)
            {
                if(IsGUI)
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
  
        /// <summary> Write a CYAN debug message only when <see cref="IsDebug"/> is true. <br/>If 'line' is true go to new line. </summary>
        public static void Debug(object text, bool line = true)
        {
            if(IsActive && IsDebug)
            {
                if(IsGUI)
                {
                    GuiWriteFunction(Color.Cyan, PrintParse(text),line);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    if(line)
                       Console.WriteLine(PrintParse(text));
                    else
                        Console.Write(PrintParse(text));
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
        

        /// <summary> Write a separation line made by a number of segments "----" </summary>
        public static void Separator(int segments)
        {
            if(IsActive)
            {
                string lineText = "";

                for(int i=0; i<segments; i++)
                    lineText += "-";

                if(IsGUI)
                {
                    GuiWriteFunction(Color.White, lineText, true);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
            
                    Console.WriteLine(lineText);
        
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }

        /// <summary> Write a number of new line </summary>
        public static void NewLine(int space = 1)
        {
            if(IsActive)
            {
                string lineText = "";

                for(int i=0; i<space; i++)
                    lineText += Environment.NewLine;

                if(IsGUI)
                {
                    GuiWriteFunction(Color.White, lineText, true);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
            
                    Console.WriteLine(lineText);
        
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }


        /// <summary> Write a plain WHITE message, only when <see cref="IsVerbose"/> is true. <br/>If 'line' is true go to new line.  </summary>
        public static void MessageVerb(object text, bool line = true)
        {
            if(IsVerbose)
                Message(text, line);
        }

        /// <summary> Write a plain BLACK message, only when <see cref="IsVerbose"/> is true. <br/>If 'line' is true go to new line.  </summary>
        public static void TextVerb(object text, bool line = true)
        {
            if(IsVerbose)
                Text(text, line);
        }

        /// <summary> Write a RED error message, only when <see cref="IsVerbose"/> is true. <br/>If 'line' is true go to new line. </summary>
        public static void ErrorVerb(object text, bool line = true)
        {
            if(IsVerbose)
                Error(text, line);
        }

        /// <summary> Write a YELLOW warning message, only when <see cref="IsVerbose"/> is true. <br/>If 'line' is true go to new line.  </summary>
        public static void WarningVerb(object text, bool line = true)
        {
            if(IsVerbose)
                Warning(text, line);
        }

        /// <summary> Write a GREEN success message, only when <see cref="IsVerbose"/> is true. <br/>If 'line' is true go to new line.  </summary>
        public static void SuccessVerb(object text, bool line = true)
        {
            if(IsVerbose)
                Success(text, line);
        }

        /// <summary> Write a GRAY note message, only when <see cref="IsVerbose"/> is true. <br/>If 'line' is true go to new line.  </summary>
        public static void NoteVerb(object text, bool line = true)
        {
            if(IsVerbose)
                Note(text, line);
        }

        /// <summary> Write a CYAN debug message, only when <see cref="IsDebug"/> is true and <see cref="IsVerbose"/> is true. <br/>If 'line' is true go to new line. </summary>
        public static void DebugVerb(object text, bool line = true)
        {
            if(IsVerbose)
                Debug(text, line);
        }


        /// <summary> Write a separation line made by a number of segments "----", only when <see cref="IsVerbose"/> is true </summary>
        public static void SeparatorVerb(int segments)
        {
            if(IsVerbose)
                Separator(segments);
        }

        /// <summary> Write a number of new line, only when <see cref="IsVerbose"/> is true </summary>
        public static void NewLineVerb(int space = 1)
        {
            if(IsVerbose)
                NewLine(space);
        }

        /// <summary> Convert the incoming values to be compatible with the terminal </summary>
        private static string PrintParse(object text)
        {
            return (string)Convert.ChangeType(text, typeof(string));
        }
    }
}
