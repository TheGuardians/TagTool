using TagTool.Cache;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_mix", Tag = "snmx", Size = 0x70, MinVersion = CacheVersion.Halo3Retail,MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "sound_mix", Tag = "snmx", Size = 0x74, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "sound_mix", Tag = "snmx", Size = 0x80, MinVersion = CacheVersion.HaloOnline106708)]
    public class SoundMix : TagStructure
	{
        public float LeftStereoGain;
        public float RightStereoGain;
        public float LeftStereoGain2;
        public float RightStereoGain2;
        public float LeftStereoGain3;
        public float RightStereoGain3;
        public float FrontSpeakerGain;
        public float RearSpeakerGain;
        public float FrontSpeakerGain2;
        public float RearSpeakerGain2;
        public float MonoUnspatializedGain;
        public float StereoTo3dGain;
        public float RearSurroundToFrontStereoGain;
        public float FrontSpeakerGain3;
        public float CenterSpeakerGain;
        public float FrontSpeakerGain4;
        public float CenterSpeakerGain2;
        public float StereoUnspatializedGain;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown1;

        public float SoloPlayerFadeOutDelay;
        public float SoloPlayerFadeOutTime;
        public float SoloPlayerFadeInTime;
        public float GameMusicFadeOutTime;
        public CachedTagInstance Unknown2;
        public float Unknown3;
        public float Unknown4;

        [TagField(Padding = true, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;
    }
}