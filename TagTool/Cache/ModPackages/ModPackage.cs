using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Cache.HaloOnline;
using TagTool.BlamFile;
using TagTool.Tags;

namespace TagTool.Cache
{
    public class ModPackage
    {
        public ModPackageHeader Header { get; set; } = new ModPackageHeader();

        public ModPackageMetadata Metadata { get; set; } = new ModPackageMetadata();

        public List<TagCacheHaloOnline> TagCaches { get; set; } = null;

        public List<Stream> TagCachesStreams { get; set; } = new List<Stream>();

        public List<Dictionary<int, string>> TagCacheNames { get; set; } = new List<Dictionary<int, string>>();

        public ResourceCacheHaloOnline Resources { get; set; } = null;

        public Stream ResourcesStream { get; set; } = new MemoryStream();

        public List<Stream> MapFileStreams { get; set; } = new List<Stream>();

        public Stream CampaignFileStream { get; set; } = new MemoryStream();

        public Dictionary<int, int> MapToCacheMapping { get; set; } = new Dictionary<int, int>();

        public List<int> MapIds = new List<int>();

        public List<string> CacheNames { get; set; } = new List<string>();

        public StringTableHaloOnline StringTable { get; set; }

        public Stream FontPackage;

        public Dictionary<string, Stream> Files { get; set; }

        public ModPackage(FileInfo file = null)
        {
            if (file != null)
                Load(file);
            else
            {
                // init a single cache
                var tagStream = new MemoryStream();
                TagCachesStreams.Add(tagStream);

                var names = new Dictionary<int, string>();
                var tags = new TagCacheHaloOnline(tagStream, names);
                TagCaches = new List<TagCacheHaloOnline>();
                TagCaches.Add(tags);
                TagCacheNames.Add(names);
                Files = new Dictionary<string, Stream>();

                Resources = new ResourceCacheHaloOnline(CacheVersion.HaloOnline106708, ResourcesStream);
                Header.SectionTable = new ModPackageSectionTable();
            }
        }

        public void AddMap(MemoryStream mapStream, int mapId, int cacheIndex)
        {
            MapFileStreams.Add(mapStream);
            MapToCacheMapping.Add(MapFileStreams.Count - 1, cacheIndex);
            MapIds.Add(mapId);
        }

        public void Load(FileInfo file)
        {
            if (!file.Exists)
                throw new FileNotFoundException(file.FullName);

            if (file.Length < typeof(ModPackageHeader).GetSize())
                throw new FormatException(file.FullName);

            using (var stream = file.OpenRead())
            using (var reader = new EndianReader(stream, leaveOpen: true))
            {
                var dataContext = new DataSerializationContext(reader);
                var deserializer = new TagDeserializer(CacheVersion.HaloOnline106708);

                Header = deserializer.Deserialize<ModPackageHeader>(dataContext);

                ReadMetadataSection(reader, dataContext, deserializer);
                ReadTagsSection(reader, dataContext, deserializer);
                ReadTagNamesSection(reader, dataContext, deserializer);
                ReadResourcesSection(reader);
                ReadMapFilesSection(reader);
                ReadCampaignFileSection(reader);
                ReadStringIdSection(reader);
                ReadFontSection(reader);
                ReadFileEntries(reader, dataContext, deserializer);

                int tagCacheCount = TagCachesStreams.Count;

                TagCaches = new List<TagCacheHaloOnline>();
                for (int i = 0; i < tagCacheCount; i++)
                {
                    TagCaches.Add(new TagCacheHaloOnline(TagCachesStreams[i], TagCacheNames[i]));
                }
                
                Resources = new ResourceCacheHaloOnline(CacheVersion.HaloOnline106708, ResourcesStream);
            }
        }

