using TagTool.Cache;
using TagTool.Tags;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TagTool.Common;
using TagTool.Tags.Definitions;
using static TagTool.Tags.TagFieldFlags;

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

                  "ForEach <Tag Block> [From: *] [To: *] <Command>",

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

            var field = TagStructure.GetTagFieldEnumerable(Structure)
                .Find(f =>
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

            string fromName = null;
            int? from = null;

            string toName = null;
            int? to = null;

            while (args.Count > 1)
            {
                var found = false;

                switch (args[1].ToLower())
                {
                    case "from:":
                        if (char.IsNumber(args[2][0]))
                            from = int.Parse(args[2]);
                        else
                        {
                            fromName = args[2];
                            from = FindLabelIndex(fieldValue, fromName);
                        }
                        args.RemoveRange(1, 2);
                        found = true;
                        break;

                    case "to:":
                        if (char.IsNumber(args[2][0]))
                            to = int.Parse(args[2]);
                        else
                        {
                            toName = args[2];
                            to = FindLabelIndex(fieldValue, toName);
                        }
                        args.RemoveRange(1, 2);
                        found = true;
                        break;
                }

                if (!found)
                    break;
            }

            blockName = args[0];
            args.RemoveRange(0, 1);

            var commandsToExecute = new List<List<string>>();

            // if no command is given, keep reading commands from stdin until an empty line encountered
            if (args.Count < 1)
            {
                string line;
                while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
                {
                    var commandsArgs = ArgumentParser.ParseCommand(line, out string redirectFile);
                    commandsToExecute.Add(commandsArgs);
                }
            }
            else
            {
                commandsToExecute.Add(args);
            }

            for (var i = (from ?? 0);
                i < (to.HasValue ? to.Value + 1 : fieldValue.Count);
                i++)
            {
                while (ContextStack.Context != previousContext)
                    ContextStack.Pop();

                Owner = previousOwner;
                Structure = previousStructure;

                if (blockName != "" && new EditBlockCommand(ContextStack, CacheContext, Tag, Owner)
                        .Execute(new List<string> { $"{blockName}[{i}]" })
                        .Equals(false))
                    return false;

                var label = GetLabel(fieldValue, i);

                Console.Write(label == null ? $"[{i}] " : $"[{label} ({i})] ");
                foreach(var command in commandsToExecute)
                    ContextStack.Context.GetCommand(command[0]).Execute(command.Skip(1).ToList());
            }

            while (ContextStack.Context != previousContext)
                ContextStack.Pop();

            Owner = previousOwner;
            Structure = previousStructure;

            return true;
        }

        private string GetLabel(IList elements, int index)
        {
            if (index < 0 || index >= elements.Count)
                return null;

            foreach (var info in TagStructure.GetTagFieldEnumerable(elements.GetType().GetGenericArguments()[0], CacheContext.Version))
            {
                if (info.Attribute == null || !info.Attribute.Flags.HasFlag(Label))
                    continue;

                var value = info.FieldInfo.GetValue(elements[index]);

                if (info.FieldType == typeof(string))
                    return (string)value;
                else if (info.FieldType == typeof(StringId))
                    return CacheContext.GetString((StringId)value);
                else if (info.FieldType.IsPrimitive && Tag.IsInGroup<Scenario>())
                    return GetLabel((IList)typeof(Scenario).GetField(nameof(Scenario.ObjectNames)).GetValue(Owner), Convert.ToInt32(value));
                else
                    return value.ToString();
            }

            return null;
        }

        private int FindLabelIndex(IList elements, string displayName)
        {
            for (var i = 0; i < elements.Count; i++)
            {
                var label = GetLabel(elements, i);

                if (label == null)
                    continue;

                if (displayName == label)
                    return i;
            }

            throw new KeyNotFoundException(displayName);
        }
    }
}