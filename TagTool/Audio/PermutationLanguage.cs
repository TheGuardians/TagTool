using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Tags;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x10, Platform = Cache.CachePlatform.Original)]
    [TagStructure(Size = 0x18, Platform = Cache.CachePlatform.MCC)]
    public class PermutationLanguage : TagStructure
    {
        [TagField(Platform = Cache.CachePlatform.MCC)]
        public int SourceLanguage;
        [TagField(Platform = Cache.CachePlatform.MCC)]
        public int DestinationLanguage;

        public uint UncompressedSampleCount;
        public List<PermutationChunk> Chunks;
    }
}
