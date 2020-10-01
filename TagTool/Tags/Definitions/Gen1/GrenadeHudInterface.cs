using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "grenade_hud_interface", Tag = "grhi", Size = 0x1F8)]
    public class GrenadeHudInterface : TagStructure
    {
        public AnchorValue Anchor;
        [TagField(Length = 0x2)]
        public byte[] Padding;
        [TagField(Length = 0x20)]
        public byte[] Padding1;
        public Point2d AnchorOffset;
        public float WidthScale;
        public float HeightScale;
        public ScalingFlagsValue ScalingFlags;
        [TagField(Length = 0x2)]
        public byte[] Padding2;
        [TagField(Length = 0x14)]
        public byte[] Padding3;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag InterfaceBitmap;
        public ArgbColor DefaultColor;
        public ArgbColor FlashingColor;
        public float FlashPeriod;
        public float FlashDelay;
        public short NumberOfFlashes;
        public FlashFlagsValue FlashFlags;
        public float FlashLength;
        public ArgbColor DisabledColor;
        [TagField(Length = 0x4)]
        public byte[] Padding4;
        public short SequenceIndex;
        [TagField(Length = 0x2)]
        public byte[] Padding5;
        public List<GlobalHudMultitextureOverlayDefinition> MultitexOverlay;
        [TagField(Length = 0x4)]
        public byte[] Padding6;
        public Point2d AnchorOffset1;
        public float WidthScale1;
        public float HeightScale1;
        public ScalingFlags1Value ScalingFlags1;
        [TagField(Length = 0x2)]
        public byte[] Padding7;
        [TagField(Length = 0x14)]
        public byte[] Padding8;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag InterfaceBitmap1;
        public ArgbColor DefaultColor1;
        public ArgbColor FlashingColor1;
        public float FlashPeriod1;
        public float FlashDelay1;
        public short NumberOfFlashes1;
        public FlashFlags1Value FlashFlags1;
        public float FlashLength1;
        public ArgbColor DisabledColor1;
        [TagField(Length = 0x4)]
        public byte[] Padding9;
        public short SequenceIndex1;
        [TagField(Length = 0x2)]
        public byte[] Padding10;
        public List<GlobalHudMultitextureOverlayDefinition> MultitexOverlay1;
        [TagField(Length = 0x4)]
        public byte[] Padding11;
        public Point2d AnchorOffset2;
        public float WidthScale2;
        public float HeightScale2;
        public ScalingFlags2Value ScalingFlags2;
        [TagField(Length = 0x2)]
        public byte[] Padding12;
        [TagField(Length = 0x14)]
        public byte[] Padding13;
        public ArgbColor DefaultColor2;
        public ArgbColor FlashingColor2;
        public float FlashPeriod2;
        public float FlashDelay2;
        public short NumberOfFlashes2;
        public FlashFlags2Value FlashFlags2;
        public float FlashLength2;
        public ArgbColor DisabledColor2;
        [TagField(Length = 0x4)]
        public byte[] Padding14;
        public sbyte MaximumNumberOfDigits;
        public FlagsValue Flags;
        public sbyte NumberOfFractionalDigits;
        [TagField(Length = 0x1)]
        public byte[] Padding15;
        [TagField(Length = 0xC)]
        public byte[] Padding16;
        public short FlashCutoff;
        [TagField(Length = 0x2)]
        public byte[] Padding17;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag OverlayBitmap;
        public List<GrenadeHudOverlayBlock> Overlays;
        public List<GrenadeHudSoundBlock> WarningSounds;
        [TagField(Length = 0x44)]
        public byte[] Padding18;
        /// <summary>
        /// sequence index into the global hud icon bitmap
        /// </summary>
        public short SequenceIndex2;
        /// <summary>
        /// extra spacing beyond bitmap width for text alignment
        /// </summary>
        public short WidthOffset;
        public Point2d OffsetFromReferenceCorner;
        public ArgbColor OverrideIconColor;
        public sbyte FrameRate030;
        public Flags1Value Flags1;
        public short TextIndex;
        [TagField(Length = 0x30)]
        public byte[] Padding19;
        
        public enum AnchorValue : short
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            Center
        }
        
        [Flags]
        public enum ScalingFlagsValue : ushort
        {
            DonTScaleOffset = 1 << 0,
            DonTScaleSize = 1 << 1,
            UseHighResScale = 1 << 2
        }
        
        [Flags]
        public enum FlashFlagsValue : ushort
        {
            ReverseDefaultFlashingColors = 1 << 0
        }
        
        [TagStructure(Size = 0x1E0)]
        public class GlobalHudMultitextureOverlayDefinition : TagStructure
        {
            [TagField(Length = 0x2)]
            public byte[] Padding;
            public short Type;
            public FramebufferBlendFuncValue FramebufferBlendFunc;
            [TagField(Length = 0x2)]
            public byte[] Padding1;
            [TagField(Length = 0x20)]
            public byte[] Padding2;
            /// <summary>
            /// where you want the origin of the texture.
            /// *"texture" uses the texture coordinates supplied
            /// *"screen" uses the origin of
            /// the screen as the origin of the texture
            /// </summary>
            public PrimaryAnchorValue PrimaryAnchor;
            public SecondaryAnchorValue SecondaryAnchor;
            public TertiaryAnchorValue TertiaryAnchor;
            /// <summary>
            /// how to blend the textures together
            /// </summary>
            public _0To1BlendFuncValue _0To1BlendFunc;
            public _1To2BlendFuncValue _1To2BlendFunc;
            [TagField(Length = 0x2)]
            public byte[] Padding3;
            /// <summary>
            /// how much to scale the textures
            /// </summary>
            public RealPoint2d PrimaryScale;
            public RealPoint2d SecondaryScale;
            public RealPoint2d TertiaryScale;
            /// <summary>
            /// how much to offset the origin of the texture
            /// </summary>
            public RealPoint2d PrimaryOffset;
            public RealPoint2d SecondaryOffset;
            public RealPoint2d TertiaryOffset;
            /// <summary>
            /// which maps to use
            /// </summary>
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Primary;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Secondary;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Tertiary;
            public PrimaryWrapModeValue PrimaryWrapMode;
            public SecondaryWrapModeValue SecondaryWrapMode;
            public TertiaryWrapModeValue TertiaryWrapMode;
            [TagField(Length = 0x2)]
            public byte[] Padding4;
            [TagField(Length = 0xB8)]
            public byte[] Padding5;
            public List<GlobalHudMultitextureOverlayEffectorDefinition> Effectors;
            [TagField(Length = 0x80)]
            public byte[] Padding6;
            
            public enum FramebufferBlendFuncValue : short
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
            
            public enum PrimaryAnchorValue : short
            {
                Texture,
                Screen
            }
            
            public enum SecondaryAnchorValue : short
            {
                Texture,
                Screen
            }
            
            public enum TertiaryAnchorValue : short
            {
                Texture,
                Screen
            }
            
            public enum _0To1BlendFuncValue : short
            {
                Add,
                Subtract,
                Multiply,
                Multiply2x,
                Dot
            }
            
            public enum _1To2BlendFuncValue : short
            {
                Add,
                Subtract,
                Multiply,
                Multiply2x,
                Dot
            }
            
            public enum PrimaryWrapModeValue : short
            {
                Clamp,
                Wrap
            }
            
            public enum SecondaryWrapModeValue : short
            {
                Clamp,
                Wrap
            }
            
            public enum TertiaryWrapModeValue : short
            {
                Clamp,
                Wrap
            }
            
            [TagStructure(Size = 0xDC)]
            public class GlobalHudMultitextureOverlayEffectorDefinition : TagStructure
            {
                [TagField(Length = 0x40)]
                public byte[] Padding;
                /// <summary>
                /// These describe the relationship that causes the effect.
                /// * destination type is the type of variable you want to be
                /// effected
                /// * destination tells which texture map (or geometry offset) to apply it to
                /// * source says which value to look at
                /// when computing the effect
                /// </summary>
                public DestinationTypeValue DestinationType;
                public DestinationValue Destination;
                public SourceValue Source;
                [TagField(Length = 0x2)]
                public byte[] Padding1;
                /// <summary>
                /// When the source is at the lower inbound, the destination ends up the lower outbound and vice-versa applies for the upper
                /// values.
                /// </summary>
                public Bounds<float> InBounds; // source units
                public Bounds<float> OutBounds; // pixels
                [TagField(Length = 0x40)]
                public byte[] Padding2;
                /// <summary>
                /// If destination is tint, these values are used instead of the out bounds.
                /// </summary>
                public RealRgbColor TintColorLowerBound;
                public RealRgbColor TintColorUpperBound;
                /// <summary>
                /// If you use a periodic function as the source, this lets you tweak it.
                /// </summary>
                public PeriodicFunctionValue PeriodicFunction;
                [TagField(Length = 0x2)]
                public byte[] Padding3;
                public float FunctionPeriod; // seconds
                public float FunctionPhase; // seconds
                [TagField(Length = 0x20)]
                public byte[] Padding4;
                
                public enum DestinationTypeValue : short
                {
                    Tint01,
                    HorizontalOffset,
                    VerticalOffset,
                    Fade01
                }
                
                public enum DestinationValue : short
                {
                    GeometryOffset,
                    PrimaryMap,
                    SecondaryMap,
                    TertiaryMap
                }
                
                public enum SourceValue : short
                {
                    PlayerPitch,
                    PlayerPitchTangent,
                    PlayerYaw,
                    WeaponAmmoTotal,
                    WeaponAmmoLoaded,
                    WeaponHeat,
                    ExplicitUsesLowBound,
                    WeaponZoomLevel
                }
                
                public enum PeriodicFunctionValue : short
                {
                    One,
                    Zero,
                    Cosine,
                    CosineVariablePeriod,
                    DiagonalWave,
                    DiagonalWaveVariablePeriod,
                    Slide,
                    SlideVariablePeriod,
                    Noise,
                    Jitter,
                    Wander,
                    Spark
                }
            }
        }
        
        [Flags]
        public enum ScalingFlags1Value : ushort
        {
            DonTScaleOffset = 1 << 0,
            DonTScaleSize = 1 << 1,
            UseHighResScale = 1 << 2
        }
        
        [Flags]
        public enum FlashFlags1Value : ushort
        {
            ReverseDefaultFlashingColors = 1 << 0
        }
        
        [Flags]
        public enum ScalingFlags2Value : ushort
        {
            DonTScaleOffset = 1 << 0,
            DonTScaleSize = 1 << 1,
            UseHighResScale = 1 << 2
        }
        
        [Flags]
        public enum FlashFlags2Value : ushort
        {
            ReverseDefaultFlashingColors = 1 << 0
        }
        
        [Flags]
        public enum FlagsValue : byte
        {
            ShowLeadingZeros = 1 << 0,
            OnlyShowWhenZoomed = 1 << 1,
            DrawATrailingM = 1 << 2
        }
        
        [TagStructure(Size = 0x88)]
        public class GrenadeHudOverlayBlock : TagStructure
        {
            public Point2d AnchorOffset;
            public float WidthScale;
            public float HeightScale;
            public ScalingFlagsValue ScalingFlags;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            [TagField(Length = 0x14)]
            public byte[] Padding1;
            public ArgbColor DefaultColor;
            public ArgbColor FlashingColor;
            public float FlashPeriod;
            public float FlashDelay;
            public short NumberOfFlashes;
            public FlashFlagsValue FlashFlags;
            public float FlashLength;
            public ArgbColor DisabledColor;
            [TagField(Length = 0x4)]
            public byte[] Padding2;
            public float FrameRate;
            public short SequenceIndex;
            public TypeValue Type;
            public FlagsValue Flags;
            [TagField(Length = 0x10)]
            public byte[] Padding3;
            [TagField(Length = 0x28)]
            public byte[] Padding4;
            
            [Flags]
            public enum ScalingFlagsValue : ushort
            {
                DonTScaleOffset = 1 << 0,
                DonTScaleSize = 1 << 1,
                UseHighResScale = 1 << 2
            }
            
            [Flags]
            public enum FlashFlagsValue : ushort
            {
                ReverseDefaultFlashingColors = 1 << 0
            }
            
            [Flags]
            public enum TypeValue : ushort
            {
                ShowOnFlashing = 1 << 0,
                ShowOnEmpty = 1 << 1,
                ShowOnDefault = 1 << 2,
                ShowAlways = 1 << 3
            }
            
            [Flags]
            public enum FlagsValue : uint
            {
                FlashesWhenActive = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x38)]
        public class GrenadeHudSoundBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "snd!","lsnd" })]
            public CachedTag Sound;
            public LatchedToValue LatchedTo;
            public float Scale;
            [TagField(Length = 0x20)]
            public byte[] Padding;
            
            [Flags]
            public enum LatchedToValue : uint
            {
                LowGrenadeCount = 1 << 0,
                NoGrenadesLeft = 1 << 1,
                ThrowOnNoGrenades = 1 << 2
            }
        }
        
        [Flags]
        public enum Flags1Value : byte
        {
            UseTextFromStringListInstead = 1 << 0,
            OverrideDefaultColor = 1 << 1,
            WidthOffsetIsAbsoluteIconWidth = 1 << 2
        }
    }
}

