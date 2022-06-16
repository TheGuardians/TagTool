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
using static TagTool.Commands.Porting.Gen2.Gen2BspGeometryConverter;
using TagTool.Geometry;
using TagTool.Common;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command
    {
        public CachedTag ConvertLightmap(TagTool.Tags.Definitions.Gen2.Scenario gen2Scenario, string scenarioPath, Stream cacheStream, Stream gen2CacheStream)
        {
            var sldtTag = Cache.TagCache.AllocateTag<ScenarioLightmapBspData>($"{scenarioPath}_faux_lightmap");
            var sldt = new ScenarioLightmap();
            sldt.LightmapDataReferences = new List<ScenarioLightmap.DataReferenceBlock>();
            for (var i = 0; i < gen2Scenario.StructureBsps.Count; i++)
            {
                var lbsp = new ScenarioLightmapBspData();
                if(gen2Scenario.StructureBsps[i].StructureLightmap != null)
                {
                    //var gen2Lightmap = Gen2Cache.Deserialize<TagTool.Tags.Definitions.Gen2.ScenarioStructureLightmap>(gen2CacheStream, gen2Scenario.StructureBsps[i].StructureLightmap);
                    //ConvertLightmapData(gen2Lightmap, gen2CacheStream, i);
                    lbsp = new ScenarioLightmapBspData();
                }
                
                lbsp.BspIndex = (short)i;
                var lbspTag = Cache.TagCache.AllocateTag<ScenarioLightmapBspData>($"{scenarioPath}_faux_lightmap_bsp_data_{i}");
                Cache.Serialize(cacheStream, lbspTag, lbsp);
                sldt.LightmapDataReferences.Add(new ScenarioLightmap.DataReferenceBlock() { LightmapBspData = lbspTag });
            }
            Cache.Serialize(cacheStream, sldtTag, sldt);
            return sldtTag;
        }

        public ScenarioLightmapBspData ConvertLightmapData(TagTool.Tags.Definitions.Gen2.ScenarioStructureLightmap gen2Lightmap, Stream gen2CacheStream, int bsp_index)
        {
            ScenarioLightmapBspData lbsp = new ScenarioLightmapBspData();

            if (gen2Lightmap.LightmapGroups.Count > 1)
                new TagToolError(CommandError.OperationFailed, ">1 lightmap group in lightmap!");

            var lgroup = gen2Lightmap.LightmapGroups[0];
            var gen2bitmap = Gen2Cache.Deserialize<TagTool.Tags.Definitions.Gen2.Bitmap>(gen2CacheStream, lgroup.BitmapGroup);

            for(var clusterindex = 0; clusterindex < lgroup.Clusters.Count; clusterindex++)
            {
                var clusterrenderdata = lgroup.ClusterRenderInfo[clusterindex];

                Gen2BSPResourceMesh clustermesh = bspMeshes[bsp_index][clusterindex];
                if(clusterrenderdata.BitmapIndex != -1)
                {
                    var image = gen2bitmap.Bitmaps[clusterrenderdata.BitmapIndex];
                    byte[] rawBitmapData = Gen2Cache.GetCacheRawData((uint)image.Lod0Pointer, (int)image.Lod0Size);
                    var palette = lgroup.SectionPalette[clusterrenderdata.PaletteIndex];
                    List<LightmapRawVertex> lightmapRawVertices = new List<LightmapRawVertex>();
                    foreach(var vert in clustermesh.RawVertices)
                    {
                        lightmapRawVertices.Add(new LightmapRawVertex
                        {
                            Texcoord = vert.PrimaryLightmapTexcoord,
                            IncidentDirection = vert.SecondaryLightmapIncidentDirection
                        });
                    }

                    RealRgbColor[] colors = new RealRgbColor[rawBitmapData.Length];
                    for (var c = 0; c < rawBitmapData.Length; c++)
                    {
                        colors[c] = ARGB_to_Real_RGB(palette.PaletteColors[rawBitmapData[c]]);
                    }

                    RealRgbColor[] incident_directions = InterpolateIncidentDirections(lightmapRawVertices, image.Width, image.Height).ToArray();

                }

            }

            return lbsp;
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
        public byte SampleLightmapBitmap(byte[] bitmap, float U, float V, int width, int height)
        {
            int xcoord = (int)Math.Floor((double)(U * width));
            int ycoord = (int)Math.Floor((double)(V * height));
            int index = ycoord * width + xcoord;
            return bitmap[index];
        }

        public struct LightmapRawVertex
        {
            public RealRgbColor Color;
            public RealPoint2d Texcoord;
            public RealVector3d IncidentDirection;
        }
        public List<RealRgbColor> InterpolateIncidentDirections(List<LightmapRawVertex> vertices, int width, int height)
        {
            List<RealRgbColor> incidentDirs = new List<RealRgbColor>();

            double[,] valuearray_I = new double[width, height];
            double[,] valuearray_J = new double[width, height];
            double[,] valuearray_K = new double[width, height];
            double[] xcoords = new double[vertices.Count];
            double[] ycoords = new double[vertices.Count];
            for(var v = 0; v < vertices.Count; v++)
            {
                var vert = vertices[v];
                int xcoord = (int)Math.Floor((double)(vert.Texcoord.X * width));
                int ycoord = (int)Math.Floor((double)(vert.Texcoord.Y * height));
                valuearray_I[xcoord, ycoord] = vert.IncidentDirection.I;
                valuearray_J[xcoord, ycoord] = vert.IncidentDirection.J;
                valuearray_K[xcoord, ycoord] = vert.IncidentDirection.K;
                xcoords[v] = xcoord;
                ycoords[v] = ycoord;
            }

            for (var i = 0; i < width * height; i++)
            {
                int xcoord = i % width;
                int ycoord = (int)Math.Floor((double)(i / width));
                RealRgbColor currentDir = new RealRgbColor
                {
                    Red = (float)BilinearInterpolation(xcoords, ycoords, valuearray_I, xcoord, ycoord),
                    Blue = (float)BilinearInterpolation(xcoords, ycoords, valuearray_J, xcoord, ycoord),
                    Green = (float)BilinearInterpolation(xcoords, ycoords, valuearray_K, xcoord, ycoord)
                };
                incidentDirs.Add(currentDir);
            }
            return incidentDirs;
        }

        public static double BilinearInterpolation(double[] x, double[] y, double[,] z, double xval, double yval)
        {
            //calculates single point bilinear interpolation
            double zval = 0.0;
            for (int i = 0; i < x.Length - 1; i++)
            {
                for (int j = 0; j < y.Length - 1; j++)
                {
                    if (xval >= x[i] && xval < x[i + 1] && yval >= y[j] && yval < y[j + 1])
                    {
                        zval = z[i, j] * (x[i + 1] - xval) * (y[j + 1] - yval) / (x[i + 1] - x[i]) / (y[j + 1] - y[j]) +
                               z[i + 1, j] * (xval - x[i]) * (y[j + 1] - yval) / (x[i + 1] - x[i]) / (y[j + 1] - y[j]) +
                               z[i, j + 1] * (x[i + 1] - xval) * (yval - y[j]) / (x[i + 1] - x[i]) / (y[j + 1] - y[j]) +
                               z[i + 1, j + 1] * (xval - x[i]) * (yval - y[j]) / (x[i + 1] - x[i]) / (y[j + 1] - y[j]);
                    }
                }
            }
            return zval;
        }
    }
}
