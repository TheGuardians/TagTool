using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "fx_test", Tag = "fxtt", Size = 0xC)]
    public class FxTest : TagStructure
    {
        public List<FxPropertyBlock> Properties;
        
        [TagStructure(Size = 0x36C)]
        public class FxPropertyBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public FxPropertyFlags Flags;
            public int Scope;
            public FxPropertyValueTypeEnum ValueType;
            [TagField(Length = 256)]
            public string Expression;
            [TagField(Length = 256)]
            public string Initialize;
            [TagField(Length = 256)]
            public string Update;
            public List<FxPropertyDefaultBlock> Defaults;
            public StringId DefaultName;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Reference;
            public FxPropertyBitmapFilterEnum FilterMode;
            public FxPropertyBitmapAddressEnum AddressModeX;
            public FxPropertyBitmapAddressEnum AddressModeY;
            public FxPropertyBitmapBiasEnum MipBiasMode;
            public FxTestScalarFunctionStruct Function;
            
            [Flags]
            public enum FxPropertyFlags : uint
            {
                Predefined = 1 << 0,
                State = 1 << 1,
                Derived = 1 << 2
            }
            
            public enum FxPropertyValueTypeEnum : int
            {
                Default,
                Expression,
                Texture,
                Function
            }
            
            public enum FxPropertyBitmapFilterEnum : sbyte
            {
                Default,
                Point,
                Bilinear,
                Trilinear,
                Anisotropic2x,
                Anisotropic4x
            }
            
            public enum FxPropertyBitmapAddressEnum : sbyte
            {
                Default,
                Wrap,
                Clamp,
                Mirror,
                Border
            }
            
            public enum FxPropertyBitmapBiasEnum : sbyte
            {
                Default,
                Bias0,
                Bias1,
                Bias2,
                Bias3
            }
            
            [TagStructure(Size = 0x4)]
            public class FxPropertyDefaultBlock : TagStructure
            {
                public StringId Name;
            }
            
            [TagStructure(Size = 0x1C)]
            public class FxTestScalarFunctionStruct : TagStructure
            {
                public StringId InputVariable;
                public StringId RangeVariable;
                public MappingFunction Mapping;
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
        }
    }
}
