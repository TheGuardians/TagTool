using TagTool.Common;
using TagTool.Tags;

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

    /// <summary>
    /// Tag variant of HkpMoppCode with the actual codes in a tag block
    /// </summary>
    [TagStructure(Size = 0x10)]
    public class TagHkpMoppCode : HkpMoppCode
    {
        public TagBlock<byte> Data;

        [TagField(Length = 4)]
        public byte[] Padding3;
    }

    /// <summary>
    /// Mopp code structure used in byte[]'s
    /// </summary>
    [TagStructure(Size = 0x30)]
    public class HkpMoppCode : TagStructure
    {
        public uint VfTableAddress;

        public HkpReferencedObject ReferencedObject;

        [TagField(Length = 8)]
        public byte[] Padding1;

        public CodeInfo Info;

        public HkArrayBase ArrayBase;

        [TagField(Length = 4)]
        public byte[] Padding2;
    }

    [TagStructure(Size = 0x10)]
    public class HkpBvMoppTreeShape : HkpShape
    {
        public HkpSingleShapeContainer Child;
        public uint ShapeCollectionAddress;
        public uint MoppCodeAddress;
    }

    [TagStructure(Size = 0x4)]
    public class HkpShapeContainer : TagStructure
    {
        public uint VfTableAddress;
    }

    [TagStructure(Size = 0x10)]
    public class HkpShape : TagStructure
    {
        public uint VfTableAddress;
        public HkpReferencedObject ReferencedObject;
        public uint UserDataAddress;
        public uint Type;
    }

    /// <summary>
    /// At runtime this is a pointer
    /// </summary>
    [TagStructure(Size = 0x4)]
    public class HkpSingleShapeContainer : HkpShapeContainer
    {
        public BlamShapeType Type;
        public short Index;
    }

    [TagStructure(Size = 0x10)]
    public class HkpShapeCollection : HkpShape
    {
        public HkpShapeContainer Container;
        public bool DisableWelding;
        [TagField(Length = 11)]
        public byte[] Padding;
    }

    [TagStructure(Size = 0xC)]
    public class HkArrayBase : TagStructure
    {
        public uint DataAddress;
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
        public ushort ReferenceCount;
    }

    [TagStructure(Size = 0x10)]
    public class CodeInfo : TagStructure
    {
        public RealQuaternion Offset; // actually vector4, refactor quaternion stuff later
    }
}