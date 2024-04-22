using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
	[TagStructure(Name = "performance_throttles", Tag = "perf", Size = 0xC)]
	public class PerformanceThrottles : TagStructure
	{
        // block 0: default non-splitscreen
        // block 1: two-way splitscreen
        // block 2: three-way splitscreen
        // block 3: four-way splitscreen

		public List<PerformanceThrottleBlock> PerformanceThrottle;
		
		[TagStructure(Size = 0x38, MinVersion = CacheVersion.Halo3Beta, Platform = CachePlatform.Original)]
		[TagStructure(Size = 0x3C, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
		public class PerformanceThrottleBlock : TagStructure
		{
			public PerformanceThrottleFlags Flags;
			public float WaterLod;
			public float DecoratorFadeDistanceScale; // 0 = off
			public float EffectLodDistanceScale;
			public float InstancedGeometryFadeModifier;
			public float ObjectFadeModifier;
			public float ObjectLodThresholdModifier;
			public float DecalFadeDistanceScale;
			public int MaxCpuDynamicLights; // will quickly fade cpu lights when we try to render more than this many
			public float CpuLightFadeDistanceScale; // scales the size used for distance-fade (set smaller to make it fade earlier)
			public int MaxGpuDynamicLights; // will quickly fade gpu lights when we try to render more than this many
			public float GpuLightFadeDistanceScale; // scales the size used for distance-fade (set smaller to make it fade earlier)
			public int MaxShadowCastingObjects; // 0 = off
			public float ShadowQualityLod;
			
			[TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
			public float AnisotropyLevel;
			
			[Flags]
			public enum PerformanceThrottleFlags : uint
			{
			    BloomIgnoreLDR = 1 << 0,
			    DisableObjectPRT = 1 << 1,
			    DisableFirstPersonShadow = 1 << 2,
			    HighQualityShadows = 1 << 3,
			    DisableFxaa = 1 << 4,
			    DisableMotionBlur = 1 << 5,
			    DisableHeavyPerfEffectsxbox = 1 << 6,
			    MultithreadedVisibility = 1 << 7
			}
		}
	}
}
