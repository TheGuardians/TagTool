using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "cache_file_sound", Tag = "$#!+", Size = 0x24)]
    public class CacheFileSound : TagStructure
    {
        public SoundDefinitionFlags Flags;
        public SoundClassEnum SoundClass;
        public sbyte PitchRangeCount;
        public short CodecIndex;
        public short FirstPitchRangeIndex;
        public short FirstLanguageDurationPitchRangeIndex;
        public short RuntimeGestaltIndexStorage;
        public short SubPriority;
        public short PlaybackIndex;
        public short ScaleIndex;
        public sbyte PromotionIndex;
        public sbyte CustomPlaybackIndex;
        public short ExtraInfoIndex;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public int MaximumPlayTime; // ms
        public TagResourceReference SoundDataResource;
        
        [Flags]
        public enum SoundDefinitionFlags : ushort
        {
            FitToAdpcmBlocksize = 1 << 0,
            // always play as 3d sound, even in first person
            AlwaysSpatialize = 1 << 1,
            // disable occlusion/obstruction for this sound
            NeverObstruct = 1 << 2,
            InternalDonTTouch = 1 << 3,
            FacialAnimationDataStripped = 1 << 4,
            UseHugeSoundTransmission = 1 << 5,
            LinkCountToOwnerUnit = 1 << 6,
            PitchRangeIsLanguage = 1 << 7,
            DonTUseSoundClassSpeakerFlag = 1 << 8,
            DonTUseLipsyncData = 1 << 9,
            InstantSoundPropagation = 1 << 10,
            FakeSpatializationWithDistance = 1 << 11,
            // picks the first permutation randomly
            PlayPermutationsInOrder = 1 << 12
        }
        
        public enum SoundClassEnum : sbyte
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
    }
}
