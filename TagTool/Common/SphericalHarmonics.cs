using System;
using System.Collections.Generic;
using TagTool.Tags;

namespace TagTool.Common
{
    /// <summary>
    /// SH coefficients are defined using float arrays. The length depends on the order
    /// </summary>
    public static class SphericalHarmonics
    {
        public static readonly int Order0Count = 1;
        public static readonly int Order1Count = 4;
        public static readonly int Order2Count = 9;
        public static readonly int Order3Count = 16;
        public static readonly int Order4Count = 25;
        public static readonly int Order5Count = 36;

        public static int GetCoefficientCount(int order)
        {
            if (order == 0)
                return 1;
            else
                return order * order;
        }

        public class SH2Probe
        {
            public float[] R = new float[4];
            public float[] G = new float[4];
            public float[] B = new float[4];

            public SH2Probe()
            {

            }

            public SH2Probe(float[] r, float[] g, float[] b)
            {
                R = r;
                G = g;
                B = b;
            }
        }

        public class SH3Probe
        {
            public float[] R = new float[9];
            public float[] G = new float[9];
            public float[] B = new float[9];

            public SH3Probe()
            {

            }

            public SH3Probe(float[] r, float[] g, float[] b)
            {
                R = r;
                G = g;
                B = b;
            }

            public static SH3Probe operator +(SH3Probe a, SH3Probe b)
            {
                var result = new SH3Probe();
                Add(result.R, 3, a.R, b.R);
                Add(result.G, 3, a.G, b.G);
                Add(result.B, 3, a.B, b.B);
                return result;
            }
        }

        public static void Add(float[] result, int order, float[] a, float[] b)
        {
            for (int i = 0; i < order * order; ++i)
                result[i] = a[i] + b[i];
        }

        public static void Scale(float[] result, int order, float[] a, float c)
        {
            for (int i = 0; i < order * order; ++i)
                result[i] = a[i] * c;
        }

        public static void ScaleAdd(float[] result, int order, float[] a, float[] b, float s)
        {
            for (int i = 0; i < order * order; ++i)
                result[i] = a[i] * s + b[i];
        }

        public static void EvaluateDirectionalLight(int order, RealRgbColor intensity, RealVector3d direction, float[] shRed, float[] shGreen, float[] shBlue)
        {
            float CosWtInt = 0.75f;
            if (order > 2)
                CosWtInt = 1.0625f;

            float s = (float)(Math.PI / CosWtInt);

            var shBasis = new float[order * order];
            EvaluateDirection(direction, order, shBasis);
            Scale(shRed, order, shBasis, intensity.Red * s);
            Scale(shGreen, order, shBasis, intensity.Green * s);
            Scale(shBlue, order, shBasis, intensity.Blue * s);
        }

        public static void EvaluateDirection(RealVector3d direction, int order, float[] sh)
        {
            if (order > 4)
                throw new ArgumentException(nameof(order), "Only supports uop to order 5");

            float i = direction.I;
            float j = direction.J;
            float k = direction.K;

            if (order >= 1)
            {
                sh[0] = 0.28209479177387814f;
            }

            if (order >= 2)
            {
                sh[1] = -0.4886025119029199f * j;
                sh[2] = 0.4886025119029199f * k;
                sh[3] = -0.4886025119029199f * i;
            }

            if (order >= 3)
            {
                sh[4] = 1.0925484305920792f * i * j;
                sh[5] = -1.0925484305920792f * j * k;
                sh[6] = -0.31539156525252005f * ((i * i) + (j * j) + (-2f * (k * k)));
                sh[7] = -1.0925484305920792f * i * k;
                sh[8] = 0.5462742152960396f * ((i * i) + (-1f * (j * j)));
            }

            if (order >= 4)
            {
                sh[9] = 0.5900435899266435f * j * ((-3f * (i * i)) + (j * j));
                sh[10] = 2.890611442640554f * i * j * (k);
                sh[11] = 0.4570457994644658f * j * ((i * i) + (j * j) + ((-4f) * (k * k)));
                sh[12] = 0.3731763325901154f * k * ((-3f * (i * i)) + (-3f * (j * j)) + (2f * (k * k)));
                sh[13] = 0.4570457994644658f * i * ((i * i) + (j * j) + (-4f * (k * k)));
                sh[14] = 1.445305721320277f * ((i * i) + (-1f * (j * j))) * k;
                sh[15] = -0.5900435899266435f * i * ((i * i) + (-3f * (j * j)));
            }
        }

        public static RealVector3d GetDominantLightDirection(float[] shRed, float[] shGreen, float[] shBlue)
        {
            return RealVector3d.Normalize(
                new RealVector3d(
                -((0.212656f * shRed[3]) + (0.715158f * shGreen[3]) + (0.0721856f * shBlue[3])),
                -((0.212656f * shRed[1]) + (0.715158f * shGreen[1]) + (0.0721856f * shBlue[1])),
                  (0.212656f * shRed[2]) + (0.715158f * shGreen[2]) + (0.0721856f * shBlue[2])));
        }

