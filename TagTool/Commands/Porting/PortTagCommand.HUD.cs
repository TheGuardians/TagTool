using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
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

                //fixup for binoculars
                if (widgetname == "binoculars_wide_fullscreen" && BlamCache.Version == CacheVersion.Halo3Retail)
                {
                    chudDefinition.HudWidgets[hudWidgetIndex].PlacementData[0].Offset.Y = 0.0f;
                }

                for (int bitmapWidgetIndex = 0; bitmapWidgetIndex < chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets.Count; bitmapWidgetIndex++)
                {
                    for (int stateDatumIndex = 0; stateDatumIndex < chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].StateData.Count; stateDatumIndex++)
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].StateData[stateDatumIndex] = ConvertStateData(chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].StateData[stateDatumIndex]);
                    for (int renderDatumIndex = 0; renderDatumIndex < chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].RenderData.Count; renderDatumIndex++)
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].RenderData[renderDatumIndex] = ConvertRenderData(chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].RenderData[renderDatumIndex]);

                    //get stringid text for patch targeting
                    var bitmapwidgetname = CacheContext.GetString(chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].Name);

                    //fixup for waypoint light
                    if (widgetname.Contains("waypoint_light") && BlamCache.Version == CacheVersion.Halo3ODST)
                    {
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].RenderData[0].Input_HO = ChudDefinition.HudWidget.RenderDatum.InputValue_HO.UnitHealth;
                    }

                    //fixup for widgets without global placement data
                    if (chudDefinition.HudWidgets[hudWidgetIndex].PlacementData.Count == 0
                        || (BlamCache.Version == CacheVersion.Halo3ODST && widgetname.Contains("vitality_meter"))
                        || (BlamCache.Version == CacheVersion.Halo3ODST && widgetname.Contains("compass")))
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

		private ChudGlobalsDefinition ConvertChudGlobalsDefinition(ChudGlobalsDefinition H3Definition)
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

                H3globs.TextFadeIn_HO = ConvertColor(H3globs.TextFadeIn);
                H3globs.GlobalDynamic21_HO = ConvertColor(H3globs.GlobalDynamic21);
                H3globs.GlobalDynamic23_HO = ConvertColor(H3globs.GlobalDynamic23);
                H3globs.GlobalDynamic24_HO = ConvertColor(H3globs.GlobalDynamic24);
                H3globs.GlobalDynamic25_UnknownWaypoint_HO = ConvertColor(H3globs.GlobalDynamic25);

                //fixups
                H3globs.GrenadeScematicsSpacing = 1.5f * H3globs.GrenadeScematicsSpacing;

                for (int hudAttributesIndex = 0; hudAttributesIndex < H3Definition.HudGlobals[hudGlobalsIndex].HudAttributes.Count; hudAttributesIndex++)
                {
                    var H3att = H3Definition.HudGlobals[hudGlobalsIndex].HudAttributes[hudAttributesIndex];

                    if (BlamCache.Version == CacheVersion.Halo3ODST)
                    {
                        H3att.NotificationOffsetX_HO = H3att.NotificationOffsetX_H3;
                        H3att.WarpAngle = Angle.FromDegrees(1.0f);
                    }
                    
                    //more fixups
                    H3att.ResolutionWidth = (uint)(H3att.ResolutionWidth * 1.5f);
                    H3att.ResolutionHeight = (uint)(H3att.ResolutionHeight * 1.5f);
                    H3att.MotionSensorOffset.X = (float)Math.Ceiling((double)(1.5f * H3att.MotionSensorOffset.X));
                    H3att.MotionSensorOffset.Y = (float)Math.Ceiling((double)(1.5f * H3att.MotionSensorOffset.Y));
                    H3att.MotionSensorRadius *= 1.5f;
                    H3att.MotionSensorScale *= 1.5f;
                    H3att.HorizontalScale = 1.0f;
                    H3att.VerticalScale = 1.0f;
                    H3att.PickupDialogOffset.Y = 0.3f;
                    H3att.PickupDialogScale = 1.2f;
                    H3att.NotificationOffsetY_HO = H3att.NotificationOffsetY_H3;
                    H3att.NotificationOffsetX_HO = H3att.NotificationOffsetX_H3;
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
                            spartanBiped.Sound = PortTagReference(H3snd.SpartanSound.Index);
                            H3snd.Bipeds.Add(spartanBiped);
                        }
                        if (H3snd.EliteSound != null)
                        {
                            var eliteBiped = new ChudGlobalsDefinition.HudGlobal.HudSound.BipedData();
                            eliteBiped.BipedType_HO = ChudGlobalsDefinition.HudGlobal.HudSound.BipedData.BipedTypeValue_HO.Elite;
                            eliteBiped.Sound = PortTagReference(H3snd.SpartanSound.Index);
                            H3snd.Bipeds.Add(eliteBiped);
                        }
                    }
                    
                    
					else if(BlamCache.Version == CacheVersion.Halo3ODST)
					{
						for (int bipedIndex = 0; bipedIndex < H3snd.Bipeds.Count; bipedIndex++)
						{
                            if (H3snd.Bipeds[bipedIndex].BipedType_ODST == ChudGlobalsDefinition.HudGlobal.HudSound.BipedData.BipedTypeValue_ODST.Rookie)
                            {
                                H3snd.Bipeds[bipedIndex].BipedType_HO = ChudGlobalsDefinition.HudGlobal.HudSound.BipedData.BipedTypeValue_HO.Spartan;
                            }
                            else
                            {
                                H3snd.Bipeds.RemoveAt(bipedIndex);
                            }
						}
                    }
                    
                }
            
            }

            //additional values
            H3Definition.Unknown5 = 1.8f;
            H3Definition.ShieldMinorThreshold = 0.75f;
            H3Definition.ShieldMajorThreshold = 0.74f;
            H3Definition.ShieldCriticalThreshold = 0.1f;
            H3Definition.HealthMinorThreshold = 0.9f;
            H3Definition.HealthMajorThreshold = 0.75f;
            H3Definition.HealthCriticalThreshold = 0.5f;

            //upscale blip bitmap sizes
            H3Definition.LargeSensorBlipScale = H3Definition.LargeSensorBlipScale * 2.0f;
            H3Definition.MediumSensorBlipScale = H3Definition.MediumSensorBlipScale * 2.0f;
            H3Definition.SmallSensorBlipScale = H3Definition.SmallSensorBlipScale * 2.0f;

            //prevent crash?
            H3Definition.Unknown71 = 3.0f;

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