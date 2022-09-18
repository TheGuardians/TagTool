using Assimp;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System;

using TagTool.Cache;
using TagTool.Commands.Bitmaps;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Commands.Tags;

namespace TagTool.Geometry
{
    // TODO: Add multi vertex stream support, mainly for water vertices
    public class ModelExtractor
    {
        private readonly Scene Scene;
        private readonly GameCache CacheContext;

        private Dictionary<int, int> MeshMapping; //key is the absolute subMesh index, value is the Scene.Meshes index
        private List<Node> BoneNodes;

        private readonly RenderModel RenderModel;

        private readonly string DecoratorBitmap;
        private HashSet<string> Bitmaps; // List of bitmaps to extract

        public ModelExtractor(GameCache cacheContext, RenderModel renderModel, string decoratorBitmap = null)
        {
            Scene = new Scene();

            CacheContext = cacheContext;
            RenderModel = renderModel.DeepCloneV2();

            MeshMapping = new Dictionary<int, int>();
            BoneNodes = new List<Node>();

            DecoratorBitmap = decoratorBitmap;

            // Deserialize the render_model resource
            var resource = cacheContext.ResourceCache.GetRenderGeometryApiResourceDefinition(RenderModel.Geometry.Resource);
            RenderModel.Geometry.SetResourceBuffers(resource);

            var meshes = RenderModel.Geometry.Meshes;

            // Index unindexed geo
            if (meshes.Count > 0 && meshes[0].Flags.HasFlag(MeshFlags.MeshIsUnindexed))
            {
                foreach (var mesh in meshes)
                {
                    mesh.IndexBufferType = PrimitiveType.TriangleList;
                    mesh.Flags &= ~MeshFlags.MeshIsUnindexed;

                    foreach (var part in mesh.Parts)
                        mesh.ResourceIndexBuffers[0] = IndexBufferConverter.CreateIndexBuffer(part.IndexCount);
                }
            }

            // Help decorators work
            if (meshes.Count > 0 && meshes[0].Type == VertexType.Decorator)
                FixUpDecorator(RenderModel);
        }

        public void ExtractBitmaps(string exportFolder, string exportFormat = "dds")
        {
            var bitmapList = (Bitmaps.Count > 0) ? Bitmaps : Scene.Materials.Select(m => m.TextureDiffuse.FilePath).ToHashSet();

            foreach (var path in bitmapList)
            {
                if (path == null)
                    continue;

                var bitmapTagPath = path.Split('.')[0] + ".bitmap";

                if (!CacheContext.TagCache.TryGetTag(bitmapTagPath, out var bitmapTag))
                    continue;

                Bitmap bitmap;
                using (var cacheStream = CacheContext.OpenCacheRead())
                    bitmap = CacheContext.Deserialize<Bitmap>(cacheStream, bitmapTag);

                if (bitmap != null)
                {
                    var outBackup = Console.Out;
                    Console.SetOut(StreamWriter.Null);
                    var outDir = Path.Combine(exportFolder, Path.GetDirectoryName(bitmapTagPath));
                    Directory.CreateDirectory(outDir);
                    new ExtractBitmapCommand(CacheContext, bitmapTag, bitmap).Execute(new List<string>() { outDir });
                    Console.SetOut(outBackup);
                }
            }
        }