        public void Save(FileInfo file)
        {
            if (!file.Directory.Exists)
                file.Directory.Create();

            using (var packageStream = file.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite))
            using (var writer = new EndianWriter(packageStream, leaveOpen: true))
            {
                var serializer = new TagSerializer(CacheVersion.HaloOnline106708);
                var dataContext = new DataSerializationContext(writer);
                
                packageStream.SetLength(0);

                //
                // reserve header space
                //

                writer.Write(new byte[typeof(ModPackageHeader).GetSize()]);

                //
                // build section table
                //

                Header.SectionTable.Count = (int)ModPackageSection.SectionCount;
                Header.SectionTable.Offset = (uint)writer.BaseStream.Position;

                writer.Write(new byte[typeof(ModPackageSectionHeader).GetSize() * (int)ModPackageSection.SectionCount]);


                uint size;
                uint offset;

                //
                // Write metadata
                //

                offset = (uint)writer.BaseStream.Position;
                WriteMetadataSection(dataContext, serializer);
                size = (uint)(writer.BaseStream.Position - offset);
                WriteSectionEntry((int)ModPackageSection.Metadata, writer, size, offset);

                //
                // Write tag cache
                //

                offset = (uint)writer.BaseStream.Position;
                WriteTagsSection(writer, dataContext, serializer);
                size = (uint)(writer.BaseStream.Position - offset);
                WriteSectionEntry((int)ModPackageSection.Tags, writer, size, offset);

                // 
                // Write tag names table
                //

                offset = (uint)writer.BaseStream.Position;
                WriteTagNamesSection(writer, dataContext, serializer);
                size = (uint)(writer.BaseStream.Position - offset);
                WriteSectionEntry((int)ModPackageSection.TagNames, writer, size, offset);

                //
                // write resource cache
                //

                offset = (uint)writer.BaseStream.Position;
                WriteResourcesSection(writer);
                size = (uint)(writer.BaseStream.Position - offset);
                WriteSectionEntry((int)ModPackageSection.Resources, writer, size, offset);

                //
                // Write map file section
                //

                if(MapFileStreams.Count > 0)
                {
                    offset = (uint)writer.BaseStream.Position;
                    WriteMapsSection(writer);
                    size = (uint)(writer.BaseStream.Position - offset);
                    WriteSectionEntry((int)ModPackageSection.MapFiles, writer, size, offset);

                    DetermineMapFlags();
                }

                //
                // Write campaign file section
                // 
                if(CampaignFileStream != null && CampaignFileStream.Length > 0)
                {
                    offset = (uint)writer.BaseStream.Position;
                    WriteCampaignFileSection(writer);
                    size = (uint)(writer.BaseStream.Position - offset);
                    WriteSectionEntry((int)ModPackageSection.CampaignFiles, writer, size, offset);
                }

                //
                // Write font file section
                // 
                if (FontPackage != null && FontPackage.Length > 0)
                {
                    offset = (uint)writer.BaseStream.Position;
                    WriteFontFileSection(writer);
                    size = (uint)(writer.BaseStream.Position - offset);
                    WriteSectionEntry((int)ModPackageSection.Fonts, writer, size, offset);
                }

                //
                // Write stringid file section
                // 
                if (StringTable != null)
                {
                    offset = (uint)writer.BaseStream.Position;
                    WriteStringIdsSection(writer);
                    size = (uint)(writer.BaseStream.Position - offset);
                    WriteSectionEntry((int)ModPackageSection.StringIds, writer, size, offset);
                }

                //
                // Write files section
                // 
                if (Files != null && Files.Count > 0)
                {
                    offset = (uint)writer.BaseStream.Position;
                    WriteFileEntries(writer, dataContext, serializer);
                    size = (uint)(writer.BaseStream.Position - offset);
                    WriteSectionEntry((int)ModPackageSection.Files, writer, size, offset);
                }

                //
                // calculate package sha1
                //

                packageStream.Position = typeof(ModPackageHeader).GetSize();
                Header.SHA1 = new SHA1Managed().ComputeHash(packageStream);

                //
                // update package header
                //

                Header.FileSize = (uint)packageStream.Length;

                packageStream.Position = 0;
                serializer.Serialize(dataContext, Header);

                if (packageStream.Length > uint.MaxValue)
                    Console.WriteLine($"WARNING: Mod package size exceeded 0x{uint.MaxValue.ToString("X8")} bytes, it will fail to load.");

            }
        }

