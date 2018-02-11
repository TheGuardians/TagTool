using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x20)]
    public class CharacterSearchProperties
    {
        public CharacterSearchFlags Flags;
        public Bounds<float> SearchTime;
        public float SearchDistance;
        public Bounds<float> UncoverDistanceBounds;
        public Bounds<float> VocalizationTime;
    }
}
