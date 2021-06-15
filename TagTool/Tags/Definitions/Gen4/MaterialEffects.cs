using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "material_effects", Tag = "foot", Size = 0xC)]
    public class MaterialEffects : TagStructure
    {
        public List<MaterialEffectBlockV2> Effects;
        
        [TagStructure(Size = 0x24)]
        public class MaterialEffectBlockV2 : TagStructure
        {
            public List<OldMaterialEffectMaterialBlock> OldMaterials;
            public List<MaterialEffectMaterialBlock> Sounds;
            public List<MaterialEffectMaterialBlock> Effects;
            
            [TagStructure(Size = 0x2C)]
            public class OldMaterialEffectMaterialBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag Effect;
                [TagField(ValidTags = new [] { "scmb","sndo","lsnd","snd!" })]
                public CachedTag Sound;
                public StringId MaterialName;
                public short RuntimeMaterialIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public SweeneterModeEnum SweetenerMode;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                
                public enum SweeneterModeEnum : sbyte
                {
                    SweetenerDefault,
                    SweetenerEnabled,
                    SweetenerDisabled
                }
            }
            
            [TagStructure(Size = 0x2C)]
            public class MaterialEffectMaterialBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "sndo","lsnd","snd!","effe" })]
                public CachedTag Tag;
                [TagField(ValidTags = new [] { "sndo","lsnd","snd!","effe" })]
                public CachedTag SecondaryTag;
                public StringId MaterialName;
                public short RuntimeMaterialIndex;
                public SweeneterModeEnum SweetenerMode;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                // manual override for the max distance this effect can be from the camera and still be rendered (not valid for
                // sounds).
                public float MaxVisibilityDistance;
                
                public enum SweeneterModeEnum : sbyte
                {
                    SweetenerDefault,
                    SweetenerEnabled,
                    SweetenerDisabled
                }
            }
        }
    }
}
