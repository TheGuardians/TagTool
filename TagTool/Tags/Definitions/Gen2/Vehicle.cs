using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "vehicle", Tag = "vehi", Size = 0x3D8)]
    public class Vehicle : TagStructure
    {
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        public FlagsValue Flags;
        public float BoundingRadius; // world units
        public RealPoint3d BoundingOffset;
        public float AccelerationScale; // [0,+inf]
        public LightmapShadowModeValue LightmapShadowMode;
        public SweetenerSizeValue SweetenerSize;
        [TagField(Flags = Padding, Length = 1)]
        public byte[] Padding2;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding3;
        public float DynamicLightSphereRadius; // sphere to use for dynamic lights and shadows. only used if not 0
        public RealPoint3d DynamicLightSphereOffset; // only used if radius not 0
        public StringId DefaultModelVariant;
        public CachedTag Model;
        public CachedTag CrateObject;
        public CachedTag ModifierShader;
        public CachedTag CreationEffect;
        public CachedTag MaterialEffects;
        public List<ObjectAiProperties> AiProperties;
        public List<ObjectFunctionDefinition> Functions;
        /// <summary>
        /// Applying collision damage
        /// </summary>
        /// <remarks>
        /// for things that want to cause more or less collision damage
        /// </remarks>
        public float ApplyCollisionDamageScale; // 0 means 1.  1 is standard scale.  Some things may want to apply more damage
        /// <summary>
        /// Game collision damage parameters
        /// </summary>
        /// <remarks>
        /// 0 - means take default value from globals.globals
        /// </remarks>
        public float MinGameAccDefault; // 0-oo
        public float MaxGameAccDefault; // 0-oo
        public float MinGameScaleDefault; // 0-1
        public float MaxGameScaleDefault; // 0-1
        /// <summary>
        /// Absolute collision damage parameters
        /// </summary>
        /// <remarks>
        /// 0 - means take default value from globals.globals
        /// </remarks>
        public float MinAbsAccDefault; // 0-oo
        public float MaxAbsAccDefault; // 0-oo
        public float MinAbsScaleDefault; // 0-1
        public float MaxAbsScaleDefault; // 0-1
        public short HudTextMessageIndex;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding4;
        public List<ObjectAttachmentDefinition> Attachments;
        public List<ObjectDefinitionWidget> Widgets;
        public List<OldObjectFunctionDefinition> OldFunctions;
        public List<ObjectChangeColorDefinition> ChangeColors;
        public List<PredictedResource> PredictedResources;
        /// <summary>
        /// $$$ UNIT $$$
        /// </summary>
        public FlagsValue Flags1;
        public DefaultTeamValue DefaultTeam;
        public ConstantSoundVolumeValue ConstantSoundVolume;
        public CachedTag IntegratedLightToggle;
        public Angle CameraFieldOfView; // degrees
        public float CameraStiffness;
        public UnitCameraStruct UnitCamera;
        public UnitSeatAcceleration Acceleration;
        public float SoftPingThreshold; // [0,1]
        public float SoftPingInterruptTime; // seconds
        public float HardPingThreshold; // [0,1]
        public float HardPingInterruptTime; // seconds
        public float HardDeathThreshold; // [0,1]
        public float FeignDeathThreshold; // [0,1]
        public float FeignDeathTime; // seconds
        public float DistanceOfEvadeAnim; // world units
        public float DistanceOfDiveAnim; // world units
        public float StunnedMovementThreshold; // [0,1]
        public float FeignDeathChance; // [0,1]
        public float FeignRepeatChance; // [0,1]
        public CachedTag SpawnedTurretCharacter; // automatically created character when this unit is driven
        public Bounds<short> SpawnedActorCount; // number of actors which we spawn
        public float SpawnedVelocity; // velocity at which we throw spawned actors
        public Angle AimingVelocityMaximum; // degrees per second
        public Angle AimingAccelerationMaximum; // degrees per second squared
        public float CasualAimingModifier; // [0,1]
        public Angle LookingVelocityMaximum; // degrees per second
        public Angle LookingAccelerationMaximum; // degrees per second squared
        public StringId RightHandNode; // where the primary weapon is attached
        public StringId LeftHandNode; // where the seconday weapon is attached (for dual-pistol modes)
        public UnitAdditionalNodeNames MoreDamnNodes;
        public CachedTag MeleeDamage;
        public UnitBoardingMeleeStructBlock YourMomma;
        public MotionSensorBlipSizeValue MotionSensorBlipSize;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding12;
        public List<PostureDefinition> Postures;
        public List<UnitHudReference> NewHudInterfaces;
        public List<DialogueVariantDefinition> DialogueVariants;
        public float GrenadeVelocity; // world units per second
        public GrenadeTypeValue GrenadeType;
        public short GrenadeCount;
        public List<PoweredSeatDefinition> PoweredSeats;
        public List<UnitInitialWeapon> Weapons;
        public List<UnitSeat> Seats;
        /// <summary>
        /// Boost
        /// </summary>
        public UnitBoostStructBlock Boost;
        /// <summary>
        /// Lipsync
        /// </summary>
        public UnitLipsyncScales Lipsync;
        /// <summary>
        /// $$$ VEHICLE $$$
        /// </summary>
        public FlagsValue Flags3;
        public TypeValue Type;
        public ControlValue Control;
        public float MaximumForwardSpeed;
        public float MaximumReverseSpeed;
        public float SpeedAcceleration;
        public float SpeedDeceleration;
        public float MaximumLeftTurn;
        public float MaximumRightTurnNegative;
        public float WheelCircumference;
        public float TurnRate;
        public float BlurSpeed;
        public SpecificTypeValue SpecificType; // if your type corresponds to something in this list choose it
        public PlayerTrainingVehicleTypeValue PlayerTrainingVehicleType;
        public StringId FlipMessage;
        public float TurnScale;
        public float SpeedTurnPenaltyPower052;
        public float SpeedTurnPenalty0None1CanTTurnAtTopSpeed;
        public float MaximumLeftSlide;
        public float MaximumRightSlide;
        public float SlideAcceleration;
        public float SlideDeceleration;
        public float MinimumFlippingAngularVelocity;
        public float MaximumFlippingAngularVelocity;
        public VehicleSizeValue VehicleSize; // The size determine what kind of seats in larger vehicles it may occupy (i.e. small or large cargo seats)
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding14;
        public float FixedGunYaw;
        public float FixedGunPitch;
        /// <summary>
        /// steering overdampening
        /// </summary>
        /// <remarks>
        /// when the steering is off by more than the cusp angle
        /// the steering will overcompensate more and more.  when it
        /// is less, it overcompensates less and less.  the exponent
        /// should be something in the neighborhood of 2.0
        /// 
        /// </remarks>
        public float OverdampenCuspAngle; // degrees
        public float OverdampenExponent;
        public float CrouchTransitionTime; // seconds
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding25;
        /// <summary>
        /// engine
        /// </summary>
        public float EngineMoment; // higher moments make engine spin up slower
        public float EngineMaxAngularVelocity; // higher moments make engine spin up slower
        public List<VehicleGear> Gears;
        public float FlyingTorqueScale; // big vehicles need to scale this down.  0 defaults to 1, which is generally a good value.  This is used with alien fighter physics
        public float SeatEnteranceAccelerationScale; // how much do we scale the force the biped the applies down on the seat when he enters. 0 == no acceleration
        public float SeatExitAccelersationScale; // how much do we scale the force the biped the applies down on the seat when he exits. 0 == no acceleration
        public float AirFrictionDeceleration; // human plane physics only. 0 is nothing.  1 is like thowing the engine to full reverse
        public float ThrustScale; // human plane physics only. 0 is default (1)
        /// <summary>
        /// sounds and effects
        /// </summary>
        public CachedTag SuspensionSound;
        public CachedTag CrashSound;
        public CachedTag Unused;
        public CachedTag SpecialEffect;
        public CachedTag UnusedEffect;
        /// <summary>
        /// physics
        /// </summary>
        public HavokVehiclePhysicsDefinition HavokVehiclePhysics;
        
        [Flags]
        public enum FlagsValue : ushort
        {
            DoesNotCastShadow = 1 << 0,
            SearchCardinalDirectionLightmapsOnFailure = 1 << 1,
            Unused = 1 << 2,
            NotAPathfindingObstacle = 1 << 3,
            ExtensionOfParent = 1 << 4,
            DoesNotCauseCollisionDamage = 1 << 5,
            EarlyMover = 1 << 6,
            EarlyMoverLocalizedPhysics = 1 << 7,
            UseStaticMassiveLightmapSample = 1 << 8,
            ObjectScalesAttachments = 1 << 9,
            InheritsPlayerSAppearance = 1 << 10,
            DeadBipedsCanTLocalize = 1 << 11,
            AttachToClustersByDynamicSphere = 1 << 12,
            EffectsCreatedByThisObjectDoNotSpawnObjectsInMultiplayer = 1 << 13
        }
        
        public enum LightmapShadowModeValue : short
        {
            Default,
            Never,
            Always
        }
        
        public enum SweetenerSizeValue : sbyte
        {
            Small,
            Medium,
            Large
        }
        
        [TagStructure(Size = 0x10)]
        public class ObjectAiProperties : TagStructure
        {
            public AiFlagsValue AiFlags;
            public StringId AiTypeName; // used for combat dialogue, etc.
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public AiSizeValue AiSize;
            public LeapJumpSpeedValue LeapJumpSpeed;
            
            [Flags]
            public enum AiFlagsValue : uint
            {
                DetroyableCover = 1 << 0,
                PathfindingIgnoreWhenDead = 1 << 1,
                DynamicCover = 1 << 2
            }
            
            public enum AiSizeValue : short
            {
                Default,
                Tiny,
                Small,
                Medium,
                Large,
                Huge,
                Immobile
            }
            
            public enum LeapJumpSpeedValue : short
            {
                None,
                Down,
                Step,
                Crouch,
                Stand,
                Storey,
                Tower,
                Infinite
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class ObjectFunctionDefinition : TagStructure
        {
            public FlagsValue Flags;
            public StringId ImportName;
            public StringId ExportName;
            public StringId TurnOffWith; // if the specified function is off, so is this function
            public float MinValue; // function must exceed this value (after mapping) to be active 0. means do nothing
            public FunctionDefinition DefaultFunction;
            public StringId ScaleBy;
            
            [Flags]
            public enum FlagsValue : uint
            {
                Invert = 1 << 0,
                MappingDoesNotControlsActive = 1 << 1,
                AlwaysActive = 1 << 2,
                RandomTimeOffset = 1 << 3
            }
            
            [TagStructure(Size = 0xC)]
            public class FunctionDefinition : TagStructure
            {
                public List<Byte> Data;
                
                [TagStructure(Size = 0x1)]
                public class Byte : TagStructure
                {
                    public sbyte Value;
                }
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class ObjectAttachmentDefinition : TagStructure
        {
            public CachedTag Type;
            public StringId Marker;
            public ChangeColorValue ChangeColor;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public StringId PrimaryScale;
            public StringId SecondaryScale;
            
            public enum ChangeColorValue : short
            {
                None,
                Primary,
                Secondary,
                Tertiary,
                Quaternary
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class ObjectDefinitionWidget : TagStructure
        {
            public CachedTag Type;
        }
        
        [TagStructure(Size = 0x50)]
        public class OldObjectFunctionDefinition : TagStructure
        {
            [TagField(Flags = Padding, Length = 76)]
            public byte[] Padding1;
            public StringId Unknown1;
        }
        
        [TagStructure(Size = 0x18)]
        public class ObjectChangeColorDefinition : TagStructure
        {
            public List<ObjectChangeColorInitialPermutation> InitialPermutations;
            public List<ObjectChangeColorFunction> Functions;
            
            [TagStructure(Size = 0x20)]
            public class ObjectChangeColorInitialPermutation : TagStructure
            {
                public float Weight;
                public RealRgbColor ColorLowerBound;
                public RealRgbColor ColorUpperBound;
                public StringId VariantName; // if empty, may be used by any model variant
            }
            
            [TagStructure(Size = 0x28)]
            public class ObjectChangeColorFunction : TagStructure
            {
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding1;
                public ScaleFlagsValue ScaleFlags;
                public RealRgbColor ColorLowerBound;
                public RealRgbColor ColorUpperBound;
                public StringId DarkenBy;
                public StringId ScaleBy;
                
                [Flags]
                public enum ScaleFlagsValue : uint
                {
                    BlendInHsv = 1 << 0,
                    MoreColors = 1 << 1
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class PredictedResource : TagStructure
        {
            public TypeValue Type;
            public short ResourceIndex;
            public int TagIndex;
            
            public enum TypeValue : short
            {
                Bitmap,
                Sound,
                RenderModelGeometry,
                ClusterGeometry,
                ClusterInstancedGeometry,
                LightmapGeometryObjectBuckets,
                LightmapGeometryInstanceBuckets,
                LightmapClusterBitmaps,
                LightmapInstanceBitmaps
            }
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
            Unused8,
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
        
        [TagStructure(Size = 0x20)]
        public class UnitCameraStruct : TagStructure
        {
            public StringId CameraMarkerName;
            public StringId CameraSubmergedMarkerName;
            public Angle PitchAutoLevel;
            public Bounds<Angle> PitchRange;
            public List<UnitCameraTrack> CameraTracks;
            
            [TagStructure(Size = 0x10)]
            public class UnitCameraTrack : TagStructure
            {
                public CachedTag Track;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class UnitSeatAcceleration : TagStructure
        {
            public RealVector3d AccelerationRange; // world units per second squared
            public float AccelActionScale; // actions fail [0,1+]
            public float AccelAttachScale; // detach unit [0,1+]
        }
        
        [TagStructure(Size = 0x4)]
        public class UnitAdditionalNodeNames : TagStructure
        {
            public StringId PreferredGunNode; // if found, use this gun marker
        }
        
        [TagStructure(Size = 0x50)]
        public class UnitBoardingMeleeStructBlock : TagStructure
        {
            public CachedTag BoardingMeleeDamage;
            public CachedTag BoardingMeleeResponse;
            public CachedTag LandingMeleeDamage;
            public CachedTag FlurryMeleeDamage;
            public CachedTag ObstacleSmashDamage;
        }
        
        public enum MotionSensorBlipSizeValue : short
        {
            Medium,
            Small,
            Large
        }
        
        [TagStructure(Size = 0x10)]
        public class PostureDefinition : TagStructure
        {
            public StringId Name;
            public RealVector3d PillOffset;
        }
        
        [TagStructure(Size = 0x10)]
        public class UnitHudReference : TagStructure
        {
            public CachedTag NewUnitHudInterface;
        }
        
        [TagStructure(Size = 0x14)]
        public class DialogueVariantDefinition : TagStructure
        {
            public short VariantNumber; // variant number to use this dialogue with (must match the suffix in the permutations on the unit's model)
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public CachedTag Dialogue;
        }
        
        public enum GrenadeTypeValue : short
        {
            HumanFragmentation,
            CovenantPlasma
        }
        
        [TagStructure(Size = 0x8)]
        public class PoweredSeatDefinition : TagStructure
        {
            public float DriverPowerupTime; // seconds
            public float DriverPowerdownTime; // seconds
        }
        
        [TagStructure(Size = 0x10)]
        public class UnitInitialWeapon : TagStructure
        {
            public CachedTag Weapon;
        }
        
        [TagStructure(Size = 0xC0)]
        public class UnitSeat : TagStructure
        {
            public FlagsValue Flags;
            public StringId Label;
            public StringId MarkerName;
            public StringId EntryMarkerSName;
            public StringId BoardingGrenadeMarker;
            public StringId BoardingGrenadeString;
            public StringId BoardingMeleeString;
            public float PingScale; // nathan is too lazy to make pings for each seat.
            public float TurnoverTime; // seconds
            public UnitSeatAcceleration Acceleration;
            public float AiScariness;
            public AiSeatTypeValue AiSeatType;
            public short BoardingSeat;
            public float ListenerInterpolationFactor; // how far to interpolate listener position from camera to occupant's head
            /// <summary>
            /// speed dependant turn rates
            /// </summary>
            /// <remarks>
            /// when the unit velocity is 0, the yaw/pitch rates are the left values
            /// at [max speed reference], the yaw/pitch rates are the right values.
            /// the max speed reference is what the code uses to generate a clamped speed from 0..1
            /// the exponent controls how midrange speeds are interpreted.
            /// </remarks>
            public Bounds<float> YawRateBounds; // degrees per second
            public Bounds<float> PitchRateBounds; // degrees per second
            public float MinSpeedReference;
            public float MaxSpeedReference;
            public float SpeedExponent;
            /// <summary>
            /// camera fields
            /// </summary>
            public UnitCameraStruct UnitCamera;
            public List<UnitHudReference> UnitHudInterface;
            public StringId EnterSeatString;
            public Angle YawMinimum;
            public Angle YawMaximum;
            public CachedTag BuiltInGunner;
            /// <summary>
            /// entry fields
            /// </summary>
            /// <remarks>
            /// note: the entry radius shouldn't exceed 3 world units, 
            /// as that is as far as the player will search for a vehicle
            /// to enter.
            /// </remarks>
            public float EntryRadius; // how close to the entry marker a unit must be
            public Angle EntryMarkerConeAngle; // angle from marker forward the unit must be
            public Angle EntryMarkerFacingAngle; // angle from unit facing the marker must be
            public float MaximumRelativeVelocity;
            public StringId InvisibleSeatRegion;
            public int RuntimeInvisibleSeatRegionIndex;
            
            [Flags]
            public enum FlagsValue : uint
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
                AllowAiNoncombatants = 1 << 10,
                BoardingSeat = 1 << 11,
                AiFiringDisabledByMaxAcceleration = 1 << 12,
                BoardingEntersSeat = 1 << 13,
                BoardingNeedAnyPassenger = 1 << 14,
                ControlsOpenAndClose = 1 << 15,
                InvalidForPlayer = 1 << 16,
                InvalidForNonPlayer = 1 << 17,
                GunnerPlayerOnly = 1 << 18,
                InvisibleUnderMajorDamage = 1 << 19
            }
            
            [TagStructure(Size = 0x14)]
            public class UnitSeatAcceleration : TagStructure
            {
                public RealVector3d AccelerationRange; // world units per second squared
                public float AccelActionScale; // actions fail [0,1+]
                public float AccelAttachScale; // detach unit [0,1+]
            }
            
            public enum AiSeatTypeValue : short
            {
                None,
                Passenger,
                Gunner,
                SmallCargo,
                LargeCargo,
                Driver
            }
            
            [TagStructure(Size = 0x20)]
            public class UnitCameraStruct : TagStructure
            {
                public StringId CameraMarkerName;
                public StringId CameraSubmergedMarkerName;
                public Angle PitchAutoLevel;
                public Bounds<Angle> PitchRange;
                public List<UnitCameraTrack> CameraTracks;
                
                [TagStructure(Size = 0x10)]
                public class UnitCameraTrack : TagStructure
                {
                    public CachedTag Track;
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class UnitHudReference : TagStructure
            {
                public CachedTag NewUnitHudInterface;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class UnitBoostStructBlock : TagStructure
        {
            public float BoostPeakPower;
            public float BoostRisePower;
            public float BoostPeakTime;
            public float BoostFallPower;
            public float DeadTime;
        }
        
        [TagStructure(Size = 0x8)]
        public class UnitLipsyncScales : TagStructure
        {
            public float AttackWeight;
            public float DecayWeight;
        }
        
        public enum TypeValue : short
        {
            HumanTank,
            HumanJeep,
            HumanBoat,
            HumanPlane,
            AlienScout,
            AlienFighter,
            Turret
        }
        
        public enum ControlValue : short
        {
            VehicleControlNormal,
            VehicleControlUnused,
            VehicleControlTank
        }
        
        public enum SpecificTypeValue : short
        {
            None,
            Ghost,
            Wraith,
            Spectre,
            SentinelEnforcer
        }
        
        public enum PlayerTrainingVehicleTypeValue : short
        {
            None,
            Warthog,
            WarthogTurret,
            Ghost,
            Banshee,
            Tank,
            Wraith
        }
        
        public enum VehicleSizeValue : short
        {
            Small,
            Large
        }
        
        [TagStructure(Size = 0x44)]
        public class VehicleGear : TagStructure
        {
            /// <summary>
            /// loaded torque
            /// </summary>
            public ToruqeCurve LoadedTorqueCurve;
            /// <summary>
            /// cruising torque
            /// </summary>
            public ToruqeCurve CruisingTorqueCurve;
            /// <summary>
            /// gearing
            /// </summary>
            public float MinTimeToUpshift; // seconds
            public float EngineUpShiftScale; // 0-1
            public float GearRatio;
            public float MinTimeToDownshift; // seconds
            public float EngineDownShiftScale; // 0-1
            
            [TagStructure(Size = 0x18)]
            public class ToruqeCurve : TagStructure
            {
                public float MinTorque;
                public float MaxTorque;
                public float PeakTorqueScale;
                public float PastPeakTorqueExponent;
                public float TorqueAtMaxAngularVelocity; // generally 0 for loading torque and something less than max torque for cruising torque
                public float TorqueAt2xMaxAngularVelocity;
            }
        }
        
        [TagStructure(Size = 0x60)]
        public class HavokVehiclePhysicsDefinition : TagStructure
        {
            public FlagsValue Flags;
            public float GroundFriction; //  for friction based vehicles only
            public float GroundDepth; //  for friction based vehicles only
            public float GroundDampFactor; //  for friction based vehicles only
            public float GroundMovingFriction; //  for friction based vehicles only
            public float GroundMaximumSlope0; // degrees 0-90
            public float GroundMaximumSlope1; // degrees 0-90.  and greater than slope 0
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding1;
            public float AntiGravityBankLift; // lift per WU.
            public float SteeringBankReactionScale; // how quickly we bank when we steer 
            public float GravityScale; // value of 0 defaults to 1.  .5 is half gravity
            public float Radius; // generated from the radius of the hkConvexShape for this vehicle
            public List<AntiGravityPointDefinition> AntiGravityPoints;
            public List<FrictionPointDefinition> FrictionPoints;
            public List<VehiclePhantomShapeDefinition> ShapePhantomShape;
            
            [Flags]
            public enum FlagsValue : uint
            {
                Invalid = 1 << 0
            }
            
            [TagStructure(Size = 0x4C)]
            public class AntiGravityPointDefinition : TagStructure
            {
                public StringId MarkerName;
                public FlagsValue Flags;
                public float AntigravStrength;
                public float AntigravOffset;
                public float AntigravHeight;
                public float AntigravDampFactor;
                public float AntigravNormalK1;
                public float AntigravNormalK0;
                public float Radius;
                [TagField(Flags = Padding, Length = 12)]
                public byte[] Padding1;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding3;
                public StringId DamageSourceRegionName;
                public float DefaultStateError;
                public float MinorDamageError;
                public float MediumDamageError;
                public float MajorDamageError;
                public float DestroyedStateError;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    GetsDamageFromRegion = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x4C)]
            public class FrictionPointDefinition : TagStructure
            {
                public StringId MarkerName;
                public FlagsValue Flags;
                public float FractionOfTotalMass; // (0.0-1.0) fraction of total vehicle mass
                public float Radius;
                public float DamagedRadius; // radius when the tire is blown off.
                public FrictionTypeValue FrictionType;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public float MovingFrictionVelocityDiff;
                public float EBrakeMovingFriction;
                public float EBrakeFriction;
                public float EBrakeMovingFrictionVelDiff;
                [TagField(Flags = Padding, Length = 20)]
                public byte[] Padding2;
                public StringId CollisionGlobalMaterialName;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding3;
                /// <summary>
                /// friction point destruction data
                /// </summary>
                public ModelStateDestroyedValue ModelStateDestroyed; // only need point can destroy flag set
                public StringId RegionName; // only need point can destroy flag set
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding4;
                
                [Flags]
                public enum FlagsValue : uint
                {
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
                    Forward
                }
                
                public enum ModelStateDestroyedValue : short
                {
                    Default,
                    MinorDamage,
                    MediumDamage,
                    MajorDamage,
                    Destroyed
                }
            }
            
            [TagStructure(Size = 0x2A0)]
            public class VehiclePhantomShapeDefinition : TagStructure
            {
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown1;
                public short Size;
                public short Count;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown2;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown3;
                public int ChildShapesSize;
                public int ChildShapesCapacity;
                public ShapeTypeValue ShapeType;
                public short Shape;
                public int CollisionFilter;
                [TagField(Length = 4)]
                public ChildShapesStorageDatum[] ChildShapesStorage;
                public int MultisphereCount;
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 8)]
                public byte[] Padding1;
                public float X0;
                public float X1;
                public float Y0;
                public float Y1;
                public float Z0;
                public float Z1;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown5;
                public short Size1;
                public short Count2;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown6;
                public int NumSpheres;
                public RealVector3d Sphere;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown7;
                [TagField(Length = 8)]
                public FourVectorsStorageDatum[] FourVectorsStorage;
                [TagField(Length = 4)]
                public MultispheresDatum[] Multispheres;
                
                public enum ShapeTypeValue : short
                {
                    Sphere,
                    Pill,
                    Box,
                    Triangle,
                    Polyhedron,
                    MultiSphere,
                    Unused0,
                    Unused1,
                    Unused2,
                    Unused3,
                    Unused4,
                    Unused5,
                    Unused6,
                    Unused7,
                    List,
                    Mopp
                }
                
                [TagStructure()]
                public class ChildShapesStorageDatum : TagStructure
                {
                    public int CollisionFilter;
                    public short Shape;
                    public ShapeTypeValue ShapeType;
                    
                    public enum ShapeTypeValue : short
                    {
                        Sphere,
                        Pill,
                        Box,
                        Triangle,
                        Polyhedron,
                        MultiSphere,
                        Unused0,
                        Unused1,
                        Unused2,
                        Unused3,
                        Unused4,
                        Unused5,
                        Unused6,
                        Unused7,
                        List,
                        Mopp
                    }
                }
                
                [Flags]
                public enum FlagsValue : uint
                {
                    HasAabbPhantom = 1 << 0,
                    Bit1 = 1 << 1
                }
                
                [TagStructure()]
                public class FourVectorsStorageDatum : TagStructure
                {
                    [TagField(Flags = Padding, Length = 4)]
                    public byte[] Unknown1;
                    public RealVector3d Sphere;
                }
                
                [TagStructure()]
                public class MultispheresDatum : TagStructure
                {
                    public int NumSpheres;
                    [TagField(Flags = Padding, Length = 4)]
                    public byte[] Unknown1;
                    public short Count;
                    public short Size;
                    [TagField(Flags = Padding, Length = 4)]
                    public byte[] Unknown2;
                }
            }
        }
    }
}

