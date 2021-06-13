using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "medal_challenge_aggregator_list", Tag = "mech", Size = 0xC)]
    public class MedalChallengeAggregatorList : TagStructure
    {
        public List<MedalchallengeAggregator> Lists;
        
        [TagStructure(Size = 0x18)]
        public class MedalchallengeAggregator : TagStructure
        {
            public StringId ChallengeToIncrement;
            public Medalaggregator Medals;
            
            [TagStructure(Size = 0x14)]
            public class Medalaggregator : TagStructure
            {
                public StringId DisplayName;
                public GameModeFlagsStruct AllowedGameModes;
                public List<MedalaggregatorEntry> ContributingMedals;
                
                [TagStructure(Size = 0x4)]
                public class GameModeFlagsStruct : TagStructure
                {
                    public GameTypeEnum GameMode;
                    public GameMatchmakingFlags MatchmakingType;
                    public GlobalCampaignDifficultyFlags Difficulty;
                    public GamePlayerCountFlags PlayerCount;
                    
                    [Flags]
                    public enum GameTypeEnum : byte
                    {
                        Campaign = 1 << 0,
                        Firefight = 1 << 1,
                        Multiplayer = 1 << 2
                    }
                    
                    [Flags]
                    public enum GameMatchmakingFlags : byte
                    {
                        CustomGame = 1 << 0,
                        Matchmaking = 1 << 1
                    }
                    
                    [Flags]
                    public enum GlobalCampaignDifficultyFlags : byte
                    {
                        Easy = 1 << 0,
                        Normal = 1 << 1,
                        Heroic = 1 << 2,
                        Legendary = 1 << 3
                    }
                    
                    public enum GamePlayerCountFlags : sbyte
                    {
                        Any,
                        _1PlayerOnly,
                        _4PlayersOnly,
                        MoreThanOnePlayer
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class MedalaggregatorEntry : TagStructure
                {
                    public StringId MedalName;
                }
            }
        }
    }
}
