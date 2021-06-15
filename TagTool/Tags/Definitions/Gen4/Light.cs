using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "light", Tag = "ligh", Size = 0x1A4)]
    public class Light : TagStructure
    {
        public MidnightLightStruct MidnightLightParameters;
        
        [TagStructure(Size = 0x1A4)]
        public class MidnightLightStruct : TagStructure
        {
            public int Version;
            // Light node name in DCC.
            public StringId HalolightNode; // ^
            // Geometry shape of light.
            public MidnightLightTypeEnum LightType;
            public RealRgbColor LightColor;
            public LightScalarFunctionStruct Intensity; // [0-1+]
            // Attenuation overrides.
            public MidnightLightingModeDefinition LightingMode;
            // Inner penumbra start.
            public float DistanceAttenuationStart;
            // Radius or size of light
            public LightScalarFunctionStruct DistanceAttenuationEnd;
            // Distance at which we start to fade out this light
            public float CameraDistanceFadeStart;
            // Distance at which the light should be completely faded out and not seen
            public float CameraDistanceFadeEnd;
            // For screen-space dynamic lights
            public float SpecularPower;
            // For screen-space dynamic lights
            public float SpecularIntensity;
            // Inner hotspot attenuation end.
            public float InnerConeAngle; // [0-160 degrees]
            // Angle size of spotlight.
            public LightScalarFunctionStruct OuterConeEnd; // [0-160 degrees]
            // Use cone in all cases unless you have a gobo you need to project, frustum uses the full clip area of the light
            public MidnightProjectionDefinition ConeProjectionShape;
            public float ShadowNearClipPlane;
            public float ShadowFarClipPlane;
            public float ShadowBiasOffset;
            // Shadow tint.
            public RealRgbColor ShadowColor;
            public MidnightDynamicShadowQualityModes DynamicShadowQuality;
            public MidnightBooleanEnum Shadows;
            public MidnightBooleanEnum1 ScreenspaceLight;
            public MidnightBooleanEnum1 IgnoreDynamicObjects;
            // This should only be turned on if you have light linking data correctly setup
            public MidnightBooleanEnum1 CinemaObjectsOnly;
            public MidnightBooleanEnum1 CinemaOnly;
            public MidnightBooleanEnum1 CinemaExclude;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public MidnightBooleanEnum1 SpecularContribution;
            public MidnightBooleanEnum1 DiffuseContribution;
            // Bitmap file for spotlight. Cube map for point light. Gobo bitmap must be synced to xbox.
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag GoboTagPath;
            public LightScalarFunctionStruct Rotation; // [0-360 degrees]
            // Shape length and width of Gobo.
            public float AspectRatio; // [-0-1+]
            // Gobo texture lookup behavior when tiled.
            public MidnightGoboTileBehaviorEnum TileBehavior;
            // Tile Gobo.
            public float RepeatX; // [0-1+]
            // Tile Gobo.
            public float RepeatY; // [0-1+]
            // Offset Gobo.
            public LightScalarFunctionStruct OffsetX;
            // Offset Gobo.
            public LightScalarFunctionStruct OffsetY;
            public LightColorFunctionStruct InterpolationFunction;
            public RealRgbColor TargetColor;
            public ChanneldefinitionFlags LightChannels;
            public ChanneldefinitionFlags ShadowChannels;
            public LightDefinitionFlags Flags;
            // automatically destroys the light after it has existed this long (0 to disable)
            public float DestroyLightAfter; // seconds
            [TagField(ValidTags = new [] { "lens" })]
            public CachedTag LensFlare;
            
            public enum MidnightLightTypeEnum : int
            {
                Point,
                Spot,
                Directional,
                Area,
                Sun
            }
            
            public enum MidnightLightingModeDefinition : int
            {
                PhysicallyCorrect,
                Artistic
            }
            
            public enum MidnightProjectionDefinition : int
            {
                Cone,
                Frustum
            }
            
            public enum MidnightDynamicShadowQualityModes : short
            {
                Normal,
                Expensive
            }
            
            public enum MidnightBooleanEnum : short
            {
                Off,
                On
            }
            
            public enum MidnightBooleanEnum1 : sbyte
            {
                Off,
                On
            }
            
            public enum MidnightGoboTileBehaviorEnum : int
            {
                Clamp,
                Repeat,
                Mirror
            }
            
            [Flags]
            public enum ChanneldefinitionFlags : uint
            {
                _0 = 1 << 0,
                _1 = 1 << 1,
                _2 = 1 << 2,
                _3 = 1 << 3,
                _4 = 1 << 4,
                _5 = 1 << 5,
                _6 = 1 << 6,
                _7 = 1 << 7,
                _8 = 1 << 8,
                _9 = 1 << 9,
                _10 = 1 << 10,
                _11 = 1 << 11,
                _12 = 1 << 12,
                _13 = 1 << 13,
                _14 = 1 << 14,
                _15 = 1 << 15,
                _16 = 1 << 16,
                _17 = 1 << 17,
                _18 = 1 << 18,
                _19 = 1 << 19,
                _20 = 1 << 20,
                _21 = 1 << 21,
                _22 = 1 << 22,
                _23 = 1 << 23,
                _24 = 1 << 24,
                _25 = 1 << 25,
                _26 = 1 << 26,
                _27 = 1 << 27,
                _28 = 1 << 28,
                _29 = 1 << 29,
                _30 = 1 << 30,
                _31 = 1u << 31
            }
            
            [Flags]
            public enum LightDefinitionFlags : uint
            {
                AllowShadowsAndGels = 1 << 0,
                // turns on shadow casting
                ShadowCasting = 1 << 1,
                // only render when camera is 1st person
                RenderFirstPersonOnly = 1 << 2,
                // don't render when camera is 1st person
                RenderThirdPersonOnly = 1 << 3,
                // no rendering this light in splitscreen mode
                DontRenderSplitscreen = 1 << 4,
                // keep rendering this light when the attached player goes camo
                RenderWhileActiveCamo = 1 << 5,
                // overrides game settings that disable dynamic lights
                RenderInMultiplayerOverride = 1 << 6,
                // moves the light to match the camera
                MoveToCameraInFirstPerson = 1 << 7,
                // never cull this light because of low priority
                NeverPriorityCull = 1 << 8,
                AffectedByGameCanUseFlashlights = 1 << 9,
                // uses expensive specular lighting on screenspace lights
                ScreenspaceSpecularLighting = 1 << 10,
                // even it is dropped
                AlwaysOnForWeapon = 1 << 11
            }
            
            [TagStructure(Size = 0x24)]
            public class LightScalarFunctionStruct : TagStructure
            {
                public StringId InputVariable;
                public StringId RangeVariable;
                public OutputModEnum OutputModifier;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId OutputModifierInput;
                public MappingFunction Mapping;
                
                public enum OutputModEnum : short
                {
                    Unknown,
                    Plus,
                    Times
                }
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
            
            [TagStructure(Size = 0x24)]
            public class LightColorFunctionStruct : TagStructure
            {
                public StringId InputVariable;
                public StringId RangeVariable;
                public OutputModEnum OutputModifier;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId OutputModifierInput;
                public MappingFunction Mapping;
                
                public enum OutputModEnum : short
                {
                    Unknown,
                    Plus,
                    Times
                }
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
        }
    }
}
