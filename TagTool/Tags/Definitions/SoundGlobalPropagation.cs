using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_global_propagation", Tag = "sgp!", Size = 0x48, MinVersion = CacheVersion.Halo3Retail)]
    public class SoundGlobalPropagation : TagStructure
	{
        public CachedTagInstance UnderwaterEnvironment;
        public CachedTagInstance UnderwaterLoop;
        public uint Unknown;
        public uint Unknown2;
        public CachedTagInstance EnterUnderater;
        public CachedTagInstance ExitUnderwater;
    }
}