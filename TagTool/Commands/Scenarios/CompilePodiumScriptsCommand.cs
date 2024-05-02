using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using TagTool.Scripting.Compiler;

namespace TagTool.Commands.Scenarios
{
    class CompilePodiumScriptsCommand : Command
    {
        private GameCache Cache { get; }
        private Scenario Definition { get; }

        public CompilePodiumScriptsCommand(GameCache cache, Scenario definition) :
            base(true,

                "CompilePodiumScripts",
                "Optionally compiles and appends podium scripts onto the scenario for you from tools\\podium_scripts.hsc",

                "CompilePodiumScripts",

                "")
        {
            Cache = cache;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            var scriptsTxt = $@"{Program.TagToolDirectory}\Tools\podium_scripts.hsc";

            if (args.Count != 0)
                return new TagToolError(CommandError.ArgCount);

            var srcTxt = new FileInfo(scriptsTxt);

            if (!srcTxt.Exists)
                return new TagToolError(CommandError.FileNotFound, $"\"{scriptsTxt}\"");

            ScriptCompiler scriptCompiler = new ScriptCompiler(Cache, Definition);

            scriptCompiler.AppendCompileFile(srcTxt);

            Console.WriteLine("Done.");

            return true;
        }
    }
}