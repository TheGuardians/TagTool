using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "material_effects", Tag = "foot", Size = 0x8C)]
    public class MaterialEffects : TagStructure
    {
        public List<MaterialEffectBlock> Effects;
        [TagField(Length = 0x80)]
        public byte[] Padding;
        
        [TagStructure(Size = 0x1C)]
        public class MaterialEffectBlock : TagStructure
        {
            public List<MaterialEffectMaterialBlock> Materials;
            [TagField(Length = 0x10)]
            public byte[] Padding;
            
            [TagStructure(Size = 0x30)]
            public class MaterialEffectMaterialBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag Effect;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                [TagField(Length = 0x10)]
                public byte[] Padding;
            }
        }
    }
}

