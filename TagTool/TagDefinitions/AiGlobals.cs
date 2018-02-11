using TagTool.Ai;
using TagTool.Cache;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "ai_globals", Tag = "aigl", Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "ai_globals", Tag = "aigl", Size = 0x18, MinVersion = CacheVersion.HaloOnline106708)]
    public class AiGlobals
    {
        public List<AiGlobalsDatum> Data;

        [TagField(Padding = true, Length = 4, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;
    }
}
