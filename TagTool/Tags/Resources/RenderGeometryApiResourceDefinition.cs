using TagTool.Cache;
using TagTool.Geometry;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Resources
{
    /// <summary>
    /// Resource definition data for renderable geometry.
    /// </summary>
    [TagStructure(Name = "render_geometry_api_resource_definition", Size = 0x30)]
    public class RenderGeometryApiResourceDefinition : TagStructure
	{
        /// <summary>
        /// Unused s_tag_d3d_texture
        /// </summary>
        public List<TagStructureReference<BitmapTextureInteropResource.BitmapDefinition>> Textures;

        /// <summary>
        /// Unused s_tag_d3d_texture_interleaved
        /// </summary>
        public List<TagStructureReference<BitmapTextureInterleavedInteropResource.BitmapInterleavedDefinition>> InterleavedTextures;

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
        [TagField(Align = 0x4)]
        public TagData Data;

        [TagField(Flags = Padding, Length = 4, MinVersion = CacheVersion.HaloOnline106708)]
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
        [TagField(Align = 0x4)]
        public TagData Data;

        [TagField(Flags = Padding, Length = 8, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;
    }

    [TagStructure(Size = 0x0)]
    public class TagD3DTexture { }

    [TagStructure(Size = 0x0)]
    public class TagD3DTextureInterleaved { }

}