using TagTool.Ai;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "ai_mission_dialogue", Tag = "mdlg", Size = 0xC)]
    public class AiMissionDialogue : TagStructure
	{
        public List<AiMissionDialogueLine> Lines;
    }
}
