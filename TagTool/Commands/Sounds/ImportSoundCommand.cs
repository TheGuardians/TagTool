using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Audio;
using System.Linq;

namespace TagTool.Commands.Sounds
{
    class ImportSoundCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private Sound Definition { get; }

        public ImportSoundCommand(GameCache cache, CachedTag tag, Sound definition) :
            base(true,
                
                "ImportSound",
                "Import one (or many) sound files into the current snd! tag. Overwrites existing sound data. See documentation for formatting and options.",
                
                "ImportSound",
                "")
        {
            Cache = cache;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            var soundDataAggregate = new byte[0].AsEnumerable();

            int currentFileOffset = 0;
            int totalSampleCount = 0;
            int maxPermutationSampleCount = 0;


            int pitchRangeCount = GetPitchRangeCountUser();

            if (pitchRangeCount <= 0)
                return false;

            //
            // Get basic information on the sounds
            //

            if (pitchRangeCount > 1)
                Definition.ImportType = ImportType.MultiLayer;
            else
                Definition.ImportType = ImportType.SingleLayer;

            Definition.SampleRate.value = GetSoundSampleRateUser();

            Definition.PlatformCodec.Compression = GetSoundCompressionUser();
            
            Definition.PlatformCodec.Encoding = GetSoundEncodingUser();

            Definition.PitchRanges = new List<PitchRange>();

            //
            // For each pitch range, get all the permutations and append sound data.
            //

            for(int u = 0; u < pitchRangeCount; u++)
            {
                int permutationCount = GetPermutationCountUser();

                if (permutationCount <= 0)
                    return false;

                var pitchRange = new PitchRange
                {
                    ImportName = new StringId(5221),   //|default|
                    Unknown5 = -1,
                    Unknown6 = -1,
                    Unknown7 = -1,
                    Unknown8 = -1,
                    PermutationCount = (short)permutationCount,
                    PitchRangeParameters = new PitchRangeParameter()
                };
                pitchRange.PitchRangeParameters.UnknownBounds = new Bounds<short>(-32768, 32767);

                pitchRange.Permutations = new List<Permutation>();
                
                //
                // Permutation section
                //

                for (int i = 0; i < permutationCount; i++)
                {
                    string soundFile = GetPermutationFileUser(i);
                    if (soundFile == null)
                        return false;

                    int sampleCount = GetPermutationSampleCountUser(i);

                    var perm = new Permutation
                    {
                        ImportName = StringId.Invalid,
                        SampleSize = (uint)sampleCount
                    };

                    if (i != 0)
                        perm.IsNotFirstPermutation = 1;

                    perm.PermutationNumber = (uint)i;

                    var permutationData = File.ReadAllBytes(soundFile);

                    perm.PermutationChunks = new List<PermutationChunk>();

                    var chunk = new PermutationChunk(currentFileOffset, permutationData.Length);
                    perm.PermutationChunks.Add(chunk);
                    currentFileOffset += permutationData.Length;
                    totalSampleCount += sampleCount;

                    if (maxPermutationSampleCount < sampleCount)
                        maxPermutationSampleCount = sampleCount;

                    soundDataAggregate = soundDataAggregate.Concat(permutationData);

                    pitchRange.Permutations.Add(perm);
                }

                Definition.PitchRanges.Add(pitchRange);
            }

            Definition.Promotion.LongestPermutationDuration =(uint)( 1000 * (double)maxPermutationSampleCount / (Definition.SampleRate.GetSampleRateHz()));
            Definition.Promotion.TotalSampleSize = (uint)totalSampleCount;


            // remove extra info for now

            Definition.ExtraInfo = new List<ExtraInfo>();


            Definition.Unknown12 = 0;

            //
            // Create new resource
            //

            Console.Write("Creating new sound resource...");

            var data = soundDataAggregate.ToArray();
            var resourceDefinition = AudioUtils.CreateSoundResourceDefinition(data);
            var resourceReference = Cache.ResourceCache.CreateSoundResource(resourceDefinition);
            Definition.Resource = resourceReference;

            Console.WriteLine("done.");
            
            return true;
        }

        private static int GetPermutationCountUser()
        {
            return GetIntFromUser("Enter the desired number of permutations (default is 1): ");
        }

        private static string GetPermutationFileUser(int index)
        {
            return GetFileFromUser($"Enter the file path to permutation {index}: ");
        }

        private static int GetPermutationSampleCountUser(int index)
        {
            int sampleCount = GetIntFromUser($"Enter the number of samples in permutation {index}: ");
            if (sampleCount > 0)
                return sampleCount;
            else
                return -1;
        }

        private static EncodingValue GetSoundEncodingUser()
        {
            int channelCount = GetIntFromUser($"Enter the number of channels in the sound (1, 2, 4, 6): ");
            switch (channelCount)
            {
                case 1:
                    return EncodingValue.Mono;
                case 2:
                    return EncodingValue.Stereo;
                case 4:
                    return EncodingValue.Surround;
                case 6:
                    return EncodingValue._51Surround;
                default:
                    Console.WriteLine("Invalid channel Count, using stereo.");
                    return EncodingValue.Stereo;
            }
        }

        private static SampleRate.SampleRateValue GetSoundSampleRateUser()
        {
            int sampleRate = GetIntFromUser($"Enter the sample rate of the sound: (0: 22050 Hz, 1: 44100 Hz, 2: 32000 Hz): ");
            switch (sampleRate)
            {
                case 0:
                    return SampleRate.SampleRateValue._22khz;
                case 1:
                    return SampleRate.SampleRateValue._44khz;
                case 2:
                    return SampleRate.SampleRateValue._32khz;
                default:
                    Console.WriteLine("Invalid sample rate, using 44100 Hz");
                    return SampleRate.SampleRateValue._44khz;
            }
        }

        private static Compression GetSoundCompressionUser()
        {
            int compressionIndex = GetIntFromUser($"Enter the compression format of the sound: (0: PCM, 1: MP3, 2: FSB): ");
            switch (compressionIndex)
            {
                case 0:
                    return Compression.PCM;
                case 1:
                    return Compression.MP3;
                case 2:
                    return Compression.FSB4;
                default:
                    Console.WriteLine("Invalid compression.");
                    return 0;
            }
        }

        private static int GetPitchRangeCountUser()
        {
            return GetIntFromUser("Enter the desired number of pitch ranges (default is 1): ");
        }

        private static string GetFileFromUser(string message)
        {
            string userInput;
            try
            {
                Console.Write(message);
                userInput = Console.ReadLine();
                if (!File.Exists(userInput))
                {
                    Console.WriteLine($"Invalid file {userInput}");
                    return null;
                }
                return userInput;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Invalid input: {e.Message}");
            }
            return null;
        }

        private static int GetIntFromUser(string message)
        {
            string userInput;
            int result;
            try
            {
                Console.Write(message);
                userInput = Console.ReadLine();
                result = Convert.ToInt32(userInput);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Invalid input: {e.Message}");
            }
            return -1;
        }
    }
}