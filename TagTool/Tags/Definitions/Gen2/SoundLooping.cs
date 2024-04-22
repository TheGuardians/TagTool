using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "sound_looping", Tag = "lsnd", Size = 0x2C)]
    public class SoundLooping : TagStructure
    {
        public FlagsValue Flags;
        public Bounds<float> MartySMusicTime; //  seconds
        public Bounds<float> DistanceBounds;
        public CachedTag Unknown1;
        /// <summary>
        /// tracks play in parallel and loop continuously for the duration of the looping sound.
        /// </summary>
        public List<LoopingSoundTrackBlock> Tracks;
        /// <summary>
        /// detail sounds play at random throughout the duration of the looping sound.
        /// </summary>
        public List<LoopingSoundDetailBlock> DetailSounds;
        
        [Flags]
        public enum FlagsValue : uint
        {
            /// <summary>
            /// when used as a background stereo track, causes nearby AIs to be unable to hear
            /// </summary>
            DeafeningToAIs = 1 << 0,
            /// <summary>
            /// this is a collection of permutations strung together that should play once then stop.
            /// </summary>
            NotALoop = 1 << 1,
            /// <summary>
            /// all other music loops will stop when this one starts.
            /// </summary>
            StopsMusic = 1 << 2,
            /// <summary>
            /// always play as 3d sound, even in first person
            /// </summary>
            AlwaysSpatialize = 1 << 3,
            /// <summary>
            /// synchronizes playback with other looping sounds attached to the owner of this sound
            /// </summary>
            SynchronizePlayback = 1 << 4,
            SynchronizeTracks = 1 << 5,
            FakeSpatializationWithDistance = 1 << 6,
            CombineAll3dPlayback = 1 << 7
        }
        
        [TagStructure(Size = 0x58)]
        public class LoopingSoundTrackBlock : TagStructure
        {
            public StringId Name;
            public FlagsValue Flags;
            public float Gain; // dB
            public float FadeInDuration; // seconds
            public float FadeOutDuration; // seconds
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag In;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag Loop;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag Out;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag AlternateLoop;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag AlternateOut;
            public OutputEffectValue OutputEffect;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag AlternateTransitionIn;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag AlternateTransitionOut;
            public float AlternateCrossfadeDuration; // seconds
            public float AlternateFadeOutDuration; // seconds
            
            [Flags]
            public enum FlagsValue : uint
            {
                /// <summary>
                /// the loop sound should fade in while the start sound is playing.
                /// </summary>
                FadeInAtStart = 1 << 0,
                /// <summary>
                /// the loop sound should fade out while the stop sound is playing.
                /// </summary>
                FadeOutAtStop = 1 << 1,
                /// <summary>
                /// when the sound changes to the alternate version,  .
                /// </summary>
                CrossfadeAlternateLoop = 1 << 2,
                MasterSurroundSoundTrack = 1 << 3,
                FadeOutAtAlternateStop = 1 << 4
            }
            
            public enum OutputEffectValue : short
            {
                None,
                OutputFrontSpeakers,
                OutputRearSpeakers,
                OutputCenterSpeakers
            }
        }
        
        [TagStructure(Size = 0x34)]
        public class LoopingSoundDetailBlock : TagStructure
        {
            public StringId Name;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag Sound;
            /// <summary>
            /// the time between successive playings of this sound will be randomly selected from this range.
            /// </summary>
            public Bounds<float> RandomPeriodBounds; // seconds
            public float DetailGain;
            public FlagsValue Flags;
            /// <summary>
            /// if the sound specified above is not stereo it will be randomly spatialized according to the following constraints. if
            /// both lower and upper bounds are zero for any of the following fields, the sound's position will be randomly selected from
            /// all possible directions or distances.
            /// </summary>
            /// <summary>
            /// the sound's position along the horizon will be randomly selected from this range.
            /// </summary>
            public Bounds<Angle> YawBounds; // degrees
            /// <summary>
            /// the sound's position above (positive values) or below (negative values) the horizon will be randomly selected from this
            /// range.
            /// </summary>
            public Bounds<Angle> PitchBounds; // degrees
            /// <summary>
            /// the sound's distance (from its spatialized looping sound or from the listener if the looping sound is stereo) will be
            /// randomly selected from this range.
            /// </summary>
            public Bounds<float> DistanceBounds; // world units
            
            [Flags]
            public enum FlagsValue : uint
            {
                DontPlayWithAlternate = 1 << 0,
                DontPlayWithoutAlternate = 1 << 1,
                StartImmediatelyWithLoop = 1 << 2
            }
        }
    }
}

