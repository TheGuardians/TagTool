using System;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Tags.Definitions;

namespace TagTool.Lighting
{
    static class VmfConversion
    {
        public static void ConvertStaticPerVertexBuffers(ScenarioLightmapBspData Lbsp, TagTool.Tags.Resources.RenderGeometryApiResourceDefinition renderGeometryResource, CacheVersion targetVersion, CachePlatform targetPlatform = CachePlatform.All)
        {
            foreach (var clusterLighting in Lbsp.ClusterStaticPerVertexLightingBuffers)
            {
                clusterLighting.StaticPerVertexLightingIndex = -1;

                if (clusterLighting.StaticPerVertexLightingIndex != -1)
                {
                    var element = Lbsp.StaticPerVertexLightingBuffers[clusterLighting.StaticPerVertexLightingIndex];

                     if (element.VertexBufferIndexReach == -1)
                     {
                         clusterLighting.StaticPerVertexLightingIndex = -1;
                     }
                     else
                     {
                        if (clusterLighting.FirstVertexIndex > 0)
                        {
                            // TODO: figure out how to make the buffer compatible with the geometry index buffer
                            clusterLighting.StaticPerVertexLightingIndex = -1;
                           
                            // var vertexBuffer = renderGeometryResource.VertexBuffers[element.VertexBufferIndexReach].Definition;
                            // var ms = new MemoryStream();
                            // StreamUtil.Fill(ms, 0xCD, clusterLighting.FirstVertexIndex * 6);
                            // ms.Write(vertexBuffer.Data.Data, 0, vertexBuffer.Data.Data.Length);
                            // vertexBuffer.Data = new Tags.TagData(ms.ToArray());
                            // vertexBuffer.Count += clusterLighting.FirstVertexIndex;
                        }
                     }
                }
            }

            foreach (var instanceLighting in Lbsp.InstancedGeometry)
            {
                if (instanceLighting.StaticPerVertexLightingIndex != -1)
                {
                    var element = Lbsp.StaticPerVertexLightingBuffers[instanceLighting.StaticPerVertexLightingIndex];
                    if (element.VertexBufferIndexReach == -1)
                    {
                        instanceLighting.StaticPerVertexLightingIndex = -1;
                    }
                }
            }

            for (int i = 0; i < Lbsp.StaticPerVertexLightingBuffers.Count; i++)
            {
                if (Lbsp.StaticPerVertexLightingBuffers[i].VertexBufferIndexReach == -1)
                    continue;

                var element = Lbsp.StaticPerVertexLightingBuffers[i];
                var vertexBuffer = renderGeometryResource.VertexBuffers[element.VertexBufferIndexReach].Definition;
                float hdrScalar = Half.ToHalf(element.HDRScalar);
                var data = ConvertStaticPerVertexData(vertexBuffer.Data.Data, hdrScalar, out int vertexCount, targetVersion, targetPlatform);
                vertexBuffer.Data = new Tags.TagData(data);
                vertexBuffer.Count = vertexCount;
            }
        }

