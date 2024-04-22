using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "particleman", Tag = "pman", Size = 0x24)]
    public class Particleman : TagStructure
    {
        public ParticleizeShapeEnum Shape;
        public ParticleizeFlags Flags;
        public float Duration; // sec
        public float Density;
        public float Size; // world units
        public ParticleizeScalarFunctionStruct Curve;
        
        public enum ParticleizeShapeEnum : short
        {
            Cloud
        }
        
        [Flags]
        public enum ParticleizeFlags : ushort
        {
            HideObjectWhenEffectCompletes = 1 << 0,
            MoveParticlesTowardsCurrentSetTargetLocation = 1 << 1,
            OverrideAnyCurrentlyRunningParticleization = 1 << 2
        }
        
        [TagStructure(Size = 0x14)]
        public class ParticleizeScalarFunctionStruct : TagStructure
        {
            public MappingFunction Mapping;
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
    }
}
