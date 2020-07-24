using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.IO;
using TagTool.Cache.HaloOnline;
using TagTool.Cache.Resources;

namespace TagTool.Commands.Tags
{
    class TagResourceCommand : Command
    {
        public GameCacheHaloOnlineBase Cache { get; }

        public TagResourceCommand(GameCacheHaloOnlineBase cache) :
            base(true,

                "TagResource",
                "Manage raw resource data",

                "TagResource Extract <Location> <Index> <Compressed Size> <Data File>\n" +
                "TagResource Import <Location> <Index> <Data File>\n" +
                "TagResource ListTags <Location> <Index>",

                "Extracts and imports raw resource data.\n" +
                "When extracting, the compressed size must include chunk headers.\n\n" +
                "Note that this is extremely low-level and does NOT edit tags\n" +
                "which reference imported resources.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 3)
                return new TagToolError(CommandError.ArgCount);

            var command = args[0];

            var location = ResourceLocation.None;

            switch (args[1].ToSnakeCase())
            {
                case "resources":
                    location = ResourceLocation.Resources;
                    break;

                case "textures":
                    location = ResourceLocation.Textures;
                    break;

                case "textures_b":
                    location = ResourceLocation.TexturesB;
                    break;

                case "audio":
                    location = ResourceLocation.Audio;
                    break;

                case "resources_b":
                    location = ResourceLocation.ResourcesB;
                    break;

                case "render_models" when Cache.Version >= CacheVersion.HaloOnline235640:
                    location = ResourceLocation.RenderModels;
                    break;

                case "lightmaps" when Cache.Version >= CacheVersion.HaloOnline235640:
                    location = ResourceLocation.Lightmaps;
                    break;

                default:
                    return new TagToolError(CommandError.ArgInvalid, $"Invalid resource location \"{args[1]}\"");
            }

            if (Cache is GameCacheModPackage)
                location = ResourceLocation.Mods;

            var index = Convert.ToUInt32(args[2], 16);

            switch (command.ToLower())
            {
                case "extract":
                    return ExtractResource(location, index, args);

                case "import":
                    return ImportResource(location, index, args);

                case "listtags":
                    return ListTags(location, index);

                default:
                    return new TagToolError(CommandError.ArgInvalid, $"\"{command}\"");
            }
        }

        private object ListTags(ResourceLocation location, uint index)
        {
            var indices = new List<int>();

            using (var cacheStream = Cache.OpenCacheRead())
            using (var reader = new EndianReader(cacheStream))
            {
                foreach (var instance in Cache.TagCacheGenHO.Tags)
                {
                    if (instance == null || instance.ResourcePointerOffsets.Count == 0)
                        continue;

                    foreach (var offset in instance.ResourcePointerOffsets)
                    {
                        reader.BaseStream.Position = instance.HeaderOffset + offset;
                        var resourcePointer = instance.PointerToOffset(reader.ReadUInt32());

                        if (resourcePointer == 0)
                            continue;

                        reader.BaseStream.Position = instance.HeaderOffset + resourcePointer + 2;
                        var flags = (OldRawPageFlags)reader.ReadByte();

                        if (flags == 0)
                            continue;

                        if (flags.HasFlag(OldRawPageFlags.InResources))
                        {
                            if (location != ResourceLocation.Resources)
                                continue;
                        }
                        else if (flags.HasFlag(OldRawPageFlags.InTextures))
                        {
                            if (location != ResourceLocation.Textures)
                                continue;
                        }
                        else if (flags.HasFlag(OldRawPageFlags.InTexturesB))
                        {
                            if (location != ResourceLocation.TexturesB)
                                continue;
                        }
                        else if (flags.HasFlag(OldRawPageFlags.InAudio))
                        {
                            if (location != ResourceLocation.Audio)
                                continue;
                        }
                        else if (flags.HasFlag(OldRawPageFlags.InResourcesB))
                        {
                            if (location != ResourceLocation.ResourcesB)
                                continue;
                        }
                        else continue;

                        reader.BaseStream.Position = instance.HeaderOffset + resourcePointer + 4;

                        if (reader.ReadInt32() == index)
                        {
                            indices.Add(instance.Index);
                            break;
                        }
                    }
                }
            }

            foreach (var tagIndex in indices)
            {
                var tag = (CachedTagHaloOnline)Cache.TagCache.GetTag(tagIndex);

                if (tag == null)
                    continue;

                var tagName = (tag.Name == null || tag.Name.Length == 0) ? $"0x{tag.Index:X4}" : tag.Name;
                var groupName = tag.Group.ToString();

                Console.WriteLine($"[Index: 0x{tag.Index:X4}, Offset: 0x{tag.HeaderOffset:X8}, Size: 0x{tag.TotalSize:X4}] {tagName}.{groupName}");
            }

            return true;
        }

        private object ExtractResource(ResourceLocation location, uint index, IReadOnlyList<string> args)
        {
            if (args.Count != 5)
                return new TagToolError(CommandError.ArgCount);

            var compressedSize = Convert.ToUInt32(args[3], 16);
            var outPath = args[4];
            var outFile = new FileInfo(args[4]);

            if (!outFile.Directory.Exists)
            {
                Console.Write("ERROR: Directory does not exist. Create it? [y/n]: ");

                switch (Console.ReadLine().ToLower())
                {
                    case "y":
                    case "yes":
                        outFile.Directory.Create();
                        break;

                    default:
                        return true;
                }
            }

            var cache = Cache.ResourceCaches.GetResourceCache(location);

            using (var stream = Cache.ResourceCaches.OpenCacheRead(location))
            {
                using (var outStream = File.Open(outPath, FileMode.Create, FileAccess.Write))
                {
                    cache.Decompress(stream, (int)index, compressedSize, outStream);
                    Console.WriteLine("Wrote 0x{0:X} bytes to {1}.", outStream.Position, outPath);
                }
            }

            return true;
        }

        private object ImportResource(ResourceLocation location, uint index, IReadOnlyList<string> args)
        {
            if (args.Count != 4)
                return new TagToolError(CommandError.ArgCount);

            var inPath = args[3];

            var cache = Cache.ResourceCaches.GetResourceCache(location);

            using (var stream = Cache.ResourceCaches.OpenCacheReadWrite(location))
            {
                var data = File.ReadAllBytes(inPath);
                var compressedSize = cache.Compress(stream, (int)index, data);
                Console.WriteLine("Imported 0x{0:X} bytes.", data.Length);
                Console.WriteLine("Compressed size = 0x{0:X}", compressedSize);
            }

            return true;
        }
    }
}