using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "material_effects", Tag = "foot", Size = 0xC)]
    public class MaterialEffects : TagStructure
    {
        public List<MaterialEffect> Effects;
        
        [TagStructure(Size = 0x24)]
        public class MaterialEffect : TagStructure
        {
            public List<MaterialEffectMaterialOld> OldMaterialsDoNotUse;
            public List<MaterialEffectMaterial> Sounds;
            public List<MaterialEffectMaterial> Effects;
            
            [TagStructure(Size = 0x2C)]
            public class MaterialEffectMaterialOld : TagStructure
            {
                public CachedTag Effect;
                public CachedTag Sound;
                public StringId MaterialName;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown1;
                public SweetenerModeValue SweetenerMode;
                [TagField(Flags = Padding, Length = 3)]
                public byte[] Padding1;
                
                public enum SweetenerModeValue : sbyte
                {
                    SweetenerDefault,
                    SweetenerEnabled,
                    SweetenerDisabled
                }
            }
            
            [TagStructure(Size = 0x28)]
            public class MaterialEffectMaterial : TagStructure
            {
                public CachedTag TagEffectOrSound;
                public CachedTag SecondaryTagEffectOrSound;
                public StringId MaterialName;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unknown1;
                public SweetenerModeValue SweetenerMode;
                [TagField(Flags = Padding, Length = 1)]
                public byte[] Padding1;
                
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

