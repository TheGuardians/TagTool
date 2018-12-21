using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;

namespace TagTool.Tags
{
    [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3Retail)]
    public class TagFunction : TagStructure
	{
        public byte[] Data;

        public enum TagFunctionType : sbyte
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
        public enum TagFunctionFlags : byte
        {
            None = 0,
            HasRange = 1 << 0,
            Bit1 = 1 << 1,
            IsBounded = 1 << 2,
            Normalize = 1 << 3,
            Bit4 = 1 << 4,
            Bit5 = 1 << 5,
            Bit6 = 1 << 6,
            Bit7 = 1 << 7
        }

        public enum TagFunctionOutputType : sbyte
        {
            Scalar,
            OneColor,
            TwoColor,
            ThreeColor,
            FourColor
        }

        [TagStructure(Size = 0x20)]
		public class ScalarFunctionHeader : TagStructure
        {
            public TagFunctionType Type;
            public TagFunctionFlags Flags;
            public TagFunctionOutputType OutputType;
            public byte Unused;
            public Bounds<float> ScaleBounds;
            public float Unknown1;
            public float Unknown2;
            public RealPoint2d Point;
            public uint Capacity;
        }

        [TagStructure(Size = 0x20)]
		public class ColorFunctionHeader : TagStructure
		{
            public TagFunctionType Type;
            public TagFunctionFlags Flags;
            public TagFunctionOutputType OutputType;
            public byte Unused;
            public ArgbColor Color2;
            public ArgbColor Color3;
            public ArgbColor Color4;
            public ArgbColor Color1;
            public RealPoint2d Point;
            public uint Capacity;
        }

        [TagStructure(Size = 0x8)]
		public class IdentityData : TagStructure
		{
            public RealPoint2d Point;
        }

        [TagStructure(Size = 0x8)]
		public class ConstantData : TagStructure
		{
            public RealPoint2d Point;
        }

        [TagStructure(Size = 0x8)]
		public class TransitionData : TagStructure
		{
            public RealPoint2d Point;
        }

        [TagStructure(Size = 0x8)]
		public class PeriodicData : TagStructure
		{
            public RealPoint2d Point;
        }

        [TagStructure(Size = 0xC)]
		public class LinearData : TagStructure
		{
            public float Unknown;
            public RealPoint2d Point;
        }

        [TagStructure(Size = 0x10)]
		public class MultiLinearKeyData : TagStructure
		{
            public RealPoint2d Point1;
            public RealPoint2d Point2;
        }

        [TagStructure(Size = 0x14)]
		public class SplineData : TagStructure
		{
            public float Unknown;
            public RealPoint2d Point1;
            public RealPoint2d Point2;
        }

        [TagStructure(Size = 0x10)]
		public class MultiSplineData : TagStructure
		{
            public RealPoint2d Point1;
            public RealPoint2d Point2;
        }

        [TagStructure(Size = 0x10)]
		public class ExponentData : TagStructure
		{
            public RealPoint2d Point1;
            public RealPoint2d Point2;
        }

        [TagStructure(Size = 0x20)]
		public class Spline2Data : TagStructure
		{
            public float Unknown1;
            public RealPoint2d Point1;
            public RealPoint2d Point2;
            public RealPoint2d Point3;
            public float Unknown2;
        }

        public float ComputeFunction(float input, float scale)
        {
            if (Data == null || Data.Length <= 0)
                return 0.0f;

            TagFunctionType opcode = (TagFunctionType)Data[0];
            TagFunctionFlags flags = (TagFunctionFlags)Data[1];

            float result = ComputeSubFunction(0, input, scale);

            if (Data[2] >= 1)
                return result;

            // Weird
            return (Data[2] - Data[1]) * result + Data[1];
        }

        private float ComputeSubFunction(int index, float input, float scale)
        {
            float result = 0.0f;
            float min = 0.0f;
            float max = 0.0f;

            TagFunctionType opcode = (TagFunctionType)Data[index];
            TagFunctionFlags flags = (TagFunctionFlags)Data[index + 1];

            min = ParseFunctionTypeMin(opcode, input, scale, index);

            if (flags.HasFlag(TagFunctionFlags.HasRange))
            {
                max = ParseFunctionTypeMax(opcode, input, scale, index);

                if (opcode == TagFunctionType.Constant)
                    result = max;
                else
                    result = ScaleOutput(min, max, scale);
            }
            else
                result = min;

            if (flags.HasFlag(TagFunctionFlags.IsBounded))
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

        public class TagFunctionHeader
        {
            public TagFunctionType Type;
            public TagFunctionFlags Flags;
            public TagFunctionOutputType OutputType;
            public byte Unused;

            public uint Unknown1;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;

            public uint RemainingDataSize;
            
            public void Read(EndianReader reader)
            {
                Type = (TagFunctionType)reader.ReadSByte();
                Flags = (TagFunctionFlags)reader.ReadByte();
                OutputType = (TagFunctionOutputType)reader.ReadSByte();
                Unused = reader.ReadByte();

                if (Type != TagFunctionType.Identity)
                {
                    Unknown1 = reader.ReadUInt32();
                    Unknown2 = reader.ReadUInt32();
                    Unknown3 = reader.ReadUInt32();
                    Unknown4 = reader.ReadUInt32();
                    Unknown5 = reader.ReadUInt32();
                    Unknown6 = reader.ReadUInt32();
                    RemainingDataSize = reader.ReadUInt32();
                }
            }

            public void Write(EndianWriter writer)
            {
                writer.Write((sbyte)Type);
                writer.Write((byte)Flags);
                writer.Write((sbyte)OutputType);
                writer.Write(Unused);

                if (Type != TagFunctionType.Identity)
                {
                    writer.Write(Unknown1);
                    writer.Write(Unknown2);
                    writer.Write(Unknown3);
                    writer.Write(Unknown4);
                    writer.Write(Unknown5);
                    writer.Write(Unknown6);
                    writer.Write(RemainingDataSize);
                }
            }

            public bool HasRange()
            {
                return (Flags & TagFunctionFlags.HasRange) != TagFunctionFlags.None;
            }
        }

        public abstract class TagFunctionTypeData
        {
            public abstract void Read(EndianReader reader);
            public abstract void Write(EndianWriter writer);
        }

        public class TagFunctionType2Data : TagFunctionTypeData
        {
            public byte Unknown;
            public byte Unused1;
            public byte Unused2;
            public byte Unused3;

            public float Unknown1;
            public float Unknown2;

            public override void Read(EndianReader reader)
            {
                Unknown = reader.ReadByte();
                Unused1 = reader.ReadByte();
                Unused2 = reader.ReadByte();
                Unused3 = reader.ReadByte();
                Unknown1 = reader.ReadSingle();
                Unknown2 = reader.ReadSingle();
                
            }

            public override void Write(EndianWriter writer)
            {
                writer.Write(Unknown);
                writer.Write(Unused1);
                writer.Write(Unused2);
                writer.Write(Unused3);
                writer.Write(Unknown1);
                writer.Write(Unknown2);
            }
        }

        public class TagFunctionType3Data : TagFunctionTypeData
        {
            public byte Unknown;
            public byte Unused1;
            public byte Unused2;
            public byte Unused3;

            public float Unknown1;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;

            public override void Read(EndianReader reader)
            {
                Unknown = reader.ReadByte();
                Unused1 = reader.ReadByte();
                Unused2 = reader.ReadByte();
                Unused3 = reader.ReadByte();
                Unknown1 = reader.ReadSingle();
                Unknown2 = reader.ReadSingle();
                Unknown3 = reader.ReadSingle();
                Unknown4 = reader.ReadSingle();

            }

            public override void Write(EndianWriter writer)
            {
                writer.Write(Unknown);
                writer.Write(Unused1);
                writer.Write(Unused2);
                writer.Write(Unused3);
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
                writer.Write(Unknown4);
            }
        }

        public class TagFunctionType4Data : TagFunctionTypeData
        {
            
            public float Unknown1;
            public float Unknown2;
            
            public override void Read(EndianReader reader)
            {
                Unknown1 = reader.ReadSingle();
                Unknown2 = reader.ReadSingle(); 
            }

            public override void Write(EndianWriter writer)
            {
                writer.Write(Unknown1);
                writer.Write(Unknown2);
            }
        }

        public class TagFunctionType5Data : TagFunctionTypeData
        {

            public float Unknown1;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;
            public float Unknown8;
            public float Unknown9;
            public float Unknown10;
            public float Unknown11;
            public float Unknown12;
            public float Unknown13;
            public float Unknown14;
            public float Unknown15;
            public float Unknown16;
            public float Unknown17;
            public float Unknown18;
            public float Unknown19;
            public float Unknown20;

            public override void Read(EndianReader reader)
            {
                Unknown1 = reader.ReadSingle();
                Unknown2 = reader.ReadSingle();
                Unknown3 = reader.ReadSingle();
                Unknown4 = reader.ReadSingle();
                Unknown5 = reader.ReadSingle();
                Unknown6 = reader.ReadSingle();
                Unknown7 = reader.ReadSingle();
                Unknown8 = reader.ReadSingle();
                Unknown9 = reader.ReadSingle();
                Unknown10 = reader.ReadSingle();
                Unknown11 = reader.ReadSingle();
                Unknown12 = reader.ReadSingle();
                Unknown13 = reader.ReadSingle();
                Unknown14 = reader.ReadSingle();
                Unknown15 = reader.ReadSingle();
                Unknown16 = reader.ReadSingle();
                Unknown17 = reader.ReadSingle();
                Unknown18 = reader.ReadSingle();
                Unknown19 = reader.ReadSingle();
                Unknown20 = reader.ReadSingle();
                Unknown10 = reader.ReadSingle();
            }

            public override void Write(EndianWriter writer)
            {
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
                writer.Write(Unknown4);
                writer.Write(Unknown5);
                writer.Write(Unknown6);
                writer.Write(Unknown7);
                writer.Write(Unknown8);
                writer.Write(Unknown9);
                writer.Write(Unknown10);
                writer.Write(Unknown11);
                writer.Write(Unknown12);
                writer.Write(Unknown13);
                writer.Write(Unknown14);
                writer.Write(Unknown15);
                writer.Write(Unknown16);
                writer.Write(Unknown17);
                writer.Write(Unknown18);
                writer.Write(Unknown19);
                writer.Write(Unknown20);
            }
        }

        public class TagFunctionType7Data : TagFunctionTypeData
        {
            public float Unknown1;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;

            public override void Read(EndianReader reader)
            {
                Unknown1 = reader.ReadSingle();
                Unknown2 = reader.ReadSingle();
                Unknown3 = reader.ReadSingle();
                Unknown4 = reader.ReadSingle();

            }

            public override void Write(EndianWriter writer)
            {
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
                writer.Write(Unknown4);
            }
        }

        public class TagFunctionType9Data : TagFunctionTypeData
        {
            public float Unknown1;
            public float Unknown2;
            public float Unknown3;

            public override void Read(EndianReader reader)
            {
                Unknown1 = reader.ReadSingle();
                Unknown2 = reader.ReadSingle();
                Unknown3 = reader.ReadSingle();

            }

            public override void Write(EndianWriter writer)
            {
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
            }
        }

        public class TagFunctionTypeAData : TagFunctionTypeData
        {

            public float Unknown1;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;

            public override void Read(EndianReader reader)
            {
                Unknown1 = reader.ReadSingle();
                Unknown2 = reader.ReadSingle();
                Unknown3 = reader.ReadSingle();
                Unknown4 = reader.ReadSingle();
                Unknown5 = reader.ReadSingle();
                Unknown6 = reader.ReadSingle();
                Unknown7 = reader.ReadSingle();  
            }

            public override void Write(EndianWriter writer)
            {
                writer.Write(Unknown1);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
                writer.Write(Unknown4);
                writer.Write(Unknown5);
                writer.Write(Unknown6);
                writer.Write(Unknown7);
            }
        }

        public class TagFunctionType8Data : TagFunctionTypeData
        {
            public uint Count;
            public List<Type8Section> Sections;

            public class Type8Section
            {
                public byte Unknown; // Specifies the length of the section 4 -> 16, 7 -> 24, other -> 36
                public byte Unused1;
                public byte Unused2;
                public byte Unused3;

                public float Unknown1;
                public float Unknown2;
                public float Unknown3;
                public float Unknown4;
                public float Unknown5;
                public float Unknown6;
                public float Unknown7;
                public float Unknown8;

                public void Read(EndianReader reader)
                {
                    Unknown = reader.ReadByte(); 
                    Unused1 = reader.ReadByte();
                    Unused2 = reader.ReadByte();
                    Unused3 = reader.ReadByte();

                    Unknown1 = reader.ReadSingle();
                    Unknown2 = reader.ReadSingle();
                    Unknown3 = reader.ReadSingle();

                    if(Unknown != 4)
                    {
                        Unknown4 = reader.ReadSingle();
                        Unknown5 = reader.ReadSingle();
                    }
                    
                    if(Unknown != 7 && Unknown != 4)
                    {
                        Unknown6 = reader.ReadSingle();
                        Unknown7 = reader.ReadSingle();
                        Unknown8 = reader.ReadSingle();
                    }
                }

                public void Write(EndianWriter writer)
                {
                    writer.Write(Unknown);
                    writer.Write(Unused1);
                    writer.Write(Unused2);
                    writer.Write(Unused3);

                    writer.Write(Unknown1);
                    writer.Write(Unknown2);
                    writer.Write(Unknown3);

                    if (Unknown != 4)
                    {
                        writer.Write(Unknown4);
                        writer.Write(Unknown5);
                    }

                    if (Unknown != 7 && Unknown != 4)
                    {
                        writer.Write(Unknown6);
                        writer.Write(Unknown7);
                        writer.Write(Unknown8);
                    }    
                }
            }

            public override void Read(EndianReader reader)
            {
                Count = reader.ReadUInt32();
                Sections = new List<Type8Section>();
                for(int i =0; i< Count; i++)
                {
                    Type8Section section = new Type8Section();
                    section.Read(reader);
                    Sections.Add(section);
                }

            }

            public override void Write(EndianWriter writer)
            {
                writer.Write(Count);
                for(int i =0; i< Count; i++)
                {
                    Sections[i].Write(writer);
                }
            }
        }

        public static TagFunction ConvertTagFunction(EndianFormat format, TagFunction function)
        {
            if (function == null || function.Data == null)
                return null;

            if (function.Data.Length == 0)
                return function;

            var result = new byte[function.Data.Length];

            using (var inputReader = new EndianReader(new MemoryStream(function.Data), format))
            using (var outputWriter = new EndianWriter(new MemoryStream(result), EndianFormat.LittleEndian))
            {
                TagFunctionHeader header = new TagFunctionHeader();
                header.Read(inputReader);
                header.Write(outputWriter);

                ParseTagFunctionData(header.Type, inputReader, outputWriter);

                if (header.HasRange())
                    ParseTagFunctionData(header.Type, inputReader, outputWriter);

                // If any tag function has remaining data, just endian swap it. It is a very rare occurence, not handled by the HO parser. That piece of the function is most likely ignored at runtime.
                while (!inputReader.EOF)
                {
                    float temp = inputReader.ReadSingle();
                    outputWriter.Write(temp);
                }

                function.Data = result;
            }

            return function;
        }

        private static void ParseTagFunctionData(TagFunctionType type, EndianReader inputReader, EndianWriter outputWriter)
        {
            switch (type)
            {
                case TagFunctionType.Identity:
                case TagFunctionType.Constant:
                    break;

                case TagFunctionType.Transition:
                    TagFunctionType2Data data2 = new TagFunctionType2Data();
                    data2.Read(inputReader);
                    data2.Write(outputWriter);
                    break;

                case TagFunctionType.Periodic:
                    TagFunctionType3Data data3 = new TagFunctionType3Data();
                    data3.Read(inputReader);
                    data3.Write(outputWriter);
                    break;

                case TagFunctionType.Linear:
                    TagFunctionType4Data data4 = new TagFunctionType4Data();
                    data4.Read(inputReader);
                    data4.Write(outputWriter);
                    break;

                case TagFunctionType.LinearKey:
                    TagFunctionType5Data data5 = new TagFunctionType5Data();
                    data5.Read(inputReader);
                    data5.Write(outputWriter);
                    break;

                case TagFunctionType.Spline:
                    TagFunctionType7Data data7 = new TagFunctionType7Data();
                    data7.Read(inputReader);
                    data7.Write(outputWriter);
                    break;

                case TagFunctionType.MultiSpline:
                    TagFunctionType8Data data8 = new TagFunctionType8Data();
                    data8.Read(inputReader);
                    data8.Write(outputWriter);
                    break;

                case TagFunctionType.Exponent:
                    TagFunctionType9Data data9 = new TagFunctionType9Data();
                    data9.Read(inputReader);
                    data9.Write(outputWriter);
                    break;

                case TagFunctionType.Spline2:
                    TagFunctionTypeAData dataA = new TagFunctionTypeAData();
                    dataA.Read(inputReader);
                    dataA.Write(outputWriter);
                    break;

                default:
                    throw new Exception($"TagFunction opcode {type} not present in Halo Online parser! REPORT IT.");
            }
        }

    }
}