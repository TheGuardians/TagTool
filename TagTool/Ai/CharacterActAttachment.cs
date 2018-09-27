using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x1C)]
    public class CharacterActAttachment : TagStructure
	{
        [TagField(Label = true)]
        public StringId Name;
        public CachedTagInstance ChildObject;
        public StringId ChildMarker;
        public StringId ParentMarker;
    }
}
