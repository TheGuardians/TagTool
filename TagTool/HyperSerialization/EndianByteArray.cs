using System;
using System.Collections.Generic;

namespace TagTool.HyperSerialization
{
	public class EndianBytes
	{
		public byte[] Data;
		public readonly bool IsLittleEndian;
		public readonly bool NeedsEndianSwap;

		public EndianBytes(byte[] data, bool isLittleEndian = true)
		{
			this.Data = data;
			this.IsLittleEndian = isLittleEndian;
			this.NeedsEndianSwap = this.IsLittleEndian != BitConverter.IsLittleEndian;
		}

		public byte[] ReadBytes(ref int offset, int count)
		{
			var bytes = new byte[count];
			Array.Copy(this.Data, offset, bytes, 0, count);
			offset += count;
			return bytes;
		}

		public Boolean ToBoolean(ref int offset)
		{
			var size = 1;
			if (offset + size >= this.Data.Length)
				throw new IndexOutOfRangeException();
			if (NeedsEndianSwap)
				Array.Reverse(this.Data, offset, size);
			var value = BitConverter.ToBoolean(this.Data, offset);
			offset += size;
			return value;
		}

		public Byte ToByte(ref int offset)
		{
			var size = 1;
			if (offset + size >= this.Data.Length)
				throw new IndexOutOfRangeException();
			if (NeedsEndianSwap)
				Array.Reverse(this.Data, offset, size);
			var value = (Byte)this.Data[offset];
			offset += size;
			return value;
		}

		public SByte ToSByte(ref int offset)
		{
			var size = 1;
			if (offset + size >= this.Data.Length)
				throw new IndexOutOfRangeException();
			if (NeedsEndianSwap)
				Array.Reverse(this.Data, offset, size);
			var value = (SByte)this.Data[offset];
			offset += size;
			return value;
		}

		public Char ToChar(ref int offset)
		{
			var size = 2;
			if (offset + size >= this.Data.Length)
				throw new IndexOutOfRangeException();
			if (NeedsEndianSwap)
				Array.Reverse(this.Data, offset, size);
			var value = BitConverter.ToChar(this.Data, offset);
			offset += size;
			return value;
		}

		public Int16 ToInt16(ref int offset)
		{
			var size = 2;
			if (offset + size >= this.Data.Length)
				throw new IndexOutOfRangeException();
			if (NeedsEndianSwap)
				Array.Reverse(this.Data, offset, size);
			var value = BitConverter.ToInt16(this.Data, offset);
			offset += size;
			return value;
		}

		public UInt16 ToUInt16(ref int offset)
		{
			var size = 2;
			if (offset + size >= this.Data.Length)
				throw new IndexOutOfRangeException();
			if (NeedsEndianSwap)
				Array.Reverse(this.Data, offset, size);
			var value = BitConverter.ToUInt16(this.Data, offset);
			offset += size;
			return value;
		}

		public Single ToSingle(ref int offset)
		{
			var size = 4;
			if (offset + size >= this.Data.Length)
				throw new IndexOutOfRangeException();
			if (NeedsEndianSwap)
				Array.Reverse(this.Data, offset, size);
			var value = BitConverter.ToSingle(this.Data, offset);
			offset += size;
			return value;
		}

		public Int32 ToInt32(ref int offset)
		{
			var size = 4;
			if (offset + size >= this.Data.Length)
				throw new IndexOutOfRangeException();
			if (NeedsEndianSwap)
				Array.Reverse(this.Data, offset, size);
			var value = BitConverter.ToInt32(this.Data, offset);
			offset += size;
			return value;
		}

		public UInt32 ToUInt32(ref int offset)
		{
			var size = 4;
			if (offset + size >= this.Data.Length)
				throw new IndexOutOfRangeException();
			if (NeedsEndianSwap)
				Array.Reverse(this.Data, offset, size);
			var value = BitConverter.ToUInt32(this.Data, offset);
			offset += size;
			return value;
		}

		public Double ToDouble(ref int offset)
		{
			var size = 8;
			if (offset + size >= this.Data.Length)
				throw new IndexOutOfRangeException();
			if (NeedsEndianSwap)
				Array.Reverse(this.Data, offset, size);
			var value = BitConverter.ToDouble(this.Data, offset);
			offset += size;
			return value;
		}

		public Int64 ToInt64(ref int offset)
		{
			var size = 8;
			if (offset + size >= this.Data.Length)
				throw new IndexOutOfRangeException();
			if (NeedsEndianSwap)
				Array.Reverse(this.Data, offset, size);
			var value = BitConverter.ToInt64(this.Data, offset);
			offset += size;
			return value;
		}

		public UInt64 ToUInt64(ref int offset)
		{
			var size = 8;
			if (offset + size >= this.Data.Length)
				throw new IndexOutOfRangeException();
			if (NeedsEndianSwap)
				Array.Reverse(this.Data, offset, size);
			var value = BitConverter.ToUInt64(this.Data, offset);
			offset += size;
			return value;
		}

		public Decimal ToDecimal(ref int offset)
		{
			var size = 16;
			if (offset + size >= this.Data.Length)
				throw new IndexOutOfRangeException();
			if (NeedsEndianSwap)
				Array.Reverse(this.Data, offset, size);
			var bytes = new byte[size];
			Array.Copy(this.Data, offset, bytes, 0, size);

			Int32[] bits = new Int32[4];
			for (int i = 0; i <= 15; i += 4)
				bits[i / 4] = BitConverter.ToInt32(bytes, i);
			var value = new decimal(bits);

			offset += size;
			return value;
		}

		public Int32 ToPointer(ref int offset)
		{
			var size = 4;
			if (offset + size >= this.Data.Length)
				throw new IndexOutOfRangeException();
			if (NeedsEndianSwap)
				Array.Reverse(this.Data, offset, size);
			var value = BitConverter.ToUInt32(this.Data, offset);
			offset += size;
			return (int)(value - 0x40000000);
		}
	}

	public static class DecimalConverter
	{
		public static byte[] GetBytes(decimal dec)
		{
			//Load four 32 bit integers from the Decimal.GetBits function
			Int32[] bits = decimal.GetBits(dec);

			//Create a temporary list to hold the bytes
			List<byte> bytes = new List<byte>();

			//iterate each 32 bit integer
			foreach (Int32 i in bits)
			{
				//add the bytes of the current 32bit integer
				//to the bytes list
				bytes.AddRange(BitConverter.GetBytes(i));
			}

			//return the bytes list as an array
			return bytes.ToArray();
		}
	}
}
