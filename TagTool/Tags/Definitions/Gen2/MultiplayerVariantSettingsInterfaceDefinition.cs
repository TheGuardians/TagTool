using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "multiplayer_variant_settings_interface_definition", Tag = "goof", Size = 0x170)]
    public class MultiplayerVariantSettingsInterfaceDefinition : TagStructure
    {
        [TagField(ValidTags = new [] { "wgit" })]
        public CachedTag Unknown;
        [TagField(ValidTags = new [] { "wgit" })]
        public CachedTag Unknown1;
        [TagField(ValidTags = new [] { "wgit" })]
        public CachedTag Unknown2;
        public List<VariantSettingEditReferenceBlock> GameEngineSettings;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag DefaultVariantStrings;
        public List<GDefaultVariantsBlock> DefaultVariants;
        public CreateNewVariantStructBlock Unknown3;
        public CreateNewVariantStructBlock1 Unknown4;
        public CreateNewVariantStructBlock2 Unknown5;
        public CreateNewVariantStructBlock3 Unknown6;
        public CreateNewVariantStructBlock4 Unknown7;
        public CreateNewVariantStructBlock5 Unknown8;
        public CreateNewVariantStructBlock6 Unknown9;
        public CreateNewVariantStructBlock7 Unknown10;
        public CreateNewVariantStructBlock8 Unknown11;
        [TagField(Length = 7)]
        public CreateNewVariantStructBlock9[] Unknown12;
        
        [TagStructure(Size = 0x18)]
        public class VariantSettingEditReferenceBlock : TagStructure
        {
            public SettingCategoryValue SettingCategory;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<TextValuePairBlock> Options;
            public List<NullBlock> Unknown;
            
            public enum SettingCategoryValue : int
            {
                MatchCtf,
                MatchSlayer,
                MatchOddball,
                MatchKing,
                MatchRace,
                MatchHeadhunter,
                MatchJuggernaut,
                MatchTerritories,
                MatchAssault,
                Players,
                Obsolete,
                Vehicles,
                Equipment,
                GameCtf,
                GameSlayer,
                GameOddball,
                GameKing,
                GameRace,
                GameHeadhunter,
                GameJuggernaut,
                GameTerritories,
                GameAssault,
                QuickOptionsCtf,
                QuickOptionsSlayer,
                QuickOptionsOddball,
                QuickOptionsKing,
                QuickOptionsRace,
                QuickOptionsHeadhunter,
                QuickOptionsJuggernaut,
                QuickOptionsTerritories,
                QuickOptionsAssault,
                TeamCtf,
                TeamSlayer,
                TeamOddball,
                TeamKing,
                TeamRace,
                TeamHeadhunter,
                TeamJuggernaut,
                TeamTerritories,
                TeamAssault
            }
            
            [TagStructure(Size = 0x8)]
            public class TextValuePairBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "sily" })]
                public CachedTag ValuePairs;
            }
            
            [TagStructure()]
            public class NullBlock : TagStructure
            {
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class GDefaultVariantsBlock : TagStructure
        {
            public StringId VariantName;
            public VariantTypeValue VariantType;
            public List<GDefaultVariantSettingsBlock> Settings;
            public sbyte DescriptionIndex;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum VariantTypeValue : int
            {
                Slayer,
                Oddball,
                Juggernaut,
                King,
                Ctf,
                Invasion,
                Territories
            }
            
            [TagStructure(Size = 0x8)]
            public class GDefaultVariantSettingsBlock : TagStructure
            {
                public SettingCategoryValue SettingCategory;
                public int Value;
                
                public enum SettingCategoryValue : int
                {
                    MatchRoundSetting,
                    MatchCtfScoreToWin,
                    MatchSlayerScoreToWinRound,
                    MatchOddballScoreToWinRound,
                    MatchKingScoreToWinRound,
                    MatchRaceScoreToWinRound,
                    MatchHeadhunterScoreToWinRound,
                    MatchJuggernautScoreToWinRound,
                    MatchTerritoriesScoreToWinRound,
                    MatchAssaultScoreToWinRound,
                    MatchRoundTimeLimit,
                    MatchRoundsResetMap,
                    MatchTieResolution,
                    MatchObservers,
                    MatchJoinInProgress,
                    MaximumPlayers,
                    LivesPerRound,
                    RespawnTime,
                    SuicidePenalty,
                    Shields,
                    MotionSensor,
                    Invisibility,
                    TeamChanging,
                    TeamScoring,
                    FriendlyFire,
                    TeamRespawnSetting,
                    BetrayalRespawnPenalty,
                    TeamKillerManagement,
                    SlayerBonusPoints,
                    SlayerSuicidePointLoss,
                    SlayerDeathPointLoss,
                    HeadhunterMovingHeadBin,
                    HeadhunterPointMultiplier,
                    HeadhunterSuicidePointLoss,
                    HeadhunterDeathPointLoss,
                    HeadhunterUncontestedBin,
                    HeadhunterSpeedWithHeads,
                    HeadhunterMaxHeadsCarried,
                    KingUncontestedHill,
                    KingTeamTimeMultiplier,
                    KingMovingHill,
                    KingExtraDamageOnHill,
                    KingDmgResistanceOnHill,
                    OddballBallSpawnCount,
                    OddballBallHitDamage,
                    OddballSpeedWithBall,
                    OddballDrivingGunningWithBall,
                    OddballWaypointToBall,
                    RaceRandomTrack,
                    RaceUncontestedFlag,
                    CtfGameType,
                    CtfSuddenDeath,
                    CtfFlagMayBeReturned,
                    CtfFlagAtHomeToScore,
                    CtfFlagResetTime,
                    CtfSpeedWithFlag,
                    CtfFlagHitDamage,
                    CtfDrivingGunningWithFlag,
                    CtfWaypointToOwnFlag,
                    AssaultGameType,
                    AssaultSuddenDeath,
                    AssaultDetonationTime,
                    AssaultBombAtHomeToScore,
                    AssaultArmingTime,
                    AssaultSpeedWithBomb,
                    AssaultBombHitDamage,
                    AssaultDrivingGunningWithBomb,
                    AssaultWaypointToOwnBomb,
                    JuggernautBetrayalPointLoss,
                    JuggernautJuggyExtraDamage,
                    JuggernautJuggyInfiniteAmmo,
                    JuggernautJuggyOvershields,
                    JuggernautJuggyActiveCamo,
                    JuggernautJuggyMotionSensor,
                    TerritoriesTerritoryCount,
                    VehRespawn,
                    VehPrimaryLightLand,
                    VehSecondaryLightLand,
                    VehPrimaryHeavyLand,
                    VehPrimaryFlying,
                    VehSecondaryHeavyLand,
                    VehPrimaryTurret,
                    VehSecondaryTurret,
                    EquipWeaponsOnMap,
                    EquipOvershieldsOnMap,
                    EquipActiveCamoOnMap,
                    EquipGrenadesOnMap,
                    EquipWeaponRespawnTimes,
                    EquipStartingGrenades,
                    EquipPrimaryStartingEquipment,
                    UnsMaxLivingPlayers,
                    UnsTeamsEnabled,
                    UnsAssaultBombMayBeReturned,
                    UnsMaxTeams,
                    UnsEquipSecondaryStartingEquipment,
                    UnsAssaultFuseTime,
                    UnsJuggyMovement,
                    UnsStickyFuse,
                    UnsTerrContestTime,
                    UnsTerrControlTime,
                    UnsOddbCarrInvis,
                    UnsKingInvisInHill,
                    UnsBallCarrDmgResis,
                    UnsKingDmgResInHill,
                    UnsPlayersExDmg,
                    UnsPlayersDmgResis,
                    UnsCtfCarrDmgResis,
                    UnsCtfCarrInvis,
                    UnsJuggyDmgResis,
                    UnsBombCarrDmgResis,
                    UnsBombCarrInvis,
                    UnsForceEvenTeams
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CreateNewVariantStructBlock : TagStructure
        {
            public StringId Unknown;
            public UnknownValue Unknown1;
            public List<GDefaultVariantSettingsBlock> Settings;
            public sbyte Unknown2;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum UnknownValue : int
            {
                Slayer,
                Oddball,
                Juggernaut,
                King,
                Ctf,
                Invasion,
                Territories
            }
            
            [TagStructure(Size = 0x8)]
            public class GDefaultVariantSettingsBlock : TagStructure
            {
                public SettingCategoryValue SettingCategory;
                public int Value;
                
                public enum SettingCategoryValue : int
                {
                    MatchRoundSetting,
                    MatchCtfScoreToWin,
                    MatchSlayerScoreToWinRound,
                    MatchOddballScoreToWinRound,
                    MatchKingScoreToWinRound,
                    MatchRaceScoreToWinRound,
                    MatchHeadhunterScoreToWinRound,
                    MatchJuggernautScoreToWinRound,
                    MatchTerritoriesScoreToWinRound,
                    MatchAssaultScoreToWinRound,
                    MatchRoundTimeLimit,
                    MatchRoundsResetMap,
                    MatchTieResolution,
                    MatchObservers,
                    MatchJoinInProgress,
                    MaximumPlayers,
                    LivesPerRound,
                    RespawnTime,
                    SuicidePenalty,
                    Shields,
                    MotionSensor,
                    Invisibility,
                    TeamChanging,
                    TeamScoring,
                    FriendlyFire,
                    TeamRespawnSetting,
                    BetrayalRespawnPenalty,
                    TeamKillerManagement,
                    SlayerBonusPoints,
                    SlayerSuicidePointLoss,
                    SlayerDeathPointLoss,
                    HeadhunterMovingHeadBin,
                    HeadhunterPointMultiplier,
                    HeadhunterSuicidePointLoss,
                    HeadhunterDeathPointLoss,
                    HeadhunterUncontestedBin,
                    HeadhunterSpeedWithHeads,
                    HeadhunterMaxHeadsCarried,
                    KingUncontestedHill,
                    KingTeamTimeMultiplier,
                    KingMovingHill,
                    KingExtraDamageOnHill,
                    KingDmgResistanceOnHill,
                    OddballBallSpawnCount,
                    OddballBallHitDamage,
                    OddballSpeedWithBall,
                    OddballDrivingGunningWithBall,
                    OddballWaypointToBall,
                    RaceRandomTrack,
                    RaceUncontestedFlag,
                    CtfGameType,
                    CtfSuddenDeath,
                    CtfFlagMayBeReturned,
                    CtfFlagAtHomeToScore,
                    CtfFlagResetTime,
                    CtfSpeedWithFlag,
                    CtfFlagHitDamage,
                    CtfDrivingGunningWithFlag,
                    CtfWaypointToOwnFlag,
                    AssaultGameType,
                    AssaultSuddenDeath,
                    AssaultDetonationTime,
                    AssaultBombAtHomeToScore,
                    AssaultArmingTime,
                    AssaultSpeedWithBomb,
                    AssaultBombHitDamage,
                    AssaultDrivingGunningWithBomb,
                    AssaultWaypointToOwnBomb,
                    JuggernautBetrayalPointLoss,
                    JuggernautJuggyExtraDamage,
                    JuggernautJuggyInfiniteAmmo,
                    JuggernautJuggyOvershields,
                    JuggernautJuggyActiveCamo,
                    JuggernautJuggyMotionSensor,
                    TerritoriesTerritoryCount,
                    VehRespawn,
                    VehPrimaryLightLand,
                    VehSecondaryLightLand,
                    VehPrimaryHeavyLand,
                    VehPrimaryFlying,
                    VehSecondaryHeavyLand,
                    VehPrimaryTurret,
                    VehSecondaryTurret,
                    EquipWeaponsOnMap,
                    EquipOvershieldsOnMap,
                    EquipActiveCamoOnMap,
                    EquipGrenadesOnMap,
                    EquipWeaponRespawnTimes,
                    EquipStartingGrenades,
                    EquipPrimaryStartingEquipment,
                    UnsMaxLivingPlayers,
                    UnsTeamsEnabled,
                    UnsAssaultBombMayBeReturned,
                    UnsMaxTeams,
                    UnsEquipSecondaryStartingEquipment,
                    UnsAssaultFuseTime,
                    UnsJuggyMovement,
                    UnsStickyFuse,
                    UnsTerrContestTime,
                    UnsTerrControlTime,
                    UnsOddbCarrInvis,
                    UnsKingInvisInHill,
                    UnsBallCarrDmgResis,
                    UnsKingDmgResInHill,
                    UnsPlayersExDmg,
                    UnsPlayersDmgResis,
                    UnsCtfCarrDmgResis,
                    UnsCtfCarrInvis,
                    UnsJuggyDmgResis,
                    UnsBombCarrDmgResis,
                    UnsBombCarrInvis,
                    UnsForceEvenTeams
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CreateNewVariantStructBlock1 : TagStructure
        {
            public StringId Unknown;
            public UnknownValue Unknown1;
            public List<GDefaultVariantSettingsBlock> Settings;
            public sbyte Unknown2;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum UnknownValue : int
            {
                Slayer,
                Oddball,
                Juggernaut,
                King,
                Ctf,
                Invasion,
                Territories
            }
            
            [TagStructure(Size = 0x8)]
            public class GDefaultVariantSettingsBlock : TagStructure
            {
                public SettingCategoryValue SettingCategory;
                public int Value;
                
                public enum SettingCategoryValue : int
                {
                    MatchRoundSetting,
                    MatchCtfScoreToWin,
                    MatchSlayerScoreToWinRound,
                    MatchOddballScoreToWinRound,
                    MatchKingScoreToWinRound,
                    MatchRaceScoreToWinRound,
                    MatchHeadhunterScoreToWinRound,
                    MatchJuggernautScoreToWinRound,
                    MatchTerritoriesScoreToWinRound,
                    MatchAssaultScoreToWinRound,
                    MatchRoundTimeLimit,
                    MatchRoundsResetMap,
                    MatchTieResolution,
                    MatchObservers,
                    MatchJoinInProgress,
                    MaximumPlayers,
                    LivesPerRound,
                    RespawnTime,
                    SuicidePenalty,
                    Shields,
                    MotionSensor,
                    Invisibility,
                    TeamChanging,
                    TeamScoring,
                    FriendlyFire,
                    TeamRespawnSetting,
                    BetrayalRespawnPenalty,
                    TeamKillerManagement,
                    SlayerBonusPoints,
                    SlayerSuicidePointLoss,
                    SlayerDeathPointLoss,
                    HeadhunterMovingHeadBin,
                    HeadhunterPointMultiplier,
                    HeadhunterSuicidePointLoss,
                    HeadhunterDeathPointLoss,
                    HeadhunterUncontestedBin,
                    HeadhunterSpeedWithHeads,
                    HeadhunterMaxHeadsCarried,
                    KingUncontestedHill,
                    KingTeamTimeMultiplier,
                    KingMovingHill,
                    KingExtraDamageOnHill,
                    KingDmgResistanceOnHill,
                    OddballBallSpawnCount,
                    OddballBallHitDamage,
                    OddballSpeedWithBall,
                    OddballDrivingGunningWithBall,
                    OddballWaypointToBall,
                    RaceRandomTrack,
                    RaceUncontestedFlag,
                    CtfGameType,
                    CtfSuddenDeath,
                    CtfFlagMayBeReturned,
                    CtfFlagAtHomeToScore,
                    CtfFlagResetTime,
                    CtfSpeedWithFlag,
                    CtfFlagHitDamage,
                    CtfDrivingGunningWithFlag,
                    CtfWaypointToOwnFlag,
                    AssaultGameType,
                    AssaultSuddenDeath,
                    AssaultDetonationTime,
                    AssaultBombAtHomeToScore,
                    AssaultArmingTime,
                    AssaultSpeedWithBomb,
                    AssaultBombHitDamage,
                    AssaultDrivingGunningWithBomb,
                    AssaultWaypointToOwnBomb,
                    JuggernautBetrayalPointLoss,
                    JuggernautJuggyExtraDamage,
                    JuggernautJuggyInfiniteAmmo,
                    JuggernautJuggyOvershields,
                    JuggernautJuggyActiveCamo,
                    JuggernautJuggyMotionSensor,
                    TerritoriesTerritoryCount,
                    VehRespawn,
                    VehPrimaryLightLand,
                    VehSecondaryLightLand,
                    VehPrimaryHeavyLand,
                    VehPrimaryFlying,
                    VehSecondaryHeavyLand,
                    VehPrimaryTurret,
                    VehSecondaryTurret,
                    EquipWeaponsOnMap,
                    EquipOvershieldsOnMap,
                    EquipActiveCamoOnMap,
                    EquipGrenadesOnMap,
                    EquipWeaponRespawnTimes,
                    EquipStartingGrenades,
                    EquipPrimaryStartingEquipment,
                    UnsMaxLivingPlayers,
                    UnsTeamsEnabled,
                    UnsAssaultBombMayBeReturned,
                    UnsMaxTeams,
                    UnsEquipSecondaryStartingEquipment,
                    UnsAssaultFuseTime,
                    UnsJuggyMovement,
                    UnsStickyFuse,
                    UnsTerrContestTime,
                    UnsTerrControlTime,
                    UnsOddbCarrInvis,
                    UnsKingInvisInHill,
                    UnsBallCarrDmgResis,
                    UnsKingDmgResInHill,
                    UnsPlayersExDmg,
                    UnsPlayersDmgResis,
                    UnsCtfCarrDmgResis,
                    UnsCtfCarrInvis,
                    UnsJuggyDmgResis,
                    UnsBombCarrDmgResis,
                    UnsBombCarrInvis,
                    UnsForceEvenTeams
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CreateNewVariantStructBlock2 : TagStructure
        {
            public StringId Unknown;
            public UnknownValue Unknown1;
            public List<GDefaultVariantSettingsBlock> Settings;
            public sbyte Unknown2;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum UnknownValue : int
            {
                Slayer,
                Oddball,
                Juggernaut,
                King,
                Ctf,
                Invasion,
                Territories
            }
            
            [TagStructure(Size = 0x8)]
            public class GDefaultVariantSettingsBlock : TagStructure
            {
                public SettingCategoryValue SettingCategory;
                public int Value;
                
                public enum SettingCategoryValue : int
                {
                    MatchRoundSetting,
                    MatchCtfScoreToWin,
                    MatchSlayerScoreToWinRound,
                    MatchOddballScoreToWinRound,
                    MatchKingScoreToWinRound,
                    MatchRaceScoreToWinRound,
                    MatchHeadhunterScoreToWinRound,
                    MatchJuggernautScoreToWinRound,
                    MatchTerritoriesScoreToWinRound,
                    MatchAssaultScoreToWinRound,
                    MatchRoundTimeLimit,
                    MatchRoundsResetMap,
                    MatchTieResolution,
                    MatchObservers,
                    MatchJoinInProgress,
                    MaximumPlayers,
                    LivesPerRound,
                    RespawnTime,
                    SuicidePenalty,
                    Shields,
                    MotionSensor,
                    Invisibility,
                    TeamChanging,
                    TeamScoring,
                    FriendlyFire,
                    TeamRespawnSetting,
                    BetrayalRespawnPenalty,
                    TeamKillerManagement,
                    SlayerBonusPoints,
                    SlayerSuicidePointLoss,
                    SlayerDeathPointLoss,
                    HeadhunterMovingHeadBin,
                    HeadhunterPointMultiplier,
                    HeadhunterSuicidePointLoss,
                    HeadhunterDeathPointLoss,
                    HeadhunterUncontestedBin,
                    HeadhunterSpeedWithHeads,
                    HeadhunterMaxHeadsCarried,
                    KingUncontestedHill,
                    KingTeamTimeMultiplier,
                    KingMovingHill,
                    KingExtraDamageOnHill,
                    KingDmgResistanceOnHill,
                    OddballBallSpawnCount,
                    OddballBallHitDamage,
                    OddballSpeedWithBall,
                    OddballDrivingGunningWithBall,
                    OddballWaypointToBall,
                    RaceRandomTrack,
                    RaceUncontestedFlag,
                    CtfGameType,
                    CtfSuddenDeath,
                    CtfFlagMayBeReturned,
                    CtfFlagAtHomeToScore,
                    CtfFlagResetTime,
                    CtfSpeedWithFlag,
                    CtfFlagHitDamage,
                    CtfDrivingGunningWithFlag,
                    CtfWaypointToOwnFlag,
                    AssaultGameType,
                    AssaultSuddenDeath,
                    AssaultDetonationTime,
                    AssaultBombAtHomeToScore,
                    AssaultArmingTime,
                    AssaultSpeedWithBomb,
                    AssaultBombHitDamage,
                    AssaultDrivingGunningWithBomb,
                    AssaultWaypointToOwnBomb,
                    JuggernautBetrayalPointLoss,
                    JuggernautJuggyExtraDamage,
                    JuggernautJuggyInfiniteAmmo,
                    JuggernautJuggyOvershields,
                    JuggernautJuggyActiveCamo,
                    JuggernautJuggyMotionSensor,
                    TerritoriesTerritoryCount,
                    VehRespawn,
                    VehPrimaryLightLand,
                    VehSecondaryLightLand,
                    VehPrimaryHeavyLand,
                    VehPrimaryFlying,
                    VehSecondaryHeavyLand,
                    VehPrimaryTurret,
                    VehSecondaryTurret,
                    EquipWeaponsOnMap,
                    EquipOvershieldsOnMap,
                    EquipActiveCamoOnMap,
                    EquipGrenadesOnMap,
                    EquipWeaponRespawnTimes,
                    EquipStartingGrenades,
                    EquipPrimaryStartingEquipment,
                    UnsMaxLivingPlayers,
                    UnsTeamsEnabled,
                    UnsAssaultBombMayBeReturned,
                    UnsMaxTeams,
                    UnsEquipSecondaryStartingEquipment,
                    UnsAssaultFuseTime,
                    UnsJuggyMovement,
                    UnsStickyFuse,
                    UnsTerrContestTime,
                    UnsTerrControlTime,
                    UnsOddbCarrInvis,
                    UnsKingInvisInHill,
                    UnsBallCarrDmgResis,
                    UnsKingDmgResInHill,
                    UnsPlayersExDmg,
                    UnsPlayersDmgResis,
                    UnsCtfCarrDmgResis,
                    UnsCtfCarrInvis,
                    UnsJuggyDmgResis,
                    UnsBombCarrDmgResis,
                    UnsBombCarrInvis,
                    UnsForceEvenTeams
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CreateNewVariantStructBlock3 : TagStructure
        {
            public StringId Unknown;
            public UnknownValue Unknown1;
            public List<GDefaultVariantSettingsBlock> Settings;
            public sbyte Unknown2;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum UnknownValue : int
            {
                Slayer,
                Oddball,
                Juggernaut,
                King,
                Ctf,
                Invasion,
                Territories
            }
            
            [TagStructure(Size = 0x8)]
            public class GDefaultVariantSettingsBlock : TagStructure
            {
                public SettingCategoryValue SettingCategory;
                public int Value;
                
                public enum SettingCategoryValue : int
                {
                    MatchRoundSetting,
                    MatchCtfScoreToWin,
                    MatchSlayerScoreToWinRound,
                    MatchOddballScoreToWinRound,
                    MatchKingScoreToWinRound,
                    MatchRaceScoreToWinRound,
                    MatchHeadhunterScoreToWinRound,
                    MatchJuggernautScoreToWinRound,
                    MatchTerritoriesScoreToWinRound,
                    MatchAssaultScoreToWinRound,
                    MatchRoundTimeLimit,
                    MatchRoundsResetMap,
                    MatchTieResolution,
                    MatchObservers,
                    MatchJoinInProgress,
                    MaximumPlayers,
                    LivesPerRound,
                    RespawnTime,
                    SuicidePenalty,
                    Shields,
                    MotionSensor,
                    Invisibility,
                    TeamChanging,
                    TeamScoring,
                    FriendlyFire,
                    TeamRespawnSetting,
                    BetrayalRespawnPenalty,
                    TeamKillerManagement,
                    SlayerBonusPoints,
                    SlayerSuicidePointLoss,
                    SlayerDeathPointLoss,
                    HeadhunterMovingHeadBin,
                    HeadhunterPointMultiplier,
                    HeadhunterSuicidePointLoss,
                    HeadhunterDeathPointLoss,
                    HeadhunterUncontestedBin,
                    HeadhunterSpeedWithHeads,
                    HeadhunterMaxHeadsCarried,
                    KingUncontestedHill,
                    KingTeamTimeMultiplier,
                    KingMovingHill,
                    KingExtraDamageOnHill,
                    KingDmgResistanceOnHill,
                    OddballBallSpawnCount,
                    OddballBallHitDamage,
                    OddballSpeedWithBall,
                    OddballDrivingGunningWithBall,
                    OddballWaypointToBall,
                    RaceRandomTrack,
                    RaceUncontestedFlag,
                    CtfGameType,
                    CtfSuddenDeath,
                    CtfFlagMayBeReturned,
                    CtfFlagAtHomeToScore,
                    CtfFlagResetTime,
                    CtfSpeedWithFlag,
                    CtfFlagHitDamage,
                    CtfDrivingGunningWithFlag,
                    CtfWaypointToOwnFlag,
                    AssaultGameType,
                    AssaultSuddenDeath,
                    AssaultDetonationTime,
                    AssaultBombAtHomeToScore,
                    AssaultArmingTime,
                    AssaultSpeedWithBomb,
                    AssaultBombHitDamage,
                    AssaultDrivingGunningWithBomb,
                    AssaultWaypointToOwnBomb,
                    JuggernautBetrayalPointLoss,
                    JuggernautJuggyExtraDamage,
                    JuggernautJuggyInfiniteAmmo,
                    JuggernautJuggyOvershields,
                    JuggernautJuggyActiveCamo,
                    JuggernautJuggyMotionSensor,
                    TerritoriesTerritoryCount,
                    VehRespawn,
                    VehPrimaryLightLand,
                    VehSecondaryLightLand,
                    VehPrimaryHeavyLand,
                    VehPrimaryFlying,
                    VehSecondaryHeavyLand,
                    VehPrimaryTurret,
                    VehSecondaryTurret,
                    EquipWeaponsOnMap,
                    EquipOvershieldsOnMap,
                    EquipActiveCamoOnMap,
                    EquipGrenadesOnMap,
                    EquipWeaponRespawnTimes,
                    EquipStartingGrenades,
                    EquipPrimaryStartingEquipment,
                    UnsMaxLivingPlayers,
                    UnsTeamsEnabled,
                    UnsAssaultBombMayBeReturned,
                    UnsMaxTeams,
                    UnsEquipSecondaryStartingEquipment,
                    UnsAssaultFuseTime,
                    UnsJuggyMovement,
                    UnsStickyFuse,
                    UnsTerrContestTime,
                    UnsTerrControlTime,
                    UnsOddbCarrInvis,
                    UnsKingInvisInHill,
                    UnsBallCarrDmgResis,
                    UnsKingDmgResInHill,
                    UnsPlayersExDmg,
                    UnsPlayersDmgResis,
                    UnsCtfCarrDmgResis,
                    UnsCtfCarrInvis,
                    UnsJuggyDmgResis,
                    UnsBombCarrDmgResis,
                    UnsBombCarrInvis,
                    UnsForceEvenTeams
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CreateNewVariantStructBlock4 : TagStructure
        {
            public StringId Unknown;
            public UnknownValue Unknown1;
            public List<GDefaultVariantSettingsBlock> Settings;
            public sbyte Unknown2;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum UnknownValue : int
            {
                Slayer,
                Oddball,
                Juggernaut,
                King,
                Ctf,
                Invasion,
                Territories
            }
            
            [TagStructure(Size = 0x8)]
            public class GDefaultVariantSettingsBlock : TagStructure
            {
                public SettingCategoryValue SettingCategory;
                public int Value;
                
                public enum SettingCategoryValue : int
                {
                    MatchRoundSetting,
                    MatchCtfScoreToWin,
                    MatchSlayerScoreToWinRound,
                    MatchOddballScoreToWinRound,
                    MatchKingScoreToWinRound,
                    MatchRaceScoreToWinRound,
                    MatchHeadhunterScoreToWinRound,
                    MatchJuggernautScoreToWinRound,
                    MatchTerritoriesScoreToWinRound,
                    MatchAssaultScoreToWinRound,
                    MatchRoundTimeLimit,
                    MatchRoundsResetMap,
                    MatchTieResolution,
                    MatchObservers,
                    MatchJoinInProgress,
                    MaximumPlayers,
                    LivesPerRound,
                    RespawnTime,
                    SuicidePenalty,
                    Shields,
                    MotionSensor,
                    Invisibility,
                    TeamChanging,
                    TeamScoring,
                    FriendlyFire,
                    TeamRespawnSetting,
                    BetrayalRespawnPenalty,
                    TeamKillerManagement,
                    SlayerBonusPoints,
                    SlayerSuicidePointLoss,
                    SlayerDeathPointLoss,
                    HeadhunterMovingHeadBin,
                    HeadhunterPointMultiplier,
                    HeadhunterSuicidePointLoss,
                    HeadhunterDeathPointLoss,
                    HeadhunterUncontestedBin,
                    HeadhunterSpeedWithHeads,
                    HeadhunterMaxHeadsCarried,
                    KingUncontestedHill,
                    KingTeamTimeMultiplier,
                    KingMovingHill,
                    KingExtraDamageOnHill,
                    KingDmgResistanceOnHill,
                    OddballBallSpawnCount,
                    OddballBallHitDamage,
                    OddballSpeedWithBall,
                    OddballDrivingGunningWithBall,
                    OddballWaypointToBall,
                    RaceRandomTrack,
                    RaceUncontestedFlag,
                    CtfGameType,
                    CtfSuddenDeath,
                    CtfFlagMayBeReturned,
                    CtfFlagAtHomeToScore,
                    CtfFlagResetTime,
                    CtfSpeedWithFlag,
                    CtfFlagHitDamage,
                    CtfDrivingGunningWithFlag,
                    CtfWaypointToOwnFlag,
                    AssaultGameType,
                    AssaultSuddenDeath,
                    AssaultDetonationTime,
                    AssaultBombAtHomeToScore,
                    AssaultArmingTime,
                    AssaultSpeedWithBomb,
                    AssaultBombHitDamage,
                    AssaultDrivingGunningWithBomb,
                    AssaultWaypointToOwnBomb,
                    JuggernautBetrayalPointLoss,
                    JuggernautJuggyExtraDamage,
                    JuggernautJuggyInfiniteAmmo,
                    JuggernautJuggyOvershields,
                    JuggernautJuggyActiveCamo,
                    JuggernautJuggyMotionSensor,
                    TerritoriesTerritoryCount,
                    VehRespawn,
                    VehPrimaryLightLand,
                    VehSecondaryLightLand,
                    VehPrimaryHeavyLand,
                    VehPrimaryFlying,
                    VehSecondaryHeavyLand,
                    VehPrimaryTurret,
                    VehSecondaryTurret,
                    EquipWeaponsOnMap,
                    EquipOvershieldsOnMap,
                    EquipActiveCamoOnMap,
                    EquipGrenadesOnMap,
                    EquipWeaponRespawnTimes,
                    EquipStartingGrenades,
                    EquipPrimaryStartingEquipment,
                    UnsMaxLivingPlayers,
                    UnsTeamsEnabled,
                    UnsAssaultBombMayBeReturned,
                    UnsMaxTeams,
                    UnsEquipSecondaryStartingEquipment,
                    UnsAssaultFuseTime,
                    UnsJuggyMovement,
                    UnsStickyFuse,
                    UnsTerrContestTime,
                    UnsTerrControlTime,
                    UnsOddbCarrInvis,
                    UnsKingInvisInHill,
                    UnsBallCarrDmgResis,
                    UnsKingDmgResInHill,
                    UnsPlayersExDmg,
                    UnsPlayersDmgResis,
                    UnsCtfCarrDmgResis,
                    UnsCtfCarrInvis,
                    UnsJuggyDmgResis,
                    UnsBombCarrDmgResis,
                    UnsBombCarrInvis,
                    UnsForceEvenTeams
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CreateNewVariantStructBlock5 : TagStructure
        {
            public StringId Unknown;
            public UnknownValue Unknown1;
            public List<GDefaultVariantSettingsBlock> Settings;
            public sbyte Unknown2;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum UnknownValue : int
            {
                Slayer,
                Oddball,
                Juggernaut,
                King,
                Ctf,
                Invasion,
                Territories
            }
            
            [TagStructure(Size = 0x8)]
            public class GDefaultVariantSettingsBlock : TagStructure
            {
                public SettingCategoryValue SettingCategory;
                public int Value;
                
                public enum SettingCategoryValue : int
                {
                    MatchRoundSetting,
                    MatchCtfScoreToWin,
                    MatchSlayerScoreToWinRound,
                    MatchOddballScoreToWinRound,
                    MatchKingScoreToWinRound,
                    MatchRaceScoreToWinRound,
                    MatchHeadhunterScoreToWinRound,
                    MatchJuggernautScoreToWinRound,
                    MatchTerritoriesScoreToWinRound,
                    MatchAssaultScoreToWinRound,
                    MatchRoundTimeLimit,
                    MatchRoundsResetMap,
                    MatchTieResolution,
                    MatchObservers,
                    MatchJoinInProgress,
                    MaximumPlayers,
                    LivesPerRound,
                    RespawnTime,
                    SuicidePenalty,
                    Shields,
                    MotionSensor,
                    Invisibility,
                    TeamChanging,
                    TeamScoring,
                    FriendlyFire,
                    TeamRespawnSetting,
                    BetrayalRespawnPenalty,
                    TeamKillerManagement,
                    SlayerBonusPoints,
                    SlayerSuicidePointLoss,
                    SlayerDeathPointLoss,
                    HeadhunterMovingHeadBin,
                    HeadhunterPointMultiplier,
                    HeadhunterSuicidePointLoss,
                    HeadhunterDeathPointLoss,
                    HeadhunterUncontestedBin,
                    HeadhunterSpeedWithHeads,
                    HeadhunterMaxHeadsCarried,
                    KingUncontestedHill,
                    KingTeamTimeMultiplier,
                    KingMovingHill,
                    KingExtraDamageOnHill,
                    KingDmgResistanceOnHill,
                    OddballBallSpawnCount,
                    OddballBallHitDamage,
                    OddballSpeedWithBall,
                    OddballDrivingGunningWithBall,
                    OddballWaypointToBall,
                    RaceRandomTrack,
                    RaceUncontestedFlag,
                    CtfGameType,
                    CtfSuddenDeath,
                    CtfFlagMayBeReturned,
                    CtfFlagAtHomeToScore,
                    CtfFlagResetTime,
                    CtfSpeedWithFlag,
                    CtfFlagHitDamage,
                    CtfDrivingGunningWithFlag,
                    CtfWaypointToOwnFlag,
                    AssaultGameType,
                    AssaultSuddenDeath,
                    AssaultDetonationTime,
                    AssaultBombAtHomeToScore,
                    AssaultArmingTime,
                    AssaultSpeedWithBomb,
                    AssaultBombHitDamage,
                    AssaultDrivingGunningWithBomb,
                    AssaultWaypointToOwnBomb,
                    JuggernautBetrayalPointLoss,
                    JuggernautJuggyExtraDamage,
                    JuggernautJuggyInfiniteAmmo,
                    JuggernautJuggyOvershields,
                    JuggernautJuggyActiveCamo,
                    JuggernautJuggyMotionSensor,
                    TerritoriesTerritoryCount,
                    VehRespawn,
                    VehPrimaryLightLand,
                    VehSecondaryLightLand,
                    VehPrimaryHeavyLand,
                    VehPrimaryFlying,
                    VehSecondaryHeavyLand,
                    VehPrimaryTurret,
                    VehSecondaryTurret,
                    EquipWeaponsOnMap,
                    EquipOvershieldsOnMap,
                    EquipActiveCamoOnMap,
                    EquipGrenadesOnMap,
                    EquipWeaponRespawnTimes,
                    EquipStartingGrenades,
                    EquipPrimaryStartingEquipment,
                    UnsMaxLivingPlayers,
                    UnsTeamsEnabled,
                    UnsAssaultBombMayBeReturned,
                    UnsMaxTeams,
                    UnsEquipSecondaryStartingEquipment,
                    UnsAssaultFuseTime,
                    UnsJuggyMovement,
                    UnsStickyFuse,
                    UnsTerrContestTime,
                    UnsTerrControlTime,
                    UnsOddbCarrInvis,
                    UnsKingInvisInHill,
                    UnsBallCarrDmgResis,
                    UnsKingDmgResInHill,
                    UnsPlayersExDmg,
                    UnsPlayersDmgResis,
                    UnsCtfCarrDmgResis,
                    UnsCtfCarrInvis,
                    UnsJuggyDmgResis,
                    UnsBombCarrDmgResis,
                    UnsBombCarrInvis,
                    UnsForceEvenTeams
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CreateNewVariantStructBlock6 : TagStructure
        {
            public StringId Unknown;
            public UnknownValue Unknown1;
            public List<GDefaultVariantSettingsBlock> Settings;
            public sbyte Unknown2;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum UnknownValue : int
            {
                Slayer,
                Oddball,
                Juggernaut,
                King,
                Ctf,
                Invasion,
                Territories
            }
            
            [TagStructure(Size = 0x8)]
            public class GDefaultVariantSettingsBlock : TagStructure
            {
                public SettingCategoryValue SettingCategory;
                public int Value;
                
                public enum SettingCategoryValue : int
                {
                    MatchRoundSetting,
                    MatchCtfScoreToWin,
                    MatchSlayerScoreToWinRound,
                    MatchOddballScoreToWinRound,
                    MatchKingScoreToWinRound,
                    MatchRaceScoreToWinRound,
                    MatchHeadhunterScoreToWinRound,
                    MatchJuggernautScoreToWinRound,
                    MatchTerritoriesScoreToWinRound,
                    MatchAssaultScoreToWinRound,
                    MatchRoundTimeLimit,
                    MatchRoundsResetMap,
                    MatchTieResolution,
                    MatchObservers,
                    MatchJoinInProgress,
                    MaximumPlayers,
                    LivesPerRound,
                    RespawnTime,
                    SuicidePenalty,
                    Shields,
                    MotionSensor,
                    Invisibility,
                    TeamChanging,
                    TeamScoring,
                    FriendlyFire,
                    TeamRespawnSetting,
                    BetrayalRespawnPenalty,
                    TeamKillerManagement,
                    SlayerBonusPoints,
                    SlayerSuicidePointLoss,
                    SlayerDeathPointLoss,
                    HeadhunterMovingHeadBin,
                    HeadhunterPointMultiplier,
                    HeadhunterSuicidePointLoss,
                    HeadhunterDeathPointLoss,
                    HeadhunterUncontestedBin,
                    HeadhunterSpeedWithHeads,
                    HeadhunterMaxHeadsCarried,
                    KingUncontestedHill,
                    KingTeamTimeMultiplier,
                    KingMovingHill,
                    KingExtraDamageOnHill,
                    KingDmgResistanceOnHill,
                    OddballBallSpawnCount,
                    OddballBallHitDamage,
                    OddballSpeedWithBall,
                    OddballDrivingGunningWithBall,
                    OddballWaypointToBall,
                    RaceRandomTrack,
                    RaceUncontestedFlag,
                    CtfGameType,
                    CtfSuddenDeath,
                    CtfFlagMayBeReturned,
                    CtfFlagAtHomeToScore,
                    CtfFlagResetTime,
                    CtfSpeedWithFlag,
                    CtfFlagHitDamage,
                    CtfDrivingGunningWithFlag,
                    CtfWaypointToOwnFlag,
                    AssaultGameType,
                    AssaultSuddenDeath,
                    AssaultDetonationTime,
                    AssaultBombAtHomeToScore,
                    AssaultArmingTime,
                    AssaultSpeedWithBomb,
                    AssaultBombHitDamage,
                    AssaultDrivingGunningWithBomb,
                    AssaultWaypointToOwnBomb,
                    JuggernautBetrayalPointLoss,
                    JuggernautJuggyExtraDamage,
                    JuggernautJuggyInfiniteAmmo,
                    JuggernautJuggyOvershields,
                    JuggernautJuggyActiveCamo,
                    JuggernautJuggyMotionSensor,
                    TerritoriesTerritoryCount,
                    VehRespawn,
                    VehPrimaryLightLand,
                    VehSecondaryLightLand,
                    VehPrimaryHeavyLand,
                    VehPrimaryFlying,
                    VehSecondaryHeavyLand,
                    VehPrimaryTurret,
                    VehSecondaryTurret,
                    EquipWeaponsOnMap,
                    EquipOvershieldsOnMap,
                    EquipActiveCamoOnMap,
                    EquipGrenadesOnMap,
                    EquipWeaponRespawnTimes,
                    EquipStartingGrenades,
                    EquipPrimaryStartingEquipment,
                    UnsMaxLivingPlayers,
                    UnsTeamsEnabled,
                    UnsAssaultBombMayBeReturned,
                    UnsMaxTeams,
                    UnsEquipSecondaryStartingEquipment,
                    UnsAssaultFuseTime,
                    UnsJuggyMovement,
                    UnsStickyFuse,
                    UnsTerrContestTime,
                    UnsTerrControlTime,
                    UnsOddbCarrInvis,
                    UnsKingInvisInHill,
                    UnsBallCarrDmgResis,
                    UnsKingDmgResInHill,
                    UnsPlayersExDmg,
                    UnsPlayersDmgResis,
                    UnsCtfCarrDmgResis,
                    UnsCtfCarrInvis,
                    UnsJuggyDmgResis,
                    UnsBombCarrDmgResis,
                    UnsBombCarrInvis,
                    UnsForceEvenTeams
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CreateNewVariantStructBlock7 : TagStructure
        {
            public StringId Unknown;
            public UnknownValue Unknown1;
            public List<GDefaultVariantSettingsBlock> Settings;
            public sbyte Unknown2;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum UnknownValue : int
            {
                Slayer,
                Oddball,
                Juggernaut,
                King,
                Ctf,
                Invasion,
                Territories
            }
            
            [TagStructure(Size = 0x8)]
            public class GDefaultVariantSettingsBlock : TagStructure
            {
                public SettingCategoryValue SettingCategory;
                public int Value;
                
                public enum SettingCategoryValue : int
                {
                    MatchRoundSetting,
                    MatchCtfScoreToWin,
                    MatchSlayerScoreToWinRound,
                    MatchOddballScoreToWinRound,
                    MatchKingScoreToWinRound,
                    MatchRaceScoreToWinRound,
                    MatchHeadhunterScoreToWinRound,
                    MatchJuggernautScoreToWinRound,
                    MatchTerritoriesScoreToWinRound,
                    MatchAssaultScoreToWinRound,
                    MatchRoundTimeLimit,
                    MatchRoundsResetMap,
                    MatchTieResolution,
                    MatchObservers,
                    MatchJoinInProgress,
                    MaximumPlayers,
                    LivesPerRound,
                    RespawnTime,
                    SuicidePenalty,
                    Shields,
                    MotionSensor,
                    Invisibility,
                    TeamChanging,
                    TeamScoring,
                    FriendlyFire,
                    TeamRespawnSetting,
                    BetrayalRespawnPenalty,
                    TeamKillerManagement,
                    SlayerBonusPoints,
                    SlayerSuicidePointLoss,
                    SlayerDeathPointLoss,
                    HeadhunterMovingHeadBin,
                    HeadhunterPointMultiplier,
                    HeadhunterSuicidePointLoss,
                    HeadhunterDeathPointLoss,
                    HeadhunterUncontestedBin,
                    HeadhunterSpeedWithHeads,
                    HeadhunterMaxHeadsCarried,
                    KingUncontestedHill,
                    KingTeamTimeMultiplier,
                    KingMovingHill,
                    KingExtraDamageOnHill,
                    KingDmgResistanceOnHill,
                    OddballBallSpawnCount,
                    OddballBallHitDamage,
                    OddballSpeedWithBall,
                    OddballDrivingGunningWithBall,
                    OddballWaypointToBall,
                    RaceRandomTrack,
                    RaceUncontestedFlag,
                    CtfGameType,
                    CtfSuddenDeath,
                    CtfFlagMayBeReturned,
                    CtfFlagAtHomeToScore,
                    CtfFlagResetTime,
                    CtfSpeedWithFlag,
                    CtfFlagHitDamage,
                    CtfDrivingGunningWithFlag,
                    CtfWaypointToOwnFlag,
                    AssaultGameType,
                    AssaultSuddenDeath,
                    AssaultDetonationTime,
                    AssaultBombAtHomeToScore,
                    AssaultArmingTime,
                    AssaultSpeedWithBomb,
                    AssaultBombHitDamage,
                    AssaultDrivingGunningWithBomb,
                    AssaultWaypointToOwnBomb,
                    JuggernautBetrayalPointLoss,
                    JuggernautJuggyExtraDamage,
                    JuggernautJuggyInfiniteAmmo,
                    JuggernautJuggyOvershields,
                    JuggernautJuggyActiveCamo,
                    JuggernautJuggyMotionSensor,
                    TerritoriesTerritoryCount,
                    VehRespawn,
                    VehPrimaryLightLand,
                    VehSecondaryLightLand,
                    VehPrimaryHeavyLand,
                    VehPrimaryFlying,
                    VehSecondaryHeavyLand,
                    VehPrimaryTurret,
                    VehSecondaryTurret,
                    EquipWeaponsOnMap,
                    EquipOvershieldsOnMap,
                    EquipActiveCamoOnMap,
                    EquipGrenadesOnMap,
                    EquipWeaponRespawnTimes,
                    EquipStartingGrenades,
                    EquipPrimaryStartingEquipment,
                    UnsMaxLivingPlayers,
                    UnsTeamsEnabled,
                    UnsAssaultBombMayBeReturned,
                    UnsMaxTeams,
                    UnsEquipSecondaryStartingEquipment,
                    UnsAssaultFuseTime,
                    UnsJuggyMovement,
                    UnsStickyFuse,
                    UnsTerrContestTime,
                    UnsTerrControlTime,
                    UnsOddbCarrInvis,
                    UnsKingInvisInHill,
                    UnsBallCarrDmgResis,
                    UnsKingDmgResInHill,
                    UnsPlayersExDmg,
                    UnsPlayersDmgResis,
                    UnsCtfCarrDmgResis,
                    UnsCtfCarrInvis,
                    UnsJuggyDmgResis,
                    UnsBombCarrDmgResis,
                    UnsBombCarrInvis,
                    UnsForceEvenTeams
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CreateNewVariantStructBlock8 : TagStructure
        {
            public StringId Unknown;
            public UnknownValue Unknown1;
            public List<GDefaultVariantSettingsBlock> Settings;
            public sbyte Unknown2;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum UnknownValue : int
            {
                Slayer,
                Oddball,
                Juggernaut,
                King,
                Ctf,
                Invasion,
                Territories
            }
            
            [TagStructure(Size = 0x8)]
            public class GDefaultVariantSettingsBlock : TagStructure
            {
                public SettingCategoryValue SettingCategory;
                public int Value;
                
                public enum SettingCategoryValue : int
                {
                    MatchRoundSetting,
                    MatchCtfScoreToWin,
                    MatchSlayerScoreToWinRound,
                    MatchOddballScoreToWinRound,
                    MatchKingScoreToWinRound,
                    MatchRaceScoreToWinRound,
                    MatchHeadhunterScoreToWinRound,
                    MatchJuggernautScoreToWinRound,
                    MatchTerritoriesScoreToWinRound,
                    MatchAssaultScoreToWinRound,
                    MatchRoundTimeLimit,
                    MatchRoundsResetMap,
                    MatchTieResolution,
                    MatchObservers,
                    MatchJoinInProgress,
                    MaximumPlayers,
                    LivesPerRound,
                    RespawnTime,
                    SuicidePenalty,
                    Shields,
                    MotionSensor,
                    Invisibility,
                    TeamChanging,
                    TeamScoring,
                    FriendlyFire,
                    TeamRespawnSetting,
                    BetrayalRespawnPenalty,
                    TeamKillerManagement,
                    SlayerBonusPoints,
                    SlayerSuicidePointLoss,
                    SlayerDeathPointLoss,
                    HeadhunterMovingHeadBin,
                    HeadhunterPointMultiplier,
                    HeadhunterSuicidePointLoss,
                    HeadhunterDeathPointLoss,
                    HeadhunterUncontestedBin,
                    HeadhunterSpeedWithHeads,
                    HeadhunterMaxHeadsCarried,
                    KingUncontestedHill,
                    KingTeamTimeMultiplier,
                    KingMovingHill,
                    KingExtraDamageOnHill,
                    KingDmgResistanceOnHill,
                    OddballBallSpawnCount,
                    OddballBallHitDamage,
                    OddballSpeedWithBall,
                    OddballDrivingGunningWithBall,
                    OddballWaypointToBall,
                    RaceRandomTrack,
                    RaceUncontestedFlag,
                    CtfGameType,
                    CtfSuddenDeath,
                    CtfFlagMayBeReturned,
                    CtfFlagAtHomeToScore,
                    CtfFlagResetTime,
                    CtfSpeedWithFlag,
                    CtfFlagHitDamage,
                    CtfDrivingGunningWithFlag,
                    CtfWaypointToOwnFlag,
                    AssaultGameType,
                    AssaultSuddenDeath,
                    AssaultDetonationTime,
                    AssaultBombAtHomeToScore,
                    AssaultArmingTime,
                    AssaultSpeedWithBomb,
                    AssaultBombHitDamage,
                    AssaultDrivingGunningWithBomb,
                    AssaultWaypointToOwnBomb,
                    JuggernautBetrayalPointLoss,
                    JuggernautJuggyExtraDamage,
                    JuggernautJuggyInfiniteAmmo,
                    JuggernautJuggyOvershields,
                    JuggernautJuggyActiveCamo,
                    JuggernautJuggyMotionSensor,
                    TerritoriesTerritoryCount,
                    VehRespawn,
                    VehPrimaryLightLand,
                    VehSecondaryLightLand,
                    VehPrimaryHeavyLand,
                    VehPrimaryFlying,
                    VehSecondaryHeavyLand,
                    VehPrimaryTurret,
                    VehSecondaryTurret,
                    EquipWeaponsOnMap,
                    EquipOvershieldsOnMap,
                    EquipActiveCamoOnMap,
                    EquipGrenadesOnMap,
                    EquipWeaponRespawnTimes,
                    EquipStartingGrenades,
                    EquipPrimaryStartingEquipment,
                    UnsMaxLivingPlayers,
                    UnsTeamsEnabled,
                    UnsAssaultBombMayBeReturned,
                    UnsMaxTeams,
                    UnsEquipSecondaryStartingEquipment,
                    UnsAssaultFuseTime,
                    UnsJuggyMovement,
                    UnsStickyFuse,
                    UnsTerrContestTime,
                    UnsTerrControlTime,
                    UnsOddbCarrInvis,
                    UnsKingInvisInHill,
                    UnsBallCarrDmgResis,
                    UnsKingDmgResInHill,
                    UnsPlayersExDmg,
                    UnsPlayersDmgResis,
                    UnsCtfCarrDmgResis,
                    UnsCtfCarrInvis,
                    UnsJuggyDmgResis,
                    UnsBombCarrDmgResis,
                    UnsBombCarrInvis,
                    UnsForceEvenTeams
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CreateNewVariantStructBlock9 : TagStructure
        {
            public StringId Unknown;
            public UnknownValue Unknown1;
            public List<GDefaultVariantSettingsBlock> Settings;
            public sbyte Unknown2;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum UnknownValue : int
            {
                Slayer,
                Oddball,
                Juggernaut,
                King,
                Ctf,
                Invasion,
                Territories
            }
            
            [TagStructure(Size = 0x8)]
            public class GDefaultVariantSettingsBlock : TagStructure
            {
                public SettingCategoryValue SettingCategory;
                public int Value;
                
                public enum SettingCategoryValue : int
                {
                    MatchRoundSetting,
                    MatchCtfScoreToWin,
                    MatchSlayerScoreToWinRound,
                    MatchOddballScoreToWinRound,
                    MatchKingScoreToWinRound,
                    MatchRaceScoreToWinRound,
                    MatchHeadhunterScoreToWinRound,
                    MatchJuggernautScoreToWinRound,
                    MatchTerritoriesScoreToWinRound,
                    MatchAssaultScoreToWinRound,
                    MatchRoundTimeLimit,
                    MatchRoundsResetMap,
                    MatchTieResolution,
                    MatchObservers,
                    MatchJoinInProgress,
                    MaximumPlayers,
                    LivesPerRound,
                    RespawnTime,
                    SuicidePenalty,
                    Shields,
                    MotionSensor,
                    Invisibility,
                    TeamChanging,
                    TeamScoring,
                    FriendlyFire,
                    TeamRespawnSetting,
                    BetrayalRespawnPenalty,
                    TeamKillerManagement,
                    SlayerBonusPoints,
                    SlayerSuicidePointLoss,
                    SlayerDeathPointLoss,
                    HeadhunterMovingHeadBin,
                    HeadhunterPointMultiplier,
                    HeadhunterSuicidePointLoss,
                    HeadhunterDeathPointLoss,
                    HeadhunterUncontestedBin,
                    HeadhunterSpeedWithHeads,
                    HeadhunterMaxHeadsCarried,
                    KingUncontestedHill,
                    KingTeamTimeMultiplier,
                    KingMovingHill,
                    KingExtraDamageOnHill,
                    KingDmgResistanceOnHill,
                    OddballBallSpawnCount,
                    OddballBallHitDamage,
                    OddballSpeedWithBall,
                    OddballDrivingGunningWithBall,
                    OddballWaypointToBall,
                    RaceRandomTrack,
                    RaceUncontestedFlag,
                    CtfGameType,
                    CtfSuddenDeath,
                    CtfFlagMayBeReturned,
                    CtfFlagAtHomeToScore,
                    CtfFlagResetTime,
                    CtfSpeedWithFlag,
                    CtfFlagHitDamage,
                    CtfDrivingGunningWithFlag,
                    CtfWaypointToOwnFlag,
                    AssaultGameType,
                    AssaultSuddenDeath,
                    AssaultDetonationTime,
                    AssaultBombAtHomeToScore,
                    AssaultArmingTime,
                    AssaultSpeedWithBomb,
                    AssaultBombHitDamage,
                    AssaultDrivingGunningWithBomb,
                    AssaultWaypointToOwnBomb,
                    JuggernautBetrayalPointLoss,
                    JuggernautJuggyExtraDamage,
                    JuggernautJuggyInfiniteAmmo,
                    JuggernautJuggyOvershields,
                    JuggernautJuggyActiveCamo,
                    JuggernautJuggyMotionSensor,
                    TerritoriesTerritoryCount,
                    VehRespawn,
                    VehPrimaryLightLand,
                    VehSecondaryLightLand,
                    VehPrimaryHeavyLand,
                    VehPrimaryFlying,
                    VehSecondaryHeavyLand,
                    VehPrimaryTurret,
                    VehSecondaryTurret,
                    EquipWeaponsOnMap,
                    EquipOvershieldsOnMap,
                    EquipActiveCamoOnMap,
                    EquipGrenadesOnMap,
                    EquipWeaponRespawnTimes,
                    EquipStartingGrenades,
                    EquipPrimaryStartingEquipment,
                    UnsMaxLivingPlayers,
                    UnsTeamsEnabled,
                    UnsAssaultBombMayBeReturned,
                    UnsMaxTeams,
                    UnsEquipSecondaryStartingEquipment,
                    UnsAssaultFuseTime,
                    UnsJuggyMovement,
                    UnsStickyFuse,
                    UnsTerrContestTime,
                    UnsTerrControlTime,
                    UnsOddbCarrInvis,
                    UnsKingInvisInHill,
                    UnsBallCarrDmgResis,
                    UnsKingDmgResInHill,
                    UnsPlayersExDmg,
                    UnsPlayersDmgResis,
                    UnsCtfCarrDmgResis,
                    UnsCtfCarrInvis,
                    UnsJuggyDmgResis,
                    UnsBombCarrDmgResis,
                    UnsBombCarrInvis,
                    UnsForceEvenTeams
                }
            }
        }
    }
}

