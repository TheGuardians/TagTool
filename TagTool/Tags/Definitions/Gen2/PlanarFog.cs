using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "planar_fog", Tag = "fog ", Size = 0x60)]
    public class PlanarFog : TagStructure
    {
        /// <summary>
        /// Please don't use these flags unless you know what you're doing! Come talk to Bernie first.
        /// </summary>
        public FlagsValue Flags;
        public short Priority;
        public StringId GlobalMaterialName;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        /// <summary>
        /// planar fog density is clamped to this value
        /// </summary>
        public float MaximumDensity; // [0,1]
        /// <summary>
        /// the fog becomes opaque (maximum density) at this distance from the viewer
        /// </summary>
        public float OpaqueDistance; // world units
        /// <summary>
        /// the fog becomes opaque at this distance below fog plane
        /// </summary>
        public float OpaqueDepth; // world units
        /// <summary>
        /// distances above fog plane where atmospheric fog supercedes planar fog and visa-versa
        /// </summary>
        public Bounds<float> AtmosphericPlanarDepth; // world units
        /// <summary>
        /// negative numbers are bad, mmmkay?
        /// </summary>
        public float EyeOffsetScale; // [-1,1]
        public RealRgbColor Color;
        public List<PlanarFogPatchyFogBlock> PatchyFog;
        [TagField(ValidTags = new [] { "lsnd" })]
        public CachedTag BackgroundSound;
        [TagField(ValidTags = new [] { "snde" })]
        public CachedTag SoundEnvironment;
        /// <summary>
        /// scales the surrounding background sound by this much
        /// </summary>
        public float EnvironmentDampingFactor;
        /// <summary>
        /// scale for fog background sound
        /// </summary>
        public float BackgroundSoundGain;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag EnterSound;
        [TagField(ValidTags = new [] { "snd!" })]
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
        
        [TagStructure(Size = 0x34)]
        public class PlanarFogPatchyFogBlock : TagStructure
        {
            public RealRgbColor Color;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public Bounds<float> Density; // [0,1]
            public Bounds<float> Distance; // world units
            /// <summary>
            /// in range (0,max_depth) world units, where patchy fog starts fading in
            /// </summary>
            public float MinDepthFraction; // [0,1]
            [TagField(ValidTags = new [] { "fpch" })]
            public CachedTag PatchyFog;
        }
    }
}

