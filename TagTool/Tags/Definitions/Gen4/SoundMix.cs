using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "sound_mix", Tag = "snmx", Size = 0x94)]
    public class SoundMix : TagStructure
    {
        public SoundTransmissionDefinitionStruct DefaultTransmissionSettings;
        public SoundStereoMixStruct FirstPersonLeftSideMix;
        public SoundStereoMixStruct FirstPersonMiddleMix;
        public SoundStereoMixStruct FirstPersonRightSideMix;
        public SoundSurroundMixStruct FirstPersonSurroundMix;
        public SoundSurroundMixStruct AmbientSurroundMix;
        public SoundGlobalMixStruct GlobalMix;
        
        [TagStructure(Size = 0x10)]
        public class SoundTransmissionDefinitionStruct : TagStructure
        {
            public GlobalSoundLowpassBlock ObstructionSettings;
            public GlobalSoundLowpassBlock OcclusionSettings;
            
            [TagStructure(Size = 0x8)]
            public class GlobalSoundLowpassBlock : TagStructure
            {
                public float CutoffFrequency; // Hz
                public float OutputGain; // dB
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class SoundStereoMixStruct : TagStructure
        {
            public float LeftStereoGain; // dB
            public float RightStereoGain; // dB
        }
        
        [TagStructure(Size = 0x8)]
        public class SoundSurroundMixStruct : TagStructure
        {
            public float FrontSpeakerGain; // dB
            public float RearSpeakerGain; // dB
        }
        
        [TagStructure(Size = 0x5C)]
        public class SoundGlobalMixStruct : TagStructure
        {
            public float MonoUnspatializedGain; // dB
            public float StereoTo3dGain; // dB
            public float RearSurroundToFrontStereoGain; // dB
            public SoundCenterMixStruct SurroundCenterMix;
            public SoundCenterMixStruct StereoCenterMix;
            public SoundCenterMixStruct RadioSurroundCenterMix;
            public SoundCenterMixStruct RadioStereoCenterMix;
            public float StereoUnspatializedGain; // dB
            public float QuadRouteToLfeGain; // dB
            public float SoloPlayerFadeOutDelay; // seconds
            public float SoloPlayerFadeOutTime; // seconds
            public float SoloPlayerFadeInTime; // seconds
            public float GameMusicFadeOutTime; // seconds
            [TagField(ValidTags = new [] { "sndo","snd!" })]
            public CachedTag PlayOnUnplayableSound;
            public float LeftRightBleed; // [0 = no bleed, 1 = swap left/right, 0.5 = mono
            public float RemoteVoiceBoost; // output= (1 + boost)*input
            
            [TagStructure(Size = 0x8)]
            public class SoundCenterMixStruct : TagStructure
            {
                public float FrontSpeakerGain; // dB
                public float CenterSpeakerGain; // dB
            }
        }
    }
}
