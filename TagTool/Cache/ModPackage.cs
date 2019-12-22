using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using TagTool.IO;
using TagTool.Serialization;

namespace TagTool.Cache
{
    public class ModPackage
    {
        
        public ModPackageHeader Header { get; set; } = new ModPackageHeader();

        public ModPackageMetadata Metadata { get; set; } = new ModPackageMetadata();

        public List<TagCache> TagCaches { get; set; } = null;

        public List<MemoryStream> TagCachesStreams { get; set; } = new List<MemoryStream>();

        public List<Dictionary<int, string>> TagCacheNames { get; set; } = new List<Dictionary<int, string>>();

        public ResourceCache Resources { get; set; } = null;

        public MemoryStream ResourcesStream { get; set; } = new MemoryStream();

        public List<MemoryStream> MapFileStreams { get; set; } = new List<MemoryStream>();

        public MemoryStream CampaignFileStream { get; set; } = new MemoryStream();

        public Dictionary<int, int> MapToCacheMapping { get; set; } = new Dictionary<int, int>();

        public List<int> MapIds = new List<int>();

        public List<string> CacheNames { get; set; } = new List<string>();

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
                var tags = new TagCache(tagStream, names);
                TagCaches = new List<TagCache>();
                TagCaches.Add(tags);
                TagCacheNames.Add(names);

                Resources = new ResourceCache(ResourcesStream);
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

                int tagCacheCount = TagCachesStreams.Count;

                for (int i = 0; i < tagCacheCount; i++)
                {
                    TagCaches.Add(new TagCache(TagCachesStreams[i], TagCacheNames[i]));
                }
                
                Resources = new ResourceCache(ResourcesStream);
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
                // Add support for the remaining sections when needed (Fonts, StringIds, Locales)
                //

                //
                // calculate package sha1
                //

                packageStream.Position = typeof(ModPackageHeader).GetSize();
                Header.SHA1 = new SHA1Managed().ComputeHash(packageStream);

                //
                // update package header
                //

                Header.FileSize = (int)packageStream.Length;

                packageStream.Position = 0;
                serializer.Serialize(dataContext, Header);

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
            GenericSectionEntry mapEntry = new GenericSectionEntry(MapFileStreams.Count, (uint)writer.BaseStream.Position + 0x8);
            mapEntry.Write(writer);
            // make room for table

            writer.Write(new byte[0x8 * mapEntry.Count]);

            for(int i = 0; i < MapFileStreams.Count; i++)
            {
                var mapFileStream = MapFileStreams[i];
                mapFileStream.Position = 0;
                uint offset = (uint)writer.BaseStream.Position;
                int size = (int)mapFileStream.Length;
                StreamUtil.Copy(mapFileStream, writer.BaseStream, (int)mapFileStream.Length);
                StreamUtil.Align(writer.BaseStream, 4);

                // seek to the table and update size and offset
                writer.BaseStream.Seek(mapEntry.TableOffset + 0x10 * i, SeekOrigin.Begin);
                var tableEntry = new CacheMapTableEntry(size, offset, MapToCacheMapping[i], MapIds[i]);
                tableEntry.Write(writer);
                writer.BaseStream.Seek(0, SeekOrigin.End);
            }

            writer.BaseStream.Seek(0, SeekOrigin.End);
        }

        private void WriteTagsSection(EndianWriter writer, DataSerializationContext context, TagSerializer serializer)
        {
            GenericSectionEntry tagCachesEntry = new GenericSectionEntry(TagCaches.Count, (uint)writer.BaseStream.Position + 0x8);
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

                writer.BaseStream.Seek(tagCachesEntry.TableOffset + 0x28 * i, SeekOrigin.Begin);
                var tableEntry = new CacheTableEntry(size, offset, CacheNames[i]);
                serializer.Serialize(context, tableEntry);
                writer.BaseStream.Seek(0, SeekOrigin.End);
            }
        }

        private void WriteTagNamesSection(EndianWriter writer, DataSerializationContext context, TagSerializer serializer)
        {
            GenericSectionEntry tagNameFileEntry = new GenericSectionEntry(TagCacheNames.Count, (uint)writer.BaseStream.Position + 0x8);
            tagNameFileEntry.Write(writer);
            // make room for table

            writer.Write(new byte[0x8 * TagCacheNames.Count]);

            for(int i = 0; i < TagCacheNames.Count; i++)
            {
                //prepare tag names
                var names = new Dictionary<int, string>();

                foreach (var entry in TagCaches[i].Index)
                    if (entry != null && entry.Name != null)
                        names.Add(entry.Index, entry.Name);

                uint offset = (uint)writer.BaseStream.Position;

                GenericSectionEntry tagNameTable = new GenericSectionEntry(names.Count, offset + 0x8);
                tagNameTable.Write(writer);

                foreach (var entry in names)
                {
                    var tagNameEntry = new ModPackageTagNamesEntry(entry.Key, entry.Value);
                    serializer.Serialize(context, tagNameEntry);
                }

                uint size = (uint)(writer.BaseStream.Position - offset);

                writer.BaseStream.Seek(tagNameFileEntry.TableOffset + 0x8 * i, SeekOrigin.Begin);
                var tableEntry = new GenericTableEntry(size, offset);
                tableEntry.Write(writer);
                writer.BaseStream.Seek(0, SeekOrigin.End);
            }
            
        }

        private void WriteResourcesSection(EndianWriter writer)
        {
            ResourcesStream.Position = 0;
            StreamUtil.Copy(ResourcesStream, writer.BaseStream, (int)ResourcesStream.Length);
            StreamUtil.Align(writer.BaseStream, 4);
        }

        private void WriteCampaignFileSection(EndianWriter writer)
        {
            CampaignFileStream.Position = 0;
            StreamUtil.Copy(CampaignFileStream, writer.BaseStream, (int)CampaignFileStream.Length);
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

            TagCachesStreams = new List<MemoryStream>();
            CacheNames = new List<string>();

            for(int i = 0; i < cacheCount; i++)
            {
                var tagStream = new MemoryStream();

                reader.BaseStream.Position = entry.TableOffset + 0x28 * i;
                var tableEntry = deserializer.Deserialize<CacheTableEntry>(context);
                CacheNames.Add(tableEntry.CacheName);
                reader.BaseStream.Position = tableEntry.Offset;

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
                reader.BaseStream.Position = entry.TableOffset + 0x8 * i;

                var tagNamesHeader = new GenericSectionEntry(reader);
                reader.BaseStream.Position = tagNamesHeader.TableOffset;

                for (int j = 0; j < tagNamesHeader.Count; j++)
                {
                    var tagNamesEntry = deserializer.Deserialize<ModPackageTagNamesEntry>(context);
                    nameDict.Add(tagNamesEntry.TagIndex, tagNamesEntry.Name);
                }

                TagCacheNames.Add(nameDict);
            }
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

            MapFileStreams = new List<MemoryStream>();
            MapToCacheMapping = new Dictionary<int, int>();

            for(int i = 0; i < mapCount; i++)
            {
                reader.BaseStream.Position = entry.TableOffset + 0x10 * i;
                var tableEntry = new CacheMapTableEntry(reader);

                reader.BaseStream.Position = tableEntry.Offset;

                MapToCacheMapping.Add(i, tableEntry.CacheIndex);
                int size = (int)section.Size;
                var stream = new MemoryStream();
                byte[] data = new byte[size];
                reader.Read(data, 0, size);
                stream.Write(data, 0, size);
                MapFileStreams.Add(stream);

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

                var type = map.Header.GetCacheType();

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