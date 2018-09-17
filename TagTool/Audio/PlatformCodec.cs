using TagTool.Cache;
using TagTool.Serialization;


namespace TagTool.Audio
{
    [TagStructure(Size = 0x3, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x4, MinVersion = CacheVersion.HaloOnline106708)]
    public class PlatformCodec
    {
        [TagField(HaloOnlineOnly = true)]
        public byte Unknown1;

        /// <summary>
        /// Should be 0 in most cases. Seems to be used to determine streaming or loading.
        /// </summary>
        [TagField(HaloOnlineOnly = true)]
        public byte LoadMode;

        [TagField(Gen3Only = true)]
        public SampleRate SampleRate;

        public Encoding Encoding;

        public Compression Compression;
    }
}