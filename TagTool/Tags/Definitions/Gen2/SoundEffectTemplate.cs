using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "sound_effect_template", Tag = "<fx>", Size = 0x1C)]
    public class SoundEffectTemplate : TagStructure
    {
        public List<SoundEffectTemplatesBlock> TemplateCollection;
        public StringId InputEffectName;
        public List<SoundEffectTemplateAdditionalSoundInputBlock> AdditionalSoundInputs;
        public List<PlatformSoundEffectTemplateCollectionBlock> Unknown;
        
        [TagStructure(Size = 0x1C)]
        public class SoundEffectTemplatesBlock : TagStructure
        {
            public StringId DspEffect;
            /// <summary>
            /// DON'T MODIFY THIS TAG UNLESS YOU KNOW WHAT YOU ARE DOING
            /// </summary>
            public byte[] Explanation;
            public FlagsValue Flags;
            public short Unknown;
            public short Unknown1;
            public List<SoundEffectTemplateParameterBlock> Parameters;
            
            [Flags]
            public enum FlagsValue : uint
            {
                UseHighLevelParameters = 1 << 0,
                CustomParameters = 1 << 1
            }
            
            [TagStructure(Size = 0x24)]
            public class SoundEffectTemplateParameterBlock : TagStructure
            {
                public StringId Name;
                public TypeValue Type;
                public FlagsValue Flags;
                public int HardwareOffset;
                public int DefaultEnumIntegerValue;
                public float DefaultScalarValue;
                public MappingFunctionBlock DefaultFunction;
                public float MinimumScalarValue;
                public float MaximumScalarValue;
                
                public enum TypeValue : short
                {
                    Integer,
                    Real,
                    FilterType
                }
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    ExposeAsFunction = 1 << 0
                }
                
                [TagStructure(Size = 0x8)]
                public class MappingFunctionBlock : TagStructure
                {
                    public List<ByteBlock> Data;
                    
                    [TagStructure(Size = 0x1)]
                    public class ByteBlock : TagStructure
                    {
                        public sbyte Value;
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class SoundEffectTemplateAdditionalSoundInputBlock : TagStructure
        {
            public StringId DspEffect;
            public MappingFunctionBlock LowFrequencySound;
            public float TimePeriod; //  seconds
            
            [TagStructure(Size = 0x8)]
            public class MappingFunctionBlock : TagStructure
            {
                public List<ByteBlock> Data;
                
                [TagStructure(Size = 0x1)]
                public class ByteBlock : TagStructure
                {
                    public sbyte Value;
                }
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class PlatformSoundEffectTemplateCollectionBlock : TagStructure
        {
            public List<PlatformSoundEffectTemplateBlock> PlatformEffectTemplates;
            public StringId InputDspEffectName;
            
            [TagStructure(Size = 0x18)]
            public class PlatformSoundEffectTemplateBlock : TagStructure
            {
                public StringId InputDspEffectName;
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<PlatformSoundEffectTemplateComponentBlock> Components;
                
                [TagStructure(Size = 0x10)]
                public class PlatformSoundEffectTemplateComponentBlock : TagStructure
                {
                    public ValueTypeValue ValueType;
                    public float DefaultValue;
                    public float MinimumValue;
                    public float MaximumValue;
                    
                    public enum ValueTypeValue : int
                    {
                        Zero,
                        Time,
                        Scale,
                        Rolloff
                    }
                }
            }
        }
    }
}

