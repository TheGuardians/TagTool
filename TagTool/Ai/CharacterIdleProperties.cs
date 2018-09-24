using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Ai
{
    [TagStructure(Size = 0xC, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3ODST)]
    public class CharacterIdleProperties : TagStructure
	{
        [TagField(Padding = true, Length = 4)]
        public byte[] Unused;

        public Bounds<float> IdlePoseDelayTime;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public Bounds<float> WanderDelayTime;
    }
}
