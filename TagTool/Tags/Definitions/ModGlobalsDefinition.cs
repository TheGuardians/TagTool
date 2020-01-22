using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "mod_globals", Tag = "modg", Size = 0xC)]
    public class ModGlobalsDefinition : TagStructure
    {
        public List<PlayerCharacterSet> PlayerCharacterSets;

        [TagStructure(Size = 0x34)]
        public class PlayerCharacterSet : TagStructure
        {
            [TagField(Length = 32)]
            public string DisplayName;
            public StringId Name;
            public float RandomChance;
            public List<PlayerCharacter> Characters;

            [TagStructure(Size = 0x28)]
            public class PlayerCharacter : TagStructure
            {
                [TagField(Length = 32)]
                public string DisplayName;
                public StringId Name;
                public float RandomChance;
            }
        }
    }
}
