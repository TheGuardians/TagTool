using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using System;
using TagTool.Shaders;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "render_method_template", Tag = "rmt2", Size = 0x84)]
    public class RenderMethodTemplate : TagStructure
	{
        public CachedTag VertexShader;
        public CachedTag PixelShader;

        [TagField(MaxVersion = CacheVersion.HaloOnline235640)]
        public EntryPointBitMask ValidEntryPoints;
        [TagField(MinVersion = CacheVersion.HaloOnline301003, MaxVersion = CacheVersion.HaloOnline700123)]
        public EntryPointBitMaskMs30 ValidEntryPointsHO;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public EntryPointBitMaskReach ValidEntryPointsReach;

        public List<TagBlockIndex> EntryPoints; // Ranges of ParameterTables by usage
        public List<ParameterTable> ParameterTables; // Ranges of Parameters
        public List<ParameterMapping> Parameters; 
        public List<ShaderArgument> RealParameterNames;
        public List<ShaderArgument> IntegerParameterNames;
        public List<ShaderArgument> BooleanParameterNames;
        public List<ShaderArgument> TextureParameterNames;

        [TagField(Flags = Padding, Length = 12)]
        public byte[] Unused;

        [TagStructure(Size = 0x2)]
        public class TagBlockIndex : TagStructure
		{
            public ushort Offset { get => GetOffset(); set => SetOffset(value); }
            public ushort Count { get => GetCount(); set => SetCount(value); }

            private ushort GetCount() => (ushort)(Integer >> 10);
            private ushort GetOffset() => (ushort)(Integer & 0x3FFu);
            private void SetCount(ushort count)
            {
                if (count > 0x3Fu) throw new System.Exception("Out of range");
                var a = GetOffset();
                var b = (count & 0x3F) << 10;
                var value = (ushort)(a | b);
                Integer = value;
            }
            private void SetOffset(ushort _offset)
            {
                if (_offset > 0x3FFu) throw new System.Exception("Out of range");
                var a = (_offset & 0x3FF);
                var b = (GetCount() & 0x3F) << 10;
                var value = (ushort)(a | b);
                Integer = value;
            }

            public ushort Integer;
        }

        [TagStructure(Size = 0x1C)]
        public class ParameterTable : TagStructure
		{
            [TagField(Length = (int)ParameterUsage.Count)]
            public TagBlockIndex[] Values = new TagBlockIndex[(int)ParameterUsage.Count];

            public TagBlockIndex this[ParameterUsage usage]
            {
                get { return Values[(int)usage]; }
                set { Values[(int)usage] = value; }
            }
        }

        /// <summary>
        /// Binds an argument in the render method tag to a pixel shader constant.
        /// </summary>
        [TagStructure(Size = 0x4)]
        public class ParameterMapping : TagStructure
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

            public byte Flags; // bitmap flag 1 = vertex sampler (register+16). also used as vector swizzle mask
        }

        [TagStructure(Size = 0x4)]
        public class ShaderArgument : TagStructure
		{
            public StringId Name;
        }
    }
}