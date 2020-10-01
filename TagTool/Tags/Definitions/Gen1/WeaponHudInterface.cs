using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "weapon_hud_interface", Tag = "wphi", Size = 0x17C)]
    public class WeaponHudInterface : TagStructure
    {
        [TagField(ValidTags = new [] { "wphi" })]
        public CachedTag ChildHud;
        public FlagsValue Flags;
        [TagField(Length = 0x2)]
        public byte[] Padding;
        public short TotalAmmoCutoff;
        public short LoadedAmmoCutoff;
        public short HeatCutoff;
        public short AgeCutoff;
        [TagField(Length = 0x20)]
        public byte[] Padding1;
        public AnchorValue Anchor;
        [TagField(Length = 0x2)]
        public byte[] Padding2;
        [TagField(Length = 0x20)]
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
        [TagField(Length = 0x4)]
        public byte[] Padding4;
        [TagField(Length = 0xC)]
        public byte[] Padding5;
        public List<GlobalHudScreenEffectDefinition> ScreenEffect;
        [TagField(Length = 0x84)]
        public byte[] Padding6;
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
        public Flags1Value Flags1;
        public short TextIndex;
        [TagField(Length = 0x30)]
        public byte[] Padding7;
        
        public enum FlagsValue : ushort
        {
            UseParentHudFlashingParameters
        }
        
        public enum AnchorValue : short
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            Center
        }
        
        [TagStructure(Size = 0xB4)]
        public class WeaponHudStaticBlock : TagStructure
        {
            public StateAttachedToValue StateAttachedTo;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            public CanUseOnMapTypeValue CanUseOnMapType;
            [TagField(Length = 0x2)]
            public byte[] Padding1;
            [TagField(Length = 0x1C)]
            public byte[] Padding2;
            public Point2d AnchorOffset;
            public float WidthScale;
            public float HeightScale;
            public ScalingFlagsValue ScalingFlags;
            [TagField(Length = 0x2)]
            public byte[] Padding3;
            [TagField(Length = 0x14)]
            public byte[] Padding4;
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
            public byte[] Padding5;
            public short SequenceIndex;
            [TagField(Length = 0x2)]
            public byte[] Padding6;
            public List<GlobalHudMultitextureOverlayDefinition> MultitexOverlay;
            [TagField(Length = 0x4)]
            public byte[] Padding7;
            [TagField(Length = 0x28)]
            public byte[] Padding8;
            
            public enum StateAttachedToValue : short
            {
                TotalAmmo,
                LoadedAmmo,
                Heat,
                Age,
                SecondaryWeaponTotalAmmo,
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
            
            public enum ScalingFlagsValue : ushort
            {
                DonTScaleOffset,
                DonTScaleSize,
                UseHighResScale
            }
            
            public enum FlashFlagsValue : ushort
            {
                ReverseDefaultFlashingColors
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
        }
        
        [TagStructure(Size = 0xB4)]
        public class WeaponHudMeterBlock : TagStructure
        {
            public StateAttachedToValue StateAttachedTo;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            public CanUseOnMapTypeValue CanUseOnMapType;
            [TagField(Length = 0x2)]
            public byte[] Padding1;
            [TagField(Length = 0x1C)]
            public byte[] Padding2;
            public Point2d AnchorOffset;
            public float WidthScale;
            public float HeightScale;
            public ScalingFlagsValue ScalingFlags;
            [TagField(Length = 0x2)]
            public byte[] Padding3;
            [TagField(Length = 0x14)]
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
            [TagField(Length = 0x10)]
            public byte[] Padding5;
            [TagField(Length = 0x28)]
            public byte[] Padding6;
            
            public enum StateAttachedToValue : short
            {
                TotalAmmo,
                LoadedAmmo,
                Heat,
                Age,
                SecondaryWeaponTotalAmmo,
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
            
            public enum ScalingFlagsValue : ushort
            {
                DonTScaleOffset,
                DonTScaleSize,
                UseHighResScale
            }
            
            public enum FlagsValue : byte
            {
                UseMinMaxForStateChanges,
                InterpolateBetweenMinMaxFlashColorsAsStateChanges,
                InterpolateColorAlongHsvSpace,
                MoreColorsForHsvInterpolation,
                InvertInterpolation
            }
        }
        
        [TagStructure(Size = 0xA0)]
        public class WeaponHudNumberBlock : TagStructure
        {
            public StateAttachedToValue StateAttachedTo;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            public CanUseOnMapTypeValue CanUseOnMapType;
            [TagField(Length = 0x2)]
            public byte[] Padding1;
            [TagField(Length = 0x1C)]
            public byte[] Padding2;
            public Point2d AnchorOffset;
            public float WidthScale;
            public float HeightScale;
            public ScalingFlagsValue ScalingFlags;
            [TagField(Length = 0x2)]
            public byte[] Padding3;
            [TagField(Length = 0x14)]
            public byte[] Padding4;
            public ArgbColor DefaultColor;
            public ArgbColor FlashingColor;
            public float FlashPeriod;
            public float FlashDelay;
            public short NumberOfFlashes;
            public FlashFlagsValue FlashFlags;
            public float FlashLength;
            public ArgbColor DisabledColor;
            [TagField(Length = 0x4)]
            public byte[] Padding5;
            public sbyte MaximumNumberOfDigits;
            public FlagsValue Flags;
            public sbyte NumberOfFractionalDigits;
            [TagField(Length = 0x1)]
            public byte[] Padding6;
            [TagField(Length = 0xC)]
            public byte[] Padding7;
            public WeaponSpecificFlagsValue WeaponSpecificFlags;
            [TagField(Length = 0x2)]
            public byte[] Padding8;
            [TagField(Length = 0x24)]
            public byte[] Padding9;
            
            public enum StateAttachedToValue : short
            {
                TotalAmmo,
                LoadedAmmo,
                Heat,
                Age,
                SecondaryWeaponTotalAmmo,
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
            
            public enum ScalingFlagsValue : ushort
            {
                DonTScaleOffset,
                DonTScaleSize,
                UseHighResScale
            }
            
            public enum FlashFlagsValue : ushort
            {
                ReverseDefaultFlashingColors
            }
            
            public enum FlagsValue : byte
            {
                ShowLeadingZeros,
                OnlyShowWhenZoomed,
                DrawATrailingM
            }
            
            public enum WeaponSpecificFlagsValue : ushort
            {
                DivideNumberByClipSize
            }
        }
        
        [TagStructure(Size = 0x68)]
        public class WeaponHudCrosshairBlock : TagStructure
        {
            public CrosshairTypeValue CrosshairType;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            public CanUseOnMapTypeValue CanUseOnMapType;
            [TagField(Length = 0x2)]
            public byte[] Padding1;
            [TagField(Length = 0x1C)]
            public byte[] Padding2;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag CrosshairBitmap;
            public List<WeaponHudCrosshairItemBlock> CrosshairOverlays;
            [TagField(Length = 0x28)]
            public byte[] Padding3;
            
            public enum CrosshairTypeValue : short
            {
                Aim,
                Zoom,
                Charge,
                ShouldReload,
                FlashHeat,
                FlashTotalAmmo,
                FlashBattery,
                ReloadOverheat,
                FlashWhenFiringAndNoAmmo,
                FlashWhenThrowingAndNoGrenade,
                LowAmmoAndNoneLeftToReload,
                ShouldReloadSecondaryTrigger,
                FlashSecondaryTotalAmmo,
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
                public short FrameRate;
                public short SequenceIndex;
                public FlagsValue Flags;
                [TagField(Length = 0x20)]
                public byte[] Padding3;
                
                public enum ScalingFlagsValue : ushort
                {
                    DonTScaleOffset,
                    DonTScaleSize,
                    UseHighResScale
                }
                
                public enum FlashFlagsValue : ushort
                {
                    ReverseDefaultFlashingColors
                }
                
                public enum FlagsValue : uint
                {
                    FlashesWhenActive,
                    NotASprite,
                    ShowOnlyWhenZoomed,
                    ShowSniperData,
                    HideAreaOutsideReticle,
                    OneZoomLevel,
                    DonTShowWhenZoomed
                }
            }
        }
        
        [TagStructure(Size = 0x68)]
        public class WeaponHudOverlaysBlock : TagStructure
        {
            public StateAttachedToValue StateAttachedTo;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            public CanUseOnMapTypeValue CanUseOnMapType;
            [TagField(Length = 0x2)]
            public byte[] Padding1;
            [TagField(Length = 0x1C)]
            public byte[] Padding2;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag OverlayBitmap;
            public List<WeaponHudOverlayBlock> Overlays;
            [TagField(Length = 0x28)]
            public byte[] Padding3;
            
            public enum StateAttachedToValue : short
            {
                TotalAmmo,
                LoadedAmmo,
                Heat,
                Age,
                SecondaryWeaponTotalAmmo,
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
                public short FrameRate;
                [TagField(Length = 0x2)]
                public byte[] Padding3;
                public short SequenceIndex;
                public TypeValue Type;
                public FlagsValue Flags;
                [TagField(Length = 0x10)]
                public byte[] Padding4;
                [TagField(Length = 0x28)]
                public byte[] Padding5;
                
                public enum ScalingFlagsValue : ushort
                {
                    DonTScaleOffset,
                    DonTScaleSize,
                    UseHighResScale
                }
                
                public enum FlashFlagsValue : ushort
                {
                    ReverseDefaultFlashingColors
                }
                
                public enum TypeValue : ushort
                {
                    ShowOnFlashing,
                    ShowOnEmpty,
                    ShowOnReloadOverheating,
                    ShowOnDefault,
                    ShowAlways
                }
                
                public enum FlagsValue : uint
                {
                    FlashesWhenActive
                }
            }
        }
        
        [TagStructure(Size = 0xB8)]
        public class GlobalHudScreenEffectDefinition : TagStructure
        {
            [TagField(Length = 0x4)]
            public byte[] Padding;
            /// <summary>
            /// Mask bitmap overlay. Use either a 2D bitmap or an interface bitmap.
            /// </summary>
            public FlagsValue Flags;
            [TagField(Length = 0x2)]
            public byte[] Padding1;
            [TagField(Length = 0x10)]
            public byte[] Padding2;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag MaskFullscreen;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag MaskSplitscreen;
            [TagField(Length = 0x8)]
            public byte[] Padding3;
            /// <summary>
            /// Warp effect like in 'Pitch-Black', sort of.
            /// </summary>
            public Flags1Value Flags1;
            [TagField(Length = 0x2)]
            public byte[] Padding4;
            public Bounds<Angle> FovInBounds; // degrees
            public Bounds<float> RadiusOutBounds; // pixels
            [TagField(Length = 0x18)]
            public byte[] Padding5;
            /// <summary>
            /// Real-time gamma correction to make dark objects appear brighter.
            /// </summary>
            public Flags2Value Flags2;
            public short ScriptSource; // [0,3]
            public float Intensity; // [0,1]
            [TagField(Length = 0x18)]
            public byte[] Padding6;
            /// <summary>
            /// Real-time monochromatic color filter.
            /// </summary>
            public Flags3Value Flags3;
            public short ScriptSource1; // [0,3]
            public float Intensity1; // [0,1]
            public RealRgbColor Tint;
            [TagField(Length = 0x18)]
            public byte[] Padding7;
            
            public enum FlagsValue : ushort
            {
                OnlyWhenZoomed
            }
            
            public enum Flags1Value : ushort
            {
                OnlyWhenZoomed
            }
            
            public enum Flags2Value : ushort
            {
                OnlyWhenZoomed,
                ConnectToFlashlight,
                Masked
            }
            
            public enum Flags3Value : ushort
            {
                OnlyWhenZoomed,
                ConnectToFlashlight,
                Additive,
                Masked
            }
        }
        
        public enum Flags1Value : byte
        {
            UseTextFromStringListInstead,
            OverrideDefaultColor,
            WidthOffsetIsAbsoluteIconWidth
        }
    }
}

