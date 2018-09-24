using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "decorator_set", Tag = "dctr", Size = 0x80)]
    public class DecoratorSet : TagStructure
	{
        public CachedTagInstance Model;
        public uint Unknown;
        public uint Unknown2;
        public uint Unknown3;
        public int Unknown4;
        public CachedTagInstance Texture;

        public byte AffectsVisibility;
        public byte UnknownByte;

        public short Unknown5;
        public RealRgbColor Color;
        public uint Unknown6;
        public uint Unknown7;
        public uint Unknown8;
        public uint Unknown9;
        public uint Unknown10;
        public float BrightnessBase;
        public float BrightnessShadow;
        public uint Unknown11;
        public uint Unknown12;
        public float Unknown13;
        public float Unknown14;
        public float Unknown15;
        public float Unknown16;
        public uint Unknown17;
        public uint Unknown18;
        public uint Unknown19;
    }
}
