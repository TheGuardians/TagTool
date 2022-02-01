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
    [TagStructure(Name = "vehicle", Tag = "vehi", Size = 0x148, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "vehicle", Tag = "vehi", Size = 0x17C, MinVersion = CacheVersion.HaloReach)]
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

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<UnitTrickDefinitionBlock> Tricks;

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

        [TagField(Flags = Padding, Length = 2, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Unused4 = new byte[2];

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public sbyte ComplexSuspensionSampleCount; // How many additional raycasts to perform per side of a tire.
        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloReach)]
        public byte[] Unused5;

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

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown31;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
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

        [TagStructure(Size = 0x78, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0xB4, MinVersion = CacheVersion.HaloReach)]
        public class VehiclePhysicsTypes : TagStructure
        {
            public List<HumanTankPhysics> HumanTank;
            public List<HumanJeepPhysics> HumanJeep;
            public List<HumanPlanePhysics> HumanPlane;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<WolverinePhysics> Wolverine;
            public List<AlienScoutPhysics> AlienScout;
            public List<AlienFighterPhysics> AlienFighter;
            public List<TurretPhysics> Turret;
            public List<VehicleMantis> Mantis;
            public List<VtolPhysics> Vtol;
            public List<ChopperPhysics> Chopper;
            public List<GuardianPhysics> Guardian;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<JackalGliderPhysics> JackalGlider;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<BoatPhysics> Boat;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<SpaceFighterPhysics> SpaceFighter;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<RevenantPhysics> Revenant;
        }

        [TagStructure(Size = 0x4)]
        public class HavokVehiclePhysicsFlags : TagStructure
        {
            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public Halo2Bits Halo2;

            [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
            public Halo3Bits Halo3;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public ReachBits Reach;

            [Flags]
            public enum Halo2Bits : uint
            {
                None,
                Invalid = 1 << 0
            }

            [Flags]
            public enum Halo3Bits : uint
            {
                None,
                Invalid = 1 << 0
            }

            [Flags]
            public enum ReachBits : uint
            {
                None,
                HasSuspension = 1 << 0,
                FrictionPointsTestOnlyEnvironment = 1 << 1
            }
        }

        [TagStructure(Size = 0x54, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x60, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x54, MinVersion = CacheVersion.HaloReach)]
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

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float Radius; // generated from the radius of the hkConvexShape for this vehicle
            [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
            public float MaximumUpdateDistance; // WU.  if a friciton point moves more than this distance it must update
            [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
            public float MaximumUpdatePeriod;  // Seconds. a friction point of this vehicle must update a least this often when controlled by a local player.
            [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
            public float MaximumRemoteUpdatePeriod; // Seconds. a friction point of this vehicle must update a least this often when controlled by an ai or remote player.

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public PhysicsUpdatePeriodEnum MaximumUpdatePeriodTicks; // ticks. 0 is default of 2
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public PhysicsUpdatePeriodEnum MaximumRemoteUpdatePeriodTicks; // ticks. when controlled by an ai or remote player. 0 defaults to 4
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int IterationCount;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int SuspensionCount;

            public List<AntiGravityPoint> AntiGravityPoints;
            public List<FrictionPoint> FrictionPoints;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<PhantomShape> PhantomShapes;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float GroundVerticalExtrusion;  // for hull surfaces that drive on ground this is how far we pretend the water is above everything physical

            public enum PhysicsUpdatePeriodEnum : short
            {
                Defaults,
                EveryFrame,
                EveryOtherFrame,
                EveryThirdFrame,
                EveryFourthFrame,
                EveryFithFrame,
                Every6thFrame,
                Every7thFrame,
                Every8thFrame
            }
        }

        [TagStructure(Size = 0x8)]
        public class LoadAndCruiseBlock : TagStructure
        {
            public StringId LoadCruiseFunction;
            public int AttachmentIndex;
        }

        [TagStructure(Size = 0x24, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x30, MinVersion = CacheVersion.HaloReach)]
        public class EnginePhysics : TagStructure
        {
            public float EngineMomentum; // higher moments make engine spin up slower
            public float EngineMaximumAngularVelocity; // higher moments make engine spin up slower
            public List<Gear> Gears;
            public CachedTag GearShiftSound;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<LoadAndCruiseBlock> LoadAndCruiseSound;
        }

        [TagStructure(Size = 0x58, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x70, MinVersion = CacheVersion.HaloReach)]
        public class HumanTankPhysics : TagStructure
        {
            public Angle ForwardArc; // outside of this arc the vehicle reverse direciton, around 110 degrees seems to be nice...

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public Angle PerpendicularForwardArc; // this is the value of forward arc when turned sideways.  We interpolate from forward arc to this value when camera becomes perpendicular to the vehicle
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float FlipWindow; // seconds
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float PeggedFraction; // 0-1

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

        [TagStructure(Size = 0x40, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x58, MinVersion = CacheVersion.HaloReach)]
        public class HumanJeepPhysics : TagStructure
        {
            public VehicleSteeringControl Steering;
            public VehicleTurningControl Turning;
            public EnginePhysics Engine;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<BoatEngineBlock> BoatEngine;

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

        [TagStructure(Size = 0x60, MinVersion = CacheVersion.HaloReach)]
        public class WolverinePhysics : TagStructure
        {
            public VehicleSteeringControl SteeringControl;
            public VehicleTurningControl TurningControl;
            public EnginePhysics Engine;
            public float WheelCircumferance;
            public float GravityAdjust; // 0-1 fraction by which we scale gravity that is not along the ground plane
            public float TurretDeploymentTime; // time it takes for turret and support legs to deploy (seconds)
            public float TurretHolsterTime; // time it takes for turret and support legs to pack up (seconds)
            public float RuntimeInverseTurretDeploymentTime;
            public float RuntimeInverseTurretHolsterTime;
            public float DeployedCameraYawScale;
        }

        [TagStructure(Size = 0x70, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x74, MinVersion = CacheVersion.HaloReach)]
        public class AlienScoutPhysics : TagStructure
        {
            public VehicleSteeringControl Steering;
            public VehicleVelocityControl VelocityControl;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float SlideSpeedAtTopSpeed;

            public AlienScoutSpecificType SpecificType;
            public AlienScoutFlags Flags;
            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;  // 0 defaults to 1
            public float DragCoefficient;
            public float ConstantDeceleration;
            public float TorqueScale;
            public AnitGravityEngineFunctionStruct EngineGravityFunction;
            public AnitGravityEngineFunctionStruct ContrailObjectFunction;
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

        [TagStructure(Size = 0x4, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x14, MinVersion = CacheVersion.HaloReach)]
        public class TurretPhysics : TagStructure
        {
            [TagField(Length = 4, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.HaloOnline700123)]
            public byte[] Unused;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public TurretFlags Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloReach)]
            public byte[] Padding;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId PhysicalYawNode;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId PhysicalPitchNode;
            // this is specificly for the wolverine which has a turret which pops up when you use it.
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId PhysicalElevateNode;
            // angle which elevate node is set to in order to operate
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public Angle OperatingElevationAngle;

            [Flags]
            public enum TurretFlags : ushort
            {
                // this is basicly a flag for the wolverine.  This turret pops up when the wolverine activates
                PoweredByParent = 1 << 0,
                // this turret holds its default position instead of swinging freely when not controlled
                IdlesInDefaultPosition = 1 << 1,
                ReverseYawMotorDirection = 1 << 2,
                ReversePitchMotorDirection = 1 << 3,
                ReverseElevateMotorDirection = 1 << 4,
                TargetableWhenOpen = 1 << 5
            }
        }

        [TagStructure(Size = 0x74, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0xD0, MinVersion = CacheVersion.HaloReach)]
        public class VtolPhysics : TagStructure
        {
            public VehicleTurningControl Turning;

            public StringId LeftLiftMarker;
            public StringId RightLiftMarker;
            public StringId ThrustMarker;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public TagFunction TriggerToThrottle;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public TagFunction DescentToBoost;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float MaxDownwardSpeed; // wu/s

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public Angle PitchUpRangeMin;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public Angle PitchUpRangeMax;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public Angle PitchDownRangeMin;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public Angle PitchDownRangeMax;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float MinimumRiseTargetLag;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float MaximumRiseTargetLag;

            public float MinimumUpAcceleration;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float MaximumUpAcceleration;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float MaximumDownAcceleration;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float VerticalDecelerationTime; // seconds

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float LiftArmPivotLengthReach;
            public float MaximumTurnAcceleration;
            public float TurnAccelerationGain;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public SpeedParametersStruct SpeedParameters;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public InterpolationParametersStruct InterpolationParameters;

            public Angle LiftAnglesAcceleration;  // how fast can the engine animations accelerate their turn in degress/SQR(sec)
            public Angle RenderLiftAnglesAcceleration;  // how fast can the engine animations accelerate their turn in degress/SQR(sec)

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float AltLockOffsetCoefficient; // scalar for altitude lock based on distance to target - higher numbers reach the target more quickly but may cause bounciness
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float AltLockVelocityCoefficient;  // scalar for altitude lock based on velocity.  Acts like friction, trying to remove vertical velocity from the system

            public Bounds<float> PropellerRotationSpeed;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float LandingTime;  // how long we must maintain the landing state in order to land
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float TakeoffTime; // how long it takes to leave the landed state
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float LandingLinearVelocity;  // must be under this linear velocity to enter/maintain landing state
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float LandingAngularVelocity; // must be under this angular velocity to enter/maintain landing state

            [TagStructure(Size = 0x4C)]
            public class InterpolationParametersStruct
            {
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float InterpolationSpeedDomain;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public TagFunction SpeedTrottleCeiling;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public TagFunction InterpolationAcc;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public TagFunction ABInterpolation;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public List<SpeedParametersStruct> SpeedInterpolated;
            }

            [TagStructure(Size = 0x24, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x20, MinVersion = CacheVersion.HaloReach)]
            public class SpeedParametersStruct : TagStructure
            {
                // maxes out around 30
                public float RotorDamping;
                public float MaximumLeftAcceleration;
                public float MaximumForwardAcceleration;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float LiftArmPivotLength;
                public float DragCoeficient;
                public float ConstantDeceleration; // magic force that torques vehicle back towards up
                public float MagicAngularAccExp; // magic force that torques vehicle back towards up
                public float MagicAngularAccScale; // magic force that torques vehicle back towards up
                public float MagicAngularAccK;
            }
        }

        [TagStructure(Size = 0x70, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x7C, MinVersion = CacheVersion.HaloReach)]
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

        [TagStructure(Size = 0x30)]
        public class GuardianPhysics : TagStructure
        {
            public VehicleSteeringControl SteeringControl;
            public VehicleVelocityControl VelocityControl;
            public float TorqueScale;  // 0 defaults to 1
            public float AntiGravityForceZOffset;
        }

        [TagStructure(Size = 0x170, MinVersion = CacheVersion.HaloReach)]
        public class JackalGliderPhysics : TagStructure
        {
            public VehicleSteeringControl SteeringControl;
            public VehicleVelocityControl VelocityControl;
            // 0 defaults to 1
            public float TorqueScale;
            public AnitGravityEngineFunctionStruct EngineObjectFunction;
            public AnitGravityEngineFunctionStruct ContrailObjectFunction;
            public SteeringAnimationStruct SteeringAnimation;
            public float FlyingVelocityThreshold;
            // degrees
            public Angle FlyingLookThreshold;
            public float FlyingHoverThreshold;
            public float GroundedHoverThreshold;
            public float LandingGroundedTime;
            // degrees
            public Angle GroundTurnRadius;
            // wu/sec
            public float GroundAcceleration;
            public float WingLiftQ;
            public float WingLiftK;
            public float WingLiftFunctionCeiling;
            public float AileronToAngularAcceleartionScale;
            public Angle AileronYawTiltAngle;
            public FlightSurfaceStruct WingSurface;
            public FlightSurfaceStruct AileronSurface;
            public FlightSurfaceStruct ElevatorSurface;
            public FlightSurfaceStruct TailSurface;
            public FlightSurfaceStruct RudderSurface;
            public FlightSurfaceStruct TaxiSurface;
            public JackalGliderDragStruct GroundDragStruct;
            public JackalGliderDragStruct AirDragStruct;
            public JackalGliderDragStruct TakeoffDragStruct;


            [TagStructure(Size = 0x8)]
            public class SteeringAnimationStruct : TagStructure
            {
                // 1= heavy interp. of steering animations
                public float InterpolationScale;
                // non-zero= max angle delta per frame
                public Angle MaxAngle;
            }

            [TagStructure(Size = 0x20)]
            public class FlightSurfaceStruct : TagStructure
            {
                public DimensionsEnum OffsetAxis;
                public DimensionsEnum PivotAxis;
                public DimensionsEnum RotationAxis;
                public DimensionsEnum NormalAxis;
                public float OffsetDistance;
                public float PivotDistance;
                public float Q;
                public float K;
                public float FunctionCeiling;
                public Angle MaximumAngle;
                public float RenderDebugRadius;

                public enum DimensionsEnum : sbyte
                {
                    Foward,
                    Left,
                    Up
                }
            }

            [TagStructure(Size = 0xC)]
            public class JackalGliderDragStruct : TagStructure
            {
                public float Q;
                public float K;
                public float ConstantDeceleration;
            }
        }

        [TagStructure(Size = 0x60, MinVersion = CacheVersion.HaloReach)]
        public class BoatPhysics : TagStructure
        {
            public BoatFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // only used when 'use tank controls' is checked above
            public Angle TankControlForwardArc; // degrees
            public VehicleSteeringControl SteeringControl;
            public VehicleTurningControl TurningControl;
            // 0 means 0.  How hard is it to type 1?
            public float GravityScale;
            public EnginePhysics Engine;
            public List<BoatEngineDefinitionBlock> BoatEngine;
            // the name of the hull surface used to spew effects along the hull of the vehicle.
            public StringId HullMarkerName;

            [Flags]
            public enum BoatFlags : byte
            {
                // use this for torpedoes
                BrickOnThrottle = 1 << 0,
                UseTankControls = 1 << 1
            }

            [TagStructure(Size = 0x6C)]
            public class BoatEngineDefinitionBlock : TagStructure
            {
                public BoatFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId PropellerMarkerName;
                public float OutOfWaterThrustScale;
                public float OutOfWaterTorqueScale;
                // used to interpolate out-of-water thrust
                public float PropellerRadius; // wu
                public float PropellerTorqueScale;
                public float ReverseLinearAcceleration;
                public float LinearAcceleration;
                public float AngularAcceleration;
                // how quickly the boat can accelerate to the desired orientation
                public Angle StabilizationAngularAcceleration; // deg/s/s
                // how far pitched back the boat tries to achieve (0 is perfectly flat)
                public Angle StabilizationDesiredInclination; // deg
                public float DragInputRangeVelocity; // wu/s
                public TagFunction NormalDrag;
                public TagFunction BoostDrag;
                public TagFunction AirDrag;
            }
        }

        [TagStructure(Size = 0xC4, MinVersion = CacheVersion.HaloReach)]
        public class SpaceFighterPhysics : TagStructure
        {
            public VehicleSteeringControl SteeringControl;
            public VehicleTurningControl TurningControl;
            public float FullThrottleSpeed; // wu/s
            public float NeutralThrottleSpeed; // wu/s
            public float ReverseThrottleSpeed; // speed when throttle = -1.0 (wu/s)
            public float SpeedAcceleration; // speed when throttle = 0.0 (wu/s/s)
            public float SpeedDeceleration; // maximum speed when throttle = 1.0 (wu/s/s)
            public float MaximumLeftSlide;
            public float MaximumRightSlide;
            public float SlideAcceleration;
            public float SlideDeceleration;
            public float FlyingTorqueScale; // big vehicles need to scale this down.  0 defaults to 1, which is generally a good value.
            public Angle FixedGunYaw;
            public Angle FixedGunPitch;
            public float MaximumTrickFrequency;
            public float LoopTrickDuration;
            public float RollTrickDuration;
            public float StrafeBoostScale;
            public float OffStickDecelerationScale;
            public float DiveSpeedScale;
            public Angle RollMaxVelocity; // max angular velocity for user-input roll (deg/sec)
            public Angle RollAcceleration; // acceleration for user-input roll (deg/sec/sec)
            public Angle RollDeceleration; // deceleration for user-input roll (when the user releases the stick) (deg/sec/sec)
            // if non-zero, when the desired velocity change is less than this fraction of the acceleration, starts interpolating the maximum acceleration towards zero.
            // You can think of this as a time in seconds where if the velocity would reach its target in this amount of time or less, it will start taking longer.
            public float RollSmoothingFraction;
            public float AutolevelTime; //  how long you must not rotate (roll, pitch, yaw) before autoleveling kicks in (s)
            public Angle AutolevelPitchCutoff; // if the ship is pitched outside of this range, autoleveling will not happen. The effect is also scaled as the pitch approaches this angle degrees
            public Angle AutolevelMaxVelocity; // maximum angular velocity for autoleveling (degrees/sec)
            public Angle AutolevelMaxAcceleration; // maximum acceleration for autoleveling (deg/sec/sec)
            public Angle AutolevelMaxUserAngVel; // autolevel can continue to happen if the user is desiring an angular velocity lower than this (deg/sec)
            public float AutolevelSpringK; // controls relationship between displacement and acceleration - higher values mean faster acceleration when the desired position is far from current position
            public float AutolevelSpringC; // controls relationship between velocity and friction - higher values will slow the system down, lower values may let  the system oscillate
            public float CosmeticRollScale; //  desired roll = delta yaw X scale
            public Angle CosmeticRollMaxBank; // maximum cosmetic roll angle (degrees)
            public Angle CosmeticRollMaxVelocity; // maximum angular velocity that cosmetic roll can achieve (deg/sec)
            public Angle CosmeticRollAcceleration; // maximum angular acceleration for cosmetic roll (deg/sec/sec)
            public float CosmeticRollSpringK; // controls relationship between displacement and acceleration - higher values mean faster acceleration when the desired position is far from current position
            public float CosmeticRollSpringC; // controls relationship between velocity and friction - higher values will slow the system down, lower values may let the system oscillate
            public Angle TurnDecelerationThreshold; // turn deceleration kicks in when turning faster than this (deg/sec)
            public float TurnDecelerationFraction; //  when turning at the maximum rate, throttle is limited to this value
            public SpaceFighterTurnBackFlags TurnBackFlags;
            public float TurnBackLatchedPeriod;
            public TagFunction TurnBackDistanceToTurnRate;

            [Flags]
            public enum SpaceFighterTurnBackFlags : uint
            {
                TurnBackToTangent = 1 << 0
            }
        }

        [TagStructure(Size = 0xA8)]
        public class RevenantPhysics : TagStructure
        {
            public HumanTankPhysics TankPhysics;
            public VehicleVelocityControl VelocityControl;
            public SteeringAnimationStruct SteeringAnimation;
            public AlienScoutSpecificTypeEnum SpecificType;
            public float DragCoeficient;
            public float ConstantDeceleration;
            // 0 defaults to 1
            public float TorqueScale;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;

            public enum AlienScoutSpecificTypeEnum : sbyte
            {
                None,
                Ghost,
                Spectre,
                Wraith,
                HoverCraft
            }

            [TagStructure(Size = 0x8)]
            public class SteeringAnimationStruct : TagStructure
            {
                // 1= heavy interp. of steering animations
                public float InterpolationScale;
                // non-zero= max angle delta per frame
                public Angle MaxAngle;
            }
        }

        [TagStructure(Size = 0x4C, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x48, MinVersion = CacheVersion.HaloReach)]
        public class AntiGravityPoint : TagStructure
        {
            public StringId MarkerName;
            public AntiGravityPointFlags Flags;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public AntiGravityPointDamping Damping;

            public float AntigravStrength;
            public float AntigravHeight;
            public float AntigravDampFactor;
            public float AntigravExtensionDamping;
            public float AntigravNormalK1;
            public float AntigravNormalK0;
            public float Radius;

            [TagField(Flags = Padding, Length = 12, MaxVersion = CacheVersion.HaloOnline700123)]
            public byte[] Unused1 = new byte[12];

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId CollisionGlobalMaterialName;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short RuntimeGlobalMaterialIndex;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused2 = new byte[2];

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
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

            public enum AntiGravityPointDamping : int
            {
                Normal,
                DampedLeft,
                DampedRight,
                UndampedLeft,
                UndampedRight
            }
        }

        [TagStructure(Size = 0x4C, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x34, MinVersion = CacheVersion.HaloReach)]
        public class FrictionPoint : TagStructure
        {
            public StringId MarkerName;
            public FlagsValue Flags;
            public float FractionOfTotalMass;  // (0.0-1.0) fraction of total vehicle mass
            public float Radius;
            public float DamagedRadius; // radius when the tire is blown off.
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public FrictionTypeValue FrictionType;
            [TagField(Length = 2, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.HaloOnline700123)]
            public byte[] Padding1;
            public float MovingFrictionVelocityDiff;
            public float EBrakeMovingFriction;
            public float EBrakeFriction;
            public float EBrakeMovingFrictionVelocityDiff;
            [TagField(Length = 20, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.HaloOnline700123)]
            public byte[] Padding2;
            [TagField(Flags = TagFieldFlags.GlobalMaterial)]
            public StringId CollisionMaterialName;
            [TagField(Flags = TagFieldFlags.GlobalMaterial)]
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

        [TagStructure(Size = 0x44, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x3C, MaxVersion = CacheVersion.HaloReach11883)]
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

            [TagStructure(Size = 0x18, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x14, MinVersion = CacheVersion.HaloReach)]
            public class TorqueCurveStruct : TagStructure
            {
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float MinTorque;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float MaxTorque;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float PeakTorqueScale;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float PastPeakTorqueExponent;
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float TorqueAtMaxAngularVelocity; // generally 0 for loading torque and something less than max torque for cruising torque
                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float TorqueAt2xMaxAngularVelocity;

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public TagFunction Function;
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

        [TagStructure(Size = 0x14)]
        public class AnitGravityEngineFunctionStruct : TagStructure
        {
            public StringId ObjectFunctionDamageRegion;  // this is the name of the region by which we gauge the overall damage of the vehicle
            public Bounds<float> AntiGravityEngineSpeedRange; // speed at which engine position funciton  moves.  value of 1 means goes from 0-1 in 1 second
            public float EngineSpeedAcceleration;  // strictly used for object funtion. in 0-1 space
            public float MaximumVehicleSpeed;  // function is capped by speed of the vehicle. So when we slow down for any reason we see the function go down
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
            public byte[] UnusedPadding;

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

        [TagStructure(Size = 0x6C)]
        public class BoatEngineBlock : TagStructure
        {
            public BoatFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public StringId PropellerMarkerName;
            public float OutOfWaterThrustScale;
            public float OutOfWaterTorqueScale;
            // used to interpolate out-of-water thrust
            public float PropellerRadius; // wu
                                          // (fake physics mode only)
                                          // Scale value for pitch when thrusting
            public float PropellerTorqueScale;
            public float ReverseLinearAcceleration;
            public float LinearAcceleration;
            public float AngularAcceleration;
            // how quickly the boat can accelerate to the desired orientation
            public Angle StabilizationAngularAcceleration; // deg/s/s
                                                           // how far pitched back the boat tries to achieve (0 is perfectly flat)
            public Angle StabilizationDesiredInclination; // deg
            public float DragInputRangeVelocity; // wu/s
            public ScalarFunctionNamedStruct NormalDrag;
            public ScalarFunctionNamedStruct BoostDrag;
            public ScalarFunctionNamedStruct AirDrag;

            [Flags]
            public enum BoatFlags : byte
            {
                UsesFakePhysics = 1 << 0
            }

            [TagStructure(Size = 0x14)]
            public class ScalarFunctionNamedStruct : TagStructure
            {
                public MappingFunction Function;

                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
        }

        [TagStructure(Size = 0x0)]
        public class NullBlock : TagStructure
        {

        }
    }

    [TagStructure(Size = 0x20)]
    public class UnitTrickDefinitionBlock : TagStructure
    {
        public StringId AnimationName;
        public UnitTrickActivationTypeEnum ActivationType;
        // specifies how pre-trick velocity is maintained during and after the trick
        // only applies to vehicles
        public UnitTrickVelocityPreservationModeEnum VelocityPreservation;
        public UnitTrickFlags Flags;
        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public float CameraInterpolationTime;  //sloppiness of the camera, only applies to vehicles                              
        public float TrickExitTime; // how long before the end of the trick we start using the below values
        public Bounds<float> TrickExitCameraInterpolationTime; // sloppiness of the camera when exiting the trick we interpolate between these values depending on how far your camera was displaced from the ideal camera
        public float TrickExitDisplacementReference;  // when your camera is this far from the ideal at the start of the trick, we use the maximum delay time above only for space fighter
        public float CooldownTime; // after ending this trick, how long until I can trick again only applies to vehicles

        public enum UnitTrickActivationTypeEnum : sbyte
        {
            BrakeLeft,
            BrakeRight,
            BrakeUp,
            BrakeDown,
            ThrowMovementLeft,
            ThrowMovementRight,
            ThrowMovementUp,
            ThrowMovementDown,
            ThrowLookLeft,
            ThrowLookRight,
            ThrowLookUp,
            ThrowLookDown,
            PegFlickJumpLeft,
            PegFlickJumpRight,
            PegFlickJumpUp,
            PegFlickJumpDown,
            DoubleJumpLeft,
            DoubleJumpRight,
            DoubleJumpUp,
            DoubleJumpDown
        }

        public enum UnitTrickVelocityPreservationModeEnum : sbyte
        {
            // velocity is completely removed
            None,
            // velocity is relative to the object's orientation at the start of the trick (so if you're moving forward before the
            // trick, you will be moving forward after the trick, even if that's a different direction)
            TrickRelative,
            // velocity is maintained in world space
            WorldRelative
        }

        [Flags]
        public enum UnitTrickFlags : byte
        {
            // as opposed to the trick camera, which is the default
            // vehicles only
            UseFollowingCamera = 1 << 0,
            // allows the player to continue to move aiming vector while tricking
            DoNotSlamPlayerControl = 1 << 1
        }
    }
}