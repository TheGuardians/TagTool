using TagTool.Tags;

namespace TagTool.Animations
{
    [TagStructure(Size = 0xC)]
    public class AnimationCodecHeader : TagStructure
    {
        public AnimationCodecType Type;
        public sbyte RotationCount;
        public sbyte TranslationCount;
        public sbyte ScaleCount;
        public float ErrorPercentage;
        public float PlaybackRate;        
    }
}