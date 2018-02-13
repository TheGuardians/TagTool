using TagTool.Cache;
using TagTool.Commands;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            var fileType = args[1].ToLower();
            var modelFileName = args[2];

            switch (fileType)
            {
                case "obj":
                case "amf":
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

            var modelVariant = Definition.Variants.FirstOrDefault(v => (CacheContext.GetString(v.Name) ?? v.Name.ToString()) == variantName);
            if (modelVariant == null && Definition.Variants.Count > 0)
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
            var resourceDefinition = CacheContext.Deserializer.Deserialize<RenderGeometryApiResourceDefinition>(resourceContext);

            using (var resourceStream = new MemoryStream())
            {
                //
                // Extract the resource data
                //

                CacheContext.ExtractResource(renderModel.Geometry.Resource, resourceStream);

                var modelFile = new FileInfo(modelFileName);

                if (!modelFile.Directory.Exists)
                    modelFile.Directory.Create();

                switch (fileType)
                {
                    case "obj":
                        ExtractObj(modelFile, renderModel, modelVariant, resourceDefinition, resourceStream);
                        break;

                    case "amf":
                        ExtractAmf(modelFile, renderModel, modelVariant, resourceDefinition, resourceStream);
                        break;

                    default:
                        throw new NotImplementedException(fileType);
                }
            }

            Console.WriteLine("Done!");

            return true;
        }

        private void ExtractObj(FileInfo modelFile, RenderModel renderModel, Model.Variant modelVariant, RenderGeometryApiResourceDefinition resourceDefinition, Stream resourceStream)
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
                        var regionName = CacheContext.GetString(region.Name) ?? region.Name.ToString();
                        var permutationName = CacheContext.GetString(permutation.Name) ?? permutation.Name.ToString();

                        Console.WriteLine("Extracting {0} mesh(es) for {1}:{2}...", meshCount, regionName, permutationName);

                        for (var i = 0; i < meshCount; i++)
                        {
                            // Create a MeshReader for the mesh and pass it to the obj extractor
                            var meshReader = new MeshReader(CacheContext.Version, renderModel.Geometry.Meshes[meshIndex + i], resourceDefinition);
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
                        var meshReader = new MeshReader(CacheContext.Version, mesh, resourceDefinition);
                        objExtractor.ExtractMesh(meshReader, vertexCompressor, resourceStream);
                    }
                }

                objExtractor.Finish();
            }
        }

        private void ExtractAmf(FileInfo modelFile, RenderModel renderModel, Model.Variant modelVariant, RenderGeometryApiResourceDefinition resourceDefinition, Stream resourceStream)
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
                amfWriter.Write((CacheContext.GetString(renderModel.Name) + "\0").ToCharArray());

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
                    amfWriter.Write((CacheContext.GetString(node.Name) + "\0").ToCharArray());
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
                    amfWriter.Write((CacheContext.GetString(group.Name) + "\0").ToCharArray());
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

                    amfWriter.Write((CacheContext.GetString(region.Name) + "\0").ToCharArray());
                    amfWriter.Write(variantPermutations.Count);
                    permAddressList.Add((int)amfWriter.BaseStream.Position);
                    amfWriter.Write(0);
                }

                var vertAddressList = new List<int>();
                var vertValueList = new List<int>();
                /*
                foreach (var variantRegion in modelVariant.Regions)
                {
                    if (variantRegion.RenderModelRegionIndex == -1)
                        continue;

                    var region = renderModel.Regions[variantRegion.RenderModelRegionIndex];
                    var variantPermutations = region.Permutations.Where(i => variantRegion.Permutations.Find(j => j.Name == i.Name) != null).ToList();

                    if (variantPermutations.Count == 0)
                        continue;

                    permValueList.Add((int)amfWriter.BaseStream.Position);

                    foreach (var permutation in variantPermutations)
                    {
                        if (permutation.MeshIndex == -1)
                            continue;

                        var mesh = renderModel.Geometry.Meshes[permutation.MeshIndex];

                        amfWriter.Write((CacheContext.GetString(permutation.Name) + "\0").ToCharArray());
                        amfWriter.Write((byte)(mesh.RigidNodeIndex == -1 ? 1 : 0));
                        amfWriter.Write((byte)mesh.RigidNodeIndex);

                        var vertexCount = 0;
                        var indexCount = 0;
                        foreach (var part in mesh.Parts)
                        {
                            vertexCount += part.VertexCount;
                            indexCount += part.IndexCount;
                        }

                        amfWriter.Write(vertexCount);
                        vertAddressList.Add((int)amfWriter.BaseStream.Position);
                        amfWriter.Write(0);

                        amfWriter.Write(indexCount);
                        indxAddressList.Add((int)amfWriter.BaseStream.Position);
                        amfWriter.Write(0);

                        amfWriter.Write(mesh.SubParts.Count);
                        meshAddressList.Add((int)amfWriter.BaseStream.Position);
                        amfWriter.Write(0);

                        amfWriter.Write(float.NaN);
                    }
                }

                foreach (var variantRegion in modelVariant.Regions)
                {
                    if (variantRegion.RenderModelRegionIndex == -1)
                        continue;

                    var region = renderModel.Regions[variantRegion.RenderModelRegionIndex];
                    var variantPermutations = region.Permutations.Where(i => variantRegion.Permutations.Find(j => j.Name == i.Name) != null).ToList();

                    if (variantPermutations.Count == 0)
                        continue;

                    foreach (var permutation in variantPermutations)
                    {
                        if (permutation.MeshIndex == -1)
                            continue;

                        var mesh = renderModel.Geometry.Meshes[permutation.MeshIndex];
                        
                        if (dupeDic.TryGetValue(mesh.VertexBuffers[0], out int address))
                        {
                            vertValueList.Add(address);
                            continue;
                        }
                        else
                            dupeDic.Add(mesh.VertexBuffers[0], (int)amfWriter.BaseStream.Position);

                        var hasNodes = mesh.RigidNodeIndex == -1;

                        vertValueList.Add((int)amfWriter.BaseStream.Position);

                        foreach (Vertex vert in part.Vertices)
                        {
                            vert.TryGetValue("position", 0, out v);
                            amfWriter.Write(v.Data.x * 100);
                            amfWriter.Write(v.Data.y * 100);
                            amfWriter.Write(v.Data.z * 100);

                            vert.TryGetValue("normal", 0, out v);
                            amfWriter.Write(v.Data.i);
                            amfWriter.Write(v.Data.j);
                            amfWriter.Write(v.Data.k);

                            vert.TryGetValue("texcoords", 0, out v);
                            amfWriter.Write(v.Data.x);
                            amfWriter.Write(v.Data.y);

                            if (hasNodes)
                            {
                                VertexValue i, w;
                                vert.TryGetValue("blendindices", 0, out i);
                                vert.TryGetValue("blendweight", 0, out w);
                                int count = 0;
                                if (w.Data.a > 0)
                                {
                                    amfWriter.Write((byte)i.Data.a);
                                    count++;
                                }
                                if (w.Data.b > 0)
                                {
                                    amfWriter.Write((byte)i.Data.b);
                                    count++;
                                }
                                if (w.Data.c > 0)
                                {
                                    amfWriter.Write((byte)i.Data.c);
                                    count++;
                                }
                                if (w.Data.d > 0)
                                {
                                    amfWriter.Write((byte)i.Data.d);
                                    count++;
                                }

                                if (count == 0)
                                {
                                    amfWriter.Write((byte)0);
                                    amfWriter.Write((byte)255);
                                    amfWriter.Write(0);
                                    continue;
                                    //throw new Exception("no weights on a weighted node. report this.");
                                }

                                if (count != 4) amfWriter.Write((byte)255);

                                if (w.Data.a > 0) amfWriter.Write(w.Data.a);
                                if (w.Data.b > 0) amfWriter.Write(w.Data.b);
                                if (w.Data.c > 0) amfWriter.Write(w.Data.c);
                                if (w.Data.d > 0) amfWriter.Write(w.Data.d);
                            }
                        }
                    }
                }

                foreach (var perm in permutations)
                {
                    var part = renderModel.ModelSections[perm.PieceIndex];

                    int address;
                    if (dupeDic.TryGetValue(part.VertsIndex, out address))
                    {
                        vertValueList.Add(address);
                        continue;
                    }
                    else
                        dupeDic.Add(part.VertsIndex, (int)amfWriter.BaseStream.Position);

                    VertexValue v;
                    bool hasNodes = part.Vertices[0].TryGetValue("blendindices", 0, out v) && part.NodeIndex == 255;
                    
                    vertValueList.Add((int)amfWriter.BaseStream.Position);

                    foreach (Vertex vert in part.Vertices)
                    {
                        vert.TryGetValue("position", 0, out v);
                        amfWriter.Write(v.Data.x * 100);
                        amfWriter.Write(v.Data.y * 100);
                        amfWriter.Write(v.Data.z * 100);

                        vert.TryGetValue("normal", 0, out v);
                        amfWriter.Write(v.Data.i);
                        amfWriter.Write(v.Data.j);
                        amfWriter.Write(v.Data.k);

                        vert.TryGetValue("texcoords", 0, out v);
                        amfWriter.Write(v.Data.x);
                        amfWriter.Write(v.Data.y);
                        
                        if (hasNodes)
                        {
                            VertexValue i, w;
                            vert.TryGetValue("blendindices", 0, out i);
                            vert.TryGetValue("blendweight", 0, out w);
                            int count = 0;
                            if (w.Data.a > 0)
                            {
                                amfWriter.Write((byte)i.Data.a);
                                count++;
                            }
                            if (w.Data.b > 0)
                            {
                                amfWriter.Write((byte)i.Data.b);
                                count++;
                            }
                            if (w.Data.c > 0)
                            {
                                amfWriter.Write((byte)i.Data.c);
                                count++;
                            }
                            if (w.Data.d > 0)
                            {
                                amfWriter.Write((byte)i.Data.d);
                                count++;
                            }

                            if (count == 0)
                            {
                                amfWriter.Write((byte)0);
                                amfWriter.Write((byte)255);
                                amfWriter.Write(0);
                                continue;
                                //throw new Exception("no weights on a weighted node. report this.");
                            }

                            if (count != 4) amfWriter.Write((byte)255);

                            if (w.Data.a > 0) amfWriter.Write(w.Data.a);
                            if (w.Data.b > 0) amfWriter.Write(w.Data.b);
                            if (w.Data.c > 0) amfWriter.Write(w.Data.c);
                            if (w.Data.d > 0) amfWriter.Write(w.Data.d);
                        }
                    }
                }
                #endregion

                dupeDic.Clear();

                #region Indices
                foreach (var perm in permutations)
                {
                    var part = renderModel.ModelSections[perm.PieceIndex];

                    int address;
                    if (dupeDic.TryGetValue(part.FacesIndex, out address))
                    {
                        indxValueList.Add(address);
                        continue;
                    }
                    else
                        dupeDic.Add(part.FacesIndex, (int)amfWriter.BaseStream.Position);

                    indxValueList.Add((int)amfWriter.BaseStream.Position);

                    foreach (var submesh in part.Submeshes)
                    {
                        var indices = GetTriangleList(part.Indices, submesh.FaceIndex, submesh.FaceCount, renderModel.IndexInfoList[part.FacesIndex].FaceFormat);
                        foreach (var index in indices)
                        {
                            if (part.Vertices.Length > 0xFFFF) amfWriter.Write(index);
                            else amfWriter.Write((ushort)index);
                        }
                    }

                }
                #endregion
                #region Submeshes
                foreach (var perm in permutations)
                {
                    var part = renderModel.ModelSections[perm.PieceIndex];
                    meshValueList.Add((int)amfWriter.BaseStream.Position);
                    int tCount = 0;
                    foreach (var mesh in part.Submeshes)
                    {

                        int sCount = GetTriangleList(part.Indices, mesh.FaceIndex, mesh.FaceCount, renderModel.IndexInfoList[part.FacesIndex].FaceFormat).Count / 3;

                        amfWriter.Write((short)mesh.ShaderIndex);
                        amfWriter.Write(tCount);
                        amfWriter.Write(sCount);

                        tCount += sCount;
                    }
                }
                #endregion
                #region Shaders
                headerValueList.Add((int)amfWriter.BaseStream.Position);
                foreach (var shaderBlock in renderModel.Shaders)
                {
                    //skip null shaders
                    if (shaderBlock.tagID == -1)
                    {
                        amfWriter.Write("null\0".ToCharArray());
                        for (int i = 0; i < 8; i++)
                            amfWriter.Write("null\0".ToCharArray());

                        for (int i = 0; i < 4; i++)
                            amfWriter.Write(0);

                        amfWriter.Write(Convert.ToByte(false));
                        amfWriter.Write(Convert.ToByte(false));

                        continue;
                    }

                    var rmshTag = Cache.IndexItems.GetItemByID(shaderBlock.tagID);
                    var rmsh = DefinitionsManager.rmsh(Cache, rmshTag);
                    string shaderName = rmshTag.Filename.Substring(rmshTag.Filename.LastIndexOf("\\") + 1) + "\0";
                    string[] paths = new string[8] { "null\0", "null\0", "null\0", "null\0", "null\0", "null\0", "null\0", "null\0" };
                    float[] uTiles = new float[8] { 1, 1, 1, 1, 1, 1, 1, 1 };
                    float[] vTiles = new float[8] { 1, 1, 1, 1, 1, 1, 1, 1 };
                    int[] tints = new int[4] { -1, -1, -1, -1 };
                    bool isTransparent = false;
                    bool ccOnly = false;

                    //Halo4 fucked this up
                    if (Cache.Version >= DefinitionSet.Halo3Beta && Cache.Version <= DefinitionSet.HaloReachRetail)
                    {
                        var rmt2Tag = Cache.IndexItems.GetItemByID(rmsh.Properties[0].TemplateTagID);
                        var rmt2 = DefinitionsManager.rmt2(Cache, rmt2Tag);

                        for (int i = 0; i < rmt2.UsageBlocks.Count; i++)
                        {
                            var s = rmt2.UsageBlocks[i].Usage;
                            var bitmTag = Cache.IndexItems.GetItemByID(rmsh.Properties[0].ShaderMaps[i].BitmapTagID);

                            switch (s)
                            {
                                case "base_map":
                                    paths[0] = (bitmTag != null) ? bitmTag.Filename + "\0" : "null\0";
                                    break;
                                case "detail_map":
                                case "detail_map_overlay":
                                    paths[1] = (bitmTag != null) ? bitmTag.Filename + "\0" : "null\0";
                                    break;
                                case "change_color_map":
                                    paths[2] = (bitmTag != null) ? bitmTag.Filename + "\0" : "null\0";
                                    break;
                                case "bump_map":
                                    paths[3] = (bitmTag != null) ? bitmTag.Filename + "\0" : "null\0";
                                    break;
                                case "bump_detail_map":
                                    paths[4] = (bitmTag != null) ? bitmTag.Filename + "\0" : "null\0";
                                    break;
                                case "self_illum_map":
                                    paths[5] = (bitmTag != null) ? bitmTag.Filename + "\0" : "null\0";
                                    break;
                                case "specular_map":
                                    paths[6] = (bitmTag != null) ? bitmTag.Filename + "\0" : "null\0";
                                    break;
                            }
                        }

                        for (int i = 0; i < rmt2.ArgumentBlocks.Count; i++)
                        {
                            var s = rmt2.ArgumentBlocks[i].Argument;

                            switch (s)
                            {
                                //case "env_tint_color":
                                //case "fresnel_color":
                                case "albedo_color":
                                    tints[0] = i;
                                    break;

                                case "self_illum_color":
                                    tints[1] = i;
                                    break;

                                case "specular_tint":
                                    tints[2] = i;
                                    break;
                            }
                        }

                        short[] tiles = new short[8] { -1, -1, -1, -1, -1, -1, -1, -1 };

                        foreach (var map in rmsh.Properties[0].ShaderMaps)
                        {
                            var bitmTag = Cache.IndexItems.GetItemByID(map.BitmapTagID);

                            for (int i = 0; i < 8; i++)
                            {
                                if (bitmTag.Filename + "\0" != paths[i]) continue;

                                tiles[i] = (short)map.TilingIndex;
                            }
                        }

                        for (int i = 0; i < 8; i++)
                        {
                            try
                            {
                                uTiles[i] = rmsh.Properties[0].Tilings[tiles[i]].UTiling;
                                vTiles[i] = rmsh.Properties[0].Tilings[tiles[i]].VTiling;
                            }
                            catch { }
                        }
                    }
                    else
                        try
                        {
                            paths[0] = Cache.IndexItems.GetItemByID(rmsh.Properties[0].ShaderMaps[0].BitmapTagID).Filename + "\0";
                            uTiles[0] = rmsh.Properties[0].Tilings[rmsh.Properties[0].ShaderMaps[0].TilingIndex].UTiling;
                            vTiles[0] = rmsh.Properties[0].Tilings[rmsh.Properties[0].ShaderMaps[0].TilingIndex].VTiling;
                        }
                        catch { }

                    if (rmshTag.ClassCode != "rmsh" && rmshTag.ClassCode != "mat")
                    {
                        isTransparent = true;
                        if (paths[0] == "null\0" && paths[2] != "null\0")
                            ccOnly = true;
                    }

                    amfWriter.Write(shaderName.ToCharArray());
                    for (int i = 0; i < 8; i++)
                    {
                        amfWriter.Write(paths[i].ToCharArray());
                        if (paths[i] != "null\0")
                        {
                            amfWriter.Write(uTiles[i]);
                            amfWriter.Write(vTiles[i]);
                        }
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        if (tints[i] == -1)
                        {
                            amfWriter.Write(0);
                            continue;
                        }

                        amfWriter.Write((byte)(255f * rmsh.Properties[0].Tilings[tints[i]].UTiling));
                        amfWriter.Write((byte)(255f * rmsh.Properties[0].Tilings[tints[i]].VTiling));
                        amfWriter.Write((byte)(255f * rmsh.Properties[0].Tilings[tints[i]].Unknown0));
                        amfWriter.Write((byte)(255f * rmsh.Properties[0].Tilings[tints[i]].Unknown1));
                    }

                    amfWriter.Write(Convert.ToByte(isTransparent));
                    amfWriter.Write(Convert.ToByte(ccOnly));
                }
                #endregion
                #region Write Addresses
                for (int i = 0; i < headerAddressList.Count; i++)
                {
                    amfWriter.BaseStream.Position = headerAddressList[i];
                    amfWriter.Write(headerValueList[i]);
                }

                for (int i = 0; i < markerAddressList.Count; i++)
                {
                    amfWriter.BaseStream.Position = markerAddressList[i];
                    amfWriter.Write(markerValueList[i]);
                }

                for (int i = 0; i < permAddressList.Count; i++)
                {
                    amfWriter.BaseStream.Position = permAddressList[i];
                    amfWriter.Write(permValueList[i]);
                }

                for (int i = 0; i < vertAddressList.Count; i++)
                {
                    amfWriter.BaseStream.Position = vertAddressList[i];
                    amfWriter.Write(vertValueList[i]);
                }

                for (int i = 0; i < indxAddressList.Count; i++)
                {
                    amfWriter.BaseStream.Position = indxAddressList[i];
                    amfWriter.Write(indxValueList[i]);
                }

                for (int i = 0; i < meshAddressList.Count; i++)
                {
                    amfWriter.BaseStream.Position = meshAddressList[i];
                    amfWriter.Write(meshValueList[i]);
                }
                #endregion*/
            }
        }
    }
}