using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Tags;

namespace TagTool.Cache.Gen2
{
    public class CachedTagGen2 : CachedTag
    {
        public override uint DefinitionOffset => Offset;
        public int Size;
        public uint Offset;
        public bool IsExternal;

        public CachedTagGen2(int index, uint ID, TagGroupGen2 tagGroup, uint address, int size, string name, bool isExternal) : base(index, tagGroup, name)
        {
            this.ID = ID;
            Offset = address;
            Size = size;
            IsExternal = isExternal;
        }
    }
}
