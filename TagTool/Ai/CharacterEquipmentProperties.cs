using TagTool.Cache;
using TagTool.Tags;
using System.Collections.Generic;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x24)]
    public class CharacterEquipmentProperties : TagStructure
	{
        [TagField(Label = true)]
        public CachedTagInstance Equipment;
        public uint Unknown;
        public float UsageChance;
        public List<CharacterEquipmentUsageCondition> UsageConditions;
    }
}
