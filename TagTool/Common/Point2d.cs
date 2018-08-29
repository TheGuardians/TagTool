using System;

namespace TagTool.Common
{
    public struct Point2d : IEquatable<Point2d>, IBlamType
	{
        public short X { get; set; }
        public short Y { get; set; }

        public Point2d(short x, short y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Point2d other) =>
            (X == other.X) &&
            (Y == other.Y);

        public override bool Equals(object obj) =>
            obj is Point2d ?
                Equals((Point2d)obj) :
            false;

        public static bool operator ==(Point2d a, Point2d b) =>
            a.Equals(b);

        public static bool operator !=(Point2d a, Point2d b) =>
            !a.Equals(b);

        public override int GetHashCode() =>
            13 * 17 + X.GetHashCode()
               * 17 + Y.GetHashCode();

        public override string ToString() =>
            $"{{ X: {X}, Y: {Y} }}";
    }
}
