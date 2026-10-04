using System.Collections.Generic;
using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x24)]
    public class BspGameAttachment : TagStructure
    {
        public List<BspAttachment> Static;
        public List<BspAttachment> Persistent;
        public List<BspAttachment> Dynamic;

        [TagStructure(Size = 0x10)]
        public class BspAttachment : TagStructure
        {
            public CachedTag Attachment;
        }
    }
}
