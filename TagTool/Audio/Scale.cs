using TagTool.Common;
using TagTool.Tags;


namespace TagTool.Audio
{
    [TagStructure(Size = 0x14)]
    public class Scale : TagStructure
	{
        public Bounds<float> GainModifierBounds;
        public Bounds<short> PitchModifierBounds;
        public Bounds<float> SkipFractionModifierBounds;
    }
}