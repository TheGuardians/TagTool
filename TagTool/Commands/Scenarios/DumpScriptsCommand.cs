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

        public DumpScriptsCommand(Scenario definition) :
            base(true,
                
                "DumpScripts",
                "Extract a scenario's scripts to use as hardcoded presets for PortTagCommand.Scenario or with the test command AdjustScriptsFromFile.",

                "DumpScripts <CSV File>",

                "Extract a scenario's scripts to use as hardcoded presets for PortTagCommand.Scenario or with the test command AdjustScriptsFromFile.")
        {
            Definition = definition;
        }

        private List<string> csvQueue1 = new List<string>();

        private void CsvAdd(string in_)
        {
            csvQueue1.Add(in_);
            Console.WriteLine($"{in_}");
        }

        private void CsvDumpQueueToFile(List<string> lines, string path)
        {
            var fileOut = new FileInfo(path);

            if (fileOut.Exists)
                fileOut.Delete();

            using (var csvWriter = fileOut.CreateText())
                foreach (var line in lines)
                    csvWriter.WriteLine(line);
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 1)
                return false;

            var csvFileName = "scriptsDumpOutput.csv";

            foreach (var a in args)
                if (a.Contains(".") || a.Contains("."))
                    csvFileName = a;

            csvQueue1 = new List<string>();
            var globals = new Dictionary<DatumHandle, string>();

            var i = -1;
            CsvAdd("Globals");
            foreach (var a in Definition.Globals)
            {
                i++;
                var salt = a.InitializationExpressionHandle.Salt;

                CsvAdd(
                    $"{i:D4}," +
                    $"{i:X4}," +
                    $"{a.InitializationExpressionHandle:X8}," +
                    $"{salt:X4}," +
                    $"{a.Name,-0x20}," +
                    $"{a.Type.HaloOnline}," +
                    $"");

                globals.Add(a.InitializationExpressionHandle, a.Name);
            }

            CsvAdd("Scripts");
            foreach (var script in Definition.Scripts)
            {
                CsvAdd($"{Definition.Scripts.IndexOf(script):D4}," +
                       $"{Definition.Scripts.IndexOf(script):X4}," +
                       $"{script.Type.ToString()}," +
                       $"{script.ReturnType.HaloOnline}," +
                       $"{script.ScriptName}," +
                       $"A:{script.RootExpressionHandle:X8}");
            }

            var failedOpcodes = new Dictionary<int, int>();

            i = -1;
            foreach (var expr in Definition.ScriptExpressions)
            {
                i++;
                if (expr.Opcode == 0xBABA)
                    continue;

                var scriptGroupName = "";
                if (expr.NextExpressionHandle == DatumHandle.None &&
                    expr.Flags == Scripting.HsSyntaxNodeFlags.Group &&
                    expr.Opcode == 0x0)
                {
                    var ScriptGroupName = Definition.Scripts.Find(x => x.RootExpressionHandle.Salt == expr.Identifier);
                    if (ScriptGroupName != null)
                        scriptGroupName = $",S:{ScriptGroupName.ScriptName}";
                }

                var ExpressionHandle = new DatumHandle((uint)((expr.Identifier << 16) + i));

                if (globals.ContainsKey(ExpressionHandle))
                    scriptGroupName = $"G:{globals[ExpressionHandle]}";

                var opcodeName = "";

                if (ScriptExpressionIsValue(expr))
                {
                    if (Scripting.ScriptInfo.ValueTypes[CacheVersion.HaloOnline106708].ContainsKey(expr.Opcode))
                        opcodeName = $"{Scripting.ScriptInfo.ValueTypes[CacheVersion.HaloOnline106708][expr.Opcode]},value";
                }
                else
                {
                    if (Scripting.ScriptInfo.Scripts[CacheVersion.HaloOnline106708].ContainsKey(expr.Opcode))
                        opcodeName = Scripting.ScriptInfo.Scripts[CacheVersion.HaloOnline106708][expr.Opcode].Name;
                }

                if (expr.Flags == Scripting.HsSyntaxNodeFlags.ScriptReference)
                    opcodeName = "";

                if (i > 0 && Definition.ScriptExpressions[i - 1].Flags == Scripting.HsSyntaxNodeFlags.ScriptReference)
                    opcodeName = "";

                var ValueType = "";

                ValueType = expr.ValueType.HaloOnline.ToString();

                CsvAdd(
                    $"{i:D8}," +
                    $"{((expr.Identifier << 16) | i):X8}," +
                    $"{expr.NextExpressionHandle.Value:X8}," +
                    $"{expr.Opcode:X4}," +
                    $"{expr.Data[0]:X2}" +
                    $"{expr.Data[1]:X2}" +
                    $"{expr.Data[2]:X2}" +
                    $"{expr.Data[3]:X2}," +
                    $"{expr.Flags}," +
                    $"{ValueType}," +
                    $"{opcodeName}," +
                    $"{scriptGroupName}" +
                    $"");
            }

            CsvDumpQueueToFile(csvQueue1, csvFileName);

            return true;
        }

        public bool ScriptExpressionIsValue(HsSyntaxNode expr)
        {
            switch (expr.Flags)
            {
                case HsSyntaxNodeFlags.ParameterReference:
                case HsSyntaxNodeFlags.GlobalsReference:
                    return true;

                case HsSyntaxNodeFlags.Expression:
                    if ((int)expr.ValueType.HaloOnline > 0x4)
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
    }
}
