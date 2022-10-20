using System;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.MCC
{
    [TagStructure(Size = 0x4000, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0xA000, MinVersion = CacheVersion.HaloReach)]
    [TagStructure(Size = 0x1E000, MinVersion = CacheVersion.Halo4, MaxVersion = CacheVersion.Groundhog)]
    public class CacheFileHeaderMCC : CacheFileHeader
    {
        public Tag HeaderSignature;
        public CacheFileVersion FileVersion;
        public uint FileLength;
        public HaloEngineVersion EngineVersion;
        public sbyte PlatformType; // Xbox One = 1, PC = 2
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public TagMemoryHeader TagMemoryHeader;
        public CacheFileType CacheType;
        public CacheFileSharedType SharedCacheType;
        public ushort CacheFlags;
        public byte ValidSharedFileMask;
        public byte Unused1;
        public TagNameHeader TagNamesHeader;
        public StringIDHeaderMCC StringIdsHeader;
        public int Language;
        public int MinorVersion;
        [TagField(Length = 6)]
        public ulong[] Timestamps;
        [TagField(Length = 32)]
        public byte[] CreatorId;
        [TagField(Length = 32)]
        public string Build;
        [TagField(Length = 32)]
        public string Name;
        [TagField(Length = 256)]
        public string ScenarioPath;
        [TagField(Length = 256)]
        public string SourceFile;
        public PlatformUnsignedValue VirtualBaseAddress;
        public PlatformUnsignedValue TagTableHeaderOffset;
        [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
        public byte[] Unused3;
        [TagField(Length = (int)CacheFilePartitionType.Count)]
        public CacheFilePartition[] Partitions = new CacheFilePartition[(int)CacheFilePartitionType.Count];
        public uint Checksum;
        public uint ContentHashMask;
        public ulong SecuritySalt;
        [TagField(Length = 0x14)]
        public byte[] ContentHash1;
        [TagField(Length = 0x14)]
        public byte[] ContentHash2;
        [TagField(Length = 0x14)]
        public byte[] ContentHash3;
        [TagField(Length = 0x20)]
        public byte[] RSAKeyBlobHash;
        [TagField(Length = 0x100)]
        public byte[] RSASignature;
        public CacheFileSectionTable SectionTable;
        [TagField(Length = 0x3B00, MaxVersion = CacheVersion.Halo3Retail)]
        [TagField(Length = 0x9B00, MinVersion = CacheVersion.HaloReach)]
        public byte[] Unknown1;
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

        [TagStructure(Size = 0x18, MinVersion = CacheVersion.Halo3Retail)]
        public class StringIDHeaderMCC
        {
            public int Count;
            public uint BufferOffset;
            public int BufferSize;
            public uint IndicesOffset;
            public int NamespacesCount;
            public uint NamespacesOffset;
        }

        public enum HaloEngineVersion : sbyte
        {
            Halo1,
            Unknown1,
            Halo3,
            Unknown3,
            Unknown4,
            Halo3ODST,
            HaloReach,
            Halo4,
            Groundhog
        }
    }
}
