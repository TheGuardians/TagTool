using TagTool.Geometry;

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
        public TagBlock<D3DStructure<BitmapTextureInteropResource.BitmapDefinition>> Textures;

        /// <summary>
        /// Unused s_tag_d3d_texture_interleaved
        /// </summary>
        public TagBlock<D3DStructure<BitmapTextureInterleavedInteropResource.BitmapInterleavedDefinition>> InterleavedTextures;

        /// <summary>
        /// The vertex buffer definitions for the model data.
        /// </summary>
        public TagBlock<D3DStructure<VertexBufferDefinition>> VertexBuffers;

        /// <summary>
        /// The index buffer definitions for the model data.
        /// </summary>
        public TagBlock<D3DStructure<IndexBufferDefinition>> IndexBuffers;
    }
}