using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Cache
{
    [TagStructure(Size = 0x800, MinVersion = CacheVersion.Halo2Alpha, MaxVersion = CacheVersion.Halo2Vista)]
    public class CacheFileHeaderGen2 : CacheFileHeader
    {
        //
        // Header definition
        //

        public Tag HeaderSignature;
        public CacheFileVersion FileVersion;
        public int FileLength;
        public int FileCompressedLength;
        public uint TagTableHeaderOffset;
        public TagMemoryHeader TagMemoryHeader;

        [TagField(MinVersion = CacheVersion.Halo2Vista)]
        public int TagDependencyGraphOffset;
        [TagField(MinVersion = CacheVersion.Halo2Vista)]
        public uint TagDependencyGraphSize;


        [TagField(Length = 256)]
        public string SourceFile;

        [TagField(Length = 32)]
        public string Build;

        public CacheFileType CacheType;
        public CacheFileSharedType SharedCacheType;
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

        public StringIDHeader StringIdsHeader;

        public int ExternalDependencies;
        public ulong Timestamp;
        public ulong MainMenuTimestamp;
        public ulong SharedTimestamp;
        public ulong CampaignTimestamp;       
        [TagField(Length = 0x20)]
        public string Name;
        public int Unknown13;
        [TagField(Length = 256)]
        public string ScenarioPath;
        public int MinorVersion;

        public TagNameHeader TagNamesHeader;

        [TagField(MinVersion = CacheVersion.Halo2Vista)]
        public int LanguagePacksOffset = -1;

        [TagField(MinVersion = CacheVersion.Halo2Vista)]
        public uint LanguagePacksSize = 0;

        [TagField(MinVersion = CacheVersion.Halo2Vista)]
        public uint SecondarySoundGestaltDatumIndex = uint.MaxValue;

        [TagField(MinVersion = CacheVersion.Halo2Vista)]
        public int FastLoadGeometryBlockOffset = -1;

        [TagField(MinVersion = CacheVersion.Halo2Vista)]
        public uint FastLoadGeometryBlockSize = 0;

        public uint Checksum;

        [TagField(MinVersion = CacheVersion.Halo2Vista)]
        public uint MoppCodesChecksum;

        [TagField(Length = 1320, MinVersion = CacheVersion.Halo2Alpha, MaxVersion = CacheVersion.Halo2Xbox)]
        public byte[] UnknownsH2X = new byte[1320];

        [TagField(Length = 1284, MinVersion = CacheVersion.Halo2Vista, MaxVersion = CacheVersion.Halo2Vista)]
        public byte[] UnknownsH2V = new byte[1284];

        public Tag FooterSignature;

        //
        // overrides
        //

        public override Tag GetFootTag() => FooterSignature;
        public override Tag GetHeadTag() => HeaderSignature;
        public override ulong GetTagTableHeaderOffset() => TagTableHeaderOffset;
        public override string GetName() => Name;
        public override string GetBuild() => Build;
        public override string GetScenarioPath() => null;
        public override CacheFileType GetCacheType() => CacheType;
        public override CacheFileSharedType GetSharedCacheType() => SharedCacheType;
        public override StringIDHeader GetStringIDHeader() => StringIdsHeader;
        public override TagNameHeader GetTagNameHeader() => TagNamesHeader;
        public override TagMemoryHeader GetTagMemoryHeader() => TagMemoryHeader;
        public override int GetScenarioTagIndex() => -1;
    }
}
