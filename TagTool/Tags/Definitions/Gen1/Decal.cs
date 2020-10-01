using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "decal", Tag = "deca", Size = 0x10C)]
    public class Decal : TagStructure
    {
        /// <summary>
        /// A 'compound decal' is a chain of decals which are instantiated simultaneously. Compound decals are created by choosing a
        /// next_decal_in_chain below. NOTE: Do not attempt to create a circularly linked decal chain, i.e. A-B-C-A! Also, do
        /// not reference a decal from an effect if it is not the 'head' of the chain; for example an effect should not instantiate
        /// decal B if the chain was A-B-C. Compound decals can have seperate bitmaps, seperate framebuffer blend functions, and
        /// can be drawn in seperate layers. In addition, each decal in the chain can either inherit its parent's radius, rotation,
        /// color, fade, and sequence - or it can randomly choose its own. This behavior is controlled by the
        /// 'geometry_inherited_by_next_decal_in_chain' flag, below. 
        /// 
        /// The decal type (or layer) determines the drawing order of
        /// the decal with respect to the rest of the environment. Decals in the primary layer are drawn after the environment
        /// diffuse texture, hence they affect the already-lit texture of the surface. Decals in the secondary layer are drawn
        /// immediately after decals in the primary layer, so they 'cover up' the primary decals. Decals in the 'light' layer are
        /// drawn before the environment diffuse texture, hence they affect the accumulated diffuse light and only indirectly affect
        /// the lit texture.
        /// </summary>
        public FlagsValue Flags;
        /// <summary>
        /// controls how the decal wraps onto surface geometry
        /// </summary>
        public TypeValue Type;
        public LayerValue Layer;
        [TagField(Length = 0x2)]
        public byte[] Padding;
        [TagField(ValidTags = new [] { "deca" })]
        public CachedTag NextDecalInChain;
        /// <summary>
        /// 0 defaults to 0.125
        /// </summary>
        public Bounds<float> Radius; // world units
        [TagField(Length = 0xC)]
        public byte[] Padding1;
        /// <summary>
        /// 1 is fully visible, 0 is invisible
        /// </summary>
        public Bounds<float> Intensity; // [0,1]
        public RealRgbColor ColorLowerBounds;
        public RealRgbColor ColorUpperBounds;
        [TagField(Length = 0xC)]
        public byte[] Padding2;
        public short AnimationLoopFrame;
        public short AnimationSpeed; // [1,120] ticks per frame
        [TagField(Length = 0x1C)]
        public byte[] Padding3;
        public Bounds<float> Lifetime; // seconds
        public Bounds<float> DecayTime; // seconds
        [TagField(Length = 0xC)]
        public byte[] Padding4;
        [TagField(Length = 0x28)]
        public byte[] Padding5;
        [TagField(Length = 0x2)]
        public byte[] Padding6;
        [TagField(Length = 0x2)]
        public byte[] Padding7;
        public FramebufferBlendFunctionValue FramebufferBlendFunction;
        [TagField(Length = 0x2)]
        public byte[] Padding8;
        [TagField(Length = 0x14)]
        public byte[] Padding9;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Map;
        [TagField(Length = 0x14)]
        public byte[] Padding10;
        public float MaximumSpriteExtent; // pixels*
        [TagField(Length = 0x4)]
        public byte[] Padding11;
        [TagField(Length = 0x8)]
        public byte[] Padding12;
        
        public enum FlagsValue : ushort
        {
            GeometryInheritedByNextDecalInChain,
            InterpolateColorInHsv,
            MoreColors,
            NoRandomRotation,
            WaterEffect,
            SapienSnapToAxis,
            SapienIncrementalCounter,
            AnimationLoop,
            PreserveAspect
        }
        
        public enum TypeValue : short
        {
            Scratch,
            Splatter,
            Burn,
            PaintedSign
        }
        
        public enum LayerValue : short
        {
            Primary,
            Secondary,
            Light,
            AlphaTested,
            Water
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
    }
}

