using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Cache
{
    [TagStructure(Size = 0x800, MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x3000, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
    public sealed class CacheFileHeader
    {
        [TagField(Local = true)]
        public int Magic;

        public Tag HeadTag;
        public int Version;
        public int FileLength;
        public int Unknown1;
        public uint TagIndexAddress;
        public int MemoryBufferOffset;
        public int MemoryBufferSize;

        [TagField(MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista)]
        public int MemoryBufferCapacity;

        [TagField(MinVersion = CacheVersion.Halo2Vista, MaxVersion = CacheVersion.Halo2Vista)]
        public uint VirtualAddress;

        [TagField(MinVersion = CacheVersion.Halo2Vista, MaxVersion = CacheVersion.Halo2Vista)]
        public int TagDependencyGraphOffset;

        [TagField(MinVersion = CacheVersion.Halo2Vista, MaxVersion = CacheVersion.Halo2Vista)]
        public uint TagDependencyGraphSize;

        [TagField(Length = 256)]
        public string SourceFile;

        [TagField(Length = 32)]
        public string Build;

        public CacheFileType CacheType;
        public CacheFileSharedType SharedType;

        [TagField(MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista)]
        public uint CacheResourceCRC;

        public bool Unknown2;
        public bool TrackedBuild;
        public bool Unknown3;
        public byte Unknown4;

        public int Unknown5;
        public int Unknown6;
        public int Unknown7;
        public int Unknown8;
        public int Unknown9;

        [TagField(MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista)]
        public int StringIDsBufferAlignedOffset;

        public int StringIDsCount;
        public int StringIDsBufferSize;
        public int StringIDsIndicesOffset;
        public int StringIDsBufferOffset;

        public int ExternalDependencies;

        public int HighDateTime;
        public int LowDateTime;
        public int MainMenuHighDateTime;
        public int MainMenuLowDateTime;
        public int SharedHighDateTime;
        public int SharedLowDateTime;
        public int CampaignHighDateTime;
        public int CampaignLowDateTime;

        [TagField(Length = 32)]
        public string Name;

        public int Unknown13;

        [TagField(Length = 256)]
        public string ScenarioPath;

        public int MinorVersion;

        public int TagNamesCount;
        public int TagNamesBufferOffset;
        public int TagNamesBufferSize;
        public int TagNamesIndicesOffset;

        [TagField(MinVersion = CacheVersion.Halo2Vista, MaxVersion = CacheVersion.Halo2Vista)]
        public int LanguagePacksOffset = -1;

        [TagField(MinVersion = CacheVersion.Halo2Vista, MaxVersion = CacheVersion.Halo2Vista)]
        public uint LanguagePacksSize = 0;

        [TagField(MinVersion = CacheVersion.Halo2Vista, MaxVersion = CacheVersion.Halo2Vista)]
        public uint SecondarySoundGestaltDatumIndex = uint.MaxValue;

        [TagField(MinVersion = CacheVersion.Halo2Vista, MaxVersion = CacheVersion.Halo2Vista)]
        public int FastLoadGeometryBlockOffset = -1;

        [TagField(MinVersion = CacheVersion.Halo2Vista, MaxVersion = CacheVersion.Halo2Vista)]
        public uint FastLoadGeometryBlockSize = 0;

        public uint Checksum;

        [TagField(MinVersion = CacheVersion.Halo2Vista, MaxVersion = CacheVersion.Halo2Vista)]
        public uint MoppCodesChecksum;

        [TagField(Length = 1320, MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Xbox)]
        public byte[] UnknownsH2X = new byte[1320];

        [TagField(Length = 1284, MinVersion = CacheVersion.Halo2Vista, MaxVersion = CacheVersion.Halo2Vista)]
        public byte[] UnknownsH2V = new byte[1284];

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int Unknown14;
        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int Unknown15;
        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int Unknown16;
        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int Unknown17;
        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int Unknown18;
        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int Unknown19;
        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int Unknown20;
        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int Unknown21;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public uint BaseAddress;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int XDKVersion;

        [TagField(Length = (int)CacheFilePartitionType.Count, MinVersion = CacheVersion.Halo3Retail)]
        public CacheFilePartition[] Partitions = new CacheFilePartition[(int)CacheFilePartitionType.Count];

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int CountUnknown1;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int Unknown22;
        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int Unknown23;
        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int Unknown24;

        [TagField(Length = 5, MinVersion = CacheVersion.Halo3Retail)]
        public int[] SHA1_A;

        [TagField(Length = 5, MinVersion = CacheVersion.Halo3Retail)]
        public int[] SHA1_B;

        [TagField(Length = 5, MinVersion = CacheVersion.Halo3Retail)]
        public int[] SHA1_C;

        [TagField(Length = 64, MinVersion = CacheVersion.Halo3Retail)]
        public int[] RSA;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public CacheFileInterop Interop;

        [TagField(Length = 4, MinVersion = CacheVersion.Halo3Retail)]
        public int[] GUID;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public short Unknown108;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public short CountUnknown2;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int Unknown109;

        [TagField(Length = 4, MinVersion = CacheVersion.Halo3Retail)]
        public int[] CompressionGUID;

        [TagField(Length = 0x2300, MinVersion = CacheVersion.Halo3Retail)]
        public byte[] Elements1;

        [TagField(Length = 0x708, MinVersion = CacheVersion.Halo3Retail)]
        public byte[] Elements2;

        [TagField(Length = 0x12C, MinVersion = CacheVersion.Halo3Retail)]
        public byte[] Unknown114;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public uint Unknown115;

        public Tag FootTag;
    }
}