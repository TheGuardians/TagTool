using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Cache
{
    public abstract class CacheFileHeader :  TagStructure
    {
        public virtual bool IsValid()
        {
            if (GetHeadTag() == "head" && GetFootTag() == "foot")
                return true;
            else
                return false;
        }

        public static CacheFileHeader Read(CacheVersion version, EndianReader reader)
        {
            var deserializer = new TagDeserializer(version);
            reader.SeekTo(0);
            var dataContext = new DataSerializationContext(reader);

            switch (version)
            {
                case CacheVersion.HaloPC:
                case CacheVersion.HaloXbox:
                case CacheVersion.HaloCustomEdition:
                    return deserializer.Deserialize<CacheFileHeaderGen1>(dataContext);
                case CacheVersion.Halo2Beta:
                case CacheVersion.Halo2Xbox:
                case CacheVersion.Halo2Vista:
                    return deserializer.Deserialize<CacheFileHeaderGen2>(dataContext);
                case CacheVersion.Halo3Beta:
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3ODST:
                case CacheVersion.HaloReach:
                    return deserializer.Deserialize<CacheFileHeaderGen3>(dataContext);
                case CacheVersion.HaloOnline106708:
                case CacheVersion.HaloOnline235640:
                case CacheVersion.HaloOnline301003:
                case CacheVersion.HaloOnline327043:
                case CacheVersion.HaloOnline372731:
                case CacheVersion.HaloOnline416097:
                case CacheVersion.HaloOnline430475:
                case CacheVersion.HaloOnline454665:
                case CacheVersion.HaloOnline449175:
                case CacheVersion.HaloOnline498295:
                case CacheVersion.HaloOnline530605:
                case CacheVersion.HaloOnline532911:
                case CacheVersion.HaloOnline554482:
                case CacheVersion.HaloOnline571627:
                case CacheVersion.HaloOnline700123:
                    return deserializer.Deserialize<CacheFileHeaderGenHaloOnline>(dataContext);
                case CacheVersion.Halo4:
                    return deserializer.Deserialize<CacheFileHeaderGen4>(dataContext);
            }
            return null;
        }

        public abstract Tag GetHeadTag();
        public abstract Tag GetFootTag();
        public abstract ulong GetTagTableHeaderOffset();
        public abstract string GetName();
        public abstract string GetBuild();
        public abstract string GetScenarioPath();
        public abstract int GetScenarioTagIndex();
        public abstract CacheFileType GetCacheType();
        public abstract CacheFileSharedType GetSharedCacheType();
        public abstract StringIDHeader GetStringIDHeader();
        public abstract TagNameHeader GetTagNameHeader();
        public abstract TagMemoryHeader GetTagMemoryHeader();

    }

    [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo3Beta)]
    [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3Retail)]
    public class StringIDHeader
    {
        [TagField(MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo3Beta)]
        public uint BufferAlignedOffset;

        public int Count;
        public int BufferSize;
        public uint IndicesOffset;
        public uint BufferOffset;
    }

    [TagStructure(Size = 0x10)]
    public class TagNameHeader
    {
        public int TagNamesCount;
        public uint TagNamesBufferOffset;
        public int TagNamesBufferSize;
        public uint TagNameIndicesOffset;
    }

    [TagStructure(Size = 0xC, MaxVersion = CacheVersion.HaloCustomEdition)]
    [TagStructure(Size = 0xC, MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Xbox)]
    [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo2Vista, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x8, MinVersion = CacheVersion.Halo3Beta)]
    public class TagMemoryHeader
    {
        [TagField(MaxVersion = CacheVersion.HaloCustomEdition)]
        public int TagDataSize;

        public uint MemoryBufferOffset;
        public int MemoryBufferSize;

        [TagField(MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
        public int MemoryBufferCapacity;

        [TagField(MinVersion = CacheVersion.Halo2Vista, MaxVersion = CacheVersion.Halo2Vista)]
        public uint VirtualAddress;
    }
}
