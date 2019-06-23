using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Tags;

namespace TagTool.Cache
{
    [TagStructure(Size = 0x20)]
    public class HaloOnlineTagsHeader
    {
        [TagField(Length = 0x4)]
        public byte[] Padding1;

        public int TableOffset;

        [TagField(Length = 0x8)]
        public byte[] Padding2;

        public long GUID = 0x1D0631BCC791704;

        [TagField(Length = 0x8)]
        public byte[] Padding3;
    }

    [TagStructure(Size = 0x20)]
    public class HaloOnlineResourceHeader
    {
        [TagField(Length = 0x4)]
        public byte[] Padding1;

        public int TableOffset;

        public int ResourceCount;

        [TagField(Length = 0x4)]
        public byte[] Padding2;

        public long GUID = 0x1D0631BCC92931B;

        [TagField(Length = 0x8)]
        public byte[] Padding3;
    }

    [TagStructure(Size = 0x8)]
    public class HaloOnlineStringIdsHeader
    {
        public int StringIdCount;
        public int Length;
    }
}
