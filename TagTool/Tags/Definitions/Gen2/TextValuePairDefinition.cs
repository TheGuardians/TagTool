using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "text_value_pair_definition", Tag = "sily", Size = 0x24)]
    public class TextValuePairDefinition : TagStructure
    {
        public ParameterValue Parameter;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag StringList;
        public StringId TitleText;
        public StringId HeaderText;
        public StringId DescriptionText;
        public List<TextValuePairReferenceBlock> TextValuePairs;
        
        public enum ParameterValue : int
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
        
        [TagStructure(Size = 0xC)]
        public class TextValuePairReferenceBlock : TagStructure
        {
            public FlagsValue Flags;
            public int Value;
            public StringId LabelStringId;
            
            [Flags]
            public enum FlagsValue : uint
            {
                DefaultSetting = 1 << 0
            }
        }
    }
}

