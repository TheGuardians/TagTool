using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "breakable_surface", Tag = "bsdt", Size = 0x54, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "breakable_surface", Tag = "bsdt", Size = 0x68, MinVersion = CacheVersion.HaloReach)]
    public class BreakableSurface : TagStructure
    {
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public Bounds<float> DirectDamageVitality;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public Bounds<float> CollisionDamageImpulseThresholds;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public Bounds<float> AoeDamageVitality;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float MaximumVitality;
        [TagField(ValidTags = new[] { "effe" })]
        public CachedTag Effect;
        [TagField(ValidTags = new[] { "snd!" })]
        public CachedTag Sound;
        public List<Effect.Event.ParticleSystem> ParticleEffects;
        public float ParticleDensity;
        [TagField(ValidTags = new[] { "bitm" })]
        public CachedTag CrackBitmap;
        [TagField(ValidTags = new[] { "bitm" })]
        public CachedTag HoleBitmap;
    }
}
