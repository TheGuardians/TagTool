using TagTool.Cache;
using TagTool.Commands;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Commands.Geometry
{
    class DumpRenderGeometryCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private RenderGeometry Geometry { get; }

        public DumpRenderGeometryCommand(HaloOnlineCacheContext cacheContext, RenderGeometry geometry, string title = "") :
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
                using (var edResourceStream = new MemoryStream())
                using (var edResourceReader = new EndianReader(edResourceStream, EndianFormat.LittleEndian))
                {
                    var directory = args[1];
                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    var dataOutDir = directory;

                    for (var i = 0; i < definition.VertexBuffers.Count; i++)
                    {
                        edResourceStream.Position = definition.VertexBuffers[i].Definition.Data.Address.Offset;

                        var vertexBuffer = definition.VertexBuffers[i].Definition;

                        dataOutDir = Path.Combine(directory, $"{i}_{vertexBuffer.Format}_{vertexBuffer.Count}");

                        using (EndianWriter output = new EndianWriter(File.OpenWrite(dataOutDir), EndianFormat.LittleEndian))
                        {
                            byte[] data = edResourceReader.ReadBytes((int)vertexBuffer.Data.Size);
                            output.WriteBlock(data);
                        }
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
                            
                            //fileWriter.WriteLine($"Offset = {vertexBuffer.Data.Address.Offset.ToString("X8")} Count = {vertexBuffer.Count} Size = {vertexBuffer.VertexSize}, Format = {vertexBuffer.Format.ToString()}");
                            //fileWriter.WriteLine(Environment.NewLine);
                            
                            //fileWriter.WriteLine($"Vertex buffer index: {i}");
                            
                            switch (vertexBuffer.Format)
                            {
                                case VertexBufferFormat.TinyPosition:
                                    for (var j = 0; j < vertexBuffer.Count; j++)
                                    {
                                        
                                        fileWriter.WriteLine($"Position = ({edResourceReader.ReadUInt16().ToString("X4")},{edResourceReader.ReadUInt16().ToString("X4")},{edResourceReader.ReadUInt16().ToString("X4")},{edResourceReader.ReadUInt16().ToString("X4")})");
                                        fileWriter.WriteLine($"Normal   = ({edResourceReader.ReadByte().ToString("X2")},{edResourceReader.ReadByte().ToString("X2")},{edResourceReader.ReadByte().ToString("X2")},{edResourceReader.ReadByte().ToString("X2")})");
                                        fileWriter.WriteLine($"Color    = {edResourceReader.ReadUInt32().ToString("X8")}");
                                        
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