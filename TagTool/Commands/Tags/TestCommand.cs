using HaloShaderGenerator.Globals;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Bitmaps;
using TagTool.Bitmaps.Utils;
using TagTool.Bitmaps.DDS;

using TagTool.Tags.Definitions.Gen2;
using TagTool.Audio.Converter;
using TagTool.Audio;
using TagTool.Cache.Gen2;
using TagTool.BlamFile;
using TagTool.Cache.ModPackages;
using System.Linq;

namespace TagTool.Commands
{

    public class TestCommand : Command
    {
        GameCache Cache;

        public TestCommand(GameCache cache) : base(false, "Test", "Test", "Test", "Test")
        {
            Cache = cache;
        }

        public byte[] GetCacheRawData(uint resourceAddress, int size)
        {
            var cacheFileType = (resourceAddress & 0xC0000000) >> 30;
            int fileOffset = (int)(resourceAddress & 0x3FFFFFFF);

            GameCacheGen2 sourceCache;

            if (cacheFileType != 0)
            {
                string filename = "";
                switch (cacheFileType)
                {
                    case 1:
                        filename = Path.Combine(Cache.Directory.FullName, "mainmenu.map");
                        break;
                    case 2:
                        filename = Path.Combine(Cache.Directory.FullName, "shared.map");
                        break;
                    case 3:
                        filename = Path.Combine(Cache.Directory.FullName, "single_player_shared.map");
                        break;

                }
                // TODO: make this a function call with a stored reference to caches in the base cache or something better than this
                sourceCache = (GameCacheGen2)GameCache.Open(new FileInfo(filename));
            }
            else
                sourceCache = (GameCacheGen2)Cache;

            var stream = sourceCache.OpenCacheRead();

            var reader = new EndianReader(stream, Cache.Endianness);

            reader.SeekTo(fileOffset);
            var data = reader.ReadBytes(size);

            reader.Close();

            return data;
        }

        private void SetCacheVersion(GameCacheHaloOnline cache, CacheVersion version)
        {
            cache.Version = version;
            cache.TagCacheGenHO.Version = version;
            cache.TagCacheGenHO.Header.CreationTime = CacheVersionDetection.GetTimestamp(version);
            cache.StringTableHaloOnline.Version = version;
            cache.Serializer = new TagSerializer(version, CachePlatform.Original);
            cache.Deserializer = new TagDeserializer(version, CachePlatform.Original);
            cache.ResourceCaches = new ResourceCachesHaloOnline(cache);
        }

        private void UpgradeCacheForReach(GameCacheHaloOnline cache)
        {
            Console.WriteLine("Upgrading to reach cache...");

            SetCacheVersion(cache, CacheVersion.HaloOnline106708);

            using (var stream = cache.OpenCacheReadWrite())
            {
                var tasks = new List<Action>();

                var renderGeometryTags = new HashSet<Tag>() { "mode", "pmdf", "Lbsp" };
                foreach (var tag in cache.TagCache.NonNull().Where(x => renderGeometryTags.Contains(x.Group.Tag)))
                {
                    var definition = cache.Deserialize(stream, tag);
                    tasks.Add(() =>
                    {
                        Console.WriteLine($"Upgrading {tag}...");
                        cache.Serialize(stream, tag, definition);
                    });
                }

                foreach (var tag in cache.TagCache.FindAllInGroup("sbsp"))
                {
                    var sbsp = cache.Deserialize<TagTool.Tags.Definitions.ScenarioStructureBsp>(stream, tag);
                    var sbspTagResources = cache.ResourceCache.GetStructureBspTagResources(sbsp.CollisionBspResource);
                    var sbspCacheFileTagResources = cache.ResourceCache.GetStructureBspCacheFileTagResources(sbsp.PathfindingResource);
                    tasks.Add(() =>
                    {
                        Console.WriteLine($"Upgrading {tag}...");
                        if (sbspTagResources != null)
                            cache.ResourceCaches.ReplaceResource(sbsp.CollisionBspResource.HaloOnlinePageableResource, sbspTagResources);
                        if (sbspCacheFileTagResources != null)
                            cache.ResourceCaches.ReplaceResource(sbsp.PathfindingResource.HaloOnlinePageableResource, sbspCacheFileTagResources);

                        cache.Serialize(stream, tag, sbsp);
                    });
                }

                SetCacheVersion(cache, CacheVersion.HaloOnlineED);

                foreach (var task in tasks)
                    task();

                Console.WriteLine("Done.");
            }
        }

