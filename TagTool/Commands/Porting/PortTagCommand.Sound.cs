using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TagTool.Audio;
using TagTool.Audio.Converter;
using TagTool.Serialization;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private SoundCacheFileGestalt BlamSoundGestalt { get; set; } = null;

        /// <summary>
        /// Truncate WAV file when converting XMA -> WAV.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sampleRate"></param>
        /// <param name="channelCount"></param>
        /// <param name="additionalOffset"></param>
        /// <returns></returns>
        private static byte[] TruncateWAVFile(byte[] data, int sampleRate, int channelCount, int additionalOffset = 0)
        {
            // The offsets are computed as follows : you want to trim about 0.013 seconds at the start and 0.004 seconds at the end. 1152 bytes is the number of bytes required to cut this amount
            // on a 16 bit PCM single channel audio file at 44100Hz. Obtaining the offset for other type of sounds is just changing those parameters.

            int startOffset = (int)(1152 * channelCount * ((float)sampleRate / 44100));                                        // Offset from index 0 
            int endOffset = (int)(384 * channelCount * ((float)sampleRate / 44100));                                           // Offset from index data.Length -1
            int size = data.Length - startOffset - endOffset;
            byte[] result = new byte[size];
            Array.Copy(data, startOffset + additionalOffset, result, 0, size);
            return result;
        }
        
        /// <summary>
        /// Converts XMA file to WAV files using ffmpeg. True if WAVFileName exists, else false.
        /// </summary>
        /// <param name="XMAFileName">Name of the XMA file</param>
        /// <param name="WAVFileName">Name of the WAV file</param>
        /// <returns>Success or failure of conversion</returns>
        private static bool ConvertXMAToWAV(string XMAFileName, string WAVFileName)
        {
            ProcessStartInfo info = new ProcessStartInfo(@"Tools\ffmpeg.exe")
            {
                Arguments = "-i " + XMAFileName + " " + WAVFileName,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardError = false,
                RedirectStandardOutput = false,
                RedirectStandardInput = false
            };
            Process ffmpeg = Process.Start(info);
            ffmpeg.WaitForExit();

            if (File.Exists(WAVFileName))
                return true;
            else
                return false;
                
        }

        /// <summary>
        /// Converts a WAV file to MP3 using ffmpeg. True if MP3FileName exists else false.
        /// </summary>
        /// <param name="wavFileName"></param>
        /// <param name="mp3FileName"></param>
        /// <returns></returns>
        private static bool ConvertWAVToMP3(string wavFileName, string mp3FileName)
        {
            ProcessStartInfo info = new ProcessStartInfo(@"Tools\ffmpeg.exe")
            {
                Arguments = "-i " + wavFileName + " -q:a 0 " + mp3FileName,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardError = false,
                RedirectStandardOutput = false,
                RedirectStandardInput = false
            };
            Process ffmpeg = Process.Start(info);
            ffmpeg.WaitForExit();

            if (File.Exists(mp3FileName))
                return true;
            else
                return false;
        }

        private static bool ConvertWAVToMP3Looping(string wavFileName, string mp3FileName)
        {
            // Assumes that wavFileName and mp3FileName have the same name but different extensions.
            ProcessStartInfo info = new ProcessStartInfo(@"Tools\mp3loop.exe")
            {
                Arguments = wavFileName,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardError = false,
                RedirectStandardOutput = false,
                RedirectStandardInput = false
            };
            Process mp3loop = Process.Start(info);
            mp3loop.WaitForExit();

            if (File.Exists(mp3FileName))
                return true;
            else
                return false;
        }
        
        public static string ConvertXMAPermutation(byte[] buffer, int channelCount, int sampleRate, bool loopingSound, bool useCache, string permutationName)
        {
            if (!File.Exists(@"Tools\ffmpeg.exe"))
            {
                Console.WriteLine("Missing tools, please install all the required tools before porting sounds.");
                return null;
            }

            var gui = Guid.NewGuid();
            var audioFile = permutationName;
            var tempXMA = $"{audioFile}.xma";
            var tempWAV = $"{audioFile}_temp.wav";
            var fixedWAV = $"{audioFile}_truncated.wav";
            var tempMP3 = $"{audioFile}_temp.mp3";
            var resultWAV = $"{audioFile}.wav";
            var resultMP3 = $"{audioFile}.mp3";

        CLEAN_FILES:
            try
            {
                if (File.Exists(tempXMA))
                    File.Delete(tempXMA);
                if (File.Exists(tempWAV))
                    File.Delete(tempWAV);
                if (File.Exists(fixedWAV))
                    File.Delete(fixedWAV);
                if (File.Exists(resultWAV))
                    File.Delete(resultWAV);
                if (File.Exists(resultMP3))
                    File.Delete(resultMP3);
                if (File.Exists(tempMP3))
                    File.Delete(tempMP3);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                goto CLEAN_FILES;
            }

            using (EndianWriter output = new EndianWriter(new FileStream(tempXMA, FileMode.Create, FileAccess.Write, FileShare.None), EndianFormat.BigEndian))
            {
                XMAFile XMAfile = new XMAFile(buffer, channelCount, sampleRate);
                XMAfile.Write(output);
            }

            if (!ConvertXMAToWAV(tempXMA, tempWAV))
                return null;

            // Convert to MP3 and remove header
            if (loopingSound && channelCount <= 2)
            {
                if (!ConvertWAVToMP3Looping(tempWAV, tempMP3))
                    return null;
            }
            else
            {
                // truncate file
                byte[] originalWAVdata = File.ReadAllBytes(tempWAV);
                byte[] truncatedWAVdata = TruncateWAVFile(originalWAVdata, sampleRate, channelCount, 0x2C);

                using (EndianWriter writer = new EndianWriter(new FileStream(fixedWAV, FileMode.Create, FileAccess.Write, FileShare.None), EndianFormat.BigEndian))
                {
                    WAVFile WAVfile = new WAVFile(truncatedWAVdata, channelCount, sampleRate);
                    WAVfile.Write(writer);
                }

                // add code to handle looping sounds with more than 2 channels here. Mp3Loop does not handle that kind of file.

                if (!ConvertWAVToMP3(fixedWAV, tempMP3))
                    return null;
            }
            

            int size = (int)(new FileInfo(tempMP3).Length - 0x2D);
            byte[] MP3stream = File.ReadAllBytes(tempMP3);

            using (var output = new FileStream(resultMP3, FileMode.Create, FileAccess.Write, FileShare.None))
                output.Write(MP3stream, 0x2D, size);

        CLEAN_FILES2:
            try
            {
                if (File.Exists(tempXMA))
                    File.Delete(tempXMA);
                if (File.Exists(tempWAV))
                    File.Delete(tempWAV);
                if (File.Exists(fixedWAV))
                    File.Delete(fixedWAV);
                if (File.Exists(resultWAV))
                    File.Delete(resultWAV);
                if (File.Exists(tempMP3))
                    File.Delete(tempMP3);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                goto CLEAN_FILES2;
            }

            return resultMP3;
        }

        static string GetTagFileFriendlyName(string tagname)
        {
            var pieces = tagname.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries);
            var filename = string.Join("_", pieces);
            return filename;
        }

        private Sound ConvertSound(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, Sound sound, string blamTag_Name)
        {
            if (BlamSoundGestalt == null)
                BlamSoundGestalt = PortingContextFactory.LoadSoundGestalt(CacheContext, ref BlamCache);

            if (!File.Exists(@"Tools\ffmpeg.exe") || !File.Exists(@"Tools\mp3loop.exe") || !File.Exists(@"Tools\towav.exe"))
            {
                Console.WriteLine("Failed to locate sound conversion tools, please install ffmpeg, towav and mp3loop in the Tools folder");
                return null;
            }

            //
            // Convert Sound Tag Data
            //

            var platformCodec = BlamSoundGestalt.PlatformCodecs[sound.SoundReference.PlatformCodecIndex];
            var playbackParameters = BlamSoundGestalt.PlaybackParameters[sound.SoundReference.PlaybackParameterIndex];
            var scale = BlamSoundGestalt.Scales[sound.SoundReference.ScaleIndex];
            var promotion = sound.SoundReference.PromotionIndex != -1 ? BlamSoundGestalt.Promotions[sound.SoundReference.PromotionIndex] : new Promotion();
            var customPlayBack = sound.SoundReference.CustomPlaybackIndex != -1 ? new List<CustomPlayback> { BlamSoundGestalt.CustomPlaybacks[sound.SoundReference.CustomPlaybackIndex] } : new List<CustomPlayback>();
            var loop = sound.Flags.HasFlag(Sound.FlagsValue.LoopingSound);

            sound.PlaybackParameters = playbackParameters;
            sound.Scale = scale;
            sound.PlatformCodec = platformCodec;
            sound.Promotion = promotion;
            sound.CustomPlayBacks = customPlayBack;

            //
            // Tag fixes
            //

            sound.SampleRate = platformCodec.SampleRate;
            sound.ImportType = ImportType.SingleLayer;
            sound.PlatformCodec.LoadMode = 0;

            //
            // Process all the pitch ranges
            //

            sound.PitchRanges = new List<PitchRange>(sound.SoundReference.PitchRangeCount);

            var soundDataAggregate = new byte[0].AsEnumerable();
            var currentSoundDataOffset = 0;
            var largestSampleCount = (uint)0;
            var totalSampleCount = (uint)0;

            var xmaFileSize = BlamSoundGestalt.GetFileSize(sound.SoundReference.PitchRangeIndex, sound.SoundReference.PitchRangeCount);

            if (xmaFileSize < 0)
                return null;

            var xmaData = BlamCache.GetSoundRaw(sound.SoundReference.ZoneAssetHandle, xmaFileSize);

            if (xmaData == null)
                return null;

            for (int pitchRangeIndex = sound.SoundReference.PitchRangeIndex; pitchRangeIndex < sound.SoundReference.PitchRangeIndex+sound.SoundReference.PitchRangeCount; pitchRangeIndex++)
            {
                var relativePitchRangeIndex = pitchRangeIndex - sound.SoundReference.PitchRangeIndex;
                var firstPermutationIndex = BlamSoundGestalt.GetFirstPermutationIndex(pitchRangeIndex);
                var pitchRangeSampleCount = BlamSoundGestalt.GetSamplesPerPitchRange(pitchRangeIndex);

                totalSampleCount += pitchRangeSampleCount;

                if (pitchRangeSampleCount > largestSampleCount)
                    largestSampleCount = pitchRangeSampleCount;

                var permutationOrder = BlamSoundGestalt.GetPermutationOrder(pitchRangeIndex);

                //
                // Convert Blam pitch range to ElDorado format
                //

                var pitchRange = BlamSoundGestalt.PitchRanges[pitchRangeIndex];
                pitchRange.ImportName = ConvertStringId(BlamSoundGestalt.ImportNames[pitchRange.ImportNameIndex].Name);
                pitchRange.PitchRangeParameters = BlamSoundGestalt.PitchRangeParameters[pitchRange.PitchRangeParametersIndex];
                pitchRange.Unknown1 = 0;
                pitchRange.Unknown2 = 0;
                pitchRange.Unknown3 = 0;
                pitchRange.Unknown4 = 0;
                pitchRange.Unknown5 = -1;
                pitchRange.Unknown6 = -1;
                //I suspect this unknown7 to be a flag to tell if there is a Unknownblock in extrainfo. (See a sound in udlg for example)
                pitchRange.Unknown7 = 0;
                pitchRange.PermutationCount = (byte)BlamSoundGestalt.GetPermutationCount(pitchRangeIndex);
                pitchRange.Unknown8 = -1;

                // Add pitch range to ED sound
                sound.PitchRanges.Add(pitchRange);
                var newPitchRangeIndex = pitchRangeIndex - sound.SoundReference.PitchRangeIndex;
                sound.PitchRanges[newPitchRangeIndex].Permutations = new List<Permutation>();

                //
                // Determine the audio channel count
                //

                var channelCount = sound.PlatformCodec.Encoding.GetChannelCount();

                //
                // Set compression format
                //

                sound.PlatformCodec.Compression = Compression.MP3;

                //
                // Convert Blam permutations to ElDorado format
                //

                var useCache = Sounds.UseAudioCacheCommand.AudioCacheDirectory != null;

                var basePermutationCacheName = Path.Combine(
                    useCache ? Sounds.UseAudioCacheCommand.AudioCacheDirectory.FullName : TempDirectory.FullName,
                    GetTagFileFriendlyName(blamTag_Name));

                var permutationCount = BlamSoundGestalt.GetPermutationCount(pitchRangeIndex);

                for (int i = 0; i < permutationCount; i++)
                {
                    var permutationIndex = pitchRange.FirstPermutationIndex + i;
                    var permutationSize = BlamSoundGestalt.GetPermutationSize(permutationIndex);
                    var permutationOffset = BlamSoundGestalt.GetPermutationOffset(permutationIndex);

                    var permutation = BlamSoundGestalt.GetPermutation(permutationIndex);

                    permutation.ImportName = ConvertStringId(BlamSoundGestalt.ImportNames[permutation.ImportNameIndex].Name);
                    permutation.SkipFraction = new Bounds<float>(0.0f, permutation.Gain);
                    permutation.PermutationChunks = new List<PermutationChunk>();
                    permutation.PermutationNumber = (uint)permutationOrder[i];
                    permutation.IsNotFirstPermutation = (uint)(permutation.PermutationNumber == 0 ? 0 : 1);

                    var permutationData = new byte[permutationSize];
                    Array.Copy(xmaData, permutationOffset, permutationData, 0, permutationSize);

                    string permutationName = $"{basePermutationCacheName}_{relativePitchRangeIndex}_{i}";
                    var cacheFileName = $"{permutationName}.mp3";

                    bool exists = File.Exists(cacheFileName);

                    if ((permutationName != null && !exists) || !useCache)
                        cacheFileName = ConvertXMAPermutation(permutationData, channelCount, sound.SampleRate.GetSampleRateHz(), loop,  useCache, permutationName);

                    var result = File.ReadAllBytes(cacheFileName);

                    if (!useCache)
                    {
                        if (File.Exists(cacheFileName))
                            File.Delete(cacheFileName);
                    }

                    permutation.PermutationChunks.Add(new PermutationChunk(currentSoundDataOffset, result.Length));
                    currentSoundDataOffset += result.Length;
                    pitchRange.Permutations.Add(permutation);

                    soundDataAggregate = soundDataAggregate.Concat(result);
                }
            }

            sound.Promotion.LongestPermutationDuration = (uint)(1000 * ((float)largestSampleCount) / sound.SampleRate.GetSampleRateHz());
            sound.Promotion.TotalSampleSize = totalSampleCount;

            //
            // Convert Blam extra info to ElDorado format
            //

            var extraInfo = new ExtraInfo()
            {
                LanguagePermutations = new List<ExtraInfo.LanguagePermutation>(),
                EncodedPermutationSections = new List<ExtraInfo.EncodedPermutationSection>()
            };

            for (int u = 0; u < sound.SoundReference.PitchRangeCount; u++)
            {
                var pitchRange = BlamSoundGestalt.PitchRanges[sound.SoundReference.PitchRangeIndex + u];

                var languagePermutation = new ExtraInfo.LanguagePermutation
                {
                    RawInfo = new List<ExtraInfo.LanguagePermutation.RawInfoBlock>()
                };

                for (int i = 0; i < sound.PitchRanges[u].PermutationCount; i++)
                {
                    var rawInfo = new ExtraInfo.LanguagePermutation.RawInfoBlock
                    {
                        SkipFractionName = StringId.Invalid,
                        Unknown24 = 480,
                        UnknownList = new List<ExtraInfo.LanguagePermutation.RawInfoBlock.Unknown>(),
                        Compression = 8,
                        SampleCount = (uint)Math.Floor(pitchRange.Permutations[i].SampleSize * 128000.0 / (8 * sound.SampleRate.GetSampleRateHz())),
                        ResourceSampleSize = pitchRange.Permutations[i].SampleSize,
                        ResourceSampleOffset = (uint)pitchRange.Permutations[i].PermutationChunks[0].Offset
                    };

                    languagePermutation.RawInfo.Add(rawInfo);
                }

                extraInfo.LanguagePermutations.Add(languagePermutation);
            }

            if (sound.SoundReference.ExtraInfoIndex != -1)
            {
                foreach (var section in BlamSoundGestalt.ExtraInfo[sound.SoundReference.ExtraInfoIndex].EncodedPermutationSections)
                {
                    var newSection = section.DeepClone();

                    foreach (var info in newSection.SoundDialogueInfo)
                    {
                        for (var i = ((info.MouthDataLength % 2) == 0 ? 0 : 1); (i + 1) < info.MouthDataLength; i += 2)
                            Array.Reverse(newSection.EncodedData, (int)(info.MouthDataOffset + i), 2);

                        for (var i = ((info.LipsyncDataLength % 2) == 0 ? 0 : 1); (i + 1) < info.LipsyncDataLength; i += 2)
                            Array.Reverse(newSection.EncodedData, (int)(info.LipsyncDataOffset + i), 2);
                    }

                    extraInfo.EncodedPermutationSections.Add(newSection);
                }
            }

            sound.ExtraInfo = new List<ExtraInfo> { extraInfo };

            //
            // Convert Blam languages to ElDorado format
            //

            if (sound.SoundReference.LanguageIndex != -1)
            {
                sound.Languages = new List<LanguageBlock>();

                foreach (var language in BlamSoundGestalt.Languages)
                {
                    sound.Languages.Add(new LanguageBlock
                    {
                        Language = language.Language,
                        PermutationDurations = new List<LanguageBlock.PermutationDurationBlock>(),
                        PitchRangeDurations = new List<LanguageBlock.PitchRangeDurationBlock>(),
                    });

                    //Add all the  Pitch Range Duration block (pitch range count dependent)

                    var curLanguage = sound.Languages.Last();

                    for (int i = 0; i < sound.SoundReference.PitchRangeCount; i++)
                    {
                        curLanguage.PitchRangeDurations.Add(language.PitchRangeDurations[sound.SoundReference.LanguageIndex + i]);
                    }

                    //Add all the Permutation Duration Block and adjust offsets

                    for (int i = 0; i < curLanguage.PitchRangeDurations.Count; i++)
                    {
                        var curPRD = curLanguage.PitchRangeDurations[i];

                        //Get current block count for new index
                        short newPermutationIndex = (short)curLanguage.PermutationDurations.Count();

                        for (int j = curPRD.PermutationStartIndex; j < curPRD.PermutationStartIndex + curPRD.PermutationCount; j++)
                        {
                            curLanguage.PermutationDurations.Add(language.PermutationDurations[j]);
                        }

                        //apply new index
                        curPRD.PermutationStartIndex = newPermutationIndex;
                    }

                }
            }

            //
            // Prepare resource
            //

            sound.Unused = new byte[] { 0, 0, 0, 0 };
            sound.Unknown12 = 0;

            sound.Resource = new PageableResource
            {
                Page = new RawPage
                {
                    Index = -1,
                },
                Resource = new TagResourceGen3
                {
                    ResourceType = TagResourceTypeGen3.Sound,
                    DefinitionData = new byte[20],
                    DefinitionAddress = new CacheAddress(CacheAddressType.Definition, 536870912),
                    ResourceFixups = new List<TagResourceGen3.ResourceFixup>(),
                    ResourceDefinitionFixups = new List<TagResourceGen3.ResourceDefinitionFixup>(),
                    Unknown2 = 1
                }
            };

            var data = soundDataAggregate.ToArray();

            var resourceContext = new ResourceSerializationContext(sound.Resource);
            CacheContext.Serializer.Serialize(resourceContext,
                new SoundResourceDefinition
                {
                    Data = new TagData(data.Length, new CacheAddress(CacheAddressType.Resource, 0))
                });

            var definitionFixup = new TagResourceGen3.ResourceFixup()
            {
                BlockOffset = 12,
                Address = new CacheAddress(CacheAddressType.Resource, 1073741824)
            };

            sound.Resource.Resource.ResourceFixups.Add(definitionFixup);

            sound.Resource.ChangeLocation(ResourceLocation.Audio);
            var resource = sound.Resource;

            if (resource == null)
                throw new ArgumentNullException("resource");

            var cache = CacheContext.GetResourceCache(ResourceLocation.Audio);

            if (!resourceStreams.ContainsKey(ResourceLocation.Audio))
            {
                resourceStreams[ResourceLocation.Audio] = FlagIsSet(PortingFlags.Memory) ?
                    new MemoryStream() :
                    (Stream)CacheContext.OpenResourceCacheReadWrite(ResourceLocation.Audio);

                if (FlagIsSet(PortingFlags.Memory))
                    using (var resourceStream = CacheContext.OpenResourceCacheRead(ResourceLocation.Audio))
                        resourceStream.CopyTo(resourceStreams[ResourceLocation.Audio]);
            }

            resource.Page.Index = cache.Add(resourceStreams[ResourceLocation.Audio], data, out uint compressedSize);
            resource.Page.CompressedBlockSize = compressedSize;
            resource.Page.UncompressedBlockSize = (uint)data.Length;
            resource.DisableChecksum();

            for (int i = 0; i < 4; i++)
            {
                sound.Resource.Resource.DefinitionData[i] = (byte)(sound.Resource.Page.UncompressedBlockSize >> (i * 8));
            }

            return sound;
        }

        private SoundLooping ConvertSoundLooping(SoundLooping soundLooping)
        {
            if (BlamSoundGestalt == null)
                BlamSoundGestalt = PortingContextFactory.LoadSoundGestalt(CacheContext, ref BlamCache);

            soundLooping.Unused = null;

            soundLooping.SoundClass = ((int)soundLooping.SoundClass < 50) ? soundLooping.SoundClass : (soundLooping.SoundClass + 1);

            if (soundLooping.SoundClass == SoundLooping.SoundClassValue.FirstPersonInside)
                soundLooping.SoundClass = SoundLooping.SoundClassValue.InsideSurroundTail;

            if (soundLooping.SoundClass == SoundLooping.SoundClassValue.FirstPersonOutside)
                soundLooping.SoundClass = SoundLooping.SoundClassValue.OutsideSurroundTail;

            /* unsuccessful hacks of death and suffering
            foreach (var track in soundLooping.Tracks)
            {
                track.FadeInDuration *= 2f;
                track.Unknown1 *= 2f;
                track.FadeOutDuration *= 2f;
                track.AlternateCrossfadeDuration *= 2f;
                track.Unknown5 *= 2;
                track.AlternateFadeOutDuration *= 2f;
                track.Unknown6 *= 2f;
            }

            foreach (var detailSound in soundLooping.DetailSounds)
                detailSound.RandomPeriodBounds = new Bounds<float>(
                    detailSound.RandomPeriodBounds.Lower * 2f,
                    detailSound.RandomPeriodBounds.Upper * 2f);*/

            return soundLooping;
        }

        private Dialogue ConvertDialogue(Stream cacheStream, Dialogue dialogue)
        {
            if (BlamSoundGestalt == null)
                BlamSoundGestalt = PortingContextFactory.LoadSoundGestalt(CacheContext, ref BlamCache);

            CachedTagInstance edAdlg = null;
            AiDialogueGlobals adlg = null;
            foreach (var tag in CacheContext.TagCache.Index.FindAllInGroup("adlg"))
            {
                edAdlg = tag;
                break;
            }

            adlg = CacheContext.Deserialize<AiDialogueGlobals>(cacheStream, edAdlg);

            //Create empty udlg vocalization block and fill it with empty blocks matching adlg

            List<Dialogue.Vocalization> newVocalization = new List<Dialogue.Vocalization>();
            foreach (var vocalization in adlg.Vocalizations)
            {
                Dialogue.Vocalization block = new Dialogue.Vocalization
                {
                    Sound = null,
                    Flags = 0,
                    Unknown = 0,
                    Name = vocalization.Name,
                };
                newVocalization.Add(block);
            }

            //Match the tags with the proper stringId

            for (int i = 0; i < 304; i++)
            {
                var vocalization = newVocalization[i];
                for (int j = 0; j < dialogue.Vocalizations.Count; j++)
                {
                    var vocalizationH3 = dialogue.Vocalizations[j];
                    if (CacheContext.StringIdCache.GetString(vocalization.Name).Equals(CacheContext.StringIdCache.GetString(vocalizationH3.Name)))
                    {
                        vocalization.Flags = vocalizationH3.Flags;
                        vocalization.Unknown = vocalizationH3.Unknown;
                        vocalization.Sound = vocalizationH3.Sound;
                        break;
                    }
                }
            }

            dialogue.Vocalizations = newVocalization;

            return dialogue;
        }

        private SoundMix ConvertSoundMix(SoundMix soundMix)
        {
            if (BlamCache.Version == CacheVersion.Halo3Retail)
                soundMix.Unknown1 = 0;

            return soundMix;
        }
    }
}
