using System;
using TagTool.Cache;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Havok;
using TagTool.Pathfinding;

namespace TagTool.Tags.Resources
{
    [TagStructure(Name = "structure_bsp_cache_file_tag_resources", Size = 0x30, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "structure_bsp_cache_file_tag_resources", Size = 0xC4, MinVersion = CacheVersion.HaloReach)]
    public class StructureBspCacheFileTagResources : TagStructure
	{
        public TagBlock<SurfacesPlanes> SurfacePlanes;
        public TagBlock<PlaneReference> Planes;
        public TagBlock<EdgeToSeamMapping> EdgeToSeams;
        public TagBlock<ResourcePathfinding> PathfindingData;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagBlock<TagHkpMoppCode> ClusterMoppCode;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagBlock<TagHkpMoppCode> InstanceGroupMoppCode;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagBlock<InstancedGeometryInstanceReach> InstancedGeometryInstances;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagBlock<SuperNodeParentMappingBlock> ParentSuperNodeMapping;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagBlock<short> ParentSuperNodRecursableMasks;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagBlock<SuperNodeTraversalGeometryBlock> SuperNodeTraversalGeometry;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public CollisionKdHierarchyStatic CollisionKdHierarchy;

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