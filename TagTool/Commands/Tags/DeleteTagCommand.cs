using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Cache.HaloOnline;
using System.Linq;
using System.IO;
using TagTool.IO;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Cache.Resources;

namespace TagTool.Commands.Tags
{
    class DeleteTagCommand : Command
    {
        public GameCache Cache { get; }

        public DeleteTagCommand(GameCache cache)
            : base(false,

                  "DeleteTag",
                  "Nulls and removes a tag from the cache.",

                  "DeleteTag <tag> [nullresources]",
                  "Nulls and removes a tag from the cache.")
        {
            Cache = cache;
        }
        
        public override object Execute(List<string> args)
        {
            bool nullResources = false;

            for(int i = 0; i < args.Count;)
            {
                if (args[i].ToLower() == "nullresources")
                {
                    nullResources = true;
                    args.RemoveAt(i);
                    continue;
                }
                i++;
            }

            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);
            if (!Cache.TagCache.TryGetCachedTag(args[0], out var tag))
                return new TagToolError(CommandError.TagInvalid);

            // store these before tag is nulled
            var tagName = tag.Name ?? tag.Index.ToString("X");
            var tagGroup = tag.Group;

            using (var stream = Cache.OpenCacheReadWrite()) // TODO: implement better way of nulling tags, support gen3
            {
                if (Cache is GameCacheHaloOnlineBase)
                {
                    var cacheHaloOnline = Cache as GameCacheHaloOnlineBase;
                    var tagHaloOnline = tag as CachedTagHaloOnline;

                    if(nullResources)
                        NullOrphanedResources(cacheHaloOnline, stream, tagHaloOnline);

                    cacheHaloOnline.TagCacheGenHO.Tags[tag.Index] = null;

                    byte[] blankheader = Enumerable.Repeat((byte)0x00, 0x24).ToArray();
                    cacheHaloOnline.TagCacheGenHO.SetTagDataRaw(stream, tagHaloOnline, blankheader);
                }

                else
                {
                    return new TagToolError(CommandError.CacheUnsupported);
                }
            }

            Console.WriteLine($"Deleted {tagGroup} tag {tagName}");
            return true;
        }

        private static void NullOrphanedResources(GameCacheHaloOnlineBase cache, Stream stream, CachedTagHaloOnline deletedTag)
        {
            // Collect all resource references from the cache
            var allResources = new List<PageableResource>();
            foreach (CachedTagHaloOnline tag in cache.TagCache.NonNull())
                GetTagResources(allResources, cache, stream, tag);

            // Get the deleted tag's resources
            var deletedTagResources = allResources.Where(tag => tag.Resource.ParentTag == deletedTag).ToList();

            foreach (var resource in deletedTagResources)
            {
                ResourceLocation location = resource.GetLocation();

                // Count the references to this resource page, excluding the deleted tag
                int refCount = allResources.Count(
                    r => r.Resource.ParentTag != deletedTag &&
                    r.Page.Index == resource.Page.Index &&
                    r.GetLocation() == location);

                if (refCount > 0)
                    continue;

                // Open the resource cache and null the resource
                var resourceCache = cache.ResourceCaches.GetResourceCache(location);
                using (var resourceStream = cache.ResourceCaches.OpenCacheReadWrite(location))
                {
                    resourceCache.NullResource(resourceStream, resource.Page.Index);
                    Console.WriteLine($"Nulled resource page #{resource.Page.Index}");
                }
            }
        }

        private static void GetTagResources(List<PageableResource> resources, GameCache cache, Stream stream, CachedTagHaloOnline tag)
        {
            var reader = new EndianReader(stream, cache.Endianness);
            var ctx = new DataSerializationContext(reader);
            foreach (var offset in tag.ResourcePointerOffsets)
            {
                // for perforance, we're going to use knowledge of the tag layout to avoid having to deserialize.
                reader.SeekTo(tag.HeaderOffset + offset);
                reader.SeekTo(tag.HeaderOffset + new CacheAddress(reader.ReadUInt32()).Offset);
                var page = cache.Deserializer.Deserialize<ResourcePage>(ctx);
                var pageable = new PageableResource();
                pageable.Page = page;
                pageable.Resource = new ResourceData() { ParentTag = tag };
                resources.Add(pageable);
            }
        }
    }
}