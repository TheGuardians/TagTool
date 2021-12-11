using TagTool.Common;
using TagTool.Tags;


namespace TagTool.Audio
{
    // As the sound's input scale changes from zero to one, these modifiers move between the two values specified here.
    // The sound will play using the current scale modifier multiplied by the values specified above. (0 values are ignored.)
    [TagStructure(Size = 0x14)]
    public class SoundScaleModifier : TagStructure
    {
        public Bounds<float> GainModifierBounds;    // (dB)
        public Bounds<short> PitchModifierBounds;   // (cents)
        public Bounds<float> SkipFractionModifierBounds;
    }
}