using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "lightning", Tag = "elec", Size = 0x108)]
    public class Lightning : TagStructure
    {
        [TagField(Length = 0x2)]
        public byte[] Padding;
        /// <summary>
        /// number of overlapping lightning effects to render; 0 defaults to 1
        /// </summary>
        public short Count;
        [TagField(Length = 0x10)]
        public byte[] Padding1;
        /// <summary>
        /// distance at which lightning is at full brightness
        /// </summary>
        public float NearFadeDistance; // world units
        /// <summary>
        /// distance at which lightning is at zero brightness
        /// </summary>
        public float FarFadeDistance; // world units
        [TagField(Length = 0x10)]
        public byte[] Padding2;
        public JitterScaleSourceValue JitterScaleSource;
        public ThicknessScaleSourceValue ThicknessScaleSource;
        public TintModulationSourceValue TintModulationSource;
        public BrightnessScaleSourceValue BrightnessScaleSource;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Bitmap;
        [TagField(Length = 0x54)]
        public byte[] Padding3;
        public List<LightningMarkerBlock> Markers;
        public List<LightningShaderBlock> Shader;
        [TagField(Length = 0x58)]
        public byte[] Padding4;
        
        public enum JitterScaleSourceValue : short
        {
            None,
            AOut,
            BOut,
            COut,
            DOut
        }
        
        public enum ThicknessScaleSourceValue : short
        {
            None,
            AOut,
            BOut,
            COut,
            DOut
        }
        
        public enum TintModulationSourceValue : short
        {
            None,
            A,
            B,
            C,
            D
        }
        
        public enum BrightnessScaleSourceValue : short
        {
            None,
            AOut,
            BOut,
            COut,
            DOut
        }
        
        [TagStructure(Size = 0xE4)]
        public class LightningMarkerBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string AttachmentMarker;
            public FlagsValue Flags;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            public short OctavesToNextMarker;
            [TagField(Length = 0x2)]
            public byte[] Padding1;
            [TagField(Length = 0x4C)]
            public byte[] Padding2;
            public RealVector3d RandomPositionBounds; // world units
            public float RandomJitter; // world units
            public float Thickness; // world units
            /// <summary>
            /// alpha is brightness
            /// </summary>
            public RealArgbColor Tint;
            [TagField(Length = 0x4C)]
            public byte[] Padding3;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                NotConnectedToNextMarker = 1 << 0
            }
        }
        
        [TagStructure(Size = 0xB4)]
        public class LightningShaderBlock : TagStructure
        {
            [TagField(Length = 0x28)]
            public byte[] Padding;
            public ShaderFlagsValue ShaderFlags;
            public FramebufferBlendFunctionValue FramebufferBlendFunction;
            public FramebufferFadeModeValue FramebufferFadeMode;
            public MapFlagsValue MapFlags;
            [TagField(Length = 0x1C)]
            public byte[] Padding1;
            [TagField(Length = 0x10)]
            public byte[] Padding2;
            [TagField(Length = 0x2)]
            public byte[] Padding3;
            [TagField(Length = 0x2)]
            public byte[] Padding4;
            [TagField(Length = 0x38)]
            public byte[] Padding5;
            [TagField(Length = 0x1C)]
            public byte[] Padding6;
            
            [Flags]
            public enum ShaderFlagsValue : ushort
            {
                SortBias = 1 << 0,
                NonlinearTint = 1 << 1,
                DonTOverdrawFpWeapon = 1 << 2
            }
            
            public enum FramebufferBlendFunctionValue : short
            {
                AlphaBlend,
                Multiply,
                DoubleMultiply,
                Add,
                Subtract,
                ComponentMin,
                ComponentMax,
                AlphaMultiplyAdd
            }
            
            public enum FramebufferFadeModeValue : short
            {
                None,
                FadeWhenPerpendicular,
                FadeWhenParallel
            }
            
            [Flags]
            public enum MapFlagsValue : ushort
            {
                Unfiltered = 1 << 0
            }
        }
    }
}

