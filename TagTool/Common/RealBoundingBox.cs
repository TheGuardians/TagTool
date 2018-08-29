using System;

namespace TagTool.Common
{
    public struct RealBoundingBox : IBlamType
    {
        public Bounds<float> XBounds, YBounds, ZBounds;

        public float Length =>
            (float)Math.Sqrt(
                Math.Pow(XBounds.Upper - XBounds.Lower, 2) +
                Math.Pow(YBounds.Upper - YBounds.Lower, 2) +
                Math.Pow(ZBounds.Upper - ZBounds.Lower, 2));
    }
}
