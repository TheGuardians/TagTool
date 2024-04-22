using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "particle_physics", Tag = "pmov", Size = 0x34)]
    public class ParticlePhysics : TagStructure
    {
        [TagField(ValidTags = new [] { "pmov" })]
        public CachedTag Template;
        public ParticleMovementFlags Flags;
        public sbyte CollisionControllerIndex;
        public sbyte TurbulenceControllerIndex;
        public sbyte GlobalForceControllerIndex;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public List<ParticleController> Movements;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag TurbulenceTexture;
        
        [Flags]
        public enum ParticleMovementFlags : ushort
        {
            Physics = 1 << 0,
            CollideWithStructure = 1 << 1,
            CollideWithWater = 1 << 2,
            CollideWithScenery = 1 << 3,
            CollideWithVehicles = 1 << 4,
            CollideWithBipeds = 1 << 5,
            AlwaysCollideEveryFrame = 1 << 6,
            Swarm = 1 << 7,
            Wind = 1 << 8,
            Turbulence = 1 << 9,
            GlobalForce = 1 << 10,
            DisableSwarmCollision = 1 << 11
        }
        
        [TagStructure(Size = 0x18)]
        public class ParticleController : TagStructure
        {
            public ParticleMovementType Type;
            public ParticleControllerFlags Flags;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<ParticleControllerParameters> Parameters;
            public int RuntimeMConstantParameters;
            public int RuntimeMUsedParticleStates;
            
            public enum ParticleMovementType : short
            {
                Physics,
                Collider,
                Swarm,
                Wind,
                Turbulence,
                GlobalForce
            }
            
            [Flags]
            public enum ParticleControllerFlags : byte
            {
                PropertiesFullyIndexed = 1 << 0
            }
            
            [TagStructure(Size = 0x24)]
            public class ParticleControllerParameters : TagStructure
            {
                public int ParameterId;
                public ParticlePropertyScalarStructNew Property;
                
                [TagStructure(Size = 0x20)]
                public class ParticlePropertyScalarStructNew : TagStructure
                {
                    public GameStateTypeEnum InputVariable;
                    public GameStateTypeEnum RangeVariable;
                    public OutputModEnum OutputModifier;
                    public GameStateTypeEnum OutputModifierInput;
                    public MappingFunction Mapping;
                    public float RuntimeMConstantValue;
                    public ushort RuntimeMFlags;
                    public ForceFlags ForceFlags1;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    public enum GameStateTypeEnum : sbyte
                    {
                        ParticleAge,
                        SystemAge,
                        ParticleRandom,
                        SystemRandom,
                        ParticleCorrelation1,
                        ParticleCorrelation2,
                        ParticleCorrelation3,
                        ParticleCorrelation4,
                        SystemCorrelation1,
                        SystemCorrelation2,
                        ParticleEmissionTime,
                        LocationLod,
                        GameTime,
                        EffectAScale,
                        EffectBScale,
                        ParticleRotation,
                        LocationRandom,
                        DistanceFromEmitter,
                        ParticleSimulationA,
                        ParticleSimulationB,
                        ParticleVelocity,
                        InvalidStatePleaseSetAgain
                    }
                    
                    public enum OutputModEnum : sbyte
                    {
                        Unknown,
                        Plus,
                        Times
                    }
                    
                    [Flags]
                    public enum ForceFlags : byte
                    {
                        ForceConstant = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x14)]
                    public class MappingFunction : TagStructure
                    {
                        public byte[] Data;
                    }
                }
            }
        }
    }
}
