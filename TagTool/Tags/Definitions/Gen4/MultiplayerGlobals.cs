using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "multiplayer_globals", Tag = "mulg", Size = 0x18)]
    public class MultiplayerGlobals : TagStructure
    {
        public List<MultiplayerUniversalBlock> Universal;
        public List<MultiplayerRuntimeBlock> Runtime;
        
        [TagStructure(Size = 0x90)]
        public class MultiplayerUniversalBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "unic" })]
            public CachedTag RandomPlayerNames;
            [TagField(ValidTags = new [] { "unic" })]
            public CachedTag TeamNames;
            public List<TeamdefinitionBlock> Teams;
            [TagField(ValidTags = new [] { "unic" })]
            public CachedTag MultiplayerText;
            [TagField(ValidTags = new [] { "unic" })]
            public CachedTag SandboxText;
            [TagField(ValidTags = new [] { "jmrq" })]
            public CachedTag SandboxObjectPropertiesValues;
            [TagField(ValidTags = new [] { "mgee" })]
            public CachedTag Effects;
            public List<GlobalTeamRoleBlock> MultiplayerRoles;
            public List<RequisitionConstantsBlock> RequisitionConstants;
            public List<ScenarioProfilesBlock> PlayerStartingProfile;
            
            [TagStructure(Size = 0x24)]
            public class TeamdefinitionBlock : TagStructure
            {
                public StringId Name;
                public RealRgbColor PrimaryColor;
                public RealRgbColor SecondaryColor;
                public byte ForegroundEmblemIndex;
                public byte BackgroundEmblemIndex;
                public EmbleminfoFlags InfoFlags;
                public PlayercolorEnum PrimaryColorIndex;
                public PlayercolorEnum SecondaryColorIndex;
                public PlayercolorEnum BackgroundColorIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum EmbleminfoFlags : byte
                {
                    AlternateForegroundChannelOff = 1 << 0,
                    FlipForeground = 1 << 1,
                    FlipBackground = 1 << 2
                }
                
                public enum PlayercolorEnum : sbyte
                {
                    PlayerColor0,
                    PlayerColor1,
                    PlayerColor2,
                    PlayerColor3,
                    PlayerColor4,
                    PlayerColor5,
                    PlayerColor6,
                    PlayerColor7,
                    PlayerColor8,
                    PlayerColor9,
                    PlayerColor10,
                    PlayerColor11,
                    PlayerColor12,
                    PlayerColor13,
                    PlayerColor14,
                    PlayerColor15,
                    PlayerColor16,
                    PlayerColor17,
                    PlayerColor18,
                    PlayerColor19,
                    PlayerColor20,
                    PlayerColor21,
                    PlayerColor22,
                    PlayerColor23,
                    PlayerColor24,
                    PlayerColor25,
                    PlayerColor26,
                    PlayerColor27,
                    PlayerColor28,
                    PlayerColor29,
                    PlayerColor30,
                    PlayerColor31
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class GlobalTeamRoleBlock : TagStructure
            {
                public TeamRoleFlags Flags;
                public GlobalMultiplayerTeamDesignatorEnum Team;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<PlayerRoleBlock> PlayerRoles;
                
                [Flags]
                public enum TeamRoleFlags : uint
                {
                    Unused = 1 << 0
                }
                
                public enum GlobalMultiplayerTeamDesignatorEnum : short
                {
                    Defender,
                    Attacker,
                    ThirdParty,
                    FourthParty,
                    FifthParty,
                    SixthParty,
                    SeventhParty,
                    EighthParty,
                    Neutral
                }
                
                [TagStructure(Size = 0x94)]
                public class PlayerRoleBlock : TagStructure
                {
                    public StringId RoleName;
                    public PlayerRoleFlags Flags;
                    [TagField(ValidTags = new [] { "weap" })]
                    public CachedTag PrimaryWeapon;
                    public short PrimaryWeaponRoundsLoaded;
                    public short PrimaryWeaponRoundsTotal;
                    [TagField(ValidTags = new [] { "weap" })]
                    public CachedTag SecondaryWeapon;
                    public short SecondaryWeaponRoundsLoaded;
                    public short SecondaryWeaponRoundsTotal;
                    public short FragGrenadeCount;
                    public short PlasmaGrenadeCount;
                    public short Unused;
                    public short SpawnLocationIndex;
                    [TagField(ValidTags = new [] { "eqip" })]
                    public CachedTag StartingEquipment;
                    [TagField(Length = 32)]
                    public string DisplayName;
                    public CustomAppFlags InstalledApps;
                    public int KillstreakBonusTime; // secs
                    public float MovementSpeedMultiplier;
                    [TagField(ValidTags = new [] { "eqip" })]
                    public CachedTag KillstreakBonusEquipment;
                    [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                    public CachedTag KillstreakBonusActivationSound;
                    
                    [Flags]
                    public enum PlayerRoleFlags : uint
                    {
                        Unused = 1 << 0
                    }
                    
                    [Flags]
                    public enum CustomAppFlags : uint
                    {
                        Weightless = 1 << 0,
                        Defiance = 1 << 1,
                        Maltreat = 1 << 2,
                        Recharge = 1 << 3,
                        Impervious = 1 << 4,
                        Exploiter = 1 << 5,
                        Momentum = 1 << 6,
                        Reclaim = 1 << 7,
                        Detector = 1 << 8,
                        Scrimmage = 1 << 9,
                        Sprint = 1 << 10,
                        Twofold = 1 << 11,
                        Supplement = 1 << 12,
                        Manifest = 1 << 13
                    }
                }
            }
            
            [TagStructure(Size = 0x68)]
            public class RequisitionConstantsBlock : TagStructure
            {
                // multiplier to apply to money earned by minions to also give to the fireteam leader
                public float FtlBonusFraction;
                public int Kill;
                public int Assist;
                public int FireTeamLeaderKill;
                // Default, only applies if the vehicle doesn't have a custom award amount in the scenario requisition palette.
                public int VehicleKill;
                // awarded to entire team
                public int ObjectiveDestroyed;
                // awarded to entire team
                public int ObjectiveArmed;
                // awarded to entire team
                public int ObjectiveDisarmed;
                // awarded every 3 seconds to any individuals near secondary defensive objectives
                public int ObjectiveDefending;
                // awarded every 3 seconds to entire team that owns BFG
                public int NeutralTerritoryOwned;
                // awarded to a reinforcement target when a teammate spawns on him (to encourage cooperation)
                public int ServedAsReinforcementTarget;
                // awarded on gaining ownership of a gun to every member of the new owning team
                public int UberassaultGunCaptured;
                // awarded every 3 seconds to the entire team that owns this gun.  Money from multiple guns stacks (so if you own all
                // 3, you'll get 3x this money every 3 seconds).
                public int UberassaultGunOwned;
                public int BetrayedATeammate;
                public int BronzeKillMinimum;
                public int SilverKillMinimum;
                public int GoldKillMinimum;
                public float BronzeMultiplier;
                public float SilverMultiplier;
                public float GoldMultiplier;
                public int BronzeAdvancementTime;
                public int SilverAdvancementTime;
                public int GoldAdvancementTime;
                public List<RequisitionPaletteBlock> RequisitionPalette;
                
                [TagStructure(Size = 0x18)]
                public class RequisitionPaletteBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "obje","vehi","capg" })]
                    public CachedTag Name;
                    public StringId DisplayName;
                    public RequisitionSpecialBuyEnum SpecialBuy;
                    
                    public enum RequisitionSpecialBuyEnum : int
                    {
                        None,
                        Airstrike,
                        MacCannon,
                        MagneticAmmo,
                        LaserAmmo,
                        ExplosiveAmmo,
                        NormalAmmo,
                        FriendlyAiLightInfantry,
                        FriendlyAiHeavyInfantry,
                        FriendlyAiLightVehicle,
                        FriendlyAiHeavyVehicle,
                        FriendlyAiFlyer
                    }
                }
            }
            
            [TagStructure(Size = 0x7C)]
            public class ScenarioProfilesBlock : TagStructure
            {
                [TagField(Length = 32)]
                public string Name;
                public float StartingHealthDamage; // [0,1]
                public float StartingShieldDamage; // [0,1]
                [TagField(ValidTags = new [] { "weap" })]
                public CachedTag PrimaryWeapon;
                // -1 = weapon default
                public short PrimaryroundsLoaded;
                // -1 = weapon default
                public short PrimaryroundsTotal;
                // 0.0 = default, 1.0 = full
                public float PrimaryageRemaining;
                [TagField(ValidTags = new [] { "weap" })]
                public CachedTag SecondaryWeapon;
                // -1 = weapon default
                public short SecondaryroundsLoaded;
                // -1 = weapon default
                public short SecondaryroundsTotal;
                // 0.0 = default, 1.0 = full
                public float SecondaryageRemaining;
                public sbyte StartingFragmentationGrenadeCount;
                public sbyte StartingPlasmaGrenadeCount;
                public sbyte StartingGrenade3Count;
                public sbyte StartingGrenade4Count;
                public sbyte StartingGrenade5Count;
                public sbyte StartingGrenade6Count;
                public sbyte StartingGrenade7Count;
                public sbyte StartingGrenade8Count;
                [TagField(ValidTags = new [] { "eqip" })]
                public CachedTag StartingEquipment;
                public StringId StartingTacticalPackage;
                public StringId StartingSupportUpgrade;
                public short EditorFolder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
        }
        
        [TagStructure(Size = 0x188)]
        public class MultiplayerRuntimeBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "unit" })]
            public CachedTag EditorBiped;
            [TagField(ValidTags = new [] { "obje" })]
            public CachedTag EditorHelper;
            [TagField(ValidTags = new [] { "item" })]
            public CachedTag Flag;
            [TagField(ValidTags = new [] { "item" })]
            public CachedTag Ball;
            [TagField(ValidTags = new [] { "item" })]
            public CachedTag AssaultBomb;
            [TagField(ValidTags = new [] { "obje" })]
            public CachedTag VipInfluenceArea;
            [TagField(ValidTags = new [] { "unic" })]
            public CachedTag InGameText;
            public List<SoundsBlock> Sounds;
            public List<LoopingSoundsBlock> LoopingSounds;
            [TagField(ValidTags = new [] { "mgls" })]
            public CachedTag MegaloSounds;
            [TagField(ValidTags = new [] { "coms" })]
            public CachedTag CommunicationSounds;
            public int MaximumFragCount;
            public int MaximumPlasmaCount;
            public List<MultiplayerConstantsBlock> MultiplayerConstants;
            public List<GameEngineStatusResponseBlock> StateResponses;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag ScoreboardEmblemBitmap;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag ScoreboardDeadEmblemBitmap;
            [TagField(ValidTags = new [] { "mat ","rm  " })]
            public CachedTag HillShader;
            [TagField(ValidTags = new [] { "siin" })]
            public CachedTag DefaultBipedSimulationInterpolation;
            [TagField(ValidTags = new [] { "siin" })]
            public CachedTag DefaultVehicleSimulationInterpolation;
            [TagField(ValidTags = new [] { "siin" })]
            public CachedTag DefaultCrateSimulationInterpolation;
            [TagField(ValidTags = new [] { "siin" })]
            public CachedTag DefaultItemSimulationInterpolation;
            [TagField(ValidTags = new [] { "siin" })]
            public CachedTag DefaultProjectileSimulationInterpolation;
            [TagField(ValidTags = new [] { "siin" })]
            public CachedTag DefaultObjectSimulationInterpolation;
            [TagField(ValidTags = new [] { "coop" })]
            public CachedTag CoOpSpawningGlobals;
            [TagField(ValidTags = new [] { "msit" })]
            public CachedTag MegaloStringIdTable;
            // Used for non projectile killcams.
            [TagField(ValidTags = new [] { "kccd" })]
            public CachedTag KillcamParameters;
            
            [TagStructure(Size = 0x10)]
            public class SoundsBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag Sound;
            }
            
            [TagStructure(Size = 0x10)]
            public class LoopingSoundsBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "lsnd" })]
                public CachedTag LoopingSound;
            }
            
            [TagStructure(Size = 0x68)]
            public class MultiplayerConstantsBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "ssdf" })]
                public CachedTag DefaultSpawnSettings;
                public float TeleporterRechargeTime; // seconds
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag SandboxEffect;
                public StringId BlockedTeleporterString;
                public StringId VoluntaryRespawnControlInstructions;
                public StringId SpawnAllowedDefaultRespawn;
                public StringId SpawnAtPlayerAllowedLookingAtSelf;
                public StringId SpawnAtPlayerAllowedLookingAtTarget;
                public StringId SpawnAtPlayerAllowedLookingAtPotentialTarget;
                public StringId SpawnAtTerritoryAllowedLookingAtTarget;
                public StringId SpawnAtTerritoryAllowedLookingAtPotentialTarget;
                public StringId YouAreOutOfLives;
                public StringId InvalidSpawnTargetSelected;
                public StringId TargettedPlayerEnemiesNearby;
                public StringId TargettedPlayerUnfriendlyTeam;
                public StringId TargettedPlayerDead;
                public StringId TargettedPlayerInCombat;
                public StringId TargettedPlayerTooFarFromOwnedFlag;
                public StringId NoAvailableNetpoints;
                public StringId TargettedNetpointContested;
            }
            
            [TagStructure(Size = 0x24)]
            public class GameEngineStatusResponseBlock : TagStructure
            {
                public GameEngineStatusFlags Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public GameEngineStatusEnum State;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId FfaMessage;
                public StringId TeamMessage;
                public CachedTag Unused;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                
                [Flags]
                public enum GameEngineStatusFlags : ushort
                {
                    Unused = 1 << 0
                }
                
                public enum GameEngineStatusEnum : short
                {
                    WaitingForSpaceToClear,
                    Observing,
                    RespawningSoon,
                    SittingOut,
                    OutOfLives,
                    Playing,
                    Playing1,
                    Playing2,
                    GameOver,
                    GameOver1,
                    GameOver2,
                    GameOver3,
                    YouHaveFlag,
                    EnemyHasFlag,
                    FlagNotHome,
                    CarryingOddball,
                    YouAreJuggy,
                    YouControlHill,
                    SwitchingSidesSoon,
                    PlayerRecentlyStarted,
                    YouHaveBomb,
                    FlagContested,
                    BombContested,
                    LimitedLivesLeft,
                    LimitedLivesLeft1,
                    LimitedLivesLeft2,
                    Playing3,
                    Playing4,
                    Playing5,
                    WaitingToSpawn,
                    WaitingForGameStart,
                    Blank
                }
            }
        }
    }
}
