using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Scripting.Compiler;
using TagTool.Tags.Definitions;

namespace TagTool.Scripting.Compiler
{
    public class ScriptCompiler
    {
        public GameCacheContext CacheContext { get; }
        public Scenario Definition { get; }

        private List<Script> Scripts;
        private List<ScriptGlobal> Globals;
        private List<ScriptExpression> ScriptExpressions;

        private BinaryWriter StringWriter;
        private Dictionary<string, uint> StringOffsets;

        public ScriptCompiler(GameCacheContext cacheContext, Scenario definition)
        {
            CacheContext = cacheContext;
            Definition = definition;

            Scripts = new List<Script>();
            Globals = new List<ScriptGlobal>();
            ScriptExpressions = new List<ScriptExpression>();

            StringWriter = new BinaryWriter(new MemoryStream());
            StringOffsets = new Dictionary<string, uint>();
        }

        public void CompileFile(FileInfo file)
        {
            if (!file.Exists)
                throw new FileNotFoundException(file.FullName);

            //
            // Read the input file into syntax nodes
            //

            List<IScriptSyntax> nodes;

            using (var stream = file.OpenRead())
                nodes = new ScriptSyntaxReader(stream).ReadToEnd();

            //
            // Parse the input syntax nodes
            //

            foreach (var node in nodes)
            {
                CompileToplevel(node);
            }
        }

        private void CompileToplevel(IScriptSyntax node)
        {
            switch (node)
            {
                case ScriptGroup group:
                    switch (group.Head)
                    {
                        case ScriptSymbol symbol:
                            switch (symbol.Value)
                            {
                                case "global":
                                    CompileGlobal(group);
                                    break;

                                case "script":
                                    CompileScript(group);
                                    break;
                            }
                            break;

                        default:
                            throw new FormatException(group.ToString());
                    }
                    break;

                default:
                    throw new FormatException(node.ToString());
            }
        }

        private ScriptValueType ParseScriptValueType(IScriptSyntax node)
        {
            var result = new ScriptValueType();

            if (!(node is ScriptSymbol symbol) ||
                !Enum.TryParse(symbol.Value, out result.Halo3ODST))
            {
                throw new FormatException(node.ToString());
            }

            return result;
        }

        private ScriptType ParseScriptType(IScriptSyntax node)
        {
            if (!(node is ScriptSymbol symbol) ||
                !Enum.TryParse<ScriptType>(symbol.Value, out var result))
            {
                throw new FormatException(node.ToString());
            }

            return result;
        }

        private uint CompileStringAddress(string input)
        {
            if (!StringOffsets.ContainsKey(input))
            {
                var offset = (uint)StringWriter.BaseStream.Position;
                StringWriter.Write(input.ToArray());
                StringWriter.Write('\0');
                StringOffsets[input] = offset;
            }

            return StringOffsets[input];
        }

        private void CompileGlobal(IScriptSyntax node)
        {
            //
            // Verify the input syntax node is in the right format
            //

            if (!(node is ScriptGroup group) ||
                !(group.Head is ScriptSymbol symbol && symbol.Value == "group") ||
                !(group.Tail is ScriptGroup declGroup))
            {
                throw new FormatException(node.ToString());
            }

            //
            // Compile the global type
            //

            var globalType = ParseScriptValueType(declGroup.Head);

            //
            // Compile the global name
            //

            if (!(declGroup.Tail is ScriptGroup declTailGroup))
                throw new FormatException(declGroup.Tail.ToString());

            if (!(declTailGroup.Head is ScriptSymbol declName))
                throw new FormatException(declTailGroup.Head.ToString());

            var globalName = declName.Value;

            //
            // Compile the global initialization expression
            //

            if (!(declTailGroup.Tail is ScriptGroup declTailTailGroup))
                throw new FormatException(declTailGroup.Tail.ToString());

            var globalInit = CompileExpression(globalType, declTailTailGroup.Head);

            //
            // Add an entry to the globals block in the scenario definition
            //

            Globals.Add(new ScriptGlobal
            {
                Name = globalName,
                Type = globalType,
                InitializationExpressionHandle = globalInit
            });
        }

