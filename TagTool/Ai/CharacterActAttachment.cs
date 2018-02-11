using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x1C)]
    public class CharacterActAttachment
    {
        [TagField(Label = true)]
        public StringId Name;
        public CachedTagInstance ChildObject;
        public StringId ChildMarker;
        public StringId ParentMarker;
    }
}
