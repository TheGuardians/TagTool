using TagTool.Ai;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "crate", Tag = "bloc", Size = 0x14)]
    public class Crate : GameObject
    {
        public ushort Flags2;
        public short Unknown6;
        public List<CharacterMetagameProperties> MetagameProperties;
        public sbyte Unknown7;
        public sbyte Unknown8;
        public sbyte Unknown9;
        public sbyte Unknown10;

        [Flags]
        public enum CrateFlagsValue : ushort
        {
            None = 0,
            DoesNotBlockAreaOfEffect = 1 << 0,
            Camera = 1 << 1
        }
    }
}