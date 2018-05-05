using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags;
using ResourceLocation = TagTool.Common.ResourceLocation;

namespace TagTool.Commands.Editing
{
    class SetFieldCommand : Command
    {
        private CommandContextStack ContextStack { get; }
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }

        public TagStructureInfo Structure { get; set; }
        public object Owner { get; set; }

        public SetFieldCommand(CommandContextStack contextStack, GameCacheContext cacheContext, CachedTagInstance tag, TagStructureInfo structure, object owner)
            : base(CommandFlags.Inherit,

                  "SetField",
                  $"Sets the value of a specific field in the current {structure.Types[0].Name} definition.",

                  "SetField <field name> <field value>",

                  $"Sets the value of a specific field in the current {structure.Types[0].Name} definition.")
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

            var enumerator = new TagFieldEnumerator(Structure);

            var field = enumerator.Find(f =>
                f.Name == fieldName ||
                f.Name.ToLower() == fieldNameLow ||
                f.Name.ToSnakeCase() == fieldNameSnake);

            if (field == null)
            {
                Console.WriteLine("ERROR: {0} does not contain a field named \"{1}\".", Structure.Types[0].Name, fieldName);
                while (ContextStack.Context != previousContext) ContextStack.Pop();
                Owner = previousOwner;
                Structure = previousStructure;
                return false;
            }

            var fieldType = enumerator.Field.FieldType;
            var fieldValue = enumerator.Field.GetValue(Owner);

            var typeString =
                fieldType.IsGenericType ?
                    $"{fieldType.Name}<{fieldType.GenericTypeArguments[0].Name}>" :
                fieldType.Name;

            string valueString;

#if !DEBUG
            try
            {
#endif
                if (fieldValue == null)
                    valueString = "null";
                else if (fieldType.GetInterface(typeof(IList).Name) != null)
                    valueString =
                        ((IList)fieldValue).Count != 0 ?
                            $"{{...}}[{((IList)fieldValue).Count}]" :
                        "null";
                else if (fieldType == typeof(StringId))
                    valueString = CacheContext.GetString((StringId)fieldValue);
                else if (fieldType == typeof(CachedTagInstance))
                {
                    var instance = (CachedTagInstance)fieldValue;

                    var tagName = CacheContext.TagNames.ContainsKey(instance.Index) ?
                        CacheContext.TagNames[instance.Index] :
                        $"0x{instance.Index:X4}";

                    valueString = $"[0x{instance.Index:X4}] {tagName}.{CacheContext.GetString(instance.Group.Name)}";
                }
                else if (fieldType == typeof(TagFunction))
                {
                    var function = (TagFunction)fieldValue;
                    valueString = "";
                    foreach (var datum in function.Data)
                        valueString += datum.ToString("X2");
                }
                else if (fieldType == typeof(PageableResource))
                {
                    var pageable = (PageableResource)fieldValue;
                    pageable.GetLocation(out var location);
                    valueString = pageable == null ? "null" : $"{{ Location: {location}, Index: 0x{pageable.Page.Index:X4}, CompressedSize: 0x{pageable.Page.CompressedBlockSize:X8} }}";
                }
                else
                    valueString = fieldValue.ToString();
#if !DEBUG
            }
            catch (Exception e)
            {
                valueString = $"<ERROR MESSAGE=\"{e.Message}\" />";
            }
#endif

            var fieldFullName = $"{enumerator.Field.DeclaringType.FullName}.{enumerator.Field.Name}".Replace("+", ".");
            var documentationNode = EditTagContextFactory.Documentation.SelectSingleNode($"//member[starts-with(@name, 'F:{fieldFullName}')]");

            Console.WriteLine("{0}: {1} = {2} {3}", enumerator.Field.Name, typeString, valueString,
                documentationNode != null ?
                    $":: {documentationNode.FirstChild.InnerText.Replace("\r\n", "").TrimStart().TrimEnd()}" :
                    "");

            while (ContextStack.Context != previousContext) ContextStack.Pop();
            Owner = previousOwner;
            Structure = previousStructure;

