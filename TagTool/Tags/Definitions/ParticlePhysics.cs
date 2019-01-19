using TagTool.Cache;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "particle_physics", Tag = "pmov", Size = 0x20, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "particle_physics", Tag = "pmov", Size = 0x2C, MinVersion = CacheVersion.HaloOnline106708)]
    public class ParticlePhysics : TagStructure
	{
        public CachedTagInstance Template;
        public FlagsValue Flags;
        public List<Movement> Movements;

        [TagField(Padding = true, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused1;

        [Flags]
        public enum FlagsValue : uint
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
        public class Movement : TagStructure
		{
            public TypeValue Type;
            public byte Flags;
            [TagField(Padding = true, Length = 1)]
            public byte Unused;
            public List<Parameter> Parameters;
            public int Unknown2;
            public int Unknown3;

            public enum TypeValue : short
            {
                Physics,
                Collider,
                Swarm,
                Wind
            }

            [TagStructure(Size = 0x24)]
            public class Parameter : TagStructure
			{
                public int ParameterId;
                public TagMapping Property;
            }
        }
    }
}