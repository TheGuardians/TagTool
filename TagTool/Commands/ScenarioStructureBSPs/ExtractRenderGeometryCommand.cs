using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Geometry;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using TagTool.Common;

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

            var collresource = CacheContext.ResourceCache.GetStructureBspTagResources(Definition.CollisionBspResource);
            var definition = CacheContext.ResourceCache.GetRenderGeometryApiResourceDefinition(Definition.Geometry.Resource);
            Definition.Geometry.SetResourceBuffers(definition);

            using (var resourceStream = new MemoryStream())
            {
                var file = new FileInfo(fileName);

                if (!file.Directory.Exists)
                    file.Directory.Create();

                using (var objFile = new StreamWriter(file.Create()))
                {
                    var objExtractor = new ObjExtractor(CacheContext, objFile);

                    var i = 0;
                    foreach (var cluster in Definition.Clusters)
                    {
                        var meshReader = new MeshReader(CacheContext, Definition.Geometry.Meshes[cluster.MeshIndex]);
                        objExtractor.ExtractMesh(meshReader, null, Definition.Materials, String.Format("cluster_{0}", i));
                        i++;
                    }

                    foreach (var instance in Definition.InstancedGeometryInstances)
                    {
                        var instanceDef = collresource.InstancedGeometry[instance.DefinitionIndex];
                        var vertexCompressor = new VertexCompressor(Definition.Geometry.Compression[instanceDef.CompressionIndex]);
                        var meshReader = new MeshReader(CacheContext, Definition.Geometry.Meshes[instanceDef.MeshIndex]);

                        var scale = Matrix4x4.CreateScale(instance.Scale);
                        var transform = scale * new Matrix4x4(
                                    instance.Matrix.m11, instance.Matrix.m12, instance.Matrix.m13, 0.0f,
                                    instance.Matrix.m21, instance.Matrix.m22, instance.Matrix.m23, 0.0f,
                                    instance.Matrix.m31, instance.Matrix.m32, instance.Matrix.m33, 0.0f,
                                    instance.Matrix.m41, instance.Matrix.m42, instance.Matrix.m43, 0.0f);

                        if (CacheContext.Version >= CacheVersion.HaloReach)
                        {
                            objExtractor.ExtractMesh(meshReader, vertexCompressor, Definition.Materials,
                                String.Concat("%", CacheContext.StringTable.GetString(instance.NameReach)), transform);
                        }
                        else
                        {
                            objExtractor.ExtractMesh(meshReader, vertexCompressor, Definition.Materials,
                                String.Concat("%", CacheContext.StringTable.GetString(instance.Name)), transform);
                        }
                    }

                    objExtractor.Finish();
                }
            }

            Console.WriteLine("Done!");

            return true;
        }
    }
}