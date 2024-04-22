using Assimp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Havok
{
    public static class CollisionMoppGenerator
    {
        public static TagHkpMoppCode Generate(CollisionGeometry bsp)
        {
            var code = GenerateMoppCode(new IntermediateFormat(bsp));
            if (code == null)
                return null;

            return BuildTagHkpMoppCode(code);
        }

        private static TagHkpMoppCode BuildTagHkpMoppCode(byte[] code)
        {
            var deserializer = new TagDeserializer(CacheVersion.HaloOnlineED, CachePlatform.Original);
            var reader = new EndianReader(new MemoryStream(code));
            var context = new DataSerializationContext(reader);
            var header = deserializer.Deserialize<HkpMoppCode>(context);
            var data = reader.ReadBytes((int)header.ArrayBase.Size);

            return new TagHkpMoppCode()
            {
                ReferencedObject = new HkpReferencedObject { ReferenceCount = 128 },
                Info = header.Info,
                ArrayBase = new HkArrayBase { Size = (uint)data.Length, CapacityAndFlags = (uint)(data.Length | 0x80000000) },
                Data = new TagBlock<byte>(CacheAddressType.Data, data.ToList())
            };
        }

        private static byte[] GenerateMoppCode(IntermediateFormat input)
        {
            var tempDir = Directory.CreateDirectory(Path.Combine(Program.TagToolDirectory, "Temp"));
            var id = Guid.NewGuid().ToString().Replace("_", "").Substring(8);
            var inputFile = new FileInfo(Path.Combine(tempDir.FullName, $"mopp_{id}.input"));
            var outputFile = new FileInfo(Path.Combine(tempDir.FullName, $"mopp_{id}.output"));
            try
            {
                using (var tmp = inputFile.CreateText())
                    input.Write(tmp);

                int exitCode = HavokTool.ExecuteCommand("generate-collision-mopp", $"\"{inputFile.FullName}\" \"{outputFile.FullName}\"");
                if (exitCode != 0)
                    return null;

                return File.ReadAllBytes(outputFile.FullName);

            }
            finally
            {
                if (inputFile.Exists) inputFile.Delete();
                if (outputFile.Exists) outputFile.Delete();
            }
        }

        class IntermediateFormat
        {
            public List<Vector3D> Vertices;
            public List<Surfae> Surfaces;

            public class Surfae
            {
                public IList<int> Indices;
            }

            public IntermediateFormat(CollisionGeometry bsp)
            {
                Vertices = new List<Vector3D>();
                Surfaces = new List<Surfae>();

                foreach (Vertex vertex in bsp.Vertices)
                    Vertices.Add(new Vector3D(vertex.Point.X, vertex.Point.Y, vertex.Point.Z));

                for (int surfaceIndex = 0; surfaceIndex < bsp.Surfaces.Count; surfaceIndex++)
                {
                    var surface = bsp.Surfaces[surfaceIndex];
                    if (surface.Flags.HasFlag(SurfaceFlags.Invalid))
                        continue;

                    var indices = new List<int>();
                    int edgeIndex = surface.FirstEdge;
                    do
                    {
                        Edge edge = bsp.Edges[edgeIndex];
                        if (edge.RightSurface == surfaceIndex)
                        {
                            edgeIndex = edge.ReverseEdge;
                            indices.Add(edge.EndVertex);
                        }
                        else
                        {
                            edgeIndex = edge.ForwardEdge;
                            indices.Add(edge.StartVertex);
                        }

                    }
                    while (edgeIndex != surface.FirstEdge);

                    Surfaces.Add(new Surfae() { Indices = indices });
                }
            }

            public void Write(TextWriter writer)
            {
                writer.WriteLine(";### vertices ###");
                writer.WriteLine(Vertices.Count);
                foreach (var vertex in Vertices)
                    writer.WriteLine($"{vertex.X:0.0000000000}\t{vertex.Y:0.0000000000}\t{vertex.Z:0.0000000000}");

                writer.WriteLine();

                writer.WriteLine(";### surfaces ###");
                writer.WriteLine(Surfaces.Count);
                for (int i = 0; i < Surfaces.Count; i++)
                {
                    var surface = Surfaces[i];
                    writer.WriteLine(surface.Indices.Count);
                    for (int j = 0; j < surface.Indices.Count; j++)
                    {
                        writer.Write($"{surface.Indices[j]}");
                        if (j < surface.Indices.Count - 1)
                            writer.Write("\t");
                    }

                    writer.WriteLine();
                }
            }
        }
    }
}
