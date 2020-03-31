using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry.Utils;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using static TagTool.Commands.Porting.PortTagCommand;

namespace TagTool.Commands.Porting
{
    class PortInstancedGeometryObjectCommand : Command
    {
        private GameCacheHaloOnlineBase HoCache { get; }
        private GameCache BlamCache;

        public PortInstancedGeometryObjectCommand(GameCacheHaloOnlineBase cache, GameCache blamCache) :
            base(true,

                "PortInstancedGeometryObject",
                "Converts one or more instanced geometry instances to objects.",

                "PortInstancedGeometryObject [PortingFlags] <BspIndex> [<Instance index or name> [New Tagname]]",
                "Converts one or more instanced geometry instances to objects. Enter just the bsp index for a wizard.")
        {
            HoCache = cache;
            BlamCache = blamCache;
        }

        public override object Execute(List<string> args)
        {
            var argStack = new Stack<string>(args.AsEnumerable().Reverse());

            var portingFlags = ParsePortingFlags(argStack);

            if (argStack.Count < 1)
            {
                Console.WriteLine("ERROR: Expected bsp index!");
                return false;
            }

            var sbspIndex = int.Parse(argStack.Pop());

            using (var blamCacheStream = BlamCache.OpenCacheRead())
            using (var hoCacheStream = HoCache.OpenCacheReadWrite())
            {
                var blamScnr = BlamCache.Deserialize<Scenario>(blamCacheStream, BlamCache.TagCache.FindFirstInGroup("scnr"));
                var blamSbsp = BlamCache.Deserialize<ScenarioStructureBsp>(blamCacheStream, blamScnr.StructureBsps[sbspIndex].StructureBsp);

                var desiredInstances = new Dictionary<int, string>();

                if (argStack.Count > 0)
                {
                    var identifier = argStack.Pop();
                    string desiredName = null;
                    if (argStack.Count > 0)
                    {
                        desiredName = argStack.Pop();
                    }

                    var index = FindBlockIndex(blamSbsp.InstancedGeometryInstances, identifier);
                    desiredInstances.Add(index, desiredName);
                }
                else
                {
                    Console.WriteLine("------------------------------------------------------------------");
                    Console.WriteLine("Enter each instance with the format <Name or Index> [New tagname]");
                    Console.WriteLine("Enter a blank line to finish.");
                    Console.WriteLine("------------------------------------------------------------------");
                    for (string line; !String.IsNullOrWhiteSpace(line = Console.ReadLine());)
                    {
                        var parts = line.Split(' ');
                        var identifier = parts[0];
                        var name = parts.Length > 1 ? string.Join(" ", parts.Skip(1)) : null;

                        var index = FindBlockIndex(blamSbsp.InstancedGeometryInstances, identifier);
                        if (index == -1)
                        {
                            Console.WriteLine($"ERROR: Instance not found by identifier {identifier}!");
                            return false;
                        }

                        desiredInstances.Add(index, name);
                    }
                }

                if (desiredInstances.Count < 1)
                    return true;

                var converter = new GeometryToObjectConverter(HoCache, hoCacheStream, BlamCache, blamCacheStream, blamScnr, sbspIndex);
                converter.PortTag.SetFlags(portingFlags);

                foreach (var kv in desiredInstances)
                {
                    try
                    {
                        var instance = blamSbsp.InstancedGeometryInstances[kv.Key];
                        var tag = converter.ConvertGeometry(kv.Key, kv.Value);
                    }
                    finally
                    {
                        HoCache.SaveStrings();
                        HoCache.SaveTagNames();
                    }
                }
            }

            return true;
        }

        private int FindBlockIndex(IList block, string identifier)
        {
            if (block == null || block.Count == 0)
                return -1;

            if (identifier == "*")
                return block.Count - 1;

            int index;
            if (int.TryParse(identifier, out index))
                return index;

            var labelField = FindLabelField(block[0].GetType(), BlamCache.Version);

            object expectedValue = null;
            if (labelField.FieldType == typeof(StringId))
                expectedValue = BlamCache.StringTable.GetStringId(identifier);
            else
                expectedValue = identifier;

            for (int i = 0; i < block.Count; i++)
            {
                var labelValue = labelField.GetValue(block[i]);
                if (labelValue != null && labelValue.Equals(expectedValue))
                    return i;
            }

            return -1;
        }

        private static TagFieldInfo FindLabelField(Type type, CacheVersion version)
        {
            return TagStructure.GetTagFieldEnumerable(type, version)
                .FirstOrDefault(field => field.Attribute != null && field.Attribute.Flags.HasFlag(TagFieldFlags.Label));
        }

        private static PortingFlags ParsePortingFlags(Stack<string> argStack)
        {
            var portingFlags = PortingFlags.Default;
            while (argStack.Count > 0)
            {
                var arg = argStack.Peek();

                PortingFlags flag;
                bool isSet;
                if (!ParsePortingFlag(arg, out flag, out isSet))
                    break;

                if (isSet)
                    portingFlags |= flag;
                else
                    portingFlags &= ~flag;

                argStack.Pop();
            }

            return portingFlags;
        }

        static bool ParsePortingFlag(string value, out PortingFlags flag, out bool isSet)
        {
            flag = 0;
            isSet = true;
            string[] portingFlagNames = Enum.GetNames(typeof(PortingFlags));
            Array portingFlagValues = Enum.GetValues(typeof(PortingFlags));

            if (value[0] == '!')
            {
                value = value.Substring(1);
                isSet = false;
            }

            for (int i = 0; i < portingFlagNames.Length; i++)
            {
                if (portingFlagNames[i].ToLower() == value.ToLower())
                {
                    flag = (PortingFlags)portingFlagValues.GetValue(i);
                    return true;
                }
            }

            return false;
        }
    }
}
