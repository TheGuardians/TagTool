using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "screen_effect", Tag = "egor", Size = 0x90)]
    public class ScreenEffect : TagStructure
    {
        /// <summary>
        /// A screen effect is essentially a collection of pass references, each one corresponding to a shader pass reference from
        /// the template. Note that only shader passes in the TRANSPARENT layer are considered during screen effect rendering.
        /// </summary>
        [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(ValidTags = new [] { "shad" })]
        public CachedTag Shader;
        [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public List<RasterizerScreenEffectPassReferenceBlock> PassReferences;
        
        [TagStructure(Size = 0xAC)]
        public class RasterizerScreenEffectPassReferenceBlock : TagStructure
        {
            public byte[] Explanation;
            /// <summary>
            /// Used to control which shader pass implementations are used depending on whether the primary and/or secondary external
            /// inputs are greater than zero. An implementation of -1 will not draw anything.
            /// </summary>
            public short LayerPassIndex; // leave as -1 unless debugging
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public sbyte Primary0AndSecondary0; // implementation index
            public sbyte Primary0AndSecondary01; // implementation index
            public sbyte Primary0AndSecondary02; // implementation index
            public sbyte Primary0AndSecondary03; // implementation index
            [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            /// <summary>
            /// * DEFAULT: Use for mask bitmaps that are mirrored and offset through the texcoords supplied to the screen effect (e.g.
            /// through the weapon HUD interface). The shader system must handle scaling.
            /// 
            /// * VIEWPORT NORMALIZED: Use when copying from
            /// texaccum or some other buffer or when applying a texture that is not an interface mask (e.g. another kind of mask, a
            /// noise map, etc. which is not mirrored or offset through the weapon HUD interface). Texture coordinates will range from
            /// [0,1][0,1] within the viewport, and the shader system must handle scaling if the bitmap is a linear target or an
            /// interface bitmap.
            /// 
            /// * VIEWPORT RELATIVE: Should not be used! This mode was necessary before we had he ability for the
            /// shader system to scale by texture resolution. Texture coordinates will range from [0,viewport_size_x][0,viewport_size_y]
            /// within the viewport.
            /// 
            /// * FRAMEBUFFER RELATIVE: Use when copying from the framebuffer. Texture coordinates will range from
            /// [viewport_bounds.x0,viewport_bounds.x1][viewport_bounds.y0,viewport_bounds.y1] within the viewport. Let the shader system
            /// assume normalized [0,1] coordinate range.
            /// 
            /// * ZERO: Use when doing dependent-z reads. Texture coordinates will be zero
            /// before applying offset (in advanced control block). Offset should be {1/(z_max-z_min), 0, -z_min/(z_max-z_min), 0} where
            /// z_min and z_max are in world units, and the "xy scaled by z_far" flag should be checked.
            /// </summary>
            public Stage0ModeValue Stage0Mode;
            public Stage1ModeValue Stage1Mode;
            public Stage2ModeValue Stage2Mode;
            public Stage3ModeValue Stage3Mode;
            public List<RasterizerScreenEffectTexcoordGenerationAdvancedControlBlock> AdvancedControl;
            public TargetValue Target;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            public List<RasterizerScreenEffectConvolutionBlock> Convolution;
            
            public enum Stage0ModeValue : short
            {
                Default,
                ViewportNormalized,
                ViewportRelative,
                FramebufferRelative,
                Zero
            }
            
            public enum Stage1ModeValue : short
            {
                Default,
                ViewportNormalized,
                ViewportRelative,
                FramebufferRelative,
                Zero
            }
            
            public enum Stage2ModeValue : short
            {
                Default,
                ViewportNormalized,
                ViewportRelative,
                FramebufferRelative,
                Zero
            }
            
            public enum Stage3ModeValue : short
            {
                Default,
                ViewportNormalized,
                ViewportRelative,
                FramebufferRelative,
                Zero
            }
            
            [TagStructure(Size = 0x48)]
            public class RasterizerScreenEffectTexcoordGenerationAdvancedControlBlock : TagStructure
            {
                public Stage0FlagsValue Stage0Flags;
                public Stage1FlagsValue Stage1Flags;
                public Stage2FlagsValue Stage2Flags;
                public Stage3FlagsValue Stage3Flags;
                public RealPlane3d Stage0Offset;
                public RealPlane3d Stage1Offset;
                public RealPlane3d Stage2Offset;
                public RealPlane3d Stage3Offset;
                
                [Flags]
                public enum Stage0FlagsValue : ushort
                {
                    XyScaledByZFar = 1 << 0
                }
                
                [Flags]
                public enum Stage1FlagsValue : ushort
                {
                    XyScaledByZFar = 1 << 0
                }
                
                [Flags]
                public enum Stage2FlagsValue : ushort
                {
                    XyScaledByZFar = 1 << 0
                }
                
                [Flags]
                public enum Stage3FlagsValue : ushort
                {
                    XyScaledByZFar = 1 << 0
                }
            }
            
            public enum TargetValue : short
            {
                Framebuffer,
                Texaccum,
                TexaccumSmall,
                TexaccumTiny,
                CopyFbToTexaccum
            }
            
            [TagStructure(Size = 0x5C)]
            public class RasterizerScreenEffectConvolutionBlock : TagStructure
            {
                /// <summary>
                /// Convolution blurs the target of this pass reference to the SCREEN EFFECT CONVOLUTION buffer, which is accessible in the
                /// shader system through a texture extern. It is not cheap especially for large convolution amounts, so please use it
                /// sparingly!! Note that the resolution will be clamped to 64x64 minimum and 768000 pixels total maximum.
                /// </summary>
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public float ConvolutionAmount; // [0,+inf)
                public float FilterScale;
                public float FilterBoxFactor; // [0,1] not used for zoom
                public float ZoomFalloffRadius;
                public float ZoomCutoffRadius;
                public float ResolutionScale; // [0,1]
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    OnlyWhenPrimaryIsActive = 1 << 0,
                    OnlyWhenSecondaryIsActive = 1 << 1,
                    PredatorZoom = 1 << 2
                }
            }
        }
    }
}

