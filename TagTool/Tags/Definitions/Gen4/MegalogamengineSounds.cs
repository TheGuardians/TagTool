using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "megalogamengine_sounds", Tag = "mgls", Size = 0xF00)]
    public class MegalogamengineSounds : TagStructure
    {
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Slayer;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Ctf;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag FlagCaptured;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag FlagDropped;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag FlagRecovered;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag FlagReset;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag FlagStolen;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag FlagTaken;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Vip;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag NewVip;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag VipKilled;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Juggernaut;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag NewJuggernaut;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Territories;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag TerritoryCaptured;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag TerritoryLost;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Assault;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BombArmed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BombDetonated;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BombDisarmed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BombDropped;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BombReset;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BombReturned;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BombTaken;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Infection;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Infected;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag LastManStanding;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag NewZombie;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Oddball;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BallSpawned;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BallTaken;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BallDropped;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BallReset;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag King;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag HillControlled;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag HillContested;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag HillMoved;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Headhunter;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Stockpile;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Race;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Defense;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Offense;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DestinationMoved;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag GeneratorArmed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CoreArmed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag GeneratorDisarmed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CoreDisarmed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag SuddenDeath;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag GameOver;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BoneCvDefeat;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BoneCvPh1Defeat;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BoneCvPh1Intro;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BoneCvPh1Victory;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BoneCvPh2Defeat;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BoneCvPh2Victory;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BoneCvPh3Victory;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BoneCvVictory;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BoneSpDefeat;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BoneSpPh1Intro;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BoneSpPh1Victory;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BoneSpPh2Intro;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BoneSpPh2Victory;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BoneSpPh3Intro;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BoneSpPh3Victory;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag IsleCvDefeat;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag IsleCvPh1Defeat;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag IsleCvPh1Intro;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag IsleCvPh2Intro;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag IsleCvPh2Victory;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag IsleCvPh3Intro;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag IsleCvPh3Victory;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag IsleSpDefeat;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag IsleSpPh1Defeat;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag IsleSpPh1Extra;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag IsleSpPh1Intro;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag IsleSpPh1Victory;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag IsleSpPh2Defeat;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag IsleSpPh2Victory;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag IsleSpPh3Victory;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag IsleSpVictory;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BoneSpPh3Defeat;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag IsleCvPh3Defeat;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CovyBigWin;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CovyWin1;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CovyWin2;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag InvasionBeginning;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag UnscBigWin;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag UnscWin1;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag UnscWin2;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag PowerDown;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag Reinforcements;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RespawnTick;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag AlphaUnderAttack;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BravoUnderAttack;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CharlieUnderAttack;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomBase1;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomBase2;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomBase3;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomBase4;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomBase5;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomBaseContested;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomBaseCaptured;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomBaseGendown;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomScoreTick;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag RegicideIntro;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag KingCrownedRegicide;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag KingCrownedRegicide2;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag KingCrownedFrompoints;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag KingCrownedFrompoints2;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag KingKilled;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag KingKilled2;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag KingMaxBonus;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag KingSpree;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag KingTacular;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag KingReignofterror;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag KingSlayer;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag KingExecution;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag KingItsyou;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomOfflineBase;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomOfflineBase1;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomOfflineBase2;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomOfflineBase3;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomOfflineBase4;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomOfflineBase5;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomOnlineBase;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomOnlineBase1;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomOnlineBase2;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomOnlineBase3;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomOnlineBase4;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomOnlineBase5;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapEnemyBase;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapEnemyBase1;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapEnemyBase2;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapEnemyBase3;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapEnemyBase4;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapEnemyBase5;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapStartEnemyBase;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapStartEnemyBase1;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapStartEnemyBase2;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapStartEnemyBase3;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapStartEnemyBase4;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapStartEnemyBase5;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapTeamBase;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapTeamBase1;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapTeamBase2;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapTeamBase3;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapTeamBase4;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapTeamBase5;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapStopTeamBase;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapStopTeamBase1;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapStopTeamBase2;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapStopTeamBase3;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapStopTeamBase4;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomCapStopTeamBase5;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomLaststandCappedAll;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomLaststandCappedNone;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomLaststandMustCap;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomTitle;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomTitleKingdom;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomTitleTyranny;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomObective;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomObectiveKingdom;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomObectiveTyranny;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomTeamCastleLocked;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomTeamCastleUnlocked;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomEnemyCastleLocked;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomEnemyCastleUnlocked;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomTeamUpgrade2Base1;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomTeamUpgrade3Base1;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomTeamUpgrade2Base2;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomTeamUpgrade3Base2;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomTeamUpgrade2Base3;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomTeamUpgrade3Base3;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomTeamUpgrade2Base4;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomTeamUpgrade3Base4;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomTeamUpgrade2Base5;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomTeamUpgrade3Base5;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomActivateCapSfx;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomDeactivateCapSfx;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomVehicleTerminalUseSfx;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag DomLostBase;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CaDomBaseUpgrade;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CaDomBaseBoot;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CaDomBasePowerOffline;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CaDomBasePowerRestored;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CaDomGeneratorPowerOffline;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CaDomGeneratorPowerRestored;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CaDomBaseAmbientStart;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CaDomBaseAmbientStop;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CaDomShieldPowerUp;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CaDomShieldPowerDown;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CaDomBaseTerminalUseStart;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CaDomBaseTerminalUseStop;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CaDomVehicleTerminalUseStart;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CaDomVehicleTerminalUseStop;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CaDomGeneratorTerminalUseStart;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CaDomGeneratorTerminalUseStop;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CaDomGenericStop;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CaDomGenericStart;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombDisarmed;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombDisarmedSite1;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombDisarmedSite2;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombDisarmedSite3;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombDisarmedSite4;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombDisarmedSite5;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombPlanted;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombPlantedSite1;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombPlantedSite2;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombPlantedSite3;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombPlantedSite4;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombPlantedSite5;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombSiteAvailable;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombSiteMoved;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamObjectiveAttack;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamObjectiveDefend;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamTitle;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamTitleSymmetric;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamTitleGrifball;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamTitleOneshot;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamTitleScorchedearth;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombDestroyedSite1;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombDestroyedSite2;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombDestroyedSite3;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombDestroyedSite4;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombDestroyedSite5;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombArmingSfx;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombDisarmingSfx;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag BlamBombExplosionSfx;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EscapeObjectiveAttacker;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EscapeObjectiveDefender;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EscapeObjectiveAttackerSwitches;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EscapeObjectiveDefenderSwitches;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EscapePortalMoved;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EscapePortalOff;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EscapePortalOn;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EscapePortalOpen;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EscapeSwitchActivated;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EscapeSwitchPortalOn;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EscapeSwitchPortalOff;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag EscapeTitle;
    }
}
