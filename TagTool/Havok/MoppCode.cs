using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Havok
{
    [TagStructure(Size = 0x30)]
    public class MoppCode : TagStructure
	{
        [TagField(Padding = true, Length = 4)]
        public byte[] Unused1 = new byte[4];

        public short Size;
        public short Count;
        public int Address;

        [TagField(Padding = true, Length = 4)]
        public byte[] Unused2 = new byte[4];

        public RealQuaternion Offset;

        [TagField(Padding = true, Length = 4)]
        public byte[] Unused3 = new byte[4];

        public int DataSize;
        public int DataCapacityAndFlags;
        public sbyte DataBuildType;

        [TagField(Padding = true, Length = 3)]
        public byte[] Unused4 = new byte[3];
    }
}