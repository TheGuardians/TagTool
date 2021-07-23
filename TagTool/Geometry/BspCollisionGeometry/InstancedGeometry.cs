using TagTool.Cache;
using TagTool.Common;
using TagTool.Havok;
using TagTool.Tags;

namespace TagTool.Geometry.BspCollisionGeometry
{
    [TagStructure(Size = 0xC4, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
    [TagStructure(Size = 0xB8, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0xC8, MinVersion = CacheVersion.HaloOnlineED, Platform = CachePlatform.Original)]
    public class InstancedGeometryBlock : TagStructure
    {
        public int Checksum;
        public RealPoint3d BoundingSphereOffset;
        public float BoundingSphereRadius;
        public CollisionGeometry CollisionInfo;
        public TagBlock<CollisionGeometry> CollisionGeometries;
        public TagBlock<TagHkpMoppCode> CollisionMoppCodes;
        public TagBlock<BreakableSurfaceBits> BreakableSurfaces;
        public TagBlock<SurfacesPlanes> SurfacePlanes;
        public TagBlock<PlaneReference> Planes;
        public short MeshIndex;
        public short CompressionIndex;
        public float Unknown4;

        [TagField(Platform = CachePlatform.MCC)]
        public uint Unknown1;
        [TagField(Platform = CachePlatform.MCC)]
        public uint Unknown2;
        [TagField(Platform = CachePlatform.MCC)]
        public uint Unknown3;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public TagBlock<TagHkpMoppCode> UnknownBspPhysics;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public uint RuntimePointer;
    }
}
