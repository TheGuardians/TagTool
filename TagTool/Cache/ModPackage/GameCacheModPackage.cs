using LZ4;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Resources;

namespace TagTool.Cache
{
    public class GameCacheModPackage : GameCache
    {
        public FileInfo ModPackageFile;
        public ModPackage BaseModPackage;
        public ModPackageStringTable ModPackageStringTable;
        public ModPackageTagCache ModPackageTagCache;
        public ResourceCacheModPackage ModPackageResourceCache;

        /// <summary>
        /// Tag cache index in the list of tag caches.
        /// </summary>
        private int CurrentTagCacheIndex = 0;

        public override StringTable StringTable => ModPackageStringTable;

        public override TagCacheTest TagCache => ModPackageTagCache;

        public override ResourceCacheTest ResourceCache => ModPackageResourceCache;



        public GameCacheModPackage(FileInfo file)
        {
            Version = CacheVersion.HaloOnline106708;
            Endianness = EndianFormat.LittleEndian;
            Deserializer = new TagDeserializer(Version);
            Serializer = new TagSerializer(Version);
            Directory = file.Directory;

            // load mod package
            BaseModPackage = new ModPackage(file);
            DisplayName = BaseModPackage.Metadata.Name + ".pak";

            ModPackageResourceCache = new ResourceCacheModPackage(BaseModPackage);
            ModPackageTagCache = new ModPackageTagCache(BaseModPackage.TagCachesStreams[0], BaseModPackage.TagCacheNames[0]);
            ModPackageStringTable = BaseModPackage.StringTable;
        }

        public GameCacheModPackage(ModPackage modPackage, FileInfo file)
        {
            BaseModPackage = modPackage;
            Directory = file.Directory;
            DisplayName = BaseModPackage.Metadata.Name + ".pak";
            Version = CacheVersion.HaloOnline106708;
            Endianness = EndianFormat.LittleEndian;
            Deserializer = new TagDeserializer(Version);
            Serializer = new TagSerializer(Version);
        }

        public override object Deserialize(Stream stream, CachedTag instance)
        {
            throw new NotImplementedException();
        }

        public override T Deserialize<T>(Stream stream, CachedTag instance)
        {
            throw new NotImplementedException();
        }

        public override void Serialize(Stream stream, CachedTag instance, object definition)
        {
            throw new NotImplementedException();
        }

        public override Stream OpenCacheRead() => ModPackageFile.OpenRead();

        public override Stream OpenCacheReadWrite() => ModPackageFile.Open(FileMode.Open, FileAccess.ReadWrite);

        public override Stream OpenCacheWrite() => ModPackageFile.Open(FileMode.Open, FileAccess.Write);

        
    }

    public class ModPackageStringTable : StringTable
    {
        public ModPackageStringTable(Stream stream)
        {
            Version = CacheVersion.HaloOnline106708;
            Resolver = new StringIdResolverMS23();

            if (stream.Length != 0)
                Load(stream);
            else
                Clear();
        }

        public ModPackageStringTable(StringTableHaloOnline hoStringTable)
        {
            foreach(var tagString in hoStringTable)
            {
                Add(tagString);
            }
            Version = CacheVersion.HaloOnline106708;
            Resolver = new StringIdResolverMS23();
        }

        public override StringId AddString(string newString)
        {
            var strIndex = Count;
            Add(newString);
            return GetStringId(strIndex);
        }

        public void Save(Stream stream)
        {
            var writer = new EndianWriter(stream, EndianFormat.LittleEndian);

            // Write the string count and then skip over the offset table, because it will be filled in last
            writer.Write(Count);
            writer.BaseStream.Position += 4 + Count * 4; // 4 byte data size + 4 bytes per string offset

            // Write string data and keep track of offsets
            var stringOffsets = new int[Count];
            var dataOffset = (int)writer.BaseStream.Position;
            var currentOffset = 0;
            for (var i = 0; i < Count; i++)
            {
                var str = this[i];
                if (str == null)
                {
                    // Null string - set offset to -1
                    stringOffsets[i] = -1;
                    continue;
                }

                // Write the string as null-terminated ASCII
                stringOffsets[i] = currentOffset;
                var data = Encoding.ASCII.GetBytes(str);
                writer.Write(data, 0, data.Length);
                writer.Write((byte)0);
                currentOffset += data.Length + 1;
            }

            // Now go back and write the string offsets
            writer.BaseStream.Position = 0x4;
            writer.Write(currentOffset); // Data size
            foreach (var offset in stringOffsets)
                writer.Write(offset);
            writer.BaseStream.SetLength(dataOffset + currentOffset);
        }

