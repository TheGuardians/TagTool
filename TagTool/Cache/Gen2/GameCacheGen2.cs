using System;
using System.IO;
using TagTool.BlamFile;
using TagTool.Cache.Gen2;
using TagTool.Cache.Resources;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Cache
{
    public class GameCacheGen2 : GameCache
    {
        public MapFile BaseMapFile;
        public FileInfo CacheFile;

        public TagCacheGen2 TagCacheGen2;
        public StringTableGen2 StringTableGen2;
        public CacheFileType SharedCacheType = CacheFileType.None;
        public string SharedCacheName;
        public FileInfo SharedCacheFile;
        public GameCacheGen2 SharedCache;

        public override TagCache TagCache => TagCacheGen2;
        public override StringTable StringTable => StringTableGen2;

        public override ResourceCache ResourceCache => throw new NotImplementedException();

        public GameCacheGen2(MapFile mapFile, FileInfo file)
        {
            BaseMapFile = mapFile;
            CacheFile = file;
            Version = BaseMapFile.Version;
            CacheFile = file;
            Deserializer = new TagDeserializer(Version);
            Serializer = new TagSerializer(Version);
            Endianness = BaseMapFile.EndianFormat;
            DisplayName = mapFile.Header.Name + ".map";
            Directory = file.Directory;

            switch (BaseMapFile.Header.CacheType)
            {
                case CacheFileType.Campaign:
                    SharedCacheType = CacheFileType.SharedCampaign;
                    SharedCacheName = "single_player_shared.map";
                    break;
                case CacheFileType.Multiplayer:
                case CacheFileType.MainMenu:    // see if this is necessary
                    SharedCacheType = CacheFileType.Shared;
                    SharedCacheName = "shared.map";
                    break;
            }

            using (var cacheStream = OpenCacheRead())
            using (var reader = new EndianReader(cacheStream, Endianness))
            {
                TagCacheGen2 = new TagCacheGen2(reader, mapFile);
                StringTableGen2 = new StringTableGen2(reader, mapFile);

                LoadSharedCache();

                if (Version == CacheVersion.Halo2Xbox)
                    TagCacheGen2.FixupStructureBspTagsForXbox(reader, mapFile);
            }

            
        }

        private void LoadSharedCache()
        {
            if (SharedCacheType == CacheFileType.None)
                return;
            
            SharedCacheFile = new FileInfo(Path.Combine(Directory.FullName, SharedCacheName));
            SharedCache = (GameCacheGen2)GameCache.Open(SharedCacheFile);
            TagCacheGen2 = TagCacheGen2.Combine(TagCacheGen2, SharedCache.TagCacheGen2);
        }

        #region Serialization

        public override T Deserialize<T>(Stream stream, CachedTag instance) =>
            Deserialize<T>(new Gen2SerializationContext(stream, this, (CachedTagGen2)instance));

        public override object Deserialize(Stream stream, CachedTag instance) =>
            Deserialize(new Gen2SerializationContext(stream, this, (CachedTagGen2)instance), TagCache.TagDefinitions.GetTagDefinitionType(instance.Group));

        public override void Serialize(Stream stream, CachedTag instance, object definition)
        {
            if (typeof(CachedTagGen2) == instance.GetType())
                Serialize(stream, (CachedTagGen2)instance, definition);
            else
                throw new Exception($"Try to serialize a {instance.GetType()} into a Gen 3 Game Cache");
        }

        public void Serialize(Stream stream, CachedTagGen2 instance, object definition)
        {
            throw new NotImplementedException();
        }

        public T Deserialize<T>(Stream stream, CachedTagGen2 instance) =>
            Deserialize<T>(new Gen2SerializationContext(stream, this, instance));

        public object Deserialize(Stream stream, CachedTagGen2 instance) =>
            Deserialize(new Gen2SerializationContext(stream, this, instance), TagCache.TagDefinitions.GetTagDefinitionType(instance.Group));

        //
        // private methods for internal use
        //

        private T Deserialize<T>(ISerializationContext context) =>
            Deserializer.Deserialize<T>(context);

        private object Deserialize(ISerializationContext context, Type type) =>
            Deserializer.Deserialize(context, type);

        #endregion


        public override Stream OpenCacheRead() => new Gen2CacheStream(CacheFile.OpenRead(), SharedCache?.OpenCacheRead());

        public override Stream OpenCacheReadWrite()
        {
            throw new NotImplementedException();
        }

        public override Stream OpenCacheWrite()
        {
            throw new NotImplementedException();
        }

        public override void SaveStrings()
        {
            throw new NotImplementedException();
        }

        // Purpose: to opaquely manage the lifetime of the shared cache stream
        internal class Gen2CacheStream : Stream
        {
            internal readonly Stream BaseStream;
            internal readonly Stream SharedStream;

            public Gen2CacheStream(Stream baseStream, Stream sharedStream)
            {
                BaseStream = baseStream;
                SharedStream = sharedStream;
            }

            public override bool CanRead => BaseStream.CanRead;

            public override bool CanSeek => BaseStream.CanSeek;

            public override bool CanWrite => BaseStream.CanWrite;

            public override long Length => BaseStream.Length;

            public override long Position { get => BaseStream.Position; set => BaseStream.Position = value; }

            public override void Flush() => BaseStream.Flush();

            public override int Read(byte[] buffer, int offset, int count) => BaseStream.Read(buffer, offset, count);

            public override long Seek(long offset, SeekOrigin origin) => BaseStream.Seek(offset, origin);

            public override void SetLength(long value) => BaseStream.SetLength(value);

            public override void Write(byte[] buffer, int offset, int count) => BaseStream.Write(buffer, offset, count);

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                if (disposing && SharedStream != null)
                    SharedStream.Dispose();
            }
        }
    }
}
