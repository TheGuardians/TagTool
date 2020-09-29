using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "particle_physics", Tag = "pmov", Size = 0x20)]
    public class ParticlePhysics : TagStructure
    {
        public CachedTag Template;
        public FlagsValue Flags;
        public List<ParticleController> Movements;
        
        [Flags]
        public enum FlagsValue : uint
        {
            Physics = 1 << 0,
            CollideWithStructure = 1 << 1,
            CollideWithMedia = 1 << 2,
            CollideWithScenery = 1 << 3,
            CollideWithVehicles = 1 << 4,
            CollideWithBipeds = 1 << 5,
            Swarm = 1 << 6,
            Wind = 1 << 7
        }
        
        [TagStructure(Size = 0x18)]
        public class ParticleController : TagStructure
        {
            public TypeValue Type;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public List<ParticleControllerParameter> Parameters;
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding2;
            
            public enum TypeValue : short
            {
                Physics,
                Collider,
                Swarm,
                Wind
            }
            
            [TagStructure(Size = 0x18)]
            public class ParticleControllerParameter : TagStructure
            {
                public int ParameterId;
                public ParticleProperty Property;
                
                [TagStructure(Size = 0x14)]
                public class ParticleProperty : TagStructure
                {
                    public InputVariableValue InputVariable;
                    public RangeVariableValue RangeVariable;
                    public OutputModifierValue OutputModifier;
                    public OutputModifierInputValue OutputModifierInput;
                    public FunctionDefinition Mapping;
                    
                    public enum InputVariableValue : short
                    {
                        ParticleAge,
                        ParticleEmitTime,
                        ParticleRandom1,
                        ParticleRandom2,
                        EmitterAge,
                        EmitterRandom1,
                        EmitterRandom2,
                        SystemLod,
                        GameTime,
                        EffectAScale,
                        EffectBScale,
                        ParticleRotation,
                        ExplosionAnimation,
                        ExplosionRotation,
                        ParticleRandom3,
                        ParticleRandom4,
                        LocationRandom
                    }
                    
                    public enum RangeVariableValue : short
                    {
                        ParticleAge,
                        ParticleEmitTime,
                        ParticleRandom1,
                        ParticleRandom2,
                        EmitterAge,
                        EmitterRandom1,
                        EmitterRandom2,
                        SystemLod,
                        GameTime,
                        EffectAScale,
                        EffectBScale,
                        ParticleRotation,
                        ExplosionAnimation,
                        ExplosionRotation,
                        ParticleRandom3,
                        ParticleRandom4,
                        LocationRandom
                    }
                    
                    public enum OutputModifierValue : short
                    {
                        Unknown0,
                        Plus,
                        Times
                    }
                    
                    public enum OutputModifierInputValue : short
                    {
                        ParticleAge,
                        ParticleEmitTime,
                        ParticleRandom1,
                        ParticleRandom2,
                        EmitterAge,
                        EmitterRandom1,
                        EmitterRandom2,
                        SystemLod,
                        GameTime,
                        EffectAScale,
                        EffectBScale,
                        ParticleRotation,
                        ExplosionAnimation,
                        ExplosionRotation,
                        ParticleRandom3,
                        ParticleRandom4,
                        LocationRandom
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
            }
        }
    }
}

