using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "wind", Tag = "wind", Size = 0x40)]
    public class Wind : TagStructure
    {
        /// <summary>
        /// the wind magnitude in the weather region scales the wind between these bounds
        /// </summary>
        public Bounds<float> Velocity; // world units
        /// <summary>
        /// the wind direction varies inside a box defined by these angles on either side of the direction from the weather region.
        /// </summary>
        public RealEulerAngles2d VariationArea;
        public float LocalVariationWeight;
        public float LocalVariationRate;
        public float Damping;
        [TagField(Length = 0x24, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
    }
}

