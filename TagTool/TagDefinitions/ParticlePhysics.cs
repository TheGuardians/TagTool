using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "particle_physics", Tag = "pmov", Size = 0x20, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "particle_physics", Tag = "pmov", Size = 0x2C, MinVersion = CacheVersion.HaloOnline106708)]
    public class ParticlePhysics
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
            CollideWithBipeds = 1 << 5
        }

        [TagStructure(Size = 0x18)]
        public class Movement
        {
            public TypeValue Type;
            public short Unknown1;
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
            public class Parameter
            {
                public int ParameterId;
                public byte Unknown1;
                public byte Unknown2;
                public byte Unknown3;
                public byte Unknown4;
                public TagFunction Function = new TagFunction { Data = new byte[0] };
                public float Unknown5;
                public byte Unknown6;
                public sbyte Unknown7;
                public sbyte Unknown8;
                public sbyte Unknown9;
            }
        }
    }
}