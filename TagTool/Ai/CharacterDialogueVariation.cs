using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x18)]
    public class CharacterDialogueVariation : TagStructure
	{
        public CachedTagInstance Dialogue;
        [TagField(Flags = TagFieldFlags.Label)]
        public StringId Name;
        public float Weight;
    }
}
