using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private ChudDefinition.HudWidget.StateDatum ConvertStateData(ChudDefinition.HudWidget.StateDatum stateData)
        {
            //Maybe missile pod should be added...
            switch (BlamCache.Version)
            {
                case CacheVersion.Halo3Retail:
                    stateData.EngineFlags_HO = GetEquivalentFlags(stateData.EngineFlags_HO, stateData.EngineFlags_H3);
                    stateData.GamemodeFlags_HO = GetEquivalentFlags(stateData.GamemodeFlags_HO, stateData.GamemodeFlags_H3);
                    stateData.UntitledFlags1 = GetEquivalentFlags(stateData.UntitledFlags1, stateData.UntitledFlags1_H3);
                    stateData.UntitledFlags5 = GetEquivalentFlags(stateData.UntitledFlags5, stateData.UntitledFlags5_H3);

                    stateData.ConsumableFlagsC = GetEquivalentFlags(stateData.ConsumableFlagsC, stateData.EngineFlags_H3);
                    stateData.UntitledFlags5 = GetEquivalentFlags(stateData.UntitledFlags5, stateData.UntitledFlags5_H3);
                    break;

                case CacheVersion.Halo3ODST:
                    stateData.EngineFlags_HO = GetEquivalentFlags(stateData.EngineFlags_HO, stateData.EngineFlags_ODST);
                    break;
            }

            stateData.ScoreboardFlags_HO = GetEquivalentFlags(stateData.ScoreboardFlags_HO, stateData.ScoreboardFlags);
            stateData.ScoreboardFlagsB = GetEquivalentFlags(stateData.ScoreboardFlagsB, stateData.ScoreboardFlags);
            stateData.UntitledFlags2_HO = GetEquivalentFlags(stateData.UntitledFlags2_HO, stateData.UntitledFlags2);

            return stateData;
        }

        private ChudDefinition.HudWidget.RenderDatum ConvertRenderData(ChudDefinition.HudWidget.RenderDatum renderData)
        {
            foreach (FieldInfo haloOnlineFieldInfo in typeof(ChudDefinition.HudWidget.RenderDatum).GetFields())
            {
                object haloOnlineFieldValue = haloOnlineFieldInfo.GetValue(renderData);
                FieldInfo halo3FieldInfo;
                object halo3FieldValue;

                if (haloOnlineFieldValue is ArgbColor && haloOnlineFieldInfo.Name.Contains("_HO"))
                {
                    halo3FieldInfo = typeof(ChudDefinition.HudWidget.RenderDatum).GetField(haloOnlineFieldInfo.Name.Replace("_HO", ""));

                    if (halo3FieldInfo == null)
                        continue;

                    halo3FieldValue = halo3FieldInfo.GetValue(renderData);

                    if (halo3FieldValue is ArgbColor)
                    {
                        var newColor = new ArgbColor();
                        newColor = ConvertColor((ArgbColor)halo3FieldValue);
                        haloOnlineFieldInfo.SetValue(renderData, newColor);
                    }
                }
            }

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
            switch(BlamCache.Version)
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

                    //fixup for ammo charge meter widgets
                    if (bitmapwidgetname == "meter")
                    {
                            //check these two offsets to make sure only the correct meters are targeted
                            if (chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Offset.X == -7 && chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Offset.Y == 47)
                            {
                                chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Scale.X = 0.58f;
                                chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Scale.Y = 0.58f;
                            }                       
                    }
                    //fixup for corners_720 Spartan HUD widgets rendering offscreen to the left and right
                    if (bitmapwidgetname == "corners_720" && widgetname == "in_helmet_top")
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Anchor = ChudDefinition.HudWidget.PlacementDatum.AnchorValue.TopEdge;
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].MirrorOffset.X = 3.57f;
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Offset.Y = -16f;            
                    }
                    //fixup for center_720 Elite HUD widgets rendering too far offset from top and bottom
                    if (bitmapwidgetname == "center_720" && widgetname == "in_helmet")
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].MirrorOffset.Y = 7.05f;
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].MirrorOffset.X = 1.0f;
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Scale.Y = 1.2f;
                    }
                    //fixup for upper_corners_720 Elite HUD widgets rendering too far offset from center
                    if (bitmapwidgetname == "upper_corners_720" && widgetname == "in_helmet")
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].MirrorOffset.X = -2.22f;
                    }
                    //fixup for lower_corners_720 Elite HUD widgets rendering too far offset from center
                    if (bitmapwidgetname == "lower_corners_720" && widgetname == "in_helmet")
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].MirrorOffset.X = -2.9f;
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Offset.Y = -100.0f;
                    }
                    //fixup for red warning flashes not scaling with HUD
                    if (widgetname.Contains("warning_flashes_spartan") && BlamCache.Version == CacheVersion.Halo3Retail)
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Offset.X = -96.0f;
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Offset.Y = 102.0f;
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Scale.X = 2.42f;
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Scale.X = 1.0f;
                    }
                    //fixup ODST waypoint lightup
                    if (widgetname == "waypoint_light_bottom720")
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Offset.Y = 310.0f;
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Scale.Y = 1.5f;
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].PlacementData[0].Scale.X = 2.02f;
                    }

                    for (int textWidgetIndex = 0; textWidgetIndex < chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets.Count; textWidgetIndex++)
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex] = ConvertTextWidget(chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex]);

                        for (int stateDatumIndex = 0; stateDatumIndex < chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].StateData.Count; stateDatumIndex++)
                            chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].StateData[stateDatumIndex] = ConvertStateData(chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].StateData[stateDatumIndex]);
                        for (int renderDatumIndex = 0; renderDatumIndex < chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].RenderData.Count; renderDatumIndex++)
                            chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].RenderData[renderDatumIndex] = ConvertRenderData(chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].RenderData[renderDatumIndex]);
                    }
                }
                //scale all widget groups by 1.5 to match 720p > 1080p conversion
                for (int placementDatumIndex = 0; placementDatumIndex < chudDefinition.HudWidgets[hudWidgetIndex].PlacementData.Count; placementDatumIndex++)
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[placementDatumIndex].Scale.X = 1.5f * chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[placementDatumIndex].Scale.X;
                        chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[placementDatumIndex].Scale.Y = 1.5f * chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[placementDatumIndex].Scale.Y;
                    //HUD X-Scale Fixup
                    if (widgetname == "in_helmet_bottom" && BlamCache.Version == CacheVersion.Halo3ODST)
                        chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[placementDatumIndex].Scale.X = 1.0f;
                    //HUD Y-Offset Top Fixup
                    if (widgetname == "in_helmet_top" && BlamCache.Version == CacheVersion.Halo3ODST)
                        chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[placementDatumIndex].Offset.Y = -110.0f;
                    //fixup for ODST Grenade Placement
                    if (widgetname.Contains("grenade") && BlamCache.Version == CacheVersion.Halo3ODST)
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[0].Offset.Y = 1.5f * chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[0].Offset.Y;
                    }
                }               
            }
            return chudDefinition;
        }

		private ChudGlobalsDefinition ConvertChudGlobalsDefinition(Stream cacheStream, ChudGlobalsDefinition H3Definition, CacheFile.IndexItem blamTag, Cache.CachedTagInstance edTag)
		{
            //get HO tag
            var srcTag = CacheContext.GetTag(0x01BD);
            var srcContext = new TagSerializationContext(cacheStream, CacheContext, srcTag);
            ChudGlobalsDefinition HODefinition = CacheContext.Deserializer.Deserialize<ChudGlobalsDefinition>(srcContext);

            //Use HO Shaders
            H3Definition.HudShaders = HODefinition.HudShaders;

            for (int hudGlobalsIndex = 0; hudGlobalsIndex < H3Definition.HudGlobals.Count; hudGlobalsIndex++)
            {
                var H3globs = H3Definition.HudGlobals[hudGlobalsIndex];
                var HOglobs = HODefinition.HudGlobals[hudGlobalsIndex];

                //Use HO Sounds
                H3globs.HudSounds = HOglobs.HudSounds;
                
                //Color Conversion
                H3globs.HUDDisabled = ConvertColor(H3globs.HUDDisabled);
                H3globs.HUDPrimary = ConvertColor(H3globs.HUDPrimary);
                H3globs.HUDForeground = ConvertColor(H3globs.HUDForeground);
                H3globs.HUDWarning = ConvertColor(H3globs.HUDWarning);
                H3globs.NeutralReticule = ConvertColor(H3globs.NeutralReticule);
                H3globs.HostileReticule = ConvertColor(H3globs.HostileReticule);
                H3globs.FriendlyReticule = ConvertColor(H3globs.FriendlyReticule);
                H3globs.GlobalDyanamic7_UnknownBlip = ConvertColor(H3globs.GlobalDyanamic7_UnknownBlip);
                H3globs.NeutralBlip = ConvertColor(H3globs.NeutralBlip);
                H3globs.HostileBlip = ConvertColor(H3globs.HostileBlip);
                H3globs.FriendlyPlayerBlip = ConvertColor(H3globs.FriendlyPlayerBlip);
                H3globs.FriendlyAIBlip = ConvertColor(H3globs.FriendlyAIBlip);
                H3globs.GlobalDynamic12 = ConvertColor(H3globs.GlobalDynamic12);
                H3globs.WaypointBlip = ConvertColor(H3globs.WaypointBlip);
                H3globs.DistantWaypointBlip = ConvertColor(H3globs.DistantWaypointBlip);
                H3globs.FriendlyWaypoint = ConvertColor(H3globs.FriendlyWaypoint);
                H3globs.NeutralWaypoint = ConvertColor(H3globs.NeutralWaypoint);
                H3globs.HostileWaypoint = ConvertColor(H3globs.HostileWaypoint);
                H3globs.DeadWaypoint = ConvertColor(H3globs.DeadWaypoint);

                //fixups
                H3globs.GrenadeScematicsSpacing = 1.5f * H3globs.GrenadeScematicsSpacing;

                //loop through unset fields and set them to HO values
                foreach (FieldInfo H3FieldInfo in typeof(ChudGlobalsDefinition.HudGlobal).GetFields())
                {
                    object H3FieldValue = H3FieldInfo.GetValue(H3Definition.HudGlobals[hudGlobalsIndex]);
                    object HOFieldValue = H3FieldInfo.GetValue(HODefinition.HudGlobals[hudGlobalsIndex]);
                    object zeroint = (int)0;
                    object zerofloat = 0.0f;
                    object zerocolor = new ArgbColor(0x00, 0x00, 0x00, 0x00);
                    object zerotag = new CachedTagInstance(-1);

                    if (H3FieldValue == null || H3FieldValue.Equals(zeroint) || H3FieldValue.Equals(zerofloat) || H3FieldValue.Equals(zerocolor) || H3FieldValue.Equals(zerotag))
                        H3FieldInfo.SetValue(H3Definition.HudGlobals[hudGlobalsIndex], HOFieldValue);
                }

                for (int hudAttributesIndex = 0; hudAttributesIndex < H3Definition.HudGlobals[hudGlobalsIndex].HudAttributes.Count; hudAttributesIndex++)
                {
                    var H3att = H3Definition.HudGlobals[hudGlobalsIndex].HudAttributes[hudAttributesIndex];
                    var HOatt = HODefinition.HudGlobals[hudGlobalsIndex].HudAttributes[hudAttributesIndex];

                    if (BlamCache.Version == CacheVersion.Halo3ODST)
                    {
                        H3att.NotificationOffsetX_HO = H3att.NotificationOffsetX_H3;
                    }
                    
                    //more fixups
                    H3att.ResolutionWidth = 1920;
                    H3att.ResolutionHeight = 1080;
                    H3att.MotionSensorOffset.X = 1.5f * H3att.MotionSensorOffset.X;
                    H3att.MotionSensorOffset.Y = 1.5f * H3att.MotionSensorOffset.Y;
                    H3att.MotionSensorRadius = 1.5f * H3att.MotionSensorRadius;
                    H3att.MotionSensorScale = 1.5f * H3att.MotionSensorScale;
                    H3att.HorizontalScale = 1.0f;
                    H3att.VerticalScale = 1.0f;
                    H3att.PickupDialogOffset.Y = 0.3f;
                    H3att.PickupDialogScale = 1.2f;
                    H3att.NotificationScale = 1.2f;
                    H3att.NotificationLineSpacing = 30.0f;
                    H3att.NotificationOffsetY_HO = 0.65f;
                }

                /*
                for (int hudSoundsIndex = 0; hudSoundsIndex < H3Definition.HudGlobals[hudGlobalsIndex].HudSounds.Count; hudSoundsIndex++)
                {
                    var H3snd = H3Definition.HudGlobals[hudGlobalsIndex].HudSounds[hudSoundsIndex];
                    var HOsnd = HODefinition.HudGlobals[hudGlobalsIndex].HudSounds[hudSoundsIndex];

                    if (BlamCache.Version == CacheVersion.Halo3Retail)
                    {
                        chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudSounds[hudSoundsIndex].LatchedTo 
                            = GetEquivalentFlags(chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudSounds[hudSoundsIndex].LatchedTo, chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudSounds[hudSoundsIndex].LatchedTo_H3);

                        chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudSounds[hudSoundsIndex].Bipeds = new List<ChudGlobalsDefinition.HudGlobal.HudSound.BipedData>();
                        if (chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudSounds[hudSoundsIndex].SpartanSound != null)
                        {
                            var spartanBiped = new ChudGlobalsDefinition.HudGlobal.HudSound.BipedData()
                            {
                                BipedType_HO = ChudGlobalsDefinition.HudGlobal.HudSound.BipedData.BipedTypeValue_HO.Spartan,
                                Sound = chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudSounds[hudSoundsIndex].SpartanSound
                            };
                            chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudSounds[hudSoundsIndex].Bipeds.Add(spartanBiped);
                        }
                        if (chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudSounds[hudSoundsIndex].EliteSound != null)
                        {
                            var eliteBiped = new ChudGlobalsDefinition.HudGlobal.HudSound.BipedData()
                            {
                                BipedType_HO = ChudGlobalsDefinition.HudGlobal.HudSound.BipedData.BipedTypeValue_HO.Elite,
                                Sound = chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudSounds[hudSoundsIndex].EliteSound
                            };
                            chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudSounds[hudSoundsIndex].Bipeds.Add(eliteBiped);
                        }
                    }
                    */
                    /*
					else if(BlamCache.Version == CacheVersion.Halo3ODST)
					{
						for (int bipedIndex = 0; bipedIndex < chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudSounds[hudSoundsIndex].Bipeds.Count; bipedIndex++)
						{
                            if (chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudSounds[hudSoundsIndex].Bipeds[bipedIndex].BipedType_ODST == ChudGlobalsDefinition.HudGlobal.HudSound.BipedData.BipedTypeValue_ODST.Any
                                || chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudSounds[hudSoundsIndex].Bipeds[bipedIndex].BipedType_ODST == ChudGlobalsDefinition.HudGlobal.HudSound.BipedData.BipedTypeValue_ODST.Rookie)
                            {
                                chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudSounds[hudSoundsIndex].Bipeds[bipedIndex].BipedType_HO = ChudGlobalsDefinition.HudGlobal.HudSound.BipedData.BipedTypeValue_HO.Spartan;
                            }
                            else
                            {
                                chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudSounds[hudSoundsIndex].Bipeds.RemoveAt(bipedIndex);
                            }
						}
                    }
                    
                }
            */
            }

            //keep elite and monitor entries for HUDGlobals
            if (BlamCache.Version == CacheVersion.Halo3ODST)
            {
                var HOcopy = new ChudGlobalsDefinition();
                HOcopy.HudGlobals = HODefinition.HudGlobals;
                HOcopy.HudGlobals[0] = H3Definition.HudGlobals[0];
                H3Definition.HudGlobals = HOcopy.HudGlobals;
            }

            //loop through all fields and set unfilled fields to HO values
            foreach (FieldInfo H3FieldInfo in typeof(ChudGlobalsDefinition).GetFields())
            {
                object H3FieldValue = H3FieldInfo.GetValue(H3Definition);
                object HOFieldValue = H3FieldInfo.GetValue(HODefinition);
                object zeroint = (int)0;
                object zerofloat = 0.0f;
                object zerocolor = new ArgbColor(0x00, 0x00, 0x00, 0x00);
                object zerotag = new CachedTagInstance(-1);

                if (H3FieldValue == null || H3FieldValue.Equals(zeroint) || H3FieldValue.Equals(zerofloat) || H3FieldValue.Equals(zerocolor) || H3FieldValue.Equals(zerotag))
                    H3FieldInfo.SetValue(H3Definition, HOFieldValue);
                if (H3FieldInfo.FieldType == typeof(TagFunction))
                    H3FieldInfo.SetValue(H3Definition, HOFieldValue);
            }
            return H3Definition;
        }

        private ArgbColor ConvertColor(ArgbColor oldcolor)
        {
            if (BlamCache.Version == CacheVersion.Halo3ODST)
            {
                var odstcolor = new ArgbColor()
                {
                    Alpha = ((ArgbColor)oldcolor).Alpha,
                    Red = ((ArgbColor)oldcolor).Blue,
                    Green = ((ArgbColor)oldcolor).Green,
                    Blue = ((ArgbColor)oldcolor).Red
                };
                return odstcolor;
            }
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
        private flagsType1 GetEquivalentFlags<flagsType1, flagsType2>(flagsType1 flags1, flagsType2 flags2) where flagsType1 : struct, IConvertible
        {
            //Check params are enums.
            if(!(flags2 is Enum) || !(flags1 is Enum))
            {
                Console.WriteLine("GetEquevelantFlags called with a non enum parameter.");
                return flags1;
            }

            //Check for [Flags] attrib
            object[] hoFlagsAttributes = typeof(flagsType1).GetCustomAttributes(typeof(FlagsAttribute), false);
            object[] h3FlagsAttributes = typeof(flagsType2).GetCustomAttributes(typeof(FlagsAttribute), false);

            if (hoFlagsAttributes.Count() < 1 || h3FlagsAttributes.Count() < 1)
            {
                Console.WriteLine("GetEquevelantFlags called with a non flags enum parameter.");
                return flags1;
            }

            //Get enum values
            flagsType1[] flagsType1Values = (flagsType1[])Enum.GetValues(typeof(flagsType1));
            flagsType2[] flagsType2Values = (flagsType2[])Enum.GetValues(typeof(flagsType2));

            //Seperate flags for individual comparison.
            List<flagsType2> flags2Values = new List<flagsType2>();

            foreach (flagsType2 flagsType2Value in flagsType2Values)
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
            foreach (flagsType2 flag2 in flags2Values)
            {
                string flag2String = flag2.ToString();
                var flags1EnumType = Nullable.GetUnderlyingType(typeof(flagsType1));
                try
                {
                    flagsType1 flag1 = (flagsType1)Enum.Parse(typeof(flagsType1), flag2String);
                    flags1Int |= (int)Convert.ChangeType(flag1, typeof(int));
                }
                catch (ArgumentException)
                {
                    continue;
                }
            }
            return (flagsType1)(Enum.ToObject(typeof(flagsType1), flags1Int));
        }

        /// <summary>
        /// If the value of enum1 has a value with the same name as the value of enum2, return it.
        /// </summary>
        private enumType1 GetEquivalentValue<enumType1, enumType2>(enumType1 enum1, enumType2 enum2) where enumType1 : struct, IConvertible
        {
            //Check params are enums.
            if (!(enum1 is Enum) || !(enum2 is Enum))
            {
                Console.WriteLine("Get Equivalent Flags called with a non-enum parameter.");
                return enum1;
            }

            //Get enum values
            enumType1[] enumType1Values = (enumType1[])Enum.GetValues(typeof(enumType1));
            enumType2[] enumType2Values = (enumType2[])Enum.GetValues(typeof(enumType2));

            //Compare
            string enum2String = enum2.ToString();

            try
            {
                return (enumType1)Enum.Parse(typeof(enumType1), enum2String);
            }
            catch (Exception)
            {
                return enum1;
            }
        }
    }
}