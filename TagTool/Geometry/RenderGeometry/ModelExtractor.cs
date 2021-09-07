using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Assimp;

using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Commands.Bitmaps;

namespace TagTool.Geometry
{
    // TODO: Add multi vertex stream support, mainly for water vertices
    public class ModelExtractor
    {
        private readonly Scene Scene;
        private readonly GameCache CacheContext;

        private Dictionary<int, int> MeshMapping;   //key is the absolute subMesh index, value is the Scene.Meshes index
        private List<Node> BoneNodes;

        private readonly RenderModel RenderModel; 

        public ModelExtractor(GameCache cacheContext, RenderModel renderModel)
        {
            Scene = new Scene();

            CacheContext = cacheContext;
            RenderModel = renderModel;

            MeshMapping = new Dictionary<int, int>();
            BoneNodes = new List<Node>();

            // Deserialize the render_model resource
            var resource = cacheContext.ResourceCache.GetRenderGeometryApiResourceDefinition(renderModel.Geometry.Resource);
            renderModel.Geometry.SetResourceBuffers(resource);

        }

        public void ExtractBitmaps(string exportFolder, string exportFormat = "dds")
        {
            foreach (var material in RenderModel.Materials)
            {
                if (material == null)
                    continue;

                RenderMethod renderMethod;
                using (var cacheStream = CacheContext.OpenCacheRead())
                {
                    renderMethod = CacheContext.Deserialize<RenderMethod>(cacheStream, material.RenderMethod);
                }

                foreach (var uMaterial in renderMethod.ShaderProperties.SelectMany(x => x.TextureConstants).ToHashSet())
                {
                    Bitmap bitmap;
                    using (var cacheStream = CacheContext.OpenCacheRead())
                        bitmap = CacheContext.Deserialize<Bitmap>(cacheStream, uMaterial.Bitmap);

                    if (bitmap != null)
                    {
                        var outputDir = Path.Combine(exportFolder, Path.GetDirectoryName(uMaterial.Bitmap.Name));
                        Directory.CreateDirectory(outputDir);
                        var outBackup = Console.Out;
                        Console.SetOut(StreamWriter.Null);
                        new ExtractBitmapCommand(CacheContext, uMaterial.Bitmap, bitmap).Execute(new List<string>() { outputDir });
                        Console.SetOut(outBackup);
                    }
                }

            }            
        }

            public bool ExportCollada(FileInfo file)
        {
            using (var exporter = new AssimpContext())
            {
                return exporter.ExportFile(Scene, file.FullName, "collada", PostProcessSteps.ValidateDataStructure);    //collada or obj
            }
        }

        public bool ExportObject(FileInfo file)
        {
            using (var exporter = new AssimpContext())
            {
                return exporter.ExportFile(Scene, file.FullName, "obj", PostProcessSteps.ValidateDataStructure);
            }
        }

