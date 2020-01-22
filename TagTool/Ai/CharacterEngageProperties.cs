using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x28, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0x38, MinVersion = CacheVersion.Halo3ODST)]
    public class CharacterEngageProperties : TagStructure
	{
        public CharacterEngageFlags Flags;
        public uint Unknown;
        public float CrouchDangerThreshold;
        public float StandDangerThreshold;
        public float FightDangerMoveThreshold;
        public float FightDangerMoveThresholdCooldown;
        public CachedTag OverrideGrenadeProjectile;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown4;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown5;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown6;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown7;
    }
}
