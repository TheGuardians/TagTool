using BlamCore.Cache;
using BlamCore.Commands;
using BlamCore.Common;
using BlamCore.IO;
using BlamCore.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Files
{
    class CleanCacheFilesCommand : Command
    {
        public GameCacheContext CacheContext { get; }

        public CleanCacheFilesCommand(GameCacheContext cacheContext)
            : base(CommandFlags.None,

                  "CleanCacheFiles",
                  "Nulls and removes unused tags and resources from cache.",

                  "CleanCacheFiles",

                  "Nulls and removes unused tags and resources from cache.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            using (var stream = CacheContext.OpenTagCacheReadWrite())
            {
                var retainedTags = new HashSet<int>();
                LoadTagDependencies(CacheContext.TagCache.Index.FindFirstInGroup("cfgt").Index, ref retainedTags);

                foreach (var scnr in CacheContext.TagCache.Index.FindAllInGroup("scnr"))
                    LoadTagDependencies(scnr.Index, ref retainedTags);

                var resourceIndices = new Dictionary<ResourceLocation, HashSet<int>>
                {
                    [ResourceLocation.Audio] = new HashSet<int>(),
                    [ResourceLocation.Lightmaps] = new HashSet<int>(),
                    [ResourceLocation.RenderModels] = new HashSet<int>(),
                    [ResourceLocation.Resources] = new HashSet<int>(),
                    [ResourceLocation.ResourcesB] = new HashSet<int>(),
                    [ResourceLocation.Textures] = new HashSet<int>(),
                    [ResourceLocation.TexturesB] = new HashSet<int>()
                };

                for (var i = 0; i < CacheContext.TagCache.Index.Count; i++)
                {
                    var tag = CacheContext.TagCache.Index[i];

                    if (tag == null)
                        continue;

                    if (retainedTags.Contains(i))
                    {
                        using (var dataStream = new MemoryStream(CacheContext.TagCache.ExtractTagRaw(stream, tag)))
                        using (var reader = new EndianReader(dataStream))
                        {
                            var dataContext = new DataSerializationContext(reader, null, CacheAddressType.Resource);

                            foreach (var resourcePointerOffset in tag.ResourcePointerOffsets)
                            {
                                reader.BaseStream.Position = resourcePointerOffset;
                                var resourcePointer = reader.ReadUInt32();

                                reader.BaseStream.Position = tag.PointerToOffset(resourcePointer);
                                var resourceDefinition = CacheContext.Deserializer.Deserialize<PageableResource>(dataContext);

                                if (resourceDefinition.Page.Index == -1)
                                    continue;

                                var resourceLocation = resourceDefinition.GetLocation();

                                if (!resourceIndices[resourceLocation].Contains(resourceDefinition.Page.Index))
                                    resourceIndices[resourceLocation].Add(resourceDefinition.Page.Index);
                            }
                        }
                    }

                    var tagName = CacheContext.TagNames.ContainsKey(tag.Index) ? CacheContext.TagNames[tag.Index] : $"0x{tag.Index}";
                    var tagGroupName = CacheContext.GetString(tag.Group.Name);

                    Console.Write($"Nulling {tagName}.{tagGroupName}...");
                    CacheContext.TagCache.NullTag(stream, tag);
                    Console.WriteLine("done.");
                }

                foreach (var entry in resourceIndices)
                {
                    ResourceCache resourceCache = null;

                    try
                    {
                        resourceCache = CacheContext.GetResourceCache(entry.Key);
                    }
                    catch (FileNotFoundException)
                    {
                        continue;
                    }

                    using (var resourceStream = CacheContext.OpenResourceCacheReadWrite(entry.Key))
                    {
                        for (var i = resourceCache.Count - 1; i >= 0; i--)
                        {
                            if (entry.Value.Contains(i))
                                continue;

                            Console.Write($"Nulling {entry.Key} resource {i}...");
                            resourceCache.NullResource(resourceStream, i);
                            Console.WriteLine("done.");
                        }
                    }
                }
            }

            return true;
        }

        private void LoadTagDependencies(int index, ref HashSet<int> tags)
        {
            var queue = new List<int> { index };

            while (queue.Count != 0)
            {
                var nextQueue = new List<int>();

                foreach (var entry in queue)
                {
                    if (!tags.Contains(entry))
                    {
                        if (CacheContext.TagCache.Index[entry] == null)
                            continue;

                        tags.Add(entry);

                        foreach (var dependency in CacheContext.TagCache.Index[entry].Dependencies)
                        {
                            if (dependency == entry)
                                continue;

                            if (!nextQueue.Contains(dependency))
                                nextQueue.Add(dependency);
                        }
                    }
                }

                queue = nextQueue;
            }
        }
    }
}