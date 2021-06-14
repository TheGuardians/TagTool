using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "entity", Tag = "ents", Size = 0x4)]
    public class Entity : GameObject
    {
        public float EntityPlaceholder;
    }
}
