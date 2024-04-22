using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "unit_hud_interface", Tag = "unhi", Size = 0x514)]
    public class UnitHudInterface : TagStructure
    {
        public AnchorValue Anchor;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public Point2d AnchorOffset;
        public float WidthScale;
        public float HeightScale;
        public ScalingFlagsValue ScalingFlags;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag InterfaceBitmap;
        public ArgbColor DefaultColor;
        public ArgbColor FlashingColor;
        public float FlashPeriod;
        /// <summary>
        /// time between flashes
        /// </summary>
        public float FlashDelay;
        public short NumberOfFlashes;
        public FlashFlagsValue FlashFlags;
        /// <summary>
        /// time of each flash
        /// </summary>
        public float FlashLength;
        public ArgbColor DisabledColor;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding4;
        public short SequenceIndex;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding5;
        public List<GlobalHudMultitextureOverlayDefinition> MultitexOverlay;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding6;
        public Point2d AnchorOffset1;
        public float WidthScale1;
        public float HeightScale1;
        public ScalingFlagsValue1 ScalingFlags1;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding7;
        [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
        public byte[] Padding8;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag InterfaceBitmap1;
        public ArgbColor DefaultColor1;
        public ArgbColor FlashingColor1;
        public float FlashPeriod1;
        /// <summary>
        /// time between flashes
        /// </summary>
        public float FlashDelay1;
        public short NumberOfFlashes1;
        public FlashFlagsValue1 FlashFlags1;
        /// <summary>
        /// time of each flash
        /// </summary>
        public float FlashLength1;
        public ArgbColor DisabledColor1;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding9;
        public short SequenceIndex1;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding10;
        public List<GlobalHudMultitextureOverlayDefinition1> MultitexOverlay1;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding11;
        public Point2d AnchorOffset2;
        public float WidthScale2;
        public float HeightScale2;
        public ScalingFlagsValue2 ScalingFlags2;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding12;
        [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
        public byte[] Padding13;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag MeterBitmap;
        public ArgbColor ColorAtMeterMinimum;
        public ArgbColor ColorAtMeterMaximum;
        public ArgbColor FlashColor;
        public ArgbColor EmptyColor;
        public FlagsValue Flags;
        public sbyte MinumumMeterValue;
        public short SequenceIndex2;
        public sbyte AlphaMultiplier;
        public sbyte AlphaBias;
        /// <summary>
        /// used for non-integral values, i.e. health and shields
        /// </summary>
        public short ValueScale;
        public float Opacity;
        public float Translucency;
        public ArgbColor DisabledColor2;
        public List<GNullBlock> Unknown;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding14;
        public ArgbColor OverchargeMinimumColor;
        public ArgbColor OverchargeMaximumColor;
        public ArgbColor OverchargeFlashColor;
        public ArgbColor OverchargeEmptyColor;
        [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
        public byte[] Padding15;
        public Point2d AnchorOffset3;
        public float WidthScale3;
        public float HeightScale3;
        public ScalingFlagsValue3 ScalingFlags3;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding16;
        [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
        public byte[] Padding17;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag InterfaceBitmap2;
        public ArgbColor DefaultColor2;
        public ArgbColor FlashingColor2;
        public float FlashPeriod2;
        /// <summary>
        /// time between flashes
        /// </summary>
        public float FlashDelay2;
        public short NumberOfFlashes2;
        public FlashFlagsValue2 FlashFlags2;
        /// <summary>
        /// time of each flash
        /// </summary>
        public float FlashLength2;
        public ArgbColor DisabledColor3;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding18;
        public short SequenceIndex3;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding19;
        public List<GlobalHudMultitextureOverlayDefinition2> MultitexOverlay2;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding20;
        public Point2d AnchorOffset4;
        public float WidthScale4;
        public float HeightScale4;
        public ScalingFlagsValue4 ScalingFlags4;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding21;
        [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
        public byte[] Padding22;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag MeterBitmap1;
        public ArgbColor ColorAtMeterMinimum1;
        public ArgbColor ColorAtMeterMaximum1;
        public ArgbColor FlashColor1;
        public ArgbColor EmptyColor1;
        public FlagsValue1 Flags1;
        public sbyte MinumumMeterValue1;
        public short SequenceIndex4;
        public sbyte AlphaMultiplier1;
        public sbyte AlphaBias1;
        /// <summary>
        /// used for non-integral values, i.e. health and shields
        /// </summary>
        public short ValueScale1;
        public float Opacity1;
        public float Translucency1;
        public ArgbColor DisabledColor4;
        public List<GNullBlock1> Unknown1;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding23;
        public ArgbColor MediumHealthLeftColor;
        public float MaxColorHealthFractionCutoff;
        public float MinColorHealthFractionCutoff;
        [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
        public byte[] Padding24;
        public Point2d AnchorOffset5;
        public float WidthScale5;
        public float HeightScale5;
        public ScalingFlagsValue5 ScalingFlags5;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding25;
        [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
        public byte[] Padding26;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag InterfaceBitmap3;
        public ArgbColor DefaultColor3;
        public ArgbColor FlashingColor3;
        public float FlashPeriod3;
        /// <summary>
        /// time between flashes
        /// </summary>
        public float FlashDelay3;
        public short NumberOfFlashes3;
        public FlashFlagsValue3 FlashFlags3;
        /// <summary>
        /// time of each flash
        /// </summary>
        public float FlashLength3;
        public ArgbColor DisabledColor5;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding27;
        public short SequenceIndex5;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding28;
        public List<GlobalHudMultitextureOverlayDefinition3> MultitexOverlay3;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding29;
        public Point2d AnchorOffset6;
        public float WidthScale6;
        public float HeightScale6;
        public ScalingFlagsValue6 ScalingFlags6;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding30;
        [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
        public byte[] Padding31;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag InterfaceBitmap4;
        public ArgbColor DefaultColor4;
        public ArgbColor FlashingColor4;
        public float FlashPeriod4;
        /// <summary>
        /// time between flashes
        /// </summary>
        public float FlashDelay4;
        public short NumberOfFlashes4;
        public FlashFlagsValue4 FlashFlags4;
        /// <summary>
        /// time of each flash
        /// </summary>
        public float FlashLength4;
        public ArgbColor DisabledColor6;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding32;
        public short SequenceIndex6;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding33;
        public List<GlobalHudMultitextureOverlayDefinition4> MultitexOverlay4;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding34;
        [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
        public byte[] Padding35;
        /// <summary>
        /// The blips use this as a reference point
        /// </summary>
        public Point2d AnchorOffset7;
        public float WidthScale7;
        public float HeightScale7;
        public ScalingFlagsValue7 ScalingFlags7;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding36;
        [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
        public byte[] Padding37;
        public AnchorValue1 Anchor1;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding38;
        [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
        public byte[] Padding39;
        public List<UnitHudAuxilaryOverlayBlock> Overlays;
        [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
        public byte[] Padding40;
        public List<UnitHudSoundBlock> Sounds;
        public List<UnitHudAuxilaryPanelBlock> Meters;
        [TagField(ValidTags = new [] { "nhdt" })]
        public CachedTag NewHud;
        [TagField(Length = 0x164, Flags = TagFieldFlags.Padding)]
        public byte[] Padding41;
        [TagField(Length = 0x30, Flags = TagFieldFlags.Padding)]
        public byte[] Padding42;
        
        public enum AnchorValue : short
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            Center,
            Crosshair
        }
        
        [Flags]
        public enum ScalingFlagsValue : ushort
        {
            DonTScaleOffset = 1 << 0,
            DonTScaleSize = 1 << 1
        }
        
        [Flags]
        public enum FlashFlagsValue : ushort
        {
            ReverseDefaultFlashingColors = 1 << 0
        }
        
        [TagStructure(Size = 0x1C4)]
        public class GlobalHudMultitextureOverlayDefinition : TagStructure
        {
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short Type;
            public FramebufferBlendFuncValue FramebufferBlendFunc;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
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
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
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
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            [TagField(Length = 0xB8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            public List<GlobalHudMultitextureOverlayEffectorDefinition> Effectors;
            [TagField(Length = 0x80, Flags = TagFieldFlags.Padding)]
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
                AlphaMultiplyAdd,
                ConstantColorBlend,
                InverseConstantColorBlend,
                None
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
                [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
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
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                /// <summary>
                /// When the source is at the lower inbound, the destination ends up the lower outbound and vice-versa applies for the upper
                /// values.
                /// </summary>
                public Bounds<float> InBounds; // source units
                public Bounds<float> OutBounds; // pixels
                [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
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
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float FunctionPeriod; // seconds
                public float FunctionPhase; // seconds
                [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
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
                    WeaponRoundsLoaded,
                    WeaponRoundsInventory,
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
        public enum ScalingFlagsValue1 : ushort
        {
            DonTScaleOffset = 1 << 0,
            DonTScaleSize = 1 << 1
        }
        
        [Flags]
        public enum FlashFlagsValue1 : ushort
        {
            ReverseDefaultFlashingColors = 1 << 0
        }
        
        [TagStructure(Size = 0x1C4)]
        public class GlobalHudMultitextureOverlayDefinition1 : TagStructure
        {
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short Type;
            public FramebufferBlendFuncValue FramebufferBlendFunc;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
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
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
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
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            [TagField(Length = 0xB8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            public List<GlobalHudMultitextureOverlayEffectorDefinition> Effectors;
            [TagField(Length = 0x80, Flags = TagFieldFlags.Padding)]
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
                AlphaMultiplyAdd,
                ConstantColorBlend,
                InverseConstantColorBlend,
                None
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
                [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
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
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                /// <summary>
                /// When the source is at the lower inbound, the destination ends up the lower outbound and vice-versa applies for the upper
                /// values.
                /// </summary>
                public Bounds<float> InBounds; // source units
                public Bounds<float> OutBounds; // pixels
                [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
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
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float FunctionPeriod; // seconds
                public float FunctionPhase; // seconds
                [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
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
                    WeaponRoundsLoaded,
                    WeaponRoundsInventory,
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
        public enum ScalingFlagsValue2 : ushort
        {
            DonTScaleOffset = 1 << 0,
            DonTScaleSize = 1 << 1
        }
        
        [Flags]
        public enum FlagsValue : byte
        {
            UseMinMaxForStateChanges = 1 << 0,
            InterpolateBetweenMinMaxFlashColorsAsStateChanges = 1 << 1,
            InterpolateColorAlongHsvSpace = 1 << 2,
            MoreColorsForHsvInterpolation = 1 << 3,
            InvertInterpolation = 1 << 4
        }
        
        [TagStructure()]
        public class GNullBlock : TagStructure
        {
        }
        
        [Flags]
        public enum ScalingFlagsValue3 : ushort
        {
            DonTScaleOffset = 1 << 0,
            DonTScaleSize = 1 << 1
        }
        
        [Flags]
        public enum FlashFlagsValue2 : ushort
        {
            ReverseDefaultFlashingColors = 1 << 0
        }
        
        [TagStructure(Size = 0x1C4)]
        public class GlobalHudMultitextureOverlayDefinition2 : TagStructure
        {
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short Type;
            public FramebufferBlendFuncValue FramebufferBlendFunc;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
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
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
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
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            [TagField(Length = 0xB8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            public List<GlobalHudMultitextureOverlayEffectorDefinition> Effectors;
            [TagField(Length = 0x80, Flags = TagFieldFlags.Padding)]
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
                AlphaMultiplyAdd,
                ConstantColorBlend,
                InverseConstantColorBlend,
                None
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
                [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
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
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                /// <summary>
                /// When the source is at the lower inbound, the destination ends up the lower outbound and vice-versa applies for the upper
                /// values.
                /// </summary>
                public Bounds<float> InBounds; // source units
                public Bounds<float> OutBounds; // pixels
                [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
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
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float FunctionPeriod; // seconds
                public float FunctionPhase; // seconds
                [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
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
                    WeaponRoundsLoaded,
                    WeaponRoundsInventory,
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
        public enum ScalingFlagsValue4 : ushort
        {
            DonTScaleOffset = 1 << 0,
            DonTScaleSize = 1 << 1
        }
        
        [Flags]
        public enum FlagsValue1 : byte
        {
            UseMinMaxForStateChanges = 1 << 0,
            InterpolateBetweenMinMaxFlashColorsAsStateChanges = 1 << 1,
            InterpolateColorAlongHsvSpace = 1 << 2,
            MoreColorsForHsvInterpolation = 1 << 3,
            InvertInterpolation = 1 << 4
        }
        
        [TagStructure()]
        public class GNullBlock1 : TagStructure
        {
        }
        
        [Flags]
        public enum ScalingFlagsValue5 : ushort
        {
            DonTScaleOffset = 1 << 0,
            DonTScaleSize = 1 << 1
        }
        
        [Flags]
        public enum FlashFlagsValue3 : ushort
        {
            ReverseDefaultFlashingColors = 1 << 0
        }
        
        [TagStructure(Size = 0x1C4)]
        public class GlobalHudMultitextureOverlayDefinition3 : TagStructure
        {
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short Type;
            public FramebufferBlendFuncValue FramebufferBlendFunc;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
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
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
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
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            [TagField(Length = 0xB8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            public List<GlobalHudMultitextureOverlayEffectorDefinition> Effectors;
            [TagField(Length = 0x80, Flags = TagFieldFlags.Padding)]
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
                AlphaMultiplyAdd,
                ConstantColorBlend,
                InverseConstantColorBlend,
                None
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
                [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
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
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                /// <summary>
                /// When the source is at the lower inbound, the destination ends up the lower outbound and vice-versa applies for the upper
                /// values.
                /// </summary>
                public Bounds<float> InBounds; // source units
                public Bounds<float> OutBounds; // pixels
                [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
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
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float FunctionPeriod; // seconds
                public float FunctionPhase; // seconds
                [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
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
                    WeaponRoundsLoaded,
                    WeaponRoundsInventory,
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
        public enum ScalingFlagsValue6 : ushort
        {
            DonTScaleOffset = 1 << 0,
            DonTScaleSize = 1 << 1
        }
        
        [Flags]
        public enum FlashFlagsValue4 : ushort
        {
            ReverseDefaultFlashingColors = 1 << 0
        }
        
        [TagStructure(Size = 0x1C4)]
        public class GlobalHudMultitextureOverlayDefinition4 : TagStructure
        {
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short Type;
            public FramebufferBlendFuncValue FramebufferBlendFunc;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
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
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
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
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            [TagField(Length = 0xB8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            public List<GlobalHudMultitextureOverlayEffectorDefinition> Effectors;
            [TagField(Length = 0x80, Flags = TagFieldFlags.Padding)]
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
                AlphaMultiplyAdd,
                ConstantColorBlend,
                InverseConstantColorBlend,
                None
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
                [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
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
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                /// <summary>
                /// When the source is at the lower inbound, the destination ends up the lower outbound and vice-versa applies for the upper
                /// values.
                /// </summary>
                public Bounds<float> InBounds; // source units
                public Bounds<float> OutBounds; // pixels
                [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
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
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float FunctionPeriod; // seconds
                public float FunctionPhase; // seconds
                [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
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
                    WeaponRoundsLoaded,
                    WeaponRoundsInventory,
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
        public enum ScalingFlagsValue7 : ushort
        {
            DonTScaleOffset = 1 << 0,
            DonTScaleSize = 1 << 1
        }
        
        public enum AnchorValue1 : short
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            Center,
            Crosshair
        }
        
        [TagStructure(Size = 0x78)]
        public class UnitHudAuxilaryOverlayBlock : TagStructure
        {
            public Point2d AnchorOffset;
            public float WidthScale;
            public float HeightScale;
            public ScalingFlagsValue ScalingFlags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag InterfaceBitmap;
            public ArgbColor DefaultColor;
            public ArgbColor FlashingColor;
            public float FlashPeriod;
            /// <summary>
            /// time between flashes
            /// </summary>
            public float FlashDelay;
            public short NumberOfFlashes;
            public FlashFlagsValue FlashFlags;
            /// <summary>
            /// time of each flash
            /// </summary>
            public float FlashLength;
            public ArgbColor DisabledColor;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public short SequenceIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            public List<GlobalHudMultitextureOverlayDefinition> MultitexOverlay;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            public TypeValue Type;
            public FlagsValue Flags;
            [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            
            [Flags]
            public enum ScalingFlagsValue : ushort
            {
                DonTScaleOffset = 1 << 0,
                DonTScaleSize = 1 << 1
            }
            
            [Flags]
            public enum FlashFlagsValue : ushort
            {
                ReverseDefaultFlashingColors = 1 << 0
            }
            
            [TagStructure(Size = 0x1C4)]
            public class GlobalHudMultitextureOverlayDefinition : TagStructure
            {
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short Type;
                public FramebufferBlendFuncValue FramebufferBlendFunc;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
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
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
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
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
                [TagField(Length = 0xB8, Flags = TagFieldFlags.Padding)]
                public byte[] Padding5;
                public List<GlobalHudMultitextureOverlayEffectorDefinition> Effectors;
                [TagField(Length = 0x80, Flags = TagFieldFlags.Padding)]
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
                    AlphaMultiplyAdd,
                    ConstantColorBlend,
                    InverseConstantColorBlend,
                    None
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
                    [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
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
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    /// <summary>
                    /// When the source is at the lower inbound, the destination ends up the lower outbound and vice-versa applies for the upper
                    /// values.
                    /// </summary>
                    public Bounds<float> InBounds; // source units
                    public Bounds<float> OutBounds; // pixels
                    [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
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
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding3;
                    public float FunctionPeriod; // seconds
                    public float FunctionPhase; // seconds
                    [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
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
                        WeaponRoundsLoaded,
                        WeaponRoundsInventory,
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
            
            public enum TypeValue : short
            {
                TeamIcon
            }
            
            [Flags]
            public enum FlagsValue : ushort
            {
                UseTeamColor = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class UnitHudSoundBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "snd!","lsnd" })]
            public CachedTag Sound;
            public LatchedToValue LatchedTo;
            public float Scale;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [Flags]
            public enum LatchedToValue : uint
            {
                ShieldRecharging = 1 << 0,
                ShieldDamaged = 1 << 1,
                ShieldLow = 1 << 2,
                ShieldEmpty = 1 << 3,
                HealthLow = 1 << 4,
                HealthEmpty = 1 << 5,
                HealthMinorDamage = 1 << 6,
                HealthMajorDamage = 1 << 7
            }
        }
        
        [TagStructure(Size = 0x12C)]
        public class UnitHudAuxilaryPanelBlock : TagStructure
        {
            public TypeValue Type;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public Point2d AnchorOffset;
            public float WidthScale;
            public float HeightScale;
            public ScalingFlagsValue ScalingFlags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag InterfaceBitmap;
            public ArgbColor DefaultColor;
            public ArgbColor FlashingColor;
            public float FlashPeriod;
            /// <summary>
            /// time between flashes
            /// </summary>
            public float FlashDelay;
            public short NumberOfFlashes;
            public FlashFlagsValue FlashFlags;
            /// <summary>
            /// time of each flash
            /// </summary>
            public float FlashLength;
            public ArgbColor DisabledColor;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            public short SequenceIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            public List<GlobalHudMultitextureOverlayDefinition> MultitexOverlay;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding6;
            public Point2d AnchorOffset1;
            public float WidthScale1;
            public float HeightScale1;
            public ScalingFlagsValue1 ScalingFlags1;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding7;
            [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
            public byte[] Padding8;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag MeterBitmap;
            public ArgbColor ColorAtMeterMinimum;
            public ArgbColor ColorAtMeterMaximum;
            public ArgbColor FlashColor;
            public ArgbColor EmptyColor;
            public FlagsValue Flags;
            public sbyte MinumumMeterValue;
            public short SequenceIndex1;
            public sbyte AlphaMultiplier;
            public sbyte AlphaBias;
            /// <summary>
            /// used for non-integral values, i.e. health and shields
            /// </summary>
            public short ValueScale;
            public float Opacity;
            public float Translucency;
            public ArgbColor DisabledColor1;
            public List<GNullBlock> Unknown;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding9;
            public float MinimumFractionCutoff;
            public FlagsValue1 Flags1;
            [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
            public byte[] Padding10;
            [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
            public byte[] Padding11;
            
            public enum TypeValue : short
            {
                IntegratedLight
            }
            
            [Flags]
            public enum ScalingFlagsValue : ushort
            {
                DonTScaleOffset = 1 << 0,
                DonTScaleSize = 1 << 1
            }
            
            [Flags]
            public enum FlashFlagsValue : ushort
            {
                ReverseDefaultFlashingColors = 1 << 0
            }
            
            [TagStructure(Size = 0x1C4)]
            public class GlobalHudMultitextureOverlayDefinition : TagStructure
            {
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short Type;
                public FramebufferBlendFuncValue FramebufferBlendFunc;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
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
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
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
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
                [TagField(Length = 0xB8, Flags = TagFieldFlags.Padding)]
                public byte[] Padding5;
                public List<GlobalHudMultitextureOverlayEffectorDefinition> Effectors;
                [TagField(Length = 0x80, Flags = TagFieldFlags.Padding)]
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
                    AlphaMultiplyAdd,
                    ConstantColorBlend,
                    InverseConstantColorBlend,
                    None
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
                    [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
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
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    /// <summary>
                    /// When the source is at the lower inbound, the destination ends up the lower outbound and vice-versa applies for the upper
                    /// values.
                    /// </summary>
                    public Bounds<float> InBounds; // source units
                    public Bounds<float> OutBounds; // pixels
                    [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
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
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding3;
                    public float FunctionPeriod; // seconds
                    public float FunctionPhase; // seconds
                    [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
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
                        WeaponRoundsLoaded,
                        WeaponRoundsInventory,
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
            public enum ScalingFlagsValue1 : ushort
            {
                DonTScaleOffset = 1 << 0,
                DonTScaleSize = 1 << 1
            }
            
            [Flags]
            public enum FlagsValue : byte
            {
                UseMinMaxForStateChanges = 1 << 0,
                InterpolateBetweenMinMaxFlashColorsAsStateChanges = 1 << 1,
                InterpolateColorAlongHsvSpace = 1 << 2,
                MoreColorsForHsvInterpolation = 1 << 3,
                InvertInterpolation = 1 << 4
            }
            
            [TagStructure()]
            public class GNullBlock : TagStructure
            {
            }
            
            [Flags]
            public enum FlagsValue1 : uint
            {
                ShowOnlyWhenActive = 1 << 0,
                FlashOnceIfActivatedWhileDisabled = 1 << 1
            }
        }
    }
}

