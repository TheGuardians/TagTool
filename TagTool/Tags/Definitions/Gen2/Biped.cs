using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "biped", Tag = "bipd", Size = 0x3DC)]
    public class Biped : TagStructure
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
        /// $$$ BIPED $$$
        /// </summary>
        public Angle MovingTurningSpeed; // degrees per second
        public FlagsValue Flags3;
        public Angle StationaryTurningThreshold;
        /// <summary>
        /// jumping and landing
        /// </summary>
        public float JumpVelocity; // world units per second
        public float MaximumSoftLandingTime; // seconds
        public float MaximumHardLandingTime; // seconds
        public float MinimumSoftLandingVelocity; // world units per second
        public float MinimumHardLandingVelocity; // world units per second
        public float MaximumHardLandingVelocity; // world units per second
        public float DeathHardLandingVelocity; // world units per second
        public float StunDuration; // 0 is the default.  Bipeds are stuned when damaged by vehicle collisions, also some are when they take emp damage
        /// <summary>
        /// camera, collision, and autoaim
        /// </summary>
        public float StandingCameraHeight; // world units
        public float CrouchingCameraHeight; // world units
        public float CrouchTransitionTime; // seconds
        public Angle CameraInterpolationStart; // degrees
        public Angle CameraInterpolationEnd; // degrees
        public float CameraForwardMovementScale; // amount of fp camera movement forward and back (1.0 is full)
        public float CameraSideMovementScale; // amount of fp camera movement side-to-side (1.0 is full)
        public float CameraVerticalMovementScale; // amount of fp camera movement vertically (1.0 is full)
        public float CameraExclusionDistance; // world units
        public float AutoaimWidth; // world units
        public BipedLockOnData LockOnData;
        [TagField(Flags = Padding, Length = 16)]
        public byte[] Padding14;
        public float HeadShotAccScale; // when the biped ragdolls from a head shot it acceleartes based on this value.  0 defaults to the standard acceleration scale
        public CachedTag AreaDamageEffect;
        public CharacterPhysicsDefinition Physics;
        public List<BipedContactPoint> ContactPoints; // these are the points where the biped touches the ground
        public CachedTag ReanimationCharacter; // when the flood reanimate this guy, he turns into a ...
        public CachedTag DeathSpawnCharacter; // when I die, out of the ashes of my death crawls a ...
        public short DeathSpawnCount;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding25;
        
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
        
        [TagStructure(Size = 0x8)]
        public class BipedLockOnData : TagStructure
        {
            /// <summary>
            /// lock-on fields
            /// </summary>
            public FlagsValue Flags;
            public float LockOnDistance;
            
            [Flags]
            public enum FlagsValue : uint
            {
                LockedByHumanTargeting = 1 << 0,
                LockedByPlasmaTargeting = 1 << 1,
                AlwaysLockedByPlasmaTargeting = 1 << 2
            }
        }
        
        [TagStructure(Size = 0xA0)]
        public class CharacterPhysicsDefinition : TagStructure
        {
            public FlagsValue Flags;
            public float HeightStanding;
            public float HeightCrouching;
            public float Radius;
            public float Mass;
            public StringId LivingMaterialName; // collision material used when character is alive
            public StringId DeadMaterialName; // collision material used when character is dead
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public List<PhysicsModelSphere> DeadSphereShapes;
            public List<PhysicsModelPill> PillShapes;
            public List<PhysicsModelSphere> SphereShapes;
            /// <summary>
            /// ground
            /// </summary>
            public CharacterPhysicsGroundDefinition GroundPhysics;
            /// <summary>
            /// flying
            /// </summary>
            public CharacterPhysicsFlyingDefinition FlyingPhysics;
            /// <summary>
            /// dead
            /// </summary>
            public CharacterPhysicsDeadStructBlock DeadPhysics;
            /// <summary>
            /// sentinel
            /// </summary>
            public CharacterPhysicsSentinelStructBlock SentinelPhysics;
            
            [Flags]
            public enum FlagsValue : uint
            {
                CenteredAtOrigin = 1 << 0,
                ShapeSpherical = 1 << 1,
                UsePlayerPhysics = 1 << 2,
                ClimbAnySurface = 1 << 3,
                Flying = 1 << 4,
                NotPhysical = 1 << 5,
                DeadCharacterCollisionGroup = 1 << 6
            }
            
            [TagStructure(Size = 0x80)]
            public class PhysicsModelSphere : TagStructure
            {
                public StringId Name;
                public short Material;
                public FlagsValue Flags;
                public float RelativeMassScale;
                public float Friction;
                public float Restitution;
                public float Volume;
                public float Mass;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unknown1;
                public short Phantom;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown2;
                public short Size;
                public short Count;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown3;
                public float Radius;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown4;
                public short Size1;
                public short Count2;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown5;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown6;
                public RealVector3d RotationI;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown7;
                public RealVector3d RotationJ;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown8;
                public RealVector3d RotationK;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown9;
                public RealVector3d Translation;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown10;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Unused = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x50)]
            public class PhysicsModelPill : TagStructure
            {
                public StringId Name;
                public short Material;
                public FlagsValue Flags;
                public float RelativeMassScale;
                public float Friction;
                public float Restitution;
                public float Volume;
                public float Mass;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unknown1;
                public short Phantom;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown2;
                public short Size;
                public short Count;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown3;
                public float Radius;
                public RealVector3d Bottom;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown4;
                public RealVector3d Top;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown5;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Unused = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x30)]
            public class CharacterPhysicsGroundDefinition : TagStructure
            {
                public Angle MaximumSlopeAngle; // degrees
                public Angle DownhillFalloffAngle; // degrees
                public Angle DownhillCutoffAngle; // degrees
                public Angle UphillFalloffAngle; // degrees
                public Angle UphillCutoffAngle; // degrees
                public float DownhillVelocityScale;
                public float UphillVelocityScale;
                [TagField(Flags = Padding, Length = 20)]
                public byte[] Padding1;
            }
            
            [TagStructure(Size = 0x2C)]
            public class CharacterPhysicsFlyingDefinition : TagStructure
            {
                public Angle BankAngle; // degrees
                public float BankApplyTime; // seconds
                public float BankDecayTime; // seconds
                public float PitchRatio; // amount that we pitch up/down when moving up or down
                public float MaxVelocity; // world units per second
                public float MaxSidestepVelocity; // world units per second
                public float Acceleration; // world units per second squared
                public float Deceleration; // world units per second squared
                public Angle AngularVelocityMaximum; // degrees per second
                public Angle AngularAccelerationMaximum; // degrees per second squared
                public float CrouchVelocityModifier; // [0,1]
            }
            
            [TagStructure()]
            public class CharacterPhysicsDeadStructBlock : TagStructure
            {
            }
            
            [TagStructure()]
            public class CharacterPhysicsSentinelStructBlock : TagStructure
            {
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class BipedContactPoint : TagStructure
        {
            public StringId MarkerName;
        }
    }
}

