using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Commands.Tags
{
    public class CheckTagCommand : Command
    {
        private readonly GameCache Cache;

        public CheckTagCommand(GameCache cache)
            : base(true,

                "CheckTag",
                "Checks the validity of a tag, a group, or all tags in the cache if not specified.",

                "CheckTag [group tag | tag]",

                "")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            var tags = new List<CachedTag>();
            if (args.Count > 0)
            {
                if (Cache.TagCache.TagDefinitions.TagDefinitionExists(args[0]))
                {
                    tags.AddRange(Cache.TagCache.NonNull().Where(x => x.IsInGroup(args[0])));
                }
                else
                {
                    if (!Cache.TagCache.TryGetCachedTag(args[0], out var tag))
                        return new TagToolError(CommandError.TagInvalid);

                    tags.Add(tag);
                }
            }
            else
            {
                tags.AddRange(Cache.TagCache.NonNull());
            }

            long problemCount = 0;

            using (var stream = Cache.OpenCacheRead())
            {
                foreach (var group in tags.GroupBy(x => x.Group))
                {
                    if (Cache.TagCache.TagDefinitions == null || !Cache.TagCache.TagDefinitions.TagDefinitionExists(group.Key))
                        continue;

                    foreach (var tag in group.ToList())
                    {
                        var validator = new TagDataValidiator(Cache, stream);
                        validator.VerifyTag(tag);

                        problemCount += validator.Problems.Count;
                        if (validator.Problems.Count > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine($"{tag}:");
                            foreach (var problem in validator.Problems)
                                Console.WriteLine($"  {problem}");
                            Console.ResetColor();
                            Console.WriteLine();
                        }
                    }
                }
            }

            Console.Write($"{tags.Count} tags checked. Found ");
            Console.ForegroundColor = (problemCount == 0) ? Console.ForegroundColor : ConsoleColor.DarkYellow;
            Console.Write($"{problemCount} problems.");
            Console.ResetColor();
            Console.WriteLine();

            return true;
        }
    }

    public class TagDataValidiator
    {
        private readonly GameCache Cache;
        private readonly Stream Stream;
        private readonly Stack<string> PathStack = new Stack<string>();
        public List<string> Problems = new List<string>();

        public bool BreakIntoDebugger { get; set; }

        public TagDataValidiator(GameCache cache, Stream stream)
        {
            Cache = cache;
            Stream = stream;
        }

        string CurrentFieldPath => string.Join(".", PathStack.Reverse());

        public void VerifyTag(CachedTag tag)
        {
            object data = null;

            var output = CaptureConsoleOutput(() => data = Cache.Deserialize(Stream, tag));
            foreach (var line in output.Split('\r', '\n'))
            {
                var trimmed = line.Trim();
                if (trimmed.StartsWith("WARNING:") || trimmed.StartsWith("ERROR"))
                    Problems.Add(trimmed);
            }

            VerifyData(data);
        }

        public void VerifyData(object data)
        {
            switch (data)
            {
                case null:
                case byte[] _:
                    break;
                case TagStructure tagStruct:
                    foreach (var field in tagStruct.GetTagFieldEnumerable(Cache.Version, Cache.Platform))
                    {
                        PathStack.Push(field.Name);
                        VerifyData(field.GetValue(tagStruct));
                        PathStack.Pop();
                    }
                    break;
                case IList list:
                    var fieldName = PathStack.Pop();
                    for (int i = 0; i < list.Count; i++)
                    {
                        PathStack.Push($"{fieldName}[{i}]");
                        VerifyData(list[i]);
                        PathStack.Pop();
                    }
                    PathStack.Push(fieldName);
                    break;
                case StringId stringId:
                    VerifyStringId(stringId);
                    break;
                case Enum e:
                    VerifyEnum(e);
                    break;
                case RealPlane3d plane:
                    VerifyPlane3d(plane);
                    break;
                case float r:
                    VerifyReal(r);
                    break;
                case CachedTag tag:
                    VerifyTagRef(tag);
                    break;
                default:
                    {
                        var type = data.GetType();
                        if (type.IsValueType)
                        {
                            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                            {
                                PathStack.Push($"{field.Name}");
                                VerifyData(field.GetValue(data));
                                PathStack.Pop();
                            }
                        }
                    }
                    break;
            }
        }

        void AddProblem(string info)
        {
            if (BreakIntoDebugger)
                Debug.Assert(false, info);

            Problems.Add(info);
        }

        void VerifyStringId(StringId stringId)
        {
            if (stringId == StringId.Invalid || stringId.Value == 0xffffffff)
                return;

            try
            {
                Cache.StringTable.GetString(stringId);
            }
            catch (Exception ex)
            {
                AddProblem($"Invalid StringId: {CurrentFieldPath} {stringId}");
            }
        }

        void VerifyTagRef(CachedTag tag)
        {
            if (!IsValidTagRef(tag))
                AddProblem($"Invalid tag ref: {CurrentFieldPath} = {tag}");
        }

        void VerifyEnum(Enum value)
        {
            if (!IsEnumDefined(value.GetType(), value))
                AddProblem($"Enum out of range: {CurrentFieldPath} = {value}");
        }

        void VerifyPlane3d(RealPlane3d value)
        {
            if (!IsValidNormal3d(value.Normal))
                AddProblem($"Invalid plane normal: {CurrentFieldPath} = {value}");
        }

        void VerifyReal(float value)
        {
            if (!IsValidReal(value))
                AddProblem($"Not a real number: {CurrentFieldPath} = {value}");
        }

        bool IsValidTagRef(CachedTag tag)
        {
            if (!Cache.TagCache.TagDefinitions.TagDefinitionExists(tag.Group.Tag))
                return false;

            if (!(Cache is GameCacheHaloOnlineBase))
                return Cache.TagCache.IsTagIndexValid((int)(tag.ID & 0xffff));

            return true;
        }

        bool IsValidNormal3d(RealVector3d normal)
        {
            float d = Math.Abs(RealVector3d.Magnitude(normal) - 0.0f);
            if (!IsValidReal(d))
                return false;
            return d < 0.0001f || (d > 0.99f && d < 1.01f);
        }

        bool IsValidReal(float r)
        {
            return !float.IsInfinity(r) && !float.IsNaN(r);
        }

        bool IsEnumDefined(Type type, object value)
        {
            if (type.GetCustomAttribute<FlagsAttribute>() != null)
            {
                return (ValueToUInt64(value) & ~GetEnumBitMask(type)) == 0;
            }
            else
            {
                return Enum.IsDefined(type, value);
            }
        }

        ulong GetEnumBitMask(Type enumType)
        {
            ulong mask = 0UL;
            foreach (var v in Enum.GetValues(enumType))
                mask |= ValueToUInt64(v);
            return mask;
        }

        ulong ValueToUInt64(object value)
        {
            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.SByte: return (byte)(sbyte)value;
                case TypeCode.Int16: return (ushort)(short)value;
                case TypeCode.Int32: return (uint)(int)value;
                case TypeCode.Int64: return (ulong)(long)value;
                case TypeCode.Byte: return (byte)value;
                case TypeCode.UInt16: return (ushort)value;
                case TypeCode.UInt32: return (uint)value;
                case TypeCode.UInt64: return (ulong)value;
                default: throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        string CaptureConsoleOutput(Action action)
        {
            using (var sw = new StringWriter())
            {
                var oldOut = Console.Out;
                Console.SetOut(sw);
                action();
                Console.SetOut(oldOut);
                return sw.ToString();
            }
        }
    }
}
