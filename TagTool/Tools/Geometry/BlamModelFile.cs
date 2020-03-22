using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Geometry;
using TagTool.Tags.Definitions;
using TagTool.Cache;
using System.IO;
using TagTool.IO;
using TagTool.Serialization;

namespace TagTool.Tools.Geometry
{
    static class Constants
    {
        public const int StringLength = 0x40;
        public const int HeaderSize = 0x14;
    }

    /// <summary>
    /// Generic render geometry class for interfacing with other tools. Based on gen3 formats but should support any halo engine. Names\file extension temporary
    /// </summary>
    [TagStructure(Size = 0x14)]
    public class BMFHeader : TagStructure
    {
        public Tag Signature = new Tag("bmf!"); // hgf! : blam model file
        public BMFVersion Version;
        public BMFFlags Flags;
        public BMFContentFlags ContentFlags;
        public int Offset;

        public BMFHeader()
        {
            Version = BMFVersion.Prototype;
            Flags = BMFFlags.None;
            ContentFlags = BMFContentFlags.None;
            Offset = 0;
        }
    }

    [TagStructure(Size = 0x154)] // TODO: recompute
    public class BlamModelFile : TagStructure
    {
        [TagField(Flags = TagFieldFlags.Runtime)]
        BMFHeader Header;

        [TagField(Length = Constants.StringLength, ForceNullTerminated = true)]
        public string Name = "default";

        public List<BMFRegion> Regions;
        public List<BMFMaterial> Materials;
        public List<BMFNode> Nodes;
        public List<BMFMarkers> Markers;
        public List<BMFMesh> Meshes;
        public BMFGlobalLighting DefaultLighting;
        public List<BMFLightProbe> LightProbes;
        public List<RenderGeometry.BoundingSphere> BoundingSpheres;


        public void SerializeToFile(EndianWriter writer)
        {
            var context = new DataSerializationContext(writer);
            var serializer = new TagSerializer(CacheVersion.HaloOnline106708);

            writer.BaseStream.Position = Constants.HeaderSize;
            context.PointerOffset = Constants.HeaderSize;
            serializer.Serialize(context, this);
            Header.Offset = (int)context.MainStructOffset + Constants.HeaderSize;
            writer.BaseStream.Position = 0x0;
            serializer.Serialize(context, Header);
        }

        

        public RenderModel GenerateGen3RenderModel(GameCache cache)
        {
            RenderModel mode = new RenderModel();

            mode.Name = AddStringId(cache, Name);

            // set materials
            mode.Materials = new List<RenderMaterial>();
            for (int i = 0; i < Materials.Count; i++)
            {
                mode.Materials.Add(new RenderMaterial());
                Console.WriteLine($"Render material {i} is {Materials[i].Name}");
            }

            // set nodes
            mode.Nodes = new List<RenderModel.Node>();
            mode.RuntimeNodeOrientations = new List<RenderModel.RuntimeNodeOrientation>();

            for (int i = 0; i < Nodes.Count; i++)
            {
                var node = Nodes[i];
                var quat = node.Rotation.Normalize();

                float sqw = quat.W * quat.W;
                float sqx = quat.I * quat.I;
                float sqy = quat.J * quat.J;
                float sqz = quat.K * quat.K;
                // use quaternion -> rotation matrix instead later on
                RealVector3d inverseForward = new RealVector3d((sqx - sqy - sqz + sqw), 2.0f * (quat.I * quat.J - quat.K * quat.W), 2.0f * (quat.I * quat.K + quat.J * quat.W));
                RealVector3d inverseLeft = new RealVector3d(2.0f * (quat.I * quat.J + quat.K * quat.W), (-sqx + sqy - sqz + sqw), 2.0f * (quat.J * quat.K - quat.I * quat.W));
                RealVector3d inverseUp = new RealVector3d(2.0f * (quat.I * quat.K - quat.J * quat.W), 2.0f * (quat.J * quat.K + quat.I * quat.W), (-sqx - sqy + sqz + sqw));

                mode.Nodes.Add(new RenderModel.Node
                {
                    Name = AddStringId(cache, node.Name),
                    ParentNode = (short)node.ParentNodeIndex,
                    FirstChildNode = (short)node.FirstChildNodeIndex,
                    NextSiblingNode = (short)node.NextSiblingNodeIndex,
                    Flags = RenderModel.NodeFlags.None,
                    DefaultTranslation = node.Translation,
                    DefaultRotation = node.Rotation,
                    DefaultScale = node.Scale,

                    InverseForward = inverseForward,
                    InverseLeft = inverseLeft,
                    InverseUp = inverseUp,
                    InversePosition = -1 * node.Translation,

                    DistanceFromParent = i == 0 ? 0.0f : RealPoint3d.Distance(node.Translation - Nodes[node.ParentNodeIndex].Translation)
                });



                mode.RuntimeNodeOrientations.Add(new RenderModel.RuntimeNodeOrientation
                {
                    Rotation = node.Rotation,
                    Scale = node.Scale,
                    Translation = node.Translation
                });
            }

            // set lighting

            mode.UnknownSHProbes = new List<RenderModel.UnknownSHProbe>();
            mode.SHBlue = DefaultLighting.SHBlue;
            mode.SHRed = DefaultLighting.SHRed;
            mode.SHGreen = DefaultLighting.SHGreen;
            foreach (var lightProbe in LightProbes)
            {
                mode.UnknownSHProbes.Add(new RenderModel.UnknownSHProbe
                {
                    Position = lightProbe.Position,
                    Coefficients = lightProbe.Coefficients
                });
            }

            // set permutations\region\meshes\markers
            mode.Regions = new List<RenderModel.Region>();
            foreach(var bmfRegion in Regions)
            {
                var newRegion = new RenderModel.Region
                {
                    Name = AddStringId(cache, bmfRegion.Name),
                    Permutations = new List<RenderModel.Region.Permutation>()
                };
                mode.Regions.Add(newRegion);
            }

            mode.Geometry = new RenderGeometry();

            mode.Geometry.Meshes = new List<Mesh>();

            foreach(var bmfMesh in Meshes)
            {
                Mesh newMesh = new Mesh();

                // add new permutation
                mode.Regions[bmfMesh.RegionIndex].Permutations.Append(new RenderModel.Region.Permutation
                {
                    Name = AddStringId(cache, bmfMesh.Name),
                    MeshCount = 1,
                    MeshIndex = (short)mode.Geometry.Meshes.Count
                });
            }

            return mode;
        }

