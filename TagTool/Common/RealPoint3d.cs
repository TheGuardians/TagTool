using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Common
{
    public struct RealPoint3d : IEquatable<RealPoint3d>, IBlamType
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

        public static RealPoint3d operator +(RealPoint3d a, RealPoint3d b) =>
            new RealPoint3d(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static RealPoint3d operator +(RealPoint3d a, float b) =>
            new RealPoint3d(a.X + b, a.Y + b, a.Z + b);

        public static RealPoint3d operator +(float a, RealPoint3d b) =>
            new RealPoint3d(a + b.X, a + b.Y, a + b.Z);

        public static RealPoint3d operator -(RealPoint3d a, RealPoint3d b) =>
            new RealPoint3d(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static RealPoint3d operator -(RealPoint3d a, float b) =>
            new RealPoint3d(a.X - b, a.Y - b, a.Z - b);

        public static RealPoint3d operator -(float a, RealPoint3d b) =>
            new RealPoint3d(a - b.X, a - b.Y, a - b.Z);

        public static RealPoint3d operator *(RealPoint3d a, RealPoint3d b) =>
            new RealPoint3d(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

        public static RealPoint3d operator *(RealPoint3d a, float b) =>
            new RealPoint3d(a.X * b, a.Y * b, a.Z * b);

        public static RealPoint3d operator *(float a, RealPoint3d b) =>
            new RealPoint3d(a * b.X, a * b.Y, a * b.Z);

        public static RealPoint3d operator /(RealPoint3d a, RealPoint3d b) =>
            new RealPoint3d(a.X / b.X, a.Y / b.Y, a.Z / b.Z);

        public static RealPoint3d operator /(RealPoint3d a, float b) =>
            new RealPoint3d(a.X / b, a.Y / b, a.Z / b);

        public static RealPoint3d operator /(float a, RealPoint3d b) =>
            new RealPoint3d(a / b.X, a / b.Y, a / b.Z);

        public override int GetHashCode() =>
            13 * 17 + X.GetHashCode()
               * 17 + Y.GetHashCode()
               * 17 + Z.GetHashCode();

        public override string ToString() =>
            $"{{ X: {X}, Y: {Y}, Z: {Z} }}";

        public bool TryParse(GameCache cache, List<string> args, out IBlamType result, out string error)
        {
            result = null;
            if (args.Count != 3)
            {
                error = $"ERROR: {args.Count} arguments supplied; should be 3";
                return false;
            }
            else if (!float.TryParse(args[0], out float x))
            {
                error = $"ERROR: ERROR: ERROR: Unable to parse \"{args[0]}\" (x) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[1], out float y))
            {
                error = $"ERROR: Unable to parse \"{args[1]}\" (y) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[2], out float z))
            {
                error = $"ERROR: Unable to parse \"{args[2]}\" (z) as `float`.";
                return false;
            }
            else
            {
                result = new RealPoint3d(x, y, z);
                error = null;
                return true;
            }
        }

        public static float Distance(RealPoint3d a)
        {
            return (float)Math.Sqrt(a.X * a.X + a.Y * a.Y + a.Z * a.Z);
        }

        public float[] ToArray() => new[] { X, Y, Z };
    }
}
