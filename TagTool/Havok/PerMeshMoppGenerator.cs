using Assimp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Serialization;
using Mesh = TagTool.Geometry.Mesh;

namespace TagTool.Havok
{
    public static class PerMeshMoppGenerator
    {
        public static RenderGeometry.PerMeshMoppBlock Generate(GameCache cache, Mesh mesh)
        {
            var input = ExportMesh(cache, mesh);
            var moppCode = GenerateMoppCode(input);
            var reordeTable = BuildReorderTable(cache, moppCode);
            return new RenderGeometry.PerMeshMoppBlock()
            {
                MoppCode = moppCode,
                MoppReorderTable = reordeTable.Select(x => (short)x).ToList()
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

                int exitCode = HavokTool.ExecuteCommand("generate-mesh-mopp", $"\"{inputFile.FullName}\" \"{outputFile.FullName}\"");
                if (exitCode != 0)
                    return null;

                return File.ReadAllBytes(outputFile.FullName);

            }
            finally
            {
                if(inputFile.Exists) inputFile.Delete();
                if (outputFile.Exists) outputFile.Delete();
            }
        }

        public static int[] BuildReorderTable(GameCache cache, byte[] moppData)
        {
            var reader = new EndianReader(new MemoryStream(moppData));
            var dataContext = new DataSerializationContext(reader);
            var deserializer = new TagDeserializer(cache.Version, cache.Platform);

            var subpartMoppCode = deserializer.Deserialize<HkpMoppCode>(dataContext);
            var subpartKeys = HavokMoppUtil.GetAllKeys(reader.ReadBytes((int)subpartMoppCode.ArrayBase.Size));

            var offset = (subpartMoppCode.ArrayBase.Size + 0x3F) & ~0xF;
            reader.SeekTo(offset);

            var partMoppCode = deserializer.Deserialize<HkpMoppCode>(dataContext);
            var partKeys = HavokMoppUtil.GetAllKeys(reader.ReadBytes((int)partMoppCode.ArrayBase.Size));

            return BuildReorderTable(subpartKeys, partKeys);
        }

        private static int[] BuildReorderTable(IList<int> subpartKeys, IList<int> partKeys)
        {
            var reorderTable = new int[subpartKeys.Count * 2 + 2 * partKeys.Count];
            for (int i = 0; i < subpartKeys.Count; i++)
            {
                reorderTable[i] = subpartKeys[i];
                reorderTable[subpartKeys[i] + subpartKeys.Count] = i;
            }

            for (int i = 0; i < partKeys.Count; i++)
            {
                reorderTable[2 * subpartKeys.Count + i] = partKeys[i];
                reorderTable[2 * subpartKeys.Count + partKeys.Count + partKeys[i]] = i;
            }
            return reorderTable;
        }

        private static IntermediateFormat ExportMesh(GameCache cache, Mesh mesh)
        {
            var format = new IntermediateFormat();
            format.Vertices = ReadWorldVertices(cache, mesh).Select(x => new Vector3D(x.Position.I, x.Position.J, x.Position.K)).ToList();
            format.Indcies = new List<int>();
            format.Parts = new List<IntermediateFormat.Part>();
            format.Subparts = new List<IntermediateFormat.Subpart>();

            foreach (var part in mesh.Parts)
            {
                var intermediatePart = new IntermediateFormat.Part();
                intermediatePart.IndexStart = format.Indcies.Count;
                intermediatePart.SubpartStart = format.Subparts.Count;
                if (part.SubPartCount > 0)
                {
                    for (int i = part.FirstSubPartIndex; i < part.FirstSubPartIndex + part.SubPartCount; i++)
                    {
                        var intermediateSubpart = new IntermediateFormat.Subpart();
                        intermediateSubpart.IndexStart = format.Indcies.Count;

                        var subpart = mesh.SubParts[i];
                        var indciesSubpart = ReadIndices(cache, mesh, subpart.FirstIndex, subpart.IndexCount);
                        for (int j = 0; j < subpart.IndexCount; j += 3)
                        {
                            format.Indcies.Add(indciesSubpart[j + 0]);
                            format.Indcies.Add(indciesSubpart[j + 1]);
                            format.Indcies.Add(indciesSubpart[j + 2]);
                        }

                        intermediateSubpart.IndexCount = format.Indcies.Count - intermediateSubpart.IndexStart;
                        format.Subparts.Add(intermediateSubpart);
                    }
                    intermediatePart.SubpartCount = format.Subparts.Count - intermediatePart.SubpartStart;
                }
                else
                {
                    var indciesPart = ReadIndices(cache, mesh, part.FirstIndex, part.IndexCount);
                    for (int j = 0; j < part.IndexCount; j += 3)
                    {
                        format.Indcies.Add(indciesPart[j + 0]);
                        format.Indcies.Add(indciesPart[j + 1]);
                        format.Indcies.Add(indciesPart[j + 2]);
                    }
                }

                intermediatePart.IndexCount = format.Indcies.Count - intermediatePart.IndexStart;
                format.Parts.Add(intermediatePart);
            }

            return format;
        }

