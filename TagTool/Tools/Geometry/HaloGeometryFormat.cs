using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Geometry;

namespace TagTool.Tools.Geometry
{

    /// <summary>
    /// Generic render geometry class for interfacing with other tools. Based on gen3 formats but should support any halo engine. Names\file extension temporary
    /// </summary>
    [TagStructure(Size = 0x10)]
    public class HaloGeometryFormatHeader
    {
        public Tag Signature = new Tag("hgf!"); // hgf! : halo geometry format!
        public GeometryFormatVersion Version;
        public GeometryFormatFlags Flags;
        public GeometryContentFlags ContentFlags;
    }

    public static class HaloGeometryConstants
    {
        public const int StringLength = 0x80;
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
    [TagStructure(Size = 0xB4)]
    public class HaloGeometryFormat : HaloGeometryFormatHeader
    {
        [TagField(Length = HaloGeometryConstants.StringLength)]
        public string Name;

        public List<GeometryMaterial> Materials;
        public List<GeometryNode> Nodes;
        public List<GeometryMesh> Meshes;
        public SHLightingOrder3 DefaultLighting;
        public List<RenderGeometry.BoundingSphere> BoundingSpheres;
    }

    [TagStructure(Size = 0xC0)]
    public class SHLightingOrder3
    {
        [TagField(Length = 16)]
        public float[] SHRed = new float[SphericalHarmonics.Order3Count];
        [TagField(Length = 16)]
        public float[] SHGreen = new float[SphericalHarmonics.Order3Count];
        [TagField(Length = 16)]
        public float[] SHBlue = new float[SphericalHarmonics.Order3Count];
    }

    [TagStructure(Size = 0x80)]
    public class GeometryMaterial
    {
        [TagField(Length = HaloGeometryConstants.StringLength)]
        public string Name;
    }

    [TagStructure(Size = 0xA8)]
    public class GeometryNode
    {
        [TagField(Length = HaloGeometryConstants.StringLength)]
        public string Name;

        public short ParentNode;
        public short FirstChildNode;
        public short NextSiblingNode;
        public short Unused;

        public RealPoint3d Translation;
        public RealQuaternion Rotation;
        public float Scale;
    }

    [TagStructure(Size = 0xA4)]
    public class GeometryMarker
    {
        [TagField(Length = HaloGeometryConstants.StringLength)]
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
    public class GeometryMesh
    {
        [TagField(Length = HaloGeometryConstants.StringLength)]
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
}
