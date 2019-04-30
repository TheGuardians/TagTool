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
    [TagStructure(Name = "unit", Tag = "unit", Size = 0x2C8, MinVersion = CacheVersion.HaloOnline106708)] 
    public abstract class Unit : GameObject
    {
        public UnitFlagBits UnitFlags; // int
        public DefaultTeamValue DefaultTeam; // short
        public ConstantSoundVolumeValue ConstantSoundVolume; // short

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public CachedTagInstance HologramUnit;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<MetagameProperty> MetagameProperties;

        public CachedTagInstance IntegratedLightToggle;
        public Angle CameraFieldOfView;
        public float CameraStiffness;

        public UnitCamera Camera;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public UnitCamera SyncActionCamera;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public UnitAssassination Assassination;

        public UnitSeatAcceleration SeatAcceleration;

        public float SoftPingThreshold;
        public float SoftPingInterruptTime;

        public float HardPingThreshold;
        public float HardPingInterruptTime;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float HardPingDeathThreshold;

        public float FeignDeathThreshold;
        public float FeignDeathTime;
        public float DistanceOfEvadeAnimation;
        public float DistanceOfDiveAnimation;
        public float StunnedMovementThreshold;
        public float FeignDeathChance;
        public float FeignRepeatChance;
        public CachedTagInstance SpawnedTurretCharacter;
        public short SpawnedActorCountMin;
        public short SpawnedActorCountMax;
        public float SpawnedVelocity;
        public Angle AimingVelocityMaximum;
        public Angle AimingAccelerationMaximum;
        public float CasualAimingModifier;
        public Angle LookingVelocityMaximum;
        public Angle LookingAccelerationMaximum;
        public StringId RightHandNode;
        public StringId LeftHandNode;
        public StringId PreferredGunNode;

        public CachedTagInstance MeleeDamage;
        public CachedTagInstance BoardingMeleeDamage;
        public CachedTagInstance BoardingMeleeResponse;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public CachedTagInstance EjectionMeleeDamage;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public CachedTagInstance EjectionMeleeResponse;

        public CachedTagInstance LandingMeleeDamage;
        public CachedTagInstance FlurryMeleeDamage;
        public CachedTagInstance ObstacleSmashMeleeDamage;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public CachedTagInstance ShieldPopDamage;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public CachedTagInstance AssassinationDamage;

        public MotionSensorBlipSizeValue MotionSensorBlipSize; // short
        public ItemScaleValue ItemScale; // short
        public List<Posture> Postures;
        public List<HudInterface> HudInterfaces;
        public List<DialogueVariant> DialogueVariants;

        /// <summary>
        /// If the player is in a seat in this unit then modify the motion tracker range by this amount.
        /// See base value in player globals for how this modifier is applied.
        /// </summary>
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float MotionTrackerRangeModifier;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public Angle GrenadeAngle;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public Angle GrenadeAngleMaxElevation;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public Angle GrenadeAngleMinElevation;

        public float GrenadeVelocity;
        public GrenadeTypeValue GrenadeType; // short
        public ushort GrenadeCount;
        public List<PoweredSeat> PoweredSeats;
        public List<Weapon> Weapons;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<TargetTrackingBlock> TargetTracking;

        public List<UnitSeat> Seats;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public float EmpRadius;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public CachedTagInstance EmpEffect;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public CachedTagInstance BoostCollisionDamage;

        public float BoostPeakPower;
        public float BoostRisePower;
        public float BoostPeakTime;
        public float BoostFallPower;
        public float BoostDeadTime;
        public float LipsyncAttackWeight;
        public float LipsyncDecayWeight;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public CachedTagInstance DetachDamage;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public CachedTagInstance DetachedWeapon;

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
            UnitHasBoost = 1 << 28
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
        public class MetagameProperty : TagStructure
		{
            public byte Flags;
            public UnitType Unit;
            public UnitClassification Classification;
            public sbyte Unknown;
            public short Points;
            public short Unknown2;

            public enum UnitType : sbyte
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

        [TagStructure(Size = 0x18)]
        public class UnitCameraAxisAcceleration : TagStructure
        {
            public float Unknown1;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
        }

        [TagStructure(Size = 0x4C)]
        public class UnitCameraAcceleration : TagStructure
        {
            public float MaximumCameraVelocity;

            [TagField(Length = 3)]
            public UnitCameraAxisAcceleration[] AxesAcceleration = new UnitCameraAxisAcceleration[3];
        }

        [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3Retail)]
        public class UnitCameraTrack : TagStructure
        {
            public CachedTagInstance CameraTrack;
        }

        [TagStructure(Size = 0x1C, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x3C, MinVersion = CacheVersion.Halo3Retail)]
        public class UnitCamera : TagStructure
        {
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public UnitCameraFlagBits CameraFlags;

            [TagField(Flags = Padding, Length = 2, MinVersion = CacheVersion.Halo3Retail)]
            public byte[] Unused = new byte[2];

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

        [TagStructure(Size = 0x1C, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x2C, MinVersion = CacheVersion.Halo3Retail)]
        public class UnitAssassination : TagStructure
        {
            public CachedTagInstance Response;
            public CachedTagInstance Weapon;
            public StringId ToolStowAnchor;
            public StringId ToolHandMarker;
            public StringId ToolMarker;
        }

        [TagStructure(Size = 0x14)]
        public class UnitSeatAcceleration : TagStructure
        {
            public RealVector3d Range;
            public float ActionScale;
            public float AttachScale;
        }

        public enum MotionSensorBlipSizeValue : short
        {
            Medium,
            Small,
            Large
        }

        public enum ItemScaleValue : short
        {
            Small,
            Medium,
            Large,
            Huge
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
            public CachedTagInstance UnitHudInterface;
        }

        [TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3Retail)]
        public class DialogueVariant : TagStructure
		{
            public short VariantNumber;
            public short Unknown;
            public CachedTagInstance Dialogue;
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
            public float DriverPowerupTime;
            public float DriverPowerdownTime;
        }

        [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3Retail)]
        public class Weapon : TagStructure
		{
            public CachedTagInstance Weapon2;
        }

        [TagStructure(Size = 0x38, MinVersion = CacheVersion.Halo3Retail)]
        public class TargetTrackingBlock : TagStructure
		{
            public List<TrackingType> TrackingTypes;
            public float AcquireTime;
            public float GraceTime;
            public float DecayTime;
            public CachedTagInstance TrackingSound;
            public CachedTagInstance LockedSound;

            [TagStructure(Size = 0x4, MinVersion = CacheVersion.Halo3Retail)]
            public class TrackingType : TagStructure
			{
                public StringId TrackingType2;
            }
        }

        [Flags]
        public enum UnitSeatFlagBits : int
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
            AllowsExitAndDetach = 1 << 22
        }

        [TagStructure(Size = 0xB0, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0xE4, MinVersion = CacheVersion.Halo3Retail)]
        public class UnitSeat : TagStructure
		{
            public UnitSeatFlagBits Flags; // int
            public StringId SeatAnimation;
            public StringId SeatMarkerName;
            public StringId EntryMarkerName;
            public StringId BoardingGrenadeMarker;
            public StringId BoardingGrenadeString;
            public StringId BoardingMeleeString;

            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public StringId DetachWeaponString;

            public float PingScale;
            public float TurnoverTime;
            public UnitSeatAcceleration SeatAcceleration;
            public float AiScariness;
            public AiSeatTypeValue AiSeatType; // short
            public short BoardingSeat;
            public float ListenerInterpolationFactor;

            public Bounds<float> YawRateBounds;
            public Bounds<float> PitchRateBounds;

            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public float PitchInterpolationTime;

            public Bounds<float> SpeedReferenceBounds;
            public float SpeedExponent;

            public UnitCamera Camera;

            public List<UnitHudInterfaceBlock> UnitHudInterface;
            public StringId EnterSeatString;
            public Bounds<Angle> YawRange;
            public CachedTagInstance BuiltInGunner;
            public float EntryRadius;
            public Angle EntryMarkerConeAngle;
            public Angle EntryMarkerFacingAngle;
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
                public CachedTagInstance UnitHudInterface;
            }
        }
    }
}