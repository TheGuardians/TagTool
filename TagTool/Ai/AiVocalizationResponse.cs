using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0xC)]
    public class AiVocalizationResponse : TagStructure
	{
        [TagField(Label = true)]
        public StringId VocalizationName;
        public AiVocalizationResponseFlags Flags;
        public short VocalizationIndex;
        public AiVocalizationResponseType ResponseType;
        public short ImportDialogueIndex;
    }
}
