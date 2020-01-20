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
        public float Unknown18;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<TagReferenceBlock> Styles;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<TagReferenceBlock> Formations;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<TagReferenceBlock> SquadTemplates;

        // TODO
        [TagField(Length = 0xC, MinVersion = CacheVersion.HaloReach)]
        public byte[] Formations_Reach;

        // TODO
        [TagField(Length = 0xC, MinVersion = CacheVersion.HaloReach)]
        public byte[] SquadTemplates_Reach;

        // TODO: map out all the blocks for reach here

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown19;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown20;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown21;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown22;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown23;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown24;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown25;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown26;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown27;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown28;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown29;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown30;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown31;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown32;

        // TODO
        [TagField(Length = 0x74, MinVersion = CacheVersion.HaloReach)]
        public byte[] ReachValues;

        [TagStructure(Size = 0xC)]
        public class GravemindPropertyBlock : TagStructure
		{
            public float MinimumRetreatTime;
            public float IdealRetreatTime;
            public float MaximumRetreatTime;
        }
    }
}
