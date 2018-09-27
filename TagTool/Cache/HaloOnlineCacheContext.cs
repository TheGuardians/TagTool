using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Resources;

namespace TagTool.Cache
{
    /// <summary>
    /// Manages game cache file interop.
    /// </summary>
    public class HaloOnlineCacheContext : GameCacheContext
    {
        public HaloOnlineCacheContext(DirectoryInfo directory) :
            base(directory)
        {
            var tagNames = LoadTagNames();

            using (var stream = OpenTagCacheRead())
                TagCache = new TagCache(stream, tagNames);

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

            TagGroup.Instances[new Tag("obje")] = new TagGroup(new Tag("obje"), Tag.Null, Tag.Null, GetStringId("object"));
            TagGroup.Instances[new Tag("item")] = new TagGroup(new Tag("item"), new Tag("obje"), Tag.Null, GetStringId("item"));
            TagGroup.Instances[new Tag("devi")] = new TagGroup(new Tag("devi"), new Tag("obje"), Tag.Null, GetStringId("device"));
            TagGroup.Instances[new Tag("unit")] = new TagGroup(new Tag("unit"), new Tag("obje"), Tag.Null, GetStringId("unit"));
            TagGroup.Instances[new Tag("rm  ")] = new TagGroup(new Tag("rm  "), Tag.Null, Tag.Null, GetStringId("render_method"));
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
                cache = new TagCache(stream, new Dictionary<int, string>());
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
        /// Gets a tag from the current cache.
        /// </summary>
        /// <param name="index">The index of the tag.</param>
        /// <returns>The tag at the specified index from the current cache.</returns>
        public override CachedTagInstance GetTag(int index)
        {
            if (index < 0 || index >= TagCache.Index.Count)
                throw new IndexOutOfRangeException($"0x{index:X4}");

            return TagCache.Index[index];
        }

        /// <summary>
        /// Gets a tag of a specific type from the current cache.
        /// </summary>
        /// <typeparam name="T">The type of the tag definition.</typeparam>
        /// <param name="name">The name of the tag.</param>
        /// <returns>The tag of the specified type with the specified name from the current cache.</returns>
        public override CachedTagInstance GetTag<T>(string name)
        {
            if (TryGetTag<T>(name, out var result))
                return result;

			var attribute = TagDefinition.GetTagStructureAttribute(typeof(T));
            var typeName = attribute.Name ?? typeof(T).Name.ToSnakeCase();

            throw new KeyNotFoundException($"'{typeName}' tag \"{name}\"");
        }

        public bool TryAllocateTag(out CachedTagInstance result, Type type, string name = null)
        {
            result = null;

            try
            {
                var structure = TagDefinition.GetTagStructureInfo(type, Version).Structure;

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

                result = TagCache.AllocateTag(TagGroup.Instances[groupTag], name);

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

        public CachedTagInstance AllocateTag(Type type, string name = null)
        {
            if (TryAllocateTag(out var result, type, name))
                return result;

            Console.WriteLine($"Failed to allocate tag of type \"{type.Name}\".");
            return null;
        }

        public CachedTagInstance AllocateTag<T>(string name = null) where T : TagStructure
			=> AllocateTag(typeof(T), name);

        /// <summary>
        /// Attempts to get a tag of a specific type from the current cache.
        /// </summary>
        /// <typeparam name="T">The type of the tag definition.</typeparam>
        /// <param name="name">The name of the tag.</param>
        /// <param name="result">The resulting tag.</param>
        /// <returns>True if the tag was found, false otherwise.</returns>
        public bool TryGetTag<T>(string name, out CachedTagInstance result) where T : TagStructure
        {
            if (Tags.TagDefinition.Types.Values.Contains(typeof(T)))
            {
                var groupTag = Tags.TagDefinition.Types.First((KeyValuePair<Tag, Type> entry) => entry.Value == typeof(T)).Key;

                foreach (var instance in TagCache.Index)
                {
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
        public bool TryGetTag(string name, out CachedTagInstance result)
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
                if (TagCache.Index.Count == 0)
                {
                    result = null;
                    return false;
                }

                result = TagCache.Index.Last();
                return true;
            }

            if (name.StartsWith("*."))
            {
                if (!name.TrySplit('.', out var startNamePieces) || !TryParseGroupTag(startNamePieces[1], out var starGroupTag))
                {
                    result = null;
                    return false;
                }

                result = TagCache.Index.Last(tag => tag.IsInGroup(starGroupTag));
                return true;
            }

            if (name.StartsWith("0x"))
            {
                name = name.Substring(2);

                if (name.TrySplit('.', out var hexNamePieces))
                    name = hexNamePieces[0];

                if (!int.TryParse(name, NumberStyles.HexNumber, null, out int tagIndex) || !TagCache.Index.Contains(tagIndex))
                {
                    result = null;
                    return false;
                }

                result = TagCache.Index[tagIndex];
                return true;
            }

            if (!name.TrySplit('.', out var namePieces) || !TryParseGroupTag(namePieces[1], out var groupTag))
                throw new Exception($"Invalid tag name: {name}");

            var tagName = namePieces[0];

            foreach (var instance in TagCache.Index)
            {
                if (instance.IsInGroup(groupTag) && instance.Name == name)
                {
                    result = instance;
                    return true;
                }
            }

            result = null;
            return false;
        }

        public CachedTagInstance GetTag(string name)
        {
            if (TryGetTag(name, out var result))
                return result;
            
            throw new KeyNotFoundException(name);
        }

        public T Deserialize<T>(Stream stream, CachedTagInstance instance) =>
            Deserialize<T>(new TagSerializationContext(stream, this, instance));

        public object Deserialize(Stream stream, CachedTagInstance instance) =>
            Deserialize(new TagSerializationContext(stream, this, instance), Tags.TagDefinition.Find(instance.Group.Tag));

        public void Serialize(Stream stream, CachedTagInstance instance, object definition) =>
            Serializer.Serialize(new TagSerializationContext(stream, this, instance), definition);

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
				var attribute = TagDefinition.GetTagStructureAttribute(type);
                result = new Tag(attribute.Tag);
                return true;
            }

            foreach (var pair in TagGroup.Instances)
            {
                if (name == GetString(pair.Value.Name))
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
                foreach (var instance in TagCache.Index)
                    if (instance != null && instance.Name != null && !instance.Name.ToLower().StartsWith("0x"))
                        csvWriter.WriteLine($"0x{instance.Index:X8},{instance.Name}");
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
        public override string GetString(StringId id) => StringIdCache.GetString(id);

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
        /// Adds a new pageable_resource to the current cache.
        /// </summary>
        /// <param name="resource">The pageable_resource to add.</param>
        /// <param name="dataStream">The stream to read the resource data from.</param>
        /// <exception cref="System.ArgumentNullException">resource</exception>
        /// <exception cref="System.ArgumentException">The input stream is not open for reading;dataStream</exception>
        public override void AddResource(PageableResource resource, Stream dataStream)
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
        public override void ExtractResource(PageableResource pageable, Stream outStream)
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

        public T Deserialize<T>(PageableResource pageable) =>
            Deserialize<T>(new ResourceSerializationContext(pageable));

        public object Deserialize(PageableResource pageable)
        {
            switch (pageable.Resource.Type)
            {
                case TagResourceType.Animation:
                    return Deserialize<ModelAnimationTagResource>(pageable);

                case TagResourceType.Bink:
                    return Deserialize<BinkResource>(pageable);

                case TagResourceType.Bitmap:
                case TagResourceType.BitmapInterleaved:
                    return Deserialize<BitmapTextureInteropResource>(pageable);

                case TagResourceType.Collision:
                    return Deserialize<StructureBspTagResources>(pageable);

                case TagResourceType.Pathfinding:
                    return Deserialize<StructureBspCacheFileTagResources>(pageable);

                case TagResourceType.RenderGeometry:
                    return Deserialize<RenderGeometryApiResourceDefinition>(pageable);

                case TagResourceType.Sound:
                    return Deserialize<SoundResourceDefinition>(pageable);

                default:
                    throw new NotSupportedException(pageable.Resource.Type.ToString());
            }
        }

        public void Serialize(PageableResource pageable, object definition) =>
            Serialize(new ResourceSerializationContext(pageable), definition);

        private class LoadedResourceCache
        {
            public ResourceCache Cache { get; set; }

            public FileInfo File { get; set; }
        }
        #endregion
    }
}