using System;
using TagTool.Common;
using TagTool.Tags;
using static System.Runtime.InteropServices.CharSet;
using TagTool.IO;

namespace TagTool.Cache
{
    [TagStructure(Size = 0x458)]
    public class ModPackageHeader
    {
        public Tag Signature = new Tag("mod!");

        public ModPackageMajorVersion VersionMajor = ModPackageMajorVersion.Current;

        public short VersionMinor;

        public uint FileSize;

        [TagField(Length = 0x14)]
        public byte[] SHA1 = new byte[0x14];

        public MapFlags MapFlags;

        public ModifierFlags ModifierFlags;

        public ModPackageSectionTable SectionTable;

        [TagField(Length = 0x226)]
        public byte[] RSAPublicKey;

        [TagField(Length = 0x200)]
        public byte[] RSASignature;

        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
    }

    public enum ModPackageMajorVersion : short
    {
        Current = 4
    }

    [TagStructure(Size = 0x470)]
    public class ModPackageMetadata
    {
        public Tag Signature = new Tag("desc");

        [TagField(CharSet = Unicode, Length = 32)]
        public string Name;

        [TagField(CharSet = Ansi, Length = 32)]
        public string Author;

        [TagField(CharSet = Ansi, Length = 512)]
        public string Description;

        public int BuildDateHigh;
        public int BuildDateLow;

        public short VersionMajor = 1;
        public short VersionMinor;

        [TagField(CharSet = Ansi, Length = 512)]
        public string URL;
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
        Files,

        SectionCount
    }

    [Flags]
    public enum MapFlags : int
    {
        MultiplayerMaps,
        CampaignMaps,
        MainmenuMaps,
        FirefightMaps
    }

    [Flags]
    public enum ModifierFlags : int
    {
        None = 0,
        mainmenu = 1 << 0,
        multiplayer = 1 << 1,
        campaign = 1 << 2,
        firefight = 1 << 3,
        SignedBit = 1 << 4,
        character = 1 << 5
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
        public uint Offset;
    }

    [TagStructure(Size = 0x8)]
    public class ModPackageSectionHeader
    {
        /// <summary>
        /// Size of the section
        /// </summary>
        public uint Size;
        /// <summary>
        /// Offset of the section in the file
        /// </summary>
        public uint Offset;

        public ModPackageSectionHeader(uint size, uint offset)
        {
            Size = size;
            Offset = offset;
        }

        public ModPackageSectionHeader(EndianReader reader)
        {
            Size = reader.ReadUInt32();
            Offset = reader.ReadUInt32();
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
        public uint TableOffset;

        public GenericSectionEntry(int count, uint tableOffset)
        {
            Count = count;
            TableOffset = tableOffset;
        }

        public GenericSectionEntry(EndianReader reader)
        {
            Count = reader.ReadInt32();
            TableOffset = reader.ReadUInt32();
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
        public uint Size;
        public uint Offset;

        public GenericTableEntry(uint size, uint offset)
        {
            Size = size;
            Offset = offset;
        }

        public GenericTableEntry(EndianReader reader)
        {
            Size = reader.ReadUInt32();
            Offset = reader.ReadUInt32();
        }

        public void Write(EndianWriter writer)
        {
            writer.Write(Size);
            writer.Write(Offset);
        }
    }

    [TagStructure(Size = 0x28)]
    public class CacheTableEntry
    {
        public uint Size;
        public uint Offset;
        [TagField(Length = 0x20)]
        public string CacheName;

        public CacheTableEntry(uint size, uint offset, string name)
        {
            Size = size;
            Offset = offset;
            CacheName = name;
        }

        public CacheTableEntry() {}
    }

    [TagStructure(Size = 0x10)]
    public class CacheMapTableEntry
    {
        public int Size;
        public uint Offset;
        public int CacheIndex;
        public int MapId;

        public CacheMapTableEntry(int size, uint offset, int cacheIndex, int mapId)
        {
            Size = size;
            Offset = offset;
            CacheIndex = cacheIndex;
            MapId = mapId;
        }

        public CacheMapTableEntry(EndianReader reader)
        {
            Size = reader.ReadInt32();
            Offset = reader.ReadUInt32();
            CacheIndex = reader.ReadInt32();
            MapId = reader.ReadInt32();
        }

        public void Write(EndianWriter writer)
        {
            writer.Write(Size);
            writer.Write(Offset);
            writer.Write(CacheIndex);
            writer.Write(MapId);
        }
    }


    [TagStructure(Size = 0x108)]
    public class FileTableEntry
    {
        public uint Size;

        public uint Offset;

        [TagField(Length = 256)]
        public string Path;
    }
}
