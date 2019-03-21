using System;
using System.IO;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Cache
{
    public abstract class GameCacheContext
    {
        /// <summary>
        /// Gets the directory of the current cache context.
        /// </summary>
        public DirectoryInfo Directory { get; }

        /// <summary>
        /// Gets the engine version of the cache files.
        /// </summary>
        public CacheVersion Version { get; protected set; }

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
        /// <param name="type">The type of object to deserialize the tag data as.</param>
        /// <returns>The object that was read.</returns>
        public object Deserialize(ISerializationContext context, Type type) =>
            Deserializer.Deserialize(context, type);

        public GameCacheContext(DirectoryInfo directory)
        {
            Directory = directory;
        }
        
        /// <summary>
        /// Gets a tag from the current cache.
        /// </summary>
        /// <param name="index">The index of the tag.</param>
        /// <returns>The tag at the specified index from the current cache.</returns>
        public abstract CachedTagInstance GetTag(int index);

        /// <summary>
        /// Gets a tag from the current cache.
        /// </summary>
        /// <typeparam name="T">The type of the tag definition.</typeparam>
        /// <param name="name">The name of the tag.</param>
        /// <returns>The tag of the specified type with the specified name from the current cache.</returns>
        public abstract CachedTagInstance GetTag<T>(string name) where T : TagStructure;

        /// <summary>
        /// Gets the string associated with the specified id from the current cache.
        /// </summary>
        /// <param name="id">The <see cref="StringId"/> of the string.</param>
        /// <returns>The string associated with the specified <see cref="StringId"/> from the current cache.</returns>
        public abstract string GetString(StringId id);

        /// <summary>
        /// Gets the <see cref="StringId"/> associated with the input string from the current cache.
        /// </summary>
        /// <param name="input">The input string represented by the <see cref="StringId"/>.</param>
        /// <returns>The <see cref="StringId"/> associated with the input string from the current cache.</returns>
        public abstract StringId GetStringId(string input);

        /// <summary>
        /// Adds a new pageable_resource to the current cache.
        /// </summary>
        /// <param name="pageable">The <see cref="PageableResource"/> to add.</param>
        /// <param name="stream">The <see cref="Stream"/> to read the resource data from.</param>
        public abstract void AddResource(PageableResource pageable, Stream stream);

        /// <summary>
        /// Extracts and decompresses the data for a <see cref="PageableResource"/> from the current cache.
        /// </summary>
        /// <param name="resource">The <see cref="PageableResource"/> to extract.</param>
        /// <param name="stream">The <see cref="Stream"/> to write the extracted data to.</param>
        public abstract void ExtractResource(PageableResource resource, Stream stream);
    }
}