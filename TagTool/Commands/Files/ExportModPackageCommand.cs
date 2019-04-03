using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Sytem.IO;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;

namespace TagTool.Commands.Files
{
    class ExportModPackageCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public ExportModPackageCommand(HaloOnlineCacheContext cacheContext) :
            base(false,

                "ExportModPackage",
                "",

                "ExportModPackage <Package File>",

                "")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var packageFile = new FileInfo(args[0]);

            using (var srcTagStream = CacheContext.OpenTagCacheRead())
            using (var destTagsStream = new MemoryStream())
            using (var destResourceStream = new MemoryStream())
            {
                var tagIndices = new HashSet<int>();

                Console.WriteLine("Please specify the tags to be used (enter an empty line to finish):");

                string line;

                while ((line = Console.ReadLine().TrimStart().TrimEnd()) != "")
                    if (CacheContext.TryGetTag(line, out var instance) && instance != null && !tagIndices.Contains(instance.Index))
                        tagIndices.Add(instance.Index);

                var mapFiles = new HashSet<string>();

                Console.WriteLine("Please specify the .map files to be used (enter an empty line to finish):");

                while ((line = Console.ReadLine().TrimStart().TrimEnd()) != "")
                {
                    var mapFile = new FileInfo(line);

                    if (mapFile.Exists && mapFile.Extension == ".map" && !mapFiles.Contains(mapFile.FullName))
                        mapFiles.Add(mapFile.FullName);
                }

                var destTagCache = CacheContext.CreateTagCache(destTagsStream);
                var destResourceCache = CacheContext.CreateResourceCache(destResourceStream);

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

                for (var tagIndex = 0; tagIndex < CacheContext.TagCache.Index.Count; tagIndex++)
                {
                    if (!tagIndices.Contains(tagIndex))
                    {
                        destTagCache.AllocateTag();
                        continue;
                    }

                    var srcTag = CacheContext.GetTag(tagIndex);
                    var destTag = destTagCache.AllocateTag(srcTag.Group, srcTag.Name);

                    using (var tagDataStream = new MemoryStream(CacheContext.TagCache.ExtractTagRaw(srcTagStream, srcTag)))
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
                                        OldFlags = OldRawPageFlags.InMods,
                                        Index = newResourceIndex
                                    }
                                };
                            }

                            tagDataReader.BaseStream.Position = resourceOffset + 2;
                            tagDataWriter.Write((byte)pageable.Page.OldFlags);

                            tagDataReader.BaseStream.Position = resourceOffset + 4;
                            tagDataWriter.Write(pageable.Page.Index);
                        }

                        destTagCache.SetTagDataRaw(destTagsStream, destTag, tagDataStream.ToArray());
                    }
                }

                destTagCache.UpdateTagOffsets(new BinaryWriter(destTagsStream, Encoding.Default, true));

                if (!packageFile.Directory.Exists)
                    packageFile.Directory.Create();

                using (var packageStream = packageFile.Create())
                using (var writer = new BinaryWriter(packageStream))
                {
                    //
                    // reserve header space
                    //

                    writer.Write(new byte[48]);

                    //
                    // write tag cache
                    //

                    var tagCacheOffset = (uint)packageStream.Position;

                    destTagsStream.Position = 0;
                    StreamUtil.Copy(destTagsStream, packageStream, (int)destTagsStream.Length);
                    StreamUtil.Align(packageStream, 4);

                    //
                    // write tag names table
                    //

                    var names = new Dictionary<int, string>();

                    foreach (var entry in destTagCache.Index)
                        if (entry != null && entry.Name != null)
                            names[entry.Index] = entry.Name;

                    var tagNamesTableOffset = (uint)packageStream.Position;
                    var tagNamesTableCount = names.Count;

                    foreach (var entry in names)
                    {
                        writer.Write(entry.Key);

                        var chars = new char[256];

                        for (var i = 0; i < entry.Value.Length; i++)
                            chars[i] = entry.Value[i];

                        writer.Write(chars);
                    }

                    //
                    // write resource cache
                    //

                    var resourceCacheOffset = (uint)packageStream.Position;

                    destResourceStream.Position = 0;
                    StreamUtil.Copy(destResourceStream, packageStream, (int)destResourceStream.Length);
                    StreamUtil.Align(packageStream, 4);

                    //
                    // write map file table
                    //

                    var mapFileTableOffset = (uint)packageStream.Position;
                    var mapFileTableCount = mapFiles.Count;

                    var mapFileInfo = new List<(uint, uint)>();
                    writer.Write(new byte[8 * mapFileTableCount]);

                    foreach (var entry in mapFiles)
                    {
                        var mapFile = new FileInfo(entry);

                        using (var mapFileStream = mapFile.OpenRead())
                        {
                            mapFileInfo.Add(((uint)packageStream.Position, (uint)mapFileStream.Length));
                            StreamUtil.Copy(mapFileStream, packageStream, (int)mapFileStream.Length);
                            StreamUtil.Align(packageStream, 4);
                        }
                    }

                    packageStream.Position = mapFileTableOffset;

                    foreach (var entry in mapFileInfo)
                    {
                        writer.Write(entry.Item1);
                        writer.Write(entry.Item2);
                    }

                    //
                    // calculate package sha1
                    //

                    packageStream.Position = 48;
                    var packageSha1 = new SHA1Managed().ComputeHash(packageStream);

                    //
                    // update package header
                    //

                    packageStream.Position = 0;

                    writer.Write(new Tag("mod!"));
                    writer.Write(packageSha1);
                    writer.Write(tagCacheOffset);
                    writer.Write(tagNamesTableCount == 0 ? 0 : tagNamesTableOffset);
                    writer.Write(tagNamesTableCount);
                    writer.Write(resourceCacheOffset);
                    writer.Write(mapFileTableCount == 0 ? 0 : mapFileTableOffset);
                    writer.Write(mapFileTableCount);
                }
            }

            return true;
        }
    }
}