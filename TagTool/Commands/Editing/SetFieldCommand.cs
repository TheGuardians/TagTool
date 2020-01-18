using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Commands.Editing
{
    class SetFieldCommand : Command
    {
        private CommandContextStack ContextStack { get; }
        private GameCache Cache { get; }
        private CachedTag Tag { get; }

        public TagStructureInfo Structure { get; set; }
        public object Owner { get; set; }

        public SetFieldCommand(CommandContextStack contextStack, GameCache cache, CachedTag tag, TagStructureInfo structure, object owner)
            : base(true,

                  "SetField",
                  $"Sets the value of a specific field in the current {structure.Types[0].Name} definition.",

                  "SetField <field name> <field value>",

                  $"Sets the value of a specific field in the current {structure.Types[0].Name} definition.")
        {
            ContextStack = contextStack;
            Cache = cache;
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

            if (fieldName.Contains(".")) // requires EditBlock command + context
            {
                var lastIndex = fieldName.LastIndexOf('.');
                var blockName = fieldName.Substring(0, lastIndex);
                fieldName = fieldName.Substring(lastIndex + 1, (fieldName.Length - lastIndex) - 1);
                fieldNameLow = fieldName.ToLower();
                fieldNameSnake = fieldName.ToSnakeCase();

                var command = new EditBlockCommand(ContextStack, Cache, Tag, Owner);

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
                Console.WriteLine("ERROR: {0} does not contain a field named \"{1}\".", Structure.Types[0].Name, fieldName);
                while (ContextStack.Context != previousContext) ContextStack.Pop();
                Owner = previousOwner;
                Structure = previousStructure;
                return false;
            }

            var fieldType = field.FieldType;
            var fieldAttrs = field.GetCustomAttributes(typeof(TagFieldAttribute), false);
            var fieldAttr = fieldAttrs?.Length < 1 ? new TagFieldAttribute() : (TagFieldAttribute)fieldAttrs[0];
            var fieldInfo = new TagFieldInfo(field, fieldAttr, uint.MaxValue, uint.MaxValue);
            var fieldValue = ParseArgs(Cache, field.FieldType, fieldInfo, args.Skip(1).ToList());

            if (fieldValue != null && fieldValue.Equals(false))
            {
                while (ContextStack.Context != previousContext) ContextStack.Pop();
                Owner = previousOwner;
                Structure = previousStructure;
                return false;
            }

            if (Cache.GetType() == typeof(GameCacheContextHaloOnline) && field.FieldType == typeof(PageableResource))
            {
                var haloOnlineGameCache = (GameCacheContextHaloOnline)Cache;

                var ownerValue = field.GetValue(Owner);

                if (fieldValue == null)
                {
                    field.SetValue(Owner, null);
                }
                else if (ownerValue is PageableResource pageable)
                {
                    var newLocation = ResourceLocation.None;

                    FileInfo resourceFile = null;

                    switch (fieldValue)
                    {
                        case FileInfo file:
                            if (!pageable.GetLocation(out newLocation))
                                newLocation = ResourceLocation.ResourcesB;
                            resourceFile = file;
                            break;

                        case ValueTuple<ResourceLocation, FileInfo> tuple:
                            newLocation = tuple.Item1;
                            resourceFile = tuple.Item2;
                            break;

                        default:
                            throw new FormatException(fieldValue.ToString());
                    }

                    ResourceCacheHaloOnline oldCache = null;

                    if (pageable.GetLocation(out var oldLocation))
                        oldCache = new ResourceCacheHaloOnline(haloOnlineGameCache.Version, haloOnlineGameCache.ResourceCaches.OpenResourceCacheReadWrite(oldLocation));

                    var newCache = new ResourceCacheHaloOnline(haloOnlineGameCache.Version, haloOnlineGameCache.ResourceCaches.OpenResourceCacheReadWrite(newLocation));

                    var data = File.ReadAllBytes(resourceFile.FullName);

                    pageable.Page.UncompressedBlockSize = (uint)data.Length;

                    if (oldLocation == newLocation && pageable.Page.Index != -1)
                    {
                        using (var stream = haloOnlineGameCache.ResourceCaches.OpenResourceCacheReadWrite(oldLocation))
                        {
                            pageable.Page.CompressedBlockSize = oldCache.Compress(stream, pageable.Page.Index, data);
                        }
                    }
                    else
                    {
                        using (var destStream = haloOnlineGameCache.ResourceCaches.OpenResourceCacheReadWrite(newLocation))
                        {
                            pageable.Page.Index = newCache.Add(destStream, data, out pageable.Page.CompressedBlockSize);
                        }

                        pageable.ChangeLocation(newLocation);
                    }

                    pageable.DisableChecksum();

                    field.SetValue(Owner, fieldValue = pageable);
                }
            }
            else
            {
                field.SetValue(Owner, fieldValue);
            }
            
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
                else if (fieldType == typeof(StringId))
                    valueString = Cache.StringTable.GetString((StringId)fieldValue);
                else if (fieldType == typeof(CachedTag))
                {
                    var instance = (CachedTag)fieldValue;

                    var tagName = instance?.Name ?? $"0x{instance.Index:X4}";

                    valueString = $"[0x{instance.Index:X4}] {tagName}.{Cache.StringTable.GetString(instance.Group.Name)}";
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
                else if (fieldInfo.FieldType.IsArray && fieldInfo.Attribute.Length != 0)
                {
                    valueString = fieldValue == null ? "null" : $"[{fieldInfo.Attribute.Length}] {{ ";
                    var valueArray = (Array)fieldValue;

                    if (fieldValue != null)
                    {
                        for (var i = 0; i < fieldInfo.Attribute.Length; i++)
                            valueString += $"{valueArray.GetValue(i)}{((i + 1) < fieldInfo.Attribute.Length ? "," : "")} ";

                        valueString += "}";
                    }
                }
                else if (fieldType.GetInterface(typeof(IList).Name) != null)
                    valueString =
                        ((IList)fieldValue).Count != 0 ?
                            $"{{...}}[{((IList)fieldValue).Count}]" :
                        "null";
                else
                    valueString = fieldValue.ToString();
#if !DEBUG
            }
            catch (Exception e)
            {
                valueString = $"<ERROR MESSAGE=\"{e.Message}\" />";
            }
#endif

            var fieldFullName = $"{field.DeclaringType.FullName}.{field.Name}".Replace("+", ".");
            var documentationNode = EditTagContextFactory.Documentation.SelectSingleNode($"//member[starts-with(@name, 'F:{fieldFullName}')]");

            Console.WriteLine("{0}: {1} = {2} {3}", field.Name, typeString, valueString,
                documentationNode != null ?
                    $":: {documentationNode.FirstChild.InnerText.Replace("\r\n", "").TrimStart().TrimEnd()}" :
                    "");

            while (ContextStack.Context != previousContext) ContextStack.Pop();
            Owner = previousOwner;
            Structure = previousStructure;

            return true;
        }

        public static object ParseArgs(GameCache cache, Type type, TagFieldInfo info, List<string> args)
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
            else if (type.IsArray)
            {
                if (info?.FieldType == typeof(byte[]) && info?.Attribute.Length == 0)
                {   // tag_data field
                    if (args.Count != 1)
                        return false;
                    if (input.Length % 2 != 0)
                        return false;

                    List<byte> bytes = new List<byte>();

                    for (int i = 0; i < input.Length; i += 2)
                        bytes.Add(Convert.ToByte(input.Substring(i, 2), 16));

                    output = bytes.ToArray();
                }
                else
                {
                    if (info == null || args.Count != info.Attribute.Length)
                        return false;

                    var elementType = info.FieldType.GetElementType();
                    var values = Array.CreateInstance(elementType, info.Attribute.Length);

                    for (var i = 0; i < info.Attribute.Length; i++)
                        values.SetValue(Convert.ChangeType(ParseArgs(cache, elementType, null, new List<string> { args[i] }), elementType), i);

                    return values;
                }
            }
            else if (type.IsBlamType())
            {
                if (type.IsGenericType)
                {
                    var tDefinition = type.GetGenericTypeDefinition();
                    var tArguments = type.GetGenericArguments();
                    type = tDefinition.MakeGenericType(tArguments);
                }

                var blamType = Activator.CreateInstance(type) as IBlamType;
                if (!blamType.TryParse(cache, args, out blamType, out string error))
                    Console.WriteLine(error);
                return blamType;
            }
            else if (type == typeof(CachedTag))
            {
                if (args.Count != 1 || !cache.TryGetCachedTag(args[0], out var tagInstance))
                    return false;
                output = tagInstance;
            }
            else if (cache.GetType() == typeof(GameCacheContextHaloOnline) && type == typeof(PageableResource))
            {
                if (args.Count < 1 || args.Count > 2)
                    return false;

                if (args.Count == 1)
                {
                    switch (args[0].ToLower())
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
                else if (args.Count == 2)
                {
                    var resourceLocation = ResourceLocation.None;

                    switch (args[0].ToSnakeCase())
                    {
                        case "resources":
                            resourceLocation = ResourceLocation.Resources;
                            break;

                        case "textures":
                            resourceLocation = ResourceLocation.Textures;
                            break;

                        case "textures_b":
                            resourceLocation = ResourceLocation.TexturesB;
                            break;

                        case "audio":
                            resourceLocation = ResourceLocation.Audio;
                            break;

                        case "resources_b":
                            resourceLocation = ResourceLocation.ResourcesB;
                            break;

                        case "render_models" when cache.Version >= CacheVersion.HaloOnline235640:
                            resourceLocation = ResourceLocation.RenderModels;
                            break;

                        case "lightmaps" when cache.Version >= CacheVersion.HaloOnline235640:
                            resourceLocation = ResourceLocation.Lightmaps;
                            break;

                        default:
                            throw new FormatException($"Invalid resource location: {args[0]}");
                    }
                    
                    var resourceFile = new FileInfo(args[1]);

                    if (!resourceFile.Exists)
                        throw new FileNotFoundException(args[1]);

                    output = (resourceLocation, resourceFile);
                }
                else throw new NotImplementedException();
            }
            else
            {
                Console.WriteLine($"ERROR: Not Implemented.");
                return false;
                // throw new NotImplementedException();
            }

            return output;
        }
        
        public static int RangeArgCount(Type type)
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
                type == typeof(CachedTag) ||
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