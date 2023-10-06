using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TagTool.IO;

namespace TagTool.Fonts
{
    public class FontPackage
    {
        public FileVersion Version { get; set; }
        private EndianFormat Endian { get; set; }
        public List<Font> Fonts { get; set; } = new List<Font>();
        public Dictionary<int, int> FontIdMapping { get; set; } = new Dictionary<int, int>();

        public FontPackage() { }

        public FontPackage(Stream stream)
        {
            Endian = EndianFormat.LittleEndian;
            Load(stream);
        }

        private void Load(Stream stream)
        {
            var reader = new EndianReader(stream, Endian);
            Version = (FileVersion)reader.ReadUInt32();
            
            reader.SeekTo(reader.Position - 4);

            var fileHeader = ReadFileHeader(reader);

            var fontHeaders = new FontHeaderData[fileHeader.FontCount];
            for (int i = 0; i < fileHeader.FontCount; i++)
            {
                reader.SeekTo(fileHeader.Fonts[i].Offset);
                fontHeaders[i] = ReadFontHeader(reader);
            }

            var packageTable = new PackageTableEntryData[fileHeader.PackageTableCount];
            reader.SeekTo(fileHeader.PackageTableOffset);
            for (int i = 0; i < fileHeader.PackageTableCount; i++)
            {
                var entry = new PackageTableEntryData();
                entry.FirstCharacterKey = reader.ReadUInt32();
                entry.LastCharacterKey = reader.ReadUInt32();
                packageTable[i] = entry;
            }

            FontIdMapping = new Dictionary<int, int>();
            for (int i = 0; i < fileHeader.FontIdMapping.Length; i++)
            {
                if (fileHeader.FontIdMapping[i] != -1)
                    FontIdMapping.Add(i, fileHeader.FontIdMapping[i]);
            }

            for (int i = 0; i < fileHeader.FontCount; i++)
            {
                var fontHeader = fontHeaders[i];

                reader.SeekTo(fileHeader.Fonts[i].Offset + fontHeader.KerningPairsOffset);

                var pairs = new List<(byte chr, sbyte offset)>();
                for (int p = 0; p < fontHeader.KerningPairsCount; p++)
                    pairs.Add((reader.ReadByte(), reader.ReadSByte()));

                var kerningPairs = new List<KerningPair>();
                for (int j = 1; j < fontHeader.KerningPairIndices.Length; j++)
                {
                    byte firstIndex = fontHeader.KerningPairIndices[j - 1];
                    byte lastIndex = fontHeader.KerningPairIndices[j];
                    if (lastIndex <= firstIndex)
                        continue;

                    byte first_char = (byte)(j - 1);
                    for (int k = firstIndex; k < lastIndex; k++)
                    {
                        var pair = pairs[k];
                        if (pair.offset == 0)
                            continue;
                        kerningPairs.Add(new KerningPair(first_char, pair.chr, pair.offset));
                    }
                }

                Fonts.Add(new Font()
                {
                    Version = (Font.FontVersion)fontHeader.Version,
                    Name = fontHeader.Name,
                    AscendHeight = fontHeader.AscendHeight,
                    DescendHeight = fontHeader.DescendHeight,
                    LeadWidth = fontHeader.LeadWidth,
                    LeadHeight = fontHeader.LeadHeight,
                    FontScale = fontHeader.FontScale,
                    Characters = new List<FontCharacter>(),
                    KerningPairs = kerningPairs,
                });
            }

            for (int i = 0; i < fileHeader.PackageTableCount; i++)
            {
                reader.SeekTo((i + 1) * GetFontPackageSize());
                long baseOffset = reader.Position;

                var packageHeader = ReadPackageHeader(reader);
                var characterTable = new FontCharacterLocationData[packageHeader.TableCount];
                var characters = new FontCharacterData[packageHeader.TableCount];

                reader.SeekTo(baseOffset + packageHeader.TableOffset);
                for (int j = 0; j < packageHeader.TableCount; j++)
                    characterTable[j] = ReadFontCharacterLocation(reader);

                for (int j = 0; j < packageHeader.TableCount; j++)
                {
                    reader.SeekTo(baseOffset + characterTable[j].DataOffset);
                    characters[j] = ReadFontCharacter(reader);
                }

                for (int j = 0; j < characterTable.Length; j++)
                {
                    var fontCharacter = characters[j];
                    uint key = characterTable[j].CharacterKey;
                    int fontIndex = (int)(key >> 16);

                    var character = new FontCharacter()
                    {
                        Character = (ushort)(key & 0xffff),
                        PackedData = fontCharacter.PackedData,
                        Width = fontCharacter.BitmapWidth,
                        Height = fontCharacter.BitmapHeight,
                        OriginX = fontCharacter.BitmapOriginX,
                        OriginY = fontCharacter.BitmapOriginY,
                        DisplayWidth = fontCharacter.CharacterWidth,
                    };
                    Fonts[fontIndex].Characters.Add(character);
                }
            }
        }

