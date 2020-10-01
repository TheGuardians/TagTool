using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "shader_transparent_glass", Tag = "sgla", Size = 0x1E0)]
    public class ShaderTransparentGlass : TagStructure
    {
        public FlagsValue Flags;
        /// <summary>
        /// affects the density of tesselation (high means slow).
        /// </summary>
        public DetailLevelValue DetailLevel;
        /// <summary>
        /// power of emitted light from 0 to infinity
        /// </summary>
        public float Power;
        public RealRgbColor ColorOfEmittedLight;
        /// <summary>
        /// light passing through this surface (if it's transparent) will be tinted this color.
        /// </summary>
        public RealRgbColor TintColor;
        public Flags1Value Flags1;
        public MaterialTypeValue MaterialType;
        [TagField(Length = 0x2)]
        public byte[] Padding;
        [TagField(Length = 0x2)]
        public byte[] Padding1;
        public Flags2Value Flags2;
        [TagField(Length = 0x2)]
        public byte[] Padding2;
        /// <summary>
        /// Background pixels are multiplied by the tint map and constant tint color.
        /// </summary>
        [TagField(Length = 0x28)]
        public byte[] Padding3;
        public RealRgbColor BackgroundTintColor;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float BackgroundTintMapScale;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag BackgroundTintMap;
        [TagField(Length = 0x14)]
        public byte[] Padding4;
        /// <summary>
        /// Reflection maps are multiplied by fresnel terms (glancing angles cause reflections to disappear) and then added to the
        /// background. The primary reflection map is textured normally, while the secondary reflection map is magnified.
        /// </summary>
        [TagField(Length = 0x2)]
        public byte[] Padding5;
        public ReflectionTypeValue ReflectionType;
        public float PerpendicularBrightness; // [0,1]
        public RealRgbColor PerpendicularTintColor;
        public float ParallelBrightness; // [0,1]
        public RealRgbColor ParallelTintColor;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag ReflectionMap;
        public float BumpMapScale;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag BumpMap;
        [TagField(Length = 0x80)]
        public byte[] Padding6;
        /// <summary>
        /// Diffuse lights are accumulated in monochrome and then alpha-blended with diffuse map and diffuse detail map. The color is
        /// determined by double-multiplying both maps and multiplying with the accumulated light, the result being alpha-blended
        /// into the framebuffer. The opacity is determined by multiplying both map's alpha channels. Since this effect is
        /// alpha-blended, it covers up tinting and reflection on pixels with high opacity.
        /// </summary>
        [TagField(Length = 0x4)]
        public byte[] Padding7;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float DiffuseMapScale;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag DiffuseMap;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float DiffuseDetailMapScale;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag DiffuseDetailMap;
        [TagField(Length = 0x1C)]
        public byte[] Padding8;
        /// <summary>
        /// Specular lights are accumulated in monochrome and then alpha-blended with diffuse map and diffuse detail map. The color
        /// is determined by double-multiplying both maps and multiplying with the accumulated light, the result being alpha-blended
        /// into the framebuffer. The opacity is determined by multiplying both map's alpha channels. Since this effect is
        /// alpha-blended, it covers up tinting, reflection and diffuse texture on pixels with high opacity.
        /// </summary>
        [TagField(Length = 0x4)]
        public byte[] Padding9;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float SpecularMapScale;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag SpecularMap;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float SpecularDetailMapScale;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag SpecularDetailMap;
        [TagField(Length = 0x1C)]
        public byte[] Padding10;
        
        public enum FlagsValue : ushort
        {
            /// <summary>
            /// lightmap texture parametrization should correspond to diffuse texture parametrization
            /// </summary>
            SimpleParameterization,
            /// <summary>
            /// light independent of normals (trees)
            /// </summary>
            IgnoreNormals,
            TransparentLit
        }
        
        public enum DetailLevelValue : short
        {
            High,
            Medium,
            Low,
            Turd
        }
        
        public enum Flags1Value : ushort
        {
        }
        
        public enum MaterialTypeValue : short
        {
            Dirt,
            Sand,
            Stone,
            Snow,
            Wood,
            MetalHollow,
            MetalThin,
            MetalThick,
            Rubber,
            Glass,
            ForceField,
            Grunt,
            HunterArmor,
            HunterSkin,
            Elite,
            Jackal,
            JackalEnergyShield,
            EngineerSkin,
            EngineerForceField,
            FloodCombatForm,
            FloodCarrierForm,
            CyborgArmor,
            CyborgEnergyShield,
            HumanArmor,
            HumanSkin,
            Sentinel,
            Monitor,
            Plastic,
            Water,
            Leaves,
            EliteEnergyShield,
            Ice,
            HunterShield
        }
        
        public enum Flags2Value : ushort
        {
            AlphaTested,
            Decal,
            TwoSided,
            BumpMapIsSpecularMask
        }
        
        public enum ReflectionTypeValue : short
        {
            BumpedCubeMap,
            FlatCubeMap,
            DynamicMirror
        }
    }
}

