using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using TagTool.Cache;
using TagTool.Commands.Tags;
using TagTool.IO;

namespace TagTool.Commands
{
    public static class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
            ConsoleHistory.Initialize();

            // If there are extra arguments, use them to automatically execute a command
            List<string> autoexecCommand = null;
            if (args.Length > 1)
                autoexecCommand = args.Skip(1).ToList();

            if (autoexecCommand == null)
            {
                Console.WriteLine($"Tag Tool [{Assembly.GetExecutingAssembly().GetName().Version}]");
                Console.WriteLine();
                Console.WriteLine("Please report any bugs and/or feature requests at");
                Console.WriteLine("<https://gitlab.com/TheGuardians/TagTool/issues>.");
                Console.WriteLine();
            }

            start:
            // Get the file path from the first argument
            // If no argument is given, load tags.dat
            var fileInfo = new FileInfo((args.Length > 0) ? args[0] : "tags.dat");

            while (!fileInfo.Exists)
            {
                Console.WriteLine("Enter the path to 'tags.dat':");
                Console.Write("> ");
				var tagCacheFile = Console.ReadLine();

                switch (tagCacheFile.ToLower())
                {
                    case "restart":
                        Console.WriteLine();
                        goto start;

                    case "exit":
                    case "quit":
                        Console.WriteLine();
                        goto end;
                }

                //sometimes drag&drop files have quotes placed around them, remove the quotes
                tagCacheFile = tagCacheFile.Replace("\"", "").Replace("\'", "");

                if (File.Exists(tagCacheFile))
					fileInfo = new FileInfo(tagCacheFile);
				else
					Console.WriteLine("Invalid path to 'tags.dat'!");

                Console.WriteLine();
            }

            HaloOnlineCacheContext cacheContext = null;

#if !DEBUG
            try
            {
#endif
                cacheContext = new HaloOnlineCacheContext(fileInfo.Directory);
#if !DEBUG
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
                ConsoleHistory.Dump("hott_*_init.log");
                return;
            }
#endif

            // Create command context
            var contextStack = new CommandContextStack();
            var tagsContext = TagCacheContextFactory.Create(contextStack, cacheContext);
            contextStack.Push(tagsContext);

            // If autoexecuting a command, just run it and return
            if (autoexecCommand != null)
            {
                if (!ExecuteCommand(contextStack.Context, autoexecCommand))
                    Console.WriteLine("Unrecognized command: {0}", autoexecCommand[0]);
                return;
            }

            Console.WriteLine("Enter \"help\" to list available commands. Enter \"exit\" to quit.");
            while (true)
            {
                // Read and parse a command
                Console.WriteLine();
                Console.Write("{0}> ", contextStack.GetPath());
                Console.Title = $"TagTool {contextStack.GetPath()}>";
                var commandLine = Console.ReadLine();
                if (commandLine == null)
                    break;

                var commandArgs = ArgumentParser.ParseCommand(commandLine, out string redirectFile);
                if (commandArgs.Count == 0)
                    continue;

                switch (commandArgs[0].ToLower())
                {
                    case "restart":
                        Console.WriteLine();
                        goto start;

                    case "quit":
                        Console.WriteLine();
                        goto end;

                    case "exit":
                        if (!contextStack.Pop())
                            goto end;
                        continue;

                    case "cyka":
                        Console.WriteLine("blyat!");
                        continue;
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
                if (!ExecuteCommand(contextStack.Context, commandArgs))
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

            // wait for all async jobs to complete
            Tools.AsyncJobManager.WaitForAll();

            end: return;
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
                ExecuteCommand(command, commandAndArgs);
#if !DEBUG
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
                ConsoleHistory.Dump("hott_*_crash.log");
            }
#endif

            return true;
        }

        private static void ExecuteCommand(Command command, List<string> args)
        {
            if (command.Execute(args).Equals(true))
                return;

            Console.WriteLine("{0}: {1}", command.Name, command.Description);
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("{0}", command.Usage);
            Console.WriteLine();
            Console.WriteLine("Use \"help {0}\" for more information.", command.Name);
        }
    }
}