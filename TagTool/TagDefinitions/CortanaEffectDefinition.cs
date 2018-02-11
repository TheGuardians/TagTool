using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "cortana_effect_definition", Tag = "crte", Size = 0x80)]
    public class CortanaEffectDefinition
    {
        public StringId ScenarioName;
        public int Unknown1;

        public uint Unknown2;
        public uint Unknown3;
        public uint Unknown4;

        public byte[] Data1;
        public byte[] Data2;
        public CachedTagInstance CinematicScene;
        public StringId Name;

        public List<UnknownBlock1> Unknown5;
        public List<UnknownBlock2> Unknown6;
        
        public uint Unknown9;
        public uint Unknown10;
        public uint Unknown11;

        public List<UnknownBlock3> Unknown7;
        
        [TagStructure(Size = 0x18)]
        public class UnknownBlock1
        {
            public List<UnknownBlock4> Unknown1;
            public List<UnknownBlock5> Unknown2;

            [TagStructure(Size = 0x20)]
            public class UnknownBlock4
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
            public class UnknownBlock5
            {
                public uint Unknown1;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
                public uint Unknown6;
                public uint Unknown7;
                public List<UnknownBlock6> Unknown8;

                [TagStructure(Size = 0x20)]
                public class UnknownBlock6
                {
                    public List<UnknownBlock7> Unknown1;
                    public TagFunction Unknown2 = new TagFunction { Data = new byte[0] };

                    [TagStructure(Size = 0x10)]
                    public class UnknownBlock7
                    {
                        public uint Unknown1;
                        public uint Unknown2;
                        public uint Unknown3;
                        public uint Unknown4;
                    }
                }
            }
        }

        [TagStructure(Size = 0x30)]
        public class UnknownBlock2
        {
            public List<UnknownBlock4> Unknown1;
            public List<UnknownBlock5> Unknown2;
            public List<UnknownBlock6> Unknown3;
            public List<UnknownBlock4> Unknown4;

            [TagStructure(Size = 0x34)]
            public class UnknownBlock4
            {
                public uint Unknown1;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
                public uint Unknown6;
                public uint Unknown7;
                public List<UnknownBlock6> Unknown8;
                public uint Unknown9;
                public uint Unknown10;
                public uint Unknown11;

                [TagStructure(Size = 0x20)]
                public class UnknownBlock6
                {
                    public List<UnknownBlock8> Unknown1;
                    public TagFunction Unknown2 = new TagFunction { Data = new byte[0] };

                    [TagStructure(Size = 0x10)]
                    public class UnknownBlock8
                    {
                        public uint Unknown1;
                        public uint Unknown2;
                        public uint Unknown3;
                        public uint Unknown4;
                    }
                }
            }

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
                public List<UnknownBlock7> Unknown7;
                public uint Unknown8;
                public uint Unknown9;
                public uint Unknown10;

                [TagStructure(Size = 0x20)]
                public class UnknownBlock7
                {
                    public List<UnknownBlock8> Unknown1;
                    public TagFunction Unknown2 = new TagFunction { Data = new byte[0] };

                    [TagStructure(Size = 0xC)]
                    public class UnknownBlock8
                    {
                        public uint Unknown1;
                        public uint Unknown2;
                        public uint Unknown3;
                    }
                }
            }
        }

        [TagStructure(Size = 0x24)]
        public class UnknownBlock3
        {
            public List<UnknownBlock4> Unknown1;
            public List<UnknownBlock5> Unknown2;
            public List<UnknownBlock6> Unknown3;

            [TagStructure(Size = 0x30)]
            public class UnknownBlock4
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
            }

            [TagStructure(Size = 0x34)]
            public class UnknownBlock5
            {
                public uint Unknown1;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
                public uint Unknown6;
                public uint Unknown7;
                public List<UnknownBlock6> Unknown8;
                public uint Unknown9;
                public uint Unknown10;
                public uint Unknown11;

                [TagStructure(Size = 0x20)]
                public class UnknownBlock6
                {
                    public List<UnknownBlock8> Unknown1;
                    public TagFunction Unknown2 = new TagFunction { Data = new byte[0] };

                    [TagStructure(Size = 0x10)]
                    public class UnknownBlock8
                    {
                        public uint Unknown1;
                        public uint Unknown2;
                        public uint Unknown3;
                        public uint Unknown4;
                    }
                }
            }

            [TagStructure(Size = 0xC)]
            public class UnknownBlock6
            {
                public uint Unknown1;
                public uint Unknown2;
                public uint Unknown3;
            }
        }
    }
}