        private static ushort[] ReadIndices(GameCache cache, Mesh mesh, int firstIndex, int count)
        {
            var indexBuffer = mesh.ResourceIndexBuffers[0];
            if (indexBuffer == null)
                throw new InvalidOperationException("Index buffer 0 is null");

            var indexStream = new IndexBufferStream(new MemoryStream(indexBuffer.Data.Data), cache.Endianness);
            indexStream.Position = (uint)firstIndex;
            switch (indexBuffer.Format)
            {
                case IndexBufferFormat.TriangleList:
                    return indexStream.ReadIndices((uint)count);
                case IndexBufferFormat.TriangleStrip:
                    return indexStream.ReadTriangleStrip((uint)count);
                default:
                    throw new InvalidOperationException("Unsupported index buffer type: " + indexBuffer.Format);
            }
        }

        public static List<WorldVertex> ReadWorldVertices(GameCache cache, Mesh mesh)
        {
            var mainBuffer = mesh.ResourceVertexBuffers[0];
            if (mainBuffer == null)
                return new List<WorldVertex>();

            var vertexReader = VertexStreamFactory.Create(cache.Version, cache.Platform, new MemoryStream(mainBuffer.Data.Data));
            var result = new List<WorldVertex>();
            for (int i = 0; i < mainBuffer.Count; i++)
                result.Add(vertexReader.ReadWorldVertex());

            return result;
        }

        class IntermediateFormat
        {
            public List<Vector3D> Vertices;
            public List<int> Indcies;
            public List<Part> Parts;
            public List<Subpart> Subparts;

            public class Part
            {
                public int IndexStart;
                public int IndexCount;
                public int SubpartStart;
                public int SubpartCount;
            }

            public class Subpart
            {
                public int IndexStart;
                public int IndexCount;
            }

            public void Write(TextWriter writer)
            {
                writer.WriteLine(";### vertices ###");
                writer.WriteLine(Vertices.Count);
                foreach (var vertex in Vertices)
                    writer.WriteLine($"{vertex.X:0.0000000000}\t{vertex.Y:0.0000000000}\t{vertex.Z:0.0000000000}");

                writer.WriteLine();

                writer.WriteLine(";### indices ###");
                writer.WriteLine(Indcies.Count);
                for (int i = 0; i < Indcies.Count; i += 3)
                    writer.WriteLine($"{Indcies[i + 0]}\t{Indcies[i + 1]}\t{Indcies[i + 2]}");

                writer.WriteLine();
                writer.WriteLine(";### parts ###");
                writer.WriteLine($"{Parts.Count}");
                foreach (var part in Parts)
                {
                    writer.WriteLine($"{part.IndexStart}\t{part.IndexCount}");
                    writer.WriteLine($"{part.SubpartStart}\t{part.SubpartCount}");
                    writer.WriteLine();
                }

                writer.WriteLine();
                writer.WriteLine(";### subparts ###");
                writer.WriteLine($"{Subparts.Count}");
                foreach (var part in Subparts)
                    writer.WriteLine($"{part.IndexStart}\t{part.IndexCount}");
            }
        }
    }
}
