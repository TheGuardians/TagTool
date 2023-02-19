using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.IO;
using TagTool.Scripting;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.CodeDom.Compiler;

namespace TagTool.Commands.Scenarios
{
    class ExtractScriptsCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private Scenario Definition { get; }

        public ExtractScriptsCommand(GameCache cache, CachedTag tag, Scenario definition)
            : base(true,

                  "ExtractScripts",
                  "Extracts all scripts in the current scenario tag to a file.",

                  "ExtractScripts [Output Filename]",

                  "Extracts all scripts in the current scenario tag to a file.")
        {
            Cache = cache;
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

            if (ScriptInfo.Scripts[(Cache.Version, Cache.Platform)].ContainsKey(Opcode))
                result = ScriptInfo.Scripts[(Cache.Version, Cache.Platform)][Opcode].Name;

            return result;
        }

        private void WriteValueExpression(HsSyntaxNode parentExprk, HsSyntaxNode expr, BinaryReader stringReader, IndentedTextWriter indentWriter)
        {
            var exprIndex = (ushort)(Definition.ScriptExpressions.IndexOf(expr));
            var nextExpr = Definition.ScriptExpressions[exprIndex].NextExpressionHandle.Index;
            var nextExprValid = false;
            if (nextExpr < Definition.ScriptExpressions.Count)
                nextExprValid = GetHsTypeAsString(Cache.Version, Definition.ScriptExpressions[nextExpr].ValueType) != "Invalid";

            var valueType = GetHsTypeAsString(Cache.Version, expr.ValueType);
            switch (valueType)
            {
                case "FunctionName":
                    indentWriter.Write(expr.StringAddress == 0 ? OpcodeLookup(expr.Opcode) : ReadScriptString(stringReader, expr.StringAddress));
                    if (((parentExprk.Opcode <= 1) && parentExprk.Flags != HsSyntaxNodeFlags.ScriptReference && parentExprk.Flags != HsSyntaxNodeFlags.ScriptIndex) && nextExprValid)
                        indentWriter.WriteLine();
                    break; //Trust the string table, its faster than going through the dictionary with OpcodeLookup.

                case "Boolean":
                    indentWriter.Write(expr.Data[0] == 0 ? "false" : "true");
                    break;

                case "Real":
                    indentWriter.Write(BitConverter.ToSingle(SortExpressionDataArray(Cache.Endianness, expr.Data, 4), 0));
                    break;

                case "Short":
                    indentWriter.Write(BitConverter.ToInt16(SortExpressionDataArray(Cache.Endianness, expr.Data, 2), 0));
                    break;

                case "Long":
                    indentWriter.Write(BitConverter.ToInt32(SortExpressionDataArray(Cache.Endianness, expr.Data, 4), 0));
                    break;
                case "String":
                    indentWriter.Write(expr.StringAddress == 0 ? "none" : $"\"{ReadScriptString(stringReader, expr.StringAddress)}\"");
                    break;

                case "Script":
                    indentWriter.Write(Definition.Scripts[BitConverter.ToInt16(SortExpressionDataArray(Cache.Endianness, expr.Data, 2), 0)].ScriptName);
                    break;

                case "StringId":
                    indentWriter.Write($"\"{Cache.StringTable.GetString(new StringId(BitConverter.ToUInt32(SortExpressionDataArray(Cache.Endianness, expr.Data, 4), 0)))}\"");
                    break;

                case "GameDifficulty":
                    switch (BitConverter.ToInt16(SortExpressionDataArray(Cache.Endianness, expr.Data, 2), 0))
                    {
                        case 0: indentWriter.Write("easy"); break;
                        case 1: indentWriter.Write("normal"); break;
                        case 2: indentWriter.Write("heroic"); break;
                        case 3: indentWriter.Write("legendary"); break;
                        default: throw new NotImplementedException();
                    }
                    break;

                case "Folder":
                case "Unit":
                case "AnimationGraph":
                case "Object":
                case "Device":
                case "CutsceneCameraPoint":
                case "CutsceneFlag":
                case "TriggerVolume":
                case "UnitSeatMapping":
                case "Vehicle":
                case "VehicleName":
                case "Effect":
                case "Sound":
                case "LoopingSound":
                case "AnyTag":
                case "ObjectName":
                case "Scenery":
                case "Ai":
                case "PointReference":
                case "ObjectDefinition":
                case "CutsceneTitle":
                case "ZoneSet":
                case "Damage":
                case "StartingProfile":
                case "DeviceGroup":
                    indentWriter.Write(expr.StringAddress == 0 ? "none" : $"\"{ReadScriptString(stringReader, expr.StringAddress)}\"");
                    break;

                case "Team":
                case "AiCommandScript":
                case "AiLine":
                    indentWriter.Write(expr.StringAddress == 0 ? "none" : $"\"{ReadScriptString(stringReader, expr.StringAddress)}\"");
                    break;

                default:
                    indentWriter.Write($"<UNIMPLEMENTED VALUE: {expr.Flags.ToString()} {valueType}>");
                    break;
            }
            
            if (nextExprValid && !((parentExprk.Opcode <= 1) && parentExprk.Flags != HsSyntaxNodeFlags.ScriptReference && parentExprk.Flags != HsSyntaxNodeFlags.ScriptIndex))
                indentWriter.Write(' ');
        }

