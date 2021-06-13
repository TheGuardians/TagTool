using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "achievements", Tag = "achi", Size = 0xC)]
    public class Achievements : TagStructure
    {
        public List<SingleAchievementDefinitionBlock> Achievement;
        
        [TagStructure(Size = 0x14)]
        public class SingleAchievementDefinitionBlock : TagStructure
        {
            public StringId Name;
            public GlobalAchievementEnum Type;
            public GlobalCampaignDifficultyFlags Difficulty;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<SingleAchievementRestrictedLevelBlock> RestrictedLevels;
            
            public enum GlobalAchievementEnum : sbyte
            {
                M10Complete,
                M20Complete,
                M30Complete,
                M60Complete,
                M40Complete,
                M70Complete,
                M80Complete,
                M90Complete,
                CampNormalComplete,
                CampHeroicComplete,
                CampLegendaryComplete,
                CampLegendarySoloComplete,
                CampHeroic3skullsComplete,
                CampCoopMissionComplete,
                CampCoopComplete,
                Terminal1,
                TerminalAll,
                M10Special,
                M20Special,
                M30Special,
                M60Special,
                M40Special,
                M70Special,
                M80Special,
                M90Special,
                EarnRank005,
                EarnRank020,
                WargamesWin5,
                WargamesWin20,
                SpartanOpsMissionComplete,
                SpartanOpsEpisode1Complete,
                SpartanOps5episodesComplete,
                SpartanOpsLegendarySoloMissionComplete,
                Spops1Special,
                Spops2Special,
                Spops3Special,
                Spops4Special,
                Spops5Special,
                ChallengeComplete,
                _25ChallengesComplete,
                ChangeArmor,
                ChangeEmblem,
                ChangeTag,
                ChangePose,
                SaveCustomMap,
                SaveCustomGametype,
                SaveScreenshot,
                SaveFilmclip,
                UploadToFileshare,
                NowTheyFly,
                SizeIsEverything,
                Odst,
                Bigfoot,
                DavidAndGoliath,
                BadRobot,
                ClayPigeon,
                SpecialDelivery,
                CallingInTheBigGuns,
                IThrustAtThee,
                DidntSeeItComing,
                BirdOfPrey,
                BulletSponge,
                ISeeYou,
                CleverGirl,
                FlashOfLight,
                LastManGrinning,
                PigsCanFly
            }
            
            [Flags]
            public enum GlobalCampaignDifficultyFlags : byte
            {
                Easy = 1 << 0,
                Normal = 1 << 1,
                Heroic = 1 << 2,
                Legendary = 1 << 3
            }
            
            [TagStructure(Size = 0x4)]
            public class SingleAchievementRestrictedLevelBlock : TagStructure
            {
                // Compared to map name in scenario
                public StringId LevelName;
            }
        }
    }
}
