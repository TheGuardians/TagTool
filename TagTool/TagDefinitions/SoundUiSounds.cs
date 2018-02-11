using TagTool.Cache;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "sound_ui_sounds", Tag = "sus!", Size = 0x10, MinVersion = CacheVersion.HaloOnline106708)]
    public class SoundUiSounds
    {
        public List<UiSound> UiSounds;
        public uint Unknown;

        [TagStructure(Size = 0x10)]
        public class UiSound
        {
            public CachedTagInstance Sound;
        }
    }
}