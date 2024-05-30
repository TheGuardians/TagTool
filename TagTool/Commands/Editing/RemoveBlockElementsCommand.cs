using System;
using System.Collections;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags;

namespace TagTool.Commands.Editing
{
    class RemoveBlockElementsCommand : Command
    {
        private CommandContextStack ContextStack { get; }
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private TagStructureInfo Structure { get; set; }
        private object Owner { get; set; }

        public RemoveBlockElementsCommand(CommandContextStack contextStack, GameCache cache, CachedTag tag, TagStructureInfo structure, object owner)
            : base(true,

                  "RemoveBlockElements",
                  $"Removes block element(s) from a specified index of a specific tag block in the current {structure.Types[0].Name} definition.",

                  "RemoveBlockElements <block name> [* | <block index> [* | amount = 1]]",
                  $"Removes block element(s) from a specified index of a specific tag block in the current {structure.Types[0].Name} definition.")
        {
            ContextStack = contextStack;
            Cache = cache;
            Tag = tag;
            Structure = structure;
            Owner = owner;
        }

        private CommandContext previousContext;
        private object previousOwner;
        private TagStructureInfo previousStructure;

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 3)
                return new TagToolError(CommandError.ArgCount);

            var fieldName = args[0];
            var fieldNameLow = fieldName.ToLower();

            previousContext = ContextStack.Context;
            previousOwner = Owner;
            previousStructure = Structure;

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

			var field = TagStructure.GetTagFieldEnumerable(Structure)
				.Find(f => f.Name == fieldName || f.Name.ToLower() == fieldNameLow);




            if ((field == null) || (!field.FieldType.IsGenericType) || (field.FieldType.GetInterface("IList") == null))
            {
                ContextReturn(previousContext, previousOwner, previousStructure);
                return new TagToolError(CommandError.ArgInvalid, $"\"{Structure.Types[0].Name}\" does not contain a tag block named \"{args[0]}\".");
            }

            var fieldType = field.FieldType;

            var blockValue = field.GetValue(Owner) as IList;

            if (blockValue == null)
            {
                blockValue = Activator.CreateInstance(field.FieldType) as IList;
                field.SetValue(Owner, blockValue);
            }

            var elementType = field.FieldType.GenericTypeArguments[0];

            var index = blockValue.Count - 1;
            var count = 1;

            if (index < 0)
            {
                Console.WriteLine("TagBlock is already null.");
                ContextReturn(previousContext, previousOwner, previousStructure);
                return true;
            }

            var genericIndex = false;
            var genericCount = false;

            if (args.Count == 1)
            {
                count = 1;
            }
            else
            {
                if (args.Count >= 2)
                {
                    if (args[1] == "*")
                    {
                        genericIndex = true;
                        index = blockValue.Count;
                    }
                    else if (!int.TryParse(args[1], out index) || index < 0 || index >= blockValue.Count)
                    {
                        ContextReturn(previousContext, previousOwner, previousStructure);
                        return new TagToolError(CommandError.ArgInvalid, $"Invalid index specified: {args[1]}");
                    }
                }

                if (args.Count == 3)
                {
                    if (args[2] == "*")
                    {
                        genericCount = true;
                        count = blockValue.Count - index;
                    }
                    else if (!int.TryParse(args[2], out count) || count < 1)
                    {
                        return new TagToolError(CommandError.ArgInvalid, $"Invalid amount specified: {args[2]}");
                    }
                }
            }

            if (genericIndex && genericCount)
            {
                index = 0;
                count = blockValue.Count;
            }
            else if (genericIndex)
            {
                index -= count;

                if (index < 0)
                    index = 0;
            }

            if (index + count > blockValue.Count)
                return new TagToolError(CommandError.ArgInvalid, $"Amount \"{count}\" exceeds the range of existing blocks");

            for (var i = 0; i < count; i++)
                blockValue.RemoveAt(index);

            field.SetValue(Owner, blockValue);

            var typeString =
                fieldType.IsGenericType ?
                    $"{fieldType.Name}<{fieldType.GenericTypeArguments[0].Name}>" :
                fieldType.Name;

            var itemString = count < 2 ? "element" : "elements";

            var valueString =
                ((IList)blockValue).Count != 0 ?
                    $"{{...}}[{((IList)blockValue).Count}]" :
                "null";

            Console.WriteLine($"Successfully removed {count} {itemString} from {field.Name} at index {index}: {typeString}");
            Console.WriteLine(valueString);

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
