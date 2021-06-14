using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "cui_static_data", Tag = "cust", Size = 0x18)]
    public class CuiStaticData : TagStructure
    {
        public StaticDataStruct StaticData;
        
        [TagStructure(Size = 0x18)]
        public class StaticDataStruct : TagStructure
        {
            public List<StaticDataColumn> Columns;
            public List<PropertiesStruct> Rows;
            
            [TagStructure(Size = 0x8)]
            public class StaticDataColumn : TagStructure
            {
                public StringId Name;
                public PropertyType Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum PropertyType : short
                {
                    Boolean,
                    Long,
                    Real,
                    String,
                    Component,
                    TagReference,
                    StringId,
                    ArgbColor,
                    EmblemInfo
                }
            }
            
            [TagStructure(Size = 0x54)]
            public class PropertiesStruct : TagStructure
            {
                public List<PropertyLongValue> LongProperties;
                public List<PropertyRealValue> RealProperties;
                public List<PropertyStringIdValue> StringIdProperties;
                public List<PropertycomponentPtrValue> ComponentPtrProperties;
                public List<PropertyTagReferenceValue> TagReferenceProperties;
                public List<PropertyTextValue> StringProperties;
                public List<PropertyArgbColorValue> ArgbColorProperties;
                
                [TagStructure(Size = 0x8)]
                public class PropertyLongValue : TagStructure
                {
                    public StringId Name;
                    public int Value;
                }
                
                [TagStructure(Size = 0x8)]
                public class PropertyRealValue : TagStructure
                {
                    public StringId Name;
                    public float Value;
                }
                
                [TagStructure(Size = 0x8)]
                public class PropertyStringIdValue : TagStructure
                {
                    public StringId Name;
                    public StringId Value;
                }
                
                [TagStructure(Size = 0xC)]
                public class PropertycomponentPtrValue : TagStructure
                {
                    public StringId Name;
                    public StringId Value;
                    public PropertycomponentPtrFlags Flags;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [Flags]
                    public enum PropertycomponentPtrFlags : byte
                    {
                        SourceIsInExternalSystem = 1 << 0
                    }
                }
                
                [TagStructure(Size = 0x14)]
                public class PropertyTagReferenceValue : TagStructure
                {
                    public StringId Name;
                    public CachedTag Value;
                }
                
                [TagStructure(Size = 0x104)]
                public class PropertyTextValue : TagStructure
                {
                    public StringId Name;
                    [TagField(Length = 256)]
                    public string Value;
                }
                
                [TagStructure(Size = 0x14)]
                public class PropertyArgbColorValue : TagStructure
                {
                    public StringId Name;
                    public RealArgbColor Value;
                }
            }
        }
    }
}
