using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System;

using TagTool.Cache;
using TagTool.Commands.Bitmaps;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Models
{
    class ExtractModelCommand : Command
    {
        private GameCache Cache { get; }
        private Model Definition { get; }

        public ExtractModelCommand(GameCache cache, Model model)
            : base(true,

                  name:
                  "ExtractModel",

                  description:
                  "Extracts a render model from the current model definition.",

                  usage:
                  "ExtractModel <filename> [DDS]",
				  
				  examples:
				  @"extractmodel d:\mc.amf dds",

                  helpMessage:
                  "- Format will be chosen based on extension of output file.\n" +
                  "- Optionally extracts bitmaps as DDS when extracting an AMF.\n" +
                  "- DDS are extracted to a sub folder matching the model name.\n" +
                  "- Supported file types: obj, amf, dae")
        {
            Cache = cache;
            Definition = model;
        }
        
        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 2)
                return new TagToolError(CommandError.ArgCount);

            var filePath = args[0];
            var fileType = Path.GetExtension(filePath).ToLower();
            var extractBitmapsPath = "";

            if (!new string[] { ".obj", ".amf", ".dae" }.Contains(fileType))
                return new TagToolError(CommandError.ArgInvalid, $"Unsupported file type \"{fileType}\"");

            if (Definition.RenderModel is null)
                return new TagToolError(CommandError.CustomError, "The model does not have a render model associated with it.");


            // More than one variant, so ask them to pick one.
            var variantNames = Definition.Variants.Select((x, i) => new {
                Index = Convert.ToChar(i + 65).ToString(), Item = x})
                .ToDictionary(x => x.Index, x => x.Item);

            Model.Variant modelVariant = null;
            if (variantNames.Count > 1)
            {
                Console.WriteLine($"\n   Please choose a variant:");
                variantNames.Select(i => $"      [{i.Key}] {Cache.StringTable.GetString(i.Value.Name) ?? i.Value.Name.ToString()}").ToList().ForEach(Console.WriteLine);
                Console.WriteLine($"\n      Enter to rip all variants.");

                var outBackup = Console.Out;
                Console.SetOut(StreamWriter.Null);

                AwaitInput:
                var input = Console.ReadKey();
                if (variantNames.ContainsKey(input.Key.ToString().ToUpper()))
                    modelVariant = variantNames[input.Key.ToString().ToUpper()];
                else if (input.Key != ConsoleKey.Enter) goto AwaitInput;
                
                Console.SetOut(outBackup);
            }


            // Deserialize the render model tag
            RenderModel renderModel;
            using (var cacheStream = Cache.OpenCacheRead())
                renderModel = Cache.Deserialize<RenderModel>(cacheStream, Definition.RenderModel);

            if (renderModel.Geometry.Resource is null)
                return new TagToolError(CommandError.CustomError, "Render model does not have a resource associated with it");


            // Deserialize the resource definition
            var definition = Cache.ResourceCache.GetRenderGeometryApiResourceDefinition(renderModel.Geometry.Resource);
            renderModel.Geometry.SetResourceBuffers(definition);

            using (var resourceStream = new MemoryStream())
            {
                var modelFile = new FileInfo(filePath);
                
                if (args.Count > 1 && args[1].ToLower() == "dds")
                    extractBitmapsPath = modelFile.FullName.Substring(0, modelFile.FullName.Length - modelFile.Extension.Length);
                
                if (!modelFile.Directory.Exists)
                    modelFile.Directory.Create();

                switch (fileType)
                {
                    case ".obj":
                        ExtractObj(modelFile, renderModel, modelVariant);
                        break;

                    case ".amf":
                        ExtractAmf(modelFile, renderModel, modelVariant, scale: 100, extractBitmapsPath);
                        break;

                    case ".dae":
                        ModelExtractor extractor = new ModelExtractor(Cache, renderModel);
                        extractor.ExtractRenderModel();
                        extractor.ExportCollada(modelFile);
                        break;
                }
            }

            Console.WriteLine($"\n   Model extracted to {filePath}");
            if (extractBitmapsPath != "")
                Console.WriteLine($"   Bitmaps extracted to {extractBitmapsPath}\\");

            return true;
        }

        private void ExtractAmf(FileInfo modelFile, RenderModel renderModel, Model.Variant modelVariant, float scale = 1, string extractBitmapsPath = "")
        {
            byte[] NullTerminate(string x) { return Encoding.ASCII.GetBytes(x + char.MinValue); }
            byte[] GetStringNT(StringId x) { return NullTerminate(Cache.StringTable.GetString(x)); }
            byte ConvertVertTypeEnum(VertexType x) { return (byte)(x == VertexType.Rigid ? 2 : x == VertexType.Skinned ? 1 : 0); }

            using (var amfStream = modelFile.Create())
            using (var amfWriter = new EndianWriter(amfStream, EndianFormat.LittleEndian))
            {
                var dupeDic = new Dictionary<int, long>();

                var variantRPs = (modelVariant == null) ? new List<string>() : modelVariant.Regions
				.SelectMany(x => x.Permutations, (x, i) => new { x.RenderModelRegionIndex, x.Permutations })
				.SelectMany(x => x.Permutations, (x, y) => x.RenderModelRegionIndex+":"+y.RenderModelPermutationIndex).ToList();
                
				var validRegions = renderModel.Regions
                                .Select((r,ri) => new { Name = r.Name, RegionsIdx = ri, Permutations = r.Permutations.Where((p,pi) => (modelVariant == null || variantRPs.Contains(ri+":"+pi)) && p.MeshCount > 0 && renderModel.Geometry.Meshes.ElementAtOrDefault(p.MeshIndex).Parts.Count > 0).ToList() })
                                .Where(r => r.Permutations.Count > 0)
                                .ToList();


                #region Address Lists
                var headerAddressList = new List<long>();
                var headerValueList = new List<long>();

                var markerAddressList = new List<long>();
                var markerValueList = new List<long>();

                var permAddressList = new List<long>();
                var permValueList = new List<long>();

                var vertAddressList = new List<long>();
                var vertValueList = new List<long>();

                var indxAddressList = new List<long>();
                var indxValueList = new List<long>();

                var meshAddressList = new List<long>();
                var meshValueList = new List<long>();
                #endregion


                #region Header
                amfWriter.Write("AMF!".ToCharArray());
                amfWriter.Write(2.0f);

                amfWriter.Write(GetStringNT(renderModel.Name));

                amfWriter.Write(renderModel.Nodes.Count);
                headerAddressList.Add(amfWriter.BaseStream.Position);
                amfWriter.Write(0);

                amfWriter.Write(renderModel.MarkerGroups.Count);
                headerAddressList.Add(amfWriter.BaseStream.Position);
                amfWriter.Write(0);

                amfWriter.Write(validRegions.Count);
                headerAddressList.Add(amfWriter.BaseStream.Position);
                amfWriter.Write(0);

                amfWriter.Write(renderModel.Materials.Count);
                headerAddressList.Add(amfWriter.BaseStream.Position);
                amfWriter.Write(0);
                #endregion


                #region Nodes
                headerValueList.Add(amfWriter.BaseStream.Position);
                foreach (var node in renderModel.Nodes)
                {
                    amfWriter.Write(GetStringNT(node.Name));
                    amfWriter.Write(node.ParentNode);
                    amfWriter.Write(node.FirstChildNode);
                    amfWriter.Write(node.NextSiblingNode);
                    amfWriter.Write(node.DefaultTranslation.X * scale);
                    amfWriter.Write(node.DefaultTranslation.Y * scale);
                    amfWriter.Write(node.DefaultTranslation.Z * scale);
                    amfWriter.Write(node.DefaultRotation.I);
                    amfWriter.Write(node.DefaultRotation.J);
                    amfWriter.Write(node.DefaultRotation.K);
                    amfWriter.Write(node.DefaultRotation.W);
                }
                #endregion


                #region Marker Groups
                headerValueList.Add(amfWriter.BaseStream.Position);
                foreach (var group in renderModel.MarkerGroups)
                {
                    amfWriter.Write(GetStringNT(group.Name));
                    amfWriter.Write(group.Markers.Count);
                    markerAddressList.Add(amfWriter.BaseStream.Position);
                    amfWriter.Write(0);
                }
                #endregion


                #region Markers
                foreach (var group in renderModel.MarkerGroups)
                {
                    markerValueList.Add(amfWriter.BaseStream.Position);
                    foreach (var marker in group.Markers)
                    {
                        amfWriter.Write(marker.RegionIndex);
                        amfWriter.Write(marker.PermutationIndex);
                        amfWriter.Write((short)marker.NodeIndex);
                        amfWriter.Write(marker.Translation.X * scale);
                        amfWriter.Write(marker.Translation.Y * scale);
                        amfWriter.Write(marker.Translation.Z * scale);
                        amfWriter.Write(marker.Rotation.I);
                        amfWriter.Write(marker.Rotation.J);
                        amfWriter.Write(marker.Rotation.K);
                        amfWriter.Write(marker.Rotation.W);
                    }
                }
                #endregion


                #region Regions
                headerValueList.Add(amfWriter.BaseStream.Position);
                foreach (var region in validRegions)
                {
                    amfWriter.Write(GetStringNT(region.Name));
                    amfWriter.Write(region.Permutations.Count);
                    permAddressList.Add(amfWriter.BaseStream.Position);
                    amfWriter.Write(0);
                }
                #endregion


                #region Permutations
                foreach (var region in validRegions)
                {
                    permValueList.Add(amfWriter.BaseStream.Position);
                    foreach (var perm in region.Permutations)
                    {
                        var reader = new MeshReader(Cache.Version, Cache.Platform, renderModel.Geometry.Meshes[perm.MeshIndex]);
                        
                        amfWriter.Write(GetStringNT(perm.Name));
                        amfWriter.Write(ConvertVertTypeEnum(reader.Mesh.Type));
                        amfWriter.Write(unchecked((byte?)reader.Mesh.RigidNodeIndex) ?? byte.MaxValue);
                        amfWriter.Write(reader.Mesh.ResourceVertexBuffers[0].Count);
                        vertAddressList.Add(amfWriter.BaseStream.Position);
                        amfWriter.Write(0);

                        int count = 0;
                        foreach (var part in reader.Mesh.Parts)
                        {
                            var indices = ReadIndices(reader, part);
                            count += indices.Count() / 3;
                        }

                        amfWriter.Write(count);
                        indxAddressList.Add(amfWriter.BaseStream.Position);
                        amfWriter.Write(0);

                        amfWriter.Write(reader.Mesh.Parts.Count);
                        meshAddressList.Add(amfWriter.BaseStream.Position);
                        amfWriter.Write(0);

                        amfWriter.Write(float.NaN);
                    }
                }
                #endregion        


                #region Vertices
                foreach (var region in validRegions)
                {
                    foreach (var perm in region.Permutations)
                    {
                        var part = renderModel.Geometry.Meshes[perm.MeshIndex];                    
                        var meshReader = new MeshReader(Cache.Version, Cache.Platform, renderModel.Geometry.Meshes[perm.MeshIndex]);
                        var vertices = ReadVertices(meshReader);
                        DecompressVertices(vertices, new VertexCompressor(renderModel.Geometry.Compression[0]));

                        long address;
                        if (dupeDic.TryGetValue(perm.MeshIndex, out address))
                        {
                            vertValueList.Add(address);
                            continue;
                        }
                        else dupeDic.Add(perm.MeshIndex, amfWriter.BaseStream.Position);

                        vertValueList.Add(amfWriter.BaseStream.Position);

                        foreach (var vert in vertices)
                        {
                            amfWriter.Write(vert.Position.I * scale);
                            amfWriter.Write(vert.Position.J * scale);
                            amfWriter.Write(vert.Position.K * scale);
                            amfWriter.Write(vert.Normal.I);
                            amfWriter.Write(vert.Normal.J);
                            amfWriter.Write(vert.Normal.K);
                            amfWriter.Write(vert.TexCoords.I);
                            amfWriter.Write(vert.TexCoords.J - 1);

                            if (part.Type == VertexType.Rigid)
                            {
                                var indices = vert.Indices is null ? new List<int>(){0}
                                    : vert.Indices.Distinct().Where(x => x != 0).Select(v => (int)v).ToList();

                                foreach (int index in indices) amfWriter.Write((byte)index);

                                if (indices.Count < 4) amfWriter.Write(byte.MaxValue);
                            }
                            else if (part.Type == VertexType.Skinned)
                            {
                                var indices = vert.Indices.AsEnumerable().ToArray();
                                var weights = vert.Weights.AsEnumerable().ToArray();

                                var count = weights.Count(w => w > 0);
                                if (count == 0)
                                {
                                    amfWriter.Write((byte)0);
                                    amfWriter.Write((byte)255);
                                    amfWriter.Write(0);
                                    continue;
                                }

                                for (int i = 0; i < 4; i++)
                                {
                                    if (weights[i] > 0)
                                        amfWriter.Write((byte)indices[i]);
                                }

                                if (count != 4) amfWriter.Write(byte.MaxValue);

                                foreach (var w in weights.Where(w => w > 0))
                                    amfWriter.Write(w);
                            }
                        }
                    }
                }
                #endregion


                #region Indices
                dupeDic.Clear();
                foreach (var region in validRegions)
                {
                    foreach (var perm in region.Permutations)
                    {                      
                        var reader = new MeshReader(Cache.Version, Cache.Platform, renderModel.Geometry.Meshes[perm.MeshIndex]);
                        
                        long address;
                        if (dupeDic.TryGetValue(perm.MeshIndex, out address))
                        {
                            indxValueList.Add(address);
                            continue;
                        }
                        else dupeDic.Add(perm.MeshIndex, amfWriter.BaseStream.Position);

                        indxValueList.Add(amfWriter.BaseStream.Position);

                        foreach (var part in reader.Mesh.Parts)
                        {
                            var indices = ReadIndices(reader, part);

                            foreach (var index in indices)
                            {
                                if (reader.Mesh.ResourceVertexBuffers.Count() > ushort.MaxValue) amfWriter.Write(index);
                                else amfWriter.Write(index);
                            }
                        }
                    }
                }
                #endregion


                #region Submeshes
                foreach (var region in validRegions)
                {
                    foreach (var perm in region.Permutations)
                    {
                        meshValueList.Add(amfWriter.BaseStream.Position);

                        var reader = new MeshReader(Cache.Version, Cache.Platform, renderModel.Geometry.Meshes[perm.MeshIndex]);

                        int currentPosition = 0;
                        foreach (var part in reader.Mesh.Parts)
                        {
                            var indices = ReadIndices(reader, part);

                            var faceCount = indices.Count() / 3;

                            amfWriter.Write(part.MaterialIndex);
                            amfWriter.Write(currentPosition);
                            amfWriter.Write(faceCount);

                            currentPosition += faceCount;
                        }
                    }
                }
                #endregion


                #region Shaders
                headerValueList.Add(amfWriter.BaseStream.Position);
                foreach (var material in renderModel.Materials)
                {
                    const string nullPath = "null";
             
                    //skip null shaders
                    if (material == null)
                    {
                        amfWriter.Write(NullTerminate(nullPath));
                        for (int i = 0; i < 8; i++)
                            amfWriter.Write(NullTerminate(nullPath));

                        for (int i = 0; i < 4; i++)
                            amfWriter.Write(0);

                        amfWriter.Write(Convert.ToByte(false));
                        amfWriter.Write(Convert.ToByte(false));

                        continue;
                    }

                    var shaderName = material.RenderMethod.ToString().Split('\\', '.');
                    amfWriter.Write(NullTerminate(shaderName[shaderName.Length - 2]));

                    RenderMethod renderMethod;
                    RenderMethodTemplate renderMethodTemplate;
                    using (var cacheStream = Cache.OpenCacheRead())
                    {
                        renderMethod = Cache.Deserialize<RenderMethod>(cacheStream, material.RenderMethod);
                        renderMethodTemplate = Cache.Deserialize<RenderMethodTemplate>(cacheStream, renderMethod.ShaderProperties[0].Template);
                    }
                    
                    for (int i = 0; i < 8; i++)
                    {
                        var submatName = nullPath;
                        if (i < renderMethod.ShaderProperties[0].TextureConstants.Count())
                        {
                            submatName = renderMethod.ShaderProperties[0].TextureConstants[i].Bitmap.Name;
                            if (extractBitmapsPath != "")
                            {
                                Bitmap bitmap;
                                using (var cacheStream = Cache.OpenCacheRead())
                                    bitmap = Cache.Deserialize<Bitmap>(cacheStream, renderMethod.ShaderProperties[0].TextureConstants[i].Bitmap);
                                
                                if (bitmap != null)
                                {
                                    var outputDir = Path.Combine(extractBitmapsPath, Path.GetDirectoryName(submatName));
                                    Directory.CreateDirectory(outputDir);
                                    var outBackup = Console.Out;
                                    Console.SetOut(StreamWriter.Null);
                                    new ExtractBitmapCommand(Cache, renderMethod.ShaderProperties[0].TextureConstants[i].Bitmap, bitmap).Execute(new List<string>() { outputDir });
                                    Console.SetOut(outBackup);
                                }
                            }
                        }
     
                        amfWriter.Write(NullTerminate(submatName));
                        if (submatName != nullPath)
                        {
                            amfWriter.Write(1f);
                            amfWriter.Write(1f);
                        }
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        amfWriter.Write((byte)127);
                        amfWriter.Write((byte)127);
                        amfWriter.Write((byte)127);
                        amfWriter.Write((byte)255);
                    }
                    amfWriter.Write(Convert.ToByte(false));
                    amfWriter.Write(Convert.ToByte(false));
                }
                #endregion


                #region Write Addresses
                for (int i = 0; i < headerAddressList.Count; i++)
                {
                    amfWriter.BaseStream.Position = headerAddressList[i];
                    amfWriter.Write((int)headerValueList[i]);
                }

                for (int i = 0; i < markerAddressList.Count; i++)
                {
                    amfWriter.BaseStream.Position = markerAddressList[i];
                    amfWriter.Write((int)markerValueList[i]);
                }

                for (int i = 0; i < permAddressList.Count; i++)
                {
                    amfWriter.BaseStream.Position = permAddressList[i];
                    amfWriter.Write((int)permValueList[i]);
                }

                for (int i = 0; i < vertAddressList.Count; i++)
                {
                    amfWriter.BaseStream.Position = vertAddressList[i];
                    amfWriter.Write((int)vertValueList[i]);
                }

                for (int i = 0; i < indxAddressList.Count; i++)
                {
                    amfWriter.BaseStream.Position = indxAddressList[i];
                    amfWriter.Write((int)indxValueList[i]);
                }

                for (int i = 0; i < meshAddressList.Count; i++)
                {
                    amfWriter.BaseStream.Position = meshAddressList[i];
                    amfWriter.Write((int)meshValueList[i]);
                }
                #endregion
            }
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
                            var meshReader = new MeshReader(Cache.Version, Cache.Platform, renderModel.Geometry.Meshes[meshIndex + i]);
                            objExtractor.ExtractMesh(meshReader, vertexCompressor, String.Format("{0}:{1}", regionName, permutationName));
                        }
                    }
                }
                else
                {
                    // No variant - just extract every mesh
                    Console.WriteLine("Extracting {0} mesh(es)...", renderModel.Geometry.Meshes.Count);

                    var i = 0;
                    foreach (var mesh in renderModel.Geometry.Meshes)
                    {
                        // Create a MeshReader for the mesh and pass it to the obj extractor
                        var meshReader = new MeshReader(Cache.Version, Cache.Platform, mesh);
                        objExtractor.ExtractMesh(meshReader, vertexCompressor, String.Format("mesh_{0}", i));
                        i++;
                    }
                }

                objExtractor.Finish();
            }
        }


        /// <summary>
        /// Reads rigid vertices.
        /// </summary>
        /// <param name="reader">The vertex reader to read from.</param>
        /// <param name="count">The number of vertices to read.</param>
        /// <returns>The vertices that were read.</returns>
        private static List<GenericVertex> ReadRigidVertices(IVertexStream reader, int count)
        {
            var result = new List<GenericVertex>();
            for (var i = 0; i < count; i++)
            {
                var rigid = reader.ReadRigidVertex();
                result.Add(new GenericVertex
                {
                    Position = ToVector3D(rigid.Position),
                    Normal = ToVector3D(rigid.Normal),
                    TexCoords = ToVector3D(rigid.Texcoord),
                    Tangents = ToVector3D(rigid.Tangent),
                    Binormals = ToVector3D(rigid.Binormal)
                });
            }
            return result;
        }
        /// <summary>
        /// Reads skinned vertices.
        /// </summary>
        /// <param name="reader">The vertex reader to read from.</param>
        /// <param name="count">The number of vertices to read.</param>
        /// <returns>The vertices that were read.</returns>
        private static List<GenericVertex> ReadSkinnedVertices(IVertexStream reader, int count)
        {
            var result = new List<GenericVertex>();
            for (var i = 0; i < count; i++)
            {
                var skinned = reader.ReadSkinnedVertex();
                result.Add(new GenericVertex
                {
                    Position = ToVector3D(skinned.Position),
                    Normal = ToVector3D(skinned.Normal),
                    TexCoords = ToVector3D(skinned.Texcoord),
                    Weights = skinned.BlendWeights,
                    Indices = skinned.BlendIndices
                });
            }
            return result;
        }
        /// <summary>
        /// Reads dualquat vertices.
        /// </summary>
        /// <param name="reader">The vertex reader to read from.</param>
        /// <param name="count">The number of vertices to read.</param>
        /// <returns>The vertices that were read.</returns>
        private static List<GenericVertex> ReadDualQuatVertices(IVertexStream reader, int count)
        {
            var result = new List<GenericVertex>();
            for (var i = 0; i < count; i++)
            {
                var dualQuat = reader.ReadDualQuatVertex();
                result.Add(new GenericVertex
                {
                    Position = ToVector3D(dualQuat.Position),
                    Normal = ToVector3D(dualQuat.Normal),
                    TexCoords = ToVector3D(dualQuat.Texcoord),
                    Tangents = ToVector3D(dualQuat.Tangent),
                    Binormals = ToVector3D(dualQuat.Binormal)
                });
            }
            return result;
        }
        /// <summary>
        /// Reads world vertices.
        /// </summary>
        /// <param name="reader">The vertex reader to read from.</param>
        /// <param name="count">The number of vertices to read.</param>
        /// <returns>The vertices that were read.</returns>
        private static List<GenericVertex> ReadWorldVertices(IVertexStream reader, int count)
        {
            var result = new List<GenericVertex>();
            for (var i = 0; i < count; i++)
            {
                var world = reader.ReadWorldVertex();
                result.Add(new GenericVertex
                {
                    Position = ToVector3D(world.Position),
                    Normal = ToVector3D(world.Normal),
                    TexCoords = ToVector3D(world.Texcoord),
                    Tangents = ToVector3D(world.Tangent),
                    Binormals = ToVector3D(world.Binormal)
                });
            }
            return result;
        }
        /// <summary>
        /// Decompresses vertex data in-place.
        /// </summary>
        /// <param name="vertices">The vertices to decompress in-place.</param>
        /// <param name="compressor">The compressor to use.</param>
        private static void DecompressVertices(IEnumerable<GenericVertex> vertices, VertexCompressor compressor)
        {
            if (compressor == null)
                return;

            foreach (var vertex in vertices)
            {
                vertex.Position = ToVector3D(compressor.DecompressPosition(new RealQuaternion(vertex.Position.I, vertex.Position.J, vertex.Position.K, 1)));
                vertex.TexCoords = ToVector3D(compressor.DecompressUv(new RealVector2d(vertex.TexCoords.I, 1.0f - vertex.TexCoords.J)));
            }
        }
        /// <summary>
        /// Reads the index buffer data and converts it into a triangle list if necessary.
        /// </summary>
        /// <param name="reader">The mesh reader to use.</param>
        /// <param name="part">The mesh part to read.</param>
        /// <returns>The index buffer converted into a triangle list.</returns>
        private static ushort[] ReadIndices(MeshReader reader, Part part = null)
        {
            // Use index buffer 0
            var indexBuffer = reader.IndexBuffers[0];
            if (indexBuffer == null)
                throw new InvalidOperationException("Index buffer 0 is null");

            // Read the indices
            var indexStream = reader.OpenIndexBufferStream(indexBuffer);
            indexStream.Position = part is null ? 0u : part.FirstIndexOld;
            switch (indexBuffer.Format)
            {
                case IndexBufferFormat.TriangleList:
                    return indexStream.ReadIndices(part is null ? (uint)indexBuffer.Data.Data.Length : part.IndexCountOld);
                case IndexBufferFormat.TriangleStrip:
                    return indexStream.ReadTriangleStrip(part is null ? 1000u : part.IndexCountOld); //(uint)indexBuffer.Data.Data.Length
                default:
                    throw new InvalidOperationException("Unsupported index buffer type: " + indexBuffer.Format);
            }
        }
        private static List<GenericVertex> ReadVertices(MeshReader reader)
        {
            // Open a vertex reader on stream 0 (main vertex data)
            var mainBuffer = reader.VertexStreams[0];
            if (mainBuffer == null)
                return new List<GenericVertex>();

            var vertexReader = reader.OpenVertexStream(mainBuffer);

            switch (reader.Mesh.Type)
            {
                case VertexType.Rigid:
                    return ReadRigidVertices(vertexReader, mainBuffer.Count);
                case VertexType.Skinned:
                    return ReadSkinnedVertices(vertexReader, mainBuffer.Count);
                case VertexType.DualQuat:
                    return ReadDualQuatVertices(vertexReader, mainBuffer.Count);
                case VertexType.World:
                    return ReadWorldVertices(vertexReader, mainBuffer.Count);
                default:
                    throw new InvalidOperationException("Only Rigid, Skinned, DualQuat and World meshes are supported");
            }
        }
        private static RealVector3d ToVector3D(RealVector3d v)
        {
            return new RealVector3d(v.I, v.J, v.K);
        }
        private static RealVector3d ToVector3D(RealQuaternion q)
        {
            return new RealVector3d(q.I, q.J, q.K);
        }
        private static RealVector3d ToVector3D(RealVector2d u)
        {
            return new RealVector3d(u.I, u.J, 0);
        }
        private class GenericVertex
        {
            public RealVector3d Position { get; set; }
            public RealVector3d Normal { get; set; }
            public RealVector3d TexCoords { get; set; }
            public RealVector3d Tangents { get; set; }
            public RealVector3d Binormals { get; set; }
            public byte[] Indices { get; set; }
            public float[] Weights { get; set; }
        }
    }
}