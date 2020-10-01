using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "sound", Tag = "snd!", Size = 0xA4)]
    public class Sound : TagStructure
    {
        public FlagsValue Flags;
        public ClassValue Class;
        public SampleRateValue SampleRate;
        /// <summary>
        /// the distance below which this sound no longer gets louder
        /// </summary>
        public float MinimumDistance; // world units
        /// <summary>
        /// the distance beyond which this sound is no longer audible
        /// </summary>
        public float MaximumDistance; // world units
        /// <summary>
        /// fraction of requests to play this sound that will be ignored (0 means always play.)
        /// </summary>
        public float SkipFraction;
        /// <summary>
        /// these settings control random variation of volume and pitch.
        /// </summary>
        /// <summary>
        /// the sound's pitch will be randomly selected and will be in this range. (1.0 is the recorded pitch.)
        /// </summary>
        public Bounds<float> RandomPitchBounds;
        /// <summary>
        /// within the cone defined by this angle and the sound's direction, the sound plays with a gain of 1.0.
        /// </summary>
        public Angle InnerConeAngle; // degrees
        /// <summary>
        /// outside the cone defined by this angle and the sound's direction, the sound plays with a gain of OUTER CONE GAIN. (0
        /// means the sound does not attenuate with direction.)
        /// </summary>
        public Angle OuterConeAngle; // degrees
        /// <summary>
        /// the gain to use when the sound is directed away from the listener
        /// </summary>
        public float OuterConeGain;
        public float GainModifier;
        public float MaximumBendPerSecond;
        [TagField(Length = 0xC)]
        public byte[] Padding;
        /// <summary>
        /// as the sound's input scale changes from zero to one, these modifiers move between the two values specified here. the
        /// sound will play using the current scale modifier multiplied by the value specified above. (0 values are ignored.)
        /// </summary>
        public float SkipFractionModifier;
        public float GainModifier1;
        public float PitchModifier;
        [TagField(Length = 0xC)]
        public byte[] Padding1;
        /// <summary>
        /// as the sound's input scale changes from zero to one, these modifiers move between the two values specified here. the
        /// sound will play using the current scale modifier multiplied by the value specified above. (0 values are ignored.)
        /// </summary>
        public float SkipFractionModifier1;
        public float GainModifier2;
        public float PitchModifier1;
        [TagField(Length = 0xC)]
        public byte[] Padding2;
        public EncodingValue Encoding;
        public CompressionValue Compression;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PromotionSound;
        /// <summary>
        /// when there are this many instances of the sound, promote to the new sound.
        /// </summary>
        public short PromotionCount;
        [TagField(Length = 0x2)]
        public byte[] Padding3;
        [TagField(Length = 0x14)]
        public byte[] Padding4;
        /// <summary>
        /// pitch ranges allow multiple samples to represent the same sound at different pitches
        /// </summary>
        public List<SoundPitchRangeBlock> PitchRanges;
        
        public enum FlagsValue : uint
        {
            FitToAdpcmBlocksize,
            SplitLongSoundIntoPermutations
        }
        
        public enum ClassValue : short
        {
            ProjectileImpact,
            ProjectileDetonation,
            Unknown,
            Unknown1,
            WeaponFire,
            WeaponReady,
            WeaponReload,
            WeaponEmpty,
            WeaponCharge,
            WeaponOverheat,
            WeaponIdle,
            Unknown2,
            Unknown3,
            ObjectImpacts,
            ParticleImpacts,
            SlowParticleImpacts,
            Unknown4,
            Unknown5,
            UnitFootsteps,
            UnitDialog,
            Unknown6,
            Unknown7,
            VehicleCollision,
            VehicleEngine,
            Unknown8,
            Unknown9,
            DeviceDoor,
            DeviceForceField,
            DeviceMachinery,
            DeviceNature,
            DeviceComputers,
            Unknown10,
            Music,
            AmbientNature,
            AmbientMachinery,
            AmbientComputers,
            Unknown11,
            Unknown12,
            Unknown13,
            FirstPersonDamage,
            Unknown14,
            Unknown15,
            Unknown16,
            Unknown17,
            ScriptedDialogPlayer,
            ScriptedEffect,
            ScriptedDialogOther,
            ScriptedDialogForceUnspatialized,
            Unknown18,
            Unknown19,
            GameEvent
        }
        
        public enum SampleRateValue : short
        {
            _22khz,
            _44khz
        }
        
        public enum EncodingValue : short
        {
            Mono,
            Stereo
        }
        
        public enum CompressionValue : short
        {
            None,
            XboxAdpcm,
            ImaAdpcm,
            Ogg
        }
        
        [TagStructure(Size = 0x48)]
        public class SoundPitchRangeBlock : TagStructure
        {
            /// <summary>
            /// the name of the imported pitch range directory
            /// </summary>
            [TagField(Length = 32)]
            public string Name;
            /// <summary>
            /// these settings control what pitches this set of samples represents. if there is only one pitch range, all three values
            /// are ignored.
            /// </summary>
            /// <summary>
            /// the apparent pitch when these samples are played at their recorded pitch.
            /// </summary>
            public float NaturalPitch;
            /// <summary>
            /// the range of pitches that will be represented using this sample. this should always contain the natural pitch.
            /// </summary>
            public Bounds<float> BendBounds;
            public short ActualPermutationCount;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            [TagField(Length = 0xC)]
            public byte[] Padding1;
            /// <summary>
            /// permutations represent equivalent variations of this sound.
            /// </summary>
            public List<SoundPermutationsBlock> Permutations;
            
            [TagStructure(Size = 0x7C)]
            public class SoundPermutationsBlock : TagStructure
            {
                /// <summary>
                /// name of the file from which this sample was imported
                /// </summary>
                [TagField(Length = 32)]
                public string Name;
                /// <summary>
                /// fraction of requests to play this permutation that are ignored (a different permutation is selected.)
                /// </summary>
                public float SkipFraction;
                /// <summary>
                /// fraction of recorded volume to play at.
                /// </summary>
                public float Gain;
                public CompressionValue Compression;
                public short NextPermutationIndex;
                [TagField(Length = 0x14)]
                public byte[] Padding;
                /// <summary>
                /// sampled sound data
                /// </summary>
                public byte[] Samples;
                public byte[] MouthData;
                public byte[] SubtitleData;
                
                public enum CompressionValue : short
                {
                    None,
                    XboxAdpcm,
                    ImaAdpcm,
                    Ogg
                }
            }
        }
    }
}

