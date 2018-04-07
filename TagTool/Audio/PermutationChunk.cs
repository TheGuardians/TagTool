using System;
using TagTool.Cache;
using TagTool.Serialization;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x14)]
    public class PermutationChunk
    {
        public uint Offset;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public FlagsValue Flags;
        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public byte Unknown1; // size extra byte for big endian

        public ushort Size;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public byte Unknown2; // size extra byte for little endian
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public byte Unknown3; // always 4?

        public int RuntimeIndex;
        public int UnknownA;
        public int UnknownSize;

        [Flags]
        public enum FlagsValue : byte
        {
            None = 0,
            Bit0 = 1 << 0,
            Bit1 = 1 << 1,
            Bit2 = 1 << 2,
            Bit3 = 1 << 3,
            Bit4 = 1 << 4,
            HasUnknownAValue = 1 << 5,
            Bit6 = 1 << 6,
            Bit7 = 1 << 7
        }
    }
}