using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "cui_logic", Tag = "culo", Size = 0x8C)]
    public class CuiLogic : TagStructure
    {
        public StringId LogicName;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag StringList;
        public CuiSystem System;
        
        [TagStructure(Size = 0x78)]
        public class CuiSystem : TagStructure
        {
            public List<TemplateInstantiationBlock> TemplateInstantiations;
            public List<ComponentDefinition> Components;
            public List<ComponentIndexBlock> ComponentIndices;
            public List<OverlayDefinition> Overlays;
            public List<OverlayEditorOnlyDefinition> OverlaysEditorOnly;
            public List<PropertyBinding> PropertyBindings;
            public List<BindingConversionLongComparisonBlock> BindingConversionLongComparisons;
            public List<StaticDataTable> StaticDataTables;
            public List<Expression> Expressions;
            public List<Encapsulatedproperties> EncapsulatedProperties;
            
            [TagStructure(Size = 0x10)]
            public class TemplateInstantiationBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "cusc" })]
                public CachedTag ScreenReference;
            }
            
            [TagStructure(Size = 0x10)]
            public class ComponentDefinition : TagStructure
            {
                public StringId Type;
                public StringId Name;
                public StringId Parent;
                public ComponentDefinitionFlags Flags;
                public short TemplateInstantiationIndex;
                
                [Flags]
                public enum ComponentDefinitionFlags : ushort
                {
                    HiddenToToolsUser = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ComponentIndexBlock : TagStructure
            {
                public StringId Name;
                public short ComponentDefinitionIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x20)]
            public class OverlayDefinition : TagStructure
            {
                public StringId Resolution;
                public StringId Theme;
                public List<ComponentPropertiesDefinition> Components;
                public List<AnimationDefinition> Animations;
                
                [TagStructure(Size = 0x58)]
                public class ComponentPropertiesDefinition : TagStructure
                {
                    public StringId Name;
                    public PropertiesStruct PropertyValues;
                    
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
                
                [TagStructure(Size = 0x1C)]
                public class AnimationDefinition : TagStructure
                {
                    public StringId Name;
                    public float TimeBaseOffset;
                    public float TimeExponentialOffset;
                    public AnimationinputType AnimationInput;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public List<AnimationComponentDefinition> Components;
                    
                    public enum AnimationinputType : sbyte
                    {
                        Time,
                        Binding
                    }
                    
                    [TagStructure(Size = 0x20)]
                    public class AnimationComponentDefinition : TagStructure
                    {
                        public StringId Name;
                        public int TotalMilliseconds;
                        public List<AnimationComponentRealProperty> RealProperties;
                        public List<AnimationComponentArgbColorProperty> ArgbColorProperties;
                        
                        [TagStructure(Size = 0x18)]
                        public class AnimationComponentRealProperty : TagStructure
                        {
                            public StringId Name;
                            public AnimationPropertyCompositionType Composition;
                            public AnimationPropertyFlags Flags;
                            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                            public byte[] Padding;
                            public int TotalMilliseconds;
                            public List<AnimationPropertyKeyframeRealValue> RealKeyframes;
                            
                            public enum AnimationPropertyCompositionType : short
                            {
                                RelativeToCurrent,
                                AbsoluteToScreen,
                                AbsoluteToParent
                            }
                            
                            [Flags]
                            public enum AnimationPropertyFlags : byte
                            {
                                Loop = 1 << 0
                            }
                            
                            [TagStructure(Size = 0x1C)]
                            public class AnimationPropertyKeyframeRealValue : TagStructure
                            {
                                // the amount of time from the previous keyframe to this keyframe
                                public int Duration; // milliseconds
                                // the value when the current time is on this keyframe
                                public float Value;
                                public AnimationScalarFunction TransitionFunction;
                                
                                [TagStructure(Size = 0x14)]
                                public class AnimationScalarFunction : TagStructure
                                {
                                    public MappingFunction ScalarFunction;
                                    
                                    [TagStructure(Size = 0x14)]
                                    public class MappingFunction : TagStructure
                                    {
                                        public byte[] Data;
                                    }
                                }
                            }
                        }
                        
                        [TagStructure(Size = 0x18)]
                        public class AnimationComponentArgbColorProperty : TagStructure
                        {
                            public StringId Name;
                            public AnimationPropertyCompositionType Composition;
                            public AnimationPropertyFlags Flags;
                            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                            public byte[] Padding;
                            public int TotalMilliseconds;
                            public List<AnimationPropertyKeyframeArgbColorValue> ArgbColorKeyframes;
                            
                            public enum AnimationPropertyCompositionType : short
                            {
                                RelativeToCurrent,
                                AbsoluteToScreen,
                                AbsoluteToParent
                            }
                            
                            [Flags]
                            public enum AnimationPropertyFlags : byte
                            {
                                Loop = 1 << 0
                            }
                            
                            [TagStructure(Size = 0x28)]
                            public class AnimationPropertyKeyframeArgbColorValue : TagStructure
                            {
                                // the amount of time from the previous keyframe to this keyframe
                                public int Duration; // milliseconds
                                // the value when the current time is on this keyframe
                                public RealArgbColor Color;
                                public AnimationScalarFunction TransitionFunction;
                                
                                [TagStructure(Size = 0x14)]
                                public class AnimationScalarFunction : TagStructure
                                {
                                    public MappingFunction ScalarFunction;
                                    
                                    [TagStructure(Size = 0x14)]
                                    public class MappingFunction : TagStructure
                                    {
                                        public byte[] Data;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            [TagStructure(Size = 0x24)]
            public class OverlayEditorOnlyDefinition : TagStructure
            {
                public StringId Resolution;
                public StringId Theme;
                public EditorOverlayInfoFlags Flags;
                public List<ComponentEditorOnlyDefinition> ComponentsEditorOnly;
                public List<AnimationEditorOnlyDefinition> AnimationsEditorOnly;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum EditorOverlayInfoFlags : byte
                {
                    ExcludeOverlay = 1 << 0
                }
                
                [TagStructure(Size = 0x10)]
                public class ComponentEditorOnlyDefinition : TagStructure
                {
                    public StringId Name;
                    public List<PropertyEditorOnlyDefinition> PropertiesEditorOnly;
                    
                    [TagStructure(Size = 0x8)]
                    public class PropertyEditorOnlyDefinition : TagStructure
                    {
                        public StringId Name;
                        public EditorPropertyInfoFlags Flags;
                        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        
                        [Flags]
                        public enum EditorPropertyInfoFlags : byte
                        {
                            DefaultValue = 1 << 0
                        }
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class AnimationEditorOnlyDefinition : TagStructure
                {
                    public StringId Name;
                    public List<ComponentEditorOnlyDefinition> ComponentsEditorOnly;
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class PropertyBinding : TagStructure
            {
                public PropertyBindingFlags Flags;
                public BindingConversionFunctionEnum ConversionFunction;
                public StringId SourceComponentName;
                public StringId SourcePropertyName;
                public StringId TargetComponentName;
                public StringId TargetPropertyName;
                
                [Flags]
                public enum PropertyBindingFlags : ushort
                {
                    SourceIsInExternalScreen = 1 << 0,
                    SourceIsInExternalSystem = 1 << 1,
                    TargetIsInExternalSystem = 1 << 2
                }
                
                public enum BindingConversionFunctionEnum : short
                {
                    None,
                    NegateBool,
                    CompareLong
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class BindingConversionLongComparisonBlock : TagStructure
            {
                public StringId TargetComponentName;
                public StringId TargetPropertyName;
                public BindingConversionComparisonOperatorEnum ComparisonOperator;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public int ComparisonValue;
                
                public enum BindingConversionComparisonOperatorEnum : sbyte
                {
                    Equality,
                    Inequality,
                    LessThan,
                    LessThanOrEqual,
                    GreaterThanOrEqual,
                    GreaterThan
                }
            }
            
            [TagStructure(Size = 0x20)]
            public class StaticDataTable : TagStructure
            {
                public StringId Name;
                // a component on this screen that gets replaced in simulation with this data table
                public StringId MockDataForComponent;
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
            
            [TagStructure(Size = 0x110)]
            public class Expression : TagStructure
            {
                public StringId Name;
                [TagField(Length = 256)]
                public string ExpressionText;
                public List<ExpressionStep> Steps;
                
                [TagStructure(Size = 0xC)]
                public class ExpressionStep : TagStructure
                {
                    public StepType StepType1;
                    public StepOperator Operator;
                    public StepVariable Variable;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public float Value;
                    public StringId StringIdValue;
                    
                    public enum StepType : sbyte
                    {
                        Operator,
                        Variable,
                        RealValue,
                        StringIdValue
                    }
                    
                    public enum StepOperator : sbyte
                    {
                        Add,
                        Subtract,
                        Multiply,
                        Divide,
                        Negate,
                        Not,
                        LessThan,
                        LessThanOrEqualTo,
                        GreaterThan,
                        GreaterThanOrEqualTo,
                        Equals,
                        NotEquals,
                        And,
                        Or,
                        Mod
                    }
                    
                    public enum StepVariable : sbyte
                    {
                        VariableA,
                        VariableB,
                        VariableC,
                        VariableD,
                        VariableE,
                        VariableF,
                        VariableG,
                        VariableH
                    }
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class Encapsulatedproperties : TagStructure
            {
                public StringId EncapsulationName;
                public List<StaticDataColumn> Properties;
                
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
            }
        }
    }
}
