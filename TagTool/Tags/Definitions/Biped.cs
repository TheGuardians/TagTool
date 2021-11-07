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
    [TagStructure(Name = "biped", Tag = "bipd", Size = 0x240, MinVersion = CacheVersion.HaloOnlineED)]
    public class Biped : Unit
    {
        public Angle MovingTurningSpeed;
        public BipedDefinitionFlags BipedFlags;
        public Angle StationaryTurningThreshold;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public StringId RagdollRegionName;

        public float JumpVelocity;
        public float MaximumSoftLandingTime;
        public float MinimumHardLandingTime;
        public float MinimumSoftLandingVelocity;
        public float MinimumHardLandingVelocity;
        public float MaximumHardLandingVelocity;
        public float DeathHardLandingVelocity;
        public float StunDuration;
        public float StationaryStandingCameraHeight;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float MovingStandingCameraHeight;

        public float StationaryCrouchingCameraHeight;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float MovingCrouchingCameraHeight;

        public float CrouchTransitionTime;

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
        public RealVector3d CameraOffset;
        public float RootOffsetCameraScale;
        public float AutoaimWidth; // world units
        public BipedLockOnFlags LockonFlags; //bitfield32
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

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown2;

        public float HeadshotAccelerationScale; // when the biped ragdolls from a headshot it acceleartes based on this value.  0 defaults to the standard acceleration scale
        public CachedTag AreaDamageEffect;

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
        public StringId LivingMaterialName; // collision material used when character is alive
        public StringId DeadMaterialName; // collision material used when character is dead
        public short LivingGlobalMaterialIndex;
        public short DeadGlobalMaterialIndex;
        public List<PhysicsModel.Sphere> DeadSphereShapes;
        public List<PhysicsModel.Pill> PillShapes;
        public List<PhysicsModel.Sphere> SphereShapes;
        public CharacterPhysicsGroundStruct BipedGroundPhysics;
        public CharacterPhysicsFlyingStruct BipedFlyingPhysics;

        [TagStructure(Size = 0x40)]
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

        public List<ContactPoint> ContactPoints; // these are the points where the biped touches the ground
        public CachedTag ReanimationCharacter; // when the flood reanimate this guy, he turns into a ...
        public CachedTag ReanimationMorphMuffins; // the kind of muffins I create to cover my horrible transformation
        public CachedTag DeathSpawnCharacter; // when I die, out of the ashes of my death crawls a ...
        public short DeathSpawnCount;

        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;

        public BipedLeapingDataStruct BipedLeapingData;
        public BipedGroundFitting BipedGroundFittingData;

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

        [TagStructure(Size = 0x30)]
        public class BipedGroundFitting : TagStructure
        {
            public float GroundNormalDampening; // react to slope changes (0=slow, 1= fast)
            public float RootOffsetMaxScale; // vertical drop to ground allowed (0=none, 1=full)
            public float RootOffsetDampening; // react to root changes (0=slow, 1= fast)
            public float FollowingCamScale; // root offset effect on following cam (0=none, 1=full)
            public float RootLeaningScale; // lean into slopes (0=none, 1=full)
            public Angle FootRollMax; // orient to ground slope (degrees)
            public Angle FootPitchMax; // orient to ground slope (degrees)
            public float PivotonfootScale; // (0=none, 1= full)
            public float PivotMinFootDelta; // vert world units to find lowest foot
            public float PivotStrideLengthScale; // leg length * this = stride length
            public float PivotThrottleScale; // pivoting slows throttle (0=none, 1= full)
            public float PivotOffsetDampening; // react to pivot changes (0=slow, 1= fast)
        }

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
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

        [TagStructure(Size = 0x24)]
        public class MovementGateBlock : TagStructure
		{
            public float Period;
            public float ZOffset;
            public float ConstantZOffset;
            public float YOffset;
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