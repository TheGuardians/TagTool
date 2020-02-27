using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Commands.Common
{
    class RunCommands : Command
    {
        private CommandContextStack ContextStack;

        public RunCommands(CommandContextStack contextStack)
            : base(true,

                  "RunCommands",
                  "Run commands from a file.",

                  "RunCommands <file>",

                  "Run commands from a file.")
        {
            ContextStack = contextStack;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1)
                return false;

            var commandRunner = new CommandRunner(ContextStack);

            using (var stream = File.OpenText(args[0]))
            {
               for(string line; (line = stream.ReadLine()) != null;)
                    commandRunner.RunCommand(line);
            }

            return true;
        }
    }
}
