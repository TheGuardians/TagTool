using TagTool.Cache;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "swear_filter", Tag = "sweg", Size = 0xC)]
    public class SwearFilter : TagStructure
    {
        public List<FilterList> FilterLists;

        [TagStructure(Size = 0x10)]
        public class FilterList : TagStructure
        {
            [TagField(ValidTags = new[] { "swel" })]
            public CachedTag List;
        }
    }
}
