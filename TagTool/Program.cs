using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Commands.Tags;
using TagTool.Common;
using TagTool.IO;

namespace TagTool.Commands
{
    public static class Program
    {
        public static string TagToolDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static readonly Stopwatch _stopWatch = new Stopwatch();
        public static int ErrorCount = 0;
        public static int WarningCount = 0;

        static void Main(string[] args)
        {
            SetDirectories();
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
            ConsoleHistory.Initialize();

            // If there are extra arguments, use them to automatically execute a command
            List<string> autoexecCommand = null;
            if (args.Length > 1)
                autoexecCommand = args.Skip(1).ToList();

            if (autoexecCommand == null)
            {
                Console.WriteLine($"TagTool [{Assembly.GetExecutingAssembly().GetName().Version} (Built {GetLinkerTimestampUtc(Assembly.GetExecutingAssembly())} UTC)]");
                Console.WriteLine();
                Console.WriteLine("Please report any bugs and/or feature requests:");
                Console.WriteLine("https://github.com/TheGuardians-CI/TagTool/issues");
            }

            start:
            // Get the file path from the first argument
            // If no argument is given, load tags.dat
            // legacy from older cache system where only HO caches could be loaded

            var autoExecFile = new FileInfo(Path.Combine(TagToolDirectory, "autoexec.cmds"));

            if (autoExecFile.Exists && args.Count() == 0)
            {
                try
                {
                    args = new string[] { File.ReadLines("autoexec.cmds").First() };

                    args[0] = args[0].EndsWith("tags.dat") ? args[0] : 
                        (args[0].EndsWith("\\") ? args[0] += "tags.dat" : args[0] += "\\tags.dat");
                }
                catch
                {
                    Console.WriteLine();
                    new TagToolWarning("Your \"autoexec.cmds\" file contains no lines.");
                }
            }
                

            var fileInfo = new FileInfo((args.Length > 0) ? args[0] : "tags.dat");
            bool defaultCacheIsSet = fileInfo.Exists;

            if (args.Length > 0 && !fileInfo.Exists && !autoExecFile.Exists)
                new TagToolError(CommandError.CustomError, "Invalid path to a tag cache!");
            else if (fileInfo.Exists)
                defaultCacheIsSet = true;

            while (!fileInfo.Exists)
            {
                Console.WriteLine("\nEnter the path to a Halo cache file (.map/.dat):");
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

                if (string.IsNullOrWhiteSpace(tagCacheFile))
                    continue;

                if (!tagCacheFile.EndsWith(".map") && !tagCacheFile.EndsWith(".dat"))
                {
                    if (tagCacheFile.Last() == '\\' || tagCacheFile.Last() == '/')
                        tagCacheFile = tagCacheFile.Substring(0, tagCacheFile.Length - 1);

                    var append = tagCacheFile.EndsWith("maps") ? "tags.dat" : "maps\\tags.dat";
                    tagCacheFile = Path.Combine(tagCacheFile, append);
                }

                if (File.Exists(tagCacheFile))
                    fileInfo = new FileInfo(tagCacheFile);
                else
                    new TagToolError(CommandError.CustomError, "Invalid path to a tag cache!");
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

            
            if(autoExecFile.Exists)
            {
                var autoExecLines = File.ReadAllLines(autoExecFile.FullName);

                // if cache path provided at the start of autoexec.cmds, ignore it when executing
                autoExecLines = defaultCacheIsSet ? autoExecLines.Skip(1).ToArray() : autoExecLines;

                foreach (var line in autoExecLines)
                    commandRunner.RunCommand(line);
            }

            Console.WriteLine("\nEnter \"help\" to list available commands. Enter \"quit\" to quit.");
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

        public static void SetDirectories()
        {
            // Needed to use AddDllDirectory
            NativeInterop.SetDefaultDllDirectories(0x1000u); // LOAD_LIBRARY_SEARCH_DEFAULT_DIRS
            // Add the tools directory to the search path to simplify usage of [DllImport]
            NativeInterop.AddDllDirectory(Path.Combine(TagToolDirectory, "Tools"));
        }

        public static void ReportElapsed()
        {
            _stopWatch.Stop();
            TimeSpan t = TimeSpan.FromMilliseconds(_stopWatch.ElapsedMilliseconds);

            string timeDisplay = $"{t.TotalMilliseconds} milliseconds";

            if (t.TotalMilliseconds > 10000)
            {
                timeDisplay = $"{t.Minutes} minutes and {t.Seconds} seconds";

                if (t.Hours > 0)
                    timeDisplay = $"{t.Hours} hours, " + timeDisplay;
            }

            Console.Write($"{timeDisplay} elapsed with ");

            Console.ForegroundColor = (ErrorCount == 0) ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write($"{ErrorCount} errors ");
            Console.ResetColor();

            Console.Write("and ");

            Console.ForegroundColor = (WarningCount == 0) ? Console.ForegroundColor : ConsoleColor.DarkYellow;
            Console.Write($"{WarningCount} warnings");
            Console.ResetColor();

            Console.Write(".\n");

            ErrorCount = 0;
            WarningCount = 0;
            _stopWatch.Reset();
        }

        public static DateTime GetLinkerTimestampUtc(Assembly assembly)
        {
            var location = assembly.Location;
            return GetLinkerTimestampUtc(location);
        }

        public static DateTime GetLinkerTimestampUtc(string filePath)
        {
            const int peHeaderOffset = 60;
            const int linkerTimestampOffset = 8;
            var bytes = new byte[2048];

            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                file.Read(bytes, 0, bytes.Length);
            }

            var headerPos = BitConverter.ToInt32(bytes, peHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(bytes, headerPos + linkerTimestampOffset);
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return dt.AddSeconds(secondsSince1970);
        }
    }
}