using System;
using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x1)]
    public class SoundClass : TagStructure
    {
        [TagField(MinVersion = CacheVersion.HaloXbox, MaxVersion = CacheVersion.HaloCustomEdition)]
        public SoundClassHalo Halo;

        [TagField(MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
        public SoundClassHalo2 Halo2;

        [TagField(MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.Halo3Retail)]
        public SoundClassHalo3Retail Halo3Retail;

        [TagField(CacheVersion.Halo3ODST)]
        public SoundClassHalo3ODST Halo3ODST;

        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public SoundClassHaloOnline HaloOnline;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public SoundClassHaloReach HaloReach;

        public enum SoundClassHalo2 : sbyte
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
            Unused1Impact,
            Unused2Impact,
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
            UnknownUnused2,
            UnknownUnused3,
            UnknownUnused4,
            MissionUnused0,
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

        public enum SoundClassHalo3Retail : sbyte
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
            Unused1Impact,
            Unused2Impact,
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
            MissionUnused0,
            CortanaMission,
            CortanaGravemindChannel,
            MissionDialog,
            CinematicDialog,
            ScriptedCinematicFoley,
            GameEvent,
            Ui,
            Test,
            MultilingualTest
        }

        public enum SoundClassHalo3ODST : sbyte
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
            Unused1Impacts,
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
            UnknownUnused0,
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

        public enum SoundClassHaloOnline : sbyte
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

        public enum SoundClassHaloReach : sbyte
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
            WaterTransition,
            LowPassEffect,
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
            MultiplayerDialogue,
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
            UnitFootstepPostpone,
            WeaponReadyThirdPerson,
            UiMusic
        }

        public enum SoundClassHalo : int
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
            Unused1Impact,
            Unused2Impact,
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
            UnknownUnused2,
            UnknownUnused3,
            UnknownUnused4,
            MissionUnused0,
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

        public SoundClass ConvertSoundClass(CacheVersion from)
        {
            string value = null;

            switch (from)
            {
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3Beta:
                    value = Halo3Retail.ToString();
                    break;

                case CacheVersion.Halo3ODST:
                    value = Halo3ODST.ToString();
                    break;

                case CacheVersion.HaloReach:
                    value = HaloReach.ToString();
                    break;
            }

            // Fix class to match to equivalent ones

            if (value.Equals("Unused1Impacts") || value.Equals("WaterTransition"))
                value = "WeaponFireLodFar";

            else if (value.Equals("LowPassEffect"))
                value = "Unused2Impacts";

            else if (value.Equals("UnknownUnused0"))
                value = "PlayerArmor";

            else if (value.Equals("MultiplayerDialogue"))
                value = "MultilingualTest";

            else if (value.Equals("UiMusic"))
                value = "UiPda";

            // Fixups for Reach new sound classes (might be better to port them later on)

            else if (value.Equals("SpaceProjectileDetonation"))
                value = "ProjectileDetonation";

            else if (value.Equals("SpaceProjectileFlyby"))
                value = "ProjectileFlyby";

            else if (value.Equals("SpaceVehicleEngine"))
                value = "VehicleEngine";

            else if (value.Equals("SpaceWeaponFire"))
                value = "WeaponFire";

            else if (value.Equals("PlayerVoiceTeam"))
                value = "UnitDialog";

            else if (value.Equals("PlayerVoiceProxy"))
                value = "UnitDialog";

            else if (value.Equals("ProjectileImpactPostpone"))
                value = "ProjectileImpact";

            else if (value.Equals("UnitFootstepPostpone"))
                value = "UnitFootsteps";

            else if (value.Equals("WeaponReadyThirdPerson"))
                value = "WeaponReady";

            // Fix the surround sound class

            else if (value.Equals("FirstPersonInside"))
                value = "InsideSurroundTail";

            else if (value.Equals("FirstPersonOutside"))
                value = "OutsideSurroundTail";

            if (value == null || !Enum.TryParse(value, out HaloOnline))
            {
                throw new NotImplementedException($"Failed to parse sound class {value}");
            }

            return this;
        }
    }
}