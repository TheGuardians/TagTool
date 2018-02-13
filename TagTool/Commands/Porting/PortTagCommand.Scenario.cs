using TagTool.Cache;
using TagTool.Common;
using TagTool.Scripting;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;

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
                }
            }
            return cisc;
        }

        private Scenario ConvertScenario(Scenario scnr)
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

            scnr.ScenarioKillTriggers = new List<Scenario.ScenarioKillTrigger>();
            scnr.SimulationDefinitionTable = new List<Scenario.SimulationDefinitionTableBlock>();

            //
            // Convert scripts
            //

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

            return scnr;
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
                    switch (expr.ValueType.HaloOnline)
                    {
                        case ScriptValueType.HaloOnlineValue.Long:
                            dataSize = 2; // definitely not 4
                            break;
                        default:
                            dataSize = ScriptInfo.GetScriptExpressionDataLength(expr);
                            break;
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

                if (edTag.Group.Tag == blamTag.ClassCode)
                {
                    expr.Data = BitConverter.GetBytes(entry.Key).ToArray();
                    match = true;
                    break;
                }
            }

            if (!match)
            {
                Console.WriteLine($"ERROR: no tag matches {blamTag.Filename}. Replacing with 0x12FE (GBP)");

                switch (blamTag.ClassCode)
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
                ConvertStringId(new StringId((uint)blamStringId));
                /*
                Console.WriteLine($"ERROR: stringid does not exist in ED: {value}");
                expr.Data = new byte[4]; // set to blank
                return;
                */
            }

            var edStringId = CacheContext.StringIdCache.GetStringId(value);
            expr.Data = BitConverter.GetBytes(edStringId.Value).ToArray();
        }

        public bool ConvertScriptUsingPresets(Scenario scnr,ScriptExpression expr)
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

                    case 0x33C: // cinematic_object_get_unit
                    case 0x33D: // cinematic_object_get_scenery
                    case 0x33E: // cinematic_object_get_effect_scenery
                        expr.Opcode = 0x391; // cinematic_object_get
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
    }
}