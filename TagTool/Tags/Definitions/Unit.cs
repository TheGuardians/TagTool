using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "unit", Tag = "unit", Size = 0x130, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Name = "unit", Tag = "unit", Size = 0x214, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "unit", Tag = "unit", Size = 0x224, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "unit", Tag = "unit", Size = 0x2C8, MinVersion = CacheVersion.HaloOnlineED)]
    public class Unit : GameObject
    {
        public UnitFlagBits UnitFlags; // int
        public DefaultTeamValue DefaultTeam; // short
        public ConstantSoundVolumeValue ConstantSoundVolume; // short

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public CachedTag HologramUnit;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<CampaignMetagameBucket> CampaignMetagameBuckets;

        public CachedTag IntegratedLightToggle;
        public Angle CameraFieldOfView; // degrees
        public float CameraStiffness;
        public UnitCamera Camera;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public UnitCamera SyncActionCamera;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public UnitAssassination Assassination;

        public UnitSeatAcceleration SeatAcceleration;
        public float SoftPingThreshold; // [0,1]
        public float SoftPingInterruptTime; // seconds
        public float HardPingThreshold; // [0,1]
        public float HardPingInterruptTime; // seconds

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        //[TagField(Platform = CachePlatform.MCC, MinVersion = CacheVersion.Halo3Retail)]
        public float HardDeathThreshold; // [0,1]

        public float FeignDeathThreshold; // [0,1]
        public float FeignDeathTime; // seconds
        public float DistanceOfEvadeAnimation; // this must be set to tell the AI how far it should expect our dive animation to move us
        public float DistanceOfDiveAnimation;
        public float StunnedMovementThreshold; // [0,1] if we take this much damage in a short space of time we will play our 'stunned movement' animations
        public float FeignDeathChance; // [0,1]
        public float FeignRepeatChance; // [0,1]
        public CachedTag SpawnedTurretCharacter; // automatically created character when this unit is driven
        public Bounds<short> SpawnedActorCountBounds; // number of actors which we spawn
        public float SpawnedVelocity; // velocity at which we throw spawned actors
        public Angle AimingVelocityMaximum; // degrees per second
        public Angle AimingAccelerationMaximum; // degrees per second squared
        public float CasualAimingModifier; // [0,1]
        public Angle LookingVelocityMaximum; // degrees per second
        public Angle LookingAccelerationMaximum; // degrees per second squared
        public StringId RightHandNode; // where the primary weapon is attached
        public StringId LeftHandNode; // where the seconday weapon is attached (for dual-pistol modes)
        public StringId PreferredGunNode; // if found, use this gun marker
        public CachedTag MeleeDamage;
        public CachedTag BoardingMeleeDamage;
        public CachedTag BoardingMeleeResponse;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public CachedTag EvictionMeleeDamage;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public CachedTag EvictionMeleeResponse;

        public CachedTag LandingMeleeDamage;
        public CachedTag FlurryMeleeDamage;
        public CachedTag ObstacleSmashMeleeDamage;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public CachedTag ShieldPopDamage;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public CachedTag AssassinationDamage;

        public MotionSensorBlipSizeValue MotionSensorBlipSize; // short
        public UnitItemOwnerSizeValue ItemOwnerSize; // short
        public List<Posture> Postures;
        public List<HudInterface> HudInterfaces;
        public List<DialogueVariant> DialogueVariants;

        /// <summary>
        /// If the player is in a seat in this unit then modify the motion tracker range by this amount.
        /// See base value in player globals for how this modifier is applied.
        /// </summary>
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float MotionTrackerRangeModifier; //If the player is in a seat in this unit, modify the motion tracker range by this amount. (see base value in player globals)

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public Angle GrenadeAngle;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public Angle GrenadeAngleMaxElevation;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public Angle GrenadeAngleMinElevation;

        public float GrenadeVelocity; // world units per second
        public GrenadeTypeValue GrenadeType; // short
        public ushort GrenadeCount;
        public List<PoweredSeat> PoweredSeats;
        public List<UnitWeapon> Weapons;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public List<TargetTrackingBlock> TargetTracking;

        public List<UnitSeat> Seats;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public float EmpRadius;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public CachedTag EmpEffect;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public CachedTag BoostCollisionDamage;

        public float BoostPeakPower;
        public float BoostRisePower;
        public float BoostPeakTime;
        public float BoostFallPower;
        public float BoostDeadTime;
        public float LipsyncAttackWeight;
        public float LipsyncDecayWeight;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public CachedTag DetachDamage;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public CachedTag DetachedWeapon;

        [Flags]
        public enum UnitFlagBits : int
        {
            CircularAiming = 1 << 0,
            DestroyedAfterDying = 1 << 1,
            HalfSpeedInterpolation = 1 << 2,
            FiresFromCamera = 1 << 3,
            EntranceInsideBoundingSphere = 1 << 4,
            DoesntShowReadiedWeapon = 1 << 5,
            CausesPassengerDialogue = 1 << 6,
            ResistsPings = 1 << 7,
            MeleeAttackIsFatal = 1 << 8,
            DontRefaceDuringPings = 1 << 9,
            HasNoAiming = 1 << 10,
            SimpleCreature = 1 << 11,
            ImpactMeleeAttachesToUnit = 1 << 12,
            ImpactMeleeDiesOnShield = 1 << 13,
            CannotOpenDoorsAutomatically = 1 << 14,
            MeleeAttackersCannotAttach = 1 << 15,
            NotInstantlyKilledByMelee = 1 << 16,
            ShieldSapping = 1 << 17,
            RunsAroundFlaming = 1 << 18,
            Inconsequential = 1 << 19,
            SpecialCinematicUnit = 1 << 20,
            IgnoredByAutoaiming = 1 << 21,
            ShieldsFryInfectionForms = 1 << 22,
            CanDualWield = 1 << 23,
            ActsAsGunnerForParent = 1 << 24,
            ControlledByParentGunner = 1 << 25,
            ParentsPrimaryWeapon = 1 << 26,
            ParentsSecondaryWeapon = 1 << 27,
            UnitHasBoost = 1 << 28,
            AllowAimWhileOpeningOrClosing = 1 << 29 // in MCC, needs testing
        }

        public enum DefaultTeamValue : short
        {
            Default,
            Player,
            Human,
            Covenant,
            Flood,
            Sentinel,
            Heretic,
            Prophet,
            Guilty,
            Unused9,
            Unused10,
            Unused11,
            Unused12,
            Unused13,
            Unused14,
            Unused15
        }

        public enum ConstantSoundVolumeValue : short
        {
            Silent,
            Medium,
            Loud,
            Shout,
            Quiet
        }

        [TagStructure(Size = 0x8)]
        public class CampaignMetagameBucket : TagStructure
        {
            public CampaignMetagameBucketFlags Flags;
            public CampaignMetagameBucketType UnitType;
            public UnitClassification UnitClass;

            [TagField(Length = 1, Flags = Padding)]
            public byte[] Padding0;

            public short PointCount;

            [TagField(Length = 2, Flags = Padding)]
            public byte[] Padding1;

            [Flags]
            public enum CampaignMetagameBucketFlags : byte
            {
                OnlyCountsWithRiders = 1 << 0
            }

            public enum CampaignMetagameBucketType : sbyte
            {
                Brute,
                Grunt,
                Jackal,
                Marine,
                Bugger,
                Hunter,
                FloodInfection,
                FloodCarrier,
                FloodCombat,
                FloodPureform,
                Sentinel,
                Elite,
                Turret,
                Mongoose,
                Warthog,
                Scorpion,
                Hornet,
                Pelican,
                Shade,
                Watchtower,
                Ghost,
                Chopper,
                Mauler,
                Wraith,
                Banshee,
                Phantom,
                Scarab,
                Guntower,
                Engineer,
                EngineerRechargeStation
            }

            public enum UnitClassification : sbyte
            {
                Infantry,
                Leader,
                Hero,
                Specialist,
                LightVehicle,
                HeavyVehicle,
                GiantVehicle,
                StandardVehicle
            }
        }

        [Flags]
        public enum UnitCameraFlagBits : ushort
        {
            PitchBoundsAbsoluteSpace = 1 << 0,
            OnlyCollidesWithEnvironment = 1 << 1,
            HidesPlayerUnitFromCamera = 1 << 2,
            UseAimingVectorInsteadOfMarkerForward = 1 << 3
        }

        [TagStructure(Size = 0x1C, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x3C, MinVersion = CacheVersion.Halo3Retail)]
        public class UnitCamera : TagStructure
        {
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public UnitCameraFlagBits CameraFlags;

            [TagField(Flags = Padding, Length = 2, MinVersion = CacheVersion.Halo3Retail)]
            public byte[] Padding0;

            public StringId CameraMarkerName;
            public StringId CameraSubmergedMarkerName;
            public Angle PitchAutoLevel;
            public Bounds<Angle> PitchRange;
            public List<UnitCameraTrack> CameraTracks;

            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public Bounds<float> PitchSpringBounds;

            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public Angle SpringVelocity;

            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public List<UnitCameraAcceleration> CameraAcceleration;
        }

        [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3Retail)]
        public class UnitCameraTrack : TagStructure
        {
            public CachedTag CameraTrack;
        }

        [TagStructure(Size = 0x4C)]
        public class UnitCameraAcceleration : TagStructure
        {
            public UnitCameraAxisAcceleration VelocityI;
            public UnitCameraAxisAcceleration VelocityJ;
            public UnitCameraAxisAcceleration VelocityK;
            public float MaximumCameraVelocity; // this preceded the UnitCameraAxisAcceleration structs. moved to match MCC and needs to be checked.
        }

        [TagStructure(Size = 0x18)]
        public class UnitCameraAxisAcceleration : TagStructure
        {
            public float K;
            public float Scale;
            public float Power;
            public float Maximum;
            public float CameraScaleAxial; // scale factor used when this acceleration component is along the axis of the forward vector of the camera
            public float CameraScalePerpendicular; // scale factor used when this acceleration component is perpendicular to the camera
        }

        [TagStructure(Size = 0x1C, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x2C, MinVersion = CacheVersion.Halo3Retail)]
        public class UnitAssassination : TagStructure
        {
            public CachedTag Response;
            public CachedTag Weapon;
            public StringId ToolStowAnchor;
            public StringId ToolHandMarker;
            public StringId ToolMarker;
        }

        [TagStructure(Size = 0x14)]
        public class UnitSeatAcceleration : TagStructure
        {
            public RealVector3d Range; // world units per second squared
            public float ActionScale; // actions fail [0,1+]
            public float AttachScale; // detach unit [0,1+]
        }

        public enum MotionSensorBlipSizeValue : short
        {
            Medium,
            Small,
            Large
        }

        public enum UnitItemOwnerSizeValue : short
        {
            Small,
            Medium,
            Player,
            Large
        }

        [TagStructure(Size = 0x10)]
        public class Posture : TagStructure
		{
            public StringId Name;
            public RealVector3d PillOffset;
        }

        [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3Retail)]
        public class HudInterface : TagStructure
		{
            public CachedTag UnitHudInterface;
        }

        [TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3Retail)]
        public class DialogueVariant : TagStructure
		{
            public short VariantNumber;

            [TagField(Length = 2, Flags = Padding)]
            public byte[] Padding0;

            public CachedTag Dialogue;
        }

        public enum GrenadeTypeValue : short
        {
            HumanFragmentation,
            CovenantPlasma,
            BruteSpike,
            Firebomb,
        }

        [TagStructure(Size = 0x8)]
        public class PoweredSeat : TagStructure
		{
            public float DriverPowerupTime; // seconds
            public float DriverPowerdownTime; // seconds
        }

        [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3Retail)]
        public class UnitWeapon : TagStructure
		{
            public CachedTag Weapon;
        }

        [TagStructure(Size = 0x38, MinVersion = CacheVersion.Halo3Retail)]
        public class TargetTrackingBlock : TagStructure
		{
            public List<TrackingType> TrackingTypes;
            public float AcquireTime;
            public float GraceTime;
            public float DecayTime;
            public CachedTag TrackingSound;
            public CachedTag LockedSound;

            [TagStructure(Size = 0x4, MinVersion = CacheVersion.Halo3Retail)]
            public class TrackingType : TagStructure
			{
                public StringId TrackingType2;
            }
        }

        [Flags]
        public enum UnitSeatFlags : int
        {
            Invisible = 1 << 0,
            Locked = 1 << 1,
            Driver = 1 << 2,
            Gunner = 1 << 3,
            ThirdPersonCamera = 1 << 4,
            AllowsWeapons = 1 << 5,
            ThirdPersonOnEnter = 1 << 6,
            FirstPersonCameraSlavedToGun = 1 << 7,
            AllowVehicleCommunicationAnimations = 1 << 8,
            NotValidWithoutDriver = 1 << 9,
            AllowAiNonCombatants = 1 << 10,
            BoardingSeat = 1 << 11,
            AiFiringDisabledByMaxAcceleration = 1 << 12,
            BoardingEntersSeat = 1 << 13,
            BoardingNeedAnyPassenger = 1 << 14,
            ControlsOpenAndClose = 1 << 15,
            InvalidForPlayer = 1 << 16,
            InvalidForNonPlayer = 1 << 17,
            GunnerPlayerOnly = 1 << 18,
            InvisibleUnderMajorDamage = 1 << 19,
            MeleeInstantKillable = 1 << 20,
            LeaderPreference = 1 << 21,
            AllowsExitAndDetach = 1 << 22,
            BlocksHeadshots = 1 << 23, // h3ek flags from here down. unknown if functional in HO
            ExitsToGround = 1 << 24,
            ClosesEarlyOpensLate = 1 << 25,
            ForwardFromAttachment = 1 << 26,
            DisallowAIShooting = 1 << 27,
            ClosesEarlyOpensEarly = 1 << 28,
            ClosesLateOpensLate = 1 << 29,
            PreventsWeaponStowing = 1 << 30
        }

        [TagStructure(Size = 0xB0, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0xE4, MinVersion = CacheVersion.Halo3Retail)]
        public class UnitSeat : TagStructure
		{
            public UnitSeatFlags Flags; // int
            public StringId Label; // formerly SeatAnimation
            public StringId SeatMarkerName;
            public StringId EntryMarkerName;
            public StringId BoardingGrenadeMarker;
            public StringId BoardingGrenadeString;
            public StringId BoardingMeleeString;

            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public StringId SeatedDetachString; //formerly DetachWeaponString

            public float PingScale; // nathan is too lazy to make pings for each seat.
            public float FlippedEvictionTime; // how much time it takes to evict a rider from a flipped vehicle
            public UnitSeatAcceleration SeatAcceleration;
            public float AiScariness;
            public AiSeatTypeValue AiSeatType; // short
            public short BoardingSeat;
            public float ListenerInterpolationFactor; // how far to interpolate listener position from camera to occupant's head

            public Bounds<float> YawRateBounds; // degrees per second
            public Bounds<float> PitchRateBounds; // degrees per second

            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public float PitchInterpolationTime; // (seconds) 0 means use default 17

            public Bounds<float> SpeedReferenceBounds;
            public float SpeedExponent;
            public UnitCamera Camera;
            public List<UnitHudInterfaceBlock> UnitHudInterface;
            public StringId EnterSeatString;
            public Bounds<Angle> YawRange;
            public CachedTag BuiltInGunner;
            public float EntryRadius; // how close to the entry marker a unit must be
            public Angle EntryMarkerConeAngle; // angle from marker forward the unit must be
            public Angle EntryMarkerFacingAngle; // angle from unit facing the marker must be
            public float MaximumRelativeVelocity;
            public StringId InvisibleSeatRegion;
            public int RuntimeInvisibleSeatRegionIndex;

            public enum AiSeatTypeValue : short
            {
                None,
                Passenger,
                Gunner,
                SmallCargo,
                LargeCargo,
                Driver
            }

            [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo2Vista)]
            [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3Retail)]
            public class UnitHudInterfaceBlock : TagStructure
			{
                public CachedTag UnitHudInterface;
            }
        }
    }
}