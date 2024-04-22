using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "cheap_particle_type_library", Tag = "cptl", Size = 0x64)]
    public class CheapParticleTypeLibrary : TagStructure
    {
        public List<CheapParticleTypeBlock> Types;
        public List<CheapParticleBitmapReferenceBlock> Textures;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Random; // texture used to generate random values per particle
        public List<CheapparticleTurbulenceTypeBlock> TurbulenceTypes;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag TypeTexture;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag RenderTexture;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Turbulencetexture;
        
        [TagStructure(Size = 0x54)]
        public class CheapParticleTypeBlock : TagStructure
        {
            public StringId Name;
            public float Drag;
            public float Gravity;
            public float Turbulence;
            // the type of turbulence this particle will experience
            public int TurbulenceType;
            // depth range over which the particle will collide
            public float DepthRange;
            // energy remaining after collision bounce
            public float Elasticity;
            // percentage change [0-1] that the particle will die on collision
            public float Death;
            // the type this particle will change into on collision
            public int ChangeType;
            public CheapParticleTypeOrientation Orientation;
            public RealArgbColor Color0;
            public float Intensity0;
            // point in particles lifetime at which fade begins
            public float FadeStart; // [0,1]
            public Bounds<float> Size; // world units
            // how much the particle stretches as it moves
            public float MotionBlurStretch;
            public int Texture;
            // scales the texture in the y direction
            public float TextureYScale;
            
            public enum CheapParticleTypeOrientation : int
            {
                Velocity,
                ScreenFacing
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class CheapParticleBitmapReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Bitmap;
        }
        
        [TagStructure(Size = 0x24)]
        public class CheapparticleTurbulenceTypeBlock : TagStructure
        {
            public StringId Name;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Turbulence; // texture used to turbulate particles
            // change in u coordinate over time
            public float TurbDuDt;
            // change in v coordinate over time
            public float TurbDvDt;
            // change in u coordinate per particle
            public float TurbDuDp;
            // change in v coordinate per particle
            public float TurbDvDp;
        }
    }
}
