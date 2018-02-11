using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x8)]
    public class CharacterChangeDirectionPause
    {
        public Angle DirectionChangeAngle;
        public int StationaryChange;
    }
}
