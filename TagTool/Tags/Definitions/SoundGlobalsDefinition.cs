using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Size = 0x50, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x60, MinVersion = CacheVersion.HaloOnline106708)]
    public class SoundGlobalsDefinition : TagStructure
	{
        [TagField(ValidTags = new[] { "sncl" })]
        public CachedTag SoundClasses;

        [TagField(ValidTags = new[] { "sfx+" })]
        public CachedTag SoundEffects;

        [TagField(ValidTags = new[] { "snmx" })]
        public CachedTag SoundMix;

        [TagField(ValidTags = new[] { "spk!" })]
        public CachedTag SoundCombatDialogueConstants;

        [TagField(ValidTags = new[] { "sgp!" })]
        public CachedTag SoundGlobalPropagation;

        [TagField(ValidTags = new[] { "sus!" }, MinVersion = CacheVersion.HaloOnline106708)]
        public CachedTag GfxUiSounds;
    }
}