using System;

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

        public override int GetHashCode() =>
            13 * 17 + I.GetHashCode()
               * 17 + J.GetHashCode();

        public override string ToString() =>
            $"{{ I: {I}, J: {J} }}";
    }
}
