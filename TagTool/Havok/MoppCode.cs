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
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Unused1;

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
}