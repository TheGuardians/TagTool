using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.IO;
using TagTool.Scripting;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;

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

                  "ExtractScripts <Output File>",

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

        private void WriteValueExpression(HsSyntaxNode expr, BinaryReader stringReader, StreamWriter scriptWriter)
        {
            var valueType = GetHsTypeAsString(Cache.Version, expr.ValueType);

            switch (valueType)
            {
                case "FunctionName":
                    scriptWriter.Write(expr.StringAddress == 0 ? OpcodeLookup(expr.Opcode) : ReadScriptString(stringReader, expr.StringAddress)); 
                    break; //Trust the string table, its faster than going through the dictionary with OpcodeLookup.

                case "Boolean":
                    scriptWriter.Write(expr.Data[0] == 0 ? "false" : "true");
                    break;

                case "Real":
                    scriptWriter.Write(BitConverter.ToSingle(SortExpressionDataArray(Cache.Endianness, expr.Data, 4), 0));
                    break;

                case "Short":
                    scriptWriter.Write(BitConverter.ToInt16(SortExpressionDataArray(Cache.Endianness, expr.Data, 2), 0));
                    break;

                case "Long":
                    scriptWriter.Write(BitConverter.ToInt32(SortExpressionDataArray(Cache.Endianness, expr.Data, 4), 0));
                    break;

                case "String":
                    scriptWriter.Write(expr.StringAddress == 0 ? "none" : $"\"{ReadScriptString(stringReader, expr.StringAddress)}\"");
                    break;

                case "Script":
                    scriptWriter.Write(Definition.Scripts[BitConverter.ToInt16(SortExpressionDataArray(Cache.Endianness, expr.Data, 2), 0)].ScriptName);
                    break;

                case "StringId":
                    scriptWriter.Write(Cache.StringTable.GetString(new StringId(BitConverter.ToUInt32(SortExpressionDataArray(Cache.Endianness, expr.Data, 4), 0))));
                    break;

                case "GameDifficulty":
                    switch (BitConverter.ToInt16(SortExpressionDataArray(Cache.Endianness, expr.Data, 2), 0))
                    {
                        case 0: scriptWriter.Write("easy"); break;
                        case 1: scriptWriter.Write("normal"); break;
                        case 2: scriptWriter.Write("heroic"); break;
                        case 3: scriptWriter.Write("legendary"); break;
                        default: throw new NotImplementedException();
                    }
                    break;

                case "Folder":
                case "Unit":
                case "AnimationGraph":
                case "Object":
                case "Device":
                case "CutsceneCameraPoint":
                case "TriggerVolume":
                case "UnitSeatMapping":
                case "Vehicle":
                case "VehicleName":
                    scriptWriter.Write(expr.StringAddress == 0 ? "none" : $"\"{ReadScriptString(stringReader, expr.StringAddress)}\"");
                    break;

                default:
                    scriptWriter.Write($"<UNIMPLEMENTED VALUE: {expr.Flags.ToString()} {valueType}>");
                    break;
            }
        }

        private void WriteGroupExpression(HsSyntaxNode expr, BinaryReader stringReader, StreamWriter scriptWriter)
        {
            scriptWriter.Write('(');

            for (var exprIndex = (ushort)(Definition.ScriptExpressions.IndexOf(expr) + 1); GetHsTypeAsString(Cache.Version, Definition.ScriptExpressions[exprIndex].ValueType) != "Invalid"; exprIndex = Definition.ScriptExpressions[exprIndex].NextExpressionHandle.Index)
            {
                WriteExpression(Definition.ScriptExpressions[exprIndex], stringReader, scriptWriter);

                if (Definition.ScriptExpressions[exprIndex].NextExpressionHandle.Index == ushort.MaxValue || Definition.ScriptExpressions[exprIndex].NextExpressionHandle.Index + 1 > Definition.ScriptExpressions.Count)
                    break;

                scriptWriter.Write(' ');
            }

            scriptWriter.Write(')');
        }

        private void WriteExpression(HsSyntaxNode expr, BinaryReader stringReader, StreamWriter scriptWriter)
        {
            switch (expr.Flags)
            {
                case HsSyntaxNodeFlags.ScriptReference:
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
                    scriptWriter.Write($"<UNIMPLEMENTED EXPR: {expr.Flags.ToString()} {GetHsTypeAsString(Cache.Version, expr.ValueType)}>");
                    break;
            }
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

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
                    scriptWriter.Write($"(global {GetHsTypeAsString(Cache.Version, scriptGlobal.Type).ToSnakeCase()} {scriptGlobal.Name} ");

                    WriteExpression(Definition.ScriptExpressions[scriptGlobal.InitializationExpressionHandle.Index], scriptStringReader, scriptWriter);

                    scriptWriter.WriteLine(')');
                }

                scriptWriter.WriteLine();

                //
                // Export scenario scripts
                //

                foreach (var script in Definition.Scripts)
                {
                    scriptWriter.Write($"(script {script.Type.ToString().ToSnakeCase()} {GetHsTypeAsString(Cache.Version, script.ReturnType).ToSnakeCase()} ");

                    if (script.Parameters.Count == 0)
                    {
                        scriptWriter.WriteLine(script.ScriptName + ' ');
                    }
                    else
                    {
                        scriptWriter.Write($"({script.ScriptName}");

                        foreach (var parameter in script.Parameters)
                        {
                            scriptWriter.Write($" ({GetHsTypeAsString(Cache.Version, parameter.Type).ToSnakeCase()} {parameter.Name})");
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