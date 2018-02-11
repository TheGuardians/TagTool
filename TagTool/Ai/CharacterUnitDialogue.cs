using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Ai
{
    [TagStructure(Size = 0xC)]
    public class CharacterUnitDialogue
    {
        public List<CharacterDialogueVariation> DialogueVariations;
    }
}
