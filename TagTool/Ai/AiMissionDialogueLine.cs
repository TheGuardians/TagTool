using TagTool.Common;
using TagTool.Tags;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x14)]
    public class AiMissionDialogueLine : TagStructure
	{
        [TagField(Flags = Label)]
        public StringId Name;
        public List<AiMissionDialogueLineVariant> Variants;
        public StringId DefaultSoundEffect;
    }
}
