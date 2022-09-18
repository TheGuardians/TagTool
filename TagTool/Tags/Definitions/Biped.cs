using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Havok;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "biped", Tag = "bipd", Size = 0x1D4, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "biped", Tag = "bipd", Size = 0x21C, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "biped", Tag = "bipd", Size = 0x240, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "biped", Tag = "bipd", Size = 0x2B0, MinVersion = CacheVersion.HaloReach)]
    public class Biped : Unit
    {
        public Angle MovingTurningSpeed;
        public BipedDefinitionFlags BipedFlags;
        public Angle StationaryTurningThreshold;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public float RagdollThresholdVelocity; // if the biped dies while moving faster than this velocity, immediately transition to ragdoll.  Use 0 for 'never' (wu/s)

        [TagField(ValidTags = new[] { "bdpd" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag DeathProgramSelector;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public StringId RagdollRegionName; // when the biped transitions to ragdoll, this region will change to the destroyed state

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public StringId AssassinationChudText;

        public float JumpVelocity; // world units per second

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<UnitTrickDefinitionBlock> Tricks;

        public float MaximumSoftLandingTime; // the longest amount of time the biped can take to recover from a soft landing (seconds)
        public float MinimumHardLandingTime; // the longest amount of time the biped can take to recover from a hard landing (seconds)
        public float MinimumSoftLandingVelocity; // below this velocity the biped does not react when landing (world units per second)
        public float MinimumHardLandingVelocity; // below this velocity the biped will not do a soft landing when returning to the ground (world units per second)
        public float MaximumHardLandingVelocity; // the velocity corresponding to the maximum landing time (world units per second)

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float DeathHardLandingVelocity; // the maximum velocity with which a character can strike the ground and live (world units per second)

        public float StunDuration;
        public float StandingCameraHeight; // (world units, default 0) Bipeds are stuned when damaged by vehicle collisions, also some are when they take emp damage

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float RunningCameraHeight; // world units

        public float CrouchingCameraHeight; // world units

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float CrouchWalkingCameraHeight; // world units

        public float CrouchTransitionTime; // seconds

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public TagFunction CrouchingCameraFunction = new TagFunction
        {
            // TODO: make high-level constructors...
            Data = new byte[]
            {
                0x08, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xE0, 0x3F,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x1C, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
                0x07, 0x00, 0x00, 0xCD, 0xFF, 0xFF, 0x7F, 0x7F, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x80, 0xC0, 0x00, 0x00, 0x80, 0x40, 0x00, 0x00, 0x00, 0x00
            }
        };

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public List<CameraHeightBlock> CameraHeights;

        public Angle CameraInterpolationStart; // looking-downward angle that starts camera interpolation to fp position
        public Angle CameraInterpolationEnd; // looking-downward angle at which camera interpolation to fp position is complete
        public float CameraForwardMovementScale;
        public float CameraSideMovementScale;
        public float CameraVerticalMovementScale;
        public float CameraExclusionDistance;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float RootOffsetCameraDampening;
        public float AutoaimWidth; // world units

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public BipedLockOnFlags LockonFlags; //bitfield32
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float LockonDistance;

        public short PhysicsControlNodeIndex;

        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;

        public float CosineStationaryTurningThreshold;
        public float CrouchTransitionVelocity;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float CameraHeightVelocity;

        public short PelvisNodeIndex;
        public short HeadNodeIndex;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint FpCrouchMovingAnimationSpeedMultiplier;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<BipedWallProximityBlock> WallProximityFeelers;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float HeadshotAccelerationScale; // when the biped ragdolls from a headshot it acceleartes based on this value.  0 defaults to the standard acceleration scale

        public CachedTag AreaDamageEffect;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public CachedTag HealthStationRechargeEffect;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<MovementGateBlock> MovementGates;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<MovementGateBlock> MovementGatesCrouching;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float JumpAimOffsetDistance;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float JumpAimOffsetDuration;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float LandAimOffsetDistance;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float LandAimOffsetDuration;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float AimCompensateMinimumDistance;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float AimCompensateMaximumDistance;

        public BipedPhysicsFlags PhysicsFlags;
        public float HeightStanding;
        public float HeightCrouching;
        public float Radius;
        public float Mass;
        [TagField(Flags = TagFieldFlags.GlobalMaterial)]
        public StringId LivingMaterialName; // collision material used when character is alive
        [TagField(Flags = TagFieldFlags.GlobalMaterial)]
        public StringId DeadMaterialName; // collision material used when character is dead
        [TagField(Flags = TagFieldFlags.GlobalMaterial)]
        public short LivingGlobalMaterialIndex;
        [TagField(Flags = TagFieldFlags.GlobalMaterial)]
        public short DeadGlobalMaterialIndex;
        public List<PhysicsModel.Sphere> DeadSphereShapes;
        public List<PhysicsModel.Pill> PillShapes;
        public List<PhysicsModel.Sphere> SphereShapes;
        public CharacterPhysicsGroundStruct BipedGroundPhysics;
        public CharacterPhysicsFlyingStruct BipedFlyingPhysics;

        public List<ContactPoint> ContactPoints; // these are the points where the biped touches the ground
        public CachedTag ReanimationCharacter; // when the flood reanimate this guy, he turns into a ...
        public CachedTag ReanimationMorphMuffins; // the kind of muffins I create to cover my horrible transformation
        public CachedTag DeathSpawnCharacter; // when I die, out of the ashes of my death crawls a ...
        public short DeathSpawnCount;

        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;

        public BipedLeapingDataStruct BipedLeapingData;
        public BipedGroundFitting BipedGroundFittingData;


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

        [TagStructure(Size = 0x40, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x44, MinVersion = CacheVersion.HaloReach)]
        public class CharacterPhysicsGroundStruct : TagStructure
        {
            public Angle MaximumSlopeAngle;
            public Angle DownhillFalloffAngle;
            public Angle DownhillCutoffAngle;
            public Angle UphillFalloffAngle;
            public Angle UphillCutoffAngle;
            public float DownhillVelocityScale;
            public float UphillVelocityScale;
            public float RuntimeMinimumNormalK;
            public float RuntimeDownhillK0;
            public float RuntimeDownhillK1;
            public float RuntimeUphillK0;
            public float RuntimeUphillK1;
            public Angle ClimbInflectionAngle; // angle for bipeds at which climb direction changes between up and down
            public float ScaleAirborneReactionTime; // scale on the time for the entity to realize it is airborne
            public float ScaleGroundAdhesionVelocity; // scale on velocity with which the entity is pushed back into its ground plane
            public float GravityScale; // scale on gravity for this entity
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float AirborneAccelerationScale; // scale on airborne acceleration maximum
        }

        //public float AirborneAccelerationScale;

        [TagStructure(Size = 0x30)]
        public class CharacterPhysicsFlyingStruct : TagStructure
        {
            public Angle BankAngle; // angle at which we bank left/right when sidestepping or turning while moving forwards
            public float BankApplyTime; // time it takes us to apply a bank
            public float BankDecayTime; // time it takes us to recover from a bank
            public float PitchRatio; // amount that we pitch up/down when moving up or down
            public float MaximumVelocity; // max velocity when not crouching (world units per second)
            public float MaximumSidestepVelocity; // max sideways or up/down velocity when not crouching (world units per second)
            public float Acceleration; //world units per second squared
            public float Deceleration; //world units per second squared
            public Angle AngularVelocityMaximum;
            public Angle AngularAccelerationMaximum; // turn rate
            public float CrouchVelocityModifier; // [0,1]
            public FlyingPhysicsFlags Flags;

            [Flags]
            public enum FlyingPhysicsFlags : uint
            {
                UseWorldUp = 1 << 0
            }
        }



        [TagStructure(Size = 0x30)]
        public class BipedLeapingDataStruct : TagStructure
        {
            [Flags]
            public enum BipedLeapFlags : uint
            {
                None,
                ForceEarlyRoll = 1 << 0
            }

            public BipedLeapFlags LeapFlags;
            public float DampeningScale; // [0,1] 1= very slow changes
            public float RollDelay; // [0,1] 1= roll fast and late
            public float CannonballOffaxisScale; // [0,1] weight
            public float CannonballOfftrackScale; // [0,1] weight
            public Bounds<Angle> CannonballRollBounds; // degrees per second
            public Bounds<float> AnticipationRatioBounds; // current velocity/leap velocity
            public Bounds<float> ReactionForceBounds; // units per second
            public float LobbingDesire; // 1= heavy arc, 0= no arc
        }

        [TagStructure(Size = 0x30, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x78, MinVersion = CacheVersion.HaloReach)]
        public class BipedGroundFitting : TagStructure
        {
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public BipedGroundFittingFlags GroundFittingFlags;

            public float GroundNormalDampening; // react to slope changes (0=slow, 1= fast)
            public float RootOffsetMaxScale; // vertical drop to ground allowed (0=none, 1=full)
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float RootOffsetMaxScaleMoving; // vertical drop to ground allowed (0=none, 1=full)
            public float RootOffsetDampening; // react to root changes (0=slow, 1= fast)
            public float FollowingCamScale; // root offset effect on following cam (0=none, 1=full)
            public float RootLeaningScale; // lean into slopes (0=none, 1=full)
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float StanceWidthScale; // scale pill width to use as stance radius
            public Angle FootRollMax; // orient to ground slope (degrees)
            public Angle FootPitchMax; // orient to ground slope (degrees)

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float FootNormalDampening; // (0=slow, 1= fast)
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float FootFittingTestDistance;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float FootDisplacementUpwardDampening; // (0=slow, 1= fast)
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float FootDisplacementDownwardDampening; // (0=slow, 1= fast)
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float FootPullThresholdDistanceIdle; // amount past the authored plane the foot can be pulled (wu)
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float FootPullThresholdDistanceMoving; // amount past the authored plane the foot can be pulled (wu)
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float FootTurnMinimumRadius; // minimum radius at which foot is fit to turn radius
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float FootTurnMaximumRadius; // maximum radius at which foot is fit to turn radius
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float FootTurnThresholdRadius; // foot is fit to turn radius fully at minimum plus threshold and above
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float FootTurnRateDampening; // (0=slow, 1=fast)
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float FootTurnWeightDampening; // dampening of fitting value for fit to turn radius(0=none, 1=fast)
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float FootTurnBlendOnTime; // time to blend on the foot turn effect (seconds)
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float FootTurnBlendOffTime; // time to blend off the foot turn effect 
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float FreeLegStretchDistance;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public Angle FreeFootRelaxAngle; // (degrees)

            public float PivotonfootScale; // (0=none, 1= full)
            public float PivotMinFootDelta; // vert world units to find lowest foot
            public float PivotStrideLengthScale; // leg length * this = stride length
            public float PivotThrottleScale; // pivoting slows throttle (0=none, 1= full)
            public float PivotOffsetDampening; // react to pivot changes (0=slow, 1= fast)

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

        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown4;

        [Flags]
        public enum BipedDefinitionFlags : int
        {
            None,
            TurnsWithoutAnimating = 1 << 0,
            PassesThroughOtherBipeds = 1 << 1,
            ImmuneToFallingDamage = 1 << 2,
            RotateWhileAirborne = 1 << 3,
            UseLimpBodyPhysics = 1 << 4,
            Unused = 1 << 5,
            RandomSpeedIncrease = 1 << 6,
            Unused_ = 1 << 7,
            SpawnDeathChildrenOnDestroy = 1 << 8,
            StunnedByEmpDamage = 1 << 9,
            DeadPhysicsWhenStunned = 1 << 10,
            AlwaysRagdollWhenDead = 1 << 11,
            SnapsTurns = 1 << 12,
            SyncActionAlwaysProjectsOnGround = 1 << 13,
            OrientFacingToMovement = 1 << 14,
            AimDrivenOrientationInStationaryTurns = 1 << 15,
            UsePredictiveStationaryTurns = 1 << 16,
            NeverRagdoll = 1 << 17,
            ConservativelyRagdollAsFallback = 1 << 18
        }

        [TagStructure(Size = 0xC)]
        public class CameraHeightBlock : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Class;
            public float StandingHeightFraction;
            public float CrouchingHeightFraction;
        }

        [Flags]
        public enum BipedLockOnFlags : int
        {
            None,
            LockedByHumanTargeting = 1 << 0,
            LockedByPlasmaTargeting = 1 << 1,
            AlwaysLockedByHumanTargeting = 1 << 2
        }

        [TagStructure(Size = 0x24, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloReach)]
        public class MovementGateBlock : TagStructure
		{
            public float Period;
            public float ZOffset;
            public float ConstantZOffset;
            public float YOffset;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float SpeedThreshold; // world units per second
            public TagFunction DefaultFunction = new TagFunction { Data = new byte[0] };
        }

        [TagStructure(Size = 0x4)]
        public class ContactPoint : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId MarkerName;
        }
    }
}