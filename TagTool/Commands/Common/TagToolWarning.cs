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
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("WARNING: " + customMessage);
                Console.ResetColor();
            }
        }
    }
}
