using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "scenario_lightmap", Tag = "sLdT", Size = 0x58, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "scenario_lightmap", Tag = "sLdT", Size = 0x4C)]
    public class ScenarioLightmap : TagStructure
	{
        public uint Unknown;

        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public List<ScenarioLightmapBspData> Lightmaps;

        public List<LightmapDataReference> LightmapDataReferences;
        public List<TagReferenceBlock> Unknown2;
        public List<Airprobe> Airprobes;
        public List<UnknownBlock2> Unknown3;
        public List<UnknownBlock3> Unknown4;

        public uint Unknown5;
        public uint Unknown6;
        public uint Unknown7;

        [TagStructure(Size = 0x10)]
        public class LightmapDataReference : TagStructure
		{
            public CachedTagInstance LightmapData;
        }
        
        [Flags]
        public enum AirprobeFlags : ushort
        {
            None = 0,
            Bit0 = 1 << 0,
            Bit1 = 1 << 1,
            Bit2 = 1 << 2,
            Bit3 = 1 << 3,
            Bit4 = 1 << 4,
            Bit5 = 1 << 5,
            Bit6 = 1 << 6,
            Bit7 = 1 << 7,
            Bit8 = 1 << 8,
            Bit9 = 1 << 9,
            Bit10 = 1 << 10,
            Bit11 = 1 << 11,
            Bit12 = 1 << 12,
            Bit13 = 1 << 13,
            Bit14 = 1 << 14,
            Bit15 = 1 << 15
        }

        [TagStructure(Size = 0x5C)]
        public class Airprobe : TagStructure
		{
            public RealPoint3d Position;
            public StringId Name;
            public AirprobeFlags Flags;

            [TagField(Length = 12)]
            public short[] RedShiftCoefficients = new short[12];

            [TagField(Length = 12)]
            public short[] GreenShiftCoefficients = new short[12];

            [TagField(Length = 12)]
            public short[] BlueShiftCoefficients = new short[12];

            public short AnalyticalVisibility;
        }

        [TagStructure(Size = 0x50)]
        public class UnknownBlock2 : TagStructure
		{
            public uint Unknown;
            public short Unknown2;
            public byte Unknown3;
            public byte Unknown4;
            public short Unknown5;
            public short Unknown6;
            public short Unknown7;
            public short Unknown8;
            public short Unknown9;
            public short Unknown10;
            public short Unknown11;
            public short Unknown12;
            public short Unknown13;
            public short Unknown14;
            public short Unknown15;
            public short Unknown16;
            public short Unknown17;
            public short Unknown18;
            public short Unknown19;
            public short Unknown20;
            public short Unknown21;
            public short Unknown22;
            public short Unknown23;
            public short Unknown24;
            public short Unknown25;
            public short Unknown26;
            public short Unknown27;
            public short Unknown28;
            public short Unknown29;
            public short Unknown30;
            public short Unknown31;
            public short Unknown32;
            public short Unknown33;
            public short Unknown34;
            public short Unknown35;
            public short Unknown36;
            public short Unknown37;
            public short Unknown38;
            public short Unknown39;
            public short Unknown40;
        }

        [TagStructure(Size = 0x2C)]
        public class UnknownBlock3 : TagStructure
		{
            public uint Unknown;
            public short Unknown2;
            public short Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;
            public float Unknown8;
            public float Unknown9;
            public List<UnknownBlock> Unknown10;

            [TagStructure(Size = 0x54)]
            public class UnknownBlock : TagStructure
			{
                public float Unknown;
                public float Unknown2;
                public float Unknown3;
                public short Unknown4;
                public short Unknown5;
                public short Unknown6;
                public short Unknown7;
                public short Unknown8;
                public short Unknown9;
                public short Unknown10;
                public short Unknown11;
                public short Unknown12;
                public short Unknown13;
                public short Unknown14;
                public short Unknown15;
                public short Unknown16;
                public short Unknown17;
                public short Unknown18;
                public short Unknown19;
                public short Unknown20;
                public short Unknown21;
                public short Unknown22;
                public short Unknown23;
                public short Unknown24;
                public short Unknown25;
                public short Unknown26;
                public short Unknown27;
                public short Unknown28;
                public short Unknown29;
                public short Unknown30;
                public short Unknown31;
                public short Unknown32;
                public short Unknown33;
                public short Unknown34;
                public short Unknown35;
                public short Unknown36;
                public short Unknown37;
                public short Unknown38;
                public short Unknown39;
            }
        }
    }
}