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

        /// <summary>
        /// Evaluate directional light using SH basis. Output is normalized
        /// </summary>
        /// <param name="order">SH basis order</param>
        /// <param name="intensity">Light color scale</param>
        /// <param name="direction">light direction, unit</param>
        /// <param name="shRed"></param>
        /// <param name="shGreen"></param>
        /// <param name="shBlue"></param>
        public static void EvaluateDirectionalLight(int order, RealRgbColor intensity, RealVector3d direction, float[] shRed, float[] shGreen, float[] shBlue)
        {
            float CosWtInt = 0.75f;
            if (order > 2)
                CosWtInt = 1.0625f;

            float s = (float)(Math.PI / CosWtInt);

            var shBasis = new float[order * order];
            EvaluateSHBasis(direction, order, shBasis);
            Scale(shRed, order, shBasis, intensity.Red * s);
            Scale(shGreen, order, shBasis, intensity.Green * s);
            Scale(shBlue, order, shBasis, intensity.Blue * s);
        }

        /// <summary>
        /// Evaluate spherical light using SH basis
        /// </summary>
        /// <param name="order">SH basis order</param>
        /// <param name="intensity">light color scale</param>
        /// <param name="direction">light direction, unit</param>
        /// <param name="shRed"></param>
        /// <param name="shGreen"></param>
        /// <param name="shBlue"></param>
        public static void EvaluateSphericalLight(int order, RealRgbColor intensity, RealVector3d direction, float[] shRed, float[] shGreen, float[] shBlue)
        {
            float s = (float)(Math.PI);

            var shBasis = new float[order * order];
            EvaluateSHBasis(direction, order, shBasis);
            Scale(shRed, order, shBasis, intensity.Red * s);
            Scale(shGreen, order, shBasis, intensity.Green * s);
            Scale(shBlue, order, shBasis, intensity.Blue * s);
        }

        /// <summary>
        /// Returns the basis evaluated value at the point on the surface of a sphere. Typical SH evaluation code. Needs to be scaled by basis coefficients to provide function value
        /// </summary>
        /// <param name="direction">unit vector from the center of the sphere to its surface, corresponding to a point on the sphere</param>
        /// <param name="order"> SH basis order</param>
        /// <param name="sh">SH basis coefficients</param>
        /// <exception cref="ArgumentException"></exception>
        public static void EvaluateSHBasis(RealVector3d direction, int order, float[] sh)
        {
            if (order > 4)
                throw new ArgumentException(nameof(order), "Only supports up to order 5");

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

        /// <summary>
        /// Returns the dominant light direction from the directional SH basis coefficients. These coefficient determine how light dir from view affects color, max value is when x,y,z are unit, normalizing returns unit vector
        /// where light intensity is highest.
        /// </summary>
        /// <param name="shRed"></param>
        /// <param name="shGreen"></param>
        /// <param name="shBlue"></param>
        /// <returns></returns>
        public static RealVector3d GetDominantLightDirection(float[] shRed, float[] shGreen, float[] shBlue)
        {
            return RealVector3d.Normalize(
                new RealVector3d(
                -((0.212656f * shRed[3]) + (0.715158f * shGreen[3]) + (0.0721856f * shBlue[3])),
                -((0.212656f * shRed[1]) + (0.715158f * shGreen[1]) + (0.0721856f * shBlue[1])),
                  (0.212656f * shRed[2]) + (0.715158f * shGreen[2]) + (0.0721856f * shBlue[2])));
        }

        /// <summary>
        /// Evaluate SH basis in the direction of the dominant light, normalized
        /// </summary>
        /// <param name="order">SH basis order</param>
        /// <param name="shRed"></param>
        /// <param name="shGreen"></param>
        /// <param name="shBlue"></param>
        /// <returns></returns>
        public static RealRgbColor GetDominantLightIntensity(int order, float[] shRed, float[] shGreen, float[] shBlue)
        {
            RealVector3d direction = GetDominantLightDirection(shRed, shGreen, shBlue);

            float[] basis = new float[order * order];
            EvaluateSHBasis(direction, order, basis);

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
            EvaluateSHBasis(dominantLightDir, 3, quadraticBasis);

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
            EvaluateSHBasis(dominantLightDir, 3, qudratic_basis);

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

        /// <summary>
        /// Takes an ambient irradiance and converts it to SH basis
        /// </summary>
        /// <param name="ambientColor">Ambient color irradiance</param>
        /// <param name="shRed"></param>
        /// <param name="shGreen"></param>
        /// <param name="shBlue"></param>
        public static void AmbientSHCoefficientsFromAmbientColor(RealRgbColor ambientColor, float[] shRed, float[] shGreen, float[] shBlue)
        {
            // returns the basis function value. For ambient, this doesn't require a direction as it's uniform on the sphere. Assuming ambientcolor is irradiance.
            float[] basis = new float[1];
            EvaluateSHBasis(new RealVector3d(), 1, basis);
            // Need to figure out which coefficient is required such that basis * coefficient = ambientColor. This means coefficient = ambientColor / basis
            shRed[0] = ambientColor.Red / basis[0];
            shGreen[0] = ambientColor.Green / basis[0];
            shBlue[0] = ambientColor.Blue / basis[0];
            // if ambient color is very strong, this could go above 1.0f which is bad. Make sure to assert or break if it happens to investigate. 1/basis[0] = 3.54 ish
        }

        /// <summary>
        /// Estimate linear SH coefficients from ambient + direction, splitting ambient color into 2 contributions, ambient and linear. Very hackish
        /// </summary>
        /// <param name="ambientColor">Ambient color, before conversion to SH basis</param>
        /// <param name="direction">Direction of dominant light</param>
        /// <param name="ambientBlend">How much of the ambient light is converted to directional</param>
        /// <param name="shRed">Order 1 SH basis</param>
        /// <param name="shGreen">Order 1 SH basis</param>
        /// <param name="shBlue">Order 1 SH basis</param>
        public static void LinearSHFromAmbientAndDominantLight(RealRgbColor ambientColor, RealVector3d direction, float ambientBlend, float[] shRed, float[] shGreen, float[] shBlue)
        {
            if (RealVector3d.Norm(direction) - 1.0f > 0.001f)
                throw new ArgumentException("SH basis eval direction vector not normalized");

            // returns the basis function value for linear basis
            float[] basis = new float[4];
            EvaluateSHBasis(direction, 2, basis);
            // ambient coefficients like function above
            shRed[0] = ambientColor.Red / basis[0];
            shGreen[0] = ambientColor.Green / basis[0];
            shBlue[0] = ambientColor.Blue / basis[0];

            // estimate directional coefficients, such that  coefficients * basis = color
            for (int i = 1; i < 4; i++)
            {
                shRed[i] = ambientBlend * ambientColor.Red / basis[i];
                shGreen[i] = ambientBlend * ambientColor.Green / basis[i];
                shBlue[i] = ambientBlend * ambientColor.Blue / basis[i];
            }
            // TODO: might require normalization to account for double adding of lights at dominant direction?
        }

        /// <summary>
        /// Converts rgb color to luvw space
        /// </summary>
        /// <param name="rgb"></param>
        /// <param name="uvw"></param>
        /// <param name="l"></param>
        public static void ConvertToLuvwSpace(RealRgbColor rgb, ref RealRgbColor uvw, ref float l)
        {
            var c = rgb;
            l = (float)Math.Sqrt(c.Red * c.Red + c.Green * c.Green + c.Blue * c.Blue);
            if (l >= 0.0001f)
            {
                uvw.Red = ((c.Red / l) + 1.0f) * 0.5f;
                uvw.Green = ((c.Green / l) + 1.0f) * 0.5f;
                uvw.Blue = ((c.Blue / l) + 1.0f) * 0.5f;
            }
            else
            {
                l = 0.0f;
                uvw.Red = 0;
                uvw.Green = 0;
                uvw.Blue = 0;
            }
        }

        /// <summary>
        /// Converts array of colors to luvw space
        /// </summary>
        /// <param name="rgb"></param>
        /// <param name="uvw"></param>
        /// <param name="l"></param>
        public static void ConvertToLuvwSpace(RealRgbColor[] rgb, RealRgbColor[] uvw, float[] l)
        {
            for (int i = 0; i < rgb.Length; i++)
            {
                ConvertToLuvwSpace(rgb[i], ref uvw[i], ref l[i]);
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
        public ushort[] DominantLightDirection = new ushort[3];

        public ushort Padding1;

        [TagField(Length = 3)]
        public ushort[] DominantLightIntensity = new ushort[3];

        public ushort Padding2;

        [TagField(Length = 9)]
        public ushort[] SHRed = new ushort[SphericalHarmonics.Order2Count];
        [TagField(Length = 9)]
        public ushort[] SHGreen = new ushort[SphericalHarmonics.Order2Count];
        [TagField(Length = 9)]
        public ushort[] SHBlue = new ushort[SphericalHarmonics.Order2Count];

        public ushort Padding3;
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
