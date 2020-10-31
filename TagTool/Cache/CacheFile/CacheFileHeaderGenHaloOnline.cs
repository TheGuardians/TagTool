using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Cache
{
    [TagStructure(Size = 0x3390, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
    public class CacheFileHeaderGenHaloOnline : CacheFileHeader
    {
        public Tag HeaderSignature;

        public CacheFileVersion FileVersion;
        public int FileLength;
        public int FileCompressedLength;

        public uint TagTableHeaderOffset;

        public TagMemoryHeader TagMemoryHeader;


        [TagField(Length = 256)]
        public string SourceFile;

        [TagField(Length = 32)]
        public string Build;

        public CacheFileType CacheType;
        public CacheFileSharedType SharedCacheType;

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


        public int Unknown1HighDateTime;

        public int Unknown1LowDateTime;

        public int Unknown2HighDateTime;

        public int Unknown2LowDateTime;

        public int Unknown3HighDateTime;

        public int Unknown3LowDateTime;

        [TagField(Length = 0x20)]
        public string Name;


        public int Unknown13;

        [TagField(Length = 256)]
        public string ScenarioPath;

        public int MinorVersion;

        public TagNameHeader TagNamesHeader;


        public uint Checksum;

        
        public int Unknown14;
        public int Unknown15;

        [TagField(Length = 0x20)]
        public byte[] UnknownHO1;


        public int Unknown16;

        public int Unknown17;

        public int Unknown18;

        public int Unknown19;

        public int Unknown20;

        public int Unknown21_1;


        
        [TagField(Length = 0x14)]
        public byte[] Hash;

        [TagField(Length = 0x100)]
        public byte[] RSASignature;

        [TagField(Length = (int)CacheFilePartitionType.Count, MinVersion = CacheVersion.Halo3Retail)]
        public CacheFilePartition[] Partitions = new CacheFilePartition[(int)CacheFilePartitionType.Count];


       

        public int Unknown107;


        public short Unknown108;


        public short CountUnknown2;


        public int Unknown109;

        
        [TagField(Length = 0x2300)]
        public byte[] Elements1;



        public int Unknown116;


        public int Unknown117;


        public int Unknown118;


        public int Unknown119;


        public int Unknown120;


        public int Unknown121;


        public int Unknown122;


        public int Unknown123;


        public int Unknown124;

        [TagField(Length = 0x168)]
        public byte[] Unknown125;

        [TagField(Length = 0x4F0)]
        public byte[] Unknown126;


        public int MapId;


        public int ScenarioTagIndex;

        [TagField(Length = 0x598)]
        public byte[] Unknown127;

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
        public override int GetScenarioTagIndex() => ScenarioTagIndex;
    }
}
