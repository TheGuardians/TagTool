using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Commands.Common
{
    class TagToolWarning
    {
        public TagToolWarning(string customMessage = null)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("WARNING: " + customMessage);
            Console.ResetColor();
        }
    }
}
