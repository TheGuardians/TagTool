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
        public static string TagToolDirectory = System.IO.Path.GetDirectoryName(
            System.Reflection.Assembly.GetExecutingAssembly().Location);

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
            }

            start:
            // Get the file path from the first argument
            // If no argument is given, load tags.dat
            // legacy from older cache system where only HO caches could be loaded
            var fileInfo = new FileInfo((args.Length > 0) ? args[0] : "tags.dat");

            if (args.Length > 0 && !fileInfo.Exists)
                new TagToolError(CommandError.CustomError, "Invalid path to a tag cache!");

            while (!fileInfo.Exists)
            {
                Console.WriteLine("\nEnter the path to a Halo cache file (.map or tags.dat)':");
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
					new TagToolError(CommandError.CustomError,"Invalid path to a tag cache!");
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
                new TagToolError(CommandError.CustomError, e.Message);
                Console.WriteLine("\nSTACKTRACE: " + Environment.NewLine + e.StackTrace);
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

            var autoExecFile = new FileInfo(Path.Combine(TagToolDirectory, "autoexec.cmds"));
            if(autoExecFile.Exists)
            {
                foreach (var line in File.ReadAllLines(autoExecFile.FullName))
                    commandRunner.RunCommand(line);
            }

            Console.WriteLine("\nEnter \"help\" to list available commands. Enter \"exit\" to quit.");
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