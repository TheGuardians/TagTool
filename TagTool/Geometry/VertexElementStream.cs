using TagTool.Common;
using TagTool.IO;
using System;
using System.IO;
using System.Linq;

namespace TagTool.Geometry
{
    public class VertexElementStream
    {
        private EndianReader Reader { get; }
        private EndianWriter Writer { get; }

        public VertexElementStream(Stream baseStream, EndianFormat format = EndianFormat.LittleEndian)
        {
            Reader = new EndianReader(baseStream, format);
            Writer = new EndianWriter(baseStream, format);
        }

        public float ReadFloat1()
        {
            return Reader.ReadSingle();
        }

        public void WriteFloat1(float v)
        {
            Writer.Write(v);
        }

        public RealVector2d ReadFloat2()
        {
            return new RealVector2d(Read(2, () => Reader.ReadSingle()));
        }

        public void WriteFloat2(RealVector2d v)
        {
            Write(v.ToArray(), 2, e => Writer.Write(e));
        }

        public RealVector3d ReadFloat3()
        {
            return new RealVector3d(Read(3, () => Reader.ReadSingle()));
        }

        public void WriteFloat3(RealVector3d v)
        {
            Write(v.ToArray(), 3, e => Writer.Write(e));
        }

        public RealQuaternion ReadFloat4()
        {
            return new RealQuaternion(Read(4, () => Reader.ReadSingle()));
        }

        public void WriteFloat4(RealQuaternion v)
        {
            Write(v.ToArray(), 4, e => Writer.Write(e));
        }

        public uint ReadColor()
        {
            return Reader.ReadUInt32();
        }

        public void WriteColor(uint v)
        {
            Writer.Write(v);
        }

        public byte[] ReadUByte4()
        {
            return Reader.ReadBytes(4);
        }

        public void WriteUByte4(byte[] v)
        {
            Writer.Write(v, 0, 4);
        }

        public short[] ReadShort2()
        {
            return Read(2, () => Reader.ReadInt16());
        }

        public void WriteShort2(short[] v)
        {
            Write(v, 2, e => Writer.Write(e));
        }

        public short[] ReadShort4()
        {
            return Read(2, () => Reader.ReadInt16());
        }

        public void WriteShort4(short[] v)
        {
            Write(v, 4, e => Writer.Write(e));
        }
        
        public RealQuaternion ReadSByte4N()
        {
            return new RealQuaternion(Read(4, Reader.ReadSByte, e => DenormalizeSigned(e)).ToArray());
        }
        
        public RealQuaternion ReadUByte4N()
        {
            return new RealQuaternion(Read(4, Reader.ReadByte, e => DenormalizeUnsigned(e)).ToArray());
        }

        public void WriteSByte4N(RealQuaternion v)
        {

            Writer.Write(v.ToArray().Select(e => unchecked((byte)NormalizeSByte(e))).ToArray());
        }

        public void WriteUByte4N(RealQuaternion v)
        {
            Writer.Write(v.ToArray().Select(e => NormalizeByte(e)).ToArray());
        }
        
        public RealVector2d ReadShort2N()
        {
            return new RealVector2d(Read(2, () => DenormalizeSigned(Reader.ReadInt16())));
        }

        public void WriteShort2N(RealVector2d v)
        {
            Write(v.ToArray(), 2, e => Writer.Write(NormalizeShort(e)));
        }

        public RealQuaternion ReadShort4N()
        {
            return new RealQuaternion(Read(4, () => DenormalizeSigned(Reader.ReadInt16())));
        }

        public void WriteShort4N(RealQuaternion v)
        {
            Write(v.ToArray(), 4, e => Writer.Write(NormalizeShort(e)));
        }

        public RealVector2d ReadUShort2N()
        {
            return new RealVector2d(Read(2, () => DenormalizeUnsigned(Reader.ReadUInt16())));
        }

        public void WriteUShort2N(RealVector2d v)
        {
            Write(v.ToArray(), 2, e => Writer.Write(NormalizeUShort(e)));
        }

        public RealQuaternion ReadUShort4N()
        {
            return new RealQuaternion(Read(4, () => DenormalizeUnsigned(Reader.ReadUInt16())));
        }

        public void WriteUShort4N(RealQuaternion v)
        {
            Write(v.ToArray(), 4, e => Writer.Write(NormalizeUShort(e)));
        }

        public RealVector3d ReadUDec3()
        {
            var val = Reader.ReadUInt32();
            var x = (float)(val >> 22);
            var y = (float)((val >> 12) & 0x3FF);
            var z = (float)((val >> 2) & 0x3FF);
            return new RealVector3d(x, y, z);
        }

