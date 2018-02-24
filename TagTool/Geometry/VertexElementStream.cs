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
            return new RealQuaternion(Read(4, Reader.ReadSByte, e => (e < 0) ? (e / 128.0f) : (e / 127.0f)).ToArray());
        }
        
        public RealQuaternion ReadUByte4N()
        {
            return new RealQuaternion(Read(4, Reader.ReadByte, e => e / 255.0f).ToArray());
        }

        public void WriteUByte4N(RealQuaternion v)
        {
            Writer.Write(v.ToArray().Select(e => (byte)(Clamp(e) * 255.0f)).ToArray());
        }
        
        public RealVector2d ReadShort2N()
        {
            return new RealVector2d(Read(2, () => Reader.ReadInt16() / 32767.0f));
        }

        public void WriteShort2N(RealVector2d v)
        {
            Write(v.ToArray(), 2, e => Writer.Write((short)(Clamp(e) * 32767.0f)));
        }

        public RealQuaternion ReadShort4N()
        {
            return new RealQuaternion(Read(4, () => Reader.ReadInt16() / 32767.0f));
        }

        public void WriteShort4N(RealQuaternion v)
        {
            Write(v.ToArray(), 4, e => Writer.Write((short)(Clamp(e) * 32767.0f)));
        }

        public RealVector2d ReadUShort2N()
        {
            return new RealVector2d(Read(2, () => Reader.ReadUInt16() / 65535.0f));
        }

        public void WriteUShort2N(RealVector2d v)
        {
            Write(v.ToArray(), 2, e => Writer.Write((ushort)(Clamp(e) * 65535.0f)));
        }

        public RealQuaternion ReadUShort4N()
        {
            return new RealQuaternion(Read(4, () => Reader.ReadUInt16() / 65535.0f));
        }

        public void WriteUShort4N(RealQuaternion v)
        {
            Write(v.ToArray(), 4, e => Writer.Write((ushort)(Clamp(e) * 65535.0f)));
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
            var x = ConvertTenBitSignedValueToFloat((short)((val >> 0) & 0x3FF));
            var y = ConvertTenBitSignedValueToFloat((short)((val >> 10) & 0x3FF));
            var z = ConvertTenBitSignedValueToFloat((short)((val >> 20) & 0x3FF));
            return new RealVector3d(x, y, z);
        }

        public void WriteDec3N(RealVector3d v)
        {
            var x = (((uint)(Clamp(v.I) * 511.0f)) + 512) & 0x3FF;
            var y = (((uint)(Clamp(v.J) * 511.0f)) + 512) & 0x3FF;
            var z = (((uint)(Clamp(v.K) * 511.0f)) + 512) & 0x3FF;
            Writer.Write((x << 22) | (y << 12) | (z << 2));
        }

        public RealVector3d ReadDHenN3()
        {
            var DHenN3 = Reader.ReadUInt32();

            uint[] SignExtendX = { 0x00000000, 0xFFFFFC00 };
            uint[] SignExtendYZ = { 0x00000000, 0xFFFFF800 };
            uint temp;

            temp = DHenN3 & 0x3FF;
            var a = (float)(short)(temp | SignExtendX[temp >> 9]) / (float)0x1FF;

            temp = (DHenN3 >> 10) & 0x7FF;
            var b = (float)(short)(temp | SignExtendYZ[temp >> 10]) / (float)0x3FF;

            temp = (DHenN3 >> 21) & 0x7FF;
            var c = (float)(short)(temp | SignExtendYZ[temp >> 10]) / (float)0x3FF;

            return new RealVector3d(a, b, c);
        }
        
        public float ReadSByte4NToFloat()
        {
            return BitConverter.ToSingle(BitConverter.GetBytes((uint)(Reader.ReadUInt32()+0x7F7F7F7F)),0);
        }

        public RealQuaternion ReadSByte4NToUByte4N()
        {
            var result = new byte[4];
            var value = Reader.ReadUInt32()+0x7f7f7f7f;
            return new RealQuaternion((byte)(value)/255.0f, (byte)(value>>8) / 255.0f, (byte)(value>>16) / 255.0f, (byte)(value>>24) / 255.0f);
        }

        public float ReadFloat8_1()
        {
            return Reader.ReadByte() / 255.0f;
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

        private static float Clamp(float e)
        {
            return Math.Max(-1.0f, Math.Min(1.0f, e));
        }

        private static float ConvertTenBitSignedValueToFloat(short value)
        {
            float result;
            if((value & 0x200) != 0)
            {
                //Two's complement conversion for 10 bit integer

                value = (short)~value;              //Invert bits
                value = (short)(value & 0x3FF);     //Apply only the first 10 bits
                value = (short)(value + 1);         // +1
                result = -value / 512.0f;           //divide by 512.0f (max of negative)
            }
            else
            {
                value = (short)(value & 0x3FF);     //Apply mask
                result = value / 511.0f;            //divide by 511.0f (max of positive)
            }
            return result;
        }

        public RealQuaternion ReadUShort4NInv()
        {
            return new RealQuaternion(Read(4, () => ConvertUShort(Reader.ReadUInt16(), 1) / 65535.0f));
        }

        public RealQuaternion ReadTinyPositionData()
        {
            RealQuaternion result;
            byte rotation2 = ConvertByte(Reader.ReadByte(), -1);
            byte rotation1 = ConvertByte(Reader.ReadByte(), 0);

            byte scale2 = ConvertByte(Reader.ReadByte(), -1);
            byte scale1 = ConvertByte(Reader.ReadByte(), 0);

            result = new RealQuaternion(scale1/255.0f, scale2/255.0f, rotation1/255.0f, rotation2 / 255.0f);

            return result;
        }

        private static ushort ConvertUShort(ushort value, sbyte fixup)
        {
            ushort result = 0;
            bool lastBit = ((value >> 15) & 1) == 1;
            if (lastBit)
                result = (ushort)(value & 0x7FFF);
            else
                result = (ushort) (value + 0x8000);

            result = (ushort)(result + fixup); //adjust if it cause problems
            return result;
        }

        private static byte ConvertByte(byte value, sbyte fixup)
        {
            byte result = 0;
            bool lastBit = ((value >> 7) & 1) == 1;
            if (lastBit)
                result = (byte)(value & 0x7F);
            else
                result = (byte)(value + 0x80);

            result = (byte)(result + fixup); //adjust if it cause problems
            return result;
        }
    }
}
