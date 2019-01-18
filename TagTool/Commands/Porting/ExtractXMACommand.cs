using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;
using TagTool.Serialization;
using TagTool.Audio.Converter;

namespace TagTool.Commands.Porting
{
    public class ExtractXMACommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CacheFile BlamCache;
        private SoundCacheFileGestalt BlamSoundGestalt { get; set; } = null;

        public ExtractXMACommand(HaloOnlineCacheContext cacheContext, CacheFile blamCache) :
            base(true,

                "ExtractXMA",
                "Extracts XMA Files for selected Sound.",

                "ExtractXMA <tag name> <output directory>",

                "Extracts XMA Files for selected Sound.")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
            
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            //
            // Verify Blam tag instance
            //


            var blamTagName = args[0];

            var directory = args[1];

            if (!Directory.Exists(directory))
            {
                Console.Write("Destination directory does not exist. Create it? [y/n] ");
                var answer = Console.ReadLine().ToLower();

                if (answer.Length == 0 || !(answer.StartsWith("y") || answer.StartsWith("n")))
                    return false;

                if (answer.StartsWith("y"))
                    Directory.CreateDirectory(directory);
                else
                    return false;
            }

            Console.Write($"Verifying {blamTagName}.sound...");

            CacheFile.IndexItem blamTag = null;

            foreach (var tag in BlamCache.IndexItems)
            {
                if ((tag.GroupTag == "snd!") && (tag.Name == blamTagName))
                {
                    blamTag = tag;
                    break;
                }
            }

            if (blamTag == null)
            {
                Console.WriteLine($"ERROR: Blam tag does not exist: {blamTagName}.sound");
                return true;
            }

            Console.WriteLine("done.");

            //
            // Load the Blam tag definition
            //

            if (BlamSoundGestalt == null)
                BlamSoundGestalt = PortingContextFactory.LoadSoundGestalt(CacheContext, ref BlamCache);

            var blamContext = new CacheSerializationContext(ref BlamCache, blamTag);

            var sound = BlamCache.Deserializer.Deserialize<Sound>(blamContext);

            var platformCodec = BlamSoundGestalt.PlatformCodecs[sound.SoundReference.PlatformCodecIndex];

            var sampleRate = platformCodec.SampleRate.GetSampleRateHz();

            var xmaFileSize = BlamSoundGestalt.GetFileSize(sound.SoundReference.PitchRangeIndex, sound.SoundReference.PitchRangeCount);

            if (xmaFileSize < 0)
            {
                Console.WriteLine($"ERROR: Sound size is negative!");
                return false;
            }
                

            var xmaData = BlamCache.GetSoundRaw(sound.SoundReference.ZoneAssetHandle, xmaFileSize);

            if (xmaData == null)
            {
                Console.WriteLine($"ERROR: Failed to find sound data!");
                return false;
            }

            var parts = blamTag.Name.Split('\\');
            string baseName = parts[parts.Length-1];

            for (int pitchRangeIndex = sound.SoundReference.PitchRangeIndex; pitchRangeIndex < sound.SoundReference.PitchRangeIndex + sound.SoundReference.PitchRangeCount; pitchRangeIndex++)
            {
                var relativePitchRangeIndex = pitchRangeIndex - sound.SoundReference.PitchRangeIndex;
                var firstPermutationIndex = BlamSoundGestalt.GetFirstPermutationIndex(pitchRangeIndex);
                var pitchRangeSampleCount = BlamSoundGestalt.GetSamplesPerPitchRange(pitchRangeIndex);

                var permutationOrder = BlamSoundGestalt.GetPermutationOrder(pitchRangeIndex);

                //
                // Convert Blam pitch range to ElDorado format
                //

                var pitchRange = BlamSoundGestalt.PitchRanges[pitchRangeIndex];  
                var channelCount = platformCodec.Encoding.GetChannelCount();
                var permutationCount = BlamSoundGestalt.GetPermutationCount(pitchRangeIndex);

                for (int i = 0; i < permutationCount; i++)
                {
                    var permutationIndex = pitchRange.FirstPermutationIndex + i;
                    var permutationSize = BlamSoundGestalt.GetPermutationSize(permutationIndex);
                    var permutationOffset = BlamSoundGestalt.GetPermutationOffset(permutationIndex);

                    var permutation = BlamSoundGestalt.GetPermutation(permutationIndex);

                    var permutationData = new byte[permutationSize];
                    Array.Copy(xmaData, permutationOffset, permutationData, 0, permutationSize);

                    string permutationName = $"{baseName}_{relativePitchRangeIndex}_{i}";
                    var fileName = $"{directory}\\{permutationName}.xma";

                    using (EndianWriter output = new EndianWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None), EndianFormat.BigEndian))
                    {
                        XMAFile XMAfile = new XMAFile(permutationData, channelCount, sampleRate);
                        XMAfile.Write(output);
                    }
                }
            }



            return true;
        }

    }
}

