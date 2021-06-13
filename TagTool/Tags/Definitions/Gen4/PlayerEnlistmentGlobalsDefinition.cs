using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "player_enlistment_globals_definition", Tag = "pegd", Size = 0xC)]
    public class PlayerEnlistmentGlobalsDefinition : TagStructure
    {
        public List<PlayerenlistmentDefinitionBlock> Enlistments;
        
        [TagStructure(Size = 0x3C)]
        public class PlayerenlistmentDefinitionBlock : TagStructure
        {
            // the string id of the name of this enlistment
            public StringId Name;
            // the string id of the description of this enlistment
            public StringId Description;
            // the sprite index of the icon for this enlistment
            public short SpriteIndex;
            public PlayerenlistmentFlags Flags;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public StringId UnlockedEmblemFg;
            public StringId UnlockedEmblemBg;
            public StringId UnlockedHelmet;
            public StringId UnlockedChest;
            public StringId UnlockedLeftShoulder;
            public StringId UnlockedRightShoulder;
            public StringId UnlockedArms;
            public StringId UnlockedLegs;
            public StringId UnlockedVisor;
            // the grades that define the leveling track for this enlistment
            public List<PlayerGradeDefinitionBlock> Grades;
            
            [Flags]
            public enum PlayerenlistmentFlags : byte
            {
                // since we can't reorder the list after ship, this allows us to disable/enable this enlistment
                Disabled = 1 << 0
            }
            
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
}
