using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "performance_throttles", Tag = "perf", Size = 0xC)]
    public class PerformanceThrottlesDef : TagStructure
    {
        public List<PerformaneThrottleBlock> PerformanceThrottles;
        
        [TagStructure(Size = 0x44)]
        public class PerformaneThrottleBlock : TagStructure
        {
            public PerformanceThrottleFlags Flags;
            public float WaterTessellationScale;
            public float DecoratorFadeDistScale; // 0 = off
            public float EffectLodDistanceScale;
            public float InstanceFadeModifier;
            // scales down the distances at which objects first imposter and then fade out
            public float ObjectFadeModifier;
            public float ObjectDetailFadeModifier;
            // per frame time limit to spend sampling new lighting radiosity
            public float ObjectLightingTimeLimit; // s 0=0.001s
            // scales down the distances at which IOs imposter
            public float IoFadeModifier;
            // will quickly fade gpu lights when we try to render more than this many
            public int MaxForwardDynamicLights; // 0 = off
            // scales the size used for distance-fade (set smaller to make it fade earlier)
            public float ForwardDynamicLightFadeDistanceScale;
            // will quickly fade screenspace lights when we try to render more than this many
            public int MaxScreenspaceDynamicLights; // 0 = off
            // scales the size used for distance-fade (set smaller to make it fade earlier)
            public float ScreenspaceLightFadeDistanceScale;
            // limits the number of effect lights we allow to be active at a time (eg. needler needles lighting up objects)
            public int MaxEffectLights; // 0 = off
            public int MaxShadowCastingObjects; // 0 = off
            // scales resolution of object shadows
            public float ShadowQualityLod; // [0.0 to 1.0]
            // scales resolution of floating shadow
            public float FloatingShadowQualityLod; // [0.0 to 1.0], 0 = off
            
            [Flags]
            public enum PerformanceThrottleFlags : uint
            {
                DisableObjectAttachmentLights = 1 << 0,
                DisableFirstPersonShadow = 1 << 1,
                DisableCheapParticles = 1 << 2,
                DisableBlobShadows = 1 << 3,
                DisablePatchyFog = 1 << 4,
                DisableScreenDistortion = 1 << 5,
                DisableLightShafts = 1 << 6,
                DisableFirstPersonDepthOfField = 1 << 7,
                DisableMotionBlur = 1 << 8,
                DisableParticlesContinueOffscreen = 1 << 9,
                DisableLightCones = 1 << 10,
                DisableWaterInterraction = 1 << 11,
                DisableWaterRefraction = 1 << 12,
                DisableDecorators = 1 << 13,
                // WARNING Will likely hurt perf on most maps
                DisableInstanceOcclusionQueries = 1 << 14
            }
        }
    }
}
