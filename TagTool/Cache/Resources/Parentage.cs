using System;
using System.Collections.Generic;
using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x2C)]
    public class Parentage : TagStructure
    {
        public CachedTag Tag;
        public ParentageFlags Flags;
        public short ResourceOwnerIndex;

        public List<ParentageReference> Parents;
        public List<ParentageReference> Children;

        [Flags]
        public enum ParentageFlags : short
        {
            None = 0,
            LoadedByGame = 1 << 0,
            Unloaded = 1 << 1,
        }

        [TagStructure(Size = 0x4)]
        public class ParentageReference : TagStructure
        {
            public int Link;
        }
    }
}
