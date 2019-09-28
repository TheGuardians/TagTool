using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Ai;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Scripting.Compiler
{
    public class ScriptCompiler
    {
        public HaloOnlineCacheContext CacheContext { get; }
        public Scenario Definition { get; }

        private List<HsScript> Scripts;
        private List<HsGlobal> Globals;
        private List<HsSyntaxNode> ScriptExpressions;

        private BinaryWriter StringWriter;
        private Dictionary<string, uint> StringOffsets;

        public ScriptCompiler(HaloOnlineCacheContext cacheContext, Scenario definition)
        {
            CacheContext = cacheContext;
            Definition = definition;

            Scripts = new List<HsScript>();
            Globals = new List<HsGlobal>();
            ScriptExpressions = new List<HsSyntaxNode>();

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

            Definition.Globals = Globals;
            Definition.ScriptExpressions = ScriptExpressions;
            Definition.ScriptStrings = (StringWriter.BaseStream as MemoryStream).ToArray();
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
                                    //CompileScript(group);
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

        private HsType ParseHsType(IScriptSyntax node)
        {
            var result = new HsType();

            if (!(node is ScriptSymbol symbol))
                throw new FormatException(node.ToString());

            if (!Enum.TryParse(symbol.Value, true, out result.Halo3Retail))
                result.Halo3Retail = HsType.Halo3RetailValue.Invalid;

            if (!Enum.TryParse(symbol.Value, true, out result.Halo3ODST))
                result.Halo3ODST = HsType.Halo3ODSTValue.Invalid;

            if (!Enum.TryParse(symbol.Value, true, out result.HaloOnline))
                result.HaloOnline = HsType.HaloOnlineValue.Invalid;

            return result;
        }

        private HsScriptType ParseScriptType(IScriptSyntax node)
        {
            if (!(node is ScriptSymbol symbol) ||
                !Enum.TryParse<HsScriptType>(symbol.Value, true, out var result))
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
                !(group.Head is ScriptSymbol symbol && symbol.Value == "global") ||
                !(group.Tail is ScriptGroup declGroup))
            {
                throw new FormatException(node.ToString());
            }

            //
            // Compile the global type
            //

            var globalType = ParseHsType(declGroup.Head);

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

            var globalInit = CompileExpression(globalType.Halo3ODST, declTailTailGroup.Head);

            //
            // Add an entry to the globals block in the scenario definition
            //

            Globals.Add(new HsGlobal
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

            var scriptReturnType = ParseHsType(declTailGroup.Head);

            //
            // Compile the script name and parameters (if any)
            //

            if (!(declTailGroup.Tail is ScriptGroup declTailTailGroup))
                throw new FormatException(declTailGroup.Tail.ToString());

            string scriptName;
            var scriptParams = new List<HsScriptParameter>();

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

                        if (!(declNameGroup.Tail is ScriptGroup tailGroup))
                            throw new FormatException(declNameGroup.Tail.ToString());

                        if (!(tailGroup.Head is ScriptGroup declParamGroup))
                            throw new FormatException(tailGroup.Head.ToString());

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

                            var paramType = ParseHsType(paramDeclType);

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

                            scriptParams.Add(new HsScriptParameter
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
                scriptReturnType.Halo3ODST,
                new ScriptGroup
                {
                    Head = new ScriptSymbol { Value = "begin" },
                    Tail = declTailTailGroup.Tail
                });

            //
            // Add an entry to the scripts block in the scenario definition
            //

            Scripts.Add(new HsScript
            {
                ScriptName = scriptName,
                Type = scriptType,
                ReturnType = scriptReturnType,
                RootExpressionHandle = scriptInit,
                Parameters = scriptParams
            });
        }

        private DatumIndex CompileExpression(HsType.Halo3ODSTValue type, IScriptSyntax node)
        {
            if (node is ScriptGroup group)
                return CompileGroupExpression(type, group);

            switch (type)
            {
                case HsType.Halo3ODSTValue.Boolean:
                    if (node is ScriptBoolean boolValue)
                        return CompileBooleanExpression(boolValue);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Real:
                    if (node is ScriptReal realValue)
                        return CompileRealExpression(realValue);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Short:
                    if (node is ScriptInteger shortValue)
                        return CompileShortExpression(shortValue);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Long:
                    if (node is ScriptInteger longValue)
                        return CompileLongExpression(longValue);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.String:
                    if (node is ScriptString stringValue)
                        return CompileStringExpression(stringValue);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Script:
                    if (node is ScriptSymbol scriptSymbol)
                        return CompileScriptExpression(scriptSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.StringId:
                    if (node is ScriptString stringIdString)
                        return CompileStringIdExpression(stringIdString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.UnitSeatMapping:
                    if (node is ScriptSymbol unitSeatMappingSymbol)
                        return CompileUnitSeatMappingExpression(unitSeatMappingSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.TriggerVolume:
                    if (node is ScriptSymbol triggerVolumeSymbol)
                        return CompileTriggerVolumeExpression(triggerVolumeSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.CutsceneFlag:
                    if (node is ScriptSymbol cutsceneFlagSymbol)
                        return CompileCutsceneFlagExpression(cutsceneFlagSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.CutsceneCameraPoint:
                    if (node is ScriptSymbol cutsceneCameraPointSymbol)
                        return CompileCutsceneCameraPointExpression(cutsceneCameraPointSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.CutsceneTitle:
                    if (node is ScriptSymbol cutsceneTitleSymbol)
                        return CompileCutsceneTitleExpression(cutsceneTitleSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.CutsceneRecording:
                    if (node is ScriptString cutsceneRecordingString)
                        return CompileCutsceneRecordingExpression(cutsceneRecordingString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.DeviceGroup:
                    if (node is ScriptString deviceGroupString)
                        return CompileDeviceGroupExpression(deviceGroupString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Ai:
                    if (node is ScriptSymbol aiSymbol && aiSymbol.Value == "none")
                        return CompileAiExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString aiString)
                        return CompileAiExpression(aiString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.AiCommandList:
                    if (node is ScriptString aiCommandListString)
                        return CompileAiCommandListExpression(aiCommandListString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.AiCommandScript:
                    if (node is ScriptString aiCommandScriptString)
                        return CompileAiCommandScriptExpression(aiCommandScriptString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.AiBehavior:
                    if (node is ScriptString aiBehaviorString)
                        return CompileAiBehaviorExpression(aiBehaviorString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.AiOrders:
                    if (node is ScriptString aiOrdersString)
                        return CompileAiOrdersExpression(aiOrdersString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.AiLine:
                    if (node is ScriptString aiLineString)
                        return CompileAiLineExpression(aiLineString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.StartingProfile:
                    if (node is ScriptString startingProfileString)
                        return CompileStartingProfileExpression(startingProfileString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Conversation:
                    if (node is ScriptString conversationString)
                        return CompileConversationExpression(conversationString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.ZoneSet:
                    if (node is ScriptString zoneSetString)
                        return CompileZoneSetExpression(zoneSetString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.DesignerZone:
                    if (node is ScriptString designerZoneString)
                        return CompileDesignerZoneExpression(designerZoneString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.PointReference:
                    if (node is ScriptString pointReferenceString)
                        return CompilePointReferenceExpression(pointReferenceString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Style:
                    if (node is ScriptString styleString)
                        return CompileStyleExpression(styleString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.ObjectList:
                    if (node is ScriptString objectListString)
                        return CompileObjectListExpression(objectListString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Folder:
                    if (node is ScriptSymbol folderSymbol && folderSymbol.Value == "none")
                        return CompileFolderExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString folderString)
                        return CompileFolderExpression(folderString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Sound:
                    if (node is ScriptSymbol soundSymbol && soundSymbol.Value == "none")
                        return CompileSoundExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString soundString)
                        return CompileSoundExpression(soundString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Effect:
                    if (node is ScriptSymbol effectSymbol && effectSymbol.Value == "none")
                        return CompileEffectExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString effectString)
                        return CompileEffectExpression(effectString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Damage:
                    if (node is ScriptSymbol damageSymbol && damageSymbol.Value == "none")
                        return CompileDamageExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString damageString)
                        return CompileDamageExpression(damageString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.LoopingSound:
                    if (node is ScriptSymbol loopingSoundSymbol && loopingSoundSymbol.Value == "none")
                        return CompileLoopingSoundExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString loopingSoundString)
                        return CompileLoopingSoundExpression(loopingSoundString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.AnimationGraph:
                    if (node is ScriptSymbol animationGraphSymbol && animationGraphSymbol.Value == "none")
                        return CompileAnimationGraphExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString animationGraphString)
                        return CompileAnimationGraphExpression(animationGraphString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.DamageEffect:
                    if (node is ScriptSymbol damageEffectSymbol && damageEffectSymbol.Value == "none")
                        return CompileDamageEffectExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString damageEffectString)
                        return CompileDamageEffectExpression(damageEffectString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.ObjectDefinition:
                    if (node is ScriptSymbol objectDefinitionSymbol && objectDefinitionSymbol.Value == "none")
                        return CompileObjectDefinitionExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString objectDefinitionString)
                        return CompileObjectDefinitionExpression(objectDefinitionString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Bitmap:
                    if (node is ScriptSymbol bitmapSymbol && bitmapSymbol.Value == "none")
                        return CompileBitmapExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString bitmapString)
                        return CompileBitmapExpression(bitmapString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Shader:
                    if (node is ScriptSymbol shaderSymbol && shaderSymbol.Value == "none")
                        return CompileShaderExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString shaderString)
                        return CompileShaderExpression(shaderString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.RenderModel:
                    if (node is ScriptSymbol renderModelSymbol && renderModelSymbol.Value == "none")
                        return CompileRenderModelExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString renderModelString)
                        return CompileRenderModelExpression(renderModelString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.StructureDefinition:
                    if (node is ScriptSymbol structureDefinitionSymbol && structureDefinitionSymbol.Value == "none")
                        return CompileStructureDefinitionExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString structureDefinitionString)
                        return CompileStructureDefinitionExpression(structureDefinitionString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.LightmapDefinition:
                    if (node is ScriptSymbol lightmapDefinitionSymbol && lightmapDefinitionSymbol.Value == "none")
                        return CompileLightmapDefinitionExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString lightmapDefinitionString)
                        return CompileLightmapDefinitionExpression(lightmapDefinitionString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.CinematicDefinition:
                    if (node is ScriptSymbol cinematicDefinitionSymbol && cinematicDefinitionSymbol.Value == "none")
                        return CompileCinematicDefinitionExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString cinematicDefinitionString)
                        return CompileCinematicDefinitionExpression(cinematicDefinitionString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.CinematicSceneDefinition:
                    if (node is ScriptSymbol cinematicSceneSymbol && cinematicSceneSymbol.Value == "none")
                        return CompileCinematicSceneDefinitionExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString cinematicSceneDefinitionString)
                        return CompileCinematicSceneDefinitionExpression(cinematicSceneDefinitionString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.BinkDefinition:
                    if (node is ScriptSymbol binkDefinitionSymbol && binkDefinitionSymbol.Value == "none")
                        return CompileBinkDefinitionExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString binkDefinitionString)
                        return CompileBinkDefinitionExpression(binkDefinitionString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.AnyTag:
                    if (node is ScriptSymbol anyTagSymbol && anyTagSymbol.Value == "none")
                        return CompileAnyTagExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString anyTagString)
                        return CompileAnyTagExpression(anyTagString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.AnyTagNotResolving:
                    if (node is ScriptSymbol anyTagNotResolvingSymbol && anyTagNotResolvingSymbol.Value == "none")
                        return CompileAnyTagNotResolvingExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString anyTagNotResolvingString)
                        return CompileAnyTagNotResolvingExpression(anyTagNotResolvingString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.GameDifficulty:
                    if (node is ScriptSymbol gameDifficultySymbol)
                        return CompileGameDifficultyExpression(gameDifficultySymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Team:
                    if (node is ScriptSymbol teamSymbol)
                        return CompileTeamExpression(teamSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.MpTeam:
                    if (node is ScriptSymbol mpTeamSymbol)
                        return CompileMpTeamExpression(mpTeamSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Controller:
                    if (node is ScriptSymbol controllerSymbol)
                        return CompileControllerExpression(controllerSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.ButtonPreset:
                    if (node is ScriptSymbol buttonPresetSymbol)
                        return CompileButtonPresetExpression(buttonPresetSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.JoystickPreset:
                    if (node is ScriptSymbol joystickPresetSymbol)
                        return CompileJoystickPresetExpression(joystickPresetSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.PlayerColor:
                    if (node is ScriptSymbol playerColorSymbol)
                        return CompilePlayerColorExpression(playerColorSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.PlayerCharacterType:
                    if (node is ScriptSymbol playerCharacterTypeSymbol)
                        return CompilePlayerCharacterTypeExpression(playerCharacterTypeSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.VoiceOutputSetting:
                    if (node is ScriptSymbol voiceOutputSettingSymbol)
                        return CompileVoiceOutputSettingExpression(voiceOutputSettingSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.VoiceMask:
                    if (node is ScriptSymbol voiceMaskSymbol)
                        return CompileVoiceMaskExpression(voiceMaskSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.SubtitleSetting:
                    if (node is ScriptSymbol subtitleSettingSymbol)
                        return CompileSubtitleSettingExpression(subtitleSettingSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.ActorType:
                    if (node is ScriptSymbol actorTypeSymbol)
                        return CompileActorTypeExpression(actorTypeSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.ModelState:
                    if (node is ScriptSymbol modelStateSymbol)
                        return CompileModelStateExpression(modelStateSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Event:
                    if (node is ScriptSymbol eventSymbol)
                        return CompileEventExpression(eventSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.CharacterPhysics:
                    if (node is ScriptSymbol characterPhysicsSymbol)
                        return CompileCharacterPhysicsExpression(characterPhysicsSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.PrimarySkull:
                    if (node is ScriptSymbol primarySkullSymbol)
                        return CompilePrimarySkullExpression(primarySkullSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.SecondarySkull:
                    if (node is ScriptSymbol secondarySkullSymbol)
                        return CompileSecondarySkullExpression(secondarySkullSymbol);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Object:
                    if (node is ScriptSymbol objectSymbol && objectSymbol.Value == "none")
                        return CompileObjectExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString objectString)
                        return CompileObjectExpression(objectString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Unit:
                    if (node is ScriptSymbol unitSymbol && unitSymbol.Value == "none")
                        return CompileUnitExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString unitString)
                        return CompileUnitExpression(unitString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Vehicle:
                    if (node is ScriptSymbol vehicleSymbol && vehicleSymbol.Value == "none")
                        return CompileVehicleExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString vehicleString)
                        return CompileVehicleExpression(vehicleString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Weapon:
                    if (node is ScriptSymbol weaponSymbol && weaponSymbol.Value == "none")
                        return CompileWeaponExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString weaponString)
                        return CompileWeaponExpression(weaponString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Device:
                    if (node is ScriptSymbol deviceSymbol && deviceSymbol.Value == "none")
                        return CompileDeviceExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString deviceString)
                        return CompileDeviceExpression(deviceString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Scenery:
                    if (node is ScriptSymbol scenerySymbol && scenerySymbol.Value == "none")
                        return CompileSceneryExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString sceneryString)
                        return CompileSceneryExpression(sceneryString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.EffectScenery:
                    if (node is ScriptSymbol effectScenerySymbol && effectScenerySymbol.Value == "none")
                        return CompileEffectSceneryExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString effectSceneryString)
                        return CompileEffectSceneryExpression(effectSceneryString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.ObjectName:
                    if (node is ScriptString objectNameString)
                        return CompileObjectNameExpression(objectNameString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.UnitName:
                    if (node is ScriptString unitNameString)
                        return CompileUnitNameExpression(unitNameString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.VehicleName:
                    if (node is ScriptString vehicleNameString)
                        return CompileVehicleNameExpression(vehicleNameString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.WeaponName:
                    if (node is ScriptString weaponNameString)
                        return CompileWeaponNameExpression(weaponNameString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.DeviceName:
                    if (node is ScriptString deviceNameString)
                        return CompileDeviceNameExpression(deviceNameString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.SceneryName:
                    if (node is ScriptString sceneryNameString)
                        return CompileSceneryNameExpression(sceneryNameString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.EffectSceneryName:
                    if (node is ScriptString effectSceneryNameString)
                        return CompileEffectSceneryNameExpression(effectSceneryNameString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.CinematicLightprobe:
                    if (node is ScriptString cinematicLightprobeString)
                        return CompileCinematicLightprobeExpression(cinematicLightprobeString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.AnimationBudgetReference:
                    if (node is ScriptString animationBudgetReferenceString)
                        return CompileAnimationBudgetReferenceExpression(animationBudgetReferenceString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.LoopingSoundBudgetReference:
                    if (node is ScriptString loopingSoundBudgetReferenceString)
                        return CompileLoopingSoundBudgetReferenceExpression(loopingSoundBudgetReferenceString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.SoundBudgetReference:
                    if (node is ScriptString soundBudgetReferenceString)
                        return CompileSoundBudgetReferenceExpression(soundBudgetReferenceString);
                    else throw new FormatException(node.ToString());
            }

            throw new NotImplementedException(type.ToString());
        }

        private DatumIndex AllocateExpression(HsType.Halo3ODSTValue valueType, HsSyntaxNodeFlags expressionType, ushort? opcode = null)
        {
            ushort salt = 0; // TODO?
            uint stringAddress = 0; // TODO?

            var expression = new HsSyntaxNode
            {
                Identifier = salt,
                Opcode = opcode ?? (ushort)valueType,
                ValueType = new HsType { Halo3ODST = valueType },
                Flags = expressionType,
                NextExpressionHandle = DatumIndex.None,
                StringAddress = stringAddress,
                Data = BitConverter.GetBytes(-1),
                LineNumber = -1,
            };

            if (!Enum.TryParse(valueType.ToString(), out expression.ValueType.Halo3Retail))
                expression.ValueType.Halo3Retail = HsType.Halo3RetailValue.Invalid;

            if (!Enum.TryParse(valueType.ToString(), out expression.ValueType.HaloOnline))
                expression.ValueType.HaloOnline = HsType.HaloOnlineValue.Invalid;

            ScriptExpressions.Add(expression);

            return new DatumIndex(salt, (ushort)ScriptExpressions.IndexOf(expression));
        }

        private DatumIndex CompileGroupExpression(HsType.Halo3ODSTValue type, ScriptGroup group)
        {
            var handle = AllocateExpression(type, HsSyntaxNodeFlags.DoNotGC);

            if (!(group.Head is ScriptSymbol functionNameSymbol))
                throw new FormatException(group.Head.ToString());

            if (!(group.Tail is ScriptGroup parametersGroup))
                throw new FormatException(group.Tail.ToString());

            //
            // Allocate the function name expression
            //

            var functionNameHandle = AllocateExpression(HsType.Halo3ODSTValue.FunctionName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);
            var functionNameExpr = ScriptExpressions[functionNameHandle.Index];
            functionNameExpr.StringAddress = CompileStringAddress(functionNameSymbol.Value);

            //
            // Handle passthrough functions
            //

            switch (functionNameSymbol.Value)
            {
                case "begin":
                    {
                        var prevExpr = functionNameExpr;

                        for (IScriptSyntax current = parametersGroup;
                            current is ScriptGroup currentGroup;
                            current = currentGroup.Tail)
                        {
                            prevExpr.NextExpressionHandle = CompileExpression(HsType.Halo3ODSTValue.Unparsed, currentGroup.Head);
                            prevExpr = ScriptExpressions[prevExpr.NextExpressionHandle.Index];
                        }

                        break;
                    }

                case "begin_random":
                    throw new NotImplementedException("begin_random");

                case "if":
                    throw new NotImplementedException("if");

                case "cond":
                    throw new NotImplementedException("cond");

                case "set":
                    throw new NotImplementedException("set");
            }

            //
            // Check if function name is a built-in function
            //

            var functions = new List<(ushort Opcode, ScriptInfo Info)>();

            foreach (var entry in ScriptInfo.Scripts[CacheContext.Version])
            {
                if (functionNameSymbol.Value == entry.Value.Name)
                {
                    functions.Add(((ushort)entry.Key, entry.Value));
                    break;
                }
            }

            if (functions.Count > 0)
            {
                //
                // Verify supplied parameter expressions
                //

                foreach (var function in functions)
                {
                    foreach (var param in function.Info.Parameters)
                    {
                        //
                        // TODO: check if parameter is a symbol and make either
                        //       a global reference or a parameter reference
                        //
                    }
                }

                return handle;
            }

            //
            // TODO: Check if function name is a script
            //

            return handle;
        }

        private DatumIndex CompileBooleanExpression(ScriptBoolean boolValue)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Boolean, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(boolValue.Value.ToString().ToLower());
                Array.Copy(BitConverter.GetBytes(boolValue.Value), expr.Data, 1);
            }

            return handle;
        }

        private DatumIndex CompileRealExpression(ScriptReal realValue)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Real, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
                Array.Copy(BitConverter.GetBytes((float)realValue.Value), ScriptExpressions[handle.Index].Data, 4);

            return handle;
        }

        private DatumIndex CompileShortExpression(ScriptInteger shortValue)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Short, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
                Array.Copy(BitConverter.GetBytes((short)shortValue.Value), ScriptExpressions[handle.Index].Data, 2);

            return handle;
        }

        private DatumIndex CompileLongExpression(ScriptInteger longValue)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Long, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
                Array.Copy(BitConverter.GetBytes((int)longValue.Value), ScriptExpressions[handle.Index].Data, 4);

            return handle;
        }

        private DatumIndex CompileStringExpression(ScriptString stringValue)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.String, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
                ScriptExpressions[handle.Index].StringAddress = CompileStringAddress(stringValue.Value);

            return handle;
        }

        private DatumIndex CompileScriptExpression(ScriptSymbol scriptSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Script, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var scriptIndex = Scripts.FindIndex(s => s.ScriptName == scriptSymbol.Value);

                if (scriptIndex == -1)
                    throw new FormatException(scriptSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(scriptSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)scriptIndex), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileStringIdExpression(ScriptString stringIdString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.StringId, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.UnitSeatMapping, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                ScriptExpressions[handle.Index].StringAddress = CompileStringAddress(unitSeatMappingSymbol.Value);

                //
                // TODO: Compile unit_seat_mapping data here
                //

                throw new NotImplementedException(HsType.Halo3ODSTValue.UnitSeatMapping.ToString());
            }

            return handle;
        }

        private DatumIndex CompileTriggerVolumeExpression(ScriptSymbol triggerVolumeSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.TriggerVolume, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var triggerVolumeIndex = Definition.TriggerVolumes.FindIndex(tv => triggerVolumeSymbol.Value == CacheContext.GetString(tv.Name));

                if (triggerVolumeIndex == -1)
                    throw new FormatException(triggerVolumeSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(triggerVolumeSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)triggerVolumeIndex), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileCutsceneFlagExpression(ScriptSymbol cutsceneFlagSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.CutsceneFlag, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var cutsceneFlagIndex = Definition.CutsceneFlags.FindIndex(cf => cutsceneFlagSymbol.Value == CacheContext.GetString(cf.Name));

                if (cutsceneFlagIndex == -1)
                    throw new FormatException(cutsceneFlagSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(cutsceneFlagSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)cutsceneFlagIndex), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileCutsceneCameraPointExpression(ScriptSymbol cutsceneCameraPointSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.CutsceneCameraPoint, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var cutsceneCameraPointIndex = Definition.CutsceneCameraPoints.FindIndex(ccp => cutsceneCameraPointSymbol.Value == ccp.Name);

                if (cutsceneCameraPointIndex == -1)
                    throw new FormatException(cutsceneCameraPointSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(cutsceneCameraPointSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)cutsceneCameraPointIndex), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileCutsceneTitleExpression(ScriptSymbol cutsceneTitleSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.CutsceneTitle, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var cutsceneTitleIndex = Definition.CutsceneTitles.FindIndex(ct => cutsceneTitleSymbol.Value == CacheContext.GetString(ct.Name));

                if (cutsceneTitleIndex == -1)
                    throw new FormatException(cutsceneTitleSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(cutsceneTitleSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)cutsceneTitleIndex), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileCutsceneRecordingExpression(ScriptString cutsceneRecordingString) =>
            throw new NotImplementedException();

        private DatumIndex CompileDeviceGroupExpression(ScriptString deviceGroupString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.DeviceGroup, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var deviceGroupIndex = Definition.DeviceGroups.FindIndex(dg => dg.Name == deviceGroupString.Value);

                if (deviceGroupIndex == -1)
                    throw new FormatException(deviceGroupString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(deviceGroupString.Value);
                Array.Copy(BitConverter.GetBytes(deviceGroupIndex), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileAiExpression(ScriptString aiString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Ai, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var tokens = aiString.Value.Split('/');
                var value = 0;

                switch (tokens.Length)
                {
                    case 1:
                        {
                            if (aiString.Value == "none")
                            {
                                value = -1;
                                break;
                            }

                            //
                            // type 1: squad
                            //

                            var squadIndex = Definition.Squads.FindIndex(s => s.Name == tokens[0]);

                            if (squadIndex != -1)
                            {
                                value = (1 << 29) | (squadIndex & 0xFFFF);
                                break;
                            }

                            //
                            // type 2: squad group
                            //

                            var squadGroupIndex = Definition.SquadGroups.FindIndex(sg => sg.Name == tokens[0]);

                            if (squadGroupIndex != -1)
                            {
                                value = (2 << 29) | (squadGroupIndex & 0xFFFF);
                                break;
                            }

                            //
                            // type 2: actor datum index
                            //  TODO?
                            //

                            //
                            // type 6: objective (without task)
                            //

                            var objectiveIndex = Definition.AiObjectives.FindIndex(o => tokens[0] == CacheContext.GetString(o.Name));

                            if (objectiveIndex != -1)
                            {
                                value = (6 << 29) | (0x1FFF << 16) | (objectiveIndex & 0xFFFF);
                                break;
                            }

                            goto default;
                        }

                    case 2:
                        {
                            var squadIndex = Definition.Squads.FindIndex(s => s.Name == tokens[0]);

                            if (squadIndex != -1)
                            {
                                var squad = Definition.Squads[squadIndex];

                                //
                                // type 4: spawn point
                                //

                                var spawnPointIndex = squad.SpawnPoints.FindIndex(sp => tokens[1] == CacheContext.GetString(sp.Name));

                                if (spawnPointIndex != -1)
                                {
                                    value = (4 << 29) | ((squadIndex & 0x1FFF) << 16) | (spawnPointIndex & 0xFF);
                                    break;
                                }

                                //
                                // type 5: spawn formation
                                //

                                var spawnFormationIndex = squad.SpawnFormations.FindIndex(sf => tokens[1] == CacheContext.GetString(sf.Name));

                                if (spawnFormationIndex != -1)
                                {
                                    value = (5 << 29) | ((squadIndex & 0x1FFF) << 16) | (spawnFormationIndex & 0xFF);
                                    break;
                                }

                                goto default;
                            }

                            //
                            // type 6: objective task
                            //

                            var objectiveIndex = Definition.AiObjectives.FindIndex(o => tokens[0] == CacheContext.GetString(o.Name));

                            if (objectiveIndex != -1)
                            {
                                var taskIndex = Definition.AiObjectives[objectiveIndex].Tasks.FindIndex(t => tokens[1] == CacheContext.GetString(t.Name));

                                if (taskIndex != -1)
                                {
                                    value = (6 << 29) | ((taskIndex & 0x1FFF) << 16) | (objectiveIndex & 0xFFFF);
                                    break;
                                }
                            }

                            goto default;
                        }

                    default:
                        throw new FormatException(aiString.Value);
                }
            }

            return handle;
        }

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

        private DatumIndex CompileStartingProfileExpression(ScriptString startingProfileString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.StartingProfile, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var startingProfileIndex = Definition.PlayerStartingProfile.FindIndex(sp => sp.Name == startingProfileString.Value);

                if (startingProfileIndex == -1)
                    throw new FormatException(startingProfileString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(startingProfileString.Value);
                Array.Copy(BitConverter.GetBytes((short)startingProfileIndex), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileConversationExpression(ScriptString conversationString) =>
            throw new NotImplementedException();

        private DatumIndex CompileZoneSetExpression(ScriptString zoneSetString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.ZoneSet, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var zoneSetIndex = Definition.ZoneSets.FindIndex(zs => zoneSetString.Value == CacheContext.GetString(zs.Name));

                if (zoneSetIndex == -1)
                    throw new FormatException(zoneSetString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(zoneSetString.Value);
                Array.Copy(BitConverter.GetBytes((short)zoneSetIndex), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileDesignerZoneExpression(ScriptString designerZoneString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.ZoneSet, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var designerZoneIndex = Definition.DesignerZoneSets.FindIndex(dz => designerZoneString.Value == CacheContext.GetString(dz.Name));

                if (designerZoneIndex == -1)
                    throw new FormatException(designerZoneString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(designerZoneString.Value);
                Array.Copy(BitConverter.GetBytes((short)designerZoneIndex), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompilePointReferenceExpression(ScriptString pointReferenceString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.PointReference, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var tokens = pointReferenceString.Value.Split('/');

                if (tokens.Length != 2)
                    throw new FormatException(pointReferenceString.Value);

                var pointSetIndex = Definition.ScriptingData[0].PointSets.FindIndex(ps => ps.Name == tokens[0]);

                if (pointSetIndex == -1)
                    throw new FormatException(pointReferenceString.Value);

                var pointIndex = Definition.ScriptingData[0].PointSets[pointSetIndex].Points.FindIndex(p => p.Name == tokens[1]);

                if (pointIndex == -1)
                    throw new FormatException(pointReferenceString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(pointReferenceString.Value);
                Array.Copy(BitConverter.GetBytes((int)((ushort)pointIndex | (ushort)(pointSetIndex << 16))), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileStyleExpression(ScriptString styleString) =>
            throw new NotImplementedException();

        private DatumIndex CompileObjectListExpression(ScriptString objectListString) =>
            throw new NotImplementedException();

        private DatumIndex CompileFolderExpression(ScriptString folderString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Folder, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var folderIndex = folderString.Value == "none" ? -1 : Definition.EditorFolders.FindIndex(ef => ef.Name == folderString.Value);

                if (folderString.Value != "none" && folderIndex == -1)
                    throw new FormatException(folderString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(folderString.Value);
                Array.Copy(BitConverter.GetBytes(folderIndex), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileSoundExpression(ScriptString soundString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Sound, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!CacheContext.TryGetTag(soundString.Value, out var instance) ||
                    !instance.IsInGroup<Sound>())
                {
                    throw new FormatException(soundString.Value);
                }

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(soundString.Value);
                Array.Copy(BitConverter.GetBytes(instance?.Index ?? -1), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileEffectExpression(ScriptString effectString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Effect, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!CacheContext.TryGetTag(effectString.Value, out var instance) ||
                    !instance.IsInGroup<Effect>())
                {
                    throw new FormatException(effectString.Value);
                }

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(effectString.Value);
                Array.Copy(BitConverter.GetBytes(instance.Index), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileDamageExpression(ScriptString damageString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Damage, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!CacheContext.TryGetTag(damageString.Value, out var instance) ||
                    !instance.IsInGroup<DamageEffect>())
                {
                    throw new FormatException(damageString.Value);
                }

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(damageString.Value);
                Array.Copy(BitConverter.GetBytes(instance.Index), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileLoopingSoundExpression(ScriptString loopingSoundString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.LoopingSound, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!CacheContext.TryGetTag(loopingSoundString.Value, out var instance) ||
                    !instance.IsInGroup<SoundLooping>())
                {
                    throw new FormatException(loopingSoundString.Value);
                }

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(loopingSoundString.Value);
                Array.Copy(BitConverter.GetBytes(instance.Index), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileAnimationGraphExpression(ScriptString animationGraphString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.AnimationGraph, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!CacheContext.TryGetTag(animationGraphString.Value, out var instance) ||
                    !instance.IsInGroup<ModelAnimationGraph>())
                {
                    throw new FormatException(animationGraphString.Value);
                }

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(animationGraphString.Value);
                Array.Copy(BitConverter.GetBytes(instance.Index), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileDamageEffectExpression(ScriptString damageEffectString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.DamageEffect, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!CacheContext.TryGetTag(damageEffectString.Value, out var instance) ||
                    !instance.IsInGroup<DamageEffect>())
                {
                    throw new FormatException(damageEffectString.Value);
                }

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(damageEffectString.Value);
                Array.Copy(BitConverter.GetBytes(instance.Index), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileObjectDefinitionExpression(ScriptString objectDefinitionString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.ObjectDefinition, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!CacheContext.TryGetTag(objectDefinitionString.Value, out var instance) ||
                    !instance.IsInGroup<GameObject>())
                {
                    throw new FormatException(objectDefinitionString.Value);
                }

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(objectDefinitionString.Value);
                Array.Copy(BitConverter.GetBytes(instance.Index), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileBitmapExpression(ScriptString bitmapString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Bitmap, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!CacheContext.TryGetTag(bitmapString.Value, out var instance) ||
                    !instance.IsInGroup<Bitmap>())
                {
                    throw new FormatException(bitmapString.Value);
                }

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(bitmapString.Value);
                Array.Copy(BitConverter.GetBytes(instance.Index), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileShaderExpression(ScriptString shaderString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Shader, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!CacheContext.TryGetTag(shaderString.Value, out var instance) ||
                    !instance.IsInGroup<RenderMethod>())
                {
                    throw new FormatException(shaderString.Value);
                }

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(shaderString.Value);
                Array.Copy(BitConverter.GetBytes(instance.Index), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileRenderModelExpression(ScriptString renderModelString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.RenderModel, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!CacheContext.TryGetTag(renderModelString.Value, out var instance) ||
                    !instance.IsInGroup<RenderModel>())
                {
                    throw new FormatException(renderModelString.Value);
                }

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(renderModelString.Value);
                Array.Copy(BitConverter.GetBytes(instance.Index), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileStructureDefinitionExpression(ScriptString structureDefinitionString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.StructureDefinition, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!CacheContext.TryGetTag(structureDefinitionString.Value, out var instance) ||
                    !instance.IsInGroup<ScenarioStructureBsp>())
                {
                    throw new FormatException(structureDefinitionString.Value);
                }

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(structureDefinitionString.Value);
                Array.Copy(BitConverter.GetBytes(instance.Index), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileLightmapDefinitionExpression(ScriptString lightmapDefinitionString) =>
            throw new NotImplementedException();

        private DatumIndex CompileCinematicDefinitionExpression(ScriptString cinematicDefinitionString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.CinematicDefinition, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!CacheContext.TryGetTag(cinematicDefinitionString.Value, out var instance) ||
                    !instance.IsInGroup<Cinematic>())
                {
                    throw new FormatException(cinematicDefinitionString.Value);
                }

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(cinematicDefinitionString.Value);
                Array.Copy(BitConverter.GetBytes(instance.Index), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileCinematicSceneDefinitionExpression(ScriptString cinematicSceneDefinitionString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.CinematicSceneDefinition, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!CacheContext.TryGetTag(cinematicSceneDefinitionString.Value, out var instance) ||
                    !instance.IsInGroup<CinematicScene>())
                {
                    throw new FormatException(cinematicSceneDefinitionString.Value);
                }

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(cinematicSceneDefinitionString.Value);
                Array.Copy(BitConverter.GetBytes(instance.Index), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileBinkDefinitionExpression(ScriptString binkDefinitionString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.BinkDefinition, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!CacheContext.TryGetTag(binkDefinitionString.Value, out var instance) ||
                    !instance.IsInGroup<Bink>())
                {
                    throw new FormatException(binkDefinitionString.Value);
                }

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(binkDefinitionString.Value);
                Array.Copy(BitConverter.GetBytes(instance.Index), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileAnyTagExpression(ScriptString anyTagString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.AnyTag, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!CacheContext.TryGetTag(anyTagString.Value, out var instance))
                    throw new FormatException(anyTagString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(anyTagString.Value);
                Array.Copy(BitConverter.GetBytes(instance.Index), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileAnyTagNotResolvingExpression(ScriptString anyTagNotResolvingString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.AnyTagNotResolving, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!CacheContext.TryGetTag(anyTagNotResolvingString.Value, out var instance))
                    throw new FormatException(anyTagNotResolvingString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(anyTagNotResolvingString.Value);
                Array.Copy(BitConverter.GetBytes(instance.Index), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileGameDifficultyExpression(ScriptSymbol gameDifficultySymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.GameDifficulty, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!Enum.TryParse<GameDifficulty>(gameDifficultySymbol.Value, true, out var difficulty))
                    throw new FormatException(gameDifficultySymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(gameDifficultySymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)difficulty), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileTeamExpression(ScriptSymbol teamSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Team, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!Enum.TryParse<GameTeam>(teamSymbol.Value, true, out var team))
                    throw new FormatException(teamSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(teamSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)team), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileMpTeamExpression(ScriptSymbol mpTeamSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.MpTeam, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!Enum.TryParse<GameMultiplayerTeam>(mpTeamSymbol.Value, true, out var mpTeam))
                    throw new FormatException(mpTeamSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(mpTeamSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)mpTeam), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileControllerExpression(ScriptSymbol controllerSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Controller, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!Enum.TryParse<GameController>(controllerSymbol.Value, true, out var controller))
                    throw new FormatException(controllerSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(controllerSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)controller), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileButtonPresetExpression(ScriptSymbol buttonPresetSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.ButtonPreset, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!Enum.TryParse<GameControllerButtonPreset>(buttonPresetSymbol.Value, true, out var buttonPreset))
                    throw new FormatException(buttonPresetSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(buttonPresetSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)buttonPreset), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileJoystickPresetExpression(ScriptSymbol joystickPresetSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.JoystickPreset, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!Enum.TryParse<GameControllerJoystickPreset>(joystickPresetSymbol.Value, true, out var joystickPreset))
                    throw new FormatException(joystickPresetSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(joystickPresetSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)joystickPreset), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompilePlayerColorExpression(ScriptSymbol playerColorSymbol) =>
            throw new NotImplementedException();

        private DatumIndex CompilePlayerCharacterTypeExpression(ScriptSymbol playerCharacterTypeSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.PlayerCharacterType, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!Enum.TryParse<GamePlayerCharacterType>(playerCharacterTypeSymbol.Value, true, out var playerCharacterType))
                    throw new FormatException(playerCharacterTypeSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(playerCharacterTypeSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)playerCharacterType), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileVoiceOutputSettingExpression(ScriptSymbol voiceOutputSettingSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.VoiceOutputSetting, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!Enum.TryParse<GameVoiceOutputSetting>(voiceOutputSettingSymbol.Value, true, out var voiceOutputSetting))
                    throw new FormatException(voiceOutputSettingSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(voiceOutputSettingSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)voiceOutputSetting), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileVoiceMaskExpression(ScriptSymbol voiceMaskSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.VoiceMask, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!Enum.TryParse<GameVoiceMask>(voiceMaskSymbol.Value, true, out var voiceMask))
                    throw new FormatException(voiceMaskSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(voiceMaskSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)voiceMask), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileSubtitleSettingExpression(ScriptSymbol subtitleSettingSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.SubtitleSetting, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!Enum.TryParse<GameSubtitleSetting>(subtitleSettingSymbol.Value, true, out var subtitleSetting))
                    throw new FormatException(subtitleSettingSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(subtitleSettingSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)subtitleSetting), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileActorTypeExpression(ScriptSymbol actorTypeSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.ActorType, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!Enum.TryParse<AiActorType>(actorTypeSymbol.Value, true, out var actorType))
                    throw new FormatException(actorTypeSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(actorTypeSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)actorType), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileModelStateExpression(ScriptSymbol modelStateSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.ModelState, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!Enum.TryParse<GameModelState>(modelStateSymbol.Value, true, out var modelState))
                    throw new FormatException(modelStateSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(modelStateSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)modelState), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileEventExpression(ScriptSymbol eventSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Event, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!Enum.TryParse<GameEventType>(eventSymbol.Value, true, out var eventType))
                    throw new FormatException(eventSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(eventSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)eventType), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileCharacterPhysicsExpression(ScriptSymbol characterPhysicsSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.CharacterPhysics, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!Enum.TryParse<GameCharacterPhysics>(characterPhysicsSymbol.Value, true, out var characterPhysics))
                    throw new FormatException(characterPhysicsSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(characterPhysicsSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)characterPhysics), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompilePrimarySkullExpression(ScriptSymbol primarySkullSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.PrimarySkull, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!Enum.TryParse<GamePrimarySkull>(primarySkullSymbol.Value, true, out var primarySkull))
                    throw new FormatException(primarySkullSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(primarySkullSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)primarySkull), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileSecondarySkullExpression(ScriptSymbol secondarySkullSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.SecondarySkull, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                if (!Enum.TryParse<GameSecondarySkull>(secondarySkullSymbol.Value, true, out var secondarySkull))
                    throw new FormatException(secondarySkullSymbol.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(secondarySkullSymbol.Value);
                Array.Copy(BitConverter.GetBytes((short)secondarySkull), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileObjectExpression(ScriptString objectString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Object, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var objectIndex = objectString.Value == "none" ? -1 : throw new NotImplementedException();

                if (objectString.Value != "none" && objectIndex == -1)
                    throw new FormatException(objectString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(objectString.Value);
                Array.Copy(BitConverter.GetBytes(objectIndex), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileUnitExpression(ScriptString unitString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Unit, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var unitIndex = unitString.Value == "none" ? -1 : throw new NotImplementedException();

                if (unitString.Value != "none" && unitIndex == -1)
                    throw new FormatException(unitString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(unitString.Value);
                Array.Copy(BitConverter.GetBytes(unitIndex), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileVehicleExpression(ScriptString vehicleString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Vehicle, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var vehicleIndex = vehicleString.Value == "none" ? -1 : throw new NotImplementedException();

                if (vehicleString.Value != "none" && vehicleIndex == -1)
                    throw new FormatException(vehicleString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(vehicleString.Value);
                Array.Copy(BitConverter.GetBytes(vehicleIndex), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileWeaponExpression(ScriptString weaponString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Weapon, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var weaponIndex = weaponString.Value == "none" ? -1 : throw new NotImplementedException();

                if (weaponString.Value != "none" && weaponIndex == -1)
                    throw new FormatException(weaponString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(weaponString.Value);
                Array.Copy(BitConverter.GetBytes(weaponIndex), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileDeviceExpression(ScriptString deviceString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Device, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var deviceIndex = deviceString.Value == "none" ? -1 : throw new NotImplementedException();

                if (deviceString.Value != "none" && deviceIndex == -1)
                    throw new FormatException(deviceString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(deviceString.Value);
                Array.Copy(BitConverter.GetBytes(deviceIndex), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileSceneryExpression(ScriptString sceneryString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Scenery, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var sceneryIndex = sceneryString.Value == "none" ? -1 : throw new NotImplementedException();

                if (sceneryString.Value != "none" && sceneryIndex == -1)
                    throw new FormatException(sceneryString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(sceneryString.Value);
                Array.Copy(BitConverter.GetBytes(sceneryIndex), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileEffectSceneryExpression(ScriptString effectSceneryString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.EffectScenery, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var effectSceneryIndex = effectSceneryString.Value == "none" ? -1 : throw new NotImplementedException();

                if (effectSceneryString.Value != "none" && effectSceneryIndex == -1)
                    throw new FormatException(effectSceneryString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(effectSceneryString.Value);
                Array.Copy(BitConverter.GetBytes(effectSceneryIndex), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileObjectNameExpression(ScriptString objectNameString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.ObjectName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var objectNameIndex = Definition.ObjectNames.FindIndex(on => on.Name == objectNameString.Value);

                if (objectNameIndex == -1)
                    throw new FormatException(objectNameString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(objectNameString.Value);
                Array.Copy(BitConverter.GetBytes((short)objectNameIndex), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileUnitNameExpression(ScriptString objectNameString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.UnitName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var objectNameIndex = Definition.ObjectNames.FindIndex(on => on.Name == objectNameString.Value);

                if (objectNameIndex == -1)
                    throw new FormatException(objectNameString.Value);

                if (Definition.ObjectNames[objectNameIndex].ObjectType.Halo3ODST != GameObjectTypeHalo3ODST.Biped ||
                    Definition.ObjectNames[objectNameIndex].ObjectType.Halo3ODST != GameObjectTypeHalo3ODST.Giant ||
                    Definition.ObjectNames[objectNameIndex].ObjectType.Halo3ODST != GameObjectTypeHalo3ODST.Vehicle)
                {
                    throw new FormatException(objectNameString.Value);
                }

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(objectNameString.Value);
                Array.Copy(BitConverter.GetBytes((short)objectNameIndex), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileVehicleNameExpression(ScriptString objectNameString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.VehicleName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var objectNameIndex = Definition.ObjectNames.FindIndex(on => on.Name == objectNameString.Value);

                if (objectNameIndex == -1)
                    throw new FormatException(objectNameString.Value);

                if (Definition.ObjectNames[objectNameIndex].ObjectType.Halo3ODST != GameObjectTypeHalo3ODST.Vehicle)
                    throw new FormatException(objectNameString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(objectNameString.Value);
                Array.Copy(BitConverter.GetBytes((short)objectNameIndex), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileWeaponNameExpression(ScriptString objectNameString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.WeaponName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var objectNameIndex = Definition.ObjectNames.FindIndex(on => on.Name == objectNameString.Value);

                if (objectNameIndex == -1)
                    throw new FormatException(objectNameString.Value);

                if (Definition.ObjectNames[objectNameIndex].ObjectType.Halo3ODST != GameObjectTypeHalo3ODST.Weapon)
                    throw new FormatException(objectNameString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(objectNameString.Value);
                Array.Copy(BitConverter.GetBytes((short)objectNameIndex), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileDeviceNameExpression(ScriptString objectNameString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.DeviceName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var objectNameIndex = Definition.ObjectNames.FindIndex(on => on.Name == objectNameString.Value);

                if (objectNameIndex == -1)
                    throw new FormatException(objectNameString.Value);

                if (Definition.ObjectNames[objectNameIndex].ObjectType.Halo3ODST != GameObjectTypeHalo3ODST.AlternateRealityDevice ||
                    Definition.ObjectNames[objectNameIndex].ObjectType.Halo3ODST != GameObjectTypeHalo3ODST.Control ||
                    Definition.ObjectNames[objectNameIndex].ObjectType.Halo3ODST != GameObjectTypeHalo3ODST.Machine)
                {
                    throw new FormatException(objectNameString.Value);
                }

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(objectNameString.Value);
                Array.Copy(BitConverter.GetBytes((short)objectNameIndex), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileSceneryNameExpression(ScriptString objectNameString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.SceneryName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var objectNameIndex = Definition.ObjectNames.FindIndex(on => on.Name == objectNameString.Value);

                if (objectNameIndex == -1)
                    throw new FormatException(objectNameString.Value);

                if (Definition.ObjectNames[objectNameIndex].ObjectType.Halo3ODST != GameObjectTypeHalo3ODST.Scenery)
                    throw new FormatException(objectNameString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(objectNameString.Value);
                Array.Copy(BitConverter.GetBytes((short)objectNameIndex), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileEffectSceneryNameExpression(ScriptString objectNameString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.EffectSceneryName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC);

            if (handle != DatumIndex.None)
            {
                var objectNameIndex = Definition.ObjectNames.FindIndex(on => on.Name == objectNameString.Value);

                if (objectNameIndex == -1)
                    throw new FormatException(objectNameString.Value);

                if (Definition.ObjectNames[objectNameIndex].ObjectType.Halo3ODST != GameObjectTypeHalo3ODST.EffectScenery)
                    throw new FormatException(objectNameString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(objectNameString.Value);
                Array.Copy(BitConverter.GetBytes((short)objectNameIndex), expr.Data, 2);
            }

            return handle;
        }

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