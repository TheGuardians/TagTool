using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "sound_global_propagation", Tag = "sgp!", Size = 0x48, MinVersion = CacheVersion.Halo3Retail)]
    public class SoundGlobalPropagation : TagStructure
	{
        public CachedTag UnderwaterEnvironment;
        public CachedTag UnderwaterLoop;
        public float BackgroundSoundGain; // scale for fog background sound (dB)
        public float EnvironmentDuckingAmount; // scales the surrounding background sound by this much (dB)
        public CachedTag EnterWaterSound;
        public CachedTag ExitWaterSound;
    }
}
