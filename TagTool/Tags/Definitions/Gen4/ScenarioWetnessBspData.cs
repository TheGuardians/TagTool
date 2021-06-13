using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "scenario_wetness_bsp_data", Tag = "wetn", Size = 0x5C)]
    public class ScenarioWetnessBspData : TagStructure
    {
        public short BspReferenceIndex;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public int StructureBspImportChecksum;
        public int LightmapBspImportChecksum;
        public int DesignBspChecksum;
        public int AirProbeOffset;
        public int SenaryProbeOffset;
        public int MachineryOffset;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag WetnessPervertrexTextureReference;
        public List<WentnessBitVector> Wetness;
        public List<WentnessByteVector> WetnessInBytes;
        public List<ClusterWetnessOffsetBlock> ClusterOffset;
        public List<InstanceWetnessInstanceBlock> InstanceOffset;
        
        [TagStructure(Size = 0x4)]
        public class WentnessBitVector : TagStructure
        {
            public int Bits;
        }
        
        [TagStructure(Size = 0x1)]
        public class WentnessByteVector : TagStructure
        {
            public sbyte Bytes;
        }
        
        [TagStructure(Size = 0x4)]
        public class ClusterWetnessOffsetBlock : TagStructure
        {
            public int ClusterOffset;
        }
        
        [TagStructure(Size = 0x8)]
        public class InstanceWetnessInstanceBlock : TagStructure
        {
            public int ClusterOffset;
            public sbyte SingleProbe;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
        }
    }
}
