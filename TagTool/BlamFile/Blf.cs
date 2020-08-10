using System;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.BlamFile
{
    /// <summary>
    /// Main class for blf format. Reads, parse and writes blf.
    /// </summary>
    public class Blf
    {
        public CacheVersion Version;
        public EndianFormat Format;

        public BlfFileContentFlags ContentFlags;
        public BlfAuthenticationType AuthenticationType;

        public BlfChunkStartOfFile StartOfFile;
        public BlfEndOfFileCRC EndOfFileCRC;
        public BlfEndOfFileRSA EndOfFileRSA;
        public BlfEndOfFileSHA1 EndOfFileSHA1;
        public BlfChunkEndOfFile EndOfFile;
        public BlfCampaign Campaign;
        public BlfScenario Scenario;
        public BlfModPackageReference ModReference;
        public BlfMapVariantTagNames MapVariantTagNames;
        public BlfMapVariant MapVariant;
        public BlfGameVariant GameVariant;
        public BlfContentHeader ContentHeader;
        public BlfMapImage MapImage;
        public byte[] JpegImage;

        public Blf(CacheVersion version)
        {
            Version = version;
        }

        public bool Read(EndianReader reader)
        {
            if (!IsValid(reader))
                return false;

            var deserializer = new TagDeserializer(Version);

            while (!reader.EOF)
            {
                var dataContext = new DataSerializationContext(reader);
                var chunkHeaderPosition = reader.Position;

                var header = (BlfChunkHeader)deserializer.Deserialize(dataContext, typeof(BlfChunkHeader));
                reader.SeekTo(chunkHeaderPosition);

                switch (header.Signature.ToString())
                {
                    case "_blf":
                        ContentFlags |= BlfFileContentFlags.StartOfFile;
                        StartOfFile = (BlfChunkStartOfFile)deserializer.Deserialize(dataContext, typeof(BlfChunkStartOfFile));
                        break;

                    case "_eof":
                        ContentFlags |= BlfFileContentFlags.EndOfFile;
                        var position = reader.Position;
                        EndOfFile = (BlfChunkEndOfFile)deserializer.Deserialize(dataContext, typeof(BlfChunkEndOfFile));
                        AuthenticationType = EndOfFile.AuthenticationType;
                        switch (AuthenticationType)
                        {
                            case BlfAuthenticationType.None:
                                break;
                            case BlfAuthenticationType.CRC:
                                reader.SeekTo(position);
                                EndOfFileCRC = (BlfEndOfFileCRC)deserializer.Deserialize(dataContext, typeof(BlfEndOfFileCRC));
                                break;
                            case BlfAuthenticationType.RSA:
                                reader.SeekTo(position);
                                EndOfFileRSA = (BlfEndOfFileRSA)deserializer.Deserialize(dataContext, typeof(BlfEndOfFileRSA));
                                break;
                            case BlfAuthenticationType.SHA1:
                                reader.SeekTo(position);
                                EndOfFileSHA1 = (BlfEndOfFileSHA1)deserializer.Deserialize(dataContext, typeof(BlfEndOfFileSHA1));
                                break;
                        }
                        return true;

                    case "cmpn":
                        ContentFlags |= BlfFileContentFlags.Campaign;
                        Campaign = (BlfCampaign)deserializer.Deserialize(dataContext, typeof(BlfCampaign));
                        break;

                    case "levl":
                        ContentFlags |= BlfFileContentFlags.Scenario;
                        Scenario = (BlfScenario)deserializer.Deserialize(dataContext, typeof(BlfScenario));
                        break;

                    case "modp":
                        ContentFlags |= BlfFileContentFlags.ModReference;
                        if(header.MajorVersion == (short)BlfModPackageReferenceVersion.Version1)
                        {
                            var v1 = (BlfModPackageReferenceV1)deserializer.Deserialize(dataContext, typeof(BlfModPackageReferenceV1));
                            ModReference = new BlfModPackageReference(v1); // Convert to the new format
                        }
                        else
                        {
                            ModReference = (BlfModPackageReference)deserializer.Deserialize(dataContext, typeof(BlfModPackageReference));
                        }
                        break;

                    case "mapv":
                        ContentFlags |= BlfFileContentFlags.MapVariant;
                        MapVariant = (BlfMapVariant)deserializer.Deserialize(dataContext, typeof(BlfMapVariant));
                        break;
                    case "tagn":
                        ContentFlags |= BlfFileContentFlags.MapVariantTagNames;
                        MapVariantTagNames = (BlfMapVariantTagNames)deserializer.Deserialize(dataContext, typeof(BlfMapVariantTagNames));
                        break;


                    case "mpvr":
                        ContentFlags |= BlfFileContentFlags.GameVariant;
                        GameVariant = (BlfGameVariant)deserializer.Deserialize(dataContext, typeof(BlfGameVariant));
                        break;

                    case "chdr":
                        ContentFlags |= BlfFileContentFlags.ContentHeader;
                        ContentHeader = (BlfContentHeader)deserializer.Deserialize(dataContext, typeof(BlfContentHeader));
                        break;

                    case "mapi":
                        ContentFlags |= BlfFileContentFlags.MapImage;
                        MapImage = (BlfMapImage)deserializer.Deserialize(dataContext, typeof(BlfMapImage));
                        JpegImage = reader.ReadBytes(MapImage.JpegSize);
                        break;

                    case "scnd":
                    case "scnc":
                    case "flmh":
                    case "flmd":
                    case "athr":
                    case "ssig":
                    case "mps2":
                    case "chrt":
                    default:
                        throw new NotImplementedException($"BLF chunk type {header.Signature.ToString()} not implemented!");
                }
            }

            return true;
        }

        public bool Write(EndianWriter writer)
        {
            if (!ContentFlags.HasFlag(BlfFileContentFlags.StartOfFile) || !ContentFlags.HasFlag(BlfFileContentFlags.EndOfFile))
                return false;

            TagSerializer serializer = new TagSerializer(Version, Format);
            writer.Format = Format;
            var dataContext = new DataSerializationContext(writer);
            
            serializer.Serialize(dataContext, StartOfFile);

            if(ContentFlags.HasFlag(BlfFileContentFlags.Scenario))
                serializer.Serialize(dataContext, Scenario);

            if (ContentFlags.HasFlag(BlfFileContentFlags.ContentHeader))
                serializer.Serialize(dataContext, ContentHeader);

            if(ContentFlags.HasFlag(BlfFileContentFlags.MapVariant))
                serializer.Serialize(dataContext, MapVariant);

            if (ContentFlags.HasFlag(BlfFileContentFlags.GameVariant))
                serializer.Serialize(dataContext, GameVariant);

            if (ContentFlags.HasFlag(BlfFileContentFlags.Campaign))
                serializer.Serialize(dataContext, Campaign);

            if (ContentFlags.HasFlag(BlfFileContentFlags.ModReference))
                serializer.Serialize(dataContext, ModReference);

            if (ContentFlags.HasFlag(BlfFileContentFlags.MapVariantTagNames))
                serializer.Serialize(dataContext, MapVariantTagNames);

            if (ContentFlags.HasFlag(BlfFileContentFlags.MapImage))
            {
                if (JpegImage != null && JpegImage.Length > 0)
                {
                    MapImage.JpegSize = JpegImage.Length;
                    serializer.Serialize(dataContext, MapImage);
                    // image is always little endian
                    writer.Format = EndianFormat.LittleEndian;
                    writer.WriteBlock(JpegImage);
                    writer.Format = Format;
                }
                else
                {
                    Console.WriteLine("ERROR: No data, image will not be written to BLF");
                }
            }


            switch (AuthenticationType)
            {
                case BlfAuthenticationType.None:
                    serializer.Serialize(dataContext, EndOfFile);
                    break;
                case BlfAuthenticationType.CRC:
                    serializer.Serialize(dataContext, EndOfFileCRC);
                    break;
                case BlfAuthenticationType.RSA:
                    serializer.Serialize(dataContext, EndOfFileRSA);
                    break;
                case BlfAuthenticationType.SHA1:
                    serializer.Serialize(dataContext, EndOfFileSHA1);
                    break;
            }

            return true;
        }

        /// <summary>
        /// Verifies if the stream points to a valid blf start chunk and set the endian format.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private bool IsValid(EndianReader reader)
        {
            var position = reader.Position;
            try
            {
                Format = FindChunkEndianFormat(reader);
                reader.Format = Format;
            }
            catch(Exception e)
            {
                Console.WriteLine($"BLF file is invalid: {e.Message}");
                return false;
            }

            var deserializer = new TagDeserializer(Version);
            var dataContext = new DataSerializationContext(reader);

            var header = (BlfChunkHeader)deserializer.Deserialize(dataContext, typeof(BlfChunkHeader));
            reader.SeekTo(position);

            if (header.Signature == "_blf")
                return true;
            else
                return false;
        }

        /// <summary>
        /// Find the endian format of the file. Assumes that the reader points to the beginning of the blf file stream.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private EndianFormat FindChunkEndianFormat(EndianReader reader)
        {
            reader.Format = EndianFormat.LittleEndian;
            var startOfFile = reader.Position;
            var chunkHeaderSize = (int)TagStructure.GetTagStructureInfo(typeof(BlfChunkHeader), Version).TotalSize;
            reader.SeekTo(startOfFile + chunkHeaderSize);
            if(reader.ReadInt16() == -2)
            {
                reader.SeekTo(startOfFile);
                return EndianFormat.LittleEndian;
            }
            else
            {
                reader.SeekTo(startOfFile + chunkHeaderSize);
                reader.Format = EndianFormat.BigEndian;
                if(reader.ReadInt16() == -2)
                {
                    reader.SeekTo(startOfFile);
                    return EndianFormat.BigEndian;
                }
                else
                {
                    reader.SeekTo(startOfFile);
                    throw new Exception("Invalid BOM found in BLF start of file chunk");
                }
            }
        }

        /// <summary>
        /// Convert to specified Cache Version.
        /// </summary>
        /// <param name="targetVersion"></param>
        public void ConvertBlf(CacheVersion targetVersion)
        {
            switch (Version)
            {
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3Beta:
                    switch (targetVersion)
                    {
                        case CacheVersion.Halo3ODST:
                        case CacheVersion.HaloOnline106708:
                            ConvertHalo3ToODSTScenarioChunk();
                            Version = targetVersion;
                            if (targetVersion == CacheVersion.HaloOnline106708)
                                Format = EndianFormat.LittleEndian;
                            break;
                        default:
                            throw new NotImplementedException($"Conversion from Halo 3 to {targetVersion.ToString()} not supported");
                    }
                    break;

                case CacheVersion.Halo3ODST:
                case CacheVersion.HaloOnline106708:
                    if (targetVersion == CacheVersion.HaloOnline106708)
                        Format = EndianFormat.LittleEndian;
                    return;

                default:
                    throw new NotImplementedException($"Conversion from {Version.ToString()} to {targetVersion.ToString()} not supported");
            }
        }

        /// <summary>
        /// Converts a Halo 3 Scenario chunk (levl) to ODST format (HO)
        /// </summary>
        private void ConvertHalo3ToODSTScenarioChunk()
        {
            if (!ContentFlags.HasFlag(BlfFileContentFlags.Scenario))
                return;

            Scenario.InsertionsODST = new BlfScenarioInsertion[9];

            for(int i = 0; i < 9; i++)
            {
                BlfScenarioInsertion ins = null;
                if( i < 4)
                    ins = Scenario.InsertionsH3[i];
                else
                {
                    ins = new BlfScenarioInsertion();
                }
                Scenario.InsertionsODST[i] = ins;
            }
            Scenario.Length = 0x98C0; 
        }
    }

    [Flags]
    public enum BlfFileContentFlags : int
    {
        None = 0,
        StartOfFile = 1 << 0,
        EndOfFile = 1 << 1,
        ContentHeader = 1 << 2,
        MapVariant = 1 << 3,
        Scenario = 1 << 4,
        Campaign = 1 << 5,
        GameVariant = 1 << 6,
        ModReference = 1 << 7,
        MapVariantTagNames = 1 << 8,
        MapImage = 1 << 9,
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

    public enum BlfAuthenticationType : byte
    {
        None,
        CRC,
        SHA1,
        RSA
    }

    [TagStructure(Size = 0xC, Align = 0x1)]
    public class BlfChunkHeader
    {
        public Tag Signature;
        public int Length;
        public short MajorVersion;
        public short MinorVersion;
    }

    [TagStructure(Size = 0x24, Align = 0x1)]
    public class BlfChunkStartOfFile : BlfChunkHeader
    {
        // when -2, order is little endian, else order is big endian. Check byteswapepd BOM to be -2 otherwise invalid.
        public short ByteOrderMarker;
        public short Unknown;

        [TagField(Length = 0x20)]
        public string InternalName;
    }

    [TagStructure(Size = 0x5, Align = 0x1)]
    public class BlfChunkEndOfFile : BlfChunkHeader
    {
        public int AuthenticationDataSize;
        public BlfAuthenticationType AuthenticationType;
    }

    [TagStructure(Size = 0x4, Align = 0x1)]
    public class BlfEndOfFileCRC : BlfChunkEndOfFile
    {
        public uint Checksum;
    }

    [TagStructure(Size = 0x100, Align = 0x1)]
    public class BlfEndOfFileSHA1 : BlfChunkEndOfFile
    {
        [TagField(Length = 0x100)]
        public byte[] Hash;
    }

    [TagStructure(Size = 0x100, Align = 0x1)]
    public class BlfEndOfFileRSA : BlfChunkEndOfFile
    {
        [TagField(Length = 0x100)]
        public byte[] Hash;
    }

    [TagStructure(Size = 0x4D44, Align = 0x1, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0x98B4, Align = 0x1, MinVersion = CacheVersion.Halo3ODST)]
    public class BlfScenario : BlfChunkHeader
    {
        public int MapId;
        public BlfScenarioFlags MapFlags;

        [TagField(Length = 0xC)]
        public NameUnicode32[] Names;

        [TagField(Length = 0xC)]
        public NameUnicode128[] Descriptions;

        [TagField(Length = 0x100)]
        public string ImageName;

        [TagField(Length = 0x100)]
        public string MapName;

        public int MapIndex;
        public int GuiSelectableItemType;
        public byte UnknownTeamCount1;
        public byte UnknownTeamCount2;

        [TagField(Length = 0xB)]
        public byte[] GameEngineTeamCounts;

        public byte Unknown;

        public short Pad;
        public int Unknown2;

        [TagField(Length = 0x4, MaxVersion = CacheVersion.Halo3Retail)]
        public BlfScenarioInsertion[] InsertionsH3;

        [TagField(Length = 0x9, MinVersion = CacheVersion.Halo3ODST)]
        public BlfScenarioInsertion[] InsertionsODST;
    }

    [TagStructure(Size = 0xF08, Align = 0x1, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0xF10, Align = 0x1, MinVersion = CacheVersion.Halo3ODST)]
    public class BlfScenarioInsertion
    {
        public byte Visible;
        public byte Used;
        public short ZoneSetIndex;
        public int Unknown;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public int Unknown2;
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public int Unknown3;

        [TagField(Length = 0xC)]
        public NameUnicode32[] Names;

        [TagField(Length = 0xC)]
        public NameUnicode128[] Descriptions;
    }

    enum BlfModPackageReferenceVersion : short
    {
        Version1 = 1,
        Current = 2
    }

    [TagStructure(Size = 0x44, Align = 0x1)]
    public class BlfModPackageReferenceV1 : BlfChunkHeader
    {
        [TagField(Length = 0x10, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        public string Name;
        [TagField(Length = 0x10)]
        public string Author;
        [TagField(Length = 0x14)]
        public byte[] Hash;
    }

    [TagStructure(Size = 0x484)]
    public class BlfModPackageReference : BlfChunkHeader
    {
        [TagField(Length = 0x14)]
        public byte[] Hash;

        public ModPackageMetadata Metadata;

        public BlfModPackageReference()
        {
            Signature = new Tag("modp");
            Length = (int)typeof(BlfModPackageReference).GetSize() - (int)typeof(BlfChunkHeader).GetSize();
            MajorVersion = (short)BlfModPackageReferenceVersion.Current;
            MinorVersion = 0;
        }

        public BlfModPackageReference(BlfModPackageReferenceV1 v1) : this()
        {
            Hash = v1.Hash;
            Metadata = new ModPackageMetadata();
            Metadata.Name = v1.Name;
            Metadata.Author = v1.Author;
        }
    }

    [TagStructure(Size = 0x130C, Align = 0x1)]
    public class BlfCampaign : BlfChunkHeader
    {
        public int CampaignId;
        public uint Type;

        [TagField(Length = 0xC)]
        public CampaignNameUnicode32[] Names;

        [TagField(Length = 0xC)]
        public NameUnicode128[] Descriptions;

        [TagField(Length = 0x40)]
        public int[] MapIds;

        public uint unknown;
    }

    [TagStructure(Size = 0xE094, Align = 0x1)]
    public class BlfMapVariant : BlfChunkHeader
    {
        public uint VariantVersion;
        public MapVariant MapVariant;
    }

    [TagStructure(Size = 0x10000, Align = 0x1)]
    public class BlfMapVariantTagNames : BlfChunkHeader
    {
        [TagField(Length = 0x100)]
        public TagName[] Names;
    }

    [TagStructure(Size = 0xFC, Align = 0x1)]
    public class BlfContentHeader : BlfChunkHeader
    {
        public uint BuildVersion;
        public ContentItemMetadata Metadata;
    }

    [TagStructure(Size = 0x8, Align = 0x1)]
    public class BlfMapImage : BlfChunkHeader
    {
        public uint Unknown;
        public int JpegSize;
    }

    [TagStructure(Size = 0x100, Align = 0x1)]
    public class TagName
    {
        [TagField(Length = 0x100)]
        public string Name;
    }

    [TagStructure(Size = 0x264, Align = 0x1)]
    public class BlfGameVariant : BlfChunkHeader
    {
        public int GameVariantType;
        [TagField(Length = 0x260)]
        public byte[] Data; // TODO implement all the structures for each variant and take the union
    }

    [TagStructure(Size = 0x40, Align = 0x1)]
    public class NameUnicode32
    {
        [TagField(CharSet = System.Runtime.InteropServices.CharSet.Unicode, Length = 0x20, Align = 0x1)]
        public string Name;
    }

    [TagStructure(Size = 0x100, Align = 0x1)]
    public class NameUnicode128
    {
        [TagField(CharSet = System.Runtime.InteropServices.CharSet.Unicode, Length = 0x80, Align = 0x1)]
        public string Name;
    }

    [TagStructure(Size = 0x80, Align = 0x1)]
    public class CampaignNameUnicode32
    {
        [TagField(CharSet = System.Runtime.InteropServices.CharSet.Unicode, Length = 0x40, Align = 0x1)]
        public string Name;
    }
}
