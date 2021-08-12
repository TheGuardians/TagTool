using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using System;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Havok;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "vehicle", Tag = "vehi", Size = 0x114, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Name = "vehicle", Tag = "vehi", Size = 0x140, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "vehicle", Tag = "vehi", Size = 0x148, MinVersion = CacheVersion.HaloOnlineED)]
    public class Vehicle : Unit
    {
        public VehicleFlagBits VehicleFlags; // int

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public VehiclePhysicsType PhysicsType; // short

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public VehiclePhysicsControlType ControlType; // short

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float MaximumForwardSpeed;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float MaximumReverseSpeed;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float SpeedAcceleration;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float SpeedDeceleration;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float MaximumLeftTurn;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float MaximumRightTurn;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float WheelCircumference;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float TurnRate;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float BlurSpeed;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public VehiclePhysicsSpecificType PhysicsSpecificType;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public VehiclePhysicsTypes PhysicsTypes;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public HavokVehiclePhysics HavokPhysicsNew;

        public PlayerTrainingVehicleTypeValue PlayerTrainingVehicleType;

        [TagField(Flags = Padding, Length = 1, MaxVersion = CacheVersion.Halo2Vista)]
        public byte[] Unused2 = new byte[1];

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public StringId FlipOverMessageOld;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float FlipTimeOld;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float SpeedTurnPenaltyPower;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float SpeedTurnPenalty;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float MaximumLeftSlide;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float MaximumRightSlide;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float SlideAcceleration;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float SlideDeceleration;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public Bounds<float> FlippingAngularVelocityRangeOld;

        public VehicleSizeValue VehicleSize;

        [TagField(Flags = Padding, Length = 1, MaxVersion = CacheVersion.Halo2Vista)]
        public byte[] Unused3 = new byte[1];

        public sbyte ComplexSuspensionSampleCount;

        [TagField(Flags = Padding, Length = 1)]
        public byte[] Unused4 = new byte[1];

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public Bounds<float> FlippingAngularVelocityRangeNew;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public RealEulerAngles2d FixedGunOffset;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public VehicleSteeringControl Steering;

        public float CrouchTransitionTime;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float EngineUnknown;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float EngineMomentum;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float EngineMaximumAngularVelocity;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<Gear> Gears;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float FlyingTorqueScale;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public float Hoojytsu;

        public float SeatEntranceAccelerationScale;
        public float SeatExitAccelerationScale;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float AirFrictionDeceleration;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float ThrustScale;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public float FlipTimeNew;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public StringId FlipOverMessageNew;

        public CachedTag SuspensionSound;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public CachedTag CrashSound;

        public CachedTag SpecialEffect;
        public CachedTag DriverBoostDamageEffectOrResponse;
        public CachedTag RiderBoostDamageEffectOrResponse;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public HavokVehiclePhysics HavokPhysicsOld;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public float Unknown31;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public float Unknown32;

        [TagStructure(Size = 0x4)]
        public class VehicleFlagBits : TagStructure
        {
            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public Gen2Bits Gen2;

            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public Gen3Bits Gen3;

            [Flags]
            public enum Gen2Bits : int
            {
                None,
                SpeedWakesPhysics = 1 << 0,
                TurnWakesPhysics = 1 << 1,
                DriverPowerWakesPhysics = 1 << 2,
                GunnerPowerWakesPhysics = 1 << 3,
                ControlOppositeSpeedSetsBrake = 1 << 4,
                SlideWakesPhysics = 1 << 5,
                KillsRidersAtTerminalVelocity = 1 << 6,
                CausesCollisionDamage = 1 << 7,
                AiWeaponCannotRotate = 1 << 8,
                AiDoesNotRequireDriver = 1 << 9,
                AiUnused = 1 << 10,
                AiDriverEnable = 1 << 11,
                AiDriverFlying = 1 << 12,
                AiDriverCanSidestep = 1 << 13,
                AiDriverHovering = 1 << 14,
                VehicleSteersDirectly = 1 << 15,
                Bit16 = 1 << 16,
                HasEBrake = 1 << 17,
                NoncombatVehicle = 1 << 18,
                NoFrictionWithDriver = 1 << 19,
                CanTriggerAutomaticOpeningDoors = 1 << 20,
                AutoaimWhenTeamless = 1 << 21
            }

            [Flags]
            public enum Gen3Bits : int
            {
                None = 0,
                NoFrictionWithDriver = 1 << 0,
                CanTriggerAutomaticOpeningDoors = 1 << 1,
                AutoaimWhenTeamless = 1 << 2,
                AiWeaponCannotRotate = 1 << 3,
                AiDoesNotRequireDriver = 1 << 4,
                AiDriverEnable = 1 << 5,
                AiDriverFlying = 1 << 6,
                AiDriverCanSidestep = 1 << 7,
                AiDriverHovering = 1 << 8,
                NoncombatVehicle = 1 << 9,
                DoesNotCauseCollisionDamage = 1 << 10,
                AiAutoTurret = 1 << 11,
                Hydraulics = 1 << 12,
                IgnoreKillVolumes = 1 << 13,
                TargetableWhenOpen = 1 << 14,
                ReduceWeaponAccelWhenOnGround = 1 << 15,
                ReduceWeaponAccelWhenAirborne = 1 << 16,
                DoNotForceUnitsToExitWhenUpsideDown = 1 << 17,
                CreatesEnemySpawnInfluencers = 1 << 18,
                DriverCannotTakeDamage = 1 << 19,
                PlayerCannotFlipVehicle = 1 << 20,
                DoNotKillRidersAtTerminalVelocity = 1 << 21,
                RidersUseRadioOnly = 1 << 22,
                TreatDualWieldedWeaponAsSecondaryWeapon = 1 << 23
            }
        }

        public enum VehiclePhysicsType : short
        {
            HumanTank,
            HumanJeep,
            HumanBoat,
            HumanPlane,
            AlienScout,
            AlienFighter,
            Turret
        }

        public enum VehiclePhysicsControlType : short
        {
            Normal,
            Unused,
            Tank
        }

        public enum VehiclePhysicsSpecificType : short
        {
            None,
            Ghost,
            Wraith,
            Spectre,
            SentinelEnforcer
        }

        [TagStructure(Size = 0x78, MinVersion = CacheVersion.Halo3Retail)]
        public class VehiclePhysicsTypes : TagStructure
        {
            public List<HumanTankPhysics> HumanTank;
            public List<HumanJeepPhysics> HumanJeep;
            public List<HumanPlanePhysics> HumanPlane;
            public List<AlienScoutPhysics> AlienScout;
            public List<AlienFighterPhysics> AlienFighter;
            public List<TurretPhysics> Turret;
            public List<VehicleMantis> Mantis;
            public List<VtolPhysics> Vtol;
            public List<ChopperPhysics> Chopper;
            public List<GuardianPhysics> Guardian;
        }

        [TagStructure(Size = 0x4)]
        public class HavokVehiclePhysicsFlags : TagStructure
        {
            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public Gen2Bits Gen2;

            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public Gen3Bits Gen3;

            [Flags]
            public enum Gen2Bits : int
            {
                None,
                Invalid = 1 << 0
            }

            [Flags]
            public enum Gen3Bits : int
            {
                None,
                HasSuspension = 1 << 0,
                FrictionPointsTestOnlyEnvironments = 1 << 1
            }
        }

        [TagStructure(Size = 0x54, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x60, MinVersion = CacheVersion.Halo3Retail)]
        public class HavokVehiclePhysics : TagStructure
        {
            public HavokVehiclePhysicsFlags Flags; // int

            public float GroundFriction;
            public float GroundDepth;
            public float GroundDampFactor;
            public float GroundMovingFriction;
            public float GroundSlopeToStopAllTraction;
            public float GroundSlopeToStartTractionLoss;

            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public float Unknown1;
            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public float Unknown2;
            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public float Unknown3;
            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public float Unknown4;

            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public float MaximumNormalForceContribution;

            public float AntiGravityBankLift;
            public float SteeringBankReactionScale;
            public float GravityScale;
            public float Radius;

            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public float Unknown5;
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public float Unknown6;
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public float Unknown7;

            public List<AntiGravityPoint> AntiGravityPoints;
            public List<FrictionPoint> FrictionPoints;
            public List<PhantomShape> PhantomShapes;
        }

        [TagStructure(Size = 0x24)]
        public class EnginePhysics : TagStructure
        {
            public float EngineMomentum;
            public float EngineMaximumAngularVelocity;
            public List<Gear> Gears;
            public CachedTag GearShiftSound;
        }

        [TagStructure(Size = 0x58)]
        public class HumanTankPhysics : TagStructure
        {
            public Angle ForwardArc;
            public float FlipWindow;
            public float PeggedFraction;
            public float MaximumLeftDifferential;
            public float MaximumRightDifferential;
            public float DifferentialAcceleration;
            public float DifferentialDeceleration;
            public float MaximumLeftReverseDifferential;
            public float MaximumRightReverseDifferential;
            public float DifferentialReverseAcceleration;
            public float DifferentialReverseDeceleration;
            public EnginePhysics Engine;
            public float WheelCircumference;
            public float GravityAdjust;
        }

        [TagStructure(Size = 0x40)]
        public class HumanJeepPhysics : TagStructure
        {
            public VehicleSteeringControl Steering;
            public VehicleTurningControl Turning;
            public EnginePhysics Engine;
            public float WheelCircumference;
            public float GravityAdjust;
        }

        [TagStructure(Size = 0x4C)]
        public class HumanPlanePhysics : TagStructure
        {
            public VehicleVelocityControl VelocityControl;
            public float MaximumUpRise;
            public float MaximumDownRise;
            public float RiseAcceleration;
            public float RiseDeceleration;
            public float FlyingTorqueScale;
            public float AirFrictionDeceleration;
            public float ThrustScale;
            public float TurnRateScaleWhenBoosting;
            public Angle MaximumRoll;
            public VehicleSteeringAnimation SteeringAnimation;
        }

        [Flags]
        public enum VehicleScoutPhysicsFlags : byte
        {
            None,
            Hovercraft = 1 << 0,
            SlopeScalesSpeed = 1 << 1
        }

        [TagStructure(Size = 0x14)]
        public class AlienScoutGravityFunction : TagStructure
        {
            public StringId ObjectFunctionDamageRegion;
            public Bounds<float> AntiGravityEngineSpeedRange;
            public float EngineSpeedAcceleration;
            public float MaximumVehicleSpeed;
        }

        [TagStructure(Size = 0x70)]
        public class AlienScoutPhysics : TagStructure
        {
            public VehicleSteeringControl Steering;
            public VehicleVelocityControl VelocityControl;

            public VehicleScoutPhysicsFlags Flags;

            [TagField(Flags = Padding, Length = 3)]
            public byte[] Unused = new byte[3];

            public float DragCoefficient;
            public float ConstantDeceleration;
            public float TorqueScale;

            public AlienScoutGravityFunction EngineGravityFunction;
            public AlienScoutGravityFunction ContrailObjectFunction;

            public Bounds<float> GearRotationSpeed;

            public VehicleSteeringAnimation SteeringAnimation;
        }

        [TagStructure(Size = 0x64, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x68, MinVersion = CacheVersion.HaloOnlineED)]
        public class AlienFighterPhysics : TagStructure
        {
            public VehicleSteeringControl Steering;
            public VehicleTurningControl Turning;
            public VehicleVelocityControl VelocityControl;

            public float SlideAccelAgainstDirection;
            public float FlyingTorqueScale;
            public RealEulerAngles2d FixedGunOffset;
            public float LoopTrickDuration;
            public float RollTrickDuration;
            public float ZeroGravitySpeed;
            public float FullGravitySpeed;
            public float StrafeBoostScale;
            public float OffStickDecelScale;
            public float CruisingThrottle;
            public float DiveSpeedScale;

            [TagField(Flags = Padding, Length = 4, MinVersion = CacheVersion.HaloOnlineED)]
            public byte[] Unused = new byte[4];
        }

        [TagStructure(Size = 0x4, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x8, MinVersion = CacheVersion.HaloOnlineED)]
        public class TurretPhysics : TagStructure
        {
            public float Unknown1;

            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public float Unknown2;
        }

        [TagStructure(Size = 0x74)]
        public class VtolPhysics : TagStructure
        {
            public VehicleTurningControl Turning;

            public StringId LeftLiftMarker;
            public StringId RightLiftMarker;
            public StringId ThrustMarker;

            public Angle PitchUpRangeMin;
            public Angle PitchUpRangeMax;
            public Angle PitchDownRangeMin;
            public Angle PitchDownRangeMax;
            public float LiftDistance;
            public float MaximumUpwardSpeed;
            public float MaximumDownAcceleration;
            public float MaximumUpAcceleration;
            public float MaximumTurnAcceleration;
            public float TurnAccelerationGain;
            public float RotorDampening;
            public float MaximumLeftAcceleration;
            public float MaximumForwardAcceleration;
            public float LiftArmPivotLength;
            public float DragCoefficient;
            public float ConstantDeceleration;
            public float MagicAngularAccelerationExponent;
            public float MagicAngularAccelerationScale;
            public float MagicAngularAccelerationK;
            public Angle LiftAnglesAcceleration;
            public Angle RenderLiftAnglesAcceleration;
            public float PropellerRotationSpeedMin;
            public float PropellerRotationSpeedMax;
        }

        [TagStructure(Size = 0x70)]
        public class ChopperPhysics : TagStructure
        {
            public VehicleSteeringControl SteeringControl;
            public VehicleTurningControl TurningControl;

            public EnginePhysics Engine;

            public float WheelCircumference;
            public StringId RotationMarker;
            public float MagicTurningScale;
            public float MagicTurningAcceleration;
            public float MagicTurningMaximumVelocity;
            public float MagicTurningExponent;
            public float BankToSlideRatio;
            public float BankSlideExponent;
            public float BankToTurnRatio;
            public float BankTurnExponent;
            public float BankFraction;
            public float BankRate;
            public float WheelAcceleration;
            public float GyroscopicDamping;
        }

        [TagStructure(Size = 0x030)]
        public class GuardianPhysics : TagStructure
        {
            public VehicleSteeringControl SteeringControl;
            public VehicleVelocityControl VelocityControl;
            public float TorqueScale;
            public float AntiGravityForceZOffset;
        }

        [Flags]
        public enum AntiGravityPointFlags : int
        {
            None,
            GetsDamageFromRegion = 1 << 0,
            OnlyActiveOnWater = 1 << 1
        }

        [TagStructure(Size = 0x4C)]
        public class AntiGravityPoint : TagStructure
        {
            public StringId MarkerName;
            public AntiGravityPointFlags Flags;
            public float AntigravStrength;
            public float AntigravHeight;
            public float AntigravDampFactor;
            public float AntigravExtensionDamping;
            public float AntigravNormalK1;
            public float AntigravNormalK0;
            public float Radius;

            [TagField(Flags = Padding, Length = 12)]
            public byte[] Unused1 = new byte[12];

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused2 = new byte[2];

            public short DamageSourceRegionIndex;
            public StringId DamageSourceRegionName;
            public float DefaultStateError;
            public float MinorDamageError;
            public float MediumDamageError;
            public float MajorDamageError;
            public float DestroyedStateError;
        }

        [TagStructure(Size = 0x4C)]
        public class FrictionPoint : TagStructure
        {
            public StringId MarkerName;
            public FlagsValue Flags;
            public float FractionOfTotalMass;
            public float Radius;
            public float DamagedRadius;
            public FrictionTypeValue FrictionType;
            public short Unknown;
            public float MovingFrictionVelocityDiff;
            public float EBrakeMovingFriction;
            public float EBrakeFriction;
            public float EBrakeMovingFrictionVelocityDiff;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public StringId CollisionMaterialName;
            public short CollisionGlobalMaterialIndex;
            public ModelStateDestroyedValue ModelStateDestroyed;
            public StringId RegionName;
            public int RegionIndex;

            [Flags]
            public enum FlagsValue : int
            {
                None,
                GetsDamageFromRegion = 1 << 0,
                Powered = 1 << 1,
                FrontTurning = 1 << 2,
                RearTurning = 1 << 3,
                AttachedToEBrake = 1 << 4,
                CanBeDestroyed = 1 << 5
            }

            public enum FrictionTypeValue : short
            {
                Point,
                Forward,
            }

            public enum ModelStateDestroyedValue : short
            {
                Default,
                MinorDamage,
                MediumDamage,
                MajorDamage,
                Destroyed,
            }
        }

        [TagStructure(Size = 0x330, Align = 0x10, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x340, Align = 0x10, Platform = CachePlatform.MCC)]
        public class PhantomShape : TagStructure
        {
            public HkListShape ListShape;

            public int MultisphereCount;
            public uint Flags;
            public float X0;
            public float X1;
            public float Y0;
            public float Y1;
            public float Z0;
            public float Z1;

            [TagField(Length = 4, Align = 4, Platform = CachePlatform.Original)]
            [TagField(Length = 4, Align = 8, Platform = CachePlatform.MCC)]
            public HkMultiSphereShape[] Multispheres;

            [TagField(Length = 4, Align = 4, Platform = CachePlatform.Original)]
            [TagField(Length = 4, Align = 8, Platform = CachePlatform.MCC)]
            public ChildInfo[] ChildShapes;

            [TagStructure(Size = 0x10, Platform = CachePlatform.Original)]
            [TagStructure(Size = 0x18, Platform = CachePlatform.MCC)]
            public class ChildInfo : TagStructure
            {
                public Havok.HavokShapeReference Shape;
                public uint CollisionFilterInfo;
                public int ChildShapeSize;
                public int ChildShapeCount;
            }
        }

        [TagStructure(Size = 0x44)]
        public class Gear : TagStructure
        {
            public float MinLoadedTorque;
            public float MaxLoadedTorque;
            public float PeakLoadedTorqueScale;
            public float PastPeakLoadedTorqueExponent;
            public float LoadedTorqueAtMaxAngularVelocity;
            public float LoadedTorqueAt2xMaxAngularVelocity;

            public float MinCruisingTorque;
            public float MaxCruisingTorque;
            public float PeakCruisingTorqueScale;
            public float PastPeakCruisingTorqueExponent;
            public float CruisingTorqueAtMaxAngularVelocity;
            public float CruisingTorqueAt2xMaxAngularVelocity;

            public float MinTimeToUpshift;
            public float EngineUpshiftScale;
            public float GearRatio;
            public float MinTimeToDownshift;
            public float EngineDownshiftScale;
        }

        public enum PlayerTrainingVehicleTypeValue : sbyte
        {
            None,
            Warthog,
            WarthogTurret,
            Ghost,
            Banshee,
            Tank,
            Wraith,
        }

        public enum VehicleSizeValue : sbyte
        {
            Small,
            Large,
        }

        [TagStructure(Size = 0x8)]
        public class VehicleSteeringControl : TagStructure
        {
            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public float OverdampenCuspAngleOld;
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public Angle OverdampenCuspAngleNew;

            public float OverdampenExponent;
        }

        [TagStructure(Size = 0x8)]
        public class VehicleSteeringAnimation : TagStructure
        {
            public float InterpolationScale;
            public Angle MaximumAngle;
        }

        [TagStructure(Size = 0xC)]
        public class VehicleTurningControl : TagStructure
        {
            public float MaximumLeftTurn;
            public float MaximumRightTurn;
            public float TurnRate;
        }

        [TagStructure(Size = 0x20)]
        public class VehicleVelocityControl : TagStructure
        {
            public float MaximumForwardSpeed;
            public float MaximumReverseSpeed;
            public float SpeedAcceleration;
            public float SpeedDeceleration;
            public float MaximumLeftSlide;
            public float MaximumRightSlide;
            public float SlideAcceleration;
            public float SlideDeceleration;
        }

        [TagStructure(Size = 0xAC)]
        public class VehicleMantis : TagStructure
        {
            public VehicleSteeringControl SteeringContol;
            public VehicleTurningControl TurningControl;
            public VehicleVelocityControl VelocityControl;
            public WalkerPhysicsDefinition WalkerPhysics;
        }

        [TagStructure(Size = 0x78)]
        public class WalkerPhysicsDefinition : TagStructure
        {
            public RealVector3d MaximumLegMotion;
            public float MaximumTurn;

            public List<WalkerPhysicsLeg> Legs;

            public float LegApexDraction;
            public float LiftExponent;
            public float DropExponent;
            public RealVector3d ObjectSpacePivotPosition;
            public float WalkCyclePause;

            public short StablePlantedLegs;
            [TagField(Flags = Padding, Length = 0x2)]
            public byte UnusedPadding;

            public float TimeWithoutPlantBuffer;
            public float NotAlongUpGravityScale;

            public float SpeedAccelerationLimit;
            public float SpeedAccelerationMatchScale;
            public float SlideAccelerationLimit;
            public float SlideAccelerationMatchScale;
            public float TurnAccelerationLimit;
            public float TurnAccelerationMatchScale;

            public float JumpSetTime;
            public float JumpSetInterpolationFraction;
            public float JumpLeapTime;
            public float JumpRecoveryTime;
            public float JumpRecoveryFraction;
            public float JumpLegSetDistance;
            public float JumpLegDistance;
        }

        [TagStructure(Size = 0xA0)]
        public class WalkerPhysicsLeg : TagStructure
        {
            [Flags]
            public enum LegFlags : int
            {
                None,
                ContrainedPlant = 1 << 0,
            }

            public byte LegGroup;
            public byte LegSide;
            public byte LegSideOrder;
            public byte Valid;

            public StringId HipNodeAName;
            public StringId HipNodeBName;
            public StringId KneeNodeAName;
            public StringId KneeNodeBName;
            public StringId FootMarkerName;

            [TagField(Flags = Padding, Length = 0x3C)]
            public byte[] UnusedPadding;

            public LegFlags Flags;

            public RealVector3d RuntimeInitialOriginToHipOffset;
            public RealVector3d RuntimePivotCenterToHipOffset;
            public float RuntimeUpperLegLength;
            public float RuntimeLowerLegLength;
            public short RuntimeHipNodeAIndex;
            public short RuntimeHipNodeBIndex;
            public short RuntimeKneeNodeAIndex;
            public short RuntimeKneeNodeBIndex;
            public short RuntimeFootMarkerGroupIndex;
            public short RuntimeFootNodeIndex;
            public short RuntimeHipNodeIndex;
            public short RuntimeKneeNodeIndex;

            public RealVector3d PlantConstraintPosition;

            [TagField(Flags = Padding, Length = 0xC)]
            public byte[] UnusedPadding1;
        }
    }
}