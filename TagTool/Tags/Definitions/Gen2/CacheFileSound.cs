using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "cache_file_sound", Tag = "$#!+", Size = 0x14)]
    public class CacheFileSound : TagStructure
    {
        public FlagsValue Flags;
        public SoundClassValue SoundClass;
        public SampleRateValue SampleRate;
        public EncodingValue Encoding;
        public CompressionValue Compression;
        public short PlaybackIndex;
        public short FirstPitchRangeIndex;
        public sbyte PitchRangeCount;
        public sbyte ScaleIndex;
        public sbyte PromotionIndex;
        public sbyte CustomPlaybackIndex;
        public short ExtraInfoIndex;
        public int MaximumPlayTime; // ms
        
        [Flags]
        public enum FlagsValue : ushort
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
        
        public enum SoundClassValue : sbyte
        {
            ProjectileImpact,
            ProjectileDetonation,
            ProjectileFlyby,
            Unknown,
            WeaponFire,
            WeaponReady,
            WeaponReload,
            WeaponEmpty,
            WeaponCharge,
            WeaponOverheat,
            WeaponIdle,
            WeaponMelee,
            WeaponAnimation,
            ObjectImpacts,
            ParticleImpacts,
            Unknown1,
            Unknown2,
            Unknown3,
            UnitFootsteps,
            UnitDialog,
            UnitAnimation,
            Unknown4,
            VehicleCollision,
            VehicleEngine,
            VehicleAnimation,
            Unknown5,
            DeviceDoor,
            Unknown6,
            DeviceMachinery,
            DeviceStationary,
            Unknown7,
            Unknown8,
            Music,
            AmbientNature,
            AmbientMachinery,
            Unknown9,
            HugeAss,
            ObjectLooping,
            CinematicMusic,
            Unknown10,
            Unknown11,
            Unknown12,
            Unknown13,
            Unknown14,
            Unknown15,
            CortanaMission,
            CortanaCinematic,
            MissionDialog,
            CinematicDialog,
            ScriptedCinematicFoley,
            GameEvent,
            Ui,
            Test,
            MultilingualTest
        }
        
        public enum SampleRateValue : sbyte
        {
            _22khz,
            _44khz,
            _32khz
        }
        
        public enum EncodingValue : sbyte
        {
            Mono,
            Stereo,
            Codec
        }
        
        public enum CompressionValue : sbyte
        {
            NoneBigEndian,
            XboxAdpcm,
            ImaAdpcm,
            NoneLittleEndian,
            Wma
        }
    }
}

