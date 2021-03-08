using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using TagTool.Cache;
using TagTool.Commands.Common;
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
                Console.WriteLine("<https://github.com/TheGuardians-CI/TagTool/issues>.");
                Console.WriteLine();
            }

            start:
            // Get the file path from the first argument
            // If no argument is given, load tags.dat
            // legacy from older cache system where only HO caches could be loaded
            var fileInfo = new FileInfo((args.Length > 0) ? args[0] : "tags.dat");

            while (!fileInfo.Exists)
            {
                Console.WriteLine("Enter the path to a Halo cache file (.map or tags.dat)':");
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

                if (!tagCacheFile.Contains(".map") && !tagCacheFile.Contains("\\tags.dat"))
                    tagCacheFile += "\\tags.dat";

                if (File.Exists(tagCacheFile))
					fileInfo = new FileInfo(tagCacheFile);
				else
					Console.WriteLine("\nERROR: Invalid path to a tag cache!");

                Console.WriteLine();
            }

            GameCache gameCache = null;

#if !DEBUG
            try
            {
#endif
                gameCache = GameCache.Open(fileInfo);
#if !DEBUG
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
                Console.WriteLine("STACKTRACE: " + Environment.NewLine + e.StackTrace);
                ConsoleHistory.Dump("hott_*_init.log");
                return;
            }
#endif

            // Create command context
            var contextStack = new CommandContextStack();
            var tagsContext = TagCacheContextFactory.Create(contextStack, gameCache);
            contextStack.Push(tagsContext);

            var commandRunner = new CommandRunner(contextStack);

            // If autoexecuting a command, just run it and return
            if (autoexecCommand != null)
            {
                commandRunner.RunCommand(string.Join(" ", autoexecCommand), false);
                goto end;
            }           

            Console.WriteLine("Enter \"help\" to list available commands. Enter \"exit\" to quit.");
            while (!commandRunner.EOF)
            {
                // Read and parse a command
                Console.WriteLine();
                Console.Write("{0}> ", contextStack.GetPath());
                Console.Title = $"TagTool {contextStack.GetPath()}>";

                var line = Console.ReadLine();
                if (line == "restart")
                    goto start;
                commandRunner.RunCommand(line, false);
            }

            end: return;
        }
    }
}