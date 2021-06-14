using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "particle_emitter_custom_points", Tag = "pecp", Size = 0x34)]
    public class ParticleEmitterCustomPoints : TagStructure
    {
        [TagField(ValidTags = new [] { "pmdf" })]
        public CachedTag SourceReference;
        public RealVector3d CompressionScale;
        public RealVector3d CompressionOffset;
        public List<ParticleEmitterCustomPointBlock> Points;
        
        [TagStructure(Size = 0xA)]
        public class ParticleEmitterCustomPointBlock : TagStructure
        {
            public short PositionX;
            public short PositionY;
            public short PositionZ;
            public sbyte NormalX;
            public sbyte NormalY;
            public sbyte NormalZ;
            public byte Correlation;
        }
    }
}
