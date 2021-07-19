using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_looping", Tag = "lsnd", Size = 0x40, MinVersion = CacheVersion.Halo3Retail)]
    public class SoundLooping : TagStructure
	{
        public SoundLoopingFlags Flags;
        public Bounds<float> MartySMusicTime;
        public Bounds<float> DistanceBounds;
        public CachedTag Unused;
        public SoundClassValue SoundClass;
        public short LoopTypeHO;
        public List<Track> Tracks;
        public List<DetailSound> DetailSounds;

        [Flags]
        public enum SoundLoopingFlags : int
        {
            /// <summary>
            /// When used as a background stereo track, causes nearby AIs to be unable to hear
            /// </summary>
            DeafeningToAIs = 1 << 0,

            /// <summary>
            /// This is a collection of permutations strung together that should play once then stop.
            /// </summary>
            NotALoop = 1 << 1,

            /// <summary>
            /// All other music loops will stop when this one starts.
            /// </summary>
            StopsMusic = 1 << 2,

            /// <summary>
            /// Always play as 3d sound, even in first person
            /// </summary>
            AlwaysSpatialize = 1 << 3,

            /// <summary>
            /// Aynchronizes playback with other looping sounds attached to the owner of this sound
            /// </summary>
            SynchronizePlayback = 1 << 4,

            SynchronizeTracks = 1 << 5,

            FakeSpatializationWithDistance = 1 << 6,

            CombineAll3dPlayback = 1 << 7,

            /// <summary>
            /// Like a laser blast
            /// </summary>
            PersistentFlyby = 1 << 8,

            DontApplyRandomSpatializationToDetails = 1 << 9,

            /// <summary>
            /// You need to reimport the sound_looping for this to take effect
            /// </summary>
            AllowMarkerStitching = 1 << 10,

            /// <summary>
            /// Don't delay retrying the looping sound, in case the bank is loaded now
            /// </summary>
            DontDelayRetries = 1 << 11,

            /// <summary>
            /// Look to the parent of the vehicle. Only works on vehicles. Duh
            /// </summary>
            UseVehicleParentForPlayerness = 1 << 12,

            /// <summary>
            /// Looping sound speed
            /// </summary>
            ImplicitSpeedRPTC = 1 << 13,

            HasOptionalPlayerSound = 1 << 14,

            DontOcclude = 1 << 15,

            /// <summary>
            /// Use the opposite of the decision for whether to apply focus feature based on whether the sound is 2D or 3D
            /// </summary>
            NegateRadiusBasedFocus = 1 << 16
        }

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
        public class Track : TagStructure
		{
            public StringId Name;
            public LsndTrackFlags Flags;
            public float Gain;
            public float FadeInDuration;

            [Flags]
            public enum LsndTrackFlags : uint
            {
                None = 0,
                FadeInAtStart = 1 << 0,
                FadeOutAtStop = 1 << 1,
                CrossfadeAlternateLoop = 1 << 2,
                MasterSurroundSoundTrack = 1 << 3,
                FadeOutAlternateStop = 1 << 4,
                Bit5 = 1 << 5,
                Bit6 = 1 << 6,
                Bit7 = 1 << 7
            }

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public SoundFadeMode FadeInMode;

            [TagField(MinVersion = CacheVersion.Halo3ODST, Flags = TagFieldFlags.Padding, Length = 2)]
            public byte[] Padding1;

            public float FadeOutDuration;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public SoundFadeMode FadeOutMode;

            [TagField(MinVersion = CacheVersion.Halo3ODST, Flags = TagFieldFlags.Padding, Length = 2)]
            public byte[] Padding2;

            public CachedTag In;
            public CachedTag Loop;
            public CachedTag Out;
            public CachedTag AlternateLoop;
            public CachedTag AlternateOut;
            public OutputEffectValue OutputEffect;
            public short Unknown4;
            public CachedTag AlternateTransitionIn;
            public CachedTag AlternateTransitionOut;

            public float AlternateCrossfadeDuration;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public SoundFadeMode AlternateCrossfadeMode;


            [TagField(MinVersion = CacheVersion.Halo3ODST, Flags = TagFieldFlags.Padding, Length = 2)]
            public byte[] Padding3;

            public float AlternateFadeOutDuration;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public SoundFadeMode AlternateFadeOutMode;

            [TagField(MinVersion = CacheVersion.Halo3ODST, Flags = TagFieldFlags.Padding, Length = 2)]
            public byte[] Padding4;

            public enum OutputEffectValue : short
            {
                None,
                OutputFrontSpeakers,
                OutputRearSpeakers,
                OutputCenterSpeakers,
            }

            public enum SoundFadeMode : short
            {
                None,
                Linear,
                Power,
                InversePower,
                EaseInOut
            }
        }

        [TagStructure(Size = 0x3C)]
        public class DetailSound : TagStructure
		{
            public StringId Name;
            public CachedTag Sound;
            public Bounds<float> RandomPeriodBounds;
            public float Unknown;
            public uint Flags;
            public Bounds<Angle> YawBounds;
            public Bounds<Angle> PitchBounds;
            public Bounds<float> DistanceBounds;
        }
    }
}