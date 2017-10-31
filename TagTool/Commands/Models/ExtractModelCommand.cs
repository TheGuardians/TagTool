using System.Collections.Generic;
using BlamCore.Cache;
using BlamCore.TagDefinitions;
using System;
using System.Linq;
using BlamCore.TagResources;
using System.IO;
using BlamCore.Geometry;
using BlamCore.Serialization;

namespace TagTool.Commands.Models
{
    class ExtractModelCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private Model Definition { get; }

        public ExtractModelCommand(GameCacheContext cacheContext, Model model)
            : base(CommandFlags.Inherit,

                  "ExtractModel",
                  "Extracts a render model from the current model definition.",

                  "ExtractModel <variant> <filetype> <filename>",

                  "Extracts a variant of the render model to a file.\n" +
                  "Use the \"ListVariants\" command to list available variants.\n" +
                  "If the model does not have any variants, just use \"default\".\n" +
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
            var fileType = args[1];
            var fileName = args[2];

            if (fileType != "obj")
                return false;

            //
            // Find the variant to extract
            //

            if (Definition.RenderModel == null)
            {
                Console.WriteLine("The model does not have a render model associated with it.");
                return true;
            }

            var variant = Definition.Variants.FirstOrDefault(v => (CacheContext.GetString(v.Name) ?? v.Name.ToString()) == variantName);
            if (variant == null && Definition.Variants.Count > 0)
            {
                Console.WriteLine("Unable to find variant \"{0}\"", variantName);
                Console.WriteLine("Use \"listvariants\" to list available variants.");
                return true;
            }

            //
            // Deserialize the render model tag
            //

            RenderModel renderModel;
            using (var cacheStream = CacheContext.TagCacheFile.OpenRead())
            {
                var renderModelContext = new TagSerializationContext(cacheStream, CacheContext, Definition.RenderModel);
                renderModel = CacheContext.Deserializer.Deserialize<RenderModel>(renderModelContext);
            }

            if (renderModel.Geometry.Resource == null)
            {
                Console.WriteLine("Render model does not have a resource associated with it");
                return true;
            }

            //
            // Deserialize the resource definition
            //

            var resourceContext = new ResourceSerializationContext(renderModel.Geometry.Resource);
            var definition = CacheContext.Deserializer.Deserialize<RenderGeometryApiResourceDefinition>(resourceContext);

            using (var resourceStream = new MemoryStream())
            {
                //
                // Extract the resource data
                //

                CacheContext.ExtractResource(renderModel.Geometry.Resource, resourceStream);

                var file = new FileInfo(fileName);

                if (!file.Directory.Exists)
                    file.Directory.Create();

                using (var objFile = new StreamWriter(file.Create()))
                {
                    var objExtractor = new ObjExtractor(objFile);

                    // Create a (de)compressor from the first compression block
                    var vertexCompressor = new VertexCompressor(renderModel.Geometry.Compression[0]);

                    if (variant != null)
                    {
                        // Extract each region in the variant
                        foreach (var region in variant.Regions)
                        {
                            // Get the corresonding region in the render model tag
                            if (region.RenderModelRegionIndex >= renderModel.Regions.Count)
                                continue;

                            var renderModelRegion = renderModel.Regions[region.RenderModelRegionIndex];

                            // Get the corresponding permutation in the render model tag
                            // (Just extract the first permutation for now)
                            if (region.Permutations.Count == 0)
                                continue;

                            var permutation = region.Permutations[0];

                            if (permutation.RenderModelPermutationIndex < 0 ||
                                permutation.RenderModelPermutationIndex >= renderModelRegion.Permutations.Count)
                                continue;

                            var renderModelPermutation = renderModelRegion.Permutations[permutation.RenderModelPermutationIndex];

                            // Extract each mesh in the permutation
                            var meshIndex = renderModelPermutation.MeshIndex;
                            var meshCount = renderModelPermutation.MeshCount;
                            var regionName = CacheContext.GetString(region.Name) ?? region.Name.ToString();
                            var permutationName = CacheContext.GetString(permutation.Name) ?? permutation.Name.ToString();

                            Console.WriteLine("Extracting {0} mesh(es) for {1}:{2}...", meshCount, regionName, permutationName);

                            for (var i = 0; i < meshCount; i++)
                            {
                                // Create a MeshReader for the mesh and pass it to the obj extractor
                                var meshReader = new MeshReader(CacheContext.Version, renderModel.Geometry.Meshes[meshIndex + i], definition);
                                objExtractor.ExtractMesh(meshReader, vertexCompressor, resourceStream);
                            }
                        }
                    }
                    else
                    {
                        // No variant - just extract every mesh
                        Console.WriteLine("Extracting {0} mesh(es)...", renderModel.Geometry.Meshes.Count);

                        foreach (var mesh in renderModel.Geometry.Meshes)
                        {
                            // Create a MeshReader for the mesh and pass it to the obj extractor
                            var meshReader = new MeshReader(CacheContext.Version, mesh, definition);
                            objExtractor.ExtractMesh(meshReader, vertexCompressor, resourceStream);
                        }
                    }

                    objExtractor.Finish();
                }
            }

            Console.WriteLine("Done!");

            return true;
        }
    }
}