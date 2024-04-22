using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x18)]
    public class AiMissionDialogueLineVariant : TagStructure
	{
        [TagField(Flags = Label)]
        public StringId Designation; // 3-letter designation for the character
        public CachedTag Sound;
        public StringId SoundEffect;
    }
}
