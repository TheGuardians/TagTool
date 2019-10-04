namespace TagTool.Scripting.Compiler
{
    public class ScriptBoolean : IScriptSyntax
    {
        public bool Value;

        public int Line { get; set; }
    }
}