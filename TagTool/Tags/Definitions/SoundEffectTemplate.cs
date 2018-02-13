using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_effect_template", Tag = "<fx>", Size = 0x1C, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "sound_effect_template", Tag = "<fx>", Size = 0x20, MinVersion = CacheVersion.HaloOnline106708)]
    public class SoundEffectTemplate
    {
        public float TemplateCollectionBlock;
        public float TemplateCollectionBlock2;
        public float TemplateCollectionBlock3;
        public int InputEffectName;
        public List<AdditionalSoundInput> AdditionalSoundInputs;

        [TagField(Padding = true, Length = 4, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;

        [TagStructure(Size = 0x1C)]
        public class AdditionalSoundInput
        {
            public StringId DspEffect;
            public TagFunction LowFrequencySoundFunction = new TagFunction { Data = new byte[0] };
            public float TimePeriod;
        }
    }
}