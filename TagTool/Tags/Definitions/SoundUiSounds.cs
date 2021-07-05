using TagTool.Cache;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_ui_sounds", Tag = "sus!", Size = 0x10, MinVersion = CacheVersion.HaloOnlineED)]
    public class SoundUiSounds : TagStructure
	{
        public List<UiSound> UiSounds;
        public uint Unknown;

        [TagStructure(Size = 0x10)]
        public class UiSound : TagStructure
		{
            public CachedTag Sound;
        }
    }
}