using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "shield_impact", Tag = "shit", Size = 0xA4, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "shield_impact", Tag = "shit", Size = 0xB0, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "shield_impact", Tag = "shit", Size = 0x1E4, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "shield_impact", Tag = "shit", Size = 0x1E0, MinVersion = CacheVersion.HaloReach)]
    public class ShieldImpact
    {
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public ShieldImpactFlags Flags;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public short Version;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public ShieldIntensityBlock ShieldIntensity;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public ShieldEdgeBlock ShieldEdge;

        public PlasmaBlock Plasma;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public ExtrusionOscillationBlock ExtrusionOscillation;
        
        public RealQuaternion EdgeScales;
        public RealQuaternion EdgeOffsets;
        public RealQuaternion PlasmaScales;
        public RealQuaternion DepthFadeParameters;

        /// <summary>
        /// Shield intensity is a combination of recent damage and current damage.
        /// These controls let you adjust the relative intensity contribution from each.
        /// 'shield_intensity' can be used as an input to any of the animation function curves below.
        /// </summary>
        [TagStructure(Size = 0x8)]
        public class ShieldIntensityBlock
        {
            public float RecentDamageIntensity;
            public float CurrentDamageIntensity;
        }

        /// <summary>
        /// These controls allow you to define the location and width of the shield edges.
        /// The edge is faded as a function of the surface normal with respect to the camera.
        /// Radius 1.0 corresponds to the glancing edges of the surface(the silhouette edges).
        /// Radius 0.0 corresponds to the area of the surface directly facing the camera(the center).
        /// You can control separately the inner and outer fades.
        /// </summary>
        [TagStructure(Size = 0x4C, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x48, MinVersion = CacheVersion.HaloReach)]
        public class ShieldEdgeBlock
        {
            [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
            public float OneIfOvershield;
            public float DepthFadeRange;            //In world units
            public float OuterFadeRadius;           //Within [0,1]
            public float CenterRadius;              //Within [0,1]
            public float InnerFadeRadius;           //Within [0,1]
            public ShieldImpactFunction EdgeGlowColor;
            public ShieldImpactFunction EdgeGlowIntensity;
        }

        /// <summary>
        /// These controls allow you to define the appearance of the plasma effect.
        /// The plasma is calculated using the standard formula(1-abs(tex0-tex1))^(sharpness).
        /// Tiling scale controls the spatial tiling of the plasma textures.
        /// Scroll speed controls how fast the textures scroll on the surface.
        /// You can specify separate sharpness values for the edge and the center.
        /// </summary>
        [TagStructure(Size = 0x64, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x6C, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0xB0, MinVersion = CacheVersion.HaloOnline106708)]
        public class PlasmaBlock
        {
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float PlasmaDepthFadeRange;      //In world units

            [TagField(ValidTags = new[] { "bitm" })]
            public CachedTagInstance PlasmaNoiseBitmap1;
            [TagField(ValidTags = new[] { "bitm" })]
            public CachedTagInstance PlasmaNoiseBitmap2;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float TilingScale;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float ScrollSpeed;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float EdgeSharpness;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float CenterSharpness;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float PlasmaOuterFadeRadius;     //Within [0,1]
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float PlasmaCenterRadius;        //Within [0,1]
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float PlasmaInnerFadeRadius;     //Within [0,1]

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ShieldImpactFunction PlasmaCenterColor;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ShieldImpactFunction PlasmaCenterIntensity;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ShieldImpactFunction PlasmaEdgeColor;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public ShieldImpactFunction PlasmaEdgeIntensity;

            //
            // Values for H3/ODST.  Needs more reversing
            //

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown1;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown2;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown3;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown4;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown5;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown6;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown7;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown8;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown9;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown10;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown11;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown12;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown13;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown14;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown15;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown16;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown17;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown18;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown19;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown20;
        }

        /// <summary>
        /// These controls allow you to define the oscillation in the extrusion.
        /// These textures are tiled and scrolled in the xy and yz planes, and their red channel is applied as an offset to the extrusion
        /// Tiling scale controls the spatial tiling of the plasma textures.
        /// Scroll speed controls how fast the textures scroll on the surface.
        /// You can specify separate sharpness values for the edge and the center.
        /// </summary>
        [TagStructure(Size = 0x9C)]
        public class ExtrusionOscillationBlock
        {
            [TagField(ValidTags = new[] { "bitm" })]
            public CachedTagInstance OscillationBitmap1;
            [TagField(ValidTags = new[] { "bitm" })]
            public CachedTagInstance OscillationBitmap2;

            public float OscillationTilingScale;
            public float OscillationScrollSpeed;
            public ShieldImpactFunction ExtrusionAmount;
            public ShieldImpactFunction OscillationAmplitude;
            public HitResponseBlock HitResponse;

            //HO is missing hit blob bitmap and either it's HitIntensity or HitRadius ShieldFunction, to be determined

            /// <summary>
            /// These controls allow you to define the color variation in the area surrounding projectile impacts.
            /// You can control the color, and the size of the colored area.
            /// The default input is time since impact.
            /// </summary>
            [TagStructure(Size = 0x3C)]
            public class HitResponseBlock
            {
                /// <summary>
                /// The hit time of the hit response in seconds.
                /// </summary>
                public float HitTime;
                public ShieldImpactFunction HitColor;
				public ShieldImpactFunction HitIntensity;
            }
        }
    }

    /// <summary>
    /// Bitwise flags for <see cref="ShieldImpact"/>.
    /// </summary>
    [Flags]
    public enum ShieldImpactFlags : ushort
    {
        AlwaysRender = 0,
        RenderFirstPerson = 1 << 0,
        DontRenderThirdPerson = 1 << 1,
    }

    /// <summary>
    /// You can use the following variables as inputs to the functions here, in addition to any object variables:
    /// {shield_vitality (percentage of shield remaining),
    /// shield_intensity (mixture of recent and current damage),
    /// current_shield_damage,
    /// recent_shield_damage}
    /// </summary>
    [TagStructure(Size = 0x1C)]
    public class ShieldImpactFunction
    {
        public StringId InputVariable;
        public StringId RangeVariable;
        public TagFunction Function = new TagFunction { Data = new byte[0] };
    }
}