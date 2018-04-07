using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Serialization;


namespace TagTool.Audio
{
    [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3ODST)]
    public class UnknownBlock
    {
        public uint Unknown;
        public List<UnknownBlock2> Unknown2;

        [TagStructure(Size = 0x4)]
        public class UnknownBlock2
        {
            public uint Unknown;
        }
    }
}