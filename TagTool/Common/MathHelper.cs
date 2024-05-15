using System;

namespace TagTool.Common
{
    public static class MathHelper
    {
        public static bool SphereIntersectsRectangle3d(RealPoint3d center, float radius, RealRectangle3d rectangle)
        {
            var d = new RealVector3d();

            if (rectangle.X1 < center.X)
                d.I = center.X - rectangle.X1;
            else
                d.I = Math.Max(0.0f, rectangle.X0 - center.X);
            
            if (rectangle.Y1 < center.Y)
                d.J = center.Y - rectangle.Y1;
            else
                d.J = Math.Max(0.0f, rectangle.Y0 - center.Y);
            
            if (rectangle.Z1 < center.Z)
                d.K = center.Z - rectangle.Z1;
            else
                d.K = Math.Max(0.0f, rectangle.Z0 - center.Z);
            
            return (radius * radius) >= RealVector3d.Magnitude(d);
        }

        public static float Clamp(float value, float min, float max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        public static RealRgbColor Clamp(RealRgbColor value, float min, float max)
        {
            return new RealRgbColor
            {
                Red = Math.Min(Math.Max(value.Red, min), max),
                Green = Math.Min(Math.Max(value.Green, min), max),
                Blue = Math.Min(Math.Max(value.Blue, min), max),
            };
        }

        public static float Lerp(float a, float b, float s)
        {
            return ((1.0f - s) * a) + (s * b);
        }
    }
}
