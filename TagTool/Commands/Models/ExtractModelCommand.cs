using TagTool.Cache;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Serialization;

namespace TagTool.Commands.Models
{
    class ExtractModelCommand : Command
    {
        private GameCache Cache { get; }
        private Model Definition { get; }

        public ExtractModelCommand(GameCache cache, Model model)
            : base(true,

                  "ExtractModel",
                  "Extracts a render model from the current model definition.",

                  "ExtractModel <variant> <filetype> <filename>",

                  "Extracts a variant of the render model to a file.\n" +
                  "Use the \"ListVariants\" command to list available variants.\n" +
                  "If the model does not have any variants, just use \"default\".\n" +
                  "Supported file types: obj")
        {
            Cache = cache;
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
                case "amf":
                case "dae":
                    break;

                default:
                    throw new NotImplementedException(fileType);
            }

            //
            // Find the variant to extract
            //

            if (Definition.RenderModel == null)
            {
                Console.WriteLine("The model does not have a render model associated with it.");
                return true;
            }

            
            var modelVariant = Definition.Variants.FirstOrDefault(v => (Cache.StringTable.GetString(v.Name) ?? v.Name.ToString()) == variantName);
            if (modelVariant == null && Definition.Variants.Count > 0 && fileType != "dae")
            {
                Console.WriteLine("Unable to find variant \"{0}\"", variantName);
                Console.WriteLine("Use \"listvariants\" to list available variants.");
                return true;
            }

            //
            // Deserialize the render model tag
            //

            RenderModel renderModel;
            using (var cacheStream = Cache.TagCache.OpenTagCacheRead())
            {
                renderModel = Cache.Deserialize<RenderModel>(cacheStream, Definition.RenderModel);
            }

            if (renderModel.Geometry.Resource == null)
            {
                Console.WriteLine("Render model does not have a resource associated with it");
                return true;
            }

            //
            // Deserialize the resource definition
            //

            var definition = Cache.ResourceCache.GetRenderGeometryApiResourceDefinition(renderModel.Geometry.Resource);
            renderModel.Geometry.SetResourceBuffers(definition);

            using (var resourceStream = new MemoryStream())
            {
                var modelFile = new FileInfo(modelFileName);

                if (!modelFile.Directory.Exists)
                    modelFile.Directory.Create();

                switch (fileType)
                {
                    case "obj":
                        ExtractObj(modelFile, renderModel, modelVariant);
                        break;

                    case "amf":
                        ExtractAmf(modelFile, renderModel, modelVariant);
                        break;

                    case "dae":
                        ModelExtractor extractor = new ModelExtractor(Cache, renderModel);
                        extractor.ExtractRenderModel();
                        extractor.ExportCollada(modelFile);
                        break;
                    
                    default:
                        throw new NotImplementedException(fileType);
                }
            }

            Console.WriteLine("Done!");

            return true;
        }

        private void ExtractObj(FileInfo modelFile, RenderModel renderModel, Model.Variant modelVariant)
        {
            using (var objFile = new StreamWriter(modelFile.Create()))
            {
                var objExtractor = new ObjExtractor(objFile);

                // Create a (de)compressor from the first compression block
                var vertexCompressor = new VertexCompressor(renderModel.Geometry.Compression[0]);

                if (modelVariant != null)
                {
                    // Extract each region in the variant
                    foreach (var region in modelVariant.Regions)
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
                        var regionName = Cache.StringTable.GetString(region.Name) ?? region.Name.ToString();
                        var permutationName = Cache.StringTable.GetString(permutation.Name) ?? permutation.Name.ToString();

                        Console.WriteLine("Extracting {0} mesh(es) for {1}:{2}...", meshCount, regionName, permutationName);

                        for (var i = 0; i < meshCount; i++)
                        {
                            // Create a MeshReader for the mesh and pass it to the obj extractor
                            var meshReader = new MeshReader(Cache.Version, renderModel.Geometry.Meshes[meshIndex + i]);
                            objExtractor.ExtractMesh(meshReader, vertexCompressor);
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
                        var meshReader = new MeshReader(Cache.Version, mesh);
                        objExtractor.ExtractMesh(meshReader, vertexCompressor);
                    }
                }

                objExtractor.Finish();
            }
        }

