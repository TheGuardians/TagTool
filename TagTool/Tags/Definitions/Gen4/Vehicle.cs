using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "vehicle", Tag = "vehi", Size = 0x1D4)]
    public class Vehicle : Unit
    {
        public VehicleFlagsEnum VehicleFlags;
        public VehiclePhysicsTypesStruct PhysicsTypes;
        public HavokVehiclePhysicsStruct HavokVehiclePhysics;
        public List<UnitTrickDefinitionBlock> Tricks;
        public PlayerTrainingVehicleTypeEnum PlayerTrainingVehicleType;
        // The size determine what kind of seats in larger vehicles it may occupy (i.e. small or large cargo seats)
        public VehicleSizeEnum VehicleSize;
        // How many additional raycasts to perform per side of a tire.
        public sbyte ComplexSuspensionSampleCount;
        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        // 0-90 degrees of the wedge portion of the wheel to test suspension
        public Angle ComplexSuspensionDistributionAngle; // degrees
        public float ComplexSuspensionWheelDiameter;
        public float ComplexSuspensionWheelWidth;
        public float MinimumFlippingAngularVelocity;
        public float MaximumFlippingAngularVelocity;
        public float CrouchTransitionTime; // seconds
        public float Hoojytsu;
        // how much do we scale the force the vehicle the applies down on the seat when he enters. 0 == no acceleration
        public float SeatEnteranceAccelerationScale;
        // how much do we scale the force the vehicle the applies down on the seat when he exits. 0 == no acceleration
        public float SeatExitAccelersationScale;
        public float BlurSpeed;
        public StringId FlipMessage;
        // High quality player sound bank to be prefetched. Can be empty.
        [TagField(ValidTags = new [] { "sbnk" })]
        public CachedTag PlayerVehicleSoundBank;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag SuspensionSound;
        // amount to increase per frame while speeding up(.002 is a good number)
        public float FakeAudioSpeedSpeedIncreaseAmount;
        // amount to increase per frame while boosting (.006 is a good number)
        public float FakeAudioSpeedBoostSpeedIncreaseAmount;
        // amount to decrease per frame while slowing down (.002 is a good number)
        public float FakeAudioSpeedSpeedDecreaseAmount;
        // max value while not boosting (maximum is 1)
        public float FakeAudioSpeedNonBoostLimit;
        // scales speed value. Must be > 0 for this to work (ie for banshee, 5 is good)
        public float FakeAudioSpeedMaxSpeedScale;
        public List<SoundRtpcblock> SoundRtpcs;
        public List<SoundSweetenerBlock> SoundSweeteners;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag SpecialEffect;
        [TagField(ValidTags = new [] { "jpt!","drdf" })]
        public CachedTag DriverBoostDamageEffectOrResponse;
        [TagField(ValidTags = new [] { "jpt!","drdf" })]
        public CachedTag RiderBoostDamageEffectOrResponse;
        public StringId VehicleName;
        public List<PhysicsTransitionsBlock> PhysicsTransitions;
        
        [Flags]
        public enum VehicleFlagsEnum : uint
        {
            NoFrictionWDriver = 1 << 0,
            AutoaimWhenTeamless = 1 << 1,
            AiWeaponCannotRotate = 1 << 2,
            AiDoesNotRequireDriver = 1 << 3,
            AiDriverEnable = 1 << 4,
            AiDriverFlying = 1 << 5,
            AiDriverCanSidestep = 1 << 6,
            AiDriverHovering = 1 << 7,
            NoncombatVehicle = 1 << 8,
            DoesNotCauseCollisionDamage = 1 << 9,
            HugeVehiclePhysicsGroup = 1 << 10,
            EnableWheeliePoppingHack = 1 << 11,
            // will attempt to spawn Unit's 'spawned turret character' to control this turret
            AiAutoTurret = 1 << 12,
            AiSentryTurret = 1 << 13,
            IgnoreCameraPitch = 1 << 14,
            // will be ignored if 'ai auto turret' is set in campaign or survival
            AutoTurret = 1 << 15,
            // Suppress kill volume checks performed by unmanned vehicles (needed by RemoteStrike/power weapon ordnance)
            IgnoreKillVolumes = 1 << 16,
            // makes this targetable if it is in an open state
            TargetableWhenOpen = 1 << 17,
            // if set, vehicle will use all tag damage_effect->alt instantaneous acceleration fields if it is on the ground
            VehicleWantsToRecieveReducedWeaponAccelerationWhenOnGround = 1 << 18,
            // if set, vehicle will use all tag damage_effect->alt instantaneous acceleration fields if it is airborne
            VehicleWantsToRecieveReducedWeaponAccelerationWhenAirborne = 1 << 19,
            DoNotForceUnitsToExitWhenUpsideDown = 1 << 20,
            // Used for Dominion Sentry Turrets, which enemies should not spawn in range of
            VehicleCreatesEnemySpawnInfluencers = 1 << 21,
            DriverCannotTakeDamage = 1 << 22,
            // the player isn't allowed to flip the vehicle under any circumstances
            PlayerCannotFlipVehicle = 1 << 23
        }
        
        public enum PlayerTrainingVehicleTypeEnum : sbyte
        {
            None,
            Warthog,
            WarthogTurret,
            Ghost,
            Banshee,
            Tank,
            Wraith
        }
        
        public enum VehicleSizeEnum : sbyte
        {
            Small,
            Large
        }
        
        [TagStructure(Size = 0xB4)]
        public class VehiclePhysicsTypesStruct : TagStructure
        {
            public List<HumanTankStruct> TypeHumanTank;
            public List<HumanJeepBlock> TypeHumanJeep;
            public List<HumanPlaneBlock> TypeHumanPlane;
            public List<WolverineBlock> TypeWolverine;
            public List<AlienScoutBlock> TypeAlienScout;
            public List<AlienFighterBlock> TypeAlienFighter;
            public List<TurretBlock> TypeTurret;
            public List<MantisBlock> TypeMantis;
            public List<VtolBlock> TypeVtol;
            public List<ChopperBlock> TypeChopper;
            public List<GuardianBlock> TypeGuardian;
            public List<JackalGliderBlock> TypeJackalGlider;
            public List<BoatBlock> TypeBoat;
            public List<SpaceFighterBlock> TypeSpaceFighter;
            public List<RevenantBlock> TypeRevenant;
            
            [TagStructure(Size = 0x94)]
            public class HumanTankStruct : TagStructure
            {
                // outside of this arc the vehicle reverse direciton, around 110 degrees seems to be nice...
                public Angle ForwardArc;
                // this is the value of forward arc when turned sideways.  We interpolate from forward arc to this value when camera
                // becomes perpendicular to the vehicle
                public Angle PerpendicularForwardArc;
                // seconds
                public float FlipWindow;
                // 0-1
                public float PeggedFraction;
                // think of this as oversteer
                public float ForwardTurnScale;
                // think of this as oversteer
                public float ReverseTurnScale;
                public float MaximumLeftDifferential;
                public float MaximumRightDifferential;
                public float DifferentialAcceleration;
                public float DifferentialDeceleration;
                public float MaximumLeftReverseDifferential;
                public float MaximumRightReverseDifferential;
                public float DifferentialReverseAcceleration;
                public float DifferentialReverseDeceleration;
                public GlobalVehicleEngineStruct Engine;
                public float WheelCircumferance;
                // 0-1 fraction by which we scale gravity that is not along the ground plane
                public float GravityAdjust;
                public TankFlags ControlFlags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float AtRestForwardAngle;
                public float AtRestReverseAngle;
                // first angle starting arc in which the control will cause the tank to reverse while at rest and facing side on
                public float AtRestSideOnReverseAngleClosestToFront;
                // second angle ending arc in which the control will cause the tank to reverse while at rest and facing side on
                public float AtRestSideOnReverseAngleFurthestFromFront;
                // angle forming arc in which the control will cause the tank to reverse while at rest and facing forward
                public float AtRestFacingForwardReverseAngle;
                // angle forming arc in which the control will cause the tank to reverse while at rest and facing backwards
                public float AtRestFacingBackwardReverseAngle;
                // when in motion the angle in which the control must be to start moving in the opposite direction
                public float InMotionOpposingDirectionAngle;
                // the speed a tank must reach before we consider it in motion, changing the control mode
                public float InMotionSpeed;
                
                [Flags]
                public enum TankFlags : byte
                {
                    EnableNewControl = 1 << 0,
                    // Used to decide if we use linear velocity to calculate if we are in motion
                    UseLinearVelocity = 1 << 1,
                    // Used to decide if we use angular velocity to calculate if we are in motion
                    UseAngularVelocity = 1 << 2
                }
                
                [TagStructure(Size = 0x30)]
                public class GlobalVehicleEngineStruct : TagStructure
                {
                    // higher moments make engine spin up slower
                    public float EngineMoment;
                    // higher moments make engine spin up slower
                    public float EngineMaxAngularVelocity;
                    public List<GearBlock> Gears;
                    [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                    public CachedTag GearShiftSound;
                    public List<LoadAndCruiseBlock> LoadAndCruiseSound;
                    
                    [TagStructure(Size = 0x5C)]
                    public class GearBlock : TagStructure
                    {
                        public TorqueCurveStruct LoadedTorqueCurve;
                        public TorqueCurveStruct CruisingTorqueCurve;
                        // seconds
                        public float MinTimeToUpshift;
                        // 0-1
                        public float EngineUpShiftScale;
                        public float GearRatio;
                        // seconds
                        public float MinTimeToDownshift;
                        // 0-1
                        public float EngineDownShiftScale;
                        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                        public CachedTag GearShiftSoundShiftingUp;
                        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                        public CachedTag GearShiftSoundShiftingDown;
                        
                        [TagStructure(Size = 0x14)]
                        public class TorqueCurveStruct : TagStructure
                        {
                            public ScalarFunctionNamedStruct Function;
                            
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
                    }
                    
                    [TagStructure(Size = 0x8)]
                    public class LoadAndCruiseBlock : TagStructure
                    {
                        public StringId LoadCruiseFunction;
                        public int AttachmentIndex;
                    }
                }
            }
            
            [TagStructure(Size = 0x5C)]
            public class HumanJeepBlock : TagStructure
            {
                public VehicleSteeringControlStruct SteeringControl;
                public VehicleTurningControlStruct TurningControl;
                public GlobalVehicleEngineStruct Engine;
                public List<BoatEngineDefinitionBlock> BoatEngine;
                public float WheelCircumferance;
                // 0-1 fraction by which we scale gravity that is not along the ground plane
                public float GravityAdjust;
                // how much torque should be applied to prevent a vehicle from rolling. Default should be 0.0, 1.0 is a good value for
                // making it hard to roll.
                public float AntirollTorqueFactor;
                
                [TagStructure(Size = 0x8)]
                public class VehicleSteeringControlStruct : TagStructure
                {
                    public float OverdampenCuspAngle; // degrees
                    public float OverdampenExponent;
                }
                
                [TagStructure(Size = 0xC)]
                public class VehicleTurningControlStruct : TagStructure
                {
                    public float MaximumLeftTurn;
                    public float MaximumRightTurn;
                    public float TurnRate;
                }
                
                [TagStructure(Size = 0x30)]
                public class GlobalVehicleEngineStruct : TagStructure
                {
                    // higher moments make engine spin up slower
                    public float EngineMoment;
                    // higher moments make engine spin up slower
                    public float EngineMaxAngularVelocity;
                    public List<GearBlock> Gears;
                    [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                    public CachedTag GearShiftSound;
                    public List<LoadAndCruiseBlock> LoadAndCruiseSound;
                    
                    [TagStructure(Size = 0x5C)]
                    public class GearBlock : TagStructure
                    {
                        public TorqueCurveStruct LoadedTorqueCurve;
                        public TorqueCurveStruct CruisingTorqueCurve;
                        // seconds
                        public float MinTimeToUpshift;
                        // 0-1
                        public float EngineUpShiftScale;
                        public float GearRatio;
                        // seconds
                        public float MinTimeToDownshift;
                        // 0-1
                        public float EngineDownShiftScale;
                        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                        public CachedTag GearShiftSoundShiftingUp;
                        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                        public CachedTag GearShiftSoundShiftingDown;
                        
                        [TagStructure(Size = 0x14)]
                        public class TorqueCurveStruct : TagStructure
                        {
                            public ScalarFunctionNamedStruct Function;
                            
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
                    }
                    
                    [TagStructure(Size = 0x8)]
                    public class LoadAndCruiseBlock : TagStructure
                    {
                        public StringId LoadCruiseFunction;
                        public int AttachmentIndex;
                    }
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
            }
            
            [TagStructure(Size = 0x64)]
            public class HumanPlaneBlock : TagStructure
            {
                public float MaximumForwardSpeed;
                public float MaximumReverseSpeed;
                public float SpeedAcceleration;
                public float SpeedDeceleration;
                // acceleration when trying to throttle against current speed direction
                public float SpeedAccelAgainstDirection;
                public float MaximumForwardSpeedDuringBoost;
                public float MaximumLeftSlide;
                public float MaximumRightSlide;
                public float SlideAcceleration;
                public float SlideDeceleration;
                // acceleration when trying to throttle against current slide direction
                public float SlideAccelAgainstDirection;
                public float MaximumSlideSpeedDuringBoost;
                public float MaximumUpRise;
                public float MaximumDownRise;
                public float RiseAcceleration;
                public float RiseDeceleration;
                // acceleration when trying to throttle against current rise direction
                public float RiseAccelAgainstDirection;
                public float MaximumRiseSpeedDuringBoost;
                // big vehicles need to scale this down.  0 defaults to 1, which is generally a good value.
                public float FlyingTorqueScale;
                // human plane physics only. 0 is nothing.  1 is like thowing the engine to full reverse
                public float AirFrictionDeceleration;
                // human plane physics only. 0 is default (1)
                public float ThrustScale;
                // this was originally added for the sentinel enforce, but I could see other uses. 0 defaults to 1
                public float TurnRateScaleWhenBoosting;
                // 0 defaults to 90 degrees
                public float MaximumRoll;
                public SteeringAnimationStruct SteeringAnimation;
                
                [TagStructure(Size = 0x8)]
                public class SteeringAnimationStruct : TagStructure
                {
                    // 1= heavy interp. of steering animations
                    public float InterpolationScale;
                    // non-zero= max angle delta per frame
                    public Angle MaxAngle;
                }
            }
            
            [TagStructure(Size = 0x60)]
            public class WolverineBlock : TagStructure
            {
                public VehicleSteeringControlStruct SteeringControl;
                public VehicleTurningControlStruct TurningControl;
                public GlobalVehicleEngineStruct Engine;
                public float WheelCircumferance;
                // 0-1 fraction by which we scale gravity that is not along the ground plane
                public float GravityAdjust;
                // time it takes for turret and support legs to deploy
                public float TurretDeploymentTime; // seconds
                // time it takes for turret and support legs to pack up
                public float TurretHolsterTime; // seconds
                public float RuntimeInverseTurretDeploymentTime;
                public float RuntimeInverseTurretHolsterTime;
                public float DeployedCameraYawScale;
                
                [TagStructure(Size = 0x8)]
                public class VehicleSteeringControlStruct : TagStructure
                {
                    public float OverdampenCuspAngle; // degrees
                    public float OverdampenExponent;
                }
                
                [TagStructure(Size = 0xC)]
                public class VehicleTurningControlStruct : TagStructure
                {
                    public float MaximumLeftTurn;
                    public float MaximumRightTurn;
                    public float TurnRate;
                }
                
                [TagStructure(Size = 0x30)]
                public class GlobalVehicleEngineStruct : TagStructure
                {
                    // higher moments make engine spin up slower
                    public float EngineMoment;
                    // higher moments make engine spin up slower
                    public float EngineMaxAngularVelocity;
                    public List<GearBlock> Gears;
                    [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                    public CachedTag GearShiftSound;
                    public List<LoadAndCruiseBlock> LoadAndCruiseSound;
                    
                    [TagStructure(Size = 0x5C)]
                    public class GearBlock : TagStructure
                    {
                        public TorqueCurveStruct LoadedTorqueCurve;
                        public TorqueCurveStruct CruisingTorqueCurve;
                        // seconds
                        public float MinTimeToUpshift;
                        // 0-1
                        public float EngineUpShiftScale;
                        public float GearRatio;
                        // seconds
                        public float MinTimeToDownshift;
                        // 0-1
                        public float EngineDownShiftScale;
                        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                        public CachedTag GearShiftSoundShiftingUp;
                        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                        public CachedTag GearShiftSoundShiftingDown;
                        
                        [TagStructure(Size = 0x14)]
                        public class TorqueCurveStruct : TagStructure
                        {
                            public ScalarFunctionNamedStruct Function;
                            
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
                    }
                    
                    [TagStructure(Size = 0x8)]
                    public class LoadAndCruiseBlock : TagStructure
                    {
                        public StringId LoadCruiseFunction;
                        public int AttachmentIndex;
                    }
                }
            }
            
            [TagStructure(Size = 0x78)]
            public class AlienScoutBlock : TagStructure
            {
                public VehicleSteeringControlStruct SteeringControl;
                public float MaximumForwardSpeed;
                public float MaximumReverseSpeed;
                public float SpeedAcceleration;
                public float SpeedDeceleration;
                public float MaximumLeftSlide;
                public float MaximumRightSlide;
                public float SlideAcceleration;
                public float SlideDeceleration;
                // acceleration when trying to throttle against current slide direction
                public float SlideAccelAgainstDirection;
                // the slide speeds are interpolated down to this value, reaching it when the vehicle is moving at its top speed
                public float SlideSpeedAtTopSpeed; // wu/s
                public AlienScoutSpecificTypeEnum SpecificType;
                public AlienScoutFlags Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float DragCoeficient;
                public float ConstantDeceleration;
                // 0 defaults to 1
                public float TorqueScale;
                public EngineFunctionStruct EngineGravityFunction;
                public EngineFunctionStruct ContrailGravityFunction;
                // cycles per second idle to full throttle
                public Bounds<float> GearRotationSpeed;
                public SteeringAnimationStruct SteeringAnimation;
                
                public enum AlienScoutSpecificTypeEnum : sbyte
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
                    LockedCamera = 1 << 0
                }
                
                [TagStructure(Size = 0x8)]
                public class VehicleSteeringControlStruct : TagStructure
                {
                    public float OverdampenCuspAngle; // degrees
                    public float OverdampenExponent;
                }
                
                [TagStructure(Size = 0x14)]
                public class EngineFunctionStruct : TagStructure
                {
                    // this is the name of the region by which we gauge the overall damage of the vehicle
                    public StringId ObjectFunctionDamageRegion;
                    // speed at which engine position funciton  moves.  value of 1 means goes from 0-1 in 1 second
                    public float MinAntiGravityEngineSpeed;
                    // speed at which engine position funciton  moves.  value of 1 means goes from 0-1 in 1 second
                    public float MaxAntiGravityEngineSpeed;
                    // strictly used for object funtion. in 0-1 space
                    public float EngineSpeedAcceleration;
                    // function is capped by speed of the vehicle. So when we slow down for any reason we see the function go down
                    public float MaximumVehicleSpeed;
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
            
            [TagStructure(Size = 0x68)]
            public class AlienFighterBlock : TagStructure
            {
                public VehicleSteeringControlStruct SteeringControl;
                public VehicleTurningControlStruct TurningControl;
                public float MaximumForwardSpeed;
                public float MaximumReverseSpeed;
                public float SpeedAcceleration;
                public float SpeedDeceleration;
                public float MaximumLeftSlide;
                public float MaximumRightSlide;
                public float SlideAcceleration;
                public float SlideDeceleration;
                // acceleration when trying to throttle against current slide direction
                public float SlideAccelAgainstDirection;
                // big vehicles need to scale this down.  0 defaults to 1, which is generally a good value.
                public float FlyingTorqueScale;
                public Angle FixedGunYaw;
                public Angle FixedGunPitch;
                public float MaximumTrickFrequency;
                public float LoopTrickDuration;
                public float RollTrickDuration;
                public float ZeroGravitySpeed;
                public float FullGravitySpeed;
                public float StrafeBoostScale;
                public float OffStickDecelerationScale;
                public float CruisingThrottle;
                public float DiveSpeedScale;
                
                [TagStructure(Size = 0x8)]
                public class VehicleSteeringControlStruct : TagStructure
                {
                    public float OverdampenCuspAngle; // degrees
                    public float OverdampenExponent;
                }
                
                [TagStructure(Size = 0xC)]
                public class VehicleTurningControlStruct : TagStructure
                {
                    public float MaximumLeftTurn;
                    public float MaximumRightTurn;
                    public float TurnRate;
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class TurretBlock : TagStructure
            {
                public TurretFlags Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId PhysicalYawNode;
                public StringId PhysicalPitchNode;
                // this is specificly for the wolverine which has a turret which pops up when you use it.
                public StringId PhysicalElevateNode;
                // angle which elevate node is set to in order to operate
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
            
            [TagStructure(Size = 0xAC)]
            public class MantisBlock : TagStructure
            {
                public VehicleSteeringControlStruct SteeringControl;
                public VehicleTurningControlStruct TurningControl;
                public float MaximumForwardSpeed;
                public float MaximumReverseSpeed;
                public float SpeedAcceleration;
                public float SpeedDeceleration;
                public float MaximumLeftSlide;
                public float MaximumRightSlide;
                public float SlideAcceleration;
                public float SlideDeceleration;
                public WalkerPhysicsStruct WalkerPhysics;
                // fraction of walkcycle at end for pause
                public float WalkCyclePause;
                // number of legs mantis needs planted to be considered stable.
                public short StablePlantedLegs;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                // seconds
                public float TimeWithoutPlantBuffer;
                // 0-1
                public float NotAlongUpGravityScale;
                public float SpeedAccelerationLimit;
                public float SpeedAccelerationMatchScale;
                public float SlideAccelerationLimit;
                public float SlideAccelerationMatchScale;
                public float TurnAccelerationLimit;
                public float TurnAccelerationMatchScale;
                // seconds
                public float JumpSetTime;
                // 0-1, portion of set time spent interpolating into neutral stance
                public float JumpSetInterpolationFraction;
                // seconds
                public float JumpLeapTime;
                // seconds
                public float JumpRecoveryTime;
                // 0-1, portion of recovery time spent interpolating into neutral stance
                public float JumpRecoveryFraction;
                // WU, amount foot moves up to get ready to jump
                public float JumpLegSetDistance;
                // WU, amount foot moves down when jumping
                public float JumpLegDistance;
                
                [TagStructure(Size = 0x8)]
                public class VehicleSteeringControlStruct : TagStructure
                {
                    public float OverdampenCuspAngle; // degrees
                    public float OverdampenExponent;
                }
                
                [TagStructure(Size = 0xC)]
                public class VehicleTurningControlStruct : TagStructure
                {
                    public float MaximumLeftTurn;
                    public float MaximumRightTurn;
                    public float TurnRate;
                }
                
                [TagStructure(Size = 0x34)]
                public class WalkerPhysicsStruct : TagStructure
                {
                    // in WU, how far can we displace the legs in x,y,z each step
                    public RealVector3d MaximumLegMotion;
                    // in degrees, how much can this walker turn in one step
                    public float MaximumTurn;
                    public List<WalkerPhysicsLegBlock> Legs;
                    // 0-1 fraction.  where the leg tansitions from lift to drop
                    public float LegApexFraction;
                    // x(0-1) power exponent
                    public float LiftExponent;
                    // x(0-1) power exponent
                    public float DropExponent;
                    public RealVector3d ObjectSpacePivotPosition;
                    
                    [TagStructure(Size = 0xA0)]
                    public class WalkerPhysicsLegBlock : TagStructure
                    {
                        public WalkerPhysicsLegGroupEnum LegGroup;
                        public WalkerPhysicsLegSideEnum LegSide;
                        // for each side order the legs from 0-n where 0 is the most forward leg
                        public sbyte LegSideOrder;
                        public sbyte Valid;
                        public StringId HipNodeAName;
                        public StringId HipNodeBName;
                        public StringId KneeNodeAName;
                        public StringId KneeNodeBName;
                        public StringId FootMarkerName;
                        [TagField(Length = 0x3C, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
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
                        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding1;
                        
                        public enum WalkerPhysicsLegGroupEnum : sbyte
                        {
                            Primary,
                            Secondary
                        }
                        
                        public enum WalkerPhysicsLegSideEnum : sbyte
                        {
                            Left,
                            Right
                        }
                        
                        [Flags]
                        public enum WalkerPhysicsLegFlags : uint
                        {
                            ConstrainedPlant = 1 << 0
                        }
                    }
                }
            }
            
            [TagStructure(Size = 0xD0)]
            public class VtolBlock : TagStructure
            {
                public VehicleTurningControlStruct TurningControl;
                public StringId LeftLiftMarker;
                public StringId RightLiftMarker;
                public StringId ThrustMarker;
                public ScalarFunctionNamedStruct TriggerToThrottle;
                public VtolDescentFunctionStruct DescentToBoost;
                public float MaximumUpAcceleration;
                public float MaximumDownAcceleration;
                // after accelerating vertically, take this long to bleed off the throttle
                public float VerticalDecelerationTime; // seconds
                public float LiftArmPivotLength;
                public float MaximumTurnAcceleration;
                public float TurnAccelerationGain;
                public float InterpolationSpeedDomain;
                public ScalarFunctionNamedStruct SpeedTrottleCeiling;
                public ScalarFunctionNamedStruct InterpolationAcc;
                public ScalarFunctionNamedStruct ABInterpolation;
                public List<VtolSpeedInterpolatedBlock> SpeedInterpolatedParameters;
                // how fast can the engine animations accelerate their turn in degress/SQR(sec)
                public Angle LiftAnglesAcc;
                // how fast can the engine animations accelerate their turn in degress/SQR(sec)
                public Angle RenderLiftAnglesAcc;
                // scalar for altitude lock based on distance to target - higher numbers reach the target more quickly but may cause
                // bounciness
                public float AltLockOffsetCoefficient;
                // scalar for altitude lock based on velocity.  Acts like friction, trying to remove vertical velocity from the system
                public float AltLockVelocityCoefficient;
                // cycles per second idle to full throttle
                public Bounds<float> PropRotationSpeed;
                // how long we must maintain the landing state in order to land
                public float LandingTime; // s
                // how long it takes to leave the landed state
                public float TakeoffTime; // s
                // must be under this linear velocity to enter/maintain landing state
                public float LandingLinearVelocity; // wu/s
                // must be under this angular velocity to enter/maintain landing state
                public float LandingAngularVelocity; // rad/s
                
                [TagStructure(Size = 0xC)]
                public class VehicleTurningControlStruct : TagStructure
                {
                    public float MaximumLeftTurn;
                    public float MaximumRightTurn;
                    public float TurnRate;
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
                
                [TagStructure(Size = 0x18)]
                public class VtolDescentFunctionStruct : TagStructure
                {
                    public ScalarFunctionNamedStruct DescentToBoost;
                    public float MaxDownwardSpeed; // wu/s
                }
                
                [TagStructure(Size = 0x20)]
                public class VtolSpeedInterpolatedBlock : TagStructure
                {
                    // maxes out around 30
                    public float RotorDamping;
                    public float MaximumLeftAcceleration;
                    public float MaximumForwardAcceleration;
                    public float DragCoeficient;
                    public float ConstantDeceleration;
                    // magic force that torques vehicle back towards up
                    public float MagicAngularAccExp;
                    // magic force that torques vehicle back towards up
                    public float MagicAngularAccScale;
                    // magic force that torques vehicle back towards up
                    public float MagicAngularAccK;
                }
            }
            
            [TagStructure(Size = 0x7C)]
            public class ChopperBlock : TagStructure
            {
                public VehicleSteeringControlStruct SteeringControl;
                public VehicleTurningControlStruct TurningControl;
                public GlobalVehicleEngineStruct Engine;
                public float WheelCircumferance;
                public StringId RotationMarker;
                // scale up the magic force
                public float MagicTurningScale;
                // (degrees/pow(sec,2))rate at which the turning tries to accelerate
                public float MagicTurningAcc;
                // (degrees/sec)rate at which vehicl tries to turn
                public float MagicTurningMaxVel;
                // turn_acc=delta^exp
                public float MagicTurningExponent;
                // (WU/sec)the slide velocity at which we achieve full bank
                public float BankToSlideRatio;
                public float BankSlideExponent;
                // (WU/sec)the slide velocity at which we achieve full bank
                public float BankToTurnRatio;
                public float BankTurnExponent;
                // fraction of possible tire drop when we bank. 0 is full bank 1 is no bank.
                public float BankFraction;
                // bank fraction velocity. (0-1 fraction / sec)
                public float BankRate;
                // acceleration of the wheel towards the engine speed
                public float WheelAccel;
                // 0==none, 30==damn near full damping
                public float GyroscopicDamping;
                
                [TagStructure(Size = 0x8)]
                public class VehicleSteeringControlStruct : TagStructure
                {
                    public float OverdampenCuspAngle; // degrees
                    public float OverdampenExponent;
                }
                
                [TagStructure(Size = 0xC)]
                public class VehicleTurningControlStruct : TagStructure
                {
                    public float MaximumLeftTurn;
                    public float MaximumRightTurn;
                    public float TurnRate;
                }
                
                [TagStructure(Size = 0x30)]
                public class GlobalVehicleEngineStruct : TagStructure
                {
                    // higher moments make engine spin up slower
                    public float EngineMoment;
                    // higher moments make engine spin up slower
                    public float EngineMaxAngularVelocity;
                    public List<GearBlock> Gears;
                    [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                    public CachedTag GearShiftSound;
                    public List<LoadAndCruiseBlock> LoadAndCruiseSound;
                    
                    [TagStructure(Size = 0x5C)]
                    public class GearBlock : TagStructure
                    {
                        public TorqueCurveStruct LoadedTorqueCurve;
                        public TorqueCurveStruct CruisingTorqueCurve;
                        // seconds
                        public float MinTimeToUpshift;
                        // 0-1
                        public float EngineUpShiftScale;
                        public float GearRatio;
                        // seconds
                        public float MinTimeToDownshift;
                        // 0-1
                        public float EngineDownShiftScale;
                        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                        public CachedTag GearShiftSoundShiftingUp;
                        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                        public CachedTag GearShiftSoundShiftingDown;
                        
                        [TagStructure(Size = 0x14)]
                        public class TorqueCurveStruct : TagStructure
                        {
                            public ScalarFunctionNamedStruct Function;
                            
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
                    }
                    
                    [TagStructure(Size = 0x8)]
                    public class LoadAndCruiseBlock : TagStructure
                    {
                        public StringId LoadCruiseFunction;
                        public int AttachmentIndex;
                    }
                }
            }
            
            [TagStructure(Size = 0x30)]
            public class GuardianBlock : TagStructure
            {
                public VehicleSteeringControlStruct SteeringControl;
                public float MaximumForwardSpeed;
                public float MaximumReverseSpeed;
                public float SpeedAcceleration;
                public float SpeedDeceleration;
                public float MaximumLeftSlide;
                public float MaximumRightSlide;
                public float SlideAcceleration;
                public float SlideDeceleration;
                // 0 defaults to 1
                public float TorqueScale;
                public float AntiGravityForceZOffset;
                
                [TagStructure(Size = 0x8)]
                public class VehicleSteeringControlStruct : TagStructure
                {
                    public float OverdampenCuspAngle; // degrees
                    public float OverdampenExponent;
                }
            }
            
            [TagStructure(Size = 0x170)]
            public class JackalGliderBlock : TagStructure
            {
                public VehicleSteeringControlStruct SteeringControl;
                public float MaximumForwardSpeed;
                public float MaximumReverseSpeed;
                public float SpeedAcceleration;
                public float SpeedDeceleration;
                public float MaximumLeftSlide;
                public float MaximumRightSlide;
                public float SlideAcceleration;
                public float SlideDeceleration;
                // 0 defaults to 1
                public float TorqueScale;
                public EngineFunctionStruct EngineObjectFunction;
                public EngineFunctionStruct ContrailObjectFunction;
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
                public class VehicleSteeringControlStruct : TagStructure
                {
                    public float OverdampenCuspAngle; // degrees
                    public float OverdampenExponent;
                }
                
                [TagStructure(Size = 0x14)]
                public class EngineFunctionStruct : TagStructure
                {
                    // this is the name of the region by which we gauge the overall damage of the vehicle
                    public StringId ObjectFunctionDamageRegion;
                    // speed at which engine position funciton  moves.  value of 1 means goes from 0-1 in 1 second
                    public float MinAntiGravityEngineSpeed;
                    // speed at which engine position funciton  moves.  value of 1 means goes from 0-1 in 1 second
                    public float MaxAntiGravityEngineSpeed;
                    // strictly used for object funtion. in 0-1 space
                    public float EngineSpeedAcceleration;
                    // function is capped by speed of the vehicle. So when we slow down for any reason we see the function go down
                    public float MaximumVehicleSpeed;
                }
                
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
            
            [TagStructure(Size = 0x60)]
            public class BoatBlock : TagStructure
            {
                public BoatFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                // only used when 'use tank controls' is checked above
                public Angle TankControlForwardArc; // degrees
                public VehicleSteeringControlStruct SteeringControl;
                public VehicleTurningControlStruct TurningControl;
                // 0 means 0.  How hard is it to type 1?
                public float GravityScale;
                public GlobalVehicleEngineStruct Engine;
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
                
                [TagStructure(Size = 0x8)]
                public class VehicleSteeringControlStruct : TagStructure
                {
                    public float OverdampenCuspAngle; // degrees
                    public float OverdampenExponent;
                }
                
                [TagStructure(Size = 0xC)]
                public class VehicleTurningControlStruct : TagStructure
                {
                    public float MaximumLeftTurn;
                    public float MaximumRightTurn;
                    public float TurnRate;
                }
                
                [TagStructure(Size = 0x30)]
                public class GlobalVehicleEngineStruct : TagStructure
                {
                    // higher moments make engine spin up slower
                    public float EngineMoment;
                    // higher moments make engine spin up slower
                    public float EngineMaxAngularVelocity;
                    public List<GearBlock> Gears;
                    [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                    public CachedTag GearShiftSound;
                    public List<LoadAndCruiseBlock> LoadAndCruiseSound;
                    
                    [TagStructure(Size = 0x5C)]
                    public class GearBlock : TagStructure
                    {
                        public TorqueCurveStruct LoadedTorqueCurve;
                        public TorqueCurveStruct CruisingTorqueCurve;
                        // seconds
                        public float MinTimeToUpshift;
                        // 0-1
                        public float EngineUpShiftScale;
                        public float GearRatio;
                        // seconds
                        public float MinTimeToDownshift;
                        // 0-1
                        public float EngineDownShiftScale;
                        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                        public CachedTag GearShiftSoundShiftingUp;
                        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                        public CachedTag GearShiftSoundShiftingDown;
                        
                        [TagStructure(Size = 0x14)]
                        public class TorqueCurveStruct : TagStructure
                        {
                            public ScalarFunctionNamedStruct Function;
                            
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
                    }
                    
                    [TagStructure(Size = 0x8)]
                    public class LoadAndCruiseBlock : TagStructure
                    {
                        public StringId LoadCruiseFunction;
                        public int AttachmentIndex;
                    }
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
            }
            
            [TagStructure(Size = 0x110)]
            public class SpaceFighterBlock : TagStructure
            {
                public VehicleSteeringControlStruct SteeringControl;
                public VehicleTurningControlStruct TurningControl;
                // maximum speed when throttle = 1.0
                public float FullThrottleSpeed; // wu/s
                // speed when throttle = 0.0
                public float NeutralThrottleSpeed; // wu/s
                // speed when throttle = -1.0
                public float ReverseThrottleSpeed; // wu/s
                public float SpeedAcceleration; // wu/s/s
                public float SpeedDeceleration; // wu/s/s
                public float MaximumLeftSlide;
                public float MaximumRightSlide;
                public float SlideAcceleration;
                public float SlideDeceleration;
                // acceleration when trying to throttle against current slide direction
                public float SlideAccelAgainstDirection;
                // big vehicles need to scale this down.  0 defaults to 1, which is generally a good value.
                public float FlyingTorqueScale;
                public Angle FixedGunYaw;
                public Angle FixedGunPitch;
                public float MaximumTrickFrequency;
                public float LoopTrickDuration;
                public float RollTrickDuration;
                public float StrafeBoostScale;
                public float OffStickDecelerationScale;
                public float DiveSpeedScale;
                // max angular velocity for user-input roll
                public Angle RollMaxVelocity; // deg/sec
                // acceleration for user-input roll
                public Angle RollAcceleration; // deg/sec/sec
                // deceleration for user-input roll (when the user releases the stick)
                public Angle RollDeceleration; // deg/sec/sec
                // if non-zero, when the desired velocity change is less than this fraction of the acceleration, starts interpolating
                // the maximum acceleration towards zero.
                // You can think of this as a time in seconds where if the velocity would reach its target in this amount of time or
                // less, it will start taking longer.
                public float RollSmoothingFraction;
                // how long you must not rotate (roll, pitch, yaw) before autoleveling kicks in
                public float AutolevelTime; // s
                // if the ship is pitched outside of this range, autoleveling will not happen.  The effect is also scaled as the pitch
                // approaches this angle
                public Angle AutolevelPitchCutoff; // degrees
                // maximum angular velocity for autoleveling
                public Angle AutolevelMaxVelocity; // degrees/sec
                // maximum acceleration for autoleveling
                public Angle AutolevelMaxAcceleration; // deg/sec/sec
                // autolevel can continue to happen if the user is desiring an angular velocity lower than this
                public Angle AutolevelMaxUserAngVel; // deg/sec
                // controls relationship between displacement and acceleration - higher values mean faster acceleration when the
                // desired position is far from current position
                public float AutolevelSpringK;
                // controls relationship between velocity and friction - higher values will slow the system down, lower values may let
                // the system oscillate
                public float AutolevelSpringC;
                // desired roll = delta yaw X scale
                public float CosmeticRollScale;
                // maximum cosmetic roll angle
                public Angle CosmeticRollMaxBank; // degrees
                // maximum angular velocity that cosmetic roll can achieve
                public Angle CosmeticRollMaxVelocity; // deg/sec
                // maximum angular acceleration for cosmetic roll
                public Angle CosmeticRollAcceleration; // deg/sec/sec
                // controls relationship between displacement and acceleration - higher values mean faster acceleration when the
                // desired position is far from current position
                public float CosmeticRollSpringK;
                // controls relationship between velocity and friction - higher values will slow the system down, lower values may let
                // the system oscillate
                public float CosmeticRollSpringC;
                public SpaceFighterRollFlags RollFlags;
                public Angle MaximumLeftStickRollAngle;
                public float LeftStickRateSmoothing;
                public float LeftStickTrendSmoothing;
                public Angle MaximumRightStickRollAngle;
                public float RightStickRateSmoothing;
                public float RightStickTrendSmoothing;
                // turn deceleration kicks in when turning faster than this
                public Angle TurnDecelerationThreshold; // deg/sec
                // when turning at the maximum rate, throttle is limited to this value
                public float TurnDecelerationFraction;
                public SpaceFighterTurnBackFlags TurnBackFlags;
                public float TurnBackLatchedPeriod;
                public ScalarFunctionNamedStruct TurnBackDistanceToTurnRate;
                public float IdealThrustDecay;
                public float IdealThrustIncrease;
                public float MinimumThrustDecay;
                public float MinimumThrustIncrease;
                public float MaximumThrustIncrease;
                public float MinimumDiveAngle;
                public float MaximumDiveAngle;
                public float StrafeBoostPower;
                public float WingtipContrailTurn;
                public float WingtipMinTurn;
                // How much the position is predicted by the velocity to check against potential collisions
                public float DangerousTrajectoryPredictionTime; // seconds
                
                [Flags]
                public enum SpaceFighterRollFlags : uint
                {
                    UseNewRoll = 1 << 0
                }
                
                [Flags]
                public enum SpaceFighterTurnBackFlags : uint
                {
                    TurnBackToTangent = 1 << 0
                }
                
                [TagStructure(Size = 0x8)]
                public class VehicleSteeringControlStruct : TagStructure
                {
                    public float OverdampenCuspAngle; // degrees
                    public float OverdampenExponent;
                }
                
                [TagStructure(Size = 0xC)]
                public class VehicleTurningControlStruct : TagStructure
                {
                    public float MaximumLeftTurn;
                    public float MaximumRightTurn;
                    public float TurnRate;
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
            
            [TagStructure(Size = 0xCC)]
            public class RevenantBlock : TagStructure
            {
                public HumanTankStruct TankBlock;
                public float MaximumForwardSpeed;
                public float MaximumReverseSpeed;
                public float SpeedAcceleration;
                public float SpeedDeceleration;
                public float MaximumLeftSlide;
                public float MaximumRightSlide;
                public float SlideAcceleration;
                public float SlideDeceleration;
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
        }
        
        [TagStructure(Size = 0x54)]
        public class HavokVehiclePhysicsStruct : TagStructure
        {
            public HavokVehiclePhysicsDefinitionFlags Flags;
            // this sucks.  for friction based vehicles only
            public float GroundFriction;
            // this sucks.  for friction based vehicles only
            public float GroundDepth;
            // this sucks.  for friction based vehicles only
            public float GroundDampFactor;
            // this sucks.  for friction based vehicles only
            public float GroundMovingFriction;
            // degrees 0-90
            public float GroundMaximumSlope0;
            // degrees 0-90.  and greater than slope 0
            public float GroundMaximumSlope1;
            // 0 defaults to 3, this prevents the physics from becoming unstable when hucked against a surface
            public float MaximumNormalForceContribution;
            // lift per WU.
            public float AntiGravityBankLift;
            // how quickly we bank when we steer
            public float SteeringBankReactionScale;
            // value of 0 defaults to 1.  .5 is half gravity
            public float GravityScale;
            // ticks. 0 is default of 2
            public PhysicsUpdatePeriodEnum MaximumUpdatePeriodTicks;
            // ticks. when controlled by an ai or remote player. 0 defaults to 4
            public PhysicsUpdatePeriodEnum MaximumRemoteUpdatePeriodTicks;
            // 0 defaults to 1.  Number of iterations per frame of physics
            public int IterationCount;
            public int SuspensionCount;
            public List<AntiGravityPointDefinitionBlock> AntiGravityPoints;
            public List<FrictionPointDefinitionBlock> FrictionPoints;
            public BoatPhysicsDefinitionStruct BoatPhysics;
            
            [Flags]
            public enum HavokVehiclePhysicsDefinitionFlags : uint
            {
                HasSuspension = 1 << 0,
                FrictionPointsTestOnlyEnvironment = 1 << 1
            }
            
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
            
            [TagStructure(Size = 0x48)]
            public class AntiGravityPointDefinitionBlock : TagStructure
            {
                public StringId MarkerName;
                public AntiGravityPointDefinitionFlags Flags;
                public AntiGravityPointDefinitionDamping Damping;
                public float AntigravStrength;
                public float AntigravHeight;
                public float AntigravCompressionDamping;
                public float AntigravExtensionDamping;
                public float AntigravNormalK1;
                public float AntigravNormalK0;
                public float Radius;
                public StringId CollisionGlobalMaterialName;
                public short RuntimeGlobalMaterialIndex;
                public short Wu;
                public StringId DamageSourceRegionName;
                public float DefaultStateError;
                public float MinorDamageError;
                public float MediumDamageError;
                public float MajorDamageError;
                public float DestroyedStateError;
                
                [Flags]
                public enum AntiGravityPointDefinitionFlags : uint
                {
                    GetsDamageFromRegion = 1 << 0,
                    OnlyActiveOnWater = 1 << 1
                }
                
                public enum AntiGravityPointDefinitionDamping : int
                {
                    Normal,
                    DampedLeft,
                    DampedRight,
                    UndampedLeft,
                    UndampedRight
                }
            }
            
            [TagStructure(Size = 0x44)]
            public class FrictionPointDefinitionBlock : TagStructure
            {
                public StringId MarkerName;
                public FrictionPointDefinitionFlags Flags;
                // (0.0-1.0) fraction of total vehicle mass
                public float FractionOfTotalMass;
                public float Radius;
                // radius when the tire is blown off.
                public float DamagedRadius;
                // scale the non sliding friction on this friction point
                public float GroundFrictionScale;
                // use this value when an AI is driving
                public float AiGroundFrictionScaleOverride;
                public float MovingFrictionVelocityDiff;
                // scale the sliding friction on this friction point, active when velocity is greater than moving friction velocity
                // diff
                public float MovingFrictionScale;
                // use this value when an AI is driving
                public float AiMovingFrictionScaleOverride;
                public float EBrakeMovingFriction;
                public float EBrakeFriction;
                public float EBrakeMovingFrictionVelDiff;
                public StringId CollisionGlobalMaterialName;
                public short RuntimeGlobalMaterialIndex;
                // only need point can destroy flag set
                public ModelStateEnum ModelStateDestroyed;
                // only need point can destroy flag set
                public StringId RegionName;
                public int RuntimeRegionIndex;
                
                [Flags]
                public enum FrictionPointDefinitionFlags : uint
                {
                    GetsDamageFromRegion = 1 << 0,
                    Powered = 1 << 1,
                    FrontTurning = 1 << 2,
                    RearTurning = 1 << 3,
                    AiForceRearTurning = 1 << 4,
                    AttachedToEBrake = 1 << 5,
                    CanBeDestroyed = 1 << 6,
                    AiOverrideGroundFrictionScale = 1 << 7,
                    AiOverrideMovingFrictionScale = 1 << 8
                }
                
                public enum ModelStateEnum : short
                {
                    Default,
                    MinorDamage,
                    MediumDamage,
                    MajorDamage,
                    Destroyed
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class BoatPhysicsDefinitionStruct : TagStructure
            {
                // for hull surfaces that drive on ground this is how far we pretend the water is above everything physical
                public float GroundVerticalExtrusion;
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
            // sloppiness of the camera
            // only applies to vehicles
            public float CameraInterpolationTime; // s
            // how long before the end of the trick we start using the below values
            public float TrickExitTime; // s
            // sloppiness of the camera when exiting the trick
            // we interpolate between these values depending on how far your camera was displaced from the ideal camera
            public Bounds<float> TrickExitCameraInterpolationTime; // s
            // when your camera is this far from the ideal at the start of the trick, we use the maximum delay time above
            // only for space fighter
            public float TrickExitDisplacementReference; // wu
            // after ending this trick, how long until I can trick again
            // only applies to vehicles
            public float CooldownTime; // s
            
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
        
        [TagStructure(Size = 0x10)]
        public class SoundRtpcblock : TagStructure
        {
            // Sound attachment to affect
            public int AttachmentIndex;
            // Function to drive the RTPC
            public StringId Function;
            // WWise RTPC string name
            public uint RtpcName;
            public int RtpcNameHashValue;
        }
        
        [TagStructure(Size = 0x1C)]
        public class SoundSweetenerBlock : TagStructure
        {
            // Function to trigger the sweetener
            public StringId Function;
            [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
            public CachedTag Sound;
            // value of the function (between 0 and 1) where the sound is triggered
            public float SwitchPoint;
            // 0 for triggering while function is decreasing, 1 for increasing (more modes to come?)
            public int Mode;
        }
        
        [TagStructure(Size = 0x14)]
        public class PhysicsTransitionsBlock : TagStructure
        {
            // speed at which flight model transition occurs
            public float TransitionVelocity; // wu/s
            // 0 if velocity should be smaller than transition value, else 1
            public float VelocityThresholdSide; // 0 or 1
            // throttle input at which physics model transition occurs
            public float TransitionThrottle; // -1 to 1
            // 0 if throttle should be smaller than transition value, else 1
            public float ThrottleThresholdSide; // 0 or 1
            // upon reaching transition velocity, act like this vehicle type
            public VehicleTypeEnum TransitionTargetVehicleType;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum VehicleTypeEnum : sbyte
            {
                VehicleTypeHumanTank,
                VehicleTypeHumanJeep,
                VehicleTypeHumanPlane,
                VehicleTypeWolverine,
                VehicleTypeAlienScout,
                VehicleTypeAlienFighter,
                VehicleTypeTurret,
                VehicleTypeMantis,
                VehicleTypeVtol,
                VehicleTypeChopper,
                VehicleTypeGuardian,
                VehicleTypeJackalGlider,
                VehicleTypeBoat,
                VehicleTypeSpaceFighter,
                VehicleTypeRevenant
            }
        }
    }
}
