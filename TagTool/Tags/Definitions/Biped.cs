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
    [TagStructure(Name = "biped", Tag = "bipd", Size = 0x240, MinVersion = CacheVersion.HaloOnline106708)]
    public class Biped : Unit
    {
        public Angle MovingTurningSpeed;
        public BipedFlagBits BipedFlags;
        public Angle StationaryTurningSpeed;
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

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
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

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<CameraHeightBlock> CameraHeights;

        public Angle CameraInterpolationStart;
        public Angle CameraInterpolationEnd;

        public RealVector3d CameraOffset;
        public float RootOffsetCameraScale;

        public float AutoaimWidth;

        public LockOnFlagBits LockonFlags; //bitfield32
        public float LockonDistance;

        public short PhysicsControlNodeIndex;
        public short Unknown1;

        public float CosineStationaryTurningThreshold;
        public float CrouchTransitionVelocity;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float CameraHeightVelocity;

        public short PelvisNodeIndex;
        public short HeadNodeIndex;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown2;

        public float HeadshotAccelerationScale;
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
        public StringId LivingMaterialName;
        public StringId DeadMaterialName;
        public short LivingMaterialGlobalIndex;
        public short DeadMaterialGlobalIndex;
        public List<PhysicsModel.Sphere> DeadSphereShapes;
        public List<PhysicsModel.Pill> PillShapes;
        public List<PhysicsModel.Sphere> SphereShapes;
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
        public Angle ClimbInflectionAngle;
        public float ScaleAirborneReactionTime;
        public float ScaleGroundAdhesionVelocity;
        public float GravityScale;
        public float AirborneAccelerationScale;
        public Angle BankAngle;
        public float BankApplyTime;
        public float BankDecayTime;
        public float PitchRatio;
        public float MaximumVelocity;
        public float MaximumSidestepVelocity;
        public float Acceleration;
        public float Deceleration;
        public Angle AngularVelocityMaximum;
        public Angle AngularAccelerationMaximum;
        public float CrouchVelocityModifier;
        
        public List<ContactPoint> ContactPoints;
        public CachedTag ReanimationCharacter;
        public CachedTag TransformationMuffin;
        public CachedTag DeathSpawnCharacter;
        public short DeathSpawnCount;
        public short Unknown3;

        [Flags]
        public enum BipedLeapFlags : uint
        {
            None,
            ForceEarlyRoll = 1 << 0
        }

        public BipedLeapFlags LeapFlags;
        public float DampeningScale;
        public float RollDelay;
        public float CannonballOffAxisScale;
        public float CannonballOffTrackScale;
        public Bounds<Angle> CannonballRollBounds;
        public Bounds<float> AnticipationRatioBounds;
        public Bounds<float> ReactionForceBounds;
        public float LobbingDesire;

        public float GroundNormalDampening;
        public float RootOffsetMaxScale;
        public float RootOffsetDampening;
        public float FollowingCamScale;
        public float RootLeaningScale;
        public Angle FootRollMax;
        public Angle FootPitchMax;

        public float PivotOnFootScale;
        public float PivotMinFootDelta;
        public float PivotStrideLengthScale;
        public float PivotThrottleScale;
        public float PivotOffsetDampening;
        
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown4;

        [Flags]
        public enum BipedFlagBits : int
        {
            None,
            TurnsWithoutAnimating = 1 << 0,
            PassesThroughOtherBipeds = 1 << 1,
            ImmuneToFallingDamage = 1 << 2,
            RotateWhileAirborne = 1 << 3,
            UseLimpBodyPhysics = 1 << 4,
            Unused1 = 1 << 5,
            RandomSpeedIncrease = 1 << 6,
            Unused2 = 1 << 7,
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
        public enum LockOnFlagBits : int
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