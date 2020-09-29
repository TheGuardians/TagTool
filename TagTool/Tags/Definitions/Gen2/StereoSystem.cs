using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "stereo_system", Tag = "BooM", Size = 0x4)]
    public class StereoSystem : TagStructure
    {
        public int Unused;
    }
}

