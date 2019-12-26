using TagTool.Cache;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Audio;
using TagTool.IO;
using TagTool.Audio.Converter;
using TagTool.Serialization;

namespace TagTool.Commands.Sounds
{
    class ExportSoundCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private Sound Definition { get; }

        public ExportSoundCommand(HaloOnlineCacheContext cacheContext, CachedTagInstance tag, Sound definition) :
            base(true,
                
                "ExportSound",
                "Export snd! data to a file",
                
                "ExportSound <Path>",
                "")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var outDirectory = args[0];

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


            var resource = Definition.Resource;
            var resourceContext = new ResourceSerializationContext(CacheContext, resource.HaloOnlinePageableResource);
            var resourceDefinition = CacheContext.Deserializer.Deserialize<SoundResourceDefinition>(resourceContext);

            if (resourceDefinition.Data == null)
            {
                Console.WriteLine("Invalid sound definition");
                return false;
            }

            var dataReference = resourceDefinition.Data;


            byte[] soundData = new byte[dataReference.Size];
            var resourceDataStream = new MemoryStream(soundData);
            CacheContext.ExtractResource(resource.HaloOnlinePageableResource, resourceDataStream);

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
            Console.WriteLine("Done!");
            return true;
        }
    }
}