        private void UpgradeM23Cache(GameCacheHaloOnline cache)
        {
            Console.WriteLine("Upgrading ms23 cache...");

            var targetVersion = CacheVersion.HaloOnlineED;

            // tags.dat
            using (var stream = cache.TagsFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var writer = new EndianWriter(stream);
                var reader = new EndianReader(stream);
                var ctx = new DataSerializationContext(reader, writer);
            
                var header = cache.Deserializer.Deserialize<TagCacheHaloOnlineHeader>(ctx);
                header.CreationTime = CacheVersionDetection.GetTimestamp(targetVersion);
            
                stream.Position = 0;
                cache.Serializer.Serialize(ctx, header);
            }
            
            // resource caches
            foreach(var cacheName in ResourceCachesHaloOnline.ResourceCacheNames.Values)
            {
                var resourceCaches = (ResourceCachesHaloOnline)cache.ResourceCaches;
                var resourceCacheFile = new FileInfo(Path.Combine(resourceCaches.Directory.FullName, cacheName));
                if (!resourceCacheFile.Exists)
                    continue;
            
                using (var stream = resourceCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
                {
                    var writer = new EndianWriter(stream);
                    var reader = new EndianReader(stream);
                    var ctx = new DataSerializationContext(reader, writer);
            
                    var header = cache.Deserializer.Deserialize<ResourceCacheHaloOnlineHeader>(ctx);
                    header.CreationTime = CacheVersionDetection.GetTimestamp(targetVersion);
            
                    stream.Position = 0;
                    cache.Serializer.Serialize(ctx, header);
                }
            }
            
            // .map files
            foreach(var file in cache.Directory.GetFiles("*.map", SearchOption.TopDirectoryOnly))
            {
                if (file.Extension != ".map")
                    continue;
            
                MapFile mapFile;
                using (var stream = file.Open(FileMode.Open, FileAccess.ReadWrite))
                {
                    var writer = new EndianWriter(stream);
                    var reader = new EndianReader(stream);
                    var ctx = new DataSerializationContext(reader, writer);
            
                    mapFile = new MapFile();
                    mapFile.Read(reader);
                }

                var header = mapFile.Header as CacheFileHeaderGenHaloOnline;
                mapFile.Version = targetVersion;
                header.Timestamp = (ulong)CacheVersionDetection.GetTimestamp(mapFile.Version);
                header.Build = CacheVersionDetection.GetBuildName(mapFile.Version, mapFile.CachePlatform);
                for (int i = 0; i < header.ExternalDependencyTimestamps.Length; i++)
                    header.ExternalDependencyTimestamps[i] = header.Timestamp;

                using (var stream = file.Open(FileMode.Create, FileAccess.ReadWrite))
                {
                    var writer = new EndianWriter(stream);
                    mapFile.Write(writer);
                }
            }
        }

        public override object Execute(List<string> args)
        {    
            UpgradeM23Cache((GameCacheHaloOnline)Cache);
            UpgradeCacheForReach((GameCacheHaloOnline)Cache);

            return true;

            /*if (args.Count > 0)
                return false;

            var destDirectory = "Sounds";

            using(var stream = File.OpenRead("test.wma"))
            using(var reader = new EndianReader(stream, EndianFormat.LittleEndian))
            {
                ASFHeader header = new ASFHeader();
                header.Read(reader);
            }



            using (var stream = Cache.OpenCacheRead())
            {
                var ughShared = Cache.Version == CacheVersion.Halo2Vista ? (Cache as GameCacheGen2).VistaSharedTagCache.TagCache.FindFirstInGroup("ugh!") : null;
                var ugh = Cache.TagCache.FindFirstInGroup("ugh!");

                SoundCacheFileGestalt sharedSoundCacheGestalt = ughShared != null ? Cache.Deserialize<SoundCacheFileGestalt>(stream, ughShared) : null;
                SoundCacheFileGestalt fileSoundCacheGestalt = Cache.Deserialize<SoundCacheFileGestalt>(stream, ugh);

                foreach (var tag in Cache.TagCache.NonNull())
                {
                    if (tag.IsInGroup("snd!") && tag.Name.Contains("music"))
                    {
                        var soundCacheGestalt = (tag as CachedTagGen2).IsShared ? sharedSoundCacheGestalt : fileSoundCacheGestalt;


                        var prefixFilename = tag.Name.Replace('\\', '_').Replace(' ', '_').ToLower();

                        var sound = Cache.Deserialize<Sound>(stream, tag);
                        // Console.WriteLine($"Processing {tag.Name}.snd!");

                        var channels = Encoding.GetChannelCount(sound.Encoding);
                        var sampleRate = sound.SampleRate.GetSampleRateHz();

                        for (int i = 0; i < sound.PitchRangeCount; i++)
                        {
                            var pitchRangeIndex = i + sound.PitchRangeIndex;
                            var pitchRange = soundCacheGestalt.PitchRanges[pitchRangeIndex];

                            for (int j = 0; j < pitchRange.PermutationCount; j++)
                            {
                                var permutationIndex = j + pitchRange.FirstPermutation;
                                var permutation = soundCacheGestalt.Permutations[permutationIndex];

                                var permutationSize = 0;
                                
                                // compute total size
                                for(int k = 0; k < permutation.ChunkCount; k++)
                                {
                                    var permutationChunkIndex = permutation.FirstChunk + k;
                                    var chunk = soundCacheGestalt.Chunks[permutationChunkIndex];
                                    permutationSize += chunk.GetSize();
                                }

                                byte[] data = new byte[permutationSize];
                                var currentOffset = 0;

                                // move Data
                                for (int k = 0; k < permutation.ChunkCount; k++)
                                {
                                    var permutationChunkIndex = permutation.FirstChunk + k;
                                    var chunk = soundCacheGestalt.Chunks[permutationChunkIndex];
                                    var chunkSize = chunk.GetSize();
                                    byte[] chunkData = (Cache.ResourceCache as ResourceCacheGen2).GetResourceDataFromHandle(chunk.ResourceReference, chunkSize);
                                    Array.Copy(chunkData, 0, data, currentOffset, chunkSize);
                                    currentOffset += chunkSize;
                                }

                                // convert and save sound

                                BlamSound blamSound = SoundConverter.ConvertGen2Sound(Cache, soundCacheGestalt, sound, i, j, data, Compression.Tagtool_WAV, false, "", tag.Name);
                                var wavFileName = prefixFilename + $"_{i}_{j}.wav";
                                var outputPath = $"Sounds\\{wavFileName}";


                                if (File.Exists(outputPath))
                                    File.Delete(outputPath);

                                Console.WriteLine($"{wavFileName}: pitch range {i}, permutation {j} sample count: {blamSound.SampleCount}");
                                using (EndianWriter output = new EndianWriter(new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None), EndianFormat.BigEndian))
                                {
                                    output.WriteBlock(blamSound.Data);
                                }
                            }
                        }
                    }
                }
            }

            return true;*/
        }

        /*
        public object ExecuteSoundHalo1(List<string> args)
        {
            if (args.Count > 0)
                return false;

            var destDirectory = "Sounds";

            using (var stream = Cache.OpenCacheRead())
            {
                foreach(var tag in Cache.TagCache.NonNull())
                {
                    if (tag.IsInGroup("snd!"))
                    {
                        var prefixFilename = tag.Name.Replace('\\', '_').Replace(' ', '_').ToLower();

                        var sound = Cache.Deserialize<Sound>(stream, tag);
                        Console.WriteLine($"Processing {tag.Name}.snd!");

                        for(int i = 0; i < sound.PitchRanges.Count; i++)
                        {
                            var pitchRange = sound.PitchRanges[i];
                            for(int j = 0; j < pitchRange.Permutations.Count; j++)
                            {
                                var permutation = pitchRange.Permutations[j];

                                byte[] data = (Cache as GameCacheGen1).SoundResources.GetResourceData((int)permutation.Samples.Size, (int)permutation.Samples.Gen1ExternalOffset);
                                

                                var channels = sound.Encoding == Sound.EncodingValue.Mono ? 1 : 2;
                                var sampleRate = sound.SampleRate == Sound.SampleRateValue._22khz ? 22050 : 44100;

                                var fileName = prefixFilename + $"_{i}_{j}.adpcm";
                                var wavFileName = prefixFilename + $"_{i}_{j}.wav";
                                var oggFileName = prefixFilename + $"_{i}_{j}.ogg";

                                if (sound.Compression == Sound.CompressionValue.XboxAdpcm || sound.Compression == Sound.CompressionValue.ImaAdpcm)
                                {
                                    var soundFile = new XboxADPCM(data, channels, sampleRate);

                                    using (var fileStream = File.Create(Path.Combine(destDirectory, fileName)))
                                    using (var writer = new EndianWriter(fileStream, EndianFormat.LittleEndian))
                                    {
                                        soundFile.Write(writer);
                                    }

                                    SoundConverter.ConvertADPCMToWAV($"Sounds\\{fileName}", $"Sounds\\{wavFileName}");

                                    if (File.Exists($"Sounds\\{fileName}"))
                                        File.Delete($"Sounds\\{fileName}");
                                }
                                else if(sound.Compression == Sound.CompressionValue.None)
                                {
                                    Console.WriteLine("Found PCM sound data");
                                    var soundFile = new WAVFile(data, channels, sampleRate);
                                    using (var fileStream = File.Create(Path.Combine(destDirectory, wavFileName)))
                                    using (var writer = new EndianWriter(fileStream, EndianFormat.LittleEndian))
                                    {
                                        soundFile.Write(writer);
                                    }
                                }
                                else if(sound.Compression == Sound.CompressionValue.Ogg)
                                {
                                    using (var fileStream = File.Create(Path.Combine(destDirectory, oggFileName)))
                                    using (var writer = new EndianWriter(fileStream, EndianFormat.LittleEndian))
                                    {
                                        writer.Write(data);
                                    }

                                    SoundConverter.ConvertADPCMToWAV($"Sounds\\{oggFileName}", $"Sounds\\{wavFileName}");

                                    if (File.Exists($"Sounds\\{oggFileName}"))
                                        File.Delete($"Sounds\\{oggFileName}");
                                }
                                

                            }
                        }
                    }
                }
            }

            return true;
        }

        public object ExecuteBitmapHalo1(List<string> args)
        {
            if (args.Count > 0)
                return false;

            var destDirectory = "Bitmaps";

            using (var stream = Cache.OpenCacheRead())
            {
                foreach (var tag in Cache.TagCache.NonNull())
                {
                    if (tag.IsInGroup("bitm"))
                    {
                        var fileName = tag.Name.Replace('\\', '_').Replace(' ', '_').ToLower() + ".dds";

                        var bitmap = Cache.Deserialize<Bitmap>(stream, tag);
                        Console.WriteLine($"Processing {tag.Name}.bitm");

                        foreach (var image in bitmap.Bitmaps)
                        {
                            if (image.ResourceSize <= 0 || image.ResourceOffset < 0)
                            {
                                Console.WriteLine("Invalid resource for bitmap.");
                                continue;
                            }

                            byte[] data = (Cache as GameCacheGen1).BitmapResources.GetResourceData((int)image.ResourceSize, image.ResourceOffset);

                            BaseBitmap resultBitmap = new BaseBitmap();

                            resultBitmap.Width = image.Width;
                            resultBitmap.Height = image.Height;
                            resultBitmap.MipMapCount = image.MipmapCount;
                            resultBitmap.Data = data;

                            switch (image.Type)
                            {
                                case Bitmap.BitmapDataBlock.TypeValue._2dTexture:
                                    resultBitmap.Type = BitmapType.Texture2D;
                                    break;
                                case Bitmap.BitmapDataBlock.TypeValue._3dTexture:
                                    resultBitmap.Type = BitmapType.Texture3D;
                                    break;
                                case Bitmap.BitmapDataBlock.TypeValue.CubeMap:
                                    resultBitmap.Type = BitmapType.CubeMap;
                                    break;
                                case Bitmap.BitmapDataBlock.TypeValue.White:
                                    Console.WriteLine("Unknown bitmap type White");
                                    continue;
                            }

                            switch (image.Format)
                            {
                                case Bitmap.BitmapDataBlock.FormatValue.Dxt1:
                                    resultBitmap.Format = BitmapFormat.Dxt1;
                                    break;

                                case Bitmap.BitmapDataBlock.FormatValue.Dxt3:
                                    resultBitmap.Format = BitmapFormat.Dxt3;
                                    break;

                                case Bitmap.BitmapDataBlock.FormatValue.Dxt5:
                                    resultBitmap.Format = BitmapFormat.Dxt5;
                                    break;

                                case Bitmap.BitmapDataBlock.FormatValue.A8r8g8b8:
                                    resultBitmap.Format = BitmapFormat.A8R8G8B8;
                                    break;

                                case Bitmap.BitmapDataBlock.FormatValue.A8:
                                    resultBitmap.Format = BitmapFormat.A8;
                                    break;


                                case Bitmap.BitmapDataBlock.FormatValue.Y8:
                                    resultBitmap.Format = BitmapFormat.Y8;
                                    break;


                                case Bitmap.BitmapDataBlock.FormatValue.A8y8:
                                    resultBitmap.Format = BitmapFormat.A8Y8;
                                    break;

                                default:
                                    Console.WriteLine($"Format {image.Format} requires conversion");
                                    continue;
                            }

                            DDSFile bitmapFile = new DDSFile(resultBitmap);

                            using (var fileStream = File.Create(Path.Combine(destDirectory, fileName)))
                            using (var writer = new EndianWriter(fileStream, EndianFormat.LittleEndian))
                            {
                                bitmapFile.Write(writer);
                            }
                            Console.WriteLine("Successfully extracted!");
                        }
                    }
                }
            }

            return true;
        }
    */
    }
}

