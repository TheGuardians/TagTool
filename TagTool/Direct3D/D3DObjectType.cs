using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Direct3D
{
    /// <summary>
    /// D3D object types.
    /// </summary>
    public enum D3DObjectType : int
    {
        VertexBuffer,      // s_tag_d3d_vertex_buffer
        IndexBuffer,       // s_tag_d3d_index_buffer
        Texture,           // s_tag_d3d_texture
        InterleavedTexture // s_tag_d3d_texture_interleaved
    }
}
