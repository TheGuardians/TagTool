using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Havok;
using TagTool.Tags;

namespace TagTool.Geometry.BspCollisionGeometry
{
    [TagStructure(Size = 0x38, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x48, MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x40, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    public class CollisionGeometryShape : HkpShapeCollection
    {
        [TagField(Align = 16)]
        public RealQuaternion AABB_Center;
        public RealQuaternion AABB_Half_Extents;
        [TagField(Flags = TagFieldFlags.Short)] 
        public CachedTag Model;
        public PlatformUnsignedValue CollisionBspAddress; // runtime
        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
        public PlatformUnsignedValue LargeCollisionBspAddress; // runtime
        public sbyte BspIndex;
        public byte CollisionGeometryShapeType;
        public ushort CollisionGeometryShapeKey; // runtime
        public float Scale; // runtime
    }

    [TagStructure(Size = 0x28, MaxVersion = CacheVersion.Halo2Vista)]
    public class CollisionGeometryShapeGen2 : HkpShape
    {
        public uint CollisionBspAddress; // runtime
        public RealQuaternion AABB_Center;
        public RealQuaternion AABB_Half_Extents;
        [TagField(Flags = TagFieldFlags.Short)]
        public CachedTag Model;
    }
}
