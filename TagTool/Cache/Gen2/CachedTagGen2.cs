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

        public CachedTagGen2(int index, uint ID, TagGroup tagGroup, uint address, int size, string name) : base(index, tagGroup, name)
        {
            Offset = address;
            Size = size;
        }
    }
}
