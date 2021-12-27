using System;
using System.Collections;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags;

namespace TagTool.Commands.Editing
{
    class AddBlockElementsCommand : BlockManipulationCommand
    {
        private CommandContextStack ContextStack { get; }
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private TagStructureInfo Structure { get; set; }
        private object Owner { get; set; }

        public AddBlockElementsCommand(CommandContextStack contextStack, GameCache cache, CachedTag tag, TagStructureInfo structure, object owner)
            : base(contextStack, cache, tag, structure, owner, true,

                  "AddBlockElements",
                  $"Adds/inserts block element(s) to a specific tag block in the current {structure.Types[0].Name} definition.",

                  "AddBlockElements <block name> [amount = 1] [index = *]",

                  $"Adds/inserts block element(s) to a specific tag block in the current {structure.Types[0].Name} definition.")
        {
            ContextStack = contextStack;
            Cache = cache;
            Tag = tag;
            Structure = structure;
            Owner = owner;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 3)
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

            int count = 1;
            if ((args.Count > 1 && !int.TryParse(args[1], out count)) || count < 1)
                return new TagToolError(CommandError.ArgInvalid, $"Invalid amount specified: {args[1]}");

            var index = -1;

            if (args.Count > 2)
                if (args[2] != "*" && (!int.TryParse(args[2], out index) || index < 0))
                    return new TagToolError(CommandError.ArgInvalid, $"Invalid index specified: {args[2]}");

			var field = TagStructure.GetTagFieldEnumerable(Structure)
				.Find(f => f.Name == fieldName || f.Name.ToLower() == fieldNameLow);

            if ((field == null) ||
                (!field.FieldType.IsGenericType) ||
                (field.FieldType.GetInterface("IList") == null))
            {
                Console.WriteLine();
                ContextReturn(previousContext, previousOwner, previousStructure);
                return new TagToolError(CommandError.ArgInvalid, $"\"{Structure.Types[0].Name}\" does not contain a tag block named \"{args[0]}\".");
            }

            var blockValue = field.GetValue(Owner) as IList;

            if (blockValue == null)
            {
                blockValue = Activator.CreateInstance(field.FieldType) as IList;
                field.SetValue(Owner, blockValue);
            }
            
            if (index > blockValue.Count)
                return new TagToolError(CommandError.ArgInvalid, $"Invalid index specified \"{args[2]}\"");

            var elementType = field.FieldType.GenericTypeArguments[0];
            
            for (var i = 0; i < count; i++)
            {
                var element = CreateElement(elementType);

                if (index == -1)
                    blockValue.Add(element);
                else
                    blockValue.Insert(index + i, element);
            }

            field.SetValue(Owner, blockValue);

            var typeString =
                field.FieldType.IsGenericType ?
                    $"{field.FieldType.Name}<{field.FieldType.GenericTypeArguments[0].Name}>" :
                field.FieldType.Name;

            var itemString = count < 2 ? "element" : "elements";

            var valueString =
                ((IList)blockValue).Count != 0 ?
                    $"{{...}}[{((IList)blockValue).Count}]" :
                "null";

            Console.WriteLine($"Successfully added {count} {itemString} to {field.Name}: {typeString}");
            Console.WriteLine(valueString);

            ContextReturn(previousContext, previousOwner, previousStructure);

            return true;
        }

        private object CreateElement(Type elementType)
        {
            var instance = Activator.CreateInstance(elementType);

            if (!Attribute.IsDefined(elementType, typeof(TagStructureAttribute)) || !elementType.IsSubclassOf(typeof(TagStructure)))
                return instance;

            var element = (TagStructure)instance;

            foreach (var tagFieldInfo in element.GetTagFieldEnumerable(Cache.Version, Cache.Platform))
            {
                var fieldType = tagFieldInfo.FieldType;

                if (fieldType.IsArray && tagFieldInfo.Attribute.Length > 0)
                {
                    var array = (IList)Activator.CreateInstance(tagFieldInfo.FieldType,
                        new object[] { tagFieldInfo.Attribute.Length });

                    for (var i = 0; i < tagFieldInfo.Attribute.Length; i++)
                        array[i] = CreateElement(fieldType.GetElementType());

                    tagFieldInfo.SetValue(element, array);
                }
                else
                {
                    try
                    {
                        tagFieldInfo.SetValue(element, CreateElement(tagFieldInfo.FieldType));
                    }
                    catch
                    {
                        try
                        {
                            tagFieldInfo.SetValue(element, Activator.CreateInstance(tagFieldInfo.FieldType));
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return element;
        }
    }
}