        public override void Save()
        {
            throw new NotImplementedException("Can't save strings without a stream in a mod package!");
        }

        private void Load(Stream stream)
        {
            var reader = new EndianReader(stream, EndianFormat.LittleEndian);

            // Read the header
            var stringCount = reader.ReadInt32();  // int32 string count
            var dataSize = reader.ReadInt32();     // int32 string data size

            // Read the string offsets into a list of (index, offset) pairs, and then sort by offset
            // This lets us know the length of each string without scanning for a null terminator
            var stringOffsets = new List<Tuple<int, int>>(stringCount);
            for (var i = 0; i < stringCount; i++)
            {
                var offset = reader.ReadInt32();
                if (offset >= 0 && offset < dataSize)
                    stringOffsets.Add(Tuple.Create(i, offset));
            }
            stringOffsets.Sort((x, y) => x.Item2 - y.Item2);

            // Seek to each offset and read each string
            var dataOffset = reader.BaseStream.Position;
            var strings = new string[stringCount];
            for (var i = 0; i < stringOffsets.Count; i++)
            {
                var index = stringOffsets[i].Item1;
                var offset = stringOffsets[i].Item2;
                var nextOffset = (i < stringOffsets.Count - 1) ? stringOffsets[i + 1].Item2 : dataSize;
                var length = Math.Max(0, nextOffset - offset - 1); // Subtract 1 for null terminator
                reader.BaseStream.Position = dataOffset + offset;
                strings[index] = Encoding.ASCII.GetString(reader.ReadBytes(length));
            }
            Clear();
            AddRange(strings.ToList());
        }
    }
    
    public class ModPackageTagCache : TagCacheTest
    {
        public List<CachedTagHaloOnline> Tags;
        public TagCacheHaloOnlineHeader Header;
        public Stream TagCacheStream;

        public override IEnumerable<CachedTag> TagTable { get => Tags; }
        public override Stream OpenTagCacheRead() => TagCacheStream;
        public override Stream OpenTagCacheWrite() => TagCacheStream;
        public override Stream OpenTagCacheReadWrite() => TagCacheStream;


        public ModPackageTagCache(Stream stream, Dictionary<int, string> tagNames)
        {
            Tags = new List<CachedTagHaloOnline>();
            TagCacheStream = stream;
            if (stream.Length != 0)
                Load(new EndianReader(stream, EndianFormat.LittleEndian), tagNames);
            else
            {
                // create empty tag cache TODO: improve that remove duplicated code
                TagCacheHaloOnlineHeader header = new TagCacheHaloOnlineHeader
                {
                    TagTableOffset = 0x20,
                    CreationTime = 0x01D0631BCC791704
                };

                stream.Position = 0;
                var writer = new EndianWriter(stream, EndianFormat.LittleEndian);
                var dataContext = new DataSerializationContext(writer);
                var serializer = new TagSerializer(CacheVersion.HaloOnline106708);
                serializer.Serialize(dataContext, header);
                stream.Position = 0;
            }
                
        }

        private void Load(EndianReader reader, Dictionary<int, string> names)
        {
            // Read file header
            reader.SeekTo(0);
            var dataContext = new DataSerializationContext(reader);
            var deserializer = new TagDeserializer(CacheVersion.HaloOnline106708);

            Header = deserializer.Deserialize<TagCacheHaloOnlineHeader>(dataContext);

            // Read tag offset list
            var headerOffsets = new uint[Header.TagCount];
            reader.BaseStream.Position = Header.TagTableOffset;
            for (var i = 0; i < Header.TagCount; i++)
                headerOffsets[i] = reader.ReadUInt32();

            // Read each tag
            for (var i = 0; i < Header.TagCount; i++)
            {
                if (headerOffsets[i] == 0)
                {
                    // Offset of 0 = null tag
                    Tags.Add(null);
                    continue;
                }

                string name = null;

                if (names.ContainsKey(i))
                    name = names[i];

                var tag = new CachedTagHaloOnline(i, name) { HeaderOffset = headerOffsets[i] };
                Tags.Add(tag);

                reader.BaseStream.Position = tag.HeaderOffset;
                tag.ReadHeader(reader);
            }
        }

