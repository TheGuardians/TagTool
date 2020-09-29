using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "sound_looping", Tag = "lsnd", Size = 0x3C)]
    public class SoundLooping : TagStructure
    {
        public FlagsValue Flags;
        public float MartySMusicTime; //  seconds
        public float Unknown1;
        [TagField(Flags = Padding, Length = 8)]
        public byte[] Padding1;
        public CachedTag Unknown2;
        public List<LoopingSoundTrack> Tracks; // tracks play in parallel and loop continuously for the duration of the looping sound.
        public List<LoopingSoundDetail> DetailSounds; // detail sounds play at random throughout the duration of the looping sound.
        
        [Flags]
        public enum FlagsValue : uint
        {
            DeafeningToAis = 1 << 0,
            NotALoop = 1 << 1,
            StopsMusic = 1 << 2,
            AlwaysSpatialize = 1 << 3,
            SynchronizePlayback = 1 << 4,
            SynchronizeTracks = 1 << 5,
            FakeSpatializationWithDistance = 1 << 6,
            CombineAll3dPlayback = 1 << 7
        }
        
        [TagStructure(Size = 0x90)]
        public class LoopingSoundTrack : TagStructure
        {
            public StringId Name;
            public FlagsValue Flags;
            public float Gain; // dB
            public float FadeInDuration; // seconds
            public float FadeOutDuration; // seconds
            public CachedTag In;
            public CachedTag Loop;
            public CachedTag Out;
            public CachedTag AltLoop;
            public CachedTag AltOut;
            public OutputEffectValue OutputEffect;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public CachedTag AltTransIn;
            public CachedTag AltTransOut;
            public float AltCrossfadeDuration; // seconds
            public float AltFadeOutDuration; // seconds
            
            [Flags]
            public enum FlagsValue : uint
            {
                FadeInAtStart = 1 << 0,
                FadeOutAtStop = 1 << 1,
                CrossfadeAltLoop = 1 << 2,
                MasterSurroundSoundTrack = 1 << 3,
                FadeOutAtAltStop = 1 << 4
            }
            
            public enum OutputEffectValue : short
            {
                None,
                OutputFrontSpeakers,
                OutputRearSpeakers,
                OutputCenterSpeakers
            }
        }
        
        [TagStructure(Size = 0x3C)]
        public class LoopingSoundDetail : TagStructure
        {
            public StringId Name;
            public CachedTag Sound;
            /// <summary>
            /// frequency of play
            /// </summary>
            public Bounds<float> RandomPeriodBounds; // seconds
            public float Unknown1;
            public FlagsValue Flags;
            /// <summary>
            /// random spatialization
            /// </summary>
            /// <remarks>
            /// if the sound specified above is not stereo it will be randomly spatialized according to the following constraints. if both lower and upper bounds are zero for any of the following fields, the sound's position will be randomly selected from all possible directions or distances.
            /// </remarks>
            public Bounds<Angle> YawBounds; // degrees
            public Bounds<Angle> PitchBounds; // degrees
            public Bounds<float> DistanceBounds; // world units
            
            [Flags]
            public enum FlagsValue : uint
            {
                DonTPlayWithAlternate = 1 << 0,
                DonTPlayWithoutAlternate = 1 << 1,
                StartImmediatelyWithLoop = 1 << 2
            }
        }
    }
}

