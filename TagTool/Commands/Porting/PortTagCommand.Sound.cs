using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TagTool.Audio;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private SoundCacheFileGestalt BlamSoundGestalt { get; set; } = null;
        private Dictionary<Sound, Task> SoundConversionTasks = new Dictionary<Sound, Task>();
        private SemaphoreSlim ConcurrencyLimiter;

        class SoundConversionResult
        {
            public Sound Definition;
            public byte[] Data;
            // a list of functions that will be run after conversion
            // used for operations like string id conversion which cannot be done concurrently
            public List<Action> PostConversionOperations = new List<Action>();
        }

        public void InitializeSoundConverter()
        {
            ConcurrencyLimiter = new SemaphoreSlim(PortingOptions.Current.MaxThreads);
        }

        private void WaitForPendingSoundConversion()
        {
            Task.WaitAll(SoundConversionTasks.Values.ToArray());
        }

        private Sound FinishConvertSound(SoundConversionResult result)
        {
            var task = SoundConversionTasks[result.Definition];
            SoundConversionTasks.Remove(result.Definition);

            if (!task.IsFaulted)
            {
                var sound = result.Definition;
                var resourceDefinition = AudioUtils.CreateSoundResourceDefinition(result.Data);
                sound.Resource = CacheContext.ResourceCache.CreateSoundResource(resourceDefinition);
                // do post conversion operations
                result.PostConversionOperations.ForEach(op => op());
                return sound;
            }
            else
            {
                // rethrow the exception
                task.GetAwaiter().GetResult();
                return null;
            }
        }

        private Sound ConvertSound(Stream cacheStream, Stream blamCacheStream,
            Dictionary<ResourceLocation, Stream> resourceStreams, Sound sound,
            CachedTag destTag, string blamTag_Name, Action<SoundConversionResult> callback)
        {
            if (BlamSoundGestalt == null)
                BlamSoundGestalt = PortingContextFactory.LoadSoundGestalt(BlamCache, blamCacheStream);

            if (!File.Exists($@"{Program.TagToolDirectory}\Tools\ffmpeg.exe")
            || !File.Exists($@"{Program.TagToolDirectory}\Tools\towav.exe")
            || !File.Exists($@"{Program.TagToolDirectory}\Tools\xmadec.exe"))
            {
                new TagToolError(CommandError.CustomError,
                    "Failed to locate sound conversion tools. Please install ffmpeg, towav and xmadec in the Tools folder.");
                return null;
            }

            ConcurrencyLimiter.Wait();
            SoundConversionTasks.Add(sound, Task.Run(() =>
            {
                try
                {
                    SoundConversionResult result;
                    if (sound.SoundReference == null)
                        result = ConvertSoundInternalTagsBuild(sound, blamTag_Name);
                    else
                        result = ConvertSoundInternal(sound, blamTag_Name);

                    callback(result);
                }
                finally
                {
                    ConcurrencyLimiter.Release();
                }
            }));

            return sound;
        }

        private SoundConversionResult ConvertSoundInternalTagsBuild(Sound sound, string blamTag_Name)
        {
            var newData = new MemoryStream();

            var result = new SoundConversionResult();
            result.Definition = sound;
            var resourceReference = sound.GetResource(BlamCache.Version, BlamCache.Platform);
            var soundResource = BlamCache.ResourceCache.GetSoundResourceDefinition(resourceReference);
            if(soundResource == null)
                return null;


            var useCache = Sounds.UseAudioCacheCommand.AudioCacheDirectory != null;
            var soundCachePath = useCache ? Sounds.UseAudioCacheCommand.AudioCacheDirectory.FullName : "";

            byte[] xmaData = null;
            if (soundResource != null)
            {
                xmaData = soundResource.Data.Data;
                if (xmaData == null)
                    return null;
            }

            var targetFormat = PortingOptions.Current.AudioCodec;


            for (int pitchRangeIndex = 0; pitchRangeIndex < sound.PitchRanges.Count; pitchRangeIndex++)
            {
                var pitchRange = sound.PitchRanges[pitchRangeIndex];
                pitchRange.RuntimePermutationFlags = -1;
                pitchRange.RuntimeDiscardedPermutationIndex = -1;
                pitchRange.PermutationCount = (short)pitchRange.Permutations.Count;
                pitchRange.RuntimeLastPermutationIndex = -1;

                for (int permutationIndex = 0; permutationIndex < pitchRange.Permutations.Count; permutationIndex++)
                {
                    var permutation = pitchRange.Permutations[permutationIndex];

                    BlamSound blamSound = SoundConverter.ConvertGen3Sound(BlamCache, null, sound, pitchRangeIndex, permutationIndex, xmaData, targetFormat, useCache, soundCachePath, blamTag_Name);
                    byte[] permutationData = blamSound.Data;
                    permutation.SampleCount = blamSound.SampleCount;
                    permutation.FirstSample = blamSound.FirstSample;
                    permutation.PermutationChunks = new List<PermutationChunk>();
                    permutation.PermutationChunks.Add(new PermutationChunk((int)newData.Position, permutationData.Length, permutation.FirstSample + permutation.SampleCount, permutation.FirstSample));
                    newData.Write(permutationData, 0, permutationData.Length);
                }
            }

            if(CacheVersionDetection.GetGameTitle(BlamCache.Version) == GameTitle.HaloReach)
            {
                sound.Flags = sound.FlagsReach.ConvertLexical<Sound.FlagsValue>();
                sound.Playback = ConvertPlaybackReach(sound.Playback);
            }
                
            sound.PlatformCodec.Compression = targetFormat;
            result.Data = newData.ToArray();
            return result;
        }

        private SoundConversionResult ConvertSoundInternal(Sound sound, string blamTag_Name)
        {
            var result = new SoundConversionResult();
            result.Definition = sound;

            //
            // Convert Sound Tag Data
            //

            var platformCodec = BlamSoundGestalt.PlatformCodecs[sound.SoundReference.PlatformCodecIndex];
            var playbackParameters = BlamSoundGestalt.PlaybackParameters[sound.SoundReference.PlaybackParameterIndex];
            var scale = BlamSoundGestalt.Scales[sound.SoundReference.ScaleIndex];
            var promotion = sound.SoundReference.PromotionIndex != -1 ? BlamSoundGestalt.Promotions[sound.SoundReference.PromotionIndex] : new Promotion();
            var customPlayBack = sound.SoundReference.CustomPlaybackIndex != -1 ? new List<CustomPlayback> { BlamSoundGestalt.CustomPlaybacks[sound.SoundReference.CustomPlaybackIndex] } : new List<CustomPlayback>();

            sound.Playback = playbackParameters.DeepClone();
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

            if (BlamCache.Version >= CacheVersion.HaloReach)
            {
                sound.Flags = sound.FlagsReach.ConvertLexical<Sound.FlagsValue>();
                sound.Playback = ConvertPlaybackReach(playbackParameters);
            }

            //
            // Process all the pitch ranges
            //

            var xmaFileSize = BlamSoundGestalt.GetFileSize(sound.SoundReference.PitchRangeIndex, sound.SoundReference.PitchRangeCount, BlamCache.Platform);

            if (xmaFileSize < 0)
                return null;

            var soundResource = BlamCache.ResourceCache.GetSoundResourceDefinition(sound.GetResource(BlamCache.Version, BlamCache.Platform));

            byte[] xmaData = null;
            if (soundResource != null)
            {
                xmaData = soundResource.Data.Data;
                if (xmaData == null)
                    return null;
            }

            sound.PitchRanges = new List<PitchRange>(sound.SoundReference.PitchRangeCount);

            var soundDataAggregate = new byte[0].AsEnumerable();
            var currentSoundDataOffset = 0;
            var totalSampleCount = (uint)0;

            for (int pitchRangeIndex = sound.SoundReference.PitchRangeIndex; pitchRangeIndex < sound.SoundReference.PitchRangeIndex + sound.SoundReference.PitchRangeCount; pitchRangeIndex++)
            {
                totalSampleCount += BlamSoundGestalt.GetSamplesPerPitchRange(pitchRangeIndex, BlamCache.Platform);

                //
                // Convert Blam pitch range to ElDorado format
                //

                var pitchRange = BlamSoundGestalt.PitchRanges[pitchRangeIndex];

                result.PostConversionOperations.Add(() => pitchRange.ImportName = ConvertStringId(BlamSoundGestalt.ImportNames[pitchRange.ImportNameIndex].Name));
                pitchRange.PitchRangeParameters = BlamSoundGestalt.PitchRangeParameters[pitchRange.PitchRangeParametersIndex];
                pitchRange.RuntimePermutationFlags = -1;
                //I suspect this unknown7 to be a flag to tell if there is a Unknownblock in extrainfo. (See a sound in udlg for example)
                pitchRange.RuntimeDiscardedPermutationIndex = -1;
                pitchRange.PermutationCount = (byte)BlamSoundGestalt.GetPermutationCount(pitchRangeIndex, BlamCache.Platform);
                pitchRange.RuntimeLastPermutationIndex = -1;

                // Add pitch range to ED sound
                sound.PitchRanges.Add(pitchRange);
                var newPitchRangeIndex = pitchRangeIndex - sound.SoundReference.PitchRangeIndex;
                sound.PitchRanges[newPitchRangeIndex].Permutations = new List<Permutation>();

                //
                // Set compression format
                //

                var targetFormat = PortingOptions.Current.AudioCodec;
                sound.PlatformCodec.Compression = targetFormat;

                //
                // Convert Blam permutations to ElDorado format
                //

                var useCache = Sounds.UseAudioCacheCommand.AudioCacheDirectory != null;
                var soundCachePath = useCache ? Sounds.UseAudioCacheCommand.AudioCacheDirectory.FullName : "";


                var permutationCount = BlamSoundGestalt.GetPermutationCount(pitchRangeIndex, BlamCache.Platform);
                var permutationOrder = BlamSoundGestalt.GetPermutationOrder(pitchRangeIndex, BlamCache.Platform);
                var relativePitchRangeIndex = pitchRangeIndex - sound.SoundReference.PitchRangeIndex;

                for (int i = 0; i < permutationCount; i++)
                {
                    var permutationIndex = BlamSoundGestalt.GetFirstPermutationIndex(pitchRangeIndex, BlamCache.Platform) + i;

                    var permutation = BlamSoundGestalt.GetPermutation(permutationIndex).DeepClone();

                    result.PostConversionOperations.Add(() => permutation.ImportName = ConvertStringId(BlamSoundGestalt.ImportNames[permutation.ImportNameIndex].Name));
                    permutation.SkipFraction = permutation.EncodedSkipFraction / 32767.0f;
                    permutation.Gain = (float)permutation.EncodedGain;
                    permutation.PermutationChunks = new List<PermutationChunk>();
                    permutation.PermutationNumber = (uint)permutationOrder[i];
                    permutation.IsNotFirstPermutation = (uint)(permutation.PermutationNumber == 0 ? 0 : 1);

                    BlamSound blamSound = SoundConverter.ConvertGen3Sound(BlamCache, BlamSoundGestalt, sound, relativePitchRangeIndex, i, xmaData, targetFormat, useCache, soundCachePath, blamTag_Name);

                    byte[] permutationData = blamSound.Data;
                    permutation.SampleCount = blamSound.SampleCount;
                    permutation.FirstSample = blamSound.FirstSample;

                    // fixup dialog indices, might need more work
                    var firstPermutationChunk = BlamSoundGestalt.GetFirstPermutationChunk(permutationIndex);
                    var newChunk = new PermutationChunk(currentSoundDataOffset, permutationData.Length, firstPermutationChunk.LastSample, firstPermutationChunk.FirstSample);
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
                FacialAnimationInfo = new List<ExtraInfo.FacialAnimationInfoBlock>()
            };

            for (int u = 0; u < sound.SoundReference.PitchRangeCount; u++)
            {
                var pitchRange = BlamSoundGestalt.PitchRanges[sound.SoundReference.PitchRangeIndex + u];

                for (int i = 0; i < pitchRange.PermutationCount; i++)
                {
                    int permutationIndex = BlamSoundGestalt.GetFirstPermutationIndex(pitchRange, BlamCache.Platform) + i;
                    var permutation = BlamSoundGestalt.GetPermutation(permutationIndex);
                    var permutationChunk = BlamSoundGestalt.GetPermutationChunk(permutation.FirstPermutationChunkIndex);

                    extraInfo.LanguagePermutations.Add(new ExtraInfo.LanguagePermutation
                    {
                        RawInfo = new List<ExtraInfo.LanguagePermutation.RawInfoBlock>
                        {
                            new ExtraInfo.LanguagePermutation.RawInfoBlock
                            {
                                SkipFractionName = StringId.Invalid,
                                SeekTable = new List<ExtraInfo.LanguagePermutation.RawInfoBlock.SeekTableBlock>
                                {
                                    new ExtraInfo.LanguagePermutation.RawInfoBlock.SeekTableBlock
                                    {
                                        BlockRelativeSampleEnd = permutationChunk.LastSample,
                                        BlockRelativeSampleStart = permutationChunk.FirstSample,
                                        StartingSampleIndex = 0,
                                        EndingSampleIndex = permutation.SampleCount,
                                        StartingOffset = 0,
                                        EndingOffset = (uint)permutationChunk.EncodedSize & 0xFFFF
                                    }
                                },
                                Compression = 8,
                                ResourceSampleSize = pitchRange.Permutations[i].SampleCount,
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
                foreach (var section in BlamSoundGestalt.ExtraInfo[sound.SoundReference.ExtraInfoIndex].FacialAnimationInfo)
                {
                    var newSection = section.DeepClone();
                    extraInfo.FacialAnimationInfo.Add(newSection);
                }
            }

            sound.ExtraInfo = new List<ExtraInfo> { extraInfo };

            //
            // Convert Blam languages to ElDorado format
            //

            if (sound.SoundReference.LanguageIndex != -1)
            {
                if (BlamCache.Version < CacheVersion.HaloReach)
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

            result.Data = soundDataAggregate.ToArray();
            return result;
        }

        private PlaybackParameter ConvertPlaybackReach(PlaybackParameter playback)
        {
            // Fix playback parameters for reach

            playback.MaximumBendPerSecond = playback.MaximumBendPerSecondReach;
            playback.SkipFraction = playback.SkipFractionReach;

            playback.FieldDisableFlags = 0;

            if (playback.DistanceParameters.DontPlayDistance == 0)
                playback.FieldDisableFlags |= PlaybackParameter.FieldDisableFlagsValue.DistanceA;

            if (playback.DistanceParameters.AttackDistance == 0)
                playback.FieldDisableFlags |= PlaybackParameter.FieldDisableFlagsValue.DistanceB;

            if (playback.DistanceParameters.MinimumDistance == 0)
                playback.FieldDisableFlags |= PlaybackParameter.FieldDisableFlagsValue.DistanceC;

            if (playback.DistanceParameters.MaximumDistance == 0)
                playback.FieldDisableFlags |= PlaybackParameter.FieldDisableFlagsValue.DistanceD;

            playback.FieldDisableFlags |= PlaybackParameter.FieldDisableFlagsValue.Bit4;

            return playback;
        }

        private SoundLooping ConvertSoundLooping(SoundLooping soundLooping)
        {
            soundLooping.Unused = null;

            soundLooping.SoundClass = ((int)soundLooping.SoundClass < 50) ? soundLooping.SoundClass : (soundLooping.SoundClass + 1);

            if (soundLooping.SoundClass == SoundLooping.SoundClassValue.FirstPersonInside)
                soundLooping.SoundClass = SoundLooping.SoundClassValue.InsideSurroundTail;

            if (soundLooping.SoundClass == SoundLooping.SoundClassValue.FirstPersonOutside)
                soundLooping.SoundClass = SoundLooping.SoundClassValue.OutsideSurroundTail;

			if (BlamCache.Version == CacheVersion.Halo3Retail)
			{
				foreach (var track in soundLooping.Tracks)
				{
					// FadeMode was added in ODST, H3 uses InversePower for in sounds, and Power for out sounds
					track.FadeInMode = SoundLooping.Track.SoundFadeMode.None;
					track.FadeOutMode = SoundLooping.Track.SoundFadeMode.None;
					track.AlternateCrossfadeMode = SoundLooping.Track.SoundFadeMode.None;
					track.AlternateFadeOutMode = SoundLooping.Track.SoundFadeMode.None;
				}
			}
			else if (BlamCache.Version >= CacheVersion.HaloReach)
			{
				foreach (var track in soundLooping.Tracks)
				{
					track.Flags = GetEquivalentFlags(track.Flags, track.FlagsReach);
					track.OutputEffect = track.OutputEffectReach;
					track.FadeInDuration = track.FadeInDurationReach;
					track.FadeInMode = GetEquivalentValue(track.FadeInMode, track.FadeInModeReach);
					track.FadeOutDuration = track.FadeOutDurationReach;
					track.FadeOutMode = GetEquivalentValue(track.FadeOutMode, track.FadeOutModeReach);
					track.AlternateCrossfadeMode = GetEquivalentValue(track.AlternateCrossfadeMode, track.AlternateCrossfadeModeReach);
					track.AlternateFadeOutMode = GetEquivalentValue(track.AlternateFadeOutMode, track.AlternateFadeOutModeReach);
				}
			}

            return soundLooping;
        }

        private Dialogue ConvertDialogue(Stream cacheStream, Dialogue dialogue)
        {

            CachedTag edAdlg = null;
            AiDialogueGlobals adlg; ;
            foreach (var tag in CacheContext.TagCache.FindAllInGroup("adlg"))
            {
                edAdlg = tag;
                break;
            }

            adlg = CacheContext.Deserialize<AiDialogueGlobals>(cacheStream, edAdlg);

            //Create empty udlg vocalization block and fill it with empty blocks matching adlg

            List<Dialogue.SoundReference> newSoundReference = new List<Dialogue.SoundReference>();
            foreach (var soundreference in adlg.Vocalizations)
            {
                Dialogue.SoundReference block = new Dialogue.SoundReference
                {
                    Sound = null,
                    Flags = 0,
                    Vocalization = soundreference.Vocalization,
                };
                newSoundReference.Add(block);
            }

            //Match the tags with the proper stringId

            if(BlamCache.Version < CacheVersion.HaloReach)
            {
                for (int i = 0; i < 304; i++)
                {
                    var soundreference = newSoundReference[i];
                    for (int j = 0; j < dialogue.Vocalizations.Count; j++)
                    {
                        var soundreferenceH3 = dialogue.Vocalizations[j];
                        if (CacheContext.StringTable.GetString(soundreference.Vocalization).Equals(CacheContext.StringTable.GetString(soundreferenceH3.Vocalization)))
                        {
                            soundreference.Flags = soundreferenceH3.Flags;
                            soundreference.Sound = soundreferenceH3.Sound;
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 304; i++)
                {
                    var vocalization = newSoundReference[i];
                    for (int j = 0; j < dialogue.Vocalizations.Count; j++)
                    {
                        var vocalizationReach = dialogue.Vocalizations[j];
                        if (CacheContext.StringTable.GetString(vocalization.Vocalization).Equals(CacheContext.StringTable.GetString(vocalizationReach.Vocalization)))
                        {
                            // we use index 0 because other indices are for different situation like stealth. 
                            if(vocalizationReach.ReachSounds.Count > 0)
                                vocalization.Sound = vocalizationReach.ReachSounds[0].Sound;

                            break;
                        }
                    }
                }
            }

            

            dialogue.Vocalizations = newSoundReference;

            return dialogue;
        }

        private SoundMix ConvertSoundMix(SoundMix soundMix)
        {
            if (BlamCache.Version == CacheVersion.Halo3Retail)
                soundMix.GlobalMix.Unknown = 0;

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

                InternalFlags = (SoundClasses.Class.InternalFlagBits.Valid | SoundClasses.Class.InternalFlagBits.ValidXmaCompressionLevel |
                 SoundClasses.Class.InternalFlagBits.ValidDopplerFactor | SoundClasses.Class.InternalFlagBits.ValidObstructionFactor |
                 SoundClasses.Class.InternalFlagBits.ValidUnderwaterPropagation | SoundClasses.Class.InternalFlagBits.Bit9),

                Priority = 5,
                DistanceBounds = new Bounds<float>(8, 120),
                TransmissionMultiplier = 1.0f
            };

            // hud class, unique to HO. the above values seem to be okay
            if (sncl.Classes.Count >= 50)
                sncl.Classes.Insert(50, sClass);

            if (version <= CacheVersion.Halo3Retail)
            {
                foreach (var c in sncl.Classes)
                    c.CacheMissModeODST = (SoundClasses.Class.CacheMissModeODSTValue)c.CacheMissMode;

                // add classes missing from H3
                for (int i = sncl.Classes.Count; i < 65; i++)
                    sncl.Classes.Add(sClass);
            }

            // ms23 requires this flag to play a class on the mainmenu
            sncl.Classes[32].ClassFlags |= SoundClasses.Class.ExternalFlagBits.ClassPlaysOnMainmenu; // Music
            sncl.Classes[52].ClassFlags |= SoundClasses.Class.ExternalFlagBits.ClassPlaysOnMainmenu; // UI

            return sncl;
        }
    }
}