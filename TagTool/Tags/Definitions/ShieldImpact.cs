using TagTool.Cache;
using TagTool.Common;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "shield_impact", Tag = "shit", Size = 0xA4, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "shield_impact", Tag = "shit", Size = 0xB0, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "shield_impact", Tag = "shit", Size = 0x1E4, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "shield_impact", Tag = "shit", Size = 0x1E0, MinVersion = CacheVersion.HaloReach)]
    public class ShieldImpact : TagStructure
	{
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public ShieldImpactFlags Flags;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public short Version;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public H3ValuesBlock H3Values;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public ShieldIntensityBlock ShieldIntensity;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public ShieldEdgeBlock ShieldEdge;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public PlasmaBlock Plasma;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public ExtrusionOscillationBlock ExtrusionOscillation;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public HitResponseBlock HitResponse;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public RealQuaternion EdgeScales; // outer scale, inner scale, plasmaedge outer scale, plasmaedge inner scale
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public RealQuaternion EdgeOffsets; // outer offset, inner offset, plasmaedge outer offset, plasmaedge inner offset
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public RealQuaternion PlasmaScales; // U scale, V scale, plasmanoise power scale, plasmanoise power offset
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public RealQuaternion DepthFadeParameters; // edge depth fade, plasma depth fade, unused, unused

        public void UpdateParameters()
        {
            float outerScale = 1.0f / (ShieldEdge.CenterRadius - ShieldEdge.OuterFadeRadius);
            float innerScale = 1.0f / (ShieldEdge.CenterRadius - ShieldEdge.InnerFadeRadius);
            float plasmaOuterScale = 1.0f / (Plasma.PlasmaCenterRadius - Plasma.PlasmaOuterFadeRadius);
            float plasmaInnerScale = 1.0f / (Plasma.PlasmaCenterRadius - Plasma.PlasmaInnerFadeRadius);

            float outerOffset = -1.0f / ((ShieldEdge.CenterRadius - ShieldEdge.OuterFadeRadius) / ShieldEdge.OuterFadeRadius);
            float innerOffset = -1.0f / ((ShieldEdge.CenterRadius - ShieldEdge.InnerFadeRadius) / ShieldEdge.InnerFadeRadius);
            float plasmaOuterOffset = -1.0f / ((Plasma.PlasmaCenterRadius - Plasma.PlasmaOuterFadeRadius) / Plasma.PlasmaOuterFadeRadius);
            float plasmaInnerOffset = -1.0f / ((Plasma.PlasmaCenterRadius - Plasma.PlasmaInnerFadeRadius) / Plasma.PlasmaInnerFadeRadius);

            EdgeScales = new RealQuaternion(outerScale, innerScale, plasmaOuterScale, plasmaInnerScale);
            EdgeOffsets = new RealQuaternion(outerOffset, innerOffset, plasmaOuterOffset, plasmaInnerOffset);
            PlasmaScales = new RealQuaternion(Plasma.TilingScale, Plasma.TilingScale, -(Plasma.CenterSharpness - Plasma.EdgeSharpness), Plasma.CenterSharpness);
            DepthFadeParameters = new RealQuaternion(1.0f / ShieldEdge.DepthFadeRange, 1.0f / Plasma.PlasmaDepthFadeRange);
        }

        /// <summary>
        /// Shield intensity is a combination of recent damage and current damage.
        /// These controls let you adjust the relative intensity contribution from each.
        /// 'shield_intensity' can be used as an input to any of the animation function curves below.
        /// </summary>
        [TagStructure(Size = 0x8)]
        public class ShieldIntensityBlock : TagStructure
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
        [TagStructure(Size = 0x4C, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x48, MinVersion = CacheVersion.HaloReach)]
        public class ShieldEdgeBlock : TagStructure
        {
            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public float OvershieldScale;
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
        [TagStructure(Size = 0xB0, MinVersion = CacheVersion.HaloOnlineED)]
        public class PlasmaBlock : TagStructure
        {
            public float PlasmaDepthFadeRange;      //In world units
            [TagField(ValidTags = new[] { "bitm" })]
            public CachedTag PlasmaNoiseBitmap1;
            [TagField(ValidTags = new[] { "bitm" })]
            public CachedTag PlasmaNoiseBitmap2;
            public float TilingScale;
            public float ScrollSpeed;
            public float EdgeSharpness;
            public float CenterSharpness;
            public float PlasmaOuterFadeRadius;     //Within [0,1]
            public float PlasmaCenterRadius;        //Within [0,1]
            public float PlasmaInnerFadeRadius;     //Within [0,1]
            public ShieldImpactFunction PlasmaCenterColor;
            public ShieldImpactFunction PlasmaCenterIntensity;
            public ShieldImpactFunction PlasmaEdgeColor;
            public ShieldImpactFunction PlasmaEdgeIntensity;
        }

        [TagStructure(Size = 0xA8, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0xA4, MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.Halo3Retail)]
        public class H3ValuesBlock : TagStructure
        {
            public CachedTag ShieldImpactNoiseTexture1;
            public CachedTag ShieldImpactNoiseTexture2;
            public float ExtrusionDistance;

            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public float TextureScale;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public RealVector2d TextureScaleUV;

            public float ScrollSpeed;
            public float PlasmaSharpness1;
            public float PlasmaScale1;
            public float PlasmaThreshold1;
            public float PlasmaSharpness2;
            public float PlasmaScale2;
            public float PlasmaThreshold2;
            public RealRgbColor OvershieldColor1;
            public float OvershieldIntensity1;
            public RealRgbColor OvershieldColor2;
            public float OvershieldIntensity2;
            public RealRgbColor OvershieldAmbientColor;
            public float OvershieldAmbientIntensity;
            public RealRgbColor ImpactColor1;
            public float ImpactIntensity1;
            public RealRgbColor ImpactColor2;
            public float ImpactIntensity2;
            public RealRgbColor ImpactAmbientColor;
            public float ImpactAmbientIntensity;
        }

        /// <summary>
        /// These controls allow you to define the oscillation in the extrusion.
        /// These textures are tiled and scrolled in the xy and yz planes, and their red channel is applied as an offset to the extrusion
        /// Tiling scale controls the spatial tiling of the plasma textures.
        /// Scroll speed controls how fast the textures scroll on the surface.
        /// You can specify separate sharpness values for the edge and the center.
        /// </summary>
        [TagStructure(Size = 0x60)]
        public class ExtrusionOscillationBlock : TagStructure
        {
            [TagField(ValidTags = new[] { "bitm" })]
            public CachedTag OscillationBitmap1;
            [TagField(ValidTags = new[] { "bitm" })]
            public CachedTag OscillationBitmap2;

            public float OscillationTilingScale;
            public float OscillationScrollSpeed;
            public ShieldImpactFunction ExtrusionAmount;
            public ShieldImpactFunction OscillationAmplitude;

            //HO is missing hit blob bitmap and either it's HitIntensity or HitRadius ShieldFunction, to be determined

            // <summary>
            // These controls allow you to define the color variation in the area surrounding projectile impacts.
            // You can control the color, and the size of the colored area.
            // The default input is time since impact.
            // </summary>
        }

        [TagStructure(Size = 0x3C)]
        public class HitResponseBlock : TagStructure
        {
            public float HitTime; // (seconds) time of the hit response
            public ShieldImpactFunction HitColor;
            public ShieldImpactFunction HitIntensity;
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
        Unused = 1 << 2,
        DontRenderAsEffect = 1 << 3 // HO only
    }

    // The following variables can be inputs to the functions, in addition to any object variables:
    // shield_vitality (percentage of shield remaining)
    // shield_intensity (mixture of recent and current damage)
    // current_shield_damage
    // recent_shield_damage
    [TagStructure(Size = 0x1C)]
    public class ShieldImpactFunction : TagStructure
    {
        public StringId InputVariable;
        public StringId RangeVariable;
        public TagFunction Function = new TagFunction { Data = new byte[0] };
    }
}