using TagTool.Cache;
using TagTool.Tags;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Geometry
{
    /// <summary>
    /// Defines a vertex buffer in model data.
    /// </summary>
    [TagStructure(Size = 0x1C, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x1C, MinVersion = CacheVersion.HaloReach)]
    [TagStructure(Size = 0x20, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
    public class VertexBufferDefinition : TagStructure
    {
        /// <summary>
        /// The number of vertices in the buffer.
        /// </summary>
        public int Count;

        /// <summary>
        /// The format of each vertex.
        /// </summary>
        public VertexBufferFormat Format;

        /// <summary>
        /// The size of each vertex in bytes.
        /// </summary>
        /// <remarks>
        /// This multiplied by <see cref="Count"/> should equal the total buffer size.
        /// </remarks>
        public short VertexSize;

        /// <summary>
        /// The reference to the the data for the vertex buffer.
        /// </summary>
        [TagField(Align = 0x4)]
        public TagData Data;

        [TagField(Flags = Padding, Length = 4, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Unused;
    }
}
