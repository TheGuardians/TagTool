using System;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Shaders;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using static TagTool.Tags.Definitions.RenderMethodTemplate;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "render_method", Tag = "rm  ", Size = 0x40, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "render_method", Tag = "rm  ", Size = 0x64, MinVersion = CacheVersion.HaloReach)]
    public class RenderMethod : TagStructure
    {
        public CachedTag BaseRenderMethod;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public CachedTag Reference;

        public List<RenderMethodDefinitionOptionIndex> RenderMethodDefinitionOptionIndices;
        public List<ImportDatum> ImportData;
        public List<ShaderProperty> ShaderProperties;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint IsTemplate;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public int LockedOptions;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<LockedParameter> LockedParameters;

        public RenderMethodRenderFlags RenderFlags;
        public SortingLayerValue SortingLayer;
        public byte Version;
        public int SkyAtmospherePropertiesIndex; // skya AtmosphereProperties block index
        public int Unknown2; // usually -1

        [TagStructure(Size = 0x2)]
        public class RenderMethodDefinitionOptionIndex : TagStructure
        {
            public short OptionIndex;
        }

        [TagStructure(Size = 0x24)]
        public class ShaderFunction : TagStructure
        {
            public FunctionType Type;

            [TagField(Flags = Label)]
            public StringId InputName;

            public StringId RangeName;

            public float TimePeriod;

            public TagFunction Function = new TagFunction { Data = new byte[0] };

            public enum FunctionType : int
            {
                Value,
                Color,
                ScaleUniform,
                ScaleX,
                ScaleY,
                TranslationX,
                TranslationY,
                FrameIndex,
                Alpha,
                // NEW (MS23 only)
                ChangeColorPrimary,
                ChangeColorSecondary,
                ChangeColorTertiary,
                ChangeColorQuaternary,
                ChangeColorQuinary,
            }
        }

        [TagStructure(Size = 0x3C)]
        public class ImportDatum : TagStructure
        {
            [TagField(Flags = Label)]
            public StringId Name;

            public RenderMethodOption.OptionBlock.OptionDataType Type;
            public CachedTag Bitmap;
            public float DefaultRealValue;
            public int DefaultIntBoolValue;
            public short SamplerFlags;
            public short SamplerFilterMode;
            public short DefaultAddressMode;
            public short AddressU;
            public short AddressV;
            // one of these is extern_mode
            public short Unknown9;
            public uint Unknown10;
            public List<ShaderFunction> Functions;
        }

        [TagStructure(Size = 0x84, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0xAC, MinVersion = CacheVersion.HaloReach)]
        public class ShaderProperty : TagStructure
        {
            public CachedTag Template;
            public List<TextureConstant> TextureConstants;
            public List<RealConstant> RealConstants;
            public List<uint> IntegerConstants;
            public uint BooleanConstants; // Each bit indicates true/false. SourceIndex = bit index
            public List<TagBlockIndex> EntryPoints; // Ranges of ParameterTables
            public List<ParameterTable> ParameterTables; // Ranges of Parameters by usage
            public List<ParameterMapping> Parameters; // Mapping of constants functions, and registers
            public List<ShaderFunction> Functions; // Functions for animated parameters
            public AlphaBlendModeValue AlphaBlendMode;
            public BlendModeFlags BlendFlags;
            public uint Unknown8; // unused?

            // Indices of constants. TODO: create an enum
            [TagField(Length = 8, MaxVersion = CacheVersion.HaloOnline700123)]
            public short[] QueryableProperties; 
            [TagField(Length = 0x1C, MinVersion = CacheVersion.HaloReach)]
            public short[] QueryablePropertiesReach;

            public enum AlphaBlendModeValue : uint
            {
                opaque = 0,
                additive = 1,
                multiply = 2,
                alpha_blend = 3,
                double_multiply = 4,
                pre_multiplied_alpha = 5,
                maximum = 6,
                multiply_add = 7,
                add_src_times_dstalpha = 8,
                add_src_times_srcalpha = 9,
                inv_alpha_blend = 10,
                separate_alpha_blend = 11,
                separate_alpha_blend_additive = 12
            }

            [Flags]
            public enum BlendModeFlags : uint
            {
                None = 0,
                Bit0 = 1 << 0,
                EnableAlphaTest = 1 << 1,
                SfxDistort_ForceAlphaBlend = 1 << 2 // added by saber
            }

            [TagStructure(Size = 0x18)]
            public class TextureConstant : TagStructure
            {
                [TagField(Flags = Label)]
                public CachedTag Bitmap;
                public short BitmapIndex;
                public PackedSamplerAddressMode SamplerAddressMode;

                [TagField(MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.Halo3Retail)]
                public SamplerFilterMode FilterModeH3;
                [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
                public SamplerFilterMode FilterMode;
                [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
                public PackedSamplerFilterMode FilterModeODST; // not sure if the anisotropy is used
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public PackedSamplerFilterMode FilterModeReach;

                public sbyte ExternMode;
                public sbyte XFormArgumentIndex;
                public TagBlockIndex Functions = new TagBlockIndex(); // Range of Functions

                public enum SamplerFilterMode : byte
                {
                    Trilinear,
                    Point,
                    Bilinear,
                    unused_00,
                    Anisotropic_2x,
                    unused_01,
                    Anisotropic_4x,
                    LightprobeTextureArray,
                    TextureArrayQuadlinear,
                    TextureArrayQuadanisotropic_2x
                }

                [TagStructure(Size = 0x1)]
                public class PackedSamplerFilterMode : TagStructure
                {
                    public SamplerFilterMode FilterMode { get => GetFilterMode(); set => SetFilterMode(value); }
                    public byte Anisotropy { get => GetAnisotropy(); set => SetAnisotropy(value); }

                    private SamplerFilterMode GetFilterMode() => (SamplerFilterMode)(FilterValue & 0xF);
                    private byte GetAnisotropy() => (byte)(FilterValue >> 4);
                    private void SetFilterMode(SamplerFilterMode u)
                    {
                        if ((byte)u > 0xFu) throw new System.Exception("Out of range");
                        var b = ((byte)GetAnisotropy() & 0xF) << 4;
                        FilterValue = (byte)((byte)u | b);
                    }
                    private void SetAnisotropy(byte v)
                    {
                        if ((byte)v > 0xFu) throw new System.Exception("Out of range");
                        var a = (byte)GetFilterMode();
                        var b = ((byte)v & 0xF) << 4;
                        FilterValue = (byte)(a | b);
                    }

                    public byte FilterValue;
                }

                public enum SamplerAddressModeEnum : byte
                {
                    Wrap,
                    Clamp,
                    Mirror,
                    BlackBorder,
                    MirrorOnce,
                    MirrorOnceBorder
                }

                [TagStructure(Size = 0x1)]
                public class PackedSamplerAddressMode : TagStructure
                {
                    public SamplerAddressModeEnum AddressU { get => GetU(); set => SetU(value); }
                    public SamplerAddressModeEnum AddressV { get => GetV(); set => SetV(value); }

                    private SamplerAddressModeEnum GetU() => (SamplerAddressModeEnum)(SamplerAddressUV & 0xF);
                    private SamplerAddressModeEnum GetV() => (SamplerAddressModeEnum)(SamplerAddressUV >> 4);
                    private void SetU(SamplerAddressModeEnum u)
                    {
                        if ((byte)u > 0xFu) throw new System.Exception("Out of range");
                        var a = (byte)u;
                        var b = ((byte)GetV() & 0xF) << 4;
                        SamplerAddressUV = (byte)(a | b);
                    }
                    private void SetV(SamplerAddressModeEnum v)
                    {
                        if ((byte)v > 0xFu) throw new System.Exception("Out of range");
                        var a = (byte)GetU();
                        var b = ((byte)v & 0xF) << 4;
                        SamplerAddressUV = (byte)(a | b);
                    }

                    public byte SamplerAddressUV;
                }
            }

            [TagStructure(Size = 0x10)]
            public class RealConstant : TagStructure
            {
                [TagField(Length = 4)]
                public float[] Values;

                private float[] _Values
                {
                    get
                    {
                        if (Values == null)
                            Values = new float[4];
                        return Values;
                    }
                }
                public float Arg0 { get => _Values[0]; set => _Values[0] = value; }
                public float Arg1 { get => _Values[1]; set => _Values[1] = value; }
                public float Arg2 { get => _Values[2]; set => _Values[2] = value; }
                public float Arg3 { get => _Values[3]; set => _Values[3] = value; }
            }

            [TagStructure(Size = 0x6)]
            public class ParameterTable : TagStructure
            {
                public TagBlockIndex Texture = new TagBlockIndex();
                public TagBlockIndex RealVertex = new TagBlockIndex();
                public TagBlockIndex RealPixel = new TagBlockIndex();
            }

            [TagStructure(Size = 0x4)]
            public class ParameterMapping : TagStructure
            {
                public enum RenderMethodExternalValue : byte
                {
                    None = 0x00,
                    Texture_Global_Target_Texaccum = 0x01,
                    Texture_Global_Target_Normal = 0x02,
                    Texture_Global_Target_Z = 0x03,
                    Texture_Global_Target_Shadow_Buffer1 = 0x04,
                    Texture_Global_Target_Shadow_Buffer2 = 0x05,
                    Texture_Global_Target_Shadow_Buffer3 = 0x06,
                    Texture_Global_Target_Shadow_Buffer4 = 0x07,
                    Texture_Global_Target_Texture_Camera = 0x08,
                    Texture_Global_Target_Reflection = 0x09,
                    Texture_Global_Target_Refraction = 0x0a,
                    Texture_Lightprobe_Texture = 0x0b,
                    Texture_Dominant_Light_Intensity_Map = 0x0c,
                    Texture_Unused1 = 0x0d,
                    Texture_Unused2 = 0x0e,
                    Object_Change_Color_Primary = 0x0f,
                    Object_Change_Color_Secondary = 0x10,
                    Object_Change_Color_Tertiary = 0x11,
                    Object_Change_Color_Quaternary = 0x12,
                    Object_Change_Color_Quinary = 0x13,
                    Object_Change_Color_Primary_Anim = 0x14,
                    Object_Change_Color_Secondary_Anim = 0x15,
                    Texture_Dynamic_Environment_Map_0 = 0x16,
                    Texture_Dynamic_Environment_Map_1 = 0x17,
                    Texture_Cook_Torrance_Cc0236 = 0x18,
                    Texture_Cook_Torrance_Dd0236 = 0x19,
                    Texture_Cook_Torrance_C78d78 = 0x1a,
                    Light_Dir_0 = 0x1b,
                    Light_Color_0 = 0x1c,
                    Light_Dir_1 = 0x1d,
                    Light_Color_1 = 0x1e,
                    Light_Dir_2 = 0x1f,
                    Light_Color_2 = 0x20,
                    Light_Dir_3 = 0x21,
                    Light_Color_3 = 0x22,
                    Texture_Unused_3 = 0x23,
                    Texture_Unused_4 = 0x24,
                    Texture_Unused_5 = 0x25,
                    Texture_Dynamic_Light_Gel_0 = 0x26,
                    Flat_Envmap_Matrix_X = 0x27,
                    Flat_Envmap_Matrix_Y = 0x28,
                    Flat_Envmap_Matrix_Z = 0x29,
                    Debug_Tint = 0x2a,
                    Screen_Constants = 0x2b,
                    Active_Camo_Distortion_Texture = 0x2c,
                    Scene_Ldr_Texture = 0x2d,
                    Scene_Hdr_Texture = 0x2e,
                    Water_Memory_Export_Address = 0x2f,
                    Tree_Animation_Timer = 0x30,
                    Emblem_Player_Shoulder_Texture = 0x31,
                    Emblem_Clan_Chest_Texture = 0x32,
                }
                public RenderMethodExternalValue RenderMethodExternal
                {
                    get => (RenderMethodExternalValue)SourceIndex;
                    set => SourceIndex = (byte)value;
                }

                public short RegisterIndex;
                public byte FunctionIndex;
                public byte SourceIndex;
            }
        }

        [TagStructure(Size = 0xC)]
        public class LockedParameter : TagStructure
        {
            public StringId Name;
            public RenderMethodOption.OptionBlock.OptionDataType Type;
            public uint Flags;
        }

        [Flags]
        public enum RenderMethodRenderFlags : ushort
        {
            None = 0,
            IgnoreFog = 1 << 0,
            UseSkyAtmosphereProperties = 1 << 1,
            UsesDepthCamera = 1 << 2,
            DisableWithShields = 1 << 3,
            EnableWithShields = 1 << 4,
        }
    }
}