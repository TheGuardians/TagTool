using TagTool.Cache;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "particle_physics", Tag = "pmov", Size = 0x20, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "particle_physics", Tag = "pmov", Size = 0x2C, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "particle_physics", Tag = "pmov", Size = 0x30, MinVersion = CacheVersion.HaloReach)]
    public class ParticlePhysics : TagStructure
	{
        public CachedTag Template;
        public ParticleMovementFlags Flags;
        public List<ParticleController> Movements;

        [TagField(Flags = Padding, Length = 12, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Padding_ED;

        [TagField(ValidTags = new[] { "bitm" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag TurbulenceTexture;

        [Flags]
        public enum ParticleMovementFlags : uint
        {
            None = 0,
            Physics = 1 << 0,
            CollideWithStructure = 1 << 1,
            CollideWithMedia = 1 << 2,
            CollideWithScenery = 1 << 3,
            CollideWithVehicles = 1 << 4,
            CollideWithBipeds = 1 << 5,
            Swarm = 1 << 6,
            Wind = 1 << 7
        }

        [TagStructure(Size = 0x18)]
        public class ParticleController : TagStructure
		{
            public ParticleMovementType Type;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public ParticleControllerFlags Flags;
            [TagField(Flags = Padding, Length = 2, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagField(Flags = Padding, Length = 1, MinVersion = CacheVersion.HaloReach)]
            public byte[] Padding0;
            public List<ParticleControllerParameter> Parameters;
            public int RuntimeMConstantParameters;
            public int RuntimeMUsedParticleStates;

            public enum ParticleMovementType : short
            {
                Physics,
                Collider,
                Swarm,
                Wind
            }

            [Flags]
            public enum ParticleControllerFlags : byte
            {
                PropertiesFullyIndexed = 1 << 0
            }

            [TagStructure(Size = 0x24)]
            public class ParticleControllerParameter : TagStructure
			{
                public int ParameterId;
                public ParticlePropertyScalar Property;
            }
        }
    }
}