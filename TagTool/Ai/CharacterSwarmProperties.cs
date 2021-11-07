using TagTool.Common;
using TagTool.Tags;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x38)]
    public class CharacterSwarmProperties : TagStructure
	{
        public short ScatterKilledCount;

        [TagField(Flags = Padding, Length = 2)]
        public byte[] Unused;

        public float ScatterRadius; // the distance from the target that the swarm scatters
        public float ScatterTime; // amount of time to remain scattered
        public Bounds<float> HoundDistance;
        public Bounds<float> InfectionTime; // how long the infection form and its victim will wrestle before the point of no return
        public float PerlinOffsetScale; // amount of randomness added to creature's throttle [0-1]
        public Bounds<float> OffsetPeriod; // how fast the creature changes random offset to throttle (seconds)
        public float PerlinIdleMovementThreshold; // a random offset lower then given threshold is made 0. (threshold of 1 = no movement)
        public float PerlinCombatMovementThreshold; // a random offset lower then given threshold is made 0. (threshold of 1 = no movement)
        public float StuckTime; // how long we have to move (stuck distance) before we get deleted
        public float StuckDistance; // how far we have to move in (stuck time) to not get deleted
    }
}
