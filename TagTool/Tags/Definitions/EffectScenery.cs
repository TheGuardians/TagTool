using TagTool.Cache;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "effect_scenery", Tag = "efsc", Size = 0x0, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "effect_scenery", Tag = "efsc", Size = 0xC, MinVersion = CacheVersion.HaloOnline106708)]
    public class EffectScenery : GameObject
    {
        [TagField(Padding = true, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused4;
    }
}
