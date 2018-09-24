using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "collision_damage", Tag = "cddf", Size = 0x28, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "collision_damage", Tag = "cddf", Size = 0x30, MinVersion = CacheVersion.HaloOnline106708)]
    public class CollisionDamage : TagStructure
	{
        public float ApplyDamageScale;
        public float ApplyRecoilDamageScale;
        public Bounds<float> DamageAccelerationBounds;
        public Bounds<float> DamageScaleBounds;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public Bounds<float> Unknown;
        public Bounds<float> RecoilDamageAccelerationBounds;
        public Bounds<float> RecoilDamageScaleBounds;
    }
}
