using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Geometry;
using TagTool.Tags.Definitions.Gen4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using TagTool.Common;
using TagTool.Commands.Porting.Gen4;

namespace TagTool.Commands.Gen4.ScenarioStructureBSPs
{
    public class ExtractRenderGeometryCommand : Command
    {
        private GameCache CacheContext { get; }
        private ScenarioStructureBsp Definition { get; }
        private CachedTag Tag { get; }

        public ExtractRenderGeometryCommand(GameCache cacheContext, ScenarioStructureBsp definition, CachedTag tag)
            : base(true,

                  "ExtractRenderGeometry",
                  "Extracts render geometry from the current scenario_structure_bsp definition.",

                  "ExtractRenderGeometry <filetype> <filename>",

                  "Extracts render geometry from the current scenario_structure_bsp definition.\n" +
                  "Supported file types: obj")
        {
            CacheContext = cacheContext;
            Definition = definition;
            Tag = tag;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return new TagToolError(CommandError.ArgCount);

            var fileType = args[0];
            var fileName = args[1];

            if (fileType != "obj")
                return new TagToolError(CommandError.FileType);

            if (Definition.RenderGeometry.ApiResource == null)
            {
                Console.WriteLine("Render geometry does not have a resource associated with it.");
                return true;
            }

            //
            // Deserialize the resource definition
            //

            var collresource = CacheContext.ResourceCache.GetStructureBspCacheFileTagResourcesGen4(Definition.ResourceInterface.CacheFileResources);
            if (!CacheContext.TagCache.TryGetTag<ScenarioLightmapBspData>(Tag.Name, out var LbspTag))
                return new TagToolError(CommandError.CustomError, "Lightmap tag not found!");
            var cacheStream = CacheContext.OpenCacheRead();
            ScenarioLightmapBspData lightmap = CacheContext.Deserialize<ScenarioLightmapBspData>(cacheStream, LbspTag);          
            var definition = CacheContext.ResourceCache.GetRenderGeometryApiResourceDefinitionGen4(lightmap.ImportedGeometry.ApiResource);

            if(definition == null)
                return new TagToolError(CommandError.CustomError, "Lightmap resource not found!");
            if(collresource == null)
                return new TagToolError(CommandError.CustomError, "Structure bsp resource not found!");

            var Geometry = RenderModelConverter.ConvertGeometry(lightmap.ImportedGeometry);
            var renderResource = RenderModelConverter.ConvertResource(definition);           
            Geometry.SetResourceBuffers(renderResource, true);

            //format documented as unused0 in H4, seems to extract ok if treated as rigid boned
            foreach (var mesh in Geometry.Meshes)
            {
                if (mesh.ReachType == VertexTypeReach.Contrail && mesh.ResourceVertexBuffers[0] != null)
                {
                    mesh.ReachType = VertexTypeReach.RigidBoned;
                    mesh.ResourceVertexBuffers[0].Format = VertexBufferFormat.RigidBoned;
                }
            }

            List<RenderMaterial> materialList = new List<RenderMaterial>();
            foreach (var material in Definition.Materials)
                materialList.Add(new RenderMaterial { RenderMethod = material.RenderMethod });

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
                        var meshReader = new MeshReader(CacheContext, Geometry.Meshes[cluster.MeshIndex]);
                        objExtractor.ExtractMesh(meshReader, null, materialList, String.Format("cluster_{0}", i));
                        i++;
                    }

                    var j = 0;
                    foreach (var instance in collresource.InstancedGeometryInstances)
                    {
                        var vertexCompressor = new VertexCompressor(Geometry.Compression[instance.CompressionIndex]);
                        var meshReader = new MeshReader(CacheContext, Geometry.Meshes[instance.MeshIndex]);

                        var scale = Matrix4x4.CreateScale(instance.Scale);
                        var transform = scale * new Matrix4x4(
                                    instance.Matrix.m11, instance.Matrix.m12, instance.Matrix.m13, 0.0f,
                                    instance.Matrix.m21, instance.Matrix.m22, instance.Matrix.m23, 0.0f,
                                    instance.Matrix.m31, instance.Matrix.m32, instance.Matrix.m33, 0.0f,
                                    instance.Matrix.m41, instance.Matrix.m42, instance.Matrix.m43, 0.0f);

                        objExtractor.ExtractMesh(meshReader, vertexCompressor, materialList,
                            String.Concat("%", CacheContext.StringTable.GetString(Definition.InstancedGeometryInstanceNames[j].Name)), transform);
                        j++;
                    }

                    objExtractor.Finish();
                }
            }

            Console.WriteLine("Done!");

            return true;
        }
    }
}