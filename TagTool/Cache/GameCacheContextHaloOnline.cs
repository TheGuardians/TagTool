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
    public class GameCacheContextHaloOnline : GameCache
    {
        public readonly DirectoryInfo Directory;
        public static readonly string TagNamesFile = "tag_list.csv";
        public StringIdCache StringIdCache { get; set; }
        public TagCacheHaloOnline TagCacheGenHO;
        public StringTableHaloOnline StringTableHaloOnline;

        public override TagCacheTest TagCache => TagCacheGenHO;
        public override StringTable StringTable => StringTableHaloOnline;

        public GameCacheContextHaloOnline(DirectoryInfo directory)
        {
            Directory = directory;
            var tagNames = LoadTagNames();

            using (var stream = OpenTagCacheRead())
                TagCacheGenHO = new TagCacheHaloOnline(stream, tagNames);

            if (CacheVersion.Unknown == (Version = CacheVersionDetection.DetectFromTimestamp(TagCacheGenHO.Header.CreationTime, out var closestVersion)))
                Version = closestVersion;

            Deserializer = new TagDeserializer(Version);
            Serializer = new TagSerializer(Version);

            StringTableHaloOnline = new StringTableHaloOnline(Version, Directory);

            TagGroup.Instances[new Tag("obje")] = new TagGroup(new Tag("obje"), Tag.Null, Tag.Null, StringTableHaloOnline.GetStringId("object"));
            TagGroup.Instances[new Tag("item")] = new TagGroup(new Tag("item"), new Tag("obje"), Tag.Null, StringTableHaloOnline.GetStringId("item"));
            TagGroup.Instances[new Tag("devi")] = new TagGroup(new Tag("devi"), new Tag("obje"), Tag.Null, StringTableHaloOnline.GetStringId("device"));
            TagGroup.Instances[new Tag("unit")] = new TagGroup(new Tag("unit"), new Tag("obje"), Tag.Null, StringTableHaloOnline.GetStringId("unit"));
            TagGroup.Instances[new Tag("rm  ")] = new TagGroup(new Tag("rm  "), Tag.Null, Tag.Null, StringTableHaloOnline.GetStringId("render_method"));
        }

        //
        // Helpers
        //

        #region Tag Cache Functionality
        /// <summary>
        /// Gets the tag cache file information.
        /// </summary>
        public FileInfo TagCacheFile
        {
            get
            {
                var files = Directory.GetFiles("tags.dat");

                if (files.Length == 0)
                    throw new FileNotFoundException(Path.Combine(Directory.FullName, "tags.dat"));

                return files[0];
            }
        }

        public TagCache CreateTagCache(DirectoryInfo directory, out FileInfo file)
        {
            if (directory == null)
                directory = Directory;

            if (!directory.Exists)
                directory.Create();

            file = new FileInfo(Path.Combine(directory.FullName, "tags.dat"));

            using (var stream = file.Create())
                return CreateTagCache(stream);
        }

        public TagCache CreateTagCache(Stream stream)
        {
            TagCache cache = null;

            using (var writer = new BinaryWriter(stream, Encoding.Default, true))
            {
                // Write the new resource cache file
                writer.Write(0);                  // padding
                writer.Write(32);                 // table offset
                writer.Write(0);                  // table entry count
                writer.Write(0);                  // padding
                writer.Write(0x01D0631BCC791704); // guid
                writer.Write(0);                  // padding
                writer.Write(0);                  // padding

                // Load the new resource cache file
                stream.Position = 0;
                cache = new TagCache(stream, new Dictionary<int, string>());
            }

            return cache;
        }

        /// <summary>
        /// Opens the tag cache file for reading.
        /// </summary>
        /// <returns>The stream that was opened.</returns>
        public override Stream OpenTagCacheRead() => TagCacheFile.OpenRead();

        public override Stream OpenCacheRead() => OpenTagCacheRead();

        /// <summary>
        /// Opens the tag cache file for writing.
        /// </summary>
        /// <returns>The stream that was opened.</returns>
        public FileStream OpenTagCacheWrite() => TagCacheFile.Open(FileMode.Open, FileAccess.Write);

        /// <summary>
        /// Opens the tag cache file for reading and writing.
        /// </summary>
        /// <returns>The stream that was opened.</returns>
        public FileStream OpenTagCacheReadWrite() => TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite);

        /// <summary>
        /// Gets a tag from the current cache.
        /// </summary>
        /// <param name="index">The index of the tag.</param>
        /// <returns>The tag at the specified index from the current cache.</returns>
        public CachedTagHaloOnline GetTag(int index)
        {
            if (!TryGetTag(index, out var result))
                throw new IndexOutOfRangeException($"0x{index:X4}");

            return result;
        }

        /// <summary>
        /// Attempts to get a tag from the current cache.
        /// </summary>
        /// <param name="index">The index of the tag.</param>
        /// <param name="instance">The tag at the specified index from the current cache.</param>
        /// <returns>true if the index is within the range of the tag cache, false otherwise.</returns>
        public bool TryGetTag(int index, out CachedTagHaloOnline instance)
        {
            if (index < 0 || index >= TagCacheGenHO.TagTable.Count())
            {
                instance = null;
                return false;
            }

            instance = TagCacheGenHO.Tags[index];
            return true;
        }

        /// <summary>
        /// Gets a tag of a specific type from the current cache.
        /// </summary>
        /// <typeparam name="T">The type of the tag definition.</typeparam>
        /// <param name="name">The name of the tag.</param>
        /// <returns>The tag of the specified type with the specified name from the current cache.</returns>
        public CachedTagHaloOnline GetTag<T>(string name)
        {
            if (TryGetTag<T>(name, out var result))
                return result;

            var attribute = TagStructure.GetTagStructureAttribute(typeof(T));
            var typeName = attribute.Name ?? typeof(T).Name.ToSnakeCase();

            throw new KeyNotFoundException($"'{typeName}' tag \"{name}\"");
        }

        public bool TryAllocateTag(out CachedTagHaloOnline result, Type type, string name = null)
        {
            result = null;

            try
            {
                var structure = TagStructure.GetTagStructureInfo(type, Version).Structure;

                if (structure == null)
                {
                    Console.WriteLine($"TagStructure attribute not found for type \"{type.Name}\".");
                    return false;
                }

                var groupTag = new Tag(structure.Tag);

                if (!TagGroup.Instances.ContainsKey(groupTag))
                {
                    Console.WriteLine($"TagGroup not found for type \"{type.Name}\" ({structure.Tag}).");
                    return false;
                }

                result = TagCacheGenHO.AllocateTag(TagGroup.Instances[groupTag], name);

                if (result == null)
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().Name}: {e.Message}");
                return false;
            }

            return true;
        }

        public CachedTagHaloOnline AllocateTag(Type type, string name = null)
        {
            if (TryAllocateTag(out var result, type, name))
                return result;

            Console.WriteLine($"Failed to allocate tag of type \"{type.Name}\".");
            return null;
        }

        public CachedTagHaloOnline AllocateTag<T>(string name = null) where T : TagStructure
            => AllocateTag(typeof(T), name);

        /// <summary>
        /// Attempts to get a tag of a specific type from the current cache.
        /// </summary>
        /// <typeparam name="T">The type of the tag definition.</typeparam>
        /// <param name="name">The name of the tag.</param>
        /// <param name="result">The resulting tag.</param>
        /// <returns>True if the tag was found, false otherwise.</returns>
        public bool TryGetTag<T>(string name, out CachedTagHaloOnline result)
        {
            if (Tags.TagDefinition.Types.Values.Contains(typeof(T)))
            {
                var groupTag = Tags.TagDefinition.Types.First((KeyValuePair<Tag, Type> entry) => entry.Value == typeof(T)).Key;

                foreach (var instance in TagCacheGenHO.Tags)
                {
                    if (instance is null)
                        continue;

                    if (instance.IsInGroup(groupTag) && instance.Name == name)
                    {
                        result = instance;
                        return true;
                    }
                }
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Attempts to get a tag by parsing its group name.
        /// </summary>
        /// <param name="name">The full name of the tag.</param>
        /// <param name="result">The resulting tag.</param>
        /// <returns>True if the tag was found, false otherwise.</returns>
        public bool TryGetTag(string name, out CachedTagHaloOnline result)
        {
            if (name.Length == 0)
            {
                result = null;
                return false;
            }

            if (name == "null")
            {
                result = null;
                return true;
            }

            if (name == "*")
            {
                if (TagCacheGenHO.Tags.Count == 0)
                {
                    result = null;
                    return false;
                }

                result = TagCacheGenHO.Tags.Last();
                return true;
            }

            if (name.StartsWith("*."))
            {
                if (!name.TrySplit('.', out var startNamePieces) || !TryParseGroupTag(startNamePieces[1], out var starGroupTag))
                {
                    result = null;
                    return false;
                }

                result = TagCacheGenHO.Tags.Last(tag => tag != null && tag.IsInGroup(starGroupTag));
                return true;
            }

            if (name.StartsWith("0x"))
            {
                name = name.Substring(2);

                if (name.TrySplit('.', out var hexNamePieces))
                    name = hexNamePieces[0];

                if (!int.TryParse(name, NumberStyles.HexNumber, null, out int tagIndex) || !(tagIndex >= 0 && tagIndex < TagCacheGenHO.Tags.Count))
                {
                    result = null;
                    return false;
                }

                result = TagCacheGenHO.Tags[tagIndex];
                return true;
            }

            if (!name.TrySplit('.', out var namePieces) || !TryParseGroupTag(namePieces[1], out var groupTag))
                throw new Exception($"Invalid tag name: {name}");

            var tagName = namePieces[0];

            foreach (var instance in TagCacheGenHO.Tags)
            {
                if (instance is null)
                    continue;

                if (instance.IsInGroup(groupTag) && instance.Name == tagName)
                {
                    result = instance;
                    return true;
                }
            }

            result = null;
            return false;
        }

        public CachedTagHaloOnline GetTag(string name)
        {
            if (TryGetTag(name, out var result))
                return result;

            throw new KeyNotFoundException(name);
        }

        /*
        
        public void Serialize(Stream stream, CachedTagInstance instance, object definition) =>
            Serializer.Serialize(new TagSerializationContext(stream, this, instance), definition);
        */
        /// <summary>
        /// Attempts to parse a group tag or name.
        /// </summary>
        /// <param name="name">The tag or name of the tag group.</param>
        /// <param name="result">The resulting group tag.</param>
        /// <returns>True if the group tag was parsed, false otherwise.</returns>
        public bool TryParseGroupTag(string name, out Tag result)
        {
            if (Tags.TagDefinition.TryFind(name, out var type))
            {
                var attribute = TagStructure.GetTagStructureAttribute(type);
                result = new Tag(attribute.Tag);
                return true;
            }

            foreach (var pair in TagGroup.Instances)
            {
                if (name == StringTableHaloOnline.GetString(pair.Value.Name))
                {
                    result = pair.Value.Tag;
                    return true;
                }
            }

            result = Tag.Null;
            return false;
        }

        /// <summary>
        /// Parses a group tag or name;
        /// </summary>
        /// <param name="name">The tag or name of the tag group.</param>
        /// <returns>The resulting group tag.</returns>
        public Tag ParseGroupTag(string name)
        {
            if (name == "****" || name == "null")
                return Tag.Null;

            if (TryParseGroupTag(name, out var result))
                return result;

            throw new KeyNotFoundException(name);
        }

        /// <summary>
        /// Loads tag file names from the appropriate tag_list.csv file.
        /// </summary>
        /// <param name="path">The path to the tag_list.csv file.</param>
        public Dictionary<int, string> LoadTagNames(string path = null)
        {
            var names = new Dictionary<int, string>();

            if (path == null)
                path = Path.Combine(Directory.FullName, "tag_list.csv");

            if (File.Exists(path))
            {
                using (var tagNamesStream = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    var reader = new StreamReader(tagNamesStream);

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var separatorIndex = line.IndexOf(',');
                        var indexString = line.Substring(2, separatorIndex - 2);

                        if (!int.TryParse(indexString, NumberStyles.HexNumber, null, out int tagIndex))
                            tagIndex = -1;

                        //if (tagIndex < 0 || tagIndex >= TagCache.Index.Count || TagCache.Index[tagIndex] == null)
                        //continue;

                        var nameString = line.Substring(separatorIndex + 1);

                        if (nameString.Contains(" "))
                        {
                            var lastSpaceIndex = nameString.LastIndexOf(' ');
                            nameString = nameString.Substring(lastSpaceIndex + 1, nameString.Length - lastSpaceIndex - 1);
                        }

                        names[tagIndex] = nameString;
                    }

                    reader.Close();
                }
            }

            return names;
        }

        /// <summary>
        /// Saves tag file names to the appropriate tag_list.csv file.
        /// </summary>
        /// <param name="path">The path to the tag_list.csv file.</param>
        public void SaveTagNames(string path = null)
        {
            var csvFile = new FileInfo(path ?? Path.Combine(Directory.FullName, "tag_list.csv"));

            if (!csvFile.Directory.Exists)
                csvFile.Directory.Create();

            using (var csvWriter = new StreamWriter(csvFile.Create()))
            {
                foreach (var instance in TagCacheGenHO.Tags)
                    if (instance != null && instance.Name != null && !instance.Name.ToLower().StartsWith("0x"))
                        csvWriter.WriteLine($"0x{instance.Index:X8},{instance.Name}");
            }
        }
        #endregion

        #region Resource Cache Functionality
        /// <summary>
        /// The file names associated to each <see cref="ResourceLocation"/>.
        /// </summary>
        public Dictionary<ResourceLocation, string> ResourceCacheNames { get; } = new Dictionary<ResourceLocation, string>()
        {
            { ResourceLocation.Resources, "resources.dat" },
            { ResourceLocation.Textures, "textures.dat" },
            { ResourceLocation.TexturesB, "textures_b.dat" },
            { ResourceLocation.Audio, "audio.dat" },
            { ResourceLocation.ResourcesB, "resources_b.dat" },
            { ResourceLocation.RenderModels, "render_models.dat" },
            { ResourceLocation.Lightmaps, "lightmaps.dat" },
            { ResourceLocation.Mods, "mods.dat" }
        };

        /// <summary>
        /// The loaded <see cref="ResourceCache"/> for each <see cref="ResourceLocation"/>.
        /// </summary>
        private Dictionary<ResourceLocation, LoadedResourceCache> LoadedResourceCaches { get; } = new Dictionary<ResourceLocation, LoadedResourceCache>();

        public FileInfo GetResourceCacheFile(ResourceLocation location)
        {
            if (!LoadedResourceCaches.TryGetValue(location, out LoadedResourceCache cache))
            {
                var file = new FileInfo(Path.Combine(Directory.FullName, ResourceCacheNames[location]));

                using (var stream = file.OpenRead())
                {
                    cache = new LoadedResourceCache
                    {
                        File = file,
                        Cache = new ResourceCache(stream)
                    };
                }

                LoadedResourceCaches[location] = cache;
            }

            return cache.File;
        }

        /// <summary>
        /// Gets a resource cache file descriptor for the specified <see cref="ResourceLocation"/>.
        /// </summary>
        /// <param name="location">The location of the resource file.</param>
        /// <returns></returns>
        public ResourceCache GetResourceCache(ResourceLocation location)
        {
            if (!LoadedResourceCaches.TryGetValue(location, out LoadedResourceCache cache))
            {
                var file = new FileInfo(Path.Combine(Directory.FullName, ResourceCacheNames[location]));

                using (var stream = file.OpenRead())
                {
                    cache = new LoadedResourceCache
                    {
                        File = file,
                        Cache = new ResourceCache(stream)
                    };
                }

                LoadedResourceCaches[location] = cache;
            }

            return cache.Cache;
        }

        public ResourceCache CreateResourceCache(DirectoryInfo directory, ResourceLocation location, out FileInfo file)
        {
            if (!directory.Exists)
                directory.Create();

            file = new FileInfo(Path.Combine(directory.FullName, ResourceCacheNames[location]));

            using (var stream = file.Create())
                return CreateResourceCache(stream);
        }

        public ResourceCache CreateResourceCache(Stream stream)
        {
            ResourceCache cache = null;

            using (var writer = new BinaryWriter(stream, Encoding.Default, true))
            {
                // Write the new resource cache file
                writer.Write(0);                  // padding
                writer.Write(32);                 // table offset
                writer.Write(0);                  // table entry count
                writer.Write(0);                  // padding
                writer.Write(0x01D0631BCC92931B); // guid
                writer.Write(0);                  // padding
                writer.Write(0);                  // padding

                // Load the new resource cache file
                stream.Position = 0;
                cache = new ResourceCache(stream);
            }

            return cache;
        }

        /// <summary>
        /// Opens a resource cache file for reading.
        /// </summary>
        /// <param name="location">The location of the resource file.</param>
        /// <returns></returns>
        public FileStream OpenResourceCacheRead(ResourceLocation location) => LoadedResourceCaches[location].File.OpenRead();

        /// <summary>
        /// Opens a resource cache file for writing.
        /// </summary>
        /// <param name="location">The location of the resource file.</param>
        /// <returns></returns>
        public FileStream OpenResourceCacheWrite(ResourceLocation location) => LoadedResourceCaches[location].File.OpenWrite();

        /// <summary>
        /// Opens a resource cache file for reading and writing.
        /// </summary>
        /// <param name="location">The location of the resource file.</param>
        /// <returns></returns>
        public FileStream OpenResourceCacheReadWrite(ResourceLocation location) => LoadedResourceCaches[location].File.Open(FileMode.Open, FileAccess.ReadWrite);

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
            using (var stream = cache.File.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var dataSize = (int)(dataStream.Length - dataStream.Position);
                var data = new byte[dataSize];
                dataStream.Read(data, 0, dataSize);
                resource.Page.Index = cache.Cache.Add(stream, data, out uint compressedSize);
                resource.Page.CompressedBlockSize = compressedSize;
                resource.Page.UncompressedBlockSize = (uint)dataSize;
                resource.DisableChecksum();
            }
        }

        /// <summary>
        /// Adds raw, pre-compressed resource data to a cache.
        /// </summary>
        /// <param name="resource">The resource reference to initialize.</param>
        /// <param name="data">The pre-compressed data to store.</param>
        public void AddRawResource(PageableResource resource, byte[] data)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");

            resource.DisableChecksum();
            var cache = GetResourceCache(resource);
            using (var stream = cache.File.Open(FileMode.Open, FileAccess.ReadWrite))
                resource.Page.Index = cache.Cache.AddRaw(stream, data);
        }

        /// <summary>
        /// Extracts and decompresses the data for a resource from the current cache.
        /// </summary>
        /// <param name="pageable">The resource.</param>
        /// <param name="outStream">The stream to write the extracted data to.</param>
        /// <exception cref="System.ArgumentException">Thrown if the output stream is not open for writing.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the file containing the resource has not been loaded.</exception>
        public void ExtractResource(PageableResource pageable, Stream outStream)
        {
            if (pageable == null)
                throw new ArgumentNullException("resource");
            if (!outStream.CanWrite)
                throw new ArgumentException("The output stream is not open for writing", "outStream");

            var cache = GetResourceCache(pageable);
            using (var stream = cache.File.OpenRead())
                cache.Cache.Decompress(stream, pageable.Page.Index, pageable.Page.CompressedBlockSize, outStream);
        }

        /// <summary>
        /// Extracts and decompresses the data for a resource from the current cache.
        /// </summary>
        /// <param name="inStream"></param>
        /// <param name="pageable">The resource.</param>
        /// <param name="outStream">The stream to write the extracted data to.</param>
        /// <exception cref="System.ArgumentException">Thrown if the output stream is not open for writing.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the file containing the resource has not been loaded.</exception>
        public void ExtractResource(Stream inStream, PageableResource pageable, Stream outStream)
        {
            if (pageable == null)
                throw new ArgumentNullException("resource");
            if (!outStream.CanWrite)
                throw new ArgumentException("The output stream is not open for writing", "outStream");

            var cache = GetResourceCache(pageable);
            cache.Cache.Decompress(inStream, pageable.Page.Index, pageable.Page.CompressedBlockSize, outStream);
        }

        /// <summary>
        /// Extracts raw, compressed resource data.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns>The raw, compressed resource data.</returns>
        public byte[] ExtractRawResource(PageableResource resource)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");

            var cache = GetResourceCache(resource);
            using (var stream = cache.File.OpenRead())
                return cache.Cache.ExtractRaw(stream, resource.Page.Index, resource.Page.CompressedBlockSize);
        }

        /// <summary>
        /// Compresses and replaces the data for a resource.
        /// </summary>
        /// <param name="resource">The resource whose data should be replaced. On success, the reference will be adjusted to account for the new data.</param>
        /// <param name="dataStream">The stream to read the new data from.</param>
        /// <exception cref="System.ArgumentException">Thrown if the input stream is not open for reading.</exception>
        public void ReplaceResource(PageableResource resource, Stream dataStream)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");
            if (!dataStream.CanRead)
                throw new ArgumentException("The input stream is not open for reading", "dataStream");

            var cache = GetResourceCache(resource);
            using (var stream = cache.File.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var dataSize = (int)(dataStream.Length - dataStream.Position);
                var data = new byte[dataSize];
                dataStream.Read(data, 0, dataSize);
                var compressedSize = cache.Cache.Compress(stream, resource.Page.Index, data);
                resource.Page.CompressedBlockSize = compressedSize;
                resource.Page.UncompressedBlockSize = (uint)dataSize;
                resource.DisableChecksum();
            }
        }

        /// <summary>
        /// Replaces a resource with raw, pre-compressed data.
        /// </summary>
        /// <param name="resource">The resource whose data should be replaced. On success, the reference will be adjusted to account for the new data.</param>
        /// <param name="data">The raw, pre-compressed data to use.</param>
        public void ReplaceRawResource(PageableResource resource, byte[] data)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");

            resource.DisableChecksum();
            var cache = GetResourceCache(resource);
            using (var stream = cache.File.Open(FileMode.Open, FileAccess.ReadWrite))
                cache.Cache.ImportRaw(stream, resource.Page.Index, data);
        }

        private LoadedResourceCache GetResourceCache(PageableResource resource)
        {
            if (!resource.GetLocation(out var location))
                return null;

            if (!LoadedResourceCaches.TryGetValue(location, out LoadedResourceCache cache))
            {
                var file = new FileInfo(Path.Combine(Directory.FullName, ResourceCacheNames[location]));

                if (!file.Exists && file.Name == "resources_b.dat")
                    file = new FileInfo(Path.Combine(Directory.FullName, "video.dat"));

                using (var stream = file.OpenRead())
                {
                    cache = new LoadedResourceCache
                    {
                        File = file,
                        Cache = new ResourceCache(stream)
                    };
                }
            }

            return cache;
        }

        #endregion


        public void Serialize(PageableResource pageable, object definition) =>
            Serialize(new ResourceSerializationContext(null, pageable), definition);

        public void Serialize(ISerializationContext context, object definition, uint? offset = null) => 
            Serializer.Serialize(context, definition, offset);

        //
        // DeserializationMethods
        // 

        private T Deserialize<T>(ISerializationContext context) =>
            Deserializer.Deserialize<T>(context);

        private object Deserialize(ISerializationContext context, Type type) =>
            Deserializer.Deserialize(context, type);

        public override T Deserialize<T>(Stream stream, CachedTag instance) =>
            Deserialize<T>(new HaloOnlineSerializationContext(stream, this, (CachedTagHaloOnline)instance));

        public override object Deserialize(Stream stream, CachedTag instance) =>
            Deserialize(new HaloOnlineSerializationContext(stream, this, (CachedTagHaloOnline)instance), TagDefinition.Find(instance.Group.Tag));

        public T Deserialize<T>(Stream stream, CachedTagHaloOnline instance) =>
            Deserialize<T>(new HaloOnlineSerializationContext(stream, this, instance));

        public object Deserialize(Stream stream, CachedTagHaloOnline instance) =>
            Deserialize(new HaloOnlineSerializationContext(stream, this, instance), TagDefinition.Find(instance.Group.Tag));


        private class LoadedResourceCache
        {
            public ResourceCache Cache { get; set; }
            public FileInfo File { get; set; }
        }
        
    }

    [TagStructure(Size = 0x20)]
    public class TagCacheHaloOnlineHeader
    {
        public int UnusedTag;
        public uint TagListOffset;
        public int TagCount;
        public int Unused;
        public long CreationTime;
        public int Unused2;
        public int Unused3;
    }

    public class TagCacheHaloOnline : TagCacheTest
    {
        public List<CachedTagHaloOnline> Tags;

        public override IEnumerable<CachedTag> TagTable { get => Tags; }

        public TagCacheHaloOnlineHeader Header;

        public TagCacheHaloOnline(Stream stream, Dictionary<int, string> tagNames)
        {
            Tags = new List<CachedTagHaloOnline>();

            if (stream.Length != 0)
                Load(new EndianReader(stream, EndianFormat.LittleEndian), tagNames);
            else
                Console.Error.WriteLine("Failed to open tag cache");
        }

        private void Load(EndianReader reader, Dictionary<int, string> names)
        {
            // Read file header
            reader.SeekTo(0);
            var dataContext = new DataSerializationContext(reader);
            var deserializer = new TagDeserializer(CacheVersion.HaloOnline106708); // temporary workaround having a structure serializer

            Header = deserializer.Deserialize<TagCacheHaloOnlineHeader>(dataContext);

            // Read tag offset list
            var headerOffsets = new uint[Header.TagCount];
            reader.BaseStream.Position = Header.TagListOffset;
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
        /// The tag's group will be null until it is assigned data.
        /// You can give the tag data by using one of the overwrite functions.
        /// </summary>
        /// <returns>The allocated tag.</returns>
        public CachedTagHaloOnline AllocateTag() => AllocateTag(TagGroup.None);

        /// <summary>
        /// Allocates a new tag at the end of the tag list without updating the file.
        /// You can give the tag data by using one of the overwrite functions.
        /// </summary>
        /// <param name="type">The tag's type information.</param>
        /// <param name="name">The name of the tag instance.</param>
        /// <returns>The allocated tag.</returns>
        public CachedTagHaloOnline AllocateTag(TagGroup type, string name = null)
        {
            var tagIndex = Tags.Count;
            var tag = new CachedTagHaloOnline(tagIndex, type, name);
            Tags.Add(tag);
            return tag;
        }


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
        public CachedTagData ExtractTag(Stream stream, CachedTagInstance tag)
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
        private static CachedTagData BuildTagData(Stream stream, CachedTagInstance tag, out uint dataOffset)
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
            Header.TagListOffset = offsetTableOffset;
            var dataContext = new DataSerializationContext(writer);
            var serializer = new TagSerializer(CacheVersion.HaloOnline106708);
            serializer.Serialize(dataContext, Header);
        }

        // there are no IDs in HO
        public override CachedTag GetTagByID(int ID) => GetTagByIndex(ID);

        public override CachedTag GetTagByIndex(int index)
        {
            if (index < 0 || index > Tags.Count)
                return null;
            return Tags[index];
        }

        public override CachedTag GetTagByName(string name, Tag groupTag)
        {
            foreach (var tag in Tags)
            {
                if (groupTag == tag.Group.Tag && name == tag.Name)
                    return tag;
            }
            return null;
        }
    }

    public class CachedTagHaloOnline : CachedTag
    {
        public CachedTagHaloOnline() : base() { }

        public CachedTagHaloOnline(int index, string name = null) : base(index, name) { }

        public CachedTagHaloOnline(int index, TagGroup group, string name = null) : base(index, group, name) { }

        public override uint DefinitionOffset => Offset;

        // Magic constant (NOT a build-specific memory address) used for pointers in tag data
        private const uint FixupPointerBase = 0x40000000;

        // Size of a tag header with no dependencies or offsets
        private const uint TagHeaderSize = 0x24;

        private List<uint> _pointerOffsets = new List<uint>();
        private List<uint> _resourceOffsets = new List<uint>();
        private List<uint> _tagReferenceOffsets = new List<uint>();


        /// <summary>
        /// Gets the offset of the tag's header, or -1 if the tag is not in a file.
        /// </summary>
        public long HeaderOffset { get; internal set; } = -1;

        /// <summary>
        /// Gets the offset of the tag's main structure from the start of its header.
        /// </summary>
        public uint Offset;

        /// <summary>
        /// Gets the total size of the tag (including both its header and data), or 0 if the tag is not in a file.
        /// </summary>
        public uint TotalSize { get; internal set; }

        /// <summary>
        /// Gets the checksum (adler32?) of the tag's data. Ignored if checksum verification is patched out.
        /// </summary>
        public uint Checksum { get; private set; }

        /// <summary>
        /// Gets the indices of tags that this tag depends on.
        /// </summary>
        public ReadOnlySet<int> Dependencies { get; private set; } = new ReadOnlySet<int>(new HashSet<int>());

        /// <summary>
        /// Gets a list of offsets to each pointer in the tag, relative to the beginning of the tag's header.
        /// </summary>
        /// <remarks>
        /// This previously used offsets relative to the beginning of the tag's data.
        /// This was changed in order to speed up loading and be more closer to the engine.
        /// </remarks>
        public IReadOnlyList<uint> PointerOffsets => _pointerOffsets;

        /// <summary>
        /// Gets a list of offsets to each resource pointer in the tag, relative to the beginning of the tag's header.
        /// </summary>
        /// <remarks>
        /// See the remarks for <see cref="PointerOffsets"/>.
        /// </remarks>
        public IReadOnlyList<uint> ResourcePointerOffsets => _resourceOffsets;

        /// <summary>
        /// Gets a list of offsets to each tag reference in the tag, relative to the beginning of the tag's header.
        /// </summary>
        public IReadOnlyList<uint> TagReferenceOffsets => _tagReferenceOffsets;

        /// <summary>
        /// Converts a pointer to an offset relative to the tag's header.
        /// </summary>
        /// <param name="pointer">The pointer to convert.</param>
        /// <returns>The offset.</returns>
        public uint PointerToOffset(uint pointer) => pointer - FixupPointerBase;

        /// <summary>
        /// Converts an offset relative to the tag's header to a pointer.
        /// </summary>
        /// <param name="offset">The offset to convert.</param>
        /// <returns>The pointer.</returns>
        public uint OffsetToPointer(uint offset) => offset + FixupPointerBase;

        /// <summary>
        /// Reads the header for the tag instance from a stream.
        /// </summary>
        /// <param name="reader">The stream to read from.</param>
        internal void ReadHeader(BinaryReader reader)
        {
            Checksum = reader.ReadUInt32();                        // 0x00 uint32 checksum
            TotalSize = reader.ReadUInt32();                       // 0x04 uint32 total size
            var numDependencies = reader.ReadInt16();              // 0x08 int16  dependencies count
            var numDataFixups = reader.ReadInt16();                // 0x0A int16  data fixup count
            var numResourceFixups = reader.ReadInt16();            // 0x0C int16  resource fixup count
            var numTagReferenceFixups = reader.ReadInt16();        // 0x0E int16  tag reference fixup count(was padding)
            Offset = reader.ReadUInt32();                // 0x10 uint32 main struct offset
            var groupTag = new Tag(reader.ReadInt32());            // 0x14 int32  group tag
            var parentGroupTag = new Tag(reader.ReadInt32());      // 0x18 int32  parent group tag
            var grandparentGroupTag = new Tag(reader.ReadInt32()); // 0x1C int32  grandparent group tag
            var groupName = new StringId(reader.ReadUInt32());     // 0x20 uint32 group name stringid
            Group = new TagGroup(groupTag, parentGroupTag, grandparentGroupTag, groupName);

            // Read dependencies
            var dependencies = new HashSet<int>();
            for (var j = 0; j < numDependencies; j++)
                dependencies.Add(reader.ReadInt32());
            Dependencies = new ReadOnlySet<int>(dependencies);

            // Read offsets
            _pointerOffsets = new List<uint>(numDataFixups);
            for (var j = 0; j < numDataFixups; j++)
                _pointerOffsets.Add(PointerToOffset(reader.ReadUInt32()));
            _resourceOffsets = new List<uint>(numResourceFixups);
            for (var j = 0; j < numResourceFixups; j++)
                _resourceOffsets.Add(PointerToOffset(reader.ReadUInt32()));
            _tagReferenceOffsets = new List<uint>(numTagReferenceFixups);
            for (var i = 0; i < numTagReferenceFixups; i++)
                _tagReferenceOffsets.Add(PointerToOffset(reader.ReadUInt32()));
        }

        /// <summary>
        /// Writes the header for the tag instance to a stream.
        /// </summary>
        /// <param name="writer">The stream to write to.</param>
        internal void WriteHeader(BinaryWriter writer)
        {
            writer.Write(Checksum);
            writer.Write((uint)TotalSize);
            writer.Write((short)Dependencies.Count);
            writer.Write((short)PointerOffsets.Count);
            writer.Write((short)ResourcePointerOffsets.Count);
            writer.Write((short)TagReferenceOffsets.Count);
            writer.Write(DefinitionOffset);
            writer.Write(Group.Tag.Value);
            writer.Write(Group.ParentTag.Value);
            writer.Write(Group.GrandparentTag.Value);
            writer.Write(Group.Name.Value);

            // Write dependencies
            foreach (var dependency in Dependencies)
                writer.Write(dependency);

            // Write offsets
            foreach (var offset in _pointerOffsets.Concat(_resourceOffsets).Concat(_tagReferenceOffsets))
                writer.Write(OffsetToPointer(offset));
        }

        /// <summary>
        /// Calculates the header size that would be needed for a given block of tag data.
        /// </summary>
        /// <param name="data">The descriptor to use.</param>
        /// <returns>The size of the tag's header.</returns>
        internal static uint CalculateHeaderSize(CachedTagData data) =>
            (uint)(TagHeaderSize +
                data.Dependencies.Count * 4 +
                data.PointerFixups.Count * 4 +
                data.ResourcePointerOffsets.Count * 4 +
                data.TagReferenceOffsets.Count * 4);

        /// <summary>
        /// Updates the tag instance's state from a block of tag data.
        /// </summary>
        /// <param name="data">The tag data.</param>
        /// <param name="dataOffset">The offset of the tag data relative to the tag instance's header.</param>
        internal void Update(CachedTagData data, uint dataOffset)
        {
            Group = data.Group;
            Offset = data.MainStructOffset + dataOffset;
            Dependencies = new ReadOnlySet<int>(new HashSet<int>(data.Dependencies));
            _pointerOffsets = data.PointerFixups.Select(fixup => fixup.WriteOffset + dataOffset).ToList();
            _resourceOffsets = data.ResourcePointerOffsets.Select(offset => offset + dataOffset).ToList();
            _tagReferenceOffsets = data.TagReferenceOffsets.Select(offset => offset + dataOffset).ToList();
        }
    }

    public class StringTableHaloOnline : StringTable
    {
        public FileInfo StringIdCacheFile;
        public FileStream OpenStringIdCacheRead() => StringIdCacheFile.OpenRead();
        public FileStream OpenStringIdCacheWrite() => StringIdCacheFile.Open(FileMode.Open, FileAccess.Write);
        public FileStream OpenStringIdCacheReadWrite() => StringIdCacheFile.Open(FileMode.Open, FileAccess.ReadWrite);

        public StringTableHaloOnline(CacheVersion version, DirectoryInfo directory)
        {
            Version = version;

            var files = directory.GetFiles("string_ids.dat");
            if (files.Length == 0)
                throw new FileNotFoundException(Path.Combine(directory.FullName, "string_ids.dat"));
            StringIdCacheFile = files[0];
            

            Resolver = null;

            if (CacheVersionDetection.Compare(Version, CacheVersion.HaloOnline700123) >= 0)
                Resolver = new StringIdResolverMS30();
            else if (CacheVersionDetection.Compare(Version, CacheVersion.HaloOnline498295) >= 0)
                Resolver = new StringIdResolverMS28();
            else
                Resolver = new StringIdResolverMS23();

            using (var stream = OpenStringIdCacheRead())
            {
                if (stream.Length != 0)
                    Load(stream);
                else
                    Clear();
            }
                
        }

        public override StringId AddString(string newString)
        {
            throw new NotImplementedException();
        }

        public override void Save()
        {
            using(var stream = OpenStringIdCacheReadWrite())
            {
                Save(stream);
            }
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
}
