using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Tag = "unit", Size = 0x214, MaxVersion = CacheVersion.Halo3Retail)] 
    [TagStructure(Tag = "unit", Size = 0x224, MaxVersion = CacheVersion.Halo3ODST)] 
    [TagStructure(Tag = "unit", Size = 0x2C8, MinVersion = CacheVersion.HaloOnline106708)] 
    public abstract class Unit : GameObject
    {
        public uint FlagsWarningHalo4Values;
        public DefaultTeamValue DefaultTeam; // short
        public ConstantSoundVolumeValue ConstantSoundVolume; // short

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public CachedTagInstance HologramUnit;

        public List<MetagameProperty> MetagameProperties;
        public CachedTagInstance IntegratedLightToggle;
        public Angle CameraFieldOfView;
        public float CameraStiffness;
        public short Flags2;
        public short Unknown6;
        public StringId CameraMarkerName;
        public StringId CameraSubmergedMarkerName;
        public Angle PitchAutoLevel;
        public Angle PitchRangeMin;
        public Angle PitchRangeMax;
        public List<CameraTrackBlock> CameraTracks;
        public Angle Unknown7;
        public Angle Unknown8;
        public Angle Unknown9;
        public List<UnknownBlock> Unknown10;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public short Flags3;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public short Unknown11;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public StringId CameraMarkerName2;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public StringId CameraSubmergedMarkerName2;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public Angle PitchAutoLevel2;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public Angle PitchRangeMin2;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public Angle PitchRangeMax2;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<CameraTrackBlock> CameraTracks2;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public Angle Unknown12;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public Angle Unknown13;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public Angle Unknown14;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<UnknownBlock2> Unknown15;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public CachedTagInstance AssassinationResponse;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public CachedTagInstance AssassinationWeapon;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public StringId AssasinationToolStowAnchor;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public StringId AssasinationToolHandMarker;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public StringId AssasinationToolMarker;

        public RealVector3d AccelerationRange;
        public float AccelerationActionScale;
        public float AccelerationAttachScale;
        public float SoftPingThreshold;
        public float SoftPingInterruptTime;
        public float HardPingThreshold;
        public float HardPingInterruptTime;
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
        public CachedTagInstance EjectionMeleeDamage;
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

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown16;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown17;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown18;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown19;

        public float GrenadeVelocity;
        public GrenadeTypeValue GrenadeType; // short
        public short GrenadeCount;
        public List<PoweredSeat> PoweredSeats;
        public List<Weapon> Weapons;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<TargetTrackingBlock> TargetTracking;
        public List<Seat> Seats;
        public float EmpRadius;
        public CachedTagInstance EmpEffect;
        public CachedTagInstance BoostCollisionDamage;
        public float BoostPeakPower;
        public float BoostRisePower;
        public float BoostPeakTime;
        public float BoostFallPower;
        public float BoostDeadTime;
        public float LipsyncAttackWeight;
        public float LipsyncDecayWeight;
        public CachedTagInstance DetachDamage;
        public CachedTagInstance DetachedWeapon;

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
            Unused15,
        }

        public enum ConstantSoundVolumeValue : short
        {
            Silent,
            Medium,
            Loud,
            Shout,
            Quiet,
        }

        [TagStructure(Size = 0x8)]
        public class MetagameProperty
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

        [TagStructure(Size = 0x10)]
        public class CameraTrackBlock
        {
            public CachedTagInstance CameraTrack;
        }

        [TagStructure(Size = 0x4C)]
        public class UnknownBlock
        {
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
            public uint Unknown9;
            public uint Unknown10;
            public uint Unknown11;
            public uint Unknown12;
            public uint Unknown13;
            public uint Unknown14;
            public uint Unknown15;
            public uint Unknown16;
            public uint Unknown17;
            public uint Unknown18;
            public uint Unknown19;
        }
        
        [TagStructure(Size = 0x4C)]
        public class UnknownBlock2
        {
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
            public uint Unknown9;
            public uint Unknown10;
            public uint Unknown11;
            public uint Unknown12;
            public uint Unknown13;
            public uint Unknown14;
            public uint Unknown15;
            public uint Unknown16;
            public uint Unknown17;
            public uint Unknown18;
            public uint Unknown19;
        }

        public enum MotionSensorBlipSizeValue : short
        {
            Medium,
            Small,
            Large,
        }

        public enum ItemScaleValue : short
        {
            Small,
            Medium,
            Large,
            Huge,
        }

        [TagStructure(Size = 0x10)]
        public class Posture
        {
            public StringId Name;
            public RealVector3d PillOffset;
        }

        [TagStructure(Size = 0x10)]
        public class HudInterface
        {
            public CachedTagInstance UnitHudInterface;
        }

        [TagStructure(Size = 0x14)]
        public class DialogueVariant
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
        public class PoweredSeat
        {
            public float DriverPowerupTime;
            public float DriverPowerdownTime;
        }

        [TagStructure(Size = 0x10)]
        public class Weapon
        {
            public CachedTagInstance Weapon2;
        }

        [TagStructure(Size = 0x38)]
        public class TargetTrackingBlock
        {
            public List<TrackingType> TrackingTypes;
            public float AcquireTime;
            public float GraceTime;
            public float DecayTime;
            public CachedTagInstance TrackingSound;
            public CachedTagInstance LockedSound;

            [TagStructure(Size = 0x4)]
            public class TrackingType
            {
                public StringId TrackingType2;
            }
        }

        [TagStructure(Size = 0xE4)]
        public class Seat
        {
            public uint Flags;
            public StringId SeatAnimation;
            public StringId SeatMarkerName;
            public StringId EntryMarkerSName;
            public StringId BoardingGrenadeMarker;
            public StringId BoardingGrenadeString;
            public StringId BoardingMeleeString;
            public StringId DetachWeaponString;
            public float PingScale;
            public float TurnoverTime;
            public RealVector3d AccelerationRange;
            public float AccelerationActionScale;
            public float AccelerationAttachScale;
            public float AiScariness;
            public AiSeatTypeValue AiSeatType;
            public short BoardingSeat;
            public float ListenerInterpolationFactor;

            public Bounds<float> YawRateBounds;

            public Bounds<float> PitchRateBounds;
            public float PitchInterpolationTime;

            public float MinSpeedReference;
            public float MaxSpeedReference;
            public float SpeedExponent;

            public CameraFlagsValue CameraFlags;

            [TagField(Padding = true, Length = 2)]
            public byte[] Unused;

            public StringId CameraMarkerName;
            public StringId CameraSubmergedMarkerName;
            public Angle PitchAutoLevel;
            public Bounds<Angle> PitchRange;
            public List<CameraTrackBlock> CameraTracks;
            public Bounds<Angle> PitchSpringBounds;
            public Angle SpringVelocity;
            public List<CameraAccelerationBlock> Unknown7;
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
                Driver,
            }

            [Flags]
            public enum CameraFlagsValue : ushort
            {
                None = 0,
                PitchBoundsAbsoluteSpace = 1 << 0,
                OnlyCollidesWithEnvironment = 1 << 1,
                HidesPlayerUnitFromCamera = 1 << 2,
                UseAimingVectorInsteadOfMarkerForward = 1 << 3
            }
            
            [TagStructure(Size = 0x4C)]
            public class CameraAccelerationBlock
            {
                public uint Unknown;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
                public uint Unknown6;
                public uint Unknown7;
                public uint Unknown8;
                public uint Unknown9;
                public uint Unknown10;
                public uint Unknown11;
                public uint Unknown12;
                public uint Unknown13;
                public uint Unknown14;
                public uint Unknown15;
                public uint Unknown16;
                public uint Unknown17;
                public uint Unknown18;
                public uint Unknown19;
            }

            [TagStructure(Size = 0x10)]
            public class UnitHudInterfaceBlock
            {
                public CachedTagInstance UnitHudInterface;
            }
        }
    }
}