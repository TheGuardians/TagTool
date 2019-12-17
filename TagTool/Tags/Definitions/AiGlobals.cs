using TagTool.Ai;
using TagTool.Cache;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "ai_globals", Tag = "aigl", Size = 0xC)]
    public class AiGlobals : TagStructure
	{
        public List<AiGlobalsDatum> Data;
    }
}
