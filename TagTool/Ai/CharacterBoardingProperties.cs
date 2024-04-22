using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x14)]
    public class CharacterBoardingProperties : TagStructure
	{
        public CharacterBoardingFlags Flags;
        public float MaximumDistance; // maximum distance from entry point that we will consider boarding (world units)
        public float AbortDistance; // give up trying to get in boarding seat if entry point is further away than this (world units)
        public float MaximumSpeed; // maximum speed at which we will consider boarding (world units)
        public float BoardTime; // maximum time we will melee board for (seconds)
    }
}
