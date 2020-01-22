using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;

namespace TagTool.Common
{
    public struct RealBoundingBox
    {
        public Bounds<float> XBounds, YBounds, ZBounds;

        public RealBoundingBox(Bounds<float> xBounds, Bounds<float> yBounds, Bounds<float> zBounds)
        {
            XBounds = xBounds;
            YBounds = yBounds;
            ZBounds = zBounds;
        }

        public RealBoundingBox(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
        {
            XBounds = new Bounds<float>(minX, maxX);
            YBounds = new Bounds<float>(minY, maxY);
            ZBounds = new Bounds<float>(minZ, maxZ);
        }

        public float Length =>
            (float)Math.Sqrt(
                Math.Pow(XBounds.Upper - XBounds.Lower, 2) +
                Math.Pow(YBounds.Upper - YBounds.Lower, 2) +
                Math.Pow(ZBounds.Upper - ZBounds.Lower, 2));

        public override string ToString() => $"{{" +
            $"{{X: {XBounds.Lower}, {XBounds.Upper}}}, " +
            $"{{Y: {YBounds.Lower}, {YBounds.Upper}}}, " +
            $"{{Z: {ZBounds.Lower}, {ZBounds.Upper}}}}} ";

    }
}
