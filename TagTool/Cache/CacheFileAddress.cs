using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Tags;

namespace TagTool.Cache
{
    [TagStructure]
    public class CacheFileAddress
    {
        [TagField(MaxVersion = CacheVersion.HaloReach)]
        public uint Value32;

        [TagField(MinVersion = CacheVersion.HaloReachMCC824)]
        public uint Value64;
    }
}
