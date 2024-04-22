using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Havok;
using TagTool.Pathfinding;

namespace TagTool.Tags.Resources.Gen4
{
    [TagStructure(Name = "structure_bsp_cache_file_tag_resources", Size = 0xAC, Platform = CachePlatform.Original)]
    [TagStructure(Name = "structure_bsp_cache_file_tag_resources", Size = 0xB0, Platform = CachePlatform.MCC)]
    public class StructureBspCacheFileTagResources : TagStructure
	{
        public TagBlock<StructureSurface> SurfacePlanes;
        public TagBlock<StructureSurfaceToTriangleMapping> Planes;
        public TagBlock<EdgeToSeamMapping> EdgeToSeams;
        public TagBlock<PathfindingDataBlock> PathfindingData;

        public TagBlock<StructureBspInstancedGeometryInstancesBlock> InstancedGeometryInstances;
        public TagBlock<SuperNodeParentMappingBlock> ParentSuperNodeMapping;
        public TagBlock<short> ParentSuperNodRecursableMasks;
        public TagBlock<SuperNodeTraversalGeometryBlock> SuperNodeTraversalGeometry;
        public CollisionKdHierarchyStatic CollisionKdHierarchy;

        [TagStructure(Size = 0x94)]
        public class StructureBspInstancedGeometryInstancesBlock : TagStructure
        {
            public float Scale;
            public RealMatrix4x3 Matrix;
            public short InstanceDefinition;
            public InstancedGeometryFlags Flags;
            public ChanneldefinitionFlags LightChannels;
            public short MeshIndex;
            public short CompressionIndex;
            public int SeamBitVector0;
            public int SeamBitVector1;
            public int SeamBitVector2;
            public int SeamBitVector3;
            public float BoundsX0;
            public float BoundsX1;
            public float BoundsY0;
            public float BoundsY1;
            public float BoundsZ0;
            public float BoundsZ1;
            public RealPoint3d WorldBoundingSphereCenter;
            public float WorldBoundingSphereRadius;
            public float ImposterTransitionCompleteDistance;
            public float ImposterBrightness;
            public int Checksum;
            public InstancedGeometryPathfindingPolicyEnum PathfindingPolicy;
            public InstancedGeometryLightmappingPolicyEnum LightmappingPolicy;
            public InstancedGeometryImposterPolicyEnum ImposterPolicy;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public InstancedGeometryStreamingpriorityEnum StreamingPriority;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public short Cubemap0BitmapIndex;
            public float LightmapResolutionScale;
            public short GroupIndex;
            public short GroupListIndex;

            [Flags]
            public enum InstancedGeometryFlags : ushort
            {
                NotInLightprobes = 1 << 0,
                RenderOnly = 1 << 1,
                DoesNotBlockAoeDamage = 1 << 2,
                Collidable = 1 << 3,
                DecalSpacing = 1 << 4,
                RainBlocker = 1 << 5,
                VerticalRainSheet = 1 << 6,
                OutsideMap = 1 << 7,
                SeamColliding = 1 << 8,
                Unknown = 1 << 9,
                RemoveFromShadowGeometry = 1 << 10,
                CinemaOnly = 1 << 11,
                ExcludeFromCinema = 1 << 12,
                DisallowObjectLightingSamples = 1 << 13
            }

            [Flags]
            public enum ChanneldefinitionFlags : uint
            {
                _0 = 1 << 0,
                _1 = 1 << 1,
                _2 = 1 << 2,
                _3 = 1 << 3,
                _4 = 1 << 4,
                _5 = 1 << 5,
                _6 = 1 << 6,
                _7 = 1 << 7,
                _8 = 1 << 8,
                _9 = 1 << 9,
                _10 = 1 << 10,
                _11 = 1 << 11,
                _12 = 1 << 12,
                _13 = 1 << 13,
                _14 = 1 << 14,
                _15 = 1 << 15,
                _16 = 1 << 16,
                _17 = 1 << 17,
                _18 = 1 << 18,
                _19 = 1 << 19,
                _20 = 1 << 20,
                _21 = 1 << 21,
                _22 = 1 << 22,
                _23 = 1 << 23,
                _24 = 1 << 24,
                _25 = 1 << 25,
                _26 = 1 << 26,
                _27 = 1 << 27,
                _28 = 1 << 28,
                _29 = 1 << 29,
                _30 = 1 << 30,
                _31 = 1u << 31
            }

            public enum InstancedGeometryPathfindingPolicyEnum : sbyte
            {
                CutOut,
                Static,
                None
            }

            public enum InstancedGeometryLightmappingPolicyEnum : sbyte
            {
                PerPixelShared,
                PerVertex,
                SingleProbe,
                Exclude,
                PerPixelAo,
                PerVertexAo
            }

            public enum InstancedGeometryImposterPolicyEnum : sbyte
            {
                PolygonDefault,
                PolygonHigh,
                CardsMedium,
                CardsHigh,
                None,
                RainbowBox
            }

            public enum InstancedGeometryStreamingpriorityEnum : sbyte
            {
                Default,
                Higher,
                Highest
            }
        }

        [TagStructure(Size = 0x50)]
        public class PathfindingDataBlock : TagStructure
        {
            public int RuntimenavMesh;
            public int RuntimenavGraph;
            public int RuntimenavMediator;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public byte[] NavgraphData;
            public byte[] NavmediatorData;
            public List<FaceUserDataBlock> FaceuserData;
            public int StructureChecksum;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;

            [TagStructure(Size = 0xC)]
            public class FaceUserDataBlock : TagStructure
            {
                public short MFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float CurrentminPathDistance;
                public float CurrentminTargetApproachDistance;
            }
        }

        [TagStructure(Size = 0x14, MinVersion = CacheVersion.HaloReach)]
        public class SuperNodeParentMappingBlock : TagStructure
        {
            public short ParentSuperNodeIndex;
            public sbyte ParentInternalNodeIndex;
            public FlagsValue Flags;
            public int HasTraversalGeometryMask;
            public short FirstTraversalGeometry_index;
            public short FirstAabbIndex;
            public int AabbMask;
            public short NonTerminalTraversalGeometryIndex;
            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;

            [Flags]
            public enum FlagsValue : byte
            {
                Above = 1 << 0
            }
        }

        [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloReach)]
        public class SuperNodeTraversalGeometryBlock : TagStructure
        {
            public TagBlock<short> PortalIndices;
            public TagBlock<short> SeamIndices;
        }
    }
}