        private void CompileScript(IScriptSyntax node)
        {
            //
            // Verify the input syntax node is in the right format
            //

            if (!(node is ScriptGroup group) ||
                !(group.Head is ScriptSymbol symbol && symbol.Value == "script") ||
                !(group.Tail is ScriptGroup declGroup))
            {
                throw new FormatException(node.ToString());
            }

            //
            // Compile the script type
            //

            var scriptType = ParseScriptType(declGroup.Head);

            //
            // Compile the script return type
            //

            if (!(declGroup.Tail is ScriptGroup declTailGroup))
                throw new FormatException(declGroup.Tail.ToString());

            var scriptReturnType = ParseScriptValueType(declTailGroup.Head);

            //
            // Compile the script name and parameters (if any)
            //

            if (!(declTailGroup.Tail is ScriptGroup declTailTailGroup))
                throw new FormatException(declTailGroup.Tail.ToString());

            string scriptName;
            var scriptParams = new List<ScriptParameter>();

            switch (declTailTailGroup.Head)
            {
                // (script static boolean do_stuff ...)
                case ScriptSymbol declName:
                    scriptName = declName.Value;
                    break;

                // (script static boolean (do_stuff (real a) (real b)) ...)
                case ScriptGroup declNameGroup:
                    {
                        //
                        // Get the name of the script
                        //

                        if (!(declNameGroup.Head is ScriptSymbol declGroupName))
                            throw new FormatException(declNameGroup.Head.ToString());

                        scriptName = declGroupName.Value;

                        //
                        // Get a list of script parameters
                        //

                        if (!(declNameGroup.Tail is ScriptGroup declParamGroup))
                            throw new FormatException(declNameGroup.Tail.ToString());

                        for (IScriptSyntax param = declParamGroup;
                            param is ScriptGroup paramGroup;
                            param = paramGroup.Tail)
                        {
                            //
                            // Verify the input parameter syntax is correct: (type name)
                            //

                            if (!(paramGroup.Head is ScriptGroup paramDeclGroup))
                                throw new FormatException(paramGroup.Head.ToString());

                            //
                            // Get the parameter type
                            //

                            if (!(paramDeclGroup.Head is ScriptSymbol paramDeclType))
                                throw new FormatException(paramDeclGroup.Head.ToString());

                            var paramType = ParseScriptValueType(paramDeclType);

                            //
                            // Get the parameter name
                            //

                            if (!(paramDeclGroup.Tail is ScriptGroup paramDeclTailGroup))
                                throw new FormatException(paramDeclGroup.Tail.ToString());

                            if (!(paramDeclTailGroup.Head is ScriptSymbol paramDeclName))
                                throw new FormatException(paramDeclTailGroup.Head.ToString());

                            var paramName = paramDeclName.Value;

                            if (!(paramDeclTailGroup.Tail is ScriptInvalid))
                                throw new FormatException(paramDeclTailGroup.Tail.ToString());

                            //
                            // Add an entry to the script parameters list
                            //

                            scriptParams.Add(new ScriptParameter
                            {
                                Name = paramName,
                                Type = paramType
                            });
                        }
                    }
                    break;

                default:
                    throw new FormatException(declTailGroup.Head.ToString());
            }

            //
            // Compile the script expressions
            //

            var scriptInit = CompileExpression(
                scriptReturnType,
                new ScriptGroup
                {
                    Head = new ScriptSymbol { Value = "begin" },
                    Tail = declTailTailGroup.Tail
                });

            //
            // Add an entry to the scripts block in the scenario definition
            //

            Scripts.Add(new Script
            {
                ScriptName = scriptName,
                Type = scriptType,
                ReturnType = scriptReturnType,
                RootExpressionHandle = scriptInit,
                Parameters = scriptParams
            });
        }