        public void Save(Stream stream)
        {
            if (Fonts.Count > GetMaxFontCount())
                throw new InvalidOperationException("Too many fonts");

            var writer = new EndianWriter(stream, Endian);

            var packages = new List<IntermediatePackage>();
            var locationData = new List<FontCharacterLocationData>();
            var characterDataStream = new MemoryStream();
            var characterWriter = new EndianWriter(characterDataStream, Endian);
            var fontIndices = new List<int>();

            var fileHeader = new FileHeaderData();
            fileHeader.Version = (uint)Version;
            fileHeader.FontCount = Fonts.Count;
            fileHeader.Fonts = new FontEntryData[GetMaxFontCount()];
            for (int i = 0; i < fileHeader.Fonts.Length; i++)
                fileHeader.Fonts[i] = new FontEntryData();
            fileHeader.FontIdMapping = new int[GetMaxFontCount()];
            fileHeader.FontHeadersOffset = 0;
            fileHeader.FontHeadersSize = 0;
            fileHeader.PackageTableOffset = 0;
            fileHeader.PackageTableCount = 0;
            WriteFileHeader(writer, fileHeader);

            packages.Add(new IntermediatePackage());

            var fontHeaders = new List<FontHeaderData>();

            for (int i = 0; i < Fonts.Count; i++)
            {
                var font = Fonts[i];

                for (int j = 0; j < font.Characters.Count; j++)
                {
                    var character = font.Characters[j];
                    uint key = ((uint)i << 16) | character.Character;

                    var currentPackage = packages[packages.Count - 1];

                    if (!currentPackage.FontIndices.Contains(i))
                        currentPackage.FontIndices.Add(i);

                    var characterData = new MemoryStream();
                    var tmpWriter = new EndianWriter(characterData, Endian);
                    WriteFontCharacter(tmpWriter, new FontCharacterData()
                    {
                        BitmapWidth = character.Width,
                        BitmapHeight = character.Height,
                        BitmapOriginX = character.OriginX,
                        BitmapOriginY = character.OriginY,
                        CharacterWidth = character.DisplayWidth,
                        PackedSize = character.PackedData.Length,
                    });
                    tmpWriter.Write(character.PackedData);
                    tmpWriter.Flush();

                    long newDataSize = ((currentPackage.CharacterData.Length + characterData.Length) + 7) & ~7;
                    long newLocationDataSize = (currentPackage.LocationData.Count + 1) * GetCharacterLocationSize();
                    if (((GetPackageHeaderSize() + newDataSize + newLocationDataSize)) > GetFontPackageSize())
                        packages.Add(currentPackage = new IntermediatePackage());

                    currentPackage.LocationData.Add(new FontCharacterLocationData()
                    {
                        CharacterKey = key,
                        DataOffset = (uint)(currentPackage.CharacterData.Position + GetPackageHeaderSize())
                    });
                    currentPackage.CharacterData.Write(characterData.ToArray(), 0, (int)characterData.Length);
                    StreamUtil.Align(currentPackage.CharacterData, 8);
                }
            }

            for (int i = 0; i < packages.Count; i++)
            {
                var package = packages[i];
                Debug.Assert(package.CharacterData.Length <= GetFontPackageSize());
                package.LocationData = package.LocationData.OrderBy(x => x.CharacterKey).ToList();
            }

            fileHeader.FontHeadersOffset = (uint)stream.Position;

            for (int i = 0; i < Fonts.Count; i++)
            {
                long fontDataOffset = stream.Position;

                var header = BuidFontHeader(Fonts[i], out var kerningPairs);
                WriteFontHeader(writer, header);
                fileHeader.Fonts[i].Size = (uint)(GetFontHeaderSize() + header.KerningPairsCount * 2);

                header.KerningPairsOffset = (uint)(stream.Position - fontDataOffset);
                for (int j = 0; j < header.KerningPairsCount; j++)
                {
                    writer.Write((byte)kerningPairs[j].Character);
                    writer.Write(kerningPairs[j].Offset);
                }

                var packageIndices = new List<int>();
                for (int j = 0; j < packages.Count; j++)
                {
                    if (packages[j].FontIndices.Contains(i))
                        packageIndices.Add(j);
                }

                fileHeader.Fonts[i].Offset = (uint)fontDataOffset;
                fileHeader.Fonts[i].FirstPackageIndex = (short)packageIndices.First();
                fileHeader.Fonts[i].PackageCount = (short)packageIndices.Count;
            }
            fileHeader.FontHeadersSize = (uint)(stream.Position - fileHeader.FontHeadersOffset);

            fileHeader.PackageTableOffset = (uint)stream.Position;
            for (int i = 0; i < packages.Count; i++)
            {
                IntermediatePackage package = packages[i];
                var entry = new PackageTableEntryData();
                entry.FirstCharacterKey = package.LocationData[0].CharacterKey;
                entry.LastCharacterKey = package.LocationData[package.LocationData.Count - 1].CharacterKey;
                writer.Write(entry.FirstCharacterKey);
                writer.Write(entry.LastCharacterKey);
            }

            StreamUtil.Fill(stream, 0, (int)(GetFontPackageSize() - stream.Length));

            long packageBase = writer.BaseStream.Position;

            var packageTable = new PackageTableEntryData[packages.Count];
            for (int i = 0; i < packages.Count; i++)
            {
                long dataOffset = stream.Position;

                var package = packages[i];
                var characterTable = new FontCharacterLocationData[package.LocationData.Count];
                for (int j = 0; j < package.LocationData.Count; j++)
                {
                    var entry2 = new FontCharacterLocationData();
                    entry2.CharacterKey = package.LocationData[j].CharacterKey;
                    entry2.DataOffset = (uint)(package.LocationData[j].DataOffset + package.LocationData.Count * GetCharacterLocationSize());
                    characterTable[j] = entry2;
                }

                var packageHeader = new PackageHeaderData();
                packageHeader.DataOffset = (ushort)(package.LocationData.Count * GetCharacterLocationSize() + GetPackageHeaderSize());
                packageHeader.DataSize = (ushort)package.CharacterData.Length;
                packageHeader.TableCount = (short)package.LocationData.Count;
                packageHeader.TableOffset = 8;


                writer.Write(packageHeader.TableOffset);
                writer.Write(packageHeader.TableCount);
                writer.Write(packageHeader.DataOffset);
                writer.Write(packageHeader.DataSize);

                for (int j = 0; j < characterTable.Length; j++)
                {
                    writer.Write(characterTable[j].CharacterKey);
                    writer.Write(characterTable[j].DataOffset);
                }
                writer.Write(package.CharacterData.ToArray());
                StreamUtil.Fill(stream, 0, (int)(GetFontPackageSize() - (stream.Position - dataOffset)));
            }

            stream.Position = 0;
            for (int i = 0; i < fileHeader.FontIdMapping.Length; i++)
                fileHeader.FontIdMapping[i] = -1;
            foreach (var mapping in FontIdMapping)
                fileHeader.FontIdMapping[mapping.Key] = mapping.Value;

            fileHeader.PackageTableCount = packages.Count;


            WriteFileHeader(writer, fileHeader);
        }


