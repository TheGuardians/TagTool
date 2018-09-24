using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x28)]
    public class CharacterPreSearchProperties : TagStructure
	{
        public CharacterPreSearchFlags Flags;
        public Bounds<float> MinimumPreSearchTime;
        public Bounds<float> MaximumPreSearchTime;
        public float MinimumCertaintyRadius;
        public uint Unknown;
        public Bounds<float> MinimumSuppressingTime;
        public short Unknown2;
        public short Unknown3;
    }
}
