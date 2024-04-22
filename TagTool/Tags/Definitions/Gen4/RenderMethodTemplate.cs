using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "render_method_template", Tag = "rmt2", Size = 0x84)]
    public class RenderMethodTemplate : TagStructure
    {
        [TagField(ValidTags = new [] { "vtsh" })]
        public CachedTag VertexShader;
        [TagField(ValidTags = new [] { "pixl" })]
        public CachedTag PixelShader;
        public uint AvailableEntryPoints;
        public List<TagBlockIndexBlock> EntryPoints;
        public List<RenderMethodTemplatePassBlock> Passes;
        public List<RenderMethodRoutingInfoBlock> RoutingInfo;
        public List<RenderMethodTemplateConstantTableBlock> FloatConstants;
        public List<RenderMethodTemplateConstantTableBlock> IntConstants;
        public List<RenderMethodTemplateConstantTableBlock> BoolConstants;
        public List<RenderMethodTemplateConstantTableBlock> Textures;
        public List<RenderMethodTemplatePlatformBlock> OtherPlatforms;
        
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
        
        [TagStructure(Size = 0x1C)]
        public class RenderMethodTemplatePassBlock : TagStructure
        {
            // divide by 1024 for count, remainder is start index
            public ushort Bitmaps;
            // divide by 1024 for count, remainder is start index
            public ushort VertexRealConstants;
            // divide by 1024 for count, remainder is start index
            public ushort VertexIntConstants;
            // divide by 1024 for count, remainder is start index
            public ushort VertexBoolConstants;
            // divide by 1024 for count, remainder is start index
            public ushort PixelRealConstants;
            // divide by 1024 for count, remainder is start index
            public ushort PixelIntConstants;
            // divide by 1024 for count, remainder is start index
            public ushort PixelBoolConstants;
            // divide by 1024 for count, remainder is start index
            public ushort ExternBitmaps;
            // divide by 1024 for count, remainder is start index
            public ushort ExternVertexRealConstants;
            // divide by 1024 for count, remainder is start index
            public ushort ExternVertexIntConstants;
            // divide by 1024 for count, remainder is start index
            public ushort ExternPixelRealConstants;
            // divide by 1024 for count, remainder is start index
            public ushort ExternPixelIntConstants;
            public int AlphaBlendMode;
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
        
        [TagStructure(Size = 0x4)]
        public class RenderMethodTemplateConstantTableBlock : TagStructure
        {
            public StringId ParameterName;
        }
        
        [TagStructure(Size = 0x78)]
        public class RenderMethodTemplatePlatformBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "vtsh" })]
            public CachedTag VertexShader;
            [TagField(ValidTags = new [] { "pixl" })]
            public CachedTag PixelShader;
            public uint AvailableEntryPoints;
            public List<TagBlockIndexBlock> EntryPoints;
            public List<RenderMethodTemplatePassBlock> Passes;
            public List<RenderMethodRoutingInfoBlock> RoutingInfo;
            public List<RenderMethodTemplateConstantTableBlock> FloatConstants;
            public List<RenderMethodTemplateConstantTableBlock> IntConstants;
            public List<RenderMethodTemplateConstantTableBlock> BoolConstants;
            public List<RenderMethodTemplateConstantTableBlock> Textures;
        }
    }
}
