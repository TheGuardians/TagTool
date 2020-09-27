using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "wind", Tag = "wind", Size = 0x40)]
    public class Wind : TagStructure
    {
        public Bounds<float> Velocity; // world units
        public RealEulerAngles2d VariationArea; // the wind direction varies inside a box defined by these angles on either side of the direction from the weather region.
        public float LocalVariationWeight;
        public float LocalVariationRate;
        public float Damping;
        [TagField(Flags = Padding, Length = 36)]
        public byte[] Padding1;
    }
}

