using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Common
{
    public struct RealVector3d : IEquatable<RealVector3d>, IBlamType
	{
        public float I { get; set; }
        public float J { get; set; }
        public float K { get; set; }

        public RealVector3d(float x, float y, float z)
        {
            I = x;
            J = y;
            K = z;
        }

        public RealVector3d(RealQuaternion vector) : this(vector.I, vector.J, vector.K) { }

        public RealVector3d(float[] elements) :
            this(elements[0], elements[1], elements[2])
        {
        }

        public float[] ToArray() => new[] { I, J, K };

        public bool Equals(RealVector3d other) =>
            (I == other.I) &&
            (J == other.J) &&
            (K == other.K);

        public override bool Equals(object obj) =>
            obj is RealVector3d ?
                Equals((RealVector3d)obj) :
            false;

        public static bool operator ==(RealVector3d a, RealVector3d b) =>
            a.Equals(b);

        public static bool operator !=(RealVector3d a, RealVector3d b) =>
            !a.Equals(b);

        public static RealVector3d operator +(RealVector3d a, RealVector3d b) =>
                    new RealVector3d(a.I + b.I, a.J + b.J, a.K + b.K);

        public static RealVector3d operator +(RealVector3d a, float b) =>
            new RealVector3d(a.I + b, a.J + b, a.K + b);

        public static RealVector3d operator +(float a, RealVector3d b) =>
            new RealVector3d(a + b.I, a + b.J, a + b.K);

        public static RealVector3d operator -(RealVector3d a, RealVector3d b) =>
            new RealVector3d(a.I - b.I, a.J - b.J, a.K - b.K);

        public static RealVector3d operator -(RealVector3d a, float b) =>
            new RealVector3d(a.I - b, a.J - b, a.K - b);

        public static RealVector3d operator -(float a, RealVector3d b) =>
            new RealVector3d(a - b.I, a - b.J, a - b.K);

        public static RealVector3d operator *(RealVector3d a, RealVector3d b) =>
            new RealVector3d(a.I * b.I, a.J * b.J, a.K * b.K);

        public static RealVector3d operator *(RealVector3d a, float b) =>
            new RealVector3d(a.I * b, a.J * b, a.K * b);

        public static RealVector3d operator *(float a, RealVector3d b) =>
            new RealVector3d(a * b.I, a * b.J, a * b.K);

        public static RealVector3d operator /(RealVector3d a, RealVector3d b) =>
            new RealVector3d(a.I / b.I, a.J / b.J, a.K / b.K);

        public static RealVector3d operator /(RealVector3d a, float b) =>
            new RealVector3d(a.I / b, a.J / b, a.K / b);

        public static RealVector3d operator /(float a, RealVector3d b) =>
            new RealVector3d(a / b.I, a / b.J, a / b.K);

        public override int GetHashCode() =>
            13 * 17 + I.GetHashCode()
               * 17 + J.GetHashCode()
               * 17 + K.GetHashCode();

        public override string ToString() =>
            $"{{ I: {I}, J: {J}, K: {K} }}";

        public bool TryParse(GameCache cache, List<string> args, out IBlamType result, out string error)
        {
            result = null;
            if (args.Count != 3)
            {
                error = $"{args.Count} arguments supplied; should be 3";
                return false;
            }
            else if (!float.TryParse(args[0], out float i))
            {
                error = $"Unable to parse \"{args[0]}\" (i) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[1], out float j))
            {
                error = $"Unable to parse \"{args[1]}\" (j) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[2], out float k))
            {
                error = $"Unable to parse \"{args[2]}\" (k) as `float`.";
                return false;
            }
            else
            {
                result = new RealVector3d(i, j, k);
                error = null;
                return true;
            }
        }

        public static RealVector3d CrossProduct(RealVector3d a, RealVector3d b)
        {
            RealVector3d result = new RealVector3d()
            {
                I = a.J * b.K - a.K * b.J,
                J = a.K * b.I - a.I * b.K,
                K = a.I * b.J - a.J * b.I
            };
            return Normalize(result);
        }

        public static float Magnitude(RealVector3d a)
        {
            return a.I * a.I + a.J * a.J + a.K * a.K;
        }

        public static float Norm(RealVector3d a)
        {
            return (float)Math.Sqrt(a.I*a.I + a.J*a.J + a.K*a.K);
        }

        public static RealVector3d Normalize(RealVector3d a)
        {
            return 1 / Norm(a) * a;
        }
    }
}
