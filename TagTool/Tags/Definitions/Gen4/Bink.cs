using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "bink", Tag = "bink", Size = 0x1C)]
    public class Bink : TagStructure
    {
        public int FrameCount;
        public TagResourceReference BinkResource;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag ExternalSoundTrack;
    }
}
