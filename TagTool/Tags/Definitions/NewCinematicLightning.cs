using TagTool.Cache;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "new_cinematic_lighting", Tag = "nclt", Size = 0x1C, MinVersion = CacheVersion.Halo3Retail)]
    public class NewCinematicLighting : TagStructure
	{
        public List<UnknownBlock> Unknown1;
        public List<LightingBlock> Lighting;
        public float Unknown2;

        [TagStructure(Size = 0x20)]
        public class UnknownBlock : TagStructure
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
        public class LightingBlock : TagStructure
		{
            public float Unknown1;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public CachedTagInstance Light;
        }
    }
}