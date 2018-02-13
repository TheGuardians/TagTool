using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags.Definitions;
using System.IO;
using TagTool.Common;
using TagTool.Commands;

namespace TagTool.Commands.RenderModels
{
    class ResourceDataCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private RenderModel Definition { get; }

        public ResourceDataCommand(GameCacheContext cacheContext, CachedTagInstance tag, RenderModel definition)
            : base(CommandFlags.Inherit,

                  "ResourceData",
                  "Extract or import a mode tag's resource.",

                  "ResourceData <extract/import> [cache type (resources.dat or resources_b.dat)] <filename>",

                  "Extract or import a mode tag's resource.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 3 || args.Count > 4)
                return false;

            var command = args[0];
            args.RemoveAt(0);

            string cacheType = args[0].Split(".".ToCharArray())[0];
            args.RemoveAt(0);

            var filePath = args[0]; // filename

            switch (command.ToLower())
            {
                case "extract":
                    return ExtractResource(Definition, CacheContext, filePath, cacheType);

                case "import":
                    return ImportResource(Definition, CacheContext, filePath, cacheType);

                default:
                    return false;
            }
        }

        private static bool ExtractResource(RenderModel Definition, GameCacheContext CacheContext, string filePath, string cacheType)
        {
            int resourceIndex = 0;
            uint compressedSize = 0;

            try
            {
                resourceIndex = Definition.Geometry.Resource.Page.Index;
                compressedSize = Definition.Geometry.Resource.Page.CompressedBlockSize;
            }
            catch { Console.WriteLine("ERROR: resource index out of range."); };

            try
            {
                switch (cacheType)
                {
                    case "resources":
                    case "resources_b":
                    case "video":
                    case "audio":
                    case "textures":
                    case "textures_b":
                        using (var stream = File.OpenRead(CacheContext.TagCacheFile.DirectoryName + "\\" + cacheType + ".dat"))
                        {
                            var cache = new ResourceCache(stream);
                            using (var outStream = File.Open(filePath, FileMode.Create, FileAccess.Write))
                            {
                                cache.Decompress(stream, (int)resourceIndex, compressedSize, outStream);
                                Console.WriteLine("Wrote 0x{0:X} bytes to {1}.", outStream.Position, filePath);
                            }
                        }
                        break;
                    default:
                        throw new Exception("Cache type unsupported or name is incorrect.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to extract resource: {0}", ex.Message);
            }

            return true;
        }

        private static bool ImportResource(RenderModel Definition, GameCacheContext CacheContext, string filePath, string cacheType)
        {

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                switch (cacheType)
                {
                    case "resources":
                        try
                        {
                            CacheContext.AddResource(Definition.Geometry.Resource, ResourceLocation.Resources, stream);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed to import resource: {0}", ex.Message);
                        }
                        break;
                    case "video":
                    case "resources_b":
                        try
                        {
                            CacheContext.AddResource(Definition.Geometry.Resource, ResourceLocation.ResourcesB, stream);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed to import resource: {0}", ex.Message);
                        }
                        break;

                    default:
                        Console.WriteLine($"Cache type unsupported.");
                        return false;
                }
            }

            Console.WriteLine($"Imported the new resource. " +
                $"Index: 0x{Definition.Geometry.Resource.Page.Index:X4}, \n" +
                $"Index: {Definition.Geometry.Resource.Page.Index}, \n" +
                $"NewLocationFlags {Definition.Geometry.Resource.Page.NewFlags}, \n" +
                $"OldLocationFlags {Definition.Geometry.Resource.Page.OldFlags}, \n" +
                $"CompressedSize {Definition.Geometry.Resource.Page.CompressedBlockSize}, \n" +
                $"CompressedSize {Definition.Geometry.Resource.Page.CompressedBlockSize:X8}");


            return true;
        }
    }
}