        /// <summary>
        /// Allocates a new tag at the end of the tag list without updating the file.
        /// You can give the tag data by using one of the overwrite functions.
        /// </summary>
        /// <param name="type">The tag's type information.</param>
        /// <param name="name">The name of the tag instance.</param>
        /// <returns>The allocated tag.</returns>
        public override CachedTag AllocateTag(TagGroup type, string name = null)
        {
            var tagIndex = Tags.Count;
            var tag = new CachedTagHaloOnline(tagIndex, type, name);
            Tags.Add(tag);
            return tag;
        }

        /// <summary>
        /// Returns a new CachedTag instance without updating the tag cache.
        /// </summary>
        public override CachedTag CreateCachedTag(int index, TagGroup group, string name = null)
        {
            return new CachedTagHaloOnline(index, group, name);
        }

        public override CachedTag CreateCachedTag()
        {
            return new CachedTagHaloOnline(-1, TagGroup.None, null);
        }

        // there are no tag IDs in Halo Online
        public override CachedTag GetTag(uint ID) => GetTag((int)ID);

        public override CachedTag GetTag(int index)
        {
            if (index < 0 || index > Tags.Count)
                return null;
            return Tags[index];
        }

        public override CachedTag GetTag(string name, Tag groupTag)
        {
            foreach (var tag in Tags)
            {
                if (groupTag == tag.Group.Tag && name == tag.Name)
                    return tag;
            }
            return null;
        }






        //
        // copy pasta from HO cache, let's find a better solution
        //

        /// <summary>
        /// Reads a tag's raw data from the file, including its header.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="tag">The tag to read.</param>
        /// <returns>The data that was read.</returns>
        public byte[] ExtractTagRaw(Stream stream, CachedTagHaloOnline tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));
            else if (tag.HeaderOffset < 0)
                throw new ArgumentException("The tag is not in the cache file");

            var result = new byte[tag.TotalSize];

            stream.Position = tag.HeaderOffset;
            stream.Read(result, 0, result.Length);

