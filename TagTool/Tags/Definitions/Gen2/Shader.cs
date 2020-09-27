using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "shader", Tag = "shad", Size = 0x80)]
    public class Shader : TagStructure
    {
        public CachedTag Template;
        public StringId MaterialName;
        public List<ShaderProperties> RuntimeProperties;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        public FlagsValue Flags;
        public List<ShaderParameter> Parameters;
        public List<ShaderPostprocessDefinitionNew> PostprocessDefinition;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding2;
        public List<PredictedResource> PredictedResources;
        public CachedTag LightResponse;
        public ShaderLodBiasValue ShaderLodBias;
        public SpecularTypeValue SpecularType;
        public LightmapTypeValue LightmapType;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding3;
        public float LightmapSpecularBrightness;
        public float LightmapAmbientBias; // [-1,1]
        public List<PostprocessPropertiesBlock> PostprocessProperties;
        public float AddedDepthBiasOffset;
        public float AddedDepthBiasSlopeScale;
        
        [TagStructure(Size = 0x70)]
        public class ShaderProperties : TagStructure
        {
            public CachedTag DiffuseMap;
            public CachedTag LightmapEmissiveMap;
            public RealRgbColor LightmapEmissiveColor;
            public float LightmapEmissivePower;
            public float LightmapResolutionScale;
            public float LightmapHalfLife;
            public float LightmapDiffuseScale;
            public CachedTag AlphaTestMap;
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
        
        [TagStructure(Size = 0x34)]
        public class ShaderParameter : TagStructure
        {
            public StringId Name;
            public TypeValue Type;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public CachedTag Bitmap;
            public float ConstValue;
            public RealRgbColor ConstColor;
            public List<ShaderAnimationProperty> AnimationProperties;
            
            public enum TypeValue : short
            {
                Bitmap,
                Value,
                Color,
                Switch
            }
            
            [TagStructure(Size = 0x1C)]
            public class ShaderAnimationProperty : TagStructure
            {
                public TypeValue Type;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public StringId InputName;
                public StringId RangeName;
                public float TimePeriod; // sec
                /// <summary>
                /// FUNCTION
                /// </summary>
                public FunctionDefinition Function;
                
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
        
        [TagStructure(Size = 0xB8)]
        public class ShaderPostprocessDefinitionNew : TagStructure
        {
            public int ShaderTemplateIndex;
            public List<ShaderPostprocessBitmapNew> Bitmaps;
            public List<Pixel32> PixelConstants;
            public List<RealVector4d> VertexConstants;
            public List<ShaderPostprocessLevelOfDetailNew> LevelsOfDetail;
            public List<TagBlockIndex> Layers;
            public List<TagBlockIndex> Passes;
            public List<ShaderPostprocessImplementationNew> Implementations;
            public List<ShaderPostprocessOverlayNew> Overlays;
            public List<ShaderPostprocessOverlayReference> OverlayReferences;
            public List<ShaderPostprocessAnimatedParameter> AnimatedParameters;
            public List<ShaderPostprocessAnimatedParameterReference> AnimatedParameterReferences;
            public List<ShaderPostprocessBitmapProperty> BitmapProperties;
            public List<ShaderPostprocessColorProperty> ColorProperties;
            public List<ShaderPostprocessValueProperty> ValueProperties;
            public List<ShaderPostprocessLevelOfDetail> OldLevelsOfDetail;
            
            [TagStructure(Size = 0xC)]
            public class ShaderPostprocessBitmapNew : TagStructure
            {
                public int BitmapGroup;
                public int BitmapIndex;
                public float LogBitmapDimension;
            }
            
            [TagStructure(Size = 0x4)]
            public class Pixel32 : TagStructure
            {
                public ArgbColor Color;
            }
            
            [TagStructure(Size = 0x10)]
            public class RealVector4d : TagStructure
            {
                public RealVector3d Vector3;
                public float W;
            }
            
            [TagStructure(Size = 0x6)]
            public class ShaderPostprocessLevelOfDetailNew : TagStructure
            {
                public int AvailableLayerFlags;
                public TagBlockIndex Layers;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndex : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x2)]
            public class TagBlockIndex : TagStructure
            {
                public TagBlockIndex Indices;
            }
            
            [TagStructure(Size = 0xA)]
            public class ShaderPostprocessImplementationNew : TagStructure
            {
                public TagBlockIndex BitmapTransforms;
                public TagBlockIndex RenderStates;
                public TagBlockIndex TextureStates;
                public TagBlockIndex PixelConstants;
                public TagBlockIndex VertexConstants;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndex : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class ShaderPostprocessOverlayNew : TagStructure
            {
                public StringId InputName;
                public StringId RangeName;
                public float TimePeriodInSeconds;
                public FunctionDefinition Function;
                
                [TagStructure(Size = 0xC)]
                public class FunctionDefinition : TagStructure
                {
                    public FunctionDefinition Function;
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderPostprocessOverlayReference : TagStructure
            {
                public short OverlayIndex;
                public short TransformIndex;
            }
            
            [TagStructure(Size = 0x2)]
            public class ShaderPostprocessAnimatedParameter : TagStructure
            {
                public TagBlockIndex OverlayReferences;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndex : TagStructure
                {
                    public short BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderPostprocessAnimatedParameterReference : TagStructure
            {
                [TagField(Flags = Padding, Length = 3)]
                public byte[] Unknown1;
                public sbyte ParameterIndex;
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderPostprocessBitmapProperty : TagStructure
            {
                public short BitmapIndex;
                public short AnimatedParameterIndex;
            }
            
            [TagStructure(Size = 0xC)]
            public class ShaderPostprocessColorProperty : TagStructure
            {
                public RealRgbColor Color;
            }
            
            [TagStructure(Size = 0x4)]
            public class ShaderPostprocessValueProperty : TagStructure
            {
                public float Value;
            }
            
            [TagStructure(Size = 0xE0)]
            public class ShaderPostprocessLevelOfDetail : TagStructure
            {
                public float ProjectedHeightPercentage;
                public int AvailableLayers;
                public List<ShaderPostprocessLayer> Layers;
                public List<ShaderPostprocessPass> Passes;
                public List<ShaderPostprocessImplementation> Implementations;
                public List<ShaderPostprocessBitmap> Bitmaps;
                public List<ShaderPostprocessBitmapTransform> BitmapTransforms;
                public List<ShaderPostprocessValue> Values;
                public List<ShaderPostprocessColor> Colors;
                public List<ShaderPostprocessBitmapTransformOverlay> BitmapTransformOverlays;
                public List<ShaderPostprocessValueOverlay> ValueOverlays;
                public List<ShaderPostprocessColorOverlay> ColorOverlays;
                public List<ShaderVertexShaderConstant> VertexShaderConstants;
                public ShaderGpuState GpuState;
                
                [TagStructure(Size = 0x2)]
                public class ShaderPostprocessLayer : TagStructure
                {
                    public TagBlockIndex Passes;
                    
                    [TagStructure(Size = 0x2)]
                    public class TagBlockIndex : TagStructure
                    {
                        public short BlockIndexData;
                    }
                }
                
                [TagStructure(Size = 0x12)]
                public class ShaderPostprocessPass : TagStructure
                {
                    public CachedTag ShaderPass;
                    public TagBlockIndex Implementations;
                    
                    [TagStructure(Size = 0x2)]
                    public class TagBlockIndex : TagStructure
                    {
                        public short BlockIndexData;
                    }
                }
                
                [TagStructure(Size = 0x2C)]
                public class ShaderPostprocessImplementation : TagStructure
                {
                    public ShaderGpuStateReference GpuConstantState;
                    public ShaderGpuStateReference GpuVolatileState;
                    public TagBlockIndex BitmapParameters;
                    public TagBlockIndex BitmapTransforms;
                    public TagBlockIndex ValueParameters;
                    public TagBlockIndex ColorParameters;
                    public TagBlockIndex BitmapTransformOverlays;
                    public TagBlockIndex ValueOverlays;
                    public TagBlockIndex ColorOverlays;
                    public TagBlockIndex VertexShaderConstants;
                    
                    [TagStructure(Size = 0xE)]
                    public class ShaderGpuStateReference : TagStructure
                    {
                        public TagBlockIndex RenderStates;
                        public TagBlockIndex TextureStageStates;
                        public TagBlockIndex RenderStateParameters;
                        public TagBlockIndex TextureStageParameters;
                        public TagBlockIndex Textures;
                        public TagBlockIndex VnConstants;
                        public TagBlockIndex CnConstants;
                        
                        [TagStructure(Size = 0x2)]
                        public class TagBlockIndex : TagStructure
                        {
                            public short BlockIndexData;
                        }
                    }
                    
                    [TagStructure(Size = 0x2)]
                    public class TagBlockIndex : TagStructure
                    {
                        public short BlockIndexData;
                    }
                }
                
                [TagStructure(Size = 0xA)]
                public class ShaderPostprocessBitmap : TagStructure
                {
                    public sbyte ParameterIndex;
                    public sbyte Flags;
                    public int BitmapGroupIndex;
                    public float LogBitmapDimension;
                }
                
                [TagStructure(Size = 0x6)]
                public class ShaderPostprocessBitmapTransform : TagStructure
                {
                    public sbyte ParameterIndex;
                    public sbyte BitmapTransformIndex;
                    public float Value;
                }
                
                [TagStructure(Size = 0x5)]
                public class ShaderPostprocessValue : TagStructure
                {
                    public sbyte ParameterIndex;
                    public float Value;
                }
                
                [TagStructure(Size = 0xD)]
                public class ShaderPostprocessColor : TagStructure
                {
                    public sbyte ParameterIndex;
                    public RealRgbColor Color;
                }
                
                [TagStructure(Size = 0x1B)]
                public class ShaderPostprocessBitmapTransformOverlay : TagStructure
                {
                    public sbyte ParameterIndex;
                    public sbyte TransformIndex;
                    public sbyte AnimationPropertyType;
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public FunctionDefinition Function;
                    
                    [TagStructure(Size = 0xC)]
                    public class FunctionDefinition : TagStructure
                    {
                        public FunctionDefinition Function;
                    }
                }
                
                [TagStructure(Size = 0x19)]
                public class ShaderPostprocessValueOverlay : TagStructure
                {
                    public sbyte ParameterIndex;
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public FunctionDefinition Function;
                    
                    [TagStructure(Size = 0xC)]
                    public class FunctionDefinition : TagStructure
                    {
                        public FunctionDefinition Function;
                    }
                }
                
                [TagStructure(Size = 0x19)]
                public class ShaderPostprocessColorOverlay : TagStructure
                {
                    public sbyte ParameterIndex;
                    public StringId InputName;
                    public StringId RangeName;
                    public float TimePeriodInSeconds;
                    public FunctionDefinition Function;
                    
                    [TagStructure(Size = 0xC)]
                    public class FunctionDefinition : TagStructure
                    {
                        public FunctionDefinition Function;
                    }
                }
                
                [TagStructure(Size = 0x12)]
                public class ShaderVertexShaderConstant : TagStructure
                {
                    public sbyte RegisterIndex;
                    public sbyte RegisterBank;
                    public float Data;
                    public float Data1;
                    public float Data2;
                    public float Data3;
                }
                
                [TagStructure(Size = 0x54)]
                public class ShaderGpuState : TagStructure
                {
                    public List<RenderState> RenderStates;
                    public List<TextureStageState> TextureStageStates;
                    public List<RenderStateParameter> RenderStateParameters;
                    public List<TextureStageStateParameter> TextureStageParameters;
                    public List<Texture> Textures;
                    public List<VertexShaderConstant> VnConstants;
                    public List<VertexShaderConstant> CnConstants;
                    
                    [TagStructure(Size = 0x5)]
                    public class RenderState : TagStructure
                    {
                        public sbyte StateIndex;
                        public int StateValue;
                    }
                    
                    [TagStructure(Size = 0x6)]
                    public class TextureStageState : TagStructure
                    {
                        public sbyte StateIndex;
                        public sbyte StageIndex;
                        public int StateValue;
                    }
                    
                    [TagStructure(Size = 0x3)]
                    public class RenderStateParameter : TagStructure
                    {
                        public sbyte ParameterIndex;
                        public sbyte ParameterType;
                        public sbyte StateIndex;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class TextureStageStateParameter : TagStructure
                    {
                        public sbyte ParameterIndex;
                        public sbyte ParameterType;
                        public sbyte StateIndex;
                        public sbyte StageIndex;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class Texture : TagStructure
                    {
                        public sbyte StageIndex;
                        public sbyte ParameterIndex;
                        public sbyte GlobalTextureIndex;
                        public sbyte Flags;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class VertexShaderConstant : TagStructure
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
        public class PredictedResource : TagStructure
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
        public class PostprocessPropertiesBlock : TagStructure
        {
            public int BitmapGroupIndex;
        }
    }
}

