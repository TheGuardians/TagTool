using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3ODST)]
    public class CharacterIdleProperties : TagStructure
	{
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Unused;

        public Bounds<float> IdlePoseDelayTime; // time range for delays between idle poses (seconds)

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public Bounds<float> WanderDelayTime;
    }
}
