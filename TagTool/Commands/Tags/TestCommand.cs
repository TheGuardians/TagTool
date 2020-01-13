using TagTool.Cache;
using TagTool.IO;
using System.Collections.Generic;
using System.IO;
using System;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Tags;
using TagTool.Serialization;
using TagTool.Bitmaps;
using TagTool.Tags.Resources;
using TagTool.Bitmaps.Utils;
using TagTool.Bitmaps.DDS;
using TagTool.Geometry;

namespace TagTool.Commands
{
    
    public class TestCommand : Command
    {
        GameCache Cache;

        public TestCommand(GameCache cache) : base(false, "Test", "Test", "Test", "Test")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 0)
                return false;

            //
            // Insert what test command you want below
            //

            using(var stream = Cache.TagCache.OpenTagCacheRead())
            {
                foreach(var tag in Cache.TagCache.TagTable)
                {
                    /*
                    if(tag.Group.Tag == "sLdT")
                    {
                        var sldt = Cache.Deserialize<ScenarioLightmap>(stream, tag);
                        var resource = Cache.ResourceCache.GetRenderGeometryApiResourceDefinition(sldt.Lightmaps[0].Geometry.Resource);
                        sldt.Lightmaps[0].Geometry.SetResourceBuffers(resource);
                        foreach(var mesh in sldt.Lightmaps[0].Geometry.Meshes)
                        {
                            if (mesh.Water.Count != 0)
                            {
                                var unknown1A = mesh.ResourceVertexBuffers[6];
                                var unknown1B = mesh.ResourceVertexBuffers[7];
                                var fileWorld = new FileInfo($"Geometry\\h3_riverworld_unknown1A_water_{mesh.VertexBufferIndices[6]}.bin");
                                var fileParameters = new FileInfo($"Geometry\\h3_riverworld_parameters_water_{mesh.VertexBufferIndices[7]}.bin");
                                using (var fileStream = fileWorld.OpenWrite())
                                {
                                    fileStream.Write(unknown1A.Data.Data, 0, unknown1A.Data.Data.Length);
                                }
                                using (var fileStream = fileParameters.OpenWrite())
                                {
                                    fileStream.Write(unknown1B.Data.Data, 0, unknown1B.Data.Data.Length);
                                }
                            }
                        }
                    }
                    */
                    
                    if (tag.Group.Tag == "Lbsp" && tag.Index == 0x3F8C)
                    {
                        var lbsp = Cache.Deserialize<ScenarioLightmapBspData>(stream, tag);
                        var resource = Cache.ResourceCache.GetRenderGeometryApiResourceDefinition(lbsp.Geometry.Resource);
                        lbsp.Geometry.SetResourceBuffers(resource);

                        foreach (var mesh in lbsp.Geometry.Meshes)
                        {
                            if (mesh.Water.Count != 0)
                            {
                                var world = mesh.ResourceVertexBuffers[0];
                                var waterParametersBufferworldWaterBuffer = mesh.ResourceVertexBuffers[7];
                                var worldWaterBuffer = mesh.ResourceVertexBuffers[6];
                                var fildWater = new FileInfo($"Geometry\\riverworld_world_{mesh.VertexBufferIndices[0]}.bin");
                                var fileWorldWater = new FileInfo($"Geometry\\riverworld_world_water_{mesh.VertexBufferIndices[6]}.bin");
                                var fileParameters = new FileInfo($"Geometry\\riverworld_paramters_water_{mesh.VertexBufferIndices[7]}.bin");
                                using (var fileStream = fildWater.OpenWrite())
                                {
                                    fileStream.Write(world.Data.Data, 0, world.Data.Data.Length);
                                }
                                using (var fileStream = fileWorldWater.OpenWrite())
                                {
                                    fileStream.Write(worldWaterBuffer.Data.Data, 0, worldWaterBuffer.Data.Data.Length);
                                }
                                using (var fileStream = fileParameters.OpenWrite())
                                {
                                    fileStream.Write(waterParametersBufferworldWaterBuffer.Data.Data, 0, waterParametersBufferworldWaterBuffer.Data.Data.Length);
                                }
                            }
                        }
                    }
                    
                }
            }
            



            return true;
        }
    }
}

