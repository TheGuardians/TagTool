using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Geometry;
using TagTool.Tags.Definitions.Gen2;
using System;
using System.Collections.Generic;
using System.IO;
using static TagTool.Commands.Porting.Gen2.Gen2BspGeometryConverter;
using TagTool.Common;
using System.Linq;

namespace TagTool.Commands.Gen2.ScenarioStructureBSPs
{
    class ExtractRenderGeometryCommand : Command
    {
        private GameCache CacheContext { get; }
        private ScenarioStructureBsp Definition { get; }

        public ExtractRenderGeometryCommand(GameCache cacheContext, ScenarioStructureBsp definition)
            : base(true,

                  "ExtractRenderGeometry",
                  "Extracts render geometry from the current scenario_structure_bsp definition.",

                  "ExtractRenderGeometry <filetype> <filename>",

                  "Extracts render geometry from the current scenario_structure_bsp definition.\n" +
                  "Supported file types: obj")
        {
            CacheContext = cacheContext;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return new TagToolError(CommandError.ArgCount);

            var fileType = args[0];
            var fileName = args[1];

            if (fileType != "obj")
                return new TagToolError(CommandError.FileType);

            //
            // Deserialize the resource definition
            //

            using (var resourceStream = new MemoryStream())
            {
                var file = new FileInfo(fileName);

                if (!file.Directory.Exists)
                    file.Directory.Create();

                using (var objFile = new StreamWriter(file.Create()))
                {
                    List<List<RealPoint3d>> vertices = new List<List<RealPoint3d>>();
                    List<List<ushort>> indices = new List<List<ushort>>();
                    foreach (var cluster in Definition.Clusters)
                    {
                        //render geometry
                        var compressor = new VertexCompressor(
                            cluster.SectionInfo.Compression.Count > 0 ?
                                cluster.SectionInfo.Compression[0] :
                                new RenderGeometryCompression
                                {
                                    X = new Bounds<float>(0.0f, 1.0f),
                                    Y = new Bounds<float>(0.0f, 1.0f),
                                    Z = new Bounds<float>(0.0f, 1.0f),
                                    U = new Bounds<float>(0.0f, 1.0f),
                                    V = new Bounds<float>(0.0f, 1.0f),
                                    U2 = new Bounds<float>(0.0f, 1.0f),
                                    V2 = new Bounds<float>(0.0f, 1.0f),
                                });
                        List<Gen2BSPResourceMesh> clustermeshes = ReadResourceMeshes((GameCacheGen2)CacheContext, cluster.GeometryBlockInfo,
                            cluster.SectionInfo.TotalVertexCount, (RenderGeometryCompressionFlags)cluster.SectionInfo.GeometryCompressionFlags,
                            (RenderModel.SectionLightingFlags)cluster.SectionInfo.SectionLightingFlags, compressor);
                        var mesh = clustermeshes[0];

                        vertices.Add(mesh.RawVertices.Select(v => v.Point.Position).ToList());
                        indices.Add(mesh.StripIndices.Select(i => (ushort)i.Index).ToList());
                    }

                    foreach(var cvertices in vertices)
                        foreach(var vertex in cvertices)
                        objFile.WriteLine($"v {vertex.X * 100.0} {vertex.Z * 100.0} {vertex.Y * 100.0}");
                    int vertexoffset = 1;
                    for(var clusterindex = 0; clusterindex < indices.Count; clusterindex++)
                    {
                        objFile.WriteLine($"o Cluster_{clusterindex}");
                        var j = 0;
                        while (j < indices[clusterindex].Count)
                        {
                            objFile.WriteLine($"f {indices[clusterindex][j++] + vertexoffset} {indices[clusterindex][j++] + vertexoffset} {indices[clusterindex][j++] + vertexoffset}");
                        }
                        vertexoffset += vertices[clusterindex].Count;
                    }
                }
            }

            Console.WriteLine("Done!");

            return true;
        }
    }
}