        public bool ExportAMF(FileInfo file, float scale = 100)
        {
            byte[] NullTerminate(string x) { return Encoding.ASCII.GetBytes(x + char.MinValue); }
            byte[] GetStringNT(StringId x) { return NullTerminate(CacheContext.StringTable.GetString(x)); }
            byte ConvertVertTypeEnum(VertexType x) { return (byte)(x == VertexType.Rigid ? 2 : x == VertexType.Skinned ? 1 : 0); }
            
            using (var amfStream = file.Create())
            using (var amfWriter = new EndianWriter(amfStream, EndianFormat.LittleEndian))
            {
                var dupeDic = new Dictionary<int, long>();

                var validRegions = RenderModel.Regions
                                .Select((r, ri) => new { Name = r.Name, RegionsIdx = ri, Permutations = r.Permutations.Where((p, pi) => p.MeshIndex > -1 && p.MeshCount > 0 && RenderModel.Geometry.Meshes.ElementAtOrDefault(p.MeshIndex).Parts.Count > 0).ToList() })
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
                        var reader = new MeshReader(CacheContext.Version, CacheContext.Platform, RenderModel.Geometry.Meshes[perm.MeshIndex]);

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
                        var part = RenderModel.Geometry.Meshes[perm.MeshIndex];
                        var meshReader = new MeshReader(CacheContext.Version, CacheContext.Platform, RenderModel.Geometry.Meshes[perm.MeshIndex]);
                        var vertices = ReadVertices(meshReader);
                        DecompressVertices(vertices, new VertexCompressor(RenderModel.Geometry.Compression[0]), invertTexY: false);

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

                            if (part.Type == VertexType.Rigid)
                            {
                                var indices = vert.Indices is null ? new List<int>() { 0 }
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
                        var reader = new MeshReader(CacheContext.Version, CacheContext.Platform, RenderModel.Geometry.Meshes[perm.MeshIndex]);

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

                        var reader = new MeshReader(CacheContext.Version, CacheContext.Platform, RenderModel.Geometry.Meshes[perm.MeshIndex]);

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

                    var shaderName = material.RenderMethod.ToString().Split('\\', '.');
                    amfWriter.Write(NullTerminate(shaderName[shaderName.Length - 2]));

                    RenderMethod renderMethod;
                    RenderMethodTemplate renderMethodTemplate;
                    using (var cacheStream = CacheContext.OpenCacheRead())
                    {
                        renderMethod = CacheContext.Deserialize<RenderMethod>(cacheStream, material.RenderMethod);
                        renderMethodTemplate = CacheContext.Deserialize<RenderMethodTemplate>(cacheStream, renderMethod.ShaderProperties[0].Template);
                    }

                    for (int i = 0; i < 8; i++)
                    {
                        var submatName = nullPath;
                        if (i < renderMethod.ShaderProperties[0].TextureConstants.Count())
                        {
                            submatName = renderMethod.ShaderProperties[0].TextureConstants[i].Bitmap.Name;
/*                            if (extractBitmapsPath != "")
                            {
                                Bitmap bitmap;
                                using (var cacheStream = CacheContext.OpenCacheRead())
                                    bitmap = CacheContext.Deserialize<Bitmap>(cacheStream, renderMethod.ShaderProperties[0].TextureConstants[i].Bitmap);

                                if (bitmap != null)
                                {
                                    var outputDir = Path.Combine(extractBitmapsPath, Path.GetDirectoryName(submatName));
                                    Directory.CreateDirectory(outputDir);
                                    var outBackup = Console.Out;
                                    Console.SetOut(StreamWriter.Null);
                                    new ExtractBitmapCommand(CacheContext, renderMethod.ShaderProperties[0].TextureConstants[i].Bitmap, bitmap).Execute(new List<string>() { outputDir });
                                    Console.SetOut(outBackup);
                                }
                            }*/
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

                return true;
            }
         
        }

        /// <summary>
        /// Build a node structure and mesh from the render model.
        /// </summary>
        /// <returns></returns>
        public void ExtractRenderModel(string variantName = "*")
        {
            Scene.RootNode = new Node(CacheContext.StringTable.GetString(RenderModel.Name));
            // Pass 1 create bones and assimp nodes as enumerated in render model nodes
            foreach (var node in RenderModel.Nodes)
            {
                Node newNode = new Node(CacheContext.StringTable.GetString(node.Name));
                //compute transform

                newNode.Transform = ComputeTransform(node);
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
                    if( permutationName == variantName || variantName == "*")
                    {
                        for (int i = 0; i < permutation.MeshCount; i++)
                        {
                            var meshName = $"mesh_{i}";
                            var meshIndex = i + permutation.MeshIndex;
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
                Scene.Materials.Add(new Material());
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
            var meshReader = new MeshReader(CacheContext.Version, CacheContext.Platform, RenderModel.Geometry.Meshes[meshIndex]);
            var vertexCompressor = new VertexCompressor(RenderModel.Geometry.Compression[0]);

            var geometryMesh = RenderModel.Geometry.Meshes[meshIndex];
            var geometryPart = geometryMesh.Parts[partIndex];

            mesh.MaterialIndex = geometryPart.MaterialIndex;

            // optimize this part to not load and decompress all mesh vertices everytime
            var vertices = ReadVertices(meshReader);
            DecompressVertices(vertices, vertexCompressor);
            // get offset in the list of all vertices for the mesh
            var vertexOffset = GetPartVertexOffset(meshIndex, partIndex);

            //vertices = vertices.GetRange(vertexOffset, geometryPart.VertexCount);

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
                bone.OffsetMatrix = new Matrix4x4();
                mesh.Bones.Add(bone);

            }

            for (int i = vertexOffset; i < vertexOffset + geometryPart.VertexCount; i++)
            {
                var vertex = vertices[i];
                mesh.Vertices.Add(vertex.Position);

                if (vertex.Normal != null)
                    mesh.Normals.Add(vertex.Normal);

                if (vertex.TexCoords != null)
                {
                    mesh.TextureCoordinateChannels[textureCoordinateIndex].Add(vertex.TexCoords);
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
                        if(RenderModel.Geometry.PerMeshNodeMaps.Count > 0)
                            index = RenderModel.Geometry.PerMeshNodeMaps[meshIndex].NodeIndices[index].Node;

                        var bone = mesh.Bones[index];
                        Matrix4x4 inverseTransform = Matrix4x4.Identity;

                        
                        var currentNode = BoneNodes[index];
                        while (currentNode!=null)
                        {
                            Matrix4x4 inverse = (currentNode.Transform.DeepClone());
                            inverse.Inverse();
                            inverseTransform = inverse * inverseTransform;
                            currentNode = currentNode.Parent;
                        }
                        bone.OffsetMatrix = inverseTransform;
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

        private int GetPartVertexOffset(int meshIndex, int partIndex)
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
        private static void DecompressVertices(IEnumerable<GenericVertex> vertices, VertexCompressor compressor, bool invertTexY = true)
        {
            if (compressor == null)
                return;

            foreach (var vertex in vertices)
            {
                vertex.Position = ToVector3D(compressor.DecompressPosition(new RealQuaternion(vertex.Position.X, vertex.Position.Y, vertex.Position.Z, 1)));
                vertex.TexCoords = ToVector3D(compressor.DecompressUv(new RealVector2d(vertex.TexCoords.X, invertTexY ? 1.0f - vertex.TexCoords.Y : vertex.TexCoords.Y)));
            }
        }

        /// <summary>
        /// Reads the index buffer data and converts it into a triangle list if necessary.
        /// </summary>
        /// <param name="reader">The mesh reader to use.</param>
        /// <param name="part">The mesh part to read.</param>
        /// <returns>The index buffer converted into a triangle list.</returns>
        private static ushort[] ReadIndices(MeshReader reader, Part part)
        {
            // Use index buffer 0
            var indexBuffer = reader.IndexBuffers[0];
            if (indexBuffer == null)
                throw new InvalidOperationException("Index buffer 0 is null");

            // Read the indices
            var indexStream = reader.OpenIndexBufferStream(indexBuffer);
            indexStream.Position = part.FirstIndexOld;
            switch (indexBuffer.Format)
            {
                case IndexBufferFormat.TriangleList:
                    return indexStream.ReadIndices(part.IndexCountOld);
                case IndexBufferFormat.TriangleStrip:
                    return indexStream.ReadTriangleStrip(part.IndexCountOld);
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
        private class GenericVertex
        {
            public Vector3D Position { get; set; }
            public Vector3D Normal { get; set; }
            public Vector3D TexCoords { get; set; }
            public Vector3D Tangents { get; set; }
            public Vector3D Binormals { get; set; }
            public byte[] Indices { get; set; }
            public float[] Weights { get; set; }
        }

        private Matrix4x4 ComputeTransform(RenderModel.Node node)
        {
            Matrix4x4 matrix_translation = new Matrix4x4();

            matrix_translation.A4 = node.DefaultTranslation.X;
            matrix_translation.B4 = node.DefaultTranslation.Y;
            matrix_translation.C4 = node.DefaultTranslation.Z;

            matrix_translation.A1 = 1;
            matrix_translation.B2 = 1;
            matrix_translation.C3 = 1;
            matrix_translation.D4 = 1;

            Matrix4x4 matrix = ComputeRotation(node) * matrix_translation;

            return matrix;
        }

        private Matrix4x4 ComputeRotation(RenderModel.Node node)
        {
            Matrix4x4 rot = new Matrix4x4();
            rot.D4 = 1;
            var quat = node.DefaultRotation.Normalize();

            float sqw = quat.W * quat.W;
            float sqx = quat.I * quat.I;
            float sqy = quat.J * quat.J;
            float sqz = quat.K * quat.K;

            
            rot.A1 = (sqx - sqy - sqz + sqw);
            rot.B2 = (-sqx + sqy - sqz + sqw);
            rot.C3 = (-sqx - sqy + sqz + sqw);

            float tmp1 = quat.I * quat.J;
            float tmp2 = quat.K * quat.W;
            rot.B1 = 2.0f * (tmp1 + tmp2);
            rot.A2 = 2.0f * (tmp1 - tmp2);

            tmp1 = quat.I * quat.K;
            tmp2 = quat.J * quat.W;
            rot.C1 = 2.0f * (tmp1 - tmp2);
            rot.A3 = 2.0f * (tmp1 + tmp2);
            tmp1 = quat.J * quat.K;
            tmp2 = quat.I * quat.W;
            rot.C2 = 2.0f * (tmp1 + tmp2);
            rot.B3 = 2.0f * (tmp1 - tmp2);
            return rot;
        }

    }
}