        private FontHeaderData BuidFontHeader(Font font, out KerningPairData[] kerningPairs)
        {
            var locationDataSize = font.Version == Font.FontVersion.Version6 ? 8 : 4;

            var fontHeader = new FontHeaderData();
            fontHeader.Version = (uint)font.Version;
            fontHeader.Name = font.Name;
            fontHeader.KerningPairsCount = font.KerningPairs.Count;
            fontHeader.KerningPairsOffset = GetFontHeaderSize();
            fontHeader.LocationTableCount = font.Characters.Max(x => x.Character) + 1;
            fontHeader.LocationTableOffset = (uint)(fontHeader.KerningPairsOffset + 2 * fontHeader.KerningPairsCount);
            fontHeader.CharacterDataOffset = (uint)(fontHeader.LocationTableOffset + fontHeader.LocationTableCount * locationDataSize);
            fontHeader.CharacterCount = font.Characters.Count;
            fontHeader.AscendHeight = (short)font.AscendHeight;
            fontHeader.DescendHeight = (short)font.DescendHeight;
            fontHeader.LeadWidth = (short)font.LeadWidth;
            fontHeader.LeadHeight = (short)font.LeadHeight;
            fontHeader.FallbackLocation = -1;
            fontHeader.FontScale = font.FontScale;

            var intermediatePairs = font.KerningPairs.OrderBy(x => x.FirstCharacter).ThenBy(x => x.SecondCharacter).ToList();

            kerningPairs = new KerningPairData[intermediatePairs.Count];
            fontHeader.KerningPairsCount = intermediatePairs.Count;
            for (int j = 0; j < intermediatePairs.Count; j++)
            {
                kerningPairs[j].Character = (byte)intermediatePairs[j].SecondCharacter;
                kerningPairs[j].Offset = (sbyte)intermediatePairs[j].Offset;
            }

            fontHeader.KerningPairIndices = new byte[256];
            ushort lastCharacter = 0;
            int pairIndex = 0;
            for (int firstCharacter = 0; firstCharacter < fontHeader.KerningPairIndices.Length; firstCharacter++)
            {
                for (; pairIndex < intermediatePairs.Count; pairIndex++)
                {
                    if (intermediatePairs[pairIndex].FirstCharacter >= lastCharacter)
                        break;
                }
                fontHeader.KerningPairIndices[(byte)firstCharacter] = (byte)pairIndex;
                lastCharacter++;
            }

            foreach (var character in font.Characters)
            {
                if (character.PackedData.Length > fontHeader.MaxPackedPixelSizeBytes)
                    fontHeader.MaxPackedPixelSizeBytes = character.PackedData.Length;
                int unpackedSize = (character.Width * character.Height) * 2;
                if (unpackedSize > fontHeader.MaxUnpackedPixelSizebytes)
                    fontHeader.MaxUnpackedPixelSizebytes = unpackedSize;
                fontHeader.TotalPackedPixelSizeBytes += character.PackedData.Length;
                fontHeader.TotalUnpackedPixelSizebytes += unpackedSize;
                fontHeader.CharacterDataSize += (uint)(GetFontCharacterHeaderSize() + character.PackedData.Length);
                fontHeader.CharacterDataSize = (fontHeader.CharacterDataSize + 7u) & ~7u;
            }

            return fontHeader;
        }

