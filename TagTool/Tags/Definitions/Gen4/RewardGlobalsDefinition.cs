using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "reward_globals_definition", Tag = "cook", Size = 0xC)]
    public class RewardGlobalsDefinition : TagStructure
    {
        public List<RewarddefinitionBlock> RewardDefinitions;
        
        [TagStructure(Size = 0x14)]
        public class RewarddefinitionBlock : TagStructure
        {
            public StringId Name;
            public RewardtypeEnum Type;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<Rewardblock> Rewards;
            
            public enum RewardtypeEnum : sbyte
            {
                Invasion,
                Bounties,
                SlotMachine,
                Heat,
                MissionScripting,
                Commendation,
                DailyChallenge,
                Achievement,
                ConsoleScripting,
                GameComplete,
                TimeSpent,
                Score,
                Winning,
                Hopper,
                SlotMachinelspJackpot,
                DoubleXp,
                FastTrackArmor
            }
            
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
