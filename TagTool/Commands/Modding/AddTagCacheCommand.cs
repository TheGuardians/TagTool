using TagTool.Cache;
using System.Collections.Generic;
using TagTool.Cache.HaloOnline;
using System;
using TagTool.Commands.Common;
using System.IO;
using TagTool.Cache.Gen3;
using TagTool.IO;

namespace TagTool.Commands.Modding
{
    class AddTagCacheCommand : Command
    {
        private GameCacheModPackage Cache;

        public AddTagCacheCommand(GameCacheModPackage modCache) :
            base(true,

                "AddTagCache",
                "Add the specified number of tag caches to the mod package.",

                "AddTagCache [Number of tag caches]",

                "Add the specified number of tag caches to the mod package.")
        {
            Cache = modCache;
        }

        public override object Execute(List<string> args)
        {
            int tagCacheCount = 1;

            if (args.Count > 0 && !int.TryParse(args[0], System.Globalization.NumberStyles.Integer, null, out tagCacheCount))
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[0]}\"");
                
            // initialze mod package with current HO cache
            Console.WriteLine($"Building initial tag cache from reference...");

            var referenceStream = new MemoryStream(); // will be reused by all base caches
            var modTagCache = new TagCacheHaloOnline(referenceStream, Cache.BaseModPackage.StringTable);

            for (var tagIndex = 0; tagIndex < Cache.BaseCacheReference.TagCache.Count; tagIndex++)
            {
                var srcTag = Cache.BaseCacheReference.TagCache.GetTag(tagIndex);

                if (srcTag == null)
                {
                    modTagCache.AllocateTag(new TagGroupGen3());
                    continue;
                }

                var emptyTag = modTagCache.AllocateTag(srcTag.Group, srcTag.Name);

                var cachedTagData = new CachedTagData
                {
                    Data = new byte[0],
                    Group = (TagGroupGen3)emptyTag.Group
                };

                modTagCache.SetTagData(referenceStream, (CachedTagHaloOnline)emptyTag, cachedTagData);

                if (!((CachedTagHaloOnline)emptyTag).IsEmpty())
                {
                    return new TagToolError(CommandError.OperationFailed, "A tag in the base cache was empty");
                }
            }

            var currentTagCacheCount = Cache.BaseModPackage.GetTagCacheCount();

            for (int i = currentTagCacheCount; i < tagCacheCount + currentTagCacheCount; i++)
            {
                Console.WriteLine($"Enter the name of tag cache {i + 1} (32 chars max):");
                string name = Console.ReadLine().Trim();
                name = name.Length <= 32 ? name : name.Substring(0, 32);

                var newTagCacheStream = new MemoryStream();
                referenceStream.Position = 0;
                referenceStream.CopyTo(newTagCacheStream);

                Dictionary<int, string> tagNames = new Dictionary<int, string>();

                foreach (var tag in Cache.BaseCacheReference.TagCache.NonNull())
                    tagNames[tag.Index] = tag.Name;

                Cache.BaseModPackage.TagCachesStreams.Add(new ModPackageStream(newTagCacheStream));
                Cache.BaseModPackage.CacheNames.Add(name);
                Cache.BaseModPackage.TagCacheNames.Add(tagNames);
            }

            Console.WriteLine("Done!");
            return true;
        }
    }
}