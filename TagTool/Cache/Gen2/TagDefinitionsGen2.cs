using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache.Resources;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Gen2
{
    public class TagDefinitionsGen2 : TagDefinitionsNew
    {
        public Dictionary<TagGroupNew, Type> Gen1Types = new Dictionary<TagGroupNew, Type>
        {

        };

        public override Dictionary<TagGroupNew, Type> Types { get => Gen1Types; }
    }

}
