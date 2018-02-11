using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x8)]
    public class CharacterReadyProperties
    {
        public Bounds<float> ReadTimeBounds;
    }
}
