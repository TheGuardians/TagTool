using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "detail_object_collection", Tag = "dobc", Size = 0x80)]
    public class DetailObjectCollection : TagStructure
    {
        public CollectionTypeValue CollectionType;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        public float GlobalZOffset; //  Applied to all detail objects in this collection so they don't float above the ground.
        [TagField(Flags = Padding, Length = 44)]
        public byte[] Padding2;
        public CachedTag SpritePlate;
        public List<DetailObjectTypeDefinition> Types;
        [TagField(Flags = Padding, Length = 48)]
        public byte[] Padding3;
        
        public enum CollectionTypeValue : short
        {
            ScreenFacing,
            ViewerFacing
        }
        
        [TagStructure(Size = 0x60)]
        public class DetailObjectTypeDefinition : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public sbyte SequenceIndex; // [0,15]
            public TypeFlagsValue TypeFlags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public float ColorOverrideFactor; // Fraction of detail object color to use instead of the base map color in the environment:[0,1]
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding2;
            public float NearFadeDistance; // world units
            public float FarFadeDistance; // world units
            public float Size; // world units per pixel
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding3;
            public RealRgbColor MinimumColor; // [0,1]
            public RealRgbColor MaximumColor; // [0,1]
            public ArgbColor AmbientColor; // [0,255]
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding4;
            
            [Flags]
            public enum TypeFlagsValue : byte
            {
                Unused = 1 << 0,
                Unused0 = 1 << 1,
                InterpolateColorInHsv = 1 << 2,
                MoreColors = 1 << 3
            }
        }
    }
}

