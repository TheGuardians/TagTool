using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "planar_fog", Tag = "fog ", Size = 0x84)]
    public class PlanarFog : TagStructure
    {
        /// <summary>
        /// PLANAR FOG
        /// </summary>
        /// <remarks>
        /// Please don't use these flags unless you know what you're doing! Come talk to Bernie first.
        /// </remarks>
        public FlagsValue Flags;
        public short Priority;
        public StringId GlobalMaterialName;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding2;
        /// <summary>
        /// DENSITY
        /// </summary>
        public float MaximumDensity; // [0,1]
        public float OpaqueDistance; // world units
        public float OpaqueDepth; // world units
        /// <summary>
        /// DENSITY (ADVANCED CONTROLS)
        /// </summary>
        public Bounds<float> AtmosphericPlanarDepth; // world units
        public float EyeOffsetScale; // [-1,1]
        /// <summary>
        /// COLOR
        /// </summary>
        public RealRgbColor Color;
        public List<PlanarFogPatchyFog> PatchyFog;
        /// <summary>
        /// SOUND
        /// </summary>
        public CachedTag BackgroundSound;
        public CachedTag SoundEnvironment;
        public float EnvironmentDampingFactor; // scales the surrounding background sound by this much
        public float BackgroundSoundGain; // scale for fog background sound
        public CachedTag EnterSound;
        public CachedTag ExitSound;
        
        [Flags]
        public enum FlagsValue : ushort
        {
            RenderOnlySubmergedGeometry = 1 << 0,
            ExtendInfinitelyWhileVisible = 1 << 1,
            DonTFloodfill = 1 << 2,
            AggressiveFloodfill = 1 << 3,
            DoNotRender = 1 << 4,
            DoNotRenderUnlessSubmerged = 1 << 5
        }
        
        [TagStructure(Size = 0x3C)]
        public class PlanarFogPatchyFog : TagStructure
        {
            public RealRgbColor Color;
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Padding1;
            public Bounds<float> Density; // [0,1]
            public Bounds<float> Distance; // world units
            public float MinDepthFraction; // [0,1]
            public CachedTag PatchyFog;
        }
    }
}

