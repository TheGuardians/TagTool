using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "game_progression", Tag = "gpdt", Size = 0x3C, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "game_progression", Tag = "gpdt", Size = 0x44, MinVersion = CacheVersion.HaloOnline106708)]
    public class GameProgression : TagStructure
	{
        public List<UnknownBlock> Unknown;
        public List<UnknownBlock2> Unknown2;
        public List<UnknownBlock3> Unknown3;
        public List<UnknownBlock4> Unknown4;
        public List<UnknownBlock5> Unknown5;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown6;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown7;

        [TagStructure(Size = 0x4)]
        public class UnknownBlock : TagStructure
		{
            public StringId Unknown;
        }

        [TagStructure(Size = 0x4)]
        public class UnknownBlock2 : TagStructure
		{
            public StringId Unknown;
        }

        [TagStructure(Size = 0x4)]
        public class UnknownBlock3 : TagStructure
		{
            public short Unknown;
            public short Unknown2;
        }

        [TagStructure(Size = 0x4)]
        public class UnknownBlock4 : TagStructure
		{
            public short Unknown;
            public short Unknown2;
        }

        [TagStructure(Size = 0x124)]
        public class UnknownBlock5 : TagStructure
		{
            public StringId MapName;
            public int Unknown;
            public int Unknown2;
            public CachedTagInstance Unknown3;
            public int MapId;
            public int Unknown4;
            [TagField(Length = 256)] public string MapScenarioPath;
        }
    }
}
