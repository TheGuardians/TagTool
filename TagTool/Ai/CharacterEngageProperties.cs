using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x28, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0x38, MinVersion = CacheVersion.Halo3ODST)]
    public class CharacterEngageProperties : TagStructure
	{
        public CharacterEngageFlags Flags;
        public Bounds<float> RepositionBounds; // How long should I remain at a firing position before moving? (0 values will use the default values of 6 and 7 seconds)
        public float CrouchDangerThreshold; // When danger rises above the threshold, the actor crouches
        public float StandDangerThreshold; // When danger drops below this threshold, the actor can stand again.
        public float FightDangerMoveThreshold; // When danger goes above given level, this actor switches firing positions
        public CachedTag OverrideGrenadeProjectile; // when I throw a grenade, forget what type I officially have

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
