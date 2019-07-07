using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using TagTool.IO;
using TagTool.Serialization;

namespace TagTool.Cache
{
    public class ModPackageExtended
    {
        
        public ModPackageHeaderExtended Header { get; set; } = new ModPackageHeaderExtended();

        public ModPackageMetadata Metadata { get; set; } = new ModPackageMetadata();

        public TagCache Tags { get; set; } = null;

        public MemoryStream TagsStream { get; set; } = new MemoryStream();

        public Dictionary<int, string> TagNames { get; set; } = new Dictionary<int, string>();

        public ResourceCache Resources { get; set; } = null;

        public MemoryStream ResourcesStream { get; set; } = new MemoryStream();

        public List<MemoryStream> MapFileStreams { get; set; } = new List<MemoryStream>();

        public MemoryStream CampaignFileStream { get; set; } = new MemoryStream();

        public ModPackageExtended(FileInfo file = null)
        {
            if (file != null)
                Load(file);
            else
            {
                Tags = new TagCache(TagsStream, new Dictionary<int, string>());
                Resources = new ResourceCache(ResourcesStream);
                Header.SectionTable = new ModPackageSectionTable();
            }
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

                Header = deserializer.Deserialize<ModPackageHeaderExtended>(dataContext);

                ReadMetadataSection(reader, dataContext, deserializer);
                ReadTagsSection(reader);
                ReadTagNamesSection(reader, dataContext, deserializer);
                ReadResourcesSection(reader);
                ReadMapFilesSection(reader);
                ReadCampaignFileSection(reader);

                Tags = new TagCache(TagsStream, TagNames);
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

                writer.Write(new byte[typeof(ModPackageHeaderExtended).GetSize()]);

                //
                // build section table
                //

                Header.SectionTable.Count = (int)ModPackageSection.SectionCount;
                Header.SectionTable.Offset = (int)writer.BaseStream.Position;

                writer.Write(new byte[typeof(ModPackageSectionHeader).GetSize() * (int)ModPackageSection.SectionCount]);


                int size, offset;

                //
                // Write metadata
                //

                offset = (int)writer.BaseStream.Position;
                WriteMetadataSection(dataContext, serializer);
                size = (int)writer.BaseStream.Position - offset;
                WriteSectionEntry((int)ModPackageSection.Metadata, writer, size, offset);

                //
                // Write tag cache
                //

                offset = (int)writer.BaseStream.Position;
                WriteTagsSection(writer);
                size = (int)writer.BaseStream.Position - offset;
                WriteSectionEntry((int)ModPackageSection.Tags, writer, size, offset);

                // 
                // Write tag names table
                //

                offset = (int)writer.BaseStream.Position;
                WriteTagNamesSection(writer, dataContext, serializer);
                size = (int)writer.BaseStream.Position - offset;
                WriteSectionEntry((int)ModPackageSection.TagNames, writer, size, offset);

                //
                // write resource cache
                //

                offset = (int)writer.BaseStream.Position;
                WriteResourcesSection(writer);
                size = (int)writer.BaseStream.Position - offset;
                WriteSectionEntry((int)ModPackageSection.Resources, writer, size, offset);

                //
                // Write map file section
                //

                if(MapFileStreams.Count > 0)
                {
                    offset = (int)writer.BaseStream.Position;
                    WriteMapsSection(writer);
                    size = (int)writer.BaseStream.Position - offset;
                    WriteSectionEntry((int)ModPackageSection.MapFiles, writer, size, offset);

                    DetermineMapFlags();
                }

                //
                // Write campaign file section
                // 
                if(CampaignFileStream != null && CampaignFileStream.Length > 0)
                {
                    offset = (int)writer.BaseStream.Position;
                    WriteCampaignFileSection(writer);
                    size = (int)writer.BaseStream.Position - offset;
                    WriteSectionEntry((int)ModPackageSection.CampaignFiles, writer, size, offset);
                }
                
                //
                // Add support for the remaining sections when needed (Fonts, StringIds, Locales)
                //

                //
                // calculate package sha1
                //

                packageStream.Position = typeof(ModPackageHeaderExtended).GetSize();
                Header.SHA1 = new SHA1Managed().ComputeHash(packageStream);

                //
                // update package header
                //

                Header.FileSize = (int)packageStream.Length;

                packageStream.Position = 0;
                serializer.Serialize(dataContext, Header);

            }
        }

