using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands;
using TagTool.Common;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Havok
{
    public static class HavokMoppGenerator
    {
        /// <summary>
        /// Generates mopp code for the given collision geometry
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        public static TagHkpMoppCode GenerateMoppCode(CollisionGeometry geometry)
        {
            return GenerateMoppCode(GenerateInput(geometry));
        }

        #region Internal

        private static MoppGeneratorInput GenerateInput(CollisionGeometry bsp)
        {
            var input = new MoppGeneratorInput();
            input.IndexMask = uint.MaxValue;

            foreach (Vertex vertex in bsp.Vertices)
            {
                float sink = vertex.Sink / short.MaxValue * 2;
                input.Vertices.Add(new RealPoint3d(vertex.Point.X, vertex.Point.Y, vertex.Point.Z - sink));
            }

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

                } while (edgeIndex != surface.FirstEdge);

                input.Faces.Add(new MoppGeneratorInput.Face(indices.ToArray(), (uint)surfaceIndex));
            }

            return input;
        }

        private static TagHkpMoppCode GenerateMoppCode(MoppGeneratorInput input)
        {
            var tempDir = Directory.CreateDirectory(Path.Combine(Program.TagToolDirectory, "Temp"));
            var id = Guid.NewGuid().ToString().Replace("_", "").Substring(8);

            var inputFile = new FileInfo(Path.Combine(tempDir.FullName, $"mopp_{id}.input"));
            var outputFile = new FileInfo(Path.Combine(tempDir.FullName, $"mopp_{id}.output"));

            try
            {
                using (var fs = inputFile.Create())
                    WriteInput(fs, input);

                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = Path.GetFullPath(Path.Combine(Program.TagToolDirectory, "Tools", "mopp.exe")),
                        Arguments = $"\"{inputFile.FullName}\" \"{outputFile.FullName}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                process.WaitForExit();

                int result = process.ExitCode;
                if (result != 0)
                    return null;

                using (var fs = outputFile.OpenRead())
                    return ReadMoppCode(fs);
            }
            finally
            {
                if (inputFile.Exists) inputFile.Delete();
                if (outputFile.Exists) outputFile.Delete();
            }
        }

        private static TagHkpMoppCode ReadMoppCode(Stream stream)
        {
            var deserializer = new TagDeserializer(CacheVersion.HaloOnlineED, CachePlatform.Original);
            var reader = new EndianReader(stream, true);
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

        private static void WriteInput(Stream stream, MoppGeneratorInput input)
        {
            var baseOffset = stream.Position;

            var writer = new EndianWriter(stream, true);
            var context = new DataSerializationContext(writer);
            var serializer = new TagSerializer(CacheVersion.HaloOnlineED, CachePlatform.Original);

            uint headersize = TagStructure.GetStructureSize(typeof(InputFileHeader), serializer.Version, serializer.CachePlatform);

            // make room for the header
            StreamUtil.Fill(stream, 0, (int)(headersize + 0xF & ~0xF));

            // write the data
            uint dataOffset = (uint)stream.Position;
            serializer.Serialize(context, input);
            uint dataSize = (uint)(stream.Position - dataOffset);

            // write the fixups
            StreamUtil.Align(stream, 4);
            uint fixupsOffset = (uint)stream.Position;
            foreach (var offset in context.PointerOffsets)
                writer.Write(offset);

            var header = new InputFileHeader();
            header.Signature = "mopp";
            header.DataSize = dataSize;
            header.MainStructOffset = context.MainStructOffset;
            header.FixupCount = context.PointerOffsets.Count;
            header.FixupsOffset = fixupsOffset;

            // write the header
            stream.Position = baseOffset;
            serializer.Serialize(context, header);
        }

        [TagStructure(Size = 0x1C)]
        class MoppGeneratorInput : TagStructure
        {
            public uint IndexMask = uint.MaxValue;
            public List<RealPoint3d> Vertices = new List<RealPoint3d>();
            public List<Face> Faces = new List<Face>();

            [TagStructure(Size = 0x28)]
            public class Face
            {
                [TagField(Length = 8)]
                public int[] Indices = new int[8];
                public int IndexCount;
                public uint ShapeKey;

                public Face(int[] inIndices, uint shapeKey = 0)
                {
                    for (int i = 0; i < inIndices.Length; i++)
                        Indices[i] = inIndices[i];

                    IndexCount = inIndices.Length;
                    ShapeKey = shapeKey;
                }
            }
        }

        [TagStructure(Size = 0x14)]
        class InputFileHeader : TagStructure
        {
            public Tag Signature;
            public uint DataSize;
            public uint MainStructOffset;
            public int FixupCount;
            public uint FixupsOffset;
        }
        #endregion
    }
}
