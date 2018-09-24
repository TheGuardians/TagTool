using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "user_interface_sounds_definition", Tag = "uise", Size = 0x140, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "user_interface_sounds_definition", Tag = "uise", Size = 0x14C, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "user_interface_sounds_definition", Tag = "uise", Size = 0x150, MinVersion = CacheVersion.HaloOnline106708)]
    public class UserInterfaceSoundsDefinition : TagStructure
	{
        public CachedTagInstance Error;
        public CachedTagInstance VerticalNavigation;
        public CachedTagInstance HorizontalNavigation;
        public CachedTagInstance AButton;
        public CachedTagInstance BButton;
        public CachedTagInstance XButton;
        public CachedTagInstance YButton;
        public CachedTagInstance StartButton;
        public CachedTagInstance BackButton;
        public CachedTagInstance LeftBumper;
        public CachedTagInstance RightBumper;
        public CachedTagInstance LeftTrigger;
        public CachedTagInstance RightTrigger;
        public CachedTagInstance TimerSound;
        public CachedTagInstance TimerSoundZero;
        public CachedTagInstance AltTimerSound;
        public CachedTagInstance SecondAltTimerSound;
        public CachedTagInstance MatchmakingAdvanceSound;
        public CachedTagInstance RankUp;
        public CachedTagInstance MatchmakingPartyUpSound;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<AtlasSound> AtlasSounds;

        [TagField(Padding = true, Length = 4, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;

        [TagStructure(Size = 0x14)]
        public class AtlasSound : TagStructure
		{
            public StringId Name;
            public CachedTagInstance Sound;
        }
    }
}