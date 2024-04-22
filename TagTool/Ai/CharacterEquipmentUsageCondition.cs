using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0xC)]
    public class CharacterEquipmentUsageCondition : TagStructure
	{
        public CharacterEquipmentUsageWhenEnum UseWhen; // When should we use this equipment?
        public CharacterEquipmentUsageHowEnum UseHow; // How should we use this equipment?
        public float Easynormal; // 0-1
        public float Legendary; // 0-1

        public enum CharacterEquipmentUsageWhenEnum : short
        {
            Combat,
            Cover,
            Shield,
            Health,
            Uncover,
            Berserk,
            Investigate,
            AntiVehicle
        }

        public enum CharacterEquipmentUsageHowEnum : short
        {
            AttachToSelf,
            ThrowAtEnemy,
            ThrowAtFeet,
            UseOnSelf
        }
    }
}
