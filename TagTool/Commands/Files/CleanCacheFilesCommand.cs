using TagTool.Cache;
using TagTool.IO;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Common;

namespace TagTool.Commands.Files
{
    class CleanCacheFilesCommand : Command
    {
        public HaloOnlineCacheContext CacheContext { get; }

        public CleanCacheFilesCommand(HaloOnlineCacheContext cacheContext)
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

            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            using (var reader = new EndianReader(cacheStream))
            using (var writer = new EndianWriter(cacheStream))
            {
                var retainedTags = new HashSet<int>();
                LoadTagDependencies(CacheContext.TagCache.Index.FindFirstInGroup("cfgt").Index, ref retainedTags);

                foreach (var scnr in CacheContext.TagCache.Index.FindAllInGroup("scnr"))
                    LoadTagDependencies(scnr.Index, ref retainedTags);

                var resourceIndices = new Dictionary<ResourceLocation, Dictionary<int, PageableResource>>
                {
                    [ResourceLocation.Audio] = new Dictionary<int, PageableResource>(),
                    [ResourceLocation.Lightmaps] = new Dictionary<int, PageableResource>(),
                    [ResourceLocation.RenderModels] = new Dictionary<int, PageableResource>(),
                    [ResourceLocation.Resources] = new Dictionary<int, PageableResource>(),
                    [ResourceLocation.ResourcesB] = new Dictionary<int, PageableResource>(),
                    [ResourceLocation.Textures] = new Dictionary<int, PageableResource>(),
                    [ResourceLocation.TexturesB] = new Dictionary<int, PageableResource>()
                };

                //
                // Set up source resource caches
                //

                var srcResourceCaches = new Dictionary<ResourceLocation, ResourceCache>();
                var srcResourceStreams = new Dictionary<ResourceLocation, Stream>();

                foreach (var value in Enum.GetValues(typeof(ResourceLocation)))
                {
                    ResourceCache resourceCache = null;
                    var location = (ResourceLocation)value;

                    try
                    {
                        resourceCache = CacheContext.GetResourceCache(location);
                    }
                    catch (FileNotFoundException)
                    {
                        continue;
                    }

                    srcResourceCaches[location] = resourceCache;
                    srcResourceStreams[location] = CacheContext.OpenResourceCacheRead(location);
                }

                //
                // Set up destination resource caches
                //

                var destDirectory = new DirectoryInfo("new");
                var destResourceCaches = new Dictionary<ResourceLocation, ResourceCache>();
                var destResourceStreams = new Dictionary<ResourceLocation, Stream>();

                foreach (var entry in srcResourceStreams)
                {
                    var resourceCache = CacheContext.CreateResourceCache(destDirectory, entry.Key);
                    destResourceCaches[entry.Key] = resourceCache;
                    destResourceStreams[entry.Key] = File.Open(Path.Combine(destDirectory.FullName, CacheContext.ResourceCacheNames[entry.Key]), FileMode.Open, FileAccess.ReadWrite);
                }

                //
                // Copy any used resources to the new resource caches
                //

                for (var i = 0; i < CacheContext.TagCache.Index.Count; i++)
                {
                    var tag = CacheContext.TagCache.Index[i];

                    if (tag == null)
                        continue;

                    if (!retainedTags.Contains(i))
                    {
                        var tagName = CacheContext.TagNames.ContainsKey(tag.Index) ? CacheContext.TagNames[tag.Index] : $"0x{tag.Index:X4}";
                        var tagGroupName = CacheContext.GetString(tag.Group.Name);

                        Console.Write($"Nulling {tagName}.{tagGroupName}...");
                        CacheContext.TagCache.NullTag(cacheStream, tag);
                        Console.WriteLine("done.");

                        continue;
                    }

                    foreach (var resourcePointerOffset in tag.ResourcePointerOffsets)
                    {
                        reader.BaseStream.Position = tag.HeaderOffset + resourcePointerOffset;
                        var resourcePointer = reader.ReadUInt32();

                        if (resourcePointer == 0)
                            continue;

                        var resourceOffset = tag.PointerToOffset(resourcePointer);

                        reader.BaseStream.Position = tag.HeaderOffset + resourceOffset + 2;
                        var locationFlags = (OldRawPageFlags)reader.ReadByte();

                        var resourceLocation =
                            locationFlags.HasFlag(OldRawPageFlags.InAudio) ?
                                ResourceLocation.Audio :
                            locationFlags.HasFlag(OldRawPageFlags.InResources) ?
                                ResourceLocation.Resources :
                            locationFlags.HasFlag(OldRawPageFlags.InResourcesB) ?
                                ResourceLocation.ResourcesB :
                            locationFlags.HasFlag(OldRawPageFlags.InTextures) ?
                                ResourceLocation.Textures :
                            locationFlags.HasFlag(OldRawPageFlags.InTexturesB) ?
                                ResourceLocation.TexturesB :
                                ResourceLocation.ResourcesB;

                        reader.BaseStream.Position = tag.HeaderOffset + resourceOffset + 4;
                        var resourceIndex = reader.ReadInt32();
                        var compressedSize = reader.ReadUInt32();

                        if (resourceIndex == -1)
                            continue;

                        PageableResource pageable = null;

                        if (resourceIndices[resourceLocation].ContainsKey(resourceIndex))
                        {
                            pageable = resourceIndices[resourceLocation][resourceIndex];
                        }
                        else
                        {
                            var newResourceIndex = destResourceCaches[resourceLocation].AddRaw(
                                destResourceStreams[resourceLocation],
                                srcResourceCaches[resourceLocation].ExtractRaw(
                                    srcResourceStreams[resourceLocation],
                                    resourceIndex,
                                    compressedSize));

                            pageable = resourceIndices[resourceLocation][resourceIndex] = new PageableResource
                            {
                                Page = new RawPage
                                {
                                    OldFlags =
                                        resourceLocation == ResourceLocation.Audio ?
                                            OldRawPageFlags.InAudio :
                                        resourceLocation == ResourceLocation.Resources ?
                                            OldRawPageFlags.InResources :
                                        resourceLocation == ResourceLocation.Textures ?
                                            OldRawPageFlags.InTextures :
                                        resourceLocation == ResourceLocation.TexturesB ?
                                            OldRawPageFlags.InTexturesB :
                                            OldRawPageFlags.InResourcesB,
                                    Index = newResourceIndex
                                }
                            };
                        }

                        reader.BaseStream.Position = tag.HeaderOffset + resourceOffset + 2;
                        writer.Write((byte)pageable.Page.OldFlags);

                        reader.BaseStream.Position = tag.HeaderOffset + resourceOffset + 4;
                        writer.Write(pageable.Page.Index);
                    }
                }

                //
                // Close resource streams
                //

                foreach (var entry in srcResourceStreams)
                    entry.Value.Close();

                foreach (var entry in destResourceStreams)
                    entry.Value.Close();
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