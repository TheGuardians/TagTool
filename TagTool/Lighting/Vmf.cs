using System;
using System.Diagnostics;
using System.Linq;
using TagTool.Common;

namespace TagTool.Lighting
{
    public class VmfLight
    {
        public RealVector3d Direction;
        public float AnalyticalMask;
        public RealRgbColor Color;
        public float Bandwidth;

        public VmfLight()
        {

        }

        public VmfLight(float[] coefficients)
        {
            Direction = new RealVector3d(coefficients[0], coefficients[1], coefficients[2]);
            AnalyticalMask = coefficients[3];
            Color = new RealRgbColor(coefficients[4], coefficients[5], coefficients[6]);
            Bandwidth = coefficients[7];
        }

        public VmfLight(ushort[] coefficients) : this(coefficients.Select(x => (float)Half.ToHalf(x)).ToArray())
        {

        }

        private static float coth(double x)
        {
            // y = cosh(x) / sinh(x)

            double denom = Math.Exp(x + x) - 1;
            if (double.IsInfinity(denom))
            {
                Debug.Assert(x > 10);
                return 1.0f;
            }

            if (Math.Abs(denom) <= 1.0e-15)
                return float.MaxValue;
            else
                return (float)((Math.Exp(x + x) + 1) / denom);
        }

        public static void EvalulateVmf(float k, float[] CZH)
        {
            double vmf0 = 0.5 / Math.Sqrt(Math.PI) * Math.Sqrt(4 * Math.PI);
            double vmf1 = Math.Sqrt(3 / Math.PI) * Math.Sqrt(4 * Math.PI / 3) * 0.5;
            double vmf2 = Math.Sqrt(5 / Math.PI) * Math.Sqrt(4 * Math.PI / 5) * 0.5;

            double y = coth(k);
            
            CZH[0] = (float)vmf0;
            if (y == 3.4028235e38)
            {
                CZH[2] = 0.0f;
                CZH[1] = 0.0f;
            }
            else
            {
                CZH[1] = (float)((vmf1 * ((y * k) - 1)) / k);
                CZH[2] = (float)((vmf2 * (3 + (k * k) - ((3 * k) * y))) / (k * k));
            }

            Debug.Assert(!float.IsInfinity(CZH[0]));
            Debug.Assert(!float.IsInfinity(CZH[1]));
            Debug.Assert(!float.IsInfinity(CZH[2]));

            CZH[2] = Math.Min(CZH[2], CZH[0]);
            CZH[1] = Math.Min(CZH[1], CZH[0]);
        }

        public RealRgbColor EvaluateSH(RealVector3d normal)
        {
            float sqrtPi = (float)Math.Sqrt(Math.PI);
            float sqrt3 = (float)Math.Sqrt(3);
            float c0 = 1.0f / (2 * sqrtPi);
            float c1 = sqrt3 / (3.0f * sqrtPi);

            var linearSH = new RealVector4d(Direction.I, Direction.J, Direction.K, AnalyticalMask);
            float s = RealVector4d.Dot(linearSH, new RealVector4d(c1 * normal, c0));
            return new RealRgbColor(s * Color.Red, s * Color.Green, s * Color.Blue);
        }

        public void RebalanceSH(RealVector3d normal, float scale)
        {
            scale = Math.Min(Math.Max(scale, 0), 1.0f);

            float sqrtPi = (float)Math.Sqrt(Math.PI);
            float sqrt3 = (float)Math.Sqrt(3);
            float c0 = 1.0f / (2 * sqrtPi);
            float c1 = sqrt3 / (3.0f * sqrtPi);

            var maxEnergy = GetMaxEnergySH();
            var dcEnergy = GetDCEnergySH();

            float energy = dcEnergy / maxEnergy;
            if (energy > scale && energy < 1.0f)
            {
                float newMagnitude = ((AnalyticalMask * maxEnergy) * scale) / dcEnergy;
                float s = 1.0f + ((c0 * (AnalyticalMask - newMagnitude)) / RealVector3d.DotProduct(Direction * c1, normal));
                Direction *= s;
                AnalyticalMask = newMagnitude;
            }
        }

        public float GetDCEnergySH()
        {
            return RealRgbColor.GetLuminance(EvaluateSH(new RealVector3d(0, 0, 0)));
        }

        public float GetMaxEnergySH()
        {
            return RealRgbColor.GetLuminance(EvaluateSH(Direction));
        }
    }

    public class DualVmfBasis
    {
        public VmfLight Direct;
        public VmfLight Indirect;

        public DualVmfBasis()
        {

        }

        public DualVmfBasis(float[] coefficients)
        {
            Direct = new VmfLight(coefficients.Take(8).ToArray());
            Indirect = new VmfLight(coefficients.Skip(8).ToArray());
        }

        public DualVmfBasis(ushort[] coefficients)
        {
            Direct = new VmfLight(coefficients.Take(8).ToArray());
            Indirect = new VmfLight(coefficients.Skip(8).ToArray());
        }

        public float GetLobeDirection(out RealVector3d direction)
        {
            float directBrigthness = RealRgbColor.GetBrightness(Direct.Color);
            float indirectBrightness = RealRgbColor.GetBrightness(Indirect.Color);
            direction = Direct.Direction * indirectBrightness;
            return directBrigthness + indirectBrightness;
        }
    }
}
