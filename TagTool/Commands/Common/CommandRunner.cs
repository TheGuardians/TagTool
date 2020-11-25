using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.IO;

namespace TagTool.Commands.Common
{
    public class CommandRunner
    {
        public CommandContextStack ContextStack;
        public bool EOF { get; private set; } = false;

        public CommandRunner(CommandContextStack contextStack)
        {
            ContextStack = contextStack;
        }

        public void RunCommand(string commandLine, bool printInput)
        {
            if (commandLine == null)
            {
                EOF = true;
                return;
            }

            if (printInput)
                Console.WriteLine(commandLine);

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
                        Console.WriteLine("Cannot exit, already at base context! Use 'quit' to quit tagtool.");
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
                Console.WriteLine("ERROR: Unrecognized command \"{0}\"", commandArgs[0]);
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

        public static string CurrentCommandName = "";

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
            CurrentCommandName = command.Name;
            command.Execute(commandAndArgs);
            CurrentCommandName = "";
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
