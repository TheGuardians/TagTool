using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Commands.Editing
{
    class ExtractResourceCommand : Command
    {
        private CommandContextStack ContextStack { get; }
        private HaloOnlineCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }

        public TagStructureInfo Structure { get; set; }
        public object Owner { get; set; }

        public ExtractResourceCommand(CommandContextStack contextStack, HaloOnlineCacheContext cacheContext, CachedTagInstance tag, TagStructureInfo structure, object owner) :
            base(true,
                
                "ExtractResource",
                $"Extracts the data of a specific resource field in the current {structure.Types[0].Name} definition.",

                "ExtractResource <field name> <path>",

                $"Extracts the data of a specific resource field in the current {structure.Types[0].Name} definition.")
        {
            ContextStack = contextStack;
            CacheContext = cacheContext;
            Tag = tag;
            Structure = structure;
            Owner = owner;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 2)
                return false;

            var fieldName = args[0];
            var file = new FileInfo(args[1]);

            var fieldNameLow = fieldName.ToLower();
            var fieldNameSnake = fieldName.ToSnakeCase();

            var previousContext = ContextStack.Context;
            var previousOwner = Owner;
            var previousStructure = Structure;

            if (fieldName.Contains("."))
            {
                var lastIndex = fieldName.LastIndexOf('.');
                var blockName = fieldName.Substring(0, lastIndex);
                fieldName = fieldName.Substring(lastIndex + 1, (fieldName.Length - lastIndex) - 1);
                fieldNameLow = fieldName.ToLower();
                fieldNameSnake = fieldName.ToSnakeCase();

                var command = new EditBlockCommand(ContextStack, CacheContext, Tag, Owner);

                if (command.Execute(new List<string> { blockName }).Equals(false))
                {
                    while (ContextStack.Context != previousContext) ContextStack.Pop();
                    Owner = previousOwner;
                    Structure = previousStructure;
                    return false;
                }

                command = (ContextStack.Context.GetCommand("EditBlock") as EditBlockCommand);

                Owner = command.Owner;
                Structure = command.Structure;

                if (Owner == null)
                {
                    while (ContextStack.Context != previousContext) ContextStack.Pop();
                    Owner = previousOwner;
                    Structure = previousStructure;
                    return false;
                }
            }

            var field = TagStructure.GetTagFieldEnumerable(Structure)
                .Find(f =>
                    f.Name == fieldName ||
                    f.Name.ToLower() == fieldNameLow ||
                    f.Name.ToSnakeCase() == fieldNameSnake);

            if (field == null)
            {
                Console.WriteLine($"ERROR: {Structure.Types[0].Name} does not contain a field named \"{fieldName}\".");
                while (ContextStack.Context != previousContext) ContextStack.Pop();
                Owner = previousOwner;
                Structure = previousStructure;
                return false;
            }
            else if (field.FieldType != typeof(PageableResource))
            {
                Console.WriteLine($"ERROR: {Structure.Types[0].Name}.{field.Name} is not of type {nameof(PageableResource)}.");
                while (ContextStack.Context != previousContext) ContextStack.Pop();
                Owner = previousOwner;
                Structure = previousStructure;
                return false;
            }

            var fieldType = field.FieldType;
            var fieldAttrs = field.GetCustomAttributes(typeof(TagFieldAttribute), false);
            var fieldAttr = fieldAttrs?.Length < 1 ? new TagFieldAttribute() : (TagFieldAttribute)fieldAttrs[0];
            var fieldInfo = new TagFieldInfo(field, fieldAttr, uint.MaxValue, uint.MaxValue);
            var fieldValue = field.GetValue(Owner) as PageableResource;

            if (!file.Directory.Exists)
                file.Directory.Create();

            File.WriteAllBytes(file.FullName, CacheContext.ExtractRawResource(fieldValue));
            Console.WriteLine($"Wrote 0x{fieldValue.Page.CompressedBlockSize:X} bytes to \"{file.FullName}\".");

            while (ContextStack.Context != previousContext) ContextStack.Pop();
            Owner = previousOwner;
            Structure = previousStructure;

            return true;
        }
    }
}
