using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "material_effects", Tag = "foot", Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "material_effects", Tag = "foot", Size = 0x14, MinVersion = CacheVersion.HaloOnline106708)]
    public class MaterialEffects
    {
        public List<Effect> Effects;

        [TagField(Padding = true, Length = 8, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;

        [TagStructure(Size = 0x24)]
        public class Effect
        {
            public List<EffectReference> OldMaterials;
            public List<EffectReference> Sounds;
            public List<EffectReference> Effects;

            [TagStructure(Size = 0x28, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x2C, MinVersion = CacheVersion.Halo3ODST)]
            public class EffectReference
            {
                public CachedTagInstance Effect;
                public CachedTagInstance Sound;
                public StringId MaterialName;
                public short GlobalMaterialIndex;
                public SweetenerModeValue SweetenerMode;

                [TagField(Padding = true, Length = 1)]
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