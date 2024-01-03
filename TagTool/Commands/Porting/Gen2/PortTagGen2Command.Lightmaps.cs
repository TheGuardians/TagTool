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
using static TagTool.Lighting.ReachLightmapConverter;
using static TagTool.Tags.Definitions.ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.DeathAndDamageBlock;
using static TagTool.Cache.HaloOnline.ResourceCacheHaloOnline;
using TagTool.Bitmaps.DDS;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        public CachedTag ConvertLightmap(TagTool.Tags.Definitions.Gen2.Scenario gen2Scenario, Scenario newScenario, string scenarioPath, Stream cacheStream, Stream gen2CacheStream)
        {
            bool use_per_pixel = true;
            var sldtTag = Cache.TagCache.AllocateTag<ScenarioLightmapBspData>($"{scenarioPath}_faux_lightmap");
            var sldt = new ScenarioLightmap();
            sldt.PerPixelLightmapDataReferences = new List<ScenarioLightmap.DataReferenceBlock>();
            sldt.PerVertexLightmapDataReferences = new List<CachedTag>();
            for (var i = 0; i < gen2Scenario.StructureBsps.Count; i++)
            {
                var lbsp = new ScenarioLightmapBspData();
                var lightmapDataName = $"{scenarioPath}_faux_lightmap_bsp_data_{i}";
                if (gen2Scenario.StructureBsps[i].StructureLightmap != null)
                {
                    ScenarioStructureBsp sbsp = Cache.Deserialize<ScenarioStructureBsp>(cacheStream, newScenario.StructureBsps[i].StructureBsp);
                    var gen2Lightmap = Gen2Cache.Deserialize<TagTool.Tags.Definitions.Gen2.ScenarioStructureLightmap>(gen2CacheStream, gen2Scenario.StructureBsps[i].StructureLightmap);
                    lbsp = ConvertLightmapData(sbsp, gen2Lightmap, cacheStream, gen2CacheStream, i, lightmapDataName, use_per_pixel);
                    sbsp.Geometry = lbsp.Geometry;
                    Cache.Serialize(cacheStream, newScenario.StructureBsps[i].StructureBsp, sbsp);
                }
                
                lbsp.BspIndex = (short)i;
                var lbspTag = Cache.TagCache.AllocateTag<ScenarioLightmapBspData>(lightmapDataName);
                Cache.Serialize(cacheStream, lbspTag, lbsp);
                if (use_per_pixel)
                    sldt.PerPixelLightmapDataReferences.Add(new ScenarioLightmap.DataReferenceBlock() { LightmapBspData = lbspTag });
                else
                    sldt.PerVertexLightmapDataReferences.Add(lbspTag);
            }
            Cache.Serialize(cacheStream, sldtTag, sldt);
            return sldtTag;
        }

        public ScenarioLightmapBspData ConvertLightmapData(ScenarioStructureBsp sbsp, TagTool.Tags.Definitions.Gen2.ScenarioStructureLightmap gen2Lightmap, Stream cacheStream, Stream gen2CacheStream, int bsp_index, string lightmapDataName, bool use_per_pixel)
        {

            List<int> clusterMeshIndices = new List<int>();
            List<int> instanceMeshIndices = new List<int>();
            foreach (var cluster in sbsp.Clusters)
                clusterMeshIndices.Add(cluster.MeshIndex);
            var sbspResource = Cache.ResourceCache.GetStructureBspTagResources(sbsp.CollisionBspResource);
            foreach(var instance in sbsp.InstancedGeometryInstances)
            instanceMeshIndices.Add(sbspResource.InstancedGeometry[instance.DefinitionIndex].MeshIndex);

            //clean out render geometry resource for re-use, keeping index buffers
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
            
            //set mesh parts to per vertex
            if (!use_per_pixel)
            {
                foreach (var mesh in lbsp.Geometry.Meshes)
                    mesh.Flags |= MeshFlags.MeshHasPerInstanceLighting;
            }

            var gen2bitmap = Gen2Cache.Deserialize<TagTool.Tags.Definitions.Gen2.Bitmap>(gen2CacheStream, lgroup.BitmapGroup);

            Bitmap linearSHBitmap = new Bitmap();
            Bitmap intensityBitmap = new Bitmap();

            if (use_per_pixel)
            {
                linearSHBitmap = ConvertBitmap(gen2bitmap);
                intensityBitmap = ConvertBitmap(gen2bitmap);
            }

            //clusters
            for (var clusterindex = 0; clusterindex < lgroup.Clusters.Count; clusterindex++)
            {
                var clusterrenderdata = lgroup.ClusterRenderInfo[clusterindex];

                Gen2BSPResourceMesh clustermesh = bspMeshes[bsp_index][clusterMeshIndices[clusterindex]];
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

                    List<LightmapRawVertex> lightmapRawVertices = new List<LightmapRawVertex>();
                    foreach(var vert in clustermesh.RawVertices)
                    {
                        lightmapRawVertices.Add(new LightmapRawVertex
                        {
                            Color = ARGB_to_Real_RGB(palette.PaletteColors[SampleLightmapBitmap(rawBitmapData, vert.PrimaryLightmapTexcoord, image.Width, image.Height)]),
                            Texcoord = vert.PrimaryLightmapTexcoord,
                            IncidentDirection = vert.PrimaryLightmapIncidentDirection
                        });
                    }

                    //static per pixel lighting
                    if (use_per_pixel)
                    {
                        //fixup geometry and resource
                        int bufferIndex = BuildPerPixelResourceData(resourceDef, lightmapRawVertices);
                        lbsp.Geometry.Meshes[clusterMeshIndices[clusterindex]].VertexBufferIndices[1] = (short)bufferIndex;

                        var coefficients = new List<RealRgbColor[]>();
                        var dominantIntensities = new RealRgbColor[rawBitmapData.Length];
                        for (int i = 0; i < 4; i++)
                        {
                            coefficients.Add(new RealRgbColor[rawBitmapData.Length]);
                        }
                            
                        for (var c = 0; c < rawBitmapData.Length; c++)
                        {
                            float[] R = new float[4];
                            float[] G = new float[4];
                            float[] B = new float[4];
                            SphericalHarmonics.EvaluateDirectionalLight(2, ARGB_to_Real_RGB(palette.PaletteColors[rawBitmapData[c]]), new RealVector3d(1.0f, 1.0f, 1.0f), R, G, B);
                            var sh = new SphericalHarmonics.SH2Probe(R, G, B);
                            for (int i = 0; i < 4; i++)
                            {
                                coefficients[i][c].Red = sh.R[i];
                                coefficients[i][c].Green = sh.G[i];
                                coefficients[i][c].Blue = sh.B[i];
                            }

                            dominantIntensities[c] = new RealRgbColor(1.0f,1.0f,1.0f);
                        }

                        Console.WriteLine($"Converting Cluster Lightmap {clusterindex}...");
                        CachedLightmap convertedLightmap = new CachedLightmap();
                        var lightmapConverter = new H2LightmapConverter();
                        lightmapConverter.ProgressUpdated += progress => Console.Write($"\rProgress: {progress * 100:0.0}%");
                        var result = lightmapConverter.Convert(coefficients, dominantIntensities, image.Width, image.Height);
                        Console.WriteLine();
                        if (result != null)
                        {
                            convertedLightmap.Height = result.Height;
                            convertedLightmap.Width = result.Width;
                            convertedLightmap.MaxLs = result.MaxLs;
                            convertedLightmap.LinearSH = result.LinearSH;
                            convertedLightmap.Intensity = result.Intensity;
                        }
                        else
                        {
                            convertedLightmap = null;
                        }

                        ImportIntoLbsp(convertedLightmap, linearSHBitmap, intensityBitmap, Cache, lbsp, clusterindex);
                        lbsp.ClusterStaticPerVertexLightingBuffers.Add(new ScenarioLightmapBspData.ClusterStaticPerVertexLighting
                        {
                            LightmapBitmapsImageIndex = (short)clusterindex,
                            StaticPerVertexLightingIndex = -1
                        });
                    }
                    //static per vertex lighting
                    else
                    {
                        //fixup geometry and resource
                        int bufferIndex = BuildPerVertexResourceData(resourceDef, lightmapRawVertices);
                        lbsp.ClusterStaticPerVertexLightingBuffers.Add(new ScenarioLightmapBspData.ClusterStaticPerVertexLighting
                        {
                            LightmapBitmapsImageIndex = -1,
                            StaticPerVertexLightingIndex = (short)lbsp.StaticPerVertexLightingBuffers.Count()
                        });
                        lbsp.StaticPerVertexLightingBuffers.Add(new ScenarioLightmapBspData.StaticPerVertexLighting
                        {
                            VertexBufferIndex = bufferIndex
                        });
                    }
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
            if (use_per_pixel)
            {
                CachedTag SHBitmapTag = Cache.TagCache.AllocateTag<Bitmap>(lgroup.BitmapGroup.Name + "_SH");
                CachedTag intensityBitmapTag = Cache.TagCache.AllocateTag<Bitmap>(lgroup.BitmapGroup.Name + "_intensity");
                Cache.Serialize(cacheStream, SHBitmapTag, linearSHBitmap);
                Cache.Serialize(cacheStream, intensityBitmapTag, intensityBitmap);
                lbsp.LightmapSHCoefficientsBitmap = SHBitmapTag;
                lbsp.LightmapDominantLightDirectionBitmap = intensityBitmapTag;
            }

            lbsp.Geometry.Resource = Cache.ResourceCache.CreateRenderGeometryApiResource(resourceDef);

            return lbsp;
        }

        public class ConversionResult
        {
            public byte[] LinearSH;
            public byte[] Intensity;
            public float[] MaxLs;
            public int Width;
            public int Height;
        }

        public class H2LightmapConverter
        {
            public Action<float> ProgressUpdated;
            public int ProgressUpdateIntervalMS = 1000;
            public int MaxDegreeOfParallelism = -1;

            public ConversionResult Convert(List<RealRgbColor[]> coefficients, RealRgbColor[] dominantIntensities, int width, int height)
            {
                var compressor = new LightmapCompressor();
                compressor.MaxDegreeOfParallelism = MaxDegreeOfParallelism;
                compressor.ProgressUpdateInterval = ProgressUpdateIntervalMS;
                compressor.ProgressUpdated += ProgressUpdated;

                var tasks = compressor.Tasks;
                for (int i = 0; i < 4; i++)
                    compressor.AddTask(new LightmapCompressionTask(i, coefficients[i], width, height));
                compressor.AddTask(new LightmapCompressionTask(4, dominantIntensities, width, height));
                compressor.Run();
                compressor.ProgressUpdated -= ProgressUpdated;

                var result = new ConversionResult();
                result.MaxLs = tasks.Select(x => x.MaxL).ToArray();
                result.Width = width;
                result.Height = height;

                using (var ms = new MemoryStream())
                {
                    for (int i = 0; i < 4; i++)
                    {
                        ms.Write(tasks[i].Dxt0, 0, tasks[i].Dxt0.Length);
                        ms.Write(tasks[i].Dxt1, 0, tasks[i].Dxt1.Length);
                    }
                    result.LinearSH = ms.ToArray();
                }

                using (var ms = new MemoryStream())
                {
                    var task = tasks[4];
                    ms.Write(task.Dxt0, 0, task.Dxt0.Length);
                    ms.Write(task.Dxt1, 0, task.Dxt1.Length);
                    result.Intensity = ms.ToArray();
                }

                return result;
            }
        }

        public void ImportIntoLbsp(CachedLightmap result, Bitmap linearSHBitmap, Bitmap intensityBitmap, GameCacheHaloOnlineBase cache, ScenarioLightmapBspData Lbsp, int imageindex)
        {
            var linearSH = new BaseBitmap();
            linearSH.Type = BitmapType.Texture3D;
            linearSH.Data = result.LinearSH;
            linearSH.Width = result.Width;
            linearSH.Height = result.Height;
            linearSH.Depth = 8;
            linearSH.UpdateFormat(BitmapFormat.Dxt5);

            var intensity = new BaseBitmap();
            intensity.Type = BitmapType.Texture3D;
            intensity.Data = result.Intensity;
            intensity.Width = result.Width;
            intensity.Height = result.Height;
            intensity.Depth = 2;
            intensity.UpdateFormat(BitmapFormat.Dxt5);

            Lbsp.CoefficientsMapScale = new TagTool.Common.LuminanceScale[9];
            for (int i = 0; i < 5; i++)
                Lbsp.CoefficientsMapScale[i] = new TagTool.Common.LuminanceScale() { Scale = result.MaxLs[i] };

            ImportBitmap(cache, linearSHBitmap, imageindex, linearSH);
            ImportBitmap(cache, intensityBitmap, imageindex, intensity);
        }

        private static void ImportBitmap(GameCacheHaloOnlineBase cache, Bitmap bitmap, int imageIndex, BaseBitmap bitmapImport)
        {
            BitmapTextureInteropResource resource = BitmapUtils.CreateEmptyBitmapTextureInteropResource();
            resource.Texture.Definition.PrimaryResourceData = new TagData(bitmapImport.Data);
            resource.Texture.Definition.Bitmap = BitmapUtils.CreateBitmapTextureInteropDefinition(bitmapImport);
            var reference = cache.ResourceCache.CreateBitmapResource(resource);
            bitmap.Images[imageIndex] = BitmapUtils.CreateBitmapImageFromResourceDefinition(resource.Texture.Definition.Bitmap);
            bitmap.HardwareTextures[imageIndex] = reference;
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
            int xcoord = (int)Math.Floor((double)(texcoord.I * width));
            int ycoord = (int)Math.Floor((double)(texcoord.J * height));
            int index = ycoord * width + xcoord;
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
                    Texcoord = vert.Texcoord
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
    }
}