        private void WriteSectionEntry(int index, EndianWriter writer, uint size, uint offset)
        {
            // seek to the table and update size and offset
            writer.BaseStream.Seek(Header.SectionTable.Offset + typeof(ModPackageSectionHeader).GetSize() * index, SeekOrigin.Begin);
            var tableEntry = new ModPackageSectionHeader(size, offset);
            tableEntry.Write(writer);
            writer.BaseStream.Seek(0, SeekOrigin.End);
        }

        private void WriteMapsSection(EndianWriter writer)
        {
            uint sectionOffset = (uint)writer.BaseStream.Position;
            GenericSectionEntry mapEntry = new GenericSectionEntry(MapFileStreams.Count, 0x8);
            mapEntry.Write(writer);
            // make room for table

            writer.Write(new byte[0x10 * mapEntry.Count]);

            for(int i = 0; i < MapFileStreams.Count; i++)
            {
                var mapFileStream = MapFileStreams[i];
                uint offset = (uint)writer.BaseStream.Position;
                int size = (int)mapFileStream.Length;

                mapFileStream.Position = 0;
                StreamUtil.Copy(mapFileStream, writer.BaseStream, (int)mapFileStream.Length);
                StreamUtil.Align(writer.BaseStream, 4);

                // seek to the table and update size and offset
                writer.BaseStream.Seek(mapEntry.TableOffset + 0x10 * i + sectionOffset, SeekOrigin.Begin);
                var tableEntry = new CacheMapTableEntry(size, offset - sectionOffset, MapToCacheMapping[i], MapIds[i]);
                tableEntry.Write(writer);
                writer.BaseStream.Seek(0, SeekOrigin.End);
            }

            writer.BaseStream.Seek(0, SeekOrigin.End);
        }

        private void WriteTagsSection(EndianWriter writer, DataSerializationContext context, TagSerializer serializer)
        {
            uint sectionOffset = (uint)writer.BaseStream.Position;
            GenericSectionEntry tagCachesEntry = new GenericSectionEntry(TagCaches.Count, 0x8);
            tagCachesEntry.Write(writer);
            // make room for table

            writer.Write(new byte[0x28 * TagCaches.Count]);

            for(int i = 0; i < TagCaches.Count; i++)
            {

                uint offset = (uint)writer.BaseStream.Position;

                TagCachesStreams[i].Position = 0;
                StreamUtil.Copy(TagCachesStreams[i], writer.BaseStream, (int)TagCachesStreams[i].Length);
                StreamUtil.Align(writer.BaseStream, 4);

                uint size = (uint)(writer.BaseStream.Position - offset);

                writer.BaseStream.Seek(tagCachesEntry.TableOffset + 0x28 * i + sectionOffset, SeekOrigin.Begin);
                var tableEntry = new CacheTableEntry(size, offset - sectionOffset, CacheNames[i]);
                serializer.Serialize(context, tableEntry);
                writer.BaseStream.Seek(0, SeekOrigin.End);
            }
        }