        public static byte[] ConvertStaticPerVertexData(byte[] data, float hdrScalar, out int vertexCount, CacheVersion targetVersion, CachePlatform targetPlatform)
        {
            var outputStream = new MemoryStream();
            var vertexStream = VertexStreamFactory.Create(targetVersion, targetPlatform, outputStream);

            if (data.Length % 6 != 0)
            {
                if (!data.Skip(data.Length / 6 * 6).Take(data.Length % 6).All(x => x == 0xCD))
                    throw new InvalidDataException();
            }

            vertexCount = data.Length / 6;
            for (int i = 0; i < vertexCount; i++)
            {
                int offset = i * 6;
                int directionIndex = data[offset + 0] & 0x7f;
                if (directionIndex >= tessellatedIcosahedron.Length)
                    throw new InvalidDataException();

                float dc = (((data[offset + 0] >> 7) & 1) | ((data[offset + 1] & 1) << 1)) / 3.0f;
                float r1 = (float)Math.Pow(((data[offset + 1] >> 1) & 31) / 31.0f, 2) * hdrScalar;
                float g1 = (float)Math.Pow((((data[offset + 1] >> 6) & 3) | ((data[offset + 2] & 7) << 2)) / 31.0f, 2) * hdrScalar;
                float b1 = (float)Math.Pow(((data[offset + 2] >> 3) & 31) / 31.0f, 2) * hdrScalar;
                float r0 = (float)Math.Pow(data[offset + 3] / 255.0f, 2) * hdrScalar;
                float g0 = (float)Math.Pow(data[offset + 4] / 255.0f, 2) * hdrScalar;
                float b0 = (float)Math.Pow(data[offset + 5] / 255.0f, 2) * hdrScalar;

                var direction = tessellatedIcosahedron[directionIndex];

                var vmfProbe = new DualVmfBasis();
                vmfProbe.Direct = new VmfLight()
                {
                    Direction = direction,
                    Magnitude = dc,
                    Color = new RealRgbColor(r0, g0, b0),
                    Scale = 1.0f
                };
                vmfProbe.Indirect = new VmfLight()
                {
                    Direction = new RealVector3d(0, 0, 1),
                    Magnitude = dc,
                    Color = new RealRgbColor(r1, g1, b1),
                    Scale = 0.0f
                };

                var qudraticShProbe = ConvertVmfLightprobe(vmfProbe);
                SphericalHarmonics.QudraticToLinearAndIntensity(qudraticShProbe, out SphericalHarmonics.SH2Probe linearShProbe, out RealRgbColor intensity);
                var spv = CompressStaticPerVertex(intensity, linearShProbe, targetVersion, targetPlatform);
                vertexStream.WriteStaticPerVertexData(spv);
            }

            return outputStream.ToArray();
        }

        public static HalfRGBLightProbe ConvertHalfLightprobe(DualVmfLightProbe halfVmf)
        {
            var vmf = new DualVmfBasis(halfVmf.VmfTerms);
            var sh = ConvertVmfLightprobe(vmf);
            var halfsh = new HalfRGBLightProbe();

            var dominantDirection = SphericalHarmonics.GetDominantLightDirection(sh.R, sh.G, sh.B);
            SphericalHarmonics.QudraticToLinearAndIntensity(sh, out SphericalHarmonics.SH2Probe linearShProbe, out RealRgbColor intensity);

            halfsh.DominantLightDirection[0] = Half.GetBits((Half)dominantDirection.I);
            halfsh.DominantLightDirection[1] = Half.GetBits((Half)dominantDirection.J);
            halfsh.DominantLightDirection[2] = Half.GetBits((Half)dominantDirection.K);
            halfsh.DominantLightIntensity[0] = Half.GetBits((Half)intensity.Red);
            halfsh.DominantLightIntensity[1] = Half.GetBits((Half)intensity.Green);
            halfsh.DominantLightIntensity[2] = Half.GetBits((Half)intensity.Blue);
            
            for (int i = 0; i < 9; i++)
            {
                halfsh.SHRed[i] = Half.GetBits((Half)sh.R[i]);
                halfsh.SHGreen[i] = Half.GetBits((Half)sh.G[i]);
                halfsh.SHBlue[i] = Half.GetBits((Half)sh.B[i]);
            }

            return halfsh;
        }

        public static SphericalHarmonics.SH3Probe ConvertVmfLightprobe(DualVmfBasis vmf)
        {
            var directLinear = new[] { vmf.Direct.Magnitude, -vmf.Direct.Direction.J, vmf.Direct.Direction.K, -vmf.Direct.Direction.I };
            var indirectLinear = new[] { vmf.Indirect.Magnitude, -vmf.Indirect.Direction.J, vmf.Indirect.Direction.K, -vmf.Indirect.Direction.I };

            var result = new SphericalHarmonics.SH3Probe();
            for (int i = 0; i < 4; i++)
            {
                result.R[i] = (directLinear[i] * vmf.Direct.Color.Red) + (indirectLinear[i] * vmf.Indirect.Color.Red);
                result.G[i] = (directLinear[i] * vmf.Direct.Color.Green) + (indirectLinear[i] * vmf.Indirect.Color.Green);
                result.B[i] = (directLinear[i] * vmf.Direct.Color.Blue) + (indirectLinear[i] * vmf.Indirect.Color.Blue);
            }
            return result;
        }

