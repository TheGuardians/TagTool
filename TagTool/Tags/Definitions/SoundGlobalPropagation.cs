using TagTool.Cache;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_global_propagation", Tag = "sgp!", Size = 0x48, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "sound_global_propagation", Tag = "sgp!", Size = 0x50, MinVersion = CacheVersion.HaloOnline106708)]
    public class SoundGlobalPropagation : TagStructure
	{
        public CachedTagInstance UnderwaterEnvironment;
        public CachedTagInstance UnderwaterLoop;
        public uint Unknown;
        public uint Unknown2;
        public CachedTagInstance EnterUnderater;
        public CachedTagInstance ExitUnderwater;

        [TagField(Padding = true, Length = 8, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;
    }
}