using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Commands.Editing;
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
            if (args.Count < 2)
                return false;

            var isConst = false;

            if (args[0].ToLower() == "const")
            {
                args.RemoveAt(0);
                isConst = true;
            }

            if (args.Count < 2)
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

            string pattern = null;

            while (args[0].EndsWith(":"))
            {
                switch (args[0].ToLower())
                {
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

            var rootContext = ContextStack.Context;
            var groupName = CacheContext.GetString(TagGroup.Instances[groupTag].Name);

            using (var stream = CacheContext.OpenTagCacheReadWrite())
            {
                foreach (var instance in CacheContext.TagCache.Index)
                {
                    if (instance == null || !instance.IsInGroup(groupTag))
                        continue;

                    var tagName = CacheContext.TagNames.ContainsKey(instance.Index) ?
                        CacheContext.TagNames[instance.Index] :
                        $"0x{instance.Index:X4}";

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
                    Console.WriteLine($"{tagName}.{groupName}:");
                    ContextStack.Context.GetCommand(args[0]).Execute(args.Skip(1).ToList());

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
