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

                switch (fileType)
                {
                    case "obj":
                        return ExtractObj(variantName, modelFile, Definition, resourceDefinition, resourceStream);
                        
                    default:
                        throw new NotImplementedException(fileType);
                }
            }
        }

        private bool ExtractObj(string variantName, FileInfo modelFile, RenderModel renderModel, RenderGeometryApiResourceDefinition resourceDefinition, Stream resourceStream)
        {
            var meshes = new Dictionary<string, Mesh>();
            var vertexCompressor = new VertexCompressor(renderModel.Geometry.Compression[0]);

            foreach (var region in renderModel.Regions)
            {
                var regionName = CacheContext.GetString(region.Name);

                foreach (var permutation in region.Permutations)
                {
                    var permutationName = CacheContext.GetString(permutation.Name);

                    if (variantName != "*" && variantName != permutationName)
                        continue;

                    for (var i = 0; i < permutation.MeshCount; i++)
                    {
                        var name = $"{regionName}_{permutationName}_{i}";
                        meshes[name] = renderModel.Geometry.Meshes[permutation.MeshIndex + i];
                    }
                }
            }

            if (meshes.Count == 0)
            {
                Console.WriteLine($"ERROR: No meshes found under variant '{variantName}'!");
                return false;
            }

            Console.Write("Extracting {0} mesh(es)...", meshes.Count);

            using (var objFile = new StreamWriter(modelFile.Create()))
            {
                var objExtractor = new ObjExtractor(objFile);

                foreach (var entry in meshes)
                {
                    var meshReader = new MeshReader(CacheContext.Version, entry.Value, resourceDefinition);
                    objExtractor.ExtractMesh(meshReader, vertexCompressor, resourceStream, entry.Key);
                }

                objExtractor.Finish();
            }

            Console.WriteLine("done!");

            return true;
        }
    }
}