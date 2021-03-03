using System.Collections.Generic;
using System.IO;
using TagTool.Cache;

namespace TagTool.Common
{
    public struct RealMatrix4x3 : IBlamType
	{
        public float m11 { get; set; }
        public float m12 { get; set; }
        public float m13 { get; set; }
        public float m21 { get; set; }
        public float m22 { get; set; }
        public float m23 { get; set; }
        public float m31 { get; set; }
        public float m32 { get; set; }
        public float m33 { get; set; }
        public float m41 { get; set; }
        public float m42 { get; set; }
        public float m43 { get; set; }

        public bool IsIdentity =>
            (m11 == 1.0f && m12 == 0.0f && m13 == 0.0f &&
             m21 == 0.0f && m22 == 1.0f && m23 == 0.0f &&
             m31 == 0.0f && m32 == 0.0f && m33 == 1.0f &&
             m41 == 0.0f && m42 == 0.0f && m43 == 0.0f);

        public static RealMatrix4x3 Identity =>
            new RealMatrix4x3(
                1.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 0.0f);

        public bool Equals(RealMatrix4x3 M) =>
            (m11 == M.m11 &&
             m12 == M.m12 &&
             m13 == M.m13 &&
             m21 == M.m21 &&
             m22 == M.m22 &&
             m23 == M.m23 &&
             m31 == M.m31 &&
             m32 == M.m32 &&
             m33 == M.m33 &&
             m41 == M.m41 &&
             m42 == M.m42 &&
             m43 == M.m43);

        public RealMatrix4x3(
            float M11, float M12, float M13,
            float M21, float M22, float M23,
            float M31, float M32, float M33,
            float M41, float M42, float M43)
        {
            m11 = M11; m12 = M12; m13 = M13;
            m21 = M21; m22 = M22; m23 = M23;
            m31 = M31; m32 = M32; m33 = M33;
            m41 = M41; m42 = M42; m43 = M43;
        }

        public static RealMatrix4x3 operator *(RealMatrix4x3 matrix1, RealMatrix4x3 matrix2)
        {
            if (matrix1.IsIdentity) return matrix2;
            if (matrix2.IsIdentity) return matrix1;

            return new RealMatrix4x3(
                matrix1.m11 * matrix2.m11 + matrix1.m12 * matrix2.m21 +
                matrix1.m13 * matrix2.m31 + 0 * matrix2.m41,
                matrix1.m11 * matrix2.m12 + matrix1.m12 * matrix2.m22 +
                matrix1.m13 * matrix2.m32 + 0 * matrix2.m42,
                matrix1.m11 * matrix2.m13 + matrix1.m12 * matrix2.m23 +
                matrix1.m13 * matrix2.m33 + 0 * matrix2.m43,
                matrix1.m21 * matrix2.m11 + matrix1.m22 * matrix2.m21 +
                matrix1.m23 * matrix2.m31 + 0 * matrix2.m41,
                matrix1.m21 * matrix2.m12 + matrix1.m22 * matrix2.m22 +
                matrix1.m23 * matrix2.m32 + 0 * matrix2.m42,
                matrix1.m21 * matrix2.m13 + matrix1.m22 * matrix2.m23 +
                matrix1.m23 * matrix2.m33 + 0 * matrix2.m43,
                matrix1.m31 * matrix2.m11 + matrix1.m32 * matrix2.m21 +
                matrix1.m33 * matrix2.m31 + 0 * matrix2.m41,
                matrix1.m31 * matrix2.m12 + matrix1.m32 * matrix2.m22 +
                matrix1.m33 * matrix2.m32 + 0 * matrix2.m42,
                matrix1.m31 * matrix2.m13 + matrix1.m32 * matrix2.m23 +
                matrix1.m33 * matrix2.m33 + 0 * matrix2.m43,
                matrix1.m41 * matrix2.m11 + matrix1.m42 * matrix2.m21 +
                matrix1.m43 * matrix2.m31 + 1 * matrix2.m41,
                matrix1.m41 * matrix2.m12 + matrix1.m42 * matrix2.m22 +
                matrix1.m43 * matrix2.m32 + 1 * matrix2.m42,
                matrix1.m41 * matrix2.m13 + matrix1.m42 * matrix2.m23 +
                matrix1.m43 * matrix2.m33 + 1 * matrix2.m43);
        }

