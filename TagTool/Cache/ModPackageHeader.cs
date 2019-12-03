using TagTool.Common;
using TagTool.Tags;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Cache
{
    [TagStructure(Size = 0x40)]
    public class ModPackageHeader
    {
        public Tag Signature = new Tag("mod!");
        public uint Version = ModPackageSimplified.FormatVersion;

        [TagField(Flags = Padding, Length = 12)]
        public byte[] Unused = new byte[12];

        [TagField(Length = 20)]
        public byte[] SHA1 = new byte[20];

        public uint TagCacheOffset;

        public uint TagNamesTableOffset;
        public int TagNamesTableCount;

        public uint ResourceCacheOffset;

        public uint MapFileTableOffset;
        public int MapFileTableCount;
    }
}