        private PackageHeaderData ReadPackageHeader(EndianReader reader)
        {
            var header = new PackageHeaderData();
            header.TableOffset = reader.ReadUInt16();
            header.TableCount = reader.ReadInt16();
            header.DataOffset = reader.ReadUInt16();
            header.DataSize = reader.ReadUInt16();
            return header;
        }
        
        private void WritePackageHeader(EndianWriter writer, PackageHeaderData header)
        {
            writer.Write(header.TableOffset);
            writer.Write(header.TableCount);
            writer.Write(header.DataOffset);
            writer.Write(header.DataSize);
        }

        private FontCharacterLocationData ReadFontCharacterLocation(EndianReader reader)
        {
            var entry = new FontCharacterLocationData();
            entry.CharacterKey = reader.ReadUInt32();
            if (Version == FileVersion.Version4)
                entry.DataOffset = reader.ReadUInt32();
            else
            {
                entry.DataOffset = reader.ReadUInt16();
                reader.ReadUInt16(); // pad
            }
            return entry;
        }

        private void WriteFontCharacterLocation(EndianWriter writer, FontCharacterLocationData location)
        {
            var entry = new FontCharacterLocationData();
            writer.Write(entry.CharacterKey);
            if (Version == FileVersion.Version4)
                writer.Write(entry.DataOffset);
            else
            {
                writer.Write((ushort)entry.DataOffset);
                writer.Write((ushort)0); // pad
            }
        }

