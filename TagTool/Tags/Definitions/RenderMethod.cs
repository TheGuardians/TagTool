using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using static TagTool.Tags.Definitions.RenderMethodTemplate;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "render_method", Tag = "rm  ", Size = 0x40)]
    public class RenderMethod : TagStructure
    {
        public CachedTag BaseRenderMethod;
        public List<RenderMethodDefinitionOptionIndex> RenderMethodDefinitionOptionIndices;
        public List<ImportDatum> ImportData;
        public List<ShaderProperty> ShaderProperties;
        public TagMapping.VariableTypeValue InputVariable;
        public TagMapping.VariableTypeValue RangeVariable;
        public TagMapping.OutputModifierValue OutputModifier;
        public TagMapping.VariableTypeValue OutputModifierInput;
        public float RuntimeMConstantValue;
        public int Unknown2; // usually -1

        [TagStructure(Size = 0x2)]
        public class RenderMethodDefinitionOptionIndex : TagStructure
        {
            public short OptionIndex;
        }

        [TagStructure(Size = 0x24)]
        public class ShaderFunction : TagStructure
        {
            // TODO: determine if this is an enum or an index
            public int Type;

            [TagField(Flags = Label)]
            public StringId InputName;

            public StringId RangeName;

            public float TimePeriod;

            public TagFunction Function = new TagFunction { Data = new byte[0] };
        }

        [TagStructure(Size = 0x3C)]
        public class ImportDatum : TagStructure
        {
            [TagField(Flags = Label)]
            public StringId Name;

            public RenderMethodOption.OptionBlock.OptionDataType Type;
            public CachedTag Bitmap;
            public uint Unknown2;
            public int Unknown3;
            public short Unknown4;
            public short Unknown5;
            public short Unknown6;
            public short Unknown7;
            public short Unknown8;
            public short Unknown9;
            public uint Unknown10;
            public List<ShaderFunction> Functions;
        }

        [TagStructure(Size = 0x84)]
        public class ShaderProperty : TagStructure
        {
            public CachedTag Template;
            public List<TextureConstant> TextureConstants;
            public List<RealConstant> RealConstants;
            public List<IntegerConstant> IntegerConstants;
            public uint BooleanConstants; // Each bit indicates true/false. SourceIndex = bit index
            public List<PackedInteger_10_6> EntryPoints; // Ranges of ParameterTables
            public List<ParameterTable> ParameterTables; // Ranges of Parameters by usage
            public List<ParameterMapping> Parameters; // Mapping of constants functions, and registers
            public List<ShaderFunction> Functions; // Functions for animated parameters
            public int AlphaBlendMode;
            public uint BlendFlags;
            public uint Unknown8; // unused?
            [TagField(Length = 8)]
            public short[] QueryableProperties; // Indices of constants. TODO: create an enum

            [TagStructure(Size = 0x18)]
            public class TextureConstant : TagStructure
            {
                [TagField(Flags = Label)]
                public CachedTag Bitmap;
                public short BitmapIndex;
                public sbyte SamplerFlags;
                public sbyte SamplerFilterMode;
                public sbyte ExternMode;
                public sbyte XFormArgumentIndex;
                public PackedInteger_10_6 Functions = new PackedInteger_10_6(); // Range of Functions
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

            [TagStructure(Size = 0x4)]
            public class IntegerConstant : TagStructure
            {
                public uint Value;
            }

            [TagStructure(Size = 0x6)]
            public class ParameterTable : TagStructure
            {
                public PackedInteger_10_6 Texture = new PackedInteger_10_6();
                public PackedInteger_10_6 RealVertex = new PackedInteger_10_6();
                public PackedInteger_10_6 RealPixel = new PackedInteger_10_6();
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
    }
}