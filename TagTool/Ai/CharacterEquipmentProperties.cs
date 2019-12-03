using TagTool.Cache;
using TagTool.Tags;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x24)]
    public class CharacterEquipmentProperties : TagStructure
	{
        [TagField(Flags = Label)]
        public CachedTagInstance Equipment;
        public uint Unknown;
        public float UsageChance;
        public List<CharacterEquipmentUsageCondition> UsageConditions;
    }
}
