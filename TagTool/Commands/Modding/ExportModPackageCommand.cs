using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.IO;
using TagTool.Cache.HaloOnline;
using TagTool.BlamFile;
using TagTool.Cache.Resources;
using TagTool.Extensions;
using TagTool.Cache.Gen3;

namespace TagTool.Commands.Modding
{
    class ExportModPackageCommand : Command
    {
        private GameCacheHaloOnlineBase CacheContext { get; }

        private ExportOptions Options = ExportOptions.None;

        private ModPackage ModPackage = null;

        public ExportModPackageCommand(GameCacheHaloOnlineBase cacheContext) :
            base(false,

                "ExportModPackage",
                "NO LONGER SUPPORTED - use CreateModPackage instead",

                "ExportModPackage [TagFile] [TagList] [TagBounds] [MapFiles] [CampaignFile] [FontPackageFile] <Package File>",

                "NO LONGER SUPPORTED - use CreateModPackage instead")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 7)
                return new TagToolError(CommandError.ArgCount);

            string packageName;
            string line = null;

            //
            // Parse input options
            //
            while (args.Count != 1)
            {
                var arg = args[0].ToLower();
                switch (arg)
                {
                    case "tagfile":
                        Options |= ExportOptions.TagFile;
                        break;

                    case "taglist":
                        Options |= ExportOptions.TagList;
                        break;

                    case "tagbounds":
                        Options |= ExportOptions.TagBounds;
                        break;

                    case "mapfiles":
                    case "mapfile":
                        Options |= ExportOptions.MapFiles;
                        break;

                    case "campaignfile":
                        Options |= ExportOptions.CampaignFile;
                        break;

                    case "fontpackage":
                    case "fontpackagefile":
                        Options |= ExportOptions.FontPackage;
                        break;

                    case "files":
                        Options |= ExportOptions.Files;
                        break;

                    default:
                        Console.WriteLine($"Invalid argument: {arg}");
                        break;
                }
                args.RemoveAt(0);
            }

            packageName = args[0];

            ModPackage = new ModPackage();

            //
            // Process options and create mod package
            //

            // assign everything to the first cache for now

            AddStringIds();

            var tagStream = ModPackage.TagCachesStreams[0];
            ModPackage.CacheNames.Add("default");

            // temporary cache for holding tags and resources
            TagCacheHaloOnline modPackTagCache = new TagCacheHaloOnline(tagStream, ModPackage.StringTable, new Dictionary<int, string>());
            ResourceCacheHaloOnline modPackResourceCache = new ResourceCacheHaloOnline(CacheVersion.HaloOnline106708, ModPackage.ResourcesStream);

            CreateDescription();

            var tagIndices = new HashSet<int>();

