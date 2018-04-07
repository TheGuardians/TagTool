using TagTool.Cache;
using TagTool.Serialization;


namespace TagTool.Audio
{
    [TagStructure(Size = 0x3, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x4, MinVersion = CacheVersion.HaloOnline106708)]
    public class PlatformCodec
    {
        [TagField(HaloOnlineOnly = true)]
        public short Unknown;

        [TagField(Gen3Only = true)]
        public SampleRate SampleRate;

        public Encoding Encoding;

        public Compression Compression;
    }
}