using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using System;

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
        public CachedTag CinematicScene;
        public StringId AnchorName;

        public List<PostprocessingBlock> PostProcessing;

        public List<GravemindBlock> Gravemind;

        public List<CortanaBlock> Unused;

        public List<CortanaBlock> Cortana;

        [TagStructure(Size = 0x14)]
        public class SoundBlock : TagStructure
        {
            public uint Unknown;
            public CachedTag Sound;
        }

        [TagStructure(Size = 0x48)]
        public class PostprocessingBlock : TagStructure
        {
            public List<ScalarBlock> FOV;
            public List<ColorBlock> Hue;
            public List<ColorBlock> Saturation;
            public List<ScalarBlock> Rumble;
            public List<ScalarBlock> HUDBrightness;
            public List<ScalarBlock> HUDShakeAmount;
        }

        [TagStructure(Size = 0x30)]
        public class GravemindBlock : TagStructure
        {
            public List<ColorBlock> Luminance;
            public List<ColorBlock> Unknown2;
            public List<ScalarBlock> TentaclesIn;
            public List<ColorBlock> Vignette;
        }

        [TagStructure(Size = 0x24)]
        public class CortanaBlock : TagStructure
        {
            public List<ScalarBlock> Unknown1;
            public List<ColorBlock> Unknown2;
            public List<ScalarBlock> Colorize;
        }

        [Flags]
        public enum CortanaEffectFlags : int
        {
            UseCosineInterpolation = 1 << 0
        }

        [TagStructure(Size = 0x30)]
        public class ScalarBlock : TagStructure
        {
            public CortanaEffectFlags Flags;

            public float Shaderargs1value1;
            public float Shaderargs1value2;
            public float Shaderargs1value3;

            public float Basevalue1;
            public float Basevalue2;

            public List<DynamicValue> DynamicValues;

            public float Shaderargs2value1;
            public float Shaderargs2value2;
            public float Shaderargs2value3;

            [TagStructure(Size = 0x20)]
            public class DynamicValue : TagStructure
            {
                public List<FrameBlock> Frames;
                public TagFunction FrameFunction = new TagFunction { Data = new byte[0] };

                [TagStructure(Size = 0xC)]
                public class FrameBlock : TagStructure
                {
                    public int Frame;
                    public float Dynamicvalue1;
                    public float Dynamicvalue2;
                }
            }
        }

        [TagStructure(Size = 0x34)]
        public class ColorBlock : TagStructure
        {
            public CortanaEffectFlags Flags;

            public float Shaderargs1value1;
            public float Shaderargs1value2;
            public float Shaderargs1value3;

            public float Basevalue1;
            public float Basevalue2;
            public float Basevalue3;

            public List<DynamicValue> DynamicValues;

            public float Shaderargs2value1;
            public float Shaderargs2value2;
            public float Shaderargs2value3;

            [TagStructure(Size = 0x20)]
            public class DynamicValue : TagStructure
            {
                public List<FrameBlock> Frames;
                public TagFunction FrameFunction = new TagFunction { Data = new byte[0] };

                [TagStructure(Size = 0x10)]
                public class FrameBlock : TagStructure
                {
                    public int Frame;
                    public float Dynamicvalue1;
                    public float Dynamicvalue2;
                    public float Dynamicvalue3;
                }
            }
        }
    }
}