        public bool InitGen3(GameCache cache, RenderModel mode)
        {
            Header = new BMFHeader();

            Name = cache.StringTable.GetString(mode.Name);

            Regions = new List<BMFRegion>();

            Dictionary<int, string> meshNames = new Dictionary<int, string>();
            Dictionary<int, int> meshRegionIndex = new Dictionary<int, int>();

            // build mesh index -> name mapping
            for (int i = 0; i < mode.Regions.Count; i++)
            {
                var region = mode.Regions[i];
                var regionName = cache.StringTable.GetString(region.Name);
                Regions.Add(new BMFRegion { Name = regionName });

                foreach (var permutation in region.Permutations)
                {
                    var permutationName = cache.StringTable.GetString(permutation.Name);

                    if (permutation.MeshCount > 1)
                    {
                        Console.WriteLine("Multi mesh per permutation not supported yet");
                        return false;
                    }

                    var name = $"{permutationName}";
                    if (!meshNames.ContainsKey(permutation.MeshIndex))
                        meshNames[permutation.MeshIndex] = name;
                    else
                    {
                        Console.WriteLine("Mesh is used twice for different permutations, not supported");
                        return false;
                    }

                    meshRegionIndex[permutation.MeshIndex] = i;
                }
            }


            // build markers
            Markers = new List<BMFMarkers>();
            foreach(var markerGroup in mode.MarkerGroups)
            {
                
                var groupName = cache.StringTable.GetString(markerGroup.Name);
                foreach(var marker in markerGroup.Markers)
                {
                    string name = "";
                    if (marker.RegionIndex == -1 || marker.PermutationIndex == -1)
                    {
                        name = $"#{groupName}";
                    }
                    else
                    {
                        var regionName = cache.StringTable.GetString(mode.Regions[marker.RegionIndex].Name);
                        var permutationName = cache.StringTable.GetString(mode.Regions[marker.RegionIndex].Permutations[marker.PermutationIndex].Name);
                        name = $"#{groupName}:{regionName}:{permutationName}";
                    }

                    Markers.Add(new BMFMarkers
                    {
                        Name = name,
                        NodeIndex = marker.NodeIndex,
                        Translation = marker.Translation,
                        Rotation = marker.Rotation,
                        Scale = marker.Scale
                    });
                }
            }

            // build list of nodes
            Nodes = new List<BMFNode>();
            foreach (var node in mode.Nodes)
            {
                var geometryNode = new BMFNode
                {
                    Name = cache.StringTable.GetString(node.Name),
                    ParentNodeIndex = node.ParentNode,
                    NextSiblingNodeIndex = node.NextSiblingNode,
                    FirstChildNodeIndex = node.FirstChildNode,
                    Translation = node.DefaultTranslation,
                    Rotation = node.DefaultRotation,
                    Scale = node.DefaultScale
                };
                Nodes.Add(geometryNode);
            }

            // build list of materials
            Materials = new List<BMFMaterial>();
            for (int i = 0; i < mode.Materials.Count; i++)
            {
                var material = mode.Materials[i];
                string name = $"material_{i}";
                if (material.RenderMethod.Name != null)
                    name = material.RenderMethod.Name.Split('\\').Last();

                Materials.Add(new BMFMaterial { Name = name });
            }

            // set SH coefficients
            DefaultLighting = new BMFGlobalLighting
            {
                SHRed = mode.SHRed,
                SHBlue = mode.SHBlue,
                SHGreen = mode.SHGreen
            };


            LightProbes = new List<BMFLightProbe>();

            foreach (var lightProbe in mode.UnknownSHProbes)
            {
                LightProbes.Add(new BMFLightProbe
                {
                    Position = lightProbe.Position,
                    Coefficients = lightProbe.Coefficients
                });
            }


            BoundingSpheres = mode.Geometry.BoundingSpheres;

            Meshes = new List<BMFMesh>();
            for (int i = 0; i < mode.Geometry.Meshes.Count; i++)
            {
                int vertexCount = 0;

                var mesh = mode.Geometry.Meshes[i];
                var geometryMesh = new BMFMesh()
                {
                    Name = meshNames[i],
                    Vertices = new List<BMFVertex>(),
                    Faces = new List<BMFFace>(),
                    PerVertexLighting = new List<BMFVertexLighting>(),
                    RegionIndex = meshRegionIndex[i],
                };

                //
                // Build vertices and faces
                //


                if (mesh.ResourceIndexBuffers[0] != null)
                {
                    var indexBuffer = mesh.ResourceIndexBuffers[0];
                    using (var indexDataStream = new MemoryStream(indexBuffer.Data.Data))
                    {
                        var indexStream = new IndexBufferStream(indexDataStream, cache.Endianness);

                        foreach (var part in mesh.Parts)
                        {
                            indexStream.Position = part.FirstIndexOld;
                            ushort[] indices = new ushort[0];
                            vertexCount += part.VertexCount;

                            switch (indexBuffer.Format)
                            {
                                case IndexBufferFormat.TriangleList:
                                    indices = indexStream.ReadIndices(part.IndexCountOld);
                                    break;
                                case IndexBufferFormat.TriangleStrip:
                                    indices = indexStream.ReadTriangleStrip(part.IndexCountOld);
                                    break;
                                default:
                                    throw new InvalidOperationException("Unsupported index buffer type: " + indexBuffer.Format);
                            }

                            // add faces
                            for (int j = 0; j < indices.Length; j += 3)
                            {
                                var face = new BMFFace
                                {
                                    MaterialIndex = part.MaterialIndex,
                                    Indices = new ushort[] { indices[j], indices[j + 1], indices[j + 2] }
                                };
                                geometryMesh.Faces.Add(face);
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("No index buffer to create faces!");
                }

                if (mesh.ResourceVertexBuffers[0] != null)
                {
                    var vertexBuffer = mesh.ResourceVertexBuffers[0];
                    var vertexCompressor = new VertexCompressor(mode.Geometry.Compression[0]);
                    geometryMesh.Vertices = ConvertGeometryVertices(vertexCompressor, vertexBuffer, cache.Version, vertexCount, mesh.RigidNodeIndex);
                }

                if (mesh.ResourceVertexBuffers[3] != null)
                {
                    var vertexBuffer = mesh.ResourceVertexBuffers[3];
                    geometryMesh.PerVertexLighting = ConvertLightingVertices(vertexBuffer, cache.Version, vertexCount);
                }

                Meshes.Add(geometryMesh);
            }

            return true;
        }

        private static StringId AddStringId(GameCache cache, string str)
        {
            var stringId = cache.StringTable.GetStringId(str);
            if (stringId == StringId.Invalid)
            {
                stringId = cache.StringTable.AddString(str);
                cache.SaveStrings();
            }
            return stringId;
        }

        private static List<BMFVertex> ConvertGeometryVertices(VertexCompressor vertexCompressor, VertexBufferDefinition vertexBuffer, CacheVersion version, int vertexCount, int rigidNodeIndex)
        {
            var vertices = new List<BMFVertex>();

            using (var vertexDataStream = new MemoryStream(vertexBuffer.Data.Data))
            {
                var vertexStream = VertexStreamFactory.Create(version, vertexDataStream);

                for (int j = 0; j < vertexCount; j++)
                {
                    var vertex = new BMFVertex();
                    RealVector2d texcoordTemp;

                    switch (vertexBuffer.Format)
                    {
                        case VertexBufferFormat.Rigid:
                            var rigid = vertexStream.ReadRigidVertex();

                            vertex.Position = vertexCompressor.DecompressPosition(rigid.Position).IJK;
                            texcoordTemp = vertexCompressor.DecompressUv(rigid.Texcoord);
                            vertex.Texcoord = new RealVector3d(texcoordTemp.I, texcoordTemp.J, 0.0f);
                            vertex.Normal = rigid.Normal;
                            vertex.Tangent = rigid.Tangent.IJK;
                            vertex.Binormal = rigid.Binormal;

                            vertex.Weights[0] = new BMFVertexWeight { NodeIndex = rigidNodeIndex, Weight = 1.0f };

                            for (int i = 1; i < 4; i++)
                            {
                                vertex.Weights[i] = new BMFVertexWeight();
                            }
                            
                            break;

                        case VertexBufferFormat.Skinned:
                            var skinned = vertexStream.ReadSkinnedVertex();

                            vertex.Position = vertexCompressor.DecompressPosition(skinned.Position).IJK;
                            texcoordTemp = vertexCompressor.DecompressUv(skinned.Texcoord);
                            vertex.Texcoord = new RealVector3d(texcoordTemp.I, texcoordTemp.J, 0.0f);
                            vertex.Normal = skinned.Normal;
                            vertex.Tangent = skinned.Tangent.IJK;
                            vertex.Binormal = skinned.Binormal;

                            for (int i = 0; i < 4; i++)
                            {
                                vertex.Weights[i] = new BMFVertexWeight { NodeIndex = skinned.BlendIndices[i], Weight = skinned.BlendWeights[i] };
                            }
                            break;
                        default:
                            throw new InvalidOperationException("Unsupported vertex buffer type: " + vertexBuffer.Format);
                    }
                    vertices.Add(vertex);
                }


            }
            return vertices;
        }

        private static List<BMFVertexLighting> ConvertLightingVertices(VertexBufferDefinition vertexBuffer, CacheVersion version, int vertexCount)
        {
            var vertices = new List<BMFVertexLighting>();

            using (var vertexDataStream = new MemoryStream(vertexBuffer.Data.Data))
            {
                var vertexStream = VertexStreamFactory.Create(version, vertexDataStream);

                for (int j = 0; j < vertexCount; j++)
                {
                    var vertex = new BMFVertexLighting();

                    switch (vertexBuffer.Format)
                    {
                        case VertexBufferFormat.AmbientPrt:
                            var ambientPrt = vertexStream.ReadAmbientPrtData();
                            vertex.SHOrder = 0;
                            vertex.PRTCoefficients[0] = ambientPrt.SHCoefficient;
                            break;

                        case VertexBufferFormat.LinearPrt:
                            var linearPrt = vertexStream.ReadLinearPrtData();
                            vertex.SHOrder = 1;
                            vertex.PRTCoefficients[0] = linearPrt.SHCoefficients.I;
                            vertex.PRTCoefficients[1] = linearPrt.SHCoefficients.J;
                            vertex.PRTCoefficients[2] = linearPrt.SHCoefficients.K;
                            vertex.PRTCoefficients[3] = linearPrt.SHCoefficients.W;
                            break;

                        case VertexBufferFormat.QuadraticPrt:
                            var quadPrt = vertexStream.ReadQuadraticPrtData();
                            vertex.SHOrder = 2;
                            vertex.PRTCoefficients[0] = quadPrt.SHCoefficients1.I;
                            vertex.PRTCoefficients[1] = quadPrt.SHCoefficients1.J;
                            vertex.PRTCoefficients[2] = quadPrt.SHCoefficients1.K;
                            vertex.PRTCoefficients[3] = quadPrt.SHCoefficients2.I;
                            vertex.PRTCoefficients[4] = quadPrt.SHCoefficients2.J;
                            vertex.PRTCoefficients[5] = quadPrt.SHCoefficients2.K;
                            vertex.PRTCoefficients[6] = quadPrt.SHCoefficients3.I;
                            vertex.PRTCoefficients[7] = quadPrt.SHCoefficients3.J;
                            vertex.PRTCoefficients[8] = quadPrt.SHCoefficients3.K;
                            break;
                        default:
                            throw new InvalidOperationException("Unsupported vertex buffer type: " + vertexBuffer.Format);
                    }
                    vertices.Add(vertex);
                }

                return vertices;
            }
        }

    }

    public enum BMFVersion : int
    {
        Prototype = 0
    }

    [Flags]
    public enum BMFFlags : int
    {
        None = 0,
    }

    [Flags]
    public enum BMFContentFlags : int
    {
        None = 0,
        Mesh = 1 << 0,
        Nodes = 1 << 1,
        Markers = 1 << 2,
        Materials = 1 << 3
    }

    [Flags]
    public enum BMFMeshDrawType : int
    {
        NotDrawn,
        OpaqueShadowOnly,
        OpaqueShadowCasting,
        OpaqueNonshadowing,
        Transparent,
        LightmapOnly
    }

    [TagStructure(Size = 0x8)]
    public class BMFFace : TagStructure
    {
        public short MaterialIndex;
        [TagField(Length = 3)]
        public ushort[] Indices;
    }

    [TagStructure(Size = 0x5C)]
    public class BMFVertex : TagStructure
    {
        public RealVector3d Position;
        public RealVector3d Texcoord;
        public RealVector3d Normal;
        public RealVector3d Tangent;
        public RealVector3d Binormal;

        [TagField(Length = 4)]
        public BMFVertexWeight[] Weights = new BMFVertexWeight[4];
    }

    [TagStructure(Size = 0x8)]
    public class BMFVertexWeight : TagStructure
    {
        public int NodeIndex;
        public float Weight;

        public BMFVertexWeight()
        {
            NodeIndex = 0;
            Weight = 0.0f;
        }
    }

    [TagStructure(Size = 0x28)]
    public class BMFVertexLighting : TagStructure
    {
        public int SHOrder;
        [TagField(Length = 9)]
        public float[] PRTCoefficients = new float[SphericalHarmonics.Order2Count];
    }

    [TagStructure(Size = 0xC0)]
    public class BMFGlobalLighting : TagStructure
    {
        [TagField(Length = 16)]
        public float[] SHRed = new float[SphericalHarmonics.Order3Count];
        [TagField(Length = 16)]
        public float[] SHGreen = new float[SphericalHarmonics.Order3Count];
        [TagField(Length = 16)]
        public float[] SHBlue = new float[SphericalHarmonics.Order3Count];
    }

    [TagStructure(Size = 0x44)] // TODO: write custom material plugin to set the draw type for each material
    public class BMFMaterial : TagStructure
    {
        [TagField(Length = Constants.StringLength, ForceNullTerminated = true)]
        public string Name;

        public Mesh.Part.PartTypeNew DrawType = Mesh.Part.PartTypeNew.OpaqueShadowCasting;

        [TagField(Flags = TagFieldFlags.Padding, Length = 3)]
        public byte[] Padding;
    }

    [TagStructure(Size = 0x40)]
    public class BMFRegion : TagStructure
    {
        [TagField(Length = Constants.StringLength, ForceNullTerminated = true)]
        public string Name;
    }

    [TagStructure(Size = 0x6C)]
    public class BMFNode : TagStructure
    {
        [TagField(Length = Constants.StringLength, ForceNullTerminated = true)]
        public string Name;

        public int ParentNodeIndex;
        public int FirstChildNodeIndex;
        public int NextSiblingNodeIndex;

        public RealPoint3d Translation;
        public RealQuaternion Rotation;
        public float Scale;
    }

    /// <summary>
    /// Name is structured as: marker_group:marker_name, marker groups are extracted then each marker is placed in the right group and given the right indices
    /// </summary>
    [TagStructure(Size = 0x64)]
    public class BMFMarkers : TagStructure
    {
        [TagField(Length = Constants.StringLength, ForceNullTerminated = true)]
        public string Name;

        public int NodeIndex;

        public RealPoint3d Translation;
        public RealQuaternion Rotation;
        public float Scale;
    }

    [TagStructure(Size = 0x68)]
    public class BMFMesh : TagStructure
    {
        [TagField(Length = Constants.StringLength, ForceNullTerminated = true)]
        public string Name;

        public int RegionIndex;

        public List<BMFVertex> Vertices;

        public List<BMFVertexLighting> PerVertexLighting;

        public List<BMFFace> Faces;
    }

    [TagStructure(Size = 0x150)]
    public class BMFLightProbe
    {
        public RealPoint3d Position;
        [TagField(Length = 81)]
        public float[] Coefficients = new float[81];
    }

}