            if (Options.HasFlag(ExportOptions.TagBounds))
            {
                int? fromIndex = -1;
                int? toIndex = CacheContext.TagCache.NonNull().Last().Index;

                Console.WriteLine("Please specify the start index to be used:");
                string input = line = Console.ReadLine().TrimStart().TrimEnd();
                if (CacheContext.TagCache.TryGetTag(input, out var fromInstance) && fromInstance != null)
                    fromIndex = fromInstance.Index;

                if (fromIndex != -1)
                {
                    Console.WriteLine("Please specify the end index to be used (press enter to skip):");
                    input = Console.ReadLine().TrimStart().TrimEnd();
                    if(input != "")
                    {
                        if (CacheContext.TagCache.TryGetTag(input, out var toInstance) && fromInstance != null)
                            toIndex = toInstance.Index;
                        else
                        {
                            return new TagToolError(CommandError.ArgInvalid, "Invalid end index");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Invalid start index");
                    return false;
                }

                // add tags to list
                foreach (var entry in Enumerable.Range(fromIndex.Value, toIndex.Value - fromIndex.Value + 1))
                    if (!tagIndices.Contains(entry) && CacheContext.TagCache.GetTag(entry) != null)
                        tagIndices.Add(entry);

            }

            if (Options.HasFlag(ExportOptions.TagFile))
            {
                Console.WriteLine("Enter the name of the tag list file (csv): ");
                string tagFile = Console.ReadLine().Trim();

                using (var tagListStream = File.Open(tagFile, FileMode.Open, FileAccess.Read))
                {
                    var reader = new StreamReader(tagListStream);

                    while (!reader.EndOfStream)
                    {
                        var tagName = reader.ReadLine();
                        if (CacheContext.TagCache.TryGetTag(tagName, out var instance) && instance != null)
                            if(!tagIndices.Contains(instance.Index))
                                tagIndices.Add(instance.Index);
                        else
                            Console.WriteLine($"Falied to find tag {tagName}");
                    }

                    reader.Close();
                }
            }

            if (Options.HasFlag(ExportOptions.TagList)){
                Console.WriteLine("Please specify the tags to be used (enter an empty line to finish):");

                while ((line = Console.ReadLine().TrimStart().TrimEnd()) != "")
                    if (CacheContext.TagCache.TryGetTag(line, out var instance) && instance != null && !tagIndices.Contains(instance.Index))
                        tagIndices.Add(instance.Index);
            }

            if (Options.HasFlag(ExportOptions.MapFiles))
            {
                var mapFileNames = new HashSet<string>();

                Console.WriteLine("Please specify the .map files to be used (enter an empty line to finish):");

                while ((line = Console.ReadLine().TrimStart().TrimEnd()) != "")
                {
                    if (!File.Exists(line))
                    {
                        Console.WriteLine($"File {line} does not exist. Please enter a valid map file");
                        continue;
                    }
                    else
                    {
                        var mapFile = new FileInfo(line);
                        if (mapFile.Extension != ".map")
                        {
                            Console.WriteLine($"File {line} is not a map file.");
                            continue;
                        }
                            
                        if(!mapFileNames.Contains(mapFile.FullName))
                            mapFileNames.Add(mapFile.FullName);
                    }
                }
                AddMaps(mapFileNames);
            }

            if (Options.HasFlag(ExportOptions.CampaignFile))
            {
                Console.WriteLine("Please specify the .campaign file to be used:");
                while (true)
                {
                    string campaignFileName = Console.ReadLine().TrimStart().TrimEnd();
                    if (!File.Exists(campaignFileName))
                        Console.WriteLine($"File {line} does not exist.");
                    else
                    {
                        var file = new FileInfo(campaignFileName);
                        if (file.Extension != ".campaign")
                        {
                            Console.WriteLine($"File {line} is not a campaign file.");
                            continue;
                        }
                        else
                        {
                            AddCampaignMap(file);
                            break;
                        }
                    }
                }
            }

            if (Options.HasFlag(ExportOptions.FontPackage))
            {
                Console.WriteLine("Using fonts\\font_package.bin");
                AddFontPackage();
            }

            if (Options.HasFlag(ExportOptions.Files))
            {
                Console.WriteLine("Please specify the directory containing the extra files to be added:");
                var directory = new DirectoryInfo(Console.ReadLine());
                if(!directory.Exists)
                {
                    Console.WriteLine($"ERROR: Directory does not exist.");
                    return new TagToolError(CommandError.DirectoryNotFound);
                }

                AddFiles(directory);
            }

            //
            // Use the tag list collected to create new mod package
            //

            Console.WriteLine("Building...");

            
            AddTags(tagIndices, modPackTagCache, modPackResourceCache);
            
            ModPackage.Save(new FileInfo(packageName));

            Console.WriteLine("Done!");

            return true;
        }

        [Flags]
        private enum ExportOptions
        {
            None = 0,
            TagFile = 1 << 0,
            TagList = 1 << 1,
            TagBounds = 1 << 2,
            MapFiles = 1 << 3,
            CampaignFile = 1 << 4,
            FontPackage = 1 << 5,
            Files = 1 << 6
        }

        private void CreateDescription()
        {
            Console.WriteLine("Enter the name of the mod package:");
            ModPackage.Metadata.Name = Console.ReadLine().Trim();

            Console.WriteLine();
            Console.WriteLine("Enter the description of the mod package:");
            ModPackage.Metadata.Description = Console.ReadLine().Trim();

            Console.WriteLine();
            Console.WriteLine("Enter the author of the mod package:");
            ModPackage.Metadata.Author = Console.ReadLine().Trim();

            ModPackage.Metadata.BuildDateLow = (int)DateTime.Now.ToFileTime() & 0x7FFFFFFF;
            ModPackage.Metadata.BuildDateHigh = (int)((DateTime.Now.ToFileTime() & 0x7FFFFFFF00000000) >> 32);
        }

        private void AddTags(HashSet<int> tagIndices, TagCacheHaloOnline modTagCache, ResourceCacheHaloOnline modPackResourceCache)
        {
            // define current cache tags, names
            var modTagNames = ModPackage.TagCacheNames[0];
            var modTagStream = ModPackage.TagCachesStreams[0];

            using (var srcTagStream = CacheContext.OpenCacheRead())
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

                var srcResourceCaches = new Dictionary<ResourceLocation, ResourceCacheHaloOnline>();
                var srcResourceStreams = new Dictionary<ResourceLocation, Stream>();

                foreach (var value in Enum.GetValues(typeof(ResourceLocation)))
                {
                    ResourceCacheHaloOnline resourceCache = null;
                    var location = (ResourceLocation)value;

                    if (location == ResourceLocation.None || location == ResourceLocation.RenderModels || location == ResourceLocation.Lightmaps || location == ResourceLocation.Mods)
                        continue;

                    try
                    {
                        resourceCache = CacheContext.ResourceCaches.GetResourceCache(location);
                    }
                    catch (FileNotFoundException)
                    {
                        continue;
                    }

                    srcResourceCaches[location] = resourceCache;
                    srcResourceStreams[location] = CacheContext.ResourceCaches.OpenCacheRead(location);
                }

                for (var tagIndex = 0; tagIndex < CacheContext.TagCache.Count; tagIndex++)
                {
                    var srcTag = CacheContext.TagCache.GetTag(tagIndex);

                    if (srcTag == null)
                    {
                        modTagCache.AllocateTag(new TagGroupGen3());
                        continue;
                    }
                        
                    if (!tagIndices.Contains(tagIndex))
                    {
                        var emptyTag = modTagCache.AllocateTag(srcTag.Group, srcTag.Name);
                        var cachedTagData = new CachedTagData();
                        cachedTagData.Data = new byte[0];
                        cachedTagData.Group = (TagGroupGen3)emptyTag.Group;
                        modTagCache.SetTagData(modTagStream, (CachedTagHaloOnline)emptyTag, cachedTagData);
                        continue;
                    }
                    
                    var destTag = modTagCache.AllocateTag(srcTag.Group, srcTag.Name);

                    using (var tagDataStream = new MemoryStream(CacheContext.TagCacheGenHO.ExtractTagRaw(srcTagStream, (CachedTagHaloOnline)srcTag)))
                    using (var tagDataReader = new EndianReader(tagDataStream, leaveOpen: true))
                    using (var tagDataWriter = new EndianWriter(tagDataStream, leaveOpen: true))
                    {
                        var resourcePointerOffsets = new HashSet<uint>();

                        foreach (var resourcePointerOffset in ((CachedTagHaloOnline)srcTag).ResourcePointerOffsets)
                        {
                            if (resourcePointerOffset == 0 || resourcePointerOffsets.Contains(resourcePointerOffset))
                                continue;

                            resourcePointerOffsets.Add(resourcePointerOffset);

                            tagDataStream.Position = resourcePointerOffset;
                            var resourcePointer = tagDataReader.ReadUInt32();

                            if (resourcePointer == 0)
                                continue;

                            var resourceOffset = ((CachedTagHaloOnline)srcTag).PointerToOffset(resourcePointer);

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
                                var newResourceIndex = modPackResourceCache.AddRaw(
                                    ModPackage.ResourcesStream,
                                    srcResourceCaches[resourceLocation].ExtractRaw(
                                        srcResourceStreams[resourceLocation],
                                        resourceIndex,
                                        compressedSize));

                                pageable = resourceIndices[resourceLocation][resourceIndex] = new PageableResource
                                {
                                    Page = new ResourcePage
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

                        modTagCache.SetTagDataRaw(modTagStream, (CachedTagHaloOnline)destTag, tagDataStream.ToArray());
                    }
                }

                foreach(var stream in srcResourceStreams.Values)
                {
                    if (stream != null)
                        stream.Dispose();
                }

                modTagCache.UpdateTagOffsets(new EndianWriter(modTagStream));
            }
        }

        private void AddMaps(HashSet<string> mapFileNames)
        {
            foreach (var entry in mapFileNames)
            {
                if (!File.Exists(entry))
                {
                    Console.WriteLine("$Map file {entry} not found.");
                    continue;
                }

                var mapFile = new FileInfo(entry);

                using (var mapFileStream = mapFile.OpenRead())
                using(var reader = new EndianReader(mapFileStream))
                {
                    var cacheStream = new MemoryStream();
                    mapFileStream.CopyTo(cacheStream);

                    MapFile map = new MapFile();
                    map.Read(reader);
                    // TODO: specify cache per map
                    ModPackage.AddMap(cacheStream, ((CacheFileHeader)map.Header).MapId , 0);

                }
            }
        }

        private void AddCampaignMap(FileInfo campaignFile)
        {
            using (var mapFileStream = campaignFile.OpenRead())
            {
                ModPackage.CampaignFileStream = new MemoryStream();
                mapFileStream.CopyTo(ModPackage.CampaignFileStream);
            }
        }

        private void AddStringIds()
        {
            ModPackage.StringTable = CacheContext.StringTableHaloOnline;
        }

        private void AddFontPackage()
        {
            var location = Path.Combine(CacheContext.Directory.FullName, $"{CacheContext.Directory.FullName}\\fonts\\font_package.bin");
            var file = new FileInfo(location);
            using(var stream = file.OpenRead())
            {
                ModPackage.FontPackage = new MemoryStream();
                StreamUtil.Copy(stream, ModPackage.FontPackage, stream.Length);
            }
        }

        private void AddFiles(DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles("*.*", SearchOption.AllDirectories))
            {
                string virtualPath = directory.GetRelativePath(file.FullName);
                ModPackage.Files.Add(virtualPath, file.OpenRead());
            }
        }
    }
}
