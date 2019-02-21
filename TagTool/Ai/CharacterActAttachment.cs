using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x1C)]
    public class CharacterActAttachment : TagStructure
	{
        [TagField(Flags = TagFieldFlags.Label)]
        public StringId ActivityName;
        public CachedTagInstance Crate;
        public StringId CrateMarkerName;
        public StringId UnitMarkerName;
    }
}
