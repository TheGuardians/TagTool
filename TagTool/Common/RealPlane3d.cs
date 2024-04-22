using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Common
{
    /// <summary>
    /// A 2-dimensional plane in real space.
    /// </summary>
    public struct RealPlane3d : IEquatable<RealPlane3d>, IBlamType
	{
        /// <summary>
        /// The direction normal vector of the plane.
        /// </summary>
        public RealVector3d Normal { get; set; }

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
            set { Normal = new RealVector3d(value, Normal.J, Normal.K); }
        }

        /// <summary>
        /// The J component of the normal vector of the plane.
        /// </summary>
        public float J
        {
            get { return Normal.J; }
            set { Normal = new RealVector3d(Normal.I, value, Normal.K); }
        }

        /// <summary>
        /// The K component of the normal vector of the plane.
        /// </summary>
        public float K
        {
            get { return Normal.K; }
            set { Normal = new RealVector3d(Normal.I, Normal.J, value); }
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
        /// Creates a new <see cref="RealPlane3d"/>.
        /// </summary>
        /// <param name="normal">The normal vector of the plane.</param>
        /// <param name="distance">The distance of the plane.</param>
        public RealPlane3d(RealVector3d normal, float distance)
        {
            Normal = normal;
            Distance = distance;
        }

        /// <summary>
        /// Creates a new <see cref="RealPlane3d"/>.
        /// </summary>
        /// <param name="i">The I component of the normal vector of the plane.</param>
        /// <param name="j">The J component of the normal vector of the plane.</param>
        /// <param name="k">The K component of the normal vector of the plane.</param>
        /// <param name="d">The distance of the plane.</param>
        public RealPlane3d(float i, float j, float k, float d) :
            this(new RealVector3d(i, j, k), d)
        {
        }

        /// <summary>
        /// Determines if the current instance is equal to another <see cref="RealPlane3d"/>.
        /// </summary>
        /// <param name="other">The other plane.</param>
        /// <returns></returns>
        public bool Equals(RealPlane3d other) =>
            (Normal == other.Normal) &&
            (Distance == other.Distance);

        public override bool Equals(object obj) =>
            obj is RealPlane3d ?
                Equals((RealPlane3d)obj) :
            false;

        public static bool operator ==(RealPlane3d a, RealPlane3d b) =>
            a.Equals(b);

        public static bool operator !=(RealPlane3d a, RealPlane3d b) =>
            !a.Equals(b);

        public override int GetHashCode() =>
            13 * 17 + Normal.GetHashCode()
               * 17 + Distance.GetHashCode();

        public override string ToString() =>
            $"{{ Normal: {Normal}, Distance: {Distance} }}";

        public bool TryParse(GameCache cache, List<string> args, out IBlamType result, out string error)
        {
            result = null;
            if (args.Count != 4)
            {
                error = $"{args.Count} arguments supplied; should be 4";
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
            else if (!float.TryParse(args[3], out float d))
            {
                error = $"Unable to parse \"{args[3]}\" (d) as `float`.";
                return false;
            }
            else
            {
                result = new RealPlane3d(i, j, k, d);
                error = null;
                return true;
            }
        }
    }
}
