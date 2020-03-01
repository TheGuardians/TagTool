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
    /// <summary>
    /// Generic render geometry class for interfacing with other tools. Based on gen3 formats but should support any halo engine. Names\file extension temporary
    /// </summary>
    [TagStructure(Size = 0x14)]
    public class HaloGeometryFormatHeader : TagStructure
    {
        public Tag Signature = new Tag("hgf!"); // hgf! : halo geometry format!
        public GeometryFormatVersion Version;
        public GeometryFormatFlags Flags;
        public GeometryContentFlags ContentFlags;
        public int Offset;
    }

    [TagStructure(Size = 0x40)]
    public static class HaloGeometryConstants
    {
        public const int StringLength = 0x40;
    }

    public enum GeometryFormatVersion : int
    {
        Prototype = 0
    }

    [Flags]
    public enum GeometryFormatFlags : int
    {
        None = 0,
    }

    [Flags]
    public enum GeometryContentFlags : int
    {
        None = 0,
        Mesh = 1 << 0,
        Nodes = 1 << 1,
        Markers = 1 << 2,
        Materials = 1 << 3,
         
    }

    /// <summary>
    /// TODO: parsing code to handle permutation/regions, per face material, marker groups/markers, vertex/index buffer handling
    /// </summary>
    [TagStructure(Size = 0x13C)]
    public class HaloGeometryFormat : TagStructure
    {
        [TagField(Length = HaloGeometryConstants.StringLength, ForceNullTerminated =  true)]
        public string Name = "default";

        public List<GeometryMaterial> Materials;
        public List<GeometryNode> Nodes;
        public List<GeometryMesh> Meshes;
        public SHLightingOrder3 DefaultLighting;
        public List<GeometryLightProbes> LightProbes;
        public List<RenderGeometry.BoundingSphere> BoundingSpheres;


        public static void SerializeToFile(EndianWriter writer, HaloGeometryFormatHeader header, HaloGeometryFormat format)
        {
            var context = new DataSerializationContext(writer);
            var serializer = new TagSerializer(CacheVersion.HaloOnline106708);

            writer.BaseStream.Position = 0x14;
            context.PointerOffset = 0x14;
            serializer.Serialize(context, format);
            header.Offset = (int)context.MainStructOffset + 0x14;
            writer.BaseStream.Position = 0x0;
            serializer.Serialize(context, header);
        }

        public RenderModel GetGen3RenderModel(GameCache cache)
        {
            RenderModel mode = new RenderModel();

            mode.Name = AddStringId(cache, Name);

            // set materials
            mode.Materials = new List<RenderMaterial>();
            for(int i = 0; i < Materials.Count; i++)
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
                    ParentNode = node.ParentNode,
                    FirstChildNode = node.FirstChildNode,
                    NextSiblingNode = node.NextSiblingNode,
                    Flags = RenderModel.NodeFlags.None,
                    DefaultTranslation = node.Translation,
                    DefaultRotation = node.Rotation,
                    DefaultScale = node.Scale,

                    InverseForward = inverseForward,
                    InverseLeft = inverseLeft,
                    InverseUp = inverseUp,
                    InversePosition = -1 * node.Translation,

                    DistanceFromParent =  i == 0 ? 0.0f : RealPoint3d.Distance(node.Translation - Nodes[node.ParentNode].Translation)
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
            foreach(var lightProbe in LightProbes)
            {
                mode.UnknownSHProbes.Add(new RenderModel.UnknownSHProbe
                {
                    Position = lightProbe.Position,
                    Coefficients = lightProbe.Coefficients
                });
            }

            // set permutations\region\meshes\markers



            return mode;
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

        public bool InitGen3(GameCache cache, RenderModel mode)
        {
            Name = cache.StringTable.GetString(mode.Name);

            Dictionary<int, string> meshNames = new Dictionary<int, string>();

            // build mesh index -> name mapping
            foreach (var region in mode.Regions)
            {
                var regionName = cache.StringTable.GetString(region.Name);
                foreach (var permutation in region.Permutations)
                {
                    var permutationName = cache.StringTable.GetString(permutation.Name);

                    if (permutation.MeshCount > 1)
                    {
                        Console.WriteLine("Multi mesh per permutation not supported yet");
                        return false;
                    }

                    var name = $"{regionName}:{permutationName}";
                    if (!meshNames.ContainsKey(permutation.MeshIndex))
                        meshNames[permutation.MeshIndex] = name;
                    else
                    {
                        Console.WriteLine("Mesh is used twice for different permutations, not supported");
                        return false;
                    }
                }
            }
            
            // build list of nodes
            Nodes = new List<GeometryNode>();
            foreach (var node in mode.Nodes)
            {
                var geometryNode = new GeometryNode
                {
                    Name = cache.StringTable.GetString(node.Name),
                    ParentNode = node.ParentNode,
                    NextSiblingNode = node.NextSiblingNode,
                    FirstChildNode = node.FirstChildNode,
                    Unused = 0,
                    Translation = node.DefaultTranslation,
                    Rotation = node.DefaultRotation,
                    Scale = node.DefaultScale
                };
                Nodes.Add(geometryNode);
            }
            
            // build list of materials
            Materials = new List<GeometryMaterial>();
            for (int i = 0; i < mode.Materials.Count; i++)
            {
                var material = mode.Materials[i];
                string name = $"material_{i}";
                if (material.RenderMethod.Name != null)
                    name = material.RenderMethod.Name.Split('\\').Last();

                Materials.Add(new GeometryMaterial { Name = name });

            }

            // set SH coefficients
            DefaultLighting = new SHLightingOrder3
            {
                SHRed = mode.SHRed,
                SHBlue = mode.SHBlue,
                SHGreen = mode.SHGreen
            };

            
            LightProbes = new List<GeometryLightProbes>();

            foreach (var lightProbe in mode.UnknownSHProbes)
            {
                LightProbes.Add(new GeometryLightProbes
                {
                    Position = lightProbe.Position,
                    Coefficients = lightProbe.Coefficients
                });
            }

            
            BoundingSpheres = mode.Geometry.BoundingSpheres;
            
            

            return true;
        }
    }

    [TagStructure(Size = 0xC0)]
    public class SHLightingOrder3 : TagStructure
    {
        [TagField(Length = 16)]
        public float[] SHRed = new float[SphericalHarmonics.Order3Count];
        [TagField(Length = 16)]
        public float[] SHGreen = new float[SphericalHarmonics.Order3Count];
        [TagField(Length = 16)]
        public float[] SHBlue = new float[SphericalHarmonics.Order3Count];
    }

    [TagStructure(Size = 0x40)]
    public class GeometryMaterial : TagStructure
    {
        [TagField(Length = HaloGeometryConstants.StringLength, ForceNullTerminated = true)]
        public string Name;
    }

    [TagStructure(Size = 0x68)]
    public class GeometryNode : TagStructure
    {
        [TagField(Length = HaloGeometryConstants.StringLength, ForceNullTerminated = true)]
        public string Name;

        public short ParentNode;
        public short FirstChildNode;
        public short NextSiblingNode;
        public short Unused;

        public RealPoint3d Translation;
        public RealQuaternion Rotation;
        public float Scale;
    }

    [TagStructure(Size = 0x64)]
    public class GeometryMarker : TagStructure
    {
        [TagField(Length = HaloGeometryConstants.StringLength, ForceNullTerminated = true)]
        public string Name;

        public sbyte RegionIndex;
        public sbyte PermutationIndex;
        public sbyte NodeIndex;
        public sbyte Unknown3;
        public RealPoint3d Translation;
        public RealQuaternion Rotation;
        public float Scale;
    }

    [TagStructure(Size = 0x140)]
    public class GeometryMesh : TagStructure
    {
        [TagField(Length = HaloGeometryConstants.StringLength, ForceNullTerminated = true)]
        public string Name;

        public sbyte RigidNodeIndex; // when type == rigid this is the node that the mesh is attached to
        public VertexType Type;
        public PrtSHType PrtType;
        public PrimitiveType IndexBufferType;

        public List<GeometryMarker> Markers;

        
        public GeometryVertexBuffer MeshVertexBuffer;
        public GeometryVertexBuffer BspMeshVertexBuffer;
        public GeometryVertexBuffer PRTVertexBuffer;
        public GeometryVertexBuffer WaterMeshVertexBuffer;
        public GeometryVertexBuffer WaterWeightsVertexBuffer;

        public GeometryIndexBuffer IndexBuffer1;
        public GeometryIndexBuffer IndexBuffer2;
    }

    [TagStructure(Size = 0x18)]
    public class GeometryVertexBuffer
    {
        public VertexBufferFormat Format;
        public short VertexSize;
        public byte[] Data;
    }

    [TagStructure(Size = 0x1C)]
    public class GeometryIndexBuffer
    {
        public IndexBufferFormat Format;
        public int FaceCount;
        public byte[] Data; // custom index buffer with material index for each face perhaps
    }


    [TagStructure(Size = 0x150)]
    public class GeometryLightProbes
    {
        public RealPoint3d Position;
        [TagField(Length = 81)]
        public float[] Coefficients = new float[81];
    }
}
