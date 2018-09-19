using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
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
                
                "ForEach <Tag Group> <Command...>",
                
                "Executes a command on every instance of the specified tag group.")
        {
            ContextStack = contextStack;
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1)
                return false;

            if (!CacheContext.TryParseGroupTag(args[0], out var groupTag))
            {
                Console.WriteLine($"Invalid tag group: {args[0]}");
                return true;
            }

            args.RemoveAt(0);

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

                    var definition = CacheContext.Deserialize(stream, instance);
                    ContextStack.Push(EditTagContextFactory.Create(ContextStack, CacheContext, instance, definition));

                    Console.WriteLine($"{tagName}.{groupName}:");
                    ContextStack.Context.GetCommand(args[0]).Execute(args.Skip(1).ToList());
                    Console.WriteLine();

                    while (ContextStack.Context != rootContext) ContextStack.Pop();
                    CacheContext.Serialize(stream, instance, definition);
                }
            }

            return true;
        }
    }
}
