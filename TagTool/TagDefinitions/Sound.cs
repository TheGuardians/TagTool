using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "sound", Tag = "snd!", Size = 0x20, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "sound", Tag = "snd!", Size = 0xD8, MinVersion = CacheVersion.HaloOnline106708)]
    public class Sound
    {
        public FlagsValue Flags;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public short Unknown1;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown2;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown3;
        
        public SoundClassValue SoundClass;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public SampleRateValue SampleRate;
        
        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public byte UniqueSoundCount;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short PlatformCodecIndex;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short PitchRangeIndex;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short LanguageBIndex;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short Unknown4;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short PlaybackParameterIndex;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short ScaleIndex;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public sbyte PromotionIndex;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public sbyte CustomPlaybackIndex;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short ExtraInfoIndex;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public int Unknown5;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public int ZoneAssetHandle;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public byte Unknown6;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public ImportTypeValue ImportType;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public SoundCacheFileGestalt.PlaybackParameter PlaybackParameters;
        
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public SoundCacheFileGestalt.Scale Scale;
        
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public SoundCacheFileGestalt.PlatformCodecBlock PlatformCodec;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public SoundCacheFileGestalt.Promotion Promotion;
        
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<SoundCacheFileGestalt.PitchRange> PitchRanges;
        
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<SoundCacheFileGestalt.CustomPlayback> CustomPlayBacks;
        
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<SoundCacheFileGestalt.ExtraInfoBlock> ExtraInfo;
        
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<SoundCacheFileGestalt.LanguageBlock> Languages;

        [TagField(Pointer = true, MinVersion = CacheVersion.HaloOnline106708)]
        public PageableResource Resource;
        
        [TagField(Padding = true, Length = 4, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;

        [TagField(MinVersion = CacheVersion.HaloOnline301003, MaxVersion = CacheVersion.HaloOnline449175)]
        public uint Unknown12;
        
        [Flags]
        public enum FlagsValue : ushort
        {
            None = 0,
            FitToAdpcmBlockSize = 1 << 0,
            SplitLongSoundIntoPermutations  = 1 << 1
        }
        
        public enum SoundClassValue : sbyte
        {
            ProjectileImpact,
            ProjectileDetonation,
            ProjectileFlyby,
            ProjectileDetonationLod,
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
            WeaponFireLod,
            WeaponFireLodFar,
            Unused2Impacts,
            UnitFootsteps,
            UnitDialog,
            UnitAnimation,
            UnitUnused,
            VehicleCollision,
            VehicleEngine,
            VehicleAnimation,
            VehicleEngineLod,
            DeviceDoor,
            DeviceUnused0,
            DeviceMachinery,
            DeviceStationary,
            DeviceUnused1,
            DeviceUnused2,
            Music,
            AmbientNature,
            AmbientMachinery,
            AmbientStationary,
            HugeAss,
            ObjectLooping,
            CinematicMusic,
            PlayerArmor,
            UnknownUnused1,
            AmbientFlock,
            NoPad,
            NoPadStationary,
            Arg,
            CortanaMission,
            CortanaGravemindChannel,
            MissionDialog,
            CinematicDialog,
            ScriptedCinematicFoley,
            Hud,
            GameEvent,
            Ui,
            Test,
            MultilingualTest,
            AmbientNatureDetails,
            AmbientMachineryDetails,
            InsideSurroundTail,
            OutsideSurroundTail,
            VehicleDetonation,
            AmbientDetonation,
            FirstPersonInside,
            FirstPersonOutside,
            FirstPersonAnywhere,
            UiPda
        }
        
        public enum SampleRateValue : sbyte
        {
            _22khz,
            _44khz,
            _32khz
        }
        
        public enum ImportTypeValue : sbyte
        {
            Unknown,
            SingleShot,
            SingleLayer,
            MultiLayer
        }
        
        public enum EncodingValue : sbyte
        {
            Mono,
            Stereo,
            Surround,
            _51Surround
        }
    }
}