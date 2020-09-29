using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "physics", Tag = "phys", Size = 0x80)]
    public class Physics : TagStructure
    {
        public float Radius; // positive uses old inferior physics, negative uses new improved physics
        public float MomentScale;
        public float Mass;
        public RealPoint3d CenterOfMass;
        public float Density;
        public float GravityScale;
        public float GroundFriction;
        public float GroundDepth;
        public float GroundDampFraction;
        public float GroundNormalK1;
        public float GroundNormalK0;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding1;
        public float WaterFriction;
        public float WaterDepth;
        public float WaterDensity;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding2;
        public float AirFriction;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding3;
        public float XxMoment;
        public float YyMoment;
        public float ZzMoment;
        public List<RealMatrix3x3> InertialMatrixAndInverse;
        public List<PoweredMassPointDefinition> PoweredMassPoints;
        public List<MassPointDefinition> MassPoints;
        
        [TagStructure(Size = 0x24)]
        public class RealMatrix3x3 : TagStructure
        {
            public RealVector3d YyZzXyZx;
            public RealVector3d XyZzXxYz;
            public RealVector3d ZxYzXxYy;
        }
        
        [TagStructure(Size = 0x80)]
        public class PoweredMassPointDefinition : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public FlagsValue Flags;
            public float AntigravStrength;
            public float AntigravOffset;
            public float AntigravHeight;
            public float AntigravDampFraction;
            public float AntigravNormalK1;
            public float AntigravNormalK0;
            public StringId DamageSourceRegionName;
            [TagField(Flags = Padding, Length = 64)]
            public byte[] Padding1;
            
            [Flags]
            public enum FlagsValue : uint
            {
                GroundFriction = 1 << 0,
                WaterFriction = 1 << 1,
                AirFriction = 1 << 2,
                WaterLift = 1 << 3,
                AirLift = 1 << 4,
                Thrust = 1 << 5,
                Antigrav = 1 << 6,
                GetsDamageFromRegion = 1 << 7
            }
        }
        
        [TagStructure(Size = 0x80)]
        public class MassPointDefinition : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short PoweredMassPoint;
            public short ModelNode;
            public FlagsValue Flags;
            public float RelativeMass;
            public float Mass;
            public float RelativeDensity;
            public float Density;
            public RealPoint3d Position;
            public RealVector3d Forward;
            public RealVector3d Up;
            public FrictionTypeValue FrictionType;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public float FrictionParallelScale;
            public float FrictionPerpendicularScale;
            public float Radius;
            [TagField(Flags = Padding, Length = 20)]
            public byte[] Padding2;
            
            [Flags]
            public enum FlagsValue : uint
            {
                Metallic = 1 << 0
            }
            
            public enum FrictionTypeValue : short
            {
                Point,
                Forward,
                Left,
                Up
            }
        }
    }
}

