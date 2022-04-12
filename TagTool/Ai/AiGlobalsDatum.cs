using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

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

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown5;

        public float MaxJumpDownHeightDown; // wu
        public float MaxJumpDownHeightStep; // wu
        public float MaxJumpDownHeightCrouch; // wu
        public float MaxJumpDownHeightStand; // wu
        public float MaxJumpDownHeightStorey; // wu
        public float MaxJumpDownHeightTower; // wu

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown6;

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

        // probably a block or padding
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public int Unknown8;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public int Unknown9;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public int Unknown10;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<UnknownBlock1> UnknownBlock;

        // Clump Throttling: helps you control how much guys will throttle when they want to stick with their squad
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public ClumpThrottlingStruct ClumpThrottling;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public SquadPatrollingStruct SquadPatrolling;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float KungFuDeactivationDelay;   // control how the kungfu circle works

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short Unknown25;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short Unknown26;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short Unknown27;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short Unknown28;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short Unknown29;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short Unknown30;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<VisionTrait> VisionTraits;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<HearingTrait> HearingTraits;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<LuckTrait> LuckTraits;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<GrenadeTrait> GrenadeTraits;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown31;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown32;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown33;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short Unknown34;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short Unknown35;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short Unknown36;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short Unknown37;

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
            public uint Unknown1;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public uint Unknown2;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public uint Unknown3;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<TagReferenceBlock> Templates;
        }

        [TagStructure(Size = 0x1C, MinVersion = CacheVersion.HaloReach)]
        public class PerformanceTemplate : TagStructure
        {
            public StringId Name;

            public List<Character> Characters;

            public uint Unknown1;
            public uint Unknown2;
            public uint Unknown3;

            [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloReach)]
            public class Character : TagStructure
            {
                public StringId Name;
                public List<TagReference> Templates;
            }
        }

        [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloReach)]
        public class UnknownBlock1 : TagStructure
        {
            public StringId Name;

            public List<UnknownBlock2> Unknown1;
            public List<StringIdBlock> Unknown2;
            public List<UnknownBlock4> Unknown3;

            [TagStructure(Size = 0x4)]
            public class UnknownBlock2 : TagStructure
            {
                public float Unknown;
            }

            [TagStructure(Size = 0x4)]
            public class StringIdBlock : TagStructure
            {
                public StringId Value;
            }

            [TagStructure(Size = 0x2C)]
            public class UnknownBlock4 : TagStructure
            {
                public uint Unknown1;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
                public uint Unknown6;
                public uint Unknown7;
                public uint Unknown8;
                public uint Unknown9;
                public uint Unknown10;
                public uint Unknown11;
            }
        }

        [TagStructure(Size = 0x8, MinVersion = CacheVersion.HaloReach)]
        public class VisionTrait : TagStructure
        {
            public float Unknown1;
            public float Unknown2;
        }

        [TagStructure(Size = 0x4, MinVersion = CacheVersion.HaloReach)]
        public class HearingTrait : TagStructure
        {
            public float Unknown1;
        }

        [TagStructure(Size = 0x2C, MinVersion = CacheVersion.HaloReach)]
        public class LuckTrait : TagStructure
        {
            public float Unknown1;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;
            public float Unknown8;
            public float Unknown9;
            public float Unknown10;
            public float Unknown11;
        }

        [TagStructure(Size = 0x1C, MinVersion = CacheVersion.HaloReach)]
        public class GrenadeTrait : TagStructure
        {
            public float Unknown1;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;
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