        private void WriteGroupExpression(HsSyntaxNode parentExpr, HsSyntaxNode expr, BinaryReader stringReader, IndentedTextWriter indentWriter, bool shouldSkip)
        {
            bool shouldSkipFirst = shouldSkip;

            if (!shouldSkip)
            {
                indentWriter.Indent++;

                if ((expr.Opcode <= 1) && expr.Flags != HsSyntaxNodeFlags.ScriptReference && expr.Flags != HsSyntaxNodeFlags.ScriptIndex && parentExpr.Opcode != 2)
                    indentWriter.WriteLine();

                indentWriter.Write('(');
            }

            var prevExpr = expr;
            var nestedExpression = true;
            for (var exprIndex = (ushort)(Definition.ScriptExpressions.IndexOf(expr) + 1); GetHsTypeAsString(Cache.Version, Definition.ScriptExpressions[exprIndex].ValueType) != "Invalid"; exprIndex = Definition.ScriptExpressions[exprIndex].NextExpressionHandle.Index)
            {
                nestedExpression = false;
                if (shouldSkipFirst)
                {
                    if (Definition.ScriptExpressions[exprIndex].NextExpressionHandle.Index == ushort.MaxValue || Definition.ScriptExpressions[exprIndex].NextExpressionHandle.Index + 1 > Definition.ScriptExpressions.Count)
                        break;

                    shouldSkipFirst = false;
                    continue;
                }
                var nextExpr = Definition.ScriptExpressions[exprIndex];
                WriteExpression(expr, nextExpr, stringReader, indentWriter, false);
                prevExpr = nextExpr;

                if (Definition.ScriptExpressions[exprIndex].NextExpressionHandle.Index == ushort.MaxValue || Definition.ScriptExpressions[exprIndex].NextExpressionHandle.Index + 1 > Definition.ScriptExpressions.Count)
                    break;
            }

            if (nestedExpression)
            {
                var exprIdx = (ushort)(Definition.ScriptExpressions.IndexOf(expr) + 2);
                var expression = Definition.ScriptExpressions[exprIdx];
                var valueType = GetHsTypeAsString(Cache.Version, expression.ValueType);
                indentWriter.Write($"<ERROR DECOMPILING GROUP EXPRESSION: {expression.Flags.ToString()} {valueType}>");
            }

            if (!shouldSkip)
            {
                var exprIndex = (ushort)(Definition.ScriptExpressions.IndexOf(expr));
                var nextExpr = Definition.ScriptExpressions[exprIndex].NextExpressionHandle.Index;
                var nextExprValid = false;
                if (nextExpr < Definition.ScriptExpressions.Count)
                    nextExprValid = GetHsTypeAsString(Cache.Version, Definition.ScriptExpressions[nextExpr].ValueType) != "Invalid";

                if (((parentExpr.Opcode <= 2) && parentExpr.Flags != HsSyntaxNodeFlags.ScriptReference && parentExpr.Flags != HsSyntaxNodeFlags.ScriptIndex))
                {
                    indentWriter.WriteLine(")");
                }
                else
                {
                    HsSyntaxNode nextExprHandle = null;
                    if(nextExprValid)
                        nextExprHandle = Definition.ScriptExpressions[nextExpr];
                    
                    if (nextExprValid && !(((nextExprHandle.Opcode <= 2) && nextExprHandle.Flags != HsSyntaxNodeFlags.ScriptReference && nextExprHandle.Flags != HsSyntaxNodeFlags.ScriptIndex)))
                    {
                        if ((expr.Opcode <= 2) && expr.Flags != HsSyntaxNodeFlags.ScriptReference && expr.Flags != HsSyntaxNodeFlags.ScriptIndex)
                        {
                            indentWriter.WriteLine(")");
                        }
                        else
                            indentWriter.Write(") ");
                    }
                    else
                        indentWriter.Write(")");
                }

                indentWriter.Indent--;
            }
        }