        private void WriteTagNamesSection(EndianWriter writer, DataSerializationContext context, TagSerializer serializer)
        {
            uint sectionOffset = (uint)writer.BaseStream.Position;
            GenericSectionEntry tagNameFileEntry = new GenericSectionEntry(TagCacheNames.Count, 0x8);
            tagNameFileEntry.Write(writer);
            // make room for table

            writer.Write(new byte[0x8 * TagCacheNames.Count]);

            for(int i = 0; i < TagCacheNames.Count; i++)
            {
                //prepare tag names
                var names = new Dictionary<int, string>();

                foreach (var entry in TagCaches[i].TagTable)
                    if (entry != null && entry.Name != null)
                        names.Add(entry.Index, entry.Name);

                uint offset = (uint)writer.BaseStream.Position;

                GenericSectionEntry tagNameTable = new GenericSectionEntry(names.Count, offset - sectionOffset + 0x8);
                tagNameTable.Write(writer);

                foreach (var entry in names)
                {
                    var tagNameEntry = new ModPackageTagNamesEntry(entry.Key, entry.Value);
                    serializer.Serialize(context, tagNameEntry);
                }

                uint size = (uint)(writer.BaseStream.Position - offset);

                writer.BaseStream.Seek(tagNameFileEntry.TableOffset + 0x8 * i + sectionOffset, SeekOrigin.Begin);
                var tableEntry = new GenericTableEntry(size, offset - sectionOffset);
                tableEntry.Write(writer);
                writer.BaseStream.Seek(0, SeekOrigin.End);
            }
            
        }

        private void WriteResourcesSection(EndianWriter writer)
        {
            ResourcesStream.Position = 0;
            StreamUtil.Copy(ResourcesStream, writer.BaseStream, ResourcesStream.Length);
            StreamUtil.Align(writer.BaseStream, 4);
        }

        private void WriteCampaignFileSection(EndianWriter writer)
        {
            CampaignFileStream.Position = 0;
            StreamUtil.Copy(CampaignFileStream, writer.BaseStream, (int)CampaignFileStream.Length);
            StreamUtil.Align(writer.BaseStream, 4);
        }

        private void WriteFontFileSection(EndianWriter writer)
        {
            FontPackage.Position = 0;
            StreamUtil.Copy(FontPackage, writer.BaseStream, (int)FontPackage.Length);
            StreamUtil.Align(writer.BaseStream, 4);
        }

        private void WriteStringIdsSection(EndianWriter writer)
        {
            var stringIdStream = new MemoryStream();
            StringTable.Save(stringIdStream);
            stringIdStream.Position = 0;
            StreamUtil.Copy(stringIdStream, writer.BaseStream, (int)stringIdStream.Length);
            StreamUtil.Align(writer.BaseStream, 4);
        }

        private void WriteMetadataSection(DataSerializationContext context, TagSerializer serializer)
        {
            serializer.Serialize(context, Metadata);
        }

        private ModPackageSectionHeader GetSectionHeader(EndianReader reader, ModPackageSection section)
        {
            reader.SeekTo(Header.SectionTable.Offset + 0x8 * (int)section);
            return new ModPackageSectionHeader(reader);
        }

        private void ReadMetadataSection(EndianReader reader, DataSerializationContext context, TagDeserializer deserializer)
        {
            var section = GetSectionHeader(reader, ModPackageSection.Metadata);
            if (!GoToSectionHeaderOffset(reader, section))
                return;

            Metadata = deserializer.Deserialize<ModPackageMetadata>(context);
        }

        private void ReadTagsSection(EndianReader reader, DataSerializationContext context, TagDeserializer deserializer)
        {
            var section = GetSectionHeader(reader, ModPackageSection.Tags);
            if (!GoToSectionHeaderOffset(reader, section))
                return;

            var entry = new GenericSectionEntry(reader);
            var cacheCount = entry.Count;

            TagCachesStreams = new List<Stream>();
            CacheNames = new List<string>();

            for(int i = 0; i < cacheCount; i++)
            {
                var tagStream = new MemoryStream();

                reader.BaseStream.Position = entry.TableOffset + 0x28 * i + section.Offset;
                var tableEntry = deserializer.Deserialize<CacheTableEntry>(context);
                CacheNames.Add(tableEntry.CacheName);
                reader.BaseStream.Position = tableEntry.Offset + section.Offset;

                if (section.Size > int.MaxValue)
                    throw new Exception("Tag cache size not supported");
                int size = (int)section.Size;

                byte[] data = new byte[size];
                reader.Read(data, 0, size);
                tagStream.Write(data, 0, size);
                tagStream.Position = 0;
                TagCachesStreams.Add(tagStream);
            }
        }

