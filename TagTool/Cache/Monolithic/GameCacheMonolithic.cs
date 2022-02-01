using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache.Resources;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;

namespace TagTool.Cache.Monolithic
{
    public class GameCacheMonolithic : GameCache
    {
        public MonolithicTagFileBackend Backend;

        public TagCacheMonolithic TagCacheMono;
        public StringTableMonolithic StringTableMono;
        public ResourceCacheMonolithic ResourceCacheMono;
        public Dictionary<Tag, TagLayout> TagLayoutCache;

        public GameCacheMonolithic(FileInfo file, CacheVersion version = CacheVersion.Unknown, CachePlatform platform = CachePlatform.All)
        {
            if (version == CacheVersion.Unknown)
                DetectCacheVersion(file, out version, out platform);

            Version = version;
            Platform = platform;
            Endianness = CacheVersionDetection.IsLittleEndian(version, Platform) ? EndianFormat.LittleEndian : EndianFormat.BigEndian;
            Backend = new MonolithicTagFileBackend(file, Endianness);
            Deserializer = new TagDeserializer(version, platform);
            TagCacheMono = new TagCacheMonolithic(Backend, version, platform);
            StringTableMono = new StringTableMonolithic();
            ResourceCacheMono = new ResourceCacheMonolithic(this);
            TagLayoutCache = new Dictionary<Tag, TagLayout>();
        }

        private void DetectCacheVersion(FileInfo file, out CacheVersion version, out CachePlatform platform)
        {
            // try to detect the cache version from the session id

            Guid guid;
            using (var reader = new EndianReader(file.OpenRead()))
                guid = new Guid(reader.ReadBytes(16));

            switch (guid.ToString())
            {
                // 11883.10.10.25.1227.dlc_1_ship__tag_test
                case "0237d057-1e3c-4390-9cfc-6108a911de01":
                    version = CacheVersion.HaloReach;
                    platform = CachePlatform.Original;
                    break;
                default:
                    throw new Exception("Unable to detect monolothic cache version");
            }
        }

        public override StringTable StringTable => StringTableMono;

        public override TagCache TagCache => TagCacheMono;

        public override ResourceCache ResourceCache => ResourceCacheMono;

        public override object Deserialize(Stream stream, CachedTag instance)
        {
            var definitionType = TagCache.TagDefinitions.GetTagDefinitionType(instance.Group);
            return Deserializer.Deserialize(new TagSerializationContextMonolithic(stream, this, (CachedTagMonolithic)instance), definitionType);
        }

        public override T Deserialize<T>(Stream stream, CachedTag instance)
        {
            return Deserializer.Deserialize<T>(new TagSerializationContextMonolithic(stream, this, (CachedTagMonolithic)instance));
        }

        public override Stream OpenCacheRead()
        {
            return new MemoryStream();
        }

        public override Stream OpenCacheReadWrite()
            => throw new NotImplementedException();

        public override Stream OpenCacheWrite()
            => throw new NotImplementedException();

        public override void SaveStrings()
            => throw new NotImplementedException();

        public override void Serialize(Stream stream, CachedTag instance, object definition)
            => throw new NotImplementedException();
    }
}