            return true;
        }

        public object ParseArgs(Type type, List<string> args)
        {
            var input = args[0];
            object output = null;

            if (type == typeof(byte))
            {
                if (args.Count != 1)
                    return false;
                if (!byte.TryParse(input, out byte value))
                    return false;
                output = value;
            }
            else if (type == typeof(sbyte))
            {
                if (args.Count != 1)
                    return false;
                if (!sbyte.TryParse(input, out sbyte value))
                    return false;
                output = value;
            }
            else if (type == typeof(short))
            {
                if (args.Count != 1)
                    return false;
                if (!short.TryParse(input, out short value))
                    return false;
                output = value;
            }
            else if (type == typeof(ushort))
            {
                if (args.Count != 1)
                    return false;
                if (!ushort.TryParse(input, out ushort value))
                    return false;
                output = value;
            }
            else if (type == typeof(int))
            {
                if (args.Count != 1)
                    return false;
                if (!int.TryParse(input, out int value))
                    return false;
                output = value;
            }
            else if (type == typeof(uint))
            {
                if (args.Count != 1)
                    return false;
                if (!uint.TryParse(input, out uint value))
                    return false;
                output = value;
            }
            else if (type == typeof(long))
            {
                if (args.Count != 1)
                    return false;
                if (!long.TryParse(input, out long value))
                    return false;
                output = value;
            }
            else if (type == typeof(ulong))
            {
                if (args.Count != 1)
                    return false;
                if (!ulong.TryParse(input, out ulong value))
                    return false;
                output = value;
            }
            else if (type == typeof(float))
            {
                if (args.Count != 1)
                    return false;
                if (!float.TryParse(input, out float value))
                    return false;
                output = value;
            }
            else if (type == typeof(string))
            {
                if (args.Count != 1)
                    return false;
                output = input;
            }
            else if (type == typeof(CachedTagInstance))
            {
                if (args.Count != 1)
                    return false;
                output = ArgumentParser.ParseTagSpecifier(CacheContext, input);
            }
            else if (type == typeof(StringId))
            {
                if (args.Count != 1)
                    return false;
                output = CacheContext.GetStringId(input);
            }
            else if (type == typeof(Angle))
            {
                if (args.Count != 1)
                    return false;
                if (!float.TryParse(input, out float value))
                    return false;
                output = Angle.FromDegrees(value);
            }
            else if (type == typeof(RealEulerAngles2d))
            {
                if (args.Count != 2)
                    return false;
                if (!float.TryParse(args[0], out float yaw) ||
                    !float.TryParse(args[1], out float pitch))
                    return false;
                output = new RealEulerAngles2d(
                    Angle.FromDegrees(yaw),
                    Angle.FromDegrees(pitch));
            }
            else if (type == typeof(RealEulerAngles3d))
            {
                if (args.Count != 3)
                    return false;
                if (!float.TryParse(args[0], out float yaw) ||
                    !float.TryParse(args[1], out float pitch) ||
                    !float.TryParse(args[2], out float roll))
                    return false;
                output = new RealEulerAngles3d(
                    Angle.FromDegrees(yaw),
                    Angle.FromDegrees(pitch),
                    Angle.FromDegrees(roll));
            }
            else if (type == typeof(RealPoint2d))
            {
                if (args.Count != 2)
                    return false;
                if (!float.TryParse(args[0], out float x) ||
                    !float.TryParse(args[1], out float y))
                    return false;
                output = new RealPoint2d(x, y);
            }
            else if (type == typeof(RealPoint3d))
            {
                if (args.Count != 3)
                    return false;
                if (!float.TryParse(args[0], out float x) ||
                    !float.TryParse(args[1], out float y) ||
                    !float.TryParse(args[2], out float z))
                    return false;
                output = new RealPoint3d(x, y, z);
            }
            else if (type == typeof(RealVector2d))
            {
                if (args.Count != 2)
                    return false;
                if (!float.TryParse(args[0], out float i) ||
                    !float.TryParse(args[1], out float j))
                    return false;
                output = new RealVector2d(i, j);
            }
            else if (type == typeof(RealVector3d))
            {
                if (args.Count != 3)
                    return false;
                if (!float.TryParse(args[0], out float i) ||
                    !float.TryParse(args[1], out float j) ||
                    !float.TryParse(args[2], out float k))
                    return false;
                output = new RealVector3d(i, j, k);
            }
            else if (type == typeof(RealQuaternion))
            {
                if (args.Count != 4)
                    return false;
                if (!float.TryParse(args[0], out float i) ||
                    !float.TryParse(args[1], out float j) ||
                    !float.TryParse(args[2], out float k) ||
                    !float.TryParse(args[3], out float w))
                    return false;
                output = new RealQuaternion(i, j, k, w);
            }
            else if (type == typeof(RealPlane2d))
            {
                if (args.Count != 3)
                    return false;
                if (!float.TryParse(args[0], out float i) ||
                    !float.TryParse(args[1], out float j) ||
                    !float.TryParse(args[2], out float d))
                    return false;
                output = new RealPlane2d(i, j, d);
            }
            else if (type == typeof(RealPlane3d))
            {
                if (args.Count != 4)
                    return false;
                if (!float.TryParse(args[0], out float i) ||
                    !float.TryParse(args[1], out float j) ||
                    !float.TryParse(args[2], out float k) ||
                    !float.TryParse(args[3], out float d))
                    return false;
                output = new RealPlane3d(i, j, k, d);
            }
            else if (type.IsEnum)
            {
                if (args.Count != 1)
                    return false;

                var query = args[0];

                object found;

                try
                {
                    found = Enum.Parse(type, query);
                }
                catch
                {
                    found = null;
                }

                var names = Enum.GetNames(type).ToList();

                if (found == null)
                {
                    var nameLow = query.ToLower();
                    var namesLow = names.Select(i => i.ToLower()).ToList();

                    found = namesLow.Find(n => n == nameLow);

                    if (found == null)
                    {
                        var nameSnake = query.ToSnakeCase();
                        var namesSnake = names.Select(i => i.ToSnakeCase()).ToList();
                        found = namesSnake.Find(n => n == nameSnake);

                        if (found == null)
                        {
                            Console.WriteLine("Invalid {0} enum option: {1}", type.Name, args[0]);
                            Console.WriteLine("");

                            Console.WriteLine("Valid options:");
                            foreach (var name in Enum.GetNames(type))
                            {
                                var fieldName = $"{type.FullName}.{name}".Replace("+", ".");
                                var documentationNode = EditTagContextFactory.Documentation.SelectSingleNode($"//member[starts-with(@name, 'F:{fieldName}')]");

                                Console.WriteLine("\t{0} {1}", name,
                                    documentationNode != null ?
                                        $":: {documentationNode.FirstChild.InnerText.Replace("\r\n", "").TrimStart().TrimEnd()}" :
                                        "");
                            }
                            Console.WriteLine();

                            return false;
                        }
                        else
                        {
                            found = Enum.Parse(type, names[namesSnake.IndexOf((string)found)]);
                        }
                    }
                    else
                    {
                        found = Enum.Parse(type, names[namesLow.IndexOf((string)found)]);
                    }
                }

                output = found;
            }
            else if (type == typeof(Bounds<>))
            {
                var rangeType = type.GenericTypeArguments[0];
                var argCount = RangeArgCount(rangeType);

                var min = ParseArgs(rangeType, args.Take(argCount).ToList());

                if (min.Equals(false))
                    return false;

                var max = ParseArgs(rangeType, args.Skip(argCount).Take(argCount).ToList());

                if (max.Equals(false))
                    return false;

                output = Activator.CreateInstance(type, new object[] { min, max });
            }
            else if (type == typeof(Bounds<float>))
            {
                var rangeType = type.GenericTypeArguments[0];
                var argCount = RangeArgCount(rangeType);

                var min = ParseArgs(rangeType, args.Take(argCount).ToList());

                if (min.Equals(false))
                    return false;

                var max = ParseArgs(rangeType, args.Skip(argCount).Take(argCount).ToList());

                if (max.Equals(false))
                    return false;

                output = Activator.CreateInstance(type, new object[] { min, max });
            }
            else if (type == typeof(Bounds<Single>))
            {
                var rangeType = type.GenericTypeArguments[0];
                var argCount = RangeArgCount(rangeType);

                var min = ParseArgs(rangeType, args.Take(argCount).ToList());

                if (min.Equals(false))
                    return false;

                var max = ParseArgs(rangeType, args.Skip(argCount).Take(argCount).ToList());

                if (max.Equals(false))
                    return false;

                output = Activator.CreateInstance(type, new object[] { min, max });
            }
            else if (type.IsArray)
            {
                if (args.Count != 1)
                    return false;
                if (input.Length % 2 != 0)
                    return false;

                List<byte> bytes = new List<byte>();

                for (int i = 0; i < input.Length; i = i + 2)
                    bytes.Add(Convert.ToByte(input.Substring(i, 2), 16));

                output = bytes.ToArray();
            }
            else if (type == typeof(RealRgbColor))
            {
                if (args.Count != 3)
                    return false;
                if (!float.TryParse(args[0], out float i) ||
                    !float.TryParse(args[1], out float j) ||
                    !float.TryParse(args[2], out float k))
                    return false;
                output = new RealRgbColor(i, j, k);
            }
            else if (type == typeof(Bounds<Angle>))
            {
                if (args.Count != 2)
                    return false;

                if (!float.TryParse(args[0], out float i) ||
                    !float.TryParse(args[1], out float j))
                    return false;

                output = new Bounds<Angle> { Lower = Angle.FromDegrees(i), Upper = Angle.FromDegrees(j) };
            }
            else if (type == typeof(PageableResource))
            {
                if (args.Count != 1)
                    return false;

                var value = args[0].ToLower();

                switch (value)
                {
                    case "null":
                        output = null;
                        break;

                    default:
                        output = new FileInfo(args[0]);
                        if (!((FileInfo)output).Exists)
                            throw new FileNotFoundException(args[0]);
                        break;
                }
            }
            else
            {
                Console.WriteLine($"ERROR: Not Implemented.");
                return false;
                // throw new NotImplementedException();
            }

            return output;
        }

        private PageableResource SetResourceData(PageableResource pageable, FileInfo file)
        {
            throw new NotImplementedException();
        }

        private int RangeArgCount(Type type)
        {
            if (type.IsEnum ||
                type == typeof(byte) ||
                type == typeof(sbyte) ||
                type == typeof(short) ||
                type == typeof(ushort) ||
                type == typeof(int) ||
                type == typeof(uint) ||
                type == typeof(long) ||
                type == typeof(ulong) ||
                type == typeof(float) ||
                type == typeof(string) ||
                type == typeof(CachedTagInstance) ||
                type == typeof(StringId) ||
                type == typeof(Angle))
                return 1;
            else if (type == typeof(RealEulerAngles2d))
                return 2;
            else if (type == typeof(RealEulerAngles3d))
                return 3;
            else if (type == typeof(RealPoint2d))
                return 2;
            else if (type == typeof(RealPoint3d))
                return 3;
            else if (type == typeof(RealQuaternion))
                return 4;
            else throw new NotImplementedException();
        }
    }
}