using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "game_completion_rewards_globals", Tag = "gcrg", Size = 0x28)]
    public class GameCompletionRewardsGlobals : TagStructure
    {
        // rewards given for playing campaign games online
        public List<GameCompletionRewardsDifficultyBlock> Campaign;
        // rewards given for playing firefight games online
        public List<GameCompletionRewardsDifficultyBlock> Firefight;
        // rewards given for playing PvP multiplayer games online
        public List<GameCompletionRewardsMultiplayerBlock> Multiplayer;
        // this multiplier is applied to the combined reward of timespent, score and performance, multiplied by the percentage
        // time the player has the armor mod active
        public float FastTrackArmorModifier;
        
        [TagStructure(Size = 0x3C)]
        public class GameCompletionRewardsDifficultyBlock : TagStructure
        {
            // this block is used for easy matchmade games
            public List<GameCompletionRewardsDefinitionBlock> EasyMatchmaking;
            // this block is used for normal matchmade games
            public List<GameCompletionRewardsDefinitionBlock> NormalMatchmaking;
            // this block is used for heroic matchmade games
            public List<GameCompletionRewardsDefinitionBlock> HeroicMatchmaking;
            // this block is used for legendary matchmade games
            public List<GameCompletionRewardsDefinitionBlock> LegendaryMatchmaking;
            // this block is used for custom games
            public List<GameCompletionRewardsDefinitionBlock> Custom;
            
            [TagStructure(Size = 0x20)]
            public class GameCompletionRewardsDefinitionBlock : TagStructure
            {
                // base amount of reward given for each minute of play up until the start of the falloff curve
                public int InitialAmountPerMinute;
                // the player's time-based reward value is multiplied by this factor before being awarded; this value can be
                // overridden by the hopper
                public float HopperScalingFactor;
                // if the player is an unambiguous winner, their time-based reward value is multiplied by (this factor - 1) and the
                // result is awarded as a bonus; this value can be overridden by the hopper
                public float WinnerScalingFactor;
                // if the player is not a winner, but is in the top half of the standings, their time-based reward is multiplied by
                // (this factor - 1) and the result is awarded as a bonus; this value can be overriden by the hopper
                public float PerformanceScalingFactor;
                // for score-based modes, the player's normalized score (0..1) is multiplied by this scaling factor and the result is
                // awarded to the player as a bonus; this value can be overridden by the hopper
                public float ScoreScalingFactor;
                // amount per minute falloff curve; used to provide diminishing returns for longer play time
                public List<GameCompletionRewardsFalloffPointBlock> ApmFalloffCurve;
                
                [TagStructure(Size = 0x4)]
                public class GameCompletionRewardsFalloffPointBlock : TagStructure
                {
                    // minutes into the game after which this new reward rate applies
                    public short StartTime;
                    // points awarded per minute once the given time is reached
                    public short AmountPerMinute;
                }
            }
        }
        
        [TagStructure(Size = 0x3C)]
        public class GameCompletionRewardsMultiplayerBlock : TagStructure
        {
            // this block is used for matchmade games
            public List<GameCompletionRewardsDefinitionBlock> Matchmaking;
            // this block is used for custom games
            public List<GameCompletionRewardsDefinitionBlock> Custom;
            public List<GameCompletionRewardsDefinitionBlock> Unused0;
            public List<GameCompletionRewardsDefinitionBlock> Unused1;
            public List<GameCompletionRewardsDefinitionBlock> Unused2;
            
            [TagStructure(Size = 0x20)]
            public class GameCompletionRewardsDefinitionBlock : TagStructure
            {
                // base amount of reward given for each minute of play up until the start of the falloff curve
                public int InitialAmountPerMinute;
                // the player's time-based reward value is multiplied by this factor before being awarded; this value can be
                // overridden by the hopper
                public float HopperScalingFactor;
                // if the player is an unambiguous winner, their time-based reward value is multiplied by (this factor - 1) and the
                // result is awarded as a bonus; this value can be overridden by the hopper
                public float WinnerScalingFactor;
                // if the player is not a winner, but is in the top half of the standings, their time-based reward is multiplied by
                // (this factor - 1) and the result is awarded as a bonus; this value can be overriden by the hopper
                public float PerformanceScalingFactor;
                // for score-based modes, the player's normalized score (0..1) is multiplied by this scaling factor and the result is
                // awarded to the player as a bonus; this value can be overridden by the hopper
                public float ScoreScalingFactor;
                // amount per minute falloff curve; used to provide diminishing returns for longer play time
                public List<GameCompletionRewardsFalloffPointBlock> ApmFalloffCurve;
                
                [TagStructure(Size = 0x4)]
                public class GameCompletionRewardsFalloffPointBlock : TagStructure
                {
                    // minutes into the game after which this new reward rate applies
                    public short StartTime;
                    // points awarded per minute once the given time is reached
                    public short AmountPerMinute;
                }
            }
        }
    }
}
