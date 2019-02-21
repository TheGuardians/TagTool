using TagTool.Cache;
using TagTool.Tags;


namespace TagTool.Audio
{
    [TagStructure(Size = 0x3, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x4, MinVersion = CacheVersion.HaloOnline106708)]
    public class PlatformCodec : TagStructure
	{
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public byte Unknown1;

        /// <summary>
        /// Should be 0 in most cases. Seems to be used to determine streaming or loading.
        /// </summary>
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public byte LoadMode;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public SampleRate SampleRate;

        public EncodingValue Encoding;

        public Compression Compression;
    }
}