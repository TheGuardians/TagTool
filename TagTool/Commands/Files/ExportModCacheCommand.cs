using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;

namespace TagTool.Commands.Files
{
    class ExportModCacheCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public ExportModCacheCommand(HaloOnlineCacheContext cacheContext) :
            base(false,
                
                "ExportModCache",
                "",
                
                "ExportModCache <Directory>",
                
                "")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var directory = new DirectoryInfo(args[0]);

            if (!directory.Exists)
                directory.Create();

            using (var srcCacheStream = CacheContext.OpenTagCacheRead())
            {
                var tagIndices = new HashSet<int>();

                Console.WriteLine("Please specify the tags to be used:");

                string line;

                while ((line = Console.ReadLine()) != "")
                    if (CacheContext.TryGetTag(line, out var instance) && instance != null && !tagIndices.Contains(instance.Index))
                        tagIndices.Add(instance.Index);

                var destTagCache = CacheContext.CreateTagCache(directory, out var destTagCacheFile);
                var destResourceCache = CacheContext.CreateResourceCache(directory, ResourceLocation.Mods, out var destResourceCacheFile);

                var resourceIndices = new Dictionary<ResourceLocation, Dictionary<int, PageableResource>>
                {
                    [ResourceLocation.Audio] = new Dictionary<int, PageableResource>(),
                    [ResourceLocation.Lightmaps] = new Dictionary<int, PageableResource>(),
                    [ResourceLocation.Mods] = new Dictionary<int, PageableResource>(),
                    [ResourceLocation.RenderModels] = new Dictionary<int, PageableResource>(),
                    [ResourceLocation.Resources] = new Dictionary<int, PageableResource>(),
                    [ResourceLocation.ResourcesB] = new Dictionary<int, PageableResource>(),
                    [ResourceLocation.Textures] = new Dictionary<int, PageableResource>(),
                    [ResourceLocation.TexturesB] = new Dictionary<int, PageableResource>()
                };

                var srcResourceCaches = new Dictionary<ResourceLocation, ResourceCache>();
                var srcResourceStreams = new Dictionary<ResourceLocation, Stream>();

                foreach (var value in Enum.GetValues(typeof(ResourceLocation)))
                {
                    ResourceCache resourceCache = null;
                    var location = (ResourceLocation)value;

                    if (location == ResourceLocation.None)
                        continue;

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

                using (var destCacheStream = destTagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
                using (var destResourceStream = destResourceCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
                {
                    for (var tagIndex = 0; tagIndex < CacheContext.TagCache.Index.Count; tagIndex++)
                    {
                        if (!tagIndices.Contains(tagIndex))
                        {
                            destTagCache.AllocateTag();
                            continue;
                        }

                        var srcTag = CacheContext.GetTag(tagIndex);
                        var destTag = destTagCache.AllocateTag(srcTag.Group, srcTag.Name);

                        using (var tagDataStream = new MemoryStream(CacheContext.TagCache.ExtractTagRaw(srcCacheStream, srcTag)))
                        using (var tagDataReader = new EndianReader(tagDataStream))
                        using (var tagDataWriter = new EndianWriter(tagDataStream))
                        {
                            var resourcePointerOffsets = new HashSet<uint>();

                            foreach (var resourcePointerOffset in srcTag.ResourcePointerOffsets)
                            {
                                if (resourcePointerOffset == 0 || resourcePointerOffsets.Contains(resourcePointerOffset))
                                    continue;

                                resourcePointerOffsets.Add(resourcePointerOffset);

                                tagDataStream.Position = resourcePointerOffset;
                                var resourcePointer = tagDataReader.ReadUInt32();

                                if (resourcePointer == 0)
                                    continue;

                                var resourceOffset = srcTag.PointerToOffset(resourcePointer);

                                tagDataReader.BaseStream.Position = resourceOffset + 2;
                                var locationFlags = (OldRawPageFlags)tagDataReader.ReadByte();

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

                                tagDataReader.BaseStream.Position = resourceOffset + 4;
                                var resourceIndex = tagDataReader.ReadInt32();
                                var compressedSize = tagDataReader.ReadUInt32();

                                if (resourceIndex == -1)
                                    continue;

                                PageableResource pageable = null;

                                if (resourceIndices[resourceLocation].ContainsKey(resourceIndex))
                                {
                                    pageable = resourceIndices[resourceLocation][resourceIndex];
                                }
                                else
                                {
                                    var newResourceIndex = destResourceCache.AddRaw(
                                        destResourceStream,
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

                                tagDataReader.BaseStream.Position = resourceOffset + 2;
                                tagDataWriter.Write((byte)pageable.Page.OldFlags);

                                tagDataReader.BaseStream.Position = resourceOffset + 4;
                                tagDataWriter.Write(pageable.Page.Index);
                            }

                            destTagCache.SetTagDataRaw(destCacheStream, destTag, tagDataStream.ToArray());
                        }
                    }

                    destTagCache.UpdateTagOffsets(new BinaryWriter(destCacheStream, Encoding.Default, true));
                }
            }

            return true;
        }
    }
}