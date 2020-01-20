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
        public float InfantryOnAiWeaponDamageScale;
        public float VehicleOnAiWeaponDamageScale;
        public float PlayerOnAiWeaponDamageScale;
        public float DangerBroadlyFacing;

        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public float Unknown1;

        public float DangerShootingNear;

        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public float Unknown2;

        public float DangerShootingAt;

        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public float Unknown3;

        public float DangerExtremelyClose;

        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public float Unknown4;

        public float DangerShieldDamage;
        public float DangerExtendedShieldDamage;
        public float DangerBodyDamage;
        public float DangerExtendedBodyDamage;

        [TagField(Flags = Padding, Length = 48, MaxVersion = CacheVersion.Halo3Retail)]
        public byte[] Unused1;

        public CachedTag GlobalDialogue;
        public StringId DefaultMissionDialogueSoundEffect;

        [TagField(Flags = Padding, Length = 20, MaxVersion = CacheVersion.Halo3Retail)]
        public byte[] Unused2;

        public float JumpDown;
        public float JumpStep;
        public float JumpCrouch;
        public float JumpStand;
        public float JumpStorey;
        public float JumpTower;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown5;

        public float MaxJumpDownHeightDown;
        public float MaxJumpDownHeightStep;
        public float MaxJumpDownHeightCrouch;
        public float MaxJumpDownHeightStand;
        public float MaxJumpDownHeightStorey;
        public float MaxJumpDownHeightTower;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown6;

        public Bounds<float> HoistStep;
        public Bounds<float> HoistCrouch;
        public Bounds<float> HoistStand;

        [TagField(Flags = Padding, Length = 24, MaxVersion = CacheVersion.Halo3Retail)]
        public byte[] Unused3;

        public Bounds<float> VaultStep;
        public Bounds<float> VaultCrouch;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float SearchRangeInfantry; // 30

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float SearchRangeFlying; // 40

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float SearchRangeVehicle; // 40

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float SearchRangeGiant; // 200

        [TagField(Flags = Padding, Length = 48, MaxVersion = CacheVersion.Halo3Retail)]
        public byte[] Unused4;

        public List<GravemindPropertyBlock> GravemindProperties;

        [TagField(Flags = Padding, Length = 48, MaxVersion = CacheVersion.Halo3Retail)]
        public byte[] Unused5;

        public float ScaryTargetThreshold;
        public float ScaryWeaponThreshold;
        public float PlayerScariness;
        public float BerserkingActorScariness;
        public float KamikazeingActorScariness;
        public float InvincibleActorScariness;
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

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown11;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown12;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown13;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown14;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown15;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown16;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown17;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown18;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown19;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown20;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown21;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown22;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown23;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown24;

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
    }
}
