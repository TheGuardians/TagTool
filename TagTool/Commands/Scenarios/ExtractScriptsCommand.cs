using TagTool.Cache;
using TagTool.Common;
using TagTool.Scripting;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Commands.Scenarios
{
    class ExtractScriptsCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private Scenario Definition { get; }

        public ExtractScriptsCommand(HaloOnlineCacheContext cacheContext, CachedTagInstance tag, Scenario definition)
            : base(true,

                  "ExtractScripts",
                  "Extracts all scripts in the current scenario tag to a file.",

                  "ExtractScripts <Output File>",

                  "Extracts all scripts in the current scenario tag to a file.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        private string ReadScriptString(BinaryReader reader, long address)
        {
            var result = "";

            reader.BaseStream.Position = address;
            for (char c; (c = reader.ReadChar()) != 0x00; result += c) ;

            return result;
        }

        private string OpcodeLookup(ushort Opcode)
        {
            string result = "unk_op";

            if (ScriptInfo.Scripts[CacheVersion.Halo3ODST].ContainsKey(Opcode))
                result = ScriptInfo.Scripts[CacheVersion.Halo3ODST][Opcode].Name;

            return result;
        }

        private void WriteValueExpression(HsSyntaxNode expr, BinaryReader stringReader, StreamWriter scriptWriter)
        {
            var valueType = (HsType.Halo3ODSTValue)Enum.Parse(typeof(HsType.Halo3ODSTValue), expr.ValueType.HaloOnline.ToString());

            switch (valueType)
            {
                case HsType.Halo3ODSTValue.FunctionName:
                    scriptWriter.Write(expr.StringAddress == 0 ? OpcodeLookup(expr.Opcode) : ReadScriptString(stringReader, expr.StringAddress)); 
                    break; //Trust the string table, its faster than going through the dictionary with OpcodeLookup.

                case HsType.Halo3ODSTValue.Boolean:
                    scriptWriter.Write(expr.Data[0] == 0 ? "false" : "true");
                    break;

                case HsType.Halo3ODSTValue.Real:
                    scriptWriter.Write(BitConverter.ToSingle(new[] { expr.Data[0], expr.Data[1], expr.Data[2], expr.Data[3] }, 0));
                    break;

                case HsType.Halo3ODSTValue.Short:
                    scriptWriter.Write(BitConverter.ToInt16(new[] { expr.Data[0], expr.Data[1], }, 0));
                    break;

                case HsType.Halo3ODSTValue.Long:
                    scriptWriter.Write(BitConverter.ToInt32(new[] { expr.Data[0], expr.Data[1], expr.Data[2], expr.Data[3] }, 0));
                    break;

                case HsType.Halo3ODSTValue.String:
                    scriptWriter.Write(expr.StringAddress == 0 ? "none" : $"\"{ReadScriptString(stringReader, expr.StringAddress)}\"");
                    break;

                case HsType.Halo3ODSTValue.Script:
                    scriptWriter.Write(Definition.Scripts[BitConverter.ToInt16(new[] { expr.Data[0], expr.Data[1] }, 0)].ScriptName);
                    break;

                case HsType.Halo3ODSTValue.StringId:
                    scriptWriter.Write(CacheContext.GetString(new StringId(BitConverter.ToUInt32(new[] { expr.Data[0], expr.Data[1], expr.Data[2], expr.Data[3] }, 0))));
                    break;

                case HsType.Halo3ODSTValue.GameDifficulty:
                    switch (BitConverter.ToInt16(new[] { expr.Data[0], expr.Data[1] }, 0))
                    {
                        case 0: scriptWriter.Write("easy"); break;
                        case 1: scriptWriter.Write("normal"); break;
                        case 2: scriptWriter.Write("heroic"); break;
                        case 3: scriptWriter.Write("legendary"); break;
                        default: throw new NotImplementedException();
                    }
                    break;

                case HsType.Halo3ODSTValue.Object:
                case HsType.Halo3ODSTValue.Device:
                case HsType.Halo3ODSTValue.CutsceneCameraPoint:
                case HsType.Halo3ODSTValue.TriggerVolume:
                case HsType.Halo3ODSTValue.UnitSeatMapping:
                case HsType.Halo3ODSTValue.Vehicle:
                case HsType.Halo3ODSTValue.VehicleName:
                    scriptWriter.Write(expr.StringAddress == 0 ? "none" : $"\"{ReadScriptString(stringReader, expr.StringAddress)}\"");
                    break;

                default:
                    scriptWriter.Write($"<UNIMPLEMENTED VALUE: {expr.Flags.ToString()} {expr.ValueType.ToString()}>");
                    break;
            }
        }

        private void WriteGroupExpression(HsSyntaxNode expr, BinaryReader stringReader, StreamWriter scriptWriter)
        {
            scriptWriter.Write('(');

            for (var exprIndex = Definition.ScriptExpressions.IndexOf(expr) + 1; Definition.ScriptExpressions[exprIndex].ValueType.HaloOnline != HsType.HaloOnlineValue.Invalid; exprIndex = Definition.ScriptExpressions[exprIndex].NextExpressionHandle.Index)
            {
                WriteExpression(Definition.ScriptExpressions[exprIndex], stringReader, scriptWriter);

                if (Definition.ScriptExpressions[exprIndex].NextExpressionHandle.Index == ushort.MaxValue)
                    break;

                scriptWriter.Write(' ');
            }

            scriptWriter.Write(')');
        }

        private void WriteExpression(HsSyntaxNode expr, BinaryReader stringReader, StreamWriter scriptWriter)
        {
            switch (expr.Flags)
            {
                case HsSyntaxNodeFlags.Group:
                    WriteGroupExpression(expr, stringReader, scriptWriter);
                    break;

                case HsSyntaxNodeFlags.Expression:
                    WriteValueExpression(expr, stringReader, scriptWriter);
                    break;

                case HsSyntaxNodeFlags.GlobalsReference:
                case HsSyntaxNodeFlags.ParameterReference:
                    scriptWriter.Write(expr.StringAddress == 0 ? "none" : ReadScriptString(stringReader, expr.StringAddress));
                    break;

                default:
                    scriptWriter.Write($"<UNIMPLEMENTED EXPR: {expr.Flags.ToString()} {expr.ValueType.ToString()}>");
                    break;
            }
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var scriptFile = new FileInfo(args[0]);

            using (var scriptFileStream = scriptFile.Create())
            using (var scriptWriter = new StreamWriter(scriptFileStream))
            using (var scriptStringStream = new MemoryStream(Definition.ScriptStrings))
            using (var scriptStringReader = new BinaryReader(scriptStringStream))
            {
                //
                // Export scenario script globals
                //

                foreach (var scriptGlobal in Definition.Globals)
                {
                    scriptWriter.Write($"(global {scriptGlobal.Type.ToString().ToSnakeCase()} {scriptGlobal.Name} ");

                    WriteExpression(Definition.ScriptExpressions[scriptGlobal.InitializationExpressionHandle.Index], scriptStringReader, scriptWriter);

                    scriptWriter.WriteLine(')');
                }

                scriptWriter.WriteLine();

                //
                // Export scenario scripts
                //

                foreach (var script in Definition.Scripts)
                {
                    scriptWriter.Write($"(script {script.Type.ToString().ToSnakeCase()} {script.ReturnType.HaloOnline.ToString().ToSnakeCase()} ");

                    if (script.Parameters.Count == 0)
                    {
                        scriptWriter.WriteLine(script.ScriptName + ' ');
                    }
                    else
                    {
                        scriptWriter.Write($"({script.ScriptName}");

                        foreach (var parameter in script.Parameters)
                        {
                            scriptWriter.Write($" ({parameter.Type.ToString().ToSnakeCase()} {parameter.Name})");
                        }

                        scriptWriter.WriteLine(')');
                    }

                    WriteExpression(Definition.ScriptExpressions[script.RootExpressionHandle.Index], scriptStringReader, scriptWriter);

                    scriptWriter.WriteLine(')');

                    scriptWriter.WriteLine();
                }
            }

            return true;
        }
    }
}