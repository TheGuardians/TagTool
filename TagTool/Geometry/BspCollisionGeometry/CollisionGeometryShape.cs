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
    [TagStructure(Size = 0x30, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0x40, MinVersion = CacheVersion.Halo3ODST)]
    public class CollisionGeometryShape : HkpShapeCollection
    {
        public RealQuaternion AABB_Min;
        public RealQuaternion AABB_Max;
        [TagField(Flags = TagFieldFlags.Short)]
        public CachedTag Model;
        public uint CollisionBspAddress; // runtime
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint LargeCollisionBspAddress; // runtime
        public byte Unknown1;
        public byte CollisionGeometryShapeType;
        public ushort CollisionGeometryShapeKey; // runtime
        public uint RuntimeData1; // runtime

        [TagField(Flags = TagFieldFlags.Padding, Length = 0xC, MinVersion = CacheVersion.Halo3ODST)]
        public byte[] Padding2;
    }
}
