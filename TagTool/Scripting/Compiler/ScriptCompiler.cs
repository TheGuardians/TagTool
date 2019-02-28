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
            //
            // Verify the input syntax node is in the right format
            //

            if (!(node is ScriptGroup group) ||
                !(group.Head is ScriptSymbol symbol && symbol.Value == "script") ||
                !(group.Tail is ScriptGroup declGroup))
            {
                throw new FormatException(node.ToString());
            }

            //
            // Compile the script type
            //

            var scriptType = CompileScriptType(declGroup.Head);

            //
            // Compile the script return type
            //

            if (!(declGroup.Tail is ScriptGroup declTailGroup))
                throw new FormatException(declGroup.Tail.ToString());

            var scriptReturnType = CompileScriptValueType(declTailGroup.Head);

            //
            // Compile the script name and parameters (if any)
            //

            if (!(declTailGroup.Tail is ScriptGroup declTailTailGroup))
                throw new FormatException(declTailGroup.Tail.ToString());

            string scriptName;
            var scriptParams = new List<ScriptParameter>();

            switch (declTailTailGroup.Head)
            {
                // (script static boolean do_stuff ...)
                case ScriptSymbol declName:
                    scriptName = declName.Value;
                    break;

                // (script static boolean (do_stuff (real a) (real b)) ...)
                case ScriptGroup declNameGroup:
                    {
                        //
                        // Get the name of the script
                        //

                        if (!(declNameGroup.Head is ScriptSymbol declGroupName))
                            throw new FormatException(declNameGroup.Head.ToString());

                        scriptName = declGroupName.Value;

                        //
                        // Get a list of script parameters
                        //

                        if (!(declNameGroup.Tail is ScriptGroup declParamGroup))
                            throw new FormatException(declNameGroup.Tail.ToString());

                        for (IScriptSyntax param = declParamGroup;
                            param is ScriptGroup paramGroup;
                            param = paramGroup.Tail)
                        {
                            //
                            // Verify the input parameter syntax is correct: (type name)
                            //

                            if (!(paramGroup.Head is ScriptGroup paramDeclGroup))
                                throw new FormatException(paramGroup.Head.ToString());

                            //
                            // Get the parameter type
                            //

                            if (!(paramDeclGroup.Head is ScriptSymbol paramDeclType))
                                throw new FormatException(paramDeclGroup.Head.ToString());

                            var paramType = CompileScriptValueType(paramDeclType);

                            //
                            // Get the parameter name
                            //

                            if (!(paramDeclGroup.Tail is ScriptGroup paramDeclTailGroup))
                                throw new FormatException(paramDeclGroup.Tail.ToString());

                            if (!(paramDeclTailGroup.Head is ScriptSymbol paramDeclName))
                                throw new FormatException(paramDeclTailGroup.Head.ToString());

                            var paramName = paramDeclName.Value;

                            if (!(paramDeclTailGroup.Tail is ScriptInvalid))
                                throw new FormatException(paramDeclTailGroup.Tail.ToString());

                            //
                            // Add an entry to the script parameters list
                            //

                            scriptParams.Add(new ScriptParameter
                            {
                                Name = paramName,
                                Type = paramType
                            });
                        }
                    }
                    break;

                default:
                    throw new FormatException(declTailGroup.Head.ToString());
            }

            //
            // Compile the script expressions
            //

            var scriptInit = CompileExpression(declTailTailGroup.Tail);

            //
            // Add the script to the scenario definition
            //

            Definition.Scripts.Add(new Script
            {
                ScriptName = scriptName,
                Type = scriptType,
                ReturnType = scriptReturnType,
                RootExpressionHandle = scriptInit,
                Parameters = scriptParams
            });
        }

        private uint CompileExpression(IScriptSyntax head)
        {
            throw new NotImplementedException();
        }
    }
}
