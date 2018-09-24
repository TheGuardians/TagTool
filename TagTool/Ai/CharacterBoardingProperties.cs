using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x14)]
    public class CharacterBoardingProperties : TagStructure
	{
        public CharacterBoardingFlags Flags;
        public float MaximumDistance;
        public float AbortDistance;
        public float MaximumSpeed;
        public float BoardTime;
    }
}
