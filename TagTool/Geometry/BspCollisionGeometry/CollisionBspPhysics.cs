using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Havok;
using TagTool.Tags;

namespace TagTool.Geometry.BspCollisionGeometry
{
    [TagStructure(Size = 0x70, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0x80, MinVersion = CacheVersion.Halo3ODST)]
    public class CollisionBspPhysicsDefinition : TagStructure
    {
        public CollisionGeometryShape GeometryShape;
        public CMoppBvTreeShape MoppBvTreeShape;
    }

    [TagStructure(Size = 0x70, MaxVersion = CacheVersion.Halo2Vista)]
    public class CollisionBspPhysicsDefinitionGen2 : TagStructure
    {
        public CollisionGeometryShapeGen2 GeometryShape;
        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public Havok.Gen2.CConvexWelderShape WelderShape;
        public Havok.Gen2.CMoppBvTreeShape BvTreeShape;
        public List<byte> MoppCodes;
        [TagField(Length = 4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
    }
}