        private FontCharacterData ReadFontCharacter(EndianReader reader)
        {
            var character = new FontCharacterData();
            character.CharacterWidth = reader.ReadInt16();
            if (Version == FileVersion.Version4)
            {
                reader.ReadUInt16(); // pad
                character.PackedSize = reader.ReadInt32();
            }
            else
            {
                character.PackedSize = reader.ReadUInt16();
            }
            character.BitmapWidth = reader.ReadInt16();
            character.BitmapHeight = reader.ReadInt16();
            character.BitmapOriginX = reader.ReadInt16();
            character.BitmapOriginY = reader.ReadInt16();
            character.PackedData = reader.ReadBytes(character.PackedSize);
            return character;
        }

        private void WriteFontCharacter(EndianWriter writer, FontCharacterData character)
        {
            writer.Write(character.CharacterWidth);
            if (Version == FileVersion.Version4)
            {
                writer.Write((short)0); // pad
                writer.Write(character.PackedSize);
            }
            else
            {
                writer.Write((short)character.PackedSize);
            }
            writer.Write(character.BitmapWidth);
            writer.Write(character.BitmapHeight);
            writer.Write(character.BitmapOriginX);
            writer.Write(character.BitmapOriginY);
        }

        private void WriteFontHeader(EndianWriter writer, FontHeaderData header)
        {
            var nameBytes = Encoding.ASCII.GetBytes(header.Name);
            var staticNameBytes = new byte[32];
            Array.Copy(nameBytes, staticNameBytes, nameBytes.Length);

            writer.Write(header.Version);
            writer.Write(staticNameBytes);
            writer.Write(header.AscendHeight);
            writer.Write(header.DescendHeight);
            writer.Write(header.LeadHeight);
            writer.Write(header.LeadWidth);
            writer.Write(header.KerningPairsOffset);

            writer.Write(header.KerningPairsCount);
            writer.Write(header.KerningPairIndices);

            writer.Write(header.LocationTableOffset);
            writer.Write(header.LocationTableCount);
            writer.Write(header.CharacterCount);
            writer.Write(header.CharacterDataOffset);
            writer.Write(header.CharacterDataSize);
            if (Version == FileVersion.Version4)
                writer.Write(header.FallbackLocation);
            else
                writer.Write((uint)header.FallbackLocation);
            writer.Write(header.MaxPackedPixelSizeBytes);
            writer.Write(header.MaxUnpackedPixelSizebytes);

            writer.Write(header.TotalPackedPixelSizeBytes);
            writer.Write(header.TotalUnpackedPixelSizebytes);
            if (Version == FileVersion.Version4)
            {
                writer.Write(header.FontScale);
                writer.Write(0);
            }
        }

