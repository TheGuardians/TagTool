using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3ODST)]
    public class CharacterInspectProperties : TagStructure
	{
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float StopDistance; // distance from object at which to stop and turn on the inspection light (wu)

        public Bounds<float> InspectTime; // time which we should inspect each object for (seconds)

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public Bounds<float> SearchRange; // range in which we should search for objects to inspect (wu)
    }
}
