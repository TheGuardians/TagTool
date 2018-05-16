using System;

namespace TagTool.Common
{
    public struct RealPoint3d : IEquatable<RealPoint3d>
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public RealPoint3d(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public bool Equals(RealPoint3d other) =>
            (X == other.X) &&
            (Y == other.Y) &&
            (Z == other.Z);

        public override bool Equals(object obj) =>
            obj is RealPoint3d ?
                Equals((RealPoint3d)obj) :
            false;

        public static bool operator ==(RealPoint3d a, RealPoint3d b) =>
            a.Equals(b);

        public static bool operator !=(RealPoint3d a, RealPoint3d b) =>
            !a.Equals(b);

        public override int GetHashCode() =>
            13 * 17 + X.GetHashCode()
               * 17 + Y.GetHashCode()
               * 17 + Z.GetHashCode();

        public override string ToString() =>
            $"{{ X: {X}, Y: {Y}, Z: {Z} }}";

        public float[] ToArray() => new[] { X, Y, Z };
    }
}
