using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "biped", Tag = "bipd", Size = 0x3E0)]
    public class Biped : Unit
    {
        public Angle MovingTurningSpeed; // degrees per second
        public BipedDefinitionFlags BipedFlags;
        public Angle StationaryTurningThreshold;
        [TagField(ValidTags = new [] { "bdpd" })]
        public CachedTag DeathProgramSelector;
        // when the biped transitions to ragdoll, this region will change to the destroyed state
        public StringId RagdollRegionName;
        // The string id for the assassination action text in the CHUD
        public StringId AssassinationChudText;
        public float JumpVelocity; // world units per second
        public List<UnitTrickDefinitionBlock> Tricks;
        // the longest amount of time the biped can take to recover from a soft landing
        public float MaximumSoftLandingTime; // seconds
        // the longest amount of time the biped can take to recover from a hard landing
        public float MaximumHardLandingTime; // seconds
        // below this velocity the biped does not react when landing
        public float MinimumSoftLandingVelocity; // world units per second
        // below this velocity the biped will not do a soft landing when returning to the ground
        public float MinimumHardLandingVelocity; // world units per second
        // the velocity corresponding to the maximum landing time
        public float MaximumHardLandingVelocity; // world units per second
        // 0 is the default.  Bipeds are stunned when damaged by vehicle collisions, also some are when they take emp damage
        public float StunDuration;
        public float StandingCameraHeight; // world units
        public float RunningCameraHeight; // world units
        public float CrouchingCameraHeight; // world units
        public float CrouchWalkingCameraHeight; // world units
        public float CrouchTransitionTime; // seconds
        public MappingFunction CameraHeightVelocityFunction;
        public List<BipedCameraHeightBlock> CameraHeights;
        // looking-downward angle that starts camera interpolation to fp position
        public Angle CameraInterpolationStart; // degrees
        // looking-downward angle at which camera interpolation to fp position is complete
        public Angle CameraInterpolationEnd; // degrees
        // amount of fp camera movement in (forward, right, down) when pitched down by 'camera interpolation end' above
        public RealVector3d CameraOffset; // wu
        public float RootOffsetCameraScale;
        public float RootOffsetCameraDampening;
        public float AutoaimWidth; // world units
        public short RuntimePhysicsControlNodeIndex;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public float RuntimeCosineStationaryTurningThreshold;
        public float RuntimeCrouchTransitionVelocity;
        public float RuntimeCameraHeightVelocity;
        public short RuntimePelvisNodeIndex;
        public short RuntimeHeadNodeIndex;
        public List<BipedWallProximityBlock> WallProximityFeelers;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag AreaDamageEffect;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag HealthStationRechargeEffect;
        public List<BipedMovementGateBlock> MovementGates;
        public List<BipedMovementGateBlock> MovementGatesCrouching;
        public float JumpAimOffsetDistance; // world units
        public float JumpAimOffsetDuration; // seconds
        public float LandAimOffsetDistance; // world units
        public float LandAimOffsetDuration; // seconds
        public float AimCompensateMinimumDistance; // world units
        public float AimCompensateMaximumDistance; // world units
        public CharacterPhysicsStruct Physics;
        // these are the points where the biped touches the ground
        public List<ContactPointBlock> ContactPoints;
        // when the flood reanimate this guy, he turns into a ...
        public CachedTag ReanimationCharacter;
        // the kind of muffins I create to cover my horrible transformation
        [TagField(ValidTags = new [] { "mffn" })]
        public CachedTag ReanimationMorphMuffins;
        // when I die, out of the ashes of my death crawls a ...
        public CachedTag DeathSpawnCharacter;
        public short DeathSpawnCount;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        public BipedLeapingDataStruct LeapingData;
        public BipedVaultingDataStruct VaultingData;
        public BipedGrabBipedDataStruct GrabBipedData;
        public BipedGrabObjectDataStruct GrabObjectData;
        public BipedGroundFittingDataStruct GroundFittingData;
        // optional particleization effect definition, if you want this to particleize when it dies
        [TagField(ValidTags = new [] { "pman" })]
        public CachedTag DeathParticleize;
        public float MovementSpeedScale;
        public BipedMovementHipLeaningStruct MovementHipLean;
        // Intended for biped vehicles (ie mantis)
        [TagField(ValidTags = new [] { "sbnk" })]
        public CachedTag PlayerBipedSoundBank;
        // plays when player is inside a RegenField
        [TagField(ValidTags = new [] { "lsnd" })]
        public CachedTag RegenFieldLoopingSound;
        // plays when player starts crouching
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CrouchDownSound;
        // plays when player stands up
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CrouchUpSound;
        // overrides shield impact sound, like when in a Mantis
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag ShieldImpactOverride;
        // overrides regular impact sound, like when in a Mantis
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag NonShieldImpactOverride;
        // increases radius of other sounds when piloting this biped (mech)
        public float SoundRadiusMultiplier;
        public List<BipedSoundRtpcblock> SoundRtpcs;
        public List<BipedSoundSweetenerBlock> SoundSweeteners;
        public List<BipedAimingJointFixupBlock> AimingFixup;
        [TagField(ValidTags = new [] { "sict" })]
        public CachedTag SelfIllumination;
        
        [Flags]
        public enum BipedDefinitionFlags : uint
        {
            TurnsWithoutAnimating = 1 << 0,
            HasPhysicalRigidBodiesWhenAlive = 1 << 1,
            ImmuneToFallingDamage = 1 << 2,
            HasAnimatedJetpack = 1 << 3,
            Unused1 = 1 << 4,
            Unused2 = 1 << 5,
            RandomSpeedIncrease = 1 << 6,
            Unused3 = 1 << 7,
            SpawnDeathChildrenOnDestroy = 1 << 8,
            StunnedByEmpDamage = 1 << 9,
            DeadPhysicsWhenStunned = 1 << 10,
            AlwaysRagdollWhenDead = 1 << 11,
            SnapsTurns = 1 << 12,
            SyncActionAlwaysProjectsOnGround = 1 << 13,
            OrientFacingToMovement = 1 << 14
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
        
        [TagStructure(Size = 0x14)]
        public class MappingFunction : TagStructure
        {
            public byte[] Data;
        }
        
        [TagStructure(Size = 0xC)]
        public class BipedCameraHeightBlock : TagStructure
        {
            public StringId WeaponClass;
            public float StandingHeight; // wu
            public float CrouchingHeight; // wu
        }
        
        [TagStructure(Size = 0x18)]
        public class BipedWallProximityBlock : TagStructure
        {
            public StringId MarkerName;
            public float SearchDistance; // wu
            public float CompressionTime; // s
            public float ExtensionTime; // s
            // if multiple markers share the same name, this specifies how to compose their search values
            public BipedWallProximityCompositionMode CompositionMode;
            // creates an object function with this name that you can use to drive an overlay animation
            public StringId OutputFunctionName;
            
            public enum BipedWallProximityCompositionMode : int
            {
                // pick the marker that has the closest obstacle
                MostCompressed,
                // pick the marker that has the furthest obstacle
                LeastCompressed,
                // average the distances from each marker
                Average
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class BipedMovementGateBlock : TagStructure
        {
            public float Period; // seconds
            public float ZOffset; // world units
            // camera z is modified by this constant z value
            public float ConstantZOffset; // world units
            public float YOffset; // world units
            public float SpeedThreshold; // world units per second
            public MappingFunction DefaultFunction;
        }
        
        [TagStructure(Size = 0xDC)]
        public class CharacterPhysicsStruct : TagStructure
        {
            public CharacterPhysicsFlags Flags;
            public float HeightStanding;
            public float HeightCrouching;
            public float Radius;
            public float Mass;
            // collision material used when character is alive
            public StringId LivingMaterialName;
            // collision material used when character is dead
            public StringId DeadMaterialName;
            public short RuntimeGlobalMaterialType;
            public short RuntimeDeadGlobalMaterialType;
            // don't be an asshole, edit something else!
            public List<SpheresBlockStruct> DeadSphereShapes;
            // don't be an asshole, edit something else!
            public List<PillsBlockStruct> PillShapes;
            // don't be an asshole, edit something else!
            public List<SpheresBlockStruct> SphereShapes;
            // don't be an asshole, edit something else!
            public List<SpheresBlockStruct> ListSphereShapes;
            // don't be an asshole, edit something else!
            public List<ListsBlock> ListShapes;
            // don't be an asshole, edit something else!
            public List<ListShapesBlockStruct> ListShapeChildinfos;
            public CharacterPhysicsGroundStruct GroundPhysics;
            public CharacterPhysicsFlyingStruct FlyingPhysics;
            
            [Flags]
            public enum CharacterPhysicsFlags : uint
            {
                CenteredAtOrigin = 1 << 0,
                ShapeSpherical = 1 << 1,
                UsePlayerPhysics = 1 << 2,
                ClimbAnySurface = 1 << 3,
                Flying = 1 << 4,
                NotPhysical = 1 << 5,
                DeadCharacterCollisionGroup = 1 << 6,
                SuppressGroundPlanesOnBipeds = 1 << 7,
                PhysicalRagdoll = 1 << 8,
                DoNotResizeDeadSpheres = 1 << 9,
                MultipleMantisShapes = 1 << 10,
                IAmAnExtremeSlipsurface = 1 << 11,
                SlipsOffMovers = 1 << 12
            }
            
            [TagStructure(Size = 0x70)]
            public class SpheresBlockStruct : TagStructure
            {
                public HavokPrimitiveStruct Base;
                public HavokConvexShapeStruct SphereShape;
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public HavokConvexTranslateShapeStruct TranslateShape;
                
                [TagStructure(Size = 0x20)]
                public class HavokPrimitiveStruct : TagStructure
                {
                    public StringId Name;
                    public sbyte Material;
                    public PhysicsMaterialFlags MaterialFlags;
                    public short RuntimeMaterialType;
                    public float RelativeMassScale;
                    public float Friction;
                    public float Restitution;
                    public float Volume;
                    public float Mass;
                    public short MassDistributionIndex;
                    public sbyte Phantom;
                    public PhysicsMaterialProxyCollisionGroups ProxyCollisionGroup;
                    
                    [Flags]
                    public enum PhysicsMaterialFlags : byte
                    {
                        SupressesEffects = 1 << 0,
                        // enables collision with the player regardless of the collision group
                        ForceEnableCollisionWithPlayer = 1 << 1
                    }
                    
                    public enum PhysicsMaterialProxyCollisionGroups : sbyte
                    {
                        None,
                        SmallCrate,
                        Crate,
                        HugeCrate,
                        Item,
                        Projectile,
                        Biped,
                        Machine,
                        EarlyMoverMachine,
                        OnlyCollideWithEnvironment,
                        TechArtCustom,
                        SmallExpensivePlant,
                        IgnoreEnvironment,
                        HugeVehicle,
                        Ragdoll,
                        SuperCollidableRagdoll,
                        ItemBlocker,
                        User00,
                        User01,
                        Everything,
                        Creatures
                    }
                }
                
                [TagStructure(Size = 0x14)]
                public class HavokConvexShapeStruct : TagStructure
                {
                    public HavokShapeStruct Base;
                    public float Radius;
                    
                    [TagStructure(Size = 0x10)]
                    public class HavokShapeStruct : TagStructure
                    {
                        public int FieldPointerSkip;
                        public short Size;
                        public short Count;
                        public sbyte Type;
                        public sbyte Dispatchtype;
                        public sbyte BitsperKey;
                        public sbyte Codectype;
                        public int UserData;
                    }
                }
                
                [TagStructure(Size = 0x30)]
                public class HavokConvexTranslateShapeStruct : TagStructure
                {
                    public HavokConvexShapeStruct Convex;
                    public int FieldPointerSkip;
                    public HavokShapeReferenceStruct HavokShapeReferenceStruct1;
                    public int ChildShapeSize;
                    public RealVector3d Translation;
                    public float HavokWTranslation;
                    
                    [TagStructure(Size = 0x4)]
                    public class HavokShapeReferenceStruct : TagStructure
                    {
                        public ShapeEnum ShapeType;
                        public short Shape;
                        
                        public enum ShapeEnum : short
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
                }
            }
            
            [TagStructure(Size = 0x60)]
            public class PillsBlockStruct : TagStructure
            {
                public HavokPrimitiveStruct Base;
                public HavokConvexShapeStruct CapsuleShape;
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public RealVector3d Bottom;
                public float HavokWBottom;
                public RealVector3d Top;
                public float HavokWTop;
                
                [TagStructure(Size = 0x20)]
                public class HavokPrimitiveStruct : TagStructure
                {
                    public StringId Name;
                    public sbyte Material;
                    public PhysicsMaterialFlags MaterialFlags;
                    public short RuntimeMaterialType;
                    public float RelativeMassScale;
                    public float Friction;
                    public float Restitution;
                    public float Volume;
                    public float Mass;
                    public short MassDistributionIndex;
                    public sbyte Phantom;
                    public PhysicsMaterialProxyCollisionGroups ProxyCollisionGroup;
                    
                    [Flags]
                    public enum PhysicsMaterialFlags : byte
                    {
                        SupressesEffects = 1 << 0,
                        // enables collision with the player regardless of the collision group
                        ForceEnableCollisionWithPlayer = 1 << 1
                    }
                    
                    public enum PhysicsMaterialProxyCollisionGroups : sbyte
                    {
                        None,
                        SmallCrate,
                        Crate,
                        HugeCrate,
                        Item,
                        Projectile,
                        Biped,
                        Machine,
                        EarlyMoverMachine,
                        OnlyCollideWithEnvironment,
                        TechArtCustom,
                        SmallExpensivePlant,
                        IgnoreEnvironment,
                        HugeVehicle,
                        Ragdoll,
                        SuperCollidableRagdoll,
                        ItemBlocker,
                        User00,
                        User01,
                        Everything,
                        Creatures
                    }
                }
                
                [TagStructure(Size = 0x14)]
                public class HavokConvexShapeStruct : TagStructure
                {
                    public HavokShapeStruct Base;
                    public float Radius;
                    
                    [TagStructure(Size = 0x10)]
                    public class HavokShapeStruct : TagStructure
                    {
                        public int FieldPointerSkip;
                        public short Size;
                        public short Count;
                        public sbyte Type;
                        public sbyte Dispatchtype;
                        public sbyte BitsperKey;
                        public sbyte Codectype;
                        public int UserData;
                    }
                }
            }
            
            [TagStructure(Size = 0x70)]
            public class ListsBlock : TagStructure
            {
                public HavokShapeCollectionStruct20102 Base;
                public int FieldPointerSkip;
                public int ChildShapesSize;
                public int ChildShapesCapacity;
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public RealVector3d AabbHalfExtents;
                public float HavokWAabbHalfExtents;
                public RealVector3d AabbCenter;
                public float HavokWAabbCenter;
                public int EnabledChildren0;
                public int EnabledChildren1;
                public int EnabledChildren2;
                public int EnabledChildren3;
                public int EnabledChildren4;
                public int EnabledChildren5;
                public int EnabledChildren6;
                public int EnabledChildren7;
                
                [TagStructure(Size = 0x18)]
                public class HavokShapeCollectionStruct20102 : TagStructure
                {
                    public HavokShapeStruct20102 Base;
                    public int FieldPointerSkip;
                    public sbyte DisableWelding;
                    public sbyte CollectionType;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [TagStructure(Size = 0x10)]
                    public class HavokShapeStruct20102 : TagStructure
                    {
                        public int FieldPointerSkip;
                        public short Size;
                        public short Count;
                        public int UserData;
                        public int Type;
                    }
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class ListShapesBlockStruct : TagStructure
            {
                public HavokShapeReferenceStruct ShapeReference;
                public int CollisionFilter;
                public int ShapeSize;
                public int NumChildShapes;
                
                [TagStructure(Size = 0x4)]
                public class HavokShapeReferenceStruct : TagStructure
                {
                    public ShapeEnum ShapeType;
                    public short Shape;
                    
                    public enum ShapeEnum : short
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
            }
            
            [TagStructure(Size = 0x44)]
            public class CharacterPhysicsGroundStruct : TagStructure
            {
                public Angle MaximumSlopeAngle; // degrees
                public Angle DownhillFalloffAngle; // degrees
                public Angle DownhillCutoffAngle; // degrees
                public Angle UphillFalloffAngle; // degrees
                public Angle UphillCutoffAngle; // degrees
                public float DownhillVelocityScale;
                public float UphillVelocityScale;
                public float RuntimeMinimumNormalK;
                public float RuntimeDownhillK0;
                public float RuntimeDownhillK1;
                public float RuntimeUphillK0;
                public float RuntimeUphillK1;
                // angle for bipeds at which climb direction changes between up and down
                public Angle ClimbInflectionAngle;
                // scale on the time for the entity to realize it is airborne
                public float ScaleAirborneReactionTime;
                // scale on velocity with which the entity is pushed back into its ground plane (set to -1 to lock to ground)
                public float ScaleGroundAdhesionVelocity;
                // scale on gravity for this entity
                public float GravityScale;
                // scale on airborne acceleration maximum
                public float AirborneAccelerationScale;
            }
            
            [TagStructure(Size = 0x30)]
            public class CharacterPhysicsFlyingStruct : TagStructure
            {
                // angle at which we bank left/right when sidestepping or turning while moving forwards
                public Angle BankAngle; // degrees
                // time it takes us to apply a bank
                public float BankApplyTime; // seconds
                // time it takes us to recover from a bank
                public float BankDecayTime; // seconds
                // amount that we pitch up/down when moving up or down
                public float PitchRatio;
                // max velocity when not crouching
                public float MaxVelocity; // world units per second
                // max sideways or up/down velocity when not crouching
                public float MaxSidestepVelocity; // world units per second
                public float Acceleration; // world units per second squared
                public float Deceleration; // world units per second squared
                // turn rate
                public Angle AngularVelocityMaximum; // degrees per second
                // turn acceleration rate
                public Angle AngularAccelerationMaximum; // degrees per second squared
                // how much slower we fly if crouching (zero = same speed)
                public float CrouchVelocityModifier; // [0,1]
                public FlyingPhysicsFlags Flags;
                
                [Flags]
                public enum FlyingPhysicsFlags : uint
                {
                    UseWorldUp = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class ContactPointBlock : TagStructure
        {
            public StringId MarkerName;
        }
        
        [TagStructure(Size = 0x30)]
        public class BipedLeapingDataStruct : TagStructure
        {
            public BipedLeapFlags LeapFlags;
            public float DampeningScale; // [0,1] 1= very slow changes
            public float RollDelay; // [0,1] 1= roll fast and late
            public float CannonballOffAxisScale; // [0,1] weight
            public float CannonballOffTrackScale; // [0,1] weight
            public Bounds<Angle> CannonballRollBounds; // degrees per second
            public Bounds<float> AnticipationRatioBounds; // current velocity/leap velocity
            public Bounds<float> ReactionForceBounds; // units per second
            public float LobbingDesire; // 1= heavy arc, 0= no arc
            
            [Flags]
            public enum BipedLeapFlags : uint
            {
                ForceEarlyRoll = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class BipedVaultingDataStruct : TagStructure
        {
            public Bounds<float> VaultHeightBounds; // wus
            public float VaultMaxHorizontalDistance; // wus
            public float VaultArcAmount; // 1= heavy arc, 0= no arc
            public float VaultMinObjectSize; // wus
            public float SearchWidth; // wus, the side-to-side width of the search path
        }
        
        [TagStructure(Size = 0x8)]
        public class BipedGrabBipedDataStruct : TagStructure
        {
            public StringId GrabBipedAnimationClass;
            public GrabBipedThrowControlModes ThrowBipedControlMode;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum GrabBipedThrowControlModes : sbyte
            {
                CameraFacing,
                ControlStickDirection
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class BipedGrabObjectDataStruct : TagStructure
        {
            public List<BipedGrabObjectAnimationSetBlock> GrabObjectAnimationSets;
            
            [TagStructure(Size = 0xC)]
            public class BipedGrabObjectAnimationSetBlock : TagStructure
            {
                public StringId AnimationSetName;
                // The marker on the biped to which we attach the grabbed object.
                public StringId AttachMarker;
                public float ThrowSpeed;
            }
        }
        
        [TagStructure(Size = 0x98)]
        public class BipedGroundFittingDataStruct : TagStructure
        {
            public BipedGroundFittingFlags GroundFittingFlags;
            public float GroundNormalDampening; // react to slope changes (0=slow, 1= fast)
            public float RootOffsetMaxScaleIdle; // vertical drop to ground allowed (0=none, 1=full)
            public float RootOffsetMaxScaleMoving; // vertical drop to ground allowed (0=none, 1=full)
            public float RootOffsetDampening; // react to root changes (0=slow, 1= fast)
            public float FollowingCamScale; // root offset effect on following cam (0=none, 1=full)
            public float RootLeaningScale; // lean into slopes (0=none, 1=full)
            public float StanceWidthScale; // scale pill width to use as stance radius
            public Angle FootRollMax; // orient to ground slope (degrees)
            public Angle FootPitchMax; // orient to ground slope (degrees)
            public float FootNormalDampening; // (0=slow, 1= fast)
            public float FootFittingTestDistance;
            public float FootDisplacementUpwardDampening; // (0=slow, 1= fast)
            public float FootDisplacementDownwardDampening; // (0=slow, 1= fast)
            public float FootPullThresholdDistanceIdle; // amount past the authored plane the foot can be pulled (wu)
            public float FootPullThresholdDistanceMoving; // amount past the authored plane the foot can be pulled (wu)
            public float FootTurnMinimumRadius; // minimum radius at which foot is fit to turn radius
            public float FootTurnMaximumRadius; // maximum radius at which foot is fit to turn radius
            public float FootTurnThresholdRadius; // foot is fit to turn radius fully at minimum plus threshold and above
            public float FootTurnRateDampening; // (0=slow, 1=fast)
            public float FootTurnWeightDampening; // dampening of fitting value for fit to turn radius(0=none, 1=fast)
            public float FootTurnBlendOnTime; // time to blend on the foot turn effect (seconds)
            public float FootTurnBlendOffTime; // time to blend off the foot turn effect (seconds)
            public float PivotOnFootScale; // (0=none, 1= full)
            public float PivotMinFootDelta; // vert world units to find lowest foot
            public float PivotStrideLengthScale; // leg length * this = stride length
            public float PivotThrottleScale; // pivoting slows throttle (0=none, 1= full)
            public float PivotOffsetDampening; // react to pivot changes (0=slow, 1= fast)
            public float PivotForceTurnRate; // turn no matter what the pivot state is (0=control turn, 1= always turn)
            // ideal ratio of distance from the pelvis to pedestal to place pelvis over the highest foot
            public float IdealPelvisOverHighFootScale; // ratio of pedestal to pelvis distance 
            // ideal ratio of distance from the pelvis to pedestal to place pelvis over the lowest foot
            public float IdealPelvisOverLowFootScale; // ratio of pedestal to pelvis distance 
            public float PushOverMag; // magnitude of throttle to push over ledges. 0= no push
            public float PushBackMag; // magnitude of throttle to push back from ledges. 0= no push
            public float LedgeiksuccessRange; // when unable to IK at least this close, consider the IK failed.
            public float LedgeWarningTime; // secs to warn the player before pushing over a ledge
            // how much this biped respects foot lock events
            public float FootlockScale; // (0-1) 
            // throttle at which foot lock should be fully on (footlockScale)
            public float FootlockMinThrottle; // (0-1) 
            // throttle at which foot lock should be fully off
            public float FootlockMaxThrottle; // (0-1) 
            
            [Flags]
            public enum BipedGroundFittingFlags : uint
            {
                FootFixupEnabled = 1 << 0,
                RootOffsetEnabled = 1 << 1,
                // deprecated
                FreeFootEnabled = 1 << 2, // deprecated
                ZLegEnabled = 1 << 3,
                FootPullPinned = 1 << 4,
                // deprecated
                FootlockAdjustsRoot = 1 << 5, // deprecated
                // slow
                RaycastVehicles = 1 << 6,
                // deprecated
                FootFixupOnComposites = 1 << 7, // deprecated
                // noramlly, we will force the feet to lock to the ground surface
                AllowFeetBelowGrade = 1 << 8,
                // for characters that climb walls
                UseBipedUpDirection = 1 << 9,
                // prevents ground marker from going below the contact point
                SnapMarkerAboveContact = 1 << 10
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class BipedMovementHipLeaningStruct : TagStructure
        {
            public float PredictionSeconds; // predict ahead to determine lean. 0= off, more time=more lean
            public float MaxLeanAngle; // (degrees) maximum lean amount
            public float MaxVerticalDip; // (fraction of leg length)
            public float MaxLeanAngleSine; // set on post-process, don't edit!*
            public float MaxLeanAngleCosine; // set on post-process, don't edit!*
        }
        
        [TagStructure(Size = 0xC)]
        public class BipedSoundRtpcblock : TagStructure
        {
            // Sound attachment to affect - leave empty for main body
            public int AttachmentIndex;
            // Function to drive the RTPC
            public StringId Function;
            // WWise RTPC string name
            public uint RtpcName;
        }
        
        [TagStructure(Size = 0x1C)]
        public class BipedSoundSweetenerBlock : TagStructure
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
        
        [TagStructure(Size = 0x20)]
        public class BipedAimingJointFixupBlock : TagStructure
        {
            public StringId RotationNode; // bone to rotate to align marker
            public StringId MarkerName;
            public Bounds<Angle> YawBounds; // degrees
            public Bounds<Angle> PitchBounds; // degrees
            public float MaxYawVelocity; // degrees per second
            public float MaxPitchVelocity; // degrees per second
        }
    }
}
