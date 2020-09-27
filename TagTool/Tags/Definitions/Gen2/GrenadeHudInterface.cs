using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "grenade_hud_interface", Tag = "grhi", Size = 0x1F8)]
    public class GrenadeHudInterface : TagStructure
    {
        /// <summary>
        /// Grenade hud screen alignment
        /// </summary>
        public AnchorValue Anchor;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        [TagField(Flags = Padding, Length = 32)]
        public byte[] Padding2;
        /// <summary>
        /// Grenade hud background
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
        /// Total grenades background
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
        /// Total grenades numbers
        /// </summary>
        public Point2d AnchorOffset16;
        public float WidthScale17;
        public float HeightScale18;
        public ScalingFlagsValue ScalingFlags19;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding13;
        [TagField(Flags = Padding, Length = 20)]
        public byte[] Padding14;
        public ArgbColor DefaultColor20;
        public ArgbColor FlashingColor21;
        public float FlashPeriod22;
        public float FlashDelay23; // time between flashes
        public short NumberOfFlashes24;
        public FlashFlagsValue FlashFlags25;
        public float FlashLength26; // time of each flash
        public ArgbColor DisabledColor27;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding15;
        public sbyte MaximumNumberOfDigits;
        public FlagsValue Flags;
        public sbyte NumberOfFractionalDigits;
        [TagField(Flags = Padding, Length = 1)]
        public byte[] Padding16;
        [TagField(Flags = Padding, Length = 12)]
        public byte[] Padding17;
        public short FlashCutoff;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding18;
        /// <summary>
        /// Total grenades overlays
        /// </summary>
        public CachedTag OverlayBitmap;
        public List<WeaponHudOverlayItem> Overlays;
        public List<SoundHudElementDefinition> WarningSounds;
        [TagField(Flags = Padding, Length = 68)]
        public byte[] Padding19;
        /// <summary>
        /// Messaging information
        /// </summary>
        public short SequenceIndex28; // sequence index into the global hud icon bitmap
        public short WidthOffset; // extra spacing beyond bitmap width for text alignment
        public Point2d OffsetFromReferenceCorner;
        public ArgbColor OverrideIconColor;
        public sbyte FrameRate030;
        public FlagsValue Flags29;
        public short TextIndex;
        [TagField(Flags = Padding, Length = 48)]
        public byte[] Padding20;
        
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
            ShowLeadingZeros = 1 << 0,
            OnlyShowWhenZoomed = 1 << 1,
            DrawATrailingM = 1 << 2
        }
        
        [TagStructure(Size = 0x88)]
        public class WeaponHudOverlayItem : TagStructure
        {
            public Point2d AnchorOffset;
            public float WidthScale;
            public float HeightScale;
            public ScalingFlagsValue ScalingFlags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 20)]
            public byte[] Padding2;
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
            public float FrameRate;
            public short SequenceIndex;
            public TypeValue Type;
            public FlagsValue Flags;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding4;
            [TagField(Flags = Padding, Length = 40)]
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
                LowGrenadeCount = 1 << 0,
                NoGrenadesLeft = 1 << 1,
                ThrowOnNoGrenades = 1 << 2
            }
        }
    }
}

