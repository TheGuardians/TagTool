using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "sound_effect_template", Tag = "<fx>", Size = 0x1C)]
    public class SoundEffectTemplate : TagStructure
    {
        public List<SoundEffectTemplatesBlock> TemplateCollection;
        public StringId InternalDspEffectName;
        public List<SoundEffectTemplateAdditionalSoundInputBlock> AdditionalSoundInputs;
        
        [TagStructure(Size = 0x2C)]
        public class SoundEffectTemplatesBlock : TagStructure
        {
            public StringId DspEffect;
            public byte[] Explanation;
            public SoundEffectTemplateFlags Flags;
            public short DspStateOffset;
            public short DspStateSize;
            public List<SoundEffectTemplateParameterBlock> Parameters;
            
            [Flags]
            public enum SoundEffectTemplateFlags : uint
            {
                UseHighLevelParameters = 1 << 0,
                CustomParameters = 1 << 1
            }
            
            [TagStructure(Size = 0x30)]
            public class SoundEffectTemplateParameterBlock : TagStructure
            {
                public StringId Name;
                public SoundEffectTemplateTypeEnum Type;
                public SoundEffectTemplateParameterFlags Flags;
                public int HardwareOffset;
                public int DefaultEnumIntegerValue;
                public float DefaultScalarValue;
                public MappingFunction DefaultFunction;
                public float MinimumScalarValue;
                public float MaximumScalarValue;
                
                public enum SoundEffectTemplateTypeEnum : short
                {
                    Integer,
                    Real,
                    FilterType
                }
                
                [Flags]
                public enum SoundEffectTemplateParameterFlags : ushort
                {
                    ExposeAsFunction = 1 << 0
                }
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
        }
        
        [TagStructure(Size = 0x1C)]
        public class SoundEffectTemplateAdditionalSoundInputBlock : TagStructure
        {
            public StringId DspEffect;
            public MappingFunction LowFrequencySound;
            public float TimePeriod; // seconds
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
    }
}
