using System.Collections.Generic;
using System.IO;
using TagTool.Ai;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private Globals ConvertGlobals(Globals matg, Stream cacheStream)
        {
            //Add aigl from H3

            if (BlamCache.Version == CacheVersion.Halo3Retail)
            {
                AiGlobals aigl = new AiGlobals
                {
                    Data = new List<AiGlobalsDatum>(),
                };
                foreach (var value in matg.AiGlobalsOld)
                {
                    value.SearchRangeInfantry = 30;
                    value.SearchRangeFlying = 40;
                    value.SearchRangeVehicle = 40;
                    value.SearchRangeGiant = 200;

                    //Something may need to be done about squad and formation tags

                    aigl.Data.Add(value);
                }

                CachedTagInstance edTag = CacheContext.TagCache.AllocateTag(TagGroup.Instances[new Tag("aigl")]);
                edTag.Name = "globals\ai_globals";
                CacheContext.Serialize(cacheStream, edTag, aigl);
                matg.AiGlobals = edTag;
                matg.AiGlobalsOld = new List<AiGlobalsDatum>();
            }

            //Might require adding the GfxUiStrings block

            if (BlamCache.Version == CacheVersion.Halo3Retail)
            {
                matg.Unknown60 = new List<Globals.UnknownBlock>
                {
                    new Globals.UnknownBlock
                    {
                        Unknown1 = 100,
                        Unknown2 = 1,
                        Unknown3 = 1
                    }
                };

                foreach (var metagame in matg.MetagameGlobals)
                {
                    //Medal values for H3 do not need to be modified, but if survival is introduced on an H3 port, it will require ODST or HO points

                    metagame.FirstWeaponSpree = 5;
                    metagame.SecondWeaponSpree = 10;
                    metagame.KillingSpree = 10;
                    metagame.KillingFrenzy = 20;
                    metagame.RunningRiot = 30;
                    metagame.Rampage = 40;
                    metagame.Untouchable = 50;
                    metagame.Invincible = 100;
                    metagame.DoubleKill = 2;
                    metagame.TripleKill = 3;
                    metagame.Overkill = 4;
                    metagame.Killtacular = 5;
                    metagame.Killtrocity = 6;
                    metagame.Killimanjaro = 7;
                    metagame.Killtastrophe = 8;
                    metagame.Killpocalypse = 9;
                    metagame.Killionaire = 10;
                }
            }

            return matg;
        }

        private void MergeMultiplayerEvent(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, MultiplayerGlobals.RuntimeBlock.EventBlock edEvent, MultiplayerGlobals.RuntimeBlock.EventBlock h3Event)
        {
            if (h3Event.EnglishSound != null)
                edEvent.EnglishSound = ConvertTag(cacheStream, resourceStreams, BlamCache.GetIndexItemFromID(h3Event.EnglishSound.Index));

            if (h3Event.JapaneseSound != null)
                edEvent.JapaneseSound = ConvertTag(cacheStream, resourceStreams, BlamCache.GetIndexItemFromID(h3Event.JapaneseSound.Index));

            if (h3Event.GermanSound != null)
                edEvent.GermanSound = ConvertTag(cacheStream, resourceStreams, BlamCache.GetIndexItemFromID(h3Event.GermanSound.Index));

            if (h3Event.FrenchSound != null)
                edEvent.FrenchSound = ConvertTag(cacheStream, resourceStreams, BlamCache.GetIndexItemFromID(h3Event.FrenchSound.Index));

            if (h3Event.SpanishSound != null)
                edEvent.SpanishSound = ConvertTag(cacheStream, resourceStreams, BlamCache.GetIndexItemFromID(h3Event.SpanishSound.Index));

            if (h3Event.LatinAmericanSpanishSound != null)
                edEvent.LatinAmericanSpanishSound = ConvertTag(cacheStream, resourceStreams, BlamCache.GetIndexItemFromID(h3Event.LatinAmericanSpanishSound.Index));

            if (h3Event.ItalianSound != null)
                edEvent.ItalianSound = ConvertTag(cacheStream, resourceStreams, BlamCache.GetIndexItemFromID(h3Event.ItalianSound.Index));

            if (h3Event.KoreanSound != null)
                edEvent.KoreanSound = ConvertTag(cacheStream, resourceStreams, BlamCache.GetIndexItemFromID(h3Event.KoreanSound.Index));

            if (h3Event.ChineseTraditionalSound != null)
                edEvent.ChineseTraditionalSound = ConvertTag(cacheStream, resourceStreams, BlamCache.GetIndexItemFromID(h3Event.ChineseTraditionalSound.Index));

            if (h3Event.ChineseSimplifiedSound != null)
                edEvent.ChineseSimplifiedSound = ConvertTag(cacheStream, resourceStreams, BlamCache.GetIndexItemFromID(h3Event.ChineseSimplifiedSound.Index));

            if (h3Event.PortugueseSound != null)
                edEvent.PortugueseSound = ConvertTag(cacheStream, resourceStreams, BlamCache.GetIndexItemFromID(h3Event.PortugueseSound.Index));

            if (h3Event.PolishSound != null)
                edEvent.PolishSound = ConvertTag(cacheStream, resourceStreams, BlamCache.GetIndexItemFromID(h3Event.PolishSound.Index));
        }

        private void MergeMultiplayerGlobals(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, CachedTagInstance edTag, CacheFile.IndexItem h3Tag)
        {
            var edDef = CacheContext.Deserialize<MultiplayerGlobals>(cacheStream, edTag);

            var h3Def = BlamCache.Deserializer.Deserialize<MultiplayerGlobals>(
                new CacheSerializationContext(ref BlamCache, h3Tag));

            if (h3Def.Runtime == null || h3Def.Runtime.Count == 0)
                return;

            for (var i = 0; i < h3Def.Runtime[0].GeneralEvents.Count; i++)
            {
                var h3Event = h3Def.Runtime[0].GeneralEvents[i];
                var h3String = CacheContext.GetString(ConvertStringId(h3Event.DisplayString));

                for (var j = 0; j < edDef.Runtime[0].GeneralEvents.Count; j++)
                {
                    var edEvent = edDef.Runtime[0].GeneralEvents[j];
                    var edString = CacheContext.GetString(edEvent.DisplayString);

                    if (edString == h3String)
                        MergeMultiplayerEvent(cacheStream, resourceStreams, edEvent, h3Event);
                }
            }

            for (var i = 0; i < h3Def.Runtime[0].FlavorEvents.Count; i++)
            {
                var h3Event = h3Def.Runtime[0].FlavorEvents[i];
                var h3String = CacheContext.GetString(ConvertStringId(h3Event.DisplayString));

                for (var j = 0; j < edDef.Runtime[0].FlavorEvents.Count; j++)
                {
                    var edEvent = edDef.Runtime[0].FlavorEvents[j];
                    var edString = CacheContext.GetString(edEvent.DisplayString);

                    if (edString == h3String)
                        MergeMultiplayerEvent(cacheStream, resourceStreams, edEvent, h3Event);
                }
            }

            for (var i = 0; i < h3Def.Runtime[0].SlayerEvents.Count; i++)
            {
                var h3Event = h3Def.Runtime[0].SlayerEvents[i];
                var h3String = CacheContext.GetString(ConvertStringId(h3Event.DisplayString));

                for (var j = 0; j < edDef.Runtime[0].SlayerEvents.Count; j++)
                {
                    var edEvent = edDef.Runtime[0].SlayerEvents[j];
                    var edString = CacheContext.GetString(edEvent.DisplayString);

                    if (edString == h3String)
                        MergeMultiplayerEvent(cacheStream, resourceStreams, edEvent, h3Event);
                }
            }

            for (var i = 0; i < h3Def.Runtime[0].CtfEvents.Count; i++)
            {
                var h3Event = h3Def.Runtime[0].CtfEvents[i];
                var h3String = CacheContext.GetString(ConvertStringId(h3Event.DisplayString));

                for (var j = 0; j < edDef.Runtime[0].CtfEvents.Count; j++)
                {
                    var edEvent = edDef.Runtime[0].CtfEvents[j];
                    var edString = CacheContext.GetString(edEvent.DisplayString);

                    if (edString == h3String)
                        MergeMultiplayerEvent(cacheStream, resourceStreams, edEvent, h3Event);
                }
            }

            for (var i = 0; i < h3Def.Runtime[0].OddballEvents.Count; i++)
            {
                var h3Event = h3Def.Runtime[0].OddballEvents[i];
                var h3String = CacheContext.GetString(ConvertStringId(h3Event.DisplayString));

                for (var j = 0; j < edDef.Runtime[0].OddballEvents.Count; j++)
                {
                    var edEvent = edDef.Runtime[0].OddballEvents[j];
                    var edString = CacheContext.GetString(edEvent.DisplayString);

                    if (edString == h3String)
                        MergeMultiplayerEvent(cacheStream, resourceStreams, edEvent, h3Event);
                }
            }

            for (var i = 0; i < h3Def.Runtime[0].KingOfTheHillEvents.Count; i++)
            {
                var h3Event = h3Def.Runtime[0].KingOfTheHillEvents[i];
                var h3String = CacheContext.GetString(ConvertStringId(h3Event.DisplayString));

                for (var j = 0; j < edDef.Runtime[0].KingOfTheHillEvents.Count; j++)
                {
                    var edEvent = edDef.Runtime[0].KingOfTheHillEvents[j];
                    var edString = CacheContext.GetString(edEvent.DisplayString);

                    if (edString == h3String)
                        MergeMultiplayerEvent(cacheStream, resourceStreams, edEvent, h3Event);
                }
            }

            for (var i = 0; i < h3Def.Runtime[0].VipEvents.Count; i++)
            {
                var h3Event = h3Def.Runtime[0].VipEvents[i];
                var h3String = CacheContext.GetString(ConvertStringId(h3Event.DisplayString));

                for (var j = 0; j < edDef.Runtime[0].VipEvents.Count; j++)
                {
                    var edEvent = edDef.Runtime[0].VipEvents[j];
                    var edString = CacheContext.GetString(edEvent.DisplayString);

                    if (edString == h3String)
                        MergeMultiplayerEvent(cacheStream, resourceStreams, edEvent, h3Event);
                }
            }

            for (var i = 0; i < h3Def.Runtime[0].JuggernautEvents.Count; i++)
            {
                var h3Event = h3Def.Runtime[0].JuggernautEvents[i];
                var h3String = CacheContext.GetString(ConvertStringId(h3Event.DisplayString));

                for (var j = 0; j < edDef.Runtime[0].JuggernautEvents.Count; j++)
                {
                    var edEvent = edDef.Runtime[0].JuggernautEvents[j];
                    var edString = CacheContext.GetString(edEvent.DisplayString);

                    if (edString == h3String)
                        MergeMultiplayerEvent(cacheStream, resourceStreams, edEvent, h3Event);
                }
            }

            for (var i = 0; i < h3Def.Runtime[0].TerritoriesEvents.Count; i++)
            {
                var h3Event = h3Def.Runtime[0].TerritoriesEvents[i];
                var h3String = CacheContext.GetString(ConvertStringId(h3Event.DisplayString));

                for (var j = 0; j < edDef.Runtime[0].TerritoriesEvents.Count; j++)
                {
                    var edEvent = edDef.Runtime[0].TerritoriesEvents[j];
                    var edString = CacheContext.GetString(edEvent.DisplayString);

                    if (edString == h3String)
                        MergeMultiplayerEvent(cacheStream, resourceStreams, edEvent, h3Event);
                }
            }

            for (var i = 0; i < h3Def.Runtime[0].AssaultEvents.Count; i++)
            {
                var h3Event = h3Def.Runtime[0].AssaultEvents[i];
                var h3String = CacheContext.GetString(ConvertStringId(h3Event.DisplayString));

                for (var j = 0; j < edDef.Runtime[0].AssaultEvents.Count; j++)
                {
                    var edEvent = edDef.Runtime[0].AssaultEvents[j];
                    var edString = CacheContext.GetString(edEvent.DisplayString);

                    if (edString == h3String)
                        MergeMultiplayerEvent(cacheStream, resourceStreams, edEvent, h3Event);
                }
            }

            for (var i = 0; i < h3Def.Runtime[0].InfectionEvents.Count; i++)
            {
                var h3Event = h3Def.Runtime[0].InfectionEvents[i];
                var h3String = CacheContext.GetString(ConvertStringId(h3Event.DisplayString));

                for (var j = 0; j < edDef.Runtime[0].InfectionEvents.Count; j++)
                {
                    var edEvent = edDef.Runtime[0].InfectionEvents[j];
                    var edString = CacheContext.GetString(edEvent.DisplayString);

                    if (edString == h3String)
                        MergeMultiplayerEvent(cacheStream, resourceStreams, edEvent, h3Event);
                }
            }
        }
    }
}