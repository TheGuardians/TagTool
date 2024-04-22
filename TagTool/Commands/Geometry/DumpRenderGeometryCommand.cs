using TagTool.Cache;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Commands.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Commands.Geometry
{
    class DumpRenderGeometryCommand : Command
    {
        private GameCache Cache { get; }
        private RenderGeometry Geometry { get; }

        public DumpRenderGeometryCommand(GameCache cache, RenderGeometry geometry, string title = "") :
            base(true,

                $"Dump{title}RenderGeometry",
                $"Dumps {title.ToLower()} render geometry in ascii format.",

                $"Dump{title}RenderGeometry [raw] <Output File>",

                $"Dumps {title.ToLower()} render geometry in ascii format.")
        {
            Cache = cache;
            Geometry = geometry;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 2)
                return new TagToolError(CommandError.ArgCount);

            if (args.Count == 2 && (args[0].ToLower() != "raw"))
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[0]}\"");

            var file = "";
            if (args.Count == 1)
                file = args[0];
            else
                file = args[1];

            var definition = Cache.ResourceCache.GetRenderGeometryApiResourceDefinition(Geometry.Resource);

            if (args.Count == 2)
            {
                using (var edResourceStream = new MemoryStream())
                using (var vertexReader = new EndianReader(edResourceStream, EndianFormat.LittleEndian))
                {
                    var directory = args[1];
                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    var dataOutDir = directory;

                    for (var i = 0; i < definition.VertexBuffers.Count; i++)
                    {
                        edResourceStream.Position = 0;

                        var vertexBuffer = definition.VertexBuffers[i].Definition;

                        dataOutDir = Path.Combine(directory, $"{i}_{vertexBuffer.Format}_{vertexBuffer.Count}");

                        using (EndianWriter output = new EndianWriter(File.OpenWrite(dataOutDir), EndianFormat.LittleEndian))
                        {
                            byte[] data = null;
                            output.WriteBlock(data);
                        }
                    }
                }
            }
            else
            {
                using (var fileStream = File.Create(file))
                using (var fileWriter = new StreamWriter(fileStream))
                {
                    for (var i = 0; i < definition.VertexBuffers.Count; i++)
                    {
                        var vertexBuffer = definition.VertexBuffers[i].Definition;

                        using(var vertexStream = new MemoryStream(vertexBuffer.Data.Data))
                        using(var vertexReader = new EndianReader(vertexStream, EndianFormat.LittleEndian))
                        {
                            switch (vertexBuffer.Format)
                            {
                                case VertexBufferFormat.TinyPosition:
                                    for (var j = 0; j < vertexBuffer.Count; j++)
                                    {
                                        fileWriter.WriteLine($"Position = ({vertexReader.ReadUInt16().ToString("X4")},{vertexReader.ReadUInt16().ToString("X4")},{vertexReader.ReadUInt16().ToString("X4")},{vertexReader.ReadUInt16().ToString("X4")})");
                                        fileWriter.WriteLine($"Normal   = ({vertexReader.ReadByte().ToString("X2")},{vertexReader.ReadByte().ToString("X2")},{vertexReader.ReadByte().ToString("X2")},{vertexReader.ReadByte().ToString("X2")})");
                                        fileWriter.WriteLine($"Color    = {vertexReader.ReadUInt32().ToString("X8")}");
                                    }
                                    break;

                                case VertexBufferFormat.StaticPerVertex:
                                    for (var j = 0; j < vertexBuffer.Count; j++)
                                    {
                                        fileWriter.WriteLine($"{vertexBuffer.Format} index: {j}:");
                                        fileWriter.WriteLine($"Texcoord = " + vertexReader.ReadUInt32().ToString("X8"));
                                        fileWriter.WriteLine($"Texcoord = " + vertexReader.ReadUInt32().ToString("X8"));
                                        fileWriter.WriteLine($"Texcoord = " + vertexReader.ReadUInt32().ToString("X8"));
                                        fileWriter.WriteLine($"Texcoord = " + vertexReader.ReadUInt32().ToString("X8"));
                                        fileWriter.WriteLine($"Texcoord = " + vertexReader.ReadUInt32().ToString("X8"));
                                        fileWriter.WriteLine($"End of {vertexBuffer.Format} index: {j}");
                                        fileWriter.WriteLine(Environment.NewLine);
                                    }
                                    break;

                                case VertexBufferFormat.Skinned:
                                    for (var j = 0; j < vertexBuffer.Count; j++)
                                    {
                                        fileWriter.WriteLine($"{j}:");
                                        fileWriter.WriteLine($"Position X = " + vertexReader.ReadSingle() + " Y = " + vertexReader.ReadSingle() + " Z = " + vertexReader.ReadSingle());
                                        fileWriter.WriteLine($"Texcoord U = " + vertexReader.ReadSingle() + " V = " + vertexReader.ReadSingle());
                                        fileWriter.WriteLine($"Normal   X = " + vertexReader.ReadSingle() + " Y = " + vertexReader.ReadSingle() + " Z = " + vertexReader.ReadSingle());
                                        fileWriter.WriteLine($"Tangent  X = " + vertexReader.ReadSingle() + " Y = " + vertexReader.ReadSingle() + " Z = " + vertexReader.ReadSingle());
                                        fileWriter.WriteLine($"Binormal X = " + vertexReader.ReadSingle() + " Y = " + vertexReader.ReadSingle() + " Z = " + vertexReader.ReadSingle());
                                        vertexReader.ReadUInt32();
                                        vertexReader.ReadUInt32();
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            
            

            return true;
        }
    }
}