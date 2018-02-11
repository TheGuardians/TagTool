using TagTool.Common;
using TagTool.Serialization;
using System;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "camo", Tag = "cmoe", Size = 0x40)]
    public class Camo
    {
        public CamoFlags Flags;

        [TagField(Padding = true, Length = 2)]
        public byte[] Unused = new byte[2];

        public CamoMapping ActiveCamoAmount;
        public CamoMapping ShadowAmount;
    }

    [Flags]
    public enum CamoFlags : ushort
    {
        None,
        AlsoApplyToObjectChildren = 1 << 0
    }

    [TagStructure(Size = 0x1C)]
    public class CamoMapping
    {
        public StringId InputVariable;
        public StringId RangeVariable;
        public TagFunction Mapping;
    }
}