        public static RealMatrix4x3 Read(BinaryReader reader) =>
            new RealMatrix4x3(
                reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

        public static RealMatrix4x3 FromBounds(Bounds<float> xBounds, Bounds<float> yBounds, Bounds<float> zBounds)
        {
            var bbXLength = xBounds.Upper - xBounds.Lower;
            var bbYLength = yBounds.Upper - yBounds.Lower;
            var bbZLength = zBounds.Upper - zBounds.Lower;

            return new RealMatrix4x3(
                bbXLength != 0.0f ? bbXLength : 1.0f, 0.0f, 0.0f,
                0.0f, bbYLength != 0.0f ? bbYLength : 1.0f, 0.0f,
                0.0f, 0.0f, bbZLength != 0.0f ? bbZLength : 1.0f,
                bbXLength, bbYLength, bbZLength);
        }

        public static RealMatrix4x3 FromBounds(RealPoint3d lower, RealPoint3d upper)
        {
            var bbXLength = upper.X - lower.X;
            var bbYLength = upper.Y - lower.Y;
            var bbZLength = upper.Z - lower.Z;

            return new RealMatrix4x3(
                bbXLength != 0.0f ? bbXLength : 1.0f, 0.0f, 0.0f,
                0.0f, bbYLength != 0.0f ? bbYLength : 1.0f, 0.0f,
                0.0f, 0.0f, bbZLength != 0.0f ? bbZLength : 1.0f,
                bbXLength, bbYLength, bbZLength);
        }

        public override string ToString() => $"{{ " +
            $"{m11}, {m12}, {m13} |" +
            $"{m21}, {m22}, {m23} |" +
            $"{m31}, {m32}, {m33} |" +
            $"{m41}, {m42}, {m43} }}";

        public bool TryParse(GameCache cache, List<string> args, out IBlamType result, out string error)
        {
            result = null;

            if (args.Count != 12)
            {
                error = $"ERROR: {args.Count} arguments supplied; should be 12";
                return false;
            }
            else if (!float.TryParse(args[0], out float M11))
            {
                error = $"ERROR: Unable to parse \"{args[0]}\" (M11) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[1], out float M12))
            {
                error = $"ERROR: Unable to parse \"{args[1]}\" (M12) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[2], out float M13))
            {
                error = $"ERROR: Unable to parse \"{args[2]}\" (M13) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[3], out float M21))
            {
                error = $"ERROR: Unable to parse \"{args[3]}\" (M21) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[4], out float M22))
            {
                error = $"ERROR: Unable to parse \"{args[4]}\" (M22) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[5], out float M23))
            {
                error = $"ERROR: Unable to parse \"{args[5]}\" (M23) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[6], out float M31))
            {
                error = $"ERROR: Unable to parse \"{args[6]}\" (M31) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[7], out float M32))
            {
                error = $"ERROR: Unable to parse \"{args[7]}\" (M32) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[8], out float M33))
            {
                error = $"ERROR: Unable to parse \"{args[8]}\" (M33) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[9], out float M41))
            {
                error = $"ERROR: Unable to parse \"{args[9]}\" (M41) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[10], out float M42))
            {
                error = $"ERROR: Unable to parse \"{args[10]}\" (M42) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[11], out float M43))
            {
                error = $"ERROR: Unable to parse \"{args[11]}\" (M43) as `float`.";
                return false;
            }
            else
            {
                result = new RealMatrix4x3(
                    M11, M12, M13,
                    M21, M22, M23,
                    M31, M32, M33,
                    M41, M42, M43);

                error = null;
                return true;
            }
        }
    }
}
