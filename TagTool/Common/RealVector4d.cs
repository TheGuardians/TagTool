using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Common
{
    public struct RealVector4d : IEquatable<RealVector4d>, IBlamType
    {
        public float I;
        public float J;
        public float K;
        public float W;

        public RealVector3d XYZ => new RealVector3d(I, J, K);


        public static RealVector4d Zero = new RealVector4d();

        public RealVector4d(RealVector3d v, float w = 0.0f)
        {
            I = v.I;
            J = v.J;
            K = v.K;
            W = w;
        }

        public RealVector4d(float i = 0, float j = 0, float k = 0, float w = 0)
        {
            I = i;
            J = j;
            K = k;
            W = w;
        }

        public static float GetLengthSquared(RealVector4d v)
        {
            return v.I * v.I + v.J * v.J + v.K * v.K + v.W * v.W;
        }

        public static float GetLength(RealVector4d v)
        {
            return (float)Math.Sqrt(GetLengthSquared(v));
        }

        public static float Dot(RealVector4d a, RealVector4d b)
        {
            return a.I * b.I + a.J * b.J + a.K * b.K + a.W * b.W;
        }

        public static RealVector4d Normalize(ref RealVector4d v)
        {
            float l = GetLength(v);
            if (l == 0.0f)
                return Zero;

            float s = 1.0f / l;
            v.I *= s;
            v.J *= s;
            v.K *= s;
            v.W *= s;
            return v;
        }

        public float[] ToArray() => new[] { I, J, K, W };

        public bool Equals(RealVector4d other) =>
            (I == other.I) &&
            (J == other.J) &&
            (K == other.K) &&
            (W == other.W);

        public override bool Equals(object obj) =>
            obj is RealVector4d ?
                Equals((RealVector4d)obj) :
            false;

        public static bool operator ==(RealVector4d a, RealVector4d b) =>
            a.Equals(b);

        public static bool operator !=(RealVector4d a, RealVector4d b) =>
            !a.Equals(b);

        public static RealVector4d operator +(RealVector4d a, RealVector4d b) =>
                    new RealVector4d(a.I + b.I, a.J + b.J, a.K + b.K, a.W + b.W);

        public static RealVector4d operator +(RealVector4d a, float b) =>
            new RealVector4d(a.I + b, a.J + b, a.K + b, a.W + b);

        public static RealVector4d operator +(float a, RealVector4d b) =>
            new RealVector4d(a + b.I, a + b.J, a + b.K, a + b.W);

        public static RealVector4d operator -(RealVector4d a, RealVector4d b) =>
            new RealVector4d(a.I - b.I, a.J - b.J, a.K - b.K, a.W - b.W);

        public static RealVector4d operator -(RealVector4d a, float b) =>
            new RealVector4d(a.I - b, a.J - b, a.K - b);

        public static RealVector4d operator -(float a, RealVector4d b) =>
            new RealVector4d(a - b.I, a - b.J, a - b.K, a - b.W);

        public static RealVector4d operator *(RealVector4d a, RealVector4d b) =>
            new RealVector4d(a.I * b.I, a.J * b.J, a.K * b.K, a.W * b.W);

        public static RealVector4d operator *(RealVector4d a, float b) =>
            new RealVector4d(a.I * b, a.J * b, a.K * b, a.W * b);

        public static RealVector4d operator *(float a, RealVector4d b) =>
            new RealVector4d(a * b.I, a * b.J, a * b.K, a * b.W);

        public static RealVector4d operator /(RealVector4d a, RealVector4d b) =>
            new RealVector4d(a.I / b.I, a.J / b.J, a.K / b.K, a.W / b.W);

        public static RealVector4d operator /(RealVector4d a, float b) =>
            new RealVector4d(a.I / b, a.J / b, a.K / b, a.W / b);

        public static RealVector4d operator /(float a, RealVector4d b) =>
            new RealVector4d(a / b.I, a / b.J, a / b.K, a / b.W);

        public override int GetHashCode() =>
            13 * 17 + I.GetHashCode()
               * 17 + J.GetHashCode()
               * 17 + K.GetHashCode()
               * 17 + W.GetHashCode();

        public override string ToString() =>
            $"{{ I: {I}, J: {J}, K: {K}, W: {W} }}";

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
            else if (!float.TryParse(args[3], out float w))
            {
                error = $"Unable to parse \"{args[2]}\" (k) as `float`.";
                return false;
            }
            else
            {
                result = new RealVector4d(i, j, k, w);
                error = null;
                return true;
            }
        }
    }

}
