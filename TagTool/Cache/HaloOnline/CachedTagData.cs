using System.Collections.Generic;
using TagTool.Cache.Gen3;
using TagTool.Tags;

namespace TagTool.Cache.HaloOnline
{
    /// <summary>
    /// Contains tag data and a description of it.
    /// </summary>
    public class CachedTagData
    {
        /// <summary>
        /// The tag data's group.
        /// </summary>
        public TagGroupGen3 Group { get; set; } = new TagGroupGen3();

        /// <summary>
        /// The offset of the main structure in the tag data.
        /// </summary>
        public uint MainStructOffset { get; set; }

        /// <summary>
        /// Gets the indices of tags that the tag data depends on.
        /// </summary>
        public HashSet<int> Dependencies { get; } = new HashSet<int>();

        /// <summary>
        /// Gets a list of fixups for pointers in the tag data.
        /// </summary>
        public List<PointerFixup> PointerFixups { get; } = new List<PointerFixup>();

        /// <summary>
        /// Gets a list of offsets to each resource pointer in the tag data.
        /// </summary>
        public List<uint> ResourcePointerOffsets { get; } = new List<uint>();

        /// <summary>
        /// Gets a list of offsets to each tag reference in the tag data.
        /// </summary>
        public List<uint> TagReferenceOffsets { get; } = new List<uint>();

        /// <summary>
        /// The serialized tag data.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Contains information about a pointer in tag data that needs to be adjusted.
        /// </summary>
        public class PointerFixup
        {
            /// <summary>
            /// The offset (from the start of the tag's data) of the pointer.
            /// </summary>
            public uint WriteOffset { get; set; }

            /// <summary>
            /// The offset (from the start of the tag's data) that the pointer should point to.
            /// </summary>
            public uint TargetOffset { get; set; }
        }
    }
}
