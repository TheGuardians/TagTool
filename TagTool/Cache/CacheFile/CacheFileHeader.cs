using TagTool.BlamFile;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache
{
    [TagStructure(Size = 0x800, MinVersion = CacheVersion.HaloXbox, MaxVersion = CacheVersion.Halo3Beta)]
    [TagStructure(Size = 0x3000, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x3390, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0xA000, MinVersion = CacheVersion.HaloReach)]
    public sealed class CacheFileHeader : TagStructure
    {
        public Tag HeaderSignature;

        public CacheFileVersion FileVersion;
        public int FileLength;
        public int FileCompressedLength;

        [TagField(Platform = CachePlatform.Only32Bit)]
        public uint TagsHeaderAddress32;
        [TagField(Platform = CachePlatform.Only64Bit)]
        public ulong TagsHeaderAddress64;

        [TagField(MaxVersion = CacheVersion.HaloPC)]
        public int TagDataSize;

        public uint MemoryBufferOffset;
        public int MemoryBufferSize;

        [TagField(MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
        public int MemoryBufferCapacity;

        [TagField(MinVersion = CacheVersion.Halo2Vista, MaxVersion = CacheVersion.Halo2Vista)]
        public uint VirtualAddress;

        [TagField(MinVersion = CacheVersion.Halo2Vista, MaxVersion = CacheVersion.Halo2Vista)]
        public int TagDependencyGraphOffset;

        [TagField(MinVersion = CacheVersion.Halo2Vista, MaxVersion = CacheVersion.Halo2Vista)]
        public uint TagDependencyGraphSize;

        [TagField(Length = 32, MaxVersion = CacheVersion.HaloPC)]
        public string NameOld;

        [TagField(Length = 256, MinVersion = CacheVersion.Halo2Beta)]
        public string SourceFile;

        [TagField(Length = 32)]
        public string Build;

        public CacheFileType CacheType;
        public CacheFileSharedType SharedType;

        [TagField(MaxVersion = CacheVersion.HaloPC)]
        public int UnknownHalo;

        [TagField(MaxVersion = CacheVersion.HaloPC, Length = 0x794)]
        public byte[] Padding;

        [TagField(MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
        public uint CacheResourceCRC;

        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public bool Unknown2;
        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public bool TrackedBuild;
        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public bool Unknown3;
        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public byte Unknown4;

        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public int Unknown5;
        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public int Unknown6;
        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public int Unknown7;
        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public int Unknown8;
        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public int Unknown9;

        [TagField(MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Vista)]
        public int StringIDsBufferAlignedOffset;

        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public int StringIDsCount;
        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public int StringIDsBufferSize;
        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public uint StringIDsIndicesOffset;
        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public uint StringIDsBufferOffset;

        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public int ExternalDependencies;

        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public ulong Timestamp;
        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public ulong MainMenuTimestamp;
        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public ulong SharedTimestamp;
        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public ulong CampaignTimestamp;
        [TagField(MinVersion = CacheVersion.HaloReachMCC0824)]
        public ulong MultiplayerTimestamp;
        
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown1HighDateTime;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown1LowDateTime;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown2HighDateTime;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown2LowDateTime;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown3HighDateTime;
        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown3LowDateTime;

        [TagField(Length = 0x20, MinVersion = CacheVersion.Halo2Beta)]
        public string Name;

        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public int Unknown13;

        [TagField(Length = 256, MinVersion = CacheVersion.Halo2Beta)]
        public string ScenarioPath;

        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public int MinorVersion;

        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public int TagNamesCount;
        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public uint TagNamesBufferOffset;
        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public int TagNamesBufferSize;
        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public uint TagNameIndicesOffset;

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

        [TagField(MinVersion = CacheVersion.Halo2Beta)]
        public uint Checksum;

        [TagField(MinVersion = CacheVersion.Halo2Vista, MaxVersion = CacheVersion.Halo2Vista)]
        public uint MoppCodesChecksum;

        [TagField(Length = 1320, MinVersion = CacheVersion.Halo2Beta, MaxVersion = CacheVersion.Halo2Xbox)]
        public byte[] UnknownsH2X = new byte[1320];

        [TagField(Length = 1284, MinVersion = CacheVersion.Halo2Vista, MaxVersion = CacheVersion.Halo2Vista)]
        public byte[] UnknownsH2V = new byte[1284];

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int Unknown14;
        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int Unknown15;

        [TagField(Gen = CacheGeneration.HaloOnline, Length = 0x20)] // should be 0x20 but they are unused and it matches better with the current def
        public byte[] UnknownHO1;

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
        public int Unknown21_1;
        [TagField(MinVersion = CacheVersion.HaloReachMCC0824)]
        public int Unknown21_2;

        [TagField(Gen = CacheGeneration.Third, Platform = CachePlatform.Only32Bit)]
        public uint VirtualBaseAddress32;
        [TagField(Gen = CacheGeneration.Third, Platform = CachePlatform.Only64Bit)]
        public ulong VirtualBaseAddress64;

        [TagField(Gen = CacheGeneration.Third)]
        public int XDKVersion;

        [TagField(MinVersion = CacheVersion.HaloReachMCC0824)]
        public int Unknown21_3;

        [TagField(Gen = CacheGeneration.HaloOnline, Length = 0x14)]
        public byte[] Hash;

        [TagField(Gen = CacheGeneration.HaloOnline, Length = 0x100)]
        public byte[] RSASignature;

        [TagField(Length = (int)CacheFilePartitionType.Count, MinVersion = CacheVersion.Halo3Retail)]
        public CacheFilePartition[] Partitions = new CacheFilePartition[(int)CacheFilePartitionType.Count];

        [TagField(Gen = CacheGeneration.Third)]
        public int CountUnknown1;

        [TagField(Gen = CacheGeneration.Third)]
        public int Unknown22;
        [TagField(Gen = CacheGeneration.Third)]
        public int Unknown23;
        [TagField(Gen = CacheGeneration.Third)]
        public int Unknown24;

        [TagField(Length = 5, Gen = CacheGeneration.Third)]
        public int[] SHA1_A;

        [TagField(Length = 5, Gen = CacheGeneration.Third)]
        public int[] SHA1_B;

        [TagField(Length = 5, Gen = CacheGeneration.Third)]
        public int[] SHA1_C;

        [TagField(Length = 64, Gen = CacheGeneration.Third)]
        public int[] RSA;

        [TagField(Gen = CacheGeneration.Third)]
        public CacheFileSectionTable SectionTable;

        [TagField(Length = 4, Gen = CacheGeneration.Third)]
        public int[] GUID;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown107;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public short Unknown108;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public short CountUnknown2;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int Unknown109;

        [TagField(Length = 4, Gen = CacheGeneration.Third)]
        public int[] CompressionGUID;

        [TagField(Length = 0x2300, MinVersion = CacheVersion.Halo3Retail)]
        public byte[] Elements1;

        [TagField(Length = 0x708, Gen = CacheGeneration.Third)]
        public byte[] Elements2;

        [TagField(Length = 0x12C, Gen = CacheGeneration.Third)]
        public byte[] Unknown114;

        [TagField(Gen = CacheGeneration.Third)]
        public uint Unknown115;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown116;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown117;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown118;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown119;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown120;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown121;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown122;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown123;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int Unknown124;

        [TagField(Gen = CacheGeneration.HaloOnline, Length = 0x168)]
        public byte[] Unknown125;

        [TagField(Gen = CacheGeneration.HaloOnline, Length = 0x4F0)]
        public byte[] Unknown126;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int MapId;

        [TagField(Gen = CacheGeneration.HaloOnline)]
        public int ScenarioTagIndex;

        [TagField(Gen = CacheGeneration.HaloOnline, Length = 0x598)]
        public byte[] Unknown127;

        [TagField(Length = 0x6FB8, MinVersion = CacheVersion.HaloReach)]
        public byte[] Unknown128;

        public Tag FooterSignature;

        //
        // Helper methods
        //

        public int GetHeaderSize(CacheVersion version) => (int)GetTagStructureInfo(typeof(CacheFileHeader), version).TotalSize;
    }
}