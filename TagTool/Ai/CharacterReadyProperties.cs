using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x8)]
    public class CharacterReadyProperties : TagStructure
	{
        public Bounds<float> ReadTimeBounds;
    }
}
