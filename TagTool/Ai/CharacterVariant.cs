using TagTool.Common;
using TagTool.Tags;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x14)]
    public class CharacterVariant : TagStructure
	{
        [TagField(Flags = Label)]
        public StringId VariantName;
        public short VariantIndex;

        [TagField(Flags = Padding, Length = 2)]
        public byte[] Unused;

        public List<CharacterDialogueVariation> DialogueVariations;
    }
}
