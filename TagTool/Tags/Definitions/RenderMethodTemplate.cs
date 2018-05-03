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
                ShaderMapSamplerRegisters,
                UnknownVectorRegisters,
                Unknown1,
                Unknown2,
                ArgumentsVectorRegisters,
                Unknown3,
                GlobalArgumentsVectorRegisters,
                RenderBufferSamplerRegisters,
                Unknown4,
                Unknown5,
                DebugVectorRegisters,
                Unknown6,
                Unknown7,
                Unknown8,
                DrawModeRegisterOffsetType_Count
            }

            public enum DrawModeRegisterOffsetTypeBits
            {
                ShaderMapSamplerRegisters = 1 << 0,
                UnknownVectorRegisters = 1 << 1,
                Unknown1 = 1 << 2,
                Unknown2 = 1 << 3,
                ArgumentsVectorRegisters = 1 << 4,
                Unknown3 = 1 << 5,
                GlobalArgumentsVectorRegisters = 1 << 6,
                RenderBufferSamplerRegisters = 1 << 7,
                Unknown4 = 1 << 8,
                Unknown5 = 1 << 9,
                DebugVectorRegisters = 1 << 10,
                Unknown6 = 1 << 11,
                Unknown7 = 1 << 12,
                Unknown8 = 1 << 13
            }

            private ushort GetValue(DrawModeRegisterOffsetType offset) => (ushort)this.GetType().GetField($"RegisterMapping{(int)offset}").GetValue(this);
            private void SetValue(DrawModeRegisterOffsetType offset, ushort value) => this.GetType().GetField($"RegisterMapping{(int)offset}").SetValue(this, value);
            public ushort GetCount(DrawModeRegisterOffsetType offset) => (ushort)(GetValue(offset) >> 10);
            public ushort GetOffset(DrawModeRegisterOffsetType offset) => (ushort)(GetValue(offset) & 0x3FFu);
            public void SetCount(DrawModeRegisterOffsetType offset, ushort count)
            {
                if (count > 0x3Fu) throw new System.Exception("Out of range");
                var value = (ushort) (GetOffset(offset) & ((count & 0x3F) << 10));
                SetValue(offset, value);
            }
            public void SetOffset(DrawModeRegisterOffsetType offset, ushort _offset)
            {
                if (_offset > 0x3FFu) throw new System.Exception("Out of range");
                var value = (ushort)((_offset & 0x3FF) & ((GetCount(offset) & 0x3F) << 10));
                SetValue(offset, value);
            }

            public ushort RegisterMapping0;
            public ushort RegisterMapping1;
            public ushort RegisterMapping2;
            public ushort RegisterMapping3;
            public ushort RegisterMapping4;
            public ushort RegisterMapping5;
            public ushort RegisterMapping6;
            public ushort RegisterMapping7;
            public ushort RegisterMapping8;
            public ushort RegisterMapping9;
            public ushort RegisterMapping10;
            public ushort RegisterMapping11;
            public ushort RegisterMapping12;
            public ushort RegisterMapping13;
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