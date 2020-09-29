using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "unit_hud_interface", Tag = "unhi", Size = 0x57C)]
    public class UnitHudInterface : TagStructure
    {
        /// <summary>
        /// Weapon hud screen alignment
        /// </summary>
        public AnchorValue Anchor;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        [TagField(Flags = Padding, Length = 32)]
        public byte[] Padding2;
        /// <summary>
        /// Unit hud background
        /// </summary>
        public Point2d AnchorOffset;
        public float WidthScale;
        public float HeightScale;
        public ScalingFlagsValue ScalingFlags;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding3;
        [TagField(Flags = Padding, Length = 20)]
        public byte[] Padding4;
        public CachedTag InterfaceBitmap;
        public ArgbColor DefaultColor;
        public ArgbColor FlashingColor;
        public float FlashPeriod;
        public float FlashDelay; // time between flashes
        public short NumberOfFlashes;
        public FlashFlagsValue FlashFlags;
        public float FlashLength; // time of each flash
        public ArgbColor DisabledColor;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding5;
        public short SequenceIndex;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding6;
        public List<MultitextureOverlayHudElementDefinition> MultitexOverlay;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding7;
        /// <summary>
        /// Shield panel background
        /// </summary>
        public Point2d AnchorOffset1;
        public float WidthScale2;
        public float HeightScale3;
        public ScalingFlagsValue ScalingFlags4;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding8;
        [TagField(Flags = Padding, Length = 20)]
        public byte[] Padding9;
        public CachedTag InterfaceBitmap5;
        public ArgbColor DefaultColor6;
        public ArgbColor FlashingColor7;
        public float FlashPeriod8;
        public float FlashDelay9; // time between flashes
        public short NumberOfFlashes10;
        public FlashFlagsValue FlashFlags11;
        public float FlashLength12; // time of each flash
        public ArgbColor DisabledColor13;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding10;
        public short SequenceIndex14;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding11;
        public List<MultitextureOverlayHudElementDefinition> MultitexOverlay15;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding12;
        /// <summary>
        /// Shield panel meter
        /// </summary>
        public Point2d AnchorOffset16;
        public float WidthScale17;
        public float HeightScale18;
        public ScalingFlagsValue ScalingFlags19;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding13;
        [TagField(Flags = Padding, Length = 20)]
        public byte[] Padding14;
        public CachedTag MeterBitmap;
        public ArgbColor ColorAtMeterMinimum;
        public ArgbColor ColorAtMeterMaximum;
        public ArgbColor FlashColor;
        public ArgbColor EmptyColor;
        public FlagsValue Flags;
        public sbyte MinumumMeterValue;
        public short SequenceIndex20;
        public sbyte AlphaMultiplier;
        public sbyte AlphaBias;
        public short ValueScale; // used for non-integral values, i.e. health and shields
        public float Opacity;
        public float Translucency;
        public ArgbColor DisabledColor21;
        public List<GNullBlock> Unknown2;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding15;
        public ArgbColor OverchargeMinimumColor;
        public ArgbColor OverchargeMaximumColor;
        public ArgbColor OverchargeFlashColor;
        public ArgbColor OverchargeEmptyColor;
        [TagField(Flags = Padding, Length = 16)]
        public byte[] Padding16;
        /// <summary>
        /// Health panel background
        /// </summary>
        public Point2d AnchorOffset22;
        public float WidthScale23;
        public float HeightScale24;
        public ScalingFlagsValue ScalingFlags25;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding17;
        [TagField(Flags = Padding, Length = 20)]
        public byte[] Padding18;
        public CachedTag InterfaceBitmap26;
        public ArgbColor DefaultColor27;
        public ArgbColor FlashingColor28;
        public float FlashPeriod29;
        public float FlashDelay30; // time between flashes
        public short NumberOfFlashes31;
        public FlashFlagsValue FlashFlags32;
        public float FlashLength33; // time of each flash
        public ArgbColor DisabledColor34;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding19;
        public short SequenceIndex35;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding20;
        public List<MultitextureOverlayHudElementDefinition> MultitexOverlay36;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding21;
        /// <summary>
        /// Health panel meter
        /// </summary>
        public Point2d AnchorOffset37;
        public float WidthScale38;
        public float HeightScale39;
        public ScalingFlagsValue ScalingFlags40;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding22;
        [TagField(Flags = Padding, Length = 20)]
        public byte[] Padding23;
        public CachedTag MeterBitmap41;
        public ArgbColor ColorAtMeterMinimum42;
        public ArgbColor ColorAtMeterMaximum43;
        public ArgbColor FlashColor44;
        public ArgbColor EmptyColor45;
        public FlagsValue Flags46;
        public sbyte MinumumMeterValue47;
        public short SequenceIndex48;
        public sbyte AlphaMultiplier49;
        public sbyte AlphaBias50;
        public short ValueScale51; // used for non-integral values, i.e. health and shields
        public float Opacity52;
        public float Translucency53;
        public ArgbColor DisabledColor54;
        public List<GNullBlock> Unknown3;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding24;
        public ArgbColor MediumHealthLeftColor;
        public float MaxColorHealthFractionCutoff;
        public float MinColorHealthFractionCutoff;
        [TagField(Flags = Padding, Length = 20)]
        public byte[] Padding25;
        /// <summary>
        /// Motion sensor background
        /// </summary>
        public Point2d AnchorOffset55;
        public float WidthScale56;
        public float HeightScale57;
        public ScalingFlagsValue ScalingFlags58;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding26;
        [TagField(Flags = Padding, Length = 20)]
        public byte[] Padding27;
        public CachedTag InterfaceBitmap59;
        public ArgbColor DefaultColor60;
        public ArgbColor FlashingColor61;
        public float FlashPeriod62;
        public float FlashDelay63; // time between flashes
        public short NumberOfFlashes64;
        public FlashFlagsValue FlashFlags65;
        public float FlashLength66; // time of each flash
        public ArgbColor DisabledColor67;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding28;
        public short SequenceIndex68;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding29;
        public List<MultitextureOverlayHudElementDefinition> MultitexOverlay69;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding30;
        /// <summary>
        /// Motion sensor foreground
        /// </summary>
        public Point2d AnchorOffset70;
        public float WidthScale71;
        public float HeightScale72;
        public ScalingFlagsValue ScalingFlags73;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding31;
        [TagField(Flags = Padding, Length = 20)]
        public byte[] Padding32;
        public CachedTag InterfaceBitmap74;
        public ArgbColor DefaultColor75;
        public ArgbColor FlashingColor76;
        public float FlashPeriod77;
        public float FlashDelay78; // time between flashes
        public short NumberOfFlashes79;
        public FlashFlagsValue FlashFlags80;
        public float FlashLength81; // time of each flash
        public ArgbColor DisabledColor82;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding33;
        public short SequenceIndex83;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding34;
        public List<MultitextureOverlayHudElementDefinition> MultitexOverlay84;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding35;
        [TagField(Flags = Padding, Length = 32)]
        public byte[] Padding36;
        /// <summary>
        /// Motion sensor center
        /// </summary>
        /// <remarks>
        /// The blips use this as a reference point
        /// </remarks>
        public Point2d AnchorOffset85;
        public float WidthScale86;
        public float HeightScale87;
        public ScalingFlagsValue ScalingFlags88;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding37;
        [TagField(Flags = Padding, Length = 20)]
        public byte[] Padding38;
        /// <summary>
        /// Auxilary overlays
        /// </summary>
        public AnchorValue Anchor89;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding39;
        [TagField(Flags = Padding, Length = 32)]
        public byte[] Padding40;
        public List<AuxilaryOverlayDefinition> Overlays;
        [TagField(Flags = Padding, Length = 16)]
        public byte[] Padding41;
        /// <summary>
        /// Hud warning sounds
        /// </summary>
        public List<SoundHudElementDefinition> Sounds;
        /// <summary>
        /// Auxilary hud meters
        /// </summary>
        public List<AuxilaryMeterDefinition> Meters;
        /// <summary>
        /// NEW hud
        /// </summary>
        public CachedTag NewHud;
        [TagField(Flags = Padding, Length = 356)]
        public byte[] Padding42;
        [TagField(Flags = Padding, Length = 48)]
        public byte[] Padding43;
        
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
        
        [TagStructure(Size = 0x1E0)]
        public class MultitextureOverlayHudElementDefinition : TagStructure
        {
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public short Type;
            public FramebufferBlendFuncValue FramebufferBlendFunc;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding3;
            /// <summary>
            /// anchors
            /// </summary>
            /// <remarks>
            /// where you want the origin of the texture.
            /// *"texture" uses the texture coordinates supplied
            /// *"screen" uses the origin of the screen as the origin of the texture
            /// </remarks>
            public PrimaryAnchorValue PrimaryAnchor;
            public SecondaryAnchorValue SecondaryAnchor;
            public TertiaryAnchorValue TertiaryAnchor;
            /// <summary>
            /// blending function
            /// </summary>
            /// <remarks>
            /// how to blend the textures together
            /// </remarks>
            public _0To1BlendFuncValue _0To1BlendFunc;
            public _1To2BlendFuncValue _1To2BlendFunc;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding4;
            /// <summary>
            /// map scales
            /// </summary>
            /// <remarks>
            /// how much to scale the textures
            /// </remarks>
            public RealPoint2d PrimaryScale;
            public RealPoint2d SecondaryScale;
            public RealPoint2d TertiaryScale;
            /// <summary>
            /// map offsets
            /// </summary>
            /// <remarks>
            /// how much to offset the origin of the texture
            /// </remarks>
            public RealPoint2d PrimaryOffset;
            public RealPoint2d SecondaryOffset;
            public RealPoint2d TertiaryOffset;
            /// <summary>
            /// map
            /// </summary>
            /// <remarks>
            /// which maps to use
            /// </remarks>
            public CachedTag Primary;
            public CachedTag Secondary;
            public CachedTag Tertiary;
            public PrimaryWrapModeValue PrimaryWrapMode;
            public SecondaryWrapModeValue SecondaryWrapMode;
            public TertiaryWrapModeValue TertiaryWrapMode;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding5;
            [TagField(Flags = Padding, Length = 184)]
            public byte[] Padding6;
            public List<MultitextureOverlayHudElementEffectorDefinition> Effectors;
            [TagField(Flags = Padding, Length = 128)]
            public byte[] Padding7;
            
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
            public class MultitextureOverlayHudElementEffectorDefinition : TagStructure
            {
                [TagField(Flags = Padding, Length = 64)]
                public byte[] Padding1;
                /// <summary>
                /// source/destination
                /// </summary>
                /// <remarks>
                /// These describe the relationship that causes the effect.
                /// * destination type is the type of variable you want to be effected
                /// * destination tells which texture map (or geometry offset) to apply it to
                /// * source says which value to look at when computing the effect
                /// </remarks>
                public DestinationTypeValue DestinationType;
                public DestinationValue Destination;
                public SourceValue Source;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
                /// <summary>
                /// in/out bounds
                /// </summary>
                /// <remarks>
                /// When the source is at the lower inbound, the destination ends up the lower outbound and vice-versa applies for the upper values.
                /// </remarks>
                public Bounds<float> InBounds; // source units
                public Bounds<float> OutBounds; // pixels
                [TagField(Flags = Padding, Length = 64)]
                public byte[] Padding3;
                /// <summary>
                /// tint color bounds
                /// </summary>
                /// <remarks>
                /// If destination is tint, these values are used instead of the out bounds.
                /// </remarks>
                public RealRgbColor TintColorLowerBound;
                public RealRgbColor TintColorUpperBound;
                /// <summary>
                /// periodic functions
                /// </summary>
                /// <remarks>
                /// If you use a periodic function as the source, this lets you tweak it.
                /// </remarks>
                public PeriodicFunctionValue PeriodicFunction;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding4;
                public float FunctionPeriod; // seconds
                public float FunctionPhase; // seconds
                [TagField(Flags = Padding, Length = 32)]
                public byte[] Padding5;
                
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
        
        [TagStructure(Size = 0x84)]
        public class AuxilaryOverlayDefinition : TagStructure
        {
            public Point2d AnchorOffset;
            public float WidthScale;
            public float HeightScale;
            public ScalingFlagsValue ScalingFlags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 20)]
            public byte[] Padding2;
            public CachedTag InterfaceBitmap;
            public ArgbColor DefaultColor;
            public ArgbColor FlashingColor;
            public float FlashPeriod;
            public float FlashDelay; // time between flashes
            public short NumberOfFlashes;
            public FlashFlagsValue FlashFlags;
            public float FlashLength; // time of each flash
            public ArgbColor DisabledColor;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding3;
            public short SequenceIndex;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding4;
            public List<MultitextureOverlayHudElementDefinition> MultitexOverlay;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding5;
            public TypeValue Type;
            public FlagsValue Flags;
            [TagField(Flags = Padding, Length = 24)]
            public byte[] Padding6;
            
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
            
            [TagStructure(Size = 0x1E0)]
            public class MultitextureOverlayHudElementDefinition : TagStructure
            {
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public short Type;
                public FramebufferBlendFuncValue FramebufferBlendFunc;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
                [TagField(Flags = Padding, Length = 32)]
                public byte[] Padding3;
                /// <summary>
                /// anchors
                /// </summary>
                /// <remarks>
                /// where you want the origin of the texture.
                /// *"texture" uses the texture coordinates supplied
                /// *"screen" uses the origin of the screen as the origin of the texture
                /// </remarks>
                public PrimaryAnchorValue PrimaryAnchor;
                public SecondaryAnchorValue SecondaryAnchor;
                public TertiaryAnchorValue TertiaryAnchor;
                /// <summary>
                /// blending function
                /// </summary>
                /// <remarks>
                /// how to blend the textures together
                /// </remarks>
                public _0To1BlendFuncValue _0To1BlendFunc;
                public _1To2BlendFuncValue _1To2BlendFunc;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding4;
                /// <summary>
                /// map scales
                /// </summary>
                /// <remarks>
                /// how much to scale the textures
                /// </remarks>
                public RealPoint2d PrimaryScale;
                public RealPoint2d SecondaryScale;
                public RealPoint2d TertiaryScale;
                /// <summary>
                /// map offsets
                /// </summary>
                /// <remarks>
                /// how much to offset the origin of the texture
                /// </remarks>
                public RealPoint2d PrimaryOffset;
                public RealPoint2d SecondaryOffset;
                public RealPoint2d TertiaryOffset;
                /// <summary>
                /// map
                /// </summary>
                /// <remarks>
                /// which maps to use
                /// </remarks>
                public CachedTag Primary;
                public CachedTag Secondary;
                public CachedTag Tertiary;
                public PrimaryWrapModeValue PrimaryWrapMode;
                public SecondaryWrapModeValue SecondaryWrapMode;
                public TertiaryWrapModeValue TertiaryWrapMode;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding5;
                [TagField(Flags = Padding, Length = 184)]
                public byte[] Padding6;
                public List<MultitextureOverlayHudElementEffectorDefinition> Effectors;
                [TagField(Flags = Padding, Length = 128)]
                public byte[] Padding7;
                
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
                public class MultitextureOverlayHudElementEffectorDefinition : TagStructure
                {
                    [TagField(Flags = Padding, Length = 64)]
                    public byte[] Padding1;
                    /// <summary>
                    /// source/destination
                    /// </summary>
                    /// <remarks>
                    /// These describe the relationship that causes the effect.
                    /// * destination type is the type of variable you want to be effected
                    /// * destination tells which texture map (or geometry offset) to apply it to
                    /// * source says which value to look at when computing the effect
                    /// </remarks>
                    public DestinationTypeValue DestinationType;
                    public DestinationValue Destination;
                    public SourceValue Source;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding2;
                    /// <summary>
                    /// in/out bounds
                    /// </summary>
                    /// <remarks>
                    /// When the source is at the lower inbound, the destination ends up the lower outbound and vice-versa applies for the upper values.
                    /// </remarks>
                    public Bounds<float> InBounds; // source units
                    public Bounds<float> OutBounds; // pixels
                    [TagField(Flags = Padding, Length = 64)]
                    public byte[] Padding3;
                    /// <summary>
                    /// tint color bounds
                    /// </summary>
                    /// <remarks>
                    /// If destination is tint, these values are used instead of the out bounds.
                    /// </remarks>
                    public RealRgbColor TintColorLowerBound;
                    public RealRgbColor TintColorUpperBound;
                    /// <summary>
                    /// periodic functions
                    /// </summary>
                    /// <remarks>
                    /// If you use a periodic function as the source, this lets you tweak it.
                    /// </remarks>
                    public PeriodicFunctionValue PeriodicFunction;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding4;
                    public float FunctionPeriod; // seconds
                    public float FunctionPhase; // seconds
                    [TagField(Flags = Padding, Length = 32)]
                    public byte[] Padding5;
                    
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
        
        [TagStructure(Size = 0x38)]
        public class SoundHudElementDefinition : TagStructure
        {
            public CachedTag Sound;
            public LatchedToValue LatchedTo;
            public float Scale;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding1;
            
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
        
        [TagStructure(Size = 0x144)]
        public class AuxilaryMeterDefinition : TagStructure
        {
            public TypeValue Type;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding2;
            /// <summary>
            /// BACKGROUND
            /// </summary>
            public Point2d AnchorOffset;
            public float WidthScale;
            public float HeightScale;
            public ScalingFlagsValue ScalingFlags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding3;
            [TagField(Flags = Padding, Length = 20)]
            public byte[] Padding4;
            public CachedTag InterfaceBitmap;
            public ArgbColor DefaultColor;
            public ArgbColor FlashingColor;
            public float FlashPeriod;
            public float FlashDelay; // time between flashes
            public short NumberOfFlashes;
            public FlashFlagsValue FlashFlags;
            public float FlashLength; // time of each flash
            public ArgbColor DisabledColor;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding5;
            public short SequenceIndex;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding6;
            public List<MultitextureOverlayHudElementDefinition> MultitexOverlay;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding7;
            /// <summary>
            /// METER
            /// </summary>
            public Point2d AnchorOffset1;
            public float WidthScale2;
            public float HeightScale3;
            public ScalingFlagsValue ScalingFlags4;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding8;
            [TagField(Flags = Padding, Length = 20)]
            public byte[] Padding9;
            public CachedTag MeterBitmap;
            public ArgbColor ColorAtMeterMinimum;
            public ArgbColor ColorAtMeterMaximum;
            public ArgbColor FlashColor;
            public ArgbColor EmptyColor;
            public FlagsValue Flags;
            public sbyte MinumumMeterValue;
            public short SequenceIndex5;
            public sbyte AlphaMultiplier;
            public sbyte AlphaBias;
            public short ValueScale; // used for non-integral values, i.e. health and shields
            public float Opacity;
            public float Translucency;
            public ArgbColor DisabledColor6;
            public List<GNullBlock> Unknown1;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding10;
            public float MinimumFractionCutoff;
            public FlagsValue Flags7;
            [TagField(Flags = Padding, Length = 24)]
            public byte[] Padding11;
            [TagField(Flags = Padding, Length = 64)]
            public byte[] Padding12;
            
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
            
            [TagStructure(Size = 0x1E0)]
            public class MultitextureOverlayHudElementDefinition : TagStructure
            {
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public short Type;
                public FramebufferBlendFuncValue FramebufferBlendFunc;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
                [TagField(Flags = Padding, Length = 32)]
                public byte[] Padding3;
                /// <summary>
                /// anchors
                /// </summary>
                /// <remarks>
                /// where you want the origin of the texture.
                /// *"texture" uses the texture coordinates supplied
                /// *"screen" uses the origin of the screen as the origin of the texture
                /// </remarks>
                public PrimaryAnchorValue PrimaryAnchor;
                public SecondaryAnchorValue SecondaryAnchor;
                public TertiaryAnchorValue TertiaryAnchor;
                /// <summary>
                /// blending function
                /// </summary>
                /// <remarks>
                /// how to blend the textures together
                /// </remarks>
                public _0To1BlendFuncValue _0To1BlendFunc;
                public _1To2BlendFuncValue _1To2BlendFunc;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding4;
                /// <summary>
                /// map scales
                /// </summary>
                /// <remarks>
                /// how much to scale the textures
                /// </remarks>
                public RealPoint2d PrimaryScale;
                public RealPoint2d SecondaryScale;
                public RealPoint2d TertiaryScale;
                /// <summary>
                /// map offsets
                /// </summary>
                /// <remarks>
                /// how much to offset the origin of the texture
                /// </remarks>
                public RealPoint2d PrimaryOffset;
                public RealPoint2d SecondaryOffset;
                public RealPoint2d TertiaryOffset;
                /// <summary>
                /// map
                /// </summary>
                /// <remarks>
                /// which maps to use
                /// </remarks>
                public CachedTag Primary;
                public CachedTag Secondary;
                public CachedTag Tertiary;
                public PrimaryWrapModeValue PrimaryWrapMode;
                public SecondaryWrapModeValue SecondaryWrapMode;
                public TertiaryWrapModeValue TertiaryWrapMode;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding5;
                [TagField(Flags = Padding, Length = 184)]
                public byte[] Padding6;
                public List<MultitextureOverlayHudElementEffectorDefinition> Effectors;
                [TagField(Flags = Padding, Length = 128)]
                public byte[] Padding7;
                
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
                public class MultitextureOverlayHudElementEffectorDefinition : TagStructure
                {
                    [TagField(Flags = Padding, Length = 64)]
                    public byte[] Padding1;
                    /// <summary>
                    /// source/destination
                    /// </summary>
                    /// <remarks>
                    /// These describe the relationship that causes the effect.
                    /// * destination type is the type of variable you want to be effected
                    /// * destination tells which texture map (or geometry offset) to apply it to
                    /// * source says which value to look at when computing the effect
                    /// </remarks>
                    public DestinationTypeValue DestinationType;
                    public DestinationValue Destination;
                    public SourceValue Source;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding2;
                    /// <summary>
                    /// in/out bounds
                    /// </summary>
                    /// <remarks>
                    /// When the source is at the lower inbound, the destination ends up the lower outbound and vice-versa applies for the upper values.
                    /// </remarks>
                    public Bounds<float> InBounds; // source units
                    public Bounds<float> OutBounds; // pixels
                    [TagField(Flags = Padding, Length = 64)]
                    public byte[] Padding3;
                    /// <summary>
                    /// tint color bounds
                    /// </summary>
                    /// <remarks>
                    /// If destination is tint, these values are used instead of the out bounds.
                    /// </remarks>
                    public RealRgbColor TintColorLowerBound;
                    public RealRgbColor TintColorUpperBound;
                    /// <summary>
                    /// periodic functions
                    /// </summary>
                    /// <remarks>
                    /// If you use a periodic function as the source, this lets you tweak it.
                    /// </remarks>
                    public PeriodicFunctionValue PeriodicFunction;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding4;
                    public float FunctionPeriod; // seconds
                    public float FunctionPhase; // seconds
                    [TagField(Flags = Padding, Length = 32)]
                    public byte[] Padding5;
                    
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
        }
    }
}

