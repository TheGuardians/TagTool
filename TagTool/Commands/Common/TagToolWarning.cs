using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Commands.Common
{
    class TagToolWarning
    {
        private static readonly object Mutex = new object();

        public TagToolWarning(string customMessage = null)
        {
            lock (Mutex)
            {
                // if we're not at the start of the line, insert a new one to avoid ugliness with Console.Write()
                if (Console.LargestWindowWidth != 0 && Console.CursorLeft > 0)
                    Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("WARNING: " + customMessage);
                Console.ResetColor();

                Program.WarningCount += 1;
            }
        }
    }
}
