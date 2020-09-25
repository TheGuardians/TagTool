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
    [TagStructure(Size = 0x38, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0x48, MinVersion = CacheVersion.Halo3ODST)]
    public class CollisionGeometryShape : HkpShapeCollection
    {
        [TagField(Length = 8, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public RealQuaternion AABB_Center;
        public RealQuaternion AABB_Half_Extents;
        [TagField(Flags = TagFieldFlags.Short)] 
        public CachedTag Model;
        public uint CollisionBspAddress; // runtime
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint LargeCollisionBspAddress; // runtime
        public sbyte BspIndex;
        public byte CollisionGeometryShapeType;
        public ushort CollisionGeometryShapeKey; // runtime
        public float Scale; // runtime
        [TagField(Flags = TagFieldFlags.Padding, Length = 0xC, MinVersion = CacheVersion.Halo3ODST)]
        public byte[] Padding2;
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
