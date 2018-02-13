using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "particle_emitter_custom_points", Tag = "pecp", Size = 0x34)]
    public class ParticleEmitterCustomPoints
    {
        public CachedTagInstance ParticleModel;
        public RealVector3d CompressionScale;
        public RealVector3d CompressionOffset;
        public List<Point> Points;

        [TagStructure(Size = 0xA)]
        public class Point
        {
            public short PositionX;
            public short PositionY;
            public short PositionZ;
            public byte NormalX;
            public byte NormalY;
            public byte NormalZ;
            public byte Correlation;
        }
    }
}