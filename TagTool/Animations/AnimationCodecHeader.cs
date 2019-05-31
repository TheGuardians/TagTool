using TagTool.Tags;

namespace TagTool.Animations
{
    [TagStructure(Size = 0x20)]
    public class AnimationCodecHeader
    {
        /// <summary>
        /// The codec type used for the animation data.
        /// </summary>
        public AnimationCodecType CodecType;

        /// <summary>
        /// The number of nodes with rotation frames.
        /// </summary>
        public byte RotationNodeCount;

        /// <summary>
        /// The number of nodes with translation frames.
        /// </summary>
        public byte TranslationNodeCount;

        /// <summary>
        /// The number of nodes with scale frames.
        /// </summary>
        public byte ScaleNodeCount;

        public float Unknown4;

        /// <summary>
        /// The playback rate of the animation data.
        /// </summary>
        public float PlaybackRate = 1.0f;

        public float UnknownC;
        public float Unknown10;
        public float Unknown14;
        public float Unknown18;
        public float Unknown1C;
    }
}
