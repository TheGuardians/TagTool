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
            {
                var decompiler = new ScriptDecompiler(Cache, Definition);
                decompiler.DecompileScripts(scriptWriter);
            }

            Console.WriteLine($"\nDecompiled script extracted to \"{scriptFile.FullName}\"");
            return true;
        }
    }
}