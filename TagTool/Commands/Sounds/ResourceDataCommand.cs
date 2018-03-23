using TagTool.Cache;
using TagTool.Commands;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Commands.Sounds
{
    class ResourceDataCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private Sound Definition { get; }

        public ResourceDataCommand(GameCacheContext cacheContext, CachedTagInstance tag, Sound definition)
            : base(CommandFlags.None,

                  "ResourceData",
                  "snd!Resource Extract/Import<Filename>",

                  "ResourceData Extract/Import a snd! resource.",

                  "Extract/Import a snd! resource.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1)
                return false;

            var command = args[0];
            args.RemoveAt(0);

            switch (command.ToLower())
            {
                case "extract":
                    return ExtractResource(Definition, CacheContext, args);

                case "import":
                    return ImportResource(Definition, CacheContext, args);

                default:
                    return false;
            }
        }

        private static bool ExtractResource(Sound Definition, GameCacheContext CacheContext, IReadOnlyList<string> args)
        {
            if (args.Count != 1)
                return false;
            
            var filePath = args[0];

            var resource = Definition.Resource;

            if (resource == null || resource.Page.Index < 0 || !resource.GetLocation(out var location))
            {
                Console.WriteLine("Resource is null.");
                return false;
            }

            var cachePath = CacheContext.ResourceCacheNames[location];

            try
            {
                using (var stream = File.OpenRead(CacheContext.TagCacheFile.DirectoryName + "\\" + cachePath))
                {
                    var cache = new ResourceCache(stream);
                    using (var outStream = File.Open(filePath, FileMode.Create, FileAccess.Write))
                    {
                        cache.Decompress(stream, resource.Page.Index, resource.Page.CompressedBlockSize, outStream);
                        Console.WriteLine("Wrote 0x{0:X} bytes to {1}.", outStream.Position, filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to extract resource: {0}", ex.Message);
            }

            return true;
        }

        private static bool ImportResource(Sound Definition, GameCacheContext CacheContext, IReadOnlyList<string> args)
        {
            if (args.Count != 2)
                return false;

            var location = args[0].Split(".".ToCharArray())[0];
            var filePath = args[1];

            var resourceLocation = ResourceLocation.Resources;

            switch (location)
            {
                case "audio":
                    resourceLocation = ResourceLocation.Audio;
                    break;
                case "resources":
                    resourceLocation = ResourceLocation.Resources;
                    break;
                case "resources_b":
                case "video":
                    resourceLocation = ResourceLocation.ResourcesB;
                    break;
                case "textures":
                    resourceLocation = ResourceLocation.Textures;
                    break;
                case "textures_b":
                    resourceLocation = ResourceLocation.TexturesB;
                    break;
                case "lightmaps":
                    resourceLocation = ResourceLocation.Lightmaps;
                    break;
                case "render_models":
                    resourceLocation = ResourceLocation.RenderModels;
                    break;
                default:
                    Console.WriteLine($"ERROR: Cache location is incorrect.");
                    return false;
            }

            try
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    CacheContext.AddResource(Definition.Resource, resourceLocation, stream);
                    Console.WriteLine($"New Geometry resource index = 0x{Definition.Resource.Page.Index:X4}, {Definition.Resource.Page.Index:D4}");
                }

                Console.WriteLine("Done.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: could not import resource: {e.Message}");
            }

            return true;
        }
    }
}
