using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Commands.Editing
{
    class EditTagCommand : Command
    {
        private CommandContextStack ContextStack { get; }
        private HaloOnlineCacheContext CacheContext { get; }

        public CachedTagInstance TagInstance { get; private set; }
        public object TagDefinition { get; private set; }

        public EditTagCommand(CommandContextStack contextStack, HaloOnlineCacheContext cacheContext) : base(
            false,

            "EditTag",
            "Edit tag-specific data",

            "EditTag <tag>",

            "If the tag contains data which is supported by this program,\n" +
            "this command will make special tag-specific commands available\n" +
            "which can be used to edit or view the data in the tag.")
        {
            ContextStack = contextStack;
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1 || !CacheContext.TryGetTag(args[0], out var tag))
                return false;

            var oldContext = ContextStack.Context;

            TagInstance = tag;

            using (var stream = CacheContext.OpenTagCacheRead())
                TagDefinition = CacheContext.Deserialize(stream, TagInstance);

            ContextStack.Push(EditTagContextFactory.Create(ContextStack, CacheContext, TagInstance, TagDefinition));

            var groupName = CacheContext.GetString(TagInstance.Group.Name);
            var tagName = TagInstance?.Name ?? $"0x{TagInstance.Index:X4}";

            Console.WriteLine($"Tag {tagName}.{groupName} has been opened for editing.");
            Console.WriteLine("New commands are now available. Enter \"help\" to view them.");
            Console.WriteLine("Use \"exit\" to return to {0}.", oldContext.Name);

            return true;
        }
    }
}