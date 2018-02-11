using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "bink", Tag = "bink", Size = 0x18, MaxVersion = CacheVersion.HaloOnline571627)]
    [TagStructure(Name = "bink", Tag = "bink", Size = 0x14, MinVersion = CacheVersion.HaloOnline700123)]
    public class Bink
    {
        public int FrameCount;
        [TagField(Pointer = true)]
        public PageableResource Resource;
        public int UselessPadding;
        public uint Unknown;
        public uint Unknown2;

        [TagField(MaxVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown3;
    }
}