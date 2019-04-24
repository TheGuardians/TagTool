using TagTool.Ai;
using TagTool.Cache;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "ai_globals", Tag = "aigl", Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "ai_globals", Tag = "aigl", Size = 0x10, MinVersion = CacheVersion.HaloOnline106708)]
    public class AiGlobals : TagStructure
	{
        public List<AiGlobalsDatum> Data;

        [TagField(Flags = Padding, Length = 4, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;
    }
}
