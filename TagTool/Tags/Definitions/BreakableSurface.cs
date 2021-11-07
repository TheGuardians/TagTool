using System.Collections.Generic;
using TagTool.Cache;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "breakable_surface", Tag = "bsdt", Size = 0x54)]
    public class BreakableSurface : TagStructure
	{
        public float MaximumVitality;
        public CachedTag Effect;
        public CachedTag Sound;
        public List<ParticleSystemDefinitionBlockNew> ParticleEffects;
        public float ParticleDensity;
        public CachedTag CrackBitmap;
        public CachedTag HoleBitmap;

        [TagStructure(Size = 0x24)]
        public class ParticleSystemDefinitionBlockNew : TagStructure
		{
            public uint Unknown;
            public uint Unknown2;
            public byte[] Unknown3;
            public uint Unknown4;
            public uint Unknown5;
        }
    }
}