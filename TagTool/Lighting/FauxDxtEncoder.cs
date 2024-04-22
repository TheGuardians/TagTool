using System;
using TagTool.Common;

namespace TagTool.Lighting
{
    public class FauxDxtEncoder
    {
        public RealRgbColor Weights { get; set; } = new RealRgbColor(0.6f, 1.0f, 0.3f);
        public ushort PixelMask = 0xFFFF;

        public ColorBlock EncodeColorBlock(RealRgbColor[] input, RealRgbColor[] residual = null)
        {
            var block = new ColorBlock();

            float x_min = float.MaxValue;
            float x_max = -float.MaxValue;
            float y_min = float.MaxValue;
            float y_max = -float.MaxValue;
            float z_min = float.MaxValue;
            float z_max = -float.MaxValue;

            float min_error = float.MaxValue;

            int possible_color_count = 0;
            for (int i = 0; i < 16; i++)
            {
                if (!InMask(i))
                    continue;

                ++possible_color_count;
                x_min = (float)Math.Min(input[i].Red, x_min);
                x_max = (float)Math.Max(input[i].Red, x_max);
                y_min = (float)Math.Min(input[i].Green, y_min);
                y_max = (float)Math.Max(input[i].Green, y_max);
                z_min = (float)Math.Min(input[i].Blue, z_min);
                z_max = (float)Math.Max(input[i].Blue, z_max);
            }

            if (possible_color_count > 0)
            {
                int color_count = 0;
                int[] reds = new int[16];
                int[] greens = new int[16];
                int[] blues = new int[16];

                for (int i = 0; i < 16; i++)
                {
                    if (!InMask(i))
                        continue;

                    if (input[i].Red == x_min || input[i].Red == x_max ||
                        input[i].Green == y_min || input[i].Green == y_max ||
                        input[i].Blue == z_min || input[i].Blue == z_max)
                    {
                        byte r = (byte)((float)((input[i].Red * 31.0) + 0.5));
                        byte g = (byte)((float)((input[i].Blue * 31.0) + 0.5));
                        byte b = (byte)((float)((input[i].Green * 63.0) + 0.5));

                        int index = 0;
                        for (int j = 0; j < color_count; ++j)
                        {
                            if (r == reds[j] && b == greens[j] && g == blues[j])
                                break;
                            ++index;
                        }

                        if (index == color_count)
                        {
                            ++color_count;
                            reds[index] = r;
                            greens[index] = b;
                            blues[index] = g;
                        }
                    }
                }

                if (color_count == 1)
                {
                    // single color fit
                    color_count = 2;
                    reds[1] = reds[0];
                    greens[1] = greens[0];
                    blues[1] = blues[0];
                }

                if (color_count - 1 > 0)
                {
                    for (int i = 0; i < color_count; i++)
                    {
                        int ry_min = reds[i] - 1;
                        int ry_max = reds[i] + 1;
                        int gy_min = greens[i] - 1;
                        int gy_max = greens[i] + 1;
                        int by_min = blues[i] - 1;
                        int by_max = blues[i] + 1;

                        if (ry_min < 0) ry_min = 0;
                        if (ry_max > 31) ry_max = 31;
                        if (gy_min < 0) gy_min = 0;
                        if (gy_max > 63) gy_max = 63;
                        if (by_min < 0) by_min = 0;
                        if (by_max > 31) by_max = 31;

                        for (int j = i + 1; j < color_count; j++)
                        {
                            int rx_min = reds[j] - 1;
                            int rx_max = reds[j] + 1;
                            int gx_min = greens[j] - 1;
                            int gx_max = greens[j] + 1;
                            int bx_min = blues[j] - 1;
                            int bx_max = blues[j] + 1;

                            if (rx_min < 0) rx_min = 0;
                            if (rx_max > 31) rx_max = 31;
                            if (gx_min < 0) gx_min = 0;
                            if (gx_max > 63) gx_max = 63;
                            if (bx_min < 0) bx_min = 0;
                            if (bx_max > 31) bx_max = 31;

                            for (int ry = ry_min; ry <= ry_max; ++ry)
                            {
                                for (int gy = gy_min; gy <= gy_max; ++gy)
                                {
                                    for (int by = by_min; by <= by_max; ++by)
                                    {
                                        for (int rx = rx_min; rx <= rx_max; ++rx)
                                        {
                                            for (int gx = gx_min; gx <= gx_max; ++gx)
                                            {
                                                for (int bx = bx_min; bx <= bx_max; ++bx)
                                                {
                                                    ushort color0 = (ushort)(((ry & 0x1f) << 11) | ((gy & 0x3f) << 5) | (by & 0x1f));
                                                    ushort color1 = (ushort)(((rx & 0x1f) << 11) | ((gx & 0x3f) << 5) | (bx & 0x1f));
                                                    float error = EvaluateColorDistance(color0, color1, input);
                                                    if (min_error > error)
                                                    {
                                                        min_error = error;
                                                        block.Color0 = color0;
                                                        block.Color1 = color1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                EvaluateColorIndices(block.Color0, block.Color1, input, block.Indices, residual);
            }

            return block;
        }

        public AlphaBlock EncodeAlphaBlock(float[] input, float[] residual = null)
        {
            var block = new AlphaBlock();

            float alpha_max = -1.0f;
            float alpha_min = 1.0f;
            float min_error = float.MaxValue;
            int count = 0;

            for (int i = 0; i < 16; i++)
            {
                if (!InMask(i))
                    continue;

                count++;
                alpha_min = (float)Math.Min(input[i], alpha_min);
                alpha_max = (float)Math.Max(input[i], alpha_max);
            }

            if (count > 0)
            {
                int a = (int)(float)Math.Min((float)Math.Max((alpha_min * 255.0f) + 0.5f, 0.0), 255.1f);
                int b = (int)(float)Math.Min((float)Math.Max((alpha_max * 255.0f) + 0.5f, 0.0), 255.1f);

                int x_min = a - 12;
                int x_max = a + 12;
                int y_min = b - 12;
                int y_max = b + 12;

                if (y_min < 0) y_min = 0;
                if (x_min < 0) x_min = 0;
                if (y_max > 255) y_max = 255;
                if (x_max > 255) x_max = 255;

                int alpha0 = x_min;
                int alpha1 = x_min;

                for (int y = y_min; y <= y_max; y += 2)
                {
                    for (int x = x_min; x < x_max; x += 2)
                    {
                        float error1 = EvalAlphaDistance((ushort)y, (ushort)x, input);
                        if (min_error > error1)
                        {
                            min_error = error1;
                            alpha0 = y;
                            alpha1 = x;
                        }

                        float error2 = EvalAlphaDistance((ushort)x, (ushort)y, input);
                        if (min_error > error2)
                        {
                            min_error = error2;
                            alpha0 = x;
                            alpha1 = y;
                        }
                    }
                }
                block.Alpha0 = (byte)alpha0;
                block.Alpha1 = (byte)alpha1;
                EvalAlphaIndices(block.Alpha0, block.Alpha1, input, block.Indices, residual);
            }

            return block;
        }

        private unsafe float EvalAlphaDistance(ushort alpha0, ushort alpha1, float[] input)
        {
            var alphas = stackalloc float[8];
            EvaluateAlphaPalette(alpha0, alpha1, alphas);

            float sum = 0;
            for (int i = 0; i < 16; ++i)
            {
                if (!InMask(i))
                    continue;

                float min_distance = float.MaxValue;
                for (int j = 0; j < 8; j++)
                {
                    float distance = AlphaDistance(input[i], alphas[j]);
                    if (min_distance > distance)
                        min_distance = distance;
                }
                sum += min_distance / ((float)Math.Abs(input[i]) + 0.1f);
            }

            return sum;
        }

        private unsafe void EvalAlphaIndices(ushort alpha0, ushort alpha1, float[] input, byte[] indices, float[] residual)
        {
            var alphas = stackalloc float[8];
            EvaluateAlphaPalette(alpha0, alpha1, alphas);

            for (int i = 0; i < 16; ++i)
            {
                if (!InMask(i))
                    continue;

                int alpha_index = 0;
                float min_distance = float.MaxValue;
                for (int j = 0; j < 8; j++)
                {
                    float distance = AlphaDistance(input[i], alphas[j]);
                    if (min_distance > distance)
                    {
                        min_distance = distance;
                        alpha_index = j;
                    }
                }

                indices[i] = (byte)alpha_index;
                if (residual != null)
                    residual[i] = alphas[alpha_index];
            }
        }

        private unsafe void EvaluateAlphaPalette(ushort alpha0, ushort alpha1, float* palette)
        {
            for (int i = 0; i < 8; i++)
            {
                ushort v7;

                if (alpha0 <= alpha1)
                {
                    if (i > 0)
                    {
                        switch (i)
                        {
                            case 1:
                                v7 = (ushort)(32 * alpha1);
                                break;
                            case 2:
                                v7 = (ushort)(6 * alpha1 + 26 * alpha0);
                                break;
                            case 3:
                                v7 = (ushort)(13 * alpha1 + 19 * alpha0);
                                break;
                            case 4:
                                v7 = (ushort)(19 * alpha1 + 13 * alpha0);
                                break;
                            case 5:
                                v7 = (ushort)(6 * alpha0 + 26 * alpha1);
                                break;
                            case 6:
                                v7 = 0;
                                break;
                            default:
                                v7 = 0x1FE0;
                                break;
                        }
                    }
                    else
                    {
                        v7 = (ushort)(32 * alpha0);
                    }
                }
                else if (i > 0)
                {
                    switch (i)
                    {
                        case 1:
                            v7 = (ushort)(32 * alpha1);
                            break;
                        case 2:
                            v7 = (ushort)(5 * alpha1 + 27 * alpha0);
                            break;
                        case 3:
                            v7 = (ushort)(9 * alpha1 + 23 * alpha0);
                            break;
                        case 4:
                            v7 = (ushort)(14 * alpha1 + 18 * alpha0);
                            break;
                        case 5:
                            v7 = (ushort)(18 * alpha1 + 14 * alpha0);
                            break;
                        case 6:
                            v7 = (ushort)(23 * alpha1 + 9 * alpha0);
                            break;
                        default:
                            v7 = (ushort)(5 * alpha0 + 27 * alpha1);
                            break;
                    }
                }
                else
                {
                    v7 = (ushort)(32 * alpha0);
                }

                palette[i] = (v7 >> 5) * 0.0039215689f;
            }
        }

        private float AlphaDistance(float a, float b)
        {
            return (float)Math.Abs(a - b);
        }

        private unsafe void EvaluateColorPalette(ushort color0, ushort color1, RealRgbColor* palette)
        {
            ushort r0 = (ushort)((int)(float)((float)((color0 >> 11) * 2114.0322f) + 0.5f) >> 8);
            ushort g0 = (ushort)((int)(float)((float)(((color0 >> 5) & 63) * 1040.238f) + 0.5f) >> 8);
            ushort b0 = (ushort)((int)(float)((float)((color0 & 31) * 2114.0322f) + 0.5f) >> 8);
            ushort r1 = (ushort)((int)(float)((float)((color1 >> 11) * 2114.0322f) + 0.5f) >> 8);
            ushort g1 = (ushort)((int)(float)((float)(((color1 >> 5) & 63) * 1040.238f) + 0.5f) >> 8);
            ushort b1 = (ushort)((int)(float)((float)((color1 & 31) * 2114.0322f) + 0.5f) >> 8);

            for (int i = 0; i < 4; i++)
            {
                ushort r, g, b;

                switch (i)
                {
                    case 0:
                        r = (ushort)(r0 & 0x7FF);
                        g = (ushort)(g0 & 0x7FF);
                        b = (ushort)(32 * b0);
                        break;
                    case 1:
                        r = (ushort)(r1 & 0x7FF);
                        g = (ushort)(g1 & 0x7FF);
                        b = (ushort)(32 * b1);
                        break;
                    case 2:
                        r = (ushort)((21 * r0 + 11 * r1) >> 5);
                        g = (ushort)((21 * g0 + 11 * g1) >> 5);
                        b = (ushort)((21 * b0) + (11 * b1));
                        break;
                    case 3:
                        r = (ushort)((11 * r0 + 21 * r1) >> 5);
                        g = (ushort)((11 * g0 + 21 * g1) >> 5);
                        b = (ushort)((11 * b0) + (21 * b1));
                        break;
                    default:
                        throw new ArgumentException();
                }

                palette[i].Red = r * 0.0039215689f;
                palette[i].Green = g * 0.0039215689f;
                palette[i].Blue = (b >> 5) * 0.0039215689f;
            }
        }

        private unsafe void EvaluateColorIndices(ushort color0, ushort color1, RealRgbColor[] input, byte[] indices, RealRgbColor[] residual)
        {
            var palette = stackalloc RealRgbColor[4];
            EvaluateColorPalette(color0, color1, palette);

            for (int i = 0; i < 16; i++)
            {
                if (!InMask(i))
                    continue;

                float min_distance2d = float.MaxValue;
                int palette_index = -1;
                for (int j = 0; j < 4; j++)
                {
                    float distance2d = ColorDistance(input[i], palette[j]);
                    if (min_distance2d > distance2d)
                    {
                        min_distance2d = distance2d;
                        palette_index = j;
                    }
                }

                if (residual != null)
                {
                    residual[i].Red = input[i].Red - palette[palette_index].Red;
                    residual[i].Green = input[i].Green - palette[palette_index].Green;
                    residual[i].Blue = input[i].Blue - palette[palette_index].Blue;
                }

                indices[i] = (byte)palette_index;
            }
        }

        private unsafe float EvaluateColorDistance(ushort color0, ushort color1, RealRgbColor[] input)
        {
            var palette = stackalloc RealRgbColor[4];
            EvaluateColorPalette(color0, color1, palette);

            float sum = 0.0f;
            for (int i = 0; i < 16; i++)
            {
                if (!InMask(i))
                    continue;

                float min_distance2d = float.MaxValue;
                for (int j = 0; j < 4; j++)
                {
                    float distance2d = ColorDistance(input[i], palette[j]);
                    if (min_distance2d > distance2d)
                        min_distance2d = distance2d;
                }
                sum += min_distance2d;
            }

            return sum;
        }

        private float ColorDistance(RealRgbColor c1, RealRgbColor c2)
        {
            float rd = (c1.Red - c2.Red) * Weights.Red;
            float gd = (c1.Green - c2.Green) * Weights.Green;
            float bd = (c1.Blue - c2.Blue) * Weights.Blue;
            return gd * gd + rd * rd + bd * bd;
        }

        private bool InMask(int i) => ((PixelMask >> i) & 1) != 0;

        public class AlphaBlock
        {
            public byte Alpha0;
            public byte Alpha1;
            public byte[] Indices = new byte[16];

            public void Pack(int offset, byte[] buffer)
            {
                buffer[offset + 0] = Alpha0;
                buffer[offset + 1] = Alpha1;
                long code = 0;
                for (int i = 15; i >= 0; i--)
                    code = (8 * code) + (Indices[i] & 7);
                Array.Copy(BitConverter.GetBytes(code), 0, buffer, offset + 2, 6);
            }
        }

        public class ColorBlock
        {
            public ushort Color0;
            public ushort Color1;
            public byte[] Indices = new byte[16];

            public void Pack(int offset, byte[] buffer)
            {
                buffer[offset + 0] = (byte)(Color0 & 0xff);
                buffer[offset + 1] = (byte)(Color0 >> 8);
                buffer[offset + 2] = (byte)(Color1 & 0xff);
                buffer[offset + 3] = (byte)(Color1 >> 8);
                int code = 0;
                for (int i = 15; i >= 0; i--)
                    code = (4 * code) + (Indices[i] & 3);
                Array.Copy(BitConverter.GetBytes(code), 0, buffer, offset + 4, 4);
            }
        }
    }
}
