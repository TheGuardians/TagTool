using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "collision_damage", Tag = "cddf", Size = 0x28, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "collision_damage", Tag = "cddf", Size = 0x30, MinVersion = CacheVersion.HaloOnlineED)]
    public class CollisionDamage : TagStructure
	{
        public float ApplyCollisionDamageScale; // 0 means 1.  1 is standard scale.  Some things may want to apply more damage
        public float FriendlyApplyCollisionDamage; // 0 means 1.  1 is standard scale.  Some things may want to apply more damage, yet go soft on their friends
        public Bounds<float> GameAccelerationBounds;
        public Bounds<float> GameDamageScaleBounds;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public Bounds<float> Unknown;

        public Bounds<float> AbsoluteDamageAccelerationBounds;
        public Bounds<float> AbsoluteDamageScaleBounds;
    }
}
