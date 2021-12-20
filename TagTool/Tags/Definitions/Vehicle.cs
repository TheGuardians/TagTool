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

        [TagField(Flags = Padding, Length = 2)]
        public byte[] Unused4 = new byte[2];

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public Bounds<float> FlippingAngularVelocityRangeNew;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public RealEulerAngles2d FixedGunOffset;
        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public VehicleSteeringControl Steering;

        public float CrouchTransitionTime; // seconds

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

        public float SeatEntranceAccelerationScale; // how much do we scale the force the vehicle the applies down on the seat when he enters. 0 == no acceleration
        public float SeatExitAccelerationScale; // how much do we scale the force the vehicle the applies down on the seat when he exits. 0 == no acceleration

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
                None,
                NoFrictionWdriver = 1 << 0,
                CanTriggerAutomaticOpeningDoors = 1 << 1,
                AutoaimWhenTeamless = 1 << 2,
                AiWeaponCannotRotate = 1 << 3,
                AiDoesNotRequireDriver = 1 << 4,
                AiDriverEnable = 1 << 5,
                AiDriverFlying = 1 << 6,
                AiDriverCansidestep = 1 << 7,
                AiDriverHovering = 1 << 8,
                NoncombatVehicle = 1 << 9,
                CausesCollisionDamage = 1 << 10,
                HugeVehiclePhysicsGroup = 1 << 11,
                EnableWheeliepoppingHack = 1 << 12,
                AiAutoTurret = 1 << 13
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
                Invalid = 1 << 0
            }
        }

        [TagStructure(Size = 0x54, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x60, MinVersion = CacheVersion.Halo3Retail)]
        public class HavokVehiclePhysics : TagStructure
        {
            public HavokVehiclePhysicsFlags Flags; // int

            public float GroundFriction; // this sucks.  for friction based vehicles only
            public float GroundDepth; // this sucks.  for friction based vehicles only
            public float GroundDampFactor;  // this sucks.  for friction based vehicles only
            public float GroundMovingFriction;  // this sucks.  for friction based vehicles only
            public float GroundMaximumSlope0;  // degrees 0-90
            public float GroundMaximumSlope1;

            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public float Unknown1;
            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public float Unknown2;
            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public float Unknown3;
            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public float Unknown4;

            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public float MaximumNormalForceContribution; // 0 defaults to 3, this prevents the physics from becoming unstable when hucked against a surface

            public float AntiGravityBankLift; // lift per WU.
            public float SteeringBankReactionScale; // how quickly we bank when we steer
            public float GravityScale; // value of 0 defaults to 1.  .5 is half gravity
            public float Radius; // generated from the radius of the hkConvexShape for this vehicle

            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public float MaximumUpdateDistance; // WU.  if a friciton point moves more than this distance it must update
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public float MaximumUpdatePeriod;  // Seconds. a friction point of this vehicle must update a least this often when controlled by a local player.
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public float MaximumRemoteUpdatePeriod; // Seconds. a friction point of this vehicle must update a least this often when controlled by an ai or remote player.

            public List<AntiGravityPoint> AntiGravityPoints;
            public List<FrictionPoint> FrictionPoints;
            public List<PhantomShape> PhantomShapes;
        }

        [TagStructure(Size = 0x24)]
        public class EnginePhysics : TagStructure
        {
            public float EngineMomentum; // higher moments make engine spin up slower
            public float EngineMaximumAngularVelocity; // higher moments make engine spin up slower
            public List<Gear> Gears;
            public CachedTag GearShiftSound;
        }

        [TagStructure(Size = 0x58)]
        public class HumanTankPhysics : TagStructure
        {
            public Angle ForwardArc; // outside of this arc the vehicle reverse direciton, around 110 degrees seems to be nice...
            public float ForwardTurnScale; // think of this as oversteer
            public float ReverseTurnScale; // think of this as oversteer
            // -------- forward differential
            public float MaximumLeftDifferential;
            public float MaximumRightDifferential;
            public float DifferentialAcceleration;
            public float DifferentialDeceleration;
            // -------- reverse differential
            public float MaximumLeftReverseDifferential;
            public float MaximumRightReverseDifferential;
            public float DifferentialReverseAcceleration;
            public float DifferentialReverseDeceleration;
            public EnginePhysics Engine;
            public float WheelCircumference;
            public float GravityAdjust;  // 0-1 fraction by which we scale gravity that is not along the ground plane
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
            // -------- human plane tuning variables
            public float FlyingTorqueScale; // big vehicles need to scale this down.  0 defaults to 1, which is generally a good value.
            public float AirFrictionDeceleration; // human plane physics only. 0 is nothing.  1 is like thowing the engine to full reverse
            public float ThrustScale;  // human plane physics only. 0 is default (1)
            public float TurnRateScaleWhenBoosting; // this was originally added for the sentinel enforce, but I could see other uses. 0 defaults to 1
            public Angle MaximumRoll; // 0 defaults to 90 degrees
            public VehicleSteeringAnimation SteeringAnimation;
        }   

        [TagStructure(Size = 0x70)]
        public class AlienScoutPhysics : TagStructure
        {
            public VehicleSteeringControl Steering;
            public VehicleVelocityControl VelocityControl;
            public AlienScoutSpecificType SpecificType;
            public AlienScoutFlags Flags;
            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;  // 0 defaults to 1
            public float DragCoefficient;
            public float ConstantDeceleration;
            public float TorqueScale;
            public EngineFunctionStruct EngineGravityFunction;
            public EngineFunctionStruct ContrailObjectFunction;
            public Bounds<float> GearRotationSpeed; // cycles per second idle to full throttle
            public VehicleSteeringAnimation SteeringAnimation;

            public enum AlienScoutSpecificType : sbyte
            {
                None,
                Ghost,
                Spectre,
                Wraith,
                HoverCraft
            }

            [Flags]
            public enum AlienScoutFlags : byte
            {
                None,
                LockedCamera = 1 << 0
            }

            [TagStructure(Size = 0x14)]
            public class EngineFunctionStruct : TagStructure
            {
                public StringId ObjectFunctionDamageRegion; // this is the name of the region by which we gauge the overall damage of the vehicle
                public Bounds<float> AntiGravityEngineSpeedRange; // speed at which engine position funciton  moves.  value of 1 means goes from 0-1 in 1 second
                public float EngineSpeedAcceleration; // strictly used for object funtion. in 0-1 space
                public float MaximumVehicleSpeed; // function is capped by speed of the vehicle. So when we slow down for any reason we see the function go down
            }
        }

        [TagStructure(Size = 0x64)]
        public class AlienFighterPhysics : TagStructure
        {
            public VehicleSteeringControl Steering;
            public VehicleTurningControl Turning;
            public VehicleVelocityControl VelocityControl;
            public float FlyingTorqueScale; // big vehicles need to scale this down.  0 defaults to 1, which is generally a good value.
            public RealEulerAngles2d FixedGunOffset;
            // -------- alien fighter trick variables
            public float MaximumTrickFrequency;
            public float LoopTrickDuration;
            public float RollTrickDuration;
            // -------- alien fighter fake flight control
            public float ZeroGravitySpeed;
            public float FullGravitySpeed;
            public float StrafeBoostScale;
            public float OffStickDecelerationScale;
            public float CruisingThrottle;
            public float DiveSpeedScale;
        }

        [TagStructure(Size = 0x4)]
        public class TurretPhysics : TagStructure
        {
            [TagField(Length = 4, Flags = TagFieldFlags.Padding)]
            public byte[] Unused;
        }

        [TagStructure(Size = 0x74)]
        public class VtolPhysics : TagStructure
        {
            public VehicleTurningControl Turning;

            public StringId LeftLiftMarker;
            public StringId RightLiftMarker;
            public StringId ThrustMarker;
            // -------- rise angles
            public Angle PitchUpRangeMin;
            public Angle PitchUpRangeMax;
            public Angle PitchDownRangeMin;
            public Angle PitchDownRangeMax;
            // -------- elevation lag
            public float MinimumRiseTargetLag;
            public float MaximumRiseTargetLag;
            // -------- minimum and maximum up acceleration
            public float MinimumUpAcceleration;
            public float MaximumUpAcceleration;
            // -------- turn, left and forward accelerations
            public float MaximumTurnAcceleration;
            public float TurnAccelerationGain;
            public float RotorDampening;  // maxes out around 30
            public float MaximumLeftAcceleration;
            public float MaximumForwardAcceleration;
            // -------- lift arm pivot
            public float LiftArmPivotLength;
            public float DragCoefficient;
            public float ConstantDeceleration;
            public float MagicAngularAccelerationExponent;  // magic force that torques vehicle back towards up
            public float MagicAngularAccelerationScale;  // magic force that torques vehicle back towards up
            public float MagicAngularAccelerationK; // magic force that torques vehicle back towards up
            public Angle LiftAnglesAcceleration;  // how fast can the engine animations accelerate their turn in degress/SQR(sec)
            public Angle RenderLiftAnglesAcceleration;  // how fast can the engine animations accelerate their turn in degress/SQR(sec)
            // -------- prop rotation
            public Bounds<float> PropellerRotationSpeed;
        }

        [TagStructure(Size = 0x70)]
        public class ChopperPhysics : TagStructure
        {
            public VehicleSteeringControl SteeringControl;
            public VehicleTurningControl TurningControl;

            public EnginePhysics Engine;

            public float WheelCircumference;
            public StringId RotationMarker;
            public float MagicTurningScale; // scale up the magic force
            public float MagicTurningAcceleration; // (degrees/pow(sec,2))rate at which the turning tries to accelerate
            public float MagicTurningMaximumVelocity;  // (degrees/sec)rate at which vehicl tries to turn
            public float MagicTurningExponent;  // turn_acc=delta^exp
            public float BankToSlideRatio;  // (WU/sec)the slide velocity at which we achieve full bank
            public float BankSlideExponent;
            public float BankToTurnRatio; // (WU/sec)the slide velocity at which we achieve full bank
            public float BankTurnExponent;
            public float BankFraction; // fraction of possible tire drop when we bank. 0 is full bank 1 is no bank.
            public float BankRate;  // bank fraction velocity. (0-1 fraction / sec)
            public float WheelAcceleration; // acceleration of the wheel towards the engine speed
            public float GyroscopicDamping; // 0==none, 30==damn near full damping
        }

        [TagStructure(Size = 0x030)]
        public class GuardianPhysics : TagStructure
        {
            public VehicleSteeringControl SteeringControl;
            public VehicleVelocityControl VelocityControl;
            public float TorqueScale;  // 0 defaults to 1
            public float AntiGravityForceZOffset;
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

            [Flags]
            public enum AntiGravityPointFlags : int
            {
                None,
                GetsDamageFromRegion = 1 << 0,
                OnlyActiveOnWater = 1 << 1
            }
        }

        [TagStructure(Size = 0x4C)]
        public class FrictionPoint : TagStructure
        {
            public StringId MarkerName;
            public FlagsValue Flags;
            public float FractionOfTotalMass;  // (0.0-1.0) fraction of total vehicle mass
            public float Radius;
            public float DamagedRadius; // radius when the tire is blown off.
            public FrictionTypeValue FrictionType;
            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public float MovingFrictionVelocityDiff;
            public float EBrakeMovingFriction;
            public float EBrakeFriction;
            public float EBrakeMovingFrictionVelocityDiff;
            [TagField(Length = 20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public StringId CollisionMaterialName;
            public short CollisionGlobalMaterialIndex;
            // -------- friction point destruction data
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
            public TorqueCurveStruct LoadedTorqueCurve;
            public TorqueCurveStruct CruisingTorqueCurve;

            // -------- gearing
            public float MinTimeToUpshift; // seconds
            public float EngineUpshiftScale; // [0,1]
            public float GearRatio;
            public float MinTimeToDownshift; // seconds
            public float EngineDownshiftScale; // [0,1]

            [TagStructure(Size = 0x18)]
            public class TorqueCurveStruct : TagStructure
            {
                public float MinTorque;
                public float MaxTorque;
                public float PeakTorqueScale;
                public float PastPeakTorqueExponent;
                public float TorqueAtMaxAngularVelocity; // generally 0 for loading torque and something less than max torque for cruising torque
                public float TorqueAt2xMaxAngularVelocity;
            }
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
            public Angle OverdampenCuspAngle;

            public float OverdampenExponent;
        }

        [TagStructure(Size = 0x8)]
        public class VehicleSteeringAnimation : TagStructure
        {
            public float InterpolationScale; // 1= heavy interp. of steering animations
            public Angle MaximumAngle;  // non-zero= max angle delta per frame
        }

        [TagStructure(Size = 0xC)]
        public class VehicleTurningControl : TagStructure
        {
            public float MaximumLeftTurn;
            public float MaximumRightTurn; // negative
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
            public RealVector3d MaximumLegMotion; // in WU, how far can we displace the legs in x,y,z each step
            public float MaximumTurn;  // in degrees, how much can this walker turn in one step

            public List<WalkerPhysicsLeg> Legs;

            public float LegApexDraction; // 0-1 fraction.  where the leg tansitions from lift to drop
            public float LiftExponent; // x(0-1) power exponent
            public float DropExponent; // x(0-1) power exponent
            public RealVector3d ObjectSpacePivotPosition;
            public float WalkCyclePause; // fraction of walkcycle at end for pause

            public short StablePlantedLegs;  // number of legs mantis needs planted to be considered stable.
            [TagField(Flags = Padding, Length = 0x2)]
            public byte UnusedPadding;

            public float TimeWithoutPlantBuffer; // seconds
            public float NotAlongUpGravityScale; // 0-1

            public float SpeedAccelerationLimit;
            public float SpeedAccelerationMatchScale;
            public float SlideAccelerationLimit;
            public float SlideAccelerationMatchScale;
            public float TurnAccelerationLimit;
            public float TurnAccelerationMatchScale;

            // -------- jumping
            public float JumpSetTime; // 0-1, portion of set time spent interpolating into neutral stance
            public float JumpSetInterpolationFraction; // seconds
            public float JumpLeapTime;  // seconds
            public float JumpRecoveryTime; // 0-1, portion of recovery time spent interpolating into neutral stance
            public float JumpRecoveryFraction;  // 0-1, portion of recovery time spent interpolating into neutral stance
            public float JumpLegSetDistance; // WU, amount foot moves up to get ready to jump
            public float JumpLegDistance; // WU, amount foot moves down when jumping
        }

        [TagStructure(Size = 0xA0)]
        public class WalkerPhysicsLeg : TagStructure
        {
            public WalkerPhysicsLegGroup LegGroup;
            public WalkerPhysicsLegGroup LegSide;
            public sbyte LegSideOrder;
            public sbyte Valid;
            public StringId HipNodeAName;
            public StringId HipNodeBName;
            public StringId KneeNodeAName;
            public StringId KneeNodeBName;
            public StringId FootMarkerName;
            [TagField(Flags = Padding, Length = 0x3C)]
            public byte[] UnusedPadding;
            public WalkerPhysicsLegFlags Flags;
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

            public enum WalkerPhysicsLegGroup : sbyte
            {
                Primary,
                Secondary
            }

            [Flags]
            public enum WalkerPhysicsLegFlags : int
            {
                None,
                ContrainedPlant = 1 << 0,
            }
        }
    }
}