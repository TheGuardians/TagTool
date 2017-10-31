using BlamCore.Cache;
using BlamCore.Geometry;
using BlamCore.IO;
using BlamCore.Serialization;
using BlamCore.TagResources;
using System;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Commands.Geometry
{
    class DumpRenderGeometryCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private RenderGeometry Geometry { get; }

        public DumpRenderGeometryCommand(GameCacheContext cacheContext, RenderGeometry geometry, string title = "") :
            base(CommandFlags.Inherit,

                $"Dump{title}RenderGeometry",
                $"Dumps {title.ToLower()} render geometry in ascii format.",

                $"Dump{title}RenderGeometry [raw] <Output File>",

                $"Dumps {title.ToLower()} render geometry in ascii format.")
        {
            CacheContext = cacheContext;
            Geometry = geometry;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count >2)
                return false;

            if (args.Count == 2 && (args[0].ToLower() != "raw"))
                return false;

            var file = "";
            if (args.Count == 1)
                file = args[0];
            else
                file = args[1];

            var resourceContext = new ResourceSerializationContext(Geometry.Resource);
            var definition = CacheContext.Deserializer.Deserialize<RenderGeometryApiResourceDefinition>(resourceContext);

            if (args.Count == 2)
            {
                using (EndianWriter output = new EndianWriter(File.OpenWrite(args[1]), EndianFormat.LittleEndian))
                {
                    using (var edResourceStream = new MemoryStream())
                    using (var edResourceReader = new EndianReader(edResourceStream, EndianFormat.LittleEndian))
                    {
                        CacheContext.ExtractResource(Geometry.Resource, edResourceStream);
                        edResourceStream.Position = 0;
                        byte[] data = edResourceReader.ReadBytes((int)edResourceStream.Length);
                        output.WriteBlock(data);
                    }
                }
            }
            else
            {
                //
                // Convert Blam data to ElDorado data
                //

                using (var fileStream = File.Create(file))
                using (var fileWriter = new StreamWriter(fileStream))
                {
                    using (var edResourceStream = new MemoryStream())
                    using (var edResourceReader = new EndianReader(edResourceStream, EndianFormat.LittleEndian))
                    {
                        //
                        // Convert Blam vertex buffers
                        //

                        Console.Write("Converting vertex buffers...");
                        CacheContext.ExtractResource(Geometry.Resource, edResourceStream);

                        for (var i = 0; i < definition.VertexBuffers.Count; i++)
                        {
                            edResourceStream.Position = definition.VertexBuffers[i].Definition.Data.Address.Offset;

                            var vertexBuffer = definition.VertexBuffers[i].Definition;
                            
                            fileWriter.WriteLine($"Offset = {vertexBuffer.Data.Address.Offset.ToString("X8")} Count = {vertexBuffer.Count} Size = {vertexBuffer.VertexSize}, Format = {vertexBuffer.Format.ToString()}");
                            //fileWriter.WriteLine(Environment.NewLine);
                            
                            //fileWriter.WriteLine($"Vertex buffer index: {i}");
                            
                            switch (vertexBuffer.Format)
                            {
                                case VertexBufferFormat.TinyPosition:
                                    for (var j = 0; j < vertexBuffer.Count; j++)
                                    {
                                        fileWriter.WriteLine($"(I,J,K,W) = ({edResourceReader.ReadUInt32().ToString("X4")},{edResourceReader.ReadUInt32().ToString("X4")},{edResourceReader.ReadUInt32().ToString("X4")},{edResourceReader.ReadUInt32().ToString("X4")})");
                                    }
                                    break;
                                    /*
                                    case VertexBufferFormat.Unknown1B:
                                        var goodCount = 0;
                                        for (var j = 0; j < vertexBuffer.Count; j++)
                                        {
                                            //fileWriter.WriteLine($"Index: {j}:");
                                            string values = $"({ edResourceReader.ReadSingle()},{ edResourceReader.ReadSingle()}," +
                                                $"{edResourceReader.ReadSingle()},{edResourceReader.ReadSingle()},{edResourceReader.ReadSingle()},{edResourceReader.ReadSingle()}" +
                                                $",{edResourceReader.ReadSingle()},{edResourceReader.ReadSingle()},{edResourceReader.ReadSingle()})";
                                            if(values != "(0,0,0,0,0,0,0,0,0)")
                                            {
                                                goodCount++;
                                                //fileWriter.WriteLine($"(1,2,3,4,5,6,7,8,9) = "+values);
                                            }


                                            /*
                                            fileWriter.WriteLine($"Unknown 1 = " + edResourceReader.ReadSingle());
                                            fileWriter.WriteLine($"Unknown 2 = " + edResourceReader.ReadSingle());
                                            fileWriter.WriteLine($"Unknown 3 = " + edResourceReader.ReadSingle());
                                            fileWriter.WriteLine($"Unknown 4 = " + edResourceReader.ReadSingle());
                                            fileWriter.WriteLine($"Unknown 5 = " + edResourceReader.ReadSingle());
                                            fileWriter.WriteLine($"Unknown 6 = " + edResourceReader.ReadSingle());
                                            fileWriter.WriteLine($"Unknown 7 = " + edResourceReader.ReadSingle());
                                            fileWriter.WriteLine($"Unknown 8 = " + edResourceReader.ReadSingle());
                                            fileWriter.WriteLine($"Unknown 9 = " + edResourceReader.ReadSingle());

                                        }
                                        //fileWriter.WriteLine($"Valid Unknown1B count = {goodCount}");
                                        break;

                                    case VertexBufferFormat.Unknown1A:
                                        for (var j = 0; j < vertexBuffer.Count; j++)
                                        {
                                            //fileWriter.WriteLine($"Index: {j}:");
                                            //fileWriter.WriteLine($"Unknown 1 = " + edResourceReader.ReadUInt32().ToString("X8"));
                                            fileWriter.WriteLine($"(1,2,3) = ({edResourceReader.ReadUInt32().ToString("X8")},{edResourceReader.ReadUInt32().ToString("X8")},{edResourceReader.ReadUInt32().ToString("X8")})");
                                        }
                                        break;

                                    case VertexBufferFormat.Rigid:
                                        for (var j = 0; j < vertexBuffer.Count; j++)
                                        {
                                            fileWriter.WriteLine($"{j}:");
                                            fileWriter.WriteLine($"Position X = " + edResourceReader.ReadSingle() + " Y = " + edResourceReader.ReadSingle() + " Z = " + edResourceReader.ReadSingle());
                                            fileWriter.WriteLine($"Texcoord U = " + edResourceReader.ReadSingle() + " V = " + edResourceReader.ReadSingle());
                                            fileWriter.WriteLine($"Normal   X = " + edResourceReader.ReadSingle() + " Y = " + edResourceReader.ReadSingle() + " Z = " + edResourceReader.ReadSingle());
                                            fileWriter.WriteLine($"Tangent  X = " + edResourceReader.ReadSingle() + " Y = " + edResourceReader.ReadSingle() + " Z = " + edResourceReader.ReadSingle());
                                            fileWriter.WriteLine($"Binormal X = " + edResourceReader.ReadSingle() + " Y = " + edResourceReader.ReadSingle() + " Z = " + edResourceReader.ReadSingle());
                                        }
                                        break;

                                    case VertexBufferFormat.StaticPerPixel:
                                        for (var j = 0; j < vertexBuffer.Count; j++)
                                        {
                                            fileWriter.WriteLine($"{j} U = " + edResourceReader.ReadSingle() + " V = " + edResourceReader.ReadSingle());
                                            //fileWriter.WriteLine(Environment.NewLine);
                                        }
                                        break;

                                    case VertexBufferFormat.AmbientPrt:
                                        for (var j = 0; j < vertexBuffer.Count; j++)
                                        {
                                            fileWriter.WriteLine($"{j} Blend weight = " + edResourceReader.ReadSingle());
                                            //fileWriter.WriteLine(Environment.NewLine);
                                        }
                                        break;


                                    case VertexBufferFormat.World:
                                        for (var j = 0; j < vertexBuffer.Count; j++)
                                        {
                                            fileWriter.WriteLine($"{j}:");
                                            fileWriter.WriteLine($"Position X = " + edResourceReader.ReadSingle() + " Y = " + edResourceReader.ReadSingle() + " Z = " + edResourceReader.ReadSingle());
                                            fileWriter.WriteLine($"Texcoord U = " + edResourceReader.ReadSingle() + " V = " + edResourceReader.ReadSingle());
                                            fileWriter.WriteLine($"Normal   X = " + edResourceReader.ReadSingle() + " Y = " + edResourceReader.ReadSingle() + " Z = " + edResourceReader.ReadSingle());
                                            fileWriter.WriteLine($"Tangent  X = " + edResourceReader.ReadSingle() + " Y = " + edResourceReader.ReadSingle() + " Z = " + edResourceReader.ReadSingle());
                                            fileWriter.WriteLine($"Binormal X = " + edResourceReader.ReadSingle() + " Y = " + edResourceReader.ReadSingle() + " Z = " + edResourceReader.ReadSingle());
                                        }
                                        break;


                                    case VertexBufferFormat.StaticPerVertex:
                                        for (var j = 0; j < vertexBuffer.Count; j++)
                                        {
                                            fileWriter.WriteLine($"{vertexBuffer.Format} index: {j}:");
                                            fileWriter.WriteLine($"Texcoord = " + edResourceReader.ReadUInt32().ToString("X8"));
                                            fileWriter.WriteLine($"Texcoord = " + edResourceReader.ReadUInt32().ToString("X8"));
                                            fileWriter.WriteLine($"Texcoord = " + edResourceReader.ReadUInt32().ToString("X8"));
                                            fileWriter.WriteLine($"Texcoord = " + edResourceReader.ReadUInt32().ToString("X8"));
                                            fileWriter.WriteLine($"Texcoord = " + edResourceReader.ReadUInt32().ToString("X8"));
                                            fileWriter.WriteLine($"End of {vertexBuffer.Format} index: {j}");
                                            fileWriter.WriteLine(Environment.NewLine);
                                        }
                                        break;
                                        */
                                    /*
                                        case VertexBufferFormat.QuadraticPrt:
                                            for (var j = 0; j < vertexBuffer.Count; j++)
                                            {
                                                fileWriter.WriteLine($"{vertexBuffer.Format} index: {j}:");
                                                fileWriter.WriteLine($"A X = " + edResourceReader.ReadSingle() + " Y = " + edResourceReader.ReadSingle() + " Z = " + edResourceReader.ReadSingle());
                                                fileWriter.WriteLine($"B X = " + edResourceReader.ReadSingle() + " Y = " + edResourceReader.ReadSingle() + " Z = " + edResourceReader.ReadSingle());
                                                fileWriter.WriteLine($"C X = " + edResourceReader.ReadSingle() + " Y = " + edResourceReader.ReadSingle() + " Z = " + edResourceReader.ReadSingle());
                                                fileWriter.WriteLine($"End of {vertexBuffer.Format} index: {j}");
                                                fileWriter.WriteLine(Environment.NewLine);
                                            }
                                            break;



                                        case VertexBufferFormat.Unknown1B:
                                            for (var j = 0; j < vertexBuffer.Count; j++)
                                            {
                                                fileWriter.WriteLine($"{vertexBuffer.Format} index: {j}:");
                                                fileWriter.WriteLine($"Unknown 1 = " + edResourceReader.ReadSingle());
                                                fileWriter.WriteLine($"Unknown 2 = " + edResourceReader.ReadSingle());
                                                fileWriter.WriteLine($"Unknown 3 = " + edResourceReader.ReadSingle());
                                                fileWriter.WriteLine($"Unknown 4 = " + edResourceReader.ReadSingle());
                                                fileWriter.WriteLine($"Unknown 5 = " + edResourceReader.ReadSingle());
                                                fileWriter.WriteLine($"Unknown 6 = " + edResourceReader.ReadSingle());
                                                fileWriter.WriteLine($"Unknown 7 = " + edResourceReader.ReadSingle());
                                                fileWriter.WriteLine($"Unknown 8 = " + edResourceReader.ReadSingle());
                                                fileWriter.WriteLine($"Unknown 9 = " + edResourceReader.ReadSingle());
                                                fileWriter.WriteLine($"End of {vertexBuffer.Format} index: {j}");
                                                fileWriter.WriteLine(Environment.NewLine);
                                            }
                                            break;

                                        case VertexBufferFormat.Decorator:
                                            for (var j = 0; j < vertexBuffer.Count; j++)
                                            {
                                                fileWriter.WriteLine($"{vertexBuffer.Format} index: {j}:");
                                                fileWriter.WriteLine($"Position X = " + edResourceReader.ReadSingle() + " Y = " + edResourceReader.ReadSingle() + " Z = " + edResourceReader.ReadSingle());
                                                fileWriter.WriteLine($"Texcoord U = " + edResourceReader.ReadSingle() + " V = " + edResourceReader.ReadSingle());
                                                fileWriter.WriteLine($"Normal   X = " + edResourceReader.ReadSingle() + " Y = " + edResourceReader.ReadSingle() + " Z = " + edResourceReader.ReadSingle());
                                                fileWriter.WriteLine($"End of {vertexBuffer.Format} index: {j}");
                                                fileWriter.WriteLine(Environment.NewLine);
                                            }
                                            break;

                                        case VertexBufferFormat.LinearPrt:
                                            for (var j = 0; j < vertexBuffer.Count; j++)
                                            {
                                                fileWriter.WriteLine($"{vertexBuffer.Format} index: {j}:");
                                                fileWriter.WriteLine($"Hex : " + edResourceReader.ReadUInt32().ToString("X8"));
                                                fileWriter.WriteLine($"End of {vertexBuffer.Format} index: {j}");
                                                fileWriter.WriteLine(Environment.NewLine);
                                            }
                                            break;
                                            */




                            }
                            
                            //fileWriter.WriteLine($"End of Vertex Buffer index: {i}");
                            
                            //fileWriter.WriteLine(Environment.NewLine);
                            
                        }
                    }
                }
            }
            
            

            return true;
        }
    }
}