using System;

namespace TagTool.Common
{
    public struct Rectangle2d : IEquatable<Rectangle2d>
    {
        public short Top { get; set; }
        public short Left { get; set; }
        public short Bottom { get; set; }
        public short Right { get; set; }

        public Rectangle2d(short top, short left, short bottom, short right)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        public bool Equals(Rectangle2d other) =>
            (Top == other.Top) &&
            (Left == other.Left) &&
            (Bottom == other.Bottom) &&
            (Right == other.Right);

        public override bool Equals(object obj) =>
            obj is Rectangle2d ?
                Equals((Rectangle2d)obj) :
            false;

        public static bool operator ==(Rectangle2d a, Rectangle2d b) =>
            a.Equals(b);

        public static bool operator !=(Rectangle2d a, Rectangle2d b) =>
            !a.Equals(b);

        public override int GetHashCode() =>
            13 * 17 + Top.GetHashCode()
               * 17 + Left.GetHashCode()
               * 17 + Bottom.GetHashCode()
               * 17 + Right.GetHashCode();

        public override string ToString() =>
            $"{{ Top: {Top}, Left: {Left}, Bottom: {Bottom}, Right: {Right} }}";
    }
}
