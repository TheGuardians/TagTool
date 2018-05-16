using System;
using System.Collections.Generic;
using System.Linq;

namespace TagTool.Common
{
    public struct RealQuaternion
    {
        public float I { get; }
        public float J { get; }
        public float K { get; }
        public float W { get; }

        public RealPoint2d XY => new RealPoint2d(I, J);

        public RealPoint3d XYZ => new RealPoint3d(I, J, K);

        public RealVector2d IJ => new RealVector2d(I, J);

        public RealVector3d IJK => new RealVector3d(I, J, K);

        public float LengthSquared => (I * I) + (J * J) + (K * K) + (W * W);

        public float Length => (float)Math.Sqrt(LengthSquared);

        public RealQuaternion(float i)
            : this(i, 0.0f, 0.0f, 0.0f)
        {
        }

        public RealQuaternion(float i, float j)
            : this(i, j, 0.0f, 0.0f)
        {
        }

        public RealQuaternion(float i, float j, float k)
            : this(i, j, k, 0.0f)
        {
        }

        public RealQuaternion(float i, float j, float k, float w)
        {
            I = i;
            J = j;
            K = k;
            W = w;
        }
        
        public RealQuaternion(RealVector2d ij, float k, float w) :
            this(ij.I, ij.J, k, w)
        {
        }
        
        public RealQuaternion(RealVector3d ijk, float w) :
            this(ijk.I, ijk.J, ijk.K, w)
        {
        }
        
        public RealQuaternion(IEnumerable<float> enumerable)
        {
            var components = enumerable.ToArray();
            I = components.Count() > 0 ? components[0] : 0.0f;
            J = components.Count() > 1 ? components[1] : 0.0f;
            K = components.Count() > 2 ? components[2] : 0.0f;
            W = components.Count() > 3 ? components[3] : 0.0f;
        }
        
        public float[] ToArray() => new[] { I, J, K, W };
        
        public RealQuaternion Normalize() => this / Length;
        
        public static float Dot(RealQuaternion lhs, RealQuaternion rhs) =>
            (lhs.I * rhs.I) +
            (lhs.J * rhs.J) +
            (lhs.K * rhs.K) +
            (lhs.W * rhs.W);
        
        public static float DistanceSquared(RealQuaternion lhs, RealQuaternion rhs) =>
            (lhs - rhs).LengthSquared;
        
        public static float Distance(RealQuaternion lhs, RealQuaternion rhs) =>
            (float)Math.Sqrt(DistanceSquared(lhs, rhs));
        
        public static RealQuaternion operator +(RealQuaternion value) =>
            value;
        
        public static RealQuaternion operator -(RealQuaternion value) =>
            new RealQuaternion(-value.I, -value.J, -value.K, -value.W);

        public static RealQuaternion operator +(RealQuaternion lhs, RealQuaternion rhs) =>
            new RealQuaternion(lhs.I + rhs.I, lhs.J + rhs.J, lhs.K + rhs.K, lhs.W + rhs.W);

        public static RealQuaternion operator +(float lhs, RealQuaternion rhs) =>
            new RealQuaternion(lhs + rhs.I, lhs + rhs.J, lhs + rhs.K, lhs + rhs.W);

        public static RealQuaternion operator +(RealQuaternion lhs, float rhs) =>
            new RealQuaternion(lhs.I + rhs, lhs.J + rhs, lhs.K + rhs, lhs.W + rhs);

        public static RealQuaternion operator -(RealQuaternion lhs, RealQuaternion rhs) =>
            new RealQuaternion(lhs.I - rhs.I, lhs.J - rhs.J, lhs.K - rhs.K, lhs.W - rhs.W);
        
        public static RealQuaternion operator *(RealQuaternion vec, float scale) =>
            new RealQuaternion(vec.I * scale, vec.J * scale, vec.K * scale, vec.W * scale);
        
        public static RealQuaternion operator *(float scale, RealQuaternion vec) =>
            vec * scale;

        public static RealQuaternion operator *(RealQuaternion lhs, RealQuaternion rhs) =>
            new RealQuaternion(lhs.I * rhs.I, lhs.J * rhs.J, lhs.K * rhs.K, lhs.W * rhs.W);
        
        public static RealQuaternion operator /(RealQuaternion vec, float divisor) =>
            new RealQuaternion(vec.I / divisor, vec.J / divisor, vec.K / divisor, vec.W / divisor);

        public static RealQuaternion FromDHenN3(uint DHenN3)
        {
            uint[] SignExtendX = { 0x00000000, 0xFFFFFC00 };
            uint[] SignExtendYZ = { 0x00000000, 0xFFFFF800 };
            uint temp;

            temp = DHenN3 & 0x3FF;
            var a = (float)(short)(temp | SignExtendX[temp >> 9]) / (float)0x1FF;

            temp = (DHenN3 >> 10) & 0x7FF;
            var b = (float)(short)(temp | SignExtendYZ[temp >> 10]) / (float)0x3FF;

            temp = (DHenN3 >> 21) & 0x7FF;
            var c = (float)(short)(temp | SignExtendYZ[temp >> 10]) / (float)0x3FF;

            return new RealQuaternion(a, b, c, 1.0f);
        }

