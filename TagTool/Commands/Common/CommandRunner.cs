using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Commands.Common
{
    class CommandRunner
    {
        public CommandContextStack ContextStack;
        public bool EOF { get; private set; } = false;

        public CommandRunner(CommandContextStack contextStack)
        {
            ContextStack = contextStack;
        }

        public void RunCommand(string commandLine)
        {
            var commandArgs = ArgumentParser.ParseCommand(commandLine, out string redirectFile);
            if (commandArgs.Count == 0)
                return;

            switch (commandArgs[0].ToLower())
            {
                case "quit":
                    EOF = true;
                    return;
                case "exit":
                    if (!ContextStack.Pop())
                        EOF = true;
                    return;
            }

            // Handle redirection
            var oldOut = Console.Out;
            StreamWriter redirectWriter = null;
            if (redirectFile != null)
            {
                redirectWriter = new StreamWriter(File.Open(redirectFile, FileMode.Create, FileAccess.Write));
                Console.SetOut(redirectWriter);
            }

            // Try to execute it
            if (!ExecuteCommand(ContextStack.Context, commandArgs))
            {
                Console.WriteLine("Unrecognized command: {0}", commandArgs[0]);
                Console.WriteLine("Use \"help\" to list available commands.");
            }

            // Undo redirection
            if (redirectFile != null)
            {
                Console.SetOut(oldOut);
                redirectWriter.Dispose();
                Console.WriteLine("Wrote output to {0}.", redirectFile);
            }
        }

        private static bool ExecuteCommand(CommandContext context, List<string> commandAndArgs)
        {
            if (commandAndArgs.Count == 0)
                return true;

            // Look up the command
            var command = context.GetCommand(commandAndArgs[0]);
            if (command == null)
                if ((command = context.GetCommand(commandAndArgs[0].ToLower())) == null)
                    return false;

            // Execute it
            commandAndArgs.RemoveAt(0);

#if !DEBUG
            try
            {
#endif
            if (!command.Execute(commandAndArgs).Equals(true))
            {
                Console.WriteLine("{0}: {1}", command.Name, command.Description);
                Console.WriteLine();
                Console.WriteLine("Usage:");
                Console.WriteLine("{0}", command.Usage);
                Console.WriteLine();
                Console.WriteLine("Use \"help {0}\" for more information.", command.Name);
            }
#if !DEBUG
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
                Console.WriteLine("STACKTRACE: " + Environment.NewLine + e.StackTrace);
                ConsoleHistory.Dump("hott_*_crash.log");
            }
#endif

            return true;
        }
    }
}
