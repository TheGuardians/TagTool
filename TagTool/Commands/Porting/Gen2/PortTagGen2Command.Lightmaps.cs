using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Tags.Definitions;
using TagTool.Tags;
using TagTool.IO;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Geometry;
using TagTool.Common;
using TagTool.Lighting;
using TagTool.Bitmaps;
using TagTool.Tags.Resources;
using System.IO.Compression;
using TagTool.Serialization;
using System.Numerics;
using static TagTool.Tags.Definitions.Gen2.ScenarioStructureLightmap.StructureLightmapGroupBlock;
using TagTool.Bitmaps.DDS;
using TagTool.Bitmaps.Utils;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        public CachedTag ConvertLightmap(TagTool.Tags.Definitions.Gen2.Scenario gen2Scenario, Scenario newScenario, string scenarioPath, Stream cacheStream, Stream gen2CacheStream)
        {
            var sldtTag = Cache.TagCache.AllocateTag<ScenarioLightmapBspData>($"{scenarioPath}_faux_lightmap");
            var sldt = new ScenarioLightmap();
            sldt.PerPixelLightmapDataReferences = new List<ScenarioLightmap.DataReferenceBlock>();
            sldt.PerVertexLightmapDataReferences = new List<CachedTag>();
            for (var i = 0; i < gen2Scenario.StructureBsps.Count; i++)
            {
                var lbsp = new ScenarioLightmapBspData();
                var lightmapDataName = newScenario.StructureBsps[i].StructureBsp.Name;
                if (gen2Scenario.StructureBsps[i].StructureLightmap != null)
                {
                    ScenarioStructureBsp sbsp = Cache.Deserialize<ScenarioStructureBsp>(cacheStream, newScenario.StructureBsps[i].StructureBsp);
                    var gen2Lightmap = Gen2Cache.Deserialize<TagTool.Tags.Definitions.Gen2.ScenarioStructureLightmap>(gen2CacheStream, gen2Scenario.StructureBsps[i].StructureLightmap);
                    lbsp = ConvertLightmapData(sbsp, gen2Lightmap, cacheStream, gen2CacheStream, i, lightmapDataName);
                    if (lbsp == null)
                        return null;
                    sbsp.Geometry = lbsp.Geometry;
                    Cache.Serialize(cacheStream, newScenario.StructureBsps[i].StructureBsp, sbsp);
                }
                
                lbsp.BspIndex = (short)i;
                var lbspTag = Cache.TagCache.AllocateTag<ScenarioLightmapBspData>(lightmapDataName);
                Cache.Serialize(cacheStream, lbspTag, lbsp);
                sldt.PerPixelLightmapDataReferences.Add(new ScenarioLightmap.DataReferenceBlock() { LightmapBspData = lbspTag });
            }
            Cache.Serialize(cacheStream, sldtTag, sldt);
            return sldtTag;
        }

        public ScenarioLightmapBspData ConvertLightmapData(ScenarioStructureBsp sbsp, TagTool.Tags.Definitions.Gen2.ScenarioStructureLightmap gen2Lightmap, Stream cacheStream, Stream gen2CacheStream, int bsp_index, string lightmapDataName)
        {
            List<int> clusterMeshIndices = new List<int>();
            List<int> instanceMeshIndices = new List<int>();
            foreach (var cluster in sbsp.Clusters)
                clusterMeshIndices.Add(cluster.MeshIndex);
            var sbspResource = Cache.ResourceCache.GetStructureBspTagResources(sbsp.CollisionBspResource);
            foreach(var instance in sbsp.InstancedGeometryInstances)
            instanceMeshIndices.Add(sbspResource.InstancedGeometry[instance.DefinitionIndex].MeshIndex);

            RenderGeometryApiResourceDefinition resourceDef = Cache.ResourceCache.GetRenderGeometryApiResourceDefinition(sbsp.Geometry.Resource);

            ScenarioLightmapBspData lbsp = new ScenarioLightmapBspData();
            lbsp.ClusterStaticPerVertexLightingBuffers = new List<ScenarioLightmapBspData.ClusterStaticPerVertexLighting>();
            lbsp.InstancedGeometry = new List<ScenarioLightmapBspData.InstancedGeometryLighting>();
            lbsp.StaticPerVertexLightingBuffers = new List<ScenarioLightmapBspData.StaticPerVertexLighting>();

            if (gen2Lightmap.LightmapGroups.Count > 1)
                new TagToolError(CommandError.OperationFailed, ">1 lightmap group in lightmap!");
            var lgroup = gen2Lightmap.LightmapGroups[0];

            //set up geometry carried over from sbsp
            lbsp.Geometry = sbsp.Geometry;          

            var gen2bitmap = Gen2Cache.Deserialize<TagTool.Tags.Definitions.Gen2.Bitmap>(gen2CacheStream, lgroup.BitmapGroup);

            Bitmap linearSHBitmap = new Bitmap() { Flags = BitmapRuntimeFlags.UsingTagInteropAndTagResource, Images = new List<Bitmap.Image>(), HardwareTextures = new List<TagResourceReference>()};
            Bitmap intensityBitmap = new Bitmap() { Flags = BitmapRuntimeFlags.UsingTagInteropAndTagResource, Images = new List<Bitmap.Image>(), HardwareTextures = new List<TagResourceReference>() };
            LightmapPacker packer = new LightmapPacker();

            //clusters
            for (var clusterindex = 0; clusterindex < lgroup.Clusters.Count; clusterindex++)
            {
                var clusterrenderdata = lgroup.ClusterRenderInfo[clusterindex];              
                if(clusterrenderdata.BitmapIndex != -1)
                {
                    var image = gen2bitmap.Bitmaps[clusterrenderdata.BitmapIndex];                   

                    byte[] rawBitmapData = Gen2Cache.GetCacheRawData((uint)image.Lod0Pointer, (int)image.Lod0Size);
                    //h2v raw bitmap data is gz compressed
                    if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                    {
                        using (var stream = new MemoryStream(rawBitmapData))
                        using (var resultStream = new MemoryStream())
                        using (var zstream = new GZipStream(stream, CompressionMode.Decompress))
                        {
                            zstream.CopyTo(resultStream);
                            rawBitmapData = resultStream.ToArray();
                        }
                    }

                    if (image.Flags.HasFlag(TagTool.Tags.Definitions.Gen2.Bitmap.BitmapDataBlock.FlagsValue.Swizzled))
                    {
                        rawBitmapData = Swizzle(rawBitmapData, image.Width, image.Height, 1, 1, true);
                    }

                    var palette = lgroup.SectionPalette[clusterrenderdata.PaletteIndex];

                    var coefficients = new List<RealRgbColor[]>();
                    var dominantIntensities = new RealRgbColor[image.Width * image.Height];
                    for (int i = 0; i < 4; i++)
                    {
                        coefficients.Add(new RealRgbColor[image.Width * image.Height]);
                    }

                    //DON'T USE INCIDENT DIRECTION FOR NOW AS IT CAUSES ARTIFACTS
                    /*
                    //get average incident direction across cluster mesh
                    Gen2BSPResourceMesh clustermesh = bspMeshes[bsp_index][clusterMeshIndices[clusterindex]];
                    RealVector3d incident_direction = clustermesh.RawVertices[0].PrimaryLightmapIncidentDirection;
                    foreach (var vert in clustermesh.RawVertices)
                        incident_direction = (vert.PrimaryLightmapIncidentDirection + incident_direction) / 2.0f;
                    */

                    /*
                    //DEBUG DUMP BITMAP
                    using (var rawdataStream = new MemoryStream(rawBitmapData))
                    using (var dumpReader = new EndianReader(rawdataStream))
                    using (var dumpStream = new MemoryStream())
                    using (var dumpWriter = new EndianWriter(dumpStream))
                    {
                        for (var c = 0; c < image.Width * image.Height; c++)
                        {
                            ArgbColor color = new ArgbColor();
                            switch (image.Format)
                            {
                                case TagTool.Tags.Definitions.Gen2.Bitmap.BitmapDataBlock.FormatValue.A8r8g8b8:
                                    var B = dumpReader.ReadByte();
                                    var G = dumpReader.ReadByte();
                                    var R = dumpReader.ReadByte();
                                    var A = dumpReader.ReadByte();
                                    color = new ArgbColor(A,R,G,B);
                                    break;
                                case TagTool.Tags.Definitions.Gen2.Bitmap.BitmapDataBlock.FormatValue.P8:
                                    color = palette.PaletteColors[rawBitmapData[c]];
                                    break;
                            }
                            dumpWriter.Write(color.GetValue());
                        }
                        StoreDDS($"cluster_{clusterindex}.dds",
                            image.Width, image.Height, 1, BitmapType.Texture2D, BitmapFormat.A8R8G8B8, dumpStream.ToArray());
                    }                       
                    */

                    var dataStream = new MemoryStream(rawBitmapData);
                    var dataReader = new EndianReader(dataStream);
                    for (var c = 0; c < image.Width * image.Height; c++)
                    {
                        float[] R = new float[4];
                        float[] G = new float[4];
                        float[] B = new float[4];
                        switch (image.Format)
                        {
                            case TagTool.Tags.Definitions.Gen2.Bitmap.BitmapDataBlock.FormatValue.A8r8g8b8:
                                var temp_B = dataReader.ReadByte();
                                var temp_G = dataReader.ReadByte();
                                var temp_R = dataReader.ReadByte();
                                var temp_A = dataReader.ReadByte();
                                RealRgbColor colorVista = ARGB_to_Real_RGB(new ArgbColor(temp_A, temp_R, temp_G, temp_B));
                                EvaluateDirectionalLightCustom(2, colorVista, new RealVector3d(), R, G, B);
                                dominantIntensities[c] = colorVista;
                                break;
                            case TagTool.Tags.Definitions.Gen2.Bitmap.BitmapDataBlock.FormatValue.P8:
                                RealRgbColor color = ARGB_to_Real_RGB(palette.PaletteColors[rawBitmapData[c]]);
                                EvaluateDirectionalLightCustom(2, color, new RealVector3d(), R, G, B);
                                dominantIntensities[c] = color;
                                break;
                            default:
                                new TagToolError(CommandError.OperationFailed, "Unknown lightmap bitmap format! Aborting!");
                                return null;
                        }
                        var sh = new SphericalHarmonics.SH2Probe(R, G, B);
                        for (int i = 0; i < 4; i++)
                        {
                            coefficients[i][c].Red = sh.R[i];
                            coefficients[i][c].Green = sh.G[i];
                            coefficients[i][c].Blue = sh.B[i];
                        }
                    }

                    packer.AddBitmap(image.Width, image.Height, clusterindex, coefficients);

                    lbsp.ClusterStaticPerVertexLightingBuffers.Add(new ScenarioLightmapBspData.ClusterStaticPerVertexLighting
                    {
                        LightmapBitmapsImageIndex = 0,
                        StaticPerVertexLightingIndex = -1
                    });
                }
                else
                {
                    lbsp.ClusterStaticPerVertexLightingBuffers.Add(new ScenarioLightmapBspData.ClusterStaticPerVertexLighting
                    {
                        LightmapBitmapsImageIndex = -1,
                        StaticPerVertexLightingIndex = -1
                    });
                }
            }


            CachedLightmap convertedLightmap = new CachedLightmap();
            convertedLightmap = packer.Pack();
            if (convertedLightmap == null)
                return null;
            convertedLightmap.MaxLs[4] = 1.0f;

            //fixup lightmap texcoords and write to resources
            for (var clusterindex = 0; clusterindex < lgroup.Clusters.Count; clusterindex++)
            {
                var clusterrenderdata = lgroup.ClusterRenderInfo[clusterindex];
                if (clusterrenderdata.BitmapIndex == -1)
                    continue;
                var image = gen2bitmap.Bitmaps[clusterrenderdata.BitmapIndex];

                Gen2BSPResourceMesh clustermesh = bspMeshes[bsp_index][clusterMeshIndices[clusterindex]];
                List<LightmapRawVertex> lightmapRawVertices = new List<LightmapRawVertex>();
                foreach (var vert in clustermesh.RawVertices)
                {
                    lightmapRawVertices.Add(new LightmapRawVertex
                    {
                        Texcoord = vert.PrimaryLightmapTexcoord,
                    });
                }                
                
                int[] clusterOffset = packer.clusterBitmapOffsets[clusterindex];

                for(var t = 0; t < lightmapRawVertices.Count; t++)
                {
                    var tex = lightmapRawVertices[t];

                    //H2X lightmap texcoords are compressed to 0.5-1 space
                    if(Gen2Cache.Version == CacheVersion.Halo2Xbox)
                        tex.Texcoord = new RealVector2d(tex.Texcoord.I * 2 - 1, tex.Texcoord.J * 2 - 1);
                    int[] originalOffset = new int[] { (int)Math.Round(tex.Texcoord.I * image.Width), (int)Math.Round(tex.Texcoord.J * image.Height) };
                    int[] newOffset = new int[] { clusterOffset[0] + originalOffset[0], clusterOffset[1] + originalOffset[1] };
                    tex.Texcoord = new RealVector2d(newOffset[0] / (float)convertedLightmap.Width, newOffset[1] / (float)convertedLightmap.Height);

                    //WRITE OUT TEXCOORDS ONTO BITMAP TO DEBUG
                    /*
                    int rawDataOffset = (newOffset[1] * convertedLightmap.Width + newOffset[0]) * 4;
                    var rawStream = new MemoryStream(convertedLightmap.LinearSH);
                    var writer = new EndianWriter(rawStream);
                    writer.BaseStream.Position = rawDataOffset;
                    writer.Write(new ArgbColor(255, 255, 0, 0).GetValue());
                    */
                }

                //fixup geometry and resource
                int bufferIndex = BuildPerPixelResourceData(resourceDef, lightmapRawVertices);
                lbsp.Geometry.Meshes[clusterMeshIndices[clusterindex]].VertexBufferIndices[1] = (short)bufferIndex;
            }

            //write blank intensity bitmaps for now
            var bitmapStreamDI = new MemoryStream();
            var vertWriterDI = new EndianWriter(bitmapStreamDI);
            for (var c = 0; c < convertedLightmap.Width * convertedLightmap.Height; c++)
            {
                vertWriterDI.Write(new ArgbColor(255, 255, 255, 255).GetValue());
            }
            for (var c = 0; c < convertedLightmap.Width * convertedLightmap.Height; c++)
            {
                vertWriterDI.Write(CreateDummySHData(new RealRgbColor(1.0f, 1.0f, 1.0f)).GetValue());
            }
            convertedLightmap.Intensity = bitmapStreamDI.ToArray();

            ImportIntoLbsp(convertedLightmap, linearSHBitmap, intensityBitmap, Cache, lbsp, 0);

            //instances
            //instance bucket data
            int[] bucketVertexOffsets = new int[lgroup.GeometryBuckets.Count];
            foreach (var bucket in lgroup.GeometryBuckets)
            {
                bucket.RawVertices = new List<LightmapVertexBufferBucketBlock.LightmapBucketRawVertexBlock>();
                using (var stream = new MemoryStream(Gen2Cache.GetCacheRawData(bucket.GeometryBlockInfo.BlockOffset, (int)bucket.GeometryBlockInfo.BlockSize)))
                using (var reader = new EndianReader(stream))
                using (var writer = new EndianWriter(stream))
                {
                    //fix up pointers within the resource so it deserializes properly
                    foreach (var resourceinstance in bucket.GeometryBlockInfo.TagResources)
                    {
                        stream.Position = resourceinstance.FieldOffset;

                        switch (resourceinstance.Type)
                        {
                            case TagResourceTypeGen2.TagBlock:
                                continue;

                            case TagResourceTypeGen2.VertexBuffer:
                                stream.Position = resourceinstance.ResourceDataOffset;
                                int vertexCount = 0;
                                switch (resourceinstance.SecondaryLocator)
                                {
                                    case 0: //incident direction
                                        vertexCount = resourceinstance.ResourceDataSize / 4;
                                        break;
                                    case 1: //color
                                        vertexCount = resourceinstance.ResourceDataSize / 3;
                                        break;
                                }
                                if (bucket.RawVertices.Count == 0)
                                {
                                    for (var v = 0; v < vertexCount; v++)
                                        bucket.RawVertices.Add(new LightmapVertexBufferBucketBlock.LightmapBucketRawVertexBlock());
                                }
                                var vertStream = new VertexElementStream(stream);
                                for (var i = 0; i < vertexCount; i++)
                                {
                                    stream.Position = 8 + bucket.GeometryBlockInfo.SectionDataSize + 
                                        resourceinstance.ResourceDataOffset + ((resourceinstance.ResourceDataSize / vertexCount) * i);
                                    
                                    switch (resourceinstance.SecondaryLocator)
                                    {
                                        case 0: //incident direction
                                            RealVector3d vertexData = vertStream.ReadDHen3N();
                                            bucket.RawVertices[i].PrimaryLightmapIncidentDirection = new RealVector3d(vertexData.I, vertexData.J, vertexData.K);
                                            break;
                                        case 1: //color
                                            RealQuaternion vertData = vertStream.ReadUByte3N();
                                            bucket.RawVertices[i].PrimaryLightmapColor = new RealRgbColor(vertData.I, vertData.J, vertData.K);
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            for (var instanceindex = 0; instanceindex < lgroup.InstanceRenderInfo.Count; instanceindex++)
            {
                if(instanceMeshIndices[instanceindex] == -1)
                {
                    lbsp.InstancedGeometry.Add(new ScenarioLightmapBspData.InstancedGeometryLighting
                    {
                        LightmapBitmapsImageIndex = -1,
                        StaticPerVertexLightingIndex = -1,
                        InstancedGeometryLightProbesIndex = -1
                    });
                    continue;
                }                  

                Gen2BSPResourceMesh instancemesh = bspMeshes[bsp_index][instanceMeshIndices[instanceindex]];

                var bucket = lgroup.GeometryBuckets[lgroup.InstanceBucketRefs[instanceindex].BucketIndex];
                List<LightmapRawVertex> lightmapRawVertices = new List<LightmapRawVertex>();
                for (var v = 0; v < instancemesh.RawVertices.Count; v++)
                    lightmapRawVertices.Add(new LightmapRawVertex());
                if (lgroup.InstanceRenderInfo[instanceindex].BitmapIndex != -1)
                {
                    var palette = lgroup.SectionPalette[lgroup.InstanceRenderInfo[instanceindex].PaletteIndex];
                    var image = gen2bitmap.Bitmaps[lgroup.InstanceRenderInfo[instanceindex].BitmapIndex];

                    byte[] rawBitmapData = Gen2Cache.GetCacheRawData((uint)image.Lod0Pointer, (int)image.Lod0Size);
                    //h2v raw bitmap data is gz compressed
                    if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                    {
                        using (var stream = new MemoryStream(rawBitmapData))
                        using (var resultStream = new MemoryStream())
                        using (var zstream = new GZipStream(stream, CompressionMode.Decompress))
                        {
                            zstream.CopyTo(resultStream);
                            rawBitmapData = resultStream.ToArray();
                        }
                    }
                    for (int v = 0; v < instancemesh.RawVertices.Count; v++)
                    {
                        lightmapRawVertices[v].Color = ARGB_to_Real_RGB(palette.PaletteColors[SampleLightmapBitmap(rawBitmapData, instancemesh.RawVertices[v].PrimaryLightmapTexcoord, image.Width, image.Height)]);
                        lightmapRawVertices[v].IncidentDirection = instancemesh.RawVertices[v].PrimaryLightmapIncidentDirection;
                    }
                }
                for(var vertIndex = 0; vertIndex < lightmapRawVertices.Count; vertIndex++)
                {
                    if (bucket.Flags.HasFlag(LightmapVertexBufferBucketBlock.FlagsValue.IncidentDirection))
                    {
                        lightmapRawVertices[vertIndex].IncidentDirection = bucket.RawVertices[vertIndex +
                            bucketVertexOffsets[lgroup.InstanceBucketRefs[instanceindex].BucketIndex]].PrimaryLightmapIncidentDirection;
                    }
                    if (bucket.Flags.HasFlag(LightmapVertexBufferBucketBlock.FlagsValue.Color))
                    {
                        lightmapRawVertices[vertIndex].Color = bucket.RawVertices[vertIndex +
                            bucketVertexOffsets[lgroup.InstanceBucketRefs[instanceindex].BucketIndex]].PrimaryLightmapColor;
                    }
                }

                //keep track of used bucket vertices
                bucketVertexOffsets[lgroup.InstanceBucketRefs[instanceindex].BucketIndex] += lightmapRawVertices.Count;

                //fixup geometry and resource
                int bufferIndex = BuildPerVertexResourceData(resourceDef, lightmapRawVertices);
                lbsp.InstancedGeometry.Add(new ScenarioLightmapBspData.InstancedGeometryLighting
                {
                    LightmapBitmapsImageIndex = -1,
                    StaticPerVertexLightingIndex = (short)lbsp.StaticPerVertexLightingBuffers.Count(),
                    InstancedGeometryLightProbesIndex = -1
                });
                lbsp.StaticPerVertexLightingBuffers.Add(new ScenarioLightmapBspData.StaticPerVertexLighting
                {
                    VertexBufferIndex = bufferIndex
                });

            }
            CachedTag SHBitmapTag = Cache.TagCache.AllocateTag<Bitmap>(lgroup.BitmapGroup.Name + "_SH");
            CachedTag intensityBitmapTag = Cache.TagCache.AllocateTag<Bitmap>(lgroup.BitmapGroup.Name + "_intensity");
            Cache.Serialize(cacheStream, SHBitmapTag, linearSHBitmap);
            Cache.Serialize(cacheStream, intensityBitmapTag, intensityBitmap);
            lbsp.LightmapSHCoefficientsBitmap = SHBitmapTag;
            lbsp.LightmapDominantLightDirectionBitmap = intensityBitmapTag;

            lbsp.Geometry.Resource = Cache.ResourceCache.CreateRenderGeometryApiResource(resourceDef);

            return lbsp;
        }

        public void ImportIntoLbsp(CachedLightmap result, Bitmap linearSHBitmap, Bitmap intensityBitmap, GameCacheHaloOnlineBase cache, ScenarioLightmapBspData Lbsp, int imageindex)
        {
            //NOTICE: USING THE DXT5 COMPRESSOR IN THIS WAY RESULTS IN ALL OF THE LAYERS BEING REMOVED BESIDES THE FIRST ONE
            //THIS REALLY SHOULDNT WORK BUT IT DOES, AND SAVES A LOT OF SPACE, SO LEAVING IT IN PLACE FOR NOW

            var dxt5Compressor = new SquishLib.Compressor(SquishLib.SquishFlags.kDxt5 | 
                SquishLib.SquishFlags.kColourIterativeClusterFit | SquishLib.SquishFlags.kSourceBgra, 
                result.LinearSH, result.Width, result.Height);
            var linearSH = new BaseBitmap();
            linearSH.Type = BitmapType.Texture3D;
            linearSH.Data = dxt5Compressor.CompressTexture();
            linearSH.Width = result.Width;
            linearSH.Height = result.Height;
            linearSH.Depth = 8;
            linearSH.UpdateFormat(BitmapFormat.Dxt5);

            //var outPath = "lightmaptest_image_" + imageindex;
            //StoreDDS(outPath, linearSH.Width, linearSH.Height, linearSH.Depth, linearSH.Type, linearSH.Format, result.LinearSH);

            var dxt5Compressor2 = new SquishLib.Compressor(SquishLib.SquishFlags.kDxt5 |
                SquishLib.SquishFlags.kColourIterativeClusterFit | SquishLib.SquishFlags.kSourceBgra,
                result.Intensity, result.Width, result.Height);
            var intensity = new BaseBitmap();
            intensity.Type = BitmapType.Texture3D;
            intensity.Data = dxt5Compressor2.CompressTexture();
            intensity.Width = result.Width;
            intensity.Height = result.Height;
            intensity.Depth = 2;
            intensity.UpdateFormat(BitmapFormat.Dxt5);

            Lbsp.CoefficientsMapScale = new TagTool.Common.LuminanceScale[9];
            for (int i = 0; i < 5; i++)
                Lbsp.CoefficientsMapScale[i] = new TagTool.Common.LuminanceScale() { Scale = result.MaxLs[i] };

            //conversion comes out a bit dark, make it brighter
            Lbsp.CoefficientsMapScale[0].Scale = Lbsp.CoefficientsMapScale[0].Scale * 4.0f;

            ImportBitmap(cache, linearSHBitmap, imageindex, linearSH);
            ImportBitmap(cache, intensityBitmap, imageindex, intensity);
        }

        private static void ImportBitmap(GameCacheHaloOnlineBase cache, Bitmap bitmap, int imageIndex, BaseBitmap bitmapImport)
        {
            BitmapTextureInteropResource resource = BitmapUtils.CreateEmptyBitmapTextureInteropResource();
            resource.Texture.Definition.PrimaryResourceData = new TagData(bitmapImport.Data);
            resource.Texture.Definition.Bitmap = BitmapUtils.CreateBitmapTextureInteropDefinition(bitmapImport);
            var reference = cache.ResourceCache.CreateBitmapResource(resource);
            bitmap.Images.Add(BitmapUtils.CreateBitmapImageFromResourceDefinition(resource.Texture.Definition.Bitmap));
            bitmap.HardwareTextures.Add(reference);
        }

        public RealRgbColor ARGB_to_Real_RGB(ArgbColor color)
        {
            return new RealRgbColor
            {
                Red = color.Red / 255.0f,
                Green = color.Green / 255.0f,
                Blue = color.Blue / 255.0f
            };
        }
        public byte SampleLightmapBitmap(byte[] bitmap, RealVector2d texcoord, int width, int height)
        {
            int xcoord = (int)Math.Round((double)(texcoord.I * width));
            int ycoord = (int)Math.Round((double)(texcoord.J * height));
            int index = ycoord * width + xcoord;
            if (index >= bitmap.Length)
                index = bitmap.Length - 1;
            return bitmap[index];
        }

        public class LightmapRawVertex
        {
            public RealRgbColor Color;
            public RealVector2d Texcoord;
            public RealVector3d IncidentDirection;
        }

        private int BuildPerVertexResourceData(RenderGeometryApiResourceDefinition definition, List<LightmapRawVertex> vertices)
        {
            definition.IndexBuffers.AddressType = CacheAddressType.Definition;
            definition.VertexBuffers.AddressType = CacheAddressType.Definition;

            // Serialize the mesh's vertex buffer
            var vertexBufferStream = new MemoryStream();
            var vertexBufferStart = (int)vertexBufferStream.Position;
            var vertexCount = vertices.Count;

            var vertexStream = VertexStreamFactory.Create(Cache.Version, Cache.Platform, vertexBufferStream);
            foreach (var vert in vertices)
            {
                float[] R = new float[4];
                float[] G = new float[4];
                float[] B = new float[4];
                SphericalHarmonics.EvaluateDirectionalLight(2, vert.Color, vert.IncidentDirection, R, G, B);
                var probe = new SphericalHarmonics.SH2Probe(R, G, B);
                vertexStream.WriteStaticPerVertexData(VmfConversion.CompressStaticPerVertex(vert.Color, probe,
                    Cache.Version, Cache.Platform));
            }              

            StreamUtil.Align(vertexBufferStream, 4);

            vertexBufferStream.Position = vertexBufferStart;

            // Add a definition for it
            int result = definition.VertexBuffers.Count;
            definition.VertexBuffers.Add(new D3DStructure<VertexBufferDefinition>
            {
                Definition = new VertexBufferDefinition
                {
                    Count = vertexCount,
                    Format = VertexBufferFormat.StaticPerVertex,
                    VertexSize = 0x14,
                    Data = new TagData(vertexBufferStream.ToArray()),
                },
            });
            return result;
        }

        private int BuildPerPixelResourceData(RenderGeometryApiResourceDefinition definition, List<LightmapRawVertex> vertices)
        {
            definition.IndexBuffers.AddressType = CacheAddressType.Definition;
            definition.VertexBuffers.AddressType = CacheAddressType.Definition;

            // Serialize the mesh's vertex buffer
            var vertexBufferStream = new MemoryStream();
            var vertexBufferStart = (int)vertexBufferStream.Position;
            var vertexCount = vertices.Count;

            var vertexStream = VertexStreamFactory.Create(Cache.Version, Cache.Platform, vertexBufferStream);
            foreach (var vert in vertices)
                vertexStream.WriteStaticPerPixelData(new StaticPerPixelData
                {                  
                    Texcoord = new RealVector2d
                    {
                        I = vert.Texcoord.I,
                        J = vert.Texcoord.J
                    }
                });

            StreamUtil.Align(vertexBufferStream, 4);

            vertexBufferStream.Position = vertexBufferStart;

            // Add a definition for it
            int result = definition.VertexBuffers.Count;
            definition.VertexBuffers.Add(new D3DStructure<VertexBufferDefinition>
            {
                Definition = new VertexBufferDefinition
                {
                    Count = vertexCount,
                    Format = VertexBufferFormat.StaticPerPixel,
                    VertexSize = 0x8,
                    Data = new TagData(vertexBufferStream.ToArray()),
                },
            });
            return result;
        }

        private ArgbColor CreateDummySHData(RealRgbColor color)
        {
            double Red = (((color.Red + 1) / 2) + 0.5) - color.Red;
            double Green = (((color.Green + 1) / 2) + 0.5) - color.Green;
            double Blue = (((color.Blue + 1) / 2) + 0.5) - color.Blue;
            return new ArgbColor(byte.MaxValue, (byte)(Red * 255), (byte)(Green * 255), (byte)(Blue * 255));
        }
        private static void EvaluateDirectionalLightCustom(int order, RealRgbColor intensity, RealVector3d direction, float[] shRed, float[] shGreen, float[] shBlue)
        {
            float s = (float)Math.PI;

            var shBasis = new float[order * order];
            SphericalHarmonics.EvaluateDirection(direction, order, shBasis);
            SphericalHarmonics.Scale(shRed, order, shBasis, intensity.Red * s);
            SphericalHarmonics.Scale(shGreen, order, shBasis, intensity.Green * s);
            SphericalHarmonics.Scale(shBlue, order, shBasis, intensity.Blue * s);
        }
        private static void StoreDDS(string filePath, int width, int height, int depth, BitmapType type, BitmapFormat format, byte[] data)
        {
            var fi = new FileInfo(filePath);
            fi.Directory.Create();

            var bitmap = new BaseBitmap();
            bitmap.Width = width;
            bitmap.Height = height;
            bitmap.Depth = depth;
            bitmap.Type = type;
            bitmap.Format = format;
            bitmap.UpdateFormat(format);
            bitmap.Data = data;
            var dds = new DDSFile(bitmap);
            using (var writer = new EndianWriter(File.Create(fi.FullName)))
                dds.Write(writer);
        }
    }
}
