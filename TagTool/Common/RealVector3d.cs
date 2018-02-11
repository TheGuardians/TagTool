using System;

namespace TagTool.Common
{
    public struct RealVector3d : IEquatable<RealVector3d>
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

        public override int GetHashCode() =>
            13 * 17 + I.GetHashCode()
               * 17 + J.GetHashCode()
               * 17 + K.GetHashCode();

        public override string ToString() =>
            $"{{ I: {I}, J: {J}, K: {K} }}";
    }
}
