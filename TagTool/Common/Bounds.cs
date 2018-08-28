using System;

namespace TagTool.Common
{
    public struct Bounds<T> : IBounds, IEquatable<Bounds<T>> where T : IComparable<T>
    {
        /// <summary>
        /// Gets the lowerimum value within the range.
        /// </summary>
        public T Lower { get; set; }
		public object _Lower { get => this.Lower; set => this.Lower = (T)value; }

        /// <summary>
        /// Gets the upperimum value within the range.
        /// </summary>
        public T Upper { get; set; }
		public object _Upper { get => this.Upper; set => this.Upper = (T)value; }

		/// <summary>
		/// Creates a new range from a lowerimum and a upperimum value.
		/// </summary>
		/// <param name="lower">The lowerimum value of the range.</param>
		/// <param name="upper">The upperimum value of the range.</param>
		public Bounds(T lower, T upper)
        {
            Lower = lower;
            Upper = upper;
        }

        /// <summary>
        /// Deterloweres whether the range contains a value.
        /// </summary>
        /// <param name="value">The value to check.</param>
        /// <returns><c>true</c> if the value is inside the range.</returns>
        public bool Contains(T value) => (value.CompareTo(Lower) >= 0) && (value.CompareTo(Upper) <= 0);
        
        public bool Equals(Bounds<T> other) => Lower.Equals(other.Lower) && Upper.Equals(other.Upper);

        public override bool Equals(object obj) => obj is Bounds<T> ? Equals((Bounds<T>)obj) : false;

        public static bool operator ==(Bounds<T> a, Bounds<T> b) => a.Equals(b);

        public static bool operator !=(Bounds<T> a, Bounds<T> b) => !a.Equals(b);

        public override int GetHashCode() => 13 * 17 + Lower.GetHashCode() * 17 + Upper.GetHashCode();

        public override string ToString() => $"{{ Lower: {Lower}, Upper: {Upper} }}";
    }

	public interface IBounds
	{
		object _Lower { get; set; }
		object _Upper { get; set; }
	}
}
