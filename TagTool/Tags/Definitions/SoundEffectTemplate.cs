using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_effect_template", Tag = "<fx>", Size = 0x1C, MinVersion = CacheVersion.Halo3Retail)]
    public class SoundEffectTemplate : TagStructure
	{
        public List<SoundEffectTemplatesBlock> TemplateCollection;
        public StringId InternalDspEffectName;
        public List<AdditionalSoundInput> AdditionalSoundInputs;

        [TagStructure(Size = 0x2C)]
        public class SoundEffectTemplatesBlock : TagStructure
        {
            public StringId DspEffect;

            /* WARNING */
            public byte[] Explanation;
            public SoundEffectTemplateFlagsDefinition Flags;
            public short DspStateOffset;
            public short DspStateSize;
            public List<SoundEffectTemplateParameterBlock> Parameters;

            [Flags]
            public enum SoundEffectTemplateFlagsDefinition : uint
            {
                UseHighLevelParameters = 1 << 0,
                CustomParameters = 1 << 1
            }

            [TagStructure(Size = 0x30)]
            public class SoundEffectTemplateParameterBlock : TagStructure
            {
                public StringId Name;
                public SoundEffectTemplateTypeEnumDefinition Type;
                public SoundEffectTemplateParameterFlagsDefinition Flags;
                public int HardwareOffset;
                public int DefaultEnumIntegerValue;
                public float DefaultScalarValue;
                public TagFunction DefaultFunction;
                public float MinimumScalarValue;
                public float MaximumScalarValue;

                public enum SoundEffectTemplateTypeEnumDefinition : short
                {
                    Integer,
                    Real,
                    FilterType
                }

                [Flags]
                public enum SoundEffectTemplateParameterFlagsDefinition : ushort
                {
                    ExposeAsFunction = 1 << 0
                }
            }
        }

        [TagStructure(Size = 0x1C)]
        public class AdditionalSoundInput : TagStructure
		{
            public StringId DspEffect;
            public TagFunction LowFrequencySoundFunction = new TagFunction { Data = new byte[0] };
            public float TimePeriod;
        }
    }
}