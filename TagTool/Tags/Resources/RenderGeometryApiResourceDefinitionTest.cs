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
    public class RenderGeometryApiResourceDefinitionTest : TagStructure
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