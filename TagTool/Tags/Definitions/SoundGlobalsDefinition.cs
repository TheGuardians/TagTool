using TagTool.Cache;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Size = 0x50, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x60, MinVersion = CacheVersion.HaloOnline106708)]
    public class SoundGlobalsDefinition : TagStructure
	{
        [TagField(ValidTags = new[] { "sncl" })]
        public CachedTagInstance SoundClasses;

        [TagField(ValidTags = new[] { "sfx+" })]
        public CachedTagInstance SoundEffects;

        [TagField(ValidTags = new[] { "snmx" })]
        public CachedTagInstance SoundMix;

        [TagField(ValidTags = new[] { "spk!" })]
        public CachedTagInstance SoundCombatDialogueConstants;

        [TagField(ValidTags = new[] { "sgp!" })]
        public CachedTagInstance SoundGlobalPropagation;

        [TagField(ValidTags = new[] { "sus!" }, MinVersion = CacheVersion.HaloOnline106708)]
        public CachedTagInstance GfxUiSounds;
    }
}