        private void WriteSectionEntry(int index, EndianWriter writer, int size, int offset)
        {
            // seek to the table and update size and offset
            writer.BaseStream.Seek(Header.SectionTable.Offset + typeof(ModPackageSectionHeader).GetSize() * index, SeekOrigin.Begin);
            var tableEntry = new ModPackageSectionHeader(size, offset);
            tableEntry.Write(writer);
            writer.BaseStream.Seek(0, SeekOrigin.End);
        }

        private void WriteMapsSection(EndianWriter writer)
        {
            GenericSectionEntry mapEntry = new GenericSectionEntry(MapFileStreams.Count, (int)writer.BaseStream.Position + 0x8);
            mapEntry.Write(writer);
            // make room for table

            writer.Write(new byte[0x8 * mapEntry.Count]);

            for(int i = 0; i < MapFileStreams.Count; i++)
            {
                var mapFileStream = MapFileStreams[i];
                mapFileStream.Position = 0;
                int offset = (int)writer.BaseStream.Position;
                int size = (int)mapFileStream.Length;
                StreamUtil.Copy(mapFileStream, writer.BaseStream, (int)mapFileStream.Length);
                StreamUtil.Align(writer.BaseStream, 4);

                // seek to the table and update size and offset
                writer.BaseStream.Seek(mapEntry.TableOffset + 0x8 * i, SeekOrigin.Begin);
                var tableEntry = new GenericTableEntry(size, offset);
                tableEntry.Write(writer);
                writer.BaseStream.Seek(0, SeekOrigin.End);
            }

            writer.BaseStream.Seek(0, SeekOrigin.End);
        }

        private void WriteTagsSection(EndianWriter writer)
        {
            TagsStream.Position = 0;
            StreamUtil.Copy(TagsStream, writer.BaseStream, (int)TagsStream.Length);
            StreamUtil.Align(writer.BaseStream, 4);
        }

        private void WriteTagNamesSection(EndianWriter writer, DataSerializationContext context, TagSerializer serializer)
        {
            //prepare tag names
            var names = new Dictionary<int, string>();

            foreach (var entry in Tags.Index)
                if (entry != null && entry.Name != null)
                    names.Add(entry.Index, entry.Name);

            // create entry and immediatly write the tag names table
            GenericSectionEntry mapEntry = new GenericSectionEntry(names.Count, (int)writer.BaseStream.Position + 0x8);
            mapEntry.Write(writer);

            foreach (var entry in names)
            {
                var tagNameEntry = new ModPackageTagNamesEntry(entry.Key, entry.Value);
                serializer.Serialize(context, tagNameEntry);
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

        private void ReadTagsSection(EndianReader reader)
        {
            var section = GetSectionHeader(reader, ModPackageSection.Tags);
            if (!GoToSectionHeaderOffset(reader, section))
                return;

            TagsStream = new MemoryStream();
            byte[] data = new byte[section.Size];
            reader.Read(data, 0, section.Size);
            TagsStream.Write(data, 0, section.Size);
            TagsStream.Position = 0;
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

            var tagNamesHeader = new GenericSectionEntry(reader);
            reader.BaseStream.Position = tagNamesHeader.TableOffset;

            for(int i = 0; i< tagNamesHeader.Count; i++)
            {
                var tagNamesEntry = deserializer.Deserialize<ModPackageTagNamesEntry>(context);
                TagNames.Add(tagNamesEntry.TagIndex, tagNamesEntry.Name);
            }
        }

        private void ReadCampaignFileSection(EndianReader reader)
        {
            var section = GetSectionHeader(reader, ModPackageSection.CampaignFiles);
            if (!GoToSectionHeaderOffset(reader, section))
                return;

            CampaignFileStream = new MemoryStream();
            byte[] data = new byte[section.Size];
            reader.Read(data, 0, section.Size);
            CampaignFileStream.Write(data, 0, section.Size);
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

            for(int i = 0; i < mapCount; i++)
            {
                reader.BaseStream.Position = entry.TableOffset + 0x8 * i;
                var tableEntry = new GenericTableEntry(reader);

                reader.BaseStream.Position = tableEntry.Offset;

                var stream = new MemoryStream();
                byte[] data = new byte[section.Size];
                reader.Read(data, 0, section.Size);
                stream.Write(data, 0, section.Size);
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
                MapFile map = new MapFile(reader);

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