        public static RealQuaternion FromUDHenN3(uint UDHenN3)
        {
            var a = (float)(UDHenN3 & 0x3FF) / (float)0x3FF;
            var b = (float)((UDHenN3 >> 10) & 0x7FF) / (float)0x7FF;
            var c = (float)((UDHenN3 >> 21) & 0x7FF) / (float)0x7FF;

            return new RealQuaternion(a, b, c, 1.0f);
        }

        public static RealQuaternion FromHenDN3(uint HenDN3)
        {
            uint[] SignExtendXY = { 0x00000000, 0xFFFFF800 };
            uint[] SignExtendZ = { 0x00000000, 0xFFFFFC00 };
            uint temp;

            temp = HenDN3 & 0x7FF;
            var a = (float)(short)(temp | SignExtendXY[temp >> 10]) / (float)0x3FF;

            temp = (HenDN3 >> 11) & 0x7FF;
            var b = (float)(short)(temp | SignExtendXY[temp >> 10]) / (float)0x3FF;

            temp = (HenDN3 >> 22) & 0x3FF;
            var c = (float)(short)(temp | SignExtendZ[temp >> 9]) / (float)0x1FF;

            return new RealQuaternion(a, b, c, 1.0f);
        }

        public static RealQuaternion FromUHenDN3(uint UHenDN3)
        {
            var a = (float)(UHenDN3 & 0x7FF) / (float)0x7FF;
            var b = (float)((UHenDN3 >> 11) & 0x7FF) / (float)0x7FF;
            var c = (float)((UHenDN3 >> 22) & 0x3FF) / (float)0x3FF;

            return new RealQuaternion(a, b, c, 1.0f);
        }

        public static RealQuaternion FromDecN4(uint DecN4)
        {
            uint[] SignExtend = { 0x00000000, 0xFFFFFC00 };
            uint[] SignExtendW = { 0x00000000, 0xFFFFFFFC };
            uint temp;

            temp = DecN4 & 0x3FF;
            var a = (float)(short)(temp | SignExtend[temp >> 9]) / 511.0f;

            temp = (DecN4 >> 10) & 0x3FF;
            var b = (float)(short)(temp | SignExtend[temp >> 9]) / 511.0f;

            temp = (DecN4 >> 20) & 0x3FF;
            var c = (float)(short)(temp | SignExtend[temp >> 9]) / 511.0f;

            temp = DecN4 >> 30;
            var d = (float)(short)(temp | SignExtendW[temp >> 1]);

            return new RealQuaternion(a, b, c, d);
        }

        public static RealQuaternion FromUDecN4(uint UDecN4)
        {
            var a = (float)(UDecN4 & 0x3FF) / (float)0x3FF;
            var b = (float)((UDecN4 >> 10) & 0x3FF) / (float)0x3FF;
            var c = (float)((UDecN4 >> 20) & 0x3FF) / (float)0x3FF;
            var d = (float)(UDecN4 >> 30) / (float)0x003;

            return new RealQuaternion(a, b, c, d);
        }

        public static RealQuaternion FromUByte4(uint UByte4)
        {
            var d = (float)(UByte4 & 0xFF);
            var c = (float)((UByte4 >> 8) & 0xFF);
            var b = (float)((UByte4 >> 16) & 0xFF);
            var a = (float)(UByte4 >> 24);

            return new RealQuaternion(a, b, c, d);
        }

        public static RealQuaternion FromUByteN4(uint UByteN4)
        {
            var d = (float)(UByteN4 & 0xFF) / (float)0xFF;
            var c = (float)((UByteN4 >> 8) & 0xFF) / (float)0xFF;
            var b = (float)((UByteN4 >> 16) & 0xFF) / (float)0xFF;
            var a = (float)(UByteN4 >> 24) / (float)0xFF;

            return new RealQuaternion(a, b, c, d);
        }

        public RealQuaternion Point3DTransform(RealMatrix4x3 transform) =>
            transform.IsIdentity ?
                this :
                new RealQuaternion(
                    I * transform.m11 + J * transform.m21 + K * transform.m31 + transform.m41,
                    I * transform.m12 + J * transform.m22 + K * transform.m32 + transform.m42,
                    I * transform.m13 + J * transform.m23 + K * transform.m33 + transform.m43,
                    W);

        public RealQuaternion Vector3DTransform(RealMatrix4x3 transform) =>
            transform.IsIdentity ?
                this :
                new RealQuaternion(
                    I * transform.m11 + J * transform.m21 + K * transform.m31,
                    I * transform.m12 + J * transform.m22 + K * transform.m32,
                    I * transform.m13 + J * transform.m23 + K * transform.m33,
                    W);

        public static bool operator ==(RealQuaternion lhs, RealQuaternion rhs) =>
            lhs.Equals(rhs);

        public static bool operator !=(RealQuaternion lhs, RealQuaternion rhs) =>
            !(lhs == rhs);

        public bool Equals(RealQuaternion other) =>
            (I == other.I) &&
            (J == other.J) &&
            (K == other.K) &&
            (W == other.W);

        public override bool Equals(object other) =>
            other is RealQuaternion ?
                Equals((RealQuaternion)other) :
                false;

        public override int GetHashCode() =>
            13 * 17 + I.GetHashCode()
               * 17 + J.GetHashCode()
               * 17 + K.GetHashCode()
               * 17 + W.GetHashCode();

        public override string ToString() =>
            $"{{ {I}, {J}, {K}, {W} }}";
    }
}
