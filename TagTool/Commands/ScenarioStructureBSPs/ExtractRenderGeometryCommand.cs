using BlamCore.Cache;
using BlamCore.Geometry;
using BlamCore.Serialization;
using BlamCore.TagDefinitions;
using BlamCore.TagResources;
using System;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Commands.ScenarioStructureBSPs
{
    class ExtractRenderGeometryCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private ScenarioStructureBsp Definition { get; }

        public ExtractRenderGeometryCommand(GameCacheContext cacheContext, ScenarioStructureBsp definition)
            : base(CommandFlags.Inherit,

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
                return false;

            var fileType = args[0];
            var fileName = args[1];

            if (fileType != "obj")
                throw new NotSupportedException(fileType);

            if (Definition.Geometry2.Resource == null)
            {
                Console.WriteLine("ERROR: Render geometry does not have a resource associated with it.");
                return true;
            }

            //
            // Deserialize the resource definition
            //

            var resourceContext = new ResourceSerializationContext(Definition.Geometry2.Resource);
            var definition = CacheContext.Deserializer.Deserialize<RenderGeometryApiResourceDefinition>(resourceContext);

            using (var resourceStream = new MemoryStream())
            {
                //
                // Extract the resource data
                //

                CacheContext.ExtractResource(Definition.Geometry2.Resource, resourceStream);

                var file = new FileInfo(fileName);

                if (!file.Directory.Exists)
                    file.Directory.Create();

                using (var objFile = new StreamWriter(file.Create()))
                {
                    var objExtractor = new ObjExtractor(objFile);

                    foreach (var cluster in Definition.Clusters)
                    {
                        var meshReader = new MeshReader(CacheContext.Version, Definition.Geometry2.Meshes[cluster.MeshIndex], definition);
                        objExtractor.ExtractMesh(meshReader, null, resourceStream);
                    }

                    foreach (var instance in Definition.InstancedGeometryInstances)
                    {
                        var vertexCompressor = new VertexCompressor(Definition.Geometry2.Compression[0]);
                        var meshReader = new MeshReader(CacheContext.Version, Definition.Geometry2.Meshes[instance.MeshIndex], definition);
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