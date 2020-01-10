using TagTool.Cache;
using TagTool.IO;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Audio.Converter;
using TagTool.Audio;
using TagTool.Commands.Porting;

namespace TagTool.Commands.Sounds
{
    public class ExtractXMACommand : Command
    {
        private GameCacheContextGen3 Cache;
        private SoundCacheFileGestalt BlamSoundGestalt { get; set; } = null;
        private CachedTag Tag;
        private Sound Sound;
        public ExtractXMACommand(GameCacheContextGen3 cache, CachedTag tag, Sound sound) :
            base(true,

                "ExtractXMA",
                "Extracts XMA Files for selected Sound.",

                "ExtractXMA [output directory]",

                "Extracts XMA Files for selected Sound.")
        {
            Cache = cache;
            Tag = tag;
            Sound = sound;
        }

        public override object Execute(List<string> args)
        {

            string directory;

            if (args.Count == 1)
            {
                directory = args[0];
            }
            else if (args.Count == 0)
            {
                directory = "Sounds";
            }
            else
                return false;


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

            ExtractXMA(directory);

            Console.WriteLine("done.");

            return true;
        }

        public void ExtractXMA(string directory)
        {
            if (BlamSoundGestalt == null)
            {
                using(var stream = Cache.TagCache.OpenTagCacheRead())
                    BlamSoundGestalt = PortingContextFactory.LoadSoundGestalt(Cache, stream);
            }
                

            var resourceDefinition = Cache.ResourceCache.GetSoundResourceDefinition(Sound.Resource);
            var xmaFileSize = BlamSoundGestalt.GetFileSize(Sound.SoundReference.PitchRangeIndex, Sound.SoundReference.PitchRangeCount);
            if (xmaFileSize < 0)
                return;

            var xmaData = resourceDefinition.Data.Data;

            if (xmaData == null)
            {
                Console.WriteLine($"ERROR: Failed to find sound data!");
                return;
            }

            var parts = Tag.Name.Split('\\');
            string baseName = parts[parts.Length - 1];

            for (int pitchRangeIndex = Sound.SoundReference.PitchRangeIndex; pitchRangeIndex < Sound.SoundReference.PitchRangeIndex + Sound.SoundReference.PitchRangeCount; pitchRangeIndex++)
            {
                var relativePitchRangeIndex = pitchRangeIndex - Sound.SoundReference.PitchRangeIndex;
                var permutationCount = BlamSoundGestalt.GetPermutationCount(pitchRangeIndex);

                for (int i = 0; i < permutationCount; i++)
                {
                    BlamSound blamSound = SoundConverter.GetXMA(Cache, BlamSoundGestalt, Sound, relativePitchRangeIndex, i, xmaData);
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

