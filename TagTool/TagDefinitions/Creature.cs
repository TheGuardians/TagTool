using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
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
        public CachedTagInstance ImpactDamage;
        public CachedTagInstance ImpactShieldDamage;
        public List<Unit.MetagameProperty> MetagameProperties;
        public Bounds<float> DestroyAfterDeathTimeBounds;
    }

    [Flags]
    public enum CreatureFlags : int
    {
        None = 0,
        ImmuneToFallingDamage = 1 << 0,
        RotateWhileAirborne = 1 << 1,
        ZappedByShields = 1 << 2,
        AttachUponImpact = 1 << 3,
        NotOnMotionSensor = 1 << 4,
        ForceGroundMovement = 1 << 5
    }
}
