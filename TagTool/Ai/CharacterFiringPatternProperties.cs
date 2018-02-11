using TagTool.Cache;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x1C)]
    public class CharacterFiringPatternProperties
    {
        [TagField(Label = true)]
        public CachedTagInstance Weapon;
        public List<CharacterFiringPattern> FiringPatterns;
    }
}