            return result;
        }

        /// <summary>
        /// Reads a tag's data from the file.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="tag">The tag to read.</param>
        /// <returns>The data that was read.</returns>
        public CachedTagData ExtractTag(Stream stream, CachedTagHaloOnline tag)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));
            else if (tag.HeaderOffset < 0)
                throw new ArgumentException("The tag is not in the cache file");

            // Build the description info and get the data offset
            var data = BuildTagData(stream, tag, out uint dataOffset);

            // Read the tag data
            stream.Position = tag.HeaderOffset + dataOffset;
            data.Data = new byte[tag.TotalSize - dataOffset];
            stream.Read(data.Data, 0, data.Data.Length);

            // Correct pointers
            using (var dataWriter = new BinaryWriter(new MemoryStream(data.Data)))
            {
                foreach (var fixup in data.PointerFixups)
                {
                    dataWriter.BaseStream.Position = fixup.WriteOffset;
                    dataWriter.Write(tag.OffsetToPointer(fixup.TargetOffset));
                }
            }
            return data;
        }

        /// <summary>
        /// Overwrites a tag's raw data, including its header.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="tag">The tag to overwrite.</param>
        /// <param name="data">The data to overwrite the tag with.</param>
        /// <exception cref="System.ArgumentNullException">tag</exception>
        public void SetTagDataRaw(Stream stream, CachedTagHaloOnline tag, byte[] data)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));

            // Ensure the data fits
            if (tag.HeaderOffset < 0)
                tag.HeaderOffset = GetNewTagOffset(tag.Index);
            ResizeBlock(stream, tag, tag.HeaderOffset, tag.TotalSize, data.Length);
            tag.TotalSize = (uint)data.Length;

            // Write the data
            stream.Position = tag.HeaderOffset;
            stream.Write(data, 0, data.Length);

            // Re-parse it
            stream.Position = tag.HeaderOffset;
            tag.ReadHeader(new BinaryReader(stream));
            UpdateTagOffsets(new EndianWriter(stream, EndianFormat.LittleEndian));
        }

        /// <summary>
        /// Overwrites a tag's data.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="tag">The tag to overwrite.</param>
        /// <param name="data">The data to store.</param>
        public void SetTagData(Stream stream, CachedTagHaloOnline tag, CachedTagData data)
        {
            if (tag == null)
                throw new ArgumentNullException(nameof(tag));
            else if (data == null)
                throw new ArgumentNullException(nameof(data));
            else if (data.Group == TagGroup.None)
                throw new ArgumentException("Cannot assign a tag to a null tag group");
            else if (data.Data == null)
                throw new ArgumentException("The tag data buffer is null");

            // Ensure the data fits
            var headerSize = CachedTagHaloOnline.CalculateHeaderSize(data);
            var alignedHeaderSize = (uint)((headerSize + 0xF) & ~0xF);
            if (tag.HeaderOffset < 0)
                tag.HeaderOffset = GetNewTagOffset(tag.Index);
            var alignedLength = (data.Data.Length + 0xF) & ~0xF;
            ResizeBlock(stream, tag, tag.HeaderOffset, tag.TotalSize, alignedHeaderSize + alignedLength);
            tag.TotalSize = (uint)(alignedHeaderSize + alignedLength);
            tag.Update(data, alignedHeaderSize);

            // Write in the new header and data
            stream.Position = tag.HeaderOffset;
            var writer = new EndianWriter(stream, EndianFormat.LittleEndian);
            tag.WriteHeader(writer);
            StreamUtil.Fill(stream, 0, (int)(alignedHeaderSize - headerSize));
            stream.Write(data.Data, 0, data.Data.Length);
            StreamUtil.Fill(stream, 0, alignedLength - data.Data.Length);

            // Correct pointers
            foreach (var fixup in data.PointerFixups)
            {
                writer.BaseStream.Position = tag.HeaderOffset + alignedHeaderSize + fixup.WriteOffset;
                writer.Write(tag.OffsetToPointer(alignedHeaderSize + fixup.TargetOffset));
            }

            UpdateTagOffsets(writer);
        }

        /// <summary>
        /// Builds a description for a tag's data without extracting anything.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="tag">The tag to read.</param>
        /// <param name="dataOffset">On return, this will contain the offset of the tag's data relative to its header.</param>
        /// <returns>The description that was built. </returns>
        private static CachedTagData BuildTagData(Stream stream, CachedTagHaloOnline tag, out uint dataOffset)
        {
            var data = new CachedTagData
            {
                Group = tag.Group,
                MainStructOffset = tag.DefinitionOffset,
            };

            foreach (var dependency in tag.Dependencies)
                data.Dependencies.Add(dependency);

            // Read pointer fixups
            var reader = new BinaryReader(stream);
            foreach (var pointerOffset in tag.PointerOffsets)
            {
                reader.BaseStream.Position = tag.HeaderOffset + pointerOffset;
                data.PointerFixups.Add(new CachedTagData.PointerFixup
                {
                    WriteOffset = pointerOffset,
                    TargetOffset = tag.PointerToOffset(reader.ReadUInt32()),
                });
            }

            // Find the start of the tag's data by finding the offset of the first block which is pointed to by something
            // We CAN'T just calculate a header size here because we don't know for sure if there's padding and how big it is
            var startOffset = tag.DefinitionOffset;
            foreach (var fixup in data.PointerFixups)
                startOffset = Math.Min(startOffset, Math.Min(fixup.WriteOffset, fixup.TargetOffset));

            // Now convert all offsets into relative ones
            foreach (var fixup in data.PointerFixups)
            {
                fixup.WriteOffset -= startOffset;
                fixup.TargetOffset -= startOffset;
            }

            data.ResourcePointerOffsets.AddRange(tag.ResourcePointerOffsets.Select(offset => offset - startOffset));

            data.TagReferenceOffsets.AddRange(tag.TagReferenceOffsets.Select(offset => offset - startOffset));

            data.MainStructOffset -= startOffset;
            dataOffset = startOffset;

            return data;
        }

        /// <summary>
        /// Resizes a block of data in the file.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="tag">The tag that the block belongs to, if any.</param>
        /// <param name="startOffset">The offset where the block to resize begins at.</param>
        /// <param name="oldSize">The current size of the block to resize.</param>
        /// <param name="newSize">The new size of the block.</param>
        /// <exception cref="System.ArgumentException">Cannot resize a block to a negative size</exception>
        private void ResizeBlock(Stream stream, CachedTagHaloOnline tag, long startOffset, long oldSize, long newSize)
        {
            if (newSize < 0)
                throw new ArgumentException("Cannot resize a block to a negative size");
            else if (oldSize == newSize)
                return;

            var oldEndOffset = startOffset + oldSize;
            var sizeDelta = newSize - oldSize;

            if (stream.Length - oldEndOffset >= 0)
            {
                StreamUtil.Copy(stream, oldEndOffset, oldEndOffset + sizeDelta, stream.Length - oldEndOffset);
                FixTagOffsets(oldEndOffset, sizeDelta, tag);
            }
            else
                return;
        }

        /// <summary>
        /// Fixes tag offsets after a resize operation.
        /// </summary>
        /// <param name="startOffset">The offset where the resize operation took place.</param>
        /// <param name="sizeDelta">The amount to add to each tag offset after the start offset.</param>
        /// <param name="ignore">A tag to ignore.</param>
        private void FixTagOffsets(long startOffset, long sizeDelta, CachedTagHaloOnline ignore)
        {
            foreach (var adjustTag in Tags.Where(t => t != null && t != ignore && t.HeaderOffset >= startOffset))
                adjustTag.HeaderOffset += sizeDelta;
        }

        /// <summary>
        /// Gets the offset that a new tag should be inserted at so that the tags are stored in order by index.
        /// </summary>
        /// <param name="index">The index of the new tag.</param>
        /// <returns>The offset that the tag data should be written to.</returns>
        private long GetNewTagOffset(int index)
        {
            if (index < 0)
                throw new ArgumentException("Index cannot be negative");

            if (index >= Tags.Count - 1)
                return GetTagDataEndOffset();

            for (var i = index - 1; i >= 0; i--)
            {
                var tag = Tags[i];
                if (tag != null && tag.HeaderOffset >= 0)
                    return tag.HeaderOffset + tag.TotalSize;
            }

            return new TagStructureInfo(typeof(TagCacheHaloOnlineHeader)).TotalSize;
        }

        /// <summary>
        /// Gets the tag data end offset.
        /// </summary>
        /// <returns>The offset of the first byte past the last tag in the file.</returns>
        private uint GetTagDataEndOffset()
        {
            uint endOffset = new TagStructureInfo(typeof(TagCacheHaloOnlineHeader)).TotalSize;
            foreach (var tag in Tags)
            {
                if (tag != null)
                    endOffset = (uint)Math.Max(endOffset, tag.HeaderOffset + tag.TotalSize);
            }
            return endOffset;
        }

        /// <summary>
        /// Updates the tag offset table in the file.
        /// </summary>
        /// <param name="writer">The stream to write to.</param>
        public void UpdateTagOffsets(EndianWriter writer)
        {
            uint offsetTableOffset = GetTagDataEndOffset();
            writer.BaseStream.Position = offsetTableOffset;
            foreach (var tag in Tags)
            {
                if (tag != null && tag.HeaderOffset >= 0)
                    writer.Write((uint)tag.HeaderOffset);
                else
                    writer.Write(0);
            }
            writer.BaseStream.SetLength(writer.BaseStream.Position); // Truncate the file to end after the last offset
            UpdateFileHeader(writer, offsetTableOffset);
        }

        /// <summary>
        /// Updates the file header.
        /// </summary>
        /// <param name="writer">The stream to write to.</param>
        /// <param name="offsetTableOffset">The offset table offset.</param>
        private void UpdateFileHeader(EndianWriter writer, uint offsetTableOffset)
        {
            Header.TagTableOffset = offsetTableOffset;
            Header.TagCount = Tags.Count;
            writer.BaseStream.Position = 0;
            var dataContext = new DataSerializationContext(writer);
            var serializer = new TagSerializer(CacheVersion.HaloOnline106708);
            serializer.Serialize(dataContext, Header);
        }
    }

    public class ResourceCacheModPackage : ResourceCacheTest
    {
        public ResourceCacheHaloOnline HoResourceCache;
        public Stream ResourceStream;
        public ResourceCacheModPackage(ModPackage modPack)
        {
            HoResourceCache = modPack.Resources;
        }

        public PageableResource GetPageableResource(TagResourceReference resourceReference)
        {
            return resourceReference.HaloOnlinePageableResource;
            // WARNING: ResourceStream is not yet initialized because I don't know how to handle it yet.
        }

        public ResourceCacheHaloOnline GetResourceCache(PageableResource resource)
        {
            if (!resource.GetLocation(out var location))
                return null;

            if(location != ResourceLocation.Mods)
            {
                Console.WriteLine("Resource is not in mods. This is invalid for mod packages.");
                return null;
            }

            return HoResourceCache;
        }

        /// <summary>
        /// Adds a new pageable_resource to the current cache.
        /// </summary>
        /// <param name="resource">The pageable_resource to add.</param>
        /// <param name="dataStream">The stream to read the resource data from.</param>
        /// <exception cref="System.ArgumentNullException">resource</exception>
        /// <exception cref="System.ArgumentException">The input stream is not open for reading;dataStream</exception>
        public void AddResource(PageableResource resource, Stream dataStream)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");
            if (!dataStream.CanRead)
                throw new ArgumentException("The input stream is not open for reading", "dataStream");

            var cache = GetResourceCache(resource);
            var dataSize = (int)(dataStream.Length - dataStream.Position);
            var data = new byte[dataSize];
            dataStream.Read(data, 0, dataSize);
            resource.Page.Index = cache.Add(ResourceStream, data, out uint compressedSize);
            resource.Page.CompressedBlockSize = compressedSize;
            resource.Page.UncompressedBlockSize = (uint)dataSize;
            resource.DisableChecksum();
        }

        public byte[] GetResourceData(TagResourceReference resourceReference)
        {
            var pageableResource = GetPageableResource(resourceReference);
            var cache = GetResourceCache(pageableResource);

            if (pageableResource.Page == null || pageableResource.Page.UncompressedBlockSize < 0)
                return null;

            byte[] result = new byte[pageableResource.Page.UncompressedBlockSize];
            using (var dataStream = new MemoryStream(result))
            {
                ExtractResource(ResourceStream, pageableResource, dataStream);
            }
            return result;
        }

        public void ExtractResource(Stream inStream, PageableResource pageable, Stream outStream)
        {
            if (pageable == null)
                throw new ArgumentNullException("resource");
            if (!outStream.CanWrite)
                throw new ArgumentException("The output stream is not open for writing", "outStream");

            var cache = GetResourceCache(pageable);
            cache.Decompress(inStream, pageable.Page.Index, pageable.Page.CompressedBlockSize, outStream);
        }

        public bool IsResourceReferenceValid(TagResourceReference resourceReference)
        {
            if (resourceReference == null || resourceReference.HaloOnlinePageableResource == null)
                return false;
            var page = resourceReference.HaloOnlinePageableResource.Page;
            var resource = resourceReference.HaloOnlinePageableResource.Resource;
            if (page == null || resource == null)
                return false;

            return true;
        }

        private void ApplyResourceDefinitionFixups(TagResourceGen3 tagResource, byte[] resourceDefinitionData)
        {
            using (var resourceDefinitionStream = new MemoryStream(resourceDefinitionData))
            using (var fixupWriter = new EndianWriter(resourceDefinitionStream, EndianFormat.LittleEndian))
            {
                for (int i = 0; i < tagResource.ResourceFixups.Count; i++)
                {
                    var fixup = tagResource.ResourceFixups[i];
                    fixupWriter.Seek((int)fixup.BlockOffset, SeekOrigin.Begin);
                    fixupWriter.Write(fixup.Address.Value);
                }
            }
        }

        private T GetResourceDefinition<T>(TagResourceReference resourceReference)
        {
            var tagResource = GetPageableResource(resourceReference).Resource;

            T result;
            byte[] resourceDefinitionData = tagResource.DefinitionData;
            ApplyResourceDefinitionFixups(tagResource, resourceDefinitionData);

            byte[] resourceRawData = GetResourceData(resourceReference);
            if (resourceRawData == null)
                resourceRawData = new byte[0];

            // deserialize the resource definition again
            using (var definitionDataStream = new MemoryStream(resourceDefinitionData))
            using (var definitionDataReader = new EndianReader(definitionDataStream, EndianFormat.LittleEndian))
            using (var dataStream = new MemoryStream(resourceRawData))
            using (var dataReader = new EndianReader(dataStream, EndianFormat.LittleEndian))
            {
                var context = new ResourceDefinitionSerializationContext(dataReader, definitionDataReader, tagResource.DefinitionAddress.Type);
                var deserializer = new ResourceDeserializer(CacheVersion.HaloOnline106708);
                // deserialize without access to the data
                definitionDataReader.SeekTo(tagResource.DefinitionAddress.Offset);
                result = deserializer.Deserialize<T>(context);
            }
            return result;
        }

        public override BinkResource GetBinkResource(TagResourceReference resourceReference)
        {
            if (!IsResourceReferenceValid(resourceReference))
                return null;
            var resource = GetPageableResource(resourceReference).Resource;
            if (resource.ResourceType != TagResourceTypeGen3.Bink)
                return null;
            return GetResourceDefinition<BinkResource>(resourceReference);
        }

        public override BitmapTextureInteropResource GetBitmapTextureInteropResource(TagResourceReference resourceReference)
        {
            if (!IsResourceReferenceValid(resourceReference))
                return null;
            var resource = GetPageableResource(resourceReference).Resource;
            if (resource.ResourceType != TagResourceTypeGen3.Bitmap)
                return null;
            return GetResourceDefinition<BitmapTextureInteropResource>(resourceReference);
        }

        public override BitmapTextureInterleavedInteropResource GetBitmapTextureInterleavedInteropResource(TagResourceReference resourceReference)
        {
            if (!IsResourceReferenceValid(resourceReference))
                return null;
            var resource = GetPageableResource(resourceReference).Resource;
            if (resource.ResourceType != TagResourceTypeGen3.BitmapInterleaved)
                return null;
            return GetResourceDefinition<BitmapTextureInterleavedInteropResource>(resourceReference);
        }

        public override RenderGeometryApiResourceDefinition GetRenderGeometryApiResourceDefinition(TagResourceReference resourceReference)
        {
            if (!IsResourceReferenceValid(resourceReference))
                return null;
            var resource = GetPageableResource(resourceReference).Resource;
            if (resource.ResourceType != TagResourceTypeGen3.RenderGeometry)
                return null;
            return GetResourceDefinition<RenderGeometryApiResourceDefinition>(resourceReference);
        }

        public override ModelAnimationTagResource GetModelAnimationTagResource(TagResourceReference resourceReference)
        {
            if (!IsResourceReferenceValid(resourceReference))
                return null;
            var resource = GetPageableResource(resourceReference).Resource;
            if (resource.ResourceType != TagResourceTypeGen3.Animation)
                return null;
            return GetResourceDefinition<ModelAnimationTagResource>(resourceReference);
        }

        public override SoundResourceDefinition GetSoundResourceDefinition(TagResourceReference resourceReference)
        {
            if (!IsResourceReferenceValid(resourceReference))
                return null;
            var resource = GetPageableResource(resourceReference).Resource;
            if (resource.ResourceType != TagResourceTypeGen3.Sound)
                return null;
            return GetResourceDefinition<SoundResourceDefinition>(resourceReference);
        }

        private TagResourceReference CreateResource<T>(T resourceDefinition, TagResourceTypeGen3 resourceType)
        {
            var resourceReference = new TagResourceReference();
            var pageableResource = new PageableResource();

            pageableResource.Page = new RawPage();
            pageableResource.Resource = new TagResourceGen3();
            pageableResource.ChangeLocation(ResourceLocation.Mods);
            pageableResource.Resource.Unknown2 = 1;
            pageableResource.Resource.ResourceType = resourceType;

            resourceReference.HaloOnlinePageableResource = pageableResource;

            var definitionStream = new MemoryStream();
            var dataStream = new MemoryStream();

            using (var definitionWriter = new EndianWriter(definitionStream, EndianFormat.LittleEndian))
            using (var dataWriter = new EndianWriter(dataStream, EndianFormat.LittleEndian))
            {
                var context = new ResourceDefinitionSerializationContext(dataWriter, definitionWriter, CacheAddressType.Definition);
                var serializer = new ResourceSerializer(CacheVersion.HaloOnline106708);
                serializer.Serialize(context, resourceDefinition);

                var data = dataStream.ToArray();
                var definitionData = definitionStream.ToArray();
                dataStream.Position = 0;

                pageableResource.DisableChecksum();

                dataStream.Position = 0;
                AddResource(pageableResource, dataStream);

                // add resource definition and fixups
                pageableResource.Resource.DefinitionData = definitionData;
                pageableResource.Resource.ResourceFixups = context.ResourceFixups;
                pageableResource.Resource.DefinitionAddress = context.MainStructOffset;
                pageableResource.Resource.D3DFixups = context.D3DFixups;
            }
            return resourceReference;
        }

        public override TagResourceReference CreateSoundResource(SoundResourceDefinition soundResourceDefinition)
        {
            return CreateResource(soundResourceDefinition, TagResourceTypeGen3.Sound);
        }

        public override TagResourceReference CreateBitmapResource(BitmapTextureInteropResource bitmapResourceDefinition)
        {
            return CreateResource(bitmapResourceDefinition, TagResourceTypeGen3.Bitmap);
        }

        public override TagResourceReference CreateBitmapInterleavedResource(BitmapTextureInterleavedInteropResource bitmapResourceDefinition)
        {
            return CreateResource(bitmapResourceDefinition,  TagResourceTypeGen3.BitmapInterleaved);
        }

        public override TagResourceReference CreateBinkResource(BinkResource binkResourceDefinition)
        {
            return CreateResource(binkResourceDefinition, TagResourceTypeGen3.Bink);
        }

        public override TagResourceReference CreateRenderGeometryApiResource(RenderGeometryApiResourceDefinition renderGeometryDefinition)
        {
            return CreateResource(renderGeometryDefinition, TagResourceTypeGen3.RenderGeometry);
        }

        public override TagResourceReference CreateModelAnimationGraphResource(ModelAnimationTagResource modelAnimationGraphDefinition)
        {
            return CreateResource(modelAnimationGraphDefinition, TagResourceTypeGen3.Animation);
        }

        public override TagResourceReference CreateStructureBspResource(StructureBspTagResources sbspResource)
        {
            return CreateResource(sbspResource, TagResourceTypeGen3.Collision);
        }

        public override TagResourceReference CreateStructureBspCacheFileResource(StructureBspCacheFileTagResources sbspCacheFileResource)
        {
            return CreateResource(sbspCacheFileResource, TagResourceTypeGen3.Pathfinding);
        }

        public override StructureBspTagResources GetStructureBspTagResources(TagResourceReference resourceReference)
        {
            if (!IsResourceReferenceValid(resourceReference))
                return null;
            var resource = GetPageableResource(resourceReference).Resource;
            if (resource.ResourceType != TagResourceTypeGen3.Collision)
                return null;
            return GetResourceDefinition<StructureBspTagResources>(resourceReference);
        }

        public override StructureBspCacheFileTagResources GetStructureBspCacheFileTagResources(TagResourceReference resourceReference)
        {
            if (!IsResourceReferenceValid(resourceReference))
                return null;
            var resource = GetPageableResource(resourceReference).Resource;
            if (resource.ResourceType != TagResourceTypeGen3.Pathfinding)
                return null;
            return GetResourceDefinition<StructureBspCacheFileTagResources>(resourceReference);
        }
    }
}
