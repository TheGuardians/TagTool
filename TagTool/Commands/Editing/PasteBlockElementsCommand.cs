using System;
using System.Collections;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags;

namespace TagTool.Commands.Editing
{
    class PasteBlockElementsCommand : Command
    {
        private CommandContextStack ContextStack { get; }
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private TagStructureInfo Structure { get; set; }
        private object Owner { get; set; }

        public PasteBlockElementsCommand(CommandContextStack contextStack, GameCache cache, CachedTag tag, TagStructureInfo structure, object owner)
            : base(true,

                  "PasteBlockElements",
                  $"Pastes block element(s) to a specific tag block in the current {structure.Types[0].Name} definition.",

                  "PasteBlockElements <tag block name> [index = *]",

                  $"Pastes block element(s) to a specific tag block in the current {structure.Types[0].Name} definition.")
        {
            ContextStack = contextStack;
            Cache = cache;
            Tag = tag;
            Structure = structure;
            Owner = owner;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 2)
                return new TagToolError(CommandError.ArgCount);

            if (CopyBlockElementsCommand.Elements == null)
                return new TagToolError(CommandError.CustomMessage, "You need to copy at least one block element first");

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
                    while (ContextStack.Context != previousContext) ContextStack.Pop();
                    Owner = previousOwner;
                    Structure = previousStructure;
                    return new TagToolError(CommandError.ArgInvalid, $"TagBlock \"{blockName}\" does not exist in the specified context");
                }

                command = (ContextStack.Context.GetCommand("EditBlock") as EditBlockCommand);

                Owner = command.Owner;
                Structure = command.Structure;

                if (Owner == null)
                {
                    while (ContextStack.Context != previousContext) ContextStack.Pop();
                    Owner = previousOwner;
                    Structure = previousStructure;
                    return new TagToolError(CommandError.OperationFailed, "Command context owner was null");
                }
            }
            
            var index = -1;

            if (args.Count > 1)
                if (args[1] != "*" && (!int.TryParse(args[1], out index) || index < 0))
                    return new TagToolError(CommandError.ArgInvalid, $"Invalid index specified: {args[1]}");

			var field = TagStructure.GetTagFieldEnumerable(Structure)
				.Find(f => f.Name == fieldName || f.Name.ToLower() == fieldNameLow);

			var fieldType = field?.FieldType;

            if (field == null || !fieldType.IsGenericType || fieldType.GetInterface("IList") == null)
            {
                while (ContextStack.Context != previousContext) ContextStack.Pop();
                Owner = previousOwner;
                Structure = previousStructure;
                return new TagToolError(CommandError.ArgInvalid, $"\"{Structure.Types[0].Name}\" does not contain a tag block named \"{args[0]}\".");
            }

            var elementType = field.FieldType.GenericTypeArguments[0];

            if (elementType != CopyBlockElementsCommand.ElementType)
            {
				while (ContextStack.Context != previousContext) ContextStack.Pop();
				Owner = previousOwner;
				Structure = previousStructure;
				return new TagToolError(CommandError.CustomError, "Invalid block element type");
            }

            var blockValue = field.GetValue(Owner) as IList;

            if (blockValue == null)
            {
                blockValue = Activator.CreateInstance(field.FieldType) as IList;
                field.SetValue(Owner, blockValue);
            }

            if (index > blockValue.Count)
                return new TagToolError(CommandError.ArgInvalid, $"Invalid index \"{index}\"");

            for (var i = 0; i < CopyBlockElementsCommand.Elements.Count; i++)
            {
                var element = CopyBlockElementsCommand.Elements[i].DeepCloneV2();

                if (index == -1)
                    blockValue.Add(element);
                else
                    blockValue.Insert(index + i, element);
            }

            field.SetValue(Owner, blockValue);

            var typeString =
                fieldType.IsGenericType ?
                    $"{fieldType.Name}<{fieldType.GenericTypeArguments[0].Name}>" :
                fieldType.Name;

            var itemString = CopyBlockElementsCommand.Elements.Count < 2 ? "element" : "elements";

            var valueString =
                ((IList)blockValue).Count != 0 ?
                    $"{{...}}[{((IList)blockValue).Count}]" :
                "null";

            Console.WriteLine($"Successfully pasted {CopyBlockElementsCommand.Elements.Count} {itemString} to {field.Name}: {typeString}");
            Console.WriteLine(valueString);

            while (ContextStack.Context != previousContext) ContextStack.Pop();
            Owner = previousOwner;
            Structure = previousStructure;

            return true;
        }

        private object CreateElement(Type elementType)
        {
            var element = Activator.CreateInstance(elementType);

            var isTagStructure = Attribute.IsDefined(elementType, typeof(TagStructureAttribute));

            if (isTagStructure)
            {
				foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(elementType, Cache.Version, Cache.Platform))
					{
						var fieldType = tagFieldInfo.FieldType;

						if (fieldType.IsArray && tagFieldInfo.Attribute.Length > 0)
						{
							var array = (IList)Activator.CreateInstance(tagFieldInfo.FieldType,
								new object[] { tagFieldInfo.Attribute.Length });

							for (var i = 0; i < tagFieldInfo.Attribute.Length; i++)
								array[i] = CreateElement(fieldType.GetElementType());
						}
						else
						{
#if !DEBUG
                        try
                        {
#endif
							tagFieldInfo.SetValue(element, CreateElement(tagFieldInfo.FieldType));
#if !DEBUG
                        }
                        catch
                        {
                            tagFieldInfo.SetValue(element, null);
                        }
#endif
						}
					}
            }

            return element;
        }
    }
}
