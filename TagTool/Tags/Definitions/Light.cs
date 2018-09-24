using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "light", Tag = "ligh", Size = 0x94, MinVersion = CacheVersion.Halo3Retail)]
    public class Light : TagStructure
	{
        public uint Flags;
        public TypeValue Type;
        public short Unknown;
        public float LightRange;
        public float NearWidth;
        public float HeightStretch;
        public Angle FieldOfView;
        public StringId FunctionName1;
        public StringId FunctionName2;
        public short Unknown2;
        public short Unknown3;
        public uint Unknown4;
        public TagFunction Function1 = new TagFunction { Data = new byte[0] };
        public StringId FunctionName3;
        public StringId FunctionName4;
        public short Unknown5;
        public short Unknown6;
        public uint Unknown7;
        public TagFunction Function2 = new TagFunction { Data = new byte[0] };
        public CachedTagInstance GelMap;
        public uint Unknown8;
        public uint Unknown9;
        public uint Unknown10;
        public uint Unknown11;
        public sbyte Unknown12;
        public sbyte Unknown13;
        public sbyte Unknown14;
        public sbyte Unknown15;
        public CachedTagInstance LensFlare;

        public enum TypeValue : short
        {
            Sphere,
            Projective,
        }
    }
}