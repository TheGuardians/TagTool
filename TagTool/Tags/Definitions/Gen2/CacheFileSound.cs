using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
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
            AlwaysSpatialize = 1 << 2,
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
            Unknown3,
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
            Unknown15,
            Unknown16,
            Unknown17,
            UnitFootsteps,
            UnitDialog,
            UnitAnimation,
            Unknown21,
            VehicleCollision,
            VehicleEngine,
            VehicleAnimation,
            Unknown25,
            DeviceDoor,
            Unknown27,
            DeviceMachinery,
            DeviceStationary,
            Unknown30,
            Unknown31,
            Music,
            AmbientNature,
            AmbientMachinery,
            Unknown35,
            HugeAss,
            ObjectLooping,
            CinematicMusic,
            Unknown39,
            Unknown40,
            Unknown41,
            Unknown42,
            Unknown43,
            Unknown44,
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

