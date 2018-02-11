using TagTool.Cache;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "new_cinematic_lightning", Tag = "nclt", Size = 0x1C, MinVersion = CacheVersion.Halo3Retail)]
    public class NewCinematicLightning
    {
        public List<UnknownBlock> Unknown1;
        public List<LightningBlock> Lightning;
        public float Unknown2;

        [TagStructure(Size = 0x20)]
        public class UnknownBlock
        {
            public int Unknown1;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;
            public float Unknown8;
        }

        [TagStructure(Size = 0x20)]
        public class LightningBlock
        {
            public float Unknown1;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public CachedTagInstance Light;
        }
    }
}