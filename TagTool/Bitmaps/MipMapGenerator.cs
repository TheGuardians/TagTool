using System;
using System.Collections.Generic;

namespace TagTool.Bitmaps
{
    public class MipMapGenerator
    {
        public List<MipMap> MipMaps;

        public MipMapGenerator()
        {
            MipMaps = new List<MipMap>();
        }

        public int GetTotalRawSize()
        {
            var size = 0;
            foreach (var mipmap in MipMaps)
            {
                size += mipmap.Data.Length;
            }
            return size;
        }

        public byte[] GetTotalRaw()
        {
            byte[] result = new byte[GetTotalRawSize()];
            var currentOffset = 0;
            for(int i = 0; i<MipMaps.Count; i++)
            {
                var currentMipMap = MipMaps[i];
                Array.Copy(currentMipMap.Data, 0, result, currentOffset, currentMipMap.Data.Length);
                currentOffset += currentMipMap.Data.Length;
            }
            return result;
        }

        public byte[] CombineImage(byte[] image)
        {
            byte[] result = new byte[image.Length + GetTotalRawSize()];
            Array.Copy(image, 0, result, 0, image.Length);
            Array.Copy(GetTotalRaw(), 0, result, image.Length, GetTotalRawSize());
            return result;
        }

        public void GenerateMipMap(int height, int width, byte[] data, int channelCount, int mipMapCount = -1)
        {
            MipMaps = new List<MipMap>();
            float[] floatData;
            floatData = NormalizeImage(data);
            GenerateMipMapFloat(height, width, floatData, channelCount, mipMapCount);
        }

        private void GenerateMipMapFloat(int height, int width, float[] data, int channelCount = 4, int level = -1)
        {
            int newHeight = Math.Max(1, ((height) / 2));
            int newWidth = Math.Max(1, ((width) / 2));

            if(newHeight == height && newWidth == width || level == 0)
                return;
            
            int size = newHeight * newWidth * channelCount;
            float[] result = new float[size];

            
            //Split channels
            for(int c = 0; c< channelCount; c++)
            {
                float[] channelData = new float[width * height];
                for (int k = 0; k < data.Length; k += channelCount)
                {
                   channelData[k / channelCount] = data[k + c];
                }
                float[] channelMipMap = ComputeColorAverage(width, height, newWidth, newHeight, channelData);
                for (int k = 0; k < size; k += channelCount)
                {
                    result[k + c] = channelMipMap[k / channelCount];
                }
            }
           
            //Add to the list of mipmaps
            byte[] byteResult;
            byteResult = DenormalizeImage(result);
            MipMaps.Add(new MipMap(byteResult, newWidth, newHeight));

            //Build the next mipmap
            GenerateMipMapFloat(newHeight, newWidth, result, channelCount, level - 1);
        }

        private static float GammaCorrectedAverage(float a, float b, float c, float d)
        {
            //Gamma = 2
            return (float)Math.Sqrt((a * a + b * b + c * c + d * d) / 4);
        }

        private static float[] ComputeColorAverage(int width, int height, int newWidth, int newHeight, float[] color)
        {
            float[] colorM = new float[newWidth*newHeight];

            float x1, x2, x3, x4;
            bool padHeight = false, padWidth = false;
            int HorizontalSize = width;

            for (int j = 0; j < newHeight; j ++)
            {
                if (j == newHeight - 1 && newHeight % 2 == 1)
                    padHeight = true;

                for (int i = 0; i < newWidth; i ++)
                {
                    if (i == newWidth - 1 && newWidth % 2 == 1)
                        padWidth = true;

                    x1 = color[2*j * HorizontalSize + 2*i];

                    if (!padHeight)
                        x3 = color[(2*j + 1) * HorizontalSize + 2*i];
                    else
                        x3 = x1;
                    if (!padWidth)
                        x2 = color[2*j * HorizontalSize + (2*i + 1)];
                    else
                        x2 = x1;
                    if (!padWidth && !padHeight)
                        x4 = color[(2*j + 1) * HorizontalSize + (2*i + 1)];
                    else
                        x4 = x1;

                    colorM[((j) * newWidth ) + i] = GammaCorrectedAverage(x1, x2, x3, x4);
                }
            }
            return colorM;
        }

        private static float[] NormalizeImage(byte[] data)
        {
            float[] result = new float[data.Length];
            for(int i = 0; i < result.Length; i++)
            {
                result[i] = ((float)data[i])/ 255;
            }
            return result;
        }

        private static byte[] DenormalizeImage(float[] data)
        {
            byte[] result = new byte[data.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)(data[i] * 255);
            }
            return result;
        }
    }

    public class MipMap
    {
        public byte[] Data;
        public int Width;
        public int Height;

        public MipMap()
        {
            Data = new byte[0];
            Width = -1;
            Height = -1;
        }
        public MipMap(byte[] data,int width, int height)
        {
            Data = data;
            Width = width;
            Height = height;
        }
    }

}
