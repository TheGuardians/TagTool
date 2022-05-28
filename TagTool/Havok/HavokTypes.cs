using TagTool.Common;
using TagTool.Tags;
using TagTool.Cache;

/*
 * Havok Structures required to deserialize tags and resources. Most havok structs are aligned at 0x10 which is why there is a lot of padding. 
 * Either the values in padding are 0 or 0xCD depending if they are in a tag or resource. The tag version of HkpMoppData has the flag DONT_DEALLOCATE_FLAG.
 */

namespace TagTool.Havok
{
    public static class HkArrayFlags
    {
        public static readonly uint CAPACITY_MASK = 0x3FFFFFFF;
        public static readonly uint FLAG_MASK = 0xC0000000;
        public static readonly uint DONT_DEALLOCATE_FLAG = 0x80000000; // Indicates that the storage is not the array's to delete
        public static readonly uint LOCKED_FLAG = 0x40000000;  // Indicates that the array will never have its dtor called (read in from packfile for instance)
    };

    public enum BlamShapeType : short
    {
        Sphere,
        Pill,
        Box,
        Triangle,
        Polyhedron,
        MultiSphere,
        TriangleMesh,
        CompoundShape,
        Unused0,
        Unused1,
        Unused2,
        Unused3,
        Unused4,
        Unused5,
        List,
        Mopp
    }

    [TagStructure(Size = 0x4, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x8, Platform = CachePlatform.MCC)]
    public class HavokShapeReference : TagStructure
    {
        // TOOD: consider endianess
        public BlamShapeType Type;
        public short Index;
        [TagField(Length = 4, Flags = TagFieldFlags.Padding, Platform = CachePlatform.MCC)]
        public byte[] Padding;

        public HavokShapeReference() { }

        public HavokShapeReference(BlamShapeType type, short index)
        {
            Type = type;
            Index = index; 
        }
    }

    /// <summary>
    /// Tag variant of HkpMoppCode with the actual codes in a tag block
    /// </summary>
    [TagStructure(Size = 0x10, Align = 16, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x10, Align = 16, Platform = CachePlatform.MCC)]
    public class TagHkpMoppCode : HkpMoppCode
    {
        public TagBlock<byte> Data;
        public MoppBuildType BuildType;
        [TagField(Flags = TagFieldFlags.Padding, Length = 3)]
        public byte[] Padding2;

        public enum MoppBuildType : sbyte
        {
            BuiltWithChunkSubdivision,
            BuiltWithoutChunkSubdivision
        }
    }

    /// <summary>
    /// Mopp code structure used in byte[]'s
    /// </summary>
    [TagStructure(Size = 0x30)]
    public class HkpMoppCode : TagStructure
    {
        public PlatformUnsignedValue VfTableAddress;
        public HkpReferencedObject ReferencedObject;
        [TagField(Align = 16)]
        public CodeInfo Info;
        public HkArrayBase ArrayBase;
        [TagField(Length = 4, Platform = CachePlatform.Original)]
        public byte[] Padding1;
    }

    [TagStructure(Size = 0xC, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x18, Platform = CachePlatform.MCC)]
    public class HkpMoppBvTreeShape : HkpShape
    {
        public HkpSingleShapeContainer Child;
        public PlatformUnsignedValue MoppCodeAddress;
    }

    [TagStructure(Size = 0x4)]
    public class CMoppBvTreeShape : HkpMoppBvTreeShape
    {
        public float Scale;
    }

    [TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo2Vista, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x20, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    public class HkpShape : TagStructure
    {
        public PlatformUnsignedValue VfTableAddress;
        public HkpReferencedObject ReferencedObject;
        public PlatformUnsignedValue UserDataAddress;
        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public uint Type;
        [TagField(Length = 4, Platform = CachePlatform.MCC)]
        public byte[] Padding1;
    }

    /// <summary>
    /// At runtime this is a pointer
    /// </summary>
    [TagStructure(Size = 0x8, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x10, Platform = CachePlatform.MCC)]
    public class HkpSingleShapeContainer : TagStructure
    {
        public PlatformUnsignedValue VTableAddress;
        public HavokShapeReference Shape;
    }

   
    [TagStructure(Size = 0x8, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x10, Platform = CachePlatform.MCC)]
    public class HkpShapeCollection : HkpShape
    {
        public PlatformUnsignedValue VTableAddress;
        public bool DisableWelding;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public sbyte CollectionType;
        [TagField(Length = 3, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Padding2;
        [TagField(Length = 2, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloReach)]
        public byte[] Padding3;
        [TagField(Length = 4, Flags = TagFieldFlags.Padding, Platform = CachePlatform.MCC)]
        public byte[] Padding4;
    }

    [TagStructure(Size = 0xC, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x10, Platform = CachePlatform.MCC)]
    public class HkArrayBase : TagStructure
    {
        public PlatformUnsignedValue DataAddress;
        public uint Size;
        public uint CapacityAndFlags;

        public uint GetCapacity()
        {
            return CapacityAndFlags & HkArrayFlags.CAPACITY_MASK;
        }
    }

    [TagStructure(Size = 0x4)]
    public class HkpReferencedObject : TagStructure
    {
        public ushort SizeAndFlags;
        public ushort ReferenceCount = 128;
    }

    [TagStructure(Size = 0x10)]
    public class CodeInfo : TagStructure
    {
        [TagField(Align = 16)]
        public RealQuaternion Offset; // actually vector4, refactor quaternion stuff later
    }   

    [TagStructure(Size = 0x90, Align = 0x10)]
    public class HkMultiSphereShape : HkpShape
    {
        public int NumSpheres;
        [TagField(Length = 8, Align = 0x10)]
        public RealQuaternion[] Spheres;
    }

    [TagStructure(Size = 0x38, Align = 0x10, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x30, Align = 0x10, Platform = CachePlatform.MCC)]
    public class HkListShape : HkpShapeCollection
    {
        [TagField(Align = 0x4, Platform = CachePlatform.Original)]
        [TagField(Align = 0x8, Platform = CachePlatform.MCC)]
        public HkArrayBase ChildInfo;
        [TagField(Align = 0x10)]
        public RealQuaternion AabbHalfExtents;
        public RealQuaternion AAbbCenter;
    }
}

namespace TagTool.Havok.Gen2
{
    [TagStructure(Size = 0x4)]
    public class HkShape : TagStructure
    {
        public uint VfTableAddress;
        public HkpReferencedObject ReferencedObject;
        public uint UserData;
    }

    [TagStructure(Size = 0x4)]
    public class HkConvexWelderShape : HkpShape
    {
        public uint ShapeAddress;
    }

    [TagStructure(Size = 0x4)]
    public class HkSingleShapeContainer : TagStructure
    {
        public uint ShapeAddress;
    }

    [TagStructure(Size = 0x8)]
    public class HkMoppBvTreeShape : HkpShape
    {
        public HkSingleShapeContainer Child;
        public uint MoppCodeAddress;
    }

    [TagStructure]
    public class CMoppBvTreeShape : HkMoppBvTreeShape
    {
        
    }

    [TagStructure(Size = 0x30)]
    public class HkMoppCode
    {
        public RealQuaternion Offset;
        public int ByteOrdering;
        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public HkpReferencedObject ReferencedObject;
        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
    }

    [TagStructure(Size = 4)]
    public class CConvexWelderShape : HkpShape
    {
        public uint ShapeAddress;
    }

    [TagStructure(Size = 0x30)]
    public class MoppCodeHeader : TagStructure
    {
        public RealQuaternion Offset;
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public uint Size;
        public int Unknown6;
        public int Unknown7;
        public int Unknown8;
    }
}