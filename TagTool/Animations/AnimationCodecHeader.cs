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

        public bool IsStatic
        {
            get
            {
                switch (Type)
                {
                    case AnimationCodecType.UncompressedStatic:
                    case AnimationCodecType.UncompressedAnimated:
                    case AnimationCodecType._8ByteQuantizedRotationOnly:
                    case AnimationCodecType.BlendScreen:
                        return true;

                    default:
                        return false;
                }
            }
        }

        public bool IsAnimated
        {
            get
            {
                switch (Type)
                {
                    case AnimationCodecType.ByteKeyframeLightlyQuantized:
                    case AnimationCodecType.WordKeyframeLightlyQuantized:
                    case AnimationCodecType.ReverseByteKeyframeLightlyQuantized:
                    case AnimationCodecType.ReverseWordKeyframeLightlyQuantized:
                        return true;

                    default:
                        return false;
                }
            }
        }
    }
}