using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Commands.Porting;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Commands.Tags
{
    class Test2Command : Command
    {
        private readonly GameCacheHaloOnlineBase Cache;

        public Test2Command(GameCacheHaloOnlineBase cache)
            : base(false, "Test2", "Test2", "Test2", "Test2")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1)
                return false;

            var mapsDir = new DirectoryInfo(args[0]);

            if (args.Count > 1)
                ResourceCachesHaloOnline.ResourceCacheNames[ResourceLocation.Textures] = args[1];

            var converted = new List<(CachedTag tag, long resourcesSize)>();

            foreach (var file in mapsDir.GetFiles("*.map"))
            {
                Console.WriteLine($"{file.Name}...");

                var srcCache = GameCache.Open(file);
                if (srcCache.TagCache == null || srcCache.TagCache.Count == 0)
                    continue;

                var resourceStreams = new Dictionary<ResourceLocation, Stream>();
                using (var srcStream = srcCache.OpenCacheRead())
                using (var dstStream = Cache.OpenCacheReadWrite())
                {
                    long totalResourcesSize = 0;
                    foreach (var tag in srcCache.TagCache.NonNull().Where(x => x.IsInGroup("bitm")))
                    {
                        CachedTag existingTag;
                        if (Cache.TagCache.TryGetTag(tag.ToString(), out existingTag))
                            continue;

                        if (tag.Name.Contains("lightmap") || tag.Name.Contains("cubemap"))
                            continue;

                        Console.WriteLine($"Converting {tag}...");

                        var portTag = new PortTagCommand(Cache, srcCache);
                        portTag.SetFlags(PortTagCommand.PortingFlags.Default);

                        var convertedTag = portTag.ConvertTag(dstStream, srcStream, resourceStreams, tag);

                        if (convertedTag == null)
                        {
                            Console.WriteLine($"[ERROR]: Failed to convert tag '{tag}'");
                            continue;
                        }

                        var resourcesSize = GetTagResourcesSize(dstStream, Cache, convertedTag);
                        totalResourcesSize += resourcesSize;

                        converted.Add((convertedTag, resourcesSize));
                    }

                    foreach (var stream in resourceStreams)
                        stream.Value.Close();
                    Cache.SaveStrings();
                    Cache.SaveTagNames();
                }

                using (var convertedDump = File.CreateText("converted.txt"))
                {
                    long totalResourcesSize = 0;
                    foreach (var entry in converted)
                    {
                        totalResourcesSize += entry.resourcesSize;
                        convertedDump.WriteLine($"{entry.resourcesSize / 1024.0 / 1024.0:0.000} MB \t\t{entry.tag.Name}.{entry.tag.Group.Tag}");
                    }

                    convertedDump.WriteLine($"Total: {totalResourcesSize / 1024.0 / 1024.0} MB");
                }
            }

            return true;
        }

        long GetTagResourcesSize(Stream stream, GameCache cache, CachedTag tag)
        {
            var definition = cache.Deserialize(stream, tag);

            return PageableResourceCollector.Collect(cache, definition as TagStructure)
                .Sum(x => x.Page.CompressedBlockSize);
        }

        class PageableResourceCollector
        {
            private readonly GameCache _cache;
            private readonly List<PageableResource> _pageableResources = new List<PageableResource>();

            public PageableResourceCollector(GameCache cache, TagStructure tagStructure)
            {
                _cache = cache;
                VisitTagStructure(tagStructure);
            }

            public static IEnumerable<PageableResource> Collect(GameCache cache, TagStructure tagStructure)
            {
                return new PageableResourceCollector(cache, tagStructure)._pageableResources;
            }

            private void VisitData(object data)
            {
                switch (data)
                {
                    case PageableResource pageableResource:
                        VisitPageableResource(pageableResource);
                        break;

                    case TagStructure tagStructure:
                        VisitTagStructure(tagStructure);
                        break;
                    case IList collection:
                        VisitCollection(collection);
                        break;
                }
            }

            private void VisitCollection(IList collection)
            {
                foreach (var element in collection)
                    VisitData(element);
            }

            private void VisitPageableResource(PageableResource pageableResource)
            {
                _pageableResources.Add(pageableResource);
            }

            private void VisitTagStructure(TagStructure tagStructure)
            {
                foreach (var field in tagStructure.GetTagFieldEnumerable(_cache.Version))
                {
                    var data = field.GetValue(tagStructure);
                    VisitData(data);
                }
            }
        }
    }
}
