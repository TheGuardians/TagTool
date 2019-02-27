using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Scripting.Compiler;
using TagTool.Tags.Definitions;

namespace TagTool.Scripting.Compiler
{
    public class ScriptCompiler
    {
        public Scenario Definition { get; }

        public ScriptCompiler(Scenario definition)
        {
            Definition = definition;
        }

        public void CompileFile(FileInfo file)
        {
            if (!file.Exists)
                throw new FileNotFoundException(file.FullName);

            //
            // Read the input file into syntax nodes
            //

            List<IScriptSyntax> nodes;

            using (var stream = file.OpenRead())
                nodes = new ScriptSyntaxReader(stream).ReadToEnd();

            //
            // Parse the input syntax nodes
            //

            foreach (var node in nodes)
                CompileToplevel(node);
        }

        private void CompileToplevel(IScriptSyntax node)
        {
            switch (node)
            {
                case ScriptGroup group:
                    switch (group.Head)
                    {
                        case ScriptSymbol symbol:
                            switch (symbol.Value)
                            {
                                case "global":
                                    CompileGlobal(group);
                                    break;

                                case "script":
                                    CompileScript(group);
                                    break;
                            }
                            break;

                        default:
                            throw new FormatException(group.ToString());
                    }
                    break;

                default:
                    throw new FormatException(node.ToString());
            }
        }

        private void CompileGlobal(IScriptSyntax node)
        {
            switch (node)
            {
                case ScriptGroup group:
                    throw new NotImplementedException();

                default:
                    throw new FormatException(node.ToString());
            }
        }

        private void CompileScript(IScriptSyntax node)
        {
            switch (node)
            {
                case ScriptGroup group:
                    throw new NotImplementedException();

                default:
                    throw new FormatException(node.ToString());
            }
        }
    }
}
