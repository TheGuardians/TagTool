using TagTool.Cache;
using TagTool.Geometry;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Serialization;

namespace TagTool.Commands.RenderModels
{
    class ExtractModelCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private RenderModel Definition { get; }

        public ExtractModelCommand(HaloOnlineCacheContext cacheContext, RenderModel model)
            : base(true,

                  "ExtractModel",
                  "Extracts the current render model definition.",

                  "ExtractModel <variant> <filetype> <filename>",

                  "Extracts a variant of the render model to a file.\n" +
                  "Supported file types: obj")
        {
            CacheContext = cacheContext;
            Definition = model;
        }
        
        public override object Execute(List<string> args)
        {
            if (args.Count != 3)
                return false;

            var variantName = args[0];
            var fileType = args[1].ToLower();
            var modelFileName = args[2];

            switch (fileType)
            {
                case "obj":
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

            var resourceContext = new ResourceSerializationContext(Definition.Geometry.Resource);
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

                switch (fileType)
                {
                    case "obj":
                        ExtractObj(variantName, modelFile, Definition, resourceDefinition, resourceStream);
                        break;
                        
                    default:
                        throw new NotImplementedException(fileType);
                }
            }

            Console.WriteLine("Done!");

            return true;
        }

        private void ExtractObj(string variantName, FileInfo modelFile, RenderModel renderModel, RenderGeometryApiResourceDefinition resourceDefinition, Stream resourceStream)
        {
            using (var objFile = new StreamWriter(modelFile.Create()))
            {
                var objExtractor = new ObjExtractor(objFile);

                // Create a (de)compressor from the first compression block
                var vertexCompressor = new VertexCompressor(renderModel.Geometry.Compression[0]);

                Console.WriteLine("Extracting {0} mesh(es)...", renderModel.Geometry.Meshes.Count);

                foreach (var region in renderModel.Regions)
                {
                    foreach (var permutation in region.Permutations)
                    {
                        if (variantName != CacheContext.GetString(permutation.Name))
                            continue;

                        for (var i = 0; i < permutation.MeshCount; i++)
                        {
                            var mesh = renderModel.Geometry.Meshes[permutation.MeshIndex + i];

                            // Create a MeshReader for the mesh and pass it to the obj extractor
                            var meshReader = new MeshReader(CacheContext.Version, mesh, resourceDefinition);
                            objExtractor.ExtractMesh(meshReader, vertexCompressor, resourceStream);
                        }
                    }
                }

                objExtractor.Finish();
            }
        }
    }
}