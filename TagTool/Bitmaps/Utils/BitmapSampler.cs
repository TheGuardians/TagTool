using System;
using TagTool.Common;

namespace TagTool.Bitmaps.Utils
{
    public class BitmapSampler
    {
        private byte[] Rgba;
        private int Width;
        private int Height;
        private FilteringMode FilterMode;
        private int MipmapCount;
        private int CurrentLevel;

        public BitmapSampler(byte[] rgba, int width, int height, FilteringMode filter = FilteringMode.Point, int mipCount = 0)
        {
            Rgba = rgba;
            Width = width;
            Height = height;
            FilterMode = filter;
            MipmapCount = mipCount;
            CurrentLevel = 0;
        }

        public void SetData(byte[] d) => Rgba = d;
        public void SetWidth(int w) => Width = w;
        public void SetHeight(int h) => Height = h;
        public void SetFilteringMode(FilteringMode f) => FilterMode = f;
        public void SetLevel(int l) => CurrentLevel = l <= MipmapCount ? l : 0;

        public byte[] GetData() => Rgba;
        public int GetWidth() => Width;
        public int GetHeight() => Height;
        public FilteringMode GetFilteringMode() => FilterMode;
        public int GetLevel() => CurrentLevel;
        public int GetMipmapCount() => MipmapCount;

        public RGBAColor Sample2d(float x, float y)
        {
            switch (FilterMode)
            {
                case FilteringMode.Bilinear:
                    return Sample2dBilinear(x, y);
                case FilteringMode.Point:
                default:
                    return Sample2dPoint(x, y);
            }
        }

        public RGBAColor Sample2dOffset(float x, float y, int offsetX, int offsetY)
        {
            float pixelWidth = 1.0f / GetWidth();
            float pixelHeight = 1.0f / GetHeight();
            float pxOffsetX = pixelWidth * offsetX;
            float pxOffsetY = pixelHeight * offsetY;

            x += pxOffsetX;
            y += pxOffsetY;

            switch (FilterMode)
            {
                case FilteringMode.Bilinear:
                    return Sample2dBilinear(x, y);
                case FilteringMode.Point:
                default:
                    return Sample2dPoint(x, y);
            }
        }

        public RealVector4d Sample2dOffsetF(float x, float y, int offsetX, int offsetY)
        {
            RGBAColor sample = Sample2dOffset(x, y, offsetX, offsetY);
            return new RealVector4d(sample.R / 255.0f, sample.G / 255.0f, sample.B / 255.0f, sample.A / 255.0f);
        }

        private float PixelWidth() => 1.0f / Width;
        private float PixelHeight() => 1.0f / Height;
        private int UvToIndex(float x, float y) => 4 * ((int)(y * Height) * Width + (int)(x * Width));
        private RGBAColor GetColour(int index) => new RGBAColor(Rgba[index], Rgba[index + 1], Rgba[index + 2], Rgba[index + 3]);
        private RGBAColor Sample2dPoint(int index) => GetColour(index);
        private RGBAColor Sample2dPoint(float x, float y) => Sample2dPoint(UvToIndex(x, y));
        private RGBAColor Sample2dPointBounded(float x, float y, bool hasX, bool hasY) => GetColourBounded(UvToIndex(x, y), x, y, hasX, hasY);

        /// <summary>
        /// Returns colour but ensures sample is within array/row bounds (for bilinear sampling)
        /// </summary>
        private RGBAColor GetColourBounded(int index, float x, float y, bool hasX, bool hasY)
        {
            if (hasX)
            {
                // reduce to row position
                int rowXPos = (index / 4) % Width;
                int rowXPosU = (int)(x * Width);

                if (rowXPos != rowXPosU)
                {
                    //Console.WriteLine("Row X out of sync!");
                    index -= 4;
                }
            }
            if (hasY)
            {
                int rowIndex = (int)(y * Height);

                if (rowIndex >= Height)
                {
                    //Console.WriteLine("Row Y out of sync!");
                    index -= Width * 4;
                }
            }

            if (index >= Rgba.Length)
            {
                Console.WriteLine("ERROR: bitmap sampler exceeded array length -- returning black");
                return new RGBAColor(0, 0, 0, 0);
            }

            return GetColour(index);
        }

        private RGBAColor Sample2dBilinear(float x, float y)
        {
            // scaling is wrong, todo: investigate further
            x *= 1.0f - PixelWidth();
            y *= 1.0f - PixelHeight();

            float x1 = (x - ((x) % PixelWidth()));
            float x2 = x1 + PixelWidth();
            float y1 = (y - ((y) % PixelHeight()));
            float y2 = y1 + PixelHeight();

            RGBAColor q11 = Sample2dPointBounded(x1, y1, false, false);
            RGBAColor q21 = Sample2dPointBounded(x2, y1, true, false);
            RGBAColor r1 = BilinearRow(q11, q21, x, x1, x2);

            RGBAColor q12 = Sample2dPointBounded(x1, y2, false, true);
            RGBAColor q22 = Sample2dPointBounded(x2, y2, true, true);
            RGBAColor r2 = BilinearRow(q12, q22, x, x1, x2);

            return BilinearRow(r1, r2, y, y1, y2);
        }

        private RGBAColor BilinearRow(RGBAColor q1, RGBAColor q2, float x, float x1, float x2)
        {
            // prevent unsafe divide
            bool canDiv = x2 != 0 && x2 - x1 != 0;

            float interp = canDiv ? (((x - x1) * (x2 / (x2 - x1))) / x2) : 0.0f;
            return new RGBAColor(
                (byte)(q1.R * (1.0f - interp) + q2.R * (interp)),
                (byte)(q1.G * (1.0f - interp) + q2.G * (interp)),
                (byte)(q1.B * (1.0f - interp) + q2.B * (interp)),
                (byte)(q1.A * (1.0f - interp) + q2.A * (interp)));
        }

        public enum FilteringMode
        {
            Point,
            Bilinear,
        }
    }
}
