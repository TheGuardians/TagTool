using TagTool.Bitmaps;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.IO;

namespace TagTool.Commands.Models
{
    class ExtractBitmapsCommand : Command
    {
        private GameCache CacheContext { get; }
        private CachedTag Tag { get; }
        private Model Definition { get; }

        public ExtractBitmapsCommand(GameCache cacheContext, CachedTag tag, Model definition)
            : base(true,
                  
                  "ExtractBitmaps",
                  "Extracts all bitmaps used by the model's shaders to a specific directory.",
                  
                  "ExtractBitmaps <Output Folder>",

                  "Extracts all bitmaps used by the model's shaders to a specific directory.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            var directory = args[0];

            if (!Directory.Exists(directory))
            {
                Console.Write("Destination directory does not exist. Create it? [y/n] ");
                var answer = Console.ReadLine().ToLower();

                if (answer.Length == 0 || !(answer.StartsWith("y") || answer.StartsWith("n")))
                    return new TagToolError(CommandError.YesNoSyntax);

                if (answer.StartsWith("y"))
                    Directory.CreateDirectory(directory);
                else
                    return true;
            }

            using (var cacheStream = CacheContext.OpenCacheRead())
            {
                var renderModel = CacheContext.Deserialize<RenderModel>(cacheStream, Definition.RenderModel);

                foreach (var shader in renderModel.Materials)
                {
                    var renderMethod = (RenderMethod)CacheContext.Deserialize(cacheStream, shader.RenderMethod);

                    foreach (var property in renderMethod.ShaderProperties)
                    {
                        var template = CacheContext.Deserialize<RenderMethodTemplate>(cacheStream, property.Template);

                        for (var i = 0; i < template.TextureParameterNames.Count; i++)
                        {
                            var mapTemplate = template.TextureParameterNames[i];

                            var bitmap = CacheContext.Deserialize<Bitmap>(cacheStream, property.TextureConstants[i].Bitmap);
                            var ddsOutDir = directory;

                            if (bitmap.Images.Count > 1)
                            {
                                ddsOutDir = Path.Combine(directory, property.TextureConstants[i].Bitmap.Index.ToString("X8"));
                                Directory.CreateDirectory(ddsOutDir);
                            }

                            for (var j = 0; j < bitmap.Images.Count; j++)
                            {
                                var outPath = Path.Combine(ddsOutDir, CacheContext.StringTable.GetString(mapTemplate.Name) + "_" + property.TextureConstants[i].Bitmap.Index.ToString("X4")) + ".dds";

                                using (var outStream = File.Open(outPath, FileMode.Create, FileAccess.Write))
                                using(var writer = new EndianWriter(outStream))
                                {
                                    var ddsFile = BitmapExtractor.ExtractBitmap(CacheContext, bitmap, j);
                                    ddsFile.Write(writer);
                                }
                                    

                                Console.WriteLine($"Bitmap {i} ({CacheContext.StringTable.GetString(mapTemplate.Name)}): {property.TextureConstants[i].Bitmap.Group.Tag} 0x{property.TextureConstants[i].Bitmap.Index:X4} extracted to '{outPath}'");
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}
