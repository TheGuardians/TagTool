using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Animations.Codecs
{
    [TagStructure(Size = 0x20)]
    public class AnimationCodecHeader
    {
        /// <summary>
        /// The codec type used for the animation data.
        /// </summary>
        public AnimationCodec Codec;

        /// <summary>
        /// The number of nodes with rotation frames.
        /// </summary>
        public byte RotationNodeCount;

        /// <summary>
        /// The number of nodes with position frames.
        /// </summary>
        public byte PositionNodeCount;

        /// <summary>
        /// The number of nodes with scale frames.
        /// </summary>
        public byte ScaleNodeCount;

        public uint Unknown4;

        /// <summary>
        /// The playback rate of the animation data.
        /// </summary>
        public float PlaybackRate = 1.0f;

        public uint UnknownC;
        public uint Unknown10;
        public uint Unknown14;
        public uint Unknown18;
        public uint Unknown1C;
    }
}
