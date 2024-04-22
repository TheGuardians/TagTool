using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "material_shader", Tag = "mats", Size = 0x58)]
    public class MaterialShader : TagStructure
    {
        public MaterialShaderFlags Flags;
        public List<MaterialShaderSourceFileBlock> SourceShaderFiles;
        public List<CompiledEffectsBlock> CompiledEffects;
        public int SourceShaderHash;
        public int CompiledShaderHash;
        [TagField(ValidTags = new [] { "mtsb" })]
        public CachedTag ShaderBank;
        public List<MaterialVertexShaderEntryPointBlockStruct> VertexShaderEntryPoints;
        public List<CompiledPixelShaderRefererenceBlockStruct> PixelShaderEntryPoints;
        public List<MaterialShaderParameterBlock> MaterialParameters;
        
        [Flags]
        public enum MaterialShaderFlags : uint
        {
            IsDistortion = 1 << 0,
            IsDecal = 1 << 1,
            BlendedMaterials = 1 << 2,
            NoPhysicsMaterial = 1 << 3,
            IsVolumeFog = 1 << 4,
            IsWater = 1 << 5,
            IsWaterfall = 1 << 6,
            IsHologram = 1 << 7,
            IsBlendedHologram = 1 << 8,
            IsEmblem = 1 << 9,
            BlendedMaterials2 = 1 << 10,
            BlendedMaterials3 = 1 << 11,
            IsAlphaClip = 1 << 12,
            IsLightableTransparent = 1 << 13
        }
        
        [TagStructure(Size = 0x114)]
        public class MaterialShaderSourceFileBlock : TagStructure
        {
            [TagField(Length = 256)]
            public string ShaderPath;
            public byte[] ShaderData;
        }
        
        [TagStructure(Size = 0x14)]
        public class CompiledEffectsBlock : TagStructure
        {
            public byte[] CompiledEffectData;
        }
        
        [TagStructure(Size = 0xC)]
        public class MaterialVertexShaderEntryPointBlockStruct : TagStructure
        {
            public List<CompiledVertexShaderRefererenceBlockStruct> VertexTypeShaderIndices;
            
            [TagStructure(Size = 0x8)]
            public class CompiledVertexShaderRefererenceBlockStruct : TagStructure
            {
                public int Hash;
                public int Index;
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class CompiledPixelShaderRefererenceBlockStruct : TagStructure
        {
            public int Hash;
            public int Index;
        }
        
        [TagStructure(Size = 0xA8)]
        public class MaterialShaderParameterBlock : TagStructure
        {
            public StringId ParameterName;
            public MaterialShaderParameterTypeEnum ParameterType;
            public int ParameterIndex;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Bitmap;
            public StringId BitmapPath;
            public RealArgbColor Color;
            public float Real;
            public RealVector3d Vector;
            public int IntBool;
            public ushort BitmapFlags;
            public ushort BitmapFilterMode;
            public ushort BitmapAddressMode;
            public ushort BitmapAddressModeX;
            public ushort BitmapAddressModeY;
            public ushort BitmapSharpenMode;
            public byte BitmapExternMode;
            public byte BitmapMinMipmap;
            public byte BitmapMaxMipmap;
            public byte RenderPhasesUsed;
            public List<MaterialShaderFunctionParameterBlock> FunctionParameters;
            public byte[] DisplayName;
            public byte[] DisplayGroup;
            public byte[] DisplayHelpText;
            public float DisplayMinimum;
            public float DisplayMaximum;
            public byte RegisterIndex;
            public byte RegisterOffset;
            public byte RegisterCount;
            public RegisterSetEnum RegisterSet;
            
            public enum MaterialShaderParameterTypeEnum : int
            {
                Bitmap,
                Real,
                Int,
                Bool,
                Color
            }
            
            public enum RegisterSetEnum : sbyte
            {
                Bool,
                Int,
                Float,
                Sampler,
                VertexBool,
                VertexInt,
                VertexFloat,
                VertexSampler
            }
            
            [TagStructure(Size = 0x2C)]
            public class MaterialShaderFunctionParameterBlock : TagStructure
            {
                public MaterialAnimatedParameterTypeEnum Type;
                public StringId InputName;
                public StringId RangeName;
                public MaterialfunctionOutputModEnum OutputModifier;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId OutputModifierInput;
                public float TimePeriod; // seconds
                public MappingFunction Function;
                
                public enum MaterialAnimatedParameterTypeEnum : int
                {
                    Value,
                    Color,
                    ScaleUniform,
                    ScaleU,
                    ScaleV,
                    OffsetU,
                    OffsetV,
                    FrameIndex,
                    Alpha
                }
                
                public enum MaterialfunctionOutputModEnum : sbyte
                {
                    Unknown,
                    Add,
                    Multiply
                }
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
        }
    }
}