        public void WriteUDec3(RealVector3d v)
        {
            var x = (uint)v.I & 0x3FF;
            var y = (uint)v.J & 0x3FF;
            var z = (uint)v.K & 0x3FF;
            Writer.Write((x << 22) | (y << 12) | (z << 2));
        }

        public RealVector3d ReadDec3N()
        {
            var val = Reader.ReadUInt32();
            var x = DenormalizeSigned10BitInt((ushort)((val >> 0) & 0x3FF));
            var y = DenormalizeSigned10BitInt((ushort)((val >> 10) & 0x3FF));
            var z = DenormalizeSigned10BitInt((ushort)((val >> 20) & 0x3FF));
            return new RealVector3d(x, y, z);
        }

        public void WriteDec3N(RealVector3d v)
        {
            var x = NormalizeSigned10BitInt(v.I);
            var y = NormalizeSigned10BitInt(v.J);
            var z = NormalizeSigned10BitInt(v.K);
            Writer.Write((x << 22) | (y << 12) | (z << 2));
        }

        public RealVector3d ReadDHenN3()
        {
            var DHenN3 = Reader.ReadUInt32();

            uint[] SignExtendX = { 0x00000000, 0xFFFFFC00 };
            uint[] SignExtendYZ = { 0x00000000, 0xFFFFF800 };
            uint temp;

            temp = DHenN3 & 0x3FF;
            var a = (short)(temp | SignExtendX[temp >> 9]) / (float)0x1FF;

            temp = (DHenN3 >> 10) & 0x7FF;
            var b = (short)(temp | SignExtendYZ[temp >> 10]) / (float)0x3FF;

            temp = (DHenN3 >> 21) & 0x7FF;
            var c = (short)(temp | SignExtendYZ[temp >> 10]) / (float)0x3FF;

            return new RealVector3d(a, b, c);
        }
        
        public float ReadFloat8_1()
        {
            return DenormalizeUnsigned(Reader.ReadByte());
        }
        
        public RealVector2d ReadFloat16_2()
        {
            return new RealVector2d(Read(2, () => (float)Half.ToHalf(Reader.ReadUInt16())));
        }

        public void WriteFloat16_2(RealVector2d v)
        {
            Write(v.ToArray(), 2, e => Writer.Write(Half.GetBytes(new Half(e))));
        }

        public RealQuaternion ReadFloat16_4()
        {
            return new RealQuaternion(Read(4, () => (float)Half.ToHalf(Reader.ReadUInt16())));
        }

        public void WriteFloat16_4(RealQuaternion v)
        {
            Write(v.ToArray(), 4, e => Writer.Write(Half.GetBytes(new Half(e))));
        }

        private static T[] Read<T>(int count, Func<T> readFunc)
        {
            var c = new T[count];
            for (var i = 0; i < count; i++)
                c[i] = readFunc();
            return c;
        }

        private static T2[] Read<T1, T2>(int count, Func<T1> readFunc, Func<T1, T2> convertFunc)
        {
            var c = new T2[count];
            for (var i = 0; i < count; i++)
                c[i] = convertFunc(readFunc());
            return c;
        }

        private static void Write<T>(T[] elems, int count, Action<T> writeAction)
        {
            for (var i = 0; i < count; i++)
                writeAction(elems[i]);
        }
        
        //
        //  Helpers
        //

        /// <summary> 
        /// Force range [-1,1] on input float
        /// </summary>
        public static float Clamp(float e)
        {
            return Math.Max(-1.0f, Math.Min(1.0f, e));
        }

        /// <summary> 
        /// Force range [a,b] on input float
        /// </summary>
        public static float Clamp(float e, float a, float b)
        {
            return Math.Max(a, Math.Min(b, e));
        }

        /// <summary> 
        /// Denormalize an unsigned integer by dividing by 0xFFFF FFFF
        /// </summary>
        private static float DenormalizeUnsigned(uint value)
        {
            return value / (float)uint.MaxValue;
        }

        /// <summary> 
        /// Denormalize an unsigned short integer by dividing by 0xFFFF
        /// </summary>
        private static float DenormalizeUnsigned(ushort value)
        {
            return value / (float)ushort.MaxValue;
        }

        /// <summary> 
        /// Denormalize an unsigned byte integer by dividing by 0xFF
        /// </summary>
        private static float DenormalizeUnsigned(byte value)
        {
            return value / (float)byte.MaxValue;
        }

        /// <summary> 
        /// Denormalize an signed integer by dividing by 0x7FFF FFFF.
        /// 0x8000 0000 is mapped to -1.0f by convention (double -1.0 mapping)
        /// </summary>
        private static float DenormalizeSigned(int value)
        {
            if (value == int.MinValue)
                return -1.0f;
            else
                return value / (float)int.MaxValue;
        }

