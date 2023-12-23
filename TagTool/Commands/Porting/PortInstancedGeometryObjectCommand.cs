using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
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
        private int sbspIndex = 0;
        private bool centergeometry = true;
        private bool allunique = false;
        private bool globalcategory = false;
        private string newCategoryName;
        private int globalCategoryIndex = -1;

        public PortInstancedGeometryObjectCommand(GameCacheHaloOnlineBase cache, GameCache blamCache) :
            base(true,

                "PortInstancedGeometryObject",
                "Converts one or more instanced geometry instances to objects.",

                "PortInstancedGeometryObject [PortingFlags] [BspIndex] [nocenter] [forge] [allunique] [<Instance index or name> [New Tagname]]",
                "When porting multiple, provide each on a new line after arguments."
                + "\nWhen the cache file scenario has only one BSP (most MP levels), providing a BspIndex is unnecessary."
                + "\n\nUse \"forgepalette\" to add your ported instances to the forge palette."
                + "\n\t- forgepalette by itself will add instances to a new category with the cache file display name."
                + "\n\t- forgepalette:<categoryname> will add instances to a new category with your the specified name."
                + "\n\t- forgepalette:<categoryindex> will add instances to a preexisting category at the specified index."
                + "\n\nUse \"allunique\" to port one of each unique instance in the chosen BSP.")
        {
            HoCache = cache;
            BlamCache = blamCache;
        }

        public override object Execute(List<string> args)
        {
            var argStack = new Stack<string>(args.AsEnumerable().Reverse());
            var portingFlags = ParsePortingFlags(argStack);

            using (var blamCacheStream = BlamCache.OpenCacheRead())
            using (var hoCacheStream = HoCache.OpenCacheReadWrite())
            {
                var blamScnr = BlamCache.Deserialize<Scenario>(blamCacheStream, BlamCache.TagCache.FindFirstInGroup("scnr"));
                var forgeGlobals = HoCache.Deserialize<ForgeGlobalsDefinition>(hoCacheStream, HoCache.TagCache.FindFirstInGroup("forg"));

                var desiredInstances = new Dictionary<int, string>();
                var forgeItems = new List<ForgeGlobalsDefinition.PaletteItem>();

                if (blamScnr.StructureBsps.Count > 1)
                {
                    if (argStack.Count < 1)
                    {
                        return new TagToolError(CommandError.ArgCount, "Multiple bsps available, please provide a bsp index!)");
                    }
                    else if (!int.TryParse(argStack.Pop(), out sbspIndex))
                        return new TagToolError(CommandError.ArgInvalid, "Invalid bsp index");
                }
                else if (argStack.Count > 0 && int.TryParse(argStack.Peek(), out var discard))
                    argStack.Pop();

                var blamSbsp = BlamCache.Deserialize<ScenarioStructureBsp>(blamCacheStream, blamScnr.StructureBsps[sbspIndex].StructureBsp);

                if (argStack.Count > 0 && argStack.Peek().ToLower() == "nocenter")
                {
                    argStack.Pop();
                    centergeometry = false;
                }
                // Get forge palette category for all instances, if provided
                if (argStack.Count > 0 && argStack.Peek().ToLower().StartsWith("forge"))
                {
                    var argParameters = argStack.Peek().Split(':');
                    if (argParameters.Count() == 1)
                    {
                        newCategoryName = BlamCache.DisplayName;
                    }
                    else if (!int.TryParse(argParameters[1], out globalCategoryIndex))
                    {
                        newCategoryName = argParameters[1];
                    }
                    if (newCategoryName != null || globalCategoryIndex != -1)
                    {
                        globalcategory = true;
                    }

                    argStack.Pop();
                }
                // port unique geometry
                if (argStack.Count > 0 && argStack.Peek().ToLower() == "allunique")
                {
                    argStack.Pop();
                    allunique = true;
                }
                if (argStack.Count > 0)
                {
                    var identifier = argStack.Pop();
                    string desiredName = null;
                    if (argStack.Count > 0)
                    {
                        desiredName = argStack.Pop();
                    }

                    int index;
                    if (blamSbsp.InstancedGeometryInstanceNames != null && blamSbsp.InstancedGeometryInstanceNames.Count > 0)
                        index = FindBlockIndex(blamSbsp.InstancedGeometryInstanceNames, identifier);
                    else
                        index = FindBlockIndex(blamSbsp.InstancedGeometryInstances, identifier);

                    desiredInstances.Add(index, desiredName);
                }
                else if (allunique)
                {
                    var visitedNames = new HashSet<string>();
                    var visitedDefinitions = new HashSet<short>();
                    var uniqueInstances = new List<(int Index, string Name)>();

                    if (BlamCache.Version >= CacheVersion.HaloReach)
                    {
                        var sbspCacheResources = BlamCache.ResourceCache.GetStructureBspCacheFileTagResources(blamSbsp.PathfindingResource);
                        for (int i = 0; i < blamSbsp.InstancedGeometryInstanceNames.Count; i++)
                        {
                            var instance = sbspCacheResources.InstancedGeometryInstances[i];
                            var name = BlamCache.StringTable.GetString(blamSbsp.InstancedGeometryInstanceNames[i].Name);
                            if (visitedNames.Add(name) && visitedDefinitions.Add(instance.DefinitionIndex))
                                uniqueInstances.Add((i, name));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < blamSbsp.InstancedGeometryInstances.Count; i++)
                        {
                            var instance = blamSbsp.InstancedGeometryInstances[i];
                            var name = BlamCache.StringTable.GetString(instance.Name);
                            if (visitedNames.Add(name) && visitedDefinitions.Add(instance.DefinitionIndex))
                                uniqueInstances.Add((i, name));
                        }
                    }

                    foreach(var (index, name) in uniqueInstances)
                    {
                        if (BlamCache.Platform == CachePlatform.MCC && name.Contains("merged"))
                            break;

                        desiredInstances.Add(index, string.Empty);

                        forgeItems.Add(new ForgeGlobalsDefinition.PaletteItem()
                        {
                            Name = name,
                            Type = ForgeGlobalsDefinition.PaletteItemType.Prop,
                            CategoryIndex = -1,
                            DescriptionIndex = -1,
                            MaxAllowed = (ushort)index,
                            Object = null
                        });
                    }
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

                        var forgeArgs = new string[0];
                        var categoryIndex = -1;
                        string paletteName = string.Empty;
                        string categoryName = string.Empty;

                        if (parts.Count() > 1 && parts[1].StartsWith("forgepalette"))
                        {
                            forgeArgs = parts[1].Split(':');

                            if (forgeArgs.Count() == 1)
                            {
                                newCategoryName = BlamCache.DisplayName;
                            }
                            else if (!int.TryParse(forgeArgs[1], out categoryIndex))
                            {
                                categoryName = forgeArgs[1];
                            }
                            if (forgeArgs.Count() == 3)
                            {
                                paletteName = forgeArgs[2];
                            }
                        }

                        var tagname = parts.Length > 1 ? string.Join(" ", parts.Skip(1)) : null;


                        int index;
                        if (blamSbsp.InstancedGeometryInstanceNames != null && blamSbsp.InstancedGeometryInstanceNames.Count > 0)
                            index = FindBlockIndex(blamSbsp.InstancedGeometryInstanceNames, identifier);
                        else
                            index = FindBlockIndex(blamSbsp.InstancedGeometryInstances, identifier);

                        if (index == -1)
                            return new TagToolError(CommandError.OperationFailed, $"Instance not found by identifier {identifier}!");

                        desiredInstances.Add(index, tagname);
                        forgeItems.Add(new ForgeGlobalsDefinition.PaletteItem()
                        {
                            Name = paletteName,
                            Type = ForgeGlobalsDefinition.PaletteItemType.Prop,
                            CategoryIndex = (short)categoryIndex,
                            DescriptionIndex = -1,
                            MaxAllowed = (ushort)index,
                            Object = null
                        });
                    }
                }

                if (desiredInstances.Count < 1)
                    return true;

                if (globalcategory && newCategoryName != null)
                {
                    forgeGlobals.PaletteCategories.Add(new ForgeGlobalsDefinition.PaletteCategory()
                    {
                        Name = newCategoryName,
                        ParentCategoryIndex = -1,
                        DescriptionIndex = -1
                    });
                }

                var converter = new GeometryToObjectConverter(HoCache, hoCacheStream, BlamCache, blamCacheStream, blamScnr, sbspIndex);
                converter.PortTag.SetFlags(portingFlags);

                foreach (var kv in desiredInstances)
                {
                    try
                    {
                        var tag = converter.ConvertGeometry(kv.Key, kv.Value, false, centergeometry);

                        foreach (var item in forgeItems)
                        {
                            if (item.MaxAllowed == (ushort)kv.Key && item.Object == null)
                            {
                                item.Object = tag;
                                item.MaxAllowed = 0;

                                if (item.Name == string.Empty)
                                {
                                    item.Name = item.Object.Name.Split('\\').Last();
                                }

                                if (item.CategoryIndex == -1)
                                {
                                    item.CategoryIndex = (short)(forgeGlobals.PaletteCategories.Count() - 1);
                                }
                            }
                        }
                    }
                    finally
                    {
                        HoCache.SaveStrings();
                        HoCache.SaveTagNames();
                    }
                }

                foreach (var item in forgeItems)
                {
                    forgeGlobals.Palette.Add(item);
                }

                HoCache.Serialize(hoCacheStream, HoCache.TagCache.FindFirstInGroup("forg"), forgeGlobals);
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

            var labelField = FindLabelField(block[0].GetType(), BlamCache.Version, BlamCache.Platform);

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

        private static TagFieldInfo FindLabelField(Type type, CacheVersion version, CachePlatform platform)
        {
            return TagStructure.GetTagFieldEnumerable(type, version, platform)
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

        public struct ForgeInfo
        {
            public ForgeInfo(int identifier, string entryName, int category, string catName)
            {
                instanceIndex = identifier;
                categoryIndex = category;
                categoryName = catName;
                itemName = entryName;
            }

            public int instanceIndex { get; }
            public int categoryIndex { get; }
            public string categoryName { get; }
            public string itemName { get; }
        }
    }
}
