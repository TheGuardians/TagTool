using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Common;

namespace TagTool.Cache
{
    /// <summary>
    /// Platform dependent unsigned integer, uint32 when 32 bit, uint64 when 64 bit. 
    /// Serializer is responsible for properly reading/writing to file.
    /// </summary>
    public class PlatformUnsignedValue
    {
        public ulong Value;

        public PlatformUnsignedValue(ulong value) { Value = value; }
        public PlatformUnsignedValue(uint value) { Value = value; }

        public uint Get32BitValue() => (uint)(Value);
        public ulong Get64BitValue() => Value;
    }

    /// <summary>
    /// Platform dependent signed integer, int32 when 32 bit, int64 when 64 bit. 
    /// Serializer is responsible for properly reading/writing to file.
    /// </summary>
    public class PlatformSignedValue
    {
        public long Value;

        public PlatformSignedValue(long value) { Value = value; }
        public PlatformSignedValue(int value) { Value = value; }

        // TODO: verify this cast actually works
        public int Get32BitValue() => (int)(Value);
        public long Get64BitValue() => Value;
    }
}