using TagTool.Cache;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "user_interface_globals_definition", Tag = "wgtz", Size = 0x3C, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "user_interface_globals_definition", Tag = "wgtz", Size = 0x50, MinVersion = CacheVersion.HaloOnlineED)]
    [TagStructure(Name = "user_interface_globals_definition", Tag = "wgtz", Size = 0x60, MinVersion = CacheVersion.HaloOnline498295)]
    
    public class UserInterfaceGlobalsDefinition : TagStructure
	{
        [TagField(ValidTags = new[] { "wigl" })]
        public CachedTag SharedGlobals;
        [TagField(ValidTags = new[] { "goof" })]
        public CachedTag MpVariantSettingsUi;
        [TagField(ValidTags = new[] { "unic" })]
        public CachedTag GameHopperDescriptions;

        public List<ScreenWidget> ScreenWidgets;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public CachedTag TextureRenderList;

        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public CachedTag SwearFilter; // TODO: Version number

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public uint UnknownHO;

        [TagStructure(Size = 0x10)]
        public class ScreenWidget : TagStructure
		{
            [TagField(ValidTags = new[] { "scn3" })]
            public CachedTag Widget;
        }
    }
}