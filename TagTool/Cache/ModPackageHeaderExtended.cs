using System;
using TagTool.Common;
using TagTool.Tags;
using static System.Runtime.InteropServices.CharSet;
using TagTool.IO;

namespace TagTool.Cache
{
    [TagStructure(Size = 0x30)]
    public class ModPackageHeaderExtended
    {
        public Tag Signature = new Tag("mod!");

        public ModPackageVersion Version = ModPackageVersion.Extended;

        public int FileSize;

        [TagField(Length = 0x14)]
        public byte[] SHA1 = new byte[0x14];

        public MapFlags MapFlags;

        public ModifierFlags ModifierFlags;

        public ModPackageSectionTable SectionTable;

    }

    [TagStructure(Size = 0xBC)]
    public class ModPackageMetadata
    {
        public Tag Signature = new Tag("desc");

        [TagField(CharSet = Unicode, Length = 16)]
        public string Name;

        [TagField(CharSet = Ansi, Length = 16)]
        public string Author;

        [TagField(CharSet = Ansi, Length = 128)]
        public string Description;

        public int BuildDateHigh;
        public int BuildDateLow;
    }

    public enum ModPackageSection : int
    {
        Metadata,
        Tags,
        TagNames,
        Resources,
        MapFiles,
        CampaignFiles,
        Fonts,
        StringIds,
        Locales,

        SectionCount
    }

    public enum ModPackageVersion : int
    {
        Unknown = 0,
        Basic = 1,
        Extended = 2
    }

    [Flags]
    public enum MapFlags : int
    {
        MultiplayerMaps,
        CampaignMaps,
        MainmenuMaps
    }

    [Flags]
    public enum ModifierFlags : int
    {
        SpecialShitGoesHere
    }

    [TagStructure(Size = 0x8)]
    public class ModPackageSectionTable
    {
        /// <summary>
        /// Number of sections in the table
        /// </summary>
        public int Count;
        /// <summary>
        /// Offset of the section table in the file
        /// </summary>
        public int Offset;
    }

    [TagStructure(Size = 0x8)]
    public class ModPackageSectionHeader
    {
        /// <summary>
        /// Size of the section
        /// </summary>
        public int Size;
        /// <summary>
        /// Offset of the section in the file
        /// </summary>
        public int Offset;

        public ModPackageSectionHeader(int size, int offset)
        {
            Size = size;
            Offset = offset;
        }

        public ModPackageSectionHeader(EndianReader reader)
        {
            Size = reader.ReadInt32();
            Offset = reader.ReadInt32();
        }

        public void Write(EndianWriter writer)
        {
            writer.Write(Size);
            writer.Write(Offset);
        }
    }

    [TagStructure(Size = 0x104)]
    public class ModPackageTagNamesEntry
    {
        public int TagIndex;

        [TagField(Length = 0x100)]
        public string Name;

        public ModPackageTagNamesEntry(int index, string name)
        {
            TagIndex = index;
            Name = name;
        }

        public ModPackageTagNamesEntry() { }
    }

    [TagStructure(Size = 0x8)]
    public class GenericSectionEntry
    {
        /// <summary>
        /// Number of item in the table
        /// </summary>
        public int Count;
        /// <summary>
        /// Offset to the table
        /// </summary>
        public int TableOffset;

        public GenericSectionEntry(int count, int tableOffset)
        {
            Count = count;
            TableOffset = tableOffset;
        }

        public GenericSectionEntry(EndianReader reader)
        {
            Count = reader.ReadInt32();
            TableOffset = reader.ReadInt32();
        }

        public void Write(EndianWriter writer)
        {
            writer.Write(Count);
            writer.Write(TableOffset);
        }

    }

    [TagStructure(Size = 0x8)]
    public class GenericTableEntry
    {
        public int Size;
        public int Offset;

        public GenericTableEntry(int size, int offset)
        {
            Size = size;
            Offset = offset;
        }

        public GenericTableEntry(EndianReader reader)
        {
            Size = reader.ReadInt32();
            Offset = reader.ReadInt32();
        }

        public void Write(EndianWriter writer)
        {
            writer.Write(Size);
            writer.Write(Offset);
        }
    }


}
