using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "weapon_hud_interface", Tag = "wphi", Size = 0x158)]
    public class WeaponHudInterface : TagStructure
    {
        [TagField(ValidTags = new [] { "wphi" })]
        public CachedTag ChildHud;
        public FlagsValue Flags;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public short InventoryAmmoCutoff;
        public short LoadedAmmoCutoff;
        public short HeatCutoff;
        public short AgeCutoff;
        [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public AnchorValue Anchor;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
        public List<WeaponHudStaticBlock> StaticElements;
        public List<WeaponHudMeterBlock> MeterElements;
        public List<WeaponHudNumberBlock> NumberElements;
        /// <summary>
        /// Crosshairs always go in the center of the screen.
        /// Crosshairs can be attached to one of four different states:
        /// 
        /// * Aim:
        /// Default crosshair. Frame 0 is the default state, frame 1 is the auto-aim state (frame rate ignored)
        /// * Zoom: Zoom overlay.
        /// Each zoom level has a corresponding frame (frame rate ignored)
        /// * Charge: Charging overlay. If you wish to display an
        /// animation for charging, put it here.
        /// * Flash: Similar to charging, but for low ammo/batter/heat states
        /// * Reload/Overheat:
        /// Similar to charging, but for reloading/overheating
        /// 
        /// </summary>
        public List<WeaponHudCrosshairBlock> Crosshairs;
        public List<WeaponHudOverlaysBlock> OverlayElements;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding4;
        public List<GNullBlock> Unknown;
        public List<GlobalHudScreenEffectDefinition> ScreenEffect;
        [TagField(Length = 0x84, Flags = TagFieldFlags.Padding)]
        public byte[] Padding5;
        /// <summary>
        /// sequence index into the global hud icon bitmap
        /// </summary>
        public short SequenceIndex;
        /// <summary>
        /// extra spacing beyond bitmap width for text alignment
        /// </summary>
        public short WidthOffset;
        public Point2d OffsetFromReferenceCorner;
        public ArgbColor OverrideIconColor;
        public sbyte FrameRate030;
        public FlagsValue1 Flags1;
        public short TextIndex;
        [TagField(Length = 0x30, Flags = TagFieldFlags.Padding)]
        public byte[] Padding6;
        
        [Flags]
        public enum FlagsValue : ushort
        {
            UseParentHudFlashingParameters = 1 << 0
        }
        
        public enum AnchorValue : short
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            Center,
            Crosshair
        }
        
        [TagStructure(Size = 0xA8)]
        public class WeaponHudStaticBlock : TagStructure
        {
            public StateAttachedToValue StateAttachedTo;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public CanUseOnMapTypeValue CanUseOnMapType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public Point2d AnchorOffset;
            public float WidthScale;
            public float HeightScale;
            public ScalingFlagsValue ScalingFlags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
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
            public byte[] Padding5;
            public short SequenceIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding6;
            public List<GlobalHudMultitextureOverlayDefinition> MultitexOverlay;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding7;
            [TagField(Length = 0x28, Flags = TagFieldFlags.Padding)]
            public byte[] Padding8;
            
            public enum StateAttachedToValue : short
            {
                InventoryAmmo,
                LoadedAmmo,
                Heat,
                Age,
                SecondaryWeaponInventoryAmmo,
                SecondaryWeaponLoadedAmmo,
                DistanceToTarget,
                ElevationToTarget
            }
            
            public enum CanUseOnMapTypeValue : short
            {
                Any,
                Solo,
                Multiplayer
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
        }
        
        [TagStructure(Size = 0xA8)]
        public class WeaponHudMeterBlock : TagStructure
        {
            public StateAttachedToValue StateAttachedTo;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public CanUseOnMapTypeValue CanUseOnMapType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public Point2d AnchorOffset;
            public float WidthScale;
            public float HeightScale;
            public ScalingFlagsValue ScalingFlags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag MeterBitmap;
            public ArgbColor ColorAtMeterMinimum;
            public ArgbColor ColorAtMeterMaximum;
            public ArgbColor FlashColor;
            public ArgbColor EmptyColor;
            public FlagsValue Flags;
            public sbyte MinumumMeterValue;
            public short SequenceIndex;
            public sbyte AlphaMultiplier;
            public sbyte AlphaBias;
            /// <summary>
            /// used for non-integral values, i.e. health and shields
            /// </summary>
            public short ValueScale;
            public float Opacity;
            public float Translucency;
            public ArgbColor DisabledColor;
            public List<GNullBlock> Unknown;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            [TagField(Length = 0x28, Flags = TagFieldFlags.Padding)]
            public byte[] Padding6;
            
            public enum StateAttachedToValue : short
            {
                InventoryAmmo,
                LoadedAmmo,
                Heat,
                Age,
                SecondaryWeaponInventoryAmmo,
                SecondaryWeaponLoadedAmmo,
                DistanceToTarget,
                ElevationToTarget
            }
            
            public enum CanUseOnMapTypeValue : short
            {
                Any,
                Solo,
                Multiplayer
            }
            
            [Flags]
            public enum ScalingFlagsValue : ushort
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
        }
        
        [TagStructure(Size = 0xA0)]
        public class WeaponHudNumberBlock : TagStructure
        {
            public StateAttachedToValue StateAttachedTo;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public CanUseOnMapTypeValue CanUseOnMapType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public Point2d AnchorOffset;
            public float WidthScale;
            public float HeightScale;
            public ScalingFlagsValue ScalingFlags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
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
            public byte[] Padding5;
            public sbyte MaximumNumberOfDigits;
            public FlagsValue Flags;
            public sbyte NumberOfFractionalDigits;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding6;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding7;
            public WeaponSpecificFlagsValue WeaponSpecificFlags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding8;
            [TagField(Length = 0x24, Flags = TagFieldFlags.Padding)]
            public byte[] Padding9;
            
            public enum StateAttachedToValue : short
            {
                InventoryAmmo,
                LoadedAmmo,
                Heat,
                Age,
                SecondaryWeaponInventoryAmmo,
                SecondaryWeaponLoadedAmmo,
                DistanceToTarget,
                ElevationToTarget
            }
            
            public enum CanUseOnMapTypeValue : short
            {
                Any,
                Solo,
                Multiplayer
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
            
            [Flags]
            public enum FlagsValue : byte
            {
                ShowLeadingZeros = 1 << 0,
                OnlyShowWhenZoomed = 1 << 1,
                DrawATrailingM = 1 << 2
            }
            
            [Flags]
            public enum WeaponSpecificFlagsValue : ushort
            {
                DivideNumberByClipSize = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x5C)]
        public class WeaponHudCrosshairBlock : TagStructure
        {
            public CrosshairTypeValue CrosshairType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public CanUseOnMapTypeValue CanUseOnMapType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag CrosshairBitmap;
            public List<WeaponHudCrosshairItemBlock> CrosshairOverlays;
            [TagField(Length = 0x28, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            
            public enum CrosshairTypeValue : short
            {
                Aim,
                Zoom,
                Charge,
                ShouldReload,
                FlashHeat,
                FlashInventoryAmmo,
                FlashBattery,
                ReloadOverheat,
                FlashWhenFiringAndNoAmmo,
                FlashWhenThrowingAndNoGrenade,
                LowAmmoAndNoneLeftToReload,
                ShouldReloadSecondaryTrigger,
                FlashSecondaryInventoryAmmo,
                FlashSecondaryReload,
                FlashWhenFiringSecondaryTriggerWithNoAmmo,
                LowSecondaryAmmoAndNoneLeftToReload,
                PrimaryTriggerReady,
                SecondaryTriggerReady,
                FlashWhenFiringWithDepletedBattery
            }
            
            public enum CanUseOnMapTypeValue : short
            {
                Any,
                Solo,
                Multiplayer
            }
            
            [TagStructure(Size = 0x6C)]
            public class WeaponHudCrosshairItemBlock : TagStructure
            {
                public Point2d AnchorOffset;
                public float WidthScale;
                public float HeightScale;
                public ScalingFlagsValue ScalingFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
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
                public short FrameRate;
                public short SequenceIndex;
                public FlagsValue Flags;
                [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                
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
                public enum FlagsValue : uint
                {
                    FlashesWhenActive = 1 << 0,
                    NotASprite = 1 << 1,
                    ShowOnlyWhenZoomed = 1 << 2,
                    ShowSniperData = 1 << 3,
                    HideAreaOutsideReticle = 1 << 4,
                    OneZoomLevel = 1 << 5,
                    DonTShowWhenZoomed = 1 << 6
                }
            }
        }
        
        [TagStructure(Size = 0x5C)]
        public class WeaponHudOverlaysBlock : TagStructure
        {
            public StateAttachedToValue StateAttachedTo;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public CanUseOnMapTypeValue CanUseOnMapType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag OverlayBitmap;
            public List<WeaponHudOverlayBlock> Overlays;
            [TagField(Length = 0x28, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            
            public enum StateAttachedToValue : short
            {
                InventoryAmmo,
                LoadedAmmo,
                Heat,
                Age,
                SecondaryWeaponInventoryAmmo,
                SecondaryWeaponLoadedAmmo,
                DistanceToTarget,
                ElevationToTarget
            }
            
            public enum CanUseOnMapTypeValue : short
            {
                Any,
                Solo,
                Multiplayer
            }
            
            [TagStructure(Size = 0x88)]
            public class WeaponHudOverlayBlock : TagStructure
            {
                public Point2d AnchorOffset;
                public float WidthScale;
                public float HeightScale;
                public ScalingFlagsValue ScalingFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
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
                public short FrameRate;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public short SequenceIndex;
                public TypeValue Type;
                public FlagsValue Flags;
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
                [TagField(Length = 0x28, Flags = TagFieldFlags.Padding)]
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
                    ShowOnReloadOverheating = 1 << 2,
                    ShowOnDefault = 1 << 3,
                    ShowAlways = 1 << 4
                }
                
                [Flags]
                public enum FlagsValue : uint
                {
                    FlashesWhenActive = 1 << 0
                }
            }
        }
        
        [TagStructure()]
        public class GNullBlock : TagStructure
        {
        }
        
        [TagStructure(Size = 0x140)]
        public class GlobalHudScreenEffectDefinition : TagStructure
        {
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            /// <summary>
            /// Mask bitmap overlay. Use either a 2D bitmap or an interface bitmap.
            /// </summary>
            public FlagsValue Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag MaskFullscreen;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag MaskSplitscreen;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding6;
            [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
            public byte[] Padding7;
            [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
            public byte[] Padding8;
            [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
            public byte[] Padding9;
            public ScreenEffectFlagsValue ScreenEffectFlags;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding10;
            [TagField(ValidTags = new [] { "egor" })]
            public CachedTag ScreenEffect;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding11;
            public ScreenEffectFlagsValue1 ScreenEffectFlags1;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding12;
            [TagField(ValidTags = new [] { "egor" })]
            public CachedTag ScreenEffect1;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding13;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                OnlyWhenZoomed = 1 << 0,
                MirrorHorizontally = 1 << 1,
                MirrorVertically = 1 << 2,
                UseNewHotness = 1 << 3
            }
            
            [Flags]
            public enum ScreenEffectFlagsValue : uint
            {
                OnlyWhenZoomed = 1 << 0
            }
            
            [Flags]
            public enum ScreenEffectFlagsValue1 : uint
            {
                OnlyWhenZoomed = 1 << 0
            }
        }
        
        [Flags]
        public enum FlagsValue1 : byte
        {
            UseTextFromStringListInstead = 1 << 0,
            OverrideDefaultColor = 1 << 1,
            WidthOffsetIsAbsoluteIconWidth = 1 << 2
        }
    }
}

