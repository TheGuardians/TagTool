using System;
using TagTool.Tags;

namespace TagTool.Geometry.BspCollisionGeometry
{
    [TagStructure(Size = 0x4C)]
    public class CollisionKdHierarchyStatic : TagStructure
    {
        public int HashTotalCount;
        public TagBlock<CollisionKdHierarchyStaticHashTableDataBlock> HashData;
        public TagBlock<CollisionKdHierarchyStaticHashTableShortBlock> HashEntryCount;
        public TagBlock<CollisionKdHierarchyStaticHashTableShortBlock> OriginalHashEntryCount;
        public TagBlock<CollisionKdHierarchyStaticNodesBlock> Nodes;
        public TagBlock<CollisionKdHierarchyStaticInUseMasksBlock> InUseMasks;
        public TagBlock<ClusterTableBlock> ClusterTable;

        [TagStructure(Size = 0x10)]
        public class CollisionKdHierarchyStaticHashTableDataBlock : TagStructure
        {
            public int NodeIndex;
            public int KeyA;
            public int KeyB;
            public int KeyC;
        }

        [TagStructure(Size = 0x2)]
        public class CollisionKdHierarchyStaticHashTableShortBlock : TagStructure
        {
            public short Index;
        }

        [TagStructure(Size = 0x20)]
        public class CollisionKdHierarchyStaticNodesBlock : TagStructure
        {
            public TagBlock<CollisionKdHierarchyStaticHashTableHeadersBlock> RenderOnlyHeaders;
            public TagBlock<CollisionKdHierarchyStaticHashTableHeadersBlock> CollidableHeaders;
            public short ChildBelow;
            public short ChildAbove;
            public short Parent;
            public short ClusterIndex;

            [TagStructure(Size = 0x10)]
            public class CollisionKdHierarchyStaticHashTableHeadersBlock : TagStructure
            {
                public CollisionKdHierarchyStaticHashTableCullFlags CullFlags;
                public short InstanceIndex;
                public int InstanceIndexDwordMask;
                public short BspIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public int BspMask;

                [Flags]
                public enum CollisionKdHierarchyStaticHashTableCullFlags : ushort
                {
                    RenderOnly = 1 << 0,
                    DoesNotBlockAoe = 1 << 1,
                    NonPathfindable = 1 << 2
                }
            }
        }

        [TagStructure(Size = 0x4)]
        public class CollisionKdHierarchyStaticInUseMasksBlock : TagStructure
        {
            public int Mask;
        }

        [TagStructure(Size = 0xC)]
        public class ClusterTableBlock : TagStructure
        {
            public TagBlock<SuperNodeMappingsBlock> SuperNodeMappings;

            [TagStructure(Size = 0x40)]
            public class SuperNodeMappingsBlock : TagStructure
            {
                [TagField(Length = 31)]
                public SuperNodeMappingIndexArray[] Indices;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;

                [TagStructure(Size = 0x2)]
                public class SuperNodeMappingIndexArray : TagStructure
                {
                    public short Index;
                }
            }
        }
    }
}
