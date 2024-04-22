using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "ai_globals", Tag = "aigl", Size = 0xC)]
    public class AiGlobals : TagStructure
    {
        public List<AiGlobalsDataBlockStruct> Data;
        
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
    }
}
