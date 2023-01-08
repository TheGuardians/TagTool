using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "creature", Tag = "crea", Size = 0x100)]
    public class Creature : GameObject
    {
        public CreatureFlags Flags2;

        public Unit.DefaultTeamValue DefaultTeam;
        public Unit.MotionSensorBlipSizeValue MotionSensorBlipSize;

        /// <summary>
        /// The maximum turning velocity of the creature. Ground creatures only.
        /// </summary>
        public Angle TurningVelocityMaximum;
        
        /// <summary>
        /// The maximum turning acceleration of the creature. Ground creatures only.
        /// </summary>
        public Angle TurningAccelerationMaximum;

        public float CasualTurningModifer;
        public float AutoaimWidth;
        public uint Flags3;
        public float HeightStanding;
        public float HeightCrouching;
        public float Radius;
        public float Mass;
        public StringId LivingMaterialName;
        public StringId DeadMaterialName;
        public short LivingGlobalMaterialIndex;
        public short DeadGlobalMaterialIndex;
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
        public uint Unknown6;
        public uint Unknown7;
        public uint Unknown8;
        public uint Unknown9;
        public uint Unknown10;
        public uint Unknown11;
        public uint Unknown12;
        public float FallingVelocityScale;
        public uint Unknown13;
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
        public uint Unknown14;
        public CachedTag ImpactDamage;
        public CachedTag ImpactShieldDamage;
        public List<Unit.CampaignMetagameBucket> MetagameProperties;
        public Bounds<float> DestroyAfterDeathTimeBounds;
    }

    [Flags]
    public enum CreatureFlags : uint
    {
        None = 0,
        Unused = 1 << 0,
        InfectionForm = 1 << 1,
        ImmuneToFallingDamage = 1 << 2,
        RotateWhileAirborne = 1 << 3,
        ZappedByShields = 1 << 4,
        AttachUponImpact = 1 << 5,
        NotOnMotionSensor = 1 << 6
    }

    [Flags]
    public enum CreatureFlagsReach : uint
    {
        // TODO: convert
        None = 0,
        Unused = 1 << 0,
        ImmuneToFallingDamage = 1 << 1,
        RotateWhileAirborne = 1 << 2,
        ZappedByShields = 1 << 3,
        AttachUponImpact = 1 << 4,
        NotOnMotionSensor = 1 << 5,
        ForceGroundMovement = 1 << 6
    }
}
