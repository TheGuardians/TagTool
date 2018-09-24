using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x14)]
    public class AiMissionDialogueLine : TagStructure
	{
        [TagField(Label = true)]
        public StringId Name;
        public List<AiMissionDialogueLineVariant> Variants;
        public StringId DefaultSoundEffect;
    }
}
