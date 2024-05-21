using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;

namespace TagTool.Bitmaps.Utils
{
    public static class BitmapDownsampler
    {
        public static byte[] Downsample4x4BlockRgba(BitmapSampler sampler)
        {
            sampler.SetFilteringMode(BitmapSampler.FilteringMode.Bilinear); // must use bilinear

            int width = sampler.GetWidth() / 2;
            int height = sampler.GetHeight() / 2;
            byte[] result = new byte[width * height * 4];

            float pixelWidth = 1.0f / width;
            float pixelHeight = 1.0f / height;
            float halfPxWidth = pixelWidth * 0.5f;
            float halfPxHeight = pixelHeight * 0.5f;

            int rowBytes = width * 4;
            int rowOffset = 0;

            for (int i = 0; i < height; i++)
            {
                float yCoord = (pixelHeight * i) + halfPxHeight;

                for (int j = 0; j < width; j++)
                {
                    float xCoord = (pixelWidth * j) + halfPxWidth;

                    // 4x4 block
                    var sample0 = sampler.Sample2dOffsetF(xCoord, yCoord, -1, -1);
                    var sample1 = sampler.Sample2dOffsetF(xCoord, yCoord,  1, -1);
                    var sample2 = sampler.Sample2dOffsetF(xCoord, yCoord, -1,  1);
                    var sample3 = sampler.Sample2dOffsetF(xCoord, yCoord,  1,  1);
                    RealVector4d sample = (sample0 + sample1 + sample2 + sample3) / 4.0f;
                    sample *= 255; // to byte

                    int streamOffset = rowOffset + j * 4;
                    result[streamOffset]     = (byte)sample.I;
                    result[streamOffset + 1] = (byte)sample.J;
                    result[streamOffset + 2] = (byte)sample.K;
                    result[streamOffset + 3] = (byte)sample.W;
                }

                rowOffset += rowBytes;
            }

            return result;
        }

        public static byte[] Downsample4x4BlockRgba(BitmapSampler sampler, int width, int height)
        {
            sampler.SetFilteringMode(BitmapSampler.FilteringMode.Bilinear); // must use bilinear

            byte[] result = new byte[width * height * 4];

            float pixelWidth = 1.0f / width;
            float pixelHeight = 1.0f / height;
            float halfPxWidth = pixelWidth * 0.5f;
            float halfPxHeight = pixelHeight * 0.5f;

            int rowBytes = width * 4;
            int rowOffset = 0;

            for (int i = 0; i < height; i++)
            {
                float yCoord = (pixelHeight * i) + halfPxHeight;

                for (int j = 0; j < width; j++)
                {
                    float xCoord = (pixelWidth * j) + halfPxWidth;

                    // 4x4 block
                    var sample0 = sampler.Sample2dOffsetF(xCoord, yCoord, -1, -1);
                    var sample1 = sampler.Sample2dOffsetF(xCoord, yCoord, 1, -1);
                    var sample2 = sampler.Sample2dOffsetF(xCoord, yCoord, -1, 1);
                    var sample3 = sampler.Sample2dOffsetF(xCoord, yCoord, 1, 1);
                    RealVector4d sample = (sample0 + sample1 + sample2 + sample3) / 4.0f;
                    sample *= 255; // to byte

                    int streamOffset = rowOffset + j * 4;
                    result[streamOffset] = (byte)sample.I;
                    result[streamOffset + 1] = (byte)sample.J;
                    result[streamOffset + 2] = (byte)sample.K;
                    result[streamOffset + 3] = (byte)sample.W;
                }

                rowOffset += rowBytes;
            }

            return result;
        }

        // try not to use this
        public static byte[] DownsampleBilinearRgba(BitmapSampler sampler)
        {
            sampler.SetFilteringMode(BitmapSampler.FilteringMode.Bilinear); // must use bilinear

            int width = sampler.GetWidth() / 2;
            int height = sampler.GetHeight() / 2;
            byte[] result = new byte[width * height * 4];

            float pixelWidth = 1.0f / width;
            float pixelHeight = 1.0f / height;
            float halfPxWidth = pixelWidth * 0.5f;
            float halfPxHeight = pixelHeight * 0.5f;

            int rowBytes = width * 4;
            int rowOffset = 0;

            for (int i = 0; i < height; i++)
            {
                float yCoord = (pixelHeight * i) + halfPxHeight;

                for (int j = 0; j < width; j++)
                {
                    float xCoord = (pixelWidth * j) + halfPxWidth;

                    var sample = sampler.Sample2d(xCoord, yCoord);

                    int streamOffset = rowOffset + j * 4;
                    result[streamOffset]     = sample.R;
                    result[streamOffset + 1] = sample.G;
                    result[streamOffset + 2] = sample.B;
                    result[streamOffset + 3] = sample.A;
                }

                rowOffset += rowBytes;
            }

            return result;
        }
    }
}
