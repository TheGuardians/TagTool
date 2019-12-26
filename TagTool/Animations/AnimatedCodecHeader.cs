using TagTool.Tags;

// Thanks Grimdoomer! -Camden
namespace TagTool.Animations
{
    [TagStructure(Size = 0x24)]
    public class AnimatedCodecHeader : AnimationCodecHeader
    {
        public int TranslationFrameInfoOffset;
        public int ScaleFrameInfoOffset;
        public int RotationKeyframeDataOffset;
        public int TranslationKeyframeDataOffset;
        public int ScaleKeyframeDataOffset;
        public int RotationDataOffset;
        public int TranslationDataOffset;
        public int ScaleDataOffset;

        [TagField(Flags = TagFieldFlags.Padding, Length = 4)]
        public byte[] Unused = new byte[4];
    }
}