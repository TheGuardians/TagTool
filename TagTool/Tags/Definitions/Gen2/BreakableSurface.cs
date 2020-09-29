using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "breakable_surface", Tag = "bsdt", Size = 0x20)]
    public class BreakableSurface : TagStructure
    {
        public float MaximumVitality;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag Effect;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag Sound;
        public List<ParticleSystemDefinitionBlockNew> ParticleEffects;
        public float ParticleDensity;
        
        [TagStructure(Size = 0x38)]
        public class ParticleSystemDefinitionBlockNew : TagStructure
        {
            [TagField(ValidTags = new [] { "prt3","PRTM" })]
            public CachedTag Particle;
            public int Location;
            public CoordinateSystemValue CoordinateSystem;
            public EnvironmentValue Environment;
            public DispositionValue Disposition;
            public CameraModeValue CameraMode;
            /// <summary>
            /// use values between -10 and 10 to move closer and farther from camera (positive is closer)
            /// </summary>
            public short SortBias;
            public FlagsValue Flags;
            /// <summary>
            /// defaults to 0.0
            /// </summary>
            public float LodInDistance;
            /// <summary>
            /// defaults to 0.0
            /// </summary>
            public float LodFeatherInDelta;
            [TagField(Length = 0x4)]
            public byte[] Unknown;
            /// <summary>
            /// defaults to 30.0
            /// </summary>
            public float LodOutDistance;
            /// <summary>
            /// defaults to 10.0
            /// </summary>
            public float LodFeatherOutDelta;
            [TagField(Length = 0x4)]
            public byte[] Unknown1;
            public List<ParticleSystemEmitterDefinitionBlock> Emitters;
            
            public enum CoordinateSystemValue : short
            {
                World,
                Local,
                Parent
            }
            
            public enum EnvironmentValue : short
            {
                AnyEnvironment,
                AirOnly,
                WaterOnly,
                SpaceOnly
            }
            
            public enum DispositionValue : short
            {
                EitherMode,
                ViolentModeOnly,
                NonviolentModeOnly
            }
            
            public enum CameraModeValue : short
            {
                IndependentOfCameraMode,
                OnlyInFirstPerson,
                OnlyInThirdPerson,
                BothFirstAndThird
            }
            
            [Flags]
            public enum FlagsValue : ushort
            {
                Glow = 1 << 0,
                Cinematics = 1 << 1,
                LoopingParticle = 1 << 2,
                DisabledForDebugging = 1 << 3,
                InheritEffectVelocity = 1 << 4,
                DonTRenderSystem = 1 << 5,
                RenderWhenZoomed = 1 << 6,
                SpreadBetweenTicks = 1 << 7,
                PersistentParticle = 1 << 8,
                ExpensiveVisibility = 1 << 9
            }
            
            [TagStructure(Size = 0xB8)]
            public class ParticleSystemEmitterDefinitionBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "pmov" })]
                public CachedTag ParticlePhysics;
                public ParticlePropertyScalarStructNewBlock ParticleEmissionRate;
                public ParticlePropertyScalarStructNewBlock1 ParticleLifespan;
                public ParticlePropertyScalarStructNewBlock2 ParticleVelocity;
                public ParticlePropertyScalarStructNewBlock3 ParticleAngularVelocity;
                public ParticlePropertyScalarStructNewBlock4 ParticleSize;
                public ParticlePropertyColorStructNewBlock ParticleTint;
                public ParticlePropertyScalarStructNewBlock5 ParticleAlpha;
                public EmissionShapeValue EmissionShape;
                public ParticlePropertyScalarStructNewBlock6 EmissionRadius;
                public ParticlePropertyScalarStructNewBlock7 EmissionAngle;
                public RealPoint3d TranslationalOffset;
                /// <summary>
                /// particle initial velocity direction relative to the location's forward
                /// </summary>
                public RealEulerAngles2d RelativeDirection;
                [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [TagStructure(Size = 0x10)]
                public class ParticlePropertyScalarStructNewBlock : TagStructure
                {
                    public InputVariableValue InputVariable;
                    public RangeVariableValue RangeVariable;
                    public OutputModifierValue OutputModifier;
                    public OutputModifierInputValue OutputModifierInput;
                    public MappingFunctionBlock Mapping;
                    
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
                        Unknown,
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
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class ParticlePropertyScalarStructNewBlock1 : TagStructure
                {
                    public InputVariableValue InputVariable;
                    public RangeVariableValue RangeVariable;
                    public OutputModifierValue OutputModifier;
                    public OutputModifierInputValue OutputModifierInput;
                    public MappingFunctionBlock Mapping;
                    
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
                        Unknown,
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
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class ParticlePropertyScalarStructNewBlock2 : TagStructure
                {
                    public InputVariableValue InputVariable;
                    public RangeVariableValue RangeVariable;
                    public OutputModifierValue OutputModifier;
                    public OutputModifierInputValue OutputModifierInput;
                    public MappingFunctionBlock Mapping;
                    
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
                        Unknown,
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
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class ParticlePropertyScalarStructNewBlock3 : TagStructure
                {
                    public InputVariableValue InputVariable;
                    public RangeVariableValue RangeVariable;
                    public OutputModifierValue OutputModifier;
                    public OutputModifierInputValue OutputModifierInput;
                    public MappingFunctionBlock Mapping;
                    
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
                        Unknown,
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
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class ParticlePropertyScalarStructNewBlock4 : TagStructure
                {
                    public InputVariableValue InputVariable;
                    public RangeVariableValue RangeVariable;
                    public OutputModifierValue OutputModifier;
                    public OutputModifierInputValue OutputModifierInput;
                    public MappingFunctionBlock Mapping;
                    
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
                        Unknown,
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
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class ParticlePropertyColorStructNewBlock : TagStructure
                {
                    public InputVariableValue InputVariable;
                    public RangeVariableValue RangeVariable;
                    public OutputModifierValue OutputModifier;
                    public OutputModifierInputValue OutputModifierInput;
                    public MappingFunctionBlock Mapping;
                    
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
                        Unknown,
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
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class ParticlePropertyScalarStructNewBlock5 : TagStructure
                {
                    public InputVariableValue InputVariable;
                    public RangeVariableValue RangeVariable;
                    public OutputModifierValue OutputModifier;
                    public OutputModifierInputValue OutputModifierInput;
                    public MappingFunctionBlock Mapping;
                    
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
                        Unknown,
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
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
                
                public enum EmissionShapeValue : int
                {
                    Sprayer,
                    Disc,
                    Globe,
                    Implode,
                    Tube,
                    Halo,
                    ImpactContour,
                    ImpactArea,
                    Debris,
                    Line
                }
                
                [TagStructure(Size = 0x10)]
                public class ParticlePropertyScalarStructNewBlock6 : TagStructure
                {
                    public InputVariableValue InputVariable;
                    public RangeVariableValue RangeVariable;
                    public OutputModifierValue OutputModifier;
                    public OutputModifierInputValue OutputModifierInput;
                    public MappingFunctionBlock Mapping;
                    
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
                        Unknown,
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
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class ParticlePropertyScalarStructNewBlock7 : TagStructure
                {
                    public InputVariableValue InputVariable;
                    public RangeVariableValue RangeVariable;
                    public OutputModifierValue OutputModifier;
                    public OutputModifierInputValue OutputModifierInput;
                    public MappingFunctionBlock Mapping;
                    
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
                        Unknown,
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
                    
                    [TagStructure(Size = 0x8)]
                    public class MappingFunctionBlock : TagStructure
                    {
                        public List<ByteBlock> Data;
                        
                        [TagStructure(Size = 0x1)]
                        public class ByteBlock : TagStructure
                        {
                            public sbyte Value;
                        }
                    }
                }
            }
        }
    }
}

