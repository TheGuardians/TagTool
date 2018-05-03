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
        public CachedTagInstance VertexShader;
        public CachedTagInstance PixelShader;
        public uint DrawModeBitmask;
        public List<DrawMode> DrawModes; // Entries in here correspond to an enum in the EXE
        public List<UnknownBlock2> Unknown3;
        public List<ArgumentMapping> ArgumentMappings;
        public List<Argument> Arguments;
        public List<UnknownBlock4> Unknown5;
        public List<UnknownBlock5> Unknown6;
        public List<ShaderMap> ShaderMaps;
        public uint Unknown7;
        public uint Unknown8;
        public uint Unknown9;

        [TagField(Padding = true, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;

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

        [TagStructure(Size = 0x2)]
        public class DrawMode
        {
            public ShaderMode PixelShaderMode;
            public ShaderMode VertexShaderMode;
        }

        [TagStructure(Size = 0x1C)]
        public class UnknownBlock2
        {
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
            public ushort UnknownVectorRegisters_Count { get => GetCount(RegistersMapping1); set => SetCount(ref RegistersMapping1, value); }
            public ushort UnknownVectorRegisters_Offset { get => GetOffset(RegistersMapping1); set => SetOffset(ref RegistersMapping1, value); }
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
            public ushort RegistersMapping1; // Unknown2
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
        public class Argument
        {
            public StringId Name;
        }

        [TagStructure(Size = 0x4)]
        public class UnknownBlock4
        {
            public StringId Unknown;
        }

        [TagStructure(Size = 0x4)]
        public class UnknownBlock5
        {
            public StringId Unknown;
        }

        [TagStructure(Size = 0x4)]
        public class ShaderMap
        {
            public StringId Name;
        }
    }
}