using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "formation", Tag = "form", Size = 0x10)]
    public class Formation : TagStructure
    {
        public StringId Name;
        public List<FormationPrimitiveDefinition> Primitives;
        
        [TagStructure(Size = 0x24)]
        public class FormationPrimitiveDefinition : TagStructure
        {
            public FormationPrimitiveFlags Flags;
            public short Priority;
            public short Capacity;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float DistForwards;
            public float DistBackwards;
            public float RankSpacing;
            public float FileSpacing;
            public List<FormationPointDefinition> Points;
            
            [Flags]
            public enum FormationPrimitiveFlags : ushort
            {
                Radial = 1 << 0,
                Leader = 1 << 1
            }
            
            [TagStructure(Size = 0x8)]
            public class FormationPointDefinition : TagStructure
            {
                public Angle Angle;
                public float Offset;
            }
        }
    }
}
