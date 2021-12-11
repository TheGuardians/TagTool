using System;
using TagTool.Cache;

namespace TagTool.Common
{
    static class ColorConversion
    {
        public static RealRgbColor FromRgbe(uint color, bool isSigned = true)
        {
            RealArgbColor result;
            float m;
            if (isSigned)
            {
                result = new RealArgbColor();
                result.Blue = (sbyte)(color & 0xFF) / 127.0f;
                result.Green = (sbyte)((color >> 8) & 0xFF) / 127.0f;
                result.Red = (sbyte)((color >> 16) & 0xFF) / 127.0f;
                result.Alpha = (sbyte)((color >> 24) & 0xFF) / 127.0f;
                m = (float)Math.Pow(2.0f, result.Alpha * (127 / 4.0f));
            }
            else
            {
                result = new RealArgbColor();
                result.Blue = (color & 0xFF) / 255.0f;
                result.Green = ((color >> 8) & 0xFF) / 255.0f;
                result.Red = ((color >> 16) & 0xFF) / 255.0f;
                result.Alpha = ((color >> 24) & 0xFF) / 255.0f;
                m = (float)Math.Pow(2.0f, result.Alpha * (255 / 4.0f) - (127 / 4.0f));
            }

            return new RealRgbColor(result.Red * m, result.Green * m, result.Blue * m);
        }

        public static uint ToRgbe(RealRgbColor color, bool isSigned)
        {
            float ar = Math.Abs(color.Red);
            float ag = Math.Abs(color.Green);
            float ab = Math.Abs(color.Blue);
            float max_component = Math.Max(ar, Math.Max(ag, ab));
            float r1 = 0, g1 = 0, b1 = 0, a1 = 0;

            if (max_component > 1.0e-32f)
            {
                int exponent;
                frexpf(max_component, out exponent);

                float exp = exponent;
                float m = exponent == 2 ? 4.0f : (float)Math.Pow(2.0f, exponent);
                exp *= 4.0f;

                float c = (float)Math.Pow(0.5f, 0.25f);
                for (m *= c; m >= max_component; m *= c)
                    --exp;

                float b = isSigned ? 127.0f : 255.0f;
                float s = b / (float)Math.Pow(2.0f, exp / 4.0f);
                r1 = Math.Min(Math.Max(ar * s, 0.0f), b);
                g1 = Math.Min(Math.Max(ag * s, 0.0f), b);
                b1 = Math.Min(Math.Max(ab * s, 0.0f), b);
                a1 = Math.Min(Math.Max(exp + 127.0f, 0.0f), 255.0f);
            }

            if (isSigned)
            {
                r1 *= Math.Sign(color.Red);
                g1 *= Math.Sign(color.Green);
                b1 *= Math.Sign(color.Blue);
                a1 -= 127.0f;
            }

            return (uint)((byte)Math.Floor(b1 + 0.5f) |
                         ((byte)Math.Floor(g1 + 0.5f) << 8) |
                         ((byte)Math.Floor(r1 + 0.5f) << 16) |
                         ((byte)a1 << 24));

            float frexpf(float x, out int pw2)
            {
                uint l = BitConverter.ToUInt32(BitConverter.GetBytes(x), 0);

                /* Find the exponent (power of 2) */
                int i = (int)((l >> 23) & 0xff);
                i -= 0x7e;
                pw2 = i;

                l &= 0x807fffff; /* strip all exponent bits */
                l |= 0x3f000000; /* mantissa between 0.5 and 1 */
                return BitConverter.ToSingle(BitConverter.GetBytes(l), 0);
            }
        }

        public static uint DecodePixel32(uint color, CacheVersion version, CachePlatform platform = CachePlatform.All)
        {
            uint c3 = color & 0xFF;
            uint c2 = (color >> 8) & 0xFF;
            uint c1 = (color >> 16) & 0xFF;
            uint c4 = (color >> 24) & 0xFF;

            if (CacheVersionDetection.IsInGen(CacheGeneration.HaloOnline, version) || platform == CachePlatform.MCC)
            {
                c1 = (c1 - 0x7F) & 0xFF;
                c2 = (c2 - 0x7F) & 0xFF;
                c3 = (c3 - 0x7F) & 0xFF;
                c4 = (c4 - 0x7F) & 0xFF;
            }

            return c1 | (c2 << 8) | (c3 << 16) | (c4 << 24);
        }

        public static uint EncodePixel32(uint color, CacheVersion version, CachePlatform platform = CachePlatform.All)
        {
            uint c3 = color & 0xFF;
            uint c2 = (color >> 8) & 0xFF;
            uint c1 = (color >> 16) & 0xFF;
            uint c4 = (color >> 24) & 0xFF;

            if (CacheVersionDetection.IsInGen(CacheGeneration.HaloOnline, version) || platform == CachePlatform.MCC)
            {
                c1 = (c1 + 0x7F) & 0xFF;
                c2 = (c2 + 0x7F) & 0xFF;
                c3 = (c3 + 0x7F) & 0xFF;
                c4 = (c4 + 0x7F) & 0xFF;
            }

            return c1 | (c2 << 8) | (c3 << 16) | (c4 << 24);
        }
    }
}