        private FontHeaderData ReadFontHeader(EndianReader reader)
        {
            var baseOffset = reader.Position;

            var header = new FontHeaderData();
            header.Version = reader.ReadUInt32();
            header.Name = reader.ReadString(32);
            header.AscendHeight = reader.ReadInt16();
            header.DescendHeight = reader.ReadInt16();
            header.LeadHeight = reader.ReadInt16();
            header.LeadWidth = reader.ReadInt16();
            header.KerningPairsOffset = reader.ReadUInt32();

            header.KerningPairsCount = reader.ReadInt32();
            header.KerningPairIndices = reader.ReadBytes(256);

            header.LocationTableOffset = reader.ReadUInt32();
            header.LocationTableCount = reader.ReadInt32();
            header.CharacterCount = reader.ReadInt32();
            header.CharacterDataOffset = reader.ReadUInt32();
            header.CharacterDataSize = reader.ReadUInt32();

            if (Version == FileVersion.Version4)
                header.FallbackLocation = reader.ReadInt64();
            else
                header.FallbackLocation = reader.ReadUInt32();

            header.MaxPackedPixelSizeBytes = reader.ReadInt32();
            header.MaxUnpackedPixelSizebytes = reader.ReadInt32();
            header.TotalPackedPixelSizeBytes = reader.ReadInt32();
            header.TotalUnpackedPixelSizebytes = reader.ReadInt32();

            if (Version == FileVersion.Version4)
            {
                header.FontScale = reader.ReadInt32();
                reader.ReadInt32(); // pad
            }

            return header;
        }

        private FileHeaderData ReadFileHeader(EndianReader reader)
        {
            var fileHeader = new FileHeaderData();
            fileHeader.Version = reader.ReadUInt32();
            fileHeader.FontCount = reader.ReadInt32();

            int maxFonts = GetMaxFontCount();

            fileHeader.Fonts = new FontEntryData[maxFonts];
            for (int i = 0; i < maxFonts; i++)
                fileHeader.Fonts[i] = ReadFontEntry(reader);

            fileHeader.FontIdMapping = new int[maxFonts];
            for (int i = 0; i < maxFonts; i++)
                fileHeader.FontIdMapping[i] = reader.ReadInt32();

            fileHeader.FontHeadersOffset = reader.ReadUInt32();
            fileHeader.FontHeadersSize = reader.ReadUInt32();
            fileHeader.PackageTableOffset = reader.ReadUInt32();
            fileHeader.PackageTableCount = reader.ReadInt32();
            return fileHeader;
        }

        private void WriteFileHeader(EndianWriter writer, FileHeaderData fileHeader)
        {
            writer.Write(fileHeader.Version);
            writer.Write(fileHeader.FontCount);
            for (int i = 0; i < fileHeader.Fonts.Length; i++)
            {
                writer.Write(fileHeader.Fonts[i].Offset);
                writer.Write(fileHeader.Fonts[i].Size);
                writer.Write(fileHeader.Fonts[i].FirstPackageIndex);
                writer.Write(fileHeader.Fonts[i].PackageCount);
            }

            for (int i = 0; i < fileHeader.FontIdMapping.Length; i++)
                writer.Write(fileHeader.FontIdMapping[i]);
            writer.Write(fileHeader.FontHeadersOffset);
            writer.Write(fileHeader.FontHeadersSize);
            writer.Write(fileHeader.PackageTableOffset);
            writer.Write(fileHeader.PackageTableCount);
        }

        private FontEntryData ReadFontEntry(EndianReader reader)
        {
            var entry = new FontEntryData();
            entry.Offset = reader.ReadUInt32();
            entry.Size = reader.ReadUInt32();
            entry.FirstPackageIndex = reader.ReadInt16();
            entry.PackageCount = reader.ReadInt16();
            return entry;
        }

        public int GetMaxFontCount()
        {
            if (Version == FileVersion.Version4)
                return 64;
            else
                return 16;
        }

        public uint GetFontPackageSize()
        {
            if (Version == FileVersion.Version4)
                return 0xC000;
            else
                return 0x8000;
        }

