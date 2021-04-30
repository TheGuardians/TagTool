using TagTool.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagTool.Geometry
{
    /// <summary>
    /// Extracts render model data to Wavefront .obj files.
    /// </summary>
    public class ObjExtractor
    {
        private readonly TextWriter _writer;
        private readonly StringWriter _faceWriter = new StringWriter();
        private uint _baseIndex = 1;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjExtractor"/> class.
        /// </summary>
        /// <param name="writer">The stream to write the output file to.</param>
        public ObjExtractor(TextWriter writer)
        {
            _writer = writer;
            WriteHeader();
        }

        /// <summary>
        /// Writes mesh data to the .obj.
        /// </summary>
        /// <param name="reader">The mesh reader to use.</param>
        /// <param name="compressor">The vertex compressor to use.</param>
        /// <param name="name">The name of the mesh.</param>
        public void ExtractMesh(MeshReader reader, VertexCompressor compressor, string name = null)
        {
            var vertices = ReadVertices(reader);
            DecompressVertices(vertices, compressor);

            var indicesList = new List<ushort[]>();
            var indexCount = 0;

            for (var i = 0; i < (reader?.Mesh?.Parts?.Count ?? -1); i++)
            {
                var part = reader.Mesh.Parts[i];
                var indices = ReadIndices(reader, part);

                indicesList.Add(indices);

                if (part.IndexCountOld > 0)
                    indexCount += indices.Length;
            }

            if (indexCount == 0)
                return;

            foreach (var vertex in vertices)
                _writer.WriteLine("v {0} {1} {2}", vertex.Position.I, vertex.Position.J, vertex.Position.K);

            foreach (var vertex in vertices)
                _writer.WriteLine("vn {0} {1} {2}", vertex.Normal.I, vertex.Normal.J, vertex.Normal.K);

            foreach (var vertex in vertices)
                _writer.WriteLine("vt {0} {1}", vertex.TexCoords.I, 1 - vertex.TexCoords.J);

            var partIndex = 0;
            
            if (indicesList.Count != 0)
                _writer.WriteLine($"o {name}");

            foreach (var indices in indicesList)
            {
                var triangles = GenerateTriangles(indices);

                if (triangles.Count() != 0)
                    _writer.WriteLine($"g {name}_part_{partIndex++}");

                foreach (var triangle in triangles)
                    _writer.WriteLine(triangle);
            }

            _baseIndex += (uint)vertices.Count;
        }

        /// <summary>
        /// Finishes writing meshes out to the file.
        /// </summary>
        public void Finish()
        {
            _writer.Write(_faceWriter.ToString());
            _faceWriter.Close();
        }

        /// <summary>
        /// Reads the vertex data for a mesh into a format-independent list.
        /// </summary>
        /// <param name="reader">The mesh reader to use.</param>
        /// <returns>The list of vertices that were read.</returns>
        private static List<ObjVertex> ReadVertices(MeshReader reader)
        {
            // Open a vertex reader on stream 0 (main vertex data)
            var mainBuffer = reader.VertexStreams[0];
            if (mainBuffer == null)
                return new List<ObjVertex>();

            var vertexReader = reader.OpenVertexStream(mainBuffer);

            switch (reader.Mesh.Type)
            {
                case VertexType.Rigid:
                    return ReadRigidVertices(vertexReader, mainBuffer.Count);
                case VertexType.Skinned:
                    return ReadSkinnedVertices(vertexReader, mainBuffer.Count);
                case VertexType.DualQuat:
                    return ReadDualQuatVertices(vertexReader, mainBuffer.Count);
                case VertexType.World:
                    return ReadWorldVertices(vertexReader, mainBuffer.Count);
                default:
                    throw new InvalidOperationException("Only Rigid, Skinned, DualQuat and World meshes are supported");
            }
        }

        /// <summary>
        /// Reads rigid vertices into a format-independent list.
        /// </summary>
        /// <param name="reader">The vertex reader to read from.</param>
        /// <param name="count">The number of vertices to read.</param>
        /// <returns>The vertices that were read.</returns>
        private static List<ObjVertex> ReadRigidVertices(IVertexStream reader, int count)
        {
            var result = new List<ObjVertex>();
            for (var i = 0; i < count; i++)
            {
                var rigid = reader.ReadRigidVertex();
                result.Add(new ObjVertex
                {
                    Position = rigid.Position,
                    Normal = rigid.Normal,
                    TexCoords = rigid.Texcoord,
                });
            }
            return result;
        }

        /// <summary>
        /// Reads skinned vertices into a format-independent list.
        /// </summary>
        /// <param name="reader">The vertex reader to read from.</param>
        /// <param name="count">The number of vertices to read.</param>
        /// <returns>The vertices that were read.</returns>
        private static List<ObjVertex> ReadSkinnedVertices(IVertexStream reader, int count)
        {
            var result = new List<ObjVertex>();
            for (var i = 0; i < count; i++)
            {
                var skinned = reader.ReadSkinnedVertex();
                result.Add(new ObjVertex
                {
                    Position = skinned.Position,
                    Normal = skinned.Normal,
                    TexCoords = skinned.Texcoord,
                });
            }
            return result;
        }

        /// <summary>
        /// Reads dualquat vertices into a format-independent list.
        /// </summary>
        /// <param name="reader">The vertex reader to read from.</param>
        /// <param name="count">The number of vertices to read.</param>
        /// <returns>The vertices that were read.</returns>
        private static List<ObjVertex> ReadDualQuatVertices(IVertexStream reader, int count)
        {
            var result = new List<ObjVertex>();
            for (var i = 0; i < count; i++)
            {
                var dualQuat = reader.ReadDualQuatVertex();
                result.Add(new ObjVertex
                {
                    Position = dualQuat.Position,
                    Normal = dualQuat.Normal,
                    TexCoords = dualQuat.Texcoord,
                });
            }
            return result;
        }

        /// <summary>
        /// Reads world vertices into a format-independent list.
        /// </summary>
        /// <param name="reader">The vertex reader to read from.</param>
        /// <param name="count">The number of vertices to read.</param>
        /// <returns>The vertices that were read.</returns>
        private static List<ObjVertex> ReadWorldVertices(IVertexStream reader, int count)
        {
            var result = new List<ObjVertex>();
            for (var i = 0; i < count; i++)
            {
                var world = reader.ReadWorldVertex();
                result.Add(new ObjVertex
                {
                    Position = world.Position,
                    Normal = world.Normal,
                    TexCoords = world.Texcoord,
                });
            }
            return result;
        }

        /// <summary>
        /// Decompresses vertex data in-place.
        /// </summary>
        /// <param name="vertices">The vertices to decompress in-place.</param>
        /// <param name="compressor">The compressor to use.</param>
        private static void DecompressVertices(IEnumerable<ObjVertex> vertices, VertexCompressor compressor)
        {
            if (compressor == null)
                return;

            foreach (var vertex in vertices)
            {
                vertex.Position = compressor.DecompressPosition(vertex.Position);
                vertex.TexCoords = compressor.DecompressUv(vertex.TexCoords);
            }
        }

        /// <summary>
        /// Reads the index buffer data and converts it into a triangle list if necessary.
        /// </summary>
        /// <param name="reader">The mesh reader to use.</param>
        /// <param name="part">The mesh part to read.</param>
        /// <returns>The index buffer converted into a triangle list.</returns>
        private static ushort[] ReadIndices(MeshReader reader, Part part)
        {
            // Use index buffer 0
            var indexBuffer = reader.IndexBuffers[0];
            if (indexBuffer == null)
                throw new InvalidOperationException("Index buffer 0 is null");

            // Read the indices
            var indexStream = reader.OpenIndexBufferStream(indexBuffer);
            indexStream.Position = part.FirstIndexOld;
            switch (indexBuffer.Format)
            {
                case IndexBufferFormat.TriangleList:
                    return indexStream.ReadIndices(part.IndexCountOld);
                case IndexBufferFormat.TriangleStrip:
                    return indexStream.ReadTriangleStrip(part.IndexCountOld);
                default:
                    throw new InvalidOperationException("Unsupported index buffer type: " + indexBuffer.Format);
            }
        }

        /// <summary>
        /// Writes a header to the file.
        /// </summary>
        private void WriteHeader()
        {
            _writer.WriteLine("# Extracted on {0}", DateTime.Now);
        }

        /// <summary>
        /// Writes vertex data out to the file.
        /// </summary>
        /// <param name="vertices">The vertices to write.</param>
        private void WriteVertices(IEnumerable<ObjVertex> vertices)
        {
            foreach (var vertex in vertices)
                WriteVertex(vertex);
        }

        /// <summary>
        /// Writes a vertex out to the file.
        /// </summary>
        /// <param name="vertex">The vertex to write.</param>
        private void WriteVertex(ObjVertex vertex)
        {
            _writer.WriteLine("v {0} {1} {2}", vertex.Position.I, vertex.Position.J, vertex.Position.K);
            _writer.WriteLine("vn {0} {1} {2}", vertex.Normal.I, vertex.Normal.J, vertex.Normal.K);
            _writer.WriteLine("vt {0} {1}", vertex.TexCoords.I, 1 - vertex.TexCoords.J);
        }

        /// <summary>
        /// Queues triangle list data to be written out to the file.
        /// </summary>
        /// <param name="indices">The indices for the triangle list. Each set of 3 indices forms one triangle.</param>
        private IEnumerable<string> GenerateTriangles(IReadOnlyList<ushort> indices)
        {
            for (var i = 0; i < indices.Count; i += 3)
            {
                var a = indices[i] + _baseIndex;
                var b = indices[i + 1] + _baseIndex;
                var c = indices[i + 2] + _baseIndex;

                // Discard degenerate triangles
                if (a == b || a == c || b == c)
                    continue;

                yield return $"f {a}/{a}/{a} {b}/{b}/{b} {c}/{c}/{c}";
            }
        }

        private class ObjVertex
        {
            public RealQuaternion Position { get; set; }
            public RealVector3d Normal { get; set; }
            public RealVector2d TexCoords { get; set; }
        }
    }
}
