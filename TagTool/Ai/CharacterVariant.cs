using TagTool.Common;
using TagTool.Tags;
using System.Collections.Generic;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x14)]
    public class CharacterVariant : TagStructure
	{
        [TagField(Flags = TagFieldFlags.Label)]
        public StringId VariantName;
        public short VariantIndex;

        [TagField(Flags = TagFieldFlags.Padding, Length = 2)]
        public byte[] Unused;

        public List<CharacterDialogueVariation> DialogueVariations;
    }
}
