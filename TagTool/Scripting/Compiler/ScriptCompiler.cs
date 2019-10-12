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

        private ushort NextIdentifier = 0xE37F;
        private HsScript CurrentScript = null;

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

            Definition.Scripts = Scripts;
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

            if (!(node is ScriptGroup group))
                throw new FormatException(node.ToString());
            
            if (!(group.Head is ScriptSymbol symbol && symbol.Value == "script"))
                throw new FormatException(node.ToString());

            if (!(group.Tail is ScriptGroup declGroup))
                throw new FormatException(node.ToString());

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

            if (scriptReturnType.Halo3ODST == HsType.Halo3ODSTValue.Invalid)
                throw new FormatException(declTailGroup.Head.ToString());

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

                        if (!(tailGroup is ScriptGroup declParamGroup))
                            throw new FormatException(tailGroup.ToString());

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
            // Add an entry to the scripts block in the scenario definition
            //

            var script = new HsScript
            {
                ScriptName = scriptName,
                Type = scriptType,
                ReturnType = scriptReturnType,
                RootExpressionHandle = DatumIndex.None,
                Parameters = scriptParams
            };

            Scripts.Add(script);

            //
            // Compile the script expressions
            //

            CurrentScript = script;

            script.RootExpressionHandle = CompileExpression(
                scriptReturnType.Halo3ODST,
                new ScriptGroup
                {
                    Head = new ScriptSymbol { Value = "begin" },
                    Tail = declTailTailGroup.Tail
                });

            CurrentScript = null;
        }

        private DatumIndex CompileExpression(HsType.Halo3ODSTValue type, IScriptSyntax node)
        {
            if (node is ScriptGroup group)
                return CompileGroupExpression(type, group);

            if (node is ScriptSymbol symbol)
            {
                //
                // Check if the symbol is a reference to a parameter
                //

                if (CurrentScript != null)
                    foreach (var parameter in CurrentScript?.Parameters)
                        if (parameter.Name == symbol.Value)
                            return CompileParameterReference(symbol, parameter);

                //
                // Check if the symbol is a reference to a global
                //

                foreach (var global in Globals)
                    if (global.Name == symbol.Value)
                        return CompileGlobalReference(symbol, global);

                foreach (var global in ScriptInfo.Globals[CacheContext.Version])
                    if (global.Value == symbol.Value)
                        return CompileGlobalReference(symbol, type, global.Value, (ushort)(global.Key | 0x8000));
            }

            switch (type)
            {
                case HsType.Halo3ODSTValue.Boolean:
                    if (node is ScriptBoolean boolValue)
                        return CompileBooleanExpression(boolValue);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.Real:
                    if (node is ScriptReal realValue)
                        return CompileRealExpression(realValue);
                    else if (node is ScriptInteger realIntegerValue)
                        return CompileRealExpression(new ScriptReal
                        {
                            Value = (double)realIntegerValue.Value,
                            Line = realIntegerValue.Line
                        });
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
                    if (node is ScriptSymbol unitSeatMappingSymbol && unitSeatMappingSymbol.Value == "none")
                        return CompileUnitSeatMappingExpression(new ScriptString { Value = "none" });
                    else if (node is ScriptString unitSeatMappingString)
                        return CompileUnitSeatMappingExpression(unitSeatMappingString);
                    else throw new FormatException(node.ToString());

                case HsType.Halo3ODSTValue.TriggerVolume:
                    if (node is ScriptString triggerVolumeString)
                        return CompileTriggerVolumeExpression(triggerVolumeString);
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

        private DatumIndex AllocateExpression(HsType.Halo3ODSTValue valueType, HsSyntaxNodeFlags expressionType, ushort? opcode = null, short? line = null)
        {
            ushort identifier = NextIdentifier;

            if (identifier == ushort.MaxValue)
                identifier = 0xE37F;

            NextIdentifier = (ushort)(identifier + 1);

            uint stringAddress = 0; // TODO?

            var expression = new HsSyntaxNode
            {
                Identifier = identifier,
                Opcode = opcode ?? (ushort)valueType,
                ValueType = new HsType { Halo3ODST = valueType },
                Flags = expressionType,
                NextExpressionHandle = DatumIndex.None,
                StringAddress = stringAddress,
                Data = BitConverter.GetBytes(-1),
                LineNumber = line.HasValue ? line.Value : (short)-1
            };

            if (!Enum.TryParse(valueType.ToString(), out expression.ValueType.Halo3Retail))
                expression.ValueType.Halo3Retail = HsType.Halo3RetailValue.Invalid;

            if (!Enum.TryParse(valueType.ToString(), out expression.ValueType.HaloOnline))
                expression.ValueType.HaloOnline = HsType.HaloOnlineValue.Invalid;

            ScriptExpressions.Add(expression);

            return new DatumIndex(identifier, (ushort)ScriptExpressions.IndexOf(expression));
        }

        private DatumIndex CompileGroupExpression(HsType.Halo3ODSTValue type, ScriptGroup group)
        {
            if (!(group.Head is ScriptSymbol functionNameSymbol))
                throw new FormatException(group.Head.ToString());

            if (!(group.Tail is ScriptGroup) && !(group.Tail is ScriptInvalid))
                throw new FormatException(group.Tail.ToString());

            //
            // Handle special builtin functions
            //

            switch (functionNameSymbol.Value)
            {
                case "begin":
                case "begin_random":
                    {
                        var builtin = ScriptInfo.Scripts[CacheContext.Version].First(x => x.Value.Name == functionNameSymbol.Value);

                        var firstHandle = DatumIndex.None;
                        HsSyntaxNode prevExpr = null;

                        for (IScriptSyntax current = group.Tail;
                            current is ScriptGroup currentGroup;
                            current = currentGroup.Tail)
                        {
                            var currentHandle = CompileExpression(HsType.Halo3ODSTValue.Unparsed, currentGroup.Head);

                            if (firstHandle == DatumIndex.None)
                                firstHandle = currentHandle;

                            if (prevExpr != null)
                                prevExpr.NextExpressionHandle = currentHandle;

                            prevExpr = ScriptExpressions[currentHandle.Index];
                        }

                        //
                        // Allocate the function name expression
                        //

                        var beginHandle = AllocateExpression(type, HsSyntaxNodeFlags.Group, (ushort)builtin.Key, (short)group.Line);
                        var beginExpr = ScriptExpressions[beginHandle.Index];

                        var functionNameHandle = AllocateExpression(HsType.Halo3ODSTValue.FunctionName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, (ushort)builtin.Key, (short)functionNameSymbol.Line);
                        var functionNameExpr = ScriptExpressions[functionNameHandle.Index];
                        functionNameExpr.NextExpressionHandle = firstHandle;
                        functionNameExpr.StringAddress = CompileStringAddress(functionNameSymbol.Value);

                        Array.Copy(BitConverter.GetBytes(functionNameHandle.Value), beginExpr.Data, 4);
                        Array.Copy(BitConverter.GetBytes(0), functionNameExpr.Data, 4);

                        return beginHandle;
                    }

                case "if":
                    {
                        var builtin = ScriptInfo.Scripts[CacheContext.Version].First(x => x.Value.Name == functionNameSymbol.Value);

                        var ifHandle = AllocateExpression(type, HsSyntaxNodeFlags.Group, (ushort)builtin.Key, (short)group.Line);
                        var ifExpr = ScriptExpressions[ifHandle.Index];

                        var functionNameHandle = AllocateExpression(HsType.Halo3ODSTValue.FunctionName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, (ushort)builtin.Key, (short)functionNameSymbol.Line);
                        var functionNameExpr = ScriptExpressions[functionNameHandle.Index];
                        functionNameExpr.StringAddress = CompileStringAddress(functionNameSymbol.Value);

                        Array.Copy(BitConverter.GetBytes(functionNameHandle.Value), ifExpr.Data, 4);
                        Array.Copy(BitConverter.GetBytes(0), functionNameExpr.Data, 4);

                        if (!(group.Tail is ScriptGroup booleanGroup))
                            throw new FormatException(group.ToString());

                        var booleanHandle = CompileExpression(HsType.Halo3ODSTValue.Unparsed, booleanGroup.Head);
                        var booleanExpr = ScriptExpressions[booleanHandle.Index];

                        functionNameExpr.NextExpressionHandle = booleanHandle;

                        if (!(booleanGroup.Tail is ScriptGroup thenGroup))
                            throw new FormatException(group.ToString());

                        var thenHandle = CompileExpression(type, thenGroup.Head);
                        var thenExpr = ScriptExpressions[thenHandle.Index];

                        booleanExpr.NextExpressionHandle = thenHandle;

                        if (thenGroup.Tail is ScriptGroup elseGroup)
                        {
                            if (!(elseGroup.Tail is ScriptInvalid))
                                throw new FormatException(group.ToString());

                            thenExpr.NextExpressionHandle = CompileExpression(type, elseGroup.Head);
                        }
                        else if (!(thenGroup.Tail is ScriptInvalid))
                        {
                            throw new FormatException(group.ToString());
                        }

                        return ifHandle;
                    }

                case "cond":
                    {
                        var builtin = ScriptInfo.Scripts[CacheContext.Version].First(x => x.Value.Name == functionNameSymbol.Value);

                        var condHandle = AllocateExpression(type, HsSyntaxNodeFlags.Group, (ushort)builtin.Key, (short)group.Line);
                        var condExpr = ScriptExpressions[condHandle.Index];

                        var functionNameHandle = AllocateExpression(HsType.Halo3ODSTValue.FunctionName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, (ushort)builtin.Key, (short)functionNameSymbol.Line);
                        var functionNameExpr = ScriptExpressions[functionNameHandle.Index];
                        functionNameExpr.StringAddress = CompileStringAddress(functionNameSymbol.Value);

                        Array.Copy(BitConverter.GetBytes(functionNameHandle.Value), condExpr.Data, 4);
                        Array.Copy(BitConverter.GetBytes(0), functionNameExpr.Data, 4);

                        var current = group.Tail;

                        if (!(current is ScriptGroup) && !(current is ScriptInvalid))
                            throw new FormatException(group.ToString());

                        var prevExpr = functionNameExpr;

                        while (current is ScriptGroup currentGroup)
                        {
                            if (!(currentGroup.Head is ScriptGroup condGroup))
                                throw new FormatException(group.ToString());

                            if (!(condGroup.Tail is ScriptGroup thenGroup))
                                throw new FormatException(group.ToString());

                            var booleanGroupHandle = AllocateExpression(type, HsSyntaxNodeFlags.Group, line: (short)condGroup.Line);
                            var booleanGroupExpr = ScriptExpressions[booleanGroupHandle.Index];

                            var booleanHandle = CompileExpression(HsType.Halo3ODSTValue.Boolean, condGroup.Head);
                            var booleanExpr = ScriptExpressions[booleanHandle.Index];
                            booleanExpr.NextExpressionHandle = CompileExpression(type,
                                new ScriptGroup
                                {
                                    Head = new ScriptSymbol { Value = "begin" },
                                    Tail = thenGroup
                                });

                            Array.Copy(BitConverter.GetBytes(booleanHandle.Value), booleanGroupExpr.Data, 4);

                            prevExpr.NextExpressionHandle = booleanGroupHandle;
                            prevExpr = booleanGroupExpr;

                            current = currentGroup.Tail;
                        }

                        return condHandle;
                    }

                case "set":
                    {
                        if (!(group.Tail is ScriptGroup setGroup))
                            throw new FormatException(group.ToString());

                        if (!(setGroup.Head is ScriptSymbol globalName))
                            throw new FormatException(group.ToString());

                        if (!(setGroup.Tail is ScriptGroup setValueGroup) || !(setValueGroup.Tail is ScriptInvalid))
                            throw new FormatException(group.ToString());

                        foreach (var global in Globals)
                        {
                            if (global.Name != globalName.Value)
                                continue;

                            var builtin = ScriptInfo.Scripts[CacheContext.Version].First(x => x.Value.Name == functionNameSymbol.Value);

                            var setHandle = AllocateExpression(global.Type.Halo3ODST, HsSyntaxNodeFlags.Group, (ushort)builtin.Key, (short)group.Line);
                            var setExpr = ScriptExpressions[setHandle.Index];

                            var functionNameHandle = AllocateExpression(HsType.Halo3ODSTValue.FunctionName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, (ushort)builtin.Key, (short)functionNameSymbol.Line);
                            var functionNameExpr = ScriptExpressions[functionNameHandle.Index];
                            functionNameExpr.StringAddress = CompileStringAddress(functionNameSymbol.Value);

                            Array.Copy(BitConverter.GetBytes(functionNameHandle.Value), setExpr.Data, 4);
                            Array.Copy(BitConverter.GetBytes(0), functionNameExpr.Data, 4);

                            var globalHandle = CompileGlobalReference(globalName, global);
                            functionNameExpr.NextExpressionHandle = globalHandle;

                            var globalExpr = ScriptExpressions[globalHandle.Index];
                            globalExpr.NextExpressionHandle = CompileExpression(global.Type.Halo3ODST, setValueGroup.Head);

                            return setHandle;
                        }

                        throw new KeyNotFoundException(globalName.Value);
                    }

                case "and":
                case "or":
                    {
                        var builtin = ScriptInfo.Scripts[CacheContext.Version].First(x => x.Value.Name == functionNameSymbol.Value);

                        var handle = AllocateExpression(builtin.Value.Type, HsSyntaxNodeFlags.Group, (ushort)builtin.Key, (short)group.Line);
                        var expr = ScriptExpressions[handle.Index];

                        var functionNameHandle = AllocateExpression(HsType.Halo3ODSTValue.FunctionName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, (ushort)builtin.Key, (short)functionNameSymbol.Line);
                        var functionNameExpr = ScriptExpressions[functionNameHandle.Index];
                        functionNameExpr.StringAddress = CompileStringAddress(functionNameSymbol.Value);

                        Array.Copy(BitConverter.GetBytes(functionNameHandle.Value), expr.Data, 4);
                        Array.Copy(BitConverter.GetBytes(0), functionNameExpr.Data, 4);

                        var prevExpr = functionNameExpr;

                        for (IScriptSyntax current = group.Tail;
                            current is ScriptGroup currentGroup;
                            current = currentGroup.Tail)
                        {
                            if (!(currentGroup.Tail is ScriptGroup) && !(currentGroup.Tail is ScriptInvalid))
                                throw new FormatException(group.ToString());

                            var currentHandle = CompileExpression(HsType.Halo3ODSTValue.Boolean, currentGroup.Head);

                            prevExpr.NextExpressionHandle = currentHandle;
                            prevExpr = ScriptExpressions[currentHandle.Index];
                        }

                        return handle;
                    }

                case "+":
                case "-":
                case "*":
                case "/":
                case "%":
                case "min":
                case "max":
                    {
                        var builtin = ScriptInfo.Scripts[CacheContext.Version].First(x => x.Value.Name == functionNameSymbol.Value);

                        var handle = AllocateExpression(builtin.Value.Type, HsSyntaxNodeFlags.Group, (ushort)builtin.Key, (short)group.Line);
                        var expr = ScriptExpressions[handle.Index];

                        var functionNameHandle = AllocateExpression(HsType.Halo3ODSTValue.FunctionName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, (ushort)builtin.Key, (short)functionNameSymbol.Line);
                        var functionNameExpr = ScriptExpressions[functionNameHandle.Index];
                        functionNameExpr.StringAddress = CompileStringAddress(functionNameSymbol.Value);

                        Array.Copy(BitConverter.GetBytes(functionNameHandle.Value), expr.Data, 4);
                        Array.Copy(BitConverter.GetBytes(0), functionNameExpr.Data, 4);

                        var prevExpr = functionNameExpr;

                        for (IScriptSyntax current = group.Tail;
                            current is ScriptGroup currentGroup;
                            current = currentGroup.Tail)
                        {
                            if (!(currentGroup.Tail is ScriptGroup) && !(currentGroup.Tail is ScriptInvalid))
                                throw new FormatException(group.ToString());

                            var currentHandle = DatumIndex.None;

                            switch (currentGroup.Head)
                            {
                                case ScriptInteger _:
                                case ScriptReal _:
                                    currentHandle = CompileExpression(HsType.Halo3ODSTValue.Real, currentGroup.Head);
                                    break;

                                default:
                                    currentHandle = CompileExpression(HsType.Halo3ODSTValue.Unparsed, currentGroup.Head);
                                    break;
                            }

                            prevExpr.NextExpressionHandle = currentHandle;
                            prevExpr = ScriptExpressions[currentHandle.Index];
                        }

                        return handle;
                    }

                case "=":
                case "!=":
                case "<":
                case ">":
                case "<=":
                case ">=":
                    {
                        var builtin = ScriptInfo.Scripts[CacheContext.Version].First(x => x.Value.Name == functionNameSymbol.Value);

                        var handle = AllocateExpression(builtin.Value.Type, HsSyntaxNodeFlags.Group, (ushort)builtin.Key, (short)group.Line);
                        var expr = ScriptExpressions[handle.Index];

                        var functionNameHandle = AllocateExpression(HsType.Halo3ODSTValue.FunctionName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, (ushort)builtin.Key, (short)functionNameSymbol.Line);
                        var functionNameExpr = ScriptExpressions[functionNameHandle.Index];
                        functionNameExpr.StringAddress = CompileStringAddress(functionNameSymbol.Value);

                        Array.Copy(BitConverter.GetBytes(functionNameHandle.Value), expr.Data, 4);
                        Array.Copy(BitConverter.GetBytes(0), functionNameExpr.Data, 4);

                        if (!(group.Tail is ScriptGroup tailGroup))
                            throw new FormatException(group.ToString());

                        switch (tailGroup.Head)
                        {
                            case ScriptInteger _:
                            case ScriptReal _:
                                functionNameExpr.NextExpressionHandle = (type == HsType.Halo3ODSTValue.Unparsed) ?
                                    CompileExpression(HsType.Halo3ODSTValue.Real, tailGroup.Head) :
                                    CompileExpression(type, tailGroup.Head);
                                break;

                            default:
                                functionNameExpr.NextExpressionHandle = CompileExpression(HsType.Halo3ODSTValue.Unparsed, tailGroup.Head);
                                break;
                        }

                        var firstExpr = ScriptExpressions[functionNameExpr.NextExpressionHandle.Index];

                        if (!(tailGroup.Tail is ScriptGroup tailTailGroup) || !(tailTailGroup.Tail is ScriptInvalid))
                            throw new FormatException(group.ToString());

                        firstExpr.NextExpressionHandle = (type == HsType.Halo3ODSTValue.Unparsed) ?
                            CompileExpression(firstExpr.ValueType.Halo3ODST, tailTailGroup.Head) :
                            CompileExpression(type, tailTailGroup.Head);

                        return handle;
                    }

                case "sleep":
                    {
                        var builtin = ScriptInfo.Scripts[CacheContext.Version].First(x => x.Value.Name == functionNameSymbol.Value);

                        var handle = AllocateExpression(builtin.Value.Type, HsSyntaxNodeFlags.Group, (ushort)builtin.Key, (short)group.Line);
                        var expr = ScriptExpressions[handle.Index];

                        var functionNameHandle = AllocateExpression(HsType.Halo3ODSTValue.FunctionName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, (ushort)builtin.Key, (short)functionNameSymbol.Line);
                        var functionNameExpr = ScriptExpressions[functionNameHandle.Index];
                        functionNameExpr.StringAddress = CompileStringAddress(functionNameSymbol.Value);

                        Array.Copy(BitConverter.GetBytes(functionNameHandle.Value), expr.Data, 4);
                        Array.Copy(BitConverter.GetBytes(0), functionNameExpr.Data, 4);

                        if (!(group.Tail is ScriptGroup tailGroup))
                            throw new FormatException(group.ToString());

                        switch (tailGroup.Head)
                        {
                            case ScriptInteger _:
                                functionNameExpr.NextExpressionHandle = CompileExpression(HsType.Halo3ODSTValue.Short, tailGroup.Head);
                                break;

                            default:
                                functionNameExpr.NextExpressionHandle = CompileExpression(HsType.Halo3ODSTValue.Unparsed, tailGroup.Head);
                                break;
                        }

                        if (tailGroup.Tail is ScriptInvalid)
                            return handle;

                        var lengthExpr = ScriptExpressions[functionNameExpr.NextExpressionHandle.Index];

                        if (!(tailGroup.Tail is ScriptGroup tailTailGroup) || !(tailTailGroup.Tail is ScriptInvalid))
                            throw new FormatException(group.ToString());

                        lengthExpr.NextExpressionHandle = CompileExpression(HsType.Halo3ODSTValue.Script, tailTailGroup.Head);

                        return handle;
                    }

                case "sleep_forever":
                    {
                        var builtin = ScriptInfo.Scripts[CacheContext.Version].First(x => x.Value.Name == functionNameSymbol.Value);

                        var handle = AllocateExpression(builtin.Value.Type, HsSyntaxNodeFlags.Group, (ushort)builtin.Key, (short)group.Line);
                        var expr = ScriptExpressions[handle.Index];

                        var functionNameHandle = AllocateExpression(HsType.Halo3ODSTValue.FunctionName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, (ushort)builtin.Key, (short)functionNameSymbol.Line);
                        var functionNameExpr = ScriptExpressions[functionNameHandle.Index];
                        functionNameExpr.StringAddress = CompileStringAddress(functionNameSymbol.Value);

                        Array.Copy(BitConverter.GetBytes(functionNameHandle.Value), expr.Data, 4);
                        Array.Copy(BitConverter.GetBytes(0), functionNameExpr.Data, 4);

                        if (group.Tail is ScriptInvalid)
                            return handle;

                        if (!(group.Tail is ScriptGroup tailGroup) || !(tailGroup.Tail is ScriptInvalid))
                            throw new FormatException(group.ToString());

                        functionNameExpr.NextExpressionHandle = CompileExpression(HsType.Halo3ODSTValue.Script, tailGroup.Head);

                        return handle;
                    }

                case "sleep_until":
                    {
                        var builtin = ScriptInfo.Scripts[CacheContext.Version].First(x => x.Value.Name == functionNameSymbol.Value);

                        var handle = AllocateExpression(builtin.Value.Type, HsSyntaxNodeFlags.Group, (ushort)builtin.Key, (short)group.Line);
                        var expr = ScriptExpressions[handle.Index];

                        var functionNameHandle = AllocateExpression(HsType.Halo3ODSTValue.FunctionName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, (ushort)builtin.Key, (short)functionNameSymbol.Line);
                        var functionNameExpr = ScriptExpressions[functionNameHandle.Index];
                        functionNameExpr.StringAddress = CompileStringAddress(functionNameSymbol.Value);

                        Array.Copy(BitConverter.GetBytes(functionNameHandle.Value), expr.Data, 4);
                        Array.Copy(BitConverter.GetBytes(0), functionNameExpr.Data, 4);

                        if (!(group.Tail is ScriptGroup tailGroup))
                            throw new FormatException(group.ToString());

                        functionNameExpr.NextExpressionHandle = CompileExpression(HsType.Halo3ODSTValue.Boolean, tailGroup.Head);

                        if (tailGroup.Tail is ScriptInvalid)
                            return handle;

                        var booleanExpr = ScriptExpressions[functionNameExpr.NextExpressionHandle.Index];

                        if (!(tailGroup.Tail is ScriptGroup tailTailGroup) || !(tailTailGroup.Tail is ScriptInvalid))
                            throw new FormatException(group.ToString());

                        booleanExpr.NextExpressionHandle = CompileExpression(HsType.Halo3ODSTValue.Short, tailTailGroup.Head);

                        return handle;
                    }

                case "wake":
                    {
                        var builtin = ScriptInfo.Scripts[CacheContext.Version].First(x => x.Value.Name == functionNameSymbol.Value);

                        var handle = AllocateExpression(builtin.Value.Type, HsSyntaxNodeFlags.Group, (ushort)builtin.Key, (short)group.Line);
                        var expr = ScriptExpressions[handle.Index];

                        var functionNameHandle = AllocateExpression(HsType.Halo3ODSTValue.FunctionName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, (ushort)builtin.Key, (short)functionNameSymbol.Line);
                        var functionNameExpr = ScriptExpressions[functionNameHandle.Index];
                        functionNameExpr.StringAddress = CompileStringAddress(functionNameSymbol.Value);

                        Array.Copy(BitConverter.GetBytes(functionNameHandle.Value), expr.Data, 4);
                        Array.Copy(BitConverter.GetBytes(0), functionNameExpr.Data, 4);

                        if (!(group.Tail is ScriptGroup tailGroup) || !(tailGroup.Tail is ScriptInvalid))
                            throw new FormatException(group.ToString());

                        functionNameExpr.NextExpressionHandle = CompileExpression(HsType.Halo3ODSTValue.Script, tailGroup.Head);

                        return handle;
                    }
            }

            //
            // Check if function name is a built-in function
            //

            foreach (var entry in ScriptInfo.Scripts[CacheContext.Version])
            {
                if (functionNameSymbol.Value != entry.Value.Name)
                    continue;

                var handle = AllocateExpression(entry.Value.Type, HsSyntaxNodeFlags.Group, (ushort)entry.Key, (short)functionNameSymbol.Line);
                var expr = ScriptExpressions[handle.Index];

                var functionNameHandle = AllocateExpression(HsType.Halo3ODSTValue.FunctionName, HsSyntaxNodeFlags.Expression, (ushort)entry.Key, (short)functionNameSymbol.Line);
                var functionNameExpr = ScriptExpressions[functionNameHandle.Index];
                functionNameExpr.StringAddress = CompileStringAddress(functionNameSymbol.Value);

                Array.Copy(BitConverter.GetBytes(functionNameHandle.Value), expr.Data, 4);
                Array.Copy(BitConverter.GetBytes(0), functionNameExpr.Data, 4);

                IScriptSyntax parameters = group.Tail;
                var prevExpr = functionNameExpr;

                foreach (var parameter in entry.Value.Parameters)
                {
                    if (!(parameters is ScriptGroup parametersGroup))
                        throw new FormatException(group.ToString());

                    prevExpr.NextExpressionHandle = CompileExpression(parameter.Type, parametersGroup.Head);
                    prevExpr = ScriptExpressions[prevExpr.NextExpressionHandle.Index];

                    parameters = parametersGroup.Tail;
                }

                return handle;
            }

            //
            // Check if function name is a script
            //

            foreach (var script in Scripts)
            {
                if (functionNameSymbol.Value != script.ScriptName)
                    continue;

                var handle = AllocateExpression(
                    script.ReturnType.Halo3ODST,
                    HsSyntaxNodeFlags.ScriptReference,
                    (ushort)Scripts.IndexOf(script),
                    (short)functionNameSymbol.Line);

                var expr = ScriptExpressions[handle.Index];

                var functionNameHandle = AllocateExpression(
                    HsType.Halo3ODSTValue.FunctionName,
                    HsSyntaxNodeFlags.Expression,
                    (ushort)Scripts.IndexOf(script),
                    (short)functionNameSymbol.Line);

                var functionNameExpr = ScriptExpressions[functionNameHandle.Index];
                functionNameExpr.StringAddress = CompileStringAddress(functionNameSymbol.Value);

                Array.Copy(BitConverter.GetBytes(functionNameHandle.Value), expr.Data, 4);
                Array.Copy(BitConverter.GetBytes(0), functionNameExpr.Data, 4);

                IScriptSyntax parameters = group.Tail;
                var prevExpr = functionNameExpr;

                foreach (var parameter in script.Parameters)
                {
                    if (!(parameters is ScriptGroup parametersGroup))
                        throw new FormatException(group.ToString());

                    prevExpr.NextExpressionHandle = CompileExpression(parameter.Type.Halo3ODST, parametersGroup.Head);
                    prevExpr = ScriptExpressions[prevExpr.NextExpressionHandle.Index];

                    parameters = parametersGroup.Tail;
                }

                return handle;
            }

            throw new KeyNotFoundException(functionNameSymbol.Value);
        }

        private DatumIndex CompileGlobalReference(ScriptSymbol symbol, HsGlobal global)
        {
            var handle = AllocateExpression(global.Type.Halo3ODST, HsSyntaxNodeFlags.GlobalsReference, line: (short)symbol.Line);

            var expr = ScriptExpressions[handle.Index];
            expr.StringAddress = CompileStringAddress(global.Name);
            Array.Copy(BitConverter.GetBytes(Globals.IndexOf(global)), expr.Data, 4);

            return handle;
        }

        private DatumIndex CompileGlobalReference(ScriptSymbol symbol, HsType.Halo3ODSTValue type, string name, ushort opcode)
        {
            var handle = AllocateExpression(type, HsSyntaxNodeFlags.GlobalsReference, line: (short)symbol.Line);

            var expr = ScriptExpressions[handle.Index];
            expr.StringAddress = CompileStringAddress(name);
            Array.Copy(BitConverter.GetBytes(opcode), expr.Data, 2);

            return handle;
        }

        private DatumIndex CompileParameterReference(ScriptSymbol symbol, HsScriptParameter parameter)
        {
            var handle = AllocateExpression(parameter.Type.Halo3ODST, HsSyntaxNodeFlags.ParameterReference, line: (short)symbol.Line);

            var expr = ScriptExpressions[handle.Index];
            expr.StringAddress = CompileStringAddress(parameter.Name);
            Array.Copy(BitConverter.GetBytes(CurrentScript.Parameters.IndexOf(parameter)), expr.Data, 4);

            return handle;
        }

        private DatumIndex CompileBooleanExpression(ScriptBoolean boolValue)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Boolean, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)boolValue.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Real, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)realValue.Line);

            if (handle != DatumIndex.None)
                Array.Copy(BitConverter.GetBytes((float)realValue.Value), ScriptExpressions[handle.Index].Data, 4);

            return handle;
        }

        private DatumIndex CompileShortExpression(ScriptInteger shortValue)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Short, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)shortValue.Line);

            if (handle != DatumIndex.None)
                Array.Copy(BitConverter.GetBytes((short)shortValue.Value), ScriptExpressions[handle.Index].Data, 2);

            return handle;
        }

        private DatumIndex CompileLongExpression(ScriptInteger longValue)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Long, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)longValue.Line);

            if (handle != DatumIndex.None)
                Array.Copy(BitConverter.GetBytes((int)longValue.Value), ScriptExpressions[handle.Index].Data, 4);

            return handle;
        }

        private DatumIndex CompileStringExpression(ScriptString stringValue)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.String, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)stringValue.Line);

            if (handle != DatumIndex.None)
            {
                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(stringValue.Value);
                Array.Copy(BitConverter.GetBytes(expr.StringAddress), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileScriptExpression(ScriptSymbol scriptSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Script, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)scriptSymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.StringId, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)stringIdString.Line);

            if (handle != DatumIndex.None)
            {
                var stringId = CacheContext.GetStringId(stringIdString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(stringIdString.Value);
                Array.Copy(BitConverter.GetBytes(stringId.Value), expr.Data, 4);
            }

            return handle;
        }

        private DatumIndex CompileUnitSeatMappingExpression(ScriptString unitSeatMappingString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.UnitSeatMapping, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)unitSeatMappingString.Line);

            if (handle != DatumIndex.None)
            {
                ScriptExpressions[handle.Index].StringAddress = CompileStringAddress(unitSeatMappingString.Value);

                //
                // TODO: Compile unit_seat_mapping data here
                //

                throw new NotImplementedException(HsType.Halo3ODSTValue.UnitSeatMapping.ToString());
            }

            return handle;
        }

        private DatumIndex CompileTriggerVolumeExpression(ScriptString triggerVolumeString)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.TriggerVolume, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)triggerVolumeString.Line);

            if (handle != DatumIndex.None)
            {
                var triggerVolumeIndex = Definition.TriggerVolumes.FindIndex(tv => triggerVolumeString.Value == CacheContext.GetString(tv.Name));

                if (triggerVolumeIndex == -1)
                    throw new FormatException(triggerVolumeString.Value);

                var expr = ScriptExpressions[handle.Index];
                expr.StringAddress = CompileStringAddress(triggerVolumeString.Value);
                Array.Copy(BitConverter.GetBytes((short)triggerVolumeIndex), expr.Data, 2);
            }

            return handle;
        }

        private DatumIndex CompileCutsceneFlagExpression(ScriptSymbol cutsceneFlagSymbol)
        {
            var handle = AllocateExpression(HsType.Halo3ODSTValue.CutsceneFlag, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)cutsceneFlagSymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.CutsceneCameraPoint, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)cutsceneCameraPointSymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.CutsceneTitle, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)cutsceneTitleSymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.DeviceGroup, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)deviceGroupString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Ai, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)aiString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.StartingProfile, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)startingProfileString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.ZoneSet, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)zoneSetString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.ZoneSet, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)designerZoneString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.PointReference, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)pointReferenceString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Folder, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)folderString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Sound, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)soundString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Effect, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)effectString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Damage, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)damageString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.LoopingSound, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)loopingSoundString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.AnimationGraph, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)animationGraphString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.DamageEffect, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)damageEffectString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.ObjectDefinition, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)objectDefinitionString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Bitmap, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)bitmapString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Shader, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)shaderString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.RenderModel, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)renderModelString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.StructureDefinition, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)structureDefinitionString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.CinematicDefinition, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)cinematicDefinitionString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.CinematicSceneDefinition, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)cinematicSceneDefinitionString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.BinkDefinition, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)binkDefinitionString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.AnyTag, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)anyTagString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.AnyTagNotResolving, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)anyTagNotResolvingString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.GameDifficulty, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)gameDifficultySymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Team, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)teamSymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.MpTeam, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)mpTeamSymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Controller, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)controllerSymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.ButtonPreset, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)buttonPresetSymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.JoystickPreset, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)joystickPresetSymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.PlayerCharacterType, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)playerCharacterTypeSymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.VoiceOutputSetting, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)voiceOutputSettingSymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.VoiceMask, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)voiceMaskSymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.SubtitleSetting, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)subtitleSettingSymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.ActorType, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)actorTypeSymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.ModelState, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)modelStateSymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Event, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)eventSymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.CharacterPhysics, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)characterPhysicsSymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.PrimarySkull, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)primarySkullSymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.SecondarySkull, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)secondarySkullSymbol.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Object, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)objectString.Line);

            if (handle != DatumIndex.None)
            {
                var objectIndex = objectString.Value == "none" ? -1 :
                    Definition.ObjectNames.Find(on => on.Name == objectString.Value).PlacementIndex;

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Unit, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)unitString.Line);

            if (handle != DatumIndex.None)
            {
                var unitIndex = unitString.Value == "none" ? -1 :
                    Definition.ObjectNames.Find(on => on.Name == unitString.Value).PlacementIndex;

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Vehicle, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)vehicleString.Line);

            if (handle != DatumIndex.None)
            {
                
                var vehicleIndex = vehicleString.Value == "none" ? -1 :
                    Definition.ObjectNames.Find(on => on.Name == vehicleString.Value).PlacementIndex;

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Weapon, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)weaponString.Line);

            if (handle != DatumIndex.None)
            {
                var weaponIndex = weaponString.Value == "none" ? -1 :
                    Definition.ObjectNames.Find(on => on.Name == weaponString.Value).PlacementIndex;

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Device, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)deviceString.Line);

            if (handle != DatumIndex.None)
            {
                var deviceIndex = deviceString.Value == "none" ? -1 :
                    Definition.ObjectNames.Find(on => on.Name == deviceString.Value).PlacementIndex;

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.Scenery, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)sceneryString.Line);

            if (handle != DatumIndex.None)
            {
                var sceneryIndex = sceneryString.Value == "none" ? -1 :
                    Definition.ObjectNames.Find(on => on.Name == sceneryString.Value).PlacementIndex;

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.EffectScenery, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)effectSceneryString.Line);

            if (handle != DatumIndex.None)
            {
                var effectSceneryIndex = effectSceneryString.Value == "none" ? -1 :
                    Definition.ObjectNames.Find(on => on.Name == effectSceneryString.Value).PlacementIndex;

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.ObjectName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)objectNameString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.UnitName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)objectNameString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.VehicleName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)objectNameString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.WeaponName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)objectNameString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.DeviceName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)objectNameString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.SceneryName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)objectNameString.Line);

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
            var handle = AllocateExpression(HsType.Halo3ODSTValue.EffectSceneryName, HsSyntaxNodeFlags.Primitive | HsSyntaxNodeFlags.DoNotGC, line: (short)objectNameString.Line);

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