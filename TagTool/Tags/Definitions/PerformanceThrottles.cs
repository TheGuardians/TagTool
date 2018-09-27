using TagTool.Cache;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "performance_throttles", Tag = "perf", Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "performance_throttles", Tag = "perf", Size = 0x10, MinVersion = CacheVersion.HaloOnline106708)]
    public class PerformanceThrottles : TagStructure
	{
        public List<PerformanceBlock> Performance;

        [TagField(Padding = true, Length = 4, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;

        [TagStructure(Size = 0x38)]
        public class PerformanceBlock : TagStructure
		{
            public uint Flags;
            public float Water;
            public float Decorators;
            public float Effects;
            public float InstancedGeometry;
            public float ObjectFade;
            public float ObjectLod;
            public float Decals;
            public int CpuLightCount;
            public float CpuLightQuality;
            public int GpuLightCount;
            public float GpuLightQuality;
            public int ShadowCount;
            public float ShadowQuality;
        }
    }
}