using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "mux_generator", Tag = "muxg", Size = 0xA4)]
    public class MuxGenerator : TagStructure
    {
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag BlendTexture;
        public int FirstMaterialInBlendMap;
        public int LastMaterialInBlendMap;
        public List<MuxGeneratorMaterialBlock> Materials;
        [TagField(ValidTags = new [] { "rmmx" })]
        public CachedTag TargetMuxShader;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag TargetAlbedoBase;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag TargetAlbedoDetail;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag TargetBumpBase;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag TargetBumpDetail;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag TargetParallax;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag TargetMaterial0;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag TargetMaterial1;
        
        [TagStructure(Size = 0x14)]
        public class MuxGeneratorMaterialBlock : TagStructure
        {
            public StringId Name;
            [TagField(ValidTags = new [] { "rmmm" })]
            public CachedTag MuxMaterial;
        }
    }
}