        private void WriteExpression(HsSyntaxNode parentExpr, HsSyntaxNode expr, BinaryReader stringReader, IndentedTextWriter indentWriter, bool shouldSkip)
        {
            var exprIndex = (ushort)(Definition.ScriptExpressions.IndexOf(expr));
            var nextExpr = Definition.ScriptExpressions[exprIndex].NextExpressionHandle.Index;
            var nextExprValid = false;
            if (nextExpr < Definition.ScriptExpressions.Count)
                nextExprValid = GetHsTypeAsString(Cache.Version, Definition.ScriptExpressions[nextExpr].ValueType) != "Invalid";

            switch (expr.Flags)
            {
                case HsSyntaxNodeFlags.ScriptIndex:
                case HsSyntaxNodeFlags.ScriptReference:
                case HsSyntaxNodeFlags.Group:
                    WriteGroupExpression(parentExpr, expr, stringReader, indentWriter, shouldSkip);
                    break;

                case HsSyntaxNodeFlags.Expression:
                    WriteValueExpression(parentExpr, expr, stringReader, indentWriter);
                    break;

                case HsSyntaxNodeFlags.GlobalsReference:
                case HsSyntaxNodeFlags.ParameterReference:
                    indentWriter.Write(expr.StringAddress == 0 ? "none" : ReadScriptString(stringReader, expr.StringAddress));
                    if (nextExprValid)
                        indentWriter.Write(' ');
                    break;
                default:
                    indentWriter.Write($"<UNIMPLEMENTED EXPR: {expr.Flags.ToString()} {GetHsTypeAsString(Cache.Version, expr.ValueType)}>");
                    break;
            }
        }

