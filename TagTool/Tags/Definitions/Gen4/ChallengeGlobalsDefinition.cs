using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "challenge_globals_definition", Tag = "chdg", Size = 0x1C)]
    public class ChallengeGlobalsDefinition : TagStructure
    {
        [TagField(ValidTags = new [] { "mech" })]
        public CachedTag MedalAggregators;
        public List<ChallengeCategoryBlock> ChallengeCategories;
        
        [TagStructure(Size = 0x14)]
        public class ChallengeCategoryBlock : TagStructure
        {
            public StringId CategoryName;
            public ChallengeCategoryEnum ChallengeCategory;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<ChallengeBlockStruct> Challenges;
            
            public enum ChallengeCategoryEnum : sbyte
            {
                Campaign,
                Competitive,
                SpartanOps,
                Waypoint
            }
            
            [TagStructure(Size = 0x54)]
            public class ChallengeBlockStruct : TagStructure
            {
                public StringId ChallengeName;
                // in the UI, name and description
                public StringId DisplayString;
                // in the UI, name
                public StringId DisplayName;
                // in the UI, description
                public StringId DisplayDescription;
                // in the UI, the text on the toast when you complete this challenge
                public StringId CompletionToastString;
                // How many times this challenge must be progressed to complete it (unless overridden by LSP).
                public int RequiredProgressCount;
                // cookies for completing this challenge; can be overridden by LSP
                public int CookiesReward;
                // XP for completing this challenge; can be overridden by LSP
                public int XpReward;
                // Frequency we toast your progress (think pink and deadly).
                public int ProgressTrackingInterval;
                // progress toast string (presumably very concise).
                public StringId ChudProgressString;
                // What icon is shown in the progress toast?
                public int ChudProgressBitmapSequenceIndex;
                public ChallengeDefinitionFlags Flags;
                public ChallengeiconDefinition Icon;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                // Conditions that progress this challenge magically (aside from incidents, which can progress any challenge)
                public ChallengeProgressFlags AutoProgressOn;
                public GameModeFlagsStruct PermittedGameTypes;
                // can only be progressed on this level, if specified
                public StringId LevelName;
                // can only be progressed on this map, if > 0 (only works for campaign and spartan ops)
                public int MapId;
                // can only be progressed on this mission, if >= 0 (only works for spartan ops)
                public short MissionId;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                // this challenge can only be progressed with at least these skulls enabled in the game options (only works for
                // campaign)
                public SkullFlags Skulls;
                // must score at least this many points, if > 0 (only works for campaign)
                public int MinimumScore;
                // must die no more than this many times, if >= 0
                public int MaximumPlayerDeathCount;
                // must complete the level in no more than this many seconds, if > 0
                public int MaximumLevelCompletionSeconds;
                
                [Flags]
                public enum ChallengeDefinitionFlags : byte
                {
                    ProgressResetsAtEndOfGame = 1 << 0
                }
                
                public enum ChallengeiconDefinition : sbyte
                {
                    Iron,
                    Bronze,
                    Silver,
                    Gold,
                    Onyx,
                    Daily,
                    Weekly
                }
                
                [Flags]
                public enum ChallengeProgressFlags : uint
                {
                    MatchmakingMultiplayerGameWon = 1 << 0,
                    CampaignMissionBeaten = 1 << 1,
                    MatchmakingMultiplayerGameCompleted = 1 << 2,
                    CompleteSingleDailyChallenge = 1 << 3,
                    CompleteSingleWeeklyChallenge = 1 << 4,
                    CompleteSingleMonthlyChallenge = 1 << 5,
                    CompleteAllDailyChallengesInADay = 1 << 6,
                    UploadFilmClipThatYouAuthored = 1 << 7,
                    WearAPieceOfArmorInArmory = 1 << 8,
                    // progresses the instant all requirements are met
                    AutoProgress = 1 << 9,
                    SpartanOpsMissionBeaten = 1 << 10,
                    SpartanOpsCompletedAllWeeklyMissionsOnEasy = 1 << 11,
                    SpartanOpsCompletedAllWeeklyMissionsOnNormal = 1 << 12,
                    SpartanOpsCompletedAllWeeklyMissionsOnHard = 1 << 13,
                    SpartanOpsCompletedAllWeeklyMissionsOnImpossible = 1 << 14
                }
                
                [Flags]
                public enum SkullFlags : uint
                {
                    SkullIron = 1 << 0,
                    SkullBlackEye = 1 << 1,
                    SkullToughLuck = 1 << 2,
                    SkullCatch = 1 << 3,
                    SkullFog = 1 << 4,
                    SkullFamine = 1 << 5,
                    SkullThunderstorm = 1 << 6,
                    SkullTilt = 1 << 7,
                    SkullMythic = 1 << 8,
                    SkullAssassin = 1 << 9,
                    SkullBlind = 1 << 10,
                    SkullSuperman = 1 << 11,
                    SkullBirthdayParty = 1 << 12,
                    SkullDaddy = 1 << 13,
                    SkullRed = 1 << 14,
                    SkullYellow = 1 << 15,
                    SkullBlue = 1 << 16
                }
                
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
            }
        }
    }
}
