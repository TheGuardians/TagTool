using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x14)]
    public class CharacterVariant : TagStructure
	{
        [TagField(Label = true)]
        public StringId VariantName;
        public short VariantIndex;

        [TagField(Padding = true, Length = 2)]
        public byte[] Unused;

        public List<CharacterDialogueVariation> DialogueVariations;
    }
}
