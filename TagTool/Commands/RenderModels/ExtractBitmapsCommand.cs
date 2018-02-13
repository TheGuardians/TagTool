using TagTool.Bitmaps;
using TagTool.Cache;
using TagTool.Commands;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Tags;

namespace TagTool.Commands.RenderModels
{
    class ExtractBitmapsCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private RenderModel Definition { get; }

        public ExtractBitmapsCommand(GameCacheContext cacheContext, CachedTagInstance tag, RenderModel definition)
            : base(CommandFlags.Inherit,
                  
                  "ExtractBitmaps",
                  "Extracts all bitmaps used by the render model's shaders to a specific directory.",
                  
                  "ExtractBitmaps <Output Folder>",

                  "Extracts all bitmaps used by the render model's shaders to a specific directory.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var directory = args[0];

            if (!Directory.Exists(directory))
            {
                Console.Write("Destination directory does not exist. Create it? [y/n] ");
                var answer = Console.ReadLine().ToLower();

                if (answer.Length == 0 || !(answer.StartsWith("y") || answer.StartsWith("n")))
                    return false;

                if (answer.StartsWith("y"))
                    Directory.CreateDirectory(directory);
                else
                    return false;
            }

            using (var cacheStream = CacheContext.TagCacheFile.OpenRead())
            {
                foreach (var shader in Definition.Materials)
                {
                    var context = new TagSerializationContext(cacheStream, CacheContext, shader.RenderMethod);
                    var renderMethod = (RenderMethod)CacheContext.Deserializer.Deserialize(context, TagDefinition.Find(shader.RenderMethod.Group.Tag));

                    foreach (var property in renderMethod.ShaderProperties)
                    {
                        context = new TagSerializationContext(cacheStream, CacheContext, property.Template);
                        var template = CacheContext.Deserializer.Deserialize<RenderMethodTemplate>(context);

                        for (var i = 0; i < template.ShaderMaps.Count; i++)
                        {
                            var mapTemplate = template.ShaderMaps[i];

                            var extractor = new BitmapDdsExtractor(CacheContext);

                            context = new TagSerializationContext(cacheStream, CacheContext, property.ShaderMaps[i].Bitmap);
                            var bitmap = CacheContext.Deserializer.Deserialize<Bitmap>(context);
                            var ddsOutDir = directory;

                            if (bitmap.Images.Count > 1)
                            {
                                ddsOutDir = Path.Combine(directory, property.ShaderMaps[i].Bitmap.Index.ToString("X8"));
                                Directory.CreateDirectory(ddsOutDir);
                            }

                            for (var j = 0; j < bitmap.Images.Count; j++)
                            {
                                var outPath = Path.Combine(ddsOutDir, CacheContext.GetString(mapTemplate.Name) + "_" + property.ShaderMaps[i].Bitmap.Index.ToString("X4")) + ".dds";

                                using (var outStream = File.Open(outPath, FileMode.Create, FileAccess.Write))
                                    extractor.ExtractDds(bitmap, j, outStream);

                                Console.WriteLine($"Bitmap {i} ({CacheContext.GetString(mapTemplate.Name)}): {property.ShaderMaps[i].Bitmap.Group.Tag} 0x{property.ShaderMaps[i].Bitmap.Index:X4} extracted to '{outPath}'");
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}