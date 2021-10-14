using System;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.MCC
{
    [TagStructure(Size = 0x4000)]
    public class CacheFileHeaderMCC : CacheFileHeader
    {
        public Tag HeaderSignature;
        public CacheFileVersion FileVersion;
        public uint FileLength;
        public HaloEngineVersion EngineVersion;
        [TagField(Align = 4)]
        public TagMemoryHeader TagMemoryHeader;
        public CacheFileType CacheType;
        public CacheFileSharedType SharedCacheType;
        public uint CacheFlags;
        public TagNameHeader TagNamesHeader;
        public StringIDHeaderMCC StringIdsHeader;
        public uint Unknown17;
        public uint Unknown18;
        public uint Unknown19;
        public uint Unknown20;
        [TagField(Length = 7)]
        public ulong[] Timestamps;
        public uint Unknown22;
        public uint Unknown23;
        public uint Unknown24;
        public uint Unknown25;
        public uint Unknown26;
        public uint Unknown27;
        [TagField(Length = 32)]
        public string Build;
        [TagField(Length = 32)]
        public string Name;
        [TagField(Length = 256)]
        public string SourceFile;
        [TagField(Length = 256)]
        public string ScenarioPath;
        public PlatformUnsignedValue VirtualBaseAddress;
        public PlatformUnsignedValue TagTableHeaderOffset;
        public uint Unknown28;
        public uint Unknown29;
        public uint Unknown30;
        public uint Unknown31;
        [TagField(Length = (int)CacheFilePartitionType.Count)]
        public CacheFilePartition[] Partitions = new CacheFilePartition[(int)CacheFilePartitionType.Count];
        public uint Unknown32;
        public uint Unknown33;
        [TagField(Length = 0x44)]
        public byte[] Unknown34;
        [TagField(Length = 0x20)]
        public byte[] Unknown35;
        [TagField(Length = 0x100)]
        public byte[] Unknown36;
        public CacheFileSectionTable SectionTable;
        [TagField(Length = 0x3B00)]
        public byte[] Unknown37;
        public Tag FooterSiganture;

        public override string GetBuild() => Build;

        public override CacheFileType GetCacheType() => CacheType;

        public override Tag GetFootTag() => FooterSiganture;

        public override Tag GetHeadTag() => HeaderSignature;

        public override string GetName() => Name;

        public override string GetScenarioPath() => ScenarioPath;

        public override int GetScenarioTagIndex() => -1;

        public override CacheFileSharedType GetSharedCacheType() => SharedCacheType;

        // TODO: clean up
        public override StringIDHeader GetStringIDHeader() => new StringIDHeader()
        {
            Count = StringIdsHeader.Count,
            IndicesOffset = StringIdsHeader.IndicesOffset,
            BufferOffset = StringIdsHeader.BufferOffset,
            BufferSize = StringIdsHeader.BufferSize
        };

        public override TagMemoryHeader GetTagMemoryHeader() => TagMemoryHeader;

        public override TagNameHeader GetTagNameHeader() => TagNamesHeader;

        public override ulong GetTagTableHeaderOffset() => throw new NotImplementedException();

        [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3Retail)]
        public class StringIDHeaderMCC
        {
            public int Count;
            public uint BufferOffset;
            public int BufferSize;
            public uint IndicesOffset;
        }

        public enum HaloEngineVersion : sbyte
        {
            Halo1,
            Unknown1,
            Halo3,
            Unknown3,
            Unknown4,
            Halo3ODST,
            HaloReach
        }
    }
}
