using TagTool.Cache;
using TagTool.Tags;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x1C)]
    public class CharacterFiringPatternProperties : TagStructure
	{
        [TagField(Flags = Label)]
        public CachedTag Weapon;
        public List<CharacterFiringPattern> FiringPatterns;
    }
}
