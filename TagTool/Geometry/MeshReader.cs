using TagTool.Cache;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagTool.Geometry
{
    /// <summary>
    /// Provides functions for reading mesh data.
    /// </summary>
    public class MeshReader
    {
        private const int StreamCount = 5;
        private const int IndexBufferCount = 2;

        private readonly CacheVersion _version;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeshReader"/> class.
        /// </summary>
        /// <param name="version">The engine version to target.</param>
        /// <param name="mesh">The mesh.</param>
        /// <param name="definition">The mesh's definition data.</param>
        public MeshReader(CacheVersion version, Mesh mesh, RenderGeometryApiResourceDefinition definition)
        {
            _version = version;
            Mesh = mesh;
            Definition = definition;
            VertexStreams = new VertexBufferDefinition[StreamCount];
            IndexBuffers = new IndexBufferDefinition[IndexBufferCount];
            BindVertexStreams();
            BindIndexBuffers();
        }

        /// <summary>
        /// Gets the mesh.
        /// </summary>
        public Mesh Mesh { get; private set; }

        /// <summary>
        /// Gets the mesh's definition data.
        /// </summary>
        public RenderGeometryApiResourceDefinition Definition { get; private set; }

        /// <summary>
        /// Gets the vertex streams for the mesh. Note that elements can be <c>null</c>.
        /// </summary>
        public VertexBufferDefinition[] VertexStreams { get; private set; }

        /// <summary>
        /// Gets the index streams for the mesh. Note that elements can be <c>null</c>.
        /// </summary>
        public IndexBufferDefinition[] IndexBuffers { get; private set; }

        /// <summary>
        /// Opens a vertex stream on one of the mesh's vertex buffers.
        /// </summary>
        /// <param name="definition">The vertex buffer definition.</param>
        /// <param name="baseStream">The stream open on the mesh's resource data to use as a base stream.</param>
        /// <returns>The vertex stream if successful, or <c>null</c> otherwise.</returns>
        public IVertexStream OpenVertexStream(VertexBufferDefinition definition, Stream baseStream)
        {
            if (definition.Data.Address.Type != CacheAddressType.Data)
                return null; // Don't bother supporting non-resource addresses
            baseStream.Position = definition.Data.Address.Offset;
            return VertexStreamFactory.Create(_version, baseStream);
        }

        /// <summary>
        /// Opens a vertex stream on one of the mesh's vertex streams.
        /// </summary>
        /// <param name="streamIndex">Index of the vertex stream to open.</param>
        /// <param name="baseStream">The stream open on the mesh's resource data to use as a base stream.</param>
        /// <returns>The vertex stream if successful, or <c>null</c> otherwise.</returns>
        public IVertexStream OpenVertexStream(int streamIndex, Stream baseStream)
        {
            if (streamIndex < 0 || streamIndex >= VertexStreams.Length)
                return null;
            var definition = VertexStreams[streamIndex];
            if (definition == null)
                return null;
            return OpenVertexStream(definition, baseStream);
        }

        /// <summary>
        /// Opens an index buffer stream on one of the mesh's index buffers.
        /// </summary>
        /// <param name="definition">The index buffer definition.</param>
        /// <param name="baseStream">The stream open on the mesh's resource data to use as a base stream.</param>
        /// <returns>The index buffer stream if successful, or <c>null</c> otherwise.</returns>
        public IndexBufferStream OpenIndexBufferStream(IndexBufferDefinition definition, Stream baseStream)
        {
            if (definition.Data.Address.Type != CacheAddressType.Data)
                return null; // Don't bother supporting non-resource addresses
            baseStream.Position = definition.Data.Address.Offset;
            return new IndexBufferStream(baseStream);
        }

        /// <summary>
        /// Opens an index buffer stream on one of the mesh's index buffers.
        /// </summary>
        /// <param name="bufferIndex">Index of the index buffer to open.</param>
        /// <param name="baseStream">The stream open on the mesh's resource data to use as a base stream.</param>
        /// <returns>The index buffer stream if successful, or <c>null</c> otherwise.</returns>
        public IndexBufferStream OpenIndexBufferStream(int bufferIndex, Stream baseStream)
        {
            if (bufferIndex < 0 || bufferIndex >= IndexBuffers.Length)
                return null;
            var definition = IndexBuffers[bufferIndex];
            if (definition == null)
                return null;
            return OpenIndexBufferStream(definition, baseStream);
        }

        /// <summary>
        /// Binds each vertex buffer in the mesh to a stream.
        /// </summary>
        private void BindVertexStreams()
        {
            // The game actually loads buffers from specific indices, but it's
            // dependent upon what type of mesh is being loaded. Instead, we
            // can just scan the entire array and bind everything as an
            // approximation.
            foreach (var bufferIndex in Mesh.VertexBufferIndices.Where(i => i < Definition.VertexBuffers.Count))
            {
                // Use the buffer format to determine the stream index
                var buffer = Definition.VertexBuffers[bufferIndex].Definition;
                if (!VertexBufferStreams.TryGetValue(buffer.Format, out int streamIndex))
                    continue;
                VertexStreams[streamIndex] = buffer;
            }
        }

        /// <summary>
        /// Binds each index buffer in the mesh.
        /// </summary>
        private void BindIndexBuffers()
        {
            if (Mesh.IndexBufferIndices.Length < IndexBuffers.Length)
                throw new InvalidOperationException("Mesh has too few index buffers");
            for (var i = 0; i < IndexBuffers.Length; i++)
            {
                var bufferIndex = Mesh.IndexBufferIndices[i];
                if (bufferIndex < Definition.IndexBuffers.Count)
                    IndexBuffers[i] = Definition.IndexBuffers[bufferIndex].Definition;
            }
        }

        // Maps vertex buffer formats to their corresponding streams.
        private static readonly Dictionary<VertexBufferFormat, int> VertexBufferStreams = new Dictionary<VertexBufferFormat, int>
        {
            { VertexBufferFormat.Invalid, 0 },
            { VertexBufferFormat.World, 0 },
            { VertexBufferFormat.Rigid, 0 },
            { VertexBufferFormat.Skinned, 0 },
            { VertexBufferFormat.StaticPerPixel, 4 },
            { VertexBufferFormat.Unknown5, 4 },
            { VertexBufferFormat.StaticPerVertex, 4 },
            { VertexBufferFormat.Unknown7, 0 },
            { VertexBufferFormat.Unused8, 2 },
            { VertexBufferFormat.AmbientPrt, 2 },
            { VertexBufferFormat.LinearPrt, 2 },
            { VertexBufferFormat.QuadraticPrt, 2 },
            { VertexBufferFormat.UnknownC, 0 },
            { VertexBufferFormat.UnknownD, 0 },
            { VertexBufferFormat.StaticPerVertexColor, 4 },
            { VertexBufferFormat.UnknownF, 0 },
            { VertexBufferFormat.Unused10, 1 },
            { VertexBufferFormat.Unused11, 2 },
            { VertexBufferFormat.Unused12, 1 },
            { VertexBufferFormat.Unused13, 1 },
            { VertexBufferFormat.TinyPosition, 1 },
            { VertexBufferFormat.Unknown15, 2 },
            { VertexBufferFormat.Unknown16, 2 },
            { VertexBufferFormat.Unknown17, 2 },
            { VertexBufferFormat.Decorator, 0 },
            { VertexBufferFormat.ParticleModel, 0 },
            { VertexBufferFormat.Unknown1A, 2 },
            { VertexBufferFormat.Unknown1B, 3 },
            { VertexBufferFormat.Unknown1C, 0 },
            { VertexBufferFormat.Unused1D, 1 },
        };
    }
}
