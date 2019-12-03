namespace TagTool.Scripting.Compiler
{
    public class ScriptInvalid : IScriptSyntax
    {
        public int Line { get; set; }

        public override string ToString() =>
            $"ScriptBoolean {{ Line: {Line} }}";
    }
}