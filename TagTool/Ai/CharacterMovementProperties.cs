using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x2C)]
    public class CharacterMovementProperties
    {
        public CharacterMovementFlags Flags;
        public float PathfindingRadius;
        public float DestinationRadius;
        public float DiveGrenadeChance;
        public AiSize ObstacleLeapMinimumSize;
        public AiSize ObstacleLeapMaximumSize;
        public AiSize ObstacleIgnoreSize;
        public AiSize ObstaceSmashableSize;
        public CharacterJumpHeight JumpHeight;
        public CharacterMovementHintFlags HintFlags;
        public float Unknown1;
        public float Unknown2;
        public float Unknown3;
    }
}
