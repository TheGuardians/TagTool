using TagTool.Common;
using TagTool.Tags;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0xC)]
    public class AiVocalizationResponse : TagStructure
	{
        [TagField(Flags = Label)]
        public StringId VocalizationName;
        public AiVocalizationResponseFlags Flags;
        public short VocalizationIndex;
        public AiVocalizationResponseType ResponseType;
        public short ImportDialogueIndex;
    }
}