        public override object Execute(List<string> args)
        {
            FileInfo scriptFile;
            string mapName = Tag.Name.Split('\\').Last();
            string fileName = $"_{Definition.MapId}_{mapName}.hsc";

            switch (args.Count)
            {
                case 0:
                    {
                        if (Cache.Version == CacheVersion.HaloOnlineED)
                            scriptFile = new FileInfo($"haloscript\\ED" + fileName);
                        else
                            scriptFile = new FileInfo($"haloscript\\{Cache.Version}" + fileName);
                    }
                    break;
                case 1:
                    scriptFile = new FileInfo(args[0]);
                    break;
                default:
                    return new TagToolError(CommandError.ArgCount);
            }

            System.IO.Directory.CreateDirectory("haloscript");

            using (var scriptFileStream = scriptFile.Create())
            using (var scriptWriter = new StreamWriter(scriptFileStream))
            using (var scriptStringStream = new MemoryStream(Definition.ScriptStrings))
            using (var scriptStringReader = new BinaryReader(scriptStringStream))
            using (var baseTextWriter = new System.IO.StringWriter())
            using (var indentWriter = new IndentedTextWriter(scriptWriter, "	"))
            {
                indentWriter.Indent = 0;

                //
                // Export scenario script globals
                //

                indentWriter.WriteLine("; Globals");
                foreach (var scriptGlobal in Definition.Globals)
                {
                    indentWriter.Write($"(global {GetHsTypeAsString(Cache.Version, scriptGlobal.Type).ToLower()} {scriptGlobal.Name} ");

                    var expr = Definition.ScriptExpressions[scriptGlobal.InitializationExpressionHandle.Index];

                    WriteExpression(expr, expr, scriptStringReader, indentWriter, false);

                    indentWriter.WriteLine(')');
                }

                indentWriter.WriteLine();

                //
                // Export Externals
                //

                indentWriter.WriteLine("; Externs");
                foreach(var script in Definition.Scripts)
                {
                    if (script.Type != HsScriptType.Extern)
                        continue;

                    indentWriter.Write($"(script {script.Type.ToString().ToLower()} {GetHsTypeAsString(Cache.Version, script.ReturnType).ToLower()} ");

                    if (script.Parameters.Count == 0)
                    {
                        indentWriter.Write(script.ScriptName);
                    }
                    else
                    {
                        indentWriter.Write($"({script.ScriptName}");

                        foreach (var parameter in script.Parameters)
                        {
                            indentWriter.Write($" ({GetHsTypeAsString(Cache.Version, parameter.Type).ToLower()} {parameter.Name})");
                        }
                        
                        indentWriter.Write(')');
                    }
                    var expr = Definition.ScriptExpressions[script.RootExpressionHandle.Index];
                    var shouldSkip = false;
                    if (expr.Opcode == 0)
                        shouldSkip = true;
                    WriteExpression(expr, expr, scriptStringReader, indentWriter, shouldSkip);

                    if (script != Definition.Scripts.Last())
                    {
                        indentWriter.WriteLine(')');
                    }
                    else
                        indentWriter.Write(')');
                }

                indentWriter.WriteLine();

                //
                // Export scenario scripts
                //

                indentWriter.WriteLine("; Scripts");
                foreach (var script in Definition.Scripts)
                {
                    if (script.Type == HsScriptType.Extern)
                        continue;

                    indentWriter.Write($"(script {script.Type.ToString().ToLower()} {GetHsTypeAsString(Cache.Version, script.ReturnType).ToLower()} ");

                    if (script.Parameters.Count == 0)
                    {
                        indentWriter.WriteLine(script.ScriptName);
                    }
                    else
                    {
                        indentWriter.Write($"({script.ScriptName}");

                        foreach (var parameter in script.Parameters)
                        {
                            indentWriter.Write($" ({GetHsTypeAsString(Cache.Version, parameter.Type).ToLower()} {parameter.Name})");
                        }

                        indentWriter.WriteLine(')');
                    }
                    var expr = Definition.ScriptExpressions[script.RootExpressionHandle.Index];
                    var shouldSkip = false;
                    if (expr.Opcode == 0)
                        shouldSkip = true;
                    WriteExpression(expr, expr, scriptStringReader, indentWriter, shouldSkip);

                    if (script != Definition.Scripts.Last())
                    {
                        indentWriter.WriteLine(')');
                        indentWriter.WriteLine();
                    }else
                        indentWriter.Write(')');
                }
            }

            Console.WriteLine($"\nDecompiled script extracted to \"{scriptFile.FullName}\"");
            return true;
        }

        private string GetHsTypeAsString(CacheVersion version, HsType type)
        {
            switch (version)
            {
                case CacheVersion.Halo3Retail:
                    return type.Halo3Retail.ToString();

                case CacheVersion.Halo3ODST:
                    return type.Halo3ODST.ToString();

                case CacheVersion.HaloOnlineED:
                case CacheVersion.HaloOnline106708:
                    return type.HaloOnline.ToString();

                default:
                    new TagToolWarning($"No HsType found for cache \"{version}\". Defaulting to HaloOnline");
                    return type.HaloOnline.ToString();
            }
        }

        private byte[] SortExpressionDataArray(EndianFormat format, byte[] data, int dataLength)
        {
            if (format == EndianFormat.BigEndian)
            {
                byte[] newData = new byte[dataLength];

                // reverse the data array, but only to the specified length
                for (int i = 0; i < dataLength; i++)
                    newData[i] = data[(dataLength - 1) - i];

                return newData;
            }

            return data;
        }
    }
}