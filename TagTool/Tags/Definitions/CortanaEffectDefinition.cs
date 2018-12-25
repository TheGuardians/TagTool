using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "cortana_effect_definition", Tag = "crte", Size = 0x80)]
    public class CortanaEffectDefinition : TagStructure
    {
        public StringId ScenarioName;
        public int FrameCount;

        public List<SoundBlock> Sounds;

        public byte[] Data1;
        public byte[] Data2;
        public CachedTagInstance CinematicScene;
        public StringId AnchorName;

        public List<UnknownBlock1> Unknown2;

        public List<PassBlock> Pass;

        public List<UnknownBlock2> Unknown3;

        public List<UnknownBlock2> Unknown4;

        [TagStructure(Size = 0x14)]
        public class SoundBlock : TagStructure
        {
            public uint Unknown;
            public CachedTagInstance Sound;
        }

        [TagStructure(Size = 0x48)]
        public class UnknownBlock1 : TagStructure
        {
            public List<UnknownStandardBlock1> Unknown1;
            public List<UnknownStandardBlock2> Unknown2;
            public List<UnknownStandardBlock2> Unknown3;
            public List<UnknownStandardBlock1> Unknown4;
            public List<UnknownStandardBlock1> Unknown5;
            public List<UnknownStandardBlock1> Unknown6;
        }

        [TagStructure(Size = 0x30)]
        public class PassBlock : TagStructure
        {
            public List<UnknownStandardBlock2> Unknown1;
            public List<UnknownStandardBlock2> Unknown2;
            public List<UnknownStandardBlock1> Unknown3;
            public List<UnknownStandardBlock2> Unknown4;
        }

        [TagStructure(Size = 0x24)]
        public class UnknownBlock2 : TagStructure
        {
            public List<UnknownStandardBlock1> Unknown1;
            public List<UnknownStandardBlock2> Unknown2;
            public List<UnknownStandardBlock1> Unknown3;
        }

        [TagStructure(Size = 0x30)]
        public class UnknownStandardBlock1 : TagStructure
        {
            public byte Flags;
            public byte Unknown1;
            public short Unknown2;

            public float Unknown3;
            public float Unknown4;
            public float Unknown5;

            public float Value1;
            public float Value2;

            public List<DynamicValue> DynamicValues;

            public float Unknown10;
            public float Unknown11;
            public float Unknown12;

            [TagStructure(Size = 0x20)]
            public class DynamicValue : TagStructure
            {
                public List<FrameBlock> Frames;
                public TagFunction FrameFunction = new TagFunction { Data = new byte[0] };

                [TagStructure(Size = 0xC)]
                public class FrameBlock : TagStructure
                {
                    public int Frame;
                    public float Value1;
                    public float Value2;
                }
            }
        }

        [TagStructure(Size = 0x34)]
        public class UnknownStandardBlock2 : TagStructure
        {
            public byte Flags;
            public byte Unknown1;
            public short Unknown2;

            public float Unknown3;
            public float Unknown4;
            public float Unknown5;

            public float Value1;
            public float Value2;
            public float Value3;

            public List<DynamicValue> DynamicValues;

            public float Unknown10;
            public float Unknown11;
            public float Unknown12;

            [TagStructure(Size = 0x20)]
            public class DynamicValue : TagStructure
            {
                public List<FrameBlock> Frames;
                public TagFunction FrameFunction = new TagFunction { Data = new byte[0] };

                [TagStructure(Size = 0x10)]
                public class FrameBlock : TagStructure
                {
                    public int Frame;
                    public float Value1;
                    public float Value2;
                    public float Value3;
                }
            }
        }
    }
}