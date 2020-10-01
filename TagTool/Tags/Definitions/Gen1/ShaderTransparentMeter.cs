using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "shader_transparent_meter", Tag = "smet", Size = 0x104)]
    public class ShaderTransparentMeter : TagStructure
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
        [TagField(Length = 0x20)]
        public byte[] Padding3;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Map;
        [TagField(Length = 0x20)]
        public byte[] Padding4;
        public RealRgbColor GradientMinColor;
        public RealRgbColor GradientMaxColor;
        public RealRgbColor BackgroundColor;
        public RealRgbColor FlashColor;
        /// <summary>
        /// modulates framebuffer color unless map alpha is zero
        /// </summary>
        public RealRgbColor TintColor1;
        /// <summary>
        /// used only when 'tint mode-2' is set
        /// </summary>
        public float MeterTransparency; // [0,1]
        /// <summary>
        /// used only when 'tint mode-2' is set
        /// </summary>
        public float BackgroundTransparency; // [0,1]
        [TagField(Length = 0x18)]
        public byte[] Padding5;
        /// <summary>
        /// overall meter brightness (default is 1)
        /// </summary>
        public MeterBrightnessSourceValue MeterBrightnessSource;
        /// <summary>
        /// brightness of flash (default is 1)
        /// </summary>
        public FlashBrightnessSourceValue FlashBrightnessSource;
        /// <summary>
        /// position of flash leading edge (default is 1)
        /// </summary>
        public ValueSourceValue ValueSource;
        /// <summary>
        /// high color leading edge (default is 1)
        /// </summary>
        public GradientSourceValue GradientSource;
        /// <summary>
        /// position of flash extension leading edge (default is 1)
        /// </summary>
        public FlashExtensionSourceValue FlashExtensionSource;
        [TagField(Length = 0x2)]
        public byte[] Padding6;
        [TagField(Length = 0x20)]
        public byte[] Padding7;
        
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
            Decal,
            TwoSided,
            FlashColorIsNegative,
            TintMode2,
            Unfiltered
        }
        
        public enum MeterBrightnessSourceValue : short
        {
            None,
            AOut,
            BOut,
            COut,
            DOut
        }
        
        public enum FlashBrightnessSourceValue : short
        {
            None,
            AOut,
            BOut,
            COut,
            DOut
        }
        
        public enum ValueSourceValue : short
        {
            None,
            AOut,
            BOut,
            COut,
            DOut
        }
        
        public enum GradientSourceValue : short
        {
            None,
            AOut,
            BOut,
            COut,
            DOut
        }
        
        public enum FlashExtensionSourceValue : short
        {
            None,
            AOut,
            BOut,
            COut,
            DOut
        }
    }
}

