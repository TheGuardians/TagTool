using TagTool.Cache;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "effect_scenery", Tag = "efsc", Size = 0x0, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "effect_scenery", Tag = "efsc", Size = 0xC, MinVersion = CacheVersion.HaloOnlineED)]
    public class EffectScenery : GameObject
    {
        [TagField(Flags = Padding, Length = 12, MinVersion = CacheVersion.HaloOnlineED)]
        public byte[] Unused4;
    }
}
