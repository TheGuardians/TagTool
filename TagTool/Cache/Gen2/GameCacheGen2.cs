using System;
using System.Collections.Generic;
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
        public string VistaSharedTagCacheName;
        public FileInfo VistaSharedTagCacheFile;
        public GameCacheGen2 VistaSharedTagCache;
        public ResourceCacheGen2 ResourceCacheGen2;
        public Dictionary<string, GameCacheGen2> ResourceCacheReferences = new Dictionary<string, GameCacheGen2>();

        public override TagCache TagCache => TagCacheGen2;
        public override StringTable StringTable => StringTableGen2;

        public override ResourceCache ResourceCache => ResourceCacheGen2;

        public GameCacheGen2(MapFile mapFile, FileInfo file)
        {
            BaseMapFile = mapFile;
            CacheFile = file;
            Version = BaseMapFile.Version;
            Platform = BaseMapFile.CachePlatform;
            CacheFile = file;
            Deserializer = new TagDeserializer(Version, Platform);
            Serializer = new TagSerializer(Version, Platform);
            Endianness = BaseMapFile.EndianFormat;
            DisplayName = mapFile.Header.GetName() + ".map";
            Directory = file.Directory;

            switch (BaseMapFile.Header.GetCacheType())
            {
                case CacheFileType.Campaign:
                    SharedCacheType = CacheFileType.SharedCampaign;
                    VistaSharedTagCacheName = "single_player_shared.map";
                    break;
                case CacheFileType.Multiplayer:
                case CacheFileType.MainMenu:    // see if this is necessary
                    SharedCacheType = CacheFileType.Shared;
                    VistaSharedTagCacheName = "shared.map";
                    break;
            }

            LoadSharedResourceCaches();

            using (var cacheStream = OpenCacheRead())
            using (var reader = new EndianReader(cacheStream, Endianness))
            {
                TagCacheGen2 = new TagCacheGen2(reader, mapFile);
                StringTableGen2 = new StringTableGen2(reader, mapFile);

                if (Version <= CacheVersion.Halo2Xbox)
                    TagCacheGen2.FixupStructureBspTagsForXbox(reader, mapFile);
                else
                    if (LoadVistaSharedTagCache())
                        TagCacheGen2 = TagCacheGen2.Combine(TagCacheGen2, VistaSharedTagCache.TagCacheGen2);
            }

            ResourceCacheGen2 = new ResourceCacheGen2(this);
        }

        private bool LoadVistaSharedTagCache()
        {
            if (SharedCacheType == CacheFileType.None)
                return false;

            VistaSharedTagCacheFile = new FileInfo(Path.Combine(Directory.FullName, VistaSharedTagCacheName));

            if (!File.Exists(VistaSharedTagCacheFile.FullName))
            {
                Console.WriteLine($"Warning, shared map file required to load tags not found: {VistaSharedTagCacheName}");
                return false;
            }

            VistaSharedTagCache = (GameCacheGen2)GameCache.Open(VistaSharedTagCacheFile);
            return true;
        }

        private void LoadSharedResourceCaches()
        {
            var thisName = BaseMapFile.Header.GetName() + ".map";

            if (thisName == "shared.map" || thisName == "single_player_shared.map")
                return;


            GameCacheGen2 mainmenuCache;
            if (thisName == "mainmenu.map")
                mainmenuCache = this;
            else
            {
                var mainmenuFile = new FileInfo(Path.Combine(Directory.FullName, "mainmenu.map"));
                mainmenuCache = (GameCacheGen2)GameCache.Open(mainmenuFile);
            }
            ResourceCacheReferences.Add("mainmenu.map", mainmenuCache);

            GameCacheGen2 sharedCache;
            if (thisName == "shared.map")
                sharedCache = this;
            else
            {
                var sharedFile = new FileInfo(Path.Combine(Directory.FullName, "shared.map"));
                sharedCache = (GameCacheGen2)GameCache.Open(sharedFile);
            }
            ResourceCacheReferences.Add("shared.map", sharedCache);


            if(Version > CacheVersion.Halo2Beta)
            {
                GameCacheGen2 spSharedCache;
                if (thisName == "single_player_shared.map")
                    spSharedCache = this;
                else
                {
                    var spSharedFile = new FileInfo(Path.Combine(Directory.FullName, "single_player_shared.map"));
                    spSharedCache = (GameCacheGen2)GameCache.Open(spSharedFile);
                }
                ResourceCacheReferences.Add("single_player_shared.map", spSharedCache);
            }
            
        }

        public byte[] GetCacheRawData(uint address, int size)
        {
            var cacheFileType = (address & 0xC0000000) >> 30;
            int fileOffset = (int)(address & 0x3FFFFFFF);

            GameCacheGen2 sourceCache = null;
            switch (cacheFileType)
            {
                case 0:
                    sourceCache = this;
                    break;
                case 1:
                    sourceCache = ResourceCacheReferences["mainmenu.map"];
                    break;
                case 2:
                    sourceCache = ResourceCacheReferences["shared.map"];
                    break;
                case 3:
                    sourceCache = ResourceCacheReferences["single_player_shared.map"];
                    break;

            }
            if(sourceCache == null)
            {
                Console.WriteLine($"Failed to get shared cache type {cacheFileType}");
                return null;
            }

            var stream = sourceCache.OpenCacheRead();

            var reader = new EndianReader(stream, Endianness);

            reader.SeekTo(fileOffset);
            var data = reader.ReadBytes(size);

            reader.Close();

            return data;
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


        public override Stream OpenCacheRead() => new Gen2CacheStream(CacheFile.OpenRead(), VistaSharedTagCache?.OpenCacheRead());

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
