using TagTool.Cache;
using TagTool.Tags;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using System;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x24)]
    public class CharacterEquipmentProperties : TagStructure
	{
        [TagField(Flags = Label)]
        public CachedTag Equipment;

        public CharacterEquipmentFlags Flags;
        public float RelativeDropChance; // The relative chance of this equipment being dropped with respect to the other pieces of equipment specified in this block
        public List<CharacterEquipmentUsageCondition> UseConditions;

        [Flags]
        public enum CharacterEquipmentFlags : uint
        {
            DefaultEquipment = 1 << 0
        }
    }
}
