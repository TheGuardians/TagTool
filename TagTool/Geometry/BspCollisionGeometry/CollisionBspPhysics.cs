using TagTool.Cache;
using TagTool.Common;
using TagTool.Havok;
using TagTool.Tags;

namespace TagTool.Geometry.BspCollisionGeometry
{
    [TagStructure(Size = 0xB0, Align=16, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    [TagStructure(Size = 0x70, Align=16, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x80, Align=16, MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    public class CollisionBspPhysicsDefinition : TagStructure
    {
        public CollisionGeometryShape GeometryShape;
        [TagField(Align = 16)]
        public CMoppBvTreeShape MoppBvTreeShape;
    }

    [TagStructure(Size = 0xB0, Align = 16, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    [TagStructure(Size = 0x70, Align = 16, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x80, Align = 16, Version = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x80, Align = 16, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x90, Align = 16, Version = CacheVersion.HaloOnlineED, Platform = CachePlatform.Original)]
    public class InstancedGeometryPhysics : TagStructure
    {
        public CollisionGeometryShape GeometryShape;
        [TagField(Align = 16)]
        public CMoppBvTreeShape MoppBvTreeShape;

        [TagField(Version = CacheVersion.HaloOnlineED)]
        public TagBlock<DecomposedPoopShape> PoopShape;
    }

    [TagStructure(Size = 0x70, Align = 16)]
    public class InstancedGeometryPhysicsReach : TagStructure
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
            public BvTreeTypeValue Type;
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
            //c_mopp_bv_tree
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

            public enum BvTreeTypeValue : sbyte
            {
                Mopp,
                TrisampledHeightfield,
                User,
            };
        }
    }

    // A collection of scaled hkpConvexVerticesShape (polyhedra in instanced geometry definition)
    [TagStructure(Size = 0x30, Align = 16)]
    public class DecomposedPoopShape : HkpShapeCollection
    {
        [TagField(Align = 16)]
        public RealQuaternion AabbCenter;
        public RealQuaternion AabbHalfExtents;
        public float Scale;
        public PlatformSignedValue InstanceDefinition;
    }

    [TagStructure(Size = 0x70, MaxVersion = CacheVersion.Halo2Vista)]
    public class CollisionBspPhysicsDefinitionGen2 : TagStructure
    {
        public CollisionGeometryShapeGen2 GeometryShape;
        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public Havok.Gen2.CConvexWelderShape WelderShape;
        public Havok.Gen2.CMoppBvTreeShape BvTreeShape;
        public byte[] MoppCodes;
        [TagField(Length = 4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
    }
}
