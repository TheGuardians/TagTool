using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Havok;

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
        public uint Unknown20;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public StringId Unknown21;
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
        public TagFunction CrouchingCameraFunction = new TagFunction { Data = new byte[0] };

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
        public short Unknown29;

        public uint Unknown30;
        public uint Unknown31;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown32;

        public short PelvisNodeIndex;
        public short HeadNodeIndex;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown33;

        public float HeadshotAccelerationScale;
        public CachedTagInstance AreaDamageEffect;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<MovementGateBlock> MovementGates;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<MovementGateBlock> MovementGatesCrouching;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown36;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown37;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown38;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown39;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown40;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown41;

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
        public float Unknown42;
        public float Unknown43;
        public float Unknown44;
        public float Unknown45;
        public float Unknown46;
        public float Unknown47;
        public float Unknown48;
        public float Unknown49;
        public float Unknown50;
        public float Unknown51;
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
        public CachedTagInstance ReanimationCharacter;
        public CachedTagInstance TransformationMuffin;
        public CachedTagInstance DeathSpawnCharacter;
        public short DeathSpawnCount;
        public short Unknown52;

        public uint Unknown53;

        public float Unknown54;
        public float Unknown55;
        public float Unknown56;
        public float Unknown57;

        public Angle Unknown58;
        public Angle Unknown59;

        public uint Unknown60;
        public uint Unknown61;

        public float Unknown62;
        public float Unknown63;
        public float Unknown64;
        public float Unknown65;
        public float Unknown66;
        public float Unknown67;
        public float Unknown68;
        public float Unknown69;

        public Angle Unknown70;
        public Angle Unknown71;

        public float Unknown72;
        public float Unknown73;
        public float Unknown74;
        public float Unknown75;
        public float Unknown76;
        
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown77;

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

        [TagStructure(Size = 0x18)]
        public class CameraHeightBlock : TagStructure
		{
            [TagField(Label = true)]
            public StringId Class;
            public float StandingHeightFraction;
            public float CrouchingHeightFraction;
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
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
            [TagField(Label = true)]
            public StringId MarkerName;
        }
    }
}