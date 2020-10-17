using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "sound_looping", Tag = "lsnd", Size = 0x54)]
    public class SoundLooping : TagStructure
    {
        public FlagsValue Flags;
        /// <summary>
        /// as the sound's input scale changes from zero to one, these modifiers move between the two values specified here. the
        /// sound will play using the current scale modifier multiplied by the value specified below. (0 values are ignored.)
        /// </summary>
        public float DetailSoundPeriod;
        [TagField(Length = 0x8)]
        public byte[] Padding;
        /// <summary>
        /// as the sound's input scale changes from zero to one, these modifiers move between the two values specified here. the
        /// sound will play using the current scale modifier multiplied by the value specified below. (0 values are ignored.)
        /// </summary>
        public float DetailSoundPeriod1;
        [TagField(Length = 0x8)]
        public byte[] Padding1;
        [TagField(Length = 0x10)]
        public byte[] Padding2;
        [TagField(ValidTags = new [] { "cdmg" })]
        public CachedTag ContinuousDamageEffect;
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
            DeafeningToAis = 1 << 0,
            /// <summary>
            /// this is a collection of permutations strung together that should play once then stop.
            /// </summary>
            NotALoop = 1 << 1,
            /// <summary>
            /// all other music loops will stop when this one starts.
            /// </summary>
            StopsMusic = 1 << 2
        }
        
        [TagStructure(Size = 0xA0)]
        public class LoopingSoundTrackBlock : TagStructure
        {
            public FlagsValue Flags;
            public float Gain;
            public float FadeInDuration; // seconds
            public float FadeOutDuration; // seconds
            [TagField(Length = 0x20)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag Start;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag Loop;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag End;
            [TagField(Length = 0x20)]
            public byte[] Padding1;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag AlternateLoop;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag AlternateEnd;
            
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
                FadeInAlternate = 1 << 2
            }
        }
        
        [TagStructure(Size = 0x68)]
        public class LoopingSoundDetailBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag Sound;
            /// <summary>
            /// the time between successive playings of this sound will be randomly selected from this range.
            /// </summary>
            public Bounds<float> RandomPeriodBounds; // seconds
            public float Gain;
            public FlagsValue Flags;
            [TagField(Length = 0x30)]
            public byte[] Padding;
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
                DonTPlayWithAlternate = 1 << 0,
                DonTPlayWithoutAlternate = 1 << 1
            }
        }
    }
}

