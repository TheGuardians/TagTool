using System;

namespace TagTool.Common
{
    /// <summary>
    /// A 2-dimensional plane in real space.
    /// </summary>
    public struct RealPlane2d : IEquatable<RealPlane2d>
    {
        /// <summary>
        /// The direction normal vector of the plane.
        /// </summary>
        public RealVector2d Normal { get; set; }

        /// <summary>
        /// The distance along the plane's normal vector from the origin.
        /// </summary>
        public float Distance { get; set; }

        /// <summary>
        /// The I component of the normal vector of the plane.
        /// </summary>
        public float I
        {
            get { return Normal.I; }
            set { Normal = new RealVector2d(value, Normal.J); }
        }

        /// <summary>
        /// The J component of the normal vector of the plane.
        /// </summary>
        public float J
        {
            get { return Normal.J; }
            set { Normal = new RealVector2d(Normal.I, value); }
        }

        /// <summary>
        /// The distance along the plane's normal vector from the origin.
        /// </summary>
        public float D
        {
            get { return Distance; }
            set { Distance = value; }
        }

        /// <summary>
        /// Creates a new <see cref="RealPlane2d"/>.
        /// </summary>
        /// <param name="normal">The normal vector of the plane.</param>
        /// <param name="distance">The distance of the plane.</param>
        public RealPlane2d(RealVector2d normal, float distance)
        {
            Normal = normal;
            Distance = distance;
        }

        /// <summary>
        /// Creates a new <see cref="RealPlane2d"/>.
        /// </summary>
        /// <param name="i">The I component of the normal vector of the plane.</param>
        /// <param name="j">The J component of the normal vector of the plane.</param>
        /// <param name="d">The distance of the plane.</param>
        public RealPlane2d(float i, float j, float d) :
            this(new RealVector2d(i, j), d)
        {
        }

        /// <summary>
        /// Determines if the current instance is equal to another <see cref="RealPlane2d"/>.
        /// </summary>
        /// <param name="other">The other plane.</param>
        /// <returns></returns>
        public bool Equals(RealPlane2d other) =>
            (Normal == other.Normal) &&
            (Distance == other.Distance);

        public override bool Equals(object obj) =>
            obj is RealPlane2d ?
                Equals((RealPlane2d)obj) :
            false;

        public static bool operator ==(RealPlane2d a, RealPlane2d b) =>
            a.Equals(b);

        public static bool operator !=(RealPlane2d a, RealPlane2d b) =>
            !a.Equals(b);

        public override int GetHashCode() =>
            13 * 17 + Normal.GetHashCode()
               * 17 + Distance.GetHashCode();

        public override string ToString() =>
            $"{{ Normal: {Normal}, Distance: {Distance} }}";
    }
}
