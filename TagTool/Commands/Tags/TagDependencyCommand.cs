using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Cache.HaloOnline;
using TagTool.Tags;
using System.IO;
using TagTool.BlamFile;
using TagTool.Tags.Definitions.Gen4;

namespace TagTool.Commands.Tags
{
    /// <summary>
    /// Command for managing tag dependencies.
    /// </summary>
    class TagDependencyCommand : Command
    {
        public GameCache Cache { get; }
        public GameCacheHaloOnlineBase HOCache { get; set; }

        public TagDependencyCommand(GameCache cache) : base(
            true,

            "TagDependency",
            "Manage tag dependencies.",

            "TagDependency Add <tag> {... dependencies ...}\n" +
            "TagDependency Remove <tag> {... dependencies ...}\n" +
            "TagDependency List <tag>\n" +
            "TagDependency ListAll <tag>\n" +
            "TagDependency ListOn <tag>",

            "\"TagDependency Add\" will cause the first tag to load the other tags.\n" +
            "\"TagDependency Remove\" will prevent the first tag from loading the other tags.\n" +
            "\"TagDependency List\" will list all immediate dependencies of a tag.\n" +
            "\"TagDependency ListAll\" will recursively list all dependencies of a tag.\n" +
            "\"TagDependency ListOn\" will list all tags that depend on a tag.\n" +
            "\n" +
            "To add dependencies to a map, use the \"GetMapInfo\" command to get its scenario tag\n" +
            "index and then add dependencies to the scenario tag.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {          
            if (args.Count < 2)
                return new TagToolError(CommandError.ArgCount);
            if (!Cache.TagCache.TryGetCachedTag(args[1], out var tag))
                return new TagToolError(CommandError.TagInvalid);

            if (Cache is GameCacheHaloOnlineBase)
            {
                HOCache = (GameCacheHaloOnlineBase)Cache;
                switch (args[0].ToLower())
                {
                    case "add":
                    case "remove":
                        return ExecuteAddRemove((CachedTagHaloOnline)tag, args);

                    case "list":
                    case "listall":
                        return ExecuteList((CachedTagHaloOnline)tag, (args[0] == "listall"), args.Skip(2).ToArray());

                    case "liston":
                        return ExecuteListDependsOn((CachedTagHaloOnline)tag);

                    default:
                        return new TagToolError(CommandError.ArgInvalid, $"\"{args[0]}\"");
                }
            }
            else
            {
                switch (args[0].ToLower())
                {
                    case "list":
                        return FindTagDepsManual(tag);
                    default:
                        return new TagToolError(CommandError.ArgInvalid, $"Only 'list' argument accepted for non-HO caches");
                }
            }
            
        }

        private object ExecuteAddRemove(CachedTagHaloOnline tag, List<string> args)
        {
            if (args.Count < 3)
                return new TagToolError(CommandError.ArgCount);

            var dependencies = args.Skip(2).Select(name => Cache.TagCache.GetTag(name)).ToList();
            
            if (dependencies.Count == 0 || dependencies.Any(d => d == null))
                return new TagToolError(CommandError.CustomError, "No dependencies were listed");

            using (var stream = Cache.OpenCacheReadWrite())
            {
                var data = HOCache.TagCacheGenHO.ExtractTag(stream, tag);

                if (args[0].ToLower() == "add")
                {
                    foreach (var dependency in dependencies)
                    {
                        if (data.Dependencies.Add(dependency.Index))
                            Console.WriteLine("Added dependency on tag {0:X8}.", dependency.Index);
                        else
                            Console.WriteLine("Tag {0:X8} already depends on tag {1:X8}.", tag.Index, dependency.Index);
                    }
                }
                else
                {
                    foreach (var dependency in dependencies)
                    {
                        if (data.Dependencies.Remove(dependency.Index))
                            Console.WriteLine("Removed dependency on tag {0:X8}.", dependency.Index);
                        else
                            Console.WriteLine("Tag {0:X8} does not depend on tag {1:X8}.", tag.Index, dependency.Index);
                    }
                }

                HOCache.TagCacheGenHO.SetTagData(stream, tag, data);
            }

            return true;
        }

