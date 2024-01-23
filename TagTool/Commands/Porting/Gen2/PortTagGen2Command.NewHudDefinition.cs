using Assimp;
using Poly2Tri.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Audio;
using TagTool.Cache.HaloOnline;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Definitions.Common;
using TagTool.Tags.Definitions.Gen2;
using TagTool.Tags.Definitions.Gen4;
using static TagTool.Tags.Definitions.ChudDefinition;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        public TagStructure ConvertNewHudDefinition(NewHudDefinition nhdt)
        {
            ChudDefinition chud = new ChudDefinition();
            var gen2Bitmaps = nhdt.BitmapWidgets;

            string[,] HudWidgetMaps = new string[,]
                  {
                      { "crosshair", "crosshair" },
                      { "crosshair", "crosshair_friend" },
                      { "crosshair", "crosshair_weakspot" },
                      { "crosshair", "crosshair_plasma_tracking" },
                      { "ammo_area_left", "weapon_background_left" },
                      { "ammo_area_left", "weapon_background_left_out" },
                      { "ammo_area_left", "heat_meter_left" },
                      { "ammo_area_left", "ammo_meter_left" },
                      { "ammo_area_right", "weapon_background_right" },
                      { "ammo_area_right", "weapon_background_right_out" },
                      { "ammo_area_right", "heat_meter_right" },
                      { "ammo_area_right", "ammo_meter_right" },
                      { "ammo_area_right", "heat_meter_single" },
                      { "ammo_area_right", "ammo_meter_single" },
                      { "ammo_area_right", "weapon_background_single" },
                      { "warning_flashes", "heat_background_left" },
                      { "warning_flashes", "heat_background_right" },
                      { "scope", "scope_mask" },
                      { "backpack", "backpack" }
                  };

            // setup hud widgets blocks
            HashSet<string> uniqueEntriesSet = new HashSet<string>();
            for (int i = 0; i < HudWidgetMaps.GetLength(0); i++)
            {
                uniqueEntriesSet.Add(HudWidgetMaps[i, 0]);
            }

            chud.HudWidgets = new List<HudWidget>(uniqueEntriesSet.Count);
            foreach (var entry in uniqueEntriesSet)
            {
                bool entryMatchFound = false;

                foreach (var bitmap in gen2Bitmaps)
                {
                    for (int i = 0; i < HudWidgetMaps.GetLength(0); i++)
                    {
                        string bitmapWidgetString = Cache.StringTable[(int)bitmap.Name.Value];
                        // Check if the bitmap widget name matches the second column entry in HudWidgetMaps
                        if (entry == HudWidgetMaps[i, 0] && bitmapWidgetString == HudWidgetMaps[i, 1] && !entryMatchFound)
                        {
                            try
                            {
                                // Create a new HudWidget and add it to chud.HudWidgets
                                HudWidget newHudWidget = new HudWidget
                                {
                                    // Assign the name of the new hud widget to the corresponding first dimension of the item in the array
                                    Name = Cache.StringTable.GetStringId((HudWidgetMaps[i, 0]))
                                };

                                chud.HudWidgets.Add(newHudWidget);
                                entryMatchFound = true;
                                break; // Exit the loop once a match is found
                            }
                            catch
                            {
                                new TagToolWarning($"bitmap stringid '{bitmap.Name}' not mapped in HudWidgetMaps");
                            }
                        }
                    }
                }
            }

            // populate bitmaps into their correct hud widgets
            foreach (var hudWidget in chud.HudWidgets)
            {
                hudWidget.BitmapWidgets = new List<HudWidget.BitmapWidget>();

                for (int bitmapIndex = 0; bitmapIndex < gen2Bitmaps.Count; bitmapIndex++)
                {
                    //hudWidget.BitmapWidgets[bitmapIndex].PlacementData = new List<HudWidget.BitmapWidget.PlacementDatum>();

                    string bitmapWidgetString = Cache.StringTable[(int)gen2Bitmaps[bitmapIndex].Name.Value];
                    string hudWidgetString = Cache.StringTable[(int)hudWidget.Name.Value];

                    for (int i = 0; i < HudWidgetMaps.GetLength(0); i++)
                    {
                        if (hudWidgetString == HudWidgetMaps[i, 0] && bitmapWidgetString == HudWidgetMaps[i, 1])
                        {
                            RealPoint2d realOffset = new RealPoint2d
                            {
                                X = nhdt.BitmapWidgets[bitmapIndex].FullscreenOffset.X,
                                Y = nhdt.BitmapWidgets[bitmapIndex].FullscreenOffset.Y,
                            };

                            HudWidget.BitmapWidget newBitmapWidget = new HudWidget.BitmapWidget
                            {
                                Name = Cache.StringTable.GetStringId((HudWidgetMaps[i, 1])),
                                Bitmap = Cache.TagCacheGenHO.GetTag(nhdt.BitmapWidgets[bitmapIndex].Bitmap.ToString()),
                                BitmapSequenceIndex = (byte)nhdt.BitmapWidgets[bitmapIndex].FullscreenSequenceIndex,
                            };
                            //HudWidget.BitmapWidget.PlacementDatum newPlacementData = new HudWidget.BitmapWidget.PlacementDatum
                            //{
                            //    Origin = nhdt.BitmapWidgets[bitmapIndex].FullscreenRegistrationPoint,
                            //    Offset = realOffset,
                            //};

                            hudWidget.BitmapWidgets.Add(newBitmapWidget);
                            //hudWidget.BitmapWidgets[bitmapIndex].PlacementData.Add(newPlacementData);
                        }
                    }
                }

                //switch (hudWidget.Name)
                //{
                //    case Cache.StringTable.GetStringId("crosshair"):
                //    case Cache.StringTable.GetStringId("ammo_area_left"):
                //    case Cache.StringTable.GetStringId("ammo_area_right"):
                //    case Cache.StringTable.GetStringId("warning_flashes"):
                //    case Cache.StringTable.GetStringId("backpack"):
                //        //do stuff
                //        break;
                //}
            }
            return chud;
        }
    }
}
