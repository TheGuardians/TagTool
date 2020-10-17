using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "decal", Tag = "deca", Size = 0xAC)]
    public class Decal : TagStructure
    {
        /// <summary>
        /// There are several "layers" which decals can be placed into, these layers are drawn in a specific order relative to the
        /// shader layers and each layer has its own specific blending mode. In general, the decal bitmap's alpha channel will be
        /// used as an opacity mask if it exists.
        /// 
        /// * LIT ALPHA-BLEND PRELIGHT: Decals in this layer are lit by the lightmap but are
        /// "faded out" by dynamic lights. What this means is that dynamic lights hitting them will cause them to disappear, sort of.
        /// This layer is rendered immediately before lightmap shadows (and before dynamic lights).
        /// 
        /// * LIT ALPHA-BLEND: Decals in
        /// this layer are lit by the lightmap but are NOT lit by dynamic lights. What this means is that if the decal exists in an
        /// area that has dark lightmapping but bright dynamic lighting, the decal will appear dark. This layer is rendered
        /// immediately after dynamic lights, and all subsequent decal layers are rendered after this one in order.
        /// 
        /// * DOUBLE
        /// MULTIPLY: Decals in this layer will double-multiply the color in the framebuffer. Gray pixels in the decal bitmap will be
        /// transparent (black darkens, white brightens). The decal color in the tag does NOT do anything!!
        /// 
        /// * MULTIPLY: Decals in
        /// this layer will multiply the color in the framebuffer. White pixels in the decal bitmap will be transparent. The decal
        /// color (in the decal tag) does NOT do anything!!
        /// 
        /// * MAX: Decals in this layer will perform a component-wise max operation
        /// on the framebuffer, replacing color values with whichever is higher. Black pixels in the decal bitmap will be
        /// transparent.
        /// 
        /// * ADD: Decals in this layer will perform an addition operation on the framebuffer, replacing color values
        /// with the sum of the two. Black pixels in the decal bitmap will be transparent.
        /// 
        /// * ERROR: Decals in this layer will render
        /// bright red and show through geometry!
        /// 
        /// A compound decal is a chain of decals which are instantiated simultaneously.
        /// Compound decals are created by referencing another decal tag in the 'next_decal_in_chain' field below. Do not attempt to
        /// create a circularly linked decal chain, i.e. A->B->C->A! This will cause problems and probably hang the game. Also, do
        /// not reference a decal from an effect if it is not the head of the chain; for example an effect should not instantiate
        /// decal B if the chain was A->B->C. Compound decals can have seperate bitmaps, etc., and can be drawn in seperate layers.
        /// In addition, we used to have the ability for each decal in the chain can either inherit its parent's radius, rotation,
        /// color, etc. - or it can randomly choose its own. This behavior was controlled by the
        /// 'geometry_inherited_by_next_decal_in_chain' flag, below but it's currently broken.
        /// </summary>
        public FlagsValue Flags;
        /// <summary>
        /// controls how the decal wraps onto surface geometry
        /// </summary>
        public TypeValue Type;
        public LayerValue Layer;
        public short MaxOverlappingCount;
        [TagField(ValidTags = new [] { "deca" })]
        public CachedTag NextDecalInChain;
        /// <summary>
        /// 0 defaults to 0.125
        /// </summary>
        public Bounds<float> Radius; // world units
        public float RadiusOverlapRejection; // muliplier
        public RealRgbColor ColorLowerBounds;
        public RealRgbColor ColorUpperBounds;
        public Bounds<float> Lifetime; // seconds
        public Bounds<float> DecayTime; // seconds
        [TagField(Length = 0x28, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding4;
        [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
        public byte[] Padding5;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Bitmap;
        [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
        public byte[] Padding6;
        public float MaximumSpriteExtent; // pixels*
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding7;
        
        [Flags]
        public enum FlagsValue : ushort
        {
            GeometryInheritedByNextDecalInChain = 1 << 0,
            InterpolateColorInHsv = 1 << 1,
            MoreColors = 1 << 2,
            NoRandomRotation = 1 << 3,
            Unused = 1 << 4,
            SapienSnapToAxis = 1 << 5,
            SapienIncrementalCounter = 1 << 6,
            Unused1 = 1 << 7,
            PreserveAspect = 1 << 8,
            Unused2 = 1 << 9
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
            LitAlphaBlendPrelight,
            LitAlphaBlend,
            DoubleMultiply,
            Multiply,
            Max,
            Add,
            Error
        }
    }
}

