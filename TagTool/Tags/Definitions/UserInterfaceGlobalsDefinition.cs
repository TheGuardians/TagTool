using TagTool.Cache;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "user_interface_globals_definition", Tag = "wgtz", Size = 0x3C, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "user_interface_globals_definition", Tag = "wgtz", Size = 0x50, MaxVersion = CacheVersion.HaloOnline449175)]
    [TagStructure(Name = "user_interface_globals_definition", Tag = "wgtz", Size = 0x60, MinVersion = CacheVersion.HaloOnline498295)]
    
    public class UserInterfaceGlobalsDefinition
    {
        public CachedTagInstance SharedUiGlobals;
        public CachedTagInstance EditableSettings;
        public CachedTagInstance MatchmakingHopperStrings;
        public List<ScreenWidget> ScreenWidgets;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public CachedTagInstance TextureRenderList;

        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public CachedTagInstance SwearFilter; // TODO: Version number

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown;

        [TagStructure(Size = 0x10)]
        public class ScreenWidget
        {
            public CachedTagInstance Widget;
        }
    }
}