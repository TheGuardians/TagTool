using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Common
{
    public struct Rectangle2d : IEquatable<Rectangle2d>, IBlamType
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

        public bool TryParse(GameCache cache, List<string> args, out IBlamType result, out string error)
        {
            result = null;
            if (args.Count != 4)
            {
                error = $"{args.Count} arguments supplied; should be 4";
                return false;
            }
            else if (!short.TryParse(args[0], out short top))
            {
                error = $"Unable to parse \"{args[0]}\" (top) as `short`.";
                return false;
            }
            else if (!short.TryParse(args[1], out short left))
            {
                error = $"Unable to parse \"{args[1]}\" (left) as `short`.";
                return false;
            }
            else if (!short.TryParse(args[2], out short bottom))
            {
                error = $"Unable to parse \"{args[2]}\" (bottom) as `short`.";
                return false;
            }
            else if (!short.TryParse(args[3], out short right))
            {
                error = $"Unable to parse \"{args[3]}\" (right) as `short`.";
                return false;
            }
            else
            {
                result = new Rectangle2d(top, left, bottom, right);
                error = null;
                return true;
            }
        }
    }
}
