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
            public List<EffectReference> Sounds;
            public List<EffectReference> Effects;

            [TagStructure(Size = 0x28, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x2C, MinVersion = CacheVersion.Halo3ODST)]
            public class EffectReference : TagStructure
			{
                public CachedTag Effect;
                public CachedTag Sound;
                public StringId MaterialName;
                public short RuntimeMaterialIndex; // formerly GlobalMaterialIndex
                public SweetenerModeValue SweetenerMode;

                [TagField(Flags = Padding)]
                public byte Unused;

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
                public CachedTag Effect;
                public CachedTag Sound;
                public StringId MaterialName;
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
