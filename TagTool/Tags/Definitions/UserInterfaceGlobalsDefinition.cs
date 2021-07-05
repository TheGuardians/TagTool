using TagTool.Cache;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "user_interface_globals_definition", Tag = "wgtz", Size = 0x3C, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "user_interface_globals_definition", Tag = "wgtz", Size = 0x50, MinVersion = CacheVersion.HaloOnlineED)]
    [TagStructure(Name = "user_interface_globals_definition", Tag = "wgtz", Size = 0x60, MinVersion = CacheVersion.HaloOnline498295)]
    
    public class UserInterfaceGlobalsDefinition : TagStructure
	{
        public CachedTag SharedUiGlobals;
        public CachedTag EditableSettings;
        public CachedTag MatchmakingHopperStrings;
        public List<ScreenWidget> ScreenWidgets;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public CachedTag TextureRenderList;

        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public CachedTag SwearFilter; // TODO: Version number

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public uint Unknown;

        [TagStructure(Size = 0x10)]
        public class ScreenWidget : TagStructure
		{
            public CachedTag Widget;
        }
    }
}