        public int GetFontCharacterHeaderSize()
        {
            if (Version == FileVersion.Version4)
                return 0x10;
            else
                return 0xC;
        }

        public int GetPackageHeaderSize()
        {
            return 8;
        }

        private uint GetFontHeaderSize()
        {
            if (Version == FileVersion.Version4)
                return 0x168;
            else
                return 0x15C;
        }

        private int GetCharacterLocationSize()
        {
            return 8;
        }

        public enum FileVersion : uint
        {
            Version3 = 0xC0000003,
            Version4 = 0xC0000004
        }

        private class FileHeaderData
        {
            public uint Version;
            public int FontCount;
            public FontEntryData[] Fonts;
            public int[] FontIdMapping;
            public uint FontHeadersOffset;
            public uint FontHeadersSize;
            public uint PackageTableOffset;
            public int PackageTableCount;
        }

        private class FontEntryData
        {
            public uint Offset;
            public uint Size;
            public short FirstPackageIndex;
            public short PackageCount;
        }

        private class FontHeaderData
        {
            public uint Version;
            public string Name;
            public short AscendHeight;
            public short DescendHeight;
            public short LeadHeight;
            public short LeadWidth;
            public uint KerningPairsOffset;
            public int KerningPairsCount;
            public byte[] KerningPairIndices;
            public uint LocationTableOffset;
            public int LocationTableCount;
            public int CharacterCount;
            public uint CharacterDataOffset;
            public uint CharacterDataSize;
            public long FallbackLocation;
            public int MaxPackedPixelSizeBytes;
            public int MaxUnpackedPixelSizebytes;
            public int TotalPackedPixelSizeBytes;
            public int TotalUnpackedPixelSizebytes;
            public int FontScale; // mcc
        }

        private class PackageTableEntryData
        {
            public uint FirstCharacterKey;
            public uint LastCharacterKey;
        }

        private class PackageHeaderData
        {
            public ushort TableOffset;
            public short TableCount;
            public ushort DataOffset;
            public ushort DataSize;
        }

        private class FontCharacterData
        {
            public short CharacterWidth;
            public int PackedSize;
            public short BitmapWidth;
            public short BitmapHeight;
            public short BitmapOriginX;
            public short BitmapOriginY;
            public byte[] PackedData;
        }

        private class FontCharacterLocationData
        {
            public uint CharacterKey;
            public uint DataOffset;
        }

        private struct KerningPairData
        {
            public byte Character;
            public sbyte Offset;
        }

        private class IntermediatePackage
        {
            public List<FontCharacterLocationData> LocationData = new List<FontCharacterLocationData>();
            public MemoryStream CharacterData = new MemoryStream();
            public List<int> FontIndices = new List<int>();
        }
    }

    public class Font
    {
        public enum FontVersion : uint
        {
            Version5 = 0xF0000005,
            Version6 = 0xF0000006
        }

        public FontVersion Version { get; set; }
        public string Name { get; set; } = "";
        public int AscendHeight { get; set; }
        public int DescendHeight { get; set; }
        public int LeadWidth { get; set; }
        public int LeadHeight { get; set; }
        public int FontScale { get; set; } // mcc
        public List<FontCharacter> Characters { get; set; } = new List<FontCharacter>();
        public List<KerningPair> KerningPairs { get; set; } = new List<KerningPair>();
    }

    public class FontCharacter
    {
        public ushort Character { get; set; }
        public short DisplayWidth { get; set; }
        public short Width { get; set; }
        public short Height { get; set; }
        public short OriginX { get; set; }
        public short OriginY { get; set; }
        public byte[] PackedData { get; set; }
    }

    public class KerningPair
    {
        public ushort FirstCharacter;
        public ushort SecondCharacter;
        public int Offset;

        public KerningPair(ushort firstCharacter, ushort secondCharacter, int offset)
        {
            FirstCharacter = firstCharacter;
            SecondCharacter = secondCharacter;
            Offset = offset;
        }
    }
}
