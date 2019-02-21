using TagTool.Cache;
using TagTool.Tags;
using System.Collections.Generic;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x1C)]
    public class CharacterFiringPatternProperties : TagStructure
	{
        [TagField(Flags = TagFieldFlags.Label)]
        public CachedTagInstance Weapon;
        public List<CharacterFiringPattern> FiringPatterns;
    }
}
