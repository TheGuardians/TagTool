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

            renderData.LocalColorA = ConvertColor(renderData.LocalColorA);
            renderData.LocalColorB = ConvertColor(renderData.LocalColorB);
            renderData.LocalColorC = ConvertColor(renderData.LocalColorC);
            renderData.LocalColorD = ConvertColor(renderData.LocalColorD);
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
                var widgetname = CacheContext.GetString(chudDefinition.HudWidgets[hudWidgetIndex].Name);

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
                    var bitmapwidgetname = CacheContext.GetString(chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].Name);

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

                    //widget names of widgets whose bitmapwidgets need extra scaling
                    List<string> bitmappatchlist = new List<string>(){
                        "vitality_meter", "compass", "in_helmet" };

                    //crosshair widget names
                    List<string> crosshairlist = new List<string>(){
                        "crosshair_fullscreen", "turret_crosshair", "crosshair_halfscreen", "crosshair", "unarmed_crosshair", "crosshair_default", "default_crosshair", "crosshair_active", "active_crosshairv", "crosshair_holding", "holding_crosshair", "crosshair_no", "no_crosshair", "crosshair_center", "ar_crosshair", "l_crosshair", "r_crosshair", "b_crosshair", "t_crosshair", "br_crosshair", "bs_crosshair", "crosshair_split", "scope_crosshair1", "scope_crosshair2", "crosshair_left_right", "crosshair_up_down", "scope_crosshairs1", "scope_crosshairs2", "scope_crosshairs3", "scope_crosshairs4", "horizontal_crosshairs", "vertical_crosshairs", "pr_crosshair", "shotty_crosshair", "h_crosshairs", "v_crosshair", "excavator_crosshair", "needler_crosshair", "crosshair_red", "crosshair_locked_flash", "fr_crosshair", "beam_crosshair", "unzoomed_crosshair", "zoomed_crosshair", "outer_crosshair", "flame_crosshair", "flamethrower_crosshair" };

                    //fixup for widgets without global placement data and those that need extra scaling
                    if (chudDefinition.HudWidgets[hudWidgetIndex].PlacementData.Count == 0
                        || bitmappatchlist.Contains(widgetname))
                    {
                        if (!crosshairlist.Contains(widgetname))
                        {
                            chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Offset.X *= 1.5f;
                            chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Offset.Y *= 1.5f;
                            chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Scale.X *= 1.5f;
                            chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Scale.Y *= 1.5f;
                        }
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
                    var textwidgetname = CacheContext.GetString(chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].Name);

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

                    //fixup for black infinite ammo counters
                    if ((textwidgetname == "ammo_count") || (textwidgetname == "battery_life"))
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].RenderData[0].ShaderIndex = ChudDefinition.HudWidget.RenderDatum.ShaderIndexValue.Crosshair;
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
            }
            return chudDefinition;
        }

        private ChudGlobalsDefinition ConvertChudGlobalsDefinition(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, ChudGlobalsDefinition H3Definition)
        {
            for (int hudGlobalsIndex = 0; hudGlobalsIndex < H3Definition.HudGlobals.Count; hudGlobalsIndex++)
            {
                var H3globs = H3Definition.HudGlobals[hudGlobalsIndex];

                //Color Conversion
                H3globs.HUDDisabled = ConvertColor(H3globs.HUDDisabled);
                H3globs.HUDPrimary = ConvertColor(H3globs.HUDPrimary);
                H3globs.HUDForeground = ConvertColor(H3globs.HUDForeground);
                H3globs.HUDWarning = ConvertColor(H3globs.HUDWarning);
                H3globs.NeutralReticule = ConvertColor(H3globs.NeutralReticule);
                H3globs.HostileReticule = ConvertColor(H3globs.HostileReticule);
                H3globs.FriendlyReticule = ConvertColor(H3globs.FriendlyReticule);
                H3globs.GlobalDynamic7_UnknownBlip = ConvertColor(H3globs.GlobalDynamic7_UnknownBlip);
                H3globs.NeutralBlip = ConvertColor(H3globs.NeutralBlip);
                H3globs.HostileBlip = ConvertColor(H3globs.HostileBlip);
                H3globs.FriendlyPlayerBlip = ConvertColor(H3globs.FriendlyPlayerBlip);
                H3globs.FriendlyAIBlip = ConvertColor(H3globs.FriendlyAIBlip);
                H3globs.GlobalDynamic12 = ConvertColor(H3globs.GlobalDynamic12);
                H3globs.WaypointBlip = ConvertColor(H3globs.WaypointBlip);
                H3globs.DistantWaypointBlip = ConvertColor(H3globs.DistantWaypointBlip);
                H3globs.FriendlyWaypoint = ConvertColor(H3globs.FriendlyWaypoint);
                H3globs.GlobalDynamic16 = ConvertColor(H3globs.GlobalDynamic16);
                H3globs.GlobalDynamic17 = ConvertColor(H3globs.GlobalDynamic17);
                H3globs.GlobalDynamic18 = ConvertColor(H3globs.GlobalDynamic18);
                H3globs.GlobalDynamic19 = ConvertColor(H3globs.GlobalDynamic19);
                H3globs.GlobalDynamic20 = ConvertColor(H3globs.GlobalDynamic20);
                H3globs.GlobalDynamic21 = ConvertColor(H3globs.GlobalDynamic21);
                H3globs.GlobalDynamic22 = ConvertColor(H3globs.GlobalDynamic22);
                H3globs.GlobalDynamic23 = ConvertColor(H3globs.GlobalDynamic23);
                H3globs.GlobalDynamic24 = ConvertColor(H3globs.GlobalDynamic24);
                H3globs.GlobalDynamic25 = ConvertColor(H3globs.GlobalDynamic25);
                H3globs.GlobalDynamic26 = ConvertColor(H3globs.GlobalDynamic26);
                H3globs.GlobalDynamic27 = ConvertColor(H3globs.GlobalDynamic27);


                //fixups
                H3globs.GrenadeScematicsSpacing = 1.5f * H3globs.GrenadeScematicsSpacing;

                for (int hudAttributesIndex = 0; hudAttributesIndex < H3Definition.HudGlobals[hudGlobalsIndex].HudAttributes.Count; hudAttributesIndex++)
                {
                    var H3att = H3Definition.HudGlobals[hudGlobalsIndex].HudAttributes[hudAttributesIndex];

                    if (BlamCache.Version == CacheVersion.Halo3ODST)
                    {
                        H3att.NotificationOffsetX_HO = H3att.NotificationOffsetX_H3;
                        H3att.WarpAngle = Angle.FromDegrees(4.5f);
                        H3att.WarpAmount = 0.1f;
                    }

                    //more fixups
                    H3att.ResolutionWidth = (uint)(H3att.ResolutionWidth * 1.5f);
                    H3att.ResolutionHeight = (uint)(H3att.ResolutionHeight * 1.5f);
                    H3att.MotionSensorOffset.X *= 1.5f;
                    H3att.MotionSensorOffset.Y *= 1.5f;
                    H3att.MotionSensorRadius *= 1.5f;
                    H3att.MotionSensorScale *= 1.5f;
                    H3att.HorizontalScale = 1.0f;
                    H3att.VerticalScale = 1.0f;
                    H3att.PickupDialogOffset.Y = 0.3f;
                    H3att.PickupDialogScale = 1.5f;
                    H3att.NotificationOffsetY_HO = H3att.NotificationOffsetY_H3;
                    H3att.NotificationOffsetX_HO = H3att.NotificationOffsetX_H3;
                    H3att.NotificationScale *= 1.5f;
                    H3att.NotificationLineSpacing *= 1.5f;
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
                            spartanBiped.Sound = (CachedTagInstance)ConvertData(cacheStream, resourceStreams, H3snd.SpartanSound, null, H3snd.SpartanSound.Name);
                            H3snd.Bipeds.Add(spartanBiped);
                        }
                        if (H3snd.EliteSound != null)
                        {
                            var eliteBiped = new ChudGlobalsDefinition.HudGlobal.HudSound.BipedData();
                            eliteBiped.BipedType_HO = ChudGlobalsDefinition.HudGlobal.HudSound.BipedData.BipedTypeValue_HO.Elite;
                            eliteBiped.Sound = (CachedTagInstance)ConvertData(cacheStream, resourceStreams, H3snd.EliteSound, null, H3snd.EliteSound.Name);
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
            H3Definition.Unknown5 = 1.8f;
            H3Definition.ShieldMinorThreshold = 1.0f;
            H3Definition.ShieldMajorThreshold = 0.5f;
            H3Definition.ShieldCriticalThreshold = 0.25f;
            H3Definition.HealthMinorThreshold = 0.9f;
            H3Definition.HealthMajorThreshold = 0.75f;
            H3Definition.HealthCriticalThreshold = 0.5f;

            //upscale blip bitmap sizes
            H3Definition.LargeSensorBlipScale *= 1.5f;
            H3Definition.MediumSensorBlipScale *= 1.5f;
            H3Definition.SmallSensorBlipScale *= 1.5f;
            H3Definition.SensorBlipGlowRadius *= 1.5f;

            //prevent crash?
            H3Definition.Unknown72 = 3.0f;

            return H3Definition;
        }

        private ArgbColor ConvertColor(ArgbColor oldcolor)
        {
            var newcolor = new ArgbColor()
            {
                Alpha = ((ArgbColor)oldcolor).Blue,
                Red = ((ArgbColor)oldcolor).Green,
                Green = ((ArgbColor)oldcolor).Red,
                Blue = ((ArgbColor)oldcolor).Alpha
            };
            return newcolor;
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