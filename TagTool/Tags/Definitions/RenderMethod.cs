using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "render_method", Tag = "rm  ", Size = 0x40)]
    public class RenderMethod
    {
        public CachedTagInstance BaseRenderMethod;
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
        public class RenderMethodDefinitionOptionIndex
        {
            public short OptionIndex;
        }

        [TagStructure(Size = 0x24)]
        public class FunctionBlock
        {
            public int Unknown;
            [TagField(Label = true)]
            public StringId Name;
            public uint Unknown2;
            public uint Unknown3;
            public TagFunction Function = new TagFunction { Data = new byte[0] };
        }

        [TagStructure(Size = 0x3C)]
        public class ImportDatum
        {
            [TagField(Label = true)]
            public StringId Name;

            public RenderMethodOption.OptionBlock.OptionDataType Type;
            public CachedTagInstance Bitmap;
            public uint Unknown2;
            public int Unknown3;
            public short Unknown4;
            public short Unknown5;
            public short Unknown6;
            public short Unknown7;
            public short Unknown8;
            public short Unknown9;
            public uint Unknown10;
            public List<FunctionBlock> Functions;
        }

        [TagStructure(Size = 0x84)]
        public class ShaderProperty
        {
            public CachedTagInstance Template;
            public List<ShaderMap> ShaderMaps;
            public List<Argument> Arguments;
            public List<UnknownBlock1> Unknown;
            public uint Unknown2;
            public List<RenderMethodTemplate.DrawMode> DrawModes;
            public List<UnknownBlock3> Unknown3;
            public List<ArgumentMapping> ArgumentMappings;
            public List<FunctionBlock> Functions;
            public int BitmapTransparency;
            public int Unknown7;
            public uint Unknown8;
            public short Unknown9;
            public short Unknown10;
            public short Unknown11;
            public short Unknown12;
            public short Unknown13;
            public short Unknown14;
            public short Unknown15;
            public short Unknown16;

            [TagStructure(Size = 0x18)]
            public class ShaderMap
            {
                [TagField(Label = true)]
                public CachedTagInstance Bitmap;
                public sbyte Unknown;
                public sbyte BitmapIndex;
                public sbyte Unknown2;
                public byte BitmapFlags;
                public sbyte UnknownBitmapIndexEnable;
                public sbyte XFormArgumentIndex;
                public sbyte Unknown3;
                public sbyte Unknown4;
            }

            [TagStructure(Size = 0x10)]
            public class Argument
            {
                [TagField(Length = 4)]
                public float[] Values;

                private float[] _Values
                {
                    get
                    {
                        if(Values == null)
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
            public class UnknownBlock1
            {
                public uint Unknown;
            }

            [TagStructure(Size = 0x6)]
            public class UnknownBlock3
            {
                public short DataHandleSampler;
                public short DataHandleUnknown;
                public short DataHandleVector;
            }

            [TagStructure(Size = 0x4)]
            public class ArgumentMapping
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
                    get => (RenderMethodExternalValue)this.ArgumentIndex;
                    set => this.ArgumentIndex = (byte)value;
                }

                public short RegisterIndex;
                public byte FunctionIndex;
                public byte ArgumentIndex;
            }
        }
    }
}