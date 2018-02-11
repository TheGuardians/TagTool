using TagTool.Serialization;
using System;

namespace TagTool.Common
{
    [TagStructure(Size = 0x14)]
    public class TagFunction
    {
        public byte[] Data;

        public enum TagFunctionType : byte
        {
            Identity,
            Constant,
            Transition,
            Periodic,
            Linear,
            LinearKey,
            MultiLinearKey,
            Spline,
            MultiSpline,
            Exponent,
            Spline2
        }

        [Flags]
        public enum TagFunctionTypeFlag : byte
        {
            HasRange = 1,
            IsBounded = 4,
            Normalize = 8,

        }

        public float ComputeFunction(float input, float scale)
        {
            if (Data == null || Data.Length <= 0)
                return 0.0f;

            else
            {
                TagFunctionType opcode = (TagFunctionType)Data[0];
                TagFunctionTypeFlag flags = (TagFunctionTypeFlag)Data[1];

                float result = ComputeSubFunction(0, input, scale);

                if (Data[2] >= 1)
                    return result;

                //Weird
                return (Data[2] - Data[1]) * result + Data[1];
            }

        }

        private float ComputeSubFunction(int index, float input, float scale)
        {
            float result = 0.0f;
            float min = 0.0f;
            float max = 0.0f;

            TagFunctionType opcode = (TagFunctionType)Data[index];
            TagFunctionTypeFlag flags = (TagFunctionTypeFlag)Data[index + 1];

            min = ParseFunctionTypeMin(opcode, input, scale, index);

            if (flags.HasFlag(TagFunctionTypeFlag.HasRange))
            {
                max = ParseFunctionTypeMax(opcode, input, scale, index);

                if (opcode == TagFunctionType.Constant)
                    result = max;
                else
                    result = ScaleOutput(min, max, scale);
            }
            else
                result = min;

            if (flags.HasFlag(TagFunctionTypeFlag.IsBounded))
                result = FitToBounds(0, 1, result);

            return result;
        }

        private float ParseFunctionTypeMin(TagFunctionType type, float input, float scale, int index)
        {
            float result = 0.0f;
            switch (type)
            {
                case TagFunctionType.Identity:
                    result = input;
                    break;

                case TagFunctionType.Constant:
                    result = 0.0f;
                    break;

                case TagFunctionType.Transition:
                    result = Transition(index + 32, input);
                    break;

                case TagFunctionType.Spline:
                    result = ComputePolynomial(3, GetCoefficients(32, 3), input);
                    break;

                case TagFunctionType.Periodic:
                    result = 0.0f;
                    break;

                case TagFunctionType.Linear:
                    result = ComputePolynomial(1, GetCoefficients(32, 1), input);
                    break;

                case TagFunctionType.LinearKey:
                    result = LinearKey(index + 32, input);
                    break;

                case TagFunctionType.Exponent:
                    result = Exponent(index + 32, 0, input); //Has a particular value for exponent, more reversing required
                    break;

                default:
                    break;
            }
            return result;
        }

        private float ParseFunctionTypeMax(TagFunctionType type, float input, float scale, int index)
        {
            float result = 0.0f;
            switch (type)
            {
                case TagFunctionType.Identity:
                    result = input;
                    break;

                case TagFunctionType.Constant:
                    result = scale;
                    break;

                case TagFunctionType.Transition:
                    result = Transition(index + 44, input);
                    break;

                case TagFunctionType.Spline:
                    result = ComputePolynomial(3, GetCoefficients(48, 3), input);
                    break;

                case TagFunctionType.Periodic:
                    result = 0.0f;
                    break;

                case TagFunctionType.Linear:
                    result = ComputePolynomial(1, GetCoefficients(40, 1), input);
                    break;

                case TagFunctionType.LinearKey:
                    result = LinearKey(index + 112, input);
                    break;

                case TagFunctionType.Exponent:
                    result = Exponent(index + 44, 0, input); //Has a particular value for exponent, more reversing required
                    break;

                default:
                    break;
            }
            return result;
        }

        private float Transition(int index, float value)
        {
            float max = GetFloatFromBytes(index + 8);
            float min = GetFloatFromBytes(index + 4);

            float scale = 0.0f; // another function call

            return (max - min) * scale + min;
        }

        private float LinearKey(int index, float value)
        {
            float a = (value - GetFloatFromBytes(index + 36)) * GetFloatFromBytes(index + 52);
            float b = (value - GetFloatFromBytes(index + 40)) * GetFloatFromBytes(index + 56);
            float c = (value - GetFloatFromBytes(index + 44)) * GetFloatFromBytes(index + 60);
            a = FitToBounds(0, 1, a);
            b = FitToBounds(0, 1, b);
            c = FitToBounds(0, 1, c);

            return GetFloatFromBytes(index + 68) * a + GetFloatFromBytes(index + 64) + GetFloatFromBytes(index + 72) * b + GetFloatFromBytes(index + 76) * c;
        }

        private float Exponent(int index, int exponent, float value)
        {
            float x = GetFloatFromBytes(index);
            float y = (float)Math.Pow(x, exponent);
            float z = GetFloatFromBytes(index + 8); //Used in a if statement, verify later
            return ScaleOutput(GetFloatFromBytes(index + 4), x, y);
        }

        private float[] GetCoefficients(int index, int order)
        {
            float[] coefficients = new float[order + 1];
            for (int i = 0; i <= order; i++)
            {
                coefficients[order - i] = GetFloatFromBytes(index + 4 * i);
            }
            return coefficients;
        }

        private float ComputePolynomial(int order, float[] coefficients, float x)
        {
            float result = 0.0f;

            if (coefficients.Length != order + 1)
                return result;

            for (int i = 0; i <= order; i++)
                result = result + (float)Math.Pow(x, i) * coefficients[i];

            return result;
        }

        private float GetFloatFromBytes(int index)
        {
            byte[] temp = new byte[4];
            Array.Copy(Data, index, temp, 0, 4);
            Array.Reverse(temp);
            return BitConverter.ToSingle(temp, index);
        }

        private float FitToBounds(float min, float max, float value)
        {
            if (value < min)
                return min;
            else if (max - value < 0.0f)
                return max;
            else
                return value;
        }

        private float ScaleOutput(float min, float max, float scale)
        {
            return (max - min) * scale + min;
        }

    }

}