        private DatumIndex CompileExpression(ScriptValueType type, IScriptSyntax node)
        {
            if (node is ScriptGroup group)
                return CompileGroupExpression(type, group);

            switch (type.Halo3ODST)
            {
                case ScriptValueType.Halo3ODSTValue.Boolean:
                    if (node is ScriptBoolean boolValue)
                        return CompileBooleanExpression(boolValue);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Real:
                    if (node is ScriptReal realValue)
                        return CompileRealExpression(realValue);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Short:
                    if (node is ScriptInteger shortValue)
                        return CompileShortExpression(shortValue);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Long:
                    if (node is ScriptInteger longValue)
                        return CompileLongExpression(longValue);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.String:
                    if (node is ScriptString stringValue)
                        return CompileStringExpression(stringValue);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Script:
                    if (node is ScriptSymbol scriptSymbol)
                        return CompileScriptExpression(scriptSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.StringId:
                    if (node is ScriptString stringIdString)
                        return CompileStringIdExpression(stringIdString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.UnitSeatMapping:
                    if (node is ScriptSymbol unitSeatMappingSymbol)
                        return CompileUnitSeatMappingExpression(unitSeatMappingSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.TriggerVolume:
                    if (node is ScriptSymbol triggerVolumeSymbol)
                        return CompileTriggerVolumeExpression(triggerVolumeSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.CutsceneFlag:
                    if (node is ScriptSymbol cutsceneFlagSymbol)
                        return CompileCutsceneFlagExpression(cutsceneFlagSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint:
                    if (node is ScriptSymbol cutsceneCameraPointSymbol)
                        return CompileCutsceneCameraPointExpression(cutsceneCameraPointSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.CutsceneTitle:
                    if (node is ScriptSymbol cutsceneTitleSymbol)
                        return CompileCutsceneTitleExpression(cutsceneTitleSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.CutsceneRecording:
                    if (node is ScriptString cutsceneRecordingString)
                        return CompileCutsceneRecordingExpression(cutsceneRecordingString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.DeviceGroup:
                    if (node is ScriptString deviceGroupString)
                        return CompileDeviceGroupExpression(deviceGroupString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Ai:
                    if (node is ScriptString aiString)
                        return CompileAiExpression(aiString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.AiCommandList:
                    if (node is ScriptString aiCommandListString)
                        return CompileAiCommandListExpression(aiCommandListString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.AiCommandScript:
                    if (node is ScriptString aiCommandScriptString)
                        return CompileAiCommandScriptExpression(aiCommandScriptString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.AiBehavior:
                    if (node is ScriptString aiBehaviorString)
                        return CompileAiBehaviorExpression(aiBehaviorString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.AiOrders:
                    if (node is ScriptString aiOrdersString)
                        return CompileAiOrdersExpression(aiOrdersString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.AiLine:
                    if (node is ScriptString aiLineString)
                        return CompileAiLineExpression(aiLineString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.StartingProfile:
                    if (node is ScriptString startingProfileString)
                        return CompileStartingProfileExpression(startingProfileString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Conversation:
                    if (node is ScriptString conversationString)
                        return CompileConversationExpression(conversationString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.ZoneSet:
                    if (node is ScriptString zoneSetString)
                        return CompileZoneSetExpression(zoneSetString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.DesignerZone:
                    if (node is ScriptString designerZoneString)
                        return CompileDesignerZoneExpression(designerZoneString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.PointReference:
                    if (node is ScriptString pointReferenceString)
                        return CompilePointReferenceExpression(pointReferenceString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Style:
                    if (node is ScriptString styleString)
                        return CompileStyleExpression(styleString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.ObjectList:
                    if (node is ScriptString objectListString)
                        return CompileObjectListExpression(objectListString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Folder:
                    if (node is ScriptString folderString)
                        return CompileFolderExpression(folderString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Sound:
                    if (node is ScriptString soundString)
                        return CompileSoundExpression(soundString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Effect:
                    if (node is ScriptString effectString)
                        return CompileEffectExpression(effectString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Damage:
                    if (node is ScriptString damageString)
                        return CompileDamageExpression(damageString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.LoopingSound:
                    if (node is ScriptString loopingSoundString)
                        return CompileLoopingSoundExpression(loopingSoundString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.AnimationGraph:
                    if (node is ScriptString animationGraphString)
                        return CompileAnimationGraphExpression(animationGraphString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.DamageEffect:
                    if (node is ScriptString damageEffectString)
                        return CompileDamageEffectExpression(damageEffectString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.ObjectDefinition:
                    if (node is ScriptString objectDefinitionString)
                        return CompileObjectDefinitionExpression(objectDefinitionString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Bitmap:
                    if (node is ScriptString bitmapString)
                        return CompileBitmapExpression(bitmapString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Shader:
                    if (node is ScriptString shaderString)
                        return CompileShaderExpression(shaderString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.RenderModel:
                    if (node is ScriptString renderModelString)
                        return CompileRenderModelExpression(renderModelString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.StructureDefinition:
                    if (node is ScriptString structureDefinitionString)
                        return CompileStructureDefinitionExpression(structureDefinitionString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.LightmapDefinition:
                    if (node is ScriptString lightmapDefinitionString)
                        return CompileLightmapDefinitionExpression(lightmapDefinitionString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.CinematicDefinition:
                    if (node is ScriptString cinematicDefinitionString)
                        return CompileCinematicDefinitionExpression(cinematicDefinitionString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.CinematicSceneDefinition:
                    if (node is ScriptString cinematicSceneDefinitionString)
                        return CompileCinematicSceneDefinitionExpression(cinematicSceneDefinitionString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.BinkDefinition:
                    if (node is ScriptString binkDefinitionString)
                        return CompileBinkDefinitionExpression(binkDefinitionString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.AnyTag:
                    if (node is ScriptString anyTagString)
                        return CompileAnyTagExpression(anyTagString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.AnyTagNotResolving:
                    if (node is ScriptString anyTagNotResolvingString)
                        return CompileAnyTagNotResolvingExpression(anyTagNotResolvingString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.GameDifficulty:
                    if (node is ScriptSymbol gameDifficultySymbol)
                        return CompileGameDifficultyExpression(gameDifficultySymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Team:
                    if (node is ScriptSymbol teamSymbol)
                        return CompileTeamExpression(teamSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.MpTeam:
                    if (node is ScriptSymbol mpTeamSymbol)
                        return CompileMpTeamExpression(mpTeamSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Controller:
                    if (node is ScriptInteger controllerInteger)
                        return CompileControllerExpression(controllerInteger);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.ButtonPreset:
                    if (node is ScriptSymbol buttonPresetSymbol)
                        return CompileButtonPresetExpression(buttonPresetSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.JoystickPreset:
                    if (node is ScriptSymbol joystickPresetSymbol)
                        return CompileJoystickPresetExpression(joystickPresetSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.PlayerColor:
                    if (node is ScriptSymbol playerColorSymbol)
                        return CompilePlayerColorExpression(playerColorSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.PlayerCharacterType:
                    if (node is ScriptSymbol playerCharacterTypeSymbol)
                        return CompilePlayerCharacterTypeExpression(playerCharacterTypeSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.VoiceOutputSetting:
                    if (node is ScriptSymbol voiceOutputSettingSymbol)
                        return CompileVoiceOutputSettingExpression(voiceOutputSettingSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.VoiceMask:
                    if (node is ScriptSymbol voiceMaskSymbol)
                        return CompileVoiceMaskExpression(voiceMaskSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.SubtitleSetting:
                    if (node is ScriptSymbol subtitleSettingSymbol)
                        return CompileSubtitleSettingExpression(subtitleSettingSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.ActorType:
                    if (node is ScriptSymbol actorTypeSymbol)
                        return CompileActorTypeExpression(actorTypeSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.ModelState:
                    if (node is ScriptSymbol modelStateSymbol)
                        return CompileModelStateExpression(modelStateSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Event:
                    if (node is ScriptSymbol eventSymbol)
                        return CompileEventExpression(eventSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.CharacterPhysics:
                    if (node is ScriptSymbol characterPhysicsSymbol)
                        return CompileCharacterPhysicsExpression(characterPhysicsSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.PrimarySkull:
                    if (node is ScriptSymbol primarySkullSymbol)
                        return CompilePrimarySkullExpression(primarySkullSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.SecondarySkull:
                    if (node is ScriptSymbol secondarySkullSymbol)
                        return CompileSecondarySkullExpression(secondarySkullSymbol);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Object:
                    if (node is ScriptString objectString)
                        return CompileObjectExpression(objectString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Unit:
                    if (node is ScriptString unitString)
                        return CompileUnitExpression(unitString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Vehicle:
                    if (node is ScriptString vehicleString)
                        return CompileVehicleExpression(vehicleString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Weapon:
                    if (node is ScriptString weaponString)
                        return CompileWeaponExpression(weaponString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Device:
                    if (node is ScriptString deviceString)
                        return CompileDeviceExpression(deviceString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Scenery:
                    if (node is ScriptString sceneryString)
                        return CompileSceneryExpression(sceneryString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.EffectScenery:
                    if (node is ScriptString effectSceneryString)
                        return CompileEffectSceneryExpression(effectSceneryString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.ObjectName:
                    if (node is ScriptString objectNameString)
                        return CompileObjectNameExpression(objectNameString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.UnitName:
                    if (node is ScriptString unitNameString)
                        return CompileUnitNameExpression(unitNameString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.VehicleName:
                    if (node is ScriptString vehicleNameString)
                        return CompileVehicleNameExpression(vehicleNameString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.WeaponName:
                    if (node is ScriptString weaponNameString)
                        return CompileWeaponNameExpression(weaponNameString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.DeviceName:
                    if (node is ScriptString deviceNameString)
                        return CompileDeviceNameExpression(deviceNameString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.SceneryName:
                    if (node is ScriptString sceneryNameString)
                        return CompileSceneryNameExpression(sceneryNameString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.EffectSceneryName:
                    if (node is ScriptString effectSceneryNameString)
                        return CompileEffectSceneryNameExpression(effectSceneryNameString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.CinematicLightprobe:
                    if (node is ScriptString cinematicLightprobeString)
                        return CompileCinematicLightprobeExpression(cinematicLightprobeString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.AnimationBudgetReference:
                    if (node is ScriptString animationBudgetReferenceString)
                        return CompileAnimationBudgetReferenceExpression(animationBudgetReferenceString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.LoopingSoundBudgetReference:
                    if (node is ScriptString loopingSoundBudgetReferenceString)
                        return CompileLoopingSoundBudgetReferenceExpression(loopingSoundBudgetReferenceString);
                    else throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.SoundBudgetReference:
                    if (node is ScriptString soundBudgetReferenceString)
                        return CompileSoundBudgetReferenceExpression(soundBudgetReferenceString);
                    else throw new FormatException(node.ToString());
            }

            throw new NotImplementedException(type.ToString());
        }

        private DatumIndex AllocateExpression(ScriptValueType.Halo3ODSTValue valueType, ScriptExpressionType expressionType, ushort? opcode = null)
        {
            ushort salt = 0; // TODO?
            uint stringAddress = 0; // TODO?

            var expression = new ScriptExpression
            {
                Salt = salt,
                Opcode = opcode ?? (ushort)valueType,
                ValueType = new ScriptValueType { Halo3ODST = valueType },
                ExpressionType = expressionType,
                NextExpressionHandle = DatumIndex.None,
                StringAddress = stringAddress,
                Data = BitConverter.GetBytes(-1),
                LineNumber = -1,
            };

            ScriptExpressions.Add(expression);

            return new DatumIndex(salt, (ushort)ScriptExpressions.IndexOf(expression));
        }

        private DatumIndex CompileGroupExpression(ScriptValueType type, ScriptGroup group) =>
            throw new NotImplementedException();

        private DatumIndex CompileBooleanExpression(ScriptBoolean boolValue)
        {
            var handle = AllocateExpression(ScriptValueType.Halo3ODSTValue.Boolean, ScriptExpressionType.Expression);

            if (handle != DatumIndex.None)
                Array.Copy(BitConverter.GetBytes(boolValue.Value), ScriptExpressions[handle.Index].Data, 1);

            return handle;
        }

        private DatumIndex CompileRealExpression(ScriptReal realValue)
        {
            var handle = AllocateExpression(ScriptValueType.Halo3ODSTValue.Real, ScriptExpressionType.Expression);

            if (handle != DatumIndex.None)
                Array.Copy(BitConverter.GetBytes((float)realValue.Value), ScriptExpressions[handle.Index].Data, 4);

            return handle;
        }

        private DatumIndex CompileShortExpression(ScriptInteger shortValue)
        {
            var handle = AllocateExpression(ScriptValueType.Halo3ODSTValue.Short, ScriptExpressionType.Expression);

            if (handle != DatumIndex.None)
                Array.Copy(BitConverter.GetBytes((short)shortValue.Value), ScriptExpressions[handle.Index].Data, 2);

            return handle;
        }

        private DatumIndex CompileLongExpression(ScriptInteger longValue)
        {
            var handle = AllocateExpression(ScriptValueType.Halo3ODSTValue.Long, ScriptExpressionType.Expression);

            if (handle != DatumIndex.None)
                Array.Copy(BitConverter.GetBytes((int)longValue.Value), ScriptExpressions[handle.Index].Data, 4);

            return handle;
        }

        private DatumIndex CompileStringExpression(ScriptString stringValue)
        {
            var handle = AllocateExpression(ScriptValueType.Halo3ODSTValue.String, ScriptExpressionType.Expression);

            if (handle != DatumIndex.None)
                ScriptExpressions[handle.Index].StringAddress = CompileStringAddress(stringValue.Value);

            return handle;
        }

        private DatumIndex CompileScriptExpression(ScriptSymbol scriptSymbol)
        {
            var handle = AllocateExpression(ScriptValueType.Halo3ODSTValue.Script, ScriptExpressionType.Expression);

            if (handle != DatumIndex.None)
            {
                ScriptExpressions[handle.Index].StringAddress = CompileStringAddress(scriptSymbol.Value);
                
                //
                // TODO: Compile script data here
                //

                throw new NotImplementedException(ScriptValueType.Halo3ODSTValue.Script.ToString());
            }

            return handle;
        }

        private DatumIndex CompileStringIdExpression(ScriptString stringIdString)
        {
            var handle = AllocateExpression(ScriptValueType.Halo3ODSTValue.StringId, ScriptExpressionType.Expression);

            if (handle != DatumIndex.None)
            {
                var stringId = CacheContext.GetStringId(stringIdString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(stringIdString.Value);
                Array.Copy(BitConverter.GetBytes(stringId.Value), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileUnitSeatMappingExpression(ScriptSymbol unitSeatMappingSymbol)
        {
            var handle = AllocateExpression(ScriptValueType.Halo3ODSTValue.UnitSeatMapping, ScriptExpressionType.Expression);

            if (handle != DatumIndex.None)
            {
                ScriptExpressions[handle.Index].StringAddress = CompileStringAddress(unitSeatMappingSymbol.Value);

                //
                // TODO: Compile unit_seat_mapping data here
                //

                throw new NotImplementedException(ScriptValueType.Halo3ODSTValue.UnitSeatMapping.ToString());
            }

            return handle;
        }

        private DatumIndex CompileTriggerVolumeExpression(ScriptSymbol triggerVolumeSymbol)
        {
            var handle = AllocateExpression(ScriptValueType.Halo3ODSTValue.TriggerVolume, ScriptExpressionType.Expression);

            if (handle != DatumIndex.None)
            {
                var triggerVolumeIndex = Definition.TriggerVolumes.FindIndex(tv => triggerVolumeSymbol.Value == CacheContext.GetString(tv.Name));

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(triggerVolumeSymbol.Value);
                Array.Copy(BitConverter.GetBytes(triggerVolumeIndex), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileCutsceneFlagExpression(ScriptSymbol cutsceneFlagSymbol)
        {
            var handle = AllocateExpression(ScriptValueType.Halo3ODSTValue.CutsceneFlag, ScriptExpressionType.Expression);

            if (handle != DatumIndex.None)
            {
                var cutsceneFlagIndex = Definition.CutsceneFlags.FindIndex(cf => cutsceneFlagSymbol.Value == CacheContext.GetString(cf.Name));

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(cutsceneFlagSymbol.Value);
                Array.Copy(BitConverter.GetBytes(cutsceneFlagIndex), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileCutsceneCameraPointExpression(ScriptSymbol cutsceneCameraPointSymbol)
        {
            var handle = AllocateExpression(ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint, ScriptExpressionType.Expression);

            if (handle != DatumIndex.None)
            {
                var cutsceneCameraPointIndex = Definition.CutsceneCameraPoints.FindIndex(ccp => cutsceneCameraPointSymbol.Value == ccp.Name);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(cutsceneCameraPointSymbol.Value);
                Array.Copy(BitConverter.GetBytes(cutsceneCameraPointIndex), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileCutsceneTitleExpression(ScriptSymbol cutsceneTitleSymbol)
        {
            var handle = AllocateExpression(ScriptValueType.Halo3ODSTValue.CutsceneTitle, ScriptExpressionType.Expression);

            if (handle != DatumIndex.None)
            {
                var cutsceneTitleIndex = Definition.CutsceneTitles.FindIndex(ct => cutsceneTitleSymbol.Value == CacheContext.GetString(ct.Name));

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(cutsceneTitleSymbol.Value);
                Array.Copy(BitConverter.GetBytes(cutsceneTitleIndex), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileCutsceneRecordingExpression(ScriptString cutsceneRecordingString) =>
            throw new NotImplementedException();

        private DatumIndex CompileDeviceGroupExpression(ScriptString deviceGroupString) =>
            throw new NotImplementedException();

        private DatumIndex CompileAiExpression(ScriptString aiString) =>
            throw new NotImplementedException();

        private DatumIndex CompileAiCommandListExpression(ScriptString aiCommandListString) =>
            throw new NotImplementedException();

        private DatumIndex CompileAiCommandScriptExpression(ScriptString aiCommandScriptString) =>
            throw new NotImplementedException();

        private DatumIndex CompileAiBehaviorExpression(ScriptString aiBehaviorString) =>
            throw new NotImplementedException();

        private DatumIndex CompileAiOrdersExpression(ScriptString aiOrdersString) =>
            throw new NotImplementedException();

        private DatumIndex CompileAiLineExpression(ScriptString aiLineString) =>
            throw new NotImplementedException();

        private DatumIndex CompileStartingProfileExpression(ScriptString startingProfileString) =>
            throw new NotImplementedException();

        private DatumIndex CompileConversationExpression(ScriptString conversationString) =>
            throw new NotImplementedException();

        private DatumIndex CompileZoneSetExpression(ScriptString zoneSetString) =>
            throw new NotImplementedException();

        private DatumIndex CompileDesignerZoneExpression(ScriptString designerZoneString) =>
            throw new NotImplementedException();

        private DatumIndex CompilePointReferenceExpression(ScriptString pointReferenceString) =>
            throw new NotImplementedException();

        private DatumIndex CompileStyleExpression(ScriptString styleString) =>
            throw new NotImplementedException();

        private DatumIndex CompileObjectListExpression(ScriptString objectListString) =>
            throw new NotImplementedException();

        private DatumIndex CompileFolderExpression(ScriptString folderString) =>
            throw new NotImplementedException();

        private DatumIndex CompileSoundExpression(ScriptString soundString) =>
            throw new NotImplementedException();

        private DatumIndex CompileEffectExpression(ScriptString effectString) =>
            throw new NotImplementedException();

        private DatumIndex CompileDamageExpression(ScriptString damageString) =>
            throw new NotImplementedException();

        private DatumIndex CompileLoopingSoundExpression(ScriptString loopingSoundString) =>
            throw new NotImplementedException();

        private DatumIndex CompileAnimationGraphExpression(ScriptString animationGraphString) =>
            throw new NotImplementedException();

        private DatumIndex CompileDamageEffectExpression(ScriptString damageEffectString) =>
            throw new NotImplementedException();

        private DatumIndex CompileObjectDefinitionExpression(ScriptString objectDefinitionString) =>
            throw new NotImplementedException();

        private DatumIndex CompileBitmapExpression(ScriptString bitmapString) =>
            throw new NotImplementedException();

        private DatumIndex CompileShaderExpression(ScriptString shaderString) =>
            throw new NotImplementedException();

        private DatumIndex CompileRenderModelExpression(ScriptString renderModelString) =>
            throw new NotImplementedException();

        private DatumIndex CompileStructureDefinitionExpression(ScriptString structureDefinitionString) =>
            throw new NotImplementedException();

        private DatumIndex CompileLightmapDefinitionExpression(ScriptString lightmapDefinitionString) =>
            throw new NotImplementedException();

        private DatumIndex CompileCinematicDefinitionExpression(ScriptString cinematicDefinitionString) =>
            throw new NotImplementedException();

        private DatumIndex CompileCinematicSceneDefinitionExpression(ScriptString cinematicSceneDefinitionString) =>
            throw new NotImplementedException();

        private DatumIndex CompileBinkDefinitionExpression(ScriptString binkDefinitionString) =>
            throw new NotImplementedException();

        private DatumIndex CompileAnyTagExpression(ScriptString anyTagString) =>
            throw new NotImplementedException();

        private DatumIndex CompileAnyTagNotResolvingExpression(ScriptString anyTagNotResolvingString) =>
            throw new NotImplementedException();

        private DatumIndex CompileGameDifficultyExpression(ScriptSymbol gameDifficultySymbol) =>
            throw new NotImplementedException();

        private DatumIndex CompileTeamExpression(ScriptSymbol teamSymbol) =>
            throw new NotImplementedException();

        private DatumIndex CompileMpTeamExpression(ScriptSymbol mpTeamSymbol) =>
            throw new NotImplementedException();

        private DatumIndex CompileControllerExpression(ScriptInteger controllerInteger) =>
            throw new NotImplementedException();

        private DatumIndex CompileButtonPresetExpression(ScriptSymbol buttonPresetSymbol) =>
            throw new NotImplementedException();

        private DatumIndex CompileJoystickPresetExpression(ScriptSymbol joystickPresetSymbol) =>
            throw new NotImplementedException();

        private DatumIndex CompilePlayerColorExpression(ScriptSymbol playerColorSymbol) =>
            throw new NotImplementedException();

        private DatumIndex CompilePlayerCharacterTypeExpression(ScriptSymbol playerCharacterTypeSymbol) =>
            throw new NotImplementedException();

        private DatumIndex CompileVoiceOutputSettingExpression(ScriptSymbol voiceOutputSettingSymbol) =>
            throw new NotImplementedException();

        private DatumIndex CompileVoiceMaskExpression(ScriptSymbol voiceMaskSymbol) =>
            throw new NotImplementedException();

        private DatumIndex CompileSubtitleSettingExpression(ScriptSymbol subtitleSettingSymbol) =>
            throw new NotImplementedException();

        private DatumIndex CompileActorTypeExpression(ScriptSymbol actorTypeSymbol) =>
            throw new NotImplementedException();

        private DatumIndex CompileModelStateExpression(ScriptSymbol modelStateSymbol) =>
            throw new NotImplementedException();

        private DatumIndex CompileEventExpression(ScriptSymbol eventSymbol) =>
            throw new NotImplementedException();

        private DatumIndex CompileCharacterPhysicsExpression(ScriptSymbol characterPhysicsSymbol) =>
            throw new NotImplementedException();

        private DatumIndex CompilePrimarySkullExpression(ScriptSymbol primarySkullSymbol) =>
            throw new NotImplementedException();

        private DatumIndex CompileSecondarySkullExpression(ScriptSymbol secondarySkullSymbol) =>
            throw new NotImplementedException();

        private DatumIndex CompileObjectExpression(ScriptString objectString) =>
            throw new NotImplementedException();

        private DatumIndex CompileUnitExpression(ScriptString unitString) =>
            throw new NotImplementedException();

        private DatumIndex CompileVehicleExpression(ScriptString vehicleString) =>
            throw new NotImplementedException();

        private DatumIndex CompileWeaponExpression(ScriptString weaponString) =>
            throw new NotImplementedException();

        private DatumIndex CompileDeviceExpression(ScriptString deviceString) =>
            throw new NotImplementedException();

        private DatumIndex CompileSceneryExpression(ScriptString sceneryString) =>
            throw new NotImplementedException();

        private DatumIndex CompileEffectSceneryExpression(ScriptString effectSceneryString) =>
            throw new NotImplementedException();

        private DatumIndex CompileObjectNameExpression(ScriptString objectNameString) =>
            throw new NotImplementedException();

        private DatumIndex CompileUnitNameExpression(ScriptString unitNameString) =>
            throw new NotImplementedException();

        private DatumIndex CompileVehicleNameExpression(ScriptString vehicleNameString) =>
            throw new NotImplementedException();

        private DatumIndex CompileWeaponNameExpression(ScriptString weaponNameString) =>
            throw new NotImplementedException();

        private DatumIndex CompileDeviceNameExpression(ScriptString deviceNameString) =>
            throw new NotImplementedException();

        private DatumIndex CompileSceneryNameExpression(ScriptString sceneryNameString) =>
            throw new NotImplementedException();

        private DatumIndex CompileEffectSceneryNameExpression(ScriptString effectSceneryNameString) =>
            throw new NotImplementedException();

        private DatumIndex CompileCinematicLightprobeExpression(ScriptString cinematicLightprobeString) =>
            throw new NotImplementedException();

        private DatumIndex CompileAnimationBudgetReferenceExpression(ScriptString animationBudgetReferenceString) =>
            throw new NotImplementedException();

        private DatumIndex CompileLoopingSoundBudgetReferenceExpression(ScriptString loopingSoundBudgetReferenceString) =>
            throw new NotImplementedException();

        private DatumIndex CompileSoundBudgetReferenceExpression(ScriptString soundBudgetReferenceString) =>
            throw new NotImplementedException();
    }
}