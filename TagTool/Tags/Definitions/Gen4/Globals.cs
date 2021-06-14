using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x8E0)]
    public class Globals : TagStructure
    {
        [TagField(Length = 0xAC, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public LanguageEnum Language;
        public List<HavokCleanupResourcesBlock> HavokCleanupResources;
        public List<SoundGlobalsBlock> SoundGlobals;
        public List<AiGlobalsDataBlockStruct> Deprecated;
        [TagField(ValidTags = new [] { "aigl" })]
        public CachedTag AiGlobalsRef;
        public List<GameGlobalsDamageBlock> DamageTable;
        public List<SoundBlock> Sounds;
        public List<CameraBlock> Camera;
        public List<ControllerInputBlock> ControllerInput;
        public List<PlayerControlBlock> PlayerControl;
        public List<GameEnginePlayerTraitsBlock> DefaultPlayerTraits;
        public List<DifficultyBlock> Difficulty;
        public List<CoopDifficultyBlockStruct> CoOpDifficulty;
        public List<SoftCeilingGlobalsBlock> SoftCeilings;
        public List<InterfaceTagReferences> InterfaceTags;
        public List<CheatWeaponsBlock> WeaponList;
        public List<CheatPowerupsBlock> CheatPowerups;
        public List<PlayerInformationBlock> PlayerInformation;
        public List<PlayerRepresentationBlock> PlayerRepresentation;
        public List<DamageGlobalsBlock> Damage;
        public List<ShieldBoostBlock> ShieldBoost;
        public List<MaterialsBlock> Materials;
        public List<MultiplayerColorBlock> ProfileColors;
        public List<MultiplayerColorBlock> EmblemColors;
        public List<VisorColorBlock> VisorColors;
        public EliteSpecularColorStruct EliteSpecularColor;
        [TagField(ValidTags = new [] { "forg" })]
        public CachedTag ForgeGlobals;
        [TagField(ValidTags = new [] { "gegl" })]
        public CachedTag GameEngineGlobals;
        [TagField(ValidTags = new [] { "mulg" })]
        public CachedTag MultiplayerGlobals;
        [TagField(ValidTags = new [] { "smdt" })]
        public CachedTag SurvivalModeGlobals;
        [TagField(ValidTags = new [] { "ffgd" })]
        public CachedTag FirefightGlobals;
        [TagField(ValidTags = new [] { "motl" })]
        public CachedTag GlobalMultiplayerObjectTypeList;
        [TagField(ValidTags = new [] { "capg" })]
        public CachedTag CustomAppGlobals;
        [TagField(ValidTags = new [] { "gggl" })]
        public CachedTag Grenades;
        [TagField(ValidTags = new [] { "ggol" })]
        public CachedTag Ordnances;
        [TagField(ValidTags = new [] { "slag" })]
        public CachedTag SilentAssist;
        public List<CinematicsGlobalsBlock> CinematicsGlobals;
        public List<CampaignMetagameGlobalsBlock> CampaignMetagameGlobals;
        [TagField(ValidTags = new [] { "gmeg" })]
        public CachedTag GameMedalGlobals;

        [TagField(Length = 17)]
        public LanguagePackDefinition[] LanguagePack = new LanguagePackDefinition[17];

        [TagField(ValidTags = new [] { "rasg" })]
        public CachedTag RasterizerGlobalsRef;
        [TagField(ValidTags = new [] { "cfxs" })]
        public CachedTag DefaultCameraFxSettings;
        [TagField(ValidTags = new [] { "wind" })]
        public CachedTag DefaultWindSettings;
        [TagField(ValidTags = new [] { "wxcg" })]
        public CachedTag WeatherGlobals;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag CollisionDamageEffect;
        [TagField(ValidTags = new [] { "cddf" })]
        public CachedTag CollisionDamage;
        // the default value for what material type water is
        public StringId GlobalWaterMaterial;
        // the default value for what material type air is
        public StringId GlobalAirMaterial;
        public short GlobalWaterMaterialType;
        public short GlobalAirMaterialType;
        [TagField(ValidTags = new [] { "effg" })]
        public CachedTag EffectGlobals;
        [TagField(ValidTags = new [] { "hcfd" })]
        public CachedTag HavokCollisionFilter;
        [TagField(ValidTags = new [] { "grfr" })]
        public CachedTag DefaultItemGroundedFriction;
        public List<ActiveCamoGlobalsBlock> ActiveCamo;
        [TagField(ValidTags = new [] { "igpd" })]
        public CachedTag IncidentGlobalProperties;
        [TagField(ValidTags = new [] { "pggd" })]
        public CachedTag PlayerGradeGlobals;
        [TagField(ValidTags = new [] { "pegd" })]
        public CachedTag EnlistmentGlobals;
        [TagField(ValidTags = new [] { "pmcg" })]
        public CachedTag PlayerModelCustomizationGlobals;
        [TagField(ValidTags = new [] { "lgtd" })]
        public CachedTag LoadoutGlobals;
        [TagField(ValidTags = new [] { "chdg" })]
        public CachedTag ChallengeGlobals;
        [TagField(ValidTags = new [] { "gcrg" })]
        public CachedTag GameCompletionRewardsGlobals;
        [TagField(ValidTags = new [] { "achi" })]
        public CachedTag GameAchievements;
        [TagField(ValidTags = new [] { "avat" })]
        public CachedTag GameAvatarAwards;
        [TagField(ValidTags = new [] { "gptd" })]
        public CachedTag GamePerformanceThortlesDefault;
        [TagField(ValidTags = new [] { "armg" })]
        public CachedTag ArmormodGlobals;
        [TagField(ValidTags = new [] { "prog" })]
        public CachedTag ProgressionGlobals;
        public List<GarbageCollectionBlock> GarbageCollection;
        public List<GlobalCameraImpulseBlock> CameraImpulse;
        public List<RuntimeMaterialsBlock> RuntimeMaterials;
        public List<HologramlightingGlobalsBlock> HologramLighting;
        [TagField(ValidTags = new [] { "narg" })]
        public CachedTag NarrativeGlobals;
        
        public enum LanguageEnum : int
        {
            English,
            Japanese,
            German,
            French,
            Spanish,
            MexicanSpanish,
            Italian,
            Korean,
            ChineseTraditional,
            ChineseSimplified,
            Portuguese,
            Polish,
            Russian,
            Danish,
            Finnish,
            Dutch,
            Norwegian
        }
        
        [TagStructure(Size = 0x10)]
        public class HavokCleanupResourcesBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag ObjectCleanupEffect;
        }
        
        [TagStructure(Size = 0xB8)]
        public class SoundGlobalsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "sncl" })]
            public CachedTag SoundClasses;
            [TagField(ValidTags = new [] { "sfx+" })]
            public CachedTag SoundEffects;
            [TagField(ValidTags = new [] { "snmx" })]
            public CachedTag SoundMix;
            [TagField(ValidTags = new [] { "spk!" })]
            public CachedTag SoundCombatDialogueConstants;
            [TagField(ValidTags = new [] { "sgp!" })]
            public CachedTag SoundPropagation;
            [TagField(ValidTags = new [] { "sbnk" })]
            // Init sound bank for WWise.
            public CachedTag InitSoundBank;
            [TagField(ValidTags = new [] { "sbnk" })]
            // Global sound bank for WWise.
            public CachedTag GlobalSoundBank;
            [TagField(ValidTags = new [] { "sbnk" })]
            // The other sound bank for WWise.
            public CachedTag ExtraSoundBank;
            [TagField(ValidTags = new [] { "sbnk" })]
            // Extra sound bank for WWise - only loaded for Campaign.
            public CachedTag CampaignSoundBank;
            [TagField(ValidTags = new [] { "sbnk" })]
            // Extra sound bank for WWise - only loaded for MP - PVP.
            public CachedTag MultiplayerSoundBank;
            public List<StreamingPackBlock> StreamingPackFiles;
            public List<CampaignUnspatializedSoundsBlock> UnSpatializedCampaignSounds;
            
            [TagStructure(Size = 0x24)]
            public class StreamingPackBlock : TagStructure
            {
                public StreamingPackBlockFlags Flags;
                [TagField(Length = 32)]
                // Name of the stream pack file
                public string StreamingPckFile;
                
                [Flags]
                public enum StreamingPackBlockFlags : uint
                {
                    LanguagePack = 1 << 0,
                    SoundBankPack = 1 << 1
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class CampaignUnspatializedSoundsBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag Sound;
            }
        }
        
        [TagStructure(Size = 0x1B0)]
        public class AiGlobalsDataBlockStruct : TagStructure
        {
            // Global scale on weapon damage made by AI on other AI
            public float AiInfantryOnAiWeaponDamageScale; // [0,1]
            // Global scale on weapon damage made by AI in a vehicle on other AI
            public float AiVehicleOnAiWeaponDamageScale; // [0,1]
            // Global scale on weapon damage made by AI in a vehicle with the player on other AI
            public float AiPlayerVehicleOnAiWeaponDamageScale; // [0,1]
            public float DangerBroadlyFacing;
            public float DangerShootingNear;
            public float DangerShootingAt;
            public float DangerExtremelyClose;
            public float DangerShieldDamage;
            public float DangerExetendedShieldDamage;
            public float DangerBodyDamage;
            public float DangerExtendedBodyDamage;
            [TagField(ValidTags = new [] { "adlg" })]
            public CachedTag GlobalDialogueTag;
            public StringId DefaultMissionDialogueSoundEffect;
            public float JumpDown; // wu/tick
            public float JumpStep; // wu/tick
            public float JumpCrouch; // wu/tick
            public float JumpStand; // wu/tick
            public float JumpStorey; // wu/tick
            public float JumpTower; // wu/tick
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float MaxJumpDownHeightDown; // wu
            public float MaxJumpDownHeightStep; // wu
            public float MaxJumpDownHeightCrouch; // wu
            public float MaxJumpDownHeightStand; // wu
            public float MaxJumpDownHeightStorey; // wu
            public float MaxJumpDownHeightTower; // wu
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public Bounds<float> HoistStep; // wus
            public Bounds<float> HoistCrouch; // wus
            public Bounds<float> HoistStand; // wus
            public Bounds<float> VaultStep; // wus
            public Bounds<float> VaultCrouch; // wus
            public float SearchRangeInfantry; // wus
            public float SearchRangeFlying; // wus
            public float SearchRangeVehicle; // wus
            public float SearchRangeGiant; // wus
            public List<AiGlobalsGravemindBlock> GravemindProperties;
            // A target of this scariness is offically considered scary (by combat dialogue, etc.)
            public float ScaryTargetThrehold;
            // A weapon of this scariness is offically considered scary (by combat dialogue, etc.)
            public float ScaryWeaponThrehold;
            public float PlayerScariness;
            public float BerserkingActorScariness;
            public float KamikazeingActorScariness;
            // when an actor's target is invincible, he is this much more scared
            public float InvincibleScariness;
            // I will be dead for at least this long
            public float MinDeathTime; // seconds
            // If there is a projectile within this distance of me, I'll stay dead
            public float ProjectileDistance; // wu
            // If there is any enemy clump within this distance of me, I'll stay dead
            public float IdleClumpDistance; // wu
            // If there is any enemy clump with a status higher than idle within this distance of me, I'll stay dead
            public float DangerousClumpDistance; // wu
            // The number of seconds that must elapse before we try to look for a firing point behind cover to teleport to.
            public float CoverSearchDuration; // seconds
            // The number of seconds we try to look for a firing point that aligns us with the actor's task direction.
            public float TaskDirectionSearchDuration; // seconds
            public List<AiGlobalsFormationBlock> SpawnFormations;
            public List<AiGlobalsSquadTemplateFolderBlockStruct> SquadTemplateFolders;
            public List<AiGlobalsPerformanceTemplateFolderBlockStruct> PerformanceTemplateFolders;
            public List<AiGlobalsCustomStimuliBlock> CustomStimuli;
            public List<AiCueTemplateBlockStruct> CueTemplates;
            // At this distance from the squad, stop.
            public float StopDist; // wu
            // At this distance from the squad, start again.
            public float ResumeDist; // wu
            // Start throttling back at this distance
            public float MinDist; // wu
            // Maximum trottle scale at this distance
            public float MaxDist; // wu
            // Minimum throttle value.
            public float MinScale; // 0-1
            // Chance of passing through a patrol objective without pausing
            public float PassthroughChance;
            // Chance of skipping the search phase when stopped at a patrol objective
            public float SearchPhaseSkipChance;
            // If the squad takes more than this time to get to their new patrol point, cancel it and go on to the next.
            public float PatrolTransitionTimeout; // seconds
            // If the squad takes longer than this time to reposition within a patrol point (e.g. searching and pausing), cancel
            // the point and go on to the next.
            public float PatrolManeuverTimeout; // seconds
            // spend this amount of time at a search firing position when in search phase
            public Bounds<float> PatrolSearchFiringPointTime; // seconds
            // If you are more than this distance from your nearest squadmate, you are officially isolated.
            public float PatrolIsolationDistance; // wus
            // If you are isolated for more than this time, you get deleted.
            public float PatrolIsolationTime; // seconds
            // When a task is disallowed from shooting, turn it off after this delay
            public float KungfuDeactivationDelay; // seconds
            public short SuppressingFireCount;
            public short UncoverCount;
            public short LeapOnCoverCount;
            public short DestroyCoverCount;
            public short GuardCount;
            public short InvestigateCount;
            public List<AiTraitVisionBlockStruct> VisionTraits;
            public List<AiTraitSoundBlockStruct> SoundTraits;
            public List<AiTraitLuckBlockStruct> LuckTraits;
            public List<AiTraitGrenadeBlockStruct> GrenadeTraits;
            public float MaxDecayTime;
            public float DecayTimePing;
            public float SearchPatternRadius;
            public short SearchPatternShellCount;
            public Bounds<short> SearchPatternCellsPerShellRange;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            
            [TagStructure(Size = 0xC)]
            public class AiGlobalsGravemindBlock : TagStructure
            {
                public float MinRetreatTime; // secs
                public float IdealRetreatTime; // secs
                public float MaxRetreatTime; // secs
            }
            
            [TagStructure(Size = 0x10)]
            public class AiGlobalsFormationBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "form" })]
                public CachedTag Formation;
            }
            
            [TagStructure(Size = 0x1C)]
            public class AiGlobalsSquadTemplateFolderBlockStruct : TagStructure
            {
                public StringId FolderName;
                public List<AiGlobalsSquadTemplateSubFolderBlockStruct> SubFolders;
                public List<AiGlobalsSquadTemplateBlock> Templates;
                
                [TagStructure(Size = 0x10)]
                public class AiGlobalsSquadTemplateSubFolderBlockStruct : TagStructure
                {
                    public StringId SubFolderName;
                    public List<AiGlobalsSquadTemplateBlock> Templates;
                    
                    [TagStructure(Size = 0x10)]
                    public class AiGlobalsSquadTemplateBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "sqtm" })]
                        public CachedTag SquadTemplate;
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class AiGlobalsSquadTemplateBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "sqtm" })]
                    public CachedTag SquadTemplate;
                }
            }
            
            [TagStructure(Size = 0x1C)]
            public class AiGlobalsPerformanceTemplateFolderBlockStruct : TagStructure
            {
                public StringId FolderName;
                public List<AiGlobalsPerformanceTemplateSubFolderBlockStruct> SubFolders;
                public List<AiGlobalsPerformanceTemplateBlock> Templates;
                
                [TagStructure(Size = 0x10)]
                public class AiGlobalsPerformanceTemplateSubFolderBlockStruct : TagStructure
                {
                    public StringId SubFolderName;
                    public List<AiGlobalsPerformanceTemplateBlock> Templates;
                    
                    [TagStructure(Size = 0x10)]
                    public class AiGlobalsPerformanceTemplateBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "pfmc" })]
                        public CachedTag ThespianTemplate;
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class AiGlobalsPerformanceTemplateBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "pfmc" })]
                    public CachedTag ThespianTemplate;
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class AiGlobalsCustomStimuliBlock : TagStructure
            {
                public StringId Name;
            }
            
            [TagStructure(Size = 0x2C)]
            public class AiCueTemplateBlockStruct : TagStructure
            {
                public StringId Name;
                public CueTemplateFlags TemplateFlags;
                public List<FiringPointPayloadBlockStruct> FiringPoints;
                public List<StimulusPayloadBlockStruct> Stimulus;
                public List<CombatCuePayloadBlockStruct> CombatCue;
                
                [Flags]
                public enum CueTemplateFlags : uint
                {
                    Ignored = 1 << 0,
                    PassiveStimulus = 1 << 1
                }
                
                [TagStructure(Size = 0x4)]
                public class FiringPointPayloadBlockStruct : TagStructure
                {
                    public float Radius;
                }
                
                [TagStructure(Size = 0x4)]
                public class StimulusPayloadBlockStruct : TagStructure
                {
                    public StringId StimulusType;
                }
                
                [TagStructure(Size = 0x34)]
                public class CombatCuePayloadBlockStruct : TagStructure
                {
                    public RealPoint3d Position;
                    public int PackedkeyOffaceref;
                    public int NavmeshUidoffaceref;
                    public GFiringPositionFlags Flags;
                    public GFiringPositionPostureFlags PostureFlags;
                    public short Area;
                    public short ClusterIndex;
                    public short ClusterBsp;
                    public sbyte BitsAndPad;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public RealEulerAngles2d Normal;
                    public Angle Facing;
                    public int LastabsoluteRejectionGameTime;
                    public CombatCuePreferenceEnum Preference;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    
                    [Flags]
                    public enum GFiringPositionFlags : ushort
                    {
                        Open = 1 << 0,
                        Partial = 1 << 1,
                        Closed = 1 << 2,
                        Mobile = 1 << 3,
                        WallLean = 1 << 4,
                        Perch = 1 << 5,
                        GroundPoint = 1 << 6,
                        DynamicCoverPoint = 1 << 7,
                        AutomaticallyGenerated = 1 << 8,
                        NavVolume = 1 << 9
                    }
                    
                    [Flags]
                    public enum GFiringPositionPostureFlags : ushort
                    {
                        CornerLeft = 1 << 0,
                        CornerRight = 1 << 1,
                        Bunker = 1 << 2,
                        BunkerHigh = 1 << 3,
                        BunkerLow = 1 << 4
                    }
                    
                    public enum CombatCuePreferenceEnum : short
                    {
                        Low,
                        High,
                        Total
                    }
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class AiTraitVisionBlockStruct : TagStructure
            {
                // Scale the distance at which an AI can see their target.
                public float VisionDistanceScale;
                // Scale the angles of the AI's vision cone.
                public float VisionAngleScale;
            }
            
            [TagStructure(Size = 0x4)]
            public class AiTraitSoundBlockStruct : TagStructure
            {
                // Scale the character's hearing distance.
                public float HearingDistanceScale;
            }
            
            [TagStructure(Size = 0x2C)]
            public class AiTraitLuckBlockStruct : TagStructure
            {
                // Scale the chance of evading fire.
                public float EvasionChanceScale;
                // Scale the chance of diving from grenades.
                public float GrenadeDiveChanceScale;
                // Scale the chance of going kamikaze when broken.
                public float BrokenKamikazeChanceScale;
                // Scale the chance of retreating when your leader dies.
                public float LeaderDeadRetreatChanceScale;
                // Scale the chance of retreating after a dive.
                public float DiveRetreatChanceScale;
                // Scale the chance of berserking when your shield is depleted.
                public float ShieldDepletedBerserkChanceScale;
                // Scale the chance of a leader berserking when all his followers die.
                public float LeaderAbandonedBerserkChanceScale;
                // Scale the time between melee attacks.
                public float MeleeAttackDelayTimerScale;
                // Scale the chance of meleeing.
                public float MeleeChanceScale;
                // Scale the delay for performing melee leaps.
                public float MeleeLeapDelayTimerScale;
                // Scale the time between grenade throws.
                public float ThrowGrenadeDelayScale;
            }
            
            [TagStructure(Size = 0x1C)]
            public class AiTraitGrenadeBlockStruct : TagStructure
            {
                // Scale the velocity at which AI throws grenades
                public float VelocityScale;
                // Scale the time between grenade throws.
                public float ThrowGrenadeDelayScale;
                public float DonTDropGrenadesChanceScale;
                public float GrenadeUncoverChanceScale;
                public float RetreatThrowGrenadeChanceScale;
                public float AntiVehicleGrenadeChanceScale;
                public float ThrowGrenadeChanceScale;
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class GameGlobalsDamageBlock : TagStructure
        {
            public List<DamageGroupBlock> DamageGroups;
            
            [TagStructure(Size = 0x10)]
            public class DamageGroupBlock : TagStructure
            {
                public StringId Name;
                public List<ArmorModifierBlock> ArmorModifiers;
                
                [TagStructure(Size = 0x8)]
                public class ArmorModifierBlock : TagStructure
                {
                    public StringId Name;
                    public float DamageMultiplier;
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class SoundBlock : TagStructure
        {
            public CachedTag Sound;
        }
        
        [TagStructure(Size = 0xD8)]
        public class CameraBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "trak" })]
            public CachedTag DefaultUnitCameraTrack;
            public ScalarFunctionNamedStruct PitchToVerticalOffset;
            public float FieldOfView; // degrees
            public float YawScale;
            public float PitchScale;
            public float ForwardScale;
            public float SideScale;
            public float UpScale;
            // time it takes for the camera to move from the initial distance to the final distance
            public float TransitionTime; // seconds
            // time it takes for the camera to move to its final position during a falling death
            public float FallingDeathTransitionTime; // seconds
            // on the first frame after death, this is how far out of the body the camera will be
            public float InitialDistance; // wu
            // how far from the body the camera will settle
            public float FinalDistance; // wu
            // how far above the body the camera focuses on
            public float DeadCamZOffset; // wu
            // the highest angle the camera can raise to (prevents it from flipping over the vertical axis)
            public float DeadCamMaximumElevation; // radians
            // delay in tracking the killer
            public float DeadCamMovementDelay; // seconds
            // how long the death camera lasts before auto-switching to orbiting camera
            public float TimeToAutoSwitchToOrbiting; // seconds
            // how long the death camera ignores stick inputs and keeps turning towards the killer (should be non-zero because
            // user could have died while running or looking around)
            public float IgnoreStickTime; // seconds
            // how long the death camera ignores shoulder button inputs and keeps turning towards the killer
            public float IgnoreButtonTime; // seconds
            // minimum velocity to switch to fell to death behavior (when biped is not actually falling to death)
            public float DeadCameraMinimumFallingVelocity;
            // the scaling factor for the left stick when the left trigger is fully depressed
            public float MaximumBoostSpeed;
            // seconds. while pegging boost, time to reach maximum speed
            public float TimeToMaximumBoost;
            public GlobalTransitionFunctionsEnum BoostFunction;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // field of view when zoomed
            public float ZoomedFieldOfView; // degrees
            // scaling factor for look speed when zoomed
            public float ZoomedLookSpeed;
            // radius of sphere for collision
            public float BoundingSphereRadius; // wu
            // how quickly the camera responds to the user's input
            public float FlyingCamMovementDelay; // seconds
            // how long it takes to zoom in or out
            public float ZoomTransitionTime; // seconds
            public float VerticalMovementTimeToMaxSpeed;
            public GlobalTransitionFunctionsEnum VerticalMovementFunction;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            // how long it takes in survival mode before switching to flying camera
            public float SurvivalSwitchTime; // seconds
            public float MinimumDistance; // wu
            public float MaximumDistance; // wu
            // how quickly the camera responds to the user's input
            public float OrbitCamMovementDelay; // seconds
            // how far above the object's root node to position the camera's focus point
            public float OrbitCamZOffset; // wu
            // lowest angle the camera can be moved to
            public float OrbitCamMinimumElevation; // radians
            // highest angle the camera can be moved to
            public float OrbitCamMaximumElevation; // radians
            // how fast the film plays when the trigger is fully depressed
            public float MaxPlaybackSpeed;
            // how long it takes for the screen to fade out when rewinding
            public float FadeOutTime; // seconds
            // see above
            public float FadeInTime; // seconds
            // how long it takes the camera to move from first to third person when entering a vehicle
            public float EnterVehicleTransitionTime; // seconds
            // see above
            public float ExitVehicleTransitionTime; // seconds
            public CameraObstructionStruct Obstruction;
            
            public enum GlobalTransitionFunctionsEnum : short
            {
                Linear,
                Early,
                VeryEarly,
                Late,
                VeryLate,
                Cosine,
                One,
                Zero
            }
            
            [TagStructure(Size = 0x14)]
            public class ScalarFunctionNamedStruct : TagStructure
            {
                public MappingFunction Function;
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class CameraObstructionStruct : TagStructure
            {
                // controls how far from the focus point the outer raycasts start.  0 == cone, 1 == cylinder
                public float CylinderFraction;
                // how wide of a cone to test
                public Angle ObstructionTestAngle; // degrees
                // how quickly the camera moves inward when it anticipates a collision
                public float ObstructionMaxInwardAccel; // 1.0/s/s
                // how quickly the camera returns to a normal position when its anticipated distance is further than its current
                public float ObstructionMaxOutwardAccel; // 1.0/s/s
                // maximum speed the camera can move
                public float ObstructionMaxVelocity; // 1.0/s
                // when the camera wants to start moving back out, wait this long before doing so
                public float ObstructionReturnDelay; // s
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class ControllerInputBlock : TagStructure
        {
            public float AxialDeadZone;
            public float RadialDeadZone;
        }
        
        [TagStructure(Size = 0x16C)]
        public class PlayerControlBlock : TagStructure
        {
            public List<ControllerMappingReferenceBlock> ControllerButtonMappings;
            public List<GamepadStickInfoBlock> MoveStickInfo;
            public List<GamepadStickInfoBlock> LookStickInfo;
            // how much the crosshair slows over enemies
            public float MagnetismFriction;
            // how much the crosshair sticks to enemies
            public float MagnetismAdhesion;
            // scales magnetism level for inconsequential targets like infection forms
            public float InconsequentialTargetScale;
            // -1..1, 0 is middle of the screen
            public RealPoint2d CrosshairLocation;
            // How long is command mode on after you initially attempt to issue an order
            public float FireteamCommandModeDuration; // seconds
            // 1 is fast, 0 is none, >1 will probably be really fast
            public float LookAutolevellingScale;
            public float GravityScale;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // amount of time player needs to move and not look up or down for autolevelling to kick in
            public short MinimumAutolevellingTicks;
            // 0 means the vehicle's up vector is along the ground, 90 means the up vector is pointing straight up
            public Angle MinimumAngleForVehicleFlipping; // degrees
            // time that player needs to press ACTION to register as a HOLD
            public float MinimumActionHoldTime; // seconds
            // teammates of player who dropped/spawned weapon
            public float MinimumActionHoldTimeForTeammates; // seconds
            // opponents of player who dropped/spawned weapon
            public float MinimumActionHoldTimeForOpponents; // seconds
            // for spinny-shotgun goodness
            public float PeggedZoomSupressionThreshold;
            public float MinimumVerticalVelocity; // wu/s
            public float CooldownTime; // seconds
            public RealVector2d DoubleJumpVelocity; // horizontal, vertical
            public RealVector2d TripleJumpVelocity; // aiming, vertical
            // how close to an axis you have to be
            public float ThrowChannelWidth;
            // how far from the center you have to be
            public float ThrowPegThreshold;
            // how long the stick must be centered to start a throw
            public float ThrowCenteredMinTime; // s
            // how long you can take to become pegged at the start of a throw
            public float ThrowDrawingMaxTime; // s
            // how long you can stay pegged at the start of a throw
            public float ThrowDrawnMaxTime; // s
            // how long you can take to throw the stick to pegged on the other side
            public float ThrowThrowingMaxTime; // s
            // max time you can be pegged and then press jump to activate
            public float FlickPegJumpMaxTime; // s
            // to engage double-tap, user must press jump twice in this much time
            public float DoubleTapIntervalTime; // s
            public float VaultSpeedGain;
            public float VaultsprintSpeedGain;
            public float VaultHeightHigh;
            public float VaultsprintHeightHigh;
            public float VaultHeightMedium;
            public float VaultsprintHeightMedium;
            public float VaultHeightMin;
            public float VaultsprintHeightMin;
            public float VaultHeightTraverse;
            public float VaultsprintHeightTraverse;
            public float VaultMaxDownwardDistance;
            public float VaultsprintMaxDownwardDistance;
            public float VaultMaxDistance;
            public float VaultsprintMaxDistance;
            public float VaultGravityGain;
            public float VaultsprintGravityGain;
            public float VaultStationaryProbeDistance;
            public float VaultStationaryProbeAngle;
            public float VaultStationaryProbeMinZ;
            public float VaultLateralSpeedGain;
            public float VaultsprintLateralSpeedGain;
            public float JumpAirControlGain;
            public float JumpForwardHorizontalSpeed0;
            public float JumpForwardHorizontalSpeed1;
            public float JumpForwardHorizontalSpeed2;
            public float JumpForwardGravity0;
            public float JumpForwardGravity1;
            public float JumpForwardGravity2;
            public float JumpForwardHeight;
            public float JumpLateralHorizontalSpeed0;
            public float JumpLateralHorizontalSpeed1;
            public float JumpLateralHorizontalSpeed2;
            public float JumpLateralGravity0;
            public float JumpLateralGravity1;
            public float JumpLateralGravity2;
            public float JumpLateralHeight;
            public float JumpReverseHorizontalSpeed0;
            public float JumpReverseHorizontalSpeed1;
            public float JumpReverseHorizontalSpeed2;
            public float JumpReverseGravity0;
            public float JumpReverseGravity1;
            public float JumpReverseGravity2;
            public float JumpReverseHeight;
            public float JumpStationaryHorizontalSpeed0;
            public float JumpStationaryHorizontalSpeed1;
            public float JumpStationaryHorizontalSpeed2;
            public float JumpStationaryGravity0;
            public float JumpStationaryGravity1;
            public float JumpStationaryGravity2;
            public float JumpStationaryHeight;
            public float DownwardVaultMinLaterialDistance;
            public float VaultAutoCorrectMaxAngle;
            // When several via vault targets are found, vaults closer to this distance are more likely to be chosen
            public float VaultIdealDistance;
            // When several via vault targets are found, vaults closer to this distance are more likely to be chosen
            public float VaultSprintIdealDistance;
            
            [TagStructure(Size = 0x10)]
            public class ControllerMappingReferenceBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "cnmp" })]
                public CachedTag Mapping;
            }
            
            [TagStructure(Size = 0x28)]
            public class GamepadStickInfoBlock : TagStructure
            {
                public InputMappingShapeEnum InputShape;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                // magnitude of analog input for pegged acceleration to kick in
                public float PegThreshold01;
                // time for a pegged look to reach maximum effect
                public RealPoint2d PeggedTime;
                // the maximum effect achieved over the duration of the pegged time.
                public RealPoint2d PeggedScale;
                // the maximum turning speed during peg
                public Angle PegMaxAngularVelocity; // degrees per sec
                public List<InputMappingFunctionBlock> InputMappingFunction;
                
                public enum InputMappingShapeEnum : sbyte
                {
                    None,
                    UnitCircle,
                    UnitSquare
                }
                
                [TagStructure(Size = 0x14)]
                public class InputMappingFunctionBlock : TagStructure
                {
                    public ScalarFunctionNamedStruct Function;
                    
                    [TagStructure(Size = 0x14)]
                    public class ScalarFunctionNamedStruct : TagStructure
                    {
                        public MappingFunction Function;
                        
                        [TagStructure(Size = 0x14)]
                        public class MappingFunction : TagStructure
                        {
                            public byte[] Data;
                        }
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x3C)]
        public class GameEnginePlayerTraitsBlock : TagStructure
        {
            public List<PlayerTraitsVitalityBlock> VitalityTraits;
            public List<PlayerTraitsWeaponsBlock> WeaponTraits;
            public List<PlayerTraitsMovementBlock> MovementTraits;
            public List<PlayerTraitsAppearanceBlock> AppearanceTraits;
            public List<PlayerTraitsSensorsBlock> SensorTraits;
            
            [TagStructure(Size = 0x40)]
            public class PlayerTraitsVitalityBlock : TagStructure
            {
                public PlayerTraitsVitalityFloatFlags ShouldApplyTrait;
                public float DamageResistance;
                public float ShieldMultiplier;
                public float BodyMultiplier;
                public float ShieldStunDuration;
                public float ShieldRechargeRate;
                public float BodyRechargeRate;
                public float OvershieldRechargeRate;
                public float VampirismPercent;
                // incoming damage multiplied by (1 - resistance)
                public float ExplosiveDamageResistance;
                public float WheelmanArmorVehicleStunTimeModifier;
                public float WheelmanArmorVehicleRechargeTimeModifier;
                public float WheelmanArmorVehicleEmpDisabledTimeModifier;
                public float FallDamageMultiplier;
                public PlayerTraitBoolEnum HeadshotImmunity;
                public PlayerTraitBoolEnum AssassinationImmunity;
                public PlayerTraitBoolEnum Deathless;
                public PlayerTraitBoolEnum FastTrackArmor;
                public PlayerTraitPowerupCancellationEnum PowerupCancellation;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum PlayerTraitsVitalityFloatFlags : uint
                {
                    DamageResistance = 1 << 0,
                    ShieldMultiplier = 1 << 1,
                    BodyMultiplier = 1 << 2,
                    ShieldStunDuration = 1 << 3,
                    ShieldRechargeRate = 1 << 4,
                    BodyRechargeRate = 1 << 5,
                    OvershieldRechargeRate = 1 << 6,
                    VampirismPercent = 1 << 7,
                    ExplosiveDamageResistance = 1 << 8,
                    WheelmanArmorVehicleStunTimeModifier = 1 << 9,
                    WheelmanArmorVehicleRechargeTimeModifier = 1 << 10,
                    WheelmanArmorVehicleEmpDisabledTimeModifier = 1 << 11,
                    FallDamageMultiplier = 1 << 12
                }
                
                public enum PlayerTraitBoolEnum : sbyte
                {
                    Unchanged,
                    False,
                    True
                }
                
                public enum PlayerTraitPowerupCancellationEnum : sbyte
                {
                    Unchanged,
                    None,
                    NoOvershield
                }
            }
            
            [TagStructure(Size = 0x80)]
            public class PlayerTraitsWeaponsBlock : TagStructure
            {
                public PlayerTraitsWeaponsFloatFlags ShouldApplyTrait;
                public float DamageMultiplier;
                public float MeleeDamageMultiplier;
                public float GrenadeRechargeSecondsFrag;
                public float GrenadeRechargeSecondsPlasma;
                public float GrenadeRechargeSecondsSpike;
                public float HeroEquipmentEnergyUseRateModifier;
                public float HeroEquipmentEnergyRechargeDelayModifier;
                public float HeroEquipmentEnergyRechargeRateModifier;
                public float HeroEquipmentInitialEnergyModifier;
                public float EquipmentEnergyUseRateModifier;
                public float EquipmentEnergyRechargeDelayModifier;
                public float EquipmentEnergyUseRechargeRateModifier;
                public float EquipmentEnergyInitialEnergyModifier;
                public float SwitchSpeedModifier;
                public float ReloadSpeedModifier;
                public float OrdnancePointsModifier;
                public float ExplosiveAreaOfEffectRadiusModifier;
                public float GunnerArmorModifier;
                public float StabilityArmorModifier;
                public float DropReconWarningSeconds;
                public float DropReconDistanceModifier;
                public float AssassinationSpeedModifier;
                public PlayerTraitBoolEnum WeaponPickupAllowed;
                public PlayerTraitInitialGrenadeCountEnum InitialGrenadeCount;
                public PlayerTraitInfiniteAmmoEnum InfiniteAmmo;
                public PlayerTraitEquipmentUsageEnum EquipmentUsage;
                // false will disable all equipment except auto turret
                public PlayerTraitEquipmentUsageEnum EquipmentUsageExceptingAutoTurret;
                public PlayerTraitBoolEnum EquipmentDrop;
                public PlayerTraitBoolEnum InfiniteEquipment;
                public PlayerTraitBoolEnum WeaponsAmmopack;
                public PlayerTraitBoolEnum WeaponsGrenadier;
                // spawns projectile specified in globals.globals
                public PlayerTraitBoolEnum WeaponsExplodeOnDeathArmormod;
                public PlayerTraitBoolEnum OrdnanceMarkersVisible;
                public PlayerTraitBoolEnum WeaponsOrdnanceRerollAvailable;
                // grenade probabilities defined in grenade_list.game_globals_grenade_list
                public PlayerTraitBoolEnum WeaponsResourceful;
                public PlayerTraitBoolEnum WeaponsWellEquipped;
                public PlayerTraitBoolEnum OrdnanceDisabled;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId InitialPrimaryWeapon;
                public StringId InitialSecondaryWeapon;
                public StringId InitialEquipment;
                public StringId InitialTacticalPackage;
                public StringId InitialSupportUpgrade;
                
                [Flags]
                public enum PlayerTraitsWeaponsFloatFlags : uint
                {
                    DamageMultiplier = 1 << 0,
                    MeleeDamageMultiplier = 1 << 1,
                    GrenadeRechargeSecondsFrag = 1 << 2,
                    GrenadeRechargeSecondsPlasma = 1 << 3,
                    GrenadeRechargeSecondsSpike = 1 << 4,
                    HeroEquipmentEnergyUseRateModifier = 1 << 5,
                    HeroEquipmentEnergyRechargeDelayModifier = 1 << 6,
                    HeroEquipmentEnergyRechargeRateModifier = 1 << 7,
                    HeroEquipmentInitialEnergyModifier = 1 << 8,
                    EquipmentEnergyUseRateModifier = 1 << 9,
                    EquipmentEnergyRechargeDelayModifier = 1 << 10,
                    EquipmentEnergyUseRechargeRateModifier = 1 << 11,
                    EquipmentEnergyInitialEnergyModifier = 1 << 12,
                    SwitchSpeedModifier = 1 << 13,
                    ReloadSpeedModifier = 1 << 14,
                    OrdnancePointsModifier = 1 << 15,
                    ExplosiveAreaOfEffectRadiusModifier = 1 << 16,
                    GunnerArmorModifier = 1 << 17,
                    StabilityArmorModifier = 1 << 18,
                    DropReconWarningSeconds = 1 << 19,
                    DropReconDistanceModifier = 1 << 20,
                    AssassinationSpeedModifier = 1 << 21
                }
                
                public enum PlayerTraitBoolEnum : sbyte
                {
                    Unchanged,
                    False,
                    True
                }
                
                public enum PlayerTraitInitialGrenadeCountEnum : sbyte
                {
                    Unchanged,
                    MapDefault,
                    _0,
                    _1Frag,
                    _2Frag,
                    _1Plasma,
                    _2Plasma,
                    _1Type2,
                    _2Type2,
                    _1Type3,
                    _2Type3,
                    _1Type4,
                    _2Type4,
                    _1Type5,
                    _2Type5,
                    _1Type6,
                    _2Type6,
                    _1Type7,
                    _2Type7
                }
                
                public enum PlayerTraitInfiniteAmmoEnum : sbyte
                {
                    Unchanged,
                    Off,
                    On,
                    BottomlessClip
                }
                
                public enum PlayerTraitEquipmentUsageEnum : sbyte
                {
                    Unchanged,
                    Off,
                    NotWithObjectives,
                    On
                }
            }
            
            [TagStructure(Size = 0x1C)]
            public class PlayerTraitsMovementBlock : TagStructure
            {
                public PlayerTraitsMovementFloatFlags ShouldApplyTrait;
                public float Speed;
                public float GravityMultiplier;
                public float JumpMultiplier;
                public float TurnSpeedMultiplier;
                public PlayerTraitVehicleUsage VehicleUsage;
                public PlayerTraitDoubleJump DoubleJump;
                public PlayerTraitBoolEnum SprintUsage;
                public PlayerTraitBoolEnum AutomaticMomentumUsage;
                public PlayerTraitBoolEnum VaultingEnabled;
                public PlayerTraitBoolEnum Stealthy;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum PlayerTraitsMovementFloatFlags : uint
                {
                    Speed = 1 << 0,
                    GravityMultiplier = 1 << 1,
                    JumpMultiplier = 1 << 2,
                    TurnSpeedMultiplier = 1 << 3
                }
                
                public enum PlayerTraitVehicleUsage : sbyte
                {
                    Unchanged,
                    None,
                    PassengerOnly,
                    DriverOnly,
                    GunnerOnly,
                    NotPassenger,
                    NotDriver,
                    NotGunner,
                    Full
                }
                
                public enum PlayerTraitDoubleJump : sbyte
                {
                    Unchanged,
                    Off,
                    On,
                    OnPlusLunge
                }
                
                public enum PlayerTraitBoolEnum : sbyte
                {
                    Unchanged,
                    False,
                    True
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class PlayerTraitsAppearanceBlock : TagStructure
            {
                public PlayerTraitActiveCamo ActiveCamo;
                public PlayerTraitWaypoint Waypoint;
                public PlayerTraitWaypoint GamertagVisible;
                public PlayerTraitAura Aura;
                [TagField(Length = 32)]
                public string DeathEffect;
                public StringId AttachedEffect;
                
                public enum PlayerTraitActiveCamo : sbyte
                {
                    Unchanged,
                    Off,
                    Poor,
                    Good,
                    Excellent,
                    Invisible
                }
                
                public enum PlayerTraitWaypoint : sbyte
                {
                    Unchanged,
                    Off,
                    Allies,
                    All
                }
                
                public enum PlayerTraitAura : sbyte
                {
                    Unchanged,
                    Off,
                    TeamColor,
                    Black,
                    White
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class PlayerTraitsSensorsBlock : TagStructure
            {
                public PlayerTraitsSensorsFloatFlags ShouldApplyTrait;
                public float MotionTrackerRange;
                public float NemesisDuration; // seconds
                public PlayerTraitMotionTracker MotionTracker;
                public PlayerTraitBoolEnum MotionTrackerWhileZoomed;
                public PlayerTraitBoolEnum DirectionalDamageIndicator;
                public PlayerTraitBoolEnum VisionMode;
                public PlayerTraitBoolEnum BattleAwareness;
                public PlayerTraitBoolEnum ThreatView;
                public PlayerTraitBoolEnum AuralEnhancement;
                public PlayerTraitBoolEnum Nemesis;
                
                [Flags]
                public enum PlayerTraitsSensorsFloatFlags : uint
                {
                    MotionTrackerRange = 1 << 0,
                    NemesisDuration = 1 << 1
                }
                
                public enum PlayerTraitMotionTracker : sbyte
                {
                    Unchanged,
                    Off,
                    MovingFriendlyBipedsMovingNeutralVehicles,
                    MovingBipedsMovingVehicles,
                    AllBipedsMovingVehicles
                }
                
                public enum PlayerTraitBoolEnum : sbyte
                {
                    Unchanged,
                    False,
                    True
                }
            }
        }
        
        [TagStructure(Size = 0x284)]
        public class DifficultyBlock : TagStructure
        {
            // enemy damage multiplier on easy difficulty
            public float EasyEnemyDamage;
            // enemy damage multiplier on normal difficulty
            public float NormalEnemyDamage;
            // enemy damage multiplier on hard difficulty
            public float HardEnemyDamage;
            // enemy damage multiplier on impossible difficulty
            public float ImpossEnemyDamage;
            // enemy maximum body vitality scale on easy difficulty
            public float EasyEnemyVitality;
            // enemy maximum body vitality scale on normal difficulty
            public float NormalEnemyVitality;
            // enemy maximum body vitality scale on hard difficulty
            public float HardEnemyVitality;
            // enemy maximum body vitality scale on impossible difficulty
            public float ImpossEnemyVitality;
            // enemy maximum shield vitality scale on easy difficulty
            public float EasyEnemyShield;
            // enemy maximum shield vitality scale on normal difficulty
            public float NormalEnemyShield;
            // enemy maximum shield vitality scale on hard difficulty
            public float HardEnemyShield;
            // enemy maximum shield vitality scale on impossible difficulty
            public float ImpossEnemyShield;
            // enemy shield recharge scale on easy difficulty
            public float EasyEnemyRecharge;
            // enemy shield recharge scale on normal difficulty
            public float NormalEnemyRecharge;
            // enemy shield recharge scale on hard difficulty
            public float HardEnemyRecharge;
            // enemy shield recharge scale on impossible difficulty
            public float ImpossEnemyRecharge;
            // friend damage multiplier on easy difficulty
            public float EasyFriendDamage;
            // friend damage multiplier on normal difficulty
            public float NormalFriendDamage;
            // friend damage multiplier on hard difficulty
            public float HardFriendDamage;
            // friend damage multiplier on impossible difficulty
            public float ImpossFriendDamage;
            // friend maximum body vitality scale on easy difficulty
            public float EasyFriendVitality;
            // friend maximum body vitality scale on normal difficulty
            public float NormalFriendVitality;
            // friend maximum body vitality scale on hard difficulty
            public float HardFriendVitality;
            // friend maximum body vitality scale on impossible difficulty
            public float ImpossFriendVitality;
            // friend maximum shield vitality scale on easy difficulty
            public float EasyFriendShield;
            // friend maximum shield vitality scale on normal difficulty
            public float NormalFriendShield;
            // friend maximum shield vitality scale on hard difficulty
            public float HardFriendShield;
            // friend maximum shield vitality scale on impossible difficulty
            public float ImpossFriendShield;
            // friend shield recharge scale on easy difficulty
            public float EasyFriendRecharge;
            // friend shield recharge scale on normal difficulty
            public float NormalFriendRecharge;
            // friend shield recharge scale on hard difficulty
            public float HardFriendRecharge;
            // friend shield recharge scale on impossible difficulty
            public float ImpossFriendRecharge;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            // enemy rate of fire scale on easy difficulty
            public float EasyRateOfFire;
            // enemy rate of fire scale on normal difficulty
            public float NormalRateOfFire;
            // enemy rate of fire scale on hard difficulty
            public float HardRateOfFire;
            // enemy rate of fire scale on impossible difficulty
            public float ImpossRateOfFire;
            // enemy projectile error scale, as a fraction of their base firing error. on easy difficulty
            public float EasyProjectileError;
            // enemy projectile error scale, as a fraction of their base firing error. on normal difficulty
            public float NormalProjectileError;
            // enemy projectile error scale, as a fraction of their base firing error. on hard difficulty
            public float HardProjectileError;
            // enemy projectile error scale, as a fraction of their base firing error. on impossible difficulty
            public float ImpossProjectileError;
            // enemy burst error scale; reduces intra-burst shot distance. on easy difficulty
            public float EasyBurstError;
            // enemy burst error scale; reduces intra-burst shot distance. on normal difficulty
            public float NormalBurstError;
            // enemy burst error scale; reduces intra-burst shot distance. on hard difficulty
            public float HardBurstError;
            // enemy burst error scale; reduces intra-burst shot distance. on impossible difficulty
            public float ImpossBurstError;
            // enemy new-target delay scale factor. on easy difficulty
            public float EasyNewTargetDelay;
            // enemy new-target delay scale factor. on normal difficulty
            public float NormalNewTargetDelay;
            // enemy new-target delay scale factor. on hard difficulty
            public float HardNewTargetDelay;
            // enemy new-target delay scale factor. on impossible difficulty
            public float ImpossNewTargetDelay;
            // delay time between bursts scale factor for enemies. on easy difficulty
            public float EasyBurstSeparation;
            // delay time between bursts scale factor for enemies. on normal difficulty
            public float NormalBurstSeparation;
            // delay time between bursts scale factor for enemies. on hard difficulty
            public float HardBurstSeparation;
            // delay time between bursts scale factor for enemies. on impossible difficulty
            public float ImpossBurstSeparation;
            // additional target tracking fraction for enemies. on easy difficulty
            public float EasyTargetTracking;
            // additional target tracking fraction for enemies. on normal difficulty
            public float NormalTargetTracking;
            // additional target tracking fraction for enemies. on hard difficulty
            public float HardTargetTracking;
            // additional target tracking fraction for enemies. on impossible difficulty
            public float ImpossTargetTracking;
            // additional target leading fraction for enemies. on easy difficulty
            public float EasyTargetLeading;
            // additional target leading fraction for enemies. on normal difficulty
            public float NormalTargetLeading;
            // additional target leading fraction for enemies. on hard difficulty
            public float HardTargetLeading;
            // additional target leading fraction for enemies. on impossible difficulty
            public float ImpossTargetLeading;
            // overcharge chance scale factor for enemies. on easy difficulty
            public float EasyOverchargeChance;
            // overcharge chance scale factor for enemies. on normal difficulty
            public float NormalOverchargeChance;
            // overcharge chance scale factor for enemies. on hard difficulty
            public float HardOverchargeChance;
            // overcharge chance scale factor for enemies. on impossible difficulty
            public float ImpossOverchargeChance;
            // delay between special-fire shots (overcharge, banshee bombs) scale factor for enemies. on easy difficulty
            public float EasySpecialFireDelay;
            // delay between special-fire shots (overcharge, banshee bombs) scale factor for enemies. on normal difficulty
            public float NormalSpecialFireDelay;
            // delay between special-fire shots (overcharge, banshee bombs) scale factor for enemies. on hard difficulty
            public float HardSpecialFireDelay;
            // delay between special-fire shots (overcharge, banshee bombs) scale factor for enemies. on impossible difficulty
            public float ImpossSpecialFireDelay;
            // guidance velocity scale factor for all projectiles targeted on a player. on easy difficulty
            public float EasyGuidanceVsPlayer;
            // guidance velocity scale factor for all projectiles targeted on a player. on normal difficulty
            public float NormalGuidanceVsPlayer;
            // guidance velocity scale factor for all projectiles targeted on a player. on hard difficulty
            public float HardGuidanceVsPlayer;
            // guidance velocity scale factor for all projectiles targeted on a player. on impossible difficulty
            public float ImpossGuidanceVsPlayer;
            // delay period added to all melee attacks, even when berserk. on easy difficulty
            public float EasyMeleeDelayBase;
            // delay period added to all melee attacks, even when berserk. on normal difficulty
            public float NormalMeleeDelayBase;
            // delay period added to all melee attacks, even when berserk. on hard difficulty
            public float HardMeleeDelayBase;
            // delay period added to all melee attacks, even when berserk. on impossible difficulty
            public float ImpossMeleeDelayBase;
            // multiplier for all existing non-berserk melee delay times. on easy difficulty
            public float EasyMeleeDelayScale;
            // multiplier for all existing non-berserk melee delay times. on normal difficulty
            public float NormalMeleeDelayScale;
            // multiplier for all existing non-berserk melee delay times. on hard difficulty
            public float HardMeleeDelayScale;
            // multiplier for all existing non-berserk melee delay times. on impossible difficulty
            public float ImpossMeleeDelayScale;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            // scale factor affecting the desicions to throw a grenade. on easy difficulty
            public float EasyGrenadeChanceScale;
            // scale factor affecting the desicions to throw a grenade. on normal difficulty
            public float NormalGrenadeChanceScale;
            // scale factor affecting the desicions to throw a grenade. on hard difficulty
            public float HardGrenadeChanceScale;
            // scale factor affecting the desicions to throw a grenade. on impossible difficulty
            public float ImpossGrenadeChanceScale;
            // scale factor affecting the delay period between grenades thrown from the same encounter (lower is more often). on
            // easy difficulty
            public float EasyGrenadeTimerScale;
            // scale factor affecting the delay period between grenades thrown from the same encounter (lower is more often). on
            // normal difficulty
            public float NormalGrenadeTimerScale;
            // scale factor affecting the delay period between grenades thrown from the same encounter (lower is more often). on
            // hard difficulty
            public float HardGrenadeTimerScale;
            // scale factor affecting the delay period between grenades thrown from the same encounter (lower is more often). on
            // impossible difficulty
            public float ImpossGrenadeTimerScale;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            // fraction of actors upgraded to their major variant. on easy difficulty
            public float EasyMajorUpgrade;
            // fraction of actors upgraded to their major variant. on normal difficulty
            public float NormalMajorUpgrade;
            // fraction of actors upgraded to their major variant. on hard difficulty
            public float HardMajorUpgrade;
            // fraction of actors upgraded to their major variant. on impossible difficulty
            public float ImpossMajorUpgrade;
            // fraction of actors upgraded to their major variant when mix = normal. on easy difficulty
            public float EasyMajorUpgrade1;
            // fraction of actors upgraded to their major variant when mix = normal. on normal difficulty
            public float NormalMajorUpgrade1;
            // fraction of actors upgraded to their major variant when mix = normal. on hard difficulty
            public float HardMajorUpgrade1;
            // fraction of actors upgraded to their major variant when mix = normal. on impossible difficulty
            public float ImpossMajorUpgrade1;
            // fraction of actors upgraded to their major variant when mix = many. on easy difficulty
            public float EasyMajorUpgrade2;
            // fraction of actors upgraded to their major variant when mix = many. on normal difficulty
            public float NormalMajorUpgrade2;
            // fraction of actors upgraded to their major variant when mix = many. on hard difficulty
            public float HardMajorUpgrade2;
            // fraction of actors upgraded to their major variant when mix = many. on impossible difficulty
            public float ImpossMajorUpgrade2;
            // Chance of deciding to ram the player in a vehicle on easy difficulty
            public float EasyPlayerVehicleRamChance;
            // Chance of deciding to ram the player in a vehicle on normal difficulty
            public float NormalPlayerVehicleRamChance;
            // Chance of deciding to ram the player in a vehicle on hard difficulty
            public float HardPlayerVehicleRamChance;
            // Chance of deciding to ram the player in a vehicle on impossible difficulty
            public float ImpossPlayerVehicleRamChance;
            // Multiplies the chance that a flying vehicle will trick to dodge danger on easy difficulty
            public float EasyTrickDodgeChanceScale;
            // Multiplies the chance that a flying vehicle will trick to dodge danger on normal difficulty
            public float NormalTrickDodgeChanceScale;
            // Multiplies the chance that a flying vehicle will trick to dodge danger on hard difficulty
            public float HardTrickDodgeChanceScale;
            // Multiplies the chance that a flying vehicle will trick to dodge danger on impossible difficulty
            public float ImpossTrickDodgeChanceScale;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding6;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding7;
            [TagField(Length = 0x54, Flags = TagFieldFlags.Padding)]
            public byte[] Padding8;
        }
        
        [TagStructure(Size = 0x84)]
        public class CoopDifficultyBlockStruct : TagStructure
        {
            // multiplier on enemy shield recharge delay with two coop players
            public float TwoPlayerShieldRechargeDelay;
            // multiplier on enemy shield recharge delay with four coop players
            public float FourPlayerShieldRechargeDelay;
            // multiplier on enemy shield recharge delay with six coop players or more
            public float SixPlayerShieldRechargeDelay;
            // multiplier on enemy shield recharge timer with two coop players
            public float TwoPlayerShieldRechargeTimer;
            // multiplier on enemy shield recharge timer with four coop players
            public float FourPlayerShieldRechargeTimer;
            // multiplier on enemy shield recharge timer with six coop players or more
            public float SixPlayerShieldRechargeTimer;
            // multiplier on enemy grenade dive chance with two coop players
            public float TwoPlayerGrenadeDiveChance;
            // multiplier on enemy grenade dive chance with four coop players
            public float FourPlayerGrenadeDiveChance;
            // multiplier on enemy grenade dive chance with six coop players or more
            public float SixPlayerGrenadeDiveChance;
            // multiplier on enemy armor lock chance with two coop players
            public float TwoPlayerArmorLockChance;
            // multiplier on enemy armor lock chance with four coop players
            public float FourPlayerArmorLockChance;
            // multiplier on enemy armor lock chance with six coop players or more
            public float SixPlayerArmorLockChance;
            // multiplier on enemy evasion danger threshold with two coop players
            public float TwoPlayerEvasionDangerThreshold;
            // multiplier on enemy evasion danger threshold with four coop players
            public float FourPlayerEvasionDangerThreshold;
            // multiplier on enemy evasion danger threshold with six coop players or more
            public float SixPlayerEvasionDangerThreshold;
            // multiplier on enemy evasion delay timer with two coop players
            public float TwoPlayerEvasionDelayTimer;
            // multiplier on enemy evasion delay timer with four coop players
            public float FourPlayerEvasionDelayTimer;
            // multiplier on enemy evasion delay timer with six coop players or more
            public float SixPlayerEvasionDelayTimer;
            // multiplier on enemy evasion chance with two coop players
            public float TwoPlayerEvasionChance;
            // multiplier on enemy evasion chance with four coop players
            public float FourPlayerEvasionChance;
            // multiplier on enemy evasion chance with six coop players or more
            public float SixPlayerEvasionChance;
            // multiplier on the enemy shooting burst duration with two coop players
            public float TwoPlayerBurstDuration;
            // multiplier on the enemy shooting burst duration with four coop players
            public float FourPlayerBurstDuration;
            // multiplier on the enemy shooting burst duration with six coop players or more
            public float SixPlayerBurstDuration;
            // multipler on the enemy shooting burst separation with two coop players
            public float TwoPlayerBurstSeparation;
            // multipler on the enemy shooting burst separation with four coop players
            public float FourPlayerBurstSeparation;
            // multipler on the enemy shooting burst separation with six coop players or more
            public float SixPlayerBurstSeparation;
            // multipler on the enemy shooting damage multiplier with two coop players
            public float TwoPlayerDamageModifier;
            // multipler on the enemy shooting damage multiplier with four coop players
            public float FourPlayerDamageModifier;
            // multipler on the enemy shooting damage multiplier with six coop players or more
            public float SixPlayerDamageModifier;
            // multiplier on the speed of projectiles fired by enemies with two coop players
            public float TwoPlayerProjectileSpeed;
            // multiplier on the speed of projectiles fired by enemies with four coop players
            public float FourPlayerProjectileSpeed;
            // multiplier on the speed of projectiles fired by enemies with six coop players or more
            public float SixPlayerProjectileSpeed;
        }
        
        [TagStructure(Size = 0x1C)]
        public class SoftCeilingGlobalsBlock : TagStructure
        {
            public float BipedSpringConstant;
            public float BipedNormalDamping;
            public float BipedTangentDamping;
            public float BipedMinTangentDampVelocity;
            public float VehicleSpringConstant;
            public float VehicleNormalDamping;
            public float VehicleTangentDamping;
        }
        
        [TagStructure(Size = 0x130)]
        public class InterfaceTagReferences : TagStructure
        {
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag SpinnerBitmap;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Obsolete2;
            [TagField(ValidTags = new [] { "colo" })]
            public CachedTag ScreenColorTable;
            [TagField(ValidTags = new [] { "colo" })]
            public CachedTag HudColorTable;
            [TagField(ValidTags = new [] { "colo" })]
            public CachedTag EditorColorTable;
            [TagField(ValidTags = new [] { "colo" })]
            public CachedTag DialogColorTable;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag MotionSensorSweepBitmap;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag MotionSensorSweepBitmapMask;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag MultiplayerHudBitmap;
            public CachedTag Unused;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag MotionSensorBlipBitmap;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag InterfaceGooMap1;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag InterfaceGooMap2;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag InterfaceGooMap3;
            [TagField(ValidTags = new [] { "wgtz" })]
            public CachedTag MainmenuUiGlobals;
            [TagField(ValidTags = new [] { "wgtz" })]
            public CachedTag SingleplayerUiGlobals;
            [TagField(ValidTags = new [] { "wgtz" })]
            public CachedTag MultiplayerUiGlobals;
            [TagField(ValidTags = new [] { "wgtz" })]
            public CachedTag FirefightUiGlobals;
            [TagField(ValidTags = new [] { "uiss" })]
            public CachedTag StyleSheetGlobals;
        }
        
        [TagStructure(Size = 0x10)]
        public class CheatWeaponsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "item" })]
            public CachedTag Weapon;
        }
        
        [TagStructure(Size = 0x10)]
        public class CheatPowerupsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "eqip" })]
            public CachedTag Powerup;
        }
        
        [TagStructure(Size = 0xDC)]
        public class PlayerInformationBlock : TagStructure
        {
            public float WalkingSpeed; // world units per second
            public float RunForward; // world units per second
            public float RunBackward; // world units per second
            public float RunSideways; // world units per second
            public float RunAcceleration; // world units per second squared
            public float SneakForward; // world units per second
            public float SneakBackward; // world units per second
            public float SneakSideways; // world units per second
            public float SneakAcceleration; // world units per second squared
            public float AirborneAcceleration; // world units per second squared
            public float WeaponReadyAnimScaler;
            public RealPoint3d GrenadeOrigin;
            // determines the distance along the aiming vector to orient the grenade based on the camera pitch
            public ScalarFunctionNamedStruct GrenadeAiming;
            public Bounds<float> FirstPersonIdleTime; // seconds
            public float FirstPersonSkipFraction; // [0,1]
            [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
            public CachedTag CoopCountdownSound;
            [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
            public CachedTag CoopRespawnSound;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag CoopRespawnEffect;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag HologramDeathEffect;
            public int BinocularsZoomCount;
            public Bounds<float> BinocularsZoomRange;
            [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
            public CachedTag NightVisionOn;
            [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
            public CachedTag NightVisionOff;
            public float FireTeamObjectiveRange;
            public float FireTeamSandboxRange;
            public float FireTeamConeAngle; // in degrees
            public List<PlayerMomentumDataBlock> MomentumAndSprinting;
            
            [TagStructure(Size = 0x14)]
            public class ScalarFunctionNamedStruct : TagStructure
            {
                public MappingFunction Function;
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
            
            [TagStructure(Size = 0x38)]
            public class PlayerMomentumDataBlock : TagStructure
            {
                public MomentumFlag Flag;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                // how long you must be pegged before you gain momentum
                public float SecondsToStart;
                // how long you must have momentum before you reach top speed
                public float SecondsToFullSpeed;
                // how fast being unpegged decays the timer (seconds per second)
                public float DecayRate;
                // how much faster we actually go when at full momentum
                public float FullSpeedMultiplier;
                // how much faster to turn when sprinting
                public float SprintTurnMultiplier;
                // how far the stick needs to be pressed before being considered pegged
                public float PeggedMagnitude;
                // how far off straight up (in degrees) we consider pegged
                public float PeggedAngularThreshold;
                public Angle MaxLookYawVelocity; // degrees per second
                public Angle MaxLookPitchVelocity; // degrees per second
                public float MinimumPlayerVelocityToBeConsideredInAMomentumState; // world units per second
                // period of time over which we record the biped's look angle for deciding if we should drop him out of momentum
                public float LookWindowLength; // seconds
                public StringId MomentumAnimationStance;
                // [0, 1] while using this type of momentum, the player's weapon error cannot drop below this value
                public float MinWeaponError;
                
                [Flags]
                public enum MomentumFlag : byte
                {
                    DisableSoftPingCheck = 1 << 0,
                    DisableHardPingCheck = 1 << 1
                }
            }
        }
        
        [TagStructure(Size = 0x80)]
        public class PlayerRepresentationBlock : TagStructure
        {
            public PlayerRepresentationFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "cusc" })]
            public CachedTag HudScreenReference;
            [TagField(ValidTags = new [] { "hlmt" })]
            public CachedTag FirstPersonHandsModel;
            public StringId FirstPersonMultiplayerHandsVariant;
            [TagField(ValidTags = new [] { "hlmt" })]
            public CachedTag FirstPersonBodyModel;
            public StringId FirstPersonMultiplayerBodyVariant;
            public List<FirstpersonpHiddenBodyRegionsBlock> HiddenFpbodyRegions;
            [TagField(ValidTags = new [] { "unit" })]
            public CachedTag ThirdPersonUnit;
            public StringId ThirdPersonVariant;
            [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
            public CachedTag BinocularsZoomInSound;
            [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
            public CachedTag BinocularsZoomOutSounds;
            public int PlayerInformation;
            
            [Flags]
            public enum PlayerRepresentationFlags : byte
            {
                CanUseHealthPacks = 1 << 0
            }
            
            [TagStructure(Size = 0x8)]
            public class FirstpersonpHiddenBodyRegionsBlock : TagStructure
            {
                public StringId HiddenRegion;
                public FpBodyRegionFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum FpBodyRegionFlags : byte
                {
                    VisibleInIcs = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0xD4)]
        public class DamageGlobalsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag FallingDamage;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag JumpingDamage;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag SoftLandingDamage;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag HardLandingDamage;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag HsDamage;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag HsFireDamage;
            // you die if you fall faster than this (non-multiplayer only)
            public float TerminalVelocity; // wu/s
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag TerminalVelocityDamage;
            [TagField(ValidTags = new [] { "drdf" })]
            // fall back if none is specified in damage effect tags
            public CachedTag DefaultDamageResponse;
            [TagField(ValidTags = new [] { "drdf" })]
            // anytime your teammate shoots you
            public CachedTag FriendlyFireDamageResponse;
            // the minimum amount of shield vitality needed to prevent spillover from damage types that do not spillover.
            public float PlayerShieldSpillover;
            public DamageDecayStruct DamageDecayProps;
            public DamageDecayStruct AiDamageDecayProps;
            public DamageDecayStruct ShieldImpactDecayProps;
            
            [TagStructure(Size = 0x14)]
            public class DamageDecayStruct : TagStructure
            {
                // current damage begins to fall after a time delay has passed since last the damage (MAX 4.1, because timer is stored
                // in a char so 127 ticks maximum)
                public float CurrentDamageDecayDelay; // seconds
                // amount of time it would take for 100% current damage to decay to 0
                public float CurrentDamageDecayTime; // seconds
                // amount of damage that decays from our current damage every second
                public float CurrentDamageDecayRate; // damage/second
                // recent damage begins to fall after a time delay has passed since last the damage (MAX 4.1, because timer is stored
                // in a char so 127 ticks maximum)
                public float RecentDamageDecayDelay; // seconds
                // amount of time it would take for 100% recent damage to decay to 0
                public float RecentDamageDecayTime; // seconds
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class ShieldBoostBlock : TagStructure
        {
            // amount of shield-boost to decay per second
            public float ShieldBoostDecay;
            // time to recharge full shields when getting boosted
            public float ShieldBoostRechargeTime;
            // stun time when getting boosted
            public float ShieldBoostStunTime;
        }
        
        [TagStructure(Size = 0x1A8)]
        public class MaterialsBlock : TagStructure
        {
            public StringId Name;
            public StringId ParentName;
            public short RuntimeMaterialIndex;
            public GlobalMaterialFlags Flags;
            public StringId GeneralArmor;
            public StringId SpecificArmor;
            public WetProxiesStruct WetProxies;
            public short RuntimeDryBaseMaterialIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public MaterialPhysicsPropertiesStruct PhysicsProperties;
            [TagField(ValidTags = new [] { "bsdt" })]
            public CachedTag BreakableSurface;
            public MaterialsSweetenersStruct Sweeteners;
            [TagField(ValidTags = new [] { "foot" })]
            public CachedTag MaterialEffects;
            public List<UnderwaterProxiesBlock> UnderwaterProxies;
            
            [Flags]
            public enum GlobalMaterialFlags : ushort
            {
                Flammable = 1 << 0,
                Biomass = 1 << 1,
                RadXferInterior = 1 << 2,
                UsedDirectly = 1 << 3,
                UsedByChildren = 1 << 4
            }
            
            [TagStructure(Size = 0x8)]
            public class WetProxiesStruct : TagStructure
            {
                public StringId WetMaterial;
                public short RuntimeProxyMaterialIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x48)]
            public class MaterialPhysicsPropertiesStruct : TagStructure
            {
                public int Flags;
                public float Friction;
                public float Restitution;
                public float Density; // kg/m^3
                [TagField(ValidTags = new [] { "wpdp" })]
                public CachedTag WaterPhysicsDragProperties;
                public List<ObjectTypeDragPropertiesBlock> DragOverrides;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float SuperFloater;
                public float Floater;
                public float Neutral;
                public float Sinker;
                public float SuperSinker;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                
                [TagStructure(Size = 0x10)]
                public class ObjectTypeDragPropertiesBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "wpdp" })]
                    public CachedTag DragProperties;
                }
            }
            
            [TagStructure(Size = 0x114)]
            public class MaterialsSweetenersStruct : TagStructure
            {
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag SoundSweetener;
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag SoundSweetener1;
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag SoundSweetener2;
                [TagField(ValidTags = new [] { "lsnd" })]
                public CachedTag SoundSweetenerRolling;
                [TagField(ValidTags = new [] { "lsnd" })]
                public CachedTag SoundSweetenerGrinding;
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag SoundSweetener3;
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                // this is a medium sweetener and was the old default
                public CachedTag SoundSweetener4;
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag SoundSweetener5;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag EffectSweetener;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag EffectSweetener1;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag EffectSweetener2;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag EffectSweetenerRolling;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag EffectSweetenerGrinding;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag EffectSweetener3;
                [TagField(ValidTags = new [] { "rwrd" })]
                public CachedTag WaterRipple;
                [TagField(ValidTags = new [] { "rwrd" })]
                public CachedTag WaterRipple1;
                [TagField(ValidTags = new [] { "rwrd" })]
                public CachedTag WaterRipple2;
                public MaterialsSweetenersInheritanceFlags SweetenerInheritanceFlags;
                
                [Flags]
                public enum MaterialsSweetenersInheritanceFlags : uint
                {
                    SoundSmall = 1 << 0,
                    SoundMedium = 1 << 1,
                    SoundLarge = 1 << 2,
                    SoundRolling = 1 << 3,
                    SoundGrinding = 1 << 4,
                    SoundMeleeSmall = 1 << 5,
                    SoundMelee = 1 << 6,
                    SoundMeleeLarge = 1 << 7,
                    EffectSmall = 1 << 8,
                    EffectMedium = 1 << 9,
                    EffectLarge = 1 << 10,
                    EffectRolling = 1 << 11,
                    EffectGrinding = 1 << 12,
                    EffectMelee = 1 << 13,
                    WaterRippleSmall = 1 << 14,
                    WaterRippleMedium = 1 << 15,
                    WaterRippleLarge = 1 << 16
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class UnderwaterProxiesBlock : TagStructure
            {
                public StringId UnderwaterMaterial;
                public StringId ProxyMaterial;
                public short UnderwaterMaterialType;
                public short ProxyMaterialType;
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class MultiplayerColorBlock : TagStructure
        {
            public RealRgbColor Color;
        }
        
        [TagStructure(Size = 0x1C)]
        public class VisorColorBlock : TagStructure
        {
            public StringId Name;
            public RealRgbColor TertiaryColor;
            public RealRgbColor QuaternaryColor;
        }
        
        [TagStructure(Size = 0x18)]
        public class EliteSpecularColorStruct : TagStructure
        {
            public RealRgbColor TertiaryColor;
            public RealRgbColor QuaternaryColor;
        }
        
        [TagStructure(Size = 0x4C)]
        public class CinematicsGlobalsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "scen" })]
            public CachedTag CinematicAnchorReference;
            public float CinematicFilmAperture;
            public float CinematicSkipUiUpTime;
            // percentage towards the center - 0=default, 0.5=center of the screen
            public Bounds<float> SubtitleRectWidth;
            // 0=default, 0.5=center of the screen
            public Bounds<float> SubtitleRectHeight;
            public RealRgbColor DefaultSubtitleColor;
            public RealRgbColor DefaultSubtitleShadowColor;
            public List<CinematicCharactersBlock> CinematicCharacters;
            
            [TagStructure(Size = 0x1C)]
            public class CinematicCharactersBlock : TagStructure
            {
                public StringId CharacterName;
                public RealRgbColor SubtitleColor;
                public RealRgbColor ShadowColor;
            }
        }
        
        [TagStructure(Size = 0x38)]
        public class CampaignMetagameGlobalsBlock : TagStructure
        {
            public List<CampaignMetagameStyleTypeBlock> Styles;
            public List<CampaignMetagameDifficultyScaleBlock> Difficulty;
            public List<CampaignMetagameSkullBlock> Skulls;
            public int FriendlyDeathPointCount;
            public int PlayerDeathPointCount;
            public int PlayerBetrayalPointCount;
            // how long does transient score stay onscreen
            public float TransientScoreTime; // s
            // time after taking a guys shields down with emp damage you have to get the emp kill bonus (seconds)
            public float EmpKillWindow;
            
            [TagStructure(Size = 0x8)]
            public class CampaignMetagameStyleTypeBlock : TagStructure
            {
                public StringId IncidentName;
                public float StyleMultiplier;
            }
            
            [TagStructure(Size = 0x4)]
            public class CampaignMetagameDifficultyScaleBlock : TagStructure
            {
                public float DifficultyMultiplier;
            }
            
            [TagStructure(Size = 0x4)]
            public class CampaignMetagameSkullBlock : TagStructure
            {
                public float DifficultyMultiplier;
            }
        }
        
        [TagStructure(Size = 0x44)]
        public class LanguagePackDefinition : TagStructure
        {
            public int StringReferencePointer;
            public int StringDataPointer;
            public int NumberOfStrings;
            public int StringDataSize;
            public int StringReferenceCacheOffset;
            public int StringDataCacheOffset;
            [TagField(Length = 20)]
            public DataHashDefinition[]  StringReferenceChecksum;
            [TagField(Length = 20)]
            public DataHashDefinition[]  StringDataChecksum;
            public int DataLoadedBoolean;
            
            [TagStructure(Size = 0x1)]
            public class DataHashDefinition : TagStructure
            {
                public byte HashByte;
            }
        }
        
        [TagStructure(Size = 0xA0)]
        public class ActiveCamoGlobalsBlock : TagStructure
        {
            // for bipeds, the speed at which you are on the far right of the 'speed to max camo' graph
            public float BipedSpeedReference; // wu/s
            // for vehicles, the speed at which you are on the far right of the 'speed to max camo' graph
            public float VehicleSpeedReference; // wu/s
            // minimum active camo percentage at which a player's game name will start becoming visible
            public float CamoValueForGameName;
            public ScalarFunctionNamedStruct CamoValueToDistortion;
            // maps active-camo percentage to alpha for THIRD PERSON rendering
            public ScalarFunctionNamedStruct CamoValueToTransparency;
            // maps active-camo percentage to alpha for FIRST PERSON rendering
            public ScalarFunctionNamedStruct CamoValueToFpTransparency;
            public ScalarFunctionNamedStruct CamoDistortionTextureStrength;
            public RealVector2d CamoDistortionScale;
            public RealVector2d CamoDistortionTranslateSpeed;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag CamoDistortionTexture;
            // This is used to deal with ugly rendering artifacts when camo is not fully on
            public float CamoDepthBiasMaxDistance;
            // This is used to deal with ugly rendering artifacts when camo is not fully on
            public ScalarFunctionNamedStruct CamoDepthBiasFunction;
            public List<ActiveCamoLevelDefinitionBlock> CamoLevels;
            
            [TagStructure(Size = 0x14)]
            public class ScalarFunctionNamedStruct : TagStructure
            {
                public MappingFunction Function;
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
            
            [TagStructure(Size = 0x24)]
            public class ActiveCamoLevelDefinitionBlock : TagStructure
            {
                // reduces camo value by this much when throwing a grenade
                public float GrenadeThrowPenalty; // 0..1
                // reduces camo by this much when meleeing
                public float MeleePenalty; // 0..1
                // when taking damage or doing other actions that reduce camo, we will never drop below this value
                public float MinimumDingedValue;
                // time it takes to interpolate from 0.0 to 1.0
                public float InterpolationTime; // s
                public ScalarFunctionNamedStruct SpeedToMaximumCamo;
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class GarbageCollectionBlock : TagStructure
        {
            public float DroppedItem; // seconds
            public float DroppedItemByPlayer; // seconds
            public float DroppedItemInMultiplayer; // seconds
            public float BrokenConstraints; // seconds
            public float DeadUnit; // seconds
            public float DeadPlayer; // seconds
            public float DeadMpPlayer; // seconds
            public float DeadMpPlayerOverloaded; // seconds
            // above this number, overloaded mp time is used to garbage collect dead bodies
            public int MaxDeadBodyCount;
        }
        
        [TagStructure(Size = 0x14)]
        public class GlobalCameraImpulseBlock : TagStructure
        {
            public MappingFunction Mapping;
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
        
        [TagStructure(Size = 0x1A8)]
        public class RuntimeMaterialsBlock : TagStructure
        {
            public StringId Name;
            public StringId ParentName;
            public short RuntimeMaterialIndex;
            public GlobalMaterialFlags Flags;
            public StringId GeneralArmor;
            public StringId SpecificArmor;
            public WetProxiesStruct WetProxies;
            public short RuntimeDryBaseMaterialIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public MaterialPhysicsPropertiesStruct PhysicsProperties;
            [TagField(ValidTags = new [] { "bsdt" })]
            public CachedTag BreakableSurface;
            public MaterialsSweetenersStruct Sweeteners;
            [TagField(ValidTags = new [] { "foot" })]
            public CachedTag MaterialEffects;
            public List<UnderwaterProxiesBlock> UnderwaterProxies;
            
            [Flags]
            public enum GlobalMaterialFlags : ushort
            {
                Flammable = 1 << 0,
                Biomass = 1 << 1,
                RadXferInterior = 1 << 2,
                UsedDirectly = 1 << 3,
                UsedByChildren = 1 << 4
            }
            
            [TagStructure(Size = 0x8)]
            public class WetProxiesStruct : TagStructure
            {
                public StringId WetMaterial;
                public short RuntimeProxyMaterialIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x48)]
            public class MaterialPhysicsPropertiesStruct : TagStructure
            {
                public int Flags;
                public float Friction;
                public float Restitution;
                public float Density; // kg/m^3
                [TagField(ValidTags = new [] { "wpdp" })]
                public CachedTag WaterPhysicsDragProperties;
                public List<ObjectTypeDragPropertiesBlock> DragOverrides;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float SuperFloater;
                public float Floater;
                public float Neutral;
                public float Sinker;
                public float SuperSinker;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                
                [TagStructure(Size = 0x10)]
                public class ObjectTypeDragPropertiesBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "wpdp" })]
                    public CachedTag DragProperties;
                }
            }
            
            [TagStructure(Size = 0x114)]
            public class MaterialsSweetenersStruct : TagStructure
            {
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag SoundSweetener;
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag SoundSweetener1;
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag SoundSweetener2;
                [TagField(ValidTags = new [] { "lsnd" })]
                public CachedTag SoundSweetenerRolling;
                [TagField(ValidTags = new [] { "lsnd" })]
                public CachedTag SoundSweetenerGrinding;
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag SoundSweetener3;
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                // this is a medium sweetener and was the old default
                public CachedTag SoundSweetener4;
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag SoundSweetener5;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag EffectSweetener;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag EffectSweetener1;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag EffectSweetener2;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag EffectSweetenerRolling;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag EffectSweetenerGrinding;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag EffectSweetener3;
                [TagField(ValidTags = new [] { "rwrd" })]
                public CachedTag WaterRipple;
                [TagField(ValidTags = new [] { "rwrd" })]
                public CachedTag WaterRipple1;
                [TagField(ValidTags = new [] { "rwrd" })]
                public CachedTag WaterRipple2;
                public MaterialsSweetenersInheritanceFlags SweetenerInheritanceFlags;
                
                [Flags]
                public enum MaterialsSweetenersInheritanceFlags : uint
                {
                    SoundSmall = 1 << 0,
                    SoundMedium = 1 << 1,
                    SoundLarge = 1 << 2,
                    SoundRolling = 1 << 3,
                    SoundGrinding = 1 << 4,
                    SoundMeleeSmall = 1 << 5,
                    SoundMelee = 1 << 6,
                    SoundMeleeLarge = 1 << 7,
                    EffectSmall = 1 << 8,
                    EffectMedium = 1 << 9,
                    EffectLarge = 1 << 10,
                    EffectRolling = 1 << 11,
                    EffectGrinding = 1 << 12,
                    EffectMelee = 1 << 13,
                    WaterRippleSmall = 1 << 14,
                    WaterRippleMedium = 1 << 15,
                    WaterRippleLarge = 1 << 16
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class UnderwaterProxiesBlock : TagStructure
            {
                public StringId UnderwaterMaterial;
                public StringId ProxyMaterial;
                public short UnderwaterMaterialType;
                public short ProxyMaterialType;
            }
        }
        
        [TagStructure(Size = 0x34)]
        public class HologramlightingGlobalsBlock : TagStructure
        {
            public StringId Intensity;
            public HologramlightFunctions KeyLightFunctions;
            public HologramlightFunctions FillLightFunctions;
            public HologramlightFunctions RimLightFunctions;
            
            [TagStructure(Size = 0x10)]
            public class HologramlightFunctions : TagStructure
            {
                public StringId Intensity;
                public StringId Forward;
                public StringId Right;
                public StringId Up;
            }
        }
    }
}
