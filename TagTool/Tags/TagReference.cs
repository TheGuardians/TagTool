using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Common
{
    [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3Retail)]
    public class TagReference : TagStructure
    {
        public Tag Group;

        [TagField(MinVersion = CacheVersion.Halo3Beta)]
        public int Unused1;
        [TagField(MinVersion = CacheVersion.Halo3Beta)]
        public int Unused2;

        public int Index;

        [TagField(Flags = TagFieldFlags.Runtime)]
        public string Name;
    }
}
