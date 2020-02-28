using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Scripting;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Scenarios
{
    class DumpScriptsCommand : Command
    {
        private Scenario Definition { get; }
        private GameCache Cache { get; }
        private List<string> CsvQueue1 { get; }

        public DumpScriptsCommand(GameCache cache, Scenario definition) :
            base(true,
                
                "DumpScripts",
                "Extract a scenario's scripts to use as hardcoded presets for PortTagCommand.Scenario or with the test command AdjustScriptsFromFile.",

                "DumpScripts [print] <CSV File>",
                "Extract a scenario's scripts to use as hardcoded presets for PortTagCommand.Scenario or with the test command AdjustScriptsFromFile.")
        {
            Definition = definition;
            Cache = cache;
            CsvQueue1 = new List<string>();
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 2)
                return false;

            var csvFileName = $"{Cache.Version.ToString()}_{Definition.MapId}_scripts.csv";
            bool printToConsole = false;

            if (args.Count > 0 && args[0].ToLower() == "print")
            {
                printToConsole = true;
                if (args.Count == 2)
                    csvFileName = args[1];
            }
            else if (args.Count == 1)
                csvFileName = args[0];

            var globals = new Dictionary<DatumHandle, string>();

            CsvAdd("Globals", printToConsole);
            for (int index = 0; index < Definition.Globals.Count; index++)
            {
                CsvAdd($"{index:D4}," +
                       $"{index:X4}," +
                       $"{Definition.Globals[index].InitializationExpressionHandle:X8}," +
                       $"{Definition.Globals[index].InitializationExpressionHandle.Salt:X4}," +
                       $"{Definition.Globals[index].Name,-0x20}," +
                       $"{GetHsTypeAsString(Cache.Version, Definition.Globals[index].Type)}",
                       printToConsole);

                globals.Add(Definition.Globals[index].InitializationExpressionHandle, Definition.Globals[index].Name);
            }

            CsvAdd("Scripts", printToConsole);
            for (int index = 0; index < Definition.Scripts.Count; index++)
            {
                CsvAdd($"{index:D4}," +
                       $"{index:X4}," +
                       $"{Definition.Scripts[index].Type.ToString()}," +
                       $"{GetHsTypeAsString(Cache.Version, Definition.Scripts[index].ReturnType)}," +
                       $"{Definition.Scripts[index].ScriptName}," +
                       $"A:{Definition.Scripts[index].RootExpressionHandle:X8}",
                       printToConsole);
            }

            for (int index = 0; index < Definition.ScriptExpressions.Count; index++)
            {
                if (Definition.ScriptExpressions[index].Opcode == 0xBABA)
                    continue;

                string scriptGroupName = "";

                if (Definition.ScriptExpressions[index].NextExpressionHandle == DatumHandle.None &&
                    Definition.ScriptExpressions[index].Flags == HsSyntaxNodeFlags.Group &&
                    Definition.ScriptExpressions[index].Opcode == 0x0)
                {
                    var relativeHsScript = Definition.Scripts.Find(x => x.RootExpressionHandle.Salt == Definition.ScriptExpressions[index].Identifier);
                    if (relativeHsScript != null)
                        scriptGroupName = $",S:{relativeHsScript.ScriptName}";
                }

                DatumHandle expressionHandle = new DatumHandle((uint)((Definition.ScriptExpressions[index].Identifier << 16) + index));

                if (globals.ContainsKey(expressionHandle))
                    scriptGroupName = $"G:{globals[expressionHandle]}";

                string opcodeName = "";

                if (ScriptExpressionIsValue(Definition.ScriptExpressions[index]) && ScriptInfo.ValueTypes[Cache.Version].ContainsKey(Definition.ScriptExpressions[index].Opcode))
                    opcodeName = $"{ScriptInfo.ValueTypes[Cache.Version][Definition.ScriptExpressions[index].Opcode]},value";

                else if (ScriptInfo.Scripts[Cache.Version].ContainsKey(Definition.ScriptExpressions[index].Opcode))
                    opcodeName = ScriptInfo.Scripts[Cache.Version][Definition.ScriptExpressions[index].Opcode].Name;

                if ((Definition.ScriptExpressions[index].Flags == HsSyntaxNodeFlags.ScriptReference) || (index > 0 && Definition.ScriptExpressions[index - 1].Flags == HsSyntaxNodeFlags.ScriptReference))
                    opcodeName = "";

                CsvAdd($"{index:D8}," +
                       $"{((Definition.ScriptExpressions[index].Identifier << 16) | index):X8}," +
                       $"{Definition.ScriptExpressions[index].NextExpressionHandle.Value:X8}," +
                       $"{Definition.ScriptExpressions[index].Opcode:X4}," +
                       $"{Definition.ScriptExpressions[index].Data[0]:X2}" +
                       $"{Definition.ScriptExpressions[index].Data[1]:X2}" +
                       $"{Definition.ScriptExpressions[index].Data[2]:X2}" +
                       $"{Definition.ScriptExpressions[index].Data[3]:X2}," +
                       $"{Definition.ScriptExpressions[index].Flags}," +
                       $"{GetHsTypeAsString(Cache.Version, Definition.ScriptExpressions[index].ValueType)}," +
                       $"{opcodeName}," +
                       $"{scriptGroupName}",
                       printToConsole);
            }

            CsvDumpQueueToFile(CsvQueue1, csvFileName);
            return true;
        }

        private void CsvAdd(string inputLine, bool print)
        {
            CsvQueue1.Add(inputLine);
            if (print)
                Console.WriteLine($"{inputLine}");
        }

        private void CsvDumpQueueToFile(List<string> lines, string path)
        {
            var fileOut = new FileInfo(path);

            if (!fileOut.Directory.Exists)
                fileOut.Directory.Create();

            if (fileOut.Exists)
                fileOut.Delete();

            using (var csvWriter = fileOut.CreateText())
                foreach (var line in lines)
                    csvWriter.WriteLine(line);

            Console.WriteLine($"Scripts dumped to file \"{fileOut.FullName}\"");
        }

        public bool ScriptExpressionIsValue(HsSyntaxNode expr)
        {
            switch (expr.Flags)
            {
                case HsSyntaxNodeFlags.ParameterReference:
                case HsSyntaxNodeFlags.GlobalsReference:
                    return true;

                case HsSyntaxNodeFlags.Expression:
                    if (GetHsTypeAsInteger(Cache.Version, expr.ValueType) > 0x4) // 0x4 = Void
                        return true;
                    else
                        return false;

                case HsSyntaxNodeFlags.ScriptReference: // The opcode is the tagblock index of the script it uses, so ignore
                case HsSyntaxNodeFlags.Group:
                    return false;

                default:
                    return false;
            }
        }

        private string GetHsTypeAsString(CacheVersion version, HsType type)
        {
            switch (version)
            {
                case CacheVersion.Halo3Retail:
                    return type.Halo3Retail.ToString();

                case CacheVersion.Halo3ODST:
                    return type.Halo3ODST.ToString();

                case CacheVersion.HaloOnline106708:
                    return type.HaloOnline.ToString();

                default:
                    Console.WriteLine($"WARNING: No HsType found for cache \"{version}\". Defaulting to HaloOnline");
                    return type.HaloOnline.ToString();
            }
        }

        private int GetHsTypeAsInteger(CacheVersion version, HsType type)
        {
            switch (version)
            {
                case CacheVersion.Halo3Retail:
                    return (int)type.Halo3Retail;

                case CacheVersion.Halo3ODST:
                    return (int)type.Halo3ODST;

                case CacheVersion.HaloOnline106708:
                    return (int)type.HaloOnline;

                default:
                    Console.WriteLine($"WARNING: No HsType found for cache \"{version}\". Defaulting to HaloOnline");
                    return (int)type.HaloOnline;
            }
        }
    }
}
