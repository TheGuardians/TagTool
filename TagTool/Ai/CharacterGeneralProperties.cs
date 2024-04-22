using TagTool.Tags;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x1C)]
    public class CharacterGeneralProperties : TagStructure
	{
        public CharacterGeneralFlags Flags;
        public ActorTypeEnum ActorType;
        public short Rank; // The rank of this character, helps determine who should be a squad leader. (0 is lowly, 32767 is highest)
        public CombatPositioning FollowerPositioning; // Where should my followers try and position themselves when I am their leader?

        [TagField(Flags = Padding, Length = 2)]
        public byte[] Unused;

        public float MaximumLeaderDistance; // Don't let my combat range get outside this distance from my leader when in combat. (if 0 then defaults to 4wu)
        public float MaximumPlayerDialogueDistance; // Never play dialogue if all players are outside of this range. (if 0 then defaults to 20wu)
        public float Scariness; // The inherent scariness of the character.

        public CharacterDefaultGrenadeType DefaultGrenadeType;
        public CharacterBehaviorTreeRoot BehaviorTreeRoot; // Postgrenadepadding in h3mcc?
    }
}
