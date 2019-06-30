using System;
using TagTool.Common;
using TagTool.Tags;
using static System.Runtime.InteropServices.CharSet;
using System.Runtime.InteropServices.ComTypes;
using TagTool.IO;

namespace TagTool.Cache
{
    [TagStructure(Size = 0x3C)]
    public class ModPackageHeaderExtended
    {
        public Tag Signature = new Tag("mod!");

        public ModPackageVersion Version = ModPackageVersion.Extended;

        public long FileSize;

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

        public FILETIME BuildDate;
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

    [TagStructure(Size = 0x10)]
    public class ModPackageSectionTable
    {
        /// <summary>
        /// Number of sections in the table
        /// </summary>
        public long Count;
        /// <summary>
        /// Offset of the section table in the file
        /// </summary>
        public long Offset;
    }

    [TagStructure(Size = 0x10)]
    public class ModPackageSectionHeader
    {
        /// <summary>
        /// Size of the section
        /// </summary>
        public long Size;
        /// <summary>
        /// Offset of the section in the file
        /// </summary>
        public long Offset;

        public ModPackageSectionHeader(long size, long offset)
        {
            Size = size;
            Offset = offset;
        }

        public ModPackageSectionHeader(EndianReader reader)
        {
            Size = reader.ReadInt64();
            Offset = reader.ReadInt64();
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
    }

    [TagStructure(Size = 0x10)]
    public class GenericEntry
    {
        /// <summary>
        /// Number of item in the table
        /// </summary>
        public long Count;
        /// <summary>
        /// Offset to the table
        /// </summary>
        public long TableOffset;

        public GenericEntry(long count, long tableOffset)
        {
            Count = count;
            TableOffset = tableOffset;
        }

        public GenericEntry(EndianReader reader)
        {
            Count = reader.ReadInt64();
            TableOffset = reader.ReadInt64();
        }

        public void Write(EndianWriter writer)
        {
            writer.Write(Count);
            writer.Write(TableOffset);
        }

    }

    [TagStructure(Size = 0x10)]
    public class GenericTableEntry
    {
        public long Size;
        public long Offset;

        public GenericTableEntry(long size, long offset)
        {
            Size = size;
            Offset = offset;
        }

        public GenericTableEntry(EndianReader reader)
        {
            Size = reader.ReadInt64();
            Offset = reader.ReadInt64();
        }

        public void Write(EndianWriter writer)
        {
            writer.Write(Size);
            writer.Write(Offset);
        }
    }


}
