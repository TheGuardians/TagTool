using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "multiplayer_variant_settings_interface_definition", Tag = "goof", Size = 0x1D8)]
    public class MultiplayerVariantSettingsInterfaceDefinition : TagStructure
    {
        public CachedTag Unknown1;
        public CachedTag Unknown2;
        public CachedTag Unknown3;
        public List<VariantSettingEditReference> GameEngineSettings;
        public CachedTag DefaultVariantStrings;
        public List<DefaultVariantDefinition> DefaultVariants;
        /// <summary>
        /// create new slayer variant
        /// </summary>
        public DefaultVariantDefinition Unknown4;
        /// <summary>
        /// create new king of the hill variant
        /// </summary>
        public DefaultVariantDefinition Unknown5;
        public DefaultVariantDefinition Unknown7;
        /// <summary>
        /// create new oddball variant
        /// </summary>
        public DefaultVariantDefinition Unknown9;
        /// <summary>
        /// create new juggernaut variant
        /// </summary>
        public DefaultVariantDefinition Unknown10;
        public DefaultVariantDefinition Unknown12;
        /// <summary>
        /// create new capture the flag variant
        /// </summary>
        public DefaultVariantDefinition Unknown14;
        /// <summary>
        /// create new assault variant
        /// </summary>
        public DefaultVariantDefinition Unknown15;
        /// <summary>
        /// create new territories variant
        /// </summary>
        public DefaultVariantDefinition Unknown16;
        public DefaultVariantDefinition Unknown18;
        [TagField(Length = 7)]
        public DefaultVariantDefinition UnusedCreateNewVariants;
        
        [TagStructure(Size = 0x20)]
        public class VariantSettingEditReference : TagStructure
        {
            public SettingCategoryValue SettingCategory;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public List<TextValuePairTagReference> Options;
            public List<NullBlock> Unknown1;
            
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
            
            [TagStructure(Size = 0x10)]
            public class TextValuePairTagReference : TagStructure
            {
                public CachedTag ValuePairs;
            }
            
            [TagStructure()]
            public class NullBlock : TagStructure
            {
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class DefaultVariantDefinition : TagStructure
        {
            public StringId VariantName;
            public VariantTypeValue VariantType;
            public List<DefaultVariantSetting> Settings;
            public sbyte DescriptionIndex;
            [TagField(Flags = Padding, Length = 3)]
            public byte[] Padding1;
            
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
            public class DefaultVariantSetting : TagStructure
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

