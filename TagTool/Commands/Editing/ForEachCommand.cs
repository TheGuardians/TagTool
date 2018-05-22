using TagTool.Cache;
using TagTool.Commands;
using TagTool.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TagTool.Commands.Editing
{
    class ForEachCommand : Command
    {
        private CommandContextStack ContextStack { get; }
        private HaloOnlineCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }

        public TagStructureInfo Structure { get; set; }
        public object Owner { get; set; }

        public ForEachCommand(CommandContextStack contextStack, HaloOnlineCacheContext cacheContext, CachedTagInstance tag, TagStructureInfo structure, object owner)
            : base(true,

                  "ForEach",
                  "Executes a command for each element in the specified tag block.",

                  "ForEach <Tag Block> <Command>",

                  "Executes a command for each element in the specified tag block.")
        {
            ContextStack = contextStack;
            CacheContext = cacheContext;
            Tag = tag;
            Structure = structure;
            Owner = owner;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1)
                return false;

            var fieldName = args[0];
            var fieldNameLow = fieldName.ToLower();
            var fieldNameSnake = fieldName.ToSnakeCase();

            var previousContext = ContextStack.Context;
            var previousOwner = Owner;
            var previousStructure = Structure;

            var blockName = "";

            if (fieldName.Contains("."))
            {
                var lastIndex = fieldName.LastIndexOf('.');
                blockName = fieldName.Substring(0, lastIndex);
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

            var enumerator = new TagFieldEnumerator(Structure);

            var field = enumerator.Find(f =>
                f.Name == fieldName ||
                f.Name.ToLower() == fieldNameLow ||
                f.Name.ToSnakeCase() == fieldNameSnake);

            var ownerType = Owner.GetType();

            if (field == null)
            {
                Console.WriteLine("{0} does not contain a block named \"{1}\"", ownerType.Name, fieldName);
                return false;
            }

            IList fieldValue = null;

            if (field.FieldType.GetInterface("IList") == null || (fieldValue = (IList)field.GetValue(Owner)) == null)
            {
                Console.WriteLine("{0} does not contain a block named \"{1}\"", ownerType.Name, fieldName);
                return false;
            }

            blockName = args[0];
            args = args.Skip(1).ToList();

            for (var i = 0; i < fieldValue.Count; i++)
            {
                while (ContextStack.Context != previousContext)
                    ContextStack.Pop();

                Owner = previousOwner;
                Structure = previousStructure;

                if (blockName != "" && new EditBlockCommand(ContextStack, CacheContext, Tag, Owner)
                        .Execute(new List<string> { $"{blockName}[{i}]" })
                        .Equals(false))
                    return false;

                Console.Write($"[{i}] ");
                ContextStack.Context.GetCommand(args[0]).Execute(args.Skip(1).ToList());
            }

            while (ContextStack.Context != previousContext)
                ContextStack.Pop();

            Owner = previousOwner;
            Structure = previousStructure;

            return true;
        }
    }
}