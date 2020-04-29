using System;
using System.Collections.Generic;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions.Gen1;

namespace TagTool.Cache.Gen1
{
    public class TagDefinitionsGen1 : TagDefinitionsNew
    {
        public  Dictionary<TagGroupNew, Type> Gen1Types = new Dictionary<TagGroupNew, Type>
        {
            { new TagGroupGen1("snd!"), typeof(Sound) },
            { new TagGroupGen1("antr"), typeof(ModelAnimationGraph) },
        };

        public override Dictionary<TagGroupNew, Type> Types { get => Gen1Types; }
    }
}
