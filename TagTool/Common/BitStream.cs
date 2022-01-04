using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.IO;

namespace TagTool.Common
{
    public class BitStream
    {
        const int QWORD_BITS = 64;

        private readonly EndianReader _reader;
        private ulong _accumulator;
        private int _accumulatorBitsUsed;

        public BitStream(Stream stream)
        {
            _reader = new EndianReader(stream, EndianFormat.BigEndian);
            _accumulator = DecodeAccumulator();
        }

        public byte[] ReadBytes(int count)
        {
            var Buffer = new byte[count];
            for (int index = 0; index < count; index++)
                Buffer[index] = (byte)ReadUnsigned(8);
            return Buffer;
        }

        public float ReadQuantizedReal(float minValue, float maxValue, int bitCount, bool exactMidpoint, bool exactEndpoints)
        {
            var quantized = (int)ReadUnsigned(bitCount);
            return DequantizeReal(quantized, minValue, maxValue, bitCount, exactMidpoint, exactEndpoints);
        }

        public string ReadUnicodeString(int length)
        {
            var str = "";
            while (length > 0)
            {
                ushort b = (ushort)ReadUnsigned(16);
                if (b == 0)
                    break;
                var c = char.ConvertFromUtf32(b);
                str += c;
                length--;
            }
            return str;
        }

        public string ReadString(int length)
        {
            var str = "";
            while (length > 0)
            {
                byte b = (byte)ReadUnsigned(8);
                if (b == 0)
                    break;
                var c = char.ConvertFromUtf32(b);
                str += c;
                length--;
            }
            return str;
        }

        public bool ReadBool()
        {
            return ReadUnsigned(1) != 0;
        }

        public uint ReadUnsigned(int bits)
        {
            if (bits > QWORD_BITS - _accumulatorBitsUsed)
            {
                return (uint)ReadIntoAccumulator(bits);
            }
            else
            {
                uint value = (uint)(_accumulator >> (QWORD_BITS - bits));
                _accumulator <<= bits;
                _accumulatorBitsUsed += bits;
                return value;
            }
        }

        public ulong ReadUnsigned64(int bits)
        {
            if (bits > QWORD_BITS - _accumulatorBitsUsed)
            {
                return ReadIntoAccumulator(bits);
            }
            else
            {
                ulong value = _accumulator >> (QWORD_BITS - bits);
                _accumulator <<= bits;
                _accumulatorBitsUsed += bits;
                return value;
            }
        }

        private ulong ReadIntoAccumulator(int sizeInBits)
        {
            ulong oldAccumulator = _accumulator;
            ulong newAccumulator = DecodeAccumulator();
            int nextBits = _accumulatorBitsUsed + sizeInBits - QWORD_BITS;
            _accumulator = newAccumulator << nextBits;
            _accumulatorBitsUsed = nextBits;
            ulong carry = oldAccumulator >> (QWORD_BITS - sizeInBits);
            ulong value = newAccumulator >> (QWORD_BITS - nextBits);
            return carry | value;
        }

        private ulong DecodeAccumulator()
        {
            ulong acc = 0;
            if (_reader.Position + 8 > _reader.Length)
            {
                int shiftBits = 0;
                while (_reader.Position < _reader.Length)
                {
                    shiftBits += 8;
                    acc <<= 8;
                    acc |= _reader.ReadByte();
                }
                acc <<= (QWORD_BITS - shiftBits);
            }
            else
            {
                acc = _reader.ReadUInt64();
            }

            return acc;
        }

        protected float DequantizeReal(int value, float min_value, float max_value, int size_in_bits, bool exactMidpoint, bool exactEndpoints)
        {
            int stepCount = 1 << size_in_bits;
            if (exactMidpoint)
                stepCount--;

            if (exactMidpoint && 2 * value == stepCount - 1)
                return (min_value + max_value) * 0.5f;

            if (exactEndpoints)
            {
                if (value == 0)
                    return min_value;
                if (value == stepCount - 1)
                    return max_value;

                float step = (max_value - min_value) / (stepCount - 2);
                return ((value - 1) * step + min_value) + (step * 0.5f);
            }
            else
            {
                float step = (max_value - min_value) / stepCount;
                return (value * step + min_value) + (step * 0.5f);
            }
        }
    }
}
