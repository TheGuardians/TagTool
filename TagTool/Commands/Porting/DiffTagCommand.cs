using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Commands.Porting
{
    class DiffTagCommand : Command
    {
        private GameCache Cache1 { get; }
        private GameCache Cache2 { get; }

        public DiffTagCommand(GameCache cache1, GameCache cache2)
            : base(true,
                   "DiffTag",
                   "Deep compares two tags and lists their differences.",

                   "DiffTag [simple] <Tag> [OtherTag]",

                   "Deep compares two tags and lists their differences. Use the \"simple\" argument to list only the difference count.")
        {
            Cache1 = cache1;
            Cache2 = cache2;
        }

        public override object Execute(List<string> args)
        {
            bool simple = false;

            if (args.Count < 1)
                return new TagToolError(CommandError.ArgCount);

            for (int i = 0; i < args.Count; i++)
            {
                var arg = args[i].ToLower();
                if (arg == "simple")
                {
                    simple = true;
                    args.RemoveAt(i);
                }
            }

            if (!Cache1.TagCache.TryGetCachedTag(args[0], out CachedTag tag1))
                return new TagToolError(CommandError.TagInvalid, $"\"{args[0]}\"");

            string tag2name;

            if (tag1.Name.StartsWith("ms30\\") && args.Count == 1)
            {
                tag2name = args[0].Replace("ms30\\", "");
            }
            else
                tag2name = args[1];

            if (!Cache2.TagCache.TryGetCachedTag(tag2name, out CachedTag tag2))
                return new TagToolError(CommandError.TagInvalid, $"\"{(args.Count > 1 ? args[1] : tag2name)}\"");

            var differences = new List<Difference>();

            using (var stream1 = Cache1.OpenCacheRead())
            using (var stream2 = Cache2.OpenCacheRead())
                DiffTags(differences, stream1, Cache1, tag1, stream2, Cache2, tag2);

            if (!simple)
            {
                Console.WriteLine($"\n{differences.Count} differences:\n");

                foreach (var diff in differences)
                {
                    var value1 = diff.Value1 ?? "null";
                    var value2 = diff.Value2 ?? "null";

                    if (diff.Kind == DifferenceKind.ElementCount)
                        Console.WriteLine($"{diff.Path}.Count {((IList)diff.Value1).Count} | {((IList)diff.Value2).Count}");
                    else
                        Console.WriteLine($"{diff.Path} {value1} | {value2}");
                }
            }
            else
            {
                Console.WriteLine($"\n{differences.Count} differences.");
            }

            return true;
        }

        enum DifferenceKind
        {
            Value,
            ElementCount,
        }

        class Difference
        {
            public DifferenceKind Kind { get; set; }
            public string Path { get; set; }
            public object Value1 { get; set; }
            public object Value2 { get; set; }

            public Difference(DifferenceKind kind, string path, object value1, object value2)
            {
                Kind = kind;
                Path = path;
                Value1 = value1;
                Value2 = value2;
            }
        }

        static void DiffTags(List<Difference> differences, Stream stream1, GameCache cache1, CachedTag tag1, Stream stream2, GameCache cache2, CachedTag tag2)
        {
            var def1 = cache1.Deserialize(stream1, tag1);
            var def2 = cache2.Deserialize(stream2, tag2);
            DiffData(differences, def1.GetType(), cache1, def1, cache2, def2);
        }

        static void DiffData(List<Difference> differences, Type type, GameCache cache1, object data1, GameCache cache2, object data2, string path = "")
        {
            if (data1 == data2)
                return;

            if (data1 == null && data2 != null || data2 == null && data1 != null)
            {
                differences.Add(new Difference(DifferenceKind.Value, path, data1, data2));
                return;
            }

            if (type.IsPrimitive)
            {
                if (typeof(float).IsAssignableFrom(type))
                {
                    if (Math.Abs((float)data1 - (float)data2) >= 0.00001f)
                        differences.Add(new Difference(DifferenceKind.Value, path, data1, data2));
                }
                else if (typeof(double).IsAssignableFrom(type))
                {
                    if (Math.Abs((double)data1 - (double)data2) >= 0.0000001)
                        differences.Add(new Difference(DifferenceKind.Value, path, data1, data2));
                }
                else
                {
                    if (!data1.Equals(data2))
                        differences.Add(new Difference(DifferenceKind.Value, path, data1, data2));
                }
            }
            else
            {
                if (typeof(TagStructure).IsAssignableFrom(type))
                {
                    var struct1 = (TagStructure)data1;
                    var struct2 = (TagStructure)data2;
                    var fields1 = struct1.GetTagFieldEnumerable(cache1.Version, cache1.Platform);
                    var fields2 = struct2.GetTagFieldEnumerable(cache2.Version, cache2.Platform);
                    var commonFields = fields1.Join(fields2, a => a.Name, b => b.Name, (a, b) => (a, b));

                    foreach (var (field1, field2) in commonFields)
                    {
                        var value1 = field1.GetValue(struct1);
                        var value2 = field2.GetValue(struct2);
                        DiffData(differences, field1.FieldType, cache1, value1, cache2, value2, path.Length == 0 ? field1.Name : $"{path}.{field1.Name}");
                    }
                }
                else if (typeof(IList).IsAssignableFrom(type))
                {
                    var list1 = (IList)data1;
                    var list2 = (IList)data2;

                    if (list1.Count != list2.Count)
                    {
                        differences.Add(new Difference(DifferenceKind.ElementCount, path, list1, list2));
                        return;
                    }

                    Type elementType = type.IsArray ? type.GetElementType() : type.GetGenericArguments().First();
                    for (int i = 0; i < list1.Count; i++)
                        DiffData(differences, elementType, cache1, list1[i], cache2, list2[i], $"{path}[{i}]");
                }
                else if (typeof(CachedTag).IsAssignableFrom(type))
                {
                    var tag1 = (CachedTag)data1;
                    var tag2 = (CachedTag)data2;
                    if (tag1.Group.Tag != tag2.Group.Tag || tag1.Name != tag2.Name)
                        differences.Add(new Difference(DifferenceKind.Value, path, data1, data2));
                }
                else if (typeof(StringId).IsAssignableFrom(type))
                {
                    var string1 = cache1.StringTable.GetString((StringId)data1);
                    var string2 = cache2.StringTable.GetString((StringId)data2);
                    if (string1 != string2)
                        differences.Add(new Difference(DifferenceKind.Value, path, string1, string2));
                }
                else if (typeof(IComparable).IsAssignableFrom(type))
                {
                    var comparable1 = (IComparable)data1;
                    var comparable2 = (IComparable)data2;
                    if (comparable1.CompareTo(comparable2) != 0)
                        differences.Add(new Difference(DifferenceKind.Value, path, data1, data2));
                }
                else
                {
                    var properties = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    foreach (var field in properties.Where(x => x.CanRead && x.CanWrite))
                    {
                        var value1 = field.GetValue(data1);
                        var value2 = field.GetValue(data2);
                        DiffData(differences, field.PropertyType, cache1, value1, cache2, value2, path.Length == 0 ? field.Name : $"{path}.{field.Name}");
                    }

                    var fields = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    foreach (var field in fields)
                    {
                        var value1 = field.GetValue(data1);
                        var value2 = field.GetValue(data2);
                        DiffData(differences, field.FieldType, cache1, value1, cache2, value2, path.Length == 0 ? field.Name : $"{path}.{field.Name}");
                    }
                }
            }
        }
    }
}
