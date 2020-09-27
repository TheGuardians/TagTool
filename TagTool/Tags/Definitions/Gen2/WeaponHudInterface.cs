using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "weapon_hud_interface", Tag = "wphi", Size = 0x17C)]
    public class WeaponHudInterface : TagStructure
    {
        public CachedTag ChildHud;
        /// <summary>
        /// Flash cutoffs
        /// </summary>
        public FlagsValue Flags;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        public short InventoryAmmoCutoff;
        public short LoadedAmmoCutoff;
        public short HeatCutoff;
        public short AgeCutoff;
        [TagField(Flags = Padding, Length = 32)]
        public byte[] Padding2;
        /// <summary>
        /// Weapon hud screen alignment
        /// </summary>
        public AnchorValue Anchor;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding3;
        [TagField(Flags = Padding, Length = 32)]
        public byte[] Padding4;
        public List<WeaponHudStaticElement> StaticElements;
        public List<WeaponHudMeterElement> MeterElements;
        public List<WeaponHudNumberElement> NumberElements;
        /// <summary>
        /// Crosshairs
        /// </summary>
        /// <remarks>
        /// Crosshairs always go in the center of the screen.
        /// Crosshairs can be attached to one of four different states:
        /// 
        /// * Aim: Default crosshair. Frame 0 is the default state, frame 1 is the auto-aim state (frame rate ignored)
        /// * Zoom: Zoom overlay. Each zoom level has a corresponding frame (frame rate ignored)
        /// * Charge: Charging overlay. If you wish to display an animation for charging, put it here.
        /// * Flash: Similar to charging, but for low ammo/batter/heat states
        /// * Reload/Overheat: Similar to charging, but for reloading/overheating
        /// 
        /// </remarks>
        public List<WeaponHudCrosshairsElement> Crosshairs;
        public List<WeaponHudOverlaysElement> OverlayElements;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding5;
        public List<GNullBlock> Unknown1;
        public List<HudScreenEffectDefinition> ScreenEffect;
        [TagField(Flags = Padding, Length = 132)]
        public byte[] Padding6;
        /// <summary>
        /// Messaging information
        /// </summary>
        public short SequenceIndex; // sequence index into the global hud icon bitmap
        public short WidthOffset; // extra spacing beyond bitmap width for text alignment
        public Point2d OffsetFromReferenceCorner;
        public ArgbColor OverrideIconColor;
        public sbyte FrameRate030;
        public FlagsValue Flags1;
        public short TextIndex;
        [TagField(Flags = Padding, Length = 48)]
        public byte[] Padding7;
        
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
        
        [TagStructure(Size = 0xB4)]
        public class WeaponHudStaticElement : TagStructure
        {
            public StateAttachedToValue StateAttachedTo;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public CanUseOnMapTypeValue CanUseOnMapType;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 28)]
            public byte[] Padding3;
            public Point2d AnchorOffset;
            public float WidthScale;
            public float HeightScale;
            public ScalingFlagsValue ScalingFlags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding4;
            [TagField(Flags = Padding, Length = 20)]
            public byte[] Padding5;
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
            public byte[] Padding6;
            public short SequenceIndex;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding7;
            public List<MultitextureOverlayHudElementDefinition> MultitexOverlay;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding8;
            [TagField(Flags = Padding, Length = 40)]
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
        }
        
        [TagStructure(Size = 0xB4)]
        public class WeaponHudMeterElement : TagStructure
        {
            public StateAttachedToValue StateAttachedTo;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public CanUseOnMapTypeValue CanUseOnMapType;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 28)]
            public byte[] Padding3;
            public Point2d AnchorOffset;
            public float WidthScale;
            public float HeightScale;
            public ScalingFlagsValue ScalingFlags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding4;
            [TagField(Flags = Padding, Length = 20)]
            public byte[] Padding5;
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
            public short ValueScale; // used for non-integral values, i.e. health and shields
            public float Opacity;
            public float Translucency;
            public ArgbColor DisabledColor;
            public List<GNullBlock> Unknown1;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding6;
            [TagField(Flags = Padding, Length = 40)]
            public byte[] Padding7;
            
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
        public class WeaponHudNumberElement : TagStructure
        {
            public StateAttachedToValue StateAttachedTo;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public CanUseOnMapTypeValue CanUseOnMapType;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 28)]
            public byte[] Padding3;
            public Point2d AnchorOffset;
            public float WidthScale;
            public float HeightScale;
            public ScalingFlagsValue ScalingFlags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding4;
            [TagField(Flags = Padding, Length = 20)]
            public byte[] Padding5;
            public ArgbColor DefaultColor;
            public ArgbColor FlashingColor;
            public float FlashPeriod;
            public float FlashDelay; // time between flashes
            public short NumberOfFlashes;
            public FlashFlagsValue FlashFlags;
            public float FlashLength; // time of each flash
            public ArgbColor DisabledColor;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding6;
            public sbyte MaximumNumberOfDigits;
            public FlagsValue Flags;
            public sbyte NumberOfFractionalDigits;
            [TagField(Flags = Padding, Length = 1)]
            public byte[] Padding7;
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Padding8;
            public WeaponSpecificFlagsValue WeaponSpecificFlags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding9;
            [TagField(Flags = Padding, Length = 36)]
            public byte[] Padding10;
            
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
        
        [TagStructure(Size = 0x68)]
        public class WeaponHudCrosshairsElement : TagStructure
        {
            public CrosshairTypeValue CrosshairType;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public CanUseOnMapTypeValue CanUseOnMapType;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 28)]
            public byte[] Padding3;
            public CachedTag CrosshairBitmap;
            public List<WeaponHudCrosshairItem> CrosshairOverlays;
            [TagField(Flags = Padding, Length = 40)]
            public byte[] Padding4;
            
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
            public class WeaponHudCrosshairItem : TagStructure
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
                public short FrameRate;
                public short SequenceIndex;
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 32)]
                public byte[] Padding4;
                
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
        
        [TagStructure(Size = 0x68)]
        public class WeaponHudOverlaysElement : TagStructure
        {
            public StateAttachedToValue StateAttachedTo;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public CanUseOnMapTypeValue CanUseOnMapType;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 28)]
            public byte[] Padding3;
            public CachedTag OverlayBitmap;
            public List<WeaponHudOverlayItem> Overlays;
            [TagField(Flags = Padding, Length = 40)]
            public byte[] Padding4;
            
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
                public short FrameRate;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding4;
                public short SequenceIndex;
                public TypeValue Type;
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 16)]
                public byte[] Padding5;
                [TagField(Flags = Padding, Length = 40)]
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
        
        [TagStructure(Size = 0x160)]
        public class HudScreenEffectDefinition : TagStructure
        {
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            /// <summary>
            /// Mask
            /// </summary>
            /// <remarks>
            /// Mask bitmap overlay. Use either a 2D bitmap or an interface bitmap.
            /// </remarks>
            public FlagsValue Flags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding3;
            public CachedTag MaskFullscreen;
            public CachedTag MaskSplitscreen;
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding4;
            [TagField(Flags = Padding, Length = 20)]
            public byte[] Padding5;
            [TagField(Flags = Padding, Length = 24)]
            public byte[] Padding6;
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding7;
            [TagField(Flags = Padding, Length = 24)]
            public byte[] Padding8;
            [TagField(Flags = Padding, Length = 20)]
            public byte[] Padding9;
            [TagField(Flags = Padding, Length = 24)]
            public byte[] Padding10;
            /// <summary>
            /// Screen effect (fullscreen)
            /// </summary>
            public ScreenEffectFlagsValue ScreenEffectFlags;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding11;
            public CachedTag ScreenEffect;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding12;
            /// <summary>
            /// Screen effect (splitscreen)
            /// </summary>
            public ScreenEffectFlagsValue ScreenEffectFlags1;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding13;
            public CachedTag ScreenEffect2;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding14;
            
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
        }
    }
}

