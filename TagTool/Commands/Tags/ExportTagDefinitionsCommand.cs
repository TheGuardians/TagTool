using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Commands.Tags
{
    class ExportTagDefinitionsCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public ExportTagDefinitionsCommand(HaloOnlineCacheContext cacheContext) :
            base(true,
                
                "ExportTagDefinitions",
                "Exports all internal tag definitions for use in ElDewrito.",
                
                "ExportTagDefinitions <Dest Dir>",
                "Exports all internal tag definitions for use in ElDewrito.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var destDir = new DirectoryInfo(args[0]);

            if (!destDir.Exists)
                destDir.Create();

            foreach (var entry in TagGroup.Instances)
            {
                if (entry.Key.Value == -1)
                    continue;

                var tagGroupName = CacheContext.GetString(entry.Value.Name).ToSnakeCase();
                var tagStructureInfo = TagStructure.GetTagStructureInfo(TagDefinition.Find(entry.Key), CacheContext.Version);

                foreach (var type in tagStructureInfo.Types.Reverse<Type>())
                {
                    var info = type.GetCustomAttributes<TagStructureAttribute>(false).Where(attr =>
                        CacheVersionDetection.IsBetween(CacheContext.Version, attr.MinVersion, attr.MaxVersion)).First();

                    if (info.Name == null)
                    {
                        Console.WriteLine($"WARNING: {type.FullName} has no tag structure name defined!");
                        continue;
                    }

                    ExportType(type, new FileInfo(Path.Combine(destDir.FullName, $"{info.Name}.hpp")));
                }
            }

            return true;
        }

        private class ExportedType
        {
            public string Name;
            public string File;
        }

        private Dictionary<string, ExportedType> ExportedTypes = new Dictionary<string, ExportedType>();

        private ExportedType ExportType(Type type, FileInfo file)
        {
            if (file.Name == ".hpp")
                return null;

            if (ExportedTypes.ContainsKey(type.FullName))
                return ExportedTypes[type.FullName];

            using (var writer = file.CreateText())
            {
                writer.WriteLine("#pragma once");
                writer.WriteLine("#include \"../cseries/cseries.hpp\"");
                writer.WriteLine("#include \"../math/real_math.hpp\"");
                writer.WriteLine("#include \"../tag_files/tag_groups.hpp\"");

                foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).OrderBy(i => i.MetadataToken))
                {
                    if (field.FieldType.IsPrimitive)
                        continue;

                    var info = type.GetCustomAttributes<TagStructureAttribute>(false).Where(attr =>
                        CacheVersionDetection.IsBetween(CacheContext.Version, attr.MinVersion, attr.MaxVersion)).First();

                    var typeBaseName = field.FieldType.FullName.ToSnakeCase().Replace("._", ".");
                    typeBaseName = typeBaseName.Substring(typeBaseName.LastIndexOf('.') + 1).Replace("+", "");

                    while (typeBaseName.StartsWith("_"))
                        typeBaseName = typeBaseName.Substring(1);

                    ExportedType typeInfo = null;

                    if (ExportedTypes.ContainsKey(field.FieldType.FullName))
                    {
                        continue;
                    }
                    else if (field.FieldType.IsArray)
                    {
                    }
                    else if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        typeBaseName = field.FieldType.GetGenericArguments()[0].FullName
                            .ToSnakeCase().Replace("._", ".").Replace("+", "");

                        typeBaseName = typeBaseName.Substring(typeBaseName.LastIndexOf('.') + 1);

                        while (typeBaseName.StartsWith("_"))
                            typeBaseName = typeBaseName.Substring(1);

                        typeInfo = ExportedTypes[field.FieldType.GetGenericArguments()[0].FullName] = new ExportedType
                        {
                            Name = $"s_{typeBaseName}",
                            File = file.FullName
                        };
                    }
                    else if (field.FieldType.IsEnum)
                    {
                        if (typeBaseName.EndsWith("_value"))
                            typeBaseName = typeBaseName.Substring(0, typeBaseName.Length - 6);

                        typeInfo = ExportedTypes[field.FieldType.FullName] = new ExportedType
                        {
                            Name = $"e_{typeBaseName}",
                            File = file.FullName
                        };
                    }
                    else if (field.FieldType.IsSubclassOf(typeof(TagStructure)))
                    {
                        typeInfo = ExportedTypes[field.FieldType.FullName] = new ExportedType
                        {
                            Name = $"s_{typeBaseName}",
                            File = file.FullName
                        };
                    }

                    if (field.FieldType.IsArray)
                    {
                    }
                    else if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        writer.WriteLine();

                        if (typeInfo.Name.Contains('.'))
                            break;

                        writer.WriteLine($"struct {typeInfo.Name}");
                        writer.WriteLine("{");
                        writer.WriteLine("};");
                    }
                    else if (field.FieldType.IsEnum)
                    {
                        //
                        // Export the enum
                        //

                        writer.WriteLine();

                        writer.WriteLine($"enum {typeInfo.Name}");
                        writer.WriteLine("{");

                        var isFlags = field.FieldType.GetCustomAttribute<FlagsAttribute>(false) != null;

                        foreach (var option in Enum.GetValues(field.FieldType))
                        {
                            var optionValue = (int)Convert.ChangeType(option, typeof(int));

                            if (isFlags)
                            {
                                if (optionValue == 0)
                                    continue;

                                var bitSet = false;
                                for (var i = 0; i < 32; i++)
                                {
                                    if ((optionValue & (1 << i)) != 0)
                                    {
                                        if (bitSet)
                                            throw new NotSupportedException("multiple bits set in flags enum");

                                        optionValue = i;
                                        bitSet = true;
                                    }
                                }
                            }

                            writer.WriteLine($"\t_{typeBaseName}_{option.ToString().ToSnakeCase()} = {optionValue},");
                        }

                        writer.WriteLine("};");
                    }
                    else if (field.FieldType.IsSubclassOf(typeof(TagStructure)))
                    {
                        writer.WriteLine();

                        writer.WriteLine($"struct {typeInfo.Name}");
                        writer.WriteLine("{");
                        writer.WriteLine("};");
                    }
                }
            }

            return ExportedTypes.ContainsKey(type.FullName) ? ExportedTypes[type.FullName] : null;
        }
    }
}
