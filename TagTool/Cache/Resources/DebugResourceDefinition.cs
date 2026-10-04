using System.Collections.Generic;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0xC)]
    public class DebugResourceDefinition : TagStructure
    {
        public List<ResourceCategory> Categories;

        [TagStructure(Size = 0x4)]
        public class ResourceCategory : TagStructure
        {
            public StringId Name;
        }
    }
}
