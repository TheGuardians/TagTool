using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TagTool.Tags;

namespace TagTool.Cache
{
    /// <summary>
    /// Manages game cache file interop.
    /// </summary>
    public class GameCacheContext
    {
        /// <summary>
        /// Gets the directory of the current cache context.
        /// </summary>
        public DirectoryInfo Directory { get; }

        /// <summary>
        /// Gets the engine version of the cache files.
        /// </summary>
        public CacheVersion Version { get; }

        /// <summary>
        /// The tag serializer to use.
        /// </summary>
        public TagSerializer Serializer { get; set; }

        /// <summary>
        /// Serializes a tag structure into a context.
        /// </summary>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="definition">The tag definition to serialize.</param>
        /// <param name="offset">An optional offset to begin serializing at.</param>
        public void Serialize(ISerializationContext context, object definition, uint? offset = null) =>
            Serializer.Serialize(context, definition, offset);

        /// <summary>
        /// The tag deserializer to use.
        /// </summary>
        public TagDeserializer Deserializer { get; set; }

        /// <summary>
        /// Deserializes tag data into an object.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the tag data as.</typeparam>
        /// <param name="context">The serialization context to use.</param>
        /// <returns>The object that was read.</returns>
        public T Deserialize<T>(ISerializationContext context) =>
            Deserializer.Deserialize<T>(context);

        /// <summary>
        /// Deserializes tag data into an object.
        /// </summary>
        /// <param name="context">The serialization context to use.</param>
        /// <param name="structureType">The type of object to deserialize the tag data as.</param>
        /// <returns>The object that was read.</returns>
        public object Deserialize(ISerializationContext context, Type type) =>
            Deserializer.Deserialize(context, type);

        public GameCacheContext(DirectoryInfo directory)
        {
            Directory = directory;

            using (var stream = OpenTagCacheRead())
                TagCache = new TagCache(stream);

            if (CacheVersion.Unknown == (Version = CacheVersionDetection.DetectFromTagCache(TagCache, out var closestVersion)))
                Version = closestVersion;

            Deserializer = new TagDeserializer(Version == CacheVersion.Unknown ? closestVersion : Version);
            Serializer = new TagSerializer(Version == CacheVersion.Unknown ? closestVersion : Version);

            StringIdResolver stringIdResolver = null;

            if (CacheVersionDetection.Compare(Version, CacheVersion.HaloOnline700123) >= 0)
                stringIdResolver = new StringIdResolverMS30();
            else if (CacheVersionDetection.Compare(Version, CacheVersion.HaloOnline498295) >= 0)
                stringIdResolver = new StringIdResolverMS28();
            else
                stringIdResolver = new StringIdResolverMS23();

            using (var stream = OpenStringIdCacheRead())
                StringIdCache = new StringIdCache(stream, stringIdResolver);

            LoadTagNames();
        }

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

        /// <summary>
        /// A dictionary of tag names.
        /// </summary>
        public Dictionary<int, string> TagNames { get; set; } = new Dictionary<int, string>();

        /// <summary>
        /// A dictionary of all ElDorado RenderMethodTemplates and lists of their bitmaps and arguments names.
        /// </summary>
        public Dictionary<int, List<List<string>>> Rmt2TagsInfo { get; set; } = new Dictionary<int, List<List<string>>>();

        /// <summary>
        /// The tag cache.
        /// </summary>
        public TagCache TagCache { get; set; }

        public TagCache CreateTagCache(DirectoryInfo directory = null)
        {
            if (directory == null)
                directory = Directory;

            if (!directory.Exists)
                directory.Create();

            var file = new FileInfo(Path.Combine(directory.FullName, "tags.dat"));

            TagCache cache = null;

            using (var stream = file.Create())
            using (var writer = new BinaryWriter(stream))
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
                cache = new TagCache(stream);
            }

            return cache;
        }

        /// <summary>
        /// Opens the tag cache file for reading.
        /// </summary>
        /// <returns>The stream that was opened.</returns>
        public Stream OpenTagCacheRead() => TagCacheFile.OpenRead();

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
        /// Gets a tag from the tag cache.
        /// </summary>
        /// <param name="index">The index of the tag.</param>
        /// <returns></returns>
        public CachedTagInstance GetTag(int index) => index >= 0 ? TagCache.Index[index] : throw new KeyNotFoundException(index.ToString());

        public CachedTagInstance GetTagInstance<T>(string name)
        {
            var groupTag = TagDefinition.Types.First(entry => entry.Value == typeof(T)).Key;

            foreach (var entry in TagNames)
            {
                if (entry.Value != name)
                    continue;

                var instance = TagCache.Index[entry.Key];

                if (instance.IsInGroup(groupTag))
                    return instance;
            }

            throw new KeyNotFoundException($"'{groupTag}' tag \"{name}\"");
        }

