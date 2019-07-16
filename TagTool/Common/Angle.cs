using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Common
{
    /// <summary>
    /// An angle value.
    /// </summary>
    public struct Angle : IEquatable<Angle>, IComparable<Angle>, IBlamType
    {
        /// <summary>
        /// The value used to convert between degrees and radians.
        /// </summary>
        public const float UnitConversion = (float)(180.0 / Math.PI);

		/// <summary>
		/// Gets the angle's value in radians.
		/// THERE BE DRAGONS HERE: Do not set this manually outside of serialization.
		/// </summary>
		public float Radians { get; set; }

        /// <summary>
        /// Gets the angle's value in degrees.
        /// </summary>
        public float Degrees =>
            Radians * UnitConversion;

        /// <summary>
        /// Creates a new angle from radians.
        /// </summary>
        /// <param name="radians">The radians of the angle.</param>
        /// <returns>The angle that was created.</returns>
        public static Angle FromRadians(float radians) =>
            new Angle(radians);

        /// <summary>
        /// Creates a new angle from degrees.
        /// </summary>
        /// <param name="degrees">The degrees of the angle.</param>
        /// <returns>The angle that was created.</returns>
        public static Angle FromDegrees(float degrees) =>
            new Angle(degrees / UnitConversion);

        private Angle(float radians)
        {
            Radians = radians;
        }

        public bool Equals(Angle other) =>
            Radians == other.Radians;

        public override bool Equals(object obj) =>
            obj is Angle ?
                Equals((Angle)obj) :
            false;

        public static bool operator ==(Angle a, Angle b) =>
            a.Equals(b);

        public static bool operator !=(Angle a, Angle b) =>
            !a.Equals(b);

        public override int GetHashCode() =>
            Radians.GetHashCode();

        public int CompareTo(Angle other) =>
            Radians.CompareTo(other.Radians);

        public override string ToString() =>
            $"{{ Degrees: {Degrees}, Radians: {Radians} }}";

        public bool TryParse(HaloOnlineCacheContext cacheContext, List<string> args, out IBlamType result, out string error)
        {
            result = null;
            if (args.Count != 1)
            {
                error = $"{args.Count} arguments supplied; should be 1";
                return false;
            }
            else if (!float.TryParse(args[0], out float _value))
            {
                error = $"Unable to parse \"{args[0]}\" as `float`.";
                return false;
            }
            else
            {
                result = Angle.FromDegrees(_value);
                error = null;
                return true;
            }
        }
    }
}
