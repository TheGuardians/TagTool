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
using static TagTool.Tags.Definitions.ChudDefinition.HudWidgetBase;
using static TagTool.Tags.Definitions.Gen2.NewHudDefinition;

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
                    string bitmapWidgetString = Cache.StringTable[(int)gen2Bitmaps[bitmapIndex].Name.Value];
                    string hudWidgetString = Cache.StringTable[(int)hudWidget.Name.Value];
                    string anchorString = gen2Bitmaps[bitmapIndex].Anchor.ToString();

                    // populate bitmaps and subblocks
                    for (int i = 0; i < HudWidgetMaps.GetLength(0); i++)
                    {
                        if (hudWidgetString == HudWidgetMaps[i, 0] && bitmapWidgetString == HudWidgetMaps[i, 1])
                        {
                                
                            HudWidget.BitmapWidget newBitmapWidget = new HudWidget.BitmapWidget
                            {
                                Name = Cache.StringTable.GetStringId((HudWidgetMaps[i, 1])),
                                Bitmap = Cache.TagCacheGenHO.GetTag(nhdt.BitmapWidgets[bitmapIndex].Bitmap.ToString()),
                                BitmapSequenceIndex = (byte)nhdt.BitmapWidgets[bitmapIndex].FullscreenSequenceIndex,
                                PlacementData = new List<HudWidgetBase.PlacementDatum>(),
                                StateData = new List<HudWidgetBase.StateDatum>(),
                            };

                            RealPoint2d realOffset = new RealPoint2d()
                            {
                                X = nhdt.BitmapWidgets[bitmapIndex].FullscreenOffset.X / 2,
                                Y = nhdt.BitmapWidgets[bitmapIndex].FullscreenOffset.Y / 2,
                            };
                            HudWidgetBase.PlacementDatum newPlacementData = new HudWidgetBase.PlacementDatum
                            {
                                Anchor = (HudWidgetBase.PlacementDatum.ChudAnchorType)Enum.Parse(typeof(HudWidgetBase.PlacementDatum.ChudAnchorType), ConvertAnchor(nhdt.BitmapWidgets[bitmapIndex].Anchor.ToString())),
                                Origin = nhdt.BitmapWidgets[bitmapIndex].FullscreenRegistrationPoint,
                                Offset = realOffset,
                                Scale = new RealPoint2d { X = 1, Y = 1 },
                            };
                            HudWidgetBase.StateDatum newStateData = new HudWidgetBase.StateDatum { };
                            ProcessBitmapStateData(nhdt.BitmapWidgets[bitmapIndex].WidgetState, newStateData);    

                            newBitmapWidget.StateData.Add(newStateData);
                            newBitmapWidget.PlacementData.Add(newPlacementData);
                            hudWidget.BitmapWidgets.Add(newBitmapWidget);
                        }
                    }
                }

                switch (Cache.StringTable[(int)hudWidget.Name.Value])
                {
                    case "crosshair":
                    case "ammo_area_left":
                    case "ammo_area_right":
                    case "warning_flashes":
                    case "backpack":
                        //do stuff
                        break;
                }
            }
            return chud;
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
                    return "BottomLeft";
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

        public void ProcessBitmapStateData(HudBitmapWidgets.HudWidgetStateDefinitionStructBlock gen2WidgetState, StateDatum newStateData)
        {
            var fields = gen2WidgetState.GetType().GetFields();
            List<string> results = new List<string>();
            bool flagEnables = false;

            foreach (var field in fields) // for every bitfield in the widget state
            {
                var fieldType = field.FieldType;

                // Check if the field is an enum and the underlying type is ushort
                if (fieldType.IsEnum && Enum.GetUnderlyingType(fieldType) == typeof(ushort))
                {
                    if (field.Name.StartsWith("Y")) { flagEnables = true; }
                    ushort enumValue = (ushort)field.GetValue(gen2WidgetState);
                    List<string> newBits = ProcessEnumField(enumValue, fieldType, flagEnables);
                    results.AddRange(newBits);
                }
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
                                results.Add("UnitZoomFlags.BinocularsEnabled");
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
                    else if (!flagEnables)
                    {
                        switch (enumName)
                        {
                            case "UnitIsUnzoomed":
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
                            case "UnitIsZoomedLevel1":
                            case "UnitIsZoomedLevel2":
                            case "GrenadesDisabled":
                            case "BinocularsEnabled":
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

        public String ConvertStateFlags(HudWidget hudWidget, int bitmapIndex, NewHudDefinition nhdt)
        {
            string convertedFlag = null;
            var flagBlocks = nhdt.BitmapWidgets[bitmapIndex].WidgetState;

            return convertedFlag;
        }

    }
}
