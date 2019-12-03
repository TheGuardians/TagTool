using TagTool.Cache;
using TagTool.Geometry;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Serialization;
using Assimp;

namespace TagTool.Commands.RenderModels
{
    class ExtractModelCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private RenderModel Definition { get; }

        public ExtractModelCommand(HaloOnlineCacheContext cacheContext, RenderModel model)
            : base(true,

                  "ExtractModel",
                  "Extracts the current render model definition. (filetypes: obj, dae)",

                  "ExtractModel [variant name] <filetype> <filename>",

                  "Extracts a variant of the render model to a file. \n" +
                  "Supported file types: obj, dae")
        {
            CacheContext = cacheContext;
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
                return false;

            switch (fileType)
            {
                case "obj":
                case "dae":
                    break;

                default:
                    throw new NotImplementedException(fileType);
            }
            
            if (Definition.Geometry.Resource == null)
            {
                Console.WriteLine("Render model does not have a resource associated with it");
                return true;
            }

            //
            // Deserialize the resource definition
            //

            var resourceContext = new ResourceSerializationContext(CacheContext, Definition.Geometry.Resource);
            var resourceDefinition = CacheContext.Deserializer.Deserialize<RenderGeometryApiResourceDefinition>(resourceContext);

            using (var resourceStream = new MemoryStream())
            {
                //
                // Extract the resource data
                //

                CacheContext.ExtractResource(Definition.Geometry.Resource, resourceStream);

                var modelFile = new FileInfo(modelFileName);

                if (!modelFile.Directory.Exists)
                    modelFile.Directory.Create();

                ModelExtractor extractor = new ModelExtractor(CacheContext, Definition);
                extractor.ExtractRenderModel(variantName);

                switch (fileType)
                {
                    case "obj":
                        return extractor.ExportObject(modelFile);

                    case "dae":
                        return extractor.ExportCollada(modelFile);

                    default:
                        throw new NotImplementedException(fileType);
                }
            }
        }
    }
}