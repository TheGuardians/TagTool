using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "lightmapper_globals", Tag = "LMgS", Size = 0x98)]
    public class LightmapperGlobals : TagStructure
    {
        public int Version;
        public GlobalLightmapGlobalSettingsStruct GlobalLightmapperSettings;
        public GlobalLightmapLocalSettingsStruct LocalLightmapperSettings;
        
        [TagStructure(Size = 0x38)]
        public class GlobalLightmapGlobalSettingsStruct : TagStructure
        {
            public LightmapGlobalFlags GlobalFlags;
            public LightmapModeEnum Mode;
            public LightmapHemicubeResolutionEnum HemicubeResolution;
            public LightmapDxtQualityEnum DxtCompressionQuality;
            public LightmapIndirectQualityEnum IndirectQuality;
            public LightmapDirectShadowMapResolutionEnum DirectShadowMapResolution;
            public LightmapSupersamplingFactorEnum SuperSamplingFactor;
            public LightmapAoQualityEnum AoSampleQuality;
            public RealVector3d IndirectRestrictAabbMin;
            public RealVector3d IndirectRestrictAabbMax;
            
            [Flags]
            public enum LightmapGlobalFlags : uint
            {
                ForceDraftLighting = 1 << 0,
                PerformSunMultisampling = 1 << 1,
                PerformSkyMultisampling = 1 << 2,
                UseHighResolutionPointShadows = 1 << 3,
                UseNewAtlaser = 1 << 4,
                UseHighQualityPacking = 1 << 5,
                IncludeGeometryFromAdjacentBsps = 1 << 6,
                Use32BitPrecisionForDirectPass = 1 << 7,
                EnableSubsamplingIndirectAcceleration = 1 << 8,
                RestrictLightmapSamplesToMayaRegions = 1 << 9,
                PerformRepack = 1 << 10,
                IsFarmBurn = 1 << 11,
                DesaturateLighting = 1 << 12
            }
            
            public enum LightmapModeEnum : int
            {
                DirectOnly,
                OneBounce,
                TwoBounce
            }
            
            public enum LightmapHemicubeResolutionEnum : int
            {
                _64X64,
                _128X128,
                _256X256,
                _512X512,
                _32X32,
                _16X16
            }
            
            public enum LightmapDxtQualityEnum : int
            {
                Draft,
                High
            }
            
            public enum LightmapIndirectQualityEnum : int
            {
                Low,
                Medium,
                High,
                VeryHigh
            }
            
            public enum LightmapDirectShadowMapResolutionEnum : int
            {
                _1x,
                _2x,
                _4x
            }
            
            public enum LightmapSupersamplingFactorEnum : int
            {
                _1x,
                _2x,
                _4x,
                _8x
            }
            
            public enum LightmapAoQualityEnum : int
            {
                Off,
                _1x,
                _2x,
                _4x
            }
        }
        
        [TagStructure(Size = 0x5C)]
        public class GlobalLightmapLocalSettingsStruct : TagStructure
        {
            public LightmapLocalFlags LocalFlags;
            public float SkydomeAmplificationFactor;
            public float IndirectAmplificationFactor;
            public float LightmapCompressionMaximum;
            public float PerVertexAoAutoThreshold;
            public GlobalLightmapAoSettingsStruct AoSettings;
            public GlobalLightmapGlobalIlluminationFalloffSettingsStruct GlobalIlluminationFalloffSettings;
            public GlobalLightmapLocalOverrideSettingsStruct LocalLightmapperOverrideSettings;
            public ScenarioStructureSizeEnum ForgeLightmapSizeClass;
            
            [Flags]
            public enum LightmapLocalFlags : uint
            {
                EnableFloatingShadows = 1 << 0,
                ReplaceSunWithBrightestDirectional = 1 << 1,
                UseLegacyMagicLightScalars = 1 << 2,
                RemoveLightsOutsideTargetBsp = 1 << 3,
                IncludeSkydome = 1 << 4,
                IncludeSun = 1 << 5,
                PerformBadPixelAnalysis = 1 << 6,
                EnableAo = 1 << 7,
                GenerateNewAo = 1 << 8,
                IgnoreSunSolidAngle = 1 << 9,
                OverrideAoOnly = 1 << 10,
                PerformAnalysis = 1 << 11,
                GlobalIlluminationFalloff = 1 << 12,
                GenerateBvhAo = 1 << 13,
                GenerateBvhAo1 = 1 << 14,
                BuildPackingLightmaps = 1 << 15,
                AttemptToUseDiffuseUvs = 1 << 16,
                CorrectedLightLinking = 1 << 17,
                BurnSimplifiedIrradianceLighting = 1 << 18,
                DisableFloatingShadowGeometry = 1 << 19,
                DisableHybridRefinement = 1 << 20,
                BurnProbeAoLighting = 1 << 21,
                OptOutPerVertexLightingFix = 1 << 22,
                GenerateAdjacentBounceInfo = 1 << 23,
                NoShadowNoAoFix = 1 << 24,
                AllowSpotSuns = 1 << 25,
                ProbesIgnoreShadowLinking = 1 << 26
            }
            
            public enum ScenarioStructureSizeEnum : int
            {
                _32x32,
                _64x64,
                _128x128,
                _256x25604Meg,
                _512x51215Meg,
                _768x76834Meg,
                _1024x10246Meg,
                _1280x128094Meg,
                _1536x1536135Meg,
                _1792x1792184meg
            }
            
            [TagStructure(Size = 0x10)]
            public class GlobalLightmapAoSettingsStruct : TagStructure
            {
                public float Radius;
                public float FalloffPower;
                public float Multiplier;
                public float MaxThreadCount;
            }
            
            [TagStructure(Size = 0x28)]
            public class GlobalLightmapGlobalIlluminationFalloffSettingsStruct : TagStructure
            {
                public float FalloffBegin;
                public float FalloffEnd;
                public RealRgbColor FarLightingColor;
                public float FarLightingScalar;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag FarLightingTexture;
            }
            
            [TagStructure(Size = 0xC)]
            public class GlobalLightmapLocalOverrideSettingsStruct : TagStructure
            {
                public int HighQualityAverageJitterSamples;
                public int PerVertexSupersampleCount;
                public int IndirectQualityOffset;
            }
        }
    }
}
