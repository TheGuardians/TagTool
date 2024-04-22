using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "sound_looping", Tag = "lsnd", Size = 0x3C)]
    public class SoundLooping : TagStructure
    {
        public LoopingSoundFlags Flags;
        public Bounds<float> MartySMusicTime; // seconds
        public Bounds<float> RuntimeDistanceBounds;
        public float MaximumFlybyRangeDistance;
        public SoundClassEnum RuntimeSoundClass;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        // This track's markers, flags, gain and fade settings are copied to the other tracks. Its gain and fadeout settings
        // are also copied to the details. If you want to change the master track use the sound-looping tool command
        public StringId MasterMarkerTrack;
        public float MaximumRadius;
        // tracks play in parallel and loop continuously for the duration of the looping sound.
        public List<LoopingSoundTrackBlock> Tracks;
        // detail sounds play at random throughout the duration of the looping sound.
        public List<LoopingSoundDetailBlock> DetailSounds;
        
        [Flags]
        public enum LoopingSoundFlags : uint
        {
            // when used as a background stereo track, causes nearby AIs to be unable to hear
            DeafeningToAis = 1 << 0,
            // this is a collection of permutations strung together that should play once then stop.
            NotALoop = 1 << 1,
            // all other music loops will stop when this one starts.
            StopsMusic = 1 << 2,
            // always play as 3d sound, even in first person
            AlwaysSpatialize = 1 << 3,
            // synchronizes playback with other looping sounds attached to the owner of this sound
            SynchronizeWithOwner = 1 << 4,
            SynchronizeTracks = 1 << 5,
            FakeSpatializationWithDistance = 1 << 6,
            CombineAll3dPlayback = 1 << 7,
            // like a laser blast
            PersistentFlyby = 1 << 8,
            DonTApplyRandomSpatializationToDetails = 1 << 9,
            // you need to reimport the sound_looping for this to take effect
            AllowMarkerStitching = 1 << 10,
            // don't delay retrying the looping sound, in case the bank is loaded now
            DonTDelayRetries = 1 << 11,
            // Look to the parent of the vehicle. Only works on vehicles. Duh
            UseVehicleParentForPlayerness = 1 << 12,
            // looping_sound_speed
            ImplicitSpeedRptc = 1 << 13
        }
        
        public enum SoundClassEnum : short
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
            WaterTransitions,
            LowpassEffects,
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
            UnknownUnused0,
            UnknownUnused1,
            AmbientFlock,
            NoPad,
            NoPadStationary,
            EquipmentEffect,
            MissionDialog,
            CinematicDialog,
            ScriptedCinematicFoley,
            GameEvent,
            Ui,
            Test,
            MultiplayerDialog,
            AmbientNatureDetails,
            AmbientMachineryDetails,
            InsideSurroundTail,
            OutsideSurroundTail,
            VehicleDetonation,
            AmbientDetonation,
            FirstPersonInside,
            FirstPersonOutside,
            FirstPersonAnywhere,
            SpaceProjectileDetonation,
            SpaceProjectileFlyby,
            SpaceVehicleEngine,
            SpaceWeaponFire,
            PlayerVoiceTeam,
            PlayerVoiceProxy,
            ProjectileImpactPostpone,
            UnitFootstepsPostpone,
            WeaponReadyThirdPerson,
            UiMusic
        }
        
        [TagStructure(Size = 0xB0)]
        public class LoopingSoundTrackBlock : TagStructure
        {
            public StringId Name;
            public LoopingSoundTrackFlags Flags;
            public SoundEffectsEnum OutputEffect;
            public float Gain; // dB
            [TagField(ValidTags = new [] { "sndo","snd!" })]
            public CachedTag In;
            [TagField(ValidTags = new [] { "sndo","snd!" })]
            public CachedTag Loop;
            [TagField(ValidTags = new [] { "sndo","snd!" })]
            public CachedTag Out;
            [TagField(ValidTags = new [] { "sndo","snd!" })]
            public CachedTag AltLoop;
            [TagField(ValidTags = new [] { "sndo","snd!" })]
            public CachedTag AltOut;
            [TagField(ValidTags = new [] { "sndo","snd!" })]
            public CachedTag AltTransIn;
            [TagField(ValidTags = new [] { "sndo","snd!" })]
            public CachedTag AltTransOut;
            public float FadeInDuration; // seconds
            public LoopingSoundFadeModes FadeInMode;
            public float FadeOutDuration; // seconds
            public LoopingSoundFadeModes FadeOutMode;
            public float AltCrossfadeDuration; // seconds
            public LoopingSoundFadeModes AltCrossfadeMode;
            public float AltFadeOutDuration; // seconds
            public LoopingSoundFadeModes AltFadeOutMode;
            public LoopingSoundLayers Layer;
            public LoopingSoundTrackLayerFlags LayerFlags;
            public float LayerFadeInDuration; // seconds
            public LoopingSoundFadeModes LayerFadeInMode;
            public float LayerFadeOutDuration; // seconds
            public LoopingSoundFadeModes LayerFadeOutMode;
            
            [Flags]
            public enum LoopingSoundTrackFlags : ushort
            {
                // the loop sound should fade in while the start sound is playing.
                FadeInAtStart = 1 << 0,
                // the loop sound should fade out while the stop sound is playing.
                FadeOutAtStop = 1 << 1,
                // the alt loop sound should fade out while the alt stop sound is playing.
                FadeOutAtAltStop = 1 << 2,
                // crossfade when switching between alt loop and loop.
                CrossfadeAltLoop = 1 << 3,
                MakeFadesWaitForMarkers = 1 << 4,
                MasterSurroundSoundTrack = 1 << 5
            }
            
            public enum SoundEffectsEnum : short
            {
                None,
                OutputFrontSpeakers,
                OutputRearSpeakers,
                OutputCenterSpeakers
            }
            
            public enum LoopingSoundFadeModes : int
            {
                Default,
                Linear,
                EqualPower,
                InversePower,
                SCurve
            }
            
            public enum LoopingSoundLayers : short
            {
                None,
                _1,
                _2,
                _3,
                _4
            }
            
            [Flags]
            public enum LoopingSoundTrackLayerFlags : ushort
            {
                MakeLayerWaitForMarkers = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x48)]
        public class LoopingSoundDetailBlock : TagStructure
        {
            public StringId Name;
            [TagField(ValidTags = new [] { "sndo","snd!" })]
            public CachedTag Sound;
            public LoopingSoundDetailPeriodTypeEnum PeriodType;
            // the time between successive playings of this sound will be randomly selected from this range.
            public Bounds<float> RandomPeriodBounds; // seconds
            public float DetailGain; // dB
            public float FadeOutDuration; // seconds
            public LoopingSoundFadeModes FadeOutMode;
            public LoopingSoundDetailFlags Flags;
            // the sound's position along the horizon will be randomly selected from this range.
            public Bounds<Angle> YawBounds; // degrees
            // the sound's position above (positive values) or below (negative values) the horizon will be randomly selected from
            // this range.
            public Bounds<Angle> PitchBounds; // degrees
            // the sound's distance (from its spatialized looping sound or from the listener if the looping sound is stereo) will
            // be randomly selected from this range.
            public Bounds<float> DistanceBounds; // world units
            
            public enum LoopingSoundDetailPeriodTypeEnum : int
            {
                IgnoresPlaybackTime,
                RelativeToEndOfPlayback
            }
            
            public enum LoopingSoundFadeModes : int
            {
                Default,
                Linear,
                EqualPower,
                InversePower,
                SCurve
            }
            
            [Flags]
            public enum LoopingSoundDetailFlags : uint
            {
                DonTPlayWithAlternate = 1 << 0,
                DonTPlayWithoutAlternate = 1 << 1,
                StartImmediatelyWithLoop = 1 << 2,
                InheritScaleFromLoop = 1 << 3,
                DonTFadeWithLoop = 1 << 4
            }
        }
    }
}
