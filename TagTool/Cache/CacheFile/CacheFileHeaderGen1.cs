using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Cache
{
    [TagStructure(Size = 0x800, MinVersion = CacheVersion.HaloXbox, MaxVersion = CacheVersion.HaloCustomEdition)]
    public class CacheFileHeaderGen1 : CacheFileHeader
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

        [TagField(Length = 32)]
        public string Name;

        [TagField(Length = 32)]
        public string Build;

        public CacheFileType CacheType;
        public CacheFileSharedType SharedCacheType;
        public int Unknown;

        [TagField(Length = 0x794)]
        public byte[] Padding;

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
        public override StringIDHeader GetStringIDHeader() => null;
        public override TagNameHeader GetTagNameHeader() => null;
        public override TagMemoryHeader GetTagMemoryHeader() => TagMemoryHeader;
        public override int GetScenarioTagIndex() => -1;
    }
}
