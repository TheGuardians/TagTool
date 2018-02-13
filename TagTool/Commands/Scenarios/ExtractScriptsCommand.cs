using TagTool.Cache;
using TagTool.Commands;
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
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private Scenario Definition { get; }

        public ExtractScriptsCommand(GameCacheContext cacheContext, CachedTagInstance tag, Scenario definition)
            : base(CommandFlags.Inherit,

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

        private void WriteValueExpression(ScriptExpression expr, BinaryReader stringReader, StreamWriter scriptWriter)
        {
            var valueType = (ScriptValueType.Halo3ODSTValue)Enum.Parse(typeof(ScriptValueType.Halo3ODSTValue), expr.ValueType.HaloOnline.ToString());

            switch (valueType)
            {
                case ScriptValueType.Halo3ODSTValue.FunctionName:
                    scriptWriter.Write(expr.StringAddress == 0 ? "none" : ReadScriptString(stringReader, expr.StringAddress));
                    break;

                case ScriptValueType.Halo3ODSTValue.Boolean:
                    scriptWriter.Write(expr.Data[0] == 0 ? "false" : "true");
                    break;

                case ScriptValueType.Halo3ODSTValue.Real:
                    scriptWriter.Write(BitConverter.ToSingle(new[] { expr.Data[0], expr.Data[1], expr.Data[2], expr.Data[3] }, 0));
                    break;

                case ScriptValueType.Halo3ODSTValue.Short:
                    scriptWriter.Write(BitConverter.ToInt16(new[] { expr.Data[0], expr.Data[1], }, 0));
                    break;

                case ScriptValueType.Halo3ODSTValue.Long:
                    scriptWriter.Write(BitConverter.ToInt32(new[] { expr.Data[0], expr.Data[1], expr.Data[2], expr.Data[3] }, 0));
                    break;

                case ScriptValueType.Halo3ODSTValue.String:
                    scriptWriter.Write(expr.StringAddress == 0 ? "none" : $"\"{ReadScriptString(stringReader, expr.StringAddress)}\"");
                    break;

                case ScriptValueType.Halo3ODSTValue.Script:
                    scriptWriter.Write(Definition.Scripts[BitConverter.ToInt16(new[] { expr.Data[0], expr.Data[1] }, 0)].ScriptName);
                    break;

                case ScriptValueType.Halo3ODSTValue.StringId:
                    scriptWriter.Write(CacheContext.GetString(new StringId(BitConverter.ToUInt32(new[] { expr.Data[0], expr.Data[1], expr.Data[2], expr.Data[3] }, 0))));
                    break;

                case ScriptValueType.Halo3ODSTValue.GameDifficulty:
                    switch (BitConverter.ToInt16(new[] { expr.Data[0], expr.Data[1] }, 0))
                    {
                        case 0: scriptWriter.Write("easy"); break;
                        case 1: scriptWriter.Write("normal"); break;
                        case 2: scriptWriter.Write("heroic"); break;
                        case 3: scriptWriter.Write("legendary"); break;
                        default: throw new NotImplementedException();
                    }
                    break;

                case ScriptValueType.Halo3ODSTValue.Object:
                case ScriptValueType.Halo3ODSTValue.Device:
                case ScriptValueType.Halo3ODSTValue.CutsceneCameraPoint:
                    scriptWriter.Write(expr.StringAddress == 0 ? "none" : $"\"{ReadScriptString(stringReader, expr.StringAddress)}\"");
                    break;

                default:
                    scriptWriter.Write($"<UNIMPLEMENTED VALUE: {expr.ExpressionType.ToString()} {expr.ValueType.ToString()}>");
                    break;
            }
        }

        private void WriteGroupExpression(ScriptExpression expr, BinaryReader stringReader, StreamWriter scriptWriter)
        {
            scriptWriter.Write('(');

            for (var exprIndex = Definition.ScriptExpressions.IndexOf(expr) + 1; Definition.ScriptExpressions[exprIndex].ValueType.HaloOnline != ScriptValueType.HaloOnlineValue.Invalid; exprIndex = (ushort)(Definition.ScriptExpressions[exprIndex].NextExpressionHandle & ushort.MaxValue))
            {
                WriteExpression(Definition.ScriptExpressions[exprIndex], stringReader, scriptWriter);

                if ((Definition.ScriptExpressions[exprIndex].NextExpressionHandle & ushort.MaxValue) == ushort.MaxValue)
                    break;

                scriptWriter.Write(' ');
            }

            scriptWriter.Write(')');
        }

        private void WriteExpression(ScriptExpression expr, BinaryReader stringReader, StreamWriter scriptWriter)
        {
            switch (expr.ExpressionType)
            {
                case ScriptExpressionType.Group:
                    WriteGroupExpression(expr, stringReader, scriptWriter);
                    break;

                case ScriptExpressionType.Expression:
                    WriteValueExpression(expr, stringReader, scriptWriter);
                    break;

                case ScriptExpressionType.GlobalsReference:
                case ScriptExpressionType.ParameterReference:
                    scriptWriter.Write(expr.StringAddress == 0 ? "none" : ReadScriptString(stringReader, expr.StringAddress));
                    break;

                default:
                    scriptWriter.Write($"<UNIMPLEMENTED EXPR: {expr.ExpressionType.ToString()} {expr.ValueType.ToString()}>");
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

                    WriteExpression(Definition.ScriptExpressions[(ushort)(scriptGlobal.InitializationExpressionHandle & ushort.MaxValue)], scriptStringReader, scriptWriter);

                    scriptWriter.WriteLine(')');
                }

                scriptWriter.WriteLine();

                //
                // Export scenario scripts
                //

                foreach (var script in Definition.Scripts)
                {
                    scriptWriter.Write($"(script {script.Type.ToString().ToSnakeCase()} {script.ReturnType.ToString().ToSnakeCase()} ");

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

                    WriteExpression(Definition.ScriptExpressions[(ushort)(script.RootExpressionHandle & ushort.MaxValue)], scriptStringReader, scriptWriter);

                    scriptWriter.WriteLine(')');

                    scriptWriter.WriteLine();
                }
            }

            return true;
        }
    }
}