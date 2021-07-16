using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TagTool.IO;

namespace TagTool.Commands.Common
{
    public class CommandRunner
    {
        public CommandContextStack ContextStack;
        public bool EOF { get; private set; } = false;
        public static string CommandLine;

        public CommandRunner(CommandContextStack contextStack)
        {
            ContextStack = contextStack;
        }

        private string PreprocessCommandLine(string commandLine)
        {
            // Evaluate c# expresisons

            commandLine = ExecuteCSharpCommand.EvaluateInlineExpressions(commandLine);
            if (commandLine == null)
                return null;

            return commandLine;
        }

        public void RunCommand(string commandLine, bool printInput)
        {
            if (commandLine == null)
            {
                EOF = true;
                return;
            }

            CommandLine = commandLine = PreprocessCommandLine(commandLine);
            if (commandLine == null)
                return;

            if (printInput)
                Console.WriteLine(commandLine);

            var commandArgs = ArgumentParser.ParseCommand(commandLine, out string redirectFile);
            if (commandArgs.Count == 0)
                return;

            if (commandArgs[0] == "cs")
                redirectFile = null;

            switch (commandArgs[0].ToLower())
            {
                case "quit":
                    EOF = true;
                    return;
                case "exit":
                    if (ContextStack.IsBase())
                        Console.WriteLine("Cannot exit, already at base context! Use 'quit' to quit tagtool.");
                    else
                        ContextStack.Pop();
                    return;
            }

            if (commandArgs[0].StartsWith("#"))
                return; // ignore comments

            // Handle redirection
            var oldOut = Console.Out;
            StreamWriter redirectWriter = null;
            if (redirectFile != null)
            {
                redirectWriter = new StreamWriter(File.Open(redirectFile, FileMode.Create, FileAccess.Write));
                Console.SetOut(redirectWriter);
            }

            // Try to execute it
            if (!ExecuteCommand(ContextStack.Context, commandArgs, ContextStack.ArgumentVariables))
            {
                new TagToolError(CommandError.CustomError, $"Unrecognized command \"{commandArgs[0]}\"\n"
                + "Use \"help\" to list available commands.");
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

        private static bool ExecuteCommand(CommandContext context, List<string> commandAndArgs, Dictionary<string, string> argVariables)
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

            // Replace argument variables with their values
            if (!command.IgnoreArgumentVariables)
            {
                for (int i = 0; i < commandAndArgs.Count; i++)
                {
                    foreach (var variable in argVariables)
                    {
                        commandAndArgs[i] = commandAndArgs[i].Replace(variable.Key, variable.Value);
                    }
                }
            }

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
                new TagToolError(CommandError.CustomError, e.Message);
                Console.WriteLine("STACKTRACE: " + Environment.NewLine + e.StackTrace);
                ConsoleHistory.Dump("hott_*_crash.log");
            }
#endif

            return true;
        }
    }
}
