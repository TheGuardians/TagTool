using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.TagResources
{
    [TagStructure(Name = "bink_resource", Size = 0x14)]
    public class BinkResource
    {
        public TagData Data;
    }
}