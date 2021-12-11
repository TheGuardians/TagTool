using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Bitmaps;
using TagTool.Bitmaps.Utils;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Lighting
{
    public class ReachLightmapConverter
    {
        public Action<float> ProgressUpdated;
        public int ProgressUpdateIntervalMS = 1000;
        public int MaxDegreeOfParallelism = -1;

        public ConversionResult Convert(GameCache sourceCache, Stream sourceStream, ScenarioLightmapBspData Lbsp)
        {
            if (sourceCache.Platform != CachePlatform.Original)
                throw new NotSupportedException("platform not supported yet");

            var directionBitmap = sourceCache.Deserialize<Bitmap>(sourceStream, Lbsp.LightmapSHCoefficientsBitmap);
            var intensityBitmap = sourceCache.Deserialize<Bitmap>(sourceStream, Lbsp.LightmapDominantLightDirectionBitmap);
            var convertedDirectionBitmap = BitmapConverter.ConvertGen3Bitmap(sourceCache, directionBitmap, 0, true);
            var convertedIntensityBitmap = BitmapConverter.ConvertGen3Bitmap(sourceCache, intensityBitmap, 0, true);

            int width = convertedIntensityBitmap.Width;
            int height = convertedIntensityBitmap.Height;

            var direction = BitmapDecoder.DecodeBitmap(convertedDirectionBitmap.Data, convertedDirectionBitmap.Format, width, height).Select(x => x / 255.0f).ToArray();
            var intensity = DecodeTextureArray(convertedIntensityBitmap);

            var dominantIntensities = new RealRgbColor[width * height];
            var coefficients = new List<RealRgbColor[]>();
            for (int i = 0; i < 4; i++)
                coefficients.Add(new RealRgbColor[width * height]);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Float4 sample1 = SampleTextureArray(direction, width, height, x, y, 0);
                    Float4 sample2 = SampleTextureArray(intensity, width, height, x, y, 0);
                    Float4 sample3 = SampleTextureArray(intensity, width, height, x, y, 1);
                    Float4 sample4 = SampleTextureArray(intensity, width, height, x, y, 2);

                    var dualVmf = DecompressVmfLightmapSample(sample1, sample2, sample3, sample4, Lbsp.Brightness);
                    var sh = VmfConversion.ConvertVmfLightprobe(dualVmf);
                    SphericalHarmonics.QudraticToLinearAndIntensity(sh, out SphericalHarmonics.SH2Probe linear, out RealRgbColor dominantIntensity);
                    for (int i = 0; i < 4; i++)
                    {
                        coefficients[i][y * width + x].Red = sh.R[i];
                        coefficients[i][y * width + x].Green = sh.G[i];
                        coefficients[i][y * width + x].Blue = sh.B[i];
                    }

                    dominantIntensities[y * width + x] = dominantIntensity;
                }
            }

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

        private static DualVmfBasis DecompressVmfLightmapSample(Float4 c0, Float4 c1, Float4 c2, Float4 c3, float intensityCoff)
        {
            float v17 = (c1.a * 2.0f) - 1.0f;
            float v18 = (c2.a * 2.0f) - 1.0f;
            float v19 = (c3.a * 2.0f) - 1.0f;

            float v20 = (float)Math.Min(Math.Max(Math.Sqrt((v18 * v18) + (v17 * v17) + (v19 * v19)), 0.0f), 1.0f);
            float intensityExp = (float)Math.Exp(c0.r * -6.238325);
            float intensity = intensityExp * intensityCoff;

            var vmf = new DualVmfBasis();
            vmf.Direct = new VmfLight();
            vmf.Direct.Direction.I = v17 / v20;
            vmf.Direct.Direction.J = v18 / v20;
            vmf.Direct.Direction.K = v19 / v20;
            vmf.Direct.Magnitude = c0.a;
            vmf.Direct.Color.Red = (((c2.r * 2.0f) + c1.r) - 1.0f) * intensity;
            vmf.Direct.Color.Green = (((c2.g * 2.0f) + c1.g) - 1.0f) * intensity;
            vmf.Direct.Color.Blue = (((c2.b * 2.0f) + c1.b) - 1.0f) * intensity;
            vmf.Direct.Scale = v20;
            vmf.Indirect = new VmfLight();
            vmf.Indirect.Direction = new RealVector3d();
            vmf.Indirect.Magnitude = c0.a;
            vmf.Indirect.Color.Red = intensity * c3.r;
            vmf.Indirect.Color.Green = intensity * c3.g;
            vmf.Indirect.Color.Blue = intensity * c3.b;
            vmf.Indirect.Scale = 0.0f;
            return vmf;
        }

        private static Float4 SampleTextureArray(float[] data, int width, int height, int x, int y, int z)
        {
            var sliceSize = width * height;
            int offset = 4 * (x + y * width + z * sliceSize);
            return new Float4(data[offset], data[offset + 1], data[offset + 2], data[offset + 3]);
        }

        private static float[] DecodeTextureArray(BaseBitmap bitmap)
        {
            float[] result = new float[bitmap.Width * bitmap.Height * bitmap.Depth * 4];
            byte[] bitmapData = bitmap.Data;
            int sliceSize = bitmapData.Length / bitmap.Depth;
            int decodedSliceSize = bitmap.Width * bitmap.Height * 4;

            for (int i = 0; i < bitmap.Depth; i++)
            {
                var offset = GetTextureArraySliceOffset(bitmap, i);
                var decodedOffset = i * decodedSliceSize;

                unsafe
                {
                    fixed (float* output = &result[decodedOffset])
                    fixed (byte* input = &bitmapData[offset])
                        DxtCompression.DecompressImage(output, bitmap.Width, bitmap.Height, input);
                }
            }

            return result;
        }

        private static int GetTextureArraySliceOffset(BaseBitmap bitmap, int slice)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            int bitsPerPixel = BitmapFormatUtils.GetBitsPerPixel(bitmap.Format);

            int offset = 0;

            for (int i = 0; i < slice; i++)
            {
                offset += (bitsPerPixel * (width) * (height)) / 8;
            }

            return offset;
        }

        public class ConversionResult
        {
            public byte[] LinearSH;
            public byte[] Intensity;
            public float[] MaxLs;
            public int Width;
            public int Height;
        }



        public struct Float4
        {
            public float r;
            public float g;
            public float b;
            public float a;

            public Float4(float r = 0, float g = 0, float b = 0, float a = 0)
            {
                this.r = r;
                this.g = g;
                this.b = b;
                this.a = a;
            }
        }
    }
}
