using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private ChudDefinition.HudWidget.StateDatum ConvertStateData(ChudDefinition.HudWidget.StateDatum stateData)
        {
            switch (BlamCache.Version)
            {
                case CacheVersion.Halo3Retail:
                    stateData.EngineFlags = GetEquivalentFlags(stateData.EngineFlags, stateData.EngineFlags_H3);
                    stateData.EngineGeneralFlags = GetEquivalentFlags(stateData.EngineGeneralFlags, stateData.EngineGeneralFlags_H3);
                    stateData.UnitBaseFlags = GetEquivalentFlags(stateData.UnitBaseFlags, stateData.UnitBaseFlags_H3);
                    stateData.Player_SpecialFlags = GetEquivalentFlags(stateData.Player_SpecialFlags, stateData.Player_SpecialFlags_H3);
                    break;

                case CacheVersion.Halo3ODST:
                    stateData.EngineFlags = GetEquivalentFlags(stateData.EngineFlags, stateData.EngineFlags_ODST);
                    stateData.UnitBaseFlags = GetEquivalentFlags(stateData.UnitBaseFlags, stateData.UnitBaseFlags_ODST);
                    break;
            }

            stateData.MultiplayerEventsFlags = GetEquivalentFlags(stateData.MultiplayerEventsFlags, stateData.MultiplayerEventsFlags_H3);
            stateData.GeneralKudosFlags = GetEquivalentFlags(stateData.GeneralKudosFlags, stateData.GeneralKudosFlags_H3);

            return stateData;
        }

        private ChudDefinition.HudWidget.RenderDatum ConvertRenderData(ChudDefinition.HudWidget.RenderDatum renderData)
        {
            //For some reason, the turbulence shader in H:O only works if the following unknown is 1.
            //So if the turbulance shader is selected, set the following unknown to 1.
            if (renderData.ShaderIndex == ChudDefinition.HudWidget.RenderDatum.ShaderIndexValue.Turbulence)
            {
                renderData.Unknown = 1;
            }

            switch (BlamCache.Version)
            {
                case CacheVersion.Halo3Retail:
                    renderData.Input_HO = GetEquivalentValue(renderData.Input_HO, renderData.Input_H3);
                    renderData.RangeInput_HO = GetEquivalentValue(renderData.RangeInput_HO, renderData.RangeInput_H3);
                    break;
                case CacheVersion.Halo3ODST:
                    renderData.Input_HO = GetEquivalentValue(renderData.Input_HO, renderData.Input_ODST);
                    renderData.RangeInput_HO = GetEquivalentValue(renderData.RangeInput_HO, renderData.RangeInput_ODST);
                    break;
                default:
                    break;
            }
            renderData.OutputColorA_HO = GetEquivalentValue(renderData.OutputColorA_HO, renderData.OutputColorA);
            renderData.OutputColorB_HO = GetEquivalentValue(renderData.OutputColorB_HO, renderData.OutputColorB);
            renderData.OutputColorC_HO = GetEquivalentValue(renderData.OutputColorC_HO, renderData.OutputColorC);
            renderData.OutputColorD_HO = GetEquivalentValue(renderData.OutputColorD_HO, renderData.OutputColorD);
            renderData.OutputColorE_HO = GetEquivalentValue(renderData.OutputColorE_HO, renderData.OutputColorE);
            renderData.OutputColorF_HO = GetEquivalentValue(renderData.OutputColorF_HO, renderData.OutputColorF);

            return renderData;
        }

        private ChudDefinition.HudWidget.TextWidget ConvertTextWidget(ChudDefinition.HudWidget.TextWidget textWidget)
        {
            switch (BlamCache.Version)
            {
                case CacheVersion.Halo3Retail:
                    textWidget.Flags = GetEquivalentFlags(textWidget.Flags, textWidget.Flags_H3);
                    break;
            }

            return textWidget;
        }

        private ChudDefinition ConvertChudDefinition(ChudDefinition chudDefinition)
        {
            for (int hudWidgetIndex = 0; hudWidgetIndex < chudDefinition.HudWidgets.Count; hudWidgetIndex++)
            {
                //get stringid text for patch targeting
                var widgetname = CacheContext.StringTable.GetString(chudDefinition.HudWidgets[hudWidgetIndex].Name);

                for (int stateDatumIndex = 0; stateDatumIndex < chudDefinition.HudWidgets[hudWidgetIndex].StateData.Count; stateDatumIndex++)
                    chudDefinition.HudWidgets[hudWidgetIndex].StateData[stateDatumIndex] = ConvertStateData(chudDefinition.HudWidgets[hudWidgetIndex].StateData[stateDatumIndex]);
                for (int renderDatumIndex = 0; renderDatumIndex < chudDefinition.HudWidgets[hudWidgetIndex].RenderData.Count; renderDatumIndex++)
                    chudDefinition.HudWidgets[hudWidgetIndex].RenderData[renderDatumIndex] = ConvertRenderData(chudDefinition.HudWidgets[hudWidgetIndex].RenderData[renderDatumIndex]);

                for (int bitmapWidgetIndex = 0; bitmapWidgetIndex < chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets.Count; bitmapWidgetIndex++)
                {
                    for (int stateDatumIndex = 0; stateDatumIndex < chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].StateData.Count; stateDatumIndex++)
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].StateData[stateDatumIndex] = ConvertStateData(chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].StateData[stateDatumIndex]);
                    for (int renderDatumIndex = 0; renderDatumIndex < chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].RenderData.Count; renderDatumIndex++)
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].RenderData[renderDatumIndex] = ConvertRenderData(chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].RenderData[renderDatumIndex]);

                    //get stringid text for patch targeting
                    var bitmapwidgetname = CacheContext.StringTable.GetString(chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].Name);

                    //fixup for waypoint light
                    if (bitmapwidgetname.Contains("waypoint_light"))
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].RenderData[0].OutputColorC_HO = ChudDefinition.HudWidget.RenderDatum.OutputColorValue_HO.LocalB;
                    }

                    //Fix some elements not displaying in forge mode
                    if (bitmapwidgetname.Contains("who_am_i") || bitmapwidgetname.Contains("summary_sandbox_title"))
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].StateData[0].EngineFlags_H3 |= ChudDefinition.HudWidget.StateDatum.Engine_H3.Editor;
                    }

                    //fix elite upper corner weirdness
                    if (bitmapwidgetname.Contains("upper_corners_720"))
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Scale.X /= 1.5f;
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Scale.Y /= 1.5f;
                    }

                    //fix spartan bottom HUD gap
                    if (bitmapwidgetname.Contains("middle_bar_720"))
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].MirrorOffset.X = 1.0179f;
                    }

                    //fix enemy scorebar spacing
                    if (bitmapwidgetname.Contains("enemy_area"))
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Offset.Y = 13.0f;
                    }

                    //odst HUD fixups
                    if (BlamCache.Version == CacheVersion.Halo3ODST)
                    {
                        //bump up size, HO distortion doesnt stretch it enough
                        if (widgetname.Contains("init_wipe"))
                        {
                            chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Scale.X *= 1.5f;
                            chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Scale.Y *= 1.5f;
                        }

                        if (widgetname.Contains("vitality_meter"))
                        {
                            chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].MirrorOffset.X = -1.01333f;
                        }

                        //fix odst bottom HUD gap
                        if (bitmapwidgetname.Contains("middle_bar_720"))
                        {
                            chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].MirrorOffset.X = 1.006253f;
                        }

                        //fix odst top HUD gap
                        if (bitmapwidgetname.Contains("center_720"))
                        {
                            chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].MirrorOffset.X = 1.01005f;
                        }

                        //remove beacon and waypoint (non-functional in HO)
                        if (bitmapwidgetname.Contains("beacon_") || bitmapwidgetname.Contains("user_placed_") || bitmapwidgetname.Contains("waypoint_light"))
                        {
                            chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Scale.X = 0.0f;
                            chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Scale.Y = 0.0f;
                        }

                        //fix binocular ruler scaling
                        if (bitmapwidgetname.Contains("vertical_ruler"))
                        {
                            chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Offset.X *= 1.5f;
                            chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Offset.Y *= 1.5f;
                            chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Scale.X *= 1.5f;
                            chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Scale.Y *= 1.5f;
                        }

                    }

                    //widget names of widgets whose bitmapwidgets need extra scaling
                    List<string> bitmappatchlist = new List<string>(){
                        "vitality_meter", "compass", "in_helmet" };

                    //fixup for widgets without global placement data and those that need extra scaling
                    if (chudDefinition.HudWidgets[hudWidgetIndex].PlacementData.Count == 0
                        || bitmappatchlist.Contains(widgetname))
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Offset.X *= 1.5f;
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Offset.Y *= 1.5f;
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Scale.X *= 1.5f;
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Scale.Y *= 1.5f;
                    }
                }

                for (int textWidgetIndex = 0; textWidgetIndex < chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets.Count; textWidgetIndex++)
                {
                    chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex] = ConvertTextWidget(chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex]);
                    for (int stateDatumIndex = 0; stateDatumIndex < chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].StateData.Count; stateDatumIndex++)
                        chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].StateData[stateDatumIndex] = ConvertStateData(chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].StateData[stateDatumIndex]);
                    for (int renderDatumIndex = 0; renderDatumIndex < chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].RenderData.Count; renderDatumIndex++)
                        chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].RenderData[renderDatumIndex] = ConvertRenderData(chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].RenderData[renderDatumIndex]);
                    //get stringid text for patch targeting
                    var textwidgetname = CacheContext.StringTable.GetString(chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].Name);

                    //fixup for 'budget' label
                    if (textwidgetname == "budget_meter_name")
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].PlacementData[0].Offset.X *= 1.5f;
                        chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].PlacementData[0].Offset.Y *= 1.5f;
                        chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].PlacementData[0].Scale.X *= 1.5f;
                        chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].PlacementData[0].Scale.Y *= 1.5f;
                    }

                    //fixup for widgets without global placement data
                    if (chudDefinition.HudWidgets[hudWidgetIndex].PlacementData.Count == 0)
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].PlacementData[0].Offset.X *= 1.5f;
                        chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].PlacementData[0].Offset.Y *= 1.5f;
                        chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].PlacementData[0].Scale.X *= 1.5f;
                        chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].PlacementData[0].Scale.Y *= 1.5f;
                    }
                }

                //scale all widget groups by 1.5 to match 720p > 1080p conversion
                for (int placementDatumIndex = 0; placementDatumIndex < chudDefinition.HudWidgets[hudWidgetIndex].PlacementData.Count; placementDatumIndex++)
                {
                    chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[placementDatumIndex].Scale.X *= 1.5f;
                    chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[placementDatumIndex].Scale.Y *= 1.5f;
                    chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[placementDatumIndex].Offset.X *= 1.5f;
                    chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[placementDatumIndex].Offset.Y *= 1.5f;
                }

                //hide odst hud lines when in vehicle/turret
                if (BlamCache.Version == CacheVersion.Halo3ODST && (widgetname.Contains("in_helmet_bottom") || widgetname.Contains("in_helmet_top")))
                {
                    chudDefinition.HudWidgets[hudWidgetIndex].StateData[0].UnitGeneralFlags = ChudDefinition.HudWidget.StateDatum.UnitGeneral.ThirdPersonCamera;
                }

                // reposition h3 metagame widgets (anchors changed in ODST)
                if (BlamCache.Version == CacheVersion.Halo3Retail)
                {
                    if (widgetname == "top_bar")
                    { 
                        chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[0].Offset.Y = -90;
                        chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[0].Anchor = ChudDefinition.HudWidget.PlacementDatum.AnchorValue.BottomRight;
                    }
                    if (widgetname == "p1_bar")
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[0].Offset.Y = -40;
                        chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[0].Anchor = ChudDefinition.HudWidget.PlacementDatum.AnchorValue.BottomRight;
                    }
                    if (widgetname == "p2_bar")
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[0].Offset.Y = 13;
                        chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[0].Anchor = ChudDefinition.HudWidget.PlacementDatum.AnchorValue.BottomRight;
                    }
                    if (widgetname == "p3_bar")
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[0].Offset.Y = 13;
                        chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[0].Anchor = ChudDefinition.HudWidget.PlacementDatum.AnchorValue.BottomRight;
                    }
                    if (widgetname == "p4_bar")
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[0].Offset.Y = 13;
                        chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[0].Anchor = ChudDefinition.HudWidget.PlacementDatum.AnchorValue.BottomRight;
                    }
                }
            }
            return chudDefinition;
        }

        private ChudGlobalsDefinition ConvertChudGlobalsDefinition(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, ChudGlobalsDefinition H3Definition)
        {
            for (int hudGlobalsIndex = 0; hudGlobalsIndex < H3Definition.HudGlobals.Count; hudGlobalsIndex++)
            {
                var H3globs = H3Definition.HudGlobals[hudGlobalsIndex];

                //fixups
                H3globs.GrenadeScematicsSpacing = 1.5f * H3globs.GrenadeScematicsSpacing;

                for (int hudAttributesIndex = 0; hudAttributesIndex < H3Definition.HudGlobals[hudGlobalsIndex].HudAttributes.Count; hudAttributesIndex++)
                {
                    var H3att = H3Definition.HudGlobals[hudGlobalsIndex].HudAttributes[hudAttributesIndex];

                    if (BlamCache.Version <= CacheVersion.Halo3Retail)
                    {
                        H3att.HorizontalScale = 1.0f;
                        H3att.VerticalScale = 1.0f;
                    }

                    //more fixups
                    H3att.ResolutionWidth = (uint)(H3att.ResolutionWidth * 1.5f);
                    H3att.ResolutionHeight = (uint)(H3att.ResolutionHeight * 1.5f);
                    H3att.MotionSensorOffset.X *= 1.5f;
                    H3att.MotionSensorOffset.Y *= 1.5f;
                    H3att.MotionSensorRadius *= 1.5f;
                    H3att.MotionSensorScale *= 1.5f;
                    H3att.StateLeftRightOffset_HO.Y = H3att.StateLeftRightOffsetY_H3;
                    H3att.StateScale_HO = H3att.StateScale;
                    H3att.NotificationOffsetY_HO = H3att.NotificationOffsetY_H3;
                    H3att.NotificationOffsetX_HO = H3att.NotificationOffsetX_H3;
                    H3att.NotificationScale *= 1.5f;
                    H3att.NotificationLineSpacing *= 1.5f;

                    if (BlamCache.Version == CacheVersion.Halo3ODST)
                    {
                        H3att.WarpAngle = Angle.FromDegrees(4.5f);
                        H3att.WarpAmount = 0.1f;
                        H3att.StateLeftRightOffset_HO.Y = 0.2f; // 0.2 due to odsts 0.87 hud scale
                    }
                }


                for (int hudSoundsIndex = 0; hudSoundsIndex < H3Definition.HudGlobals[hudGlobalsIndex].HudSounds.Count; hudSoundsIndex++)
                {
                    var H3snd = H3Definition.HudGlobals[hudGlobalsIndex].HudSounds[hudSoundsIndex];

                    if (BlamCache.Version == CacheVersion.Halo3Retail)
                    {
                        H3snd.LatchedTo = GetEquivalentFlags(H3snd.LatchedTo, H3snd.LatchedTo_H3);

                        H3snd.Bipeds = new List<ChudGlobalsDefinition.HudGlobal.HudSound.BipedData>();

                        if (H3snd.SpartanSound != null)
                        {
                            var spartanBiped = new ChudGlobalsDefinition.HudGlobal.HudSound.BipedData();
                            spartanBiped.BipedType_HO = ChudGlobalsDefinition.HudGlobal.HudSound.BipedData.BipedTypeValue_HO.Spartan;
                            spartanBiped.Sound = (CachedTag)ConvertData(cacheStream, blamCacheStream, resourceStreams, H3snd.SpartanSound, null, H3snd.SpartanSound.Name);
                            H3snd.Bipeds.Add(spartanBiped);
                        }
                        if (H3snd.EliteSound != null)
                        {
                            var eliteBiped = new ChudGlobalsDefinition.HudGlobal.HudSound.BipedData();
                            eliteBiped.BipedType_HO = ChudGlobalsDefinition.HudGlobal.HudSound.BipedData.BipedTypeValue_HO.Elite;
                            eliteBiped.Sound = (CachedTag)ConvertData(cacheStream, blamCacheStream, resourceStreams, H3snd.EliteSound, null, H3snd.EliteSound.Name);
                            H3snd.Bipeds.Add(eliteBiped);
                        }
                    }

                    else if (BlamCache.Version == CacheVersion.Halo3ODST)
                    {
                        for (int bipedIndex = 0; bipedIndex < H3snd.Bipeds.Count; bipedIndex++)
                        {
                            if (H3snd.Bipeds[bipedIndex].BipedType_ODST == ChudGlobalsDefinition.HudGlobal.HudSound.BipedData.BipedTypeValue_ODST.Rookie
                                || H3snd.Bipeds[bipedIndex].BipedType_ODST == ChudGlobalsDefinition.HudGlobal.HudSound.BipedData.BipedTypeValue_ODST.Any)
                            {
                                H3snd.Bipeds[bipedIndex].BipedType_HO = ChudGlobalsDefinition.HudGlobal.HudSound.BipedData.BipedTypeValue_HO.Spartan;
                            }
                            else
                            {
                                H3snd.Bipeds.RemoveAt(bipedIndex);
                                //indexes are shifted left by one because of this removal
                                bipedIndex -= 1;
                            }
                        }
                    }

                }

            }

            //additional values
            if (BlamCache.Version <= CacheVersion.Halo3Retail)
            {
                H3Definition.ShieldMinorThreshold = 1.0f;
                H3Definition.ShieldMajorThreshold = 0.25f;
                H3Definition.ShieldCriticalThreshold = 0.0f;
                H3Definition.HealthMinorThreshold = 0.9f;
                H3Definition.HealthMajorThreshold = 0.75f;
                H3Definition.HealthCriticalThreshold = 0.5f;
            }

            //prevent crash when porting from odst
            if (BlamCache.Version == CacheVersion.Halo3ODST)
            {
                try
                {
                    H3Definition.MotionSensorBlip = CacheContext.TagCache.GetTag<Bitmap>(@"ui\chud\bitmaps\sensor_blips");
                }
                catch
                {
                    Console.WriteLine("WARNING: Motion sensor bitmap 'ui\\chud\\bitmaps\\sensor_blips' not found.");
                }
            }

            //metagame values
            H3Definition.CampaignMedalScale = 1.5f;
            H3Definition.CampaignMedalSpacing = 60.0f;

            if (BlamCache.Version <= CacheVersion.Halo3Retail)
            {
                // H3 metagame fixups
                H3Definition.CampaignMedalSpacing = 47.0f;
                H3Definition.CampaignMedalChudAnchor = ChudDefinition.HudWidget.PlacementDatum.AnchorValue.BottomRight;
                H3Definition.CampaignMedalOffsetX = -290;
                H3Definition.CampaignMedalOffsetY = -113;
                H3Definition.MetagameScoreboardTopY = 0;
                H3Definition.MetagameScoreboardSpacing = 0;
            }

            //upscale blip bitmap sizes
            H3Definition.LargeSensorBlipScale *= 1.5f;
            H3Definition.MediumSensorBlipScale *= 1.5f;
            H3Definition.SmallSensorBlipScale *= 1.5f;
            H3Definition.SensorBlipGlowRadius *= 1.5f;

            //prevent crash?
            H3Definition.AchievementDisplayTime = 3.0f;

            return H3Definition;
        }

        /// <summary>
        /// Adds shared values from flags2 to flags1.
        /// </summary>
        private E1 GetEquivalentFlags<E1, E2>(E1 flags1, E2 flags2) where E1 : unmanaged, Enum where E2 : unmanaged, Enum
        {
            //Get enum values
            E1[] flagsType1Values = (E1[])Enum.GetValues(typeof(E1));
            E2[] flagsType2Values = (E2[])Enum.GetValues(typeof(E2));

            //Seperate flags for individual comparison.
            List<E2> flags2Values = new List<E2>();

            foreach (E2 flagsType2Value in flagsType2Values)
            {
                //Ignore the None value.
                if (flagsType2Value.ToString() == "None")
                    continue;

                if ((flags2 as Enum).HasFlag((flagsType2Value as Enum)))
                {
                    flags2Values.Add(flagsType2Value);
                }
            }

            //Compare
            int flags1Int = 0;
            foreach (E2 flag2 in flags2Values)
            {
                string flag2String = flag2.ToString();
                var flags1EnumType = Nullable.GetUnderlyingType(typeof(E1));
                try
                {
                    E1 flag1 = (E1)Enum.Parse(typeof(E1), flag2String);
                    flags1Int |= (int)Convert.ChangeType(flag1, typeof(int));
                }
                catch (ArgumentException)
                {
                    continue;
                }
            }
            return (E1)(Enum.ToObject(typeof(E1), flags1Int));
        }

        /// <summary>
        /// If the value of enum1 has a value with the same name as the value of enum2, return it.
        /// </summary>
        private E1 GetEquivalentValue<E1, E2>(E1 enum1, E2 enum2) where E1 : unmanaged, Enum where E2 : unmanaged, Enum
        {

            //Get enum values
            var enumType1Values = Enum.GetValues(typeof(E1)) as E1[];
            var enumType2Values = Enum.GetValues(typeof(E2)) as E2[];

            //Compare
            string enum2String = enum2.ToString();

            try
            {
                return (E1)Enum.Parse(typeof(E1), enum2String);
            }
            catch (Exception)
            {
                return enum1;
            }
        }
    }
}