        /// <summary>
        /// Loads tag file names from the appropriate tagnames.csv file.
        /// </summary>
        public void LoadTagNames()
        {
            var tagNamesPath = Path.Combine(Directory.FullName, "tag_list.csv");

            if (File.Exists(tagNamesPath))
            {
                using (var tagNamesStream = File.Open(tagNamesPath, FileMode.Open, FileAccess.Read))
                {
                    var reader = new StreamReader(tagNamesStream);

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var separatorIndex = line.IndexOf(',');
                        var indexString = line.Substring(2, separatorIndex - 2);

                        if (!int.TryParse(indexString, NumberStyles.HexNumber, null, out int tagIndex))
                            tagIndex = -1;

                        if (tagIndex < 0 || tagIndex >= TagCache.Index.Count || TagCache.Index[tagIndex] == null)
                            continue;

                        var nameString = line.Substring(separatorIndex + 1);

                        if (nameString.Contains(" "))
                        {
                            var lastSpaceIndex = nameString.LastIndexOf(' ');
                            nameString = nameString.Substring(lastSpaceIndex + 1, nameString.Length - lastSpaceIndex - 1);
                        }

                        TagNames[tagIndex] = nameString;
                    }

                    reader.Close();
                }
            }
        }

        public void SaveTagNames(string path = null)
        {
            var csvFile = new FileInfo(path ?? Path.Combine(Directory.FullName, "tag_list.csv"));

            if (!csvFile.Directory.Exists)
                csvFile.Directory.Create();

            using (var csvWriter = new StreamWriter(csvFile.Create()))
            {
                var entries = TagNames.ToList();
                entries.Sort((a, b) => a.Key.CompareTo(b.Key));

                foreach (var entry in entries)
                    if (TagCache.Index[entry.Key] != null && !entry.Value.ToLower().StartsWith("0x"))
                        csvWriter.WriteLine($"0x{entry.Key:X8},{entry.Value}");
            }
        }
        #endregion

        #region StringId Cache Functionality
        /// <summary>
        /// Gets the string_id cache file information.
        /// </summary>
        public FileInfo StringIdCacheFile
        {
            get
            {
                var files = Directory.GetFiles("string_ids.dat");

                if (files.Length == 0)
                    throw new FileNotFoundException(Path.Combine(Directory.FullName, "string_ids.dat"));

                return files[0];
            }
        }

        /// <summary>
        /// The stringID cache.
        /// Can be <c>null</c>.
        /// </summary>
        public StringIdCache StringIdCache { get; set; }

        /// <summary>
        /// Opens the string_id cache file for reading.
        /// </summary>
        /// <returns>The stream that was opened.</returns>
        public FileStream OpenStringIdCacheRead() => StringIdCacheFile.OpenRead();

        /// <summary>
        /// Opens the string_id cache file for writing.
        /// </summary>
        /// <returns>The stream that was opened.</returns>
        public FileStream OpenStringIdCacheWrite() => StringIdCacheFile.Open(FileMode.Open, FileAccess.Write);

        /// <summary>
        /// Opens the string_id cache file for reading and writing.
        /// </summary>
        /// <returns>The stream that was opened.</returns>
        public FileStream OpenStringIdCacheReadWrite() => StringIdCacheFile.Open(FileMode.Open, FileAccess.ReadWrite);

        /// <summary>
        /// Gets a string from the string_id cache.
        /// </summary>
        /// <param name="id">The id of the string.</param>
        /// <returns></returns>
        public string GetString(StringId id) => StringIdCache.GetString(id);

        /// <summary>
        /// Gets the string_id associated with the specified value from the string_id cache.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <returns></returns>
        public StringId GetStringId(string value) => StringIdCache.GetStringId(value);

        /// <summary>
        /// Gets the string_id associated with the specified index from the string_id cache.
        /// </summary>
        /// <param name="index">The index of the string.</param>
        /// <returns></returns>
        public StringId GetStringId(int index) => StringIdCache.GetStringId(index);
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

        public ResourceCache CreateResourceCache(DirectoryInfo directory, ResourceLocation location)
        {
            if (!directory.Exists)
                directory.Create();

            var file = new FileInfo(Path.Combine(directory.FullName, ResourceCacheNames[location]));

            ResourceCache cache = null;

            using (var stream = file.Create())
            using (var writer = new BinaryWriter(stream))
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
        /// Adds a new resource to a cache.
        /// </summary>
        /// <param name="resource">The resource reference to initialize.</param>
        /// <param name="location">The location where the resource should be stored.</param>
        /// <param name="dataStream">The stream to read the resource data from.</param>
        /// <exception cref="System.ArgumentNullException">resource</exception>
        /// <exception cref="System.ArgumentException">The input stream is not open for reading;dataStream</exception>
        public void AddResource(PageableResource resource, ResourceLocation location, Stream dataStream)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");
            if (!dataStream.CanRead)
                throw new ArgumentException("The input stream is not open for reading", "dataStream");

            resource.ChangeLocation(location);
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
        /// <param name="location">The location where the resource should be stored.</param>
        /// <param name="data">The pre-compressed data to store.</param>
        public void AddRawResource(PageableResource resource, ResourceLocation location, byte[] data)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");

            resource.ChangeLocation(location);
            resource.DisableChecksum();
            var cache = GetResourceCache(resource);
            using (var stream = cache.File.Open(FileMode.Open, FileAccess.ReadWrite))
                resource.Page.Index = cache.Cache.AddRaw(stream, data);
        }

        /// <summary>
        /// Extracts and decompresses the data for a resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="outStream">The stream to write the extracted data to.</param>
        /// <exception cref="System.ArgumentException">Thrown if the output stream is not open for writing.</exception>
        /// <exception cref="System.InvalidOperationException">Thrown if the file containing the resource has not been loaded.</exception>
        public void ExtractResource(PageableResource resource, Stream outStream)
        {
            if (resource == null)
                throw new ArgumentNullException("resource");
            if (!outStream.CanWrite)
                throw new ArgumentException("The output stream is not open for writing", "outStream");

            var cache = GetResourceCache(resource);
            using (var stream = cache.File.OpenRead())
                cache.Cache.Decompress(stream, resource.Page.Index, resource.Page.CompressedBlockSize, outStream);
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

        private class LoadedResourceCache
        {
            public ResourceCache Cache { get; set; }

            public FileInfo File { get; set; }
        }
        #endregion
    }
}
