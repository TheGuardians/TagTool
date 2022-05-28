using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Common
{
    public struct RealVector2d : IEquatable<RealVector2d>, IBlamType
	{
        public float I { get; set; }
        public float J { get; set; }

        public RealPoint2d XY => new RealPoint2d(I, J);

        public RealVector2d(float x, float y)
        {
            I = x;
            J = y;
        }

        public RealVector2d(float x)
        {
            I = x;
            J = x;
        }

        public RealVector2d(float[] elements) :
            this(elements[0], elements[1])
        {
        }

        public float[] ToArray() => new[] { I, J };

        public bool Equals(RealVector2d other) =>
            (I == other.I) &&
            (J == other.J);

        public override bool Equals(object obj) =>
            obj is RealVector2d ?
                Equals((RealVector2d)obj) :
            false;

        public static bool operator ==(RealVector2d a, RealVector2d b) =>
            a.Equals(b);

        public static bool operator !=(RealVector2d a, RealVector2d b) =>
            !a.Equals(b);

        public static RealVector2d operator +(RealVector2d a, float b)
        {
            return new RealVector2d(a.I + b, a.J + b);
        }

        public static RealVector2d operator -(RealVector2d a, float b)
        {
            return new RealVector2d(a.I - b, a.J - b);
        }

        public static RealVector2d operator *(RealVector2d a, float b)
        {
            return new RealVector2d(a.I * b, a.J * b);
        }

        public static RealVector2d operator *(RealVector2d a, RealVector2d b)
        {
            return new RealVector2d(a.I * b.I, a.J * b.J);
        }

        public override int GetHashCode() =>
            13 * 17 + I.GetHashCode()
               * 17 + J.GetHashCode();

        public override string ToString() =>
            $"{{ I: {I}, J: {J} }}";

        public static RealVector2d Frac(RealVector2d a)
        {
            RealVector2d result = new RealVector2d
            {
                I = a.I - (float)Math.Floor(a.I),
                J = a.J - (float)Math.Floor(a.J)
            };

            return result;
        }

        public float Dot() => I * I + J * J;

        public bool TryParse(GameCache cache, List<string> args, out IBlamType result, out string error)
        {
            result = null;
            if (args.Count != 2)
            {
                error = $"{args.Count} arguments supplied; should be 2";
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
            else
            {
                result = new RealVector2d(i, j);
                error = null;
                return true;
            }
        }
    }
}
