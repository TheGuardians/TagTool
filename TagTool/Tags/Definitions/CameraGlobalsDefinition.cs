using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "camera_globals_definition", Tag = "glca", Size = 0xA0, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "camera_globals_definition", Tag = "glca", Size = 0xA4, MinVersion = CacheVersion.Halo3ODST)]
    public class CameraGlobalsDefinition : TagStructure
	{
        [TagField(Flags = TagFieldFlags.Label, ValidTags = new[] { "trak" })]
        public CachedTagInstance DefaultUnitCameraTrack;

        public float Unknown1;
        public float Unknown2;
        public float Unknown3;
        public float Unknown4;
        public float Unknown5;
        public float Unknown6;
        public float Unknown7;
        public float Unknown8;
        public float Unknown9;
        public float Unknown10;
        public float Unknown11;
        public float Unknown12;
        public float Unknown13;
        public float Unknown14;
        public float Unknown15;
        public float Unknown16;
        public float Unknown17;
        public float Unknown18;
        public float Unknown19;
        public float Unknown20;
        public float Unknown21;
        public float Unknown22;
        public float Unknown23;
        public float Unknown24;

        public FunctionType BoostFunction;
        public FunctionType HoistFunction;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float Unknown27;

        public float Unknown28;
        public float Unknown29;
        public float Unknown30;
        public float Unknown31;
        public float Unknown32;
        public float Unknown33;
        public float Unknown34;
        public float Unknown35;
        public float Unknown36;
        public float Unknown37;
        public float Unknown38;
    }
}