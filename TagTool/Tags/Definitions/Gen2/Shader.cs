using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "shader", Tag = "shad", Size = 0x5C)]
    public class Shader : TagStructure
    {
        [TagField(ValidTags = new [] { "stem" })]
        public CachedTag Template;
        public StringId MaterialName;
        public List<ShaderPropertiesBlock> RuntimeProperties;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public FlagsValue Flags;
        public List<GlobalShaderParameterBlock> Parameters;
        public List<ShaderPostprocessDefinitionNewBlock> PostprocessDefinition;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public List<PredictedResourceBlock> PredictedResources;
        [TagField(ValidTags = new [] { "slit" })]
        public CachedTag LightResponse;
        public ShaderLodBiasValue ShaderLodBias;
        public SpecularTypeValue SpecularType;
        public LightmapTypeValue LightmapType;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        public float LightmapSpecularBrightness;
        public float LightmapAmbientBias; // [-1,1]
        public List<LongBlock> PostprocessProperties;
        public float AddedDepthBiasOffset;
        public float AddedDepthBiasSlopeScale;
        
        [TagStructure(Size = 0x50)]
        public class ShaderPropertiesBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag DiffuseMap;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag LightmapEmissiveMap;
            public RealRgbColor LightmapEmissiveColor;
            public float LightmapEmissivePower;
            public float LightmapResolutionScale;
            public float LightmapHalfLife;
            public float LightmapDiffuseScale;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag AlphaTestMap;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag TranslucentMap;
            public RealRgbColor LightmapTransparentColor;
            public float LightmapTransparentAlpha;
            public float LightmapFoliageScale;
        }
        
        [Flags]
        public enum FlagsValue : ushort
        {
            Water = 1 << 0,
            SortFirst = 1 << 1,
            NoActiveCamo = 1 << 2
        }
        
        [TagStructure(Size = 0x28)]
        public class GlobalShaderParameterBlock : TagStructure
        {
            public StringId Name;
            public TypeValue Type;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Bitmap;
            public float ConstValue;
            public RealRgbColor ConstColor;
            public List<ShaderAnimationPropertyBlock> AnimationProperties;
            
            public enum TypeValue : short
            {
                Bitmap,
                Value,
                Color,
                Switch
            }
            
            [TagStructure(Size = 0x18)]
            public class ShaderAnimationPropertyBlock : TagStructure
            {
                public TypeValue Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId InputName;
                public StringId RangeName;
                public float TimePeriod; // sec
                public MappingFunctionBlock Function;
                
                public enum TypeValue : short
                {
                    BitmapScaleUniform,
                    BitmapScaleX,
                    BitmapScaleY,
                    BitmapScaleZ,
                    BitmapTranslationX,
                    BitmapTranslationY,
                    BitmapTranslationZ,
                    BitmapRotationAngle,
                    BitmapRotationAxisX,
                    BitmapRotationAxisY,
                    BitmapRotationAxisZ,
                    Value,
                    Color,
                    BitmapIndex
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
        
        [TagStructure(Size = 0x7C)]
        public class ShaderPostprocessDefinitionNewBlock : TagStructure
        {
            public int ShaderTemplateIndex;
            public List<ShaderPostprocessBitmapNewBlock> Bitmaps;
            public List<Pixel32Block> PixelConstants;
            public List<RealVector4dBlock> VertexConstants;
            public List<ShaderPostprocessLevelOfDetailNewBlock> LevelsOfDetail;
            public List<TagBlockIndexGen2Block> Layers;
            public List<TagBlockIndexGen2Block> Passes;
            public List<ShaderPostprocessImplementationNewBlock> Implementations;
            public List<ShaderPostprocessOverlayNewBlock> Overlays;
            public List<ShaderPostprocessOverlayReferenceNewBlock> OverlayReferences;
            public List<ShaderPostprocessAnimatedParameterNewBlock> AnimatedParameters;
            public List<ShaderPostprocessAnimatedParameterReferenceNewBlock> AnimatedParameterReferences;
            public List<ShaderPostprocessBitmapPropertyBlock> BitmapProperties;
            public List<ShaderPostprocessColorPropertyBlock> ColorProperties;
            public List<ShaderPostprocessValuePropertyBlock> ValueProperties;
            public List<ShaderPostprocessLevelOfDetailBlock> OldLevelsOfDetail;
            
            [TagStructure(Size = 0xC)]
            public class ShaderPostprocessBitmapNewBlock : TagStructure
            {
                [TagField(Flags = TagFieldFlags.Short)]
                public CachedTag Bitmap;
                public int BitmapIndex;
                public float LogBitmapDimension;
            }
            
            [TagStructure(Size = 0x4)]
            public class Pixel32Block : TagStructure
            {
                public ArgbColor Color;
            }
            
            [TagStructure(Size = 0x10)]
            public class RealVector4dBlock : TagStructure
            {
                public RealVector3d Vector3;
                public float W;
            }
            
            [TagStructure(Size = 0x6)]
            public class ShaderPostprocessLevelOfDetailNewBlock : TagStructure
            {
                public int AvailableLayerFlags;
                public TagBlockIndexGen2 Layers;
            }
            
            [TagStructure(Size = 0xA)]
            public class ShaderPostprocessImplementationNewBlock : TagStructure
            {
                public TagBlockIndexGen2 BitmapTransforms;
                public TagBlockIndexGen2 RenderStates;
                public TagBlockIndexGen2 TextureStates;
                public TagBlockIndexGen2 PixelConstants;
                public TagBlockIndexGen2 VertexConstants;
            }
            
            [TagStructure(Size = 0x14)]
            public class ShaderPostprocessOverlayNewBlock : TagStructure
            {
                public StringId InputName;
                public StringId RangeName;
                public float TimePeriodInSeconds;
                public ScalarFunctionStructBlock Function;
                
                [TagStructure(Size = 0x8)]
                public class ScalarFunctionStructBlock : TagStructure
                {
                    public MappingFunctionBlock Function;
                    
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
            
            [TagStructure(Size = 0x4)]
            public class ShaderPostprocessOverlayReferenceNewBlock : TagStructure
            {
                public short OverlayIndex;
                public short TransformIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class ShaderPostprocessAnimatedParameterNewBlock : TagStructure
            {
                public TagBlockIndexGen2 OverlayReferences;
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderPostprocessAnimatedParameterReferenceNewBlock : TagStructure
            {
                public sbyte Unknown0;
                public sbyte Unknown1;
                public sbyte Unknown2;
                public sbyte ParameterIndex;
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderPostprocessBitmapPropertyBlock : TagStructure
            {
                public short BitmapIndex;
                public short AnimatedParameterIndex;
            }
            
            [TagStructure(Size = 0xC)]
            public class ShaderPostprocessColorPropertyBlock : TagStructure
            {
                public RealRgbColor Color;
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderPostprocessValuePropertyBlock : TagStructure
            {
                public float Value;
            }
            
            [TagStructure(Size = 0x98)]
            public class ShaderPostprocessLevelOfDetailBlock : TagStructure
            {
                public float ProjectedHeightPercentage;
                public int AvailableLayers;
                public List<ShaderPostprocessLayerBlock> Layers;
                public List<ShaderPostprocessPassBlock> Passes;
                public List<ShaderPostprocessImplementationBlock> Implementations;
                public List<ShaderPostprocessBitmapBlock> Bitmaps;
                public List<ShaderPostprocessBitmapTransformBlock> BitmapTransforms;
                public List<ShaderPostprocessValueBlock> Values;
                public List<ShaderPostprocessColorBlock> Colors;
                public List<ShaderPostprocessBitmapTransformOverlayBlock> BitmapTransformOverlays;
                public List<ShaderPostprocessValueOverlayBlock> ValueOverlays;
                public List<ShaderPostprocessColorOverlayBlock> ColorOverlays;
                public List<ShaderPostprocessVertexShaderConstantBlock> VertexShaderConstants;
                public ShaderGpuStateStructBlock GpuState;
                
                [TagStructure(Size = 0x2)]
                public class ShaderPostprocessLayerBlock : TagStructure
                {
                    public TagBlockIndexGen2 Passes;
                }
                
                [TagStructure(Size = 0xA)]
                public class ShaderPostprocessPassBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "spas" })]
                    public CachedTag ShaderPass;
                    public TagBlockIndexGen2 Implementations;
                }
                
                [TagStructure(Size = 0x2C)]
                public class ShaderPostprocessImplementationBlock : TagStructure
                {
                    public ShaderGpuStateReferenceStructBlock GpuConstantState;
                    public ShaderGpuStateReferenceStructBlock1 GpuVolatileState;
                    public TagBlockIndexGen2 BitmapParameters;
                    public TagBlockIndexGen2 BitmapTransforms;
                    public TagBlockIndexGen2 ValueParameters;
                    public TagBlockIndexGen2 ColorParameters;
                    public TagBlockIndexGen2 BitmapTransformOverlays;
                    public TagBlockIndexGen2 ValueOverlays;
                    public TagBlockIndexGen2 ColorOverlays;
                    public TagBlockIndexGen2 VertexShaderConstants;
                    
                    [TagStructure(Size = 0xE)]
                    public class ShaderGpuStateReferenceStructBlock : TagStructure
                    {
                        public TagBlockIndexGen2 RenderStates;
                        public TagBlockIndexGen2 TextureStageStates;
                        public TagBlockIndexGen2 RenderStateParameters;
                        public TagBlockIndexGen2 TextureStageParameters;
                        public TagBlockIndexGen2 Textures;
                        public TagBlockIndexGen2 VnConstants;
                        public TagBlockIndexGen2 CnConstants;
                    }
                    
                    [TagStructure(Size = 0xE)]
                    public class ShaderGpuStateReferenceStructBlock1 : TagStructure
                    {
                        public TagBlockIndexGen2 RenderStates;
                        public TagBlockIndexGen2 TextureStageStates;
                        public TagBlockIndexGen2 RenderStateParameters;
                        public TagBlockIndexGen2 TextureStageParameters;
                        public TagBlockIndexGen2 Textures;
                        public TagBlockIndexGen2 VnConstants;
                        public TagBlockIndexGen2 CnConstants;
                    }
                }
                
                [TagStructure(Size = 0xA)]
                public class ShaderPostprocessBitmapBlock : TagStructure
                {
                    public sbyte ParameterIndex;
                    public sbyte Flags;
                    public int BitmapGroupIndex;
                    public float LogBitmapDimension;
                }
                
                [TagStructure(Size = 0x6)]
                public class ShaderPostprocessBitmapTransformBlock : TagStructure
                {
                    public sbyte ParameterIndex;
                    public sbyte BitmapTransformIndex;
                    public float Value;
                }
                
                [TagStructure(Size = 0x5)]
                public class ShaderPostprocessValueBlock : TagStructure
                {
                    public sbyte ParameterIndex;
                    public float Value;
                }
                
                [TagStructure(Size = 0xD)]
                public class ShaderPostprocessColorBlock : TagStructure
                {
                    public sbyte ParameterIndex;
                    public RealRgbColor Color;
                }
                
                [TagStructure(Size = 0x17)]
                public class ShaderPostprocessBitmapTransformOverlayBlock : TagStructure
                {
                    public sbyte ParameterIndex;
                    public sbyte TransformIndex;
                    public sbyte AnimationPropertyType;
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public ScalarFunctionStructBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class ScalarFunctionStructBlock : TagStructure
                    {
                        public MappingFunctionBlock Function;
                        
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
                
                [TagStructure(Size = 0x15)]
                public class ShaderPostprocessValueOverlayBlock : TagStructure
                {
                    public sbyte ParameterIndex;
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public ScalarFunctionStructBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class ScalarFunctionStructBlock : TagStructure
                    {
                        public MappingFunctionBlock Function;
                        
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
                
                [TagStructure(Size = 0x15)]
                public class ShaderPostprocessColorOverlayBlock : TagStructure
                {
                    public sbyte ParameterIndex;
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public ColorFunctionStructBlock Function;
                    
                    [TagStructure(Size = 0x8)]
                    public class ColorFunctionStructBlock : TagStructure
                    {
                        public MappingFunctionBlock Function;
                        
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
                
                [TagStructure(Size = 0x12)]
                public class ShaderPostprocessVertexShaderConstantBlock : TagStructure
                {
                    public sbyte RegisterIndex;
                    public sbyte RegisterBank;
                    public float Data;
                    public float Data1;
                    public float Data2;
                    public float Data3;
                }
                
                [TagStructure(Size = 0x38)]
                public class ShaderGpuStateStructBlock : TagStructure
                {
                    public List<RenderStateBlock> RenderStates;
                    public List<TextureStageStateBlock> TextureStageStates;
                    public List<RenderStateParameterBlock> RenderStateParameters;
                    public List<TextureStageStateParameterBlock> TextureStageParameters;
                    public List<TextureBlock> Textures;
                    public List<VertexShaderConstantBlock> VnConstants;
                    public List<VertexShaderConstantBlock1> CnConstants;
                    
                    [TagStructure(Size = 0x5)]
                    public class RenderStateBlock : TagStructure
                    {
                        public sbyte StateIndex;
                        public int StateValue;
                    }
                    
                    [TagStructure(Size = 0x6)]
                    public class TextureStageStateBlock : TagStructure
                    {
                        public sbyte StateIndex;
                        public sbyte StageIndex;
                        public int StateValue;
                    }
                    
                    [TagStructure(Size = 0x3)]
                    public class RenderStateParameterBlock : TagStructure
                    {
                        public sbyte ParameterIndex;
                        public sbyte ParameterType;
                        public sbyte StateIndex;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class TextureStageStateParameterBlock : TagStructure
                    {
                        public sbyte ParameterIndex;
                        public sbyte ParameterType;
                        public sbyte StateIndex;
                        public sbyte StageIndex;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class TextureBlock : TagStructure
                    {
                        public sbyte StageIndex;
                        public sbyte ParameterIndex;
                        public sbyte GlobalTextureIndex;
                        public sbyte Flags;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class VertexShaderConstantBlock : TagStructure
                    {
                        public sbyte RegisterIndex;
                        public sbyte ParameterIndex;
                        public sbyte DestinationMask;
                        public sbyte ScaleByTextureStage;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class VertexShaderConstantBlock1 : TagStructure
                    {
                        public sbyte RegisterIndex;
                        public sbyte ParameterIndex;
                        public sbyte DestinationMask;
                        public sbyte ScaleByTextureStage;
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class PredictedResourceBlock : TagStructure
        {
            public TypeValue Type;
            public short ResourceIndex;
            public int TagIndex;
            
            public enum TypeValue : short
            {
                Bitmap,
                Sound,
                RenderModelGeometry,
                ClusterGeometry,
                ClusterInstancedGeometry,
                LightmapGeometryObjectBuckets,
                LightmapGeometryInstanceBuckets,
                LightmapClusterBitmaps,
                LightmapInstanceBitmaps
            }
        }
        
        public enum ShaderLodBiasValue : short
        {
            None,
            _4xSize,
            _2xSize,
            _12Size,
            _14Size,
            Never,
            Cinematic
        }
        
        public enum SpecularTypeValue : short
        {
            None,
            Default,
            Dull,
            Shiny
        }
        
        public enum LightmapTypeValue : short
        {
            Diffuse,
            DefaultSpecular,
            DullSpecular,
            ShinySpecular
        }
        
        [TagStructure(Size = 0x4)]
        public class LongBlock : TagStructure
        {
            public int BitmapGroupIndex;
        }
    }
}

