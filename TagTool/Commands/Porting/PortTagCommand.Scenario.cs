using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Scripting;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private Cinematic ConvertCinematic(Cinematic cine)
        {
            return cine;
        }

        private CinematicScene ConvertCinematicScene(CinematicScene cisc)
        {
            if (BlamCache.Version == CacheVersion.Halo3Retail)
            {
                foreach (var shot in cisc.Shots)
                {
                    shot.ScreenEffects = new List<CinematicScene.ShotBlock.ScreenEffectBlock>();
                    shot.Unknown5 = 0;
                    shot.Unknown6 = 0;
                    shot.Unknown7 = 0;
                }
            }
            return cisc;
        }

        private Scenario ConvertScenario(Scenario scnr, string tagName)
        {
            //
            // Ai Pathfinding block conversion 
            //

            if (BlamCache.Version == CacheVersion.Halo3Retail)
            {
                foreach (var pathfindingdata in scnr.AiPathfindingData)
                {
                    foreach (var block in pathfindingdata.Unknown7)
                        block.Unknown19 = new List<Scenario.AiPathfindingDatum.UnknownBlock7.UnknownBlock>();

                    foreach (var block in pathfindingdata.Unknown9)
                        block.Unknown1 = block.UnknownH3;
                }
            }

            //
            // Temporary fixes
            //

            // Cheap color fix for now

            foreach(var title in scnr.CutsceneTitles)
            {
                title.TextColor = new ArgbColor(title.TextColor.Blue, title.TextColor.Green, title.TextColor.Red, title.TextColor.Alpha);
                title.ShadowColor = new ArgbColor(title.ShadowColor.Blue, title.ShadowColor.Green, title.ShadowColor.Red, title.ShadowColor.Alpha);
            }

            // Null cubemaps until shaders and bitmaps are fixed
            foreach (var sbspblock in scnr.StructureBsps)
                sbspblock.Cubemap = null;

            //
            // Convert Squads
            //

            if (BlamCache.Version == CacheVersion.Halo3Retail)
            {
                foreach (var squad in scnr.Squads)
                {
                    //First section is correct. From base squad, create spawn points and designer cell  blocks. The rest is null

                    squad.SpawnFormations = new List<Scenario.Squad.SpawnFormation>();
                    squad.SpawnPoints = new List<Scenario.Squad.SpawnPoint>();
                    squad.SquadTemplate = null;
                    squad.TemplatedCells = new List<Scenario.Squad.Cell>();
                    squad.DesignerCells = new List<Scenario.Squad.Cell>();

                    foreach (var baseSquad in squad.BaseSquad)
                    {
                        //
                        // Convert StringIds?
                        //

                        baseSquad.InitialState = ConvertStringId(baseSquad.InitialState);

                        foreach (var spawnpoint in baseSquad.StartingLocations)
                        {
                            spawnpoint.Name = ConvertStringId(spawnpoint.Name);
                            spawnpoint.ActorVariant = ConvertStringId(spawnpoint.ActorVariant);
                            spawnpoint.VehicleVariant = ConvertStringId(spawnpoint.VehicleVariant);

                            foreach (var squadpoint in spawnpoint.Points)
                            {
                                squadpoint.ActivityName = ConvertStringId(squadpoint.ActivityName);
                            }
                        }



                        //Append all starting locations from all baseSquads into Spawnpoints
                        foreach (var spawnpoint in baseSquad.StartingLocations)
                            squad.SpawnPoints.Add(spawnpoint);

                        Scenario.Squad.Cell designer = new Scenario.Squad.Cell
                        {
                            Name = new StringId(0),
                            DifficultyFlags = baseSquad.DifficultyFlags,
                            Count = baseSquad.Count,

                            InitialWeapon = new List<Scenario.Squad.Cell.ItemBlock>(),
                            InitialSecondaryWeapon = new List<Scenario.Squad.Cell.ItemBlock>(),
                            InitialEquipment = new List<Scenario.Squad.Cell.ItemBlock>(),
                            CharacterType = new List<Scenario.Squad.Cell.CharacterTypeBlock>()
                        };

                        //Add character type
                        if (baseSquad.CharacterType != -1)
                        {
                            designer.CharacterType.Add(new Scenario.Squad.Cell.CharacterTypeBlock
                            {
                                CharacterTypeIndex = baseSquad.CharacterType,
                                Chance = 1
                            });
                        }

                        //Add initial weapon
                        if (baseSquad.InitialPrimaryWeapon != -1)
                        {
                            designer.InitialWeapon.Add(new Scenario.Squad.Cell.ItemBlock
                            {
                                Weapon2 = baseSquad.InitialPrimaryWeapon,
                                Probability = 1
                            });
                        }

                        //Add secondary initial weapon
                        if (baseSquad.InitialSecondaryWeapon != -1)
                        {
                            designer.InitialSecondaryWeapon.Add(new Scenario.Squad.Cell.ItemBlock
                            {
                                Weapon2 = baseSquad.InitialSecondaryWeapon,
                                Probability = 1
                            });
                        }

                        //Add equipment
                        if (baseSquad.Equipment != -1)
                        {
                            designer.InitialEquipment.Add(new Scenario.Squad.Cell.ItemBlock
                            {
                                Weapon2 = baseSquad.Equipment,
                                Probability = 1
                            });
                        }

                        //Add grenade type
                        designer.GrenadeType = baseSquad.GrenadeType;

                        designer.VehicleVariant = new StringId(baseSquad.VehicleVariant.Value);
                        designer.VehicleTypeIndex = baseSquad.Vehicle;

                        designer.Unknown14 = -1;
                        designer.Unknown17 = -1;

                        //Add new cell to the list
                        squad.DesignerCells.Add(designer);
                    }
                }
            }

            //
            // Add prematch camera position
            //

            bool createPrematchCamera = false;

            RealPoint3d position = new RealPoint3d();
            float yaw = 0.0f;
            float pitch = 0.0f;

            switch (tagName)
            {
                case "levels\\dlc\\chillout\\chillout":
                    createPrematchCamera = true;
                    position = new RealPoint3d(-6.559f, 4.763f, 0.699f);
                    yaw = 239.89f;
                    pitch = -1.36f;
                    break;

                case "levels\\dlc\\descent\\descent":
                    createPrematchCamera = true;
                    position = new RealPoint3d(9.259f, 3.942f, -17.249f);
                    yaw = 188.57f;
                    pitch = -12.60f;
                    break;

                case "levels\\multi\\salvation\\salvation":
                    createPrematchCamera = true;
                    position = new RealPoint3d(1.522f, 16.490f, 4.648f);
                    yaw = 252.14f;
                    pitch = -13.68f;
                    break;

                case "levels\\dlc\\armory\\armory":
                    createPrematchCamera = true;
                    position = new RealPoint3d(-33.057f, -17.607f, -7.658f);
                    yaw = 48.44f;
                    pitch = -2.48f;
                    break;
            }

            if(createPrematchCamera)
                scnr.CutsceneCameraPoints = new List<Scenario.CutsceneCameraPoint>() { MultiplayerPrematchCamera(position, yaw, pitch) };
            
            //
            // Convert scripts
            //

            if (Flags.HasFlag(PortingFlags.ConvertScripts))
            {
                foreach (var global in scnr.Globals)
                {
                    ConvertScriptValueType(global.Type);
                }

                foreach (var script in scnr.Scripts)
                {
                    ConvertScriptValueType(script.ReturnType);

                    foreach (var parameter in script.Parameters)
                        ConvertScriptValueType(parameter.Type);
                }

                foreach (var expr in scnr.ScriptExpressions)
                {
                    ConvertScriptExpression(scnr, expr);
                }

                AdjustScripts(scnr, tagName);
            }
            else
            {
                scnr.Globals = new List<ScriptGlobal>();
                scnr.Scripts = new List<Script>();
                scnr.ScriptExpressions = new List<ScriptExpression>();
            }

            return scnr;
        }

        /// <summary>
        /// Given the position and the yaw/pitch given by the camera coordinates, create a CutsceneCameraPoint block. Note that roll is always 0 in the coordinates.
        /// </summary>
        public Scenario.CutsceneCameraPoint MultiplayerPrematchCamera(RealPoint3d position, float yaw, float pitch)
        {
            var orientation = ConvertXYZtoZYX(new RealEulerAngles3d(Angle.FromDegrees(yaw), Angle.FromDegrees(pitch), Angle.FromDegrees(0)));

            var camera = new Scenario.CutsceneCameraPoint()
            {
                Flags = Scenario.CutsceneCameraPointFlags.PrematchCameraHack,
                Type = Scenario.CutsceneCameraPointType.Normal,
                Name = "prematch_camera",
                Position = position,
                Orientation = orientation

            };
            return camera;
        }

        /// <summary>
        /// Convert from the Tait-Bryan XYZ to ZYX coordinate, assuming input Z = 0.
        /// </summary>
        public RealEulerAngles3d ConvertXYZtoZYX(RealEulerAngles3d angle)
        {
            var x1 = angle.Yaw.Radians;
            var y1 = angle.Pitch.Radians;

            var y2 = (float)Math.Asin(Math.Cos(x1) * Math.Sin(y1));
            var z2 = (float)Math.Acos(Math.Cos(y1) / Math.Cos(y2));
            var x2 = x1;

            return new RealEulerAngles3d(Angle.FromRadians(x2), Angle.FromRadians(y2), Angle.FromRadians(z2));
        }

        public void ConvertScriptExpression(Scenario scnr, ScriptExpression expr)
        {
            if (expr.Opcode == 0xBABA)
                return;

            ConvertScriptValueType(expr.ValueType);

            switch (expr.ExpressionType)
            {
                case ScriptExpressionType.Expression:
                case ScriptExpressionType.Group:
                    if (ScriptExpressionIsValue(expr))
                        ConvertScriptValueOpcode(expr);
                    else
                    {
                        if (!ConvertScriptUsingPresets(scnr, expr))
                            ConvertScriptExpressionOpcode(scnr, expr);
                    }
                    break;

                case ScriptExpressionType.ScriptReference: // The opcode is the tagblock index of the script it uses. Don't convert opcode
                case ScriptExpressionType.GlobalsReference: // The opcode is the tagblock index of the global it uses. Don't convert opcode
                case ScriptExpressionType.ParameterReference: // Probably as above
                    break;

                default:
                    break;
            }

            ConvertScriptExpressionData(expr);
        }

        public bool ScriptExpressionIsValue(ScriptExpression expr)
        {
            switch (expr.ExpressionType)
            {
                case ScriptExpressionType.ParameterReference:
                    return true;

                case ScriptExpressionType.Expression:
                    if ((int)expr.ValueType.HaloOnline > 0x4)
                        return true;
                    else
                        return false;

                case ScriptExpressionType.ScriptReference: // The opcode is the tagblock index of the script it uses, so ignore
                case ScriptExpressionType.GlobalsReference: // The opcode is the tagblock index of the global it uses, so ignore
                case ScriptExpressionType.Group:
                    return false;

                default:
                    return false;
            }
        }

        public void ConvertScriptValueType(ScriptValueType type)
        {
            if (!Enum.TryParse(
                BlamCache.Version == CacheVersion.Halo3Retail ?
                    type.Halo3Retail.ToString() :
                    type.Halo3ODST.ToString(),
                out type.HaloOnline))
            {
                throw new NotImplementedException(
                    BlamCache.Version == CacheVersion.Halo3Retail ?
                        type.Halo3Retail.ToString() :
                        type.Halo3ODST.ToString());
            }

            if ((int)type.HaloOnline == 255)
                type.HaloOnline = ScriptValueType.HaloOnlineValue.Invalid;
        }

        public void ConvertScriptValueOpcode(ScriptExpression expr)
        {
            if (expr.Opcode == 0xFFFF || expr.Opcode == 0xBABA || expr.Opcode == 0x0000)
                return;

            switch (expr.ExpressionType)
            {
                case ScriptExpressionType.Expression:
                case ScriptExpressionType.Group:
                    break;

                case ScriptExpressionType.ScriptReference: // The opcode is the tagblock index of the script it uses. Don't convert opcode
                case ScriptExpressionType.GlobalsReference: // The opcode is the tagblock index of the global it uses. Don't convert opcode
                case ScriptExpressionType.ParameterReference: // Probably as above
                    return;

                default:
                    return;
            }
            
            // Some script expressions use opcode as a script reference. Only continue if it is a reference
            if (!ScriptInfo.ValueTypes[BlamCache.Version].ContainsKey(expr.Opcode))
            {
                Console.WriteLine($"ERROR: not in {BlamCache.Version} opcode table 0x{expr.Opcode:X3}.");

                return;
            }

            var blamValueTypeName = ScriptInfo.ValueTypes[BlamCache.Version][expr.Opcode];

            foreach (var valueType in ScriptInfo.ValueTypes[CacheContext.Version])
            {
                if (valueType.Value == blamValueTypeName)
                {
                    expr.Opcode = (ushort)valueType.Key;
                    break;
                }
            }
        }
        
        public void ConvertScriptExpressionUnsupportedOpcode(ScriptExpression expr)
        {
            if (expr.Opcode == 0xBABA)
                return;

            switch (expr.Opcode)
            {
                // These should have never converted due to opcode name mismatch

                case 0x2CE: // camera_set_animation_relative_with_speed_loop
                case 0x2CF: // camera_set_animation_relative_with_speed_loop_offset
                case 0x070: // object_copy_player_appearance this one's a pos
                case 0x676: // player_set_look_training_hack
                case 0x1D3: // chud_show_ai_navpoint
                case 0x2F5: // player_action_test_vision_trigger
                case 0x302: // player_action_test_unknown1
                case 0x303: // player_action_test_start
                case 0x30F: // unit_action_test_grenade_trigger
                case 0x31D: // unit_action_test_unknown1
                case 0x31E: // unit_action_test_unknown2
                case 0x32E: // player_action_test_1?
                case 0x32F: // player_action_test_2?
                case 0x640: // player_inside_active_beacon
                case 0x668: // player_add_footprint
                case 0x66C: // player_set_nav_enabled
                case 0x66D: // player_set_fourth_wall_enabled
                case 0x66E: // player_set_objectives_enabled

                case 0x4D5: // net_delegate_host
                case 0x4D6: // net_delegate_leader
                case 0x4DA: // net_player_color
                case 0x526: // bug_now
                case 0x527: // bug_now_lite
                case 0x528: // bug_now_auto

                case 0x427: // chud_display_pda_minimap_message
                case 0x428: // chud_display_pda_objectives_message
                case 0x429: // chud_display_pda_communications_message
                case 0x42B: // chud_show_object_navpoint
                case 0x678: // device_arg_has_been_touched_by_unit
                case 0x66B: // player_set_pda_enabled
                case 0x66F: // player_force_pda
                case 0x670: // player_close_pda
                case 0x63B: // pda_activate_beacon
                case 0x63D: // pda_activate_marker
                case 0x63E: // pda_activate_marker_named
                case 0x63F: // pda_beacon_is_selected
                case 0x66A: // pda_is_active_deterministic
                case 0x671: // pda_set_footprint_dead_zone
                case 0x674: // pda_play_arg_sound
                case 0x675: // pda_stop_arg_sound
                case 0x677: // pda_set_active_pda_definition
                case 0x67B: // pda_input_enable
                case 0x67F: // pda_input_enable_a
                case 0x680: // pda_input_enable_dismiss
                case 0x681: // pda_input_enable_x
                case 0x682: // pda_input_enable_y
                case 0x683: // pda_input_enable_dpad
                case 0x684: // pda_input_enable_analog_sticks
                    expr.Opcode = 0xCDCD;
                    break;

                default:
                    break;
            }
        }

        public void ConvertScriptExpressionOpcode(Scenario scnr, ScriptExpression expr)
        {
            switch (expr.ExpressionType)
            {
                case ScriptExpressionType.Expression:
                    if (expr.ExpressionType == ScriptExpressionType.ScriptReference)
                        return;
                    
                    // If the previous scriptExpr is a scriptRef, don't convert. The opcode is the script reference. They always come in pairs.
                    if (scnr.ScriptExpressions[scnr.ScriptExpressions.IndexOf(expr) - 1].ExpressionType == ScriptExpressionType.ScriptReference)
                        return;

                    break;

                case ScriptExpressionType.Group:
                    break;

                case ScriptExpressionType.ScriptReference: // The opcode is the tagblock index of the script it uses. Don't convert opcode
                case ScriptExpressionType.GlobalsReference: // The opcode is the tagblock index of the global it uses. Don't convert opcode
                case ScriptExpressionType.ParameterReference: // Probably as above
                    return;

                default:
                    return;
            }

            if (!ScriptInfo.Scripts[BlamCache.Version].ContainsKey(expr.Opcode))
            {
                Console.WriteLine($"ERROR: not in {BlamCache.Version} opcode table: 0x{expr.Opcode:X3}. (ConvertScriptExpressionOpcode)");
                return;
            }

            var blamScript = ScriptInfo.Scripts[BlamCache.Version][expr.Opcode];

            var match = true;

            foreach (var entry in ScriptInfo.Scripts[CacheContext.Version])
            {
                var newOpcode = entry.Key;
                var newScript = entry.Value;

                if (newScript.Name != blamScript.Name)
                    continue;

                if (newScript.Arguments.Count != blamScript.Arguments.Count)
                    continue;

                for (var k = 0; k < newScript.Arguments.Count; k++)
                {
                    if (newScript.Arguments[k].Type != blamScript.Arguments[k].Type)
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    expr.Opcode = (ushort)newOpcode;
                    return;
                }
            }

            // If it manages to bypass the loop without returning true, then no match was found.
            Console.WriteLine($"ERROR: no equivalent was found for 0x{expr.Opcode:X3}");
            ConvertScriptExpressionUnsupportedOpcode(expr);
        }

        public void ConvertScriptExpressionData(ScriptExpression expr)
        {
            if (expr.ExpressionType == ScriptExpressionType.Expression)
                switch (expr.ValueType.HaloOnline)
                {
                    case ScriptValueType.HaloOnlineValue.Sound:
                    case ScriptValueType.HaloOnlineValue.Effect:
                    case ScriptValueType.HaloOnlineValue.Damage:
                    case ScriptValueType.HaloOnlineValue.LoopingSound:
                    case ScriptValueType.HaloOnlineValue.AnimationGraph:
                    case ScriptValueType.HaloOnlineValue.DamageEffect:
                    case ScriptValueType.HaloOnlineValue.ObjectDefinition:
                    case ScriptValueType.HaloOnlineValue.Bitmap:
                    case ScriptValueType.HaloOnlineValue.Shader:
                    case ScriptValueType.HaloOnlineValue.RenderModel:
                    case ScriptValueType.HaloOnlineValue.StructureDefinition:
                    case ScriptValueType.HaloOnlineValue.LightmapDefinition:
                    case ScriptValueType.HaloOnlineValue.CinematicDefinition:
                    case ScriptValueType.HaloOnlineValue.CinematicSceneDefinition:
                    case ScriptValueType.HaloOnlineValue.BinkDefinition:
                    case ScriptValueType.HaloOnlineValue.AnyTag:
                    case ScriptValueType.HaloOnlineValue.AnyTagNotResolving:
                        ConvertScriptTagReferenceExpressionData(expr);
                        return;

                    case ScriptValueType.HaloOnlineValue.AiLine:
                    case ScriptValueType.HaloOnlineValue.StringId:
                        ConvertScriptStringIdExpressionData(expr);
                        return;

                    default:
                        break;
                }

            var dataSize = 4;

            switch (expr.ExpressionType)
            {
                case ScriptExpressionType.Expression:
                    switch (expr.ValueType.HaloOnline)
                    {
                        case ScriptValueType.HaloOnlineValue.Scenery:
                        case ScriptValueType.HaloOnlineValue.Object:
                        case ScriptValueType.HaloOnlineValue.Vehicle:
                        case ScriptValueType.HaloOnlineValue.Device:
                            dataSize = 2; // definitely not 4
                            break;

                        case ScriptValueType.HaloOnlineValue.Ai: // int
                            break;

                        default:
                            dataSize = ScriptInfo.GetScriptExpressionDataLength(expr);
                            break;
                    }
                    break;

                case ScriptExpressionType.GlobalsReference:
                    if (BlamCache.Version == CacheVersion.Halo3Retail)
                    {
                        switch (expr.ValueType.HaloOnline)
                        {
                            case ScriptValueType.HaloOnlineValue.Long:
                                dataSize = 4; // definitely not 4
                                break;
                            default:
                                dataSize = ScriptInfo.GetScriptExpressionDataLength(expr);
                                break;
                        }
                    }
                    else if (BlamCache.Version == CacheVersion.Halo3ODST)
                    {
                        switch (expr.ValueType.HaloOnline)
                        {
                            case ScriptValueType.HaloOnlineValue.Long:
                                dataSize = 2; // definitely not 4
                                break;
                            default:
                                dataSize = ScriptInfo.GetScriptExpressionDataLength(expr);
                                break;
                        }
                    }
                    break;

                default:
                    dataSize = ScriptInfo.GetScriptExpressionDataLength(expr);
                    break;
            }

            Array.Reverse(expr.Data, 0, dataSize);
        }

        public void ConvertScriptTagReferenceExpressionData(ScriptExpression expr)
        {
            var blamTag = BlamCache.IndexItems.Find(x => x.ID == BitConverter.ToInt32(expr.Data.Reverse().ToArray(), 0));

            if (blamTag == null)
            {
                Console.WriteLine($"ERROR: Blam tag referenced by the script was not found. 0x{BitConverter.ToInt32(expr.Data.Reverse().ToArray(), 0)}");
                return;
            }
                
            var match = false;

            foreach (var entry in CacheContext.TagNames)
            {
                if (entry.Value != blamTag.Filename)
                    continue;

                var edTag = CacheContext.GetTag(entry.Key);

                if (edTag.Group.Tag == blamTag.GroupTag)
                {
                    expr.Data = BitConverter.GetBytes(entry.Key).ToArray();
                    match = true;
                    break;
                }
            }

            if (!match)
            {
                Console.WriteLine($"ERROR: no tag matches {blamTag.Filename}. Replacing with 0x12FE (GBP)");

                switch (blamTag.GroupTag.ToString())
                {
                    case "effe":
                        expr.Data = new byte[] { 0xFE, 0x12, 0x00, 0x00 };
                        break;

                    default: // should definitely add a tag for each class
                        expr.Data = new byte[4];
                        break;
                }
            }
        }

        public void ConvertScriptStringIdExpressionData(ScriptExpression expr)
        {
            int blamStringId = (int)BitConverter.ToUInt32(expr.Data.Reverse().ToArray(), 0);
            var value = BlamCache.Strings.GetItemByID(blamStringId);

            if (value == null)
                return;

            if (!CacheContext.StringIdCache.Contains(value))
            {
                ConvertStringId(new StringId((uint)blamStringId, BlamCache.Version));
                /*
                Console.WriteLine($"ERROR: stringid does not exist in ED: {value}");
                expr.Data = new byte[4]; // set to blank
                return;
                */
            }

            var edStringId = CacheContext.StringIdCache.GetStringId(value);
            expr.Data = BitConverter.GetBytes(edStringId.Value).ToArray();
        }

        public bool ConvertScriptUsingPresets(Scenario scnr, ScriptExpression expr)
        {
            // Return false to convert normally.

            if (BlamCache.Version == CacheVersion.Halo3Retail)
            {
                switch (expr.Opcode)
                {
                    case 0x353:
                        expr.Opcode = 0x3A6; // Remove the additional H3 argument
                        if (expr.ExpressionType == ScriptExpressionType.Group && expr.ValueType.HaloOnline == ScriptValueType.HaloOnlineValue.Void)
                        {
                            var expr2 = scnr.ScriptExpressions[scnr.ScriptExpressions.IndexOf(expr) + 4];
                            expr2.NextExpressionHandle = scnr.ScriptExpressions[scnr.ScriptExpressions.IndexOf(expr) + 5].NextExpressionHandle;
                        }
                        return true;

                    case 0x2D2: // player_action_test_cinematic_skip
                        expr.Opcode = 0x2FE;
                        return true; // player_action_test_cancel

                    case 0x34D:// cinematic_scripting_destroy_object; remove last argument
                        expr.Opcode = 0x3A0;
                        return true;

                    case 0x44D: // objectives_secondary_show (Doesn't exist in H3)
                        expr.Opcode = 0x4AE; // objectives_show
                        return true;

                    case 0x44F: // objectives_secondary_unavailable (Doesn't exist in H3)
                    case 0x44E: // objectives_primary_unavailable ""
                        expr.Opcode = 0x4B2; // objectives_show
                        return true;

                    case 0x1B7: // campaign_metagame_award_primary_skull
                         expr.Opcode = 0x1E5; // ^
                         return true;

                    case 0x1B8: //campaign_metagame_award_secondary_skull
                        expr.Opcode = 0x1E6; // ^
                        return true;

                    case 0x33C: // cinematic_object_get_unit
                    case 0x33D: // cinematic_object_get_scenery
                    case 0x33E: // cinematic_object_get_effect_scenery
                        expr.Opcode = 0x391; // cinematic_object_get
                        return true;

                    case 0x354: //cinematic_scripting_create_and_animate_object_no_animation
                        expr.Opcode = 0x3A7; // ^
                        return true;

                    default:
                        return false;
                }
            }
            else if (BlamCache.Version == CacheVersion.Halo3ODST)
            {
                switch (expr.Opcode)
                {
                    case 0x3A3: // cinematic_scripting_create_and_animate_cinematic_object
                        expr.Opcode = 0x3A6; // cinematic_scripting_create_and_animate_cinematic_object // example
                        return true;

                    default:
                        return false;
                }
            }
            else
                return false;
        }
        
        public void AdjustScripts(Scenario scnr, string tagName)
        {
            tagName = tagName.Split("\\".ToCharArray()).Last();

            if (tagName == "mainmenu" && BlamCache.Version == CacheVersion.Halo3ODST)
                tagName = "mainmenu_odst";

            if (!DisabledScriptsString.ContainsKey(tagName))
                return;

            foreach (var line in DisabledScriptsString[tagName])
            {
                var items = line.Split(",".ToCharArray());

                int scriptIndex = Convert.ToInt32(items[0]);
                
                uint.TryParse(items[2], NumberStyles.HexNumber, null, out uint NextExpressionHandle);
                ushort.TryParse(items[3], NumberStyles.HexNumber, null, out ushort Opcode);
                byte.TryParse(items[4].Substring(0, 2), NumberStyles.HexNumber, null, out byte data0);
                byte.TryParse(items[4].Substring(2, 2), NumberStyles.HexNumber, null, out byte data1);
                byte.TryParse(items[4].Substring(4, 2), NumberStyles.HexNumber, null, out byte data2);
                byte.TryParse(items[4].Substring(6, 2), NumberStyles.HexNumber, null, out byte data3);

                scnr.ScriptExpressions[scriptIndex].NextExpressionHandle = NextExpressionHandle;
                scnr.ScriptExpressions[scriptIndex].Opcode = Opcode;
                scnr.ScriptExpressions[scriptIndex].Data[0] = data0;
                scnr.ScriptExpressions[scriptIndex].Data[1] = data1;
                scnr.ScriptExpressions[scriptIndex].Data[2] = data2;
                scnr.ScriptExpressions[scriptIndex].Data[3] = data3;
            }
        }

        private static Dictionary<string, List<string>> DisabledScriptsString = new Dictionary<string, List<string>>
        {
            // Format: Script expression tagblock index (dec), expression handle (salt + tagblock index), next expression handle (salt + tagblock index), opcode, data, 
            // expression type, value type, script expression name, original value, comment
            // Ideally this should use a dictionary with a list of script expressions per map name. I'm using a simple text format as this is how I dump scripts and modify them currently.

            ["mainmenu_odst"] = new List<string>
            {
                "00001043,E7860413,E79B0428,0112,140487E7,Group,Void,unit_enable_vision_mode, //default:E78B0418",
                "00001064,E79B0428,E7AD043A,0009,29049CE7,ScriptReference,Void,, //default:E79D042A",
                "00001328,E8A30530,E8B80545,0112,3105A4E8,Group,Void,unit_enable_vision_mode, //default:E8A80535",
                "00001572,E9970624,FFFFFFFF,0000,00000000,Expression,FunctionName,begin, //default:E9790606",
            },
            
            ["c100"] = new List<string>
            {
                "00000293,E4980125,E48E011B,0000,00000000,Expression,FunctionName,begin,// E4860113",
                "00000444,E52F01BC,E53D01CA,030F,BD0130E5,Group,Void,unit_action_test_reset,// E53401C1",
                "00000495,E56201EF,E57001FD,030F,F00163E5,Group,Void,unit_action_test_reset,// E56701F4",
                "00000509,E57001FD,E5780205,0667,FE0171E5,Group,Void,chud_show_navpoint,// FFFFFFFF",
                "00000548,E5970224,E5A50232,030F,250298E5,Group,Void,unit_action_test_reset,// E59C0229",
                "00000562,E5A50232,E5B70244,0000,3302A6E5,Group,Void,begin,// FFFFFFFF",
                "00000622,E5E1026E,E5EF027C,030F,6F02E2E5,Group,Void,unit_action_test_reset,// E5E60273",
                "00000636,E5EF027C,E6090296,0000,7D02F0E5,Group,Void,begin,// FFFFFFFF",
                "00000715,E63E02CB,E64C02D9,030F,CC023FE6,Group,Void,unit_action_test_reset,// E64302D0",
                "00000729,E64C02D9,E66E02FB,0000,DA024DE6,Group,Void,begin,// FFFFFFFF",
                "00000852,E6C70354,E6D0035D,03F4,5503C8E6,Group,Void,sound_impulse_start,// E6CC0359",
                "00000884,E6E70374,FFFFFFFF,03F4,7503E8E6,Group,Void,sound_impulse_start,//E6EC0379",
                "00001572,E9970624,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E9790606",
                "00001043,E7860413,E79B0428,0112,140487E7,Group,Void,unit_enable_vision_mode,// E78B0418",
                "00001064,E79B0428,E7AD043A,0009,29049CE7,ScriptReference,Void,,// E79D042A",
                "00001328,E8A30530,E8B80545,0112,3105A4E8,Group,Void,unit_enable_vision_mode,// E8A80535",
                "00004033,F3340FC1,FFFFFFFF,0002,C20F35F3,Group,Void,if,// F3420FCF",
                "00013476,981834A4,982834B4,005A,A5341998,Group,Void,object_set_function_variable,// 982234AE cinematic_scripting_set_user_input_constraints",
                "00014308,9B5837E4,9B6837F4,005A,E537599B,Group,Void,object_set_function_variable,// 9B6237EE cinematic_scripting_set_user_input_constraints",
                "00020922,B52E51BA,B53D51C9,03A1,BB512FB5,Group,Void,cinematic_scripting_start_effect,// B53751C3 cinematic_scripting_set_user_input_constraints"
            },

            ["c200"] = new List<string>
            {
                "00000293,E4980125,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E4860113",
                "00000482,E55501E2,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E52F01BC",
                "00000532,E5870214,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E56201EF",
                "00000603,E5CE025B,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E5970224",
                "00000693,E62802B5,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E5E1026E",
                "00000802,E6950322,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E63E02CB",
                "00000852,E6C70354,E6D0035D,03F4,5503C8E6,Group,Void,sound_impulse_start,// E6CC0359",
                "00000884,E6E70374,FFFFFFFF,03F4,7503E8E6,Group,Void,sound_impulse_start,// E6EC0379",
                "00001043,E7860413,E79B0428,0112,140487E7,Group,Void,unit_enable_vision_mode,// E78B0418",
                "00001064,E79B0428,E7AD043A,0009,29049CE7,ScriptReference,Void,,// E79D042A",
                "00001328,E8A30530,E8B80545,0112,3105A4E8,Group,Void,unit_enable_vision_mode,// E8A80535",
                "00001572,E9970624,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E9790606"
            },

            ["sc140"] = new List<string>
            {
                "00000909,E700038D,E6F60383,0000,00000000,Expression,FunctionName,begin,// E6EE037B",
                "00001098,E7BD044A,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E7970424",
                "00001148,E7EF047C,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E7CA0457",
                "00001219,E83604C3,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E7FF048C",
                "00001309,E890051D,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E84904D6",
                "00001418,E8FD058A,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E8A60533",
                "00001468,E92F05BC,E93805C5,03F4,BD0530E9,Group,Void,sound_impulse_start,// E93405C1",
                "00001500,E94F05DC,FFFFFFFF,03F4,DD0550E9,Group,Void,sound_impulse_start,// E95405E1",
                "00001659,E9EE067B,EA030690,0112,7C06EFE9,Group,Void,unit_enable_vision_mode,// E9F30680",
                "00001680,EA030690,EA1506A2,0012,910604EA,ScriptReference,Void,,// EA050692",
                "00001944,EB0B0798,EB2007AD,0112,99070CEB,Group,Void,unit_enable_vision_mode,// EB10079D",
                "00002188,EBFF088C,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// EBE1086E",
                "00005201,F7C41451,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// F7A2142F",
                "00015275,9F1F3BAB,9F293BB5,0192,AC3B209F,Group,Void,ai_force_active,// 9F233BAF",
                "00015458,9FD63C62,9FE03C6C,0192,633CD79F,Group,Void,ai_force_active,// 9FDA3C66",
                "00015771,A10F3D9B,A1193DA5,0192,9C3D10A1,Group,Void,ai_force_active,// A1133D9F",
                "00015966,A1D23E5E,A1DC3E68,0192,5F3ED3A1,Group,Void,ai_force_active,// A1D63E62",
                "00016351,A3533FDF,A35A3FE6,017F,E03F54A3,Group,Void,ai_allegiance,// A3573FE3",
                "00016405,A3894015,A3A44030,0002,16408AA3,Group,Void,if,// A390401C",
                "00016728,A4CC4158,A4D64162,0192,5941CDA4,Group,Void,ai_force_active,// A4D0415C",
                "00018114,AA3646C2,AA4046CC,0178,C34637AA,Group,Void,ai_magically_see,// AA3A46C6",
                "00018134,AA4A46D6,AA5346DF,0166,D7464BAA,Group,Void,ai_place,// AA4D46D9",
                "00021577,B7BD5449,B7C3544F,0333,4A54BEB7,Group,Void,switch_zone_set,// B7C0544C",
                "00001538,E9750602,FFFFFFFF,0376,030676E9,Group,Void,cinematic_skip_stop_internal,// E9770604",
                                
                // engine globals, thanks zedd
                // Can and should be automatized
                "00002196,EC070894,EC080895,0005,3C84FFFF,GlobalsReference,Boolean,and,  // global_playtest_mode",
                "00005345,F85414E1,F85514E2,0005,1A85FFFF,GlobalsReference,Boolean,and,  // survival_performance_mode",
                "00017846,A92A45B6,FFFFFFFF,0007,9083FFFF,GlobalsReference,Short,+,      // ai_movement_flee",
                "00019797,B0C94D55,FFFFFFFF,0013,FB82FFFF,GlobalsReference,Ai,<=,        // ai_current_actor",
                "00022826,BC9E592A,FFFFFFFF,0013,FA82FFFF,GlobalsReference,Ai,<=,        // ai_current_squad",
                "00022898,BCE65972,FFFFFFFF,0013,FA82FFFF,GlobalsReference,Ai,<=,        // ai_current_squad",
                "00022973,BD3159BD,FFFFFFFF,0013,FA82FFFF,GlobalsReference,Ai,<=,        // ai_current_squad",
                "00023042,BD765A02,FFFFFFFF,0013,FA82FFFF,GlobalsReference,Ai,<=,        // ai_current_squad",
                // "00025500,C710639C,C711639D,FFFF,EB80FFFF,GlobalsReference,Real,,        // render_near_clip_distance", breaks
                "00030064,D8E47570,D8E57571,FFFF,BE80FFFF,GlobalsReference,Boolean,,     // render_lightmap_shadows",
                "00030246,D99A7626,D99B7627,FFFF,BE80FFFF,GlobalsReference,Boolean,,     // render_lightmap_shadows",
            },

            ["h100"] = new List<string>
            {
                "00000909,E700038D,E6F60383,0000,00000000,Expression,FunctionName,begin,// E6EE037B",
                "00001060,E7970424,E7A50432,030F,250498E7,Group,Void,unit_action_test_reset,// E79C0429",
                "00001111,E7CA0457,E7D80465,030F,5804CBE7,Group,Void,unit_action_test_reset,// E7CF045C",
                "00001125,E7D80465,E7E0046D,0667,6604D9E7,Group,Void,chud_show_navpoint,// FFFFFFFF",
                "00001164,E7FF048C,E80D049A,030F,8D0400E8,Group,Void,unit_action_test_reset,// E8040491",
                "00001178,E80D049A,E81F04AC,0000,9B040EE8,Group,Void,begin,// FFFFFFFF",
                "00001238,E84904D6,E85704E4,030F,D7044AE8,Group,Void,unit_action_test_reset,// E84E04DB",
                "00001252,E85704E4,E87104FE,0000,E50458E8,Group,Void,begin,// FFFFFFFF",
                "00001331,E8A60533,E8B40541,030F,3405A7E8,Group,Void,unit_action_test_reset,// E8AB0538",
                "00001336,E8AB0538,E8D60563,0002,3905ACE8,Group,Void,if,// E8D60563",
                "00001345,E8B40541,E8D60563,0000,4205B5E8,Group,Void,begin,// FFFFFFFF",
                "00001468,E92F05BC,E93805C5,03F4,BD0530E9,Group,Void,sound_impulse_start,// E93405C1",
                "00001500,E94F05DC,FFFFFFFF,03F4,DD0550E9,Group,Void,sound_impulse_start,// E95405E1",
                "00001659,E9EE067B,EA030690,0112,7C06EFE9,Group,Void,unit_enable_vision_mode,// E9F30680",
                "00001680,EA030690,EA1506A2,0012,910604EA,ScriptReference,Void,,// EA050692",
                "00001944,EB0B0798,EB2007AD,0112,99070CEB,Group,Void,unit_enable_vision_mode,// EB10079D",
                "00002188,EBFF088C,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// EBE1086E",
                "00005201,F7C41451,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// F7A2142F",
                "00012780,956031EC,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// 954D31D9",
                "00012813,9581320D,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// 956631F2",
                "00015198,9ED23B5E,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// 9D5439E0",
                "00016273,A3053F91,A3113F9D,0002,923F06A3,Group,Void,if,// A30B3F97",
                "00016285,A3113F9D,A31D3FA9,0112,9E3F12A3,Group,Void,unit_enable_vision_mode,// A3173FA3",
                "00016606,A45240DE,A46940F5,0112,DF4053A4,Group,Void,unit_enable_vision_mode,// A45840E4",
                "00017061,A61942A5,A62342AF,0004,A6421AA6,Group,Void,set,// A61D42A9",
                "00018774,ACCA4956,ACD44960,013A,5749CBAC,Group,Void,device_set_power,// ACCE495A",
                "00019806,B0D24D5E,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B0BA4D46",
                "00019836,B0F04D7C,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B0D84D64",
                "00019866,B10E4D9A,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B0F64D82",
                "00019896,B12C4DB8,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B1144DA0",
                "00020043,B1BF4E4B,B1C84E54,0014,4C4EC0B1,Group,Void,sleep,// B1C24E4E",
                "00020052,B1C84E54,B1D54E61,0014,554EC9B1,Group,Void,sleep,// B1CB4E57",
                "00020089,B1ED4E79,B1F94E85,0002,7A4EEEB1,Group,Void,if,// B1F34E7F",
                "00020113,B2054E91,B2104E9C,0014,924E06B2,Group,Void,sleep,// B2084E94",
                "00020847,B4E3516F,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B25A4EE6",
                "00020937,B53D51C9,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B5095195",
                "00020994,B5765202,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B54251CE",
                "00021051,B5AF523B,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B57B5207",
                "00021108,B5E85274,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B5B45240",
                "00021639,B7FB5487,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B7D75463",
                "00021815,B8AB5537,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B86554F1",
                "00022191,BA2356AF,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B9D45660",
                "00022480,BB4457D0,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// BB03578F",
                "00022823,BC9B5927,FFFFFFFF,040D,28599CBC,Group,Void,vehicle_auto_turret,// BCA2592E",
                "00022920,BCFC5988,FFFFFFFF,040D,8959FDBC,Group,Void,vehicle_auto_turret,// BD03598F",
                "00023021,BD6159ED,FFFFFFFF,040D,EE5962BD,Group,Void,vehicle_auto_turret,// BD6859F4",
                "00023115,BDBF5A4B,FFFFFFFF,040D,4C5AC0BD,Group,Void,vehicle_auto_turret,// BDC65A52",
                "00023224,BE2C5AB8,FFFFFFFF,040D,B95A2DBE,Group,Void,vehicle_auto_turret,// BE335ABF",
                "00023357,BEB15B3D,FFFFFFFF,040D,3E5BB2BE,Group,Void,vehicle_auto_turret,// BEB85B44",
                "00023407,BEE35B6F,BEE85B74,01C9,705BE4BE,ScriptReference,Void,,// BEE55B71",
                "00023449,BF0D5B99,BEE35B6F,0000,00000000,Expression,FunctionName,begin,// BEDD5B69",
                "00024398,C2C25F4E,C2F05F7C,004F,4F5FC3C2,Group,Void,object_create_folder_anew,// C2C55F51",
                "00025402,C6AE633A,C6B2633E,0000,00000000,Expression,FunctionName,begin,// C6AF633B",
                "00025462,C6EA6376,C6EE637A,0000,00000000,Expression,FunctionName,begin,// C6EB6377",
                "00025508,C71863A4,C71C63A8,0000,00000000,Expression,FunctionName,begin,// C71963A5",
                "00025906,C8A66532,C84864D4,0000,00000000,Expression,FunctionName,begin,// C83B64C7",
                "00026011,C90F659B,C8B1653D,0000,00000000,Expression,FunctionName,begin,// C8AB6537",
                "00026116,C9786604,C91A65A6,0000,00000000,Expression,FunctionName,begin,// C91465A0",
                "00026221,C9E1666D,C983660F,0000,00000000,Expression,FunctionName,begin,// C97D6609",
                "00026326,CA4A66D6,C9EC6678,0000,00000000,Expression,FunctionName,begin,// C9E66672",
                "00026431,CAB3673F,CA5566E1,0000,00000000,Expression,FunctionName,begin,// CA4F66DB",
                "00027261,CDF16A7D,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// CDC06A4C",
                "00027330,CE366AC2,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// CDF76A83",
                "00027448,CEAC6B38,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// CE3C6AC8",
                "00027580,CF306BBC,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// CEB26B3E",
                "00036488,F1FC8E88,F19F8E2B,0000,00000000,Expression,FunctionName,begin,// F19C8E28",
                "00037912,F78C9418,F7959421,0002,19948DF7,Group,Void,if,// F792941E",
                "00038519,F9EB9677,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,//F9E49670",
                "00040775,82BC9F47,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// 81CD9E58",
                "00040831,82F49F7F,FFFFFFFF,0014,809FF582,Group,Void,sleep,// 82F79F82",
                "00041280,84B5A140,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// 83339FBE",
                "00041327,84E4A16F,84F6A181,0141,70A1E584,Group,Void,device_group_set_immediate,// 84E8A173",
                "00041542,85BBA246,85CDA258,0141,47A2BC85,Group,Void,device_group_set_immediate,// 85BFA24A",
                "00041751,868CA317,869EA329,0141,18A38D86,Group,Void,device_group_set_immediate,// 8690A31B",
                "00041972,8769A3F4,8774A3FF,0141,F5A36A87,Group,Void,device_group_set_immediate,// 876DA3F8",
                "00043346,8CC7A952,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,G:ai_wave_07_squad_02// 8C21A8AC",
            },

            ["l200"] = new List<string>
            {
                "00000909,E700038D,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E6EE037B",
                "00001060,E7970424,E7A50432,030F,250498E7,Group,Void,unit_action_test_reset,// E79C0429 bypass pda check",
                "00001111,E7CA0457,E7D80465,030F,5804CBE7,Group,Void,unit_action_test_reset,// E7CF045C",
                "00001125,E7D80465,E7E0046D,0667,6604D9E7,Group,Void,chud_show_navpoint,// FFFFFFFF",
                "00001164,E7FF048C,E80D049A,030F,8D0400E8,Group,Void,unit_action_test_reset,// E8040491",
                "00001178,E80D049A,E81F04AC,0000,9B040EE8,Group,Void,begin,// FFFFFFFF",
                "00001238,E84904D6,E85704E4,030F,D7044AE8,Group,Void,unit_action_test_reset,// E84E04DB",
                "00001252,E85704E4,E87104FE,0000,E50458E8,Group,Void,begin,// FFFFFFFF",
                "00001278,E87104FE,E8740501,0014,FF0472E8,Group,Void,sleep,",
                "00001331,E8A60533,E8B40541,030F,3405A7E8,Group,Void,unit_action_test_reset,// E8AB0538 not sure if i should bypass the begin",
                "00001345,E8B40541,E8D60563,0000,4205B5E8,Group,Void,begin,// FFFFFFFF",
                "00001468,E92F05BC,E93805C5,03F4,BD0530E9,Group,Void,sound_impulse_start,// E93405C1",
                "00001500,E94F05DC,FFFFFFFF,03F4,DD0550E9,Group,Void,sound_impulse_start,// E95405E1",
                "00001659,E9EE067B,EA030690,0112,7C06EFE9,Group,Void,unit_enable_vision_mode,// E9F30680",
                "00001680,EA030690,EA1506A2,0012,910604EA,ScriptReference,Void,,// EA050692",
                "00001944,EB0B0798,EB2007AD,0112,99070CEB,Group,Void,unit_enable_vision_mode,// EB10079D",
                "00002188,EBFF088C,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// EBE1086E",
                "00005201,F7C41451,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// F7A2142F",
                "00011278,8F822C0E,8F8C2C18,0141,0F2C838F,Group,Void,device_group_set_immediate,// 8F862C12",
                "00011307,8F9F2C2B,8FD02C5C,0014,2C2CA08F,Group,Void,sleep,// 8FA22C2E",
                "00011412,90082C94,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// 8FDD2C69",
                "00013111,96AB3337,96B1333D,0017,3833AC96,Group,Void,wake,// 96AE333A",
                "00013215,9713339F,971C33A8,0166,A0331497,Group,Void,ai_place,// 971633A2",
                "00013263,974333CF,974933D5,0017,D0334497,Group,Void,wake,// 974633D2",
                "00013377,97B53441,97BE344A,0166,4234B697,Group,Void,ai_place,// 97B83444",
                "00013428,97E83474,97EE347A,0017,7534E997,Group,Void,wake,// 97EB3477",
                "00013542,985A34E6,986034EC,0017,E7345B98,Group,Void,wake,// 985D34E9",
                "00013696,98F43580,98FD3589,0166,8135F598,Group,Void,ai_place,// 98F73583",
                "00013792,995435E0,995A35E6,0017,E1355599,Group,Void,wake,// 995735E3",
                "00013936,99E43670,99ED3679,0166,7136E599,Group,Void,ai_place,// 99E73673",
                "00013960,99FC3688,9A02368E,0017,8936FD99,Group,Void,wake,// 99FF368B",
                "00014093,9A81370D,9A8A3716,0166,0E37829A,Group,Void,ai_place,// 9A843710",
                "00014113,9A953721,9A9B3727,0017,2237969A,Group,Void,wake,// 9A983724",
                "00014250,9B1E37AA,9B2737B3,0166,AB371F9B,Group,Void,ai_place,// 9B2137AD",
                "00014270,9B3237BE,9B3B37C7,0166,BF37339B,Group,Void,ai_place,// 9B3537C1",
                "00014290,9B4637D2,9B4F37DB,0166,D337479B,Group,Void,ai_place,// 9B4937D5",
                "00014313,9B5D37E9,9B6337EF,0017,EA375E9B,Group,Void,wake,// 9B6037EC",
                "00014817,9D5539E1,9D5B39E7,04EC,E239569D,Group,Void,data_mine_set_mission_segment,// 9D5839E4",
                "00015167,9EB33B3F,9EBC3B48,001D,403BB49E,Group,Void,print,// 9EB63B42",
                "00015336,9F5C3BE8,FFFFFFFF,0016,E93B5D9F,Group,Void,sleep_until,// 9F663BF2",
                "00015361,9F753C01,9F7B3C07,04EC,023C769F,Group,Void,data_mine_set_mission_segment,// 9F783C04",
                "00015760,A1043D90,A10A3D96,04EC,913D05A1,Group,Void,data_mine_set_mission_segment,// A1073D93",
                "00016074,A23E3ECA,A2443ED0,04EC,CB3E3FA2,Group,Void,data_mine_set_mission_segment,// A2413ECD",
                "00016535,A40B4097,A411409D,04EC,98400CA4,Group,Void,data_mine_set_mission_segment,// A40E409A",
                "00016938,A59E422A,A5A74233,0166,2B429FA5,Group,Void,ai_place,// A5A1422D",
                "00017274,A6EE437A,A6F44380,04EC,7B43EFA6,Group,Void,data_mine_set_mission_segment,// A6F1437D",
                "00017728,A8B44540,A8BD4549,0014,4145B5A8,Group,Void,sleep,// A8B74543",
                "00018175,AA7346FF,AA794705,04EC,004774AA,Group,Void,data_mine_set_mission_segment,// AA764702",
                "00018437,AB794805,AB82480E,0166,06487AAB,Group,Void,ai_place,// AB7C4808",
                "00018846,AD12499E,AD1B49A7,0166,9F4913AD,Group,Void,ai_place,// AD1549A1",
                "00018866,AD2649B2,AD2F49BB,0166,B34927AD,Group,Void,ai_place,// AD2949B5",
                "00020192,B2544EE0,B25A4EE6,04EC,E14E55B2,Group,Void,data_mine_set_mission_segment,// B2574EE3",
                "00023448,BF0C5B98,BF155BA1,0158,995B0DBF,Group,Void,ai_dialogue_enable,// BF0F5B9B",
                "00024050,C1665DF2,FFFFFFFF,0169,F35D67C1,Group,Void,ai_cannot_die,// C16A5DF6",
                "00028696,D38C7018,D3957021,0371,19708DD3,Group,Void,fade_in,// D392701E",

                "00001538,E9750602,FFFFFFFF,0376,030676E9,Group,Void,cinematic_skip_stop_internal,// E9770604 prevent cinematic from looping; to fix properly",
                "00001555,E9860613,FFFFFFFF,0376,140687E9,Group,Void,cinematic_skip_stop_internal,// E9880615 prevent cinematic from looping; to fix properly",
                "00015667,A0A73D33,A0A23D2E,0000,00000000,Expression,FunctionName,begin,// A0933D1F // test: force cop to teleport to open the hatch",
            },

            ["l300"] = new List<string>
            {
                "00000891,E6EE037B,E6F50382,0016,7C03EFE6,Group,Void,sleep_until,// E6F60383",
                "00001060,E7970424,E7A50432,030F,250498E7,Group,Void,unit_action_test_reset,// E79C0429",
                "00001111,E7CA0457,E7D80465,030F,5804CBE7,Group,Void,unit_action_test_reset,// E7CF045C",
                "00001125,E7D80465,E7E0046D,0667,6604D9E7,Group,Void,chud_show_navpoint,// FFFFFFFF",
                "00001164,E7FF048C,E80D049A,030F,8D0400E8,Group,Void,unit_action_test_reset,// E8040491",
                "00001178,E80D049A,E81F04AC,0000,9B040EE8,Group,Void,begin,// FFFFFFFF",
                "00001238,E84904D6,E85704E4,030F,D7044AE8,Group,Void,unit_action_test_reset,// E84E04DB",
                "00001252,E85704E4,E87104FE,0000,E50458E8,Group,Void,begin,// FFFFFFFF",
                "00001331,E8A60533,E8B40541,030F,3405A7E8,Group,Void,unit_action_test_reset,// E8AB0538",
                "00001345,E8B40541,E8D60563,0000,4205B5E8,Group,Void,begin,// FFFFFFFF",
                "00001468,E92F05BC,E93805C5,03F4,BD0530E9,Group,Void,sound_impulse_start,// E93405C1",
                "00001500,E94F05DC,FFFFFFFF,03F4,DD0550E9,Group,Void,sound_impulse_start,// E95405E1",
                "00001659,E9EE067B,EA030690,0112,7C06EFE9,Group,Void,unit_enable_vision_mode,// E9F30680",
                "00001680,EA030690,EA1506A2,0012,910604EA,ScriptReference,Void,,// EA050692",
                "00001944,EB0B0798,EB2007AD,0112,99070CEB,Group,Void,unit_enable_vision_mode,// EB10079D",
                "00002188,EBFF088C,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// EBE1086E",
                "00005201,F7C41451,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// F7A2142F",
                "00020425,B33D4FC9,B3494FD5,0004,CA4F3EB3,Group,Void,set,// B3434FCF",
                "00020521,B39D5029,B3A75033,0169,2A509EB3,Group,Void,ai_cannot_die,// B3A1502D",
                "00020550,B3BA5046,B3C65052,0004,4750BBB3,Group,Void,set,// B3C0504C",
                "00020646,B41A50A6,B42450B0,0169,A7501BB4,Group,Void,ai_cannot_die,// B41E50AA",
                "00020675,B43750C3,B44350CF,0004,C45038B4,Group,Void,set,// B43D50C9",
                "00020771,B4975123,B4A1512D,0169,245198B4,Group,Void,ai_cannot_die,// B49B5127",
                "00020800,B4B45140,B4C0514C,0004,4151B5B4,Group,Void,set,// B4BA5146",
                "00020896,B51451A0,B51E51AA,0169,A15115B5,Group,Void,ai_cannot_die,// B51851A4",
                "00020925,B53151BD,B53D51C9,0004,BE5132B5,Group,Void,set,// B53751C3",
                "00021021,B591521D,B59B5227,0169,1E5292B5,Group,Void,ai_cannot_die,// B5955221",
                "00021050,B5AE523A,B5BA5246,0004,3B52AFB5,Group,Void,set,// B5B45240",
                "00021146,B60E529A,B61852A4,0169,9B520FB6,Group,Void,ai_cannot_die,// B612529E",
                "00021175,B62B52B7,B63752C3,0004,B8522CB6,Group,Void,set,// B63152BD",
                "00021271,B68B5317,B6955321,0169,18538CB6,Group,Void,ai_cannot_die,// B68F531B",
                "00021300,B6A85334,B6B45340,0004,3553A9B6,Group,Void,set,// B6AE533A",
                "00021396,B7085394,B712539E,0169,955309B7,Group,Void,ai_cannot_die,// B70C5398",
                "00021425,B72553B1,B73153BD,0004,B25326B7,Group,Void,set,// B72B53B7",
                "00021517,B781540D,B78B5417,0169,0E5482B7,Group,Void,ai_cannot_die,// B7855411",
                "00021546,B79E542A,B7AA5436,0004,2B549FB7,Group,Void,set,// B7A45430",
                "00021638,B7FA5486,B8045490,0169,8754FBB7,Group,Void,ai_cannot_die,// B7FE548A",
                "00021753,B86D54F9,B8775503,0169,FA546EB8,Group,Void,ai_cannot_die,// B87154FD",
                "00021795,B8975523,FFFFFFFF,0169,245598B8,Group,Void,ai_cannot_die,// B89B5527",
                "00022403,BAF75783,BB03578F,0004,8457F8BA,Group,Void,set,// BAFD5789",
                "00022523,BB6F57FB,BB795805,0169,FC5770BB,Group,Void,ai_cannot_die,// BB7357FF",
                "00022689,BC1558A1,BC1C58A8,017F,A25816BC,Group,Void,ai_allegiance,// BC1958A5",
                "00023456,BF145BA0,FFFFFFFF,01B1,A15B15BF,Group,Void,ai_set_objective,// BF185BA4",
                "00027973,D0B96D45,D0C36D4F,0169,466DBAD0,Group,Void,ai_cannot_die,// D0BD6D49",
                "00028614,D33A6FC6,D3486FD4,0016,C76F3BD3,Group,Void,sleep_until,// D3426FCE",
                "00028649,D35D6FE9,D3676FF3,0169,EA6F5ED3,Group,Void,ai_cannot_die,// D3616FED",
                "00028675,D3777003,D381700D,0192,047078D3,Group,Void,ai_force_active,// D37B7007",
                "00029030,D4DA7166,D4EF717B,0017,6771DBD4,Group,Void,wake,// D4DD7169",
                "00029086,D512719E,D52771B3,0166,9F7113D5,Group,Void,ai_place,// D51571A1",
                "00034658,EAD68762,EAEF877B,0006,6387D7EA,Group,Boolean,or,// EAE98775",
                "00034789,EB5987E5,EABD8749,0000,00000000,Expression,FunctionName,begin,// EABD8749 disable the whole thing for now",
                "00035083,EC7F890B,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// EB5F87EB disable the whole thing for now",
                "00035212,ED00898C,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// EC858911 disable the whole thing for now",
                "00035397,EDB98A45,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// ED068992 disable the whole thing for now",
                "00035767,EF2B8BB7,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// EF238BAF",
                "00035910,EFBA8C46,EFC08C4C,0333,478CBBEF,Group,Void,switch_zone_set,// EFBD8C49",
            },

            ["005_intro"] = new List<string>
            {
                "00002585,ED8C0A19,ED920A1F,0424,1A0A8DED,Group,Void,,chud_show_shield,ED8F0A1C"
            },

            ["020_base"] = new List<string>
            {
                "00000557,E5A0022D,E5950222,0000,00000000,Expression,FunctionName,,begin",
                "00000546,E5950222,FFFFFFFF,0376,230296E5,Group,Void,,cinematic_skip_stop_internal,E5970224",
            },

            ["040_voi"] = new List<string>
            {
                "00001611,E9BE064B,FFFFFFFF,0005,01FFFFFF,Expression,Boolean,boolean,C:player_disable_movement",
                "00009478,887A2506,FFFFFFFF,0005,01FFFFFF,Expression,Boolean,boolean,C:player_disable_movement",
                "00012297,937D3009,FFFFFFFF,0005,01FFFFFF,Expression,Boolean,boolean,C:player_disable_movement",
            },

            ["070_waste"] = new List<string>
            {
                "00000546,E5950222,FFFFFFFF,0376,230296E5,Group,Void,,cinematic_skip_stop_internal,D:E5970224",

                // Default script lines:
                "00001611,E9BE064B,FFFFFFFF,0005,01FFFFFF,Expression,Boolean,boolean,",
                "00009478,887A2506,887B2507,0013,3C000040,Expression,Ai,ai,",
                "00012297,937D3009,937E300A,0002,00000000,Expression,FunctionName,,if",
            },

            ["110_hc"] = new List<string>
            {
                "00000308,E4A70134,FFFFFFFF,0376,3501A8E4,Group,Void,,cinematic_skip_stop_internal",
            },

            ["120_halo"] = new List<string>
            {
                "00000308,E4A70134,FFFFFFFF,0376,3501A8E4,Group,Void,,cinematic_skip_stop_internal,D:E4A90136",
            },
        };
    }
}
