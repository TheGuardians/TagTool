using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Scripting;
using TagTool.Scripting.Compiler;

namespace TagTool.Commands.Scenarios
{
    class CompileScriptsCommand : Command
    {
        private GameCache Cache { get; }
        private Scenario Definition { get; }

        public CompileScriptsCommand(GameCache cache, Scenario definition) :
            base(true,

                "CompileScripts",
                "Compile scripts from a file. (Unfinished!)",

                "CompileScripts <input_file>",

                "Examples: 'CompileScripts scripts.txt' or 'CompileScripts scripts.hsc'\n" +
                "The input file must be abide the HaloScriptSyntax.")
        {
            Cache = cache;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var srcTxt = new FileInfo(args[0]);

            if (!srcTxt.Exists)
                throw new FileNotFoundException(srcTxt.FullName);

            ScriptCompiler scriptCompiler = new ScriptCompiler(Cache, Definition);

            scriptCompiler.CompileFile(srcTxt);

            Console.WriteLine("Done.");

            return true;
        }
    }
}