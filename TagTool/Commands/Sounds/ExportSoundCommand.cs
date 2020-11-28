using TagTool.Cache;
using TagTool.Commands.Common;
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

                "ExportSound [format] <Path>",
                "")
        {
            Cache = cache;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            string outDirectory = "";

            Compression? targetFormat = null;
            if (args.Count > 0)
            {
                if (Enum.TryParse(args[0], true, out Compression format))
                {
                    targetFormat = format;
                    args.RemoveAt(0);
                }
            }

            if (args.Count == 1)
                outDirectory = args[0];
            else if (args.Count == 0)
                outDirectory = "Sounds";
            else
                return new TagToolError(CommandError.ArgCount);

            if (!Directory.Exists(outDirectory))
            {
                Console.Write("Destination directory does not exist. Create it? [y/n] ");
                var answer = Console.ReadLine().ToLower();

                if (answer.Length == 0 || !(answer.StartsWith("y") || answer.StartsWith("n")))
                    return new TagToolError(CommandError.YesNoSyntax);

                if (answer.StartsWith("y"))
                    Directory.CreateDirectory(outDirectory);
                else
                    return true;
            }

            var resourceReference = Definition.Resource;
            var resourceDefinition = Cache.ResourceCache.GetSoundResourceDefinition(resourceReference);

            if (resourceDefinition == null || resourceDefinition.Data == null)
            {
                Console.WriteLine("The sound resource contains no data");
                return true;
            }

            var dataReference = resourceDefinition.Data;
            byte[] soundData = dataReference.Data;

            switch (Cache)
            {
                case GameCacheHaloOnlineBase _:
                    ExportHaloOnlineSound(outDirectory, soundData, targetFormat);
                    break;
                case GameCacheGen3 _:
                    ExportGen3Sound(outDirectory, soundData, targetFormat);
                    break;
                default:
                    throw new NotSupportedException("Cache not supported");
            }

            Console.WriteLine("Done!");
            return true;
        }

        private void ExportGen3Sound(string outDirectory, byte[] soundData, Compression? targetFormat)
        {
            if (BlamSoundGestalt == null)
            {
                using (var stream = Cache.OpenCacheRead())
                    BlamSoundGestalt = PortingContextFactory.LoadSoundGestalt(Cache, stream);
            }

            for (int pitchRangeIndex = Definition.SoundReference.PitchRangeIndex; pitchRangeIndex < Definition.SoundReference.PitchRangeIndex + Definition.SoundReference.PitchRangeCount; pitchRangeIndex++)
            {
                var relativePitchRangeIndex = pitchRangeIndex - Definition.SoundReference.PitchRangeIndex;
                var permutationCount = BlamSoundGestalt.GetPermutationCount(pitchRangeIndex);

                if (targetFormat == null)
                    targetFormat = BlamSoundGestalt.PlatformCodecs[Definition.SoundReference.PlatformCodecIndex].Compression;

                for (int i = 0; i < permutationCount; i++)
                {
                    var filename = GetExportFileName(targetFormat.Value, relativePitchRangeIndex, i);
                    var outPath = Path.Combine(outDirectory, filename);
                    BlamSound blamSound = SoundConverter.ConvertGen3Sound(Cache, BlamSoundGestalt, Definition, relativePitchRangeIndex, i, soundData, targetFormat.Value, false, "", Tag.Name);
                    Console.WriteLine($"{filename}: pitch range {pitchRangeIndex}, permutation {i} sample count: {blamSound.SampleCount}");
                    using (EndianWriter output = new EndianWriter(new FileStream(outPath, FileMode.Create, FileAccess.Write, FileShare.None), EndianFormat.BigEndian))
                    {
                        output.WriteBlock(blamSound.Data);
                    }
                }
            }
        }

        private void ExportHaloOnlineSound(string outDirectory, byte[] soundData, Compression? targetFormat)
        {
            if (targetFormat != null)
                throw new NotSupportedException("Converting formats from halo online cache not supported");

            targetFormat = Definition.PlatformCodec.Compression;

            for (int i = 0; i < Definition.PitchRanges.Count; i++)
            {
                var pitchRange = Definition.PitchRanges[i];
                for (int j = 0; j < pitchRange.Permutations.Count; j++)
                {
                    var permutation = pitchRange.Permutations[j];

                    byte[] permutationData = new byte[permutation.PermutationChunks[0].EncodedSize & 0x3FFFFFF];
                    Array.Copy(soundData, permutation.PermutationChunks[0].Offset, permutationData, 0, permutationData.Length);

                    var filename = GetExportFileName(targetFormat.Value, i, j);
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

                    Console.WriteLine($"{filename}: pitch range {i}, permutation {j} sample count: {permutation.SampleCount}");
                }
            }
        }

        private string GetExportFileName(Compression targetFormat, int pitchRangeIndex, int permutationIndex)
        {
            string extension = GetFormtFileExtension(targetFormat);
            return $"{Tag.ToString().Replace('\\', '_')}_{pitchRangeIndex}_{permutationIndex}.{extension}";
        }

        private string GetFormtFileExtension(Compression format)
        {
            switch (format)
            {
                case Compression.XMA:
                    return "xma";
                case Compression.PCM:
                    return "wav";
                case Compression.MP3:
                    return "mp3";
                case Compression.FSB4:
                    return "fsb";
                default:
                    throw new NotSupportedException();
            }
        }
    }
}