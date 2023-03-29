using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using static TagTool.Tags.Definitions.Effect.Event.ParticleSystem.Emitter.RuntimeMGpuData;

namespace TagTool.Tags.Definitions.Common
{
    [TagStructure(Size = 0x24, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x24, Platform = CachePlatform.MCC, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0x10, Platform = CachePlatform.MCC, MinVersion = CacheVersion.Halo3ODST)]
    public class RuntimeGpuData : TagStructure
    {

        [TagField(Platform = CachePlatform.MCC, MinVersion = CacheVersion.Halo3ODST)]
        public int StructuredScenarioInteropIndex;

        [TagField(Platform = CachePlatform.Original)]
        [TagField(Platform = CachePlatform.MCC, MaxVersion = CacheVersion.Halo3Retail)]
        public List<RealVector4dBlock> Properties;
        [TagField(Platform = CachePlatform.Original)]
        [TagField(Platform = CachePlatform.MCC, MaxVersion = CacheVersion.Halo3Retail)]
        public List<GpuFunctionBlock> Functions;
        [TagField(Platform = CachePlatform.Original)]
        [TagField(Platform = CachePlatform.MCC, MaxVersion = CacheVersion.Halo3Retail)]
        public List<RealVector4dBlock> Colors;

        [TagField(Platform = CachePlatform.MCC, MinVersion = CacheVersion.Halo3ODST)]
        public List<ODSTMCCGpuBlock> RuntimeGpuBlocks; // contains the above blocks in ODSTMCC

        [TagStructure(Size = 0x40, Align = 0x10)]
        public class GpuFunctionBlock : TagStructure
        {
            [TagField(Length = 4)]
            public RealVector4dBlock[] Elements;
        }

        [TagStructure(Size = 0x10, Align = 0x10)]
        public class RealVector4dBlock : TagStructure
        {
            public RealRgbColor Color;
            public float Magnitude;
        }

        [TagStructure(Size = 0x24)]
        public class ODSTMCCGpuBlock : TagStructure
        {
            public List<RealVector4dBlock> Properties;
            public List<GpuFunctionBlock> Functions;
            public List<RealVector4dBlock> Colors;

        }
    }
}
