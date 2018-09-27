using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Tags;


namespace TagTool.Audio
{
    [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3ODST)]
    public class UnknownBlock : TagStructure
	{
        public uint Unknown;
        public List<UnknownBlock2> Unknown2;

        [TagStructure(Size = 0x4)]
        public class UnknownBlock2 : TagStructure
		{
            public uint Unknown;
        }
    }
}