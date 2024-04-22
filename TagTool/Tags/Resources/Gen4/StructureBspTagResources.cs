using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Havok;
using TagTool.Tags.Definitions.Gen4;

namespace TagTool.Tags.Resources.Gen4
{
    [TagStructure(Name = "structure_bsp_tag_resources", Size = 0x30)]
    public class StructureBspTagResources : TagStructure
    {
        public TagBlock<CollisionGeometry> CollisionBsps;
        public TagBlock<LargeCollisionBspBlock> LargeCollisionBsps;
        public TagBlock<InstancedGeometryDefinition> InstancedGeometry;
        public TagBlock<StructureiohavokDataBlockStruct> HavokData;

        [TagStructure(Size = 0x14)]
        public class InstancedGeometryDefinition : TagStructure
        {
            public int Checksum;
            public InstancedGeometryDefinitionFlags Flags;
            public short MeshIndex;
            public short CompressionIndex;
            public float GlobalLightmapResolutionScale;
            public short ExternalIndex;
            [TagField(Flags = TagFieldFlags.Padding, Length = 0x2)]
            public byte[] Pad = new byte[2];

            [Flags]
            public enum InstancedGeometryDefinitionFlags : uint
            {
                MiscoloredBsp = 1 << 0,
                ErrorFree = 1 << 1,
                SurfaceToTriangleRemapped = 1 << 2,
                NoPhysics = 1 << 3,
                StitchedPhysics = 1 << 4
            }
        }

        [TagStructure(Size = 0x48)]
        public class StructureiohavokDataBlockStruct : TagStructure
        {
            public int Version;
            public int RuntimeDeserializedBodyPointer;
            public int RuntimeDeserializedDataPointer;
            public int PrefabIndex;
            public byte[] SerializedHavokData;
            public List<SerializedHavokGeometryDataBlockStruct> SerializedPerCollisionTypeHavokGeometry;
            public RealPoint3d ShapesBoundsMin;
            public RealPoint3d ShapesBoundsMax;

            [TagStructure(Size = 0x34)]
            public class SerializedHavokGeometryDataBlockStruct : TagStructure
            {
                public byte[] SerializedHavokData;
                public byte[] SerializedStaticHavokData;
                public int CollisionType;
                public int RuntimeDeserializedBodyPointer;
                public int RuntimeDeserializedDataPointer;
            }
        }
    }
}