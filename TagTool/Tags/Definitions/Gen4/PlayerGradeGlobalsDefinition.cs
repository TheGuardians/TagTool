using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "player_grade_globals_definition", Tag = "pggd", Size = 0xC)]
    public class PlayerGradeGlobalsDefinition : TagStructure
    {
        public List<PlayerGradeDefinitionBlock> PlayerGrades;
        
        [TagStructure(Size = 0x1C)]
        public class PlayerGradeDefinitionBlock : TagStructure
        {
            // total earned XP needed to reach this rank
            public int XpThreshold;
            public StringId Name;
            public short SpriteIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // This is an additional multiplier to rewards given for time played for players at this grade
            public float TimePlayedMultiplier;
            // These rewards will be given to the player when they reach this grade
            public List<Rewardblock> LevelUpRewards;
            
            [TagStructure(Size = 0x4)]
            public class Rewardblock : TagStructure
            {
                // Type of currency given by this reward.
                public CurrencytypeEnum CurrencyType;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                // Amount of the given currency to be given with this reward.
                public short RewardAmount;
                
                public enum CurrencytypeEnum : sbyte
                {
                    Cookies,
                    Xp
                }
            }
        }
    }
}
