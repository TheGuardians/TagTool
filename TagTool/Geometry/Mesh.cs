using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.Geometry
{
    /// <summary>
    /// A 3D mesh which can be rendered.
    /// </summary>
    [TagStructure(Size = 0x4C)]
    public class Mesh
    {
        public List<Part> Parts;
        public List<SubPart> SubParts;
        [TagField(Length = 8)] public ushort[] VertexBuffers;
        [TagField(Length = 2)] public ushort[] IndexBuffers;
        public MeshFlags Flags;
        public sbyte RigidNodeIndex;
        public VertexType Type;
        public PrtType PrtType;
        public PrimitiveType IndexBufferType;

        [TagField(Padding = true, Length = 3)]
        public byte[] Unused;

        public List<InstancedGeometryBlock> InstancedGeometry;
        public List<WaterBlock> Water;

        /// <summary>
        /// Associates geometry with a specific material.
        /// </summary>
        [TagStructure(Size = 0x10)]
        public class Part
        {
            /// <summary>
            /// The block index of the material of the mesh part.
            /// </summary>
            public short MaterialIndex;

            /// <summary>
            /// The transparent sorting index of the mesh part.
            /// </summary>
            public short TransparentSortingIndex;

            /// <summary>
            /// The index of the first vertex in the index buffer.
            /// </summary>
            public ushort FirstIndex;

            /// <summary>
            /// The number of indices in the part.
            /// </summary>
            public ushort IndexCount;

            /// <summary>
            /// The index of the first subpart that makes up this part.
            /// </summary>
            public short FirstSubPartIndex;

            /// <summary>
            /// The number of subparts that make up this part.
            /// </summary>
            public short SubPartCount;
            
            /// <summary>
            /// The type of the mesh part.
            /// </summary>
            public sbyte Type;

            /// <summary>
            /// The flags of the mesh part.
            /// </summary>
            public PartFlags Flags;

            /// <summary>
            /// The number of vertices that the mesh part uses.
            /// </summary>
            public ushort VertexCount;

            /// <summary>
            /// WARNING: EXPERIMENTAL H5F VALUES!!!
            /// </summary>
            [Flags]
            public enum PartFlags : byte
            {
                None = 0,
                IsWaterSurface = 1 << 0,
                PerVertexLightmapPart = 1 << 1,
                RenderInZPrepass = 1 << 2,
                CanBeRenderedInDrawBundles = 1 << 3,
                DrawCullDistanceMedium = 1 << 4,
                DrawCullDistanceClose = 1 << 5,
                DrawCullRenderingShields = 1 << 6,
                DrawCullRenderingActiveCamo = 1 << 7
            }
        }

        /// <summary>
        /// A subpart of a mesh which can be rendered selectively.
        /// </summary>
        [TagStructure(Size = 0x8)]
        public class SubPart
        {
            /// <summary>
            /// The index of the first vertex in the subpart.
            /// </summary>
            public ushort FirstIndex;

            /// <summary>
            /// The number of indices in the subpart.
            /// </summary>
            public ushort IndexCount;

            /// <summary>
            /// The index of the part which this subpart belongs to.
            /// </summary>
            public short PartIndex;

            /// <summary>
            /// The number of vertices that the part uses.
            /// </summary>
            /// <remarks>
            /// Note that this actually seems to be unused. The value is pulled from
            /// the vertex buffer definition instead.
            /// </remarks>
            public ushort VertexCount;
        }

        [TagStructure(Size = 0x10)]
        public class InstancedGeometryBlock
        {
            public short Section1;
            public short Section2;
            public List<ContentsBlock> Contents;

            [TagStructure(Size = 0x2)]
            public struct ContentsBlock
            {
                public short Value;
            }
        }

        [TagStructure(Size = 0x2)]
        public struct WaterBlock
        {
            public short Value;
        }
    }
}
