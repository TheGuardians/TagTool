using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "particle_emitter_boat_hull_shape", Tag = "ebhd", Size = 0x10)]
    public class ParticleEmitterBoatHullShape : TagStructure
    {
        // heuristic used to determine where to spawn particles
        public ParticleEmitterBoatHullDistributionType Distribution;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public List<ParticleEmitterBoatHullGroupBlock> Markers;
        
        public enum ParticleEmitterBoatHullDistributionType : sbyte
        {
            // project hull surface locations onto the water and spawn particles along the resulting path.
            ConnectTheDots,
            // spawn particles on the hull surface where it intersects with the water.
            AlongHullSurfaceOnly
        }
        
        [TagStructure(Size = 0x4)]
        public class ParticleEmitterBoatHullGroupBlock : TagStructure
        {
            public StringId MarkerGroup;
        }
    }
}
