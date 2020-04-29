using System;
using System.Collections.Generic;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions.Gen1;

namespace TagTool.Cache.Gen1
{
    public class TagDefinitionsGen1 : TagDefinitions
    {
        public  Dictionary<TagGroup, Type> Gen1Types = new Dictionary<TagGroup, Type>
        {
            { new TagGroupGen1("snd!"), typeof(Sound) },
            { new TagGroupGen1("antr"), typeof(ModelAnimationGraph) },
        };

        public override Dictionary<TagGroup, Type> Types { get => Gen1Types; }
    }
}
