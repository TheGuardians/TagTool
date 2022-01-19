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

        public List<TagBlockIndex> EntryPoints;
        public List<PassBlock> Passes;
        public List<RoutingInfoBlock> RoutingInfo; 
        public List<ShaderArgument> RealParameterNames;
        public List<ShaderArgument> IntegerParameterNames;
        public List<ShaderArgument> BooleanParameterNames;
        public List<ShaderArgument> TextureParameterNames;
        public List<RenderMethodTemplatePlatformBlock> OtherPlatforms;

        [TagStructure(Size = 0x78)]
        public class RenderMethodTemplatePlatformBlock : TagStructure
        {
            public CachedTag VertexShader;
            public CachedTag PixelShader;

            [TagField(MaxVersion = CacheVersion.HaloOnline235640)]
            public EntryPointBitMask ValidEntryPoints;
            [TagField(MinVersion = CacheVersion.HaloOnline301003, MaxVersion = CacheVersion.HaloOnline700123)]
            public EntryPointBitMaskMs30 ValidEntryPointsHO;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public EntryPointBitMaskReach ValidEntryPointsReach;

            public List<TagBlockIndex> EntryPoints;
            public List<PassBlock> Passes;
            public List<RoutingInfoBlock> RoutingInfo;
            public List<ShaderArgument> RealParameterNames;
            public List<ShaderArgument> IntegerParameterNames;
            public List<ShaderArgument> BooleanParameterNames;
            public List<ShaderArgument> TextureParameterNames;
        }

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

        [TagStructure(Size = 0x1C, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x20, Align = 0x8, Platform = CachePlatform.MCC)]
        public class PassBlock : TagStructure
		{
            [TagField(Length = (int)ParameterUsage.Count)]
            public TagBlockIndex[] Values = new TagBlockIndex[(int)ParameterUsage.Count];

            public short PixelParametersSize;
            public short VertexParametersSize;

            public TagBlockIndex this[ParameterUsage usage]
            {
                get { return Values[(int)usage]; }
                set { Values[(int)usage] = value; }
            }
        }

        [TagStructure(Size = 0x4)]
        public class RoutingInfoBlock : TagStructure
		{
            public ushort DestinationIndex; // The GPU register to bind the argument to.
            public byte SourceIndex; // Source argument block index.
            public byte Flags; // Changes on usage, bitmap: bit 1 = vertex sampler. real: vector swizzle mask
        }

        [TagStructure(Size = 0x4)]
        public class ShaderArgument : TagStructure
		{
            public StringId Name;
        }
    }
}