        private object ExecuteList(CachedTagHaloOnline tag, bool all, params string[] groups)
        {
            if (tag.Dependencies.Count == 0)
            {
                Console.WriteLine("Tag {0:X8} has no dependencies.", tag.Index);
                return true;
            }

            IEnumerable<CachedTagHaloOnline> dependencies;

            if (all)
                dependencies = HOCache.TagCacheGenHO.FindDependencies(tag);
            else
                dependencies = tag.Dependencies.Where(i => i >= 0 && i <= Cache.TagCache.Count).Select(i => HOCache.TagCacheGenHO.Tags[i]);

            var groupTags = groups.Select(group => Cache.TagCache.ParseGroupTag(group)).ToArray();

            foreach (var dependency in dependencies)
            {
                if (groupTags.Length != 0 && !dependency.IsInGroup(groupTags))
                    continue;

                var tagName = dependency?.Name ?? $"0x{dependency.Index:X4}";

                Console.WriteLine($"[Index: 0x{dependency.Index:X4}, Offset: 0x{dependency.HeaderOffset:X8}, Size: 0x{dependency.TotalSize:X4}] {tagName}.{dependency.Group}");
            }
            
            foreach (var instance in tag.Dependencies)
                if (instance < 0 || instance >= Cache.TagCache.Count)
                    new TagToolWarning($"Dependency is a nonexistent tag: 0x{instance:X4}");
                    
            return true;
        }

        private object ExecuteListDependsOn(CachedTagHaloOnline tag)
        {
            var dependsOn = HOCache.TagCacheGenHO.NonNull().Where(t => ((CachedTagHaloOnline)t).Dependencies.Contains(tag.Index));

            foreach (var dependency in dependsOn)
            {
                var tagName = dependency?.Name ?? $"0x{dependency.Index:X4}";

                Console.WriteLine($"[Index: 0x{dependency.Index:X4}, Offset: 0x{dependency.DefinitionOffset:X8}] {tagName}.{dependency.Group}");
            }

            return true;
        }

        private object FindTagDepsManual(CachedTag tag)
        {
            HashSet<CachedTag> tagRefs = new HashSet<CachedTag>();
            using (var stream = Cache.OpenCacheRead())
            {
                var tagStruct = Cache.Deserialize(stream, tag);
                ScanStructureForDeps((TagStructure)tagStruct, ref tagRefs);
                if (tagRefs.Count == 0)
                    Console.WriteLine("Tag {0:X8} has no dependencies.", tag.Index);
                else
                {
                    foreach(var dependency in tagRefs)
                    {
                        if (dependency == null)
                            continue;
                        Console.WriteLine($"[Index: 0x{dependency.Index:X4}, Offset: 0x{dependency.DefinitionOffset:X8}] {dependency.Name}.{dependency.Group}");
                    }                   
                }
            }
            return true;
        }

        private void ScanStructureForDeps(TagStructure tagStruct, ref HashSet<CachedTag> tagRefs)
        {
            var inputinfo = TagStructure.GetTagStructureInfo(tagStruct.GetType(), Cache.Version, Cache.Platform);
            foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(inputinfo.Types[0], inputinfo.Version, inputinfo.CachePlatform))
            {
                if (tagFieldInfo.FieldType == typeof(CachedTag))
                {
                    tagRefs.Add((CachedTag)tagFieldInfo.GetValue(tagStruct));
                }
                //if its a sub-tagstructure, iterate into it
                else if (tagFieldInfo.FieldType.BaseType == typeof(TagStructure))
                {
                    ScanStructureForDeps((TagStructure)tagFieldInfo.GetValue(tagStruct), ref tagRefs);
                }
                //if its a tagblock, call convertlist to iterate through and convert each one and return a converted list
                else if (tagFieldInfo.FieldType.IsGenericType && tagFieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var input = tagFieldInfo.GetValue(tagStruct);
                    IEnumerable<object> enumerable = input as IEnumerable<object>;
                    if (enumerable == null)
                        throw new InvalidOperationException("listData must be enumerable");
                    foreach (object item in enumerable.OfType<object>())
                    {
                        ScanStructureForDeps((TagStructure)item, ref tagRefs);
                    }
                }
            }
        }
    }
}
