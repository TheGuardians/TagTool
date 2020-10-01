using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "shader_transparent_plasma", Tag = "spla", Size = 0x14C)]
    public class ShaderTransparentPlasma : TagStructure
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
        [TagField(Length = 0x2)]
        public byte[] Padding2;
        [TagField(Length = 0x2)]
        public byte[] Padding3;
        /// <summary>
        /// Controls how bright the plasma is.
        /// </summary>
        public IntensitySourceValue IntensitySource;
        [TagField(Length = 0x2)]
        public byte[] Padding4;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float IntensityExponent;
        /// <summary>
        /// Controls how far the plasma energy extends from the model geometry.
        /// </summary>
        public OffsetSourceValue OffsetSource;
        [TagField(Length = 0x2)]
        public byte[] Padding5;
        public float OffsetAmount; // world units
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float OffsetExponent;
        [TagField(Length = 0x20)]
        public byte[] Padding6;
        /// <summary>
        /// Controls the tint color and Fresnel brightness effects.
        /// </summary>
        public float PerpendicularBrightness; // [0,1]
        public RealRgbColor PerpendicularTintColor;
        public float ParallelBrightness; // [0,1]
        public RealRgbColor ParallelTintColor;
        /// <summary>
        /// modulates perpendicular and parallel colors above
        /// </summary>
        public TintColorSourceValue TintColorSource;
        [TagField(Length = 0x2)]
        public byte[] Padding7;
        [TagField(Length = 0x20)]
        public byte[] Padding8;
        [TagField(Length = 0x2)]
        public byte[] Padding9;
        [TagField(Length = 0x2)]
        public byte[] Padding10;
        [TagField(Length = 0x10)]
        public byte[] Padding11;
        [TagField(Length = 0x4)]
        public byte[] Padding12;
        [TagField(Length = 0x4)]
        public byte[] Padding13;
        public float PrimaryAnimationPeriod; // seconds
        public RealVector3d PrimaryAnimationDirection;
        public float PrimaryNoiseMapScale;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag PrimaryNoiseMap;
        [TagField(Length = 0x20)]
        public byte[] Padding14;
        [TagField(Length = 0x4)]
        public byte[] Padding15;
        public float SecondaryAnimationPeriod; // seconds
        public RealVector3d SecondaryAnimationDirection;
        public float SecondaryNoiseMapScale;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag SecondaryNoiseMap;
        [TagField(Length = 0x20)]
        public byte[] Padding16;
        
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
        
        public enum IntensitySourceValue : short
        {
            None,
            AOut,
            BOut,
            COut,
            DOut
        }
        
        public enum OffsetSourceValue : short
        {
            None,
            AOut,
            BOut,
            COut,
            DOut
        }
        
        public enum TintColorSourceValue : short
        {
            None,
            A,
            B,
            C,
            D
        }
    }
}

