using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "user_interface_sounds_definition", Tag = "uise", Size = 0x140, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "user_interface_sounds_definition", Tag = "uise", Size = 0x14C, MinVersion = CacheVersion.Halo3ODST)]
    public class UserInterfaceSoundsDefinition : TagStructure
	{
        public CachedTag Error;
        public CachedTag CursorSoundVertical;
        public CachedTag CursorSoundHorizontal;
        public CachedTag AButton;
        public CachedTag BButton;
        public CachedTag XButton;
        public CachedTag YButton;
        public CachedTag StartButton;
        public CachedTag BackButton;
        public CachedTag LeftBumper;
        public CachedTag RightBumper;
        public CachedTag LeftTrigger;
        public CachedTag RightTrigger;
        public CachedTag TimerSound;
        public CachedTag TimerSoundZero;
        public CachedTag AltTimerSound;
        public CachedTag AltTimerSoundZero;
        public CachedTag MatchmakingAdvanceSound;
        public CachedTag RankUpSound;
        public CachedTag MatchmakingPartyUpSound;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<AtlasSound> AtlasSounds;

        [TagStructure(Size = 0x14)]
        public class AtlasSound : TagStructure
		{
            public StringId Name;
            public CachedTag Sound;
        }
    }
}