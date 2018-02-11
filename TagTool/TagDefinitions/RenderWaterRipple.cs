using TagTool.Cache;
using TagTool.Serialization;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "render_water_ripple", Tag = "rwrd", Size = 0x4C, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "render_water_ripple", Tag = "rwrd", Size = 0x50, MinVersion = CacheVersion.Halo3ODST)]
    public class RenderWaterRipple
    {
        public int UnknownFlags;
        public float Unknown1;
        public float Unknown2;
        public float Unknown3;
        public float Unknown4;
        public float Unknown5;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown6;

        public float Unknown7;
        public float Unknown8;
        public float Unknown9;

        public short UnknownFlags2;
        public short Unknown10;

        public float Unknown11;
        public float Unknown12;
        public float Unknown13;
        public float Unknown14;

        public short Unknown15;
        public short Unknown16;

        public float Unknown17;
        public float Unknown18;
        public float Unknown19;

        public short Unknown20;
        public short Unknown21;
    }
}