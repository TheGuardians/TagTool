using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0xC)]
    public class AiVocalizationResponse : TagStructure
	{
        [TagField(Flags = TagFieldFlags.Label)]
        public StringId VocalizationName;
        public AiVocalizationResponseFlags Flags;
        public short VocalizationIndex;
        public AiVocalizationResponseType ResponseType;
        public short ImportDialogueIndex;
    }
}
