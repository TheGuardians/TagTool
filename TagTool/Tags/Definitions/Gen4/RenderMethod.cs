using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "render_method", Tag = "rm  ", Size = 0x64)]
    public class RenderMethod : TagStructure
    {
        [TagField(ValidTags = new [] { "rmdf" })]
        public CachedTag Definition;
        [TagField(ValidTags = new [] { "rm  " })]
        public CachedTag Reference;
        public List<ShortBlock> Options;
        public List<RenderMethodParameterBlock> Parameters;
        public List<RenderMethodPostprocessBlock> Postprocess;
        public int IsTemplate;
        public GlobalRenderMethodLockOptionFlagsDefintion LockedOptions;
        public List<RenderMethodLockedParameterBlock> LockedParameters;
        public GlobalRenderMethodFlagsDefintion ShaderFlags;
        public GlobalSortLayerEnumDefintion SortLayer;
        public sbyte Version;
        public int CustomFogSettingIndex;
        public int PredictionAtomIndex;
        
        [Flags]
        public enum GlobalRenderMethodLockOptionFlagsDefintion : uint
        {
            Option0 = 1 << 0,
            Option1 = 1 << 1,
            Option2 = 1 << 2,
            Option3 = 1 << 3,
            Option4 = 1 << 4,
            Option5 = 1 << 5,
            Option6 = 1 << 6,
            Option7 = 1 << 7,
            Option8 = 1 << 8,
            Option9 = 1 << 9,
            Option10 = 1 << 10,
            Option11 = 1 << 11,
            Option12 = 1 << 12,
            Option13 = 1 << 13,
            Option14 = 1 << 14,
            Option15 = 1 << 15,
            Option16 = 1 << 16,
            Option17 = 1 << 17,
            Option18 = 1 << 18,
            Option19 = 1 << 19,
            Option20 = 1 << 20,
            Option21 = 1 << 21,
            Option22 = 1 << 22,
            Option23 = 1 << 23,
            Option24 = 1 << 24,
            Option25 = 1 << 25,
            Option26 = 1 << 26,
            Option27 = 1 << 27,
            Option28 = 1 << 28,
            Option29 = 1 << 29,
            Option30 = 1 << 30,
            Option31 = 1u << 31
        }
        
        [Flags]
        public enum GlobalRenderMethodFlagsDefintion : ushort
        {
            DonTFogMe = 1 << 0,
            UseCustomSetting = 1 << 1,
            CalculateZCamera = 1 << 2,
            NeverRenderForShields = 1 << 3,
            OnlyRenderForShields = 1 << 4
        }
        
        public enum GlobalSortLayerEnumDefintion : sbyte
        {
            Invalid,
            PrePass,
            Normal,
            PostPass
        }
        
        [TagStructure(Size = 0x2)]
        public class ShortBlock : TagStructure
        {
            public short Short;
        }
        
        [TagStructure(Size = 0x3C)]
        public class RenderMethodParameterBlock : TagStructure
        {
            public StringId ParameterName;
            public RenderMethodParameterTypeEnum ParameterType;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Bitmap;
            public float Real;
            public int IntBool;
            public ushort BitmapFlags;
            public ushort BitmapFilterMode;
            public ushort BitmapAddressMode;
            public ushort BitmapAddressModeX;
            public ushort BitmapAddressModeY;
            public short BitmapAnisotropyAmount;
            public short BitmapExternRttMode;
            public ushort BitmapSharpenMode;
            public List<RenderMethodAnimatedParameterBlock> AnimatedParameters;
            
            public enum RenderMethodParameterTypeEnum : int
            {
                Bitmap,
                Color,
                Real,
                Int,
                Bool,
                ArgbColor
            }
            
            [TagStructure(Size = 0x24)]
            public class RenderMethodAnimatedParameterBlock : TagStructure
            {
                public RenderMethodAnimatedParameterTypeEnum Type;
                public StringId InputName;
                public StringId RangeName;
                public float TimePeriod; // seconds
                public MappingFunction Function;
                
                public enum RenderMethodAnimatedParameterTypeEnum : int
                {
                    Value,
                    Color,
                    ScaleUniform,
                    ScaleX,
                    ScaleY,
                    TranslationX,
                    TranslationY,
                    FrameIndex,
                    Alpha
                }
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
        }
        
        [TagStructure(Size = 0xAC)]
        public class RenderMethodPostprocessBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "rmt2" })]
            public CachedTag ShaderTemplate;
            public List<RenderMethodPostprocessTextureBlock> Textures;
            public List<RealVector4dBlock> RealVectors;
            public List<IntBlock> IntConstants;
            public int BoolConstants;
            public List<TagBlockIndexBlock> EntryPoints;
            public List<RenderMethodPostprocessPassBlock> Passes;
            public List<RenderMethodRoutingInfoBlock> RoutingInfo;
            public List<RenderMethodAnimatedParameterBlock> Overlays;
            public int BlendMode;
            public int Flags;
            public int ImSoFiredPad;
            [TagField(Length = 28)]
            public RuntimeQueryableProperties[]  RuntimeQueryablePropertiesTable;
            
            [TagStructure(Size = 0x18)]
            public class RenderMethodPostprocessTextureBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag BitmapReference;
                public short BitmapIndex;
                public byte AddressMode;
                public byte FilterMode;
                public byte ExternTextureMode;
                public sbyte TextureTransformConstantIndex;
                public TagBlockIndexStruct TextureTransformOverlayIndices;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndexStruct : TagStructure
                {
                    // divide by 1024 for count, remainder is start index
                    public ushort BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class RealVector4dBlock : TagStructure
            {
                public RealVector3d Vector;
                public float VectorW;
            }
            
            [TagStructure(Size = 0x4)]
            public class IntBlock : TagStructure
            {
                public int IntValue;
            }
            
            [TagStructure(Size = 0x2)]
            public class TagBlockIndexBlock : TagStructure
            {
                public TagBlockIndexStruct BlockIndex;
                
                [TagStructure(Size = 0x2)]
                public class TagBlockIndexStruct : TagStructure
                {
                    // divide by 1024 for count, remainder is start index
                    public ushort BlockIndexData;
                }
            }
            
            [TagStructure(Size = 0x6)]
            public class RenderMethodPostprocessPassBlock : TagStructure
            {
                // divide by 1024 for count, remainder is start index
                public ushort Bitmaps;
                // divide by 1024 for count, remainder is start index
                public ushort VertexReal;
                // divide by 1024 for count, remainder is start index
                public ushort PixelReal;
            }
            
            [TagStructure(Size = 0x4)]
            public class RenderMethodRoutingInfoBlock : TagStructure
            {
                // D3D constant index or sampler index
                public ushort DestinationIndex;
                // into constant tables below, unless this is an extern parameter
                public byte SourceIndex;
                // bitmap flags or shader component mask
                public byte TypeSpecific;
            }
            
            [TagStructure(Size = 0x24)]
            public class RenderMethodAnimatedParameterBlock : TagStructure
            {
                public RenderMethodAnimatedParameterTypeEnum Type;
                public StringId InputName;
                public StringId RangeName;
                public float TimePeriod; // seconds
                public MappingFunction Function;
                
                public enum RenderMethodAnimatedParameterTypeEnum : int
                {
                    Value,
                    Color,
                    ScaleUniform,
                    ScaleX,
                    ScaleY,
                    TranslationX,
                    TranslationY,
                    FrameIndex,
                    Alpha
                }
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
            
            [TagStructure(Size = 0x2)]
            public class RuntimeQueryableProperties : TagStructure
            {
                public short Index;
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class RenderMethodLockedParameterBlock : TagStructure
        {
            public StringId ParameterName;
            public RenderMethodParameterTypeEnum ParameterType;
            public uint AnimatedParameterFlags;
            
            public enum RenderMethodParameterTypeEnum : int
            {
                Bitmap,
                Color,
                Real,
                Int,
                Bool,
                ArgbColor
            }
        }
    }
}
