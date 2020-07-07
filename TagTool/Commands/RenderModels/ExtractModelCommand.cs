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
        private RenderModel Definition { get; }

        public ExtractModelCommand(GameCache cacheContext, RenderModel model)
            : base(true,

                  "ExtractModel",
                  "Extracts the current render model definition. (filetypes: obj, dae)",

                  "ExtractModel [variant name] <filetype> <filename>",

                  "Extracts a variant of the render model to a file. \n" +
                  "Supported file types: obj, dae")
        {
            Cache = cacheContext;
            Definition = model;
        }
        
        public override object Execute(List<string> args)
        {
            string variantName;
            string fileType;
            string modelFileName;

            if(args.Count == 2)
            {
                variantName = "*";
                fileType = args[0].ToLower();
                modelFileName = args[1];
            }
            else if (args.Count == 3)
            {
                variantName = args[0];
                fileType = args[1].ToLower();
                modelFileName = args[2];
            }
            else
                return new TagToolError(CommandError.ArgCount);

            switch (fileType)
            {
                case "obj":
                case "dae":
                    break;

                default:
                    return new TagToolError(CommandError.ArgInvalid, $"Unsupported file type \"{fileType}\"");
            }
            
            if (Definition.Geometry.Resource == null)
            {
                Console.WriteLine("Render model does not have a resource associated with it");
                return true;
            }

            //
            // Deserialize the resource definition
            //

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
                        return extractor.ExportObject(modelFile);

                    case "dae":
                        return extractor.ExportCollada(modelFile);
                }
            }

            return true;
        }
    }
}