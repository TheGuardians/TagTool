using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "cheap_particle_emitter", Tag = "cpem", Size = 0x10C)]
    public class CheapParticleEmitter : TagStructure
    {
        public CheapParticleEmitterFlags Flags;
        public ushort Version;
        public float SpawnRate; // particles per second
        public CheapParticleScalarObjectFunctionStruct Spawnrate;
        // the distance where the number of spawned particles starts to be reduced
        public float DistanceFadeStart; // world_units
        // the distance where the number of spawned particles is zero
        public float DistanceFadeEnd; // world units
        public StringId Type0;
        public float Weight0;
        public StringId Type1;
        public float Weight1;
        public StringId Type2;
        public float Weight2;
        public StringId Type3;
        public float Weight3;
        public Bounds<float> Lifetime; // seconds
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag PositionTexture;
        public float PositionScale;
        public float PositionFlatten;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag VelocityTexture;
        // Blends between a random direction and the forward direction
        public Bounds<float> Directionality; // [0-1]
        // scales the initial speed of the particle
        public Bounds<float> SpeedScale;
        // spawns particle at subframe time
        public Bounds<float> SubframeOffset; // frames
        // how much the particle is randomly rotated
        public float RotationRandomness; // [0-1]
        // modifies the inherent scale of the particles
        public float ParticleScaleModifier; // [0-2]
        // increase or decrease contrast between bright and dark areas
        public float LightingContrast;
        // adjust overall brightness in all areas
        public float LightingOffset;
        // clamps darkest particles to this exposure
        public float LightingMin;
        // clamps brightest particles to this exposure
        public float LightingMax;
        public RealQuaternion SpawnParams0;
        public RealQuaternion SpawnParams1;
        public RealQuaternion SpawnParams2;
        public RealQuaternion SpawnParams3;
        public RealQuaternion SpawnParams4;
        [TagField(ValidTags = new [] { "cptl" })]
        public CachedTag GlobalTypeLibrary;
        
        [Flags]
        public enum CheapParticleEmitterFlags : ushort
        {
            CorrelatePositionAndVelocity = 1 << 0,
            PositionTextureInLocalSpace = 1 << 1,
            VelocityTextureInLocalSpace = 1 << 2,
            NormalizeVelocityBeforeScaling = 1 << 3,
            RandomlyRotateEmitterAboutUpVector = 1 << 4
        }
        
        [TagStructure(Size = 0x1C)]
        public class CheapParticleScalarObjectFunctionStruct : TagStructure
        {
            public StringId InputVariable;
            public StringId RangeVariable;
            public MappingFunction Mapping;
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
    }
}
