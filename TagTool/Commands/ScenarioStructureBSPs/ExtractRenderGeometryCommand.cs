using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Geometry;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Commands.ScenarioStructureBSPs
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

            if (Definition.Geometry.Resource == null)
            {
                Console.WriteLine("Render geometry does not have a resource associated with it.");
                return true;
            }

            //
            // Deserialize the resource definition
            //


            var definition = CacheContext.ResourceCache.GetRenderGeometryApiResourceDefinition(Definition.Geometry.Resource);
            Definition.Geometry.SetResourceBuffers(definition);

            using (var resourceStream = new MemoryStream())
            {
                var file = new FileInfo(fileName);

                if (!file.Directory.Exists)
                    file.Directory.Create();

                using (var objFile = new StreamWriter(file.Create()))
                {
                    var objExtractor = new ObjExtractor(objFile);

                    var i = 0;
                    foreach (var cluster in Definition.Clusters)
                    {
                        var meshReader = new MeshReader(CacheContext.Version, Definition.Geometry.Meshes[cluster.MeshIndex]);
                        objExtractor.ExtractMesh(meshReader, null, String.Format("cluster_{0}", i));
                        i++;
                    }

                    foreach (var instance in Definition.InstancedGeometryInstances)
                    {
                        var vertexCompressor = new VertexCompressor(Definition.Geometry.Compression[0]);
                        var meshReader = new MeshReader(CacheContext.Version, Definition.Geometry.Meshes[instance.DefinitionIndex]);
                        objExtractor.ExtractMesh(meshReader, vertexCompressor, CacheContext.StringTable.GetString(instance.Name));
                    }

                    objExtractor.Finish();
                }
            }

            Console.WriteLine("Done!");

            return true;
        }
    }
}