using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "sound_effect_template", Tag = "<fx>", Size = 0x28)]
    public class SoundEffectTemplate : TagStructure
    {
        public List<SoundEffectTemplateDefinition> TemplateCollection;
        public StringId InputEffectName;
        public List<SoundEffectTemplateSoundInputDefinition> AdditionalSoundInputs;
        public List<PlatformSoundEffectTemplateCollection> Unknown1;
        
        [TagStructure(Size = 0x2C)]
        public class SoundEffectTemplateDefinition : TagStructure
        {
            public StringId DspEffect;
            /// <summary>
            /// WARNING
            /// </summary>
            /// <remarks>
            /// DON'T MODIFY THIS TAG UNLESS YOU KNOW WHAT YOU ARE DOING
            /// </remarks>
            public byte[] Explanation;
            public FlagsValue Flags;
            public short Unknown1;
            public short Unknown2;
            public List<SoundEffectTemplateParameterDefinition> Parameters;
            
            [Flags]
            public enum FlagsValue : uint
            {
                UseHighLevelParameters = 1 << 0,
                CustomParameters = 1 << 1
            }
            
            [TagStructure(Size = 0x28)]
            public class SoundEffectTemplateParameterDefinition : TagStructure
            {
                public StringId Name;
                public TypeValue Type;
                public FlagsValue Flags;
                public int HardwareOffset;
                public int DefaultEnumIntegerValue;
                public float DefaultScalarValue;
                public FunctionDefinition DefaultFunction;
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
                
                [TagStructure(Size = 0xC)]
                public class FunctionDefinition : TagStructure
                {
                    public List<Byte> Data;
                    
                    [TagStructure(Size = 0x1)]
                    public class Byte : TagStructure
                    {
                        public sbyte Value;
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class SoundEffectTemplateSoundInputDefinition : TagStructure
        {
            public StringId DspEffect;
            public FunctionDefinition LowFrequencySound;
            public float TimePeriod; //  seconds
            
            [TagStructure(Size = 0xC)]
            public class FunctionDefinition : TagStructure
            {
                public List<Byte> Data;
                
                [TagStructure(Size = 0x1)]
                public class Byte : TagStructure
                {
                    public sbyte Value;
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class PlatformSoundEffectTemplateCollection : TagStructure
        {
            public List<PlatformSoundEffectTemplate> PlatformEffectTemplates;
            public StringId InputDspEffectName;
            
            [TagStructure(Size = 0x1C)]
            public class PlatformSoundEffectTemplate : TagStructure
            {
                public StringId InputDspEffectName;
                [TagField(Flags = Padding, Length = 12)]
                public byte[] Padding1;
                public List<PlatformSoundEffectTemplateComponent> Components;
                
                [TagStructure(Size = 0x10)]
                public class PlatformSoundEffectTemplateComponent : TagStructure
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