        public static StaticPerVertexData CompressStaticPerVertex(RealRgbColor intensity, SphericalHarmonics.SH2Probe linear, CacheVersion version, CachePlatform platform = CachePlatform.All)
        {
            var data = new StaticPerVertexData();
            data.Color1 = ColorConversion.EncodePixel32(ColorConversion.ToRgbe(intensity, true), version, platform);
            data.Color2 = ColorConversion.EncodePixel32(ColorConversion.ToRgbe(new RealRgbColor(linear.R[0], linear.G[0], linear.B[0]), true), version, platform);
            data.Color3 = ColorConversion.EncodePixel32(ColorConversion.ToRgbe(new RealRgbColor(linear.R[1], linear.G[1], linear.B[1]), true), version, platform);
            data.Color4 = ColorConversion.EncodePixel32(ColorConversion.ToRgbe(new RealRgbColor(linear.R[2], linear.G[2], linear.B[2]), true), version, platform);
            data.Color5 = ColorConversion.EncodePixel32(ColorConversion.ToRgbe(new RealRgbColor(linear.R[3], linear.G[3], linear.B[3]), true), version, platform);
            return data;
        }

        public static RealVector3d GetStaticDirection(int index)
        {
            return tessellatedIcosahedron[index];
        }

        static RealVector3d[] tessellatedIcosahedron = new[]
        {
            new RealVector3d(0.0f, 0.0f, 1.0f),
            new RealVector3d(0.36070001f, 0.0f, 0.93269998f),
            new RealVector3d(0.1115f, 0.34310001f, 0.93269998f),
            new RealVector3d(0.67290002f, 0.0f, 0.73970002f),
            new RealVector3d(0.4795f, 0.3484f, 0.80540001f),
            new RealVector3d(0.2079f, 0.63990003f, 0.73970002f),
            new RealVector3d(0.8944f, 0.0f, 0.4472f),
            new RealVector3d(0.78439999f, 0.34310001f, 0.51679999f),
            new RealVector3d(0.56870002f, 0.63990003f, 0.51679999f),
            new RealVector3d(0.2764f, 0.85070002f, 0.4472f),
            new RealVector3d(-0.29179999f, 0.212f, 0.93269998f),
            new RealVector3d(-0.1832f, 0.56370002f, 0.80540001f),
            new RealVector3d(-0.54439998f, 0.3955f, 0.73970002f),
            new RealVector3d(-0.083899997f, 0.852f, 0.51679999f),
            new RealVector3d(-0.43290001f, 0.73860002f, 0.51679999f),
            new RealVector3d(-0.72359997f, 0.52569997f, 0.4472f),
            new RealVector3d(-0.29179999f, -0.212f, 0.93269998f),
            new RealVector3d(-0.5927f, -0.0f, 0.80540001f),
            new RealVector3d(-0.54439998f, -0.3955f, 0.73970002f),
            new RealVector3d(-0.8362f, 0.18350001f, 0.51679999f),
            new RealVector3d(-0.8362f, -0.18350001f, 0.51679999f),
            new RealVector3d(-0.72359997f, -0.52569997f, 0.4472f),
            new RealVector3d(0.1115f, -0.34310001f, 0.93269998f),
            new RealVector3d(-0.1832f, -0.56370002f, 0.80540001f),
            new RealVector3d(0.2079f, -0.63990003f, 0.73970002f),
            new RealVector3d(-0.43290001f, -0.73860002f, 0.51679999f),
            new RealVector3d(-0.083899997f, -0.852f, 0.51679999f),
            new RealVector3d(0.2764f, -0.85070002f, 0.4472f),
            new RealVector3d(0.4795f, -0.3484f, 0.80540001f),
            new RealVector3d(0.56870002f, -0.63990003f, 0.51679999f),
            new RealVector3d(0.78439999f, -0.34310001f, 0.51679999f),
            new RealVector3d(0.96469998f, -0.212f, 0.1561f),
            new RealVector3d(0.96469998f, 0.212f, 0.1561f),
            new RealVector3d(0.90509999f, -0.3955f, -0.1561f),
            new RealVector3d(0.98549998f, 0.0f, -0.1699f),
            new RealVector3d(0.90509999f, 0.3955f, -0.1561f),
            new RealVector3d(0.72359997f, -0.52569997f, -0.4472f),
            new RealVector3d(0.8362f, -0.18350001f, -0.51679999f),
            new RealVector3d(0.8362f, 0.18350001f, -0.51679999f),
            new RealVector3d(0.72359997f, 0.52569997f, -0.4472f),
            new RealVector3d(0.4998f, 0.852f, 0.1561f),
            new RealVector3d(0.096500002f, 0.98299998f, 0.1561f),
            new RealVector3d(0.65579998f, 0.73860002f, -0.1561f),
            new RealVector3d(0.30450001f, 0.93720001f, -0.1699f),
            new RealVector3d(-0.096500002f, 0.98299998f, -0.1561f),
            new RealVector3d(0.43290001f, 0.73860002f, -0.51679999f),
            new RealVector3d(0.083899997f, 0.852f, -0.51679999f),
            new RealVector3d(-0.2764f, 0.85070002f, -0.4472f),
            new RealVector3d(-0.65579998f, 0.73860002f, 0.1561f),
            new RealVector3d(-0.90509999f, 0.3955f, 0.1561f),
            new RealVector3d(-0.4998f, 0.852f, -0.1561f),
            new RealVector3d(-0.79729998f, 0.57920003f, -0.1699f),
            new RealVector3d(-0.96469998f, 0.212f, -0.1561f),
            new RealVector3d(-0.56870002f, 0.63990003f, -0.51679999f),
            new RealVector3d(-0.78439999f, 0.34310001f, -0.51679999f),
            new RealVector3d(-0.8944f, -0.0f, -0.4472f),
            new RealVector3d(-0.90509999f, -0.3955f, 0.1561f),
            new RealVector3d(-0.65579998f, -0.73860002f, 0.1561f),
            new RealVector3d(-0.96469998f, -0.212f, -0.1561f),
            new RealVector3d(-0.79729998f, -0.57920003f, -0.1699f),
            new RealVector3d(-0.4998f, -0.852f, -0.1561f),
            new RealVector3d(-0.78439999f, -0.34310001f, -0.51679999f),
            new RealVector3d(-0.56870002f, -0.63990003f, -0.51679999f),
            new RealVector3d(-0.2764f, -0.85070002f, -0.4472f),
            new RealVector3d(0.096500002f, -0.98299998f, 0.1561f),
            new RealVector3d(0.4998f, -0.852f, 0.1561f),
            new RealVector3d(-0.096500002f, -0.98299998f, -0.1561f),
            new RealVector3d(0.30450001f, -0.93720001f, -0.1699f),
            new RealVector3d(0.65579998f, -0.73860002f, -0.1561f),
            new RealVector3d(0.083899997f, -0.852f, -0.51679999f),
            new RealVector3d(0.43290001f, -0.73860002f, -0.51679999f),
            new RealVector3d(0.79729998f, 0.57920003f, 0.1699f),
            new RealVector3d(-0.30450001f, 0.93720001f, 0.1699f),
            new RealVector3d(-0.98549998f, -0.0f, 0.1699f),
            new RealVector3d(-0.30450001f, -0.93720001f, 0.1699f),
            new RealVector3d(0.79729998f, -0.57920003f, 0.1699f),
            new RealVector3d(0.0f, 0.0f, -1.0f),
            new RealVector3d(-0.1115f, 0.34310001f, -0.93269998f),
            new RealVector3d(0.29179999f, 0.212f, -0.93269998f),
            new RealVector3d(-0.2079f, 0.63990003f, -0.73970002f),
            new RealVector3d(0.1832f, 0.56370002f, -0.80540001f),
            new RealVector3d(0.54439998f, 0.3955f, -0.73970002f),
            new RealVector3d(-0.36070001f, -0.0f, -0.93269998f),
            new RealVector3d(-0.67290002f, -0.0f, -0.73970002f),
            new RealVector3d(-0.4795f, 0.3484f, -0.80540001f),
            new RealVector3d(-0.1115f, -0.34310001f, -0.93269998f),
            new RealVector3d(-0.2079f, -0.63990003f, -0.73970002f),
            new RealVector3d(-0.4795f, -0.3484f, -0.80540001f),
            new RealVector3d(0.29179999f, -0.212f, -0.93269998f),
            new RealVector3d(0.54439998f, -0.3955f, -0.73970002f),
            new RealVector3d(0.1832f, -0.56370002f, -0.80540001f),
            new RealVector3d(0.5927f, 0.0f, -0.80540001f)
        };
    }
}
