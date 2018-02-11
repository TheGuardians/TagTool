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

        public void GenerateMipMap(int height, int width, byte[] data, BitmapFormat format = BitmapFormat.A8R8G8B8)
        {
            MipMaps = new List<MipMap>();
            float[] floatData;

            switch (format)
            {
                case BitmapFormat.A8R8G8B8:
                    floatData = NormalizeImage(data);
                    break;
                case BitmapFormat.A8:
                case BitmapFormat.Y8:
                case BitmapFormat.AY8:
                    floatData = NormalizeByteImage(data);
                    break;

                case BitmapFormat.A8Y8:
                    floatData = NormalizeUShortImage(data);
                    break;

                default:
                    floatData = new float[0];
                    break;
            }

            
            GenerateMipMapFloat(height, width, floatData, format);
        }

        public void GenerateMipMap(int height, int width, byte[] data, int count, BitmapFormat format = BitmapFormat.A8R8G8B8)
        {
            MipMaps = new List<MipMap>();
            float[] floatData;
            switch (format)
            {
                case BitmapFormat.A8R8G8B8:
                    floatData = NormalizeImage(data);
                    break;
                case BitmapFormat.A8:
                case BitmapFormat.Y8:
                case BitmapFormat.AY8:
                    floatData = NormalizeByteImage(data);
                    break;

                case BitmapFormat.A8Y8:
                    floatData = NormalizeUShortImage(data);
                    break;

                default:
                    floatData = new float[0];
                    break;
            }
            GenerateMipMapFloat(height, width, floatData, format, count);
        }

        private void GenerateMipMapFloat(int height, int width, float[] data, BitmapFormat format = BitmapFormat.A8R8G8B8, int count = -1)
        {
            int newHeight = Math.Max(1, ((height) / 2));
            int newWidth = Math.Max(1, ((width) / 2));

            if(newHeight == height && newWidth == width || count == 0)
                return;
            
            int size = newHeight * newWidth * 4;                //4 floats per pixel
            float[] result = new float[size];

            //  [bx1 gx1 bx2 gx2]
            //  [rx1 ax1 rx2 ax2]
            //  [bx3 gx3 bx4 gx4]
            //  [rx3 ax3 rx4 ax4]

            //Split colors
            float[] A = new float[width * height];
            float[] R = new float[width * height];
            float[] G = new float[width * height];
            float[] B = new float[width * height];

            for (int k = 0; k < data.Length; k+=4)
            {
                B[k/4] = data[k];
                G[k/4] = data[k+1];
                R[k/4] = data[k+2];
                A[k/4] = data[k+3];
            }

            float[] Am = ComputeColorAverage(width, height, newWidth, newHeight, A);
            float[] Rm = ComputeColorAverage(width, height, newWidth, newHeight, R);
            float[] Gm = ComputeColorAverage(width, height, newWidth, newHeight, G);
            float[] Bm = ComputeColorAverage(width, height, newWidth, newHeight, B);

            //Merge color
            for (int k = 0; k < size; k+=4)
            {
                result[k] = Bm[k/4];
                result[k+1] = Gm[k/4];
                result[k+2] = Rm[k/4];
                result[k+3] = Am[k/4];
            }

            //Add to the list of mipmaps
            byte[] byteResult;
            switch (format)
            {
                case BitmapFormat.A8R8G8B8:
                    byteResult = DenormalizeImage(result);
                    break;
                case BitmapFormat.A8:
                case BitmapFormat.Y8:
                case BitmapFormat.AY8:
                    byteResult = DenormalizeByteImage(result);
                    break;

                case BitmapFormat.A8Y8:
                    byteResult = DenormalizeUShortImage(result);
                    break;

                default:
                    byteResult = new byte[0];
                    break;
            }
            MipMaps.Add(new MipMap(byteResult, newWidth, newHeight));

            //Build the next mipmap
            GenerateMipMapFloat(newHeight, newWidth, result, format, count -1);
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

            for (int j = 0; j < height/2; j ++)
            {
                for (int i = 0; i < width/2; i ++)
                {
                    
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


            /*
             *  for (int j = 0; j < height; j += 2)
            {
                
                if (j + 1 >= height)
                    padHeight = true;
                
                for (int i = 0; i < width; i += 2)
                {
                    
                    if (i + 1 >= width)
                        padWidth = true;
                    
                    x1 = color[j * HorizontalSize + i];

                    if (!padHeight)
                        x3 = color[(j + 1) * HorizontalSize + i];
                    else
                        x3 = x1;
                    if (!padWidth)
                        x2 = color[j * HorizontalSize + (i + 1)];
                    else
                        x2 = x1;
                    if (!padWidth && !padHeight)
                        x4 = color[(j + 1) * HorizontalSize + (i + 1)];
                    else
                        x4 = x1;

                    colorM[((j/2) * newWidth ) + i/2] = GammaCorrectedAverage(x1, x2, x3, x4);
                }
            }
             */
            return colorM;
        } 

        public static float[] NormalizeImage(byte[] data)
        {
            float[] result = new float[data.Length];
            for(int i = 0; i < result.Length; i++)
            {
                result[i] = ((float)data[i])/ 255;
            }
            return result;
        }

        public static byte[] DenormalizeImage(float[] data)
        {
            byte[] result = new byte[data.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)(data[i] * 255);
            }
            return result;
        }

        public static float[] NormalizeByteImage(byte[] data)
        {
            float[] result = new float[data.Length*4];
            for (int i = 0; i < result.Length; i+=4)
            {
                var val = ((float)data[i/4]) / 255;
                result[i] = val;
                result[i + 1] = val;
                result[i + 2] = val;
                result[i + 3] = val;
            }
            return result;
        }

        public static byte[] DenormalizeByteImage(float[] data)
        {
            byte[] result = new byte[data.Length/4];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)(data[i*4] * 255);
            }
            return result;
        }

        public static float[] NormalizeUShortImage(byte[] data)
        {
            float[] result = new float[data.Length * 2];
            for (int i = 0; i < result.Length; i += 2)
            {
                var val = ((float)data[i / 2]) / 255;
                result[i] = val;
                result[i + 1] = val;
            }
            return result;
        }

        public static byte[] DenormalizeUShortImage(float[] data)
        {
            byte[] result = new byte[data.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)(data[i * 2] * 255);
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
