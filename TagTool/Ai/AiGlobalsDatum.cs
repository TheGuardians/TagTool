using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using System;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x1B0, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0x144, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x1B0, MinVersion = CacheVersion.HaloReach)]
    public class AiGlobalsDatum : TagStructure
    {
        public float AiInfantryOnAiWeaponDamageScale; // [0,1] Global scale on weapon damage made by AI on other AI
        public float AiVehicleOnAiWeaponDamageScale; // [0,1] Global scale on weapon damage made by AI in a vehicle on other AI
        public float AiInPlayerVehicleOnAiWeaponDamageScale; // [0,1] Global scale on weapon damage made by AI in a vehicle with the player on other AI
        public float DangerBroadlyFacing;

        [TagField(MaxVersion = CacheVersion.Halo3Retail, Length = 4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;

        public float DangerShootingNear;

        [TagField(MaxVersion = CacheVersion.Halo3Retail, Length = 4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;

        public float DangerShootingAt;

        [TagField(MaxVersion = CacheVersion.Halo3Retail, Length = 4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;

        public float DangerExtremelyClose;

        [TagField(MaxVersion = CacheVersion.Halo3Retail, Length = 4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;

        public float DangerShieldDamage;
        public float DangerExtendedShieldDamage;
        public float DangerBodyDamage;
        public float DangerExtendedBodyDamage;

        [TagField(MaxVersion = CacheVersion.Halo3Retail, Length = 48, Flags = TagFieldFlags.Padding)]
        public byte[] Padding4;

        public CachedTag GlobalDialogue;
        public StringId DefaultMissionDialogueSoundEffect;

        [TagField(MaxVersion = CacheVersion.Halo3Retail, Length = 20, Flags = TagFieldFlags.Padding)]
        public byte[] Padding5;

        public float JumpDown; // wu/tick
        public float JumpStep; // wu/tick
        public float JumpCrouch; // wu/tick
        public float JumpStand; // wu/tick
        public float JumpStorey; // wu/tick
        public float JumpTower; // wu/tick

        [TagField(MinVersion = CacheVersion.HaloReach, Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] ReachPadding;

        public float MaxJumpDownHeightDown; // wu
        public float MaxJumpDownHeightStep; // wu
        public float MaxJumpDownHeightCrouch; // wu
        public float MaxJumpDownHeightStand; // wu
        public float MaxJumpDownHeightStorey; // wu
        public float MaxJumpDownHeightTower; // wu

        [TagField(MinVersion = CacheVersion.HaloReach, Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] ReachPadding1;

        public Bounds<float> HoistStep;
        public Bounds<float> HoistCrouch;
        public Bounds<float> HoistStand;

        [TagField(MaxVersion = CacheVersion.Halo3Retail, Length = 24, Flags = TagFieldFlags.Padding)]
        public byte[] Padding6;

        public Bounds<float> VaultStep; // wus
        public Bounds<float> VaultCrouch; // wus

        // Pathfinding Search Ranges
        // The maximum ranges to whcih firing point evaluations will do pathfinding searches.
        // Increasing these will almost certainly have a negative impact on performance

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public PathfindingSearchRangeStruct PathfindingSearchRanges;

        [TagField(Flags = TagFieldFlags.Padding, Length = 48, MaxVersion = CacheVersion.Halo3Retail)]
        public byte[] Padding7;

        public List<GravemindPropertyBlock> GravemindProperties;

        [TagField(Flags = TagFieldFlags.Padding, Length = 48, MaxVersion = CacheVersion.Halo3Retail)]
        public byte[] Padding8;

        public float ScaryTargetThreshold; // A target of this scariness is offically considered scary (by combat dialogue, etc.)
        public float ScaryWeaponThreshold; // A weapon of this scariness is offically considered scary (by combat dialogue, etc.)
        public float PlayerScariness;
        public float BerserkingActorScariness;
        public float KamikazeingActorScariness;
        public float InvincibleActorScariness; // when an actor's target is invincible, he is this much more scared
        public float MinimumDeathTime;
        public float ProjectileDistance;
        public float IdleClumpDistance;
        public float DangerousClumpDistance;
        public float ConverSearchDuration;
        public float TaskSearchDuration;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown7;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<TagReferenceBlock> Styles;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<TagReferenceBlock> Formations;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<SquadTemplate> SquadTemplates;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<PerformanceTemplate> PerformanceTemplates;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<AiGlobalsCustomStimuli> CustomStimuli;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<AiCueTemplateBlock> CueTemplates;

        // Clump Throttling: helps you control how much guys will throttle when they want to stick with their squad
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public ClumpThrottlingStruct ClumpThrottling;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public SquadPatrollingStruct SquadPatrolling;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float KungFuDeactivationDelay;   // control how the kungfu circle works

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short SuppressingFireCount;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short UncoverCount;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short LeapOnCoverCount;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short DestroyCoverCount;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short GuardCount;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short InvestigateCount;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<VisionTrait> VisionTraits;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<HearingTrait> HearingTraits;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<LuckTrait> LuckTraits;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<GrenadeTrait> GrenadeTraits;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float MaxDecayTime;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float DecayTimePing;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float SearchPatternRadius;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short SearchPatternShellCount;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public Bounds<short> SearchPatternCellsPerShellRange;

        [TagField(MinVersion = CacheVersion.HaloReach, Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] ReachPadding2;

        [TagStructure(Size = 0xC)]
        public class GravemindPropertyBlock : TagStructure
        {
            public float MinimumRetreatTime;
            public float IdealRetreatTime;
            public float MaximumRetreatTime;
        }

        [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x1C, MinVersion = CacheVersion.HaloReach)]
        public class SquadTemplate : TagStructure
        {
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public CachedTag Template;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId Name;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<AiGlobalsSquadTemplateSubFolderBlock> SubFolders;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<TagReferenceBlock> Templates;
        }

        [TagStructure(Size = 0x1C, MinVersion = CacheVersion.HaloReach)]
        public class PerformanceTemplate : TagStructure
        {
            public StringId Name;

            public List<Character> Characters;

            public List<AiGlobalsPerformanceTemplateBlock> Templates;

            [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloReach)]
            public class Character : TagStructure
            {
                public StringId Name;
                public List<TagReference> Templates;
            }

            [TagStructure(Size = 0x10)]
            public class AiGlobalsPerformanceTemplateBlock : TagStructure
            {
                [TagField(ValidTags = new[] { "pfmc" })]
                public CachedTag ThespianTemplate;
            }
        }

        [TagStructure(Size = 0x10)]
        public class AiGlobalsSquadTemplateSubFolderBlock : TagStructure
        {
            public StringId SubFolderName;
            public List<AiGlobalsSquadTemplateBlock> Templates;

            [TagStructure(Size = 0x10)]
            public class AiGlobalsSquadTemplateBlock : TagStructure
            {
                [TagField(ValidTags = new[] { "sqtm" })]
                public CachedTag SquadTemplate;
            }
        }

        [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloReach)]
        public class AiCueTemplateBlock : TagStructure
        {
            public StringId Name;

            public List<FiringPointPayloadBlock> FiringPoints;
            public List<StimulusPayloadBlock> Stimulus;
            public List<CombatCuePayloadBlock> CombatCue;

            [TagStructure(Size = 0x4)]
            public class FiringPointPayloadBlock : TagStructure
            {
                public float StimulusType;
            }

            [TagStructure(Size = 0x4)]
            public class StimulusPayloadBlock : TagStructure
            {
                public StringId StimulusType;
            }

            [TagStructure(Size = 0x2C)]
            public class CombatCuePayloadBlock : TagStructure
            {
                public RealPoint3d Position;
                public short ReferenceFrame;
                public short StructureBsp;
                public GFiringPositionFlags Flags;
                public GFiringPositionPostureFlags PostureFlags;
                public short Area;
                public short ClusterIndex;
                public short BspIndex;
                public short SectorIndex;
                public RealEulerAngles2d Normal;
                public Angle Facing;
                public CombatCuePreferenceEnum Preference;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;

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
                    AutomaticallyGenerated = 1 << 8
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

        [TagStructure(Size = 0x4)]
        public class AiGlobalsCustomStimuli : TagStructure
        {
            public StringId Name;
        }

        [TagStructure(Size = 0x8, MinVersion = CacheVersion.HaloReach)]
        public class VisionTrait : TagStructure
        {
            public float VisionDistanceScale; // Scale the distance at which an AI can see their target.
            public float VisionAngleScale; // Scale the angles of the AI's vision cone.
        }

        [TagStructure(Size = 0x4, MinVersion = CacheVersion.HaloReach)]
        public class HearingTrait : TagStructure
        {
            public float HearingDistanceScale; // Scale the character's hearing distance.
        }

        [TagStructure(Size = 0x2C, MinVersion = CacheVersion.HaloReach)]
        public class LuckTrait : TagStructure
        {
            public float EvasionChanceScale; // Scale the chance of evading fire.
            public float GrenadeDiveChanceScale; // Scale the chance of diving from grenades.
            public float BrokenKamikazeChanceScale; // Scale the chance of going kamikaze when broken.
            public float LeaderDeadRetreatChanceScale; // Scale the chance of retreating when your leader dies.
            public float DiveRetreatChanceScale; // Scale the chance of retreating after a dive.
            public float ShieldDepletedBerserkChanceScale; // Scale the chance of berserking when your shield is depleted.
            public float LeaderAbandonedBerserkChanceScale; // Scale the chance of a leader berserking when all his followers die.
            public float MeleeAttackDelayTimerScale; // Scale the time between melee attacks.
            public float MeleeChanceScale; // Scale the chance of meleeing.
            public float MeleeLeapDelayTimerScale; // Scale the delay for performing melee leaps.
            public float ThrowGrenadeDelayScale; // Scale the time between grenade throws.
        }

        [TagStructure(Size = 0x1C, MinVersion = CacheVersion.HaloReach)]
        public class GrenadeTrait : TagStructure
        {
            public float VelocityScale; // Scale the velocity at which AI throws grenades
            public float ThrowGrenadeDelayScale; // Scale the time between grenade throws.
            public float DontDropGrenadesChanceScale;
            public float GrenadeUncoverChanceScale;
            public float RetreatThrowGrenadeChanceScale;
            public float AntiVehicleGrenadeChanceScale;
            public float ThrowGrenadeChanceScale;
        }

        [TagStructure(Size = 0x10)]
        public class PathfindingSearchRangeStruct : TagStructure
        {
            public float Infantry; // 30
            public float Flying; // 40
            public float Vehicle; // 40
            public float Giant; // 200
        }

        [TagStructure(Size = 0x14)]
        public class ClumpThrottlingStruct : TagStructure
        {
            public float StopDistance;
            public float ResumeDistance;
            public Bounds<float> DistanceBounds; // world units
            public float MinimumScale;   // 0-1
        }

        [TagStructure(Size = 0x20)]
        public class SquadPatrollingStruct : TagStructure
        {

            public float PassthroughChance;
            public float SearchPhaseSkipChance;
            public float PatrolTransitionTimeout;
            public float PatrolManeuverTImeout;
            public Bounds<float> PatrolSearchFiringPointTime;
            public float PatrolIsolationDistance;
            public float PatrolIsolationTime;
        }
    }
}
