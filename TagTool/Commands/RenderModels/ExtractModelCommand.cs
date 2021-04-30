using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Geometry;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Commands.RenderModels
{
    class ExtractModelCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private RenderModel Definition { get; }

        public ExtractModelCommand(GameCache cacheContext, CachedTag tag, RenderModel model)
            : base(true,

                  "ExtractModel",
                  "Extracts the current render model definition. (supported filetypes: obj, dae)",

                  "ExtractModel [variant name] <filename>",

                  "Extracts a variant of the render model to a file. \n" +
                  "If no extension is found, a dae named for the tag will be extracted to the folder that ends the path.")
        {
            Cache = cacheContext;
            Tag = tag;
            Definition = model;
        }
        
        public override object Execute(List<string> args)
        {
            string variantName = "*";
            string fileType = "dae";
            string modelFileName = "";

            switch (args.Count)
            {
                case (1):
                    modelFileName = args[0];
                    if (args[0].Contains("."))
                        fileType = modelFileName.Substring(modelFileName.LastIndexOf('.') + 1).ToLower();
                    break;

                case (2):
                    variantName = args[0];
                    modelFileName = args[1];
                    break;

                default:
                    return new TagToolError(CommandError.ArgCount);
            }

            if (!modelFileName.Contains("."))
            {
                if (Tag.Name != null)
                {
                    var split = Tag.Name.Split('\\');
                    modelFileName += "\\" + split[split.Length - 1];
                }
                else
                    modelFileName += "\\" + Tag.Index.ToString("X8");

                modelFileName += "." + fileType;
            }

            if (Definition.Geometry.Resource == null)
            {
                new TagToolError(CommandError.CustomError, "Render model does not have a resource associated with it!");
                return true;
            }

            // Deserialize the resource definition

            var definition = Cache.ResourceCache.GetRenderGeometryApiResourceDefinition(Definition.Geometry.Resource);
            Definition.Geometry.SetResourceBuffers(definition);

            using (var resourceStream = new MemoryStream())
            {
                var modelFile = new FileInfo(modelFileName);

                if (!modelFile.Directory.Exists)
                    modelFile.Directory.Create();

                ModelExtractor extractor = new ModelExtractor(Cache, Definition);
                extractor.ExtractRenderModel(variantName);

                switch (fileType)
                {
                    case "obj":
                        extractor.ExportObject(modelFile);
                        break;
                    case "dae":
                        extractor.ExportCollada(modelFile);
                        break;
                    default:
                        return new TagToolError(CommandError.ArgInvalid, $"Unsupported file type \"{fileType}\"");
                }

                Console.WriteLine($"Model successfully extracted to \"{modelFileName}\"");
            }

            return true;
        }
    }
}