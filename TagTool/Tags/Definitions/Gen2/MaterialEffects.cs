using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "material_effects", Tag = "foot", Size = 0x8)]
    public class MaterialEffects : TagStructure
    {
        public List<MaterialEffectBlockV2> Effects;
        
        [TagStructure(Size = 0x18)]
        public class MaterialEffectBlockV2 : TagStructure
        {
            public List<OldMaterialEffectMaterialBlock> OldMaterialsDoNotUse;
            public List<MaterialEffectMaterialBlock> Sounds;
            public List<MaterialEffectMaterialBlock1> Effects;
            
            [TagStructure(Size = 0x1C)]
            public class OldMaterialEffectMaterialBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag Effect;
                [TagField(ValidTags = new [] { "snd!","lsnd" })]
                public CachedTag Sound;
                public StringId MaterialName;
                [TagField(Length = 0x4)]
                public byte[] Unknown;
                public SweetenerModeValue SweetenerMode;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum SweetenerModeValue : sbyte
                {
                    SweetenerDefault,
                    SweetenerEnabled,
                    SweetenerDisabled
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class MaterialEffectMaterialBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "snd!","lsnd","effe" })]
                public CachedTag TagEffectOrSound;
                [TagField(ValidTags = new [] { "snd!","lsnd","effe" })]
                public CachedTag SecondaryTagEffectOrSound;
                public StringId MaterialName;
                [TagField(Length = 0x2)]
                public byte[] Unknown;
                public SweetenerModeValue SweetenerMode;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum SweetenerModeValue : sbyte
                {
                    SweetenerDefault,
                    SweetenerEnabled,
                    SweetenerDisabled
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class MaterialEffectMaterialBlock1 : TagStructure
            {
                [TagField(ValidTags = new [] { "snd!","lsnd","effe" })]
                public CachedTag TagEffectOrSound;
                [TagField(ValidTags = new [] { "snd!","lsnd","effe" })]
                public CachedTag SecondaryTagEffectOrSound;
                public StringId MaterialName;
                [TagField(Length = 0x2)]
                public byte[] Unknown;
                public SweetenerModeValue SweetenerMode;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum SweetenerModeValue : sbyte
                {
                    SweetenerDefault,
                    SweetenerEnabled,
                    SweetenerDisabled
                }
            }
        }
    }
}

