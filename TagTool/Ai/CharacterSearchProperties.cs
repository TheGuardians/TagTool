using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x20)]
    public class CharacterSearchProperties : TagStructure
	{
        public CharacterSearchFlags Flags;
        public Bounds<float> SearchTime;
        public float SearchDistance;
        public Bounds<float> UncoverDistanceBounds;
        public Bounds<float> VocalizationTime;
    }
}
