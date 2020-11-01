using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Audio;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private SoundCacheFileGestalt BlamSoundGestalt { get; set; } = null;

        static string GetTagFileFriendlyName(string tagname)
        {
            var pieces = tagname.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries);
            var filename = string.Join("_", pieces);
            return filename;
        }

        private Sound ConvertSound(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, Sound sound, string blamTag_Name)
        {
            if (BlamSoundGestalt == null)
                BlamSoundGestalt = PortingContextFactory.LoadSoundGestalt(BlamCache, blamCacheStream);

            if (!File.Exists(@"Tools\ffmpeg.exe") || !File.Exists(@"Tools\towav.exe") || !File.Exists(@"Tools\xmadec.exe"))
            {
                Console.WriteLine("Failed to locate sound conversion tools, please install ffmpeg, towav and xmadec in the Tools folder");
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

            sound.PlaybackParameters = playbackParameters.DeepClone();
            sound.Scale = scale;
            sound.PlatformCodec = platformCodec.DeepClone();
            sound.Promotion = promotion;
            sound.CustomPlayBacks = customPlayBack;

            //
            // Tag fixes
            //

            sound.SampleRate = platformCodec.SampleRate;
            sound.ImportType = ImportType.SingleLayer;
            // helps looping sound? there is another value, 10 for Unknown2 but I don't know when to activate it.
            if (sound.SoundReference.PitchRangeCount > 1)
                sound.ImportType = ImportType.MultiLayer;

            sound.PlatformCodec.LoadMode = 0;

            if(BlamCache.Version >= CacheVersion.HaloReach)
            {
                // Fix playback parameters for reach

                sound.PlaybackParameters.MaximumBendPerSecond = sound.PlaybackParameters.MaximumBendPerSecondReach;
                sound.PlaybackParameters.SkipFraction = sound.PlaybackParameters.SkipFractionReach;

                sound.PlaybackParameters.DistanceB = sound.PlaybackParameters.DistanceC;
                sound.PlaybackParameters.DistanceC = sound.PlaybackParameters.DistanceE;
                sound.PlaybackParameters.DistanceD = sound.PlaybackParameters.DistanceG;

                sound.PlaybackParameters.FieldDisableFlags = 0;

                if (sound.PlaybackParameters.DistanceA == 0)
                    sound.PlaybackParameters.FieldDisableFlags |= PlaybackParameter.FieldDisableFlagsValue.DistanceA;

                if (sound.PlaybackParameters.DistanceB == 0)
                    sound.PlaybackParameters.FieldDisableFlags |= PlaybackParameter.FieldDisableFlagsValue.DistanceB;

                if (sound.PlaybackParameters.DistanceC == 0)
                    sound.PlaybackParameters.FieldDisableFlags |= PlaybackParameter.FieldDisableFlagsValue.DistanceC;

                if (sound.PlaybackParameters.DistanceD == 0)
                    sound.PlaybackParameters.FieldDisableFlags |= PlaybackParameter.FieldDisableFlagsValue.DistanceD;

                sound.PlaybackParameters.FieldDisableFlags |= PlaybackParameter.FieldDisableFlagsValue.Bit4;
            }

            //
            // Process all the pitch ranges
            //

            var xmaFileSize = BlamSoundGestalt.GetFileSize(sound.SoundReference.PitchRangeIndex, sound.SoundReference.PitchRangeCount);

            if (xmaFileSize < 0)
                return null;

            var soundResource = BlamCache.ResourceCache.GetSoundResourceDefinition(sound.Resource);

            if (soundResource == null)
                return null;

            var xmaData = soundResource.Data.Data;

            if (xmaData == null)
                return null;

            sound.PitchRanges = new List<PitchRange>(sound.SoundReference.PitchRangeCount);

            var soundDataAggregate = new byte[0].AsEnumerable();
            var currentSoundDataOffset = 0;
            var totalSampleCount = (uint)0;

            for (int pitchRangeIndex = sound.SoundReference.PitchRangeIndex; pitchRangeIndex < sound.SoundReference.PitchRangeIndex + sound.SoundReference.PitchRangeCount; pitchRangeIndex++)
            {
                totalSampleCount += BlamSoundGestalt.GetSamplesPerPitchRange(pitchRangeIndex);

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

                var channelCount = Encoding.GetChannelCount(sound.PlatformCodec.Encoding);

                //
                // Set compression format
                //

                var targetFormat = Compression.OGG;

                string extension;
                switch (targetFormat)
                {
                    case Compression.MP3:
                        extension = "mp3";
                        break;
                    case Compression.PCM:
                        extension = "wav";
                        break;
                    case Compression.OGG:
                        extension = "ogg";
                        break;
                    default:
                        extension = "mp3";
                        break;
                }

                sound.PlatformCodec.Compression = targetFormat;

                //
                // Convert Blam permutations to ElDorado format
                //

                var useCache = Sounds.UseAudioCacheCommand.AudioCacheDirectory != null;
                var basePermutationCacheName = Path.Combine(TempDirectory.FullName + GetTagFileFriendlyName(blamTag_Name)); //temp File path
                var originalFilePath = "";


                if (useCache)
                {
                    originalFilePath = Path.Combine(Sounds.UseAudioCacheCommand.AudioCacheDirectory.FullName, GetTagFileFriendlyName(blamTag_Name)); //old structure file path

                    var split = blamTag_Name.Split('\\');
                    var endName = split[split.Length - 1]; //get the last portion of the tag name
                    var newPath = Sounds.UseAudioCacheCommand.AudioCacheDirectory.FullName;

                    for (int i = 0; i < split.Length - 1; i++)
                    {

                        var folder = split[i];

                        var dir = Path.Combine(newPath, folder); //combine the new path with the current folder
                        if (!Directory.Exists(dir))// check if that specific folder exists and if not create it
                            Directory.CreateDirectory(dir);

                        newPath = Path.Combine(newPath, folder); // update the new path varible with the current folder
                    }

                    basePermutationCacheName = Path.Combine(newPath, endName); //combine the last portion of the tag name with the new path
                }

                var permutationCount = BlamSoundGestalt.GetPermutationCount(pitchRangeIndex);
                var permutationOrder = BlamSoundGestalt.GetPermutationOrder(pitchRangeIndex);
                var relativePitchRangeIndex = pitchRangeIndex - sound.SoundReference.PitchRangeIndex;

                for (int i = 0; i < permutationCount; i++)
                {
                    var permutationIndex = pitchRange.FirstPermutationIndex + i;
                    var permutationSize = BlamSoundGestalt.GetPermutationSize(permutationIndex);
                    var permutationOffset = BlamSoundGestalt.GetPermutationOffset(permutationIndex);

                    var permutation = BlamSoundGestalt.GetPermutation(permutationIndex).DeepClone();

                    permutation.ImportName = ConvertStringId(BlamSoundGestalt.ImportNames[permutation.ImportNameIndex].Name);
                    permutation.SkipFraction = new Bounds<float>(0.0f, permutation.EncodedSkipFraction / 32767.0f);
                    permutation.PermutationChunks = new List<PermutationChunk>();
                    permutation.PermutationNumber = (uint)permutationOrder[i];
                    permutation.IsNotFirstPermutation = (uint)(permutation.PermutationNumber == 0 ? 0 : 1);

                    string permutationName = $"{basePermutationCacheName}_{relativePitchRangeIndex}_{i}";
                    
                    var cacheFileName = $"{permutationName}.{extension}";

                    if (useCache) { 
                        string oldPermutationName = $"{originalFilePath}_{relativePitchRangeIndex}_{i}";
                        var oldCacheFileName = $"{oldPermutationName}.{extension}";

                        if (File.Exists(oldCacheFileName)) //check if sound exists in the old format and rename it to use the new folder structure
                        {
                            Console.WriteLine("Sound exists in original format: " + oldCacheFileName + " Renaming...");
                            File.Move(oldCacheFileName, cacheFileName);
                        }
                    }

                    bool exists = File.Exists(cacheFileName);

                    byte[] permutationData = null;

                    if (!exists || !useCache)
                    {
                        BlamSound blamSound = SoundConverter.ConvertGen3Sound(BlamCache, BlamSoundGestalt, sound, relativePitchRangeIndex, i, xmaData, targetFormat);
                        permutationData = blamSound.Data;
                        if (useCache)
                        {
                            using (EndianWriter output = new EndianWriter(new FileStream(cacheFileName, FileMode.Create, FileAccess.Write, FileShare.None), EndianFormat.BigEndian))
                            {
                                output.WriteBlock(blamSound.Data);
                            }
                        }
                    }
                    else
                    {
                        permutationData = File.ReadAllBytes(cacheFileName);
                    }

                    // fixup dialog indices, might need more work
                    var firstPermutationChunk = BlamSoundGestalt.GetFirstPermutationChunk(permutationIndex);
                    var newChunk = new PermutationChunk(currentSoundDataOffset, permutationData.Length, firstPermutationChunk.SoundDialogInfoIndex, firstPermutationChunk.Unknown, firstPermutationChunk.UnknownSize);
                    permutation.PermutationChunks.Add(newChunk);
                    currentSoundDataOffset += permutationData.Length;
                    pitchRange.Permutations.Add(permutation);

                    soundDataAggregate = soundDataAggregate.Concat(permutationData);
                }
            }

            sound.Promotion.LongestPermutationDuration = (uint)sound.SoundReference.MaximumPlayTime;
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

                for (int i = 0; i < pitchRange.PermutationCount; i++)
                {
                    var permutation = BlamSoundGestalt.GetPermutation(pitchRange.FirstPermutationIndex + i);
                    var permutationChunk = BlamSoundGestalt.GetPermutationChunk(permutation.FirstPermutationChunkIndex);

                    extraInfo.LanguagePermutations.Add(new ExtraInfo.LanguagePermutation
                    {
                        RawInfo = new List<ExtraInfo.LanguagePermutation.RawInfoBlock>
                        {
                            new ExtraInfo.LanguagePermutation.RawInfoBlock
                            {
                                SkipFractionName = StringId.Invalid,
                                UnknownList = new List<ExtraInfo.LanguagePermutation.RawInfoBlock.Unknown>
                                {
                                    new ExtraInfo.LanguagePermutation.RawInfoBlock.Unknown
                                    {
                                        SoundDialogInfoSize = permutationChunk.SoundDialogInfoIndex,
                                        Unknown1 = permutationChunk.Unknown,
                                        Unknown2 = permutationChunk.UnknownSize,
                                        Unknown3 = 0,
                                        Unknown4 = permutation.SampleSize,
                                        Unknown5 = 0,
                                        Unknown6 = permutationChunk.EncodedSize & 0xFFFF
                                    }
                                },
                                Compression = 8,
                                ResourceSampleSize = pitchRange.Permutations[i].SampleSize,
                                ResourceSampleOffset = (uint)pitchRange.Permutations[i].PermutationChunks[0].Offset,
                                SampleCount = (uint)pitchRange.Permutations[i].PermutationChunks[0].EncodedSize & 0x3FFFFFF,
                                //SampleCount = (uint)Math.Floor(pitchRange.Permutations[i].SampleSize * 128000.0 / (8 * sound.SampleRate.GetSampleRateHz())),
                                Unknown24 = 480
                            }
                        }
                    });
                }
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
                if(BlamCache.Version < CacheVersion.HaloReach)
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
                else
                {
                    // TODO: reverse reach's facial animation resource
                }
            }

            //
            // Prepare resource
            //

            sound.Unknown12 = 0;

            var data = soundDataAggregate.ToArray();
            var resourceDefinition = AudioUtils.CreateSoundResourceDefinition(data);
            var resourceReference = CacheContext.ResourceCache.CreateSoundResource(resourceDefinition);
            sound.Resource = resourceReference;

            return sound;
        }

        private SoundLooping ConvertSoundLooping(SoundLooping soundLooping)
        {
            soundLooping.Unused = null;

            soundLooping.SoundClass = ((int)soundLooping.SoundClass < 50) ? soundLooping.SoundClass : (soundLooping.SoundClass + 1);

            if (soundLooping.SoundClass == SoundLooping.SoundClassValue.FirstPersonInside)
                soundLooping.SoundClass = SoundLooping.SoundClassValue.InsideSurroundTail;

            if (soundLooping.SoundClass == SoundLooping.SoundClassValue.FirstPersonOutside)
                soundLooping.SoundClass = SoundLooping.SoundClassValue.OutsideSurroundTail;


            //
            // Fixes for looping sound (temporary and hacky)
            //

            if (soundLooping.SoundClass == SoundLooping.SoundClassValue.VehicleEngine ||
                soundLooping.SoundClass == SoundLooping.SoundClassValue.VehicleEngineLod ||
                soundLooping.SoundClass == SoundLooping.SoundClassValue.Music)
            {
                soundLooping.Unknown4 = 1;
            }

            if (BlamCache.Version == CacheVersion.Halo3Retail)
            {
                foreach (var track in soundLooping.Tracks)
                {
                    // FadeMode was added in ODST, H3 uses Linear for in sounds, and Power for out sounds
                    track.FadeInMode = SoundLooping.Track.SoundFadeMode.Linear;
                    track.FadeOutMode = SoundLooping.Track.SoundFadeMode.Power;
                    track.AlternateCrossfadeMode = SoundLooping.Track.SoundFadeMode.Linear;
                    track.AlternateFadeOutMode = SoundLooping.Track.SoundFadeMode.Power;
                }
            }

            return soundLooping;
        }

        private Dialogue ConvertDialogue(Stream cacheStream, Dialogue dialogue)
        {

            CachedTag edAdlg = null;
            AiDialogueGlobals adlg = null;
            foreach (var tag in CacheContext.TagCache.FindAllInGroup("adlg"))
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

            if(BlamCache.Version < CacheVersion.HaloReach)
            {
                for (int i = 0; i < 304; i++)
                {
                    var vocalization = newVocalization[i];
                    for (int j = 0; j < dialogue.Vocalizations.Count; j++)
                    {
                        var vocalizationH3 = dialogue.Vocalizations[j];
                        if (CacheContext.StringTable.GetString(vocalization.Name).Equals(CacheContext.StringTable.GetString(vocalizationH3.Name)))
                        {
                            vocalization.Flags = vocalizationH3.Flags;
                            vocalization.Unknown = vocalizationH3.Unknown;
                            vocalization.Sound = vocalizationH3.Sound;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 304; i++)
                {
                    var vocalization = newVocalization[i];
                    for (int j = 0; j < dialogue.Vocalizations.Count; j++)
                    {
                        var vocalizationReach = dialogue.Vocalizations[j];
                        if (CacheContext.StringTable.GetString(vocalization.Name).Equals(CacheContext.StringTable.GetString(vocalizationReach.Name)))
                        {
                            // we use index 0 because other indices are for different situation like stealth. 
                            if(vocalizationReach.ReachSounds.Count > 0)
                                vocalization.Sound = vocalizationReach.ReachSounds[0].Sound;

                            break;
                        }
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

        // HO uses ODST classes, with H3 structure
        private SoundClasses ConvertSoundClasses(SoundClasses sncl, CacheVersion version)
        {
            // setup class with "default" values
            var sClass = new SoundClasses.Class()
            {
                MaxSoundsPerTag = 4,
                MaxSoundsPerObject = 1,
                PreemptionTime = 100,

                InternalFlags = (SoundClasses.Class.InternalFlagBits.ClassIsValid | SoundClasses.Class.InternalFlagBits.Bit4 | 
                 SoundClasses.Class.InternalFlagBits.Bit5 | SoundClasses.Class.InternalFlagBits.Bit6 |
                 SoundClasses.Class.InternalFlagBits.Bit8 | SoundClasses.Class.InternalFlagBits.Bit9),

                Priority = 5,
                DistanceBoundsMin = 8.0f,
                DistanceBoundsMax = 120.0f,
                TransmissionMultiplier = 1.0f
            };

            // hud class, unique to HO. the above values seem to be okay
            if (sncl.Classes.Count >= 50)
                sncl.Classes.Insert(50, sClass);

            if (version <= CacheVersion.Halo3Retail)
            {
                // add classes missing from H3
                for (int i = sncl.Classes.Count; i < 65; i++)
                    sncl.Classes.Add(sClass);
            }

            // ms23 requires this flag to play a class on the mainmenu
            sncl.Classes[32].Flags |= SoundClasses.Class.ClassFlagBits.ClassPlaysOnMainmenu; // Music
            sncl.Classes[52].Flags |= SoundClasses.Class.ClassFlagBits.ClassPlaysOnMainmenu; // UI

            return sncl;
        }
    }
}