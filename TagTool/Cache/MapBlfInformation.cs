using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache
{
    [TagStructure(Size = 0xC)]
    public class BlfChunkHeader
    {
        public Tag Signature;
        public int Length;
        public int Type;
    }

    [TagStructure(Size = 0x24)]
    public class BlfStartOfFile
    {
        public short Version;
        public short Unknown;

        [TagField(Length = 0x20)]
        public string InternalName;
    }

    [TagStructure(Size = 0x4)]
    public class BlfEndOfFile
    {
        public int Unknown;
    }

    [TagStructure(Size = 0x14)]
    public class BlfEndOfFileSP
    {
        [TagField(Length = 0x10)]
        public byte[] Unused;
        public Tag Signature;
    }

    [TagStructure(Size = 0x10D)]
    public class UnknownSP
    {
        [TagField(Length = 0x10D)]
        public byte[] Unknown;
    }

    // different than actual size, (problems with header sizes and endianness)
    [TagStructure(Size = 0x89A4, MinVersion = CacheVersion.HaloOnline106708)]
    public class MapBlfInformation
    {
        public int MapId;
        public BlfScenarioFlags Flags;

        [TagField(Length = 0xC)]
        public NameUnicode32[] MapNames;

        [TagField(Length = 0xC)]
        public NameUnicode128[] MapNamesInternal;

        [TagField(Length = 0x100, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        public string MapImagePath;

        [TagField(Length = 0x100, CharSet = System.Runtime.InteropServices.CharSet.Ansi)]
        public string MapNameInternal;

        public int ScenarioTagIndex;

        [TagField(Length = 0x6)]
        public byte[] Unknown1;

        [TagField(Length = 0xB)]
        public byte[] TeamCounts;

        [TagField(Length = 0x7)]
        public byte[] Unknown2;

        [TagField(Length = 0x8)]
        public BlfMapMissionInformation[] MissionInformations;

        [TagField(Length = 0x60)]
        public byte[] Unknown3;
    }

    [TagStructure(Size = 0x40)]
    public class NameUnicode32
    {
        [TagField(CharSet = System.Runtime.InteropServices.CharSet.Unicode, Length = 0x40)]
        public string Name;
    }

    [TagStructure(Size = 0x100)]
    public class NameUnicode128
    {
        [TagField(CharSet = System.Runtime.InteropServices.CharSet.Unicode, Length = 0x100)]
        public string Name;
    }

    [Flags]
    public enum BlfScenarioFlags : int
    {
        Unknown0 = 0,
        Unknown1 = 1 << 0,
        Visible = 1 << 1,
        GeneratesFilm = 1 << 2,
        IsMainmenu = 1 << 3,
        IsCampaign = 1 << 4,
        IsMultiplayer = 1 << 5,
        IsDlc = 1 << 6,
        Unknown8 = 1 << 7,
        Unknown9 = 1 << 8,
        IsFirefight = 1 << 9,
        IsCinematic = 1 << 10,
        IsForgeOnly = 1 << 11,
        Unknown13 = 1 << 12,
        Unknown14 = 1 << 13,
        Unknown15 = 1 << 14
    }

    [TagStructure(Size = 0xF04)]
    public class BlfMapMissionInformation
    {
        public bool Visible;
        public bool Used;
        public short ZoneIndex;

        [TagField(Length = 0xC)]
        public NameUnicode32[] Names;

        [TagField(Length = 0xC)]
        public NameUnicode128[] Descriptions;
    }
}
