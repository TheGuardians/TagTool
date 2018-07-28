using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "cortana_effect_definition", Tag = "crte", Size = 0x80)]
    public class CortanaEffectDefinition
    {
        public StringId ScenarioName;
        public int Unknown1;

        public List<SoundBlock> Sounds;

        public byte[] Data1;
        public byte[] Data2;
        public CachedTagInstance CinematicScene;
        public StringId Name;

        public List<UnknownBlock1> Unknown2;
        public List<PassBlock> Pass;
        public List<UnknownBlock3> Unknown3;
        public List<UnknownBlock3> Unknown4;

        [TagStructure(Size = 0x14)]
        public class SoundBlock
        {
            public uint Unknown;
            public CachedTagInstance Sound;
        }

        [TagStructure(MaxVersion = CacheVersion.Halo3Retail, Size = 0x18)]
        [TagStructure(MinVersion = CacheVersion.HaloOnline106708, Size = 0x48)]
        public class UnknownBlock1
        {
            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public List<UnknownBlock5> Unknown1;
            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public List<UnknownBlock6> Unknown2;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public UnknownObject Unknown3;

            [TagStructure(Size = 0x48)]
            public class UnknownObject
            {
                public UnknownBlock5 Unknown1;
                public UnknownBlock6 Unknown2;
            }

            [TagStructure(Size = 0x20)]
            public class UnknownBlock5
            {
                public uint Unknown1;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
                public uint Unknown6;
                public uint Unknown7;
                public uint Unknown8;
            }

            [TagStructure(Size = 0x28)]
            public class UnknownBlock6
            {
                public uint Unknown1;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
                public uint Unknown6;
                public uint Unknown7;
                public List<UnknownTagFunctionBlock> Unknown8;
            }
        }

        [TagStructure(Size = 0x30)]
        public class PassBlock
        {
            public List<UnknownBlock4> Unknown1;
            public List<UnknownBlock5> Unknown2;
            public List<UnknownBlock6> Unknown3;
            public List<UnknownBlock4> Unknown4;

            [TagStructure(Size = 0x34, Align = 0x08)]
            public class UnknownBlock5
            {
                public uint Unknown1;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
                public uint Unknown6;
                public uint Unknown7;
                public uint Unknown8;
                public uint Unknown9;
                public uint Unknown10;
                public uint Unknown11;
                public uint Unknown12;
                public uint Unknown13;
            }

            [TagStructure(Size = 0x30)]
            public class UnknownBlock6
            {
                public uint Unknown1;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
                public uint Unknown6;
                public List<UnknownTagFunctionBlock> Unknown7;
                public uint Unknown8;
                public uint Unknown9;
                public uint Unknown10;
            }
        }

        [TagStructure(Size = 0x24)]
        public class UnknownBlock3
        {
            public List<UnknownBlock5> Unknown1;
            public List<UnknownBlock4> Unknown2;
            public List<UnknownBlock6> Unknown3;

            [TagStructure(Size = 0x30)]
            public class UnknownBlock5
            {
                public uint Unknown1;
                public float Unknown2;
                public float Unknown3;
                public float Unknown4;
                public float Unknown5;
                public float Unknown6;
                public float Unknown7;
                public float Unknown8;
                public float Unknown9;
                public float Unknown10;
                public float Unknown11;
                public float Unknown12;
            }

            [TagStructure(Size = 0xC)]
            public class UnknownBlock6
            {
                public float Unknown1;
                public float Unknown2;
                public float Unknown3;
            }
        }

        [TagStructure(Size = 0x34)]
        public class UnknownBlock4
        {
            public uint Unknown1;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;
            public List<UnknownTagFunctionBlock> Function;
            public RealRgbColor Color;
        }

        [TagStructure(Size = 0x20)]
        public class UnknownTagFunctionBlock
        {
            public List<UnknownBlock8> Unknown1;
            public TagFunction Unknown2 = new TagFunction { Data = new byte[0] };

            [TagStructure(Size = 0x10)]
            public class UnknownBlock8
            {
                public int Frame;
                public float Unknown2;
                public float Unknown3;
                public float Unknown4;
            }
        }
    }
}