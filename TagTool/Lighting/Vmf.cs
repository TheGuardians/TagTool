using System;
using System.Linq;
using TagTool.Common;

namespace TagTool.Lighting
{
    public class VmfLight
    {
        public RealVector3d Direction;
        public float Magnitude;
        public RealRgbColor Color;
        public float Scale;

        public VmfLight()
        {

        }

        public VmfLight(float[] coefficients)
        {
            Direction = new RealVector3d(coefficients[0], coefficients[1], coefficients[2]);
            Magnitude = coefficients[3];
            Color = new RealRgbColor(coefficients[4], coefficients[5], coefficients[6]);
            Scale = coefficients[7];
        }

        public VmfLight(ushort[] coefficients) : this(coefficients.Select(x => (float)Half.ToHalf(x)).ToArray())
        {

        }

        public RealRgbColor EvaluateSH(RealVector3d normal)
        {
            float sqrtPi = (float)Math.Sqrt(Math.PI);
            float sqrt3 = (float)Math.Sqrt(3);
            float c0 = 1.0f / (2 * sqrtPi);
            float c1 = sqrt3 / (3.0f * sqrtPi);

            var linearSH = new RealVector4d(Direction.I, Direction.J, Direction.K, Magnitude);
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
                float newMagnitude = ((Magnitude * maxEnergy) * scale) / dcEnergy;
                float s = 1.0f + ((c0 * (Magnitude - newMagnitude)) / RealVector3d.DotProduct(Direction * c1, normal));
                Direction *= s;
                Magnitude = newMagnitude;
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

    class DualVmfBasis
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
