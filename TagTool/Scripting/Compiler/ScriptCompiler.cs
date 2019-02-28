using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Scripting.Compiler;
using TagTool.Tags.Definitions;

namespace TagTool.Scripting.Compiler
{
    public class ScriptCompiler
    {
        public Scenario Definition { get; }

        public ScriptCompiler(Scenario definition)
        {
            Definition = definition;

            Definition.ScriptStrings = new byte[0];

            Definition.Globals.Clear();
            Definition.Scripts.Clear();
            Definition.ScriptExpressions.Clear();
            Definition.ScriptSourceFileReferences.Clear();
            Definition.ScriptExternalFileReferences.Clear();
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
                CompileToplevel(node);
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

            Definition.Globals.Add(new ScriptGlobal
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

            Definition.Scripts.Add(new Script
            {
                ScriptName = scriptName,
                Type = scriptType,
                ReturnType = scriptReturnType,
                RootExpressionHandle = scriptInit,
                Parameters = scriptParams
            });
        }

        private uint CompileExpression(ScriptValueType type, IScriptSyntax node)
        {
            if (node is ScriptGroup group)
                return CompileGroupExpression(type, group);

            switch (type.Halo3ODST)
            {
                case ScriptValueType.Halo3ODSTValue.Boolean:
                    if (node is ScriptBoolean boolValue)
                        return CompileBooleanExpression(boolValue);
                    else
                        throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Real:
                    if (node is ScriptReal realValue)
                        return CompileRealExpression(realValue);
                    else
                        throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Short:
                    if (node is ScriptInteger shortValue)
                        return CompileShortExpression(shortValue);
                    else
                        throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Long:
                    if (node is ScriptInteger longValue)
                        return CompileLongExpression(longValue);
                    else
                        throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.String:
                    if (node is ScriptString stringValue)
                        return CompileStringExpression(stringValue);
                    else
                        throw new FormatException(node.ToString());

                case ScriptValueType.Halo3ODSTValue.Script:
                    break;

                case ScriptValueType.Halo3ODSTValue.StringId:
                    break;

                case ScriptValueType.Halo3ODSTValue.UnitSeatMapping:
                    break;

                case ScriptValueType.Halo3ODSTValue.TriggerVolume:
                    break;

                case ScriptValueType.Halo3ODSTValue.CutsceneFlag:
                    break;

                case ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint:
                    break;

                case ScriptValueType.Halo3ODSTValue.CutsceneTitle:
                    break;

                case ScriptValueType.Halo3ODSTValue.CutsceneRecording:
                    break;

                case ScriptValueType.Halo3ODSTValue.DeviceGroup:
                    break;

                case ScriptValueType.Halo3ODSTValue.Ai:
                    break;

                case ScriptValueType.Halo3ODSTValue.AiCommandList:
                    break;

                case ScriptValueType.Halo3ODSTValue.AiCommandScript:
                    break;

                case ScriptValueType.Halo3ODSTValue.AiBehavior:
                    break;

                case ScriptValueType.Halo3ODSTValue.AiOrders:
                    break;

                case ScriptValueType.Halo3ODSTValue.AiLine:
                    break;

                case ScriptValueType.Halo3ODSTValue.StartingProfile:
                    break;

                case ScriptValueType.Halo3ODSTValue.Conversation:
                    break;

                case ScriptValueType.Halo3ODSTValue.ZoneSet:
                    break;

                case ScriptValueType.Halo3ODSTValue.DesignerZone:
                    break;

                case ScriptValueType.Halo3ODSTValue.PointReference:
                    break;

                case ScriptValueType.Halo3ODSTValue.Style:
                    break;

                case ScriptValueType.Halo3ODSTValue.ObjectList:
                    break;

                case ScriptValueType.Halo3ODSTValue.Folder:
                    break;

                case ScriptValueType.Halo3ODSTValue.Sound:
                    break;

                case ScriptValueType.Halo3ODSTValue.Effect:
                    break;

                case ScriptValueType.Halo3ODSTValue.Damage:
                    break;

                case ScriptValueType.Halo3ODSTValue.LoopingSound:
                    break;

                case ScriptValueType.Halo3ODSTValue.AnimationGraph:
                    break;

                case ScriptValueType.Halo3ODSTValue.DamageEffect:
                    break;

                case ScriptValueType.Halo3ODSTValue.ObjectDefinition:
                    break;

                case ScriptValueType.Halo3ODSTValue.Bitmap:
                    break;

                case ScriptValueType.Halo3ODSTValue.Shader:
                    break;

                case ScriptValueType.Halo3ODSTValue.RenderModel:
                    break;

                case ScriptValueType.Halo3ODSTValue.StructureDefinition:
                    break;

                case ScriptValueType.Halo3ODSTValue.LightmapDefinition:
                    break;

                case ScriptValueType.Halo3ODSTValue.CinematicDefinition:
                    break;

                case ScriptValueType.Halo3ODSTValue.CinematicSceneDefinition:
                    break;

                case ScriptValueType.Halo3ODSTValue.BinkDefinition:
                    break;

                case ScriptValueType.Halo3ODSTValue.AnyTag:
                    break;

                case ScriptValueType.Halo3ODSTValue.AnyTagNotResolving:
                    break;

                case ScriptValueType.Halo3ODSTValue.GameDifficulty:
                    break;

                case ScriptValueType.Halo3ODSTValue.Team:
                    break;

                case ScriptValueType.Halo3ODSTValue.MpTeam:
                    break;

                case ScriptValueType.Halo3ODSTValue.Controller:
                    break;

                case ScriptValueType.Halo3ODSTValue.ButtonPreset:
                    break;

                case ScriptValueType.Halo3ODSTValue.JoystickPreset:
                    break;

                case ScriptValueType.Halo3ODSTValue.PlayerColor:
                    break;

                case ScriptValueType.Halo3ODSTValue.PlayerCharacterType:
                    break;

                case ScriptValueType.Halo3ODSTValue.VoiceOutputSetting:
                    break;

                case ScriptValueType.Halo3ODSTValue.VoiceMask:
                    break;

                case ScriptValueType.Halo3ODSTValue.SubtitleSetting:
                    break;

                case ScriptValueType.Halo3ODSTValue.ActorType:
                    break;

                case ScriptValueType.Halo3ODSTValue.ModelState:
                    break;

                case ScriptValueType.Halo3ODSTValue.Event:
                    break;

                case ScriptValueType.Halo3ODSTValue.CharacterPhysics:
                    break;

                case ScriptValueType.Halo3ODSTValue.PrimarySkull:
                    break;

                case ScriptValueType.Halo3ODSTValue.SecondarySkull:
                    break;

                case ScriptValueType.Halo3ODSTValue.Object:
                    break;

                case ScriptValueType.Halo3ODSTValue.Unit:
                    break;

                case ScriptValueType.Halo3ODSTValue.Vehicle:
                    break;

                case ScriptValueType.Halo3ODSTValue.Weapon:
                    break;

                case ScriptValueType.Halo3ODSTValue.Device:
                    break;

                case ScriptValueType.Halo3ODSTValue.Scenery:
                    break;

                case ScriptValueType.Halo3ODSTValue.EffectScenery:
                    break;

                case ScriptValueType.Halo3ODSTValue.ObjectName:
                    break;

                case ScriptValueType.Halo3ODSTValue.UnitName:
                    break;

                case ScriptValueType.Halo3ODSTValue.VehicleName:
                    break;

                case ScriptValueType.Halo3ODSTValue.WeaponName:
                    break;

                case ScriptValueType.Halo3ODSTValue.DeviceName:
                    break;

                case ScriptValueType.Halo3ODSTValue.SceneryName:
                    break;

                case ScriptValueType.Halo3ODSTValue.EffectSceneryName:
                    break;

                case ScriptValueType.Halo3ODSTValue.CinematicLightprobe:
                    break;

                case ScriptValueType.Halo3ODSTValue.AnimationBudgetReference:
                    break;

                case ScriptValueType.Halo3ODSTValue.LoopingSoundBudgetReference:
                    break;

                case ScriptValueType.Halo3ODSTValue.SoundBudgetReference:
                    break;
            }

            throw new NotImplementedException(type.ToString());
        }

        private uint CompileGroupExpression(ScriptValueType type, ScriptGroup group)
        {
            throw new NotImplementedException();
        }

        private uint CompileBooleanExpression(ScriptBoolean boolean)
        {
            throw new NotImplementedException();
        }

        private uint CompileRealExpression(ScriptReal real)
        {
            throw new NotImplementedException();
        }

        private uint CompileShortExpression(ScriptInteger integer)
        {
            throw new NotImplementedException();
        }

        private uint CompileLongExpression(ScriptInteger integer)
        {
            throw new NotImplementedException();
        }

        private uint CompileStringExpression(ScriptString stringValue)
        {
            throw new NotImplementedException();
        }
    }
}