        public static RealRgbColor GetDominantLightIntensity(int order, float[] shRed, float[] shGreen, float[] shBlue)
        {
            RealVector3d direction = GetDominantLightDirection(shRed, shGreen, shBlue);

            float[] basis = new float[order * order];
            EvaluateDirection(direction, order, basis);

            var intensity = new RealRgbColor(0, 0, 0);
            intensity.Red = basis[0] * shRed[0];
            intensity.Green = basis[0] * shGreen[0];
            intensity.Blue = basis[0] * shBlue[0];
            float scale = basis[0] * basis[0];

            for (int i = 1; i < order * order; i++)
            {
                intensity.Red += basis[i] * shRed[i];
                intensity.Green += basis[i] * shGreen[i];
                intensity.Blue += basis[i] * shBlue[i];
                scale += basis[i] * basis[i];
            }

            intensity.Red = (float)Math.Max(intensity.Red, 0.0f);
            intensity.Green = (float)Math.Max(intensity.Green, 0.0f);
            intensity.Blue = (float)Math.Max(intensity.Blue, 0.0f);
            scale = (float)Math.Max(scale, 0.0001f);

            intensity.Red /= scale;
            intensity.Green /= scale;
            intensity.Blue /= scale;

            return intensity;
        }

        public static void QuadraticFromLinearAndIntensity(float[] shRed, float[] shGreen, float[] shBlue, RealRgbColor intensity)
        {
            RealVector3d dominantLightDir = GetDominantLightDirection(shRed, shGreen, shBlue);
            float[] quadraticBasis = new float[9];
            EvaluateDirection(dominantLightDir, 3, quadraticBasis);

            Scale(quadraticBasis, 3, quadraticBasis, intensity.Red);
            for (int i = 0; i < 4; i++)
            {
                shRed[4 + i] = quadraticBasis[4 + i];
                shRed[4 + i] = quadraticBasis[4 + i];
                shRed[4 + i] = quadraticBasis[4 + i];
            }
            Scale(quadraticBasis, 3, quadraticBasis, intensity.Green);
            for (int i = 0; i < 4; i++)
            {
                shGreen[4 + i] = quadraticBasis[4 + i];
                shGreen[4 + i] = quadraticBasis[4 + i];
                shGreen[4 + i] = quadraticBasis[4 + i];
            }
            Scale(quadraticBasis, 3, quadraticBasis, intensity.Blue);
            for (int i = 0; i < 4; i++)
            {
                shBlue[4 + i] = quadraticBasis[4 + i];
                shBlue[4 + i] = quadraticBasis[4 + i];
                shBlue[4 + i] = quadraticBasis[4 + i];
            }
        }

        public static void QudraticToLinearAndIntensity(SH3Probe qudraticProbe, out SH2Probe linearProbe, out RealRgbColor intensity)
        {
            RealVector3d dominantLightDir = GetDominantLightDirection(qudraticProbe.R, qudraticProbe.G, qudraticProbe.B);
            float[] qudratic_basis = new float[9];
            EvaluateDirection(dominantLightDir, 3, qudratic_basis);

            linearProbe = new SH2Probe();
            intensity = new RealRgbColor();
            float s = 0;
            for (int i = 0; i < 9; i++)
            {
                intensity.Red += qudratic_basis[i] * qudraticProbe.R[i];
                intensity.Green += qudratic_basis[i] * qudraticProbe.G[i];
                intensity.Blue += qudratic_basis[i] * qudraticProbe.B[i];
                s += qudratic_basis[i] * qudratic_basis[i];
            }

            if (s >= 0.0001f)
            {
                intensity.Red /= s;
                intensity.Green /= s;
                intensity.Blue /= s;
            }

            for (int i = 0; i < 4; i++)
            {
                linearProbe.R[i] = qudraticProbe.R[i];
                linearProbe.G[i] = qudraticProbe.G[i];
                linearProbe.B[i] = qudraticProbe.B[i];
            }
        }
    }

    /// <summary>
    /// Precomputed radiance transfer (PRT) types using spherical harmonics as basis function.
    /// </summary>
    public enum PrtSHType : byte
    {
        None,
        /// <summary>
        /// SH order 0, 1 coefficent per vertex
        /// </summary>
        Ambient,
        /// <summary>
        /// SH order 1, 4 coefficients per vertex
        /// </summary>
        Linear,
        /// <summary>
        /// SH order 2, 9 coefficients per vertex
        /// </summary>
        Quadratic
    }

    [TagStructure(Size = 0x48)]
    public class HalfRGBLightProbe : TagStructure
    {
        [TagField(Length = 3)]
        public short[] DominantLightDirection = new short[3];

        public short Padding1;

        [TagField(Length = 3)]
        public short[] DominantLightIntensity = new short[3];

        public short Padding2;

        [TagField(Length = 9)]
        public short[] SHRed = new short[SphericalHarmonics.Order2Count];
        [TagField(Length = 9)]
        public short[] SHGreen = new short[SphericalHarmonics.Order2Count];
        [TagField(Length = 9)]
        public short[] SHBlue = new short[SphericalHarmonics.Order2Count];

        public short Padding3;
    }

    [TagStructure(Size = 0x18)]
    public class LuminanceScale : TagStructure
    {
        public float Scale;
        public float Unknown1;
        public float Unknown2;
        public float Unknown3;
        public float Unknown4;
        public float Unknown5;
    }
}
