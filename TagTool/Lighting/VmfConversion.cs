using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Lighting
{
    public static class VmfConversion
    {
        public static void ConvertStaticPerVertexBuffers(ScenarioLightmapBspData Lbsp, RenderGeometryApiResourceDefinition renderGeometryResource, CacheVersion targetVersion, CachePlatform targetPlatform = CachePlatform.All)
        {
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

            foreach (var clusterLighting in Lbsp.ClusterStaticPerVertexLightingBuffers)
            {
                if (clusterLighting.StaticPerVertexLightingIndex != -1)
                {
                    var element = Lbsp.StaticPerVertexLightingBuffers[clusterLighting.StaticPerVertexLightingIndex];

                    if (clusterLighting.PerVertexOffset > 0)
                    {
                        // We need pad the buffer to make it 1:1 with the geometry index buffer
                        // The mesh parts < PerVertexOffset will have the PerVertexLightmapPart flag, which will then
                        // will be used to disable static per vertex for just those part (at runtime)

                        var vertexBuffer = renderGeometryResource.VertexBuffers[element.VertexBufferIndexReach].Definition;
                        var ms = new MemoryStream();
                        StreamUtil.Fill(ms, 0xCD, clusterLighting.PerVertexOffset * 0x14);
                        ms.Write(vertexBuffer.Data.Data, 0, vertexBuffer.Data.Data.Length);
                        vertexBuffer.Data = new Tags.TagData(ms.ToArray());
                        vertexBuffer.Count += clusterLighting.PerVertexOffset;
                    }
                }
            }
        }

        public static byte[] ConvertStaticPerVertexData(byte[] data, float hdrScalar, out int vertexCount, CacheVersion targetVersion, CachePlatform targetPlatform)
        {
            var outputStream = new MemoryStream();
            var vertexStream = VertexStreamFactory.Create(targetVersion, targetPlatform, outputStream);

            if (data.Length % 6 != 0)
            {
                if (!data.Skip(data.Length / 6 * 6).Take(data.Length % 6).All(x => x == 0xCD))
                {
                    new TagToolWarning("Expected debug fill in static per vertex data!");
                }
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
                    AnalyticalMask = dc,
                    Color = new RealRgbColor(r0, g0, b0),
                    Bandwidth = 1.0f
                };
                vmfProbe.Indirect = new VmfLight()
                {
                    Direction = new RealVector3d(0, 0, 1),
                    AnalyticalMask = dc,
                    Color = new RealRgbColor(r1, g1, b1),
                    Bandwidth = 0.0f
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

        public static float DecompressedNormalizedBandwidth(float y)
        {
            if (y < 0.0f) y = 0.0f;
            if (y > 1.0f) y = 1.0f;
            return (float)((Math.Pow(56.976875 - (y * 56.0), -0.93375814) * 55.384312) - 0.81427675);
        }

        public static SphericalHarmonics.SH3Probe ConvertVmfLightprobe(DualVmfBasis vmf)
        {
            float[] dczh = new float[3];
            float[] iczh = new float[3];
            VmfLight.EvalulateVmf(DecompressedNormalizedBandwidth(vmf.Direct.Bandwidth), dczh);
            VmfLight.EvalulateVmf(DecompressedNormalizedBandwidth(vmf.Indirect.Bandwidth), iczh);

            float[] sh_basis = new float[9];
            SphericalHarmonics.EvaluateDirection(vmf.Direct.Direction, 3, sh_basis);

            var result = new SphericalHarmonics.SH3Probe();
            result.R[0] = (sh_basis[0] * dczh[0] * vmf.Direct.Color.Red) + (sh_basis[0] * iczh[0] * vmf.Indirect.Color.Red);
            result.R[1] = (sh_basis[1] * dczh[1] * vmf.Direct.Color.Red);
            result.R[2] = (sh_basis[2] * dczh[1] * vmf.Direct.Color.Red);
            result.R[3] = (sh_basis[3] * dczh[1] * vmf.Direct.Color.Red);
            result.R[4] = (sh_basis[4] * dczh[2] * vmf.Direct.Color.Red);
            result.R[5] = (sh_basis[5] * dczh[2] * vmf.Direct.Color.Red);
            result.R[6] = (sh_basis[6] * dczh[2] * vmf.Direct.Color.Red);
            result.R[7] = (sh_basis[7] * dczh[2] * vmf.Direct.Color.Red);
            result.R[8] = (sh_basis[8] * dczh[2] * vmf.Direct.Color.Red);

            result.G[0] = (sh_basis[0] * dczh[0] * vmf.Direct.Color.Green) + (sh_basis[0] * iczh[0] * vmf.Indirect.Color.Green);
            result.G[1] = (sh_basis[1] * dczh[1] * vmf.Direct.Color.Green);
            result.G[2] = (sh_basis[2] * dczh[1] * vmf.Direct.Color.Green);
            result.G[3] = (sh_basis[3] * dczh[1] * vmf.Direct.Color.Green);
            result.G[4] = (sh_basis[4] * dczh[2] * vmf.Direct.Color.Green);
            result.G[5] = (sh_basis[5] * dczh[2] * vmf.Direct.Color.Green);
            result.G[6] = (sh_basis[6] * dczh[2] * vmf.Direct.Color.Green);
            result.G[7] = (sh_basis[7] * dczh[2] * vmf.Direct.Color.Green);
            result.G[8] = (sh_basis[8] * dczh[2] * vmf.Direct.Color.Green);

            result.B[0] = (sh_basis[0] * dczh[0] * vmf.Direct.Color.Blue) + (sh_basis[0] * iczh[0] * vmf.Indirect.Color.Blue);
            result.B[1] = (sh_basis[1] * dczh[1] * vmf.Direct.Color.Blue);
            result.B[2] = (sh_basis[2] * dczh[1] * vmf.Direct.Color.Blue);
            result.B[3] = (sh_basis[3] * dczh[1] * vmf.Direct.Color.Blue);
            result.B[4] = (sh_basis[4] * dczh[2] * vmf.Direct.Color.Blue);
            result.B[5] = (sh_basis[5] * dczh[2] * vmf.Direct.Color.Blue);
            result.B[6] = (sh_basis[6] * dczh[2] * vmf.Direct.Color.Blue);
            result.B[7] = (sh_basis[7] * dczh[2] * vmf.Direct.Color.Blue);
            result.B[8] = (sh_basis[8] * dczh[2] * vmf.Direct.Color.Blue);

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
