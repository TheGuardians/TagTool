using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "collision_damage", Tag = "cddf", Size = 0x28, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "collision_damage", Tag = "cddf", Size = 0x30, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "collision_damage", Tag = "cddf", Size = 0x50, MinVersion = CacheVersion.HaloReach)]
    public class CollisionDamage : TagStructure
	{
        /* Applying collision damage */
        public float ApplyCollisionDamageScale; // 0 means 1.  1 is standard scale.  Some things may want to apply more damage
        public float FriendlyApplyCollisionDamage; // 0 means 1.  1 is standard scale.  Some things may want to apply more damage, yet go soft on their friends

        /* Game collision damage parameters */
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float MinimumVelocityForGameDamage; // if you are going below this velocity we stop all game damage
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagFunction GameCollisionDamage;

        public Bounds<float> GameAccelerationBounds;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public Bounds<float> GameDamageScaleBounds;

        /* Applying absolute collision damage */
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public float ApplyAbsoluteCollisionDamageScale; // 0 means 1.  1 is standard scale.  Some things may want to apply more damage
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public float FriendlyApplyAbsoluteCollisionDamageScale; // 0 means 1.  1 is standard scale.  Some things may want to apply more damage, yet go soft on their friends

        /* Absolute collision damage parameters */
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float MinimumVelocityForAbsoluteDamage; // if you are going below this velocity we stop all absolute damage
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagFunction AbsoluteCollisionDamage;

        public Bounds<float> AbsoluteDamageAccelerationBounds;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public Bounds<float> AbsoluteDamageScaleBounds;
    }
}
