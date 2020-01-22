using System;
using System.Collections.Generic;
using TagTool.Common;
using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
    /// <summary>
    /// This definition exists solely for the purpose of testing code.
    /// </summary>
    [TagStructure(Name = "test_blah", Tag = "test", Size = 0xC)]
    public class TestDefinition : TagStructure
    {
        public List<TestBlockDefinition> TestBlockField;

        public enum TestEnumDefinition : int
        {
            // TODO:
        }

        [Flags]
        public enum TestFlagsDefinition : int
        {
            // TODO:
        }

        [TagStructure(Size = 0x1E0)]
        public class TestBlockDefinition : TagStructure
        {
            public byte     Byte; // 1
            public sbyte    SByte; // 2
            public ushort   UShort; // 4
            public short    Short; // 6
            public uint     UInt; // 10
            public int      Int; // 14
            public ulong    ULong; // 22
            public long     Long; // 30
            public float    Float; // 34

            public TestEnumDefinition   Enum; // 38
            public TestFlagsDefinition  Flags; // 42

            public Angle                Angle; // 44
            public ArgbColor            ArgbColor; // 48
            public CachedTag    CachedTagInstance; // 64
            public CacheAddress         CacheAddress; // 68
            public DatumIndex           DatumIndex; // 72
            public PageableResource     PageableResource; // 180
            public Point2d              Point2d; // 184
            public RealArgbColor        RealArgbColor; // 200
            public RealBoundingBox      RealBoundingBox; // 224
            public RealEulerAngles2d    RealEulerAngles2d; // 232
            public RealEulerAngles3d    RealEulerAngles3d; // 248
            public RealMatrix4x3        RealMatrix4x3; // 296
            public RealPlane2d          RealPlane2d; // 308
            public RealPlane3d          RealPlane3d; // 324
            public RealPoint2d          RealPoint2d; // 332
            public RealPoint3d          RealPoint3d; // 344
            public RealQuaternion       RealQuaternion; // 360
            public RealRgbColor         RealRgbColor; // 372
            public RealVector2d         RealVector2d; // 380
            public RealVector3d         RealVector3d; // 392
            public Rectangle2d          Rectangle2d; // 400
            public StringId             StringId; // 404
            public Tag                  Tag; // 408

            public Bounds<Angle>        BoundsAngle; // 416
            public Bounds<byte>         BoundsByte; // 418
            public Bounds<sbyte>        BoundsSByte; // 420
            public Bounds<ushort>       BoundsUShort; // 424
            public Bounds<short>        BoundsShort; // 428
            public Bounds<uint>         BoundsUInt; // 436
            public Bounds<int>          BoundsInt; // 444
            public Bounds<ulong>        BoundsULong; // 460
            public Bounds<long>         BoundsLong; // 472
            public Bounds<float>        BoundsFloat; // 480
        }
    }
}
