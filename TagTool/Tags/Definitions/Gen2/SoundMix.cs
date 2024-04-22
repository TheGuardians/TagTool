using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "sound_mix", Tag = "snmx", Size = 0x58)]
    public class SoundMix : TagStructure
    {
        /// <summary>
        /// for first person sounds to the left of you
        /// </summary>
        public float LeftStereoGain; // dB
        public float RightStereoGain; // dB
        /// <summary>
        /// for first person sounds between your ears
        /// </summary>
        public float LeftStereoGain1; // dB
        public float RightStereoGain1; // dB
        /// <summary>
        /// for first person sounds to the right of you
        /// </summary>
        public float LeftStereoGain2; // dB
        public float RightStereoGain2; // dB
        public float FrontSpeakerGain; // dB
        public float RearSpeakerGain; // dB
        public float FrontSpeakerGain1; // dB
        public float RearSpeakerGain1; // dB
        public SoundGlobalMixStructBlock GlobalMix;
        
        [TagStructure(Size = 0x30)]
        public class SoundGlobalMixStructBlock : TagStructure
        {
            public float MonoUnspatializedGain; // dB
            public float StereoTo3dGain; // dB
            public float RearSurroundToFrontStereoGain; // dB
            /// <summary>
            /// for sounds that use the center speaker
            /// </summary>
            public float FrontSpeakerGain; // dB
            public float CenterSpeakerGain; // dB
            /// <summary>
            /// for sounds that use the center speaker
            /// </summary>
            public float FrontSpeakerGain1; // dB
            public float CenterSpeakerGain1; // dB
            /// <summary>
            /// for sounds that use the center speaker
            /// </summary>
            public float StereoUnspatializedGain; // dB
            public float SoloPlayerFadeOutDelay; //  seconds
            public float SoloPlayerFadeOutTime; //  seconds
            public float SoloPlayerFadeInTime; //  seconds
            public float GameMusicFadeOutTime; //  seconds
        }
    }
}

