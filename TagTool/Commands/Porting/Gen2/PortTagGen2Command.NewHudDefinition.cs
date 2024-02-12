using Assimp;
using Poly2Tri.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TagTool.Audio;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Definitions.Common;
using TagTool.Tags.Definitions.Gen2;
using TagTool.Tags.Definitions.Gen4;
using static TagTool.Tags.Definitions.Bitmap;
using static TagTool.Tags.Definitions.ChudDefinition;
using static TagTool.Tags.Definitions.ChudDefinition.HudWidgetBase;
using static TagTool.Tags.Definitions.Gen2.NewHudDefinition;
using ShaderGen2 = TagTool.Tags.Definitions.Gen2.Shader;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        public TagStructure ConvertNewHudDefinition(NewHudDefinition nhdt, NewHudDefinition gen2Hud, Stream cacheStream, Stream gen2CacheStream, CachedTag gen2Tag)
        {
            ChudDefinition chud = new ChudDefinition();
            var gen2Bitmaps = nhdt.BitmapWidgets;
            var gen2Text = nhdt.TextWidgets;
            var widgetDictionary = new Dictionary<string, string>();

            // setup bitmap widget pairs
            foreach (var bitmap in gen2Bitmaps)
            {
                string bitmapWidgetName = Cache.StringTable.GetString(bitmap.Name);
                switch (bitmapWidgetName)
                {
                    case "crosshair":
                    case "crosshair_friend":
                    case "crosshair_weakspot":
                    case "crosshair_plasma_tracking":
                        AddWidgetDictionaryPair(bitmapWidgetName, "crosshair", widgetDictionary);
                        break;
                    case "weapon_background_left":
                    case "weapon_background_left_out":
                    case "heat_meter_left":
                    case "ammo_meter_left":
                        AddWidgetDictionaryPair(bitmapWidgetName, "ammo_area_left", widgetDictionary);
                        break;
                    case "weapon_background_right":
                    case "weapon_background_right_out":
                    case "heat_meter_right":
                    case "ammo_meter_right":
                        AddWidgetDictionaryPair(bitmapWidgetName, "ammo_area_right", widgetDictionary);
                        break;
                    case "heat_meter_single":
                    case "ammo_meter_single":
                    case "weapon_background_single":
                    case "weapon_background_out":
                        AddWidgetDictionaryPair(bitmapWidgetName, "ammo_area", widgetDictionary);
                        break;
                    case "heat_background_left":
                    case "heat_background_right":
                        AddWidgetDictionaryPair(bitmapWidgetName, "warning_flashes", widgetDictionary);
                        break;
                    case "scope_mask":
                    case "left_crosshair":
                    case "right_crosshair":
                    case "bottom_crosshair":
                    case "top_crosshair":
                    case "2x":
                    case "covenant_2xa":
                    case "covenant_2xb":
                    case "distance_meter":
                        AddWidgetDictionaryPair(bitmapWidgetName, "scope", widgetDictionary);
                        break;
                    case "backpack":
                        AddWidgetDictionaryPair(bitmapWidgetName, "backpack", widgetDictionary);
                        break;
                    case "frag_grenade_default":
                        AddWidgetDictionaryPair(bitmapWidgetName, "frag_grenade", widgetDictionary);
                        break;
                    case "plasma_grenade_default":
                        AddWidgetDictionaryPair(bitmapWidgetName, "plasma_grenade", widgetDictionary);
                        break;
                    case "no_grenades":
                        AddWidgetDictionaryPair(bitmapWidgetName, "no_grenades", widgetDictionary);
                        break;
                    case "shield_meter":
                    case "shield_mask":
                        AddWidgetDictionaryPair(bitmapWidgetName, "shield_meter", widgetDictionary);
                        break;
                    case "motion_tracker_background":
                    case "motion_tracker_disabled":
                        AddWidgetDictionaryPair(bitmapWidgetName, "motion_tracker", widgetDictionary);
                        break;
                    case "binocular_mask":
                        AddWidgetDictionaryPair(bitmapWidgetName, "binoculars_wide_fullscreen", widgetDictionary);
                        break;
                    default:
                        AddWidgetDictionaryPair(bitmapWidgetName, "unmapped", widgetDictionary);
                        break;
                }
            }
            // setup text widget pairs
            foreach (var text in gen2Text)
            {
                string textWidgetName = Cache.StringTable.GetString(text.Name);
                switch (textWidgetName)
                {
                    case "frag_count":
                    case "frag_hidden":
                        AddWidgetDictionaryPair(textWidgetName, "frag_grenade", widgetDictionary);
                        break;
                    case "plasma_count":
                    case "plasma_hidden":
                        AddWidgetDictionaryPair(textWidgetName, "plasma_grenade", widgetDictionary);
                        break;
                    case "battery_test_right":
                    case "battery_test_right_out":
                    case "bullets_right":
                    case "bullets_right_out":
                        AddWidgetDictionaryPair(textWidgetName, "ammo_area_right", widgetDictionary);
                        break;
                    case "battery_test_left":
                    case "battery_test_left_out":
                    case "bullets_left":
                    case "bullets_left_out":
                        AddWidgetDictionaryPair(textWidgetName, "ammo_area_left", widgetDictionary);
                        break;
                    case "bullets_single":
                    case "bullets_out":
                    case "heat":
                    case "heat_out":
                        AddWidgetDictionaryPair(textWidgetName, "ammo_area", widgetDictionary);
                        break;
                    default:
                        AddWidgetDictionaryPair(textWidgetName, "unmapped", widgetDictionary);
                        break;
                }
            }

            // populate hud widgets blocks
            chud.HudWidgets = new List<HudWidget>();
            AddHudWidgets(chud, widgetDictionary);

            int widgetIndex = 0;
            // populate bitmaps into their correct hud widgets
            foreach (var hudWidget in chud.HudWidgets)
            {
                hudWidget.BitmapWidgets = new List<HudWidget.BitmapWidget>();
                hudWidget.TextWidgets = new List<HudWidget.TextWidget>();

                for (int bitmapIndex = 0; bitmapIndex < gen2Bitmaps.Count; bitmapIndex++)
                {
                    string bitmapWidgetString = Cache.StringTable.GetString(gen2Bitmaps[bitmapIndex].Name);
                    string hudWidgetString = Cache.StringTable.GetString(hudWidget.Name);

                    // populate bitmaps and subblocks
                    foreach (var widgetDictionaryEntry in widgetDictionary)
                    {
                        if (widgetDictionaryEntry.Key == bitmapWidgetString && widgetDictionaryEntry.Value == hudWidgetString)
                        {
                            RealPoint2d realOffset = new RealPoint2d();
                            RealPoint2d realOrigin = new RealPoint2d { X = -1f, Y = -1f };
                            RealPoint2d realScale = new RealPoint2d { X = 1.25f, Y = 1.25f };
                            RenderDatum newRenderData = new RenderDatum();

                            switch (ConvertAnchor(nhdt.BitmapWidgets[bitmapIndex].Anchor.ToString()))
                            {
                                // widget offset calculated from bitmap scale factor and offset to adjust for different anchor positions between engines
                                case "TopLeft":
                                    realOffset = new RealPoint2d
                                    {
                                        X = nhdt.BitmapWidgets[bitmapIndex].FullscreenOffset.X - 50f,
                                        Y = (nhdt.BitmapWidgets[bitmapIndex].FullscreenOffset.Y * 1.25f),
                                    };
                                    break;
                                case "TopRight":
                                    realOffset = new RealPoint2d
                                    {
                                        X = nhdt.BitmapWidgets[bitmapIndex].FullscreenOffset.X - 5f,
                                        Y = nhdt.BitmapWidgets[bitmapIndex].FullscreenOffset.Y * 1.25f,
                                    };
                                    break;
                                case "MotionSensor":
                                    realOffset = new RealPoint2d
                                    {
                                        X = nhdt.BitmapWidgets[bitmapIndex].FullscreenOffset.X - 106f,
                                        Y = (nhdt.BitmapWidgets[bitmapIndex].FullscreenOffset.Y * 1.25f) + 103f,
                                    };
                                    break;
                                case "BottomRight":
                                    realOffset = new RealPoint2d
                                    {
                                        X = nhdt.BitmapWidgets[bitmapIndex].FullscreenOffset.X,
                                        Y = nhdt.BitmapWidgets[bitmapIndex].FullscreenOffset.Y * 1.25f,
                                    };
                                    break;
                                case "Crosshair":
                                    realOffset = new RealPoint2d
                                    {
                                        X = nhdt.BitmapWidgets[bitmapIndex].FullscreenOffset.X * 1.25f,
                                        Y = nhdt.BitmapWidgets[bitmapIndex].FullscreenOffset.Y * 1.25f,
                                    };
                                    realOrigin = new RealPoint2d
                                    {
                                        X = realOrigin.X * -1,
                                        Y = realOrigin.Y * -1,
                                    };
                                    break;
                                default:
                                    realOffset = new RealPoint2d
                                    {
                                        X = nhdt.BitmapWidgets[bitmapIndex].FullscreenOffset.X,
                                        Y = nhdt.BitmapWidgets[bitmapIndex].FullscreenOffset.Y * 1.25f,
                                    };
                                    break;
                            }
                            switch (bitmapWidgetString)
                            {
                                case "crosshair":
                                    realOffset = new RealPoint2d
                                    {
                                        X = 0,
                                        Y = 0,
                                    };
                                    realOrigin = new RealPoint2d
                                    {
                                        X = 0,
                                        Y = 0,
                                    };
                                    break;
                            }

                            HudBitmapWidgets.FlagsValue flags = nhdt.BitmapWidgets[bitmapIndex].Flags;

                            // Check and apply Flip to widget scale
                            realScale.X *= (flags & HudBitmapWidgets.FlagsValue.FlipHorizontally) == HudBitmapWidgets.FlagsValue.FlipHorizontally ? -1 : 1;
                            realOrigin.X *= (flags & HudBitmapWidgets.FlagsValue.FlipHorizontally) == HudBitmapWidgets.FlagsValue.FlipHorizontally ? -1 : 1;
                            realScale.Y *= (flags & HudBitmapWidgets.FlagsValue.FlipVertically) == HudBitmapWidgets.FlagsValue.FlipVertically ? -1 : 1;
                            realOrigin.Y *= (flags & HudBitmapWidgets.FlagsValue.FlipVertically) == HudBitmapWidgets.FlagsValue.FlipVertically ? -1 : 1;

                            if (hudWidgetString == "scope" && bitmapWidgetString != "scope_mask")
                            {
                                realOrigin.X *= -1;
                                realOrigin.Y *= -1;
                            }

                            HudWidget.BitmapWidget newBitmapWidget = new HudWidget.BitmapWidget
                            {
                                Name = Cache.StringTable.GetStringId(bitmapWidgetString),
                                Bitmap = Cache.TagCacheGenHO.GetTag(nhdt.BitmapWidgets[bitmapIndex].Bitmap.ToString()),
                                BitmapSequenceIndex = (byte)nhdt.BitmapWidgets[bitmapIndex].FullscreenSequenceIndex,
                                StateData = new List<StateDatum>(),
                                PlacementData = new List<PlacementDatum>(),
                                AnimationData = new List<AnimationDatum>(),
                                RenderData = new List<RenderDatum>(),
                                RuntimeWidgetIndex = widgetIndex
                            };
                            widgetIndex++;
                            StateDatum newStateData = new StateDatum { };
                            ProcessStateData(nhdt, bitmapIndex, newStateData, "bitmap");
                            PlacementDatum newPlacementData = new PlacementDatum
                            {
                                Anchor = (PlacementDatum.ChudAnchorType)Enum.Parse(typeof(PlacementDatum.ChudAnchorType), ConvertAnchor(nhdt.BitmapWidgets[bitmapIndex].Anchor.ToString())),
                                Origin = realOrigin,
                                Offset = realOffset,
                                Scale = realScale,
                            };
                            AnimationDatum newAnimationData = new AnimationDatum();
                            PopulateAnimationData(newAnimationData, bitmapWidgetString);
                            ConvertFlags(nhdt.BitmapWidgets[bitmapIndex].Flags, newBitmapWidget, null);

                            CachedTag gen2Shader = gen2Hud.BitmapWidgets[bitmapIndex].Shader;
                            ShaderGen2 gen2ShaderInstance = Gen2Cache.Deserialize<ShaderGen2>(gen2CacheStream, gen2Shader);

                            // Populate render data from SCRAPS I tell you, SCRAPS
                            switch (bitmapWidgetString)
                            {
                                case "crosshair":
                                    newRenderData = new RenderDatum
                                    {
                                        ShaderType = RenderDatum.ChudShaderType.Crosshair,
                                        BlendModeHO = RenderDatum.ChudBlendMode.AlphaBlend,
                                        ExternalInput = RenderDatum.ChudRenderExternalInputHO.Autoaim,
                                        LocalColorA = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorA, "LocalColorA"),
                                        LocalColorB = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorB, "LocalColorB"),
                                        LocalColorC = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorC, "LocalColorC"),
                                        LocalColorD = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorD, "LocalColorD"),
                                        OutputColorA = RenderDatum.OutputColorValue_HO.CrosshairNormal,
                                        OutputColorB = RenderDatum.OutputColorValue_HO.CrosshairEnemy,
                                        OutputColorC = RenderDatum.OutputColorValue_HO.CrosshairFriendly,
                                    };
                                    break;
                                case "shield_meter":
                                    newRenderData = new RenderDatum
                                    {
                                        ShaderType = RenderDatum.ChudShaderType.MeterShield,
                                        BlendModeHO = RenderDatum.ChudBlendMode.AlphaBlend,
                                        ExternalInput = RenderDatum.ChudRenderExternalInputHO.ShieldAmount,
                                        RangeInput = RenderDatum.ChudRenderExternalInputHO.ShieldRecentDamage,
                                        LocalColorA = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorA, "LocalColorA"),
                                        LocalColorB = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorB, "LocalColorB"),
                                        LocalColorC = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorC, "LocalColorC"),
                                        LocalColorD = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorD, "LocalColorD"),
                                        OutputColorA = RenderDatum.OutputColorValue_HO.PrimaryBackground,
                                        OutputColorB = RenderDatum.OutputColorValue_HO.PrimaryBackground,
                                    };
                                    break;
                                case "shield_mask":
                                    newBitmapWidget.SortLayer = WidgetLayerEnum.Middle;
                                    newRenderData = new RenderDatum
                                    {
                                        ShaderType = RenderDatum.ChudShaderType.Simple,
                                        BlendModeHO = RenderDatum.ChudBlendMode.AlphaBlend,
                                        ExternalInput = RenderDatum.ChudRenderExternalInputHO.Zero,
                                        RangeInput = RenderDatum.ChudRenderExternalInputHO.ShieldRecentDamage,
                                        LocalColorA = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorA, "LocalColorA"),
                                        LocalColorB = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorB, "LocalColorB"),
                                        LocalColorC = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorC, "LocalColorC"),
                                        LocalColorD = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorD, "LocalColorD"),
                                        OutputColorA = RenderDatum.OutputColorValue_HO.SecondaryBackground,
                                        OutputColorB = RenderDatum.OutputColorValue_HO.HighlightForeground,
                                    };
                                    break;
                                case "heat_meter":
                                case "heat_meter_zoomed":
                                case "heat_meter_left":
                                case "heat_meter_right":
                                    newRenderData = new RenderDatum
                                    {
                                        ShaderType = RenderDatum.ChudShaderType.MeterGradient,
                                        BlendModeHO = RenderDatum.ChudBlendMode.AlphaBlend,
                                        ExternalInput = ConvertInput(nhdt.BitmapWidgets[bitmapIndex].Unknown.Input1, newRenderData.ExternalInput),
                                        LocalColorA = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorA, "LocalColorA"),
                                        LocalColorB = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorB, "LocalColorB"),
                                        LocalColorC = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorC, "LocalColorC"),
                                        LocalColorD = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorD, "LocalColorD"),
                                    };
                                    break;
                                case "ammo_meter_single":
                                case "ammo_meter_left":
                                case "ammo_meter_right":
                                    newRenderData = new RenderDatum
                                    {
                                        ShaderType = RenderDatum.ChudShaderType.Meter,
                                        BlendModeHO = RenderDatum.ChudBlendMode.AlphaBlend,
                                        ExternalInput = ConvertInput(nhdt.BitmapWidgets[bitmapIndex].Unknown.Input1, newRenderData.ExternalInput),
                                        LocalColorA = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorA, "LocalColorA"),
                                        LocalColorB = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorB, "LocalColorB"),
                                        LocalColorC = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorC, "LocalColorC"),
                                        LocalColorD = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorD, "LocalColorD"),
                                    };
                                    break;
                                case "motion_tracker_background":
                                    newRenderData = new RenderDatum
                                    {
                                        ShaderType = RenderDatum.ChudShaderType.Simple,
                                        BlendModeHO = RenderDatum.ChudBlendMode.AlphaBlend,
                                        LocalColorA = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorA, "LocalColorA"),
                                        LocalColorB = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorB, "LocalColorB"),
                                        LocalColorC = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorC, "LocalColorC"),
                                        LocalColorD = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorD, "LocalColorD"),
                                        OutputColorA = RenderDatum.OutputColorValue_HO.PrimaryBackground,
                                        OutputColorB = RenderDatum.OutputColorValue_HO.PrimaryBackground,
                                        OutputColorC = RenderDatum.OutputColorValue_HO.HighlightForeground,
                                    };
                                    break;
                                case "frag_grenade_default":
                                    newRenderData = new RenderDatum
                                    {
                                        ShaderType = RenderDatum.ChudShaderType.Simple,
                                        BlendModeHO = RenderDatum.ChudBlendMode.AlphaBlend,
                                        LocalColorA = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorA, "LocalColorA"),
                                        LocalColorB = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorB, "LocalColorB"),
                                        LocalColorC = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorC, "LocalColorC"),
                                        LocalColorD = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorD, "LocalColorD"),
                                        OutputColorA = RenderDatum.OutputColorValue_HO.PrimaryBackground,
                                        OutputColorB = RenderDatum.OutputColorValue_HO.PrimaryBackground,
                                        OutputColorC = RenderDatum.OutputColorValue_HO.HighlightForeground,
                                    };
                                    break;
                                case "plasma_grenade_default":
                                    newRenderData = new RenderDatum
                                    {
                                        ShaderType = RenderDatum.ChudShaderType.Simple,
                                        BlendModeHO = RenderDatum.ChudBlendMode.AlphaBlend,
                                        LocalColorA = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorA, "LocalColorA"),
                                        LocalColorB = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorB, "LocalColorB"),
                                        LocalColorC = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorC, "LocalColorC"),
                                        LocalColorD = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorD, "LocalColorD"),
                                        OutputColorA = RenderDatum.OutputColorValue_HO.PrimaryBackground,
                                        OutputColorB = RenderDatum.OutputColorValue_HO.PrimaryBackground,
                                        OutputColorC = RenderDatum.OutputColorValue_HO.HighlightForeground,
                                    };
                                    break;
                                case "no_grenades":
                                    newRenderData = new RenderDatum
                                    {
                                        ShaderType = RenderDatum.ChudShaderType.Simple,
                                        BlendModeHO = RenderDatum.ChudBlendMode.AlphaBlend,
                                        LocalColorA = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorA, "LocalColorA"),
                                        LocalColorB = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorB, "LocalColorB"),
                                        LocalColorC = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorC, "LocalColorC"),
                                        LocalColorD = ConvertColorData(gen2ShaderInstance, newRenderData.LocalColorD, "LocalColorD"),
                                        OutputColorA = RenderDatum.OutputColorValue_HO.SecondaryBackground,
                                        OutputColorB = RenderDatum.OutputColorValue_HO.PrimaryBackground,
                                        OutputColorC = RenderDatum.OutputColorValue_HO.HighlightForeground,
                                    };
                                    break;
                                case "binocular_mask":
                                default:
                                    new TagToolWarning($"bitmap string '{bitmapWidgetString}' not yet supported");
                                    break;
                            }

                            newBitmapWidget.StateData.Add(newStateData);
                            newBitmapWidget.PlacementData.Add(newPlacementData);
                            newBitmapWidget.AnimationData.Add(newAnimationData);
                            newBitmapWidget.RenderData.Add(newRenderData);
                            hudWidget.BitmapWidgets.Add(newBitmapWidget);
                        }
                    }
                }

                // populate text widgets into their correct hud widgets
                for (int textIndex = 0; textIndex < gen2Text.Count; textIndex++)
                {
                    string textWidgetString = Cache.StringTable.GetString(gen2Text[textIndex].Name);
                    string hudWidgetString = Cache.StringTable.GetString(hudWidget.Name);

                    foreach (var widgetDictionaryEntry in widgetDictionary)
                    {
                        if (widgetDictionaryEntry.Key == textWidgetString && widgetDictionaryEntry.Value == hudWidgetString)
                        {
                            RealPoint2d realOffset = new RealPoint2d();
                            RenderDatum newRenderData = new RenderDatum();

                            switch (ConvertAnchor(nhdt.TextWidgets[textIndex].Anchor.ToString()))
                            {
                                // widget offset calculated from bitmap scale factor and offset to adjust for different anchor positions between engines
                                case "TopLeft":
                                    realOffset = new RealPoint2d
                                    {
                                        X = nhdt.TextWidgets[textIndex].FullscreenOffset.X - 50f,
                                        Y = (nhdt.TextWidgets[textIndex].FullscreenOffset.Y * 1.25f),
                                    };
                                    break;
                                case "TopRight":
                                    realOffset = new RealPoint2d
                                    {
                                        X = nhdt.TextWidgets[textIndex].FullscreenOffset.X - 5f,
                                        Y = nhdt.TextWidgets[textIndex].FullscreenOffset.Y * 1.25f,
                                    };
                                    break;
                                case "MotionSensor":
                                    realOffset = new RealPoint2d
                                    {
                                        X = nhdt.TextWidgets[textIndex].FullscreenOffset.X - 106f,
                                        Y = (nhdt.TextWidgets[textIndex].FullscreenOffset.Y * 1.25f) + 103f,
                                    };
                                    break;
                                case "BottomRight":
                                    realOffset = new RealPoint2d
                                    {
                                        X = nhdt.TextWidgets[textIndex].FullscreenOffset.X,
                                        Y = nhdt.TextWidgets[textIndex].FullscreenOffset.Y * 1.25f,
                                    };
                                    break;
                                case "Crosshair":
                                    realOffset = new RealPoint2d
                                    {
                                        X = nhdt.TextWidgets[textIndex].FullscreenOffset.X - 44f,
                                        Y = (nhdt.TextWidgets[textIndex].FullscreenOffset.Y * 1.25f) - 44f,
                                    };
                                    break;
                                default:
                                    realOffset = new RealPoint2d
                                    {
                                        X = nhdt.TextWidgets[textIndex].FullscreenOffset.X,
                                        Y = nhdt.TextWidgets[textIndex].FullscreenOffset.Y * 1.25f,
                                    };
                                    break;
                            }

                            string originalInput = Cache.StringTable.GetString(nhdt.TextWidgets[textIndex].String);
                            string convertedInput = null;
                            switch (originalInput)
                            {
                                case "hud_input1":
                                    convertedInput = "chud_out_a";
                                    break;
                                default:
                                    new TagToolWarning($"could not convert hud input string '{Gen2Cache.StringTable.GetString(nhdt.TextWidgets[textIndex].String)}'");
                                    break;
                            }

                            HudWidget.TextWidget newTextWidget = new HudWidget.TextWidget
                            {
                                Name = Cache.StringTable.GetStringId(textWidgetString),
                                InputString = Cache.StringTable.GetStringId(convertedInput),
                                StateData = new List<StateDatum>(),
                                PlacementData = new List<PlacementDatum>(),
                                AnimationData = new List<AnimationDatum>(),
                                RenderData = new List<RenderDatum>(),
                                Font = WidgetFontValue.FullscreenHudMessage,
                                WidgetIndex = widgetIndex
                            };
                            widgetIndex++;
                            StateDatum newStateData = new StateDatum { };
                            ProcessStateData(nhdt, textIndex, newStateData, "text");
                            PlacementDatum newPlacementData = new PlacementDatum
                            {
                                Anchor = (PlacementDatum.ChudAnchorType)Enum.Parse(typeof(PlacementDatum.ChudAnchorType), ConvertAnchor(nhdt.TextWidgets[textIndex].Anchor.ToString())),
                                Origin = new RealPoint2d { X = -1f, Y = -1f },
                                Offset = realOffset,
                                Scale = new RealPoint2d { X = 1.75f, Y = 1.75f },
                            };
                            AnimationDatum newAnimationData = new AnimationDatum();
                            PopulateAnimationData(newAnimationData, textWidgetString);

                            string extraData = null;
                            if (nhdt.TextWidgets[textIndex].Justification == HudTextWidgets.JustificationValue.Center)
                            {
                                extraData = "CenterHorizontally";
                            }
                            ConvertFlags(nhdt.TextWidgets[textIndex].Flags, newTextWidget, extraData);

                            CachedTag gen2Shader = gen2Hud.TextWidgets[textIndex].Shader;
                            ShaderGen2 gen2ShaderInstance = Gen2Cache.Deserialize<ShaderGen2>(gen2CacheStream, gen2Shader);

                            // Populate render data from SCRAPS I tell you, SCRAPS
                            switch (textWidgetString)
                            {
                                case "frag_count":
                                case "plasma_count":
                                    newRenderData = new RenderDatum
                                    {
                                        ShaderType = RenderDatum.ChudShaderType.TextSimple,
                                        BlendModeHO = RenderDatum.ChudBlendMode.AlphaBlend,
                                        ExternalInput = RenderDatum.ChudRenderExternalInputHO.GrenadeCount,
                                        OutputColorB = RenderDatum.OutputColorValue_HO.HighlightForeground,
                                    };
                                    break;
                                default:
                                    new TagToolWarning($"text string '{textWidgetString}' not yet supported");
                                    break;
                            }

                            newTextWidget.StateData.Add(newStateData);
                            newTextWidget.PlacementData.Add(newPlacementData);
                            newTextWidget.RenderData.Add(newRenderData);
                            hudWidget.TextWidgets.Add(newTextWidget);
                        }
                    }
                }

                switch (Cache.StringTable.GetString(hudWidget.Name))
                {
                    // Weapon stuff
                    case "crosshair":
                        hudWidget.ScriptingClass = ChudScriptingClass.Crosshair;
                        hudWidget.SortLayer = WidgetLayerEnum.Child;
                        break;
                    case "scope":
                        hudWidget.ScriptingClass = ChudScriptingClass.Crosshair;
                        hudWidget.SortLayer = WidgetLayerEnum.Background;
                        break;
                    case "ammo_area_left":
                        hudWidget.ScriptingClass = ChudScriptingClass.WeaponStats;
                        hudWidget.SortLayer = WidgetLayerEnum.Child;
                        break;
                    case "ammo_area_right":
                        hudWidget.ScriptingClass = ChudScriptingClass.WeaponStats;
                        hudWidget.SortLayer = WidgetLayerEnum.Child;
                        break;
                    case "ammo_area":
                        hudWidget.ScriptingClass = ChudScriptingClass.WeaponStats;
                        hudWidget.SortLayer = WidgetLayerEnum.Child;
                        break;
                    case "warning_flashes":
                        hudWidget.ScriptingClass = ChudScriptingClass.WeaponStats;
                        hudWidget.SortLayer = WidgetLayerEnum.Flashes;
                        break;
                    case "backpack":
                        hudWidget.ScriptingClass = ChudScriptingClass.WeaponStats;
                        hudWidget.SortLayer = WidgetLayerEnum.Child;
                        break;
                    case "frag_grenade":
                        hudWidget.ScriptingClass = ChudScriptingClass.Grenades;
                        hudWidget.SortLayer = WidgetLayerEnum.Parent;
                        break;
                    case "plasma_grenade":
                        hudWidget.ScriptingClass = ChudScriptingClass.Grenades;
                        hudWidget.SortLayer = WidgetLayerEnum.Parent;
                        break;
                    case "no_grenades":
                        hudWidget.ScriptingClass = ChudScriptingClass.Grenades;
                        hudWidget.SortLayer = WidgetLayerEnum.Parent;
                        break;

                    // Biped stuff
                    case "shield_meter":
                        hudWidget.ScriptingClass = ChudScriptingClass.Shield;
                        hudWidget.SortLayer = WidgetLayerEnum.Parent;
                        break;
                    case "motion_tracker":
                        hudWidget.ScriptingClass = ChudScriptingClass.MotionSensor;
                        hudWidget.SortLayer = WidgetLayerEnum.Parent;
                        break;
                    case "binoculars_wide_fullscreen":
                        hudWidget.ScriptingClass = ChudScriptingClass.Crosshair;
                        hudWidget.SortLayer = WidgetLayerEnum.Background;
                        break;
                    default:
                        new TagToolWarning($"Hud Widget '{Cache.StringTable[(int)hudWidget.Name.Value]}' found but not handled");
                        break;
                }
            }

            ChudAmmunitionInfo newHudAmmunitionInfo = new ChudAmmunitionInfo 
            {
                LowAmmoLoadedThreshold = nhdt.DashlightData.LowClipCutoff,
                LowAmmoReserveThreshold = nhdt.DashlightData.LowAmmoCutoff,
                LowBatteryThreshold = (int)nhdt.DashlightData.AgeCutoff,
            };
            chud.HudAmmunitionInfo = newHudAmmunitionInfo;

            switch (gen2Tag.Name.ToString())
            {
                case "ui\\hud\\masterchief":
                    UpdateCHGD(cacheStream, "masterchief");
                    break;
                case "ui\\hud\\dervish":
                    UpdateCHGD(cacheStream, "dervish");
                    break;
            }

            return chud;
        }

        public void AddWidgetDictionaryPair(string bitmapWidgetString, string hudWidgetString, Dictionary<string, string> widgetDictionary)
        {
            if (!widgetDictionary.ContainsKey(bitmapWidgetString))
            {
                widgetDictionary.Add(bitmapWidgetString, hudWidgetString);
            }
        }

        public void AddHudWidgets(ChudDefinition chud, Dictionary<string, string> widgetDictionary)
        {
            List<string> hudWidgets = new List<string>();
            foreach (var key in widgetDictionary.Values)
            {
                if (!hudWidgets.Contains(key))
                {
                    HudWidget newHudWidget = new HudWidget
                    {
                        Name = Cache.StringTable.GetStringId(key)
                    };

                    hudWidgets.Add(key);
                    chud.HudWidgets.Add(newHudWidget);
                }
            }
        }

        public AnimationDatum PopulateAnimationData(AnimationDatum animationData, string widgetString)
        {
            animationData.Active = new AnimationDatum.ChudWidgetAnimationStruct();
            animationData.Initializing = new AnimationDatum.ChudWidgetAnimationStruct();
            switch (widgetString)
            {
                case "binocular_mask":
                    animationData.Active.InputType = AnimationDatum.ChudWidgetAnimationStruct.ChudWidgetAnimationInputTypeEnum.Time;
                    animationData.Active.Animation = Cache.TagCacheGenHO.GetTag("ui\\chud\\animations\\zoom_scope.chud_animation_definition");
                    break;
                case "frag_grenade_default":
                case "frag_count":
                case "frag_count_out":
                case "plasma_grenade_default":
                case "plasma_count":
                case "plasma_count_out":
                case "no_grenades":
                case "shield_meter":
                case "shield_mask":
                case "motion_tracker_background":
                    animationData.Initializing.InputType = AnimationDatum.ChudWidgetAnimationStruct.ChudWidgetAnimationInputTypeEnum.Time;
                    animationData.Initializing.Animation = Cache.TagCacheGenHO.GetTag("ui\\chud\\animations\\boot_sequence\\default_boot_flash.chud_animation_definition");
                    break;
            }
            return animationData;
        }

        public uint PopulateRuntimeIndex(string bitmapWidgetName, uint hudWidgetIndex)
        {
            uint newWidgetIndex = 0;
            switch (bitmapWidgetName)
            {
                case "binocular_mask":
                    newWidgetIndex = 56;
                    break;
            }
            return newWidgetIndex;
        }


        public void ConvertFlags(dynamic gen2Flags, dynamic newData, string extraData)
        {
            var enumType = gen2Flags.GetType();
            List<string> results = new List<string>();
            if (extraData != null) { results.Add(extraData); }

            // Assuming ProcessFlagField processes individual flags and returns their string representations
            foreach (var field in Enum.GetValues(enumType))
            {
                // Convert the current field to a numeric value for bitwise comparison
                int flagValue = (ushort)field;
                int gen2FlagValue = Convert.ToInt32(gen2Flags);

                // Check if the flag is set in gen2BitmapFlags using bitwise AND
                if ((gen2FlagValue & flagValue) == flagValue)
                {
                    // This flag is set, process it
                    string flagName = Enum.GetName(enumType, field);
                    if (!string.IsNullOrEmpty(flagName))
                    {
                        switch (flagName)
                        {
                            case "ScopeMirrorHorizontally":
                                results.Add("MirrorHorizontally");
                                break;
                            case "ScopeMirrorVertically":
                                results.Add("MirrorVertically");
                                break;
                            case "ScopeStretch":
                                results.Add("Stretch");
                                break;
                            case "StringIsANumber":
                                results.Add("StringIsANumber");
                                break;
                            case "Force2DigitNumber":
                                results.Add("Force2DigitNumber");
                                break;
                            case "Force3DigitNumber":
                                results.Add("Force3DigitNumber");
                                break;
                            case "FlipVertically":
                            case "FlipHorizontally":
                            case "TalkingPlayerHack":
                            default:
                                new TagToolWarning($"match not found for bitmap flag type {flagName}");
                                break;
                        }
                    }
                }
            }

            foreach (var result in results)
            {
                // Use reflection to find the enum
                var field = newData.GetType().GetField("Flags") ?? newData.GetType().GetField("TextFlags");
                if (field != null && field.FieldType.IsEnum)
                {
                    // Get current enum value
                    object enumValue = field.GetValue(newData);

                    // Parse the bit to set
                    object bitValue = Enum.Parse(field.FieldType, result);

                    // Set the bit using bitwise OR
                    uint updatedValue = (uint)(Convert.ToUInt32(enumValue) | Convert.ToUInt32(bitValue));

                    // Set the updated value back
                    field.SetValue(newData, Enum.ToObject(field.FieldType, updatedValue));
                }
            }
            results.Clear();
        }

        private string ConvertAnchor(string input)
        {
            switch (input)
            {
                case "WeaponHud":
                    return "TopLeft";
                case "HealthAndShield":
                    return "TopRight";
                case "MotionSensor":
                    return "MotionSensor";
                case "Scorebaord":
                    return "BottomRight";
                case "Crosshair":
                    return "Crosshair";
                case "LockOnTarget":
                    return "WeaponTarget";
                default:
                    new TagToolWarning($"match not found for anchor type {input}");
                    return "Crosshair";
            }
        }

        public ArgbColor ConvertColorData(ShaderGen2 shader, ArgbColor newColor, string slot)
        {
            var h2_pixel_constants = new List<string>();
            string shader_template = shader.Template.Name;
            shader_template = shader_template.Split('\\').Last();

            switch(shader_template)
            {
                case "shield_test":
                    h2_pixel_constants.Add(""); // og: 
                    h2_pixel_constants.Add(""); // og: 
                    h2_pixel_constants.Add("LocalColorB"); // og: gradient_min_color?
                    h2_pixel_constants.Add("LocalColorC"); // og: gradient_max_color?
                    break;
                case "widget_gradient_flash":
                    h2_pixel_constants.Add(""); // og: meter_amount
                    h2_pixel_constants.Add(""); // og: gradient_max_color_normal
                    h2_pixel_constants.Add(""); // og: gradient_min_color_normal
                    h2_pixel_constants.Add(""); // og: gradient_max_color_flash
                    h2_pixel_constants.Add(""); // og: gradient_min_color_flash
                    h2_pixel_constants.Add("LocalColorA"); // og: foreground_color_normal
                    h2_pixel_constants.Add(""); // og: foreground_color_flash
                    break;
                case "ammo_meter":
                case "widget_legacy_meter":
                    h2_pixel_constants.Add(""); // og: meter_amount
                    h2_pixel_constants.Add("LocalColorA"); // og: meter_gradient_min
                    h2_pixel_constants.Add("LocalColorB"); // og: meter_gradient_max
                    h2_pixel_constants.Add("LocalColorC"); // og: meter_empty_color
                    break;
                case "dashlight_generic":
                case "player_training":
                case "text_flash":
                case "waypoint_arrow":
                case "waypoint_arrow_meter":
                case "waypoint_emblem":
                case "waypoint_icon_meter":
                case "widget_meter_custom":
                case "widget_simple_custom":
                case "widget_simple_flash":
                default:
                    new TagToolWarning($"unsupported hud shader template '{shader_template}'");
                    break;
            }
            for (int i = 0; i < h2_pixel_constants.Count; i++)
            {
                if (h2_pixel_constants[i] == slot)
                {
                    newColor.Red = shader.PostprocessDefinition[0].PixelConstants[i].Color.Red;
                    newColor.Green = shader.PostprocessDefinition[0].PixelConstants[i].Color.Green;
                    newColor.Blue = shader.PostprocessDefinition[0].PixelConstants[i].Color.Blue;
                    newColor.Alpha = shader.PostprocessDefinition[0].PixelConstants[i].Color.Alpha;
                    return newColor;
                }
            }
            return newColor;
        }

        public RenderDatum.ChudRenderExternalInputHO ConvertInput(HudBitmapWidgets.HudWidgetInputsStructBlock.Input1Value gen2Input, RenderDatum.ChudRenderExternalInputHO newInput)
        {
            switch (gen2Input.ToString())
            {
                case "WeaponHeat":
                    return RenderDatum.ChudRenderExternalInputHO.WeaponHeatFraction;
                case "WeaponClipAmmoFraction":
                    return RenderDatum.ChudRenderExternalInputHO.WeaponAmmoLoaded;
                case "UnitAutoaimed":
                    return RenderDatum.ChudRenderExternalInputHO.Autoaim;
                default:
                    return RenderDatum.ChudRenderExternalInputHO.Zero;
            }
        }

        public void ProcessStateData(NewHudDefinition nhdt, int i, StateDatum newStateData, string type)
        {
            List<string> results = new List<string>();
            switch (type)
            {
                case "text":
                    HudTextWidgets.HudWidgetStateDefinitionStructBlock gen2TextWidgetState = nhdt.TextWidgets[i].WidgetState;
                    var textFields = gen2TextWidgetState.GetType().GetFields();
                    bool textFlagEnables = false;

                    foreach (var field in textFields) // for every bitfield in the widget state
                    {
                        var fieldType = field.FieldType;

                        // Check if the field is an enum and the underlying type is ushort
                        if (fieldType.IsEnum && Enum.GetUnderlyingType(fieldType) == typeof(ushort))
                        {
                            if (field.Name.StartsWith("Y")) { textFlagEnables = true; }
                            else { textFlagEnables = false; }
                            ushort enumValue = (ushort)field.GetValue(gen2TextWidgetState);
                            List<string> newBits = ProcessEnumField(enumValue, fieldType, textFlagEnables);
                            results.AddRange(newBits);
                        }
                    }
                    break;
                case "bitmap":
                    HudBitmapWidgets.HudWidgetStateDefinitionStructBlock gen2BitmapWidgetState = nhdt.BitmapWidgets[i].WidgetState;
                    var bitmapFields = gen2BitmapWidgetState.GetType().GetFields();
                    bool bitmapFlagEnables = false;

                    foreach (var field in bitmapFields) // for every bitfield in the widget state
                    {
                        var fieldType = field.FieldType;

                        // Check if the field is an enum and the underlying type is ushort
                        if (fieldType.IsEnum && Enum.GetUnderlyingType(fieldType) == typeof(ushort))
                        {
                            if (field.Name.StartsWith("Y")) { bitmapFlagEnables = true; }
                            else { bitmapFlagEnables = false; }
                            ushort enumValue = (ushort)field.GetValue(gen2BitmapWidgetState);
                            List<string> newBits = ProcessEnumField(enumValue, fieldType, bitmapFlagEnables);
                            results.AddRange(newBits);
                        }
                    }
                    break;
            }

            results.Sort();
            foreach (var result in results)
            {
                string[] parts = result.Split('.');
                if (parts.Length == 2)
                {
                    string enumName = parts[0];
                    string bitName = parts[1];

                    // Use reflection to find the enum in StateDatum
                    var field = newStateData.GetType().GetField(enumName);
                    if (field != null && field.FieldType.IsEnum)
                    {
                        // Get current enum value
                        object enumValue = field.GetValue(newStateData);

                        // Parse the bit to set
                        object bitValue = Enum.Parse(field.FieldType, bitName);

                        // Set the bit using bitwise OR
                        ushort updatedValue = (ushort)(Convert.ToUInt16(enumValue) | Convert.ToUInt16(bitValue));

                        // Set the updated value back
                        field.SetValue(newStateData, Enum.ToObject(field.FieldType, updatedValue));
                    }
                }
            }
        }

        private List<string> ProcessEnumField(ushort enumValue, Type enumType, bool flagEnables)
        {
            List<string> results = new List<string>();

            foreach (var enumName in Enum.GetNames(enumType)) // for every bit in the bitfield
            {
                var value = (ushort)Enum.Parse(enumType, enumName);
                if ((enumValue & value) == value) // if bit is true
                {
                    if (flagEnables)
                    {
                        switch (enumName)
                        {
                            case "GrenadeTypeIsFrag":
                                results.Add("UnitGeneralFlags.SelectedFragGrenades");
                                break;
                            case "GrenadeTypeIsPlasma":
                                results.Add("UnitGeneralFlags.SelectedPlasmaGrenades");
                                break;
                            case "UnitIsSingleWielding":
                                results.Add("UnitInventoryFlags.IsSingleWielding");
                                break;
                            case "UnitIsDualWielding":
                                results.Add("UnitInventoryFlags.IsDualWielding");
                                break;
                            case "UnitIsUnzoomed":
                                Console.WriteLine("redundant zoom check, already handling");
                                break;
                            case "UnitIsZoomedLevel1":
                                results.Add("UnitZoomFlags.UnitIsZoomedLevel1");
                                break;
                            case "UnitIsZoomedLevel2":
                                results.Add("UnitZoomFlags.UnitIsZoomedLevel2");
                                break;
                            case "BinocularsEnabled":
                                results.Add("UnitGeneralFlags.BinocularsActive");
                                results.Add("WindowState.WideFull");
                                results.Add("WindowState.NativeFull");
                                break;
                            case "MotionSensorEnabled":
                                results.Add("UnitGeneralFlags.MotionTrackerEnabled");
                                break;
                            case "ShieldEnabled":
                                results.Add("UnitGeneralFlags.HasShields");
                                break;
                            case "AutoaimFriendly":
                                results.Add("WeaponTargetFlags.Friendly");
                                break;
                            case "AutoaimPlasma":
                                results.Add("WeaponTargetFlags.PlasmaTrack");
                                break;
                            case "AutoaimHeadshot":
                                results.Add("WeaponTargetFlags.EnemyHeadshot");
                                break;
                            case "AutoaimVulnerable":
                                results.Add("WeaponTargetFlags.EnemyWeakpoint");
                                break;
                            case "AutoaimInvincible":
                                results.Add("WeaponTargetFlags.Invincible");
                                break;
                            case "PrimaryWeapon":
                                results.Add("WeaponStatusFlags.SourceIsPrimaryWeapon");
                                break;
                            case "SecondaryWeapon":
                                results.Add("WeaponStatusFlags.SourceIsDualWeapon");
                                break;
                            case "BackpackWeapon":
                                results.Add("WeaponStatusFlags.SourceIsBackpacked");
                                break;
                            case "Overheated":
                                results.Add("Weapon_SpecialFlags.Overheated");
                                break;
                            case "OutOfAmmo":
                                results.Add("Weapon_SpecialFlags.AmmoEmpty");
                                break;
                            case "LockTargetAvailable":
                                results.Add("WeaponTargetFlags.LockAvailable");
                                break;
                            case "Locking":
                                results.Add("WeaponTargetBFlags.LockingOnAvailable");
                                break;
                            case "Locked":
                                results.Add("WeaponTargetBFlags.LockedOnAvailable");
                                break;
                            case "CampaignSolo":
                                results.Add("GameState.CampaignSolo");
                                break;
                            case "CampaignCoop":
                                results.Add("GameState.CampaignCoop");
                                break;
                            case "FreeForAll":
                                results.Add("GameState.FreeForAll");
                                break;
                            case "TeamGame":
                                results.Add("GameState.TeamGame");
                                break;
                            case "Default":
                            case "GrenadeTypeIsNone":
                            case "GrenadesDisabled":
                            case "Dervish":
                            case "AgeBelowCutoff":
                            case "ClipBelowCutoff":
                            case "TotalBelowCutoff":
                            case "UserLeading":
                            case "UserNotLeading":
                            case "TimedGame":
                            case "UntimedGame":
                            case "OtherScoreValid":
                            case "OtherScoreInvalid":
                            case "PlayerIsArmingBomb":
                            case "PlayerTalking":
                            default:
                                Console.WriteLine($"'{enumName}' was detected as state enabling flag but no matches were found");
                                break;
                        }
                    }
                    if (!flagEnables)
                    {
                        switch (enumName)
                        {
                            case "UnitIsZoomedLevel1":
                            case "UnitIsZoomedLevel2":
                            case "BinocularsEnabled":
                                results.Add("InverseFlags.NotZoomedIn");
                                break;
                            case "MotionSensorEnabled":
                                results.Add("UnitGeneralFlags.MotionTrackerDisabled");
                                break;
                            case "Default":
                            case "GrenadeTypeIsNone":
                            case "GrenadeTypeIsFrag":
                            case "GrenadeTypeIsPlasma":
                            case "UnitIsSingleWielding":
                            case "UnitIsDualWielding":
                            case "GrenadesDisabled":
                            case "ShieldEnabled":
                            case "Dervish":
                            case "AutoaimFriendly":
                            case "AutoaimPlasma":
                            case "AutoaimHeadshot":
                            case "AutoaimVulnerable":
                            case "AutoaimInvincible":
                            case "PrimaryWeapon":
                            case "SecondaryWeapon":
                            case "BackpackWeapon":
                            case "AgeBelowCutoff":
                            case "ClipBelowCutoff":
                            case "TotalBelowCutoff":
                            case "Overheated":
                            case "OutOfAmmo":
                            case "LockTargetAvailable":
                            case "Locking":
                            case "Locked":
                            case "CampaignSolo":
                            case "CampaignCoop":
                            case "FreeForAll":
                            case "TeamGame":
                            case "UserLeading":
                            case "UserNotLeading":
                            case "TimedGame":
                            case "UntimedGame":
                            case "OtherScoreValid":
                            case "OtherScoreInvalid":
                            case "PlayerIsArmingBomb":
                            case "PlayerTalking":
                            default:
                                Console.WriteLine($"'{enumName}' unmatched state disabling flag ignored");
                                break;
                        }
                    }
                }
            }
            return results;
        }

        public void UpdateCHGD(Stream cacheStream, string unit)
        {
            switch (unit)
            {
                case "masterchief":
                    CachedTag hudGlobalsTag = Cache.TagCacheGenHO.GetTag("ui\\chud\\globals.chud_globals_definition");
                    ChudGlobalsDefinition hudGlobals = Cache.Deserialize<ChudGlobalsDefinition>(cacheStream, hudGlobalsTag);
                    hudGlobals.HudGlobals[0].HudAttributes[0].WarpSourceFovY = Angle.FromDegrees(1f);
                    hudGlobals.HudGlobals[0].HudAttributes[0].WarpAmount = 1;
                    hudGlobals.HudGlobals[0].HudAttributes[0].MotionSensorOrigin = new RealPoint2d(58f, 976f);
                    hudGlobals.HudGlobals[0].PrimaryBackground = new ArgbColor(0, 3, 28, 56);
                    hudGlobals.HudGlobals[0].SecondaryBackground = new ArgbColor(0, 36, 139, 255);
                    hudGlobals.HudGlobals[0].HighlightForeground = new ArgbColor(0, 58, 108, 255);
                    Cache.Serialize(cacheStream, hudGlobalsTag, hudGlobals);
                    break;
                case "dervish":
                    CachedTag hudGlobalsTagElite = Cache.TagCacheGenHO.GetTag("objects\\characters\\elite\\mp_elite\\chud\\globals.chud_globals_definition");
                    ChudGlobalsDefinition hudGlobalsElite = Cache.Deserialize<ChudGlobalsDefinition>(cacheStream, hudGlobalsTagElite);
                    hudGlobalsElite.HudGlobals[0].HudAttributes[0].WarpSourceFovY = Angle.FromDegrees(1f);
                    hudGlobalsElite.HudGlobals[0].HudAttributes[0].WarpAmount = 1;
                    hudGlobalsElite.HudGlobals[0].HudAttributes[0].MotionSensorOrigin = new RealPoint2d(58f, 976f);
                    Cache.Serialize(cacheStream, hudGlobalsTagElite, hudGlobalsElite);
                    break;
            }
        }
    }
}