        private void ReadResourcesSection(EndianReader reader)
        {
            var section = GetSectionHeader(reader, ModPackageSection.Resources);
            if (!GoToSectionHeaderOffset(reader, section))
                return;

            ResourcesStream = new MemoryStream();
            int resourceSize = (int)section.Size;
            byte[] data = new byte[resourceSize];
            reader.Read(data, 0, resourceSize);
            ResourcesStream.Write(data, 0, resourceSize);
            ResourcesStream.Position = 0;
        }

        private void ReadTagNamesSection(EndianReader reader, DataSerializationContext context, TagDeserializer deserializer)
        {
            var section = GetSectionHeader(reader, ModPackageSection.TagNames);
            if (!GoToSectionHeaderOffset(reader, section))
                return;

            var entry = new GenericSectionEntry(reader);
            var cacheCount = entry.Count;

            TagCacheNames = new List<Dictionary<int, string>>();

            for(int i = 0; i < cacheCount; i++)
            {
                var nameDict = new Dictionary<int, string>();
                reader.BaseStream.Position = entry.TableOffset + 0x8 * i + section.Offset;

                var tagNamesTableEntry = new GenericTableEntry(reader);
                if (tagNamesTableEntry.Size == 0)
                    throw new Exception("invalid tag name table entry size!");

                reader.BaseStream.Position = tagNamesTableEntry.Offset + section.Offset;
                var tagNamesHeader = new GenericSectionEntry(reader);
                reader.BaseStream.Position = tagNamesHeader.TableOffset + section.Offset;

                for (int j = 0; j < tagNamesHeader.Count; j++)
                {
                    var tagNamesEntry = deserializer.Deserialize<ModPackageTagNamesEntry>(context);
                    nameDict.Add(tagNamesEntry.TagIndex, tagNamesEntry.Name);
                }

                TagCacheNames.Add(nameDict);
            }
        }

        private void ReadStringIdSection(EndianReader reader)
        {
            var section = GetSectionHeader(reader, ModPackageSection.StringIds);
            if (!GoToSectionHeaderOffset(reader, section))
                return;

            int size = (int)section.Size;   // string_ids.dat will never exceed the int limits.

            var stringIdCacheStream = new MemoryStream();
            byte[] data = new byte[size];
            reader.Read(data, 0, size);
            stringIdCacheStream.Write(data, 0, size);
            stringIdCacheStream.Position = 0;
            StringTable = new StringTableHaloOnline(CacheVersion.HaloOnline106708, stringIdCacheStream);
        }

        private void ReadFontSection(EndianReader reader)
        {
            var section = GetSectionHeader(reader, ModPackageSection.Fonts);
            if (!GoToSectionHeaderOffset(reader, section))
                return;

            int size = (int)section.Size;   // font_package.bin will never exceed the int limits.

            FontPackage = new MemoryStream();
            byte[] data = new byte[size];
            reader.Read(data, 0, size);
            FontPackage.Write(data, 0, size);
            FontPackage.Position = 0;
        }

        private void ReadCampaignFileSection(EndianReader reader)
        {
            var section = GetSectionHeader(reader, ModPackageSection.CampaignFiles);
            if (!GoToSectionHeaderOffset(reader, section))
                return;

            int size = (int)section.Size;   // Campaign files will never exceed the int limits.

            CampaignFileStream = new MemoryStream();
            byte[] data = new byte[size];
            reader.Read(data, 0, size);
            CampaignFileStream.Write(data, 0, size);
            CampaignFileStream.Position = 0;
        }

