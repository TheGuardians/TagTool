using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x1C)]
    public class CharacterGeneralProperties : TagStructure
	{
        public CharacterGeneralFlags Flags;

        public AiActorType ActorType;

        /// <summary>
        /// The rank of this character, helps determine who should be a squad leader. (0 is lowly, 32767 is highest)
        /// </summary>
        public short Rank;

        /// <summary>
        /// Where should my followers try and position themselves when I am their leader?
        /// </summary>
        public AiFollowerPositioning FollowerPositioning;

        [TagField(Padding = true, Length = 2)]
        public byte[] Unused;

        /// <summary>
        /// Don't let my combat range get outside this distance from my leader when in combat. (if 0 then defaults to 4wu)
        /// </summary>
        public float MaximumLeaderDistance;

        /// <summary>
        /// Never play dialogue if all players are outside of this range. (if 0 then defaults to 20wu)
        /// </summary>
        public float MaximumPlayerDialogueDistance;

        /// <summary>
        /// The inherent scariness of the character.
        /// </summary>
        public float Scariness;

        public CharacterDefaultGrenadeType DefaultGrenadeType;
        public CharacterBehaviorTreeRoot BehaviorTreeRoot;
    }
}
