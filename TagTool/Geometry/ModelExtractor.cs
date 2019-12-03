using TagTool.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assimp;
using TagTool.Tags.Definitions;
using TagTool.Cache;
using TagTool.Serialization;
using TagTool.Tags.Resources;

namespace TagTool.Geometry
{
    // TODO: Add multi vertex stream support, mainly for water vertices
    public class ModelExtractor
    {
        private readonly Scene Scene;
        private readonly HaloOnlineCacheContext CacheContext;

        private Dictionary<int, int> MeshMapping;   //key is the absolute subMesh index, value is the Scene.Meshes index
        private List<Node> BoneNodes;

        private readonly RenderModel RenderModel; 
        private readonly RenderGeometryApiResourceDefinition RenderModelResourceDefinition;
        private Stream RenderModelResourceStream;

        public ModelExtractor(HaloOnlineCacheContext cacheContext, RenderModel renderModel)
        {
            Scene = new Scene();

            CacheContext = cacheContext;
            RenderModel = renderModel;

            MeshMapping = new Dictionary<int, int>();
            BoneNodes = new List<Node>();

            // Deserialize the render_model resource
            var resourceContext = new ResourceSerializationContext(CacheContext, RenderModel.Geometry.Resource);
            RenderModelResourceDefinition = CacheContext.Deserializer.Deserialize<RenderGeometryApiResourceDefinition>(resourceContext);
            RenderModelResourceStream = new MemoryStream();
            CacheContext.ExtractResource(RenderModel.Geometry.Resource, RenderModelResourceStream);

        }

        ~ModelExtractor()
        {
            RenderModelResourceStream.Close();
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

        /// <summary>
        /// Build a node structure and mesh from the render model.
        /// </summary>
        /// <returns></returns>
        public void ExtractRenderModel(string variantName = "*")
        {
            Scene.RootNode = new Node(CacheContext.GetString(RenderModel.Name));
            // Pass 1 create bones and assimp nodes as enumerated in render model nodes
            foreach (var node in RenderModel.Nodes)
            {
                Node newNode = new Node(CacheContext.GetString(node.Name));
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
                var regionName = CacheContext.GetString(region.Name);
                foreach (var permutation in region.Permutations)
                {
                    var permutationName = CacheContext.GetString(permutation.Name);
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
            var meshReader = new MeshReader(CacheContext.Version, RenderModel.Geometry.Meshes[meshIndex], RenderModelResourceDefinition);
            var vertexCompressor = new VertexCompressor(RenderModel.Geometry.Compression[0]);

            var geometryMesh = RenderModel.Geometry.Meshes[meshIndex];
            var geometryPart = geometryMesh.Parts[partIndex];

            mesh.MaterialIndex = geometryPart.MaterialIndex;

            // optimize this part to not load and decompress all mesh vertices everytime
            var vertices = ReadVertices(meshReader, RenderModelResourceStream);
            DecompressVertices(vertices, vertexCompressor);
            // get offset in the list of all vertices for the mesh
            var vertexOffset = GetPartVertexOffset(meshIndex, partIndex);

            //vertices = vertices.GetRange(vertexOffset, geometryPart.VertexCount);

            var indices = ReadIndices(meshReader, geometryPart, RenderModelResourceStream);

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
                bone.Name = CacheContext.GetString(node.Name);
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
                    mesh.TextureCoordinateChannels[textureCoordinateIndex].Add(vertex.TexCoords);

                if (vertex.Tangents != null)
                    mesh.Tangents.Add(vertex.Tangents);

                if (vertex.Binormals != null)
                    mesh.BiTangents.Add(vertex.Binormals);

                if (vertex.Indices != null)
                {
                    for (int j = 0; j < vertex.Indices.Length; j++)
                    {
                        var index = vertex.Indices[j];
                        var bone = mesh.Bones[index];
                        Matrix4x4 inverseTransform = new Matrix4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);


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

        private static List<GenericVertex> ReadVertices(MeshReader reader, Stream resourceStream)
        {
            // Open a vertex reader on stream 0 (main vertex data)
            var mainBuffer = reader.VertexStreams[0];
            if (mainBuffer == null)
                return new List<GenericVertex>();

            var vertexReader = reader.OpenVertexStream(mainBuffer, resourceStream);

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
        private static void DecompressVertices(IEnumerable<GenericVertex> vertices, VertexCompressor compressor)
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
        /// <param name="resourceStream">A stream open on the resource data.</param>
        /// <returns>The index buffer converted into a triangle list.</returns>
        private static ushort[] ReadIndices(MeshReader reader, Mesh.Part part, Stream resourceStream)
        {
            // Use index buffer 0
            var indexBuffer = reader.IndexBuffers[0];
            if (indexBuffer == null)
                throw new InvalidOperationException("Index buffer 0 is null");

            // Read the indices
            var indexStream = reader.OpenIndexBufferStream(indexBuffer, resourceStream);
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
