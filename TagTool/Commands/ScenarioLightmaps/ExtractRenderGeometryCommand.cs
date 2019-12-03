using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Geometry;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Commands.ScenarioLightmaps
{
    class ExtractRenderGeometryCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private ScenarioLightmapBspData Definition { get; }

        public ExtractRenderGeometryCommand(HaloOnlineCacheContext cacheContext, ScenarioLightmapBspData definition)
            : base(true,

                  "ExtractRenderGeometry",
                  "Extracts render geometry from the current scenario_lightmap_bsp_data definition.",

                  "ExtractRenderGeometry <filetype> <filename>",

                  "Extracts render geometry from the current scenario_lightmap_bsp_data definition.\n" +
                  "Supported file types: obj")
        {
            CacheContext = cacheContext;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            var fileType = args[0];
            var fileName = args[1];

            if (fileType != "obj")
                throw new NotSupportedException(fileType);

            if (Definition.Geometry.Resource == null)
            {
                Console.WriteLine("ERROR: Render geometry does not have a resource associated with it.");
                return true;
            }

            //
            // Deserialize the resource definition
            //

            var resourceContext = new ResourceSerializationContext(CacheContext, Definition.Geometry.Resource);
            var definition = CacheContext.Deserializer.Deserialize<RenderGeometryApiResourceDefinition>(resourceContext);

            using (var resourceStream = new MemoryStream())
            {
                //
                // Extract the resource data
                //

                CacheContext.ExtractResource(Definition.Geometry.Resource, resourceStream);

                var file = new FileInfo(fileName);

                if (!file.Directory.Exists)
                    file.Directory.Create();

                using (var objFile = new StreamWriter(file.Create()))
                {
                    var objExtractor = new ObjExtractor(objFile);

                    foreach (var mesh in Definition.Geometry.Meshes)
                    {
                        var vertexCompressor = new VertexCompressor(Definition.Geometry.Compression[0]);
                        var meshReader = new MeshReader(CacheContext.Version, mesh, definition);
                        objExtractor.ExtractMesh(meshReader, vertexCompressor, resourceStream);
                    }

                    objExtractor.Finish();
                }
            }

            Console.WriteLine("Done!");

            return true;
        }
    }
}
