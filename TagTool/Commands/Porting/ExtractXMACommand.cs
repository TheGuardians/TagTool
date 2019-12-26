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

                ExtractXMA(blamTag, directory);
            }
            else
            {
                foreach (var tag in BlamCache.IndexItems)
                {
                    if ((tag.GroupTag == "bitm"))
                    {
                        ExtractXMA(tag, directory);
                    }
                }
            }

            Console.WriteLine("done.");

            return true;
        }

        public void ExtractXMA(CacheFile.IndexItem blamTag, string directory)
        {
            if (BlamSoundGestalt == null)
                BlamSoundGestalt = PortingContextFactory.LoadSoundGestalt(CacheContext, ref BlamCache);

            var blamContext = new CacheSerializationContext(ref BlamCache, blamTag);

            var sound = BlamCache.Deserializer.Deserialize<Sound>(blamContext);


            var xmaFileSize = BlamSoundGestalt.GetFileSize(sound.SoundReference.PitchRangeIndex, sound.SoundReference.PitchRangeCount);
            if (xmaFileSize < 0)
                return;

            var xmaData = BlamCache.GetSoundRaw(sound.Resource.Gen3ResourceID, xmaFileSize);

            if (xmaData == null)
            {
                Console.WriteLine($"ERROR: Failed to find sound data!");
                return;
            }

            var parts = blamTag.Name.Split('\\');
            string baseName = parts[parts.Length - 1];

            for (int pitchRangeIndex = sound.SoundReference.PitchRangeIndex; pitchRangeIndex < sound.SoundReference.PitchRangeIndex + sound.SoundReference.PitchRangeCount; pitchRangeIndex++)
            {
                var relativePitchRangeIndex = pitchRangeIndex - sound.SoundReference.PitchRangeIndex;
                var permutationCount = BlamSoundGestalt.GetPermutationCount(pitchRangeIndex);

                for (int i = 0; i < permutationCount; i++)
                {
                    BlamSound blamSound = SoundConverter.GetXMA(BlamCache, BlamSoundGestalt, sound, relativePitchRangeIndex, i, xmaData);
                    string permutationName = $"{baseName}_{relativePitchRangeIndex}_{i}";
                    var fileName = $"{directory}\\{permutationName}.xma";

                    using (EndianWriter output = new EndianWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None), EndianFormat.BigEndian))
                    {
                        XMAFile XMAfile = new XMAFile(blamSound);
                        XMAfile.Write(output);
                    }
                }
            }
        }

    }
}

