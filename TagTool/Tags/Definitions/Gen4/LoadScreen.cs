using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "load_screen", Tag = "ldsc", Size = 0x60)]
    public class Loadscreen : TagStructure
    {
        public Loadscreenflags Flags;
        [TagField(ValidTags = new [] { "mode" })]
        public CachedTag SourceRenderModel;
        // the higher this number, the sooner the model is fully spawned in
        public float SpawnRateMultiplier;
        // the direction in model space that the scan starts from.
        // Ignored if do not sort vertex order flag is set above
        public RealPoint3d SourceScanPosition;
        // model scale to match camera space
        public float ScaleFactor;
        public RealPoint3d CameraPosition;
        public RealPoint3d CameraTargetPosition;
        // 0 means camera is stationary
        public float CameraOrbitPeriod;
        // scale factor for individual particles
        public float ParticleScale;
        // This is the acceleration rate when particles spawn in and move to their ultimate destination.  Higher numbers move
        // faster.
        public float ParticleAcceleration;
        // If flag particles spawn radially is set, this is the multiple of the particle location along the radial axis.
        // If not, this the random spawn distance from the particle destination.
        public float ParticleInitialOffsetMultiplier;
        public int ComputedModelVertexCount; // vertices
        public List<Vertexblock> ModelVertices;
        
        [Flags]
        public enum Loadscreenflags : uint
        {
            ResolveTwoSided = 1 << 0,
            DoNotSortVertexOrder = 1 << 1,
            // see particle initial offset multiplier
            ParticlesSpawnRadially = 1 << 2
        }
        
        [TagStructure(Size = 0xC)]
        public class Vertexblock : TagStructure
        {
            public RealPoint3d Point;
        }
    }
}
