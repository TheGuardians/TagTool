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
        public override uint DefinitionOffset => throw new NotImplementedException();

        public CachedTagGen1(int index, uint ID, TagGroup tagGroup, string name) : base(index, tagGroup, name)
        {
            this.ID = ID;
        }
    }
}
