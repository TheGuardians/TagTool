using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x8)]
    public class CharacterChangeDirectionPause : TagStructure
	{
        public Angle DirectionChangeAngle;
        public int StationaryChange;
    }
}
