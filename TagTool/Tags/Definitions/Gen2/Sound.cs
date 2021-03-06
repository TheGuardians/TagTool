using TagTool.Cache;
using System;
using TagTool.Audio;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "sound", Tag = "snd!", MinVersion = CacheVersion.Halo2Beta, MaxVersion= CacheVersion.Halo2Vista, Size = 0x14)]
    public class Sound : TagStructure
    {
        public Gen2SoundFlags Flags;
        public SoundClass Class;
        public SampleRate SampleRate;

        public Gen2Encoding Encoding;
        public Compression Compression;

        public short PlaybackParamaterIndex;
        public short PitchRangeIndex;
        public sbyte PitchRangeCount;
        public sbyte ScaleIndex;
        public sbyte PromotionIndex;
        public sbyte CustomPlaybackIndex;
        public short ExtraInfoIndex;
        public int MaximumPlayTime;

        [Flags]
        public enum Gen2SoundFlags : ushort
        {
            FitToAdpcmBlocksize = 1 << 0,
            SplitLongSoundIntoPermutations = 1 << 1,
            /// <summary>
            /// always play as 3d sound, even in first person
            /// </summary>
            AlwaysSpatialize = 1 << 2,
            /// <summary>
            /// disable occlusion/obstruction for this sound
            /// </summary>
            NeverObstruct = 1 << 3,
            InternalDonTTouch = 1 << 4,
            UseHugeSoundTransmission = 1 << 5,
            LinkCountToOwnerUnit = 1 << 6,
            PitchRangeIsLanguage = 1 << 7,
            DonTUseSoundClassSpeakerFlag = 1 << 8,
            DonTUseLipsyncData = 1 << 9
        }  
    }
}