        private void ReadMapFilesSection(EndianReader reader)
        {
            var section = GetSectionHeader(reader, ModPackageSection.MapFiles);
            if (!GoToSectionHeaderOffset(reader, section))
                return;

            var entry = new GenericSectionEntry(reader);
            var mapCount = entry.Count;

            MapFileStreams = new List<Stream>();
            MapToCacheMapping = new Dictionary<int, int>();

            for(int i = 0; i < mapCount; i++)
            {
                reader.BaseStream.Position = entry.TableOffset + 0x10 * i + section.Offset;
                var tableEntry = new CacheMapTableEntry(reader);

                reader.BaseStream.Position = tableEntry.Offset + section.Offset;

                MapToCacheMapping.Add(i, tableEntry.CacheIndex);
                int size = (int)section.Size;
                var stream = new MemoryStream();
                byte[] data = new byte[size];
                reader.Read(data, 0, size);
                stream.Write(data, 0, size);
                MapFileStreams.Add(stream);

            }
        }

        private void ReadFileEntries(EndianReader reader, ISerializationContext context, TagDeserializer deserializer)
        {
            Files = new Dictionary<string, Stream>();

            var section = GetSectionHeader(reader, ModPackageSection.Files);
            if (!GoToSectionHeaderOffset(reader, section))
                return;

            var fileTable = new GenericSectionEntry(reader);

            reader.BaseStream.Position = fileTable.TableOffset + section.Offset;
            for(int i = 0; i < fileTable.Count; i++)
            {         
                var tableEntry = deserializer.Deserialize<FileTableEntry>(context);

                var stream = new MemoryStream();
                StreamUtil.Copy(reader.BaseStream, stream, tableEntry.Size);

                Files.Add(tableEntry.Path, stream);
            }
        }

        private void WriteFileEntries(EndianWriter writer, ISerializationContext context, TagSerializer serializer)
        {
            const int kFileTableEntrySize = 0x108;

            uint sectionOffset = (uint)writer.BaseStream.Position;
            GenericSectionEntry table = new GenericSectionEntry(Files.Count, 0x8);
            table.Write(writer);

            // make room for table
            writer.BaseStream.Position = sectionOffset + table.TableOffset + Files.Count * kFileTableEntrySize;
            var index = 0;
            foreach (var fileEntry in Files)
            {
                StreamUtil.Align(writer.BaseStream, 0x10);
                uint offset = (uint)(writer.BaseStream.Position - sectionOffset);
                // write the contents
                fileEntry.Value.CopyTo(writer.BaseStream);

                // seek to the file table entry
                writer.BaseStream.Position = sectionOffset + table.TableOffset + index * kFileTableEntrySize;
                index++;

                // write the table entry
                var tableEntry = new FileTableEntry();
                tableEntry.Path = fileEntry.Key;
                tableEntry.Size = (uint)fileEntry.Value.Length;
                tableEntry.Offset = offset;
                serializer.Serialize(context, tableEntry);

                // move back to where we were
                writer.Seek(0, SeekOrigin.End);
            }
        }

        private bool GoToSectionHeaderOffset(EndianReader reader, ModPackageSectionHeader header)
        {
            if (header.Size == 0 || header.Size == 0)
                return false;
            else
            {
                reader.BaseStream.Position = header.Offset;
                return true;
            }  
        }

        public void DetermineMapFlags()
        {
            foreach (var mapFile in MapFileStreams)
            {
                var reader = new EndianReader(mapFile);
                MapFile map = new MapFile();
                map.Read(reader);

                var type = map.Header.CacheType;

                if (type == CacheFileType.Campaign)
                    Header.MapFlags |= MapFlags.CampaignMaps;
                else if (type == CacheFileType.MainMenu)
                    Header.MapFlags |= MapFlags.MainmenuMaps;
                else if (type == CacheFileType.Multiplayer)
                    Header.MapFlags |= MapFlags.MultiplayerMaps;
            }
        }
    }
}