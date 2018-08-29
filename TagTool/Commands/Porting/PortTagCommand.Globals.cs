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

            if(BlamCache.Version == CacheVersion.Halo3Retail)
            {
                AiGlobals aigl = new AiGlobals
                {
                    Data = new List<AiGlobalsDatum>(),
                };
                foreach(var value in matg.AiGlobalsOld)
                {
                    value.SearchRangeInfantry = 30;
                    value.SearchRangeFlying = 40;
                    value.SearchRangeVehicle = 40;
                    value.SearchRangeGiant = 200;

                    //Something may need to be done about squad and formation tags

                    aigl.Data.Add(value);
                }

                CachedTagInstance edTag = CacheContext.TagCache.AllocateTag(TagGroup.Instances[new Tag("aigl")]);
                CacheContext.TagNames[edTag.Index] = "globals\ai_globals";
                CacheContext.Serialize(cacheStream, edTag, aigl);
                matg.AiGlobals = edTag;
                matg.AiGlobalsOld = new List<AiGlobalsDatum>();
            }

            //Might require adding the GfxUiStrings block

            if(BlamCache.Version == CacheVersion.Halo3Retail)
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
    }
}