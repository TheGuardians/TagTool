using TagTool.Common;
using TagTool.Tags;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Havok
{
    [TagStructure(Size = 0x10)]
    public class CollisionMoppCode : MoppCodeHeader
    {
        public TagBlock<byte> Data;
        public sbyte MoppBuildType;

        [TagField(Flags = Padding, Length = 3)]
        public byte[] Unused = new byte[3];
    }

    [TagStructure(Size = 0x30)]
    public class MoppCodeHeader : TagStructure
    {
        public uint VfTableAddress;

        public short Size;
        public short Count;

        public int Address;

        [TagField(Flags = Padding, Length = 4)]
        public byte[] Unused2;

        public RealQuaternion Offset;

        [TagField(Flags = Padding, Length = 4)]
        public byte[] Unused3;

        public int DataSize;
        public uint DataCapacityAndFlags;
        public sbyte DataBuildType;

        [TagField(Flags = Padding, Length = 3)]
        public byte[] Unused4;
    }

    [TagStructure(Size = 0x4)]
    public class HkpReferencedObject : TagStructure
    {
        public ushort SizeAndFlags;
        public ushort ReferenceCount;
    }

    [TagStructure(Size = 0x20)]
    public class MoppTest : TagStructure
    {
        public uint VfTableAddress;
        public HkpReferencedObject ReferencedObject;
        public int Offset;
        public int Unknown2;
        public uint Unknown3;
        public HavokShapeType ShapeType;
        public short ShapeIndex;
        public uint Unknown4;
        public uint Unknown5;
    }

    [TagStructure(Size = 0xC)]
    public class HkArrayBase : TagStructure
    {
        public uint DataAddress;
        public uint Size;
        public uint CapacityAndFlags;
    }

    [TagStructure(Size = 0x10)]
    public class CodeInfo : TagStructure
    {
        public RealQuaternion Offset; // actually vector4, refactor quaternion stuff later
    }

    [TagStructure(Size = 0x30)]
    public class HkMoppCode : TagStructure
    {
        public uint VfTableAddress;

        public HkpReferencedObject ReferencedObject;

        [TagField(Flags = Padding, Length = 8)]
        public byte[] Padding1;

        public CodeInfo Info;

        public HkArrayBase ArrayBase;
        public BuildType DataBuildType;

        [TagField(Flags = Padding, Length = 3)]
        public byte[] Padding2;
    }

    public enum BuildType : byte
    {
        BUILT_WITH_CHUNK_SUBDIVISION,
        BUILT_WITHOUT_CHUNK_SUBDIVISION
    }

    /// <summary>
    /// Mopp code structure used in byte[]'s
    /// </summary>
    [TagStructure(Size = 0x30)]
    public class HkpMoppCode : TagStructure
    {
        public uint VfTableAddress;

        public HkpReferencedObject ReferencedObject;

        [TagField(Flags = Padding, Length = 8)]
        public byte[] Padding1;

        public CodeInfo Info;

        public HkArrayBase ArrayBase;
        public BuildType DataBuildType;

        [TagField(Flags = Padding, Length = 3)]
        public byte[] Padding2;
    }
}