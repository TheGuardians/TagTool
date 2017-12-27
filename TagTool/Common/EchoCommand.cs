using BlamCore.Commands;
using System;
using System.Collections.Generic;

namespace TagTool.Common
{
    class EchoCommand : Command
    {
        public EchoCommand()
            : base(CommandFlags.Inherit,

                  "Echo",
                  "Prints arguments to the console.",

                  "Echo <arg1> <arg2> ... <argN>",

                  "Prints arguments to the console.")
        {
        }

        public override object Execute(List<string> args)
        {
            foreach (var arg in args)
                Console.Write($"{arg} ");
            
            return true;
        }
    }
}
