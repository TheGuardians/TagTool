using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x18)]
    public class CharacterDialogueVariation : TagStructure
	{
        public CachedTag Dialogue;
        [TagField(Flags = Label)]
        public StringId Name;
        public float Weight;
    }
}
