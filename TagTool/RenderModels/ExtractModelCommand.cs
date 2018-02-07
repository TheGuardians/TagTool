using BlamCore.Cache;
using BlamCore.Commands;
using BlamCore.Geometry;
using BlamCore.IO;
using BlamCore.Serialization;
using BlamCore.TagDefinitions;
using BlamCore.TagResources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagTool.RenderModels
{
    class ExtractModelCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private RenderModel Definition { get; }

        public ExtractModelCommand(GameCacheContext cacheContext, RenderModel model)
            : base(CommandFlags.Inherit,

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
                        ExtractObj(modelFile, Definition, resourceDefinition, resourceStream);
                        break;
                        
                    default:
                        throw new NotImplementedException(fileType);
                }
            }

            Console.WriteLine("Done!");

            return true;
        }

        private void ExtractObj(FileInfo modelFile, RenderModel renderModel, RenderGeometryApiResourceDefinition resourceDefinition, Stream resourceStream)
        {
            using (var objFile = new StreamWriter(modelFile.Create()))
            {
                var objExtractor = new ObjExtractor(objFile);

                // Create a (de)compressor from the first compression block
                var vertexCompressor = new VertexCompressor(renderModel.Geometry.Compression[0]);

                Console.WriteLine("Extracting {0} mesh(es)...", renderModel.Geometry.Meshes.Count);

                foreach (var mesh in renderModel.Geometry.Meshes)
                {
                    // Create a MeshReader for the mesh and pass it to the obj extractor
                    var meshReader = new MeshReader(CacheContext.Version, mesh, resourceDefinition);
                    objExtractor.ExtractMesh(meshReader, vertexCompressor, resourceStream);
                }

                objExtractor.Finish();
            }
        }
    }
}