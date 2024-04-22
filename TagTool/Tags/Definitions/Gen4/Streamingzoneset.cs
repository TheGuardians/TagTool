using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "streamingzoneset", Tag = "SDzs", Size = 0x28)]
    public class Streamingzoneset : TagStructure
    {
        public RealPoint2d ResourceSubregionAabbMin;
        public RealVector2d ResourceSubregionBlockSize;
        public List<StreamingZoneSetResourceIdBlock> StreamingResourceIds;
        public List<StreamingZoneSetResourceSubregionDataBlock> StreamingResourceSubregions;
        
        [TagStructure(Size = 0x10)]
        public class StreamingZoneSetResourceIdBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag StreamingReferenceTag;
        }
        
        [TagStructure(Size = 0x1C)]
        public class StreamingZoneSetResourceSubregionDataBlock : TagStructure
        {
            public int BlockOffsetX;
            public int BlockOffsetY;
            public float MinZ;
            public float MaxZ;
            public List<StreamingZoneSetResourceLevelsBlock> ResourceLevels;
            
            [TagStructure(Size = 0x1)]
            public class StreamingZoneSetResourceLevelsBlock : TagStructure
            {
                public byte StreamingSubregionRequestedResolution;
            }
        }
    }
}
