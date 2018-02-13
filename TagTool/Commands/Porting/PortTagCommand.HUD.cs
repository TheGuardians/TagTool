using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {

        private ChudDefinition.HudWidget.StateDatum ConvertStateData(ChudDefinition.HudWidget.StateDatum stateData, CacheVersion portingVersion)
        {
            //Maybe missile pod should be added...
            switch (portingVersion)
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

        private ChudDefinition.HudWidget.RenderDatum ConvertRenderData(ChudDefinition.HudWidget.RenderDatum renderData, CacheVersion portingVersion)
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
                        var newColor = new ArgbColor()
                        {
                            Alpha = ((ArgbColor)halo3FieldValue).Alpha,
                            Red = ((ArgbColor)halo3FieldValue).Blue,
                            Green = ((ArgbColor)halo3FieldValue).Green,
                            Blue = ((ArgbColor)halo3FieldValue).Red
                        };
                        haloOnlineFieldInfo.SetValue(renderData, newColor);
                    }
                }
            }

            //For some reason, the turbulance shader in H:O only works if the following unknown is 1.
            //So if the turbulance shader is selected, set the following unknown to 1.
            if (renderData.ShaderIndex == ChudDefinition.HudWidget.RenderDatum.ShaderIndexValue.Turbulence)
            {
                renderData.Unknown = 1;
            }

            switch (portingVersion)
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

        private ChudDefinition.HudWidget.TextWidget ConvertTextWidget(ChudDefinition.HudWidget.TextWidget textWidget, CacheVersion portingVersion)
        {
            switch(portingVersion)
            {
                case CacheVersion.Halo3Retail:
                    textWidget.Flags = GetEquivalentFlags(textWidget.Flags, textWidget.Flags_H3);
                    break;
            }

            return textWidget;
        }

        private ChudDefinition ConvertChudDefinition(CacheVersion portingVersion, ChudDefinition chudDefinition)
        {
            for (int hudWidgetIndex = 0; hudWidgetIndex < chudDefinition.HudWidgets.Count; hudWidgetIndex++)
            {
                for (int stateDatumIndex = 0; stateDatumIndex < chudDefinition.HudWidgets[hudWidgetIndex].StateData.Count; stateDatumIndex++)
                    chudDefinition.HudWidgets[hudWidgetIndex].StateData[stateDatumIndex] = ConvertStateData(chudDefinition.HudWidgets[hudWidgetIndex].StateData[stateDatumIndex], portingVersion);
                for (int renderDatumIndex = 0; renderDatumIndex < chudDefinition.HudWidgets[hudWidgetIndex].RenderData.Count; renderDatumIndex++)
                    chudDefinition.HudWidgets[hudWidgetIndex].RenderData[renderDatumIndex] = ConvertRenderData(chudDefinition.HudWidgets[hudWidgetIndex].RenderData[renderDatumIndex], portingVersion);
                for (int bitmapWidgetIndex = 0; bitmapWidgetIndex < chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets.Count; bitmapWidgetIndex++)
                {
                    for (int stateDatumIndex = 0; stateDatumIndex < chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].StateData.Count; stateDatumIndex++)
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].StateData[stateDatumIndex] = ConvertStateData(chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].StateData[stateDatumIndex], portingVersion);
                    for (int renderDatumIndex = 0; renderDatumIndex < chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].RenderData.Count; renderDatumIndex++)
                        chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].RenderData[renderDatumIndex] = ConvertRenderData(chudDefinition.HudWidgets[hudWidgetIndex].BitmapWidgets[bitmapWidgetIndex].RenderData[renderDatumIndex], portingVersion);
                }

                for (int textWidgetIndex = 0; textWidgetIndex < chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets.Count; textWidgetIndex++)
                {
                    chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex] = ConvertTextWidget(chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex], portingVersion);

                    for (int stateDatumIndex = 0; stateDatumIndex < chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].StateData.Count; stateDatumIndex++)
                        chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].StateData[stateDatumIndex] = ConvertStateData(chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].StateData[stateDatumIndex], portingVersion);
                    for (int renderDatumIndex = 0; renderDatumIndex < chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].RenderData.Count; renderDatumIndex++)
                        chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].RenderData[renderDatumIndex] = ConvertRenderData(chudDefinition.HudWidgets[hudWidgetIndex].TextWidgets[textWidgetIndex].RenderData[renderDatumIndex], portingVersion);
				}
            }
            return chudDefinition;
        }

		private ChudGlobalsDefinition ConvertChudGlobalsDefinition(CacheVersion portingVersion, ChudGlobalsDefinition chudGlobalsDefinition)
		{
            Console.WriteLine("Warning: The tagtool is about to port a HUD Globals (CHGD) tag. HUD globals cannot yet be fully ported without manual modification, and will result in frequent crashes.");

			chudGlobalsDefinition.SprintFOVMultiplier = 1;
			chudGlobalsDefinition.SprintFOVTransitionInTime = 0.5f;
			chudGlobalsDefinition.SprintFOXTransitionOutTime = 1;
			chudGlobalsDefinition.Unknown56 = new byte[] { 0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			chudGlobalsDefinition.Unknown57 = new byte[] { 0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			chudGlobalsDefinition.Unknown60 = new byte[] { 0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			chudGlobalsDefinition.Unknown61 = new byte[] { 0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			chudGlobalsDefinition.Unknown62 = new byte[] { 0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			chudGlobalsDefinition.Unknown63 = 1.33f;
			chudGlobalsDefinition.Unknown64 = new byte[] { 0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			chudGlobalsDefinition.Unknown65 = new byte[] { 0x03, 0x34, 0x00, 0x00, 0x9A, 0x99, 0x99, 0xBD, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x00, 0xCD, 0xCC, 0xCC, 0xBD, 0xCD, 0xCC, 0x8C, 0x3F };
			chudGlobalsDefinition.Unknown66 = new byte[] { 0x03, 0x34, 0x00, 0x00, 0xBC, 0x74, 0x93, 0xBB, 0xBC, 0x74, 0x93, 0x3B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F };
			chudGlobalsDefinition.Unknown67 = new byte[] { 0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			chudGlobalsDefinition.Unknown68 = new byte[] { 0x03, 0x34, 0x00, 0x00, 0x6F, 0x12, 0x83, 0xBB, 0x6F, 0x12, 0x83, 0x3B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x40, 0x00, 0x00, 0x00, 0x3F, 0xCD, 0xCC, 0xCC, 0xBD, 0xCD, 0xCC, 0x8C, 0x3F };
			chudGlobalsDefinition.Unknown70 = 3;

			for (int hudGlobalsIndex = 0; hudGlobalsIndex < chudGlobalsDefinition.HudGlobals.Count; hudGlobalsIndex++)
            {
                foreach (FieldInfo haloOnlineFieldInfo in typeof(ChudGlobalsDefinition.HudGlobal).GetFields())
                {
                    object haloOnlineFieldValue = haloOnlineFieldInfo.GetValue(chudGlobalsDefinition.HudGlobals[hudGlobalsIndex]);
                    FieldInfo halo3FieldInfo;
                    object halo3FieldValue;

                    if (haloOnlineFieldValue is ArgbColor)
                    {
                        if (haloOnlineFieldInfo.Name.Contains("_HO"))
                        {
                            halo3FieldInfo = typeof(ChudGlobalsDefinition.HudGlobal).GetField(haloOnlineFieldInfo.Name.Replace("_HO", ""));

                            if (halo3FieldInfo == null)
                                continue;

                            halo3FieldValue = halo3FieldInfo.GetValue(chudGlobalsDefinition.HudGlobals[hudGlobalsIndex]);

                            if (halo3FieldValue is ArgbColor)
                            {
                                var newColor = new ArgbColor()
                                {
                                    Alpha = ((ArgbColor)halo3FieldValue).Alpha,
                                    Red = ((ArgbColor)halo3FieldValue).Blue,
                                    Green = ((ArgbColor)halo3FieldValue).Green,
                                    Blue = ((ArgbColor)halo3FieldValue).Red
                                };
                                haloOnlineFieldInfo.SetValue(chudGlobalsDefinition.HudGlobals[hudGlobalsIndex], newColor);
                            }
                        }
                    }
                }

                //chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].GlobalDynamic27_HO = new RgbaColor();
                chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].GlobalDynamic29_HO = new ArgbColor();
                chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].DefaultItemOutline = new ArgbColor();
                chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].MAGItemOutline = new ArgbColor();
                chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].DMGItemOutline = new ArgbColor();
                chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].ACCItemOutline = new ArgbColor();
                chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].ROFItemOutline = new ArgbColor();
                chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].RNGItemOutline = new ArgbColor();
                chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].PWRItemOutline = new ArgbColor();

                for (int hudAttributesIndex = 0; hudAttributesIndex < chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudAttributes.Count; hudAttributesIndex++)
                {
                    if (portingVersion == CacheVersion.Halo3Retail)
                    {
                        chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudAttributes[hudAttributesIndex].WarpAngle_HO = chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudAttributes[hudAttributesIndex].WarpAngle_H3;
                        chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudAttributes[hudAttributesIndex].WarpAmount_HO = chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudAttributes[hudAttributesIndex].WarpAmount_H3;
                        chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudAttributes[hudAttributesIndex].WarpDirection_HO = chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudAttributes[hudAttributesIndex].WarpDirection_H3;

                        chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudAttributes[hudAttributesIndex].NotificationOffsetY_HO = chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudAttributes[hudAttributesIndex].NotificationOffsetY_H3;
                    }
                }
                for (int hudSoundsIndex = 0; hudSoundsIndex < chudGlobalsDefinition.HudGlobals[hudGlobalsIndex].HudSounds.Count; hudSoundsIndex++)
                {
                    if (portingVersion == CacheVersion.Halo3Retail)
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
					else if(portingVersion == CacheVersion.Halo3ODST)
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
            }

            return chudGlobalsDefinition;
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
                Console.WriteLine("GetEquevelantFlags called with a non enum parameter.");
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
