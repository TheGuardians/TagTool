using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "material_effects", Tag = "foot", Size = 0xC)]
    public class MaterialEffects : TagStructure
	{
        public List<Effect> Effects;

        [TagStructure(Size = 0x24)]
        public class Effect : TagStructure
		{
            public List<OldMaterialEffectBlock> OldMaterials;
            public List<EffectBlock> Sounds;
            public List<EffectBlock> Effects;

            [TagStructure(Size = 0x28, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x2C, MinVersion = CacheVersion.Halo3ODST)]
            public class EffectBlock : TagStructure
			{
                [TagField(ValidTags = new[] { "scmb", "snd!", "lsnd", "effe" })]
                public CachedTag Primary;
                [TagField(ValidTags = new[] { "scmb", "snd!", "lsnd", "effe" })]
                public CachedTag Secondary;
                [TagField(Flags = GlobalMaterial)]
                public StringId MaterialName;
                [TagField(Flags = GlobalMaterial)]
                public short RuntimeMaterialIndex; // formerly GlobalMaterialIndex
                public SweetenerModeValue SweetenerMode;

                [TagField(Flags = Padding, Length = 1)]
                public byte[] Unused;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public float MaxVisibilityDistance;

                public enum SweetenerModeValue : sbyte
                {
                    Default,
                    Enabled,
                    Disabled
                }
            }

            [TagStructure(Size = 0x2C)]
            public class OldMaterialEffectBlock : TagStructure
            {
                [TagField(ValidTags = new[] { "effe" })]
                public CachedTag Effect;
                [TagField(ValidTags = new[] { "snd!", "lsnd" })]
                public CachedTag Sound;
                [TagField(Flags = GlobalMaterial)]
                public StringId MaterialName;
                [TagField(Flags = GlobalMaterial)]
                public short RuntimeMaterialIndex;

                [TagField(Length = 2, Flags = Padding)]
                public byte[] KTRVUIKB;

                public SweeneterModeEnum SweetenerMode;

                [TagField(Length = 3, Flags = Padding)]
                public byte[] QNGPTA;

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
