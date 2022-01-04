using System;
using System.IO;
using TagTool.Common;

namespace TagTool.BlamFile.Reach
{
    public class MapVariantReader : BitStream
    {
        private const float kWorldUnitsToInches = 10.0f * 12.0f;
        private const float kInchesToWorldUnits = 1.0f / kWorldUnitsToInches;

        private static readonly int[] kUnitVectorQuantizationTable =
        {
            10, 2,
            21, 3,
            42, 5,
            85, 8,
            170, 12,
            341, 17,
            682, 25,
            1365, 35,
            2730, 51,
            5461, 72,
            10922, 103,
            21845, 146,
            43690, 208,
            87381, 294,
            174762, 417,
            349525, 590,
            699050, 835,
            1398101, 1181,
            2796202, 1671,
            5592405, 2363,
            11184810, 3343,
            22369621, 4728,
            44739242, 6687,
            89478485, 9458,
            178956970, 13376,
        };

        public MapVariantReader(Stream stream) : base(stream)
        {

        }

        public unsafe RealPoint3d ReadPosition(int bitCount, RealRectangle3d worldBounds)
        {
            if (ReadBool())
            {
                var perAxisBitCounts = stackalloc int[3] { bitCount, bitCount, bitCount };
                AdjustAxisEncodingBitCountToMatchErrorGoals(bitCount, worldBounds, 26, perAxisBitCounts);

                var point = stackalloc int[3];
                for (int i = 0; i < 3; i++)
                    point[i] = (int)ReadUnsigned(perAxisBitCounts[i]);

                return DequantizeRealPoint3dPerAxis(point, worldBounds, perAxisBitCounts);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void ReadAxes(int forwardBits, int upBits, out RealVector3d forward, out RealVector3d up)
        {
            if (ReadBool())
            {
                up = new RealVector3d(0, 0, 1);
            }
            else
            {
                var quantizedUp = (int)ReadUnsigned(upBits);
                up = DequantizeUnitVector3d(quantizedUp, upBits);
            }

            var quantizedForwardAngle = (int)ReadUnsigned(forwardBits);
            float forwardAngle = DequantizeReal(quantizedForwardAngle, -(float)Math.PI, (float)Math.PI, forwardBits, false, false);
            forward = AngleToAxis(up, forwardAngle);
        }

        private static void RotateVectorAboutAxis(RealVector3d vector, RealVector3d axis, float y, float x)
        {
            float m = (1.0f - x) * (((vector.I * axis.I) + (vector.J * axis.J)) + (vector.K * axis.K));
            vector.I = ((x * vector.I) + (m * axis.I)) - (y * ((vector.J * axis.K) - (vector.K * axis.J)));
            vector.J = ((x * vector.J) + (m * axis.J)) - (y * (vector.K * axis.I) - (vector.I * axis.K));
            vector.K = ((x * vector.K) + (m * axis.K)) - (y * (vector.I * axis.J) - (vector.J * axis.I));
        }

        private static RealVector3d AngleToAxis(RealVector3d up, float forwardAngle)
        {
            RealVector3d forwardReference;

            var globalForward3d = new RealVector3d(1, 0, 0);
            var globalLeft3d = new RealVector3d(0, 1, 0);

            float forward_amount = RealVector3d.DotProduct(up, globalForward3d);
            float left_amount = RealVector3d.DotProduct(up, globalLeft3d);
            if (Math.Abs(forward_amount) >= Math.Abs(left_amount))
                forwardReference = RealVector3d.Normalize(RealVector3d.CrossProduct(globalLeft3d, up));
            else
                forwardReference = RealVector3d.Normalize(RealVector3d.CrossProduct(globalForward3d, up));

            float x, y;
            if (forwardAngle == (float)Math.PI || forwardAngle == -(float)Math.PI)
            {
                y = 0.0f;
                x = -1.0f;
            }
            else
            {
                y = (float)Math.Sin(forwardAngle);
                x = (float)Math.Cos(forwardAngle);
            }

            RotateVectorAboutAxis(forwardReference, up, y, x);
            return RealVector3d.Normalize(forwardReference);
        }

        private unsafe RealPoint3d DequantizeRealPoint3dPerAxis(int* point, RealRectangle3d bounds, int* per_axis_bit_counts)
        {
            var realPoint = stackalloc float[3];
            var boundsArr = stackalloc float[6] { bounds.X0, bounds.X1, bounds.Y0, bounds.Y1, bounds.Z0, bounds.Z1 };

            for (int axis = 0; axis < 3; axis++)
            {
                int stepCount = 1 << per_axis_bit_counts[axis];
                float min = boundsArr[axis * 2];
                float max = boundsArr[axis * 2 + 1];
                realPoint[axis] = DequantizeReal(point[axis], min, max, per_axis_bit_counts[axis], false, false);
            }
            return new RealPoint3d(realPoint[0], realPoint[1], realPoint[2]);
        }

        private RealVector3d DequantizeUnitVector3d(int quantized_value, int bit_count)
        {
            var vector = new RealVector3d();
            int a = kUnitVectorQuantizationTable[2 * (bit_count - 6)];
            int b = kUnitVectorQuantizationTable[2 * (bit_count - 6) + 1];

            int m = quantized_value - quantized_value / a * a;
            int l = m - m / b * b;

            float c = ((m / b * (2.0f / (b - 1))) - 1.0f) + (2.0f / (b - 1) * 0.5f); // dequantize_real
            float d = ((l * (2.0f / (b - 1))) - 1.0f) + ((2.0f / (b - 1)) * 0.5f);
            if (2 * (m / b) == b - 2) c = 0;
            if (2 * l == b - 2) d = 0;

            switch (quantized_value / a)
            {
                case 0:
                    vector.J = c;
                    vector.K = d;
                    vector.I = 1.0f;
                    break;
                case 3:
                    vector.J = c;
                    vector.K = d;
                    vector.I = 1.0f;
                    break;
                case 1:
                    vector.I = c;
                    vector.K = d;
                    vector.J = 1.0f;
                    break;
                case 4:
                    vector.I = c;
                    vector.K = d;
                    vector.J = -1.0f;
                    break;
                case 2:
                    vector.I = c;
                    vector.J = d;
                    vector.K = 1.0f;
                    break;
                case 5:
                    vector.I = c;
                    vector.J = d;
                    vector.K = -1.0f;
                    break;
                default:
                    vector.I = 0.0f;
                    vector.J = 0.0f;
                    vector.K = 1.0f;
                    break;
            }
            return RealVector3d.Normalize(vector);
        }

        private unsafe void AdjustAxisEncodingBitCountToMatchErrorGoals(int bitCount, RealRectangle3d bounds, int maxBitCount, int* perAxisBitCounts)
        {
            var dimensions = stackalloc float[3]
            {
                bounds.X1 - bounds.X0,
                bounds.Y1 - bounds.Y0,
                bounds.Z1 - bounds.Z0
            };

            float maxError = GetMaximumAxisError(bitCount);
            if (maxError < 0.0001f)
            {
                for (int axis = 0; axis < 3; ++axis)
                    perAxisBitCounts[axis] = bitCount;

                return;
            }

            for (int axis = 0; axis < 3; ++axis)
            {
                int maxValue = (int)Math.Ceiling(dimensions[axis] / (maxError * 2.0f));
                if (maxValue > 0x800000)
                    maxValue = 0x800000;

                int requiredBits = (int)(Math.Ceiling(Math.Log(maxValue) / Math.Log(2)));
                perAxisBitCounts[axis] = requiredBits > maxBitCount ? maxBitCount : requiredBits;
            }
        }

        private static float GetMaximumAxisError(int bitCount)
        {
            if (bitCount <= 16)
                return kInchesToWorldUnits * (1 << (16 - bitCount));
            else
                return kInchesToWorldUnits / (1 << (bitCount - 16));
        }
    }
}
