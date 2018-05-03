using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "render_method_template", Tag = "rmt2", Size = 0x84, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "render_method_template", Tag = "rmt2", Size = 0x90, MinVersion = CacheVersion.HaloOnline106708)]
    public class RenderMethodTemplate
    {
        public enum ShaderMode : sbyte
        {
            Default,
            Albedo,
            Static_Default,
            Static_Per_Pixel,
            Static_Per_Vertex,
            Static_Sh,
            Static_Prt_Ambient,
            Static_Prt_Linear,
            Static_Prt_Quadratic,
            Dynamic_Light,
            Shadow_Generate,
            Shadow_Apply,
            Active_Camo,
            Lightmap_Debug_Mode,
            Static_Per_Vertex_Color,
            Water_Tessellation,
            Water_Shading,
            Dynamic_Light_Cinematic,
            Z_Only,
            Sfx_Distort
        }

        public enum ShaderModeBitmask : uint
        {
            Default = 1 << 0,
            Albedo = 1 << 1,
            Static_Default = 1 << 2,
            Static_Per_Pixel = 1 << 3,
            Static_Per_Vertex = 1 << 4,
            Static_Sh = 1 << 5,
            Static_Prt_Ambient = 1 << 6,
            Static_Prt_Linear = 1 << 7,
            Static_Prt_Quadratic = 1 << 8,
            Dynamic_Light = 1 << 9,
            Shadow_Generate = 1 << 10,
            Shadow_Apply = 1 << 11,
            Active_Camo = 1 << 12,
            Lightmap_Debug_Mode = 1 << 13,
            Static_Per_Vertex_Color = 1 << 14,
            Water_Tessellation = 1 << 15,
            Water_Shading = 1 << 16,
            Dynamic_Light_Cinematic = 1 << 17,
            Z_Only = 1 << 18,
            Sfx_Distort = 1 << 19,
        }

        public CachedTagInstance VertexShader;
        public CachedTagInstance PixelShader;
        public ShaderModeBitmask DrawModeBitmask;
        public List<DrawMode> DrawModes; // Entries in here correspond to an enum in the EXE
        public List<DrawModeRegisterOffsetBlock> DrawModeRegisterOffsets;
        public List<ArgumentMapping> ArgumentMappings;
        public List<ShaderArgument> Arguments;
        public List<ShaderArgument> Unknown5;
        public List<ShaderArgument> GlobalArguments;
        public List<ShaderArgument> ShaderMaps;
        public uint Unknown7;
        public uint Unknown8;
        public uint Unknown9;

        [TagField(Padding = true, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;

        [TagStructure(Size = 0x2)]
        public class DrawMode
        {
            public ShaderMode PixelShaderMode;
            public ShaderMode VertexShaderMode;
        }

        [TagStructure(Size = 0x1C)]
        public class DrawModeRegisterOffsetBlock
        {
            public enum DrawModeRegisterOffsetType
            {

            }



            static ushort GetCount(ushort value) => (ushort)(value >> 10);
            static ushort GetOffset(ushort value) => (ushort)(value & 0x3FFu);
            static void SetCount(ref ushort destination, ushort count)
            {
                if (count > 0x3Fu) throw new System.Exception("Out of range");
                destination = (ushort)(GetOffset(destination) & ((count & 0x3F) << 10));
            }
            static void SetOffset(ref ushort destination, ushort offset)
            {
                if (offset > 0x3FFu) throw new System.Exception("Out of range");
                destination = (ushort)((offset & 0x3FF) & ((GetCount(destination) & 0x3F) << 10));
            }

            public ushort ShaderMapSamplers_Count { get => GetCount(ShaderMapSamplers); set => SetCount(ref ShaderMapSamplers, value); }
            public ushort ShaderMapSamplers_Offset { get => GetOffset(ShaderMapSamplers); set => SetOffset(ref ShaderMapSamplers, value); }
            public ushort UnknownVectorRegisters_Count { get => GetCount(UnknownVectorRegisters); set => SetCount(ref UnknownVectorRegisters, value); }
            public ushort UnknownVectorRegisters_Offset { get => GetOffset(UnknownVectorRegisters); set => SetOffset(ref UnknownVectorRegisters, value); }
            public ushort RegistersMapping2_Count { get => GetCount(RegistersMapping2); set => SetCount(ref RegistersMapping2, value); }
            public ushort RegistersMapping2_Offset { get => GetOffset(RegistersMapping2); set => SetOffset(ref RegistersMapping2, value); }
            public ushort RegistersMapping3_Count { get => GetCount(RegistersMapping3); set => SetCount(ref RegistersMapping3, value); }
            public ushort RegistersMapping3_Offset { get => GetOffset(RegistersMapping3); set => SetOffset(ref RegistersMapping3, value); }
            public ushort ArgumentsVectorRegisters_Count { get => GetCount(ArgumentsVectorRegisters); set => SetCount(ref ArgumentsVectorRegisters, value); }
            public ushort ArgumentsVectorRegisters_Offset { get => GetOffset(ArgumentsVectorRegisters); set => SetOffset(ref ArgumentsVectorRegisters, value); }
            public ushort RegistersMapping5_Count { get => GetCount(RegistersMapping5); set => SetCount(ref RegistersMapping5, value); }
            public ushort RegistersMapping5_Offset { get => GetOffset(RegistersMapping5); set => SetOffset(ref RegistersMapping5, value); }
            public ushort GlobalArgumentsVectorRegisters_Count { get => GetCount(GlobalArgumentsVectorRegisters); set => SetCount(ref GlobalArgumentsVectorRegisters, value); }
            public ushort GlobalArgumentsVectorRegisters_Offset { get => GetOffset(GlobalArgumentsVectorRegisters); set => SetOffset(ref GlobalArgumentsVectorRegisters, value); }
            public ushort RenderBufferRegisters_Count { get => GetCount(RenderBufferRegisters); set => SetCount(ref RenderBufferRegisters, value); }
            public ushort RenderBufferRegisters_Offset { get => GetOffset(RenderBufferRegisters); set => SetOffset(ref RenderBufferRegisters, value); }
            public ushort RegistersMapping8_Count { get => GetCount(RegistersMapping8); set => SetCount(ref RegistersMapping8, value); }
            public ushort RegistersMapping8_Offset { get => GetOffset(RegistersMapping8); set => SetOffset(ref RegistersMapping8, value); }
            public ushort RegistersMapping9_Count { get => GetCount(RegistersMapping9); set => SetCount(ref RegistersMapping9, value); }
            public ushort RegistersMapping9_Offset { get => GetOffset(RegistersMapping9); set => SetOffset(ref RegistersMapping9, value); }
            public ushort DebugVectorRegisters_Count { get => GetCount(DebugVectorRegisters); set => SetCount(ref DebugVectorRegisters, value); }
            public ushort DebugVectorRegisters_Offset { get => GetOffset(DebugVectorRegisters); set => SetOffset(ref DebugVectorRegisters, value); }
            public ushort RegistersMapping11_Count { get => GetCount(RegistersMapping11); set => SetCount(ref RegistersMapping11, value); }
            public ushort RegistersMapping11_Offset { get => GetOffset(RegistersMapping11); set => SetOffset(ref RegistersMapping11, value); }
            public ushort RegistersMapping12_Count { get => GetCount(RegistersMapping12); set => SetCount(ref RegistersMapping12, value); }
            public ushort RegistersMapping12_Offset { get => GetOffset(RegistersMapping12); set => SetOffset(ref RegistersMapping12, value); }
            public ushort RegistersMapping13_Count { get => GetCount(RegistersMapping13); set => SetCount(ref RegistersMapping13, value); }
            public ushort RegistersMapping13_Offset { get => GetOffset(RegistersMapping13); set => SetOffset(ref RegistersMapping13, value); }

            public ushort ShaderMapSamplers; // Unknown1
            public ushort UnknownVectorRegisters; // Unknown2
            public ushort RegistersMapping2; // Unknown3
            public ushort RegistersMapping3; // Unknown4
            public ushort ArgumentsVectorRegisters; // Unknown5
            public ushort RegistersMapping5; // Unknown6
            public ushort GlobalArgumentsVectorRegisters; // Unknown7
            public ushort RenderBufferRegisters; // Unknown8
            public ushort RegistersMapping8; // Unknown9
            public ushort RegistersMapping9; // Unknown10
            public ushort DebugVectorRegisters; // Unknown11
            public ushort RegistersMapping11; // Unknown12
            public ushort RegistersMapping12; // Unknown13
            public ushort RegistersMapping13; // Unknown14
        }

        /// <summary>
        /// Binds an argument in the render method tag to a pixel shader constant.
        /// </summary>
        [TagStructure(Size = 0x4)]
        public class ArgumentMapping
        {
            /// <summary>
            /// The GPU register to bind the argument to.
            /// </summary>
            public ushort RegisterIndex;

            /// <summary>
            /// The index of the argument in one of the blocks in the render method tag.
            /// The block used depends on the argument type.
            /// </summary>
            public byte ArgumentIndex;

            public byte Unknown;
        }

        [TagStructure(Size = 0x4)]
        public class ShaderArgument
        {
            public StringId Name;
        }
    }
}