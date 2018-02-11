using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Geometry
{
    [TagStructure(Name = "render_geometry", Size = 0x84)]
    public class RenderGeometry
    {
        /// <summary>
        /// The runtime flags of the render geometry.
        /// </summary>
        public RenderGeometryRuntimeFlags RuntimeFlags;

        /// <summary>
        /// The meshes of the render geometry.
        /// </summary>
        public List<Mesh> Meshes;

        /// <summary>
        /// The compression information of the render geometry.
        /// </summary>
        public List<RenderGeometryCompression> Compression;

        /// <summary>
        /// The bounding spheres of the render geometry.
        /// </summary>
        public List<BoundingSphere> BoundingSpheres;

        public List<UnknownBlock> Unknown2;

        public uint Unknown3;
        public uint Unknown4;
        public uint Unknown5;

        public List<UnknownSection> UnknownSections;

        /// <summary>
        /// The per-mesh node mappings of the render geometry.
        /// </summary>
        public List<PerMeshNodeMap> PerMeshNodeMaps;

        /// <summary>
        /// The per-mesh subpart visibility of the render geometry.
        /// </summary>
        public List<PerMeshSubpartVisibilityBlock> PerMeshSubpartVisibility;

        public uint Unknown7;
        public uint Unknown8;
        public uint Unknown9;

        /// <summary>
        /// The per-mesh level-of-detail data of the render geometry.
        /// </summary>
        public List<PerMeshLodDatum> PerMeshLodData;
        
        /// <summary>
        /// The resource containing the raw geometry data.
        /// </summary>
        [TagField(Pointer = true, MinVersion = CacheVersion.HaloOnline106708)]
        public PageableResource Resource;

        /// <summary>
        /// The index of the resource entry in the cache_file_resource_gestalt tag.
        /// </summary>
        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public int ZoneAssetHandle;
        
        [TagField(Padding = true, Length = 4)]
        public byte[] Unused;

        [TagStructure(Size = 0x30)]
        public class BoundingSphere
        {
            public RealPlane3d Plane;
            public RealPoint3d Position;
            public float Radius;

            [TagField(Length = 4)]
            public sbyte[] NodeIndices;

            [TagField(Length = 3)]
            public float[] NodeWeights;
        }

        [TagStructure(Size = 0x18)]
        public class UnknownBlock
        {
            public byte UnknownByte1;
            public byte UnknownByte2;
            public short Unknown2;
            public byte[] Unknown3;
        }

        [TagStructure(Size = 0x20)]
        public class UnknownSection
        {
            [TagField(Align = 0x10)]
            public byte[] Unknown;

            public List<UnknownBlock> Unknown2;

            [TagStructure(Size = 0x2)]
            public struct UnknownBlock
            {
                public short Unknown;
            }
        }

        [TagStructure(Size = 0xC)]
        public struct PerMeshNodeMap
        {
            public List<NodeIndex> NodeIndices;

            [TagStructure(Size = 0x1)]
            public struct NodeIndex
            {
                public byte Node;
            }
        }

        [TagStructure(Size = 0xC)]
        public class PerMeshSubpartVisibilityBlock
        {
            public List<BoundingSphere> BoundingSpheres;
        }

        [TagStructure(Size = 0x10)]
        public class PerMeshLodDatum
        {
            public List<Index> Indices;

            public short VertexBufferIndex;

            [TagField(Padding = true, Length = 2)]
            public byte[] Unused;

            [TagStructure(Size = 0x4)]
            public struct Index
            {
                public int Value;
            }
        }
    }
}