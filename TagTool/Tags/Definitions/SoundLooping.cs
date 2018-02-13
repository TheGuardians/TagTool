using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_looping", Tag = "lsnd", Size = 0x40, MinVersion = CacheVersion.Halo3Retail)]
    public class SoundLooping
    {
        public uint Flags;
        public float MartySMusicTime;
        public float Unknown1;
        public float Unknown2;
        public float Unknown3;
        public CachedTagInstance Unused;
        public SoundClassValue SoundClass;
        public short Unknown4;
        public List<Track> Tracks;
        public List<DetailSound> DetailSounds;

        [Flags]
        public enum SoundClassValue : short
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
            UiPda,
        }

        [TagStructure(Size = 0x90, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0xA0, MinVersion = CacheVersion.Halo3ODST)]
        public class Track
        {
            public StringId Name;
            public uint Flags;
            public float Gain;
            public float FadeInDuration;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public uint Unknown1;

            public float FadeOutDuration;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public short Unknown2;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public short Unknown3;

            public CachedTagInstance In;
            public CachedTagInstance Loop;
            public CachedTagInstance Out;
            public CachedTagInstance AlternateLoop;
            public CachedTagInstance AlternateOut;
            public OutputEffectValue OutputEffect;
            public short Unknown4;
            public CachedTagInstance AlternateTransitionIn;
            public CachedTagInstance AlternateTransitionOut;

            public float AlternateCrossfadeDuration;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float Unknown5;

            public float AlternateFadeOutDuration;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float Unknown6;

            public enum OutputEffectValue : short
            {
                None,
                OutputFrontSpeakers,
                OutputRearSpeakers,
                OutputCenterSpeakers,
            }
        }

        [TagStructure(Size = 0x3C)]
        public class DetailSound
        {
            public StringId Name;
            public CachedTagInstance Sound;
            public Bounds<float> RandomPeriodBounds;
            public float Unknown;
            public uint Flags;
            public Bounds<Angle> YawBounds;
            public Bounds<Angle> PitchBounds;
            public Bounds<float> DistanceBounds;
            
        }
    }
}