using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    // TODO: Update this for cert_ms30_oct19
    [TagStructure(Name = "texture_render_list", Tag = "trdf", Size = 0x48, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline235640)]
    [TagStructure(Name = "texture_render_list", Tag = "trdf", Size = 0x3C, MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline604673)]
    //[TagStructure(Name = "texture_render_list", Tag = "trdf", Size = 0x60, MinVersion = CacheVersion.HaloOnline700123, MaxVersion = CacheVersion.HaloOnline700123)]
    public class TextureRenderList : TagStructure
	{
        public List<Bitmap> Bitmaps;
        public List<Light> Lights;
        public List<Bink> Binks;
        public List<Mannequin> Mannequins;
        public List<Weapon> Weapons;
        [TagField(MaxVersion = CacheVersion.HaloOnline235640)]
        public uint Unknown;
        [TagField(MaxVersion = CacheVersion.HaloOnline235640)]
        public uint Unknown2;
        [TagField(MaxVersion = CacheVersion.HaloOnline235640)]
        public uint Unknown3;

        [TagStructure(Size = 0x110)]
        public class Bitmap : TagStructure
		{
            public int Index;
            [TagField(Length = 256)] public string Filename;
            public int Unknown;
            public int Width;
            public int Height;
        }

        [TagStructure(Size = 0x1C, MaxVersion = CacheVersion.HaloOnline604673)]
        [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloOnline700123)]
        public class Light : TagStructure
		{
            [TagField(MaxVersion = CacheVersion.HaloOnline604673)]
            public List<UnknownBlock> Unknown;

            [TagField(MinVersion = CacheVersion.HaloOnline700123)]
            public uint U1;
            [TagField(MinVersion = CacheVersion.HaloOnline700123)]
            public uint U2;
            [TagField(MinVersion = CacheVersion.HaloOnline700123)]
            public uint U3;

            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public float Unknown5;

            [TagStructure(Size = 0x28)]
            public class UnknownBlock : TagStructure
			{
                public float Unknown;
                public float Unknown2;
                public float Unknown3;
                public Angle Unknown4;
                public Angle Unknown5;
                public Angle Unknown6;
                public CachedTag Light;
            }
        }

        [TagStructure(Size = 0x30)]
        public class Bink : TagStructure
		{
            [TagField(Length = 32)] public string Name;
            public CachedTag Bink2;
        }

        [TagStructure(Size = 0x4C, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline235640)]
        [TagStructure(Size = 0x5C, MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline604673)]
        public class Mannequin : TagStructure
		{
            public int Unknown;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public int Unknown2;
            public CachedTag Biped;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public CachedTag Animation;

            [TagField(MaxVersion = CacheVersion.HaloOnline235640)]
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
        }

        [TagStructure(Size = 0x64, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline235640)]
        [TagStructure(Size = 0x94, MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline604673)]
        public class Weapon : TagStructure
		{
            [TagField(Length = 32)]
            public string Name;
            [TagField(Length = 32, MinVersion = CacheVersion.HaloOnline498295)]
            public string Name2;
            [TagField(MaxVersion = CacheVersion.HaloOnline235640)]
            public CachedTag Weapon2;
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

            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public int Unknown14;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public Angle UnknownAngle1;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public Angle UnknownAngle2;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public Angle UnknownAngle3;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public float Unknown15;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public float Unknown16;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public float Unknown17;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public float Unknown18;
        }
    }
}