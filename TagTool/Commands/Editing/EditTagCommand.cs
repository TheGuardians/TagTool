using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;

namespace TagTool.Commands.Editing
{
    class EditTagCommand : Command
    {
        private CommandContextStack ContextStack { get; }
        private GameCache Cache { get; }

        public CachedTag TagInstance { get; private set; }
        public object TagDefinition { get; private set; }

        public EditTagCommand(CommandContextStack contextStack, GameCache cache) : base(
            false,

            "EditTag",
            "Edit tag-specific data",

            "EditTag <tag>",

            "If the tag contains data which is supported by this program,\n" +
            "this command will make special tag-specific commands available\n" +
            "which can be used to edit or view the data in the tag.")
        {
            ContextStack = contextStack;
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);
            if (!Cache.TagCache.TryGetCachedTag(args[0], out var tag))
                return new TagToolError(CommandError.TagInvalid, $"\"{args[0]}\"");

            var oldContext = ContextStack.Context;

            TagInstance = tag;

            using (var stream = Cache.OpenCacheRead())
                TagDefinition = Cache.Deserialize(stream, TagInstance);

            ContextStack.Push(EditTagContextFactory.Create(ContextStack, Cache, TagInstance, TagDefinition));

            var groupName = TagInstance.Group.ToString();
            var tagName = TagInstance?.Name ?? $"0x{TagInstance.Index:X4}";

            Console.WriteLine($"Tag {tagName}.{groupName} has been opened for editing.");
            Console.WriteLine("New commands are now available. Enter \"help\" to view them.");
            Console.WriteLine("Use \"exit\" to return to {0}.", oldContext.Name);

            return true;
        }
    }
}