using TagTool.Cache;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Audio;
using TagTool.IO;
using TagTool.Audio.Converter;
using TagTool.Commands.Porting;

namespace TagTool.Commands.Sounds
{
    class ExportSoundCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private Sound Definition { get; }
        private SoundCacheFileGestalt BlamSoundGestalt { get; set; } = null;

        public ExportSoundCommand(GameCache cache, CachedTag tag, Sound definition) :
            base(true,
                
                "ExportSound",
                "Export snd! data to a file",
                
                "ExportSound <Path>",
                "")
        {
            Cache = cache;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            string outDirectory = "";

            if (args.Count == 1)
                outDirectory = args[0];
            else if (args.Count == 0)
                outDirectory = "Sounds";
            else
                return false;

            if (!Directory.Exists(outDirectory))
            {
                Console.Write("Destination directory does not exist. Create it? [y/n] ");
                var answer = Console.ReadLine().ToLower();

                if (answer.Length == 0 || !(answer.StartsWith("y") || answer.StartsWith("n")))
                    return false;

                if (answer.StartsWith("y"))
                    Directory.CreateDirectory(outDirectory);
                else
                    return false;
            }


            var resourceReference = Definition.Resource;
            var resourceDefinition = Cache.ResourceCache.GetSoundResourceDefinition(resourceReference);
            
            if (resourceDefinition.Data == null)
            {
                Console.WriteLine("Invalid sound definition");
                return false;
            }

            var dataReference = resourceDefinition.Data;
            byte[] soundData = dataReference.Data;
            
            if(Cache is GameCacheHaloOnlineBase)
            {
                for (int i = 0; i < Definition.PitchRanges.Count; i++)
                {
                    var pitchRange = Definition.PitchRanges[i];
                    for (int j = 0; j < pitchRange.Permutations.Count; j++)
                    {
                        var permutation = pitchRange.Permutations[j];
                        var filename = Tag.Index.ToString("X8") + "_" + i.ToString() + "_" + j.ToString();

                        byte[] permutationData = new byte[permutation.PermutationChunks[0].EncodedSize & 0x3FFFFFF];
                        Array.Copy(soundData, permutation.PermutationChunks[0].Offset, permutationData, 0, permutationData.Length);

                        switch (Definition.PlatformCodec.Compression)
                        {
                            case Compression.PCM:
                                filename += ".wav";
                                break;
                            case Compression.MP3:
                                filename += ".mp3";
                                break;
                            case Compression.FSB4:
                                filename += ".fsb";
                                break;
                        }

                        var outPath = Path.Combine(outDirectory, filename);

                        using (EndianWriter writer = new EndianWriter(new FileStream(outPath, FileMode.Create, FileAccess.Write, FileShare.None), EndianFormat.BigEndian))
                        {
                            var channelCount = Encoding.GetChannelCount(Definition.PlatformCodec.Encoding);
                            var sampleRate = Definition.SampleRate.GetSampleRateHz();

                            switch (Definition.PlatformCodec.Compression)
                            {
                                case Compression.PCM:
                                    WAVFile WAVfile = new WAVFile(permutationData, channelCount, sampleRate);
                                    WAVfile.Write(writer);
                                    break;
                                case Compression.MP3:
                                case Compression.FSB4:
                                    writer.Write(permutationData);
                                    break;
                            }
                        }
                    }
                }
            }
            else if(Cache.GetType() == typeof(GameCacheGen3))
            {
                if (BlamSoundGestalt == null)
                {
                    using(var stream = Cache.OpenCacheRead())
                        BlamSoundGestalt = PortingContextFactory.LoadSoundGestalt(Cache, stream);
                }
                    

                for (int pitchRangeIndex = Definition.SoundReference.PitchRangeIndex; pitchRangeIndex < Definition.SoundReference.PitchRangeIndex + Definition.SoundReference.PitchRangeCount; pitchRangeIndex++)
                {
                    var relativePitchRangeIndex = pitchRangeIndex - Definition.SoundReference.PitchRangeIndex;
                    var permutationCount = BlamSoundGestalt.GetPermutationCount(pitchRangeIndex);

                    for (int i = 0; i < permutationCount; i++)
                    {
                        string permutationName = $"{Tag.Name.Replace('\\', '_')}_{relativePitchRangeIndex}_{i}.mp3";
                        var outPath = Path.Combine(outDirectory, permutationName);
                        BlamSound blamSound = SoundConverter.ConvertGen3Sound(Cache, BlamSoundGestalt, Definition, relativePitchRangeIndex, i, soundData);
                        using (EndianWriter output = new EndianWriter(new FileStream(outPath, FileMode.Create, FileAccess.Write, FileShare.None), EndianFormat.BigEndian))
                        {
                            output.WriteBlock(blamSound.Data);
                        }
                    }
                }
            }
            
            
            Console.WriteLine("Done!");
            return true;
        }
    }
}