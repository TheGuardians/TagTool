using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "game_progression", Tag = "gpdt", Size = 0x3C, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "game_progression", Tag = "gpdt", Size = 0x44, MinVersion = CacheVersion.HaloOnlineED)]
    public class GameProgression : TagStructure
	{
        public List<ProgressionVariable> IntegerProgressionValue;
        public List<ProgressionVariable> BooleanProgressionValue;

        // There are hardcoded boolean regions, this block defines where they are in the above tagblock
        // index: 0 = terminals, 1 = arg slots, 2 = training
        public List<BooleanProgressionRegion> BooleanProgressionRegions;

        public List<IntegerProgressionUnknownBlock> IntegerProgressionUnknown;
        public List<MapProgressionDataBlock> MapProgressionData;

        [TagField(Flags = Padding, Length = 8, MinVersion = CacheVersion.HaloOnlineED)]
        public byte[] Unused;

        [TagStructure(Size = 0x4)]
        public class ProgressionVariable : TagStructure
		{
            public StringId Variable;
        }

        [TagStructure(Size = 0x4)]
        public class BooleanProgressionRegion : TagStructure
		{
            public short StartIndex;
            public short EndIndex;
        }

        [TagStructure(Size = 0x4)]
        public class IntegerProgressionUnknownBlock : TagStructure
		{
            public short IntegerProgressionIndex;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused;
        }

        [TagStructure(Size = 0x124)]
        public class MapProgressionDataBlock : TagStructure
		{
            public StringId InternalMapName;
            public MapProgressionFlags Flags;
            public MapProgression MapProgressionType;
            public CachedTag ScenarioUnused; // unused, ODST had the group tags set to 'scnr'
            public int MapId;
            public int CampaignId;
            [TagField(Length = 256)] 
            public string ScenarioPath;

            [Flags]
            public enum MapProgressionFlags : int
            {
                None = 0,
                Bit0 = 1 << 0, // achievement related
                Bit1 = 1 << 1
            }

            public enum MapProgression : int
            {
                Default,
                IsHub,
                ReturnsToHub
            }
        }
    }
}
