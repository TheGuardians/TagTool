using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Scripting;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Scenarios
{
    class ImportScriptsCommand : Command
    {
        private Scenario scnr { get; }

        public ImportScriptsCommand(Scenario definition) :
            base(true,
                
                "Importscripts",
                "Import script globals and expressions previously dumped using dumpscripts",

                "Importscripts <CSV File>",
                "Import script globals and expressions previously dumped using dumpscripts")
        {
            scnr = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            var file = new FileInfo(args[0]);
            if (!file.Exists)
                return new TagToolError(CommandError.FileNotFound);

            using (var reader = file.OpenText())
            {
                string line;
                bool line_is_script_section = false;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == "Scripts")
                        line_is_script_section = true;

                    if ( line == "Scripts" || line == "" || line.Contains("{") || !line_is_script_section)
                        continue;

                    var items = line.Split(",".ToCharArray());
                    if (items.Length < 6)
                        return new TagToolError(CommandError.OperationFailed,"Input script expression missing components!");

                    int scriptIndex = Convert.ToInt32(items[0]);

                    uint.TryParse(items[2], NumberStyles.HexNumber, null, out uint NextExpressionHandle);
                    ushort.TryParse(items[3], NumberStyles.HexNumber, null, out ushort Opcode);
                    byte.TryParse(items[4].Substring(0, 2), NumberStyles.HexNumber, null, out byte data0);
                    byte.TryParse(items[4].Substring(2, 2), NumberStyles.HexNumber, null, out byte data1);
                    byte.TryParse(items[4].Substring(4, 2), NumberStyles.HexNumber, null, out byte data2);
                    byte.TryParse(items[4].Substring(6, 2), NumberStyles.HexNumber, null, out byte data3);

                    scnr.ScriptExpressions[scriptIndex].NextExpressionHandle = new DatumHandle(NextExpressionHandle);
                    scnr.ScriptExpressions[scriptIndex].Opcode = Opcode;
                    scnr.ScriptExpressions[scriptIndex].Data[0] = data0;
                    scnr.ScriptExpressions[scriptIndex].Data[1] = data1;
                    scnr.ScriptExpressions[scriptIndex].Data[2] = data2;
                    scnr.ScriptExpressions[scriptIndex].Data[3] = data3;
                    scnr.ScriptExpressions[scriptIndex].Flags = (HsSyntaxNodeFlags)Enum.Parse(typeof(HsSyntaxNodeFlags), items[5]);
                }
            }       
            return true;
        }    
    }
}
