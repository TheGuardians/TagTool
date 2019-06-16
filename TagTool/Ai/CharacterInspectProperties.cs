using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x14)]
    public class CharacterInspectProperties : TagStructure
	{
        /// <summary>
        /// World Units; Distance from object at which to stop and turn on the inspection light.
        /// </summary>
        public float StopDistance;

        /// <summary>
        /// Seconds; The time which we should inspect each object for.
        /// </summary>
        public Bounds<float> InspectTime;

        /// <summary>
        /// World Units; Range in which we should search for objects to inspect
        /// </summary>
        public Bounds<float> SearchRange;
    }
}
