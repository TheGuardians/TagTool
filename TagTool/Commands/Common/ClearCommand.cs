using TagTool.Commands;
using System;
using System.Collections.Generic;

namespace TagTool.Commands.Common
{
    class ClearCommand : Command
    {
        public ClearCommand()
            : base(CommandFlags.Inherit,
                  
                  "Clear",
                  "Clears the screen of all output",

                  "Clear",

                  "Clears the screen of all output.")
        {
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 0)
                return false;

            Console.Clear();

            return true;
        }
    }
}