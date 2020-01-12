using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TagTool.Cache;
using TagTool.Commands.Editing;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Commands.Tags
{
    class ForEachCommand : Command
    {
        private CommandContextStack ContextStack { get; }
        private HaloOnlineCacheContext CacheContext { get; }

        public ForEachCommand(CommandContextStack contextStack, HaloOnlineCacheContext cacheContext) :
            base(false,
                
                "ForEach",
                "Executes a command on every instance of the specified tag group.",
                
                "ForEach [Const] <Tag Group> [Named: <Regex>] <Command...>",
                
                "Executes a command on every instance of the specified tag group.")
        {
            ContextStack = contextStack;
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1)
                return false;

            var isConst = false;

            if (args[0].ToLower() == "const")
            {
                args.RemoveAt(0);
                isConst = true;
            }

            if (args.Count < 1)
                return false;

            if (!CacheContext.TryParseGroupTag(args[0], out var groupTag))
            {
                Console.WriteLine($"Invalid tag group: {args[0]}");
                return true;
            }

            args.RemoveAt(0);

            var startFilter = "";
            var endFilter = "";
            var filter = "";
            var filename = "";

            string pattern = null;

            while (args.Count > 0 && args[0].EndsWith(":"))
            {
                switch (args[0].ToLower())
                {
                    case "in_file:":
                        filename = args[1];
                        args.RemoveRange(0, 2);
                        break;
                    case "regex:":
                        if (args.Count < 3)
                            return false;
                        pattern = args[1];
                        args.RemoveRange(0, 2);
                        break;

                    case "starts:":
                    case "startswith:":
                    case "starts_with:":
                    case "starting:":
                    case "startingwith:":
                    case "starting_with:":
                    case "start_filter:":
                    case "starting_filter:":
                        startFilter = args[1];
                        args.RemoveRange(0, 2);
                        break;

                    case "ends:":
                    case "ending:":
                    case "endingwith:":
                    case "ending_with:":
                    case "endswith:":
                    case "ends_with:":
                    case "end_filter:":
                    case "ending_filter:":
                        endFilter = args[1];
                        args.RemoveRange(0, 2);
                        break;

                    case "named:":
                    case "filter:":
                    case "contains:":
                    case "containing:":
                        filter = args[1];
                        args.RemoveRange(0, 2);
                        break;
                }
            }

            var commandsToExecute = new List<List<string>>();

            // if no command is given, keep reading commands from stdin until an empty line encountered
            if (args.Count < 1)
            {
                string line;
                while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
                {
                    var commandsArgs = ArgumentParser.ParseCommand(line, out string redirectFile);
                    commandsToExecute.Add(commandsArgs);
                }
            }
            else
            {
                commandsToExecute.Add(args);
            }

            IEnumerable<CachedTagInstance> tags = null;

            // if a file is given use that as the source for tags
            if (!string.IsNullOrWhiteSpace(filename))
            {
                var tagsList = new List<CachedTagInstance>();
                foreach (var line in File.ReadAllLines(filename))
                    tagsList.Add(CacheContext.GetTag(line));

                tags = tagsList;
            }
            else
            {
                tags = CacheContext.TagCache.Index;
            }
               
            var rootContext = ContextStack.Context;

            using (var stream = CacheContext.OpenTagCacheReadWrite())
            {
                foreach (var instance in tags)
                {
                    if (instance == null || (groupTag != Tag.Null && !instance.IsInGroup(groupTag)))
                        continue;

                    var tagName = instance.Name ?? $"0x{instance.Index:X4}";

                    try
                    {
                        if (pattern != null && !Regex.IsMatch(tagName, pattern, RegexOptions.IgnoreCase))
                            continue;
                    }
                    catch
                    {
                        continue;
                    }

                    if (!tagName.StartsWith(startFilter) || !tagName.Contains(filter) || !tagName.EndsWith(endFilter))
                        continue;

                    var definition = CacheContext.Deserialize(stream, instance);
                    ContextStack.Push(EditTagContextFactory.Create(ContextStack, CacheContext, instance, definition));

                    Console.WriteLine();
                    Console.WriteLine($"{tagName}.{CacheContext.GetString(instance.Group.Name)}:");

                    foreach (var command in commandsToExecute)
                        ContextStack.Context.GetCommand(command[0]).Execute(command.Skip(1).ToList());

                    while (ContextStack.Context != rootContext) ContextStack.Pop();

                    if (!isConst)
                        CacheContext.Serialize(stream, instance, definition);
                }

                Console.WriteLine();
            }

            return true;
        }
    }
}
