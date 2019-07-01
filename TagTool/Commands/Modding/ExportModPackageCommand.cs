using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Sytem.IO;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;

namespace TagTool.Commands.Modding
{
    class ExportModPackageCommand : Command
    {
        public const uint Version = 1;

        private HaloOnlineCacheContext CacheContext { get; }

        public ExportModPackageCommand(HaloOnlineCacheContext cacheContext) :
            base(false,

                "ExportModPackage",
                "",

                "ExportModPackage [PromptTags] [PromptMaps] [From: <Index>] [To: <Index>] [ForCache: <Directory>] [TagList <file>] <Package File>",

                "")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 11)
                return false;

            bool promptTags = false;
            bool promptMaps = false;
            bool tagList = false;
            string tagListFile = "";
            int? fromIndex = null;
            int? toIndex = null;
            string forCache = null;

            while (args.Count != 1)
            {
                switch (args[0].ToLower())
                {
                    case "prompttags":
                        promptTags = true;
                        args.RemoveRange(0, 1);
                        break;

                    case "promptmaps":
                        promptMaps = true;
                        args.RemoveRange(0, 1);
                        break;

                    case "from:":
                        if (CacheContext.TryGetTag(args[1], out var fromInstance) && fromInstance != null)
                            fromIndex = fromInstance.Index;
                        args.RemoveRange(0, 2);
                        break;

                    case "to:":
                        if (CacheContext.TryGetTag(args[1], out var toInstance) && toInstance != null)
                            toIndex = toInstance.Index;
                        args.RemoveRange(0, 2);
                        break;

                    case "forCache:":
                        forCache = args[1];
                        args.RemoveRange(0, 2);
                        break;


                    case "taglist":
                        tagList = true;
                        tagListFile = args[1];
                        args.RemoveRange(0, 2);

                        if (!File.Exists(tagListFile))
                        {
                            Console.Write("Tag list not found!");
                            tagList = false;
                        }
                            
                        break;
                    default:
                        throw new ArgumentException(args[0]);
                }
            }

            var modPackage = new ModPackageSimplified();

            CacheContext.CreateTagCache(modPackage.TagsStream);
            modPackage.Tags = new TagCache(modPackage.TagsStream, new Dictionary<int, string>());

            CacheContext.CreateResourceCache(modPackage.ResourcesStream);
            modPackage.Resources = new ResourceCache(modPackage.ResourcesStream);

            Console.WriteLine("Enter the name of the mod package:");
            modPackage.Metadata.Name = Console.ReadLine().Trim();

            Console.WriteLine();
            Console.WriteLine("Enter the description of the mod package:");
            modPackage.Metadata.Description = Console.ReadLine().Trim();

            Console.WriteLine();
            Console.WriteLine("Enter the author of the mod package:");
            modPackage.Metadata.Author = Console.ReadLine().Trim();

            if (!promptTags && !promptMaps && !fromIndex.HasValue && !toIndex.HasValue)
            {
                promptTags = true;
                promptMaps = true;
            }

            if (fromIndex.HasValue && !toIndex.HasValue)
                toIndex = CacheContext.TagCache.Index.NonNull().Last().Index;

            var packageFile = new FileInfo(args[0]);

            var tagIndices = new HashSet<int>();
            var mapFiles = new HashSet<string>();
            string line = null;

            if (fromIndex.HasValue && toIndex.HasValue)
                foreach (var entry in Enumerable.Range(fromIndex.Value, toIndex.Value - fromIndex.Value + 1))
                    if (!tagIndices.Contains(entry) && CacheContext.GetTag(entry) != null)
                        tagIndices.Add(entry);

            if (promptTags)
            {
                Console.WriteLine("Please specify the tags to be used (enter an empty line to finish):");

                while ((line = Console.ReadLine().TrimStart().TrimEnd()) != "")
                    if (CacheContext.TryGetTag(line, out var instance) && instance != null && !tagIndices.Contains(instance.Index))
                        tagIndices.Add(instance.Index);
            }

            if (promptMaps)
            {
                Console.WriteLine("Please specify the .map files to be used (enter an empty line to finish):");

                while ((line = Console.ReadLine().TrimStart().TrimEnd()) != "")
                {
                    var mapFile = new FileInfo(line);

                    if (mapFile.Exists && mapFile.Extension == ".map" && !mapFiles.Contains(mapFile.FullName))
                        mapFiles.Add(mapFile.FullName);
                }
            }

            if (tagList)
            {
                using (var tagListStream = File.Open(tagListFile, FileMode.Open, FileAccess.Read))
                {
                    var reader = new StreamReader(tagListStream);

                    while (!reader.EndOfStream)
                    {
                        var tagName = reader.ReadLine();
                        if (CacheContext.TryGetTag(tagName, out var instance) && instance != null && !tagIndices.Contains(instance.Index))
                            tagIndices.Add(instance.Index);
                    }

                    reader.Close();
                }
            }

            foreach (var entry in mapFiles)
            {
                var mapFile = new FileInfo(entry);

                using (var mapFileStream = mapFile.OpenRead())
                {
                    var cacheStream = new MemoryStream();
                    mapFileStream.CopyTo(cacheStream);

                    modPackage.CacheStreams.Add(cacheStream);
                }
            }

            using (var srcTagStream = CacheContext.OpenTagCacheRead())
            {
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
                        modPackage.Tags.AllocateTag();
                        continue;
                    }

                    var srcTag = CacheContext.GetTag(tagIndex);
                    var destTag = modPackage.Tags.AllocateTag(srcTag.Group, srcTag.Name);

                    using (var tagDataStream = new MemoryStream(CacheContext.TagCache.ExtractTagRaw(srcTagStream, srcTag)))
                    using (var tagDataReader = new EndianReader(tagDataStream, leaveOpen: true))
                    using (var tagDataWriter = new EndianWriter(tagDataStream, leaveOpen: true))
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
                                var newResourceIndex = modPackage.Resources.AddRaw(
                                    modPackage.ResourcesStream,
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

                        modPackage.Tags.SetTagDataRaw(modPackage.TagsStream, destTag, tagDataStream.ToArray());
                    }
                }

                modPackage.Tags.UpdateTagOffsets(new BinaryWriter(modPackage.TagsStream, Encoding.Default, true));

                modPackage.Save(packageFile);
            }

            return true;
        }
    }
}
