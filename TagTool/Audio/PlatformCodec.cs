using TagTool.Cache;
using TagTool.Tags;


namespace TagTool.Audio
{
    [TagStructure(Size = 0x3, MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x4, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x3, MaxVersion = CacheVersion.HaloReach11883)]
    public class PlatformCodec : TagStructure
	{
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public byte Unknown1;

        /// <summary>
        /// Should be 0 in most cases. Seems to be used to determine streaming or loading.
        /// </summary>
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public byte LoadMode;

        [TagField(Gen = CacheGeneration.Third)]
        public SampleRate SampleRate;

        public EncodingValue Encoding;

        public Compression Compression;
    }
}