        /// <summary>
        /// Export geometry to 3D model file.
        /// </summary>
        /// <param name="exportFileFormat">AMF, DAE or OBJ</param>
        /// <param name="exportFilePath">File to write 3D model to.</param>
        /// <param name="exportBitmapsFolder">If set, used diffuse bitmaps will be exported as DDS to this folder.</param>
        /// <param name="filterVariant">If set, only this variant will be exported.</param>
        public bool Export(
            string exportFileFormat,
            string exportFilePath,
            string exportBitmapsFolder = null,
            string[] filterVariant = null) 
        {

            Bitmaps = new HashSet<string>();

            // Prevent models with the same name overwriting during batch rip
            var tmpExtractPath = exportFilePath.Split('.')[0];
            for (int i = 0; File.Exists(exportFilePath); i++)
                exportFilePath = $"{tmpExtractPath}({i}).{exportFileFormat}";

            var modelFile = new FileInfo($"{exportFilePath}");
            modelFile.Directory.Create();

            bool success = false;
            switch (exportFileFormat)
            {
                case "amf":
                    success = ExportAMF(modelFile, filterVariant);
                    break;
                case "obj":
                    PopulateAssimpScene(filterVariant);
                    success = ExportObject(modelFile);
                    break;
                case "dae":
                    PopulateAssimpScene(filterVariant);
                    success = ExportCollada(modelFile);
                    break;
                default:
                    new TagToolError(CommandError.ArgInvalid, $"Unsupported export format \"{exportFileFormat}\"");
                    return success;
            }

            if (success && exportBitmapsFolder != null)
                ExtractBitmaps(exportBitmapsFolder);

            Console.WriteLine($"\n   Model successfully extracted to \"{exportFilePath}\"");
            return success;
        }

        public bool ExportCollada(FileInfo file)
        {
            using (var exporter = new AssimpContext())
            {
                return exporter.ExportFile(Scene, file.FullName, "collada", PostProcessSteps.ValidateDataStructure);
            }
        }

        public bool ExportObject(FileInfo file)
        {
            ExtractBitmaps("dds");
            using (var exporter = new AssimpContext())
            {
                return exporter.ExportFile(Scene, file.FullName, "obj", PostProcessSteps.ValidateDataStructure);
            }
            
        }

        public bool ExportAMF(FileInfo file, string[] variantName = null, float scale = 100)
        {
            byte[] NullTerminate(string x) { return Encoding.ASCII.GetBytes(x + char.MinValue); }
            byte[] GetStringNT(StringId x) { return NullTerminate(CacheContext.StringTable.GetString(x)); }
            byte ConvertVertTypeEnum(VertexType x) { return (byte)(x == VertexType.Rigid ? 2 : x == VertexType.Skinned ? 1 : 0); }

            using (var amfStream = file.Create())
            using (var amfWriter = new EndianWriter(amfStream, EndianFormat.LittleEndian))
            {
                var dupeDic = new Dictionary<int, long>();

                var validRegions = RenderModel.Regions.Select((r, ri) => new { Name = r.Name, RegionsIdx = ri, Permutations = r.Permutations
                        .Where((p, pi) =>
                        p.MeshCount > 0 &&
                        p.MeshCount != 65535 &&
                        p.MeshIndex > -1 &&
                        p.MeshIndex < RenderModel.Geometry.Meshes.Count &&
                        RenderModel.Geometry.Meshes.ElementAtOrDefault(p.MeshIndex).Parts.Count > 0 &&
                        (variantName == null || variantName.Contains(CacheContext.StringTable.GetString(p.Name), StringComparer.OrdinalIgnoreCase))
                        ).ToList() })
                    .Where(r => r.Permutations.Count > 0).ToList();


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

                amfWriter.Write(GetStringNT(RenderModel.Name));

                amfWriter.Write(RenderModel.Nodes.Count);
                headerAddressList.Add(amfWriter.BaseStream.Position);
                amfWriter.Write(0);

                amfWriter.Write(RenderModel.MarkerGroups.Count);
                headerAddressList.Add(amfWriter.BaseStream.Position);
                amfWriter.Write(0);

                amfWriter.Write(validRegions.Count);
                headerAddressList.Add(amfWriter.BaseStream.Position);
                amfWriter.Write(0);

                amfWriter.Write(RenderModel.Materials.Count);
                headerAddressList.Add(amfWriter.BaseStream.Position);
                amfWriter.Write(0);
                #endregion


                #region Nodes
                headerValueList.Add(amfWriter.BaseStream.Position);
                var sn = Scene.RootNode;

                foreach (var node in RenderModel.Nodes)
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
                foreach (var group in RenderModel.MarkerGroups)
                {
                    amfWriter.Write(GetStringNT(group.Name));
                    amfWriter.Write(group.Markers.Count);
                    markerAddressList.Add(amfWriter.BaseStream.Position);
                    amfWriter.Write(0);
                }
                #endregion


                #region Markers
                foreach (var group in RenderModel.MarkerGroups)
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
                        var reader = new MeshReader(CacheContext.Version, CacheContext.Platform, RenderModel.Geometry.Meshes[perm.MeshIndex], CacheContext.Endianness);
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
                        var mesh = RenderModel.Geometry.Meshes[perm.MeshIndex];
                        var meshReader = new MeshReader(CacheContext.Version, CacheContext.Platform, RenderModel.Geometry.Meshes[perm.MeshIndex], CacheContext.Endianness);

                        List<GenericVertex> vertices;
                        if (CacheContext.Version >= CacheVersion.HaloReach)
                            vertices = ReadVerticesReach(meshReader);
                        else
                            vertices = ReadVertices(meshReader);

                        DecompressVertices(vertices, new VertexCompressor(RenderModel.Geometry.Compression[0]));

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
                            amfWriter.Write(vert.Position.X * scale);
                            amfWriter.Write(vert.Position.Y * scale);
                            amfWriter.Write(vert.Position.Z * scale);
                            amfWriter.Write(vert.Normal.X);
                            amfWriter.Write(vert.Normal.Y);
                            amfWriter.Write(vert.Normal.Z);
                            amfWriter.Write(vert.TexCoords.X);
                            amfWriter.Write(1 - vert.TexCoords.Y);

                            if (mesh.Type == VertexType.Rigid)
                            {
                                var indices = vert.Indices is null ? new List<int>() { 0 }
                                    : vert.Indices.Distinct().Where(x => x != 0).Select(v => (int)v).ToList();

                                foreach (int index in indices) amfWriter.Write((byte)index);

                                if (indices.Count < 4) amfWriter.Write(byte.MaxValue);
                            }
                            else if (mesh.Type == VertexType.Skinned)
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
                        var reader = new MeshReader(CacheContext.Version, CacheContext.Platform, RenderModel.Geometry.Meshes[perm.MeshIndex], CacheContext.Endianness);

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
                                if (reader.Mesh.ResourceVertexBuffers.Count() > ushort.MaxValue)
                                    amfWriter.Write(index);
                                else
                                    amfWriter.Write(index);
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

                        var reader = new MeshReader(CacheContext.Version, CacheContext.Platform, RenderModel.Geometry.Meshes[perm.MeshIndex], CacheContext.Endianness);

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
                foreach (var material in RenderModel.Materials)
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

                    var shaderName = Path.GetFileNameWithoutExtension(DecoratorBitmap != null ? DecoratorBitmap : material.RenderMethod.ToString());
                    amfWriter.Write(NullTerminate(shaderName));

                    RenderMethod renderMethod = new RenderMethod();
                    using (var cacheStream = CacheContext.OpenCacheRead())
                    {
                        renderMethod = CacheContext.Deserialize<RenderMethod>(cacheStream, material.RenderMethod);
                    }

                    for (int i = 0; i < 8; i++)
                    {
                        var submatName = nullPath;
                        if (i < renderMethod.ShaderProperties[0].TextureConstants.Count())
                            submatName = (DecoratorBitmap != null && i == 0) ? DecoratorBitmap
                                : renderMethod.ShaderProperties[0].TextureConstants[i].Bitmap.Name;

                        amfWriter.Write(NullTerminate(submatName));
                        if (submatName != nullPath)
                        {
                            Bitmaps.Add(submatName);
                            amfWriter.Write(1f);
                            amfWriter.Write(1f);
                        }
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        amfWriter.Write((byte)0);
                        amfWriter.Write((byte)0);
                        amfWriter.Write((byte)0);
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

                return true;
            }

        }


        /// <summary>
        /// Build a node structure and mesh from the render model.
        /// </summary>
        /// <returns></returns>
        public void PopulateAssimpScene(string[] variantName = null)
        {

            Scene.RootNode = new Node(CacheContext.StringTable.GetString(RenderModel.Name));
            
            // set Z as up
            Scene.RootNode.Transform = new Matrix4x4(
                1, 0, 0, 0,
                0, 0, 1, 0,
                0,-1, 0, 0,
                0, 0, 0, 1);

            // Pass 1 create bones and assimp nodes as enumerated in render model nodes
            foreach (var node in RenderModel.Nodes)
            {
                Node newNode = new Node(CacheContext.StringTable.GetString(node.Name));

                var r = node.DefaultRotation.Normalize();
                var t = System.Numerics.Matrix4x4
                    .CreateFromQuaternion(new System.Numerics.Quaternion(r.I, r.J, r.K, r.W));
                var transform = new Matrix4x4(
                    t.M11, t.M21, t.M31, node.DefaultTranslation.X,
                    t.M12, t.M22, t.M32, node.DefaultTranslation.Y,
                    t.M13, t.M23, t.M33, node.DefaultTranslation.Z,
                    t.M14, t.M24, t.M34, t.M44);

                newNode.Transform = transform;
                BoneNodes.Add(newNode);
            }

            // Pass 2 create node structure from render model node indices
            for (int i = 0; i < RenderModel.Nodes.Count; i++)
            {
                var node = RenderModel.Nodes[i];
                var boneNode = BoneNodes[i];
                var parentIndex = node.ParentNode;

                if (parentIndex == -1)
                    Scene.RootNode.Children.Add(boneNode);
                else
                {
                    BoneNodes[parentIndex].Children.Add(boneNode);
                }
            }

            foreach (var region in RenderModel.Regions)
            {
                var regionName = CacheContext.StringTable.GetString(region.Name);
                foreach (var permutation in region.Permutations)
                {
                    var permutationName = CacheContext.StringTable.GetString(permutation.Name);
                    // only export the specified permutations
                    if(variantName == null || variantName.Contains(permutationName, StringComparer.OrdinalIgnoreCase))
                    {
                        // Unarmed has its mesh count set to this, but no mesh.
                        if (permutation.MeshCount == 65535)
                            continue;

                        for (int i = 0; i < permutation.MeshCount; i++)
                        {
                            var meshName = $"mesh_{i}";
                            var meshIndex = i + permutation.MeshIndex;

                            if (!(meshIndex < RenderModel.Geometry.Meshes.Count))
                            {
                                Console.WriteLine($"Mesh [{meshIndex}] does not exist.");
                                continue;
                            }

                            var mesh = RenderModel.Geometry.Meshes[meshIndex];

                            for (int j = 0; j < mesh.Parts.Count; j++)
                            {
                                var partName = $"part_{j}";
                                int absSubMeshIndex = GetAbsoluteIndexSubMesh(meshIndex) + j;
                                int sceneMeshIndex;

                                if (!MeshMapping.ContainsKey(absSubMeshIndex))
                                {
                                    sceneMeshIndex = ExtractMeshPart(meshIndex, j);
                                    MeshMapping.Add(absSubMeshIndex, sceneMeshIndex);
                                }
                                else
                                {
                                    MeshMapping.TryGetValue(absSubMeshIndex, out sceneMeshIndex);
                                }
                                Node node = new Node
                                {
                                    Name = $"{regionName}:{permutationName}:{meshName}:{partName}"
                                };
                                node.MeshIndices.Add(sceneMeshIndex);
                                Scene.RootNode.Children.Add(node);
                            }
                        }
                    }
                }
            }


            for (int i = 0; i < RenderModel.Materials.Count(); i++)
            {
                var rmMaterial = RenderModel.Materials[i];

                var material = new Material
                {
                    Name = DecoratorBitmap != null ? DecoratorBitmap : rmMaterial.RenderMethod.Name
                };

                RenderMethod renderMethod;
                using (var cacheStream = CacheContext.OpenCacheRead())
                    renderMethod = CacheContext.Deserialize<RenderMethod>(cacheStream, rmMaterial.RenderMethod);

                foreach (var prop in renderMethod.ShaderProperties)
                {
                    RenderMethodTemplate template;
                    using (var cacheStream = CacheContext.OpenCacheRead())
                        template = CacheContext.Deserialize<RenderMethodTemplate>(cacheStream, prop.Template);

                    var baseMapStringId = CacheContext.StringTable.GetStringId("base_map");
                    var baseMapIndex = template.TextureParameterNames.FindIndex(x => x.Name == baseMapStringId);

                    if (baseMapIndex == -1)
                    {
                        Scene.Materials.Add(material);
                        continue;
                    }

                    var baseMapTexture = prop.TextureConstants[baseMapIndex];

                    var baseMapTS = new TextureSlot();
                    baseMapTS.FilePath = (DecoratorBitmap != null ? DecoratorBitmap : baseMapTexture.Bitmap.Name) + ".dds";
                    baseMapTS.TextureType = TextureType.Diffuse;
                    baseMapTS.WrapModeU =
                        baseMapTexture.SamplerAddressMode.AddressU.ToString() == "Clamp" ? TextureWrapMode.Clamp :
                        baseMapTexture.SamplerAddressMode.AddressU.ToString() == "Mirror" ? TextureWrapMode.Mirror :
                        TextureWrapMode.Wrap;
                    baseMapTS.WrapModeV =
                        baseMapTexture.SamplerAddressMode.AddressV.ToString() == "Clamp" ? TextureWrapMode.Clamp :
                        baseMapTexture.SamplerAddressMode.AddressV.ToString() == "Mirror" ? TextureWrapMode.Mirror :
                        TextureWrapMode.Wrap;
                    material.AddMaterialTexture(ref baseMapTS);
                }

                Scene.Materials.Add(material);
            }
        }

        /// <summary>
        /// Create a mesh and return index in scene mesh list
        /// </summary>
        /// <param name="meshIndex"></param>
        /// <param name="subMeshIndex"></param>
        /// <returns></returns>
        private int ExtractMeshPart(int meshIndex, int subMeshIndex)
        {
            int index = Scene.MeshCount;
            var mesh = ExtractMeshPartGeometry(meshIndex, subMeshIndex);
            mesh.Name = $"part_{index}";
            Scene.Meshes.Add(mesh);
            return index;
        }

        private Assimp.Mesh ExtractMeshPartGeometry(int meshIndex, int partIndex)
        {
            // create new assimp mesh

            Assimp.Mesh mesh = new Assimp.Mesh();

            // Add support for multiple UV layers
            var textureCoordinateIndex = 0;
            mesh.UVComponentCount[textureCoordinateIndex] = 2;

            // prepare vertex extraction
            var meshReader = new MeshReader(CacheContext.Version, CacheContext.Platform, RenderModel.Geometry.Meshes[meshIndex], CacheContext.Endianness);
            var vertexCompressor = new VertexCompressor(RenderModel.Geometry.Compression[0]);

            var geometryMesh = RenderModel.Geometry.Meshes[meshIndex];
            var geometryPart = geometryMesh.Parts[partIndex];

            mesh.MaterialIndex = geometryPart.MaterialIndex;

            // optimize this part to not load and decompress all mesh vertices everytime
            List<GenericVertex> vertices;
            if (CacheContext.Version >= CacheVersion.HaloReach)
                vertices = ReadVerticesReach(meshReader);
            else
                vertices = ReadVertices(meshReader);
            DecompressVertices(vertices, vertexCompressor);
            
            // get offset in the list of all vertices for the mesh
            var vertexOffset = GetPartVertexOffset(meshIndex, partIndex);

            var indices = ReadIndices(meshReader, geometryPart);

            var int_indices = indices.Select(b => (int)b).ToArray();

            var indexCount = indices.Length;

            if (indexCount == 0)
            {
                Console.WriteLine($"Failed to extract mesh, no indices.");
                return null;
            }

            // set index list, maybe require adjustment for vertex buffer offset

            mesh.SetIndices(int_indices, 3);

            // build skeleton for each mesh (meh)

            // create a list of all the mesh bones available in the scene
            foreach (var node in RenderModel.Nodes)
            {
                Bone bone = new Bone();
                bone.Name = CacheContext.StringTable.GetString(node.Name);
                
                bone.OffsetMatrix = new Matrix4x4(
                    node.InverseForward.I, node.InverseLeft.I, node.InverseUp.I, node.InversePosition.X,
                    node.InverseForward.J, node.InverseLeft.J, node.InverseUp.J, node.InversePosition.Y,
                    node.InverseForward.K, node.InverseLeft.K, node.InverseUp.K, node.InversePosition.Z,
                    0, 0, 0, 1);

                mesh.Bones.Add(bone);

            }

            for (int i = vertexOffset; i < vertexOffset + geometryPart.VertexCount; i++)
            {
                var vertex = vertices[i];

                // Flip the model right side up.
                mesh.Vertices.Add(new Vector3D(vertex.Position.X, vertex.Position.Y, vertex.Position.Z)); //vertex.Position.Z, -vertex.Position.Y

                if (vertex.Normal != null)
                    mesh.Normals.Add(vertex.Normal);

                if (vertex.TexCoords != null)
                {
                    // Y didn't like being shifted before decompression, so do it here.
                    mesh.TextureCoordinateChannels[textureCoordinateIndex].Add(
                        new Vector3D(vertex.TexCoords.X, 1 - vertex.TexCoords.Y, vertex.TexCoords.Z));
                }

                if (vertex.Tangents != null)
                    mesh.Tangents.Add(vertex.Tangents);

                if (vertex.Binormals != null)
                    mesh.BiTangents.Add(vertex.Binormals);

                if (vertex.Indices != null)
                {
                    for (int j = 0; j < vertex.Indices.Length; j++)
                    {
                        var index = vertex.Indices[j];
                        if (RenderModel.Geometry.PerMeshNodeMaps.Count > 0)
                            index = RenderModel.Geometry.PerMeshNodeMaps[meshIndex].NodeIndices[index].Node;

                        var bone = mesh.Bones[index];
                        
                        bone.VertexWeights.Add(new VertexWeight(i - vertexOffset, vertex.Weights[j]));
                    }
                }
                // Add skinned mesh support and more
            }

            // create faces
            mesh.Faces.Clear();

            GenerateFaces(int_indices, vertexOffset, mesh.Faces);

            return mesh;
        }

        private void FixUpDecorator(RenderModel renderModel) {

            renderModel.Regions.Add(new RenderModel.Region()
            {
                Name = renderModel.Name,
                Permutations = new List<RenderModel.Region.Permutation>()
            });

            var meshOG = renderModel.Geometry.Meshes[0].DeepCloneV2();
            
            int indexOffset = 0; // Seems to always be the same for all variants, but just incase.
            for (short i = 0; i < renderModel.InstancePlacements.Count; i++)
            {
                renderModel.Regions[0].Permutations.Add(new RenderModel.Region.Permutation()
                {
                    MeshIndex = i,
                    MeshCount = 1,
                    Name = renderModel.InstancePlacements[i].Name
                });

                if (i == renderModel.Geometry.Meshes.Count)
                    renderModel.Geometry.Meshes.Add(meshOG.DeepCloneV2());

                var mesh = renderModel.Geometry.Meshes[i];
                var isLE = CacheContext.Endianness == EndianFormat.LittleEndian;

                mesh.ResourceIndexBuffers[0].Data.Data = Enumerable.Range(0, mesh.SubParts[i].IndexCount).Select(x => (ushort)x)
                    .SelectMany(y => new byte[] { (byte)(isLE ? y & 255 : y >> 8), (byte)(isLE ? y >> 8 : y & 255) }  ).ToArray();

                mesh.ResourceVertexBuffers[0].Data.Data = mesh.ResourceVertexBuffers[0].Data.Data
                    .Skip(indexOffset * 32).Take(mesh.SubParts[i].IndexCount * 32).ToArray();
                
                mesh.Parts[0].FirstIndex = 0;
                mesh.Parts[0].MaterialIndex = 0;
                indexOffset += mesh.ResourceVertexBuffers[0].Count = mesh.Parts[0].IndexCount = mesh.SubParts[i].IndexCount;
            }
            renderModel.Materials.Add(new RenderMaterial() { RenderMethod = CacheContext.TagCache.GetTag(@"shaders\invalid.rmsh") });
        }

        private void GenerateFaces(int[] indices, int vertexOffset, List<Face> meshFaces)
        {
            List<Face> faces = new List<Face>();
            for (int i = 0; i < indices.Length; i += 3)
            {
                var a = indices[i] - vertexOffset;
                var b = indices[i + 1] - vertexOffset;
                var c = indices[i + 2] - vertexOffset;
                if (!(a == b || b == c || a == c))
                    meshFaces.Add(new Face(new int[] { a, b, c }));
            }
        }

        public int GetPartVertexOffset(int meshIndex, int partIndex)
        {
            var mesh = RenderModel.Geometry.Meshes[meshIndex];
            int vertexOffset = 0;
            for (int i = 0; i < partIndex; i++)
            {
                vertexOffset += mesh.Parts[i].VertexCount;
            }
            return vertexOffset;
        }

        /// <summary>
        /// Get the absolute sub_mesh index from the flattened part list list.
        /// </summary>
        /// <param name="meshIndex"></param>
        /// <returns></returns>
        private int GetAbsoluteIndexSubMesh(int meshIndex)
        {
            int absIndex = 0;
            for (int i = 0; i < meshIndex; i++)
            {
                absIndex += RenderModel.Geometry.Meshes[i].Parts.Count;
            }
            return absIndex;
        }

        public static List<GenericVertex> ReadVerticesReach(MeshReader reader)
        {
            // Open a vertex reader on stream 0 (main vertex data)
            var mainBuffer = reader.VertexStreams[0];
            if (mainBuffer == null)
                return new List<GenericVertex>();

            VertexStreamReach vertexReader = (VertexStreamReach)reader.OpenVertexStream(mainBuffer);

            switch (reader.Mesh.ReachType)
            {
                case VertexTypeReach.Rigid:
                    return ReadRigidVertices(vertexReader, mainBuffer.Count);
                case VertexTypeReach.Skinned:
                    return ReadSkinnedVertices(vertexReader, mainBuffer.Count);
                case VertexTypeReach.World:
                    return ReadWorldVertices(vertexReader, mainBuffer.Count);
                case VertexTypeReach.Decorator:
                    return ReadDecoratorVertices(vertexReader, mainBuffer.Count);
                case VertexTypeReach.SkinnedCompressed:
                    return ReadSkinnedCompressedVertices(vertexReader, mainBuffer.Count);
                case VertexTypeReach.RigidCompressed:
                    return ReadRigidCompressedVertices(vertexReader, mainBuffer.Count);
                default:
                    throw new InvalidOperationException("Only Rigid, RigidCompressed, Skinned, SkinnedCompressed, DualQuat, World and Decorator meshes are supported");
            }
        }

        public static List<GenericVertex> ReadVertices(MeshReader reader)
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
                case VertexType.Decorator:
                    return ReadDecoratorVertices(vertexReader, mainBuffer.Count);
                default:
                    throw new InvalidOperationException("Only Rigid, Skinned, DualQuat, World and Decorator meshes are supported");
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
        /// Reads rigid compressed vertices.
        /// </summary>
        /// <param name="reader">The vertex reader to read from.</param>
        /// <param name="count">The number of vertices to read.</param>
        /// <returns>The vertices that were read.</returns>
        private static List<GenericVertex> ReadRigidCompressedVertices(VertexStreamReach reader, int count)
        {
            var result = new List<GenericVertex>();
            for (var i = 0; i < count; i++)
            {
                var rigid = reader.ReadReachRigidVertex();
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
        /// Reads skinned compressed vertices.
        /// </summary>
        /// <param name="reader">The vertex reader to read from.</param>
        /// <param name="count">The number of vertices to read.</param>
        /// <returns>The vertices that were read.</returns>
        private static List<GenericVertex> ReadSkinnedCompressedVertices(VertexStreamReach reader, int count)
        {
            var result = new List<GenericVertex>();
            for (var i = 0; i < count; i++)
            {
                var skinned = reader.ReadReachSkinnedVertex();
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
        /// Reads Decorator vertices.
        /// </summary>
        /// <param name="reader">The vertex reader to read from.</param>
        /// <param name="count">The number of vertices to read.</param>
        /// <returns>The vertices that were read.</returns>
        private static List<GenericVertex> ReadDecoratorVertices(IVertexStream reader, int count)
        {
            var result = new List<GenericVertex>();
            for (var i = 0; i < count; i++)
            {
                var decorator = reader.ReadDecoratorVertex();
                result.Add(new GenericVertex
                {
                    Position = ToVector3D(decorator.Position),
                    Normal = ToVector3D(decorator.Normal),
                    TexCoords = ToVector3D(decorator.Texcoord),
                });
            }
            return result;
        }

        /// <summary>
        /// Decompresses vertex data in-place.
        /// </summary>
        /// <param name="vertices">The vertices to decompress in-place.</param>
        /// <param name="compressor">The compressor to use.</param>
        public static void DecompressVertices(IEnumerable<GenericVertex> vertices, VertexCompressor compressor)
        {
            if (compressor == null)
                return;

            foreach (var vertex in vertices)
            {
                vertex.Position = ToVector3D(compressor.DecompressPosition(new RealQuaternion(vertex.Position.X, vertex.Position.Y, vertex.Position.Z, 1)));
                vertex.TexCoords = ToVector3D(compressor.DecompressUv(new RealVector2d(vertex.TexCoords.X, vertex.TexCoords.Y)));

            }
        }

        /// <summary>
        /// Reads the index buffer data and converts it into a triangle list if necessary.
        /// </summary>
        /// <param name="reader">The mesh reader to use.</param>
        /// <param name="part">The mesh part to read.</param>
        /// <returns>The index buffer converted into a triangle list.</returns>
        public static ushort[] ReadIndices(MeshReader reader, Part part)
        {
            // Use index buffer 0
            var indexBuffer = reader.IndexBuffers[0];
            if (indexBuffer == null)
                throw new InvalidOperationException("Index buffer 0 is null");

            // Read the indices
            var indexStream = reader.OpenIndexBufferStream(indexBuffer);
            indexStream.Position = part.FirstIndex;
            switch (indexBuffer.Format)
            {
                case IndexBufferFormat.TriangleList:
                    return indexStream.ReadIndices(part.IndexCount);
                case IndexBufferFormat.TriangleStrip:
                    return indexStream.ReadTriangleStrip(part.IndexCount);
                default:
                    throw new InvalidOperationException("Unsupported index buffer type: " + indexBuffer.Format);
            }
        }

        private static Vector3D ToVector3D(RealVector3d v)
        {
            return new Vector3D(v.I, v.J, v.K);
        }

        private static Vector3D ToVector3D(RealQuaternion q)
        {
            return new Vector3D(q.I, q.J, q.K);
        }

        private static Vector3D ToVector3D(RealVector2d u)
        {
            return new Vector3D(u.I, u.J, 0);
        }

        private static Vector2D ToVector2D(RealVector2d u)
        {
            return new Vector2D(u.I, u.J);
        }

        /// <summary>
        /// Generic vertex class that contains all the possible information to generate meshes
        /// </summary>
        public class GenericVertex
        {
            public Vector3D Position { get; set; }
            public Vector3D Normal { get; set; }
            public Vector3D TexCoords { get; set; }
            public Vector3D Tangents { get; set; }
            public Vector3D Binormals { get; set; }
            public byte[] Indices { get; set; }
            public float[] Weights { get; set; }
        }
    }
}