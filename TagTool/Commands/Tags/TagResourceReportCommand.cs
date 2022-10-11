using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Commands.Tags
{
    public class TagResourceReportCommand : Command
    {
        private GameCacheHaloOnlineBase Cache;
        private ReportFlags CurrentFlags;

        public TagResourceReportCommand(GameCacheHaloOnlineBase cache) :
            base(true,

                "TagResourceReport",
                "Generate a report on tag resources usage for the specified tag or current cache",

                $"TagResourceReport [Flags = {DefaultFlags}] [Tag]",

                GenerateHelp())
        {
            Cache = cache;
        }

        [Flags]
        public enum ReportFlags
        {
            None = 0,
            [Description("Includes tag dependencies recursively")]
            Recursive = 1 << 0,
            [Description("If used from a mod package also includes the base cache tags")]
            IncludeBaseCache = 1 << 1,
        }
        const ReportFlags DefaultFlags = ReportFlags.Recursive;

        public override object Execute(List<string> args)
        {
            ParseFlags(args);
            Console.WriteLine($"Report Flags: {CurrentFlags}");

            var tags = new List<CachedTag>();
            if (args.Count > 0)
            {
                if (!Cache.TagCache.TryGetCachedTag(args[0], out var tag))
                    new TagToolError(CommandError.TagInvalid);

                tags.Add(tag);
            }
            else
            {
                tags.AddRange(Cache.TagCache.TagTable);
            }

            var resourceList = new List<PageableResource>();
            using (var cacheStream = Cache.OpenCacheRead())
            {
                var visitedTags = new HashSet<CachedTagHaloOnline>();
                foreach (CachedTagHaloOnline tag in tags)
                    CollectTagResources(Cache, cacheStream, tag, resourceList, visitedTags);
            }

            if (resourceList.Count == 0)
            {
                Console.WriteLine("No resources found.");
                return true;
            }

            string columnFormatString = "{0,-16} {1,-16} {2,-16} {3}";
            string tableSeparator = new string('-', 120);

            Console.WriteLine(tableSeparator);
            Console.WriteLine(columnFormatString, "Compressed", "Uncompressed", "Type", "Tag");
            Console.WriteLine(tableSeparator);

            long totalUncompressed = 0;
            long totalCompressed = 0;
            var typeSummary = new Dictionary<TagResourceTypeGen3, ResourceTypeSummary>();

            var sortedResourceList = resourceList
                .OrderBy(x => x.Page.CompressedBlockSize)
                .ThenBy(x => x.Page.UncompressedBlockSize)
                .ThenBy(x => x.Resource.ParentTag.Name);

            foreach (var resource in sortedResourceList)
            {
                var tag = resource.Resource.ParentTag;
                long uncompressedSize = resource.Page.UncompressedBlockSize;
                long compressedSize = resource.Page.CompressedBlockSize;
                totalUncompressed += uncompressedSize;
                totalCompressed += compressedSize;

                if (!typeSummary.ContainsKey(resource.Resource.ResourceType))
                    typeSummary.Add(resource.Resource.ResourceType, new ResourceTypeSummary());
                typeSummary[resource.Resource.ResourceType].UncompressedSize += uncompressedSize;
                typeSummary[resource.Resource.ResourceType].CompressedSize += compressedSize;
                typeSummary[resource.Resource.ResourceType].Count++;

                Console.WriteLine(columnFormatString, FormatSize(compressedSize), FormatSize(uncompressedSize), resource.Resource.ResourceType, tag);
            }

            Console.WriteLine(tableSeparator);
            Console.WriteLine();
            Console.WriteLine("{0,-20} {1}", "Total Compressed:", FormatSize(totalCompressed));
            Console.WriteLine("{0,-20} {1}", "Total Uncompressed:", FormatSize(totalUncompressed));
            Console.WriteLine();
            Console.WriteLine($"Compressed Totals:");
            foreach (var pair in typeSummary)
                Console.WriteLine($"\t{pair.Key,-16} {FormatSize(pair.Value.CompressedSize)}");
            Console.WriteLine();
            Console.WriteLine($"Uncompressed Totals:");
            foreach (var pair in typeSummary)
                Console.WriteLine($"\t{pair.Key,-16} {FormatSize(pair.Value.UncompressedSize)}");
            Console.WriteLine();
            Console.WriteLine($"Counts:");
            foreach (var pair in typeSummary)
                Console.WriteLine($"\t{pair.Key,-16} {pair.Value.Count}");

            return true;
        }

        private static string GenerateHelp()
        {
            var output = new StringBuilder();
            output.AppendLine("Flags:");
            foreach (FieldInfo enumMember in typeof(ReportFlags).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var description = enumMember.GetCustomAttribute<DescriptionAttribute>()?.Description;
                if (description == null)
                    continue;

                output.AppendLine($"\t{enumMember.Name,-18} {description}");
            }
            return output.ToString();
        }

        private void ParseFlags(List<string> args)
        {
            CurrentFlags = DefaultFlags;
            while (args.Count > 0)
            {
                string arg = args[0];

                bool not = arg.StartsWith("!");
                if (not) arg = arg.Remove(0, 1);

                if (!Enum.TryParse(arg, true, out ReportFlags flag))
                    break;

                CurrentFlags = not ? (CurrentFlags & ~flag) : (CurrentFlags | flag);
                args.RemoveAt(0);
            }
        }

        private void CollectTagResources(GameCache cache, Stream stream, CachedTagHaloOnline tag, List<PageableResource> resourcesList, HashSet<CachedTagHaloOnline> visitedTags)
        {
            if (tag == null)
                return;
            if (CurrentFlags.HasFlag(ReportFlags.IncludeBaseCache) && tag.IsEmpty())
                return;

            if (visitedTags.Add(tag))
                CollectTagResources(cache, stream, cache.Deserialize(stream, tag), resourcesList, visitedTags);
        }

        private void CollectTagResources(GameCache cache, Stream stream, object data, List<PageableResource> resourcesList, HashSet<CachedTagHaloOnline> visitedTags)
        {
            switch (data)
            {
                case PageableResource resource:
                    resourcesList.Add(resource);
                    break;
                case TagStructure tagStruct:
                    foreach (var field in tagStruct.GetTagFieldEnumerable(cache.Version, cache.Platform))
                        CollectTagResources(cache, stream, field.GetValue(data), resourcesList, visitedTags);
                    break;
                case IList list when !(list is byte[]):
                    foreach (var element in list)
                        CollectTagResources(cache, stream, element, resourcesList, visitedTags);
                    break;
                case CachedTagHaloOnline tagRef when CurrentFlags.HasFlag(ReportFlags.Recursive):
                    CollectTagResources(cache, stream, tagRef, resourcesList, visitedTags);
                    break;
            }
        }

        private static string FormatSize(double size)
        {
            const double KB = 1024;
            const double MB = KB * 1024;

            if (size < KB)
                return $"{size} B";

            if (size < MB)
                return $"{size / KB:0.0} KB";

            return $"{size / MB:0.0} MB";
        }

        private class ResourceTypeSummary
        {
            public long CompressedSize;
            public long UncompressedSize;
            public int Count;
        }
    }
}
