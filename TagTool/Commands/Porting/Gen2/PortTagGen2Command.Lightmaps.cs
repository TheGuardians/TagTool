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

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        public CachedTag ConvertLightmap(TagTool.Tags.Definitions.Gen2.Scenario gen2Scenario, Scenario newScenario, string scenarioPath, Stream cacheStream, Stream gen2CacheStream)
        {
            bool use_per_pixel = false;
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
                    //ScenarioStructureBsp sbsp = Cache.Deserialize<ScenarioStructureBsp>(cacheStream, newScenario.StructureBsps[i].StructureBsp);
                    //var gen2Lightmap = Gen2Cache.Deserialize<TagTool.Tags.Definitions.Gen2.ScenarioStructureLightmap>(gen2CacheStream, gen2Scenario.StructureBsps[i].StructureLightmap);
                    //lbsp = ConvertLightmapData(sbsp, gen2Lightmap, cacheStream, gen2CacheStream, i, lightmapDataName, use_per_pixel);
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
            //remove instanced geometry meshes for now for per pixel
            if (use_per_pixel)
            {
                for (var i = lgroup.Clusters.Count; i < lbsp.Geometry.Meshes.Count; i++)
                    lbsp.Geometry.Meshes.RemoveAt(i);
            }

            //set mesh parts to per vertex
            if (!use_per_pixel)
            {
                foreach (var mesh in lbsp.Geometry.Meshes)
                    foreach (var part in mesh.Parts)
                        part.FlagsNew |= Part.PartFlagsNew.PerVertexLightmapPart;
            }

            var gen2bitmap = Gen2Cache.Deserialize<TagTool.Tags.Definitions.Gen2.Bitmap>(gen2CacheStream, lgroup.BitmapGroup);

            Bitmap linearSHBitmap = new Bitmap();
            Bitmap intensityBitmap = new Bitmap();

            if (use_per_pixel)
            {
                linearSHBitmap = ConvertBitmap(gen2bitmap);
                intensityBitmap = linearSHBitmap;
            }

            //clusters
            for (var clusterindex = 0; clusterindex < lgroup.Clusters.Count; clusterindex++)
            {
                var clusterrenderdata = lgroup.ClusterRenderInfo[clusterindex];

                Gen2BSPResourceMesh clustermesh = bspMeshes[bsp_index][clusterindex];
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
                        lbsp.Geometry.Meshes[clusterindex].VertexBufferIndices[1] = (short)bufferIndex;

                        var coefficients = new List<RealRgbColor[]>();
                        for (int i = 0; i < 4; i++)
                            coefficients.Add(new RealRgbColor[rawBitmapData.Length]);

                        RealRgbColor[] colors = new RealRgbColor[rawBitmapData.Length];
                        for (var c = 0; c < rawBitmapData.Length; c++)
                        {
                            colors[c] = ARGB_to_Real_RGB(palette.PaletteColors[rawBitmapData[c]]);
                        }
                        coefficients[0] = colors;

                        //RealRgbColor[] incident_directions = InterpolateIncidentDirections(lightmapRawVertices, image.Width, image.Height).ToArray();
                        RealRgbColor[] incident_directions = new RealRgbColor[rawBitmapData.Length];
                        for (var i = 0; i < rawBitmapData.Length; i++)
                            incident_directions[i] = new RealRgbColor(1.0f, 1.0f, 1.0f);


                        Console.WriteLine($"Converting Cluster Lightmap {clusterindex}...");
                        CachedLightmap convertedLightmap = new CachedLightmap();
                        var lightmapConverter = new H2LightmapConverter();
                        lightmapConverter.ProgressUpdated += progress => Console.Write($"\rProgress: {progress * 100:0.0}%");
                        var result = lightmapConverter.Convert(coefficients, incident_directions, image.Width, image.Height);
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

            }

            //instances
            for (var instanceindex = 0; instanceindex < lgroup.InstanceRenderInfo.Count; instanceindex++)
            {
                lbsp.InstancedGeometry.Add(new ScenarioLightmapBspData.InstancedGeometryLighting
                {
                    LightmapBitmapsImageIndex = -1,
                    StaticPerVertexLightingIndex = -1,
                    InstancedGeometryLightProbesIndex = -1
                });
                /*
                var palette = lgroup.SectionPalette[lgroup.InstanceRenderInfo[instanceindex].PaletteIndex];
                var bucket = lgroup.GeometryBuckets[lgroup.InstanceBucketRefs[instanceindex].BucketIndex];
                var Resource = bucket.GeometryBlockInfo;

                using (var stream = new MemoryStream(Gen2Cache.GetCacheRawData(Resource.BlockOffset, (int)Resource.BlockSize)))
                using (var reader = new EndianReader(stream))
                using (var writer = new EndianWriter(stream))
                {
                    //fix up pointers within the resource so it deserializes properly
                    foreach (var resourceinstance in Resource.TagResources)
                    {
                        stream.Position = resourceinstance.FieldOffset;

                        switch (resourceinstance.Type)
                        {
                            case TagResourceTypeGen2.TagBlock:
                                //count
                                writer.Write(resourceinstance.ResourceDataSize / resourceinstance.SecondaryLocator);
                                //address
                                writer.Write(8 + Resource.SectionDataSize + resourceinstance.ResourceDataOffset);
                                break;

                            case TagResourceTypeGen2.TagData:
                                //size
                                writer.Write(resourceinstance.ResourceDataSize);
                                //address
                                writer.Write(8 + Resource.SectionDataSize + resourceinstance.ResourceDataOffset);
                                break;

                            case TagResourceTypeGen2.VertexBuffer:
                                break;
                        }
                    }

                    stream.Position = 0;

                    var dataContext = new DataSerializationContext(reader);
                    var vertBuffer = Gen2Cache.Deserializer.Deserialize
                        <TagTool.Tags.Definitions.Gen2.ScenarioStructureLightmap.StructureLightmapGroupBlock.
                        LightmapVertexBufferBucketBlock.LightmapVertexBufferBucketCacheDataBlock>(dataContext);
                }
                */
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

        public struct LightmapRawVertex
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
                vertexStream.WriteStaticPerVertexData(
                    VmfConversion.CompressStaticPerVertex(vert.Color, new SphericalHarmonics.SH2Probe(), 
                    Cache.Version, Cache.Platform));

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
