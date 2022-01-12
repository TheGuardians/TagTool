using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x18)]
    public class CharacterScarabProperties : TagStructure
	{
        public float FightingMinDistance; // When target within this distance, the scarab will back up (world units)
        public float FightingMaxDistance; // When target outside this distance, the scarab will chase (world units)
        public Bounds<float> AnticipatedAimRadius; // When within these bounds distance from the target, we blend in our anticipated facing vector (world units)
        public float SnapForwardAngle; // When moving forward within this dot of our desired facing, just move forward [0-1]
        public float SnapForwardAngleMax; // When moving forward within this dot of our desired facing, just move forward [0-1]
    }
}