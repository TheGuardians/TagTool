using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Havok;
using TagTool.Tags;

namespace TagTool.Geometry.BspCollisionGeometry
{
    [TagStructure(Size = 0xB0, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    [TagStructure(Size = 0x70, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x80, MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    public class CollisionBspPhysicsDefinition : TagStructure
    {
        public CollisionGeometryShape GeometryShape;
        [TagField(Align = 16)]
        public CMoppBvTreeShape MoppBvTreeShape;
    }

    [TagStructure(Size = 0x70, Align = 16)]
    public class CollisionBspPhysicsReach : TagStructure
    {
        public MoppBvTreeShapeStruct MoppBvTreeShape;
        public TagBlock<CollisionGeometryShape> GeometryShape;
        public TagBlock<DecomposedPoopShape> PoopShape;
        [TagField(Length = 8, Flags = TagFieldFlags.Padding)]
        public byte Padding1;

        [TagStructure(Size = 0x50)]
        public class MoppBvTreeShapeStruct : TagStructure
        {
            public HavokShapeStruct20102 MoppBvTreeShape;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public int MoppCodePointer;
            public int MoppDataSkip;
            public int MoppDataSize;
            public RealVector3d CodeInfoCopy;
            public float HavokWCodeInfoCopy;
            public int ChildShapeVtable;
            public int ChildShapePointer;
            public int ChildSize;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public float MoppScale;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;

            [TagStructure(Size = 0x10)]
            public class HavokShapeStruct20102 : TagStructure
            {
                public int FieldPointerSkip;
                public short Size;
                public short Count;
                public int UserData;
                public int Type;
            }
        }

        // A collection of scaled hkpConvexVerticesShape (polyhedra in instanced geometry definition)
        [TagStructure(Size = 0x90)]
        public class DecomposedPoopShape : HkpShapeCollection
        {
            [TagField(Align = 16)]
            public RealQuaternion AabbCenter;
            public RealQuaternion AabbHalfExtents;
            public float Scale;
            [TagField(Align = 16)]
            public CollisionGeometryShape GeometryShape;
        }
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
