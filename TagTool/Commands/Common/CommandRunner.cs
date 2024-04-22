using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        [ThreadStatic] public static string CommandLine;
        [ThreadStatic] public static CommandRunner Current;

        public CommandRunner(CommandContextStack contextStack)
        {
            ContextStack = contextStack;
        }

        private string PreprocessCommandLine(string commandLine)
        {
            // Evaluate c# expressions

            commandLine = ExecuteCSharpCommand.EvaluateInlineExpressions(ContextStack, commandLine);
            if (commandLine == null)
                return null;

            // Allow inline comments beginning with "//"

            commandLine = commandLine.Split(new[] {"//"}, StringSplitOptions.None)[0];

            return commandLine;
        }

        public void RunCommand(string commandLine, bool printInput = false, bool printOutput = true)
        {
            if (commandLine == null)
            {
                EOF = true;
                return;
            }

            Current = this;
            CommandLine = commandLine = PreprocessCommandLine(commandLine);
            if (commandLine == null)
                return;

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
                    if (ContextStack.IsBase())
                        Console.WriteLine("Cannot exit, already at base context! Use 'quit' to quit tagtool.");
                    else if (ContextStack.IsModPackage())
                        new TagToolWarning("Use 'exitmodpackage' to leave a mod package context.");
                    else
                        ContextStack.Pop();
                    return;
                case "exitmodpackage":
                    {
                        if (ContextStack.IsModPackage())
                            ContextStack.Pop();
                        else
                            new TagToolWarning("Use 'exit' to leave standard contexts.");
                    }
                    return;
                case "cs" when !ExecuteCSharpCommand.OutputIsRedirectable(commandArgs.Skip(1).ToList()):
                    redirectFile = null;
                    break;
            }

            if (commandArgs[0].StartsWith("#") || commandArgs[0].StartsWith($"//"))
                return; // ignore comments

            // Handle redirection
            var oldOut = Console.Out;
            StreamWriter redirectWriter = null;
            if (redirectFile != null || !printOutput)
            {
                redirectWriter = !printOutput ? StreamWriter.Null : new StreamWriter(File.Open(redirectFile, FileMode.Create, FileAccess.Write));
                Console.SetOut(redirectWriter);
            }

            // Try to execute it
            if (!ExecuteCommand(ContextStack.Context, commandArgs, ContextStack.ArgumentVariables))
            {
                new TagToolError(CommandError.CustomError, $"Unrecognized command \"{commandArgs[0]}\"\n"
                + "Use \"help\" to list available commands.");
            }

            // Undo redirection
            if (redirectFile != null || !printOutput)
            {
                Console.SetOut(oldOut);
                redirectWriter.Dispose();
                if (redirectFile != null)
                    Console.WriteLine("Wrote output to {0}.", redirectFile);
            }
        }

        public static string ApplyUserVars(string inputStr, bool ignoreArgumentVariables)
        {
            if (!ignoreArgumentVariables)
            {
                foreach (var variable in Current.ContextStack.ArgumentVariables)
                {
                    inputStr = inputStr.Replace(variable.Key, variable.Value);
                }
            }
            return inputStr;
        }

        public static string CurrentCommandName = "";

        private static bool ExecuteCommand(CommandContext context, List<string> commandAndArgs, Dictionary<string, string> argVariables)
        {
            if (commandAndArgs.Count == 0)
                return true;

            // Look up the command
            Command command;
            if ((command = context.GetCommand(commandAndArgs[0])) == null && (command = context.GetCommand(commandAndArgs[0].ToLower())) == null)
            {
                var tagGroup = Path.GetExtension(context.Name).Replace(".", "");
                var fileName = commandAndArgs[0].ToLower() + ".cs";
                var filePath = Path.Combine(Program.TagToolDirectory, "scripts", fileName);
                var fileContextPath = Path.Combine(Program.TagToolDirectory, "scripts", tagGroup, fileName);
                string validPath = File.Exists(fileContextPath) ? fileContextPath : File.Exists(filePath) ? filePath : "";
                if (validPath != "")
                {
                    command = context.GetCommand("cs");
                    commandAndArgs.InsertRange(1, new string[] { "<", validPath });
                }
                else return false;
            }

            // Execute it
            commandAndArgs.RemoveAt(0);

            // Replace argument variables with their values
            for (int i = 0; i < commandAndArgs.Count; i++)
                commandAndArgs[i] = ApplyUserVars(commandAndArgs[i], command.IgnoreArgumentVariables);


            if (Debugger.IsAttached)
            {
                CurrentCommandName = command.Name;
                command.Execute(commandAndArgs);
                CurrentCommandName = "";
            }
            else
            {
                try
                {
                    CurrentCommandName = command.Name;
                    command.Execute(commandAndArgs);
                    CurrentCommandName = "";
                }
                catch (Exception e)
                {
                    new TagToolError(CommandError.CustomError, e.Message);
                    Console.WriteLine("STACKTRACE: " + Environment.NewLine + e.StackTrace);
                    ConsoleHistory.Dump("hott_*_crash.log");
                }
            }
            return true;
        }
    }
}
