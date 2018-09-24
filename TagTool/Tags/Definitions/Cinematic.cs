using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "cinematic", Tag = "cine", Size = 0xB0 , MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "cinematic", Tag = "cine", Size = 0xB4, MinVersion = CacheVersion.Halo3ODST)]
    public class Cinematic : TagStructure
	{
        public uint Unknown1;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown2;

        public List<SceneIndex> SceneIndices;
        public CachedTagInstance ImportScenario;
        public int Unknown3;
        public StringId ScenarioName; 
        public short Unknown4;
        public short Unknown5;
        public int Unknown6;
        public int Unknown7;
        public int Unknown8;
        public int Unknown9;
        public int Unknown10;
        public int Unknown11;
        public int Unknown12;
        public int Unknown13;
        public int Unknown14;
        public int Unknown15;
        public int Unknown16;

        public CachedTagInstance Unknown17;

        // Scripts are in ASCIIZ format, they will probably need conversion to work in HO

        public byte[] ImportScript1;

        public List<TagReferenceBlock> CinematicScenes;

        public byte[] ImportScript2;
        public byte[] ImportScript3;

        [TagStructure(Size = 0x4)]
		public /*was_struct*/ class SceneIndex : TagStructure
		{
            public uint Value;
        }
    }
}