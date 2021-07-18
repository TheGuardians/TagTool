using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Geometry;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.ScenarioLightmaps
{
    class ExtractRenderGeometryCommand : Command
    {
        private GameCache Cache { get; }
        private ScenarioLightmapBspData Definition { get; }

        public ExtractRenderGeometryCommand(GameCache cache, ScenarioLightmapBspData definition)
            : base(true,

                  "ExtractRenderGeometry",
                  "Extracts render geometry from the current scenario_lightmap_bsp_data definition.",

                  "ExtractRenderGeometry <filetype> <filename>",

                  "Extracts render geometry from the current scenario_lightmap_bsp_data definition.\n" +
                  "Supported file types: obj")
        {
            Cache = cache;
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

            if (Definition.Geometry.Resource == null)
            {
                Console.WriteLine("Render geometry does not have a resource associated with it.");
                return true;
            }

            //
            // Deserialize the resource definition
            //

            var definition = Cache.ResourceCache.GetRenderGeometryApiResourceDefinition(Definition.Geometry.Resource);
            Definition.Geometry.SetResourceBuffers(definition);

            using (var resourceStream = new MemoryStream())
            {
                //
                // Extract the resource data
                //
                var file = new FileInfo(fileName);

                if (!file.Directory.Exists)
                    file.Directory.Create();

                using (var objFile = new StreamWriter(file.Create()))
                {
                    var objExtractor = new ObjExtractor(objFile);

                    foreach (var mesh in Definition.Geometry.Meshes)
                    {
                        var vertexCompressor = new VertexCompressor(Definition.Geometry.Compression[0]);
                        var meshReader = new MeshReader(Cache.Version, Cache.Platform, mesh);
                        objExtractor.ExtractMesh(meshReader, vertexCompressor);
                    }

                    objExtractor.Finish();
                }
            }

            Console.WriteLine("Done!");

            return true;
        }
    }
}
