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
using TagTool.Audio;

namespace TagTool.Commands.Porting
{
    public class ExtractSoundCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CacheFile BlamCache;
        private SoundCacheFileGestalt BlamSoundGestalt { get; set; } = null;

        public ExtractSoundCommand(HaloOnlineCacheContext cacheContext, CacheFile blamCache) :
            base(true,

                "ExtractSound",
                "Extracts Sound Files for selected Sound.",

                "ExtractSound <tag name> <output directory>",

                "Extracts Sound Files for selected Sound.")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
            
        }

        public override object Execute(List<string> args)
        {
            var directory = "";
            var blamTagName = "";
            bool convertAll = false;

            if (args.Count == 2)
            {
                blamTagName = args[0];
                directory = args[1];
            }
            else if (args.Count == 1)
            {
                blamTagName = args[0];
                directory = "Sounds";
            }
            else
                return false;

            if (args[0].ToLower() == "all")
                convertAll = true;

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

            if (!convertAll)
            {
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

                ExtractWAV(blamTag, directory);
            }
            else
            {
                foreach (var tag in BlamCache.IndexItems)
                {
                    if ((tag.GroupTag == "snd!"))
                    {
                        ExtractWAV(tag, directory);
                    }
                }
            }

            Console.WriteLine("done.");

            return true;
        }

        public void ExtractWAV(CacheFile.IndexItem blamTag, string directory)
        {
            Console.WriteLine($"Extracting {blamTag.Name}.sound");
            if (BlamSoundGestalt == null)
                BlamSoundGestalt = PortingContextFactory.LoadSoundGestalt(CacheContext, ref BlamCache);

            var blamContext = new CacheSerializationContext(ref BlamCache, blamTag);

            var sound = BlamCache.Deserializer.Deserialize<Sound>(blamContext);

            var xmaFileSize = BlamSoundGestalt.GetFileSize(sound.SoundReference.PitchRangeIndex, sound.SoundReference.PitchRangeCount);

            var xmaData = BlamCache.GetSoundRaw(sound.SoundReference.ZoneAssetHandle, xmaFileSize);

            if (xmaData == null)
            {
                Console.WriteLine($"ERROR: Failed to find sound data!");
                return;
            }

            var OutDir = directory;
            string sound_name = blamTag.Name.Replace('\\', '_');
            sound_name = Path.Combine(directory, sound_name); 

            for (int pitchRangeIndex = sound.SoundReference.PitchRangeIndex; pitchRangeIndex < sound.SoundReference.PitchRangeIndex + sound.SoundReference.PitchRangeCount; pitchRangeIndex++)
            {
                var relativePitchRangeIndex = pitchRangeIndex - sound.SoundReference.PitchRangeIndex;
                var permutationCount = BlamSoundGestalt.GetPermutationCount(pitchRangeIndex);

                for (int i = 0; i < permutationCount; i++)
                {
                    string permutationName = $"{sound_name}_{relativePitchRangeIndex}_{i}";
                    permutationName += ".mp3";

                    BlamSound blamSound = SoundConverter.ConvertGen3Sound(BlamCache, BlamSoundGestalt, sound, relativePitchRangeIndex, i, xmaData);
                    using (EndianWriter output = new EndianWriter(new FileStream(permutationName, FileMode.Create, FileAccess.Write, FileShare.None), EndianFormat.BigEndian))
                    {
                        output.WriteBlock(blamSound.Data);
                    }
                }
            }
        }

    }
}

