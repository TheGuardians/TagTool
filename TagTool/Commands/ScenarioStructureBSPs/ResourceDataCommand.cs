using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags.Definitions;
using System.IO;
using TagTool.Common;
using TagTool.Commands;

namespace TagTool.Commands.ScenarioStructureBSPs
{
    class ResourceDataCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private ScenarioStructureBsp Definition { get; }

        public ResourceDataCommand(GameCacheContext cacheContext, CachedTagInstance tag, ScenarioStructureBsp definition)
            : base(CommandFlags.None,

                  "ResourceData",
                  "SbspResource Extract/Import <Resource Type> <Filename>",

                  "ResourceData Extract/Import a sbsp resource.",

                  "Extract/Import a sbsp resource. Resource types: \n" +
                  "Geometry \n" +
                  "Geometry2 \n" +
                  "CollisionBspResource \n" +
                  "PathfindingResource \n")
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

        private static bool ExtractResource(ScenarioStructureBsp Definition, GameCacheContext CacheContext, IReadOnlyList<string> args)
        {
            if (args.Count < 1)
                return false;

            var resourceType = "";
            var filePath = "";
            var cachePath = "resources.dat";

            resourceType = args[1].ToLower(); // geometry2
            filePath = args[2]; // sc140_000_geometry2.raw
            
            PageableResource resource = null;
            
            switch (resourceType)
            {
                case "geometry":
                    resource = Definition.Geometry.Resource;
                    break;

                case "geometry2":
                    resource = Definition.Geometry2.Resource;
                    break;

                case "collisionbspresource":
                    resource = Definition.CollisionBspResource;
                    break;

                case "pathfindingresource":
                    resource = Definition.PathfindingResource;
                    break;

                default:
                    Console.WriteLine($"Invalid sbsp resource type.");
                    return false;
            }

            if (resource == null || resource.Page.Index < 0 || !resource.GetLocation(out var location))
            {
                Console.WriteLine("Resource is null.");
                return false;
            }

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

        private static bool ImportResource(ScenarioStructureBsp Definition, GameCacheContext CacheContext, IReadOnlyList<string> args)
        {
            if (args.Count != 4)
                return false;

            var location = args[1].Split(".".ToCharArray()).Length == 2 ? args[1].Split(".".ToCharArray())[0] : args[1]; // resources.dat or resources
            var resourceType = args[2].ToLower(); // geometry2
            var filePath = args[3]; // sc140_000_geometry2.raw
            var resourceLocation = ResourceLocation.Resources;

            switch(location)
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

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                switch (resourceType)
                {
                    case "geometry":
                        CacheContext.AddResource(Definition.Geometry.Resource, resourceLocation, stream);
                        Console.WriteLine($"New Geometry resource index = {Definition.Geometry.Resource.Page.Index:X4}; {Definition.Geometry.Resource.Page.Index:D4}");
                        break;
                    case "geometry2":
                        CacheContext.AddResource(Definition.Geometry2.Resource, resourceLocation, stream);
                        Console.WriteLine($"New Geometry2 resource index = {Definition.Geometry2.Resource.Page.Index:X4}; {Definition.Geometry2.Resource.Page.Index:D4}");
                        break;
                    case "collisionbspresource":
                        CacheContext.AddResource(Definition.CollisionBspResource, resourceLocation, stream);
                        Console.WriteLine($"New CollisionBspResource resource index = {Definition.CollisionBspResource.Page.Index:X4}; {Definition.CollisionBspResource.Page.Index:D4}");
                        break;
                    case "pathfindingresource":
                        CacheContext.AddResource(Definition.PathfindingResource, resourceLocation, stream);
                        Console.WriteLine($"New PathfindingResource resource index = {Definition.PathfindingResource.Page.Index:X4}; {Definition.PathfindingResource.Page.Index:D4}");
                        break;
                    default:
                        Console.WriteLine("ERROR: Unrecognized resource type");
                        return false;
                }
                Console.WriteLine("Done.");
            }

            return true;
        }
    }
}
