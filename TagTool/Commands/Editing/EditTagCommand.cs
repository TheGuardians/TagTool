using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands;

namespace TagTool.Commands.Editing
{
    class EditTagCommand : Command
    {
        private CommandContextStack ContextStack { get; }
        private HaloOnlineCacheContext CacheContext { get; }

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
            if (args.Count != 1)
                return false;

            CachedTagInstance tag = null;

            try
            {
                tag = ArgumentParser.ParseTagSpecifier(CacheContext, args[0]);
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (tag == null)
                return false;

            var oldContext = ContextStack.Context;

            ContextStack.Push(EditTagContextFactory.Create(ContextStack, CacheContext, tag));

            var groupName = CacheContext.GetString(tag.Group.Name);
            var tagName = $"0x{tag.Index:X4}";

            if (CacheContext.TagNames.ContainsKey(tag.Index))
            {
                tagName = CacheContext.TagNames[tag.Index];
                tagName = $"(0x{tag.Index:X4}) {tagName.Substring(tagName.LastIndexOf('\\') + 1)}";
            }

            Console.WriteLine($"Tag {tagName}.{groupName} has been opened for editing.");
            Console.WriteLine("New commands are now available. Enter \"help\" to view them.");
            Console.WriteLine("Use \"exit\" to return to {0}.", oldContext.Name);

            return true;
        }
    }
}