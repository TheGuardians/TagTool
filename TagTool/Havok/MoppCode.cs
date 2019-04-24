using TagTool.Common;
using TagTool.Tags;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Havok
{
    [TagStructure(Size = 0x30)]
    public class MoppCode : TagStructure
	{
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Unused1 = new byte[4];

        public short Size;
        public short Count;
        public int Address;

        [TagField(Flags = Padding, Length = 4)]
        public byte[] Unused2 = new byte[4];

        public RealQuaternion Offset;

        [TagField(Flags = Padding, Length = 4)]
        public byte[] Unused3 = new byte[4];

        public int DataSize;
        public uint DataCapacityAndFlags;
        public sbyte DataBuildType;

        [TagField(Flags = Padding, Length = 3)]
        public byte[] Unused4 = new byte[3];
    }
}