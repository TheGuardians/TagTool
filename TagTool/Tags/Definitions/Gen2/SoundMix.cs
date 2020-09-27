using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "sound_mix", Tag = "snmx", Size = 0x58)]
    public class SoundMix : TagStructure
    {
        /// <summary>
        /// first person left side mix
        /// </summary>
        /// <remarks>
        /// for first person sounds to the left of you
        /// </remarks>
        public float LeftStereoGain; // dB
        public float RightStereoGain; // dB
        /// <summary>
        /// first person middle mix
        /// </summary>
        /// <remarks>
        /// for first person sounds between your ears
        /// </remarks>
        public float LeftStereoGain1; // dB
        public float RightStereoGain2; // dB
        /// <summary>
        /// first person right side mix
        /// </summary>
        /// <remarks>
        /// for first person sounds to the right of you
        /// </remarks>
        public float LeftStereoGain3; // dB
        public float RightStereoGain4; // dB
        /// <summary>
        /// first person stereo mix
        /// </summary>
        public float FrontSpeakerGain; // dB
        public float RearSpeakerGain; // dB
        /// <summary>
        /// ambient stereo mix
        /// </summary>
        public float FrontSpeakerGain5; // dB
        public float RearSpeakerGain6; // dB
        /// <summary>
        /// global mix
        /// </summary>
        public SoundGlobalMixConfiguration GlobalMix;
        
        [TagStructure(Size = 0x30)]
        public class SoundGlobalMixConfiguration : TagStructure
        {
            public float MonoUnspatializedGain; // dB
            public float StereoTo3dGain; // dB
            public float RearSurroundToFrontStereoGain; // dB
            /// <summary>
            /// surround center mix
            /// </summary>
            /// <remarks>
            /// for sounds that use the center speaker
            /// </remarks>
            public float FrontSpeakerGain; // dB
            public float CenterSpeakerGain; // dB
            /// <summary>
            /// stereo center mix
            /// </summary>
            /// <remarks>
            /// for sounds that use the center speaker
            /// </remarks>
            public float FrontSpeakerGain1; // dB
            public float CenterSpeakerGain2; // dB
            /// <summary>
            /// more sound lovin'
            /// </summary>
            /// <remarks>
            /// for sounds that use the center speaker
            /// </remarks>
            public float StereoUnspatializedGain; // dB
            /// <summary>
            /// last minute values
            /// </summary>
            public float SoloPlayerFadeOutDelay; //  seconds
            public float SoloPlayerFadeOutTime; //  seconds
            public float SoloPlayerFadeInTime; //  seconds
            public float GameMusicFadeOutTime; //  seconds
        }
    }
}

