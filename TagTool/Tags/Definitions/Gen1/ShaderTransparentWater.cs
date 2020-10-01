using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "shader_transparent_water", Tag = "swat", Size = 0x140)]
    public class ShaderTransparentWater : TagStructure
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
        /// <summary>
        /// Base map color modulates the background, while base map alpha modulates reflection brightness. Both of these effects can
        /// be independently enables and disabled. Note that if the base map alpha modulates reflection flag is not set, then the
        /// perpendicular/parallel brightness has no effect (but the perpendicular/parallel tint color DOES has an effect).
        /// </summary>
        public Flags2Value Flags2;
        [TagField(Length = 0x2)]
        public byte[] Padding2;
        [TagField(Length = 0x20)]
        public byte[] Padding3;
        /// <summary>
        /// controls reflection brightness and background tint
        /// </summary>
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag BaseMap;
        [TagField(Length = 0x10)]
        public byte[] Padding4;
        public float ViewPerpendicularBrightness; // [0,1]
        public RealRgbColor ViewPerpendicularTintColor;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float ViewParallelBrightness; // [0,1]
        public RealRgbColor ViewParallelTintColor;
        [TagField(Length = 0x10)]
        public byte[] Padding5;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag ReflectionMap; // [0,1]
        [TagField(Length = 0x10)]
        public byte[] Padding6;
        public Angle RippleAnimationAngle; // [0,360]
        public float RippleAnimationVelocity;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float RippleScale;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag RippleMaps;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public short RippleMipmapLevels;
        [TagField(Length = 0x2)]
        public byte[] Padding7;
        /// <summary>
        /// flatness of last mipmap
        /// </summary>
        public float RippleMipmapFadeFactor; // [0,1]
        public float RippleMipmapDetailBias;
        [TagField(Length = 0x40)]
        public byte[] Padding8;
        public List<ShaderTransparentWaterRippleBlock> Ripples;
        [TagField(Length = 0x10)]
        public byte[] Padding9;
        
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
            BaseMapAlphaModulatesReflection,
            BaseMapColorModulatesBackground,
            AtmosphericFog,
            DrawBeforeFog
        }
        
        [TagStructure(Size = 0x4C)]
        public class ShaderTransparentWaterRippleBlock : TagStructure
        {
            [TagField(Length = 0x2)]
            public byte[] Padding;
            [TagField(Length = 0x2)]
            public byte[] Padding1;
            /// <summary>
            /// 0 defaults to 1
            /// </summary>
            public float ContributionFactor; // [0,1]
            [TagField(Length = 0x20)]
            public byte[] Padding2;
            public Angle AnimationAngle; // [0,360]
            public float AnimationVelocity;
            public RealVector2d MapOffset;
            /// <summary>
            /// 0 defaults to 1
            /// </summary>
            public short MapRepeats;
            /// <summary>
            /// index into ripple maps
            /// </summary>
            public short MapIndex;
            [TagField(Length = 0x10)]
            public byte[] Padding3;
        }
    }
}

