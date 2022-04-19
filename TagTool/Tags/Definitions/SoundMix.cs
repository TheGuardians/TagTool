using TagTool.Cache;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_mix", Tag = "snmx", Size = 0x70, MinVersion = CacheVersion.Halo3Retail,MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "sound_mix", Tag = "snmx", Size = 0x74, MinVersion = CacheVersion.Halo3ODST)]
    public class SoundMix : TagStructure
    {
        public SoundStereoMixStruct FirstPersonLeftSideMix;
        public SoundStereoMixStruct FirstPersonMiddleMix;
        public SoundStereoMixStruct FirstPersonRightSideMix;
        public SoundSurroundMixStruct FirstPersonSurroundMix;
        public SoundSurroundMixStruct AmbientSurroundMix;
        public SoundGlobalMixStruct GlobalMix;

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

        [TagStructure(Size = 0x48, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x4C, MinVersion = CacheVersion.Halo3ODST)]
        public class SoundGlobalMixStruct : TagStructure
        {
            public float MonoUnspatializedGain; // dB
            public float StereoTo3dGain; // dB
            public float RearSurroundToFrontStereoGain; // dB
            public SoundCenterMixStruct SurroundCenterMix;
            public SoundCenterMixStruct StereoCenterMix;
            public float StereoUnspatializedGain; // dB

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float QuadRouteToLfeGain;

            public float SoloPlayerFadeOutDelay; // seconds
            public float SoloPlayerFadeOutTime; // seconds
            public float SoloPlayerFadeInTime; // seconds
            public float GameMusicFadeOutTime; // seconds
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
