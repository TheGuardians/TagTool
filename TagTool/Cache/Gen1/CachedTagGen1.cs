using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Tags;

namespace TagTool.Cache.Gen1
{
    public class CachedTagGen1 : CachedTag
    {
        public uint Offset;

        public override uint DefinitionOffset => Offset;

        public CachedTagGen1(int index, uint ID, TagGroupGen1 tagGroup, uint address, string name) : base(index, tagGroup, name)
        {
            this.ID = ID;
            Offset = address;
        }
    }
}
