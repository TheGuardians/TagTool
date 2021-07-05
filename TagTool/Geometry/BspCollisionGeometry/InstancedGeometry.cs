using TagTool.Cache;
using TagTool.Common;
using TagTool.Havok;
using TagTool.Tags;

namespace TagTool.Geometry.BspCollisionGeometry
{
    [TagStructure(Size = 0xB8, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0xC8, MinVersion = CacheVersion.HaloOnlineED)]
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

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public TagBlock<TagHkpMoppCode> UnknownBspPhysics;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public uint RuntimePointer;
    }
}
