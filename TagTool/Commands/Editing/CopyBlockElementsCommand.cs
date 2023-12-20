using System;
using System.Collections;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags;

namespace TagTool.Commands.Editing
{
    class CopyBlockElementsCommand : Command
    {
        private CommandContextStack ContextStack { get; }
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private TagStructureInfo Structure { get; set; }
        private object Owner { get; set; }
        
        public static Type ElementType { get; set; } = null;
        public static List<object> Elements { get; set; } = null;

        public CopyBlockElementsCommand(CommandContextStack contextStack, GameCache cache, CachedTag tag, TagStructureInfo structure, object owner)
            : base(false,

                  "CopyBlockElements",
                  "Copies block elements from one tag to another.",

                  "CopyBlockElements <block name> [index] [count]",

                  "Stores a copy of requested blocks for pasting elsewhere." +
                  "\nNo index or count will copy the all blocks of the requested type." +
                  "\nNo count will copy one element at the requested index." +
                  "\nCount = * will copy all from the index to the end.")
        {
            ContextStack = contextStack;
            Cache = cache;
            Tag = tag;
            Structure = structure;
            Owner = owner;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 4)
                return new TagToolError(CommandError.ArgCount);

            var fieldName = args[0];
            var fieldNameLow = fieldName.ToLower();

            var previousContext = ContextStack.Context;
            var previousOwner = Owner;
            var previousStructure = Structure;

            if (fieldName.Contains("."))
            {
                var lastIndex = fieldName.LastIndexOf('.');
                var blockName = fieldName.Substring(0, lastIndex);
                fieldName = fieldName.Substring(lastIndex + 1, (fieldName.Length - lastIndex) - 1);
                fieldNameLow = fieldName.ToLower();

                var command = new EditBlockCommand(ContextStack, Cache, Tag, Owner);

                if (command.Execute(new List<string> { blockName }).Equals(false))
                {
                    ContextReturn(previousContext, previousOwner, previousStructure);
                    return new TagToolError(CommandError.ArgInvalid, $"TagBlock \"{blockName}\" does not exist in the specified context");
                }

                command = (ContextStack.Context.GetCommand("EditBlock") as EditBlockCommand);

                Owner = command.Owner;
                Structure = command.Structure;

                if (Owner == null)
                {
                    ContextReturn(previousContext, previousOwner, previousStructure);
                    return new TagToolError(CommandError.OperationFailed, "Command context owner was null");
                }
            }

            var index = 0;

            if (args.Count > 1 && args[1] != "*")
                if (!int.TryParse(args[1], out index) || index < 0)
                    return new TagToolError(CommandError.ArgInvalid, $"Invalid index specified: {args[1]}");

            var count = (args.Count == 2) ? 1 : -1;

            if (args.Count > 2)
                if (args[2] != "*" && (!int.TryParse(args[2], out count) || count < 1))
                    return new TagToolError(CommandError.ArgInvalid, $"Invalid amount specified: {args[2]}");

            var field = TagStructure.GetTagFieldEnumerable(Structure)
				.Find(f => f.Name == fieldName || f.Name.ToLower() == fieldNameLow);

            if ((field == null) ||
                (!field.FieldType.IsGenericType) ||
                (field.FieldType.GetInterface("IList") == null))
            {
                ContextReturn(previousContext, previousOwner, previousStructure);
                return new TagToolError(CommandError.ArgInvalid, $"\"{Structure.Types[0].Name}\" does not contain a tag block named \"{args[0]}\".");
            }

            var blockValue = field.GetValue(Owner) as IList;

            if (blockValue == null)
            {
                ContextReturn(previousContext, previousOwner, previousStructure);
                return new TagToolError(CommandError.ArgInvalid, $"Invalid index specified \"{args[0]}\"");
            }

            if (count < 0)
            {
                count = blockValue.Count;
            }

            if ((index + count) < 0 || (index + count) > blockValue.Count)
            {
                ContextReturn(previousContext, previousOwner, previousStructure);
                return new TagToolError(CommandError.ArgInvalid, $"Invalid index: \"{index}\", and count: \"{count}\"");
            }

            ElementType = field.FieldType.GenericTypeArguments[0];
            Elements = new List<object>();

            for (var i = index; i < (index + count); i++)
                Elements.Add(blockValue[i].DeepCloneV2());
            
            var itemString = index < 2 ? "element" : "elements";
            Console.WriteLine($"Successfully copied {count} {itemString}.");

            ContextReturn(previousContext, previousOwner, previousStructure);

            return true;
        }

        public void ContextReturn(CommandContext previousContext, object previousOwner, TagStructureInfo previousStructure)
        {
            while (ContextStack.Context != previousContext) ContextStack.Pop();
            Owner = previousOwner;
            Structure = previousStructure;
        }
    }
}