        private void ExtractAmf(FileInfo modelFile, RenderModel renderModel, Model.Variant modelVariant)
        {
            using (var amfStream = modelFile.Create())
            using (var amfWriter = new EndianWriter(amfStream))
            {
                var dupeDic = new Dictionary<int, int>();
                
                var indxAddressList = new List<int>();
                var indxValueList = new List<int>();

                var meshAddressList = new List<int>();
                var meshValueList = new List<int>();
                
                var regions = new List<RenderModel.Region>();
                var permutations = new List<RenderModel.Region.Permutation>();

                foreach (var variantRegion in modelVariant.Regions)
                {
                    if (variantRegion.RenderModelRegionIndex == -1)
                        continue;

                    var region = renderModel.Regions[variantRegion.RenderModelRegionIndex];
                    var variantPermutations = region.Permutations.Where(i => variantRegion.Permutations.Find(j => j.Name == i.Name) != null).ToList();

                    if (variantPermutations.Count == 0)
                        continue;

                    if (!regions.Contains(region))
                        regions.Add(region);

                    foreach (var variantPermutation in variantPermutations)
                        if (!permutations.Contains(variantPermutation))
                            permutations.Add(variantPermutation);
                }

                var headerAddressList = new List<int>();
                var headerValueList = new List<int>();
                
                amfWriter.Write("AMF!".ToCharArray());
                amfWriter.Write(2.0f); //format version
                amfWriter.Write((Cache.StringTable.GetString(renderModel.Name) + "\0").ToCharArray());

                amfWriter.Write(renderModel.Nodes.Count);
                headerAddressList.Add((int)amfWriter.BaseStream.Position);
                amfWriter.Write(0);

                amfWriter.Write(renderModel.MarkerGroups.Count);
                headerAddressList.Add((int)amfWriter.BaseStream.Position);
                amfWriter.Write(0);

                amfWriter.Write(regions.Count);
                headerAddressList.Add((int)amfWriter.BaseStream.Position);
                amfWriter.Write(0);

                amfWriter.Write(renderModel.Materials.Count);
                headerAddressList.Add((int)amfWriter.BaseStream.Position);
                amfWriter.Write(0);
                
                headerValueList.Add((int)amfWriter.BaseStream.Position);

                foreach (var node in renderModel.Nodes)
                {
                    amfWriter.Write((Cache.StringTable.GetString(node.Name) + "\0").ToCharArray());
                    amfWriter.Write(node.ParentNode);
                    amfWriter.Write(node.FirstChildNode);
                    amfWriter.Write(node.NextSiblingNode);
                    amfWriter.Write(node.DefaultTranslation.X * 100);
                    amfWriter.Write(node.DefaultTranslation.Y * 100);
                    amfWriter.Write(node.DefaultTranslation.Z * 100);
                    amfWriter.Write(node.DefaultRotation.I);
                    amfWriter.Write(node.DefaultRotation.J);
                    amfWriter.Write(node.DefaultRotation.K);
                    amfWriter.Write(node.DefaultRotation.W);
                }

                var markerAddressList = new List<int>();
                var markerValueList = new List<int>();
                
                headerValueList.Add((int)amfWriter.BaseStream.Position);

                foreach (var group in renderModel.MarkerGroups)
                {
                    amfWriter.Write((Cache.StringTable.GetString(group.Name) + "\0").ToCharArray());
                    amfWriter.Write(group.Markers.Count);
                    markerAddressList.Add((int)amfWriter.BaseStream.Position);
                    amfWriter.Write(0);
                }
                
                foreach (var group in renderModel.MarkerGroups)
                {
                    markerValueList.Add((int)amfWriter.BaseStream.Position);
                    foreach (var marker in group.Markers)
                    {
                        amfWriter.Write((byte)marker.RegionIndex);
                        amfWriter.Write((byte)marker.PermutationIndex);
                        amfWriter.Write((short)marker.NodeIndex);
                        amfWriter.Write(marker.Translation.X * 100);
                        amfWriter.Write(marker.Translation.Y * 100);
                        amfWriter.Write(marker.Translation.Z * 100);
                        amfWriter.Write(marker.Rotation.I);
                        amfWriter.Write(marker.Rotation.J);
                        amfWriter.Write(marker.Rotation.K);
                        amfWriter.Write(marker.Rotation.W);
                    }
                }

                var permAddressList = new List<int>();
                var permValueList = new List<int>();

                headerValueList.Add((int)amfWriter.BaseStream.Position);

                foreach (var variantRegion in modelVariant.Regions)
                {
                    if (variantRegion.RenderModelRegionIndex == -1)
                        continue;

                    var region = renderModel.Regions[variantRegion.RenderModelRegionIndex];
                    var variantPermutations = region.Permutations.Where(i => variantRegion.Permutations.Find(j => j.Name == i.Name) != null).ToList();

                    if (variantPermutations.Count == 0)
                        continue;

                    amfWriter.Write((Cache.StringTable.GetString(region.Name) + "\0").ToCharArray());
                    amfWriter.Write(variantPermutations.Count);
                    permAddressList.Add((int)amfWriter.BaseStream.Position);
                    amfWriter.Write(0);
                }

                var vertAddressList = new List<int>();
                var vertValueList = new List<int>();
            }
        }
    }
}