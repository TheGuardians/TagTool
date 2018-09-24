using TagTool.Cache;
using TagTool.Geometry;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Resources
{
    /// <summary>
    /// Resource definition data for renderable geometry.
    /// </summary>
    [TagStructure(Name = "render_geometry_api_resource_definition", Size = 0x30)]
    public class RenderGeometryApiResourceDefinition : TagStructure
	{
        [TagField(Padding = true, Length = 12)]
        public byte[] Unused1;

        [TagField(Padding = true, Length = 12)]
        public byte[] Unused2;

        /// <summary>
        /// The vertex buffer definitions for the model data.
        /// </summary>
        public List<TagStructureReference<VertexBufferDefinition>> VertexBuffers;

        /// <summary>
        /// The index buffer definitions for the model data.
        /// </summary>
        public List<TagStructureReference<IndexBufferDefinition>> IndexBuffers;
    }

    /// <summary>
    /// Defines a vertex buffer in model data.
    /// </summary>
    [TagStructure(Size = 0x1C, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x20, MinVersion = CacheVersion.HaloOnline106708)]
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
        public TagData Data;

        [TagField(Padding = true, Length = 4, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;
    }

    /// <summary>
    /// Defines an index buffer in model data.
    /// </summary>
    [TagStructure(Size = 0x18, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x20, MinVersion = CacheVersion.HaloOnline106708)]
    public class IndexBufferDefinition : TagStructure
	{
        /// <summary>
        /// The primitive type to use for the index buffer.
        /// </summary>
        public IndexBufferFormat Format;

        /// <summary>
        /// The reference to the data for the index buffer.
        /// </summary>
        public TagData Data;

        [TagField(Padding = true, Length = 8, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;
    }
}