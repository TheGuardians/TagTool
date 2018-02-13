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

        [TagStructure(Size = 0x2)]
        public class DrawMode
        {
            // rmt2 uses these pointers in both this block and UnknownBlock2.
            public ushort UnknownBlock2Pointer;
        }

        [TagStructure(Size = 0x1C)]
        public class UnknownBlock2
        {
            public short Unknown1;
            public short Unknown2;
            public short Unknown3;
            public short Unknown4;
            public short Unknown5;
            public short Unknown6;
            public short Unknown7;
            public short Unknown8;
            public short Unknown9;
            public short Unknown10;
            public short Unknown11;
            public short Unknown12;
            public short Unknown13;
            public short Unknown14;
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