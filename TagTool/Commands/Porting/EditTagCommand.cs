using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
    class EditTagCommand : Command
    {
        private CacheFile BlamCache;
        private CommandContextStack ContextStack { get; }

        public EditTagCommand(CommandContextStack contextStack, CacheFile blamCache) : base(
            false,

            "EditTag",
            "Edit tag-specific data",

            "EditTag <tag>",

            "If the tag contains data which is supported by this program,\n" +
            "this command will make special tag-specific commands available\n" +
            "which can be used to edit or view the data in the tag.")
        {
            ContextStack = contextStack;
            BlamCache = blamCache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            var groupTagInput = args[0];
            var blamTagName = args[1];
            var groupTag = Tag.Null;

            if (TagDefinition.Exists(groupTagInput))
            {
                groupTag = new Tag(groupTagInput);
            }
            else
            {
                foreach (var tagGroup in BlamCache.IndexItems.ClassList)
                {
                    if (groupTagInput == BlamCache.Strings.GetItemByID(tagGroup.StringID))
                    {
                        var chars = new char[4] { ' ', ' ', ' ', ' ' };

                        for (var i = 0; i < 4; i++)
                            chars[i] = tagGroup.ClassCode[i];

                        groupTag = new Tag(new string(chars));
                        break;
                    }
                }
            }

            CacheFile.IndexItem tag = null;

            foreach (var blamTag in BlamCache.IndexItems)
            {
                if ((blamTag.GroupTag == groupTag.ToString()) && (blamTag.Name == blamTagName))
                {
                    tag = blamTag;
                    break;
                }
            }

            if (tag == null)
                throw new Exception();

            var tagName = $"(0x{tag.ID:X8}) {tag.Name.Substring(tag.Name.LastIndexOf('\\') + 1)}";

            var tagType = TagDefinition.Find(groupTag);
            var structure = ReflectionCache.GetTagStructureInfo(tagType);

            var definition = BlamCache.Deserializer.Deserialize(new CacheSerializationContext(ref BlamCache, tag), tagType);

            var oldContext = ContextStack.Context;

            var commandContext = new CommandContext(ContextStack.Context, string.Format("{0}.{1}", tagName, groupTagInput));
            commandContext.AddCommand(new ListFieldsCommand(BlamCache, structure, definition));
            commandContext.AddCommand(new EditBlockCommand(ContextStack, BlamCache, tag, definition));
            commandContext.AddCommand(new Editing.ExitToCommand(ContextStack));
            ContextStack.Push(commandContext);

            Console.WriteLine($"Tag {tagName}.{groupTagInput} has been opened for editing.");
            Console.WriteLine("New commands are now available. Enter \"help\" to view them.");
            Console.WriteLine("Use \"exit\" to return to {0}.", oldContext.Name);

            return true;
        }
    }
}