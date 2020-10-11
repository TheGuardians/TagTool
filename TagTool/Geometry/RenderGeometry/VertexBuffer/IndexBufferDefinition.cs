using TagTool.Cache;
using TagTool.Tags;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Geometry
{
    /// <summary>
    /// Defines an index buffer in model data.
    /// </summary>
    [TagStructure(Size = 0x18, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloReach)]
    [TagStructure(Size = 0x20, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
    public class IndexBufferDefinition : TagStructure
    {
        /// <summary>
        /// The primitive type to use for the index buffer.
        /// </summary>
        public IndexBufferFormat Format;

        /// <summary>
        /// The reference to the data for the index buffer.
        /// </summary>
        [TagField(Align = 0x4)]
        public TagData Data;

        [TagField(Flags = Padding, Length = 8, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Unused;
    }
}
