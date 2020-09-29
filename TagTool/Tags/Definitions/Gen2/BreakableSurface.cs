using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "breakable_surface", Tag = "bsdt", Size = 0x34)]
    public class BreakableSurface : TagStructure
    {
        public float MaximumVitality;
        public CachedTag Effect;
        public CachedTag Sound;
        public List<ParticleSystemDefinition> ParticleEffects;
        public float ParticleDensity;
        
        [TagStructure(Size = 0x44)]
        public class ParticleSystemDefinition : TagStructure
        {
            public CachedTag Particle;
            public int Location;
            public CoordinateSystemValue CoordinateSystem;
            public EnvironmentValue Environment;
            public DispositionValue Disposition;
            public CameraModeValue CameraMode;
            public short SortBias; // use values between -10 and 10 to move closer and farther from camera (positive is closer)
            public FlagsValue Flags;
            public float LodInDistance; // defaults to 0.0
            public float LodFeatherInDelta; // defaults to 0.0
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown1;
            public float LodOutDistance; // defaults to 30.0
            public float LodFeatherOutDelta; // defaults to 10.0
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown2;
            public List<ParticleEmitterDefinition> Emitters;
            
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
            
            [TagStructure(Size = 0xE4)]
            public class ParticleEmitterDefinition : TagStructure
            {
                public CachedTag ParticlePhysics;
                /// <summary>
                /// particle emission rate (particles/tick)
                /// </summary>
                public ParticleProperty ParticleEmissionRate;
                /// <summary>
                /// particle lifespan(seconds)
                /// </summary>
                public ParticleProperty ParticleLifespan;
                /// <summary>
                /// particle velocity(world units/second)
                /// </summary>
                public ParticleProperty ParticleVelocity;
                /// <summary>
                /// particle angular velocity(degrees/second)
                /// </summary>
                public ParticleProperty ParticleAngularVelocity;
                /// <summary>
                /// particle size(world units)
                /// </summary>
                public ParticleProperty ParticleSize;
                /// <summary>
                /// particle tint
                /// </summary>
                public ParticleProperty ParticleTint;
                /// <summary>
                /// particle alpha
                /// </summary>
                public ParticleProperty ParticleAlpha;
                /// <summary>
                /// EMISSION SETTINGS
                /// </summary>
                public EmissionShapeValue EmissionShape;
                /// <summary>
                /// emission radius(world units)
                /// </summary>
                public ParticleProperty EmissionRadius;
                /// <summary>
                /// emission angle(degrees)
                /// </summary>
                public ParticleProperty EmissionAngle;
                public RealPoint3d TranslationalOffset;
                public RealEulerAngles2d RelativeDirection; // particle initial velocity direction relative to the location's forward
                [TagField(Flags = Padding, Length = 8)]
                public byte[] Padding1;
                
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
            }
        }
    }
}

