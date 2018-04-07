using TagTool.Common;
using TagTool.Serialization;


namespace TagTool.Audio
{
    [TagStructure(Size = 0x14)]
    public class Scale
    {
        public Bounds<float> GainModifierBounds;
        public Bounds<short> PitchModifierBounds;
        public Bounds<float> SkipFractionModifierBounds;
    }
}