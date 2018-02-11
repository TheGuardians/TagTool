using System;

namespace TagTool.Common
{
    public struct RealPoint2d : IEquatable<RealPoint2d>
    {
        public float X { get; set; }
        public float Y { get; set; }

        public RealPoint2d(float x, float y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(RealPoint2d other) =>
            (X == other.X) &&
            (Y == other.Y);

        public override bool Equals(object obj) =>
            obj is RealPoint2d ?
                Equals((RealPoint2d)obj) :
            false;

        public static bool operator ==(RealPoint2d a, RealPoint2d b) =>
            a.Equals(b);

        public static bool operator !=(RealPoint2d a, RealPoint2d b) =>
            !a.Equals(b);

        public override int GetHashCode() =>
            13 * 17 + X.GetHashCode()
               * 17 + Y.GetHashCode();

        public override string ToString() =>
            $"{{ X: {X}, Y: {Y} }}";
    }
}
