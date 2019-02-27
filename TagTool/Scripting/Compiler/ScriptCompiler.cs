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

        private ScriptValueType CompileScriptValueType(IScriptSyntax node)
        {
            var result = new ScriptValueType();

            if (!(node is ScriptSymbol symbol) ||
                !Enum.TryParse(symbol.Value, out result.Halo3ODST))
            {
                throw new FormatException(node.ToString());
            }

            return result;
        }

        private ScriptType CompileScriptType(IScriptSyntax node)
        {
            if (!(node is ScriptSymbol symbol) ||
                !Enum.TryParse<ScriptType>(symbol.Value, out var result))
            {
                throw new FormatException(node.ToString());
            }

            return result;
        }

        private void CompileGlobal(IScriptSyntax node)
        {
            if (!(node is ScriptGroup group) ||
                !(group.Head is ScriptSymbol symbol && symbol.Value == "group") ||
                !(group.Tail is ScriptGroup declGroup))
            {
                throw new FormatException(node.ToString());
            }

            var globalType = CompileScriptValueType(declGroup.Head);

            if (!(declGroup.Tail is ScriptGroup declTailGroup))
                throw new FormatException(declGroup.Tail.ToString());

            if (!(declTailGroup.Head is ScriptSymbol declName))
                throw new FormatException(declTailGroup.Head.ToString());

            var globalName = declName.Value;

            if (!(declTailGroup.Tail is ScriptGroup declTailTailGroup))
                throw new FormatException(declTailGroup.Tail.ToString());

            var globalInit = CompileExpression(declTailTailGroup.Head);

            Definition.Globals.Add(new ScriptGlobal
            {
                Name = globalName,
                Type = globalType,
                InitializationExpressionHandle = globalInit
            });
        }

        private void CompileScript(IScriptSyntax node)
        {
            if (!(node is ScriptGroup group) ||
                !(group.Head is ScriptSymbol symbol && symbol.Value == "script") ||
                !(group.Tail is ScriptGroup declGroup))
            {
                throw new FormatException(node.ToString());
            }

            var scriptType = CompileScriptType(declGroup.Head);

            if (!(declGroup.Tail is ScriptGroup declTailGroup))
                throw new FormatException(declGroup.Tail.ToString());

            var scriptReturnType = CompileScriptValueType(declTailGroup.Head);

            if (!(declTailGroup.Tail is ScriptGroup declTailTailGroup))
                throw new FormatException(declTailGroup.Tail.ToString());

            //
            // BEGIN TODO: Parse paremeters in possible (script_name (type1 name1) (type2 name2) ...)
            //

            if (!(declTailTailGroup.Head is ScriptSymbol declName))
                throw new FormatException(declTailGroup.Head.ToString());

            var scriptName = declName.Value;

            //
            // END TODO
            //

            var scriptInit = CompileExpression(declTailTailGroup.Head);

            Definition.Scripts.Add(new Script
            {
                ScriptName = scriptName,
                Type = scriptType,
                ReturnType = scriptReturnType,
                RootExpressionHandle = scriptInit,
                Parameters = new List<ScriptParameter>() // TODO
            });
        }

        private uint CompileExpression(IScriptSyntax head)
        {
            throw new NotImplementedException();
        }
    }
}
