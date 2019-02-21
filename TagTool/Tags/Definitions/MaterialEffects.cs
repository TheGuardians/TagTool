using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "material_effects", Tag = "foot", Size = 0xC)]
    public class MaterialEffects : TagStructure
	{
        public List<Effect> Effects;

        [TagStructure(Size = 0x24)]
        public class Effect : TagStructure
		{
            public List<EffectReference> OldMaterials;
            public List<EffectReference> Sounds;
            public List<EffectReference> Effects;

            [TagStructure(Size = 0x28, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x2C, MinVersion = CacheVersion.Halo3ODST)]
            public class EffectReference : TagStructure
			{
                public CachedTagInstance Effect;
                public CachedTagInstance Sound;
                public StringId MaterialName;
                public short GlobalMaterialIndex;
                public SweetenerModeValue SweetenerMode;

                [TagField(Flags = TagFieldFlags.Padding, Length = 1)]
                public byte[] Unused;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public float MaxVisibilityDistance;

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