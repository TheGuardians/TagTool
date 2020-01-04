using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "light", Tag = "ligh", Size = 0x94, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "light", Tag = "ligh", Size = 0xCC, MinVersion = CacheVersion.HaloReach)]
    public class Light : TagStructure
	{
        public uint Flags;
        public TypeValue Type;
        public short Unknown;
        public float LightRange;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float MaxIntensityRange;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown0;

        public float NearWidth;
        public float HeightStretch;
        public Angle FieldOfView;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public Angle UnknownAngle;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float FrustumLightSpread;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown1;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float FrustumMinimumViewDistance;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown2;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short Unknown3;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public short Unknown4;

        public StringId FunctionName1;
        public StringId FunctionName2;
        public short Unknown5;
        public short Unknown6;
        public uint Unknown7;
        public TagFunction Function1 = new TagFunction { Data = new byte[0] };
        public StringId FunctionName3;
        public StringId FunctionName4;
        public short Unknown8;
        public short Unknown9;
        public uint Unknown10;
        public TagFunction Function2 = new TagFunction { Data = new byte[0] };
        public CachedTag GelMap;
        public float Unknown11;
        public float Duration;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown12;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float Unknown13;

        public sbyte Unknown14;
        public sbyte Unknown15;
        public sbyte Unknown16;
        public sbyte Unknown17;
        public CachedTag LensFlare;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float RadiusModifer1;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float RadiusModifer2;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float RadiusModifer3;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown18;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown19;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown20;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown21;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Unknown22;

        public enum TypeValue : short
        {
            Sphere,
            Projective,
        }
    }
}