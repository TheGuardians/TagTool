using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Common;
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
        private Dictionary<DatumHandle, string> Globals { get; }
        private CachedTag Tag { get; }

        public DumpScriptsCommand(GameCache cache, CachedTag tag, Scenario definition) :
            base(true,
                
                "DumpScripts",
                "Extract a scenario or function's scripts to use as hardcoded presets for PortTagCommand.Scenario or with the test command AdjustScriptsFromFile.",

                "DumpScripts [function name] [CSV Filename]",
                "Extract a scenario or function's scripts to use as hardcoded presets for PortTagCommand.Scenario or with the test command AdjustScriptsFromFile.")
        {
            Definition = definition;
            Tag = tag;
            Cache = cache;
            CsvQueue1 = new List<string>();
            Globals = new Dictionary<DatumHandle, string>();
        }

        public override object Execute(List<string> args)
        {
            string functionName = "";
            string mapName = Tag.Name.Split('\\').Last();
            string csvFileName = $"_{Definition.MapId}_{mapName}_compiled.csv";

        switch (args.Count)
        {
            case 0:
                {
                    if (Cache.Version == CacheVersion.HaloOnlineED)
                        csvFileName = "ED" + csvFileName;
                    else
                        csvFileName = $"{Cache.Version}" + csvFileName;
                }
                break;
            case 1:
                csvFileName = args[0];
                break;
            case 2:
                functionName = args[0];
                csvFileName = args[1];
                break;
            default:
                return new TagToolError(CommandError.ArgCount);
        }

            if (functionName != "")
            {
                DumpFunction(functionName);
            }
            else
            {
                CsvAdd("Globals");
                for (int index = 0; index < Definition.Globals.Count; index++)
                {
                    CsvAdd($"{index:D4}," +
                           $"{index:X4}," +
                           $"{Definition.Globals[index].InitializationExpressionHandle:X8}," +
                           $"{Definition.Globals[index].InitializationExpressionHandle.Salt:X4}," +
                           $"{Definition.Globals[index].Name,-0x20}," +
                           $"{GetHsTypeAsString(Cache.Version, Definition.Globals[index].Type)}");

                    Globals.Add(Definition.Globals[index].InitializationExpressionHandle, Definition.Globals[index].Name);
                }

                CsvAdd("Scripts");
                for (int index = 0; index < Definition.Scripts.Count; index++)
                {
                    CsvAdd($"{index:D4}," +
                           $"{index:X4}," +
                           $"{Definition.Scripts[index].Type.ToString()}," +
                           $"{GetHsTypeAsString(Cache.Version, Definition.Scripts[index].ReturnType)}," +
                           $"{Definition.Scripts[index].ScriptName}," +
                           $"A:{Definition.Scripts[index].RootExpressionHandle:X8}");
                }

                for (int index = 0; index < Definition.ScriptExpressions.Count; index++)
                {
                    ParseScriptExpression(index, 0);
                }
            }

            CsvDumpQueueToFile(CsvQueue1, csvFileName);
            return true;
        }

        private void CsvAdd(string inputLine)
        {
            CsvQueue1.Add(inputLine);
        }

        private void CsvDumpQueueToFile(List<string> lines, string path)
        {
            path = @"haloscript\" + path;
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

                case CacheVersion.HaloOnlineED:
                case CacheVersion.HaloOnline106708:
                    return type.HaloOnline.ToString();

                default:
                    new TagToolWarning($"No HsType found for cache \"{version}\". Defaulting to HaloOnline");
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

                case CacheVersion.HaloOnlineED:
                case CacheVersion.HaloOnline106708:
                    return (int)type.HaloOnline;

                default:
                    new TagToolWarning($"No HsType found for cache \"{version}\". Defaulting to HaloOnline");
                    return (int)type.HaloOnline;
            }
        }

        private void ParseScriptExpression(int index, int tabcount)
        {
            if (Definition.ScriptExpressions[index].Opcode == 0xBABA)
                return;

            List<string> tablist = new List<string>();
            for (int i = 0; i < tabcount; i++)
            {
                tablist.Add("\t");
            }

            string tabs = string.Concat(tablist);

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

            if (Globals.ContainsKey(expressionHandle))
                scriptGroupName = $"G:{Globals[expressionHandle]}";

            string opcodeName = "";

            if (ScriptExpressionIsValue(Definition.ScriptExpressions[index]) && ScriptInfo.ValueTypes[(Cache.Version, Cache.Platform)].ContainsKey(Definition.ScriptExpressions[index].Opcode))
                opcodeName = $"{ScriptInfo.ValueTypes[(Cache.Version, Cache.Platform)][Definition.ScriptExpressions[index].Opcode]},value";

            else if (ScriptInfo.Scripts[(Cache.Version, Cache.Platform)].ContainsKey(Definition.ScriptExpressions[index].Opcode))
                opcodeName = ScriptInfo.Scripts[(Cache.Version, Cache.Platform)][Definition.ScriptExpressions[index].Opcode].Name;

            try
            {
                if ((Definition.ScriptExpressions[index].Flags == HsSyntaxNodeFlags.ScriptReference) || (index > 0 && Definition.ScriptExpressions[index - 1].Flags == HsSyntaxNodeFlags.ScriptReference))
                    opcodeName = $"call {Definition.Scripts[Definition.ScriptExpressions[index].Opcode].ScriptName}";
            }
            catch (Exception ex)
            {
                new TagToolError(CommandError.CustomError, "Out-of-range exception in Definition.Scripts! (?)");
            }

            CsvAdd(tabs +
                   $"{index:D8}," +
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
                   $"{scriptGroupName}");
        }

        private void DumpFunction(string function_name)
        {
            Stack<int> ContextStack = new Stack<int>();
            int index = 0;
            int lastindex = 0;
            foreach(var Script in Definition.Scripts)
            {
                if (Script.ScriptName == function_name)
                {
                    index = Script.RootExpressionHandle.Index;
                    break;
                }
            }
            while (true)
            {
                ParseScriptExpression(index, ContextStack.Count);
                lastindex = index;
                if (Definition.ScriptExpressions[index].Flags == HsSyntaxNodeFlags.Group)
                {
                    if (Definition.ScriptExpressions[index].NextExpressionHandle != DatumHandle.None)
                    {
                        ContextStack.Push(Definition.ScriptExpressions[index].NextExpressionHandle.Index);
                        index += 1;
                        continue;
                    }
                    else
                    {
                        index += 1;
                        continue;
                    }
                }
                else if (Definition.ScriptExpressions[index].NextExpressionHandle == DatumHandle.None)
                {
                    if(ContextStack.Count == 0)
                    {
                        return;
                    }
                    else
                    {
                        index = ContextStack.Pop();
                        continue;
                    }
                }

                index = Definition.ScriptExpressions[index].NextExpressionHandle.Index;
            }
        }       
    }
}