        /// <summary> 
        /// Denormalize an signed short integer by dividing by 0x7FFF.
        /// 0x8000 is mapped to -1.0f by convention (double -1.0 mapping)
        /// </summary>
        private static float DenormalizeSigned(short value)
        {
            if (value == short.MinValue)
                return -1.0f;
            else
                return value / (float)short.MaxValue;
        }

        /// <summary> 
        /// Denormalize an signed byte integer by dividing by 0x7F.
        /// 0x80 is mapped to -1.0f by convention (double -1.0 mapping)
        /// </summary>
        private static float DenormalizeSigned(sbyte value)
        {
            if (value == sbyte.MinValue)
                return -1.0f;
            else
                return value / (float)sbyte.MaxValue;
        }

        /// <summary> 
        /// Normalize a floating point number in [0,1] to an unsigned integer by multiplication with 0xFFFF FFFF.
        /// </summary>
        private static uint NormalizeUInt(float value)
        {
            value = Clamp(value, 0.0f, 1.0f);
            return (uint)(value * uint.MaxValue);
        }

        /// <summary> 
        /// Normalize a floating point number in [0,1] to an unsigned short by multiplication with 0xFFFF.
        /// </summary>
        private static ushort NormalizeUShort(float value)
        {
            value = Clamp(value, 0.0f, 1.0f);
            return (ushort)(value * ushort.MaxValue);
        }

        /// <summary> 
        /// Normalize a floating point number in [0,1] to an unsigned byte by multiplication with 0xFF.
        /// </summary>
        private static byte NormalizeByte(float value)
        {
            value = Clamp(value, 0.0f, 1.0f);
            return (byte)(value * byte.MaxValue);
        }

        /// <summary> 
        /// Normalize a floating point number in [-1,1] to a signed int by multiplication with 0x7FFF FFFF.
        /// Note that 0x8000 0000 is never returned by convention.
        /// </summary>
        private static int NormalizeInt(float value)
        {
            value = Clamp(value);
            return (int)(value * int.MaxValue);
        }

        /// <summary> 
        /// Normalize a floating point number in [-1,1] to a signed short by multiplication with 0x7FFF.
        /// Note that 0x8000 is never returned by convention.
        /// </summary>
        private static short NormalizeShort(float value)
        {
            value = Clamp(value);
            return (short)(value * short.MaxValue);
        }

        /// <summary> 
        /// Normalize a floating point number in [-1,1] to a signed byte by multiplication with 0x7F.
        /// Note that 0x80 is never returned by convention.
        /// </summary>
        private static sbyte NormalizeSByte(float value)
        {
            value = Clamp(value);
            return (sbyte)(value * sbyte.MaxValue);
        }

        /// <summary> 
        /// Convert a 10 byte signed integer to float.
        /// Expected format: bits are contained in 0x3FF.
        /// </summary>
        private static float DenormalizeSigned10BitInt(ushort value)
        {
            float result;
            if ((value & 0x200) != 0)
            {
                //Two's complement conversion for 10 bit integer

                value = (ushort)~value;              //Invert bits
                value = (ushort)(value & 0x3FF);     //Apply only the first 10 bits
                value = (ushort)(value + 1);         // +1
                result = -value;
            }
            else
                result = (ushort)(value & 0x1FF);     //Apply mask
                
            
            return result / 511.0f;
        }

        /// <summary> 
        /// Convert a 10 byte unsigned integer to float.
        /// Expected format: bits are contained in 0x3FF.
        /// </summary>
        private static float DenormalizeUnsigned10BitInt(ushort value)
        {
            return value / 1023.0f;
        }

        /// <summary> 
        /// Convert a float to a 10 bit unsigned integer
        /// 0x200 is never returned by convention
        /// </summary>
        private static uint NormalizeSigned10BitInt(float value)
        {
            value = Clamp(value);
            ushort result = (ushort)(value * 0x1FF);
            //Apply Two's complement
            if (value < 0)
            {
                result = (ushort)~result;              //Invert bits
                result = (ushort)(result & 0x3FF);     //Apply only the first 10 bits
                result = (ushort)(result + 1);         // +1
            }
                
            return (ushort)(result & 0x3FF);
        }

        /// <summary> 
        /// Convert a float to a 10 bit signed integer
        /// </summary>
        private static uint NormalizeUnsigned10BitInt(float value)
        {
            uint result = (uint)(Clamp(value, 0.0f, 1.0f)*1023.0f);
            return (result & 0x3FF);
        }
    }
}
