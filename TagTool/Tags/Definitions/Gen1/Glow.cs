using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "glow", Tag = "glw!", Size = 0x154)]
    public class Glow : TagStructure
    {
        /// <summary>
        /// the marker name that the glow should be attached to
        /// </summary>
        [TagField(Length = 32)]
        public string AttachmentMarker;
        /// <summary>
        /// the number of particles that comprise the glow system
        /// </summary>
        public short NumberOfParticles;
        /// <summary>
        /// particles behavior on reaching the end of an object
        /// </summary>
        public BoundaryEffectValue BoundaryEffect;
        /// <summary>
        /// distribution of the normal particles about the object
        /// </summary>
        public NormalParticleDistributionValue NormalParticleDistribution;
        /// <summary>
        /// distribution of the trailing particles about the object
        /// </summary>
        public TrailingParticleDistributionValue TrailingParticleDistribution;
        public GlowFlagsValue GlowFlags;
        [TagField(Length = 0x1C)]
        public byte[] Padding;
        [TagField(Length = 0x2)]
        public byte[] Padding1;
        [TagField(Length = 0x2)]
        public byte[] Padding2;
        [TagField(Length = 0x4)]
        public byte[] Padding3;
        public AttachmentValue Attachment;
        [TagField(Length = 0x2)]
        public byte[] Padding4;
        /// <summary>
        /// radians per second
        /// </summary>
        public float ParticleRotationalVelocity;
        /// <summary>
        /// multiplied by overall velocity; only used if controlled by attachment
        /// </summary>
        public float ParticleRotVelMulLow;
        /// <summary>
        /// multiplied by overall velocity; only used if controlled by attachment
        /// </summary>
        public float ParticleRotVelMulHigh;
        public Attachment1Value Attachment1;
        [TagField(Length = 0x2)]
        public byte[] Padding5;
        /// <summary>
        /// in radians per second
        /// </summary>
        public float EffectRotationalVelocity;
        /// <summary>
        /// multiplied by overall velocity; only used if controlled by attachment
        /// </summary>
        public float EffectRotVelMulLow;
        /// <summary>
        /// multiplied by overall velocity; only used if controlled by attachment
        /// </summary>
        public float EffectRotVelMulHigh;
        public Attachment2Value Attachment2;
        [TagField(Length = 0x2)]
        public byte[] Padding6;
        /// <summary>
        /// in world units per second
        /// </summary>
        public float EffectTranslationalVelocity;
        /// <summary>
        /// multiplied by overall velocity; only used if controlled by attachment
        /// </summary>
        public float EffectTransVelMulLow;
        /// <summary>
        /// multiplied by overall velocity; only used if controlled by attachment
        /// </summary>
        public float EffectTransVelMulHigh;
        public Attachment3Value Attachment3;
        [TagField(Length = 0x2)]
        public byte[] Padding7;
        /// <summary>
        /// in world units
        /// </summary>
        public float MinDistanceParticleToObject;
        /// <summary>
        /// in world units
        /// </summary>
        public float MaxDistanceParticleToObject;
        /// <summary>
        /// multiplied by particle distance; only used if controlled by attachment
        /// </summary>
        public float DistanceToObjectMulLow;
        /// <summary>
        /// multiplied by particle distance; only used if controlled by attachment
        /// </summary>
        public float DistanceToObjectMulHigh;
        [TagField(Length = 0x8)]
        public byte[] Padding8;
        public Attachment4Value Attachment4;
        [TagField(Length = 0x2)]
        public byte[] Padding9;
        /// <summary>
        /// size of particles
        /// </summary>
        public Bounds<float> ParticleSizeBounds; // world units
        /// <summary>
        /// multiplied by particle size; only used if controlled by attachment
        /// </summary>
        public Bounds<float> SizeAttachmentMultiplier;
        public Attachment5Value Attachment5;
        [TagField(Length = 0x2)]
        public byte[] Padding10;
        /// <summary>
        /// the color of all particles will vary between color bound 0 and color bound 1
        /// </summary>
        public RealArgbColor ColorBound0;
        /// <summary>
        /// the color of all particles will vary between color bound 0 and color bound 1
        /// </summary>
        public RealArgbColor ColorBound1;
        /// <summary>
        /// used to scale the particle color; only used if controlled by attachment
        /// </summary>
        public RealArgbColor ScaleColor0;
        /// <summary>
        /// used to scale the particle color; only used if controlled by attachment
        /// </summary>
        public RealArgbColor ScaleColor1;
        /// <summary>
        /// for glow effects that dynamically control particle color; sets rate of change
        /// </summary>
        public float ColorRateOfChange;
        /// <summary>
        /// percentage of the glow that is fading at any given time
        /// </summary>
        public float FadingPercentageOfGlow;
        /// <summary>
        /// frequency in Hz that trailing particles are generated:Hz
        /// </summary>
        public float ParticleGenerationFreq;
        /// <summary>
        /// seconds that a trailing particle remains in existence:s
        /// </summary>
        public float LifetimeOfTrailingParticles;
        public float VelocityOfTrailingParticles; // wu/s
        public float TrailingParticleMinimumT;
        public float TrailingParticleMaximumT;
        [TagField(Length = 0x34)]
        public byte[] Padding11;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Texture;
        
        public enum BoundaryEffectValue : short
        {
            Bounce,
            Wrap
        }
        
        public enum NormalParticleDistributionValue : short
        {
            DistributedRandomly,
            DistributedUniformly
        }
        
        public enum TrailingParticleDistributionValue : short
        {
            EmitVertically,
            EmitNormalUp,
            EmitRandomly
        }
        
        [Flags]
        public enum GlowFlagsValue : uint
        {
            ModifyParticleColorInRange = 1 << 0,
            ParticlesMoveBackwards = 1 << 1,
            ParticesMoveInBothDirections = 1 << 2,
            TrailingParticlesFadeOverTime = 1 << 3,
            TrailingParticlesShrinkOverTime = 1 << 4,
            TrailingParticlesSlowOverTime = 1 << 5
        }
        
        public enum AttachmentValue : short
        {
            None,
            AOut,
            BOut,
            COut,
            DOut
        }
        
        public enum Attachment1Value : short
        {
            None,
            AOut,
            BOut,
            COut,
            DOut
        }
        
        public enum Attachment2Value : short
        {
            None,
            AOut,
            BOut,
            COut,
            DOut
        }
        
        public enum Attachment3Value : short
        {
            None,
            AOut,
            BOut,
            COut,
            DOut
        }
        
        public enum Attachment4Value : short
        {
            None,
            AOut,
            BOut,
            COut,
            DOut
        }
        
        public enum Attachment5Value : short
        {
            None,
            AOut,
            BOut,
            COut,
            DOut
        }
    }
}

