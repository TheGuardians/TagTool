using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TagTool.Commands.Common
{
    class HelpCommand : Command
    {
        private CommandContextStack ContextStack { get; }
 
        public HelpCommand(CommandContextStack contextStack)
            : base(true,

                  "Help",
                  "Display help",

                  "Help [command]",

                  "Displays help on how to use a command.\n" +
                  "If no command is given, help will list all available commands.")
        {
            ContextStack = contextStack;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 1)
                return new TagToolError(CommandError.ArgCount);
            if (args.Count == 1)
                DisplayCommandHelp(args[0]);
            else
                ListCommands();
            return true;
        }

        private void ListCommands()
        {
            ListCommands(ContextStack.Context, new HashSet<string>());
            Console.WriteLine("Use \"help <command>\" for more information on how to use a command.");
            Console.WriteLine();
            Console.WriteLine("To write the output of a command to a file instead of the screen,");
            Console.WriteLine("use > followed by the name of a file. For example,");
            Console.WriteLine("\"list bipd > bipeds.txt\" will write a list of bipd tags to bipeds.txt.");
            Console.WriteLine();
            Console.WriteLine("All numbers and tag indices will be parsed as big-endian hexadecimal.");
        }

        private void ListCommands(CommandContext context, HashSet<string> ignore)
        {
            if (context.Parent != null)
                ListCommands(context.Parent, ignore);

            // Sort commands and pad them to the length of the longest command name
            // Commands which aren't inherited or which have already been displayed are ignored
            var commands = context.Commands
                .Where(c => !ignore.Contains(c.Name) && IsAvailable(context, c))
                .OrderBy(c => c.Name);

            if(commands.Count() == 0)
            {
                Console.WriteLine();
                return;
            }

            var width = commands.Max(c => c.Name.Length);
            var format = "{0,-" + width + "}  {1}";

            Console.WriteLine("Available commands for {0}:", context.Name);
            Console.WriteLine();
            foreach (var command in commands)
            {
                Console.WriteLine(format, command.Name, Wrap(command.Description, wrap:Console.BufferWidth - (width + 4), padLeft: width + 3));
                ignore.Add(command.Name);
            }
            Console.WriteLine();
        }

        private void DisplayCommandHelp(string commandName)
        {
            var command = ContextStack.Context.GetCommand(commandName);
            if (command == null)
            {
                new TagToolError(CommandError.CustomError,$"Unable to find command \"{commandName}\"");
                return;
            }
			
            // Print help info
            ushort indent = 3;
            Console.WriteLine(FormatHelpInfo(command.Name, command.Description, indent, wrap:110));
            Console.WriteLine(FormatHelpInfo("Usage", command.Usage, indent));
            if (command.Examples != "")
                Console.WriteLine(FormatHelpInfo("Examples", command.Examples, indent));
            if (command.HelpMessage != "")
                Console.WriteLine(FormatHelpInfo("Notes", command.HelpMessage, indent, wrap:110));
        }

        private bool IsAvailable(CommandContext context, Command command)
        {
            return (command.Inherit || ContextStack.Context == context);
        }
		
        private static string FormatHelpInfo(string head, string body, ushort indent = 0, int wrap = int.MaxValue)
        {
            string indentStr = new string(' ', indent);
            string Indent(string line) { return (line.StartsWith("- ") ? $"\n{indentStr}" : $"\n{indentStr}  ") + line; }
            return $"\n{indentStr}{head}:" + String.Join("", body.Split('\n').Select(x => Indent(Wrap(x, wrap, indent + (x.StartsWith("- ") ? 4 : 2)))));          
        }   
        
        private static string Wrap(string strIn, int wrap, int padLeft)
        {
            return String.Join("\n".PadRight(padLeft),
               Regex.Matches(strIn, @"(.{1," + (wrap) + @"})(?:\s|$)").